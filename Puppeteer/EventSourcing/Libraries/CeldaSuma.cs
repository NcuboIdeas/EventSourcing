using System;

namespace Puppeteer.EventSourcing.Libraries
{

	public class CeldaSuma : Celda
	{
		private Tabla miTabla;

		public CeldaSuma(string nombre) : base(nombre, null)
		{
		}

		internal virtual void tabla(Tabla miTabla)
		{
			this.miTabla = miTabla;
		}

		public override Objeto Valor
		{
			get
			{
				Objeto resultado = null;
				foreach (Celda c in miTabla)
				{
					if (!(c is CeldaSuma) && c.Nombre.Equals(this.Nombre))
					{
						Objeto o = c.Valor;
						if (o is Moneda)
						{
							resultado = (resultado == null) ? (Moneda) c.Valor : ((Moneda) resultado).sumar(c.Valor);
						}
						else if (o is Numero)
						{
							resultado = (resultado == null) ? (Numero) c.Valor : ((Numero) resultado).sumar(c.Valor);
						}
						else if (o is Decimal)
						{
							resultado = (resultado == null) ? (Decimal) c.Valor : ((Decimal) resultado).sumar(c.Valor);
						}
					}
				}
				return resultado;
			}
		}

		public override string print()
		{
			return "";
		}

	}

}