using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Decimal = Puppeteer.EventSourcing.Libraries.Decimal;
	using Moneda = Puppeteer.EventSourcing.Libraries.Moneda;
	using Numero = Puppeteer.EventSourcing.Libraries.Numero;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpRestar : Expresion
	{

		private Expresion e1;
		private Expresion e2;

		public OpRestar(Expresion e1, Expresion e2)
		{
			this.e1 = e1;
			this.e2 = e2;
		}

		internal override Type calcularTipo()
		{
			if (e1.calcularTipo().Equals(typeof(Numero)) && e2.calcularTipo().Equals(typeof(Numero)))
			{
				return typeof(Numero);
			}
			else if (e1.calcularTipo().Equals(typeof(Decimal)) && e2.calcularTipo().Equals(typeof(Numero)))
			{
				return typeof(Decimal);
			}
			else if (e1.calcularTipo().Equals(typeof(Numero)) && e2.calcularTipo().Equals(typeof(Decimal)))
			{
				return typeof(Decimal);
			}
			else if (e1.calcularTipo().Equals(typeof(Decimal)) && e2.calcularTipo().Equals(typeof(Decimal)))
			{
				return typeof(Decimal);
			}
			else if (typeof(Moneda).IsSubclassOf(e1.calcularTipo()) && e1.calcularTipo().Equals(e2.calcularTipo()))
			{
				return e1.calcularTipo();
			}
			return typeof(Objeto);
		}

		internal override void validarEstaticamente()
		{
			if (calcularTipo().Equals(typeof(Objeto)))
			{
				throw new LanguageException(string.Format("No se le puede restar a un valor tipo {0} un valor tipo {1}.", e1.GetType().Name, e2.GetType().Name));
			}
		}

		public override Objeto ejecutar()
		{
			Objeto objeto1 = e1.ejecutar();
			Objeto objeto2 = e2.ejecutar();
			return objeto1.restar(objeto2);
		}

		internal override void write(StringBuilder resultado)
		{
			e1.write(resultado);
			resultado.Append(" - ");
			e2.write(resultado);
		}
	}
}