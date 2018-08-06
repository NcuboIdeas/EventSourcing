using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Nulo = Puppeteer.EventSourcing.Libraries.Nulo;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpNoIgualQue : Expresion
	{

		private Expresion e1;
		private Expresion e2;

		public OpNoIgualQue(Expresion e1, Expresion e2)
		{
			this.e1 = e1;
			this.e2 = e2;
		}

		internal override Type calcularTipo()
		{
			return typeof(EventSourcing.Libraries.Boolean);
		}

		public override Objeto ejecutar()
		{
			Objeto objeto1 = e1.ejecutar();
			Objeto objeto2 = e2.ejecutar();
			if (objeto2.GetType() == typeof(Nulo))
			{
				return objeto2.noEsIgualQue(objeto1);
			}
			else
			{
				return objeto1.noEsIgualQue(objeto2);
			}
		}

		internal override void write(StringBuilder resultado)
		{
			e1.write(resultado);
			resultado.Append(" != ");
			e2.write(resultado);
		}
	}
}