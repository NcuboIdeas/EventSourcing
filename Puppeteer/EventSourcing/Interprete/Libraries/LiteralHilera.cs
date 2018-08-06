using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Hilera = Puppeteer.EventSourcing.Libraries.Hilera;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralHilera : Expresion
	{
		private readonly string valor;

		public LiteralHilera(string valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(Hilera);
		}

		public override Objeto ejecutar()
		{
			return new Hilera(valor);
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append('\'');
			resultado.Append(valor.Replace("\'", "\\\'"));
			resultado.Append('\'');
		}
	}

}