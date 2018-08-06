using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Nulo = Puppeteer.EventSourcing.Libraries.Nulo;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralNull : Expresion
	{
		public LiteralNull()
		{
		}

		public override Objeto ejecutar()
		{
			return Nulo.NULO;
		}

		internal override Type calcularTipo()
		{
			return typeof(Nulo);
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append("Null");
		}
	}

}