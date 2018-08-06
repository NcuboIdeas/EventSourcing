using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Numero = Puppeteer.EventSourcing.Libraries.Numero;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralNumero : Expresion
	{

		private int valor;

		public LiteralNumero(int valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(Numero);
		}

		public override Objeto ejecutar()
		{
			return new Numero(valor);
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(valor);
		}
	}

}