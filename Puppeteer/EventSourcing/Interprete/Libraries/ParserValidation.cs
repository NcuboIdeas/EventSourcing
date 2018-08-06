using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{


	using TokenType = Puppeteer.EventSourcing.Interprete.Token.TokenType;
	using Decimal = Puppeteer.EventSourcing.Libraries.Decimal;
	using Denominacion = Puppeteer.EventSourcing.Libraries.Denominacion;
	using Fecha = Puppeteer.EventSourcing.Libraries.Fecha;
	using FechaHora = Puppeteer.EventSourcing.Libraries.FechaHora;
	using Hilera = Puppeteer.EventSourcing.Libraries.Hilera;
	using Moneda = Puppeteer.EventSourcing.Libraries.Moneda;
	using Monedas = Puppeteer.EventSourcing.Libraries.Monedas;
    using System.Reflection;

	public class ParserValidation
	{

		public static Moneda validacionMoneda(Hilera unidadMonetaria, double cantidad, Lexer lexer)
		{
			Moneda moneda;
			bool esUnaMonedaNuestra = Monedas.contieneLaMoneda(unidadMonetaria.Valor);
			if (esUnaMonedaNuestra)
			{
				Monedas tipo = Monedas.valueOf(unidadMonetaria.Valor.ToUpper());
				moneda = new Denominacion(new Decimal(cantidad),tipo);
			}
			else
			{
				throw new LanguageException(string.Format("Se esta tratando de ingresar un monto en {0} economia, la cual el sistema no soporta.", unidadMonetaria));
			}
			return moneda;
		}

		public static Fecha parseFechaValidacion(Lexer lexer)
		{
			if (lexer.tokenActual.Type != TokenType.fecha)
			{
				throw new LanguageException("Se esperaba una fecha en el comando, por favor verificar formato.");
			}
			Fecha resultado = Fecha.fromString(lexer.tokenActual.Valor);
			lexer.Aceptar(TokenType.fecha);

			return resultado;
		}

		public static FechaHora parseFechaHoraValidacion(Fecha fecha, Lexer lexer)
		{
			bool tieneHora = lexer.tokenActual != null && lexer.tokenActual.Type == TokenType.hora;
			if (!tieneHora)
			{
				throw new LanguageException("Se esperaba una hora en el comando, por favor verificar formato");
			}
			FechaHora resultado = FechaHora.FromString(fecha, lexer.tokenActual.Valor);
			lexer.Aceptar(TokenType.hora);
			return resultado;
		}


		public static void validacionDeMetodo(Type clase, string methodName, Type[] firma)
		{
			bool existeElNombreDelMetodoEnLaClase = existeELNombreDelMetodoEnLaClase(clase, methodName);
			if (!existeElNombreDelMetodoEnLaClase)
			{
				throw new LanguageException(string.Format("La función '{0}' no se encuentra definida en los valores de tipo '{1}', por favor verifique si la funcion corresponde al tipo de valor.", methodName, clase.Name), "", 1, 1);
			}
			else
			{
				bool existeAlMenosUnMetodoConEseNombreYConLaMisMaCantidadDeArgumentos = existeAlMenosUnMetodoConLaMismaCantidadArgumentos(clase, methodName, firma);
				if (existeAlMenosUnMetodoConEseNombreYConLaMisMaCantidadDeArgumentos)
				{
					validaErrorEnMetodoConMismaCantidadDeArgumentos(clase, methodName, firma);
				}
				else
				{
					validaErrorEnMetodoConDiferenteCantidadDeArgumentos(clase, methodName, firma);
				}
			}
		}

		private static void validaErrorEnMetodoConDiferenteCantidadDeArgumentos(Type clase, string methodName, Type[] firma)
		{
            List<MethodInfo> metodosEncontrados = obtenerMetodosDiferenteTamanno(clase, methodName);

			throw new LanguageException(string.Format("Esta trantando de hacer el llamado a la funcion '{0}' con una cantidad erronea de argumentos para los valores de tipo '{1}'. {2}", methodName, clase.Name, obtenerEncabezadosDeMetodosSugeridos(metodosEncontrados)), "", 1, 1);
		}

		private static string obtenerEncabezadosDeMetodosSugeridos(List<MethodInfo> metodosEncontrados)
		{
			StringBuilder encabezados = new StringBuilder();
			encabezados.Append("Funciones Sugeridas:");

            foreach (MethodInfo metodo in metodosEncontrados)
			{
				string argumentos = "";
				foreach (ParameterInfo type in metodo.GetParameters())
				{
					argumentos += "" + type.Name +":"+ type.ParameterType.ToString() + ", ";
				}
                if ( ! String.IsNullOrEmpty(argumentos))
                {
                    argumentos = argumentos.Substring(0, argumentos.Length - 2);
                }
				encabezados.Append(string.Format(" {0}({1}); ", metodo.Name, argumentos));
			}
			return encabezados.ToString();
		}

		private static void validaErrorEnMetodoConMismaCantidadDeArgumentos(Type claseDelObjeto, string methodName, Type[] firma)
		{
			Dictionary<int, MethodInfo> pesosDeMetodosPorErrores = new Dictionary<int, MethodInfo>();

            List<MethodInfo> metodosEncontrados = obtenerMetodosMismoTamanno(claseDelObjeto, methodName, firma);

            foreach (MethodInfo metodo in metodosEncontrados)
			{
				ParameterInfo[] firmaEsperadaTemp = metodo.GetParameters();

				int cantidadErrores = pesosDeMetodosPorErrores.Count;
				for (int i = 0; i < firma.Length; i++)
				{
					Type miClase = firma[i];
					ParameterInfo claseEsperada = firmaEsperadaTemp[i];

                    bool sonCompatibles = miClase.IsAssignableFrom(claseEsperada.ParameterType);

                    if (!sonCompatibles && 
                        miClase.IsGenericType && claseEsperada.ParameterType.IsGenericType &&
                        miClase.GetGenericArguments()[0] == claseEsperada.ParameterType.GetGenericArguments()[0])
                    {
                        sonCompatibles = true;
                    }
					if (!sonCompatibles)
					{
						cantidadErrores++;
					}
				}
				pesosDeMetodosPorErrores[cantidadErrores] = metodo;
			}

			List<int> keys = new List<int>(pesosDeMetodosPorErrores.Keys);
			keys.Sort();
			int metodoConMenosCantidadDeErrores = obtenerKeyMenor(keys);

			StringBuilder mensajeDeError = new StringBuilder();
			foreach (int key in keys)
			{
				MethodInfo metodo = pesosDeMetodosPorErrores[key];
				ParameterInfo[] firmaEsperadaTemp = metodo.GetParameters();

				if (key == metodoConMenosCantidadDeErrores)
				{
					for (int i = 0; i < firma.Length; i++)
					{
						Type miClase = firma[i];
						Type claseEsperada = firmaEsperadaTemp[i].ParameterType;

                        bool sonCompatibles = miClase.IsAssignableFrom(claseEsperada);

                        if (!sonCompatibles &&
                            miClase.IsGenericType && claseEsperada.IsGenericType &&
                            miClase.GetGenericArguments()[0] == claseEsperada.GetGenericArguments()[0])
                        {
                            sonCompatibles = true;
                        }
                        if (!sonCompatibles)
						{
							mensajeDeError.Append(string.Format("Esta trantando de hacer el llamado a la funcion '{0}' con un valor de tipo '{1}' en el parametro #{2}, donde el esperado debe ser un valor de tipo '{3}', por favor corrijalo.", methodName, miClase.Name, i + 1, claseEsperada.Name));
						}
					}
				}
				else
				{
					mensajeDeError.Append("\n").Append(string.Format("Funciones Sugeridas: {0}", obtenerEncabezadoMetodoSugerido(metodo)));
				}
			}
			throw new LanguageException(mensajeDeError.ToString(),"", 1, 1);
		}

		private static int obtenerKeyMenor(List<int> keys)
		{
			int menor = 0;
			foreach (int k in keys)
			{
				if (menor == 0 || k < menor)
				{
					menor = k;
				}
			}
			return menor;
		}

        private static string obtenerEncabezadoMetodoSugerido(MethodInfo metodo)
		{
			string argumentos = "";
			foreach (ParameterInfo type in metodo.GetParameters())
			{
				argumentos += "" + type.Name +":"+ type.ParameterType + ", ";
			}
			argumentos = argumentos.Substring(0, argumentos.Length - 2);

			return string.Format(" {0}({1})", metodo.Name, argumentos);
		}

		private static List<MethodInfo> obtenerMetodosMismoTamanno(Type clase, string methodName, Type[] firma)
		{
			List<MethodInfo> metodosEncontrados = new List<MethodInfo>();
			string methodUnsensitive = methodName.ToUpper();
			foreach (MethodInfo method in clase.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				bool esElMismoNombre = method.Name.ToUpper().Equals(methodUnsensitive);
				if (esElMismoNombre)
				{
					bool poseenLaMismaCantidadDeArgumentos = method.GetParameters().Length == firma.Length;
					if (poseenLaMismaCantidadDeArgumentos)
					{
						metodosEncontrados.Add(method);
					}
				}
			}
			return metodosEncontrados;
		}

		private static List<MethodInfo> obtenerMetodosDiferenteTamanno(Type clase, string methodName)
		{
			string methodUnsensitive = methodName.ToUpper();
            List<MethodInfo> metodosEncontrados = new List<MethodInfo>();
            foreach (MethodInfo method in clase.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				bool esElMismoNombre = method.Name.ToUpper().Equals(methodUnsensitive);
				if (esElMismoNombre)
				{
					metodosEncontrados.Add(method);
				}
			}
			return metodosEncontrados;
		}

		private static bool existeAlMenosUnMetodoConLaMismaCantidadArgumentos(Type clase, string methodName, Type[] firma)
		{
			string methodUnsensitive = methodName.ToUpper();
            foreach (MethodInfo method in clase.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				bool esElMismoNombre = method.Name.ToUpper().Equals(methodUnsensitive);
				if (esElMismoNombre)
				{
					bool poseenLaMismaCantidadDeArgumentos = method.GetParameters().Length == firma.Length;
					if (poseenLaMismaCantidadDeArgumentos)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool existeELNombreDelMetodoEnLaClase(Type clase, string methodName)
		{
			string methodUnsensitive = methodName.ToUpper();
            foreach (MethodInfo method in clase.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
			{
				bool esElMismoNombre = method.Name.ToUpper().Equals(methodUnsensitive);
				if (esElMismoNombre)
				{
					return true;
				}
			}
			return false;
		}
	}
}