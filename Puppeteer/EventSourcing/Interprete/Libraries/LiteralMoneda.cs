using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Moneda = Puppeteer.EventSourcing.Libraries.Moneda;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralMoneda : Expresion
	{
		private Moneda valor;

		public LiteralMoneda(Moneda valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return valor.GetType();
		}

		public override Objeto ejecutar()
		{
			return valor;
		}

		internal override void validarEstaticamente()
		{
			string caracteresDelMonto = "" + valor.ConvertirADouble();
			int posicionDelPunto = caracteresDelMonto.IndexOf(".", StringComparison.Ordinal);

			bool existeElPuntoEnElMonto = !(posicionDelPunto == -1);

			if (existeElPuntoEnElMonto)
			{
				int caracteresDespuesDelaPunto = caracteresDelMonto.Substring(posicionDelPunto + 1, caracteresDelMonto.Length - (posicionDelPunto + 1)).Length;
				bool laMonedaPoseeMasDeDosDecimales = caracteresDespuesDelaPunto > 2;

				if (laMonedaPoseeMasDeDosDecimales)
				{
					throw new LanguageException(string.Format("Las cantidades de Tipo {0} deben tener a sumo solo 2 decimales. La cifra: {1} tiene {2}.", valor.NombreDeLaMoneda(), valor, caracteresDespuesDelaPunto));
				}
			}
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(valor);
		}
	}

}