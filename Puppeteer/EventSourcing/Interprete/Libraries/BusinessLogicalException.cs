using System;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public class BusinessLogicalException : Exception
	{
		private const long serialVersionUID = 6050080560415037354L;

		private string stack_Renamed;
		internal int fila_Renamed;
		internal int columna_Renamed;

		public BusinessLogicalException(string mensaje) : base(mensaje)
		{
			this.stack_Renamed = "";
			this.fila_Renamed = 0;
			this.columna_Renamed = 0;
		}

		public BusinessLogicalException(string mensaje, string stack) : base(mensaje)
		{
			this.stack_Renamed = stack;
			this.fila_Renamed = 0;
			this.columna_Renamed = 0;
		}

		public virtual string stack()
		{
			return stack_Renamed;
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