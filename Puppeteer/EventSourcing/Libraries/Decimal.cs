using System;
using System.Globalization;

namespace Puppeteer.EventSourcing.Libraries
{

	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Decimal : Objeto
	{
		private double valor;

		public Decimal(double valor)
		{
			this.valor = valor;
		}

		public Decimal(int valor)
		{
			this.valor = valor;
		}

		public override string print()
		{
			return ToString();
		}

		public override string ToString()
		{
			string resultado = "" + valor.ToString(CultureInfo.GetCultureInfo("en-US"));
            return resultado;
		}

		public virtual double Valor
		{
			get
			{
				return valor;
			}
			set
			{
				this.valor = value;
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
				resultado = new Decimal(valor + ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor + ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede sumar un {0}a un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			return resultado;
		}

		public override Objeto restar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Decimal(valor - ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor - ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede restar un {0}a un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			return resultado;
		}

		public override Objeto multiplicar(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Decimal(valor * ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor * ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede multiplicar un {0}a un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			return resultado;
		}

		public override Objeto dividir(Objeto objeto)
		{
			Objeto resultado;
			if (objeto is Numero)
			{
				resultado = new Numero((int) valor / ((Numero) objeto).Valor);
			}
			else if (objeto is Decimal)
			{
				resultado = new Decimal(valor / ((Decimal) objeto).Valor);
			}
			else
			{
				throw new LanguageException(string.Format("No se puede dividir un {0}a un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			return resultado;
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			double otroValor = 0.0;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Decimal).Name, objeto.GetType().Name));
			}
			return valor == otroValor ? Boolean.True : Boolean.False;
		}

		public override Boolean esMayorQue(Objeto objeto)
		{
			double otroValor = 0.0;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Decimal).Name, objeto.GetType().Name));
			}
			return valor > otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorQue(Objeto objeto)
		{
			double otroValor = 0.0;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Decimal).Name, objeto.GetType().Name));
			}
			return valor < otroValor ? Boolean.True : Boolean.False;
		}

		public override Boolean esMenorOIgualQue(Objeto objeto)
		{
			double otroValor = 0.0;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Decimal).Name, objeto.GetType().Name));
			}
			return valor <= otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorOIgualQue(Objeto objeto)
		{
			double otroValor = 0.0;
			try
			{
				otroValor = objeto is Numero ? ((Numero) objeto).valor : ((Decimal)objeto).valor;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Decimal).Name, objeto.GetType().Name));
			}
            return valor >= otroValor ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return  ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

	}
}