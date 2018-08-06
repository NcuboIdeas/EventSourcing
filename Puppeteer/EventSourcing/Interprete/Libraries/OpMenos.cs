using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Decimal = Puppeteer.EventSourcing.Libraries.Decimal;
	using Moneda = Puppeteer.EventSourcing.Libraries.Moneda;
	using Numero = Puppeteer.EventSourcing.Libraries.Numero;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpMenos : Expresion
	{

		private Expresion e;

		public OpMenos(Expresion e)
		{
			this.e = e;
		}

		internal override Type calcularTipo()
		{
			if (e.calcularTipo().Equals(typeof(Decimal)) || e.calcularTipo().Equals(typeof(Numero)) || typeof(Moneda).IsSubclassOf(e.calcularTipo()))
			{
				return e.calcularTipo();
			}
			return typeof(Objeto);
		}

		public override Objeto ejecutar()
		{
			Objeto objeto1 = e.ejecutar();
			if (objeto1 is Numero)
			{
				return objeto1.multiplicar(new Numero(-1));
			}
			else if (objeto1 is Decimal)
			{
				return objeto1.multiplicar(new Decimal(-1));
			}
			else
			{
				throw new BusinessLogicalException(string.Format("No se reconoce la estructura {0}", objeto1.GetType().Name));
			}
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append('-');
			e.write(resultado);
		}
	}
}