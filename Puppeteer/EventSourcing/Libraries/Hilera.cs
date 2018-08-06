using System;

namespace Puppeteer.EventSourcing.Libraries
{
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Hilera : Objeto
	{
		private string valor;

		public Hilera(string valor)
		{
			this.valor = valor;
		}

		public override string print()
		{
			string resultado = "\"" + valor.Replace("\"","\\\"") + "\"";
			return resultado;
		}

		public virtual bool esIgual(Hilera literal)
		{
			return valor.Equals(literal.valor);
		}

		public override string ToString()
		{
			return valor;
		}

		public virtual string Valor
		{
			get
			{
				return valor;
			}
		}

		internal virtual string enMinuscula()
		{
			return valor.ToLower().Replace(" ", "");
		}

		public override Objeto sumar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Hilera)
			{
				resultado = new Hilera(valor + ((Hilera) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Hilera(valor + ((Decimal) objeto).Valor);
			}
			else if (objeto is Numero)
			{
				resultado = new Hilera(valor + ((Numero) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede sumar un {0} a un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			return resultado;
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			Hilera cadena = null;
			try
			{
				cadena = (Hilera)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Hilera).Name, objeto.GetType().Name));
			}
            return this.valor == cadena.valor ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

		public override int GetHashCode()
		{
			const int prime = 31;
			int result = 1;
			result = prime * result + ((string.ReferenceEquals(valor, null)) ? 0 : valor.GetHashCode());
			return result;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (this.GetType() != obj.GetType())
			{
				return false;
			}
			Hilera other = (Hilera) obj;
			if (string.ReferenceEquals(valor, null))
			{
				if (!string.ReferenceEquals(other.valor, null))
				{
					return false;
				}
			}
			else if (!valor.Equals(other.valor))
			{
				return false;
			}
			return true;
		}



	}

}