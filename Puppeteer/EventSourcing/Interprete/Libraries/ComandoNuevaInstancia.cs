using Puppeteer.EventSourcing.Libraries;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	class ComandoNuevaInstancia : Comando
	{
		private Expresion lValue;
		private Expresion rValue;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public ComandoNuevaInstancia(TablaDeSimbolos tablaDeSimbolos, Expresion lValue, Expresion rValue)
		{
			this.lValue = lValue;
			this.rValue = rValue;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		public override void Ejecutar()
		{
			if (lValue is IdConPunto)
			{
				IdConPunto referencia = (IdConPunto) lValue;

                string instancia = referencia.instancia();
				if (!tablaDeSimbolos.ExisteLaVariable(instancia))
				{
					throw new LanguageException(string.Format("No se ha definido la variable {0}. Verifique el nombre", instancia));
				}
				Objeto objeto = tablaDeSimbolos.Valor(instancia);
                Objeto valorDeLaExpresionDerecha = rValue.ejecutar();

                FieldInfo fieldInfo = ObtenerElFieldDelObjetoSiExiste();
                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(objeto, ImplicitCast(UnBoxing(valorDeLaExpresionDerecha), fieldInfo.FieldType) );
                    return;
                }

                PropertyInfo propertyInfo = ObtenerElPropertyDelObjetoSiExiste();
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(objeto, ImplicitCast(UnBoxing(valorDeLaExpresionDerecha), propertyInfo.PropertyType));
                    return;
                }
                else
                {
                    objeto.setAtributo(referencia.Propiedad(), valorDeLaExpresionDerecha);
                }
            }
            else if (lValue is PuntoConPunto)
            {

            }
			else
			{
				string nuevaVariable = ((Id) lValue).Valor;
				Objeto valorDeLaExpresionDerecha = rValue.ejecutar();
				tablaDeSimbolos.GuardarVariable(nuevaVariable, valorDeLaExpresionDerecha);
            }
        }

        private object ImplicitCast(object valor, Type target)
        {
            object resultado = null;
            Type actual = valor.GetType();
            if (actual == typeof(int) && target == typeof(double) )
            {
                resultado = (double)valor;
            }
            else if (actual == typeof(int) && target == typeof(decimal))
            {
                resultado = Convert.ToDecimal((int)valor);
            }
            else if (actual == typeof(double) && target == typeof(decimal))
            {
                resultado = Convert.ToDecimal((double)valor);
            }
            else if (actual == typeof(decimal) && target == typeof(double))
            {
                resultado = System.Decimal.ToDouble((decimal)valor);
            }
            else
            {
                resultado = valor;
            }
            return resultado;
        }

        private object UnBoxing(Objeto valorObjeto)
        {
            object resultado = null;
            Type tipo = valorObjeto.GetType();
            if (tipo == typeof(Puppeteer.EventSourcing.Libraries.Boolean))
            {
                resultado = ((Puppeteer.EventSourcing.Libraries.Boolean)valorObjeto).Valor;
            }
            else if (tipo == typeof(Puppeteer.EventSourcing.Libraries.Decimal))
            {
                resultado = ((Puppeteer.EventSourcing.Libraries.Decimal)valorObjeto).Valor;
            }
            else if (tipo == typeof(Numero))
            {
                resultado = ((Numero)valorObjeto).Valor;
            }
            else if (tipo == typeof(Hilera))
            {
                resultado = ((Hilera)valorObjeto).Valor;
            }
            else if (tipo == typeof(FechaHora))
            {
                FechaHora fh = (Puppeteer.EventSourcing.Libraries.FechaHora)valorObjeto;
                resultado = new DateTime(fh.Anno, fh.Mes, fh.Dia, fh.Hora, fh.Minutos, fh.Segundos);
            }
            else if (tipo == typeof(Fecha))
            {
                Fecha fh = (Puppeteer.EventSourcing.Libraries.Fecha)valorObjeto;
                resultado = new DateTime(fh.Anno, fh.Mes, fh.Dia);
            }
            else if (tipo == typeof(Lista))
            {
                throw new Exception("Falta de Unboxing Lista a List<primitives>");
                /*Lista valorLista = (Lista) valorObjeto;
                valorLista..demeElTipoObjetoAlQuePertenece();
                if (tipo == typeof(List<bool>))
                {
                    Lista newLista = new Lista();
                    foreach (bool valor in (List<bool>)valorCSharp) newLista.guardarObjeto(valor ? Puppeteer.EventSourcing.Libraries.Boolean.True : Puppeteer.EventSourcing.Libraries.Boolean.False);
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
                else if (tipo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(List<>) && tipo.GetGenericArguments().Length == 1 && typeof(Objeto).IsAssignableFrom(tipo.GetGenericArguments()[0]))
                {
                    Lista newLista = new Lista();
                    foreach (var valor in (IEnumerable<dynamic>)valorCSharp) newLista.guardarObjeto((Objeto)valor);
                    resultado = newLista;
                }*/
            }
            else
            {
                resultado = valorObjeto;
            }
            return resultado;
        }


        public override void ValidarEstaticamente()
		{
			lValue.validarEstaticamente();
			rValue.validarEstaticamente();
		}

		internal override void Write(StringBuilder resultado, int tabs)
		{
			if (lValue != null && rValue != null)
			{
				resultado.Append(GenerarTabs(tabs));
				lValue.write(resultado);
				resultado.Append(" = ");
				rValue.write(resultado);
				resultado.Append(";\r");
			}
		}

        private FieldInfo ObtenerElFieldDelObjetoSiExiste()
        {
            IdConPunto referencia = (IdConPunto)lValue;
            string id = referencia.instancia();
            Objeto instancia = tablaDeSimbolos.Valor(id);
            string nombreDelField = referencia.Propiedad();
            FieldInfo fieldEncontrado = null;
            string fieldUnsensitivo = nombreDelField.ToUpper();
            foreach (FieldInfo field in instancia.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.IsPublic || field.IsAssembly)
                {
                    string fieldName = field.Name.ToUpper();
                    if (fieldName == fieldUnsensitivo)
                    {
                        fieldEncontrado = field;
                        break;
                    }
                }
            }
            return fieldEncontrado;
        }

        private PropertyInfo ObtenerElPropertyDelObjetoSiExiste()
        {
            IdConPunto referencia = (IdConPunto)lValue;
            string id = referencia.instancia();
            Objeto instancia = tablaDeSimbolos.Valor(id);
            string nombreDeLaPropiedad = referencia.Propiedad();
            string propertyUnsensitivo = nombreDeLaPropiedad.ToUpper();

            PropertyInfo propertyEncontrado = null;
            foreach (PropertyInfo property in instancia.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                string propertyName = property.Name.ToUpper();
                if (propertyName == propertyUnsensitivo && property.SetMethod != null)
                {
                    ParameterInfo[] variables = property.SetMethod.GetParameters();
                    variables = QuitarleValueDelSetProperty(variables);

                    bool tienenLaMismaCantidadDeArgumentos =
                        (variables.Length == 0 && referencia.Argumentos() == null) ||
                        (variables.Length == referencia.Argumentos().Length);

                    if (tienenLaMismaCantidadDeArgumentos)
                    {
                        bool sonValidasLasFirmas = referencia.ValidarLaIntegridadDeLosArgumentosDelMetodo(variables);
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

        private ParameterInfo[] QuitarleValueDelSetProperty(ParameterInfo[] parametros)
        {
            List<ParameterInfo> resultado = null;
            if (parametros != null)
            {
                resultado = new List<ParameterInfo>();
                foreach (var parametro in parametros)
                {
                    if (parametro.Name != "value")
                    {
                        resultado.Add(parametro);
                    }
                }
            }
            return resultado.ToArray();
        }

    }

}