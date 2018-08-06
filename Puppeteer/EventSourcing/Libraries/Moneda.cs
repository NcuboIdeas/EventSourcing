using System;

namespace Puppeteer.EventSourcing.Libraries
{


	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public abstract class Moneda : Objeto
	{
		protected internal Decimal monto;
		protected internal Numero montoEntero;

		private Monedas tipoDeMoneda_Renamed;

		protected internal Moneda(Decimal monto, Monedas tipo)
		{
			tipoDeMoneda_Renamed = tipo;
			double cantidadOriginal = monto.Valor;
			double cantidadRedondeada = Math.Floor(cantidadOriginal * 100.0) / 100.0;

			if (cantidadOriginal - cantidadRedondeada > 0)
			{
				throw new BusinessLogicalException(string.Format("La cantidad monetaria {0} contiene más de dos decimales", cantidadOriginal));
			}
			this.monto = monto;
		}

		protected internal Moneda(Numero monto, Monedas tipo)
		{
			this.tipoDeMoneda_Renamed = tipo;
			this.montoEntero = monto;
		}

		public virtual Decimal Monto
		{
			get
			{
				return monto;
			}
		}

		public virtual Numero MontoEntero
		{
			get
			{
				return montoEntero;
			}
		}

		public virtual bool EsCero()
		{
			return monto.esCero();
		}

		public override Objeto sumar(Objeto objeto)
		{
			double resultado = 0.0;
			if (objeto is Moneda estaMoneda)
			{
				if (EstaEnLaMismaMonedaQue(estaMoneda))
				{
					resultado = (ConvertirADouble() + estaMoneda.ConvertirADouble());
				}
				else
				{
					throw new BusinessLogicalException(string.Format("No se puede sumar un {0} y un {1}.", ((Moneda) objeto).tipoDeMoneda_Renamed, tipoDeMoneda_Renamed));
				}
			}
			else
			{
				throw new LanguageException(string.Format("No se puede sumar un {0} y un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			Moneda nuevoMonto = ObtenerNuevaMonedaEnLaMismaEconomia(resultado);

			return nuevoMonto;
		}

		public override Objeto restar(Objeto objeto)
		{
			double resultado = 0.0;
			if (objeto is Moneda estaMoneda)
			{
				if (EstaEnLaMismaMonedaQue(estaMoneda))
				{
					resultado = (ConvertirADouble() - estaMoneda.ConvertirADouble());
				}
				else
				{
					throw new BusinessLogicalException(string.Format("No se puede restar un {0} y un {1}.", objeto.GetType().Name, this.GetType().Name));
				}
			}
			else
			{
				throw new LanguageException(string.Format("No se puede restar un {0} y un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			Moneda nuevoMonto = ObtenerNuevaMonedaEnLaMismaEconomia(resultado);

			return nuevoMonto;
		}

        public override Objeto multiplicar(Objeto objeto)
		{
			double resultado = 0.0;
			if (objeto is Decimal unDecimal)
			{
				resultado = ConvertirADouble() * unDecimal.Valor;
			}
			else if (objeto is Numero numero)
			{
				resultado = ConvertirADouble() * numero.Valor;
			}
			else
			{
				throw new LanguageException(string.Format("No se puede multiplicar un {0} y un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			Moneda nuevoMonto = ObtenerNuevaMonedaEnLaMismaEconomia(Moneda.ConvertirADouble(resultado));
			return nuevoMonto;
		}

        public override Objeto dividir(Objeto objeto)
		{
			double resultado = 0.0;
			if (objeto is Decimal unDecimal)
			{
				resultado = ConvertirADouble() / unDecimal.Valor;
			}
			else if (objeto is Numero numero)
			{
				resultado = ConvertirADouble() / (double) numero.Valor;
			}
			else
			{
				throw new LanguageException(string.Format("No se puede dividir un {0} y un {1}.", objeto.GetType().Name, this.GetType().Name));
			}
			Moneda nuevoMonto = ObtenerNuevaMonedaEnLaMismaEconomia(Moneda.ConvertirADouble(resultado));
			return nuevoMonto;
		}

		public static double ConvertirADouble(double cantidadOriginal)
		{
			double cantidadRedondeada = Math.Floor(cantidadOriginal * 100.0) / 100.0;
			return cantidadRedondeada;
		}

		public override string print()
		{
			string simbolo = SimboloMonetario();
			if (simbolo.Equals("CRC"))
			{
				simbolo = "�";
			}
			if (simbolo.Equals("USD"))
			{
				simbolo = "$";
			}
			string montoConFormato;
			if (montoEntero != null)
			{
                montoConFormato = simbolo + ((int)montoEntero.Valor).ToString();
			}
			else
			{
                montoConFormato = simbolo + monto.Valor.ToString("F");
			}

			return montoConFormato;
		}

		public override string ToString()
		{
			if (montoEntero != null)
			{
                return SimboloMonetario() + ((int) montoEntero.Valor).ToString();
			}
			else
			{
                return SimboloMonetario() + monto.Valor.ToString("F");
			}
		}

		internal abstract string SimboloMonetario();

		internal abstract bool EstaEnLaMismaMonedaQue(Moneda otroMonto);

		public abstract string NombreDeLaMoneda();

		public abstract Moneda CeroEnEstaMoneda();

		public abstract Moneda ObtenerNuevaMonedaEnLaMismaEconomia(double cantidadDePlata);

		public abstract double ConvertirADouble();

		public abstract Objeto SinDecimales();

        public override Boolean esIgualQue(Objeto objeto)
		{

			Moneda otraMoneda = null;
			try
			{
				otraMoneda = (Moneda)objeto;
				if (tipoDeMoneda_Renamed != otraMoneda.tipoDeMoneda_Renamed)
				{
					throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", tipoDeMoneda_Renamed.nombre(), otraMoneda.tipoDeMoneda_Renamed.nombre()));
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Denominacion).Name, objeto.GetType().Name));
			}
            return ConvertirADouble() == otraMoneda.ConvertirADouble() && tipoDeMoneda_Renamed == otraMoneda.tipoDeMoneda_Renamed ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorQue(Objeto objeto)
		{
			bool resultado = false;
			if (objeto is Moneda estaMoneda)
			{
				if (tipoDeMoneda_Renamed != estaMoneda.tipoDeMoneda_Renamed)
				{
					throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", tipoDeMoneda_Renamed.nombre(), estaMoneda.tipoDeMoneda_Renamed.nombre()));
				}
				resultado = ConvertirADouble() < estaMoneda.ConvertirADouble();
			}
			else
			{
				throw new LanguageException(string.Format("No se puede comparar un {0} con un {1}.", objeto.GetType().Name, this.GetType().Name));
			}

			return resultado ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorQue(Objeto objeto)
		{
			bool resultado = false;
			if (objeto is Moneda estaMoneda)
			{
				if (tipoDeMoneda_Renamed != estaMoneda.tipoDeMoneda_Renamed)
				{
					throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", tipoDeMoneda_Renamed.nombre(), estaMoneda.tipoDeMoneda_Renamed.nombre()));
				}
				resultado = ConvertirADouble() > estaMoneda.ConvertirADouble();
			}
			else
			{
				throw new LanguageException(string.Format("No se puede comparar un {0} con un {1}.", objeto.GetType().Name, this.GetType().Name));
			}

			return resultado ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorOIgualQue(Objeto objeto)
		{
			bool resultado = false;
			if (objeto is Moneda estaMoneda)
			{
				if (tipoDeMoneda_Renamed != estaMoneda.tipoDeMoneda_Renamed)
				{
					throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", tipoDeMoneda_Renamed.nombre(), estaMoneda.tipoDeMoneda_Renamed.nombre()));
				}
				resultado = ConvertirADouble() >= estaMoneda.ConvertirADouble();
			}
			else
			{
				throw new LanguageException(string.Format("No se puede comparar un {0} con un {1}.", objeto.GetType().Name, this.GetType().Name));
			}

			return resultado ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorOIgualQue(Objeto objeto)
		{
			bool resultado = false;
			if (objeto is Moneda estaMoneda)
			{
				if (tipoDeMoneda_Renamed != estaMoneda.tipoDeMoneda_Renamed)
				{
					throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", tipoDeMoneda_Renamed.nombre(), estaMoneda.tipoDeMoneda_Renamed.nombre()));
				}
				resultado = ConvertirADouble() <= estaMoneda.ConvertirADouble();
			}
			else
			{
				throw new LanguageException(string.Format("No se puede comparar un {0} con un {1}.", objeto.GetType().Name, this.GetType().Name));
			}

			return resultado ? Boolean.True : Boolean.False;
		}

        public virtual Boolean EsDeTipo(Monedas tipo)
		{
			return tipoDeMoneda_Renamed == tipo ? Boolean.True : Boolean.False;
		}

		public virtual Monedas TipoDeMoneda()
		{
			return tipoDeMoneda_Renamed;
		}
	}
}