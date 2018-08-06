using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Decimal = Puppeteer.EventSourcing.Libraries.Decimal;
	using Moneda = Puppeteer.EventSourcing.Libraries.Moneda;
	using Numero = Puppeteer.EventSourcing.Libraries.Numero;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpDividir : Expresion
	{

		private Expresion e1;
		private Expresion e2;

		public OpDividir(Expresion e1, Expresion e2)
		{
			this.e1 = e1;
			this.e2 = e2;
		}

		internal override Type calcularTipo()
		{
			Type tipoE1 = e1.calcularTipo();
			Type tipoE2 = e2.calcularTipo();
			if (tipoE1.Equals(typeof(Numero)) || tipoE1.Equals(typeof(Decimal)))
			{
				if (tipoE2.Equals(typeof(Numero)))
				{
					return typeof(Numero);
				}
				else if (tipoE2.Equals(typeof(Decimal)))
				{
					return typeof(Decimal);
				}
			}
            else if (typeof(Moneda).IsAssignableFrom(tipoE1) || tipoE2.Equals(typeof(Decimal)))
			{
				return tipoE1;
			}
            else if (typeof(Moneda).IsAssignableFrom(tipoE1) || tipoE2.Equals(typeof(Numero)))
			{
				return tipoE1;
			}
			return typeof(Objeto);
		}

		internal override void validarEstaticamente()
		{
			if (calcularTipo().Equals(typeof(Objeto)))
			{
				throw new LanguageException(string.Format("No se puede dividir un valor tipo {0} entre un denominador tipo {1}.", e1.GetType().Name, e2.GetType().Name));
			}
		}

		public override Objeto ejecutar()
		{
			Objeto objeto1 = e1.ejecutar();
			Objeto objeto2 = e2.ejecutar();
			return objeto1.dividir(objeto2);
		}

		internal override void write(StringBuilder resultado)
		{
			e1.write(resultado);
			resultado.Append(" / ");
			e2.write(resultado);
		}
	}
}