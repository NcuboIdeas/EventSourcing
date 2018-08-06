using System;

namespace Puppeteer.EventSourcing.Libraries
{


	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Fecha : Objeto, IComparable<Fecha>
	{
		private int dia_Renamed;
		private int mes_Renamed;
		private int anno_Renamed;

		public Fecha(int dia, int mes, int anno)
		{
			this.dia_Renamed = dia;
			this.mes_Renamed = mes;
			this.anno_Renamed = anno;
			validaCreacionDeFecha(dia, mes, anno);
		}

		public Fecha(int dia, Meses mes, int anno) : this(dia, mes.numero(), anno)
		{
		}

		public static Fecha minFecha()
		{
			return new Fecha(1, 1, 1950);
		}

		private static readonly int[] diasDelMes = new int[]{31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};

		private void validaCreacionDeFecha(int dia, int mes, int anno)
		{
			if (mes < 1 || mes > 12)
			{
				throw new BusinessLogicalException("El mes ingresado no es valido debe estar contenido entre 1-12");
			}
			bool esAnoBisiesto = (anno % 4 == 0) && ((anno % 100 != 0) || (anno % 400 == 0));
			if (dia == 29 && mes == 2 && esAnoBisiesto)
			{
				//Esta Ok.
			}
			else if (dia < 1 || dia > diasDelMes[mes - 1])
			{
				throw new BusinessLogicalException(string.Format("El dia ingresado no es valido debe estar contenido entre 1 y {0}", diasDelMes[mes - 1]));
			}
			if (anno < 1950 || anno > 2050)
			{
				throw new BusinessLogicalException("El anno ingresado no es valido debe poseer 4 digitos");
			}
		}

		public virtual DateTime Date
		{
			get
			{
				return covertirFechaACalendar();
			}
		}

		public virtual int Dia
		{
			get
			{
				return dia_Renamed;
			}
		}

		public virtual int Mes
		{
			get
			{
				return mes_Renamed;
			}
		}

		public virtual int Anno
		{
			get
			{
				return anno_Renamed;
			}
		}

		public virtual Mes mesDeLaFecha()
		{
			return new Mes(Meses.obtenerMes(mes_Renamed), anno_Renamed);
		}

		public virtual FechaHora toFechaHora()
		{
			return new FechaHora(dia_Renamed, mes_Renamed, anno_Renamed, 0, 0, 0);
		}

		internal virtual Fecha siguienteDia()
		{
			int sgteDia = dia_Renamed;
			int sgteMes = mes_Renamed;
			int sgteAno = anno_Renamed;
			if (dia_Renamed == mesDeLaFecha().diasDelMes())
			{
				sgteDia = 1;
				sgteMes++;
			}
			else
			{
				sgteDia++;
			}
			if (sgteMes == 13)
			{
				sgteMes = 1;
				sgteAno++;
			}
			return new Fecha(sgteDia, sgteMes, sgteAno);
		}

		internal virtual Fecha restarMeses(int mesesPorRestar)
		{
			Mes mes = this.mesDeLaFecha();
			for (int i = 0; i < mesesPorRestar; i++)
			{
				mes = mes.mesAnterior();
			}
			int diaAnterior = dia_Renamed > mes.diasDelMes() ? mes.diasDelMes() : dia_Renamed;
            return new Fecha(diaAnterior, mes.MesDelAnno, mes.Anno);
		}

		internal virtual Fecha diaAnterior()
		{
			int diaAnterior = dia_Renamed;
			int mesAnterior = mes_Renamed;
			int anoAnterior = anno_Renamed;
			if (dia_Renamed == 1 & mes_Renamed == 1)
			{
				mesAnterior = 12;
				anoAnterior--;

				diaAnterior = (new Mes(Meses.obtenerMes(mesAnterior), anoAnterior)).diasDelMes();
			}
			if (dia_Renamed == 1 & mes_Renamed != 1)
			{
				mesAnterior--;
				diaAnterior = (new Mes(Meses.obtenerMes(mesAnterior), anoAnterior)).diasDelMes();
			}
			if (dia_Renamed != 1)
			{
				diaAnterior--;
			}
			return new Fecha(diaAnterior, mesAnterior, anoAnterior);
		}

		internal virtual Fecha sumarDia(int dia)
		{			
            DateTime fechaASumar = new DateTime(this.anno_Renamed, this.mes_Renamed, this.dia_Renamed);
			DateTime nuevaFecha = fechaASumar.AddDays(dia);
            return new Fecha(nuevaFecha.Day, nuevaFecha.Month, nuevaFecha.Year);
		}

		public override string print()
		{
			string result = ToString();
			return result;
		}

		public override string ToString()
		{
			string resultado = (mes_Renamed < 10 ? "0" + mes_Renamed : "" + mes_Renamed) + (dia_Renamed < 10 ? "/0" + dia_Renamed : "/" + dia_Renamed) + "/" + anno_Renamed;
            resultado = '"' + resultado + '"';
            return resultado;
		}

        public string MMMddyyyy()
        {
            string[] meses = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string resultado = meses[mes_Renamed-1] + " " + dia_Renamed + ", " + anno_Renamed;
            return resultado;
        }

		public virtual int cantidadDiasQueHayAl(Fecha otraFecha)
		{
			DateTime miFecha = this.covertirFechaACalendar();
			DateTime laOtraFecha = otraFecha.covertirFechaACalendar();

			int resultado = (int)((laOtraFecha.Ticks - miFecha.Ticks) / (1000 * 60 * 60 * 24));
			return resultado;
		}

		public virtual bool esUnFinDeSemana()
		{
			DateTime cal = covertirFechaACalendar();
            return cal.DayOfWeek == DayOfWeek.Saturday || cal.DayOfWeek == DayOfWeek.Sunday;
		}

		public virtual bool esUnDomingo()
		{
			DateTime cal = covertirFechaACalendar();
            return (cal.DayOfWeek == DayOfWeek.Sunday);
		}

		private DateTime covertirFechaACalendar()
		{
            DateTime cal = new DateTime (anno_Renamed, mes_Renamed - 1, dia_Renamed);
			return cal;
		}

		public virtual bool esIgualQue(Fecha literal)
		{
			return dia_Renamed == literal.Dia && mes_Renamed == literal.Mes && anno_Renamed == literal.Anno;
		}

        public override Boolean esMayorQue(Objeto otraFecha)
		{
			try
			{
                return otraFecha is Fecha ? esMayorQue((Fecha)otraFecha) : this.toFechaHora().esMayorQue(otraFecha);
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Fecha).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMenorQue(Objeto otraFecha)
		{
			try
			{
				return otraFecha is Fecha ? esMenorQue((Fecha) otraFecha) : this.toFechaHora().esMenorQue(otraFecha);
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Fecha).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMayorOIgualQue(Objeto otraFecha)
		{
			try
			{
				return otraFecha is Fecha ? esMayorOIgualQue((Fecha) otraFecha) : this.toFechaHora().esMayorOIgualQue(otraFecha);
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Fecha).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMenorOIgualQue(Objeto otraFecha)
		{
			try
			{
				return otraFecha is Fecha ? esMenorOIgualQue((Fecha) otraFecha) : this.toFechaHora().esMenorOIgualQue(otraFecha);
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Fecha).Name, otraFecha.GetType().Name));
			}
		}

        private Boolean esMayorQue(Fecha otraFecha)
		{
			int miFecha = anno_Renamed * 10000 + mes_Renamed * 100 + dia_Renamed; //22/11/2013 => 20131122
			int laOtraFecha = otraFecha.anno_Renamed * 10000 + otraFecha.mes_Renamed * 100 + otraFecha.dia_Renamed;
			return miFecha > laOtraFecha ? Boolean.True : Boolean.False;
		}

        private Boolean esMenorQue(Fecha otraFecha)
		{
			int miFecha = anno_Renamed * 10000 + mes_Renamed * 100 + dia_Renamed; //22/11/2013 => 20131122
			int laOtraFecha = otraFecha.anno_Renamed * 10000 + otraFecha.mes_Renamed * 100 + otraFecha.dia_Renamed;
			return miFecha < laOtraFecha ? Boolean.True : Boolean.False;
		}

        private Boolean esMayorOIgualQue(Fecha otraFecha)
		{
			int miFecha = anno_Renamed * 10000 + mes_Renamed * 100 + dia_Renamed; //22/11/2013 => 20131122
			int laOtraFecha = otraFecha.anno_Renamed * 10000 + otraFecha.mes_Renamed * 100 + otraFecha.dia_Renamed;
			return miFecha >= laOtraFecha ? Boolean.True : Boolean.False;
		}

        private Boolean esMenorOIgualQue(Fecha otraFecha)
		{
			int miFecha = anno_Renamed * 10000 + mes_Renamed * 100 + dia_Renamed; //22/11/2013 => 20131122
			int laOtraFecha = otraFecha.anno_Renamed * 10000 + otraFecha.mes_Renamed * 100 + otraFecha.dia_Renamed;
			return miFecha <= laOtraFecha ? Boolean.True : Boolean.False;
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			try
			{
				if (objeto is Fecha)
				{
					Fecha fecha = (Fecha)objeto;
					return dia_Renamed == fecha.dia_Renamed && mes_Renamed == fecha.mes_Renamed && anno_Renamed == fecha.anno_Renamed ? Boolean.True : Boolean.False;
				}
				else
				{
					FechaHora fecha = (FechaHora)objeto;
					return this.toFechaHora().esIgualQue(fecha);
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Fecha).Name, objeto.GetType().Name));
			}

		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

