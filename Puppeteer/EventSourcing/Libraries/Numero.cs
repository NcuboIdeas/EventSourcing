using System;

namespace Puppeteer.EventSourcing.Libraries
{

	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Numero : Objeto, IComparable<Numero>
	{
		public int valor;

		public Numero(int valor)
		{
			this.valor = valor;
		}

		public override string print()
		{
            return ToString();
		}

		public override string ToString()
		{
			return "" + valor;
		}

		public virtual int Valor
		{
			get
			{
				return valor;
			}
		}

		public virtual bool esCero()
		{
			return valor == 0;
		}

		public override Objeto sumar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Numero(valor + ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor + ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede sumar un {0} a un {1}.", this.GetType().Name, objeto.GetType().Name));
			}
			return resultado;
		}

		public override Objeto restar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Numero(valor - ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor - ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede restar un {0} a un {1}.", this.GetType().Name, objeto.GetType().Name));
			}
			return resultado;
		}

		public override Objeto multiplicar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Numero(valor * ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor * ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede multiplicar un {0} a un {1}.", this.GetType().Name, objeto.GetType().Name));
			}
			return resultado;
		}

		public override Objeto dividir(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Numero(valor / ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor / ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede dividir un {0} a un {1}.", this.GetType().Name, objeto.GetType().Name));
			}
			return resultado;
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			double otroValor;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).Valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Numero).Name, objeto.GetType().Name));
			}
			return valor == otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorQue(Objeto objeto)
		{
			double otroValor;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).Valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Numero).Name, objeto.GetType().Name));
			}
			return valor > otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorQue(Objeto objeto)
		{
			double otroValor;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).Valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Numero).Name, objeto.GetType().Name));
			}
			return valor < otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorOIgualQue(Objeto objeto)
		{
			double otroValor;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).Valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Numero).Name, objeto.GetType().Name));
			}
			return valor <= otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorOIgualQue(Objeto objeto)
		{
			double otroValor;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).Valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Numero).Name, objeto.GetType().Name));
			}
			return valor >= otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

		public virtual Numero siguiente()
		{
			return new Numero(valor + 1);
		}

		public virtual Numero anterior()
		{
			return new Numero(valor - 1);
		}

		public override int GetHashCode()
		{
			return valor;
		}

		public override bool Equals(object objeto)
		{
			Numero otroNumero = (Numero) objeto;
			return valor == otroNumero.Valor;
		}


		public virtual int CompareTo(Numero numero)
		{
			return valor - numero.Valor;
		}

	}

}