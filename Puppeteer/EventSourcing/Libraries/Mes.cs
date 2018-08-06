using System;

namespace Puppeteer.EventSourcing.Libraries
{

	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Mes : Objeto, IComparable<Mes>
	{
		private Meses mes;
		private int ano;

		public Meses MesDelAnno
		{
            get
            {
                return mes;
            }
		}

		public virtual int Anno
		{
			get
			{
				return ano;
			}
		}

		public Mes(Meses mes, int anno)
		{
			this.ano = anno;
			this.mes = mes;

		}

		public virtual Numero FormatoAnnoMes
		{
			get
			{
				string temp = "" + ano;
				temp += mes.numero();
    
				return new Numero(int.Parse(temp));
			}
		}

		public virtual Mes mesSiguiente()
		{
			if (mes.numero() == 12)
			{
				return new Mes(Meses.ENE, ano + 1);
			}
			else
			{
				return new Mes(mes.siguienteMes(), ano);
			}
		}

		public virtual Mes mesAnterior()
		{
			if (mes.numero() == 1)
			{
				return new Mes(Meses.DIC, ano - 1);
			}
			else
			{
				return new Mes(mes.anteriorMes(), ano);
			}
		}

		public override string ToString()
		{
			return "" + mes.name() + "/" + ano;
		}

		private static readonly string SIMPLE_NAME = typeof(Mes).Name;

		public override string print()
		{
			string result = "" + mes + "/" + ano;
			return "{\"" + SIMPLE_NAME + "\":\"" + result + "\"}";
		}

		
		private bool esAnoBisiesto()
		{
            return DateTime.IsLeapYear(ano);
		}

		public virtual int diasDelMes()
		{
			bool esFebrero = mes.numero() == 2;
			if (esFebrero)
			{
				bool esBisiesto = esAnoBisiesto();
				if (esBisiesto)
				{
					return 29;
				}
			}

            int[] DIAS_POR_MES = new int []{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
			return DIAS_POR_MES[mes.numero() - 1];
		}

		public virtual Fecha ultimoDiaDelMes()
		{
			return new Fecha(diasDelMes(), mes.numero(), ano);
		}

		public virtual Fecha primerDiaDelMes()
		{
			return new Fecha(1, mes.numero(), ano);
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			Mes argumento = null;
			try
			{
				argumento = (Mes)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Mes).Name, objeto.GetType().Name));
			}
			return mes == argumento.mes && ano == argumento.ano ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

		public override int GetHashCode()
		{
			const int prime = 31;
			int resultado = 1;
			resultado = prime * resultado + ano;
			resultado = prime * resultado + mes.numero();
			return resultado;
		}

		public override bool Equals(object objeto)
		{
			Mes otroMes = (Mes) objeto;
			return ano == otroMes.ano && mes == otroMes.mes;
		}

		public virtual bool comparteMismoTrimestreCon(Mes mesAcomparar)
		{
			bool resultado = false;
			bool elMesActualEsPrimerTrimestre = mes.numero() == 1 || mes.numero() == 2 || mes.numero() == 3;
			bool elMesActualEsSegundoTrimestre = mes.numero() == 4 || mes.numero() == 5 || mes.numero() == 6;
			bool elMesActualEsTercerTrimestre = mes.numero() == 7 || mes.numero() == 8 || mes.numero() == 9;
			bool elMesActualEsCuartoTrimestre = mes.numero() == 10 || mes.numero() == 11 || mes.numero() == 12;

			if (elMesActualEsPrimerTrimestre)
			{
                resultado = mesAcomparar.mes.numero() >= 1 && mesAcomparar.mes.numero() <= 3;
			}
			else if (elMesActualEsSegundoTrimestre)
			{
                resultado = mesAcomparar.mes.numero() >= 4 && mesAcomparar.mes.numero() <= 6;
			}
			else if (elMesActualEsTercerTrimestre)
			{
                resultado = mesAcomparar.mes.numero() >= 7 && mesAcomparar.mes.numero() <= 9;
			}
			else if (elMesActualEsCuartoTrimestre)
			{
                resultado = mesAcomparar.mes.numero() >= 10 && mesAcomparar.mes.numero() <= 12;
			}

			return resultado;
		}

		internal virtual bool estaDentroDelRangoDeUnAnno(Mes mesFinal)
		{
            int diferencia = mesFinal.Anno * 12 + mesFinal.mes.numero() - (this.Anno * 12 + this.mes.numero());
			return diferencia >= 0 && diferencia < 12;
		}

		public virtual Mes obtenerElMismoMesPeroUnAnoPosterior()
		{
			return new Mes(mes, ano + 1);
		}

		public virtual int CompareTo(Mes mes)
		{
			int id = GetHashCode();
			int idArgumento = mes.GetHashCode();

			return id - idArgumento;
		}

        public override Boolean esMenorOIgualQue(Objeto objeto)
		{
			Mes argumento = null;
			try
			{
				argumento = (Mes)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Mes).Name, objeto.GetType().Name));
			}
			return ano < argumento.ano || (ano == argumento.ano && mes.numero() <= argumento.mes.numero()) ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorQue(Objeto objeto)
		{
			Mes argumento = null;
			try
			{
				argumento = (Mes)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Mes).Name, objeto.GetType().Name));
			}
			return ano > argumento.ano || (ano == argumento.ano && mes.numero() > argumento.mes.numero()) ? Boolean.True : Boolean.False;
		}

        public override Boolean esMenorQue(Objeto objeto)
		{
			Mes argumento = null;
			try
			{
				argumento = (Mes)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Mes).Name, objeto.GetType().Name));
			}
			return ano < argumento.ano || (ano == argumento.ano && mes.numero() < argumento.mes.numero()) ? Boolean.True : Boolean.False;
		}

        public override Boolean esMayorOIgualQue(Objeto objeto)
		{
			Mes argumento = null;
			try
			{
				argumento = (Mes)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Mes).Name, objeto.GetType().Name));
			}
			return ano > argumento.ano || (ano == argumento.ano && mes.numero() >= argumento.mes.numero()) ? Boolean.True : Boolean.False;
		}

		public virtual string nombre()
		{
			return mes.nombreCompleto();
		}
	}

}