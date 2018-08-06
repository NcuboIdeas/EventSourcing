using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

    using TablaDeSimbolos = DB.TablaDeSimbolos;
    using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;
    using System.Reflection;
    using Libraries;
    using Puppeteer.EventSourcing.Libraries;
    using System.Collections;

    public class NuevaInstancia : Expresion
    {
        private string clase_Renamed;
        private Expresion[] argumentos;
        private Objeto instancia;
        private readonly TablaDeSimbolos tablaDeSimbolos;
        private readonly List<Type> libraries;

        public NuevaInstancia(List<Type> libraries, TablaDeSimbolos tablaDeSimbolos, Id clase, Expresion[] argumentos)
        {
            this.tablaDeSimbolos = tablaDeSimbolos;
            this.clase_Renamed = clase.Valor;
            this.argumentos = argumentos;
            this.libraries = libraries;
        }

        internal override void write(StringBuilder resultado)
        {
            resultado.Append(clase_Renamed);
            resultado.Append('(');
            bool esElPrimero = true;
            foreach (Expresion e in argumentos)
            {
                if (!esElPrimero) resultado.Append(',');

                e.write(resultado);
            }
            resultado.Append(')');
        }

        internal override Type calcularTipo()
        {
            if (instancia == null)
            {
                instancia = InstanciarElObjeto();
            }
            return instancia.GetType();
        }

        internal string Clase()
        {
            return clase_Renamed;
        }

        public override Objeto ejecutar()
        {
            if (tablaDeSimbolos.ExisteLaDeclaracion(clase_Renamed))
            {
                DeclaracionProcedure procedimiento = tablaDeSimbolos.Procedure(clase_Renamed);
                Objeto[] resultados = new Objeto[argumentos.Length];
                bool cantidadDeArguemtosEsLaMisma = argumentos.Length == procedimiento.cantidadDeParametros();
                if (!cantidadDeArguemtosEsLaMisma)
                {
                    throw new LanguageException(string.Format("La cantidad de argumentos No coincide con la cantidad de parametros del procedimiento"));
                }
                int i = 0;
                foreach (Expresion argumento in argumentos)
                {
                    Type tipoDelArgumento = argumento.calcularTipo();
                    Type tipoDelParametro = procedimiento.calcularTipoDelParametro(i);
                    if (tipoDelParametro != tipoDelArgumento)
                    {
                        throw new LanguageException(string.Format("Se esperaba un argumento de tipo '" + tipoDelParametro.Name + "' pero se encontro '" + tipoDelArgumento.Name + "'"));
                    }
                    i++;
                }
                i = 0;
                tablaDeSimbolos.AbrirBloque();
                foreach (Expresion argumento in argumentos)
                {
                    resultados[i] = argumento.ejecutar();
                    tablaDeSimbolos.GuardarVariable(procedimiento.nombreDelParametro(i), resultados[i]);
                    i++;
                }
                procedimiento.ejecutar();
                tablaDeSimbolos.CerrarBloque();
            }
            else
            {
                if (instancia == null)
                {
                    instancia = InstanciarElObjeto();
                }
                return instancia;
            }
            return instancia;
        }

        private object[] ObtenerFirmaDeValoresBasadosEnElConstructor(ConstructorInfo constructor)
        {
            ParameterInfo[] firmaDelConstructor = constructor.GetParameters();
            object[] resultado = new object[argumentos == null ? 0 : argumentos.Length];
            for (int i = 0; i < firmaDelConstructor.Length; i++)
            {
                ParameterInfo tipoDelParametro = firmaDelConstructor[i];

                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.ParameterType.IsEnum && argumentos[i].GetType() == typeof(Id);
                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                {
                    try
                    {
                        Type tipoDelArgumentoEnum = tipoDelParametro.ParameterType;
                        string nombreDelPosibleValorEnumerado = ((Id)argumentos[i]).Valor.ToUpper();

                        foreach (string s in Enum.GetNames(tipoDelArgumentoEnum))
                        {
                            string valorEnElEnum = s.ToUpper();
                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                            {
                                resultado[i] = Enum.Parse(tipoDelArgumentoEnum, s);
                                break;
                            }
                        }
                    }
                    catch (System.ArgumentException ex)
                    {
                        throw new LanguageException("" + ex.Message);
                    }
                }
                else
                {
                    string nombreDeLaClaseALaQuePerteneceElArgumento = firmaDelConstructor[i].ParameterType.FullName.ToLower();
                    if (nombreDeLaClaseALaQuePerteneceElArgumento.StartsWith("system.collections.generic.list"))
                    {
                        nombreDeLaClaseALaQuePerteneceElArgumento = "system.collections.generic.list";
                    }
                    else if (nombreDeLaClaseALaQuePerteneceElArgumento.StartsWith("system.collections.generic.ienumerable"))
                    {
                        nombreDeLaClaseALaQuePerteneceElArgumento = "system.collections.generic.ienumerable";
                    }
                    Objeto argumentoEvaluado = argumentos[i].ejecutar();
                    switch (nombreDeLaClaseALaQuePerteneceElArgumento)
                    {
                        case "system.int32":
                            resultado[i] = ((EventSourcing.Libraries.Numero)argumentoEvaluado).valor;
                            break;
                        case "system.boolean":
                            resultado[i] = ((EventSourcing.Libraries.Boolean)argumentoEvaluado).valor;
                            break;
                        case "system.double":
                            if ("Puppeteer.EventSourcing.Libraries.Numero".Equals(argumentoEvaluado.GetType().FullName.ToString()))
                            {
                                resultado[i] = ((EventSourcing.Libraries.Numero)argumentoEvaluado).Valor;
                            }
                            else
                            {
                                resultado[i] = ((EventSourcing.Libraries.Decimal)argumentoEvaluado).Valor;
                            }
                            break;
                        case "system.string":
                            resultado[i] = ((EventSourcing.Libraries.Hilera)argumentoEvaluado).Valor;
                            break;
                        case "system.collections.generic.list":
                        case "system.collections.generic.ienumerable":
                            if (firmaDelConstructor[i].ParameterType.GetGenericArguments().Length == 1)
                            {
                                var argumentoLista = ((Lista)argumentoEvaluado).getLista();

                                Type tipoElementos = firmaDelConstructor[i].ParameterType.GetGenericArguments()[0];
                                if (tipoElementos.Equals(typeof(System.String)))
                                {
                                    List<string> listaConvertida = new List<string>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Hilera)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(System.Int32)))
                                {
                                    List<int> listaConvertida = new List<int>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Numero)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(System.Boolean)))
                                {
                                    List<bool> listaConvertida = new List<bool>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Boolean)elemento).Valor);
                                    }
                                    resultado[i] = listaConvertida;
                                }
                                else if (tipoElementos.Equals(typeof(System.Double)))
                                {
                                    List<double> listaConvertida = new List<double>();
                                    foreach (var elemento in argumentoLista)
                                    {
                                        listaConvertida.Add(((Puppeteer.EventSourcing.Libraries.Decimal)elemento).Valor);
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
                            break;
                        default:
                            resultado[i] = argumentoEvaluado;
                            break;
                    }
                }
            }
            return resultado;
        }

        private Objeto InstanciarElObjeto()
        {
            ConstructorInfo constructor = OtenerConstructorDeLaClase();
            object[] firmaValoresConstructor = ObtenerFirmaDeValoresBasadosEnElConstructor(constructor);

            try
            {
                return (Objeto)constructor.Invoke(firmaValoresConstructor);
            }
            catch (Exception e)
            {
                throw new LanguageException(string.Format("Hubo un error al instanciar la clase {0}. Ver detalles: {1}", clase_Renamed, e.Message));
            }
        }

        public static Type[] TiposPrimitivos()
        {
            Type aTypeOnThisAssembly = typeof(NuevaInstancia);
            Assembly thisAssembly = aTypeOnThisAssembly.Assembly;

            List<Type> primitivos = new List<Type>();
            foreach (Type unaClase in thisAssembly.GetTypes())
            {
                if (unaClase.IsSubclassOf(typeof(Objeto)))
                {
                    primitivos.Add((Type)unaClase);
                }
            }
            Type[] primitivosArr = primitivos.ToArray();
            return primitivosArr;
        }

        private bool isPuppet(Type t)
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);
            foreach (System.Attribute attr in attrs)
            {
                if (attr is Puppet) return true;
            }
            return false;
        }

        private ConstructorInfo OtenerConstructorDeLaClase()
        {
            string claseUnsensitive = clase_Renamed.ToLower();
            foreach (Type unaClase in libraries)
            {
                if (unaClase.Name.ToLower().Equals(claseUnsensitive) && isPuppet(unaClase) && unaClase.IsSubclassOf(typeof(Objeto)))
                {
                    foreach (ConstructorInfo constructorInfo in unaClase.GetConstructors(BindingFlags.NonPublic| BindingFlags.Public| BindingFlags.Instance))
                    {
                        int cantidadDeParametros = constructorInfo.GetParameters().Length;
                        bool tieneElMismoNumeroDeArgumentos = cantidadDeParametros == argumentos.Length;

                        if (tieneElMismoNumeroDeArgumentos)
                        {
                            ParameterInfo[] firmaDelConstructor = constructorInfo.GetParameters();
                            bool sonCompatibles = true;
                            for (int i = 0; sonCompatibles && i < cantidadDeParametros; i++)
                            {
                                Type tipoDelParametro = firmaDelConstructor[i].ParameterType;
                                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.IsEnum && argumentos[i].GetType() == typeof(Id);//TODO
                                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                                {
                                    bool siSonCompatiblesCambiandoAEnum = false;
                                    try
                                    {
                                        Type tipoDelParametroEnum = tipoDelParametro;
                                        string nombreDelPosibleValorEnumerado = ((Id)argumentos[i]).Valor.ToUpper();

                                        foreach (string s in Enum.GetNames(tipoDelParametroEnum))
                                        {
                                            string valorEnElEnum = s.ToUpper();
                                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                                            {
                                                siSonCompatiblesCambiandoAEnum = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (System.ArgumentException)
                                    {
                                        siSonCompatiblesCambiandoAEnum = false;
                                    }
                                    sonCompatibles = siSonCompatiblesCambiandoAEnum;
                                }
                                else
                                {
                                    Type tipoDelArgumento = argumentos[i].calcularTipo();

                                    string tipoDeVariablePasadaComoArgumento = tipoDelParametro.FullName.ToLower();
                                    if (tipoDeVariablePasadaComoArgumento.StartsWith("system.collections.generic.list"))
                                    {
                                        tipoDeVariablePasadaComoArgumento = "system.collections.generic.list";
                                    }
                                    else if (tipoDeVariablePasadaComoArgumento.StartsWith("system.collections.generic.ienumerable"))
                                    {
                                        tipoDeVariablePasadaComoArgumento = "system.collections.generic.ienumerable";
                                    }
                                    switch (tipoDeVariablePasadaComoArgumento)
                                    {
                                        case "system.int32":
                                            sonCompatibles = tipoDelArgumento.IsAssignableFrom(typeof(EventSourcing.Libraries.Numero));
                                            break;
                                        case "system.boolean":
                                            sonCompatibles = tipoDelArgumento.IsAssignableFrom(typeof(EventSourcing.Libraries.Boolean));
                                            break;
                                        case "system.double":
                                            if ("Puppeteer.EventSourcing.Libraries.Numero".Equals(tipoDelArgumento.ToString()))
                                            {
                                                sonCompatibles = true;
                                            }
                                            else
                                            {
                                                sonCompatibles = tipoDelArgumento.IsAssignableFrom(typeof(EventSourcing.Libraries.Decimal));
                                            }
                                            break;
                                        case "system.string":
                                            sonCompatibles = tipoDelArgumento.IsAssignableFrom(typeof(EventSourcing.Libraries.Hilera));
                                            break;
                                        case "system.collections.generic.list":
                                        case "system.collections.generic.ienumerable":
                                            sonCompatibles = false;
                                            if (tipoDelParametro.GetGenericArguments().Length == 1)
                                            {
                                                bool p1 = tipoDelArgumento.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                                bool p2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(List<>);
                                                bool p3 = tipoDelArgumento.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                                sonCompatibles = p1 && p2 && p3;
                                                if (!sonCompatibles)
                                                {
                                                    bool r1 = tipoDelArgumento.GetGenericTypeDefinition() == typeof(List<>);
                                                    bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(List<>);
                                                    bool r3 = tipoDelArgumento.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                                    sonCompatibles = r1 && r2 && r3;
                                                }
                                                if (!sonCompatibles)
                                                {
                                                    bool r1 = tipoDelArgumento.GetGenericTypeDefinition() == typeof(List<>);
                                                    bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                                    bool r3 = tipoDelArgumento.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                                    sonCompatibles = r1 && r2 && r3;
                                                }
                                                if (!sonCompatibles)
                                                {
                                                    bool r1 = tipoDelArgumento.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                                    bool r2 = tipoDelParametro.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                                                    bool r3 = tipoDelArgumento.GetGenericArguments()[0] == tipoDelParametro.GetGenericArguments()[0];
                                                    sonCompatibles = r1 && r2 && r3;
                                                }
                                            }
                                            break;
                                        default:
                                            sonCompatibles = tipoDelParametro.IsAssignableFrom(tipoDelArgumento);
                                            break;
                                    }
                                }
                            }
                            if (sonCompatibles)
                            {
                                return constructorInfo;
                            }
                        }
                    }
                }
            }
            throw new LanguageException(ObtieneErrorEnElConstructorDeLaClase());
        }

        private string ObtieneErrorEnElConstructorDeLaClase()
        {
            string mensajeDeError = "";

            string claseUnsensitive = clase_Renamed.ToUpper();
            foreach (Type unaClase in libraries)
            {
                if (unaClase.Name.ToUpper().Equals(claseUnsensitive) && isPuppet(unaClase) && unaClase.IsSubclassOf(typeof(Objeto)))
                {
                    foreach (ConstructorInfo constructorInfo in unaClase.GetConstructors())
                    {
                        bool tieneElMismoNumeroDeArgumentos = constructorInfo.GetParameters().Length == argumentos.Length;

                        if (tieneElMismoNumeroDeArgumentos)
                        {
                            ParameterInfo[] firmaDelConstructor = constructorInfo.GetParameters();
                            bool sonCompatibles = true;
                            for (int i = 0; sonCompatibles && i < firmaDelConstructor.Length; i++)
                            {
                                Type tipoDelParametro = firmaDelConstructor[i].ParameterType;
                                bool deboTambienTratarDeProbarSiPuedeSerUnEnum = tipoDelParametro.IsEnum && argumentos[i].GetType() == typeof(Id);
                                if (deboTambienTratarDeProbarSiPuedeSerUnEnum)
                                {
                                    bool siSonCompatiblesCambiandoAEnum = false;
                                    try
                                    {
                                        Type tipoDelParametroEnum = tipoDelParametro;
                                        string nombreDelPosibleValorEnumerado = ((Id)argumentos[i]).Valor.ToUpper();

                                        foreach (string s in Enum.GetNames(tipoDelParametroEnum))
                                        {
                                            string valorEnElEnum = s.ToUpper();
                                            if (valorEnElEnum.Equals(nombreDelPosibleValorEnumerado))
                                            {
                                                siSonCompatiblesCambiandoAEnum = true;
                                                break;
                                            }
                                        }
                                    }
                                    catch (System.ArgumentException)
                                    {
                                        siSonCompatiblesCambiandoAEnum = false;
                                    }
                                    sonCompatibles = siSonCompatiblesCambiandoAEnum;
                                }
                                else
                                {
                                    Type tipoDelArgumento = argumentos[i].calcularTipo();
                                    sonCompatibles = tipoDelParametro.IsAssignableFrom(tipoDelArgumento);

                                    if (!sonCompatibles)
                                    {
                                        mensajeDeError = string.Format("Esta trantando de hacer el llamado al Contructor de '{0}' con un valor de tipo '{1}' en el parametro #{2}, donde el esperado debe ser un valor de tipo '{3}', por favor corrijalo.", clase_Renamed, tipoDelArgumento.Name, i + 1, tipoDelParametro.Name);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (mensajeDeError.Length == 0)
            {
                mensajeDeError = string.Format("Esta trantando de hacer el llamado al Contructor de '{0}' pero en la biblioteca no se reconoce ninguna clase con ese nombre y que herede de Objeto, por favor verifique si existe.", clase_Renamed);
            }
            return mensajeDeError;
        }

    }
}