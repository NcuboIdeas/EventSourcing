using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class DeclaracionDeParametro
	{
		private Id nombre;
		private Type tipo;

		public DeclaracionDeParametro(Id nombre, Type tipo)
		{
			this.nombre = nombre;
			this.tipo = tipo;
		}

		public virtual Type tipoDelParametro()
		{
			return tipo;
		}

		public virtual string nombreDelParametro()
		{
			return nombre.Valor;
		}

		public virtual void write(StringBuilder resultado)
		{
			nombre.write(resultado);
			resultado.Append(" as ");
			resultado.Append(tipo.Name);
		}

	}

}