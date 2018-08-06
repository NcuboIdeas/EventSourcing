using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpNot : Expresion
	{

		private Expresion e;

		public OpNot(Expresion e)
		{
			this.e = e;
		}

		internal override Type calcularTipo()
		{
			return typeof(EventSourcing.Libraries.Boolean);
		}

		internal override void validarEstaticamente()
		{
			if (!e.calcularTipo().Equals(typeof(EventSourcing.Libraries.Boolean)))
			{
				throw new LanguageException(string.Format("La expresi�n {0} al lado derecho del NOT debe retornar un valor true o false.", e.GetType().Name));
			}
		}

		public override Objeto ejecutar()
		{
            EventSourcing.Libraries.Boolean objeto1 = (EventSourcing.Libraries.Boolean) e.ejecutar();
			return ! objeto1.Valor ? EventSourcing.Libraries.Boolean.True : EventSourcing.Libraries.Boolean.False;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(" ! ");
			e.write(resultado);
		}
	}
}