using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using FechaHora = Puppeteer.EventSourcing.Libraries.FechaHora;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralFechaHora : Expresion
	{
		private readonly FechaHora valor;

		public LiteralFechaHora(FechaHora valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(FechaHora);
		}

		public override Objeto ejecutar()
		{
			return valor;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(valor);
		}

	}

}