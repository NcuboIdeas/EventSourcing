using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Mes = Puppeteer.EventSourcing.Libraries.Mes;
	using Meses = Puppeteer.EventSourcing.Libraries.Meses;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class LiteralMes : Expresion
	{

		private readonly Mes mes;

		public LiteralMes(Meses mes, int ano)
		{
			this.mes = new Mes(mes, ano);
		}

		internal override Type calcularTipo()
		{
			return typeof(Mes);
		}

		public override Objeto ejecutar()
		{
			return mes;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(mes);
		}
	}

}