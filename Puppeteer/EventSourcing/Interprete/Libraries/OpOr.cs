using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class OpOr : Expresion
	{

		private Expresion e1;
		private Expresion e2;

		public OpOr(Expresion e1, Expresion e2)
		{
			this.e1 = e1;
			this.e2 = e2;
		}

		internal override Type calcularTipo()
		{
			return typeof(EventSourcing.Libraries.Boolean);
		}

		internal override void validarEstaticamente()
		{
			if (!e1.calcularTipo().Equals(typeof(EventSourcing.Libraries.Boolean)))
			{
				throw new LanguageException(string.Format("La expresi�n {0} al lado izquierdo del AND debe retornar un valor true o false.", e1.GetType().Name));
			}
			if (!e2.calcularTipo().Equals(typeof(EventSourcing.Libraries.Boolean)))
			{
				throw new LanguageException(string.Format("La expresi�n {0} al lado derecho del OR debe retornar un valor true o false.", e2.GetType().Name));
			}
		}

		public override Objeto ejecutar()
		{
            EventSourcing.Libraries.Boolean objeto1 = (EventSourcing.Libraries.Boolean) e1.ejecutar();
			bool cortoCircuito = objeto1.Valor;
			if (cortoCircuito)
			{
				return EventSourcing.Libraries.Boolean.True;
			}
            EventSourcing.Libraries.Boolean objeto2 = (EventSourcing.Libraries.Boolean) e2.ejecutar();
			return objeto2.Valor ? EventSourcing.Libraries.Boolean.True : EventSourcing.Libraries.Boolean.False;
		}

		internal override void write(StringBuilder resultado)
		{
			e1.write(resultado);
			resultado.Append(" || ");
			e2.write(resultado);
		}
	}
}