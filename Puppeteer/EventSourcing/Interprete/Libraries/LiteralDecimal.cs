using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralDecimal : Expresion
	{
		private readonly double valor;

		public LiteralDecimal(double valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(EventSourcing.Libraries.Decimal);
		}

		public override Objeto ejecutar()
		{
			return new EventSourcing.Libraries.Decimal(valor);
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(valor);
		}



	}

}