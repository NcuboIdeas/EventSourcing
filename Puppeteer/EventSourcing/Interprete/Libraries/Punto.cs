using System;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

    using Nulo = Puppeteer.EventSourcing.Libraries.Nulo;
    using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;
    using System.Reflection;
    using System.Collections.Generic;
    using Puppeteer.EventSourcing.Libraries;
    using System.Linq;
    using System.Collections;
    using System.Text;

    public abstract class Punto : Expresion
    {
        private string nombreDelMetodo;
        private string nombreDeLaPropiedad;
        private Expresion[] argumentos_Renamed;

        protected internal Punto(string metodo, Expresion[] argumentos)
        {
            this.nombreDelMetodo = metodo;
            this.argumentos_Renamed = argumentos;
        }

        protected internal Punto(string propiedad)
        {
            this.nombreDeLaPropiedad = propiedad;
        }

        internal virtual string Metodo()
        {
            return nombreDelMetodo;
        }

        internal virtual string Propiedad()
        {
            return nombreDeLaPropiedad;
        }

        internal virtual Expresion[] Argumentos()
        {
            return argumentos_Renamed;
        }

        public override Objeto ejecutar()
        {
            bool esUnMetodoYNoUnaPropiedad = nombreDelMetodo != null;
            if (esUnMetodoYNoUnaPropiedad)
            {
                return InvocarElMetodo();
            }

            FieldInfo fieldInfo = ObtenerElFieldDelObjetoSiExiste();
            if (fieldInfo != null)
            {
                return InvocarElField();
            }

            PropertyInfo propertyInfo = ObtenerElPropertyDelObjetoSiExiste();
            if (propertyInfo != null)
            {
                return InvocarElGetProperty();
            }
            else
            {
                return PropiedadEnObjeto();
            }
        }

        private FieldInfo ObtenerElFieldDelObjetoSiExiste()
        {
            object instancia = ObtenerElObjeto();
            FieldInfo fieldEncontrado = null;
            string fieldUnsensitivo = nombreDeLaPropiedad.ToUpper();
            foreach (FieldInfo field in instancia.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                string fieldName = field.Name.ToUpper();
                if (fieldName == fieldUnsensitivo)
                {
                    fieldEncontrado = field;
                    break;
                }
            }
            return fieldEncontrado;
        }

        private PropertyInfo ObtenerElPropertyDelObjetoSiExiste()
        {
            object instancia = ObtenerElObjeto();
            PropertyInfo propertyEncontrado = null;
            string propertyUnsensitivo = nombreDeLaPropiedad.ToUpper();
            foreach (PropertyInfo property in instancia.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = property.Name.ToUpper();
                if (propertyName == propertyUnsensitivo && property.GetGetMethod(true) != null)
                {
                    ParameterInfo[] variables = property.GetGetMethod(true).GetParameters();

                    bool tienenLaMismaCantidadDeArgumentos =
                        (variables.Length == 0 && argumentos_Renamed == null) ||
                        (variables.Length == argumentos_Renamed.Length);

                    if (tienenLaMismaCantidadDeArgumentos)
                    {
                        bool sonValidasLasFirmas = ValidarLaIntegridadDeLosArgumentosDelMetodo(variables);
                        if (sonValidasLasFirmas)
                        {
                            propertyEncontrado = property;
                            break;
                        }
                    }
                }
            }
            return propertyEncontrado;
        }

        protected internal virtual Objeto PropiedadEnObjeto()
        {
            Objeto objeto = (Objeto)ObtenerElObjeto();
            Objeto atributo;
            if (objeto.tieneElAtributo(nombreDeLaPropiedad))
            {
                atributo = objeto.getDatoDelAtributo(nombreDeLaPropiedad);
            }
            else
            {
                throw new LanguageException($"El Objeto {objeto.GetType().FullName} no tiene el Field, ni la Propiedad Dinámica llamada {nombreDeLaPropiedad}.");
            }
            return atributo;
        }

        protected internal virtual Type CalcularElTipoDeUnCallExpresion(Type classDeLaInstancia)
        {
            MethodInfo methodABuscar = null;

            bool esUnMetodoYNoUnaPropiedad = nombreDelMetodo != null;
            if (esUnMetodoYNoUnaPropiedad)
            {
                methodABuscar = ObtenerElMetodoDelObjetoSiExiste(classDeLaInstancia);
            }

            bool seEncontroELMetodoRespectivo = methodABuscar != null;
            if (seEncontroELMetodoRespectivo)
            {
                if (methodABuscar.ReturnType.Equals(typeof(void)))
                {
                    return typeof(Puppeteer.EventSourcing.Libraries.Void);
                }
                return (Type)methodABuscar.ReturnType;
            }

            FieldInfo fieldInfo = ObtenerElFieldDelObjetoSiExiste();
            if (fieldInfo != null)
            {
                return fieldInfo.FieldType;
            }

            PropertyInfo propertyInfo = ObtenerElPropertyDelObjetoSiExiste();
            if (propertyInfo != null)
            {
                return propertyInfo.PropertyType;
            }
            else
            {
                return PropiedadEnObjeto().GetType();
            }
        }

        private Type[] ObtenerFirmaDeArgumentos()
        {
            Type[] firmas = new Type[0];
            if (argumentos_Renamed != null)
            {
                firmas = new Type[argumentos_Renamed.Length];
                for (int i = 0; i < argumentos_Renamed.Length; i++)
                {
                    Expresion e = argumentos_Renamed[i];
                    firmas[i] = e.calcularTipo();
                }
            }
            return firmas;
        }

        protected internal virtual Objeto InvocarElField()
        {
            object instancia = ObtenerElObjeto();

            FieldInfo fieldABuscar = ObtenerElFieldDelObjetoSiExiste();

            bool seEncontroELFieldRespectivo = fieldABuscar != null;

            if (! seEncontroELFieldRespectivo)
            {
                ParserValidation.validacionDeMetodo(instancia.GetType(), nombreDelMetodo, ObtenerFirmaDeArgumentos());
            }
            object elObjeto = fieldABuscar.GetValue(instancia);
            Objeto resultadoDeInvocarAlMetodo = Boxing(elObjeto);
            return resultadoDeInvocarAlMetodo;
        }

        protected internal virtual Objeto InvocarElMetodo()
        {
            object instancia = ObtenerElObjeto();

            MethodInfo methodABuscar = ObtenerElMetodoDelObjetoSiExiste(instancia.GetType());

            bool seEncontroELMetodoRespectivo = methodABuscar != null;

            if (!seEncontroELMetodoRespectivo)
            {
                ParserValidation.validacionDeMetodo(instancia.GetType(), nombreDelMetodo, ObtenerFirmaDeArgumentos());
            }
            var firmaDeLosParametros = ObtenerFirmaDeValoresBasadosEnElMetodo(methodABuscar);
            object elObjeto = methodABuscar.Invoke(instancia, firmaDeLosParametros);
            Objeto resultadoDeInvocarAlMetodo = Boxing(elObjeto);
            return resultadoDeInvocarAlMetodo;
        }

        protected internal virtual Objeto InvocarElGetProperty()
        {
            object instancia = ObtenerElObjeto();

            PropertyInfo propiedadABuscar = ObtenerElPropertyDelObjetoSiExiste();

            bool seEncontroELMetodoRespectivo = propiedadABuscar != null;

            if (!seEncontroELMetodoRespectivo)
            {
                ParserValidation.validacionDeMetodo(instancia.GetType(), nombreDelMetodo, ObtenerFirmaDeArgumentos());
            }

            object elObjeto = propiedadABuscar.GetValue(instancia, ObtenerFirmaDeValoresBasadosEnElPropertyGet(propiedadABuscar));
            Objeto resultadoDeInvocarAlMetodo = Boxing(elObjeto);
            return resultadoDeInvocarAlMetodo;
        }

        private Objeto Boxing(object valorCSharp)
        {
            Objeto resultado = null;
            Type tipo = valorCSharp == null ? null : valorCSharp.GetType();
            if (tipo == typeof(bool))
            {
                resultado = (bool) valorCSharp ? Puppeteer.EventSourcing.Libraries.Boolean.True : Puppeteer.EventSourcing.Libraries.Boolean.False;
            }
            else if (tipo == typeof(double))
            {
                resultado = new Puppeteer.EventSourcing.Libraries.Decimal((double)valorCSharp);
            }
            else if (valorCSharp == null)
            {
                resultado = Puppeteer.EventSourcing.Libraries.Nulo.NULO;
            }
            else if (tipo == typeof(decimal))
            {
                resultado = new Puppeteer.EventSourcing.Libraries.Decimal((double)(decimal)valorCSharp);
            }
            else if (tipo == typeof(int))
            {
                resultado = new Puppeteer.EventSourcing.Libraries.Numero((int)valorCSharp);
            }
            else if (tipo == typeof(string))
            {
                resultado = new Puppeteer.EventSourcing.Libraries.Hilera((System.String)valorCSharp);
            }
            else if (tipo == typeof(DateTime))
            {
                DateTime f = (DateTime)valorCSharp;
                if (f.Hour == 0 && f.Minute == 0 && f.Second == 0)
                    resultado = new Fecha(f.Day, f.Month, f.Year);
                else
                    resultado = new FechaHora(f.Day, f.Month, f.Year, f.Hour, f.Minute, f.Second);
            }
            else if (tipo == typeof(List<bool>))
            {
                Lista newLista = new Lista();
                foreach (bool valor in (List<bool>)valorCSharp) newLista.guardarObjeto( valor ? Puppeteer.EventSourcing.Libraries.Boolean.True : Puppeteer.EventSourcing.Libraries.Boolean.False);
                resultado = newLista;
            }
            else if (tipo == typeof(List<double>))
            {
                Lista newLista = new Lista();
                foreach (double valor in (List<double>)valorCSharp) newLista.guardarObjeto(new Puppeteer.EventSourcing.Libraries.Decimal(valor));
                resultado = newLista;
            }
            else if (tipo == typeof(List<decimal>))
            {
                Lista newLista = new Lista();
                foreach (decimal valor in (List<decimal>)valorCSharp) newLista.guardarObjeto(new Puppeteer.EventSourcing.Libraries.Decimal((double)valor));
                resultado = newLista;
            }
            else if (tipo == typeof(List<int>))
            {
                Lista newLista = new Lista();
                foreach (System.Int32 valor in (List<System.Int32>)valorCSharp) newLista.guardarObjeto(new Puppeteer.EventSourcing.Libraries.Numero(valor));
                resultado = newLista;
            }
            else if (tipo == typeof(List<string>))
            {
                Lista newLista = new Lista();
                foreach (string valor in (List<String>)valorCSharp) newLista.guardarObjeto(new Hilera(valor));
                resultado = newLista;
            }
            else if (tipo != null && tipo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(List<>) && tipo.GetGenericArguments().Length == 1 && typeof(Objeto).IsAssignableFrom(tipo.GetGenericArguments()[0]))
            {
                Lista newLista = new Lista();
                foreach (var valor in (IEnumerable<dynamic>)valorCSharp) newLista.guardarObjeto((Objeto)valor);
                resultado = newLista;
            }
            else if (tipo != null && tipo.DeclaringType == typeof(System.Linq.Enumerable) && tipo.GetGenericArguments().Length == 1 && typeof(Objeto).IsAssignableFrom(tipo.GetGenericArguments()[0]))
            {
                var x = tipo.DeclaringType;
                Lista newLista = new Lista();
                foreach (var valor in (IEnumerable<dynamic>)valorCSharp) newLista.guardarObjeto((Objeto)valor);
                resultado = newLista;
            }
            else
            {
                resultado = (Objeto)valorCSharp;
            }
            return resultado;
        }

        protected internal MethodInfo ObtenerElMetodoDelObjetoSiExiste(Type classDelObjeto)
        {
            MethodInfo metodoEncontrado = null;
            string metodoUnsensitivo = nombreDelMetodo.ToUpper();
            foreach (MethodInfo method in classDelObjeto.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                string methodName = method.Name.ToUpper();
                if (methodName.Equals(metodoUnsensitivo))
                {
                    ParameterInfo[] variables = method.GetParameters( );

                    bool tienenLaMismaCantidadDeArgumentos = variables.Length == argumentos_Renamed.Length;

                    if (tienenLaMismaCantidadDeArgumentos)
                    {
                        bool sonValidasLasFirmas = ValidarLaIntegridadDeLosArgumentosDelMetodo(variables);
                        if (sonValidasLasFirmas)
                        {
                            metodoEncontrado = method;
                            break;
                        }
                    }
                }
            }
            return metodoEncontrado;
        }

        private object[] ObtenerFirmaDeValoresBasadosEnElMetodo(MethodInfo metodo)
        {
            return ObtenerFirmaDeValoresBasadosLosParametros(metodo.GetParameters());
        }

        private object[] ObtenerFirmaDeValoresBasadosEnElPropertyGet(PropertyInfo propiedad)
        {
            return ObtenerFirmaDeValoresBasadosLosParametros(propiedad.GetGetMethod(true).GetParameters());
        }

        private object[] ObtenerFirmaDeValoresBasadosLosParametrosViejo(ParameterInfo[] firmaDelValoresDelMetodo)
        {
            object[] resultado = new object[argumentos_Renamed == null ? 0 : argumentos_Renamed.Length];
            for (int i = 0; i < firmaDelValoresDelMetodo.Length; i++)
            {
                ParameterInfo tipoDelParametro = firmaDelValoresDelMetodo[i];
                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.ParameterType.IsEnum && argumentos_Renamed[i].GetType() == typeof(Id);
                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                {
                    try
                    {
                        Type tipoDelArgumentoEnum = tipoDelParametro.ParameterType;
                        string nombreDelPosibleValorEnumerado = ((Id)argumentos_Renamed[i]).Valor.ToUpper(); // DolaR

                        string[] nombres = tipoDelArgumentoEnum.GetEnumNames();
                        foreach (String s in Enum.GetNames(tipoDelArgumentoEnum))
                        {
                            string valorEnElEnum = s.ToUpper();
                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                            {
                                resultado[i] = Enum.Parse(tipoDelArgumentoEnum, s);
                                break;
                            }
                        }
                    }
                    catch (System.ArgumentException)
                    {
                    }
                }
                else
                {
                   // >>> hay que hacer mas casos de Viene y Recibe

                    string tipoDeVariablePasadaComoArgumento = firmaDelValoresDelMetodo[i].ParameterType.FullName.ToLower();
                    if (tipoDeVariablePasadaComoArgumento.StartsWith("system.collections.generic.list"))
                    {
                        tipoDeVariablePasadaComoArgumento = "system.collections.generic.list";
                    }
                    else if (tipoDeVariablePasadaComoArgumento.StartsWith("system.collections.generic.ienumerable"))
                    {
                        tipoDeVariablePasadaComoArgumento = "system.collections.generic.ienumerable";
                    }
                    Objeto argumentoEvaluado = argumentos_Renamed[i].ejecutar();
                    switch (tipoDeVariablePasadaComoArgumento)
                    {
                        case "system.int32":
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Numero)argumentoEvaluado).valor;
                            break;
                        case "system.boolean":
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Boolean)argumentoEvaluado).valor;
                            break;
                        case "system.double":
                        case "system.decimal":
                            if ("Puppeteer.EventSourcing.Libraries.Numero".Equals(argumentoEvaluado.GetType().FullName.ToString()))
                            {
                                resultado[i] = ((Puppeteer.EventSourcing.Libraries.Numero)argumentoEvaluado).Valor;
                            }
                            else
                            {
                                resultado[i] = ((Puppeteer.EventSourcing.Libraries.Decimal)argumentoEvaluado).Valor;
                            }
                            break;
                        case "system.string":
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Hilera)argumentoEvaluado).Valor;
                            break;
                        case "system.collections.generic.list":
                        case "system.collections.generic.ienumerable":
                            if (firmaDelValoresDelMetodo[i].ParameterType.GetGenericArguments().Length == 1)
                            {
                                var argumentoLista = ((Lista)argumentoEvaluado).getLista();
                                
                                Type tipoElementos = firmaDelValoresDelMetodo[i].ParameterType.GetGenericArguments()[0];
                                if (tipoElementos.Equals(typeof(string)))
                                {
                                    List<string> listaConvertida = new List<string>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Hilera)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(int)))
                                {
                                    List<int> listaConvertida = new List<int>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Numero)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(bool)))
                                {
                                    List<bool> listaConvertida = new List<bool>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Boolean)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(double)))
                                {
                                    List<double> listaConvertida = new List<double>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Decimal)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(decimal)))
                                {
                                    List<decimal> listaConvertida = new List<decimal>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add( (decimal) ((Puppeteer.EventSourcing.Libraries.Decimal)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (typeof(Objeto).IsAssignableFrom(tipoElementos))
                                {
                                    Type tipoLista = typeof(List<>);
                                    Type[] tipoListaElementos = { tipoElementos };
                                    Type makeme = tipoLista.MakeGenericType(tipoListaElementos);
                                    var listaConvertida = (IList) Activator.CreateInstance(makeme);

                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(elemento);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                            }
                            break;
                        default:
                            resultado[i] = argumentoEvaluado;
                            break;
                    }
                }
            }

            return resultado;
        }


        private object[] ObtenerFirmaDeValoresBasadosLosParametros(ParameterInfo[] firmaDelValoresDelMetodo)
        {
            object[] resultado = new object[argumentos_Renamed == null ? 0 : argumentos_Renamed.Length];
            for (int i = 0; i < firmaDelValoresDelMetodo.Length; i++)
            {
                ParameterInfo tipoDelParametro = firmaDelValoresDelMetodo[i];
                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.ParameterType.IsEnum && argumentos_Renamed[i].GetType() == typeof(Id);
                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                {
                    try
                    {
                        Type tipoDelArgumentoEnum = tipoDelParametro.ParameterType;
                        string nombreDelPosibleValorEnumerado = ((Id)argumentos_Renamed[i]).Valor.ToUpper(); // DolaR

                        string[] nombres = tipoDelArgumentoEnum.GetEnumNames();
                        foreach (String s in Enum.GetNames(tipoDelArgumentoEnum))
                        {
                            string valorEnElEnum = s.ToUpper();
                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                            {
                                resultado[i] = Enum.Parse(tipoDelArgumentoEnum, s);
                                break;
                            }
                        }
                    }
                    catch (System.ArgumentException)
                    {
                    }
                }
                else
                {
                    Type tipoQueRecibe = firmaDelValoresDelMetodo[i].ParameterType;

                    Objeto argumentoEvaluado = argumentos_Renamed[i].ejecutar();
                    if (argumentoEvaluado == null)
                    {
                        StringBuilder expresion = new StringBuilder();
                        argumentos_Renamed[i].write(expresion);
                        throw new LanguageException($"Parámetro inválido: '{expresion}' no pudo ser evaluada.");
                    }
                    Type tipoEnviado = argumentoEvaluado.GetType();

                    if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Numero)))
                    {
                        if (tipoQueRecibe.Equals(typeof(int)))
                        {
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Numero)argumentoEvaluado).valor;
                        }
                        else if (tipoQueRecibe.Equals(typeof(double)))
                        {
                            int valor = ((Puppeteer.EventSourcing.Libraries.Numero)argumentoEvaluado).valor;
                            resultado[i] = Convert.ToDouble(valor);
                        }
                        else if (tipoQueRecibe.Equals(typeof(decimal)))
                        {
                            int valor = ((Puppeteer.EventSourcing.Libraries.Numero)argumentoEvaluado).valor;
                            resultado[i] = Convert.ToDecimal(valor);
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Nulo)))
                    {
                        resultado[i] = null;
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Boolean)))
                    {
                        if (tipoQueRecibe.Equals(typeof(bool)))
                        {
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Boolean)argumentoEvaluado).valor;
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Hilera)))
                    {
                        if (tipoQueRecibe.Equals(typeof(string)))
                        {
                            resultado[i] = ((Puppeteer.EventSourcing.Libraries.Hilera)argumentoEvaluado).Valor;
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.FechaHora)))
                    {
                        if (tipoQueRecibe.Equals(typeof(DateTime)))
                        {
                            FechaHora fh = (Puppeteer.EventSourcing.Libraries.FechaHora)argumentoEvaluado;
                            resultado[i] = new DateTime(fh.Anno, fh.Mes, fh.Dia, fh.Hora, fh.Minutos, fh.Segundos);
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Fecha)))
                    {
                        if (tipoQueRecibe.Equals(typeof(DateTime)))
                        {
                            Fecha f = (Puppeteer.EventSourcing.Libraries.Fecha)argumentoEvaluado;
                            resultado[i] = new DateTime(f.Anno, f.Mes, f.Dia);
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Decimal)))
                    {
                        if (tipoQueRecibe.Equals(typeof(double)))
                        {
                            double valor = ((Puppeteer.EventSourcing.Libraries.Decimal)argumentoEvaluado).Valor;
                            resultado[i] = valor;
                        }
                        else if (tipoQueRecibe.Equals(typeof(decimal)))
                        {
                            double valor = ((Puppeteer.EventSourcing.Libraries.Decimal)argumentoEvaluado).Valor;
                            resultado[i] = Convert.ToDecimal(valor);
                        }
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Lista)))
                    {
                        bool entrar = tipoEnviado.IsGenericType && tipoEnviado.GetGenericTypeDefinition() == typeof(List<>);

                        if (firmaDelValoresDelMetodo[i].ParameterType.GetGenericArguments().Length == 1)
                        {
                            var argumentoLista = ((Lista)argumentoEvaluado).getLista();

                            Type tipoElementos = firmaDelValoresDelMetodo[i].ParameterType.GetGenericArguments()[0];
                            if (tipoElementos.Equals(typeof(string)))
                            {
                                List<string> listaConvertida = new List<string>();
                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add(((Hilera)elemento).Valor);
                                }
                                resultado[i] = listaConvertida;
                            }
                            else if (tipoElementos.Equals(typeof(int)))
                            {
                                List<int> listaConvertida = new List<int>();
                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add(((Numero)elemento).Valor);
                                }
                                resultado[i] = listaConvertida;
                            }
                            else if (tipoElementos.Equals(typeof(bool)))
                            {
                                List<bool> listaConvertida = new List<bool>();
                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Boolean)elemento).Valor);
                                }
                                resultado[i] = listaConvertida;
                            }
                            else if (tipoElementos.Equals(typeof(double)))
                            {
                                List<double> listaConvertida = new List<double>();
                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Decimal)elemento).Valor);
                                }
                                resultado[i] = listaConvertida;
                            }
                            else if (tipoElementos.Equals(typeof(decimal)))
                            {
                                List<decimal> listaConvertida = new List<decimal>();
                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add((decimal)((Puppeteer.EventSourcing.Libraries.Decimal)elemento).Valor);
                                }
                                resultado[i] = listaConvertida;
                            }
                            else if (typeof(Objeto).IsAssignableFrom(tipoElementos))
                            {
                                Type tipoLista = typeof(List<>);
                                Type[] tipoListaElementos = { tipoElementos };
                                Type makeme = tipoLista.MakeGenericType(tipoListaElementos);
                                var listaConvertida = (IList)Activator.CreateInstance(makeme);

                                foreach (var elemento in argumentoLista)
                                {
                                    listaConvertida.Add(elemento);
                                }
                                resultado[i] = listaConvertida;
                            }
                        }
                    }
                    else
                    {
                        resultado[i] = argumentoEvaluado;
                    }
                }
            }

            return resultado;
        }

        internal bool ValidarLaIntegridadDeLosArgumentosDelMetodo(ParameterInfo[] firmaDelValoresDelMetodo)
        {
            bool sonCompatibles = 
                (firmaDelValoresDelMetodo.Length == 0 && argumentos_Renamed == null) ||
                (argumentos_Renamed.Length == firmaDelValoresDelMetodo.Length);
            for (int i = 0; sonCompatibles && i < firmaDelValoresDelMetodo.Length; i++)
            {
                Type tipoDelParametro = firmaDelValoresDelMetodo[i].ParameterType;

                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.IsEnum && argumentos_Renamed[i].GetType() == typeof(Id);

                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                {
                    bool siSonCompatiblesCambiandoAEnum = true;
                    try
                    {
                        Type tipoDelArgumentoEnum = tipoDelParametro;
                        string nombreDelPosibleValorEnumerado = ((Id)argumentos_Renamed[i]).Valor.ToUpper();
                        bool sonCompatiblesLosEnums = false;

                        foreach (string s in Enum.GetNames(tipoDelArgumentoEnum))
                        {
                            string valorEnElEnum = s.ToUpper();
                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                            {
                                sonCompatiblesLosEnums = true;
                                break;
                            }
                        }

                        if (!sonCompatiblesLosEnums)
                        {
                            string valoresDelEnum = "";
                            foreach (string s in Enum.GetNames(tipoDelArgumentoEnum))
                            {
                                valoresDelEnum = valoresDelEnum + s + " o ";
                            }

                            valoresDelEnum = valoresDelEnum.Substring(0, valoresDelEnum.Length - 2);

                            throw new LanguageException(string.Format("Esta trantando de poner un valor de tipo {0} pero escribio {1}, cuando en realidad debio haber escrito {2}, por favor corrija el valor.", tipoDelArgumentoEnum.Name, nombreDelPosibleValorEnumerado, valoresDelEnum));
                        }
                    }
                    catch (System.ArgumentException e)
                    {
                        throw new BusinessLogicalException(e.Message);
                    }
                    sonCompatibles = siSonCompatiblesCambiandoAEnum;
                }
                else
                {
                    Type tipoQueRecibe = firmaDelValoresDelMetodo[i].ParameterType;

                    Type tipoEnviado = argumentos_Renamed[i].calcularTipo().GetType();

                    if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Numero)))
                    {
                        sonCompatibles =
                            tipoQueRecibe.IsAssignableFrom(typeof(Puppeteer.EventSourcing.Libraries.Numero)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(int)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(double)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(decimal))
                            ;
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Boolean)))
                    {
                        sonCompatibles =
                            tipoQueRecibe.IsAssignableFrom(typeof(Puppeteer.EventSourcing.Libraries.Boolean)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(bool))
                            ;
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Hilera)))
                    {
                        sonCompatibles =
                            tipoQueRecibe.IsAssignableFrom(typeof(Puppeteer.EventSourcing.Libraries.Hilera)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(string))
                            ;
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Decimal)))
                    {
                        sonCompatibles =
                            tipoQueRecibe.IsAssignableFrom(typeof(Puppeteer.EventSourcing.Libraries.Numero)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(Puppeteer.EventSourcing.Libraries.Decimal)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(int)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(decimal)) ||
                            tipoQueRecibe.IsAssignableFrom(typeof(double))
                            ;
                    }
                    else if (tipoEnviado.Equals(typeof(Puppeteer.EventSourcing.Libraries.Lista)))
                    {
                        sonCompatibles = false;
                        if (tipoDelParametro.GetGenericArguments().Length == 1)
                        {
                            bool p1 = tipoQueRecibe.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                            bool p2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(List<>);
                            bool p3 = tipoQueRecibe.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                            sonCompatibles = p1 && p2 && p3;
                            if (!sonCompatibles)
                            {
                                bool r1 = tipoQueRecibe.GetGenericTypeDefinition() == typeof(List<>);
                                bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(List<>);
                                bool r3 = tipoQueRecibe.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                sonCompatibles = r1 && r2 && r3;
                            }
                            if (!sonCompatibles)
                            {
                                bool r1 = tipoQueRecibe.GetGenericTypeDefinition() == typeof(List<>);
                                bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                bool r3 = tipoQueRecibe.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                sonCompatibles = r1 && r2 && r3;
                            }
                            if (!sonCompatibles)
                            {
                                bool r1 = tipoQueRecibe.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                bool r3 = tipoQueRecibe.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                sonCompatibles = r1 && r2 && r3;
                            }

                        }
                    }
                    else
                    {
                        sonCompatibles = tipoQueRecibe.IsAssignableFrom(tipoDelParametro);
                    }
                }
            }
            return sonCompatibles;
        }

        protected internal abstract object ObtenerElObjeto();

    }
}