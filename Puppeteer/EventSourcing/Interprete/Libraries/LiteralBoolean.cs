using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralBoolean : Expresion
	{
		private readonly bool valor;

		public LiteralBoolean(bool valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(EventSourcing.Libraries.Boolean);
		}

		public override Objeto ejecutar()
		{
			return valor ? EventSourcing.Libraries.Boolean.True : EventSourcing.Libraries.Boolean.False;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(valor);
		}

	}

}