		public override int GetHashCode()
		{
			const int prime = 31;
			int resultado = 1;
			resultado = prime * resultado + anno_Renamed;
			resultado = prime * resultado + dia_Renamed;
			resultado = prime * resultado + mes_Renamed;
			return resultado;
		}

        public override bool Equals(object objeto)
		{
			Fecha otraFecha = (Fecha) objeto;
			return anno_Renamed == otraFecha.anno_Renamed && dia_Renamed == otraFecha.dia_Renamed && mes_Renamed == otraFecha.mes_Renamed;
		}

		public virtual int CompareTo(Fecha fecha)
		{
			int id = anno_Renamed * 10000 + mes_Renamed * 100 + dia_Renamed;
			int idArgumento = fecha.anno_Renamed * 10000 + fecha.mes_Renamed * 100 + fecha.dia_Renamed;
			return id - idArgumento;
		}

		
		public virtual string DiaDeLaSemana
		{
			get
			{
                string[] DIAS_DE_LA_SEMANA = new string[] { "Sun", "Mon", "Tue", "Wed", "Thru", "Fri", "Sat" };
                switch (covertirFechaACalendar().DayOfWeek)
                {
                    case DayOfWeek.Sunday: return DIAS_DE_LA_SEMANA[0];
                    case DayOfWeek.Monday: return DIAS_DE_LA_SEMANA[1];
                    case DayOfWeek.Tuesday: return DIAS_DE_LA_SEMANA[2];
                    case DayOfWeek.Wednesday: return DIAS_DE_LA_SEMANA[3];
                    case DayOfWeek.Thursday: return DIAS_DE_LA_SEMANA[4];
                    case DayOfWeek.Friday: return DIAS_DE_LA_SEMANA[5];
                    default: return DIAS_DE_LA_SEMANA[6];
                }
			}
		}

		public static Fecha fromString(string unaFecha)
		{
			int dia = parseDia(unaFecha);
			int mes = parseMes(unaFecha);
			int anno = parseAnno(unaFecha);
			return new Fecha(dia, mes, anno);
		}

        private static int parseDia(string unaFecha)
		{
			const char SEPARADOR = '/';
			int index = unaFecha.IndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(0, index));
		}

        private static int parseMes(string unaFecha)
		{
			const char SEPARADOR = '/';
			int indexIni = unaFecha.IndexOf(SEPARADOR);
			int indexFin = unaFecha.LastIndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(indexIni + 1, indexFin - (indexIni + 1)));
		}

		private static int parseAnno(string unaFecha)
		{
			const char SEPARADOR = '/';
			int index = unaFecha.LastIndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(index + 1));
		}
	}
}