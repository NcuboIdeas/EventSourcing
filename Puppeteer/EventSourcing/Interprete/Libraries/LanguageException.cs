using System;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public class LanguageException : Exception
	{

		private const long serialVersionUID = 2081427811811732501L;
		internal int fila_Renamed;
		internal int columna_Renamed;

		public LanguageException(string mensaje, string lineaConError, int fila, int columna) : base(mensaje + "\r" + lineaConError)
		{
			this.fila_Renamed = fila;
			this.columna_Renamed = columna;
		}

		public LanguageException(string mensaje) : base(mensaje)
		{
			this.fila_Renamed = 0;
			this.columna_Renamed = 0;
		}

		public virtual string lineaConError()
		{
			return base.Message;
		}

		public virtual int fila()
		{
			return fila_Renamed;
		}

		public virtual int columna()
		{
			return columna_Renamed;
		}
	}

}