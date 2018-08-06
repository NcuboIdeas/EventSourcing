using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Fecha = Puppeteer.EventSourcing.Libraries.Fecha;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralFecha : Expresion
	{
		private readonly Fecha valor;

		public LiteralFecha(Fecha valor)
		{
			this.valor = valor;
		}

		internal override Type calcularTipo()
		{
			return typeof(Fecha);
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