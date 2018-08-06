using System;
using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Libraries
{


	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class FechaHora : Objeto, IComparable<FechaHora>
	{
		private int dia;
		private int mes;
		private int anno;
		private int hora_Renamed;
		private int minutos;
		private int segundos;

		public FechaHora(int dia, int mes, int anno, int hora, int minutos, int segundos)
		{
			ValidaCreacionDeFechaHora(dia, mes, anno, hora, minutos, segundos);
			this.hora_Renamed = hora;
			this.minutos = minutos;
			this.segundos = segundos;
			this.dia = dia;
			this.mes = mes;
			this.anno = anno;
		}

		private void ValidaCreacionDeFechaHora(int dia, int mes, int anno, int hora, int minutos, int segundos)
		{
			new Fecha(dia, mes, anno);
			if (hora >= 24)
			{
				throw new BusinessLogicalException(string.Format("La hora ingresada {0} no es valido debe estar contenido entre 0-23", hora));
			}

			if (minutos >= 60)
			{
				throw new BusinessLogicalException(string.Format("Los minutos ingresados {0} no son validos debe estar contenido entre 0-59", minutos));
			}

			if (segundos >= 60)
			{
				throw new BusinessLogicalException(string.Format("Los segundos ingresados {0} no son validos debe estar contenido entre 0-59", segundos));
			}
		}

		public virtual int Dia
		{
			get
			{
				return dia;
			}
		}

		public virtual int Mes
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
				return anno;
			}
		}

		public virtual int Hora
		{
			get
			{
				return hora_Renamed;
			}
		}

		public virtual int Minutos
		{
			get
			{
				return minutos;
			}
		}

		public virtual int Segundos
		{
			get
			{
				return segundos;
			}
		}

		public virtual int CantidadDeHorasQueHayAl(FechaHora otraFechaHora)
		{
			DateTime miFecha = CovertirFechaADate();
			DateTime laOtraFecha = otraFechaHora.CovertirFechaADate();


			int resultado = (int)((laOtraFecha.Ticks - miFecha.Ticks) / (1000 * 60 * 60));
			return resultado;
		}

		private DateTime CovertirFechaADate()
		{
            DateTime dateRepresentation = new DateTime(anno, mes, dia, hora_Renamed, minutos, segundos);
            return dateRepresentation;
		}

		public virtual Mes MesDeLaFecha()
		{
			return new Mes(Meses.obtenerMes(mes), anno);
		}

		public virtual string ToMySQLFormat()
		{
			return "\'" + anno + '-' + mes + '-' + dia + ' ' + hora_Renamed + ':' + minutos + ':' + segundos + "\'";
		}

		public override string print()
		{
			string result = ToString();
			return result;
		}

		public override string ToString()
		{
			string resultado = (mes < 10 ? "0" + mes : "" + mes) + (dia < 10 ? "/0" + dia : "/" + dia) + "/" + anno + ' ' + (hora_Renamed < 10 ? "0" + hora_Renamed : "" + hora_Renamed) + (minutos < 10 ? ":0" + minutos : ":" + minutos) + (segundos < 10 ? ":0" + segundos : ":" + segundos);
            resultado = '"' + resultado + '"';
            return resultado;
		}

        public string MMMddyyyy()
        {
            string[] meses = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            string resultado = meses[mes-1] + " " + dia + ", " + anno;
            if (hora_Renamed != 0 && minutos != 0 && segundos != 0)
            {
                resultado = resultado + " " + (hora_Renamed < 10 ? "0" + hora_Renamed : "" + hora_Renamed);
                if (minutos != 0 && segundos != 0)
                {
                    resultado = resultado + ":" + (minutos < 10 ? "0" + minutos : "" + minutos);
                    if (segundos != 0)
                    {
                        resultado = resultado + ":" + (segundos < 10 ? "0" + segundos : "" + segundos);
                    }
                }
            }
            return resultado;
        }

        public string HHmmAMPM()
        {
            string resultado = hora_Renamed < 10 ? "0" + hora_Renamed : hora_Renamed <= 12 ? "" + hora_Renamed : "" + (hora_Renamed - 12);
            resultado = resultado + ":" + (minutos < 10 ? "0" + minutos : "" + minutos);
            resultado = hora_Renamed >= 12 ? resultado + " PM" : resultado + " AM";
            return resultado;
        }

        public override Boolean esMayorOIgualQue(Objeto otraFecha)
		{
			try
			{
				if (otraFecha is FechaHora)
				{
                    return this.EsMayorOIgualQue((FechaHora)otraFecha).valor ? Boolean.True : Boolean.False;
				}
				else
				{
					Fecha fecha = (Fecha) otraFecha;
                    return this.EsMayorOIgualQue(new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, 0, 0, 0)).valor ? Boolean.True : Boolean.False;
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(FechaHora).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMenorOIgualQue(Objeto otraFecha)
		{
			try
			{
				if (otraFecha is FechaHora)
				{
                    return this.EsMenorOIgualQue((FechaHora)otraFecha).valor ? Boolean.True : Boolean.False;
				}
				else
				{
					Fecha fecha = (Fecha) otraFecha;
                    return this.EsMenorOIgualQue(new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, 0, 0, 0)).valor ? Boolean.True : Boolean.False;
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(FechaHora).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMenorQue(Objeto otraFecha)
		{
			try
			{
				if (otraFecha is FechaHora)
				{
                    return this.EsMenorQue((FechaHora)otraFecha).valor ? Boolean.True : Boolean.False;
				}
				else
				{
					Fecha fecha = (Fecha) otraFecha;
                    return this.EsMenorQue(new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, 0, 0, 0)).valor ? Boolean.True : Boolean.False;
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(FechaHora).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esMayorQue(Objeto otraFecha)
		{
			try
			{
				if (otraFecha is FechaHora)
				{
                    return this.EsMayorQue((FechaHora)otraFecha).valor ? Boolean.True : Boolean.False;
				}
				else
				{
					Fecha fecha = (Fecha) otraFecha;
                    return this.EsMayorQue(new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, 0, 0, 0)).valor ? Boolean.True : Boolean.False;
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(FechaHora).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean esIgualQue(Objeto otraFecha)
		{
			try
			{
				if (otraFecha is FechaHora)
				{
                    return this.EsIgualQue((FechaHora)otraFecha).valor ? Boolean.True : Boolean.False;
				}
				else
				{
					Fecha fecha = (Fecha) otraFecha;
                    return this.EsIgualQue(new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, 0, 0, 0)).valor ? Boolean.True : Boolean.False;
				}
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(FechaHora).Name, otraFecha.GetType().Name));
			}
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

        private Boolean EsIgualQue(FechaHora fechaHora)
		{
			return dia == fechaHora.dia && mes == fechaHora.mes && anno == fechaHora.anno && hora_Renamed == fechaHora.hora_Renamed && minutos == fechaHora.minutos && segundos == fechaHora.segundos ? Boolean.True : Boolean.False;
		}

        private Boolean EsMayorOIgualQue(FechaHora otraFecha)
		{
			DateTime miFecha = CovertirFechaADate();
			DateTime laOtraFecha = otraFecha.CovertirFechaADate();

			bool esMayor = miFecha > laOtraFecha;
			bool sonIguales = miFecha.Equals(laOtraFecha);

			if (esMayor || sonIguales)
			{
				return Boolean.True;
			}
			else
			{
				return Boolean.False;
			}
		}

        private Boolean EsMayorQue(FechaHora otraFecha)
		{
			DateTime miFecha = CovertirFechaADate();
			DateTime laOtraFecha = otraFecha.CovertirFechaADate();

			return miFecha > laOtraFecha ? Boolean.True : Boolean.False;
		}

        private Boolean EsMenorOIgualQue(FechaHora otraFecha)
		{
			DateTime miFecha = CovertirFechaADate();
			DateTime laOtraFecha = otraFecha.CovertirFechaADate();

			return laOtraFecha > miFecha || miFecha.Equals(laOtraFecha) ? Boolean.True : Boolean.False;
		}

        private Boolean EsMenorQue(FechaHora otraFecha)
		{
			DateTime miFecha = CovertirFechaADate();
			DateTime laOtraFecha = otraFecha.CovertirFechaADate();

			return laOtraFecha > miFecha ? Boolean.True : Boolean.False;
		}

		public virtual Fecha Fecha()
		{
			return new Fecha(dia, mes, anno);
		}

		public override int GetHashCode()
		{
			const int prime = 31;
			int result = 1;
			result = prime * result + anno;
			result = prime * result + dia;
			result = prime * result + hora_Renamed;
			result = prime * result + mes;
			result = prime * result + minutos;
			result = prime * result + segundos;
			return result;
		}

        public override bool Equals(object objeto)
		{
			FechaHora otraFecha = (FechaHora) objeto;

			return anno == otraFecha.anno && mes == otraFecha.mes && dia == otraFecha.dia && hora_Renamed == otraFecha.hora_Renamed && minutos == otraFecha.minutos && segundos == otraFecha.segundos;
		}

		public virtual Fecha ToFecha()
		{
			return new Fecha(dia, mes, anno);
		}

		public virtual int CompareTo(FechaHora fechaHora)
		{
			int id = GetHashCode();
			int idArgumento = fechaHora.GetHashCode();

			return id - idArgumento;
		}

		public virtual int CantidadDiasQueHayAl(FechaHora otraFecha)
		{
			DateTime miFecha = this.CovertirFechaADate();
			DateTime laOtraFecha = otraFecha.CovertirFechaADate();

			int resultado = (int)((laOtraFecha.Ticks - miFecha.Ticks) / (1000 * 60 * 60 * 24));
			return resultado;
		}

		public FechaHora FechaDiasAtras(Numero cantidad)
		{
            DateTime calendar = CovertirFechaADate().AddDays((double)-cantidad.valor);
			FechaHora fechaActual = new FechaHora(calendar.Day, calendar.Month + 1, calendar.Year, calendar.Hour, calendar.Minute, calendar.Second);

			return fechaActual;
		}

		public FechaHora FechaMesesAtras(Numero cantidad)
		{
            DateTime calendar = CovertirFechaADate().AddMonths(-cantidad.valor);
            FechaHora fechaActual = new FechaHora(calendar.Day, calendar.Month + 1, calendar.Year, calendar.Hour, calendar.Minute, calendar.Second);

			return fechaActual;
		}

		internal virtual Hilera FechaCompleta()
		{
			Fecha fecha = new Fecha(dia, Mes, anno);
            String[] mes = new String[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "enero", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre" };

			string fechaEnCadena = fecha.DiaDeLaSemana + " " + fecha.Dia + " de " + mes[fecha.Mes-1] + " del " + fecha.Anno + ", " + hora_Renamed + ":" + minutos;
			return new Hilera(fechaEnCadena);
		}

		public static FechaHora FromString(Fecha fecha, string unaFecha)
		{
			int hora = ParseHora(unaFecha);
			int minuto = ParseMinuto(unaFecha);
			int segundo = ParseSegundo(unaFecha);
			return new FechaHora(fecha.Dia, fecha.Mes, fecha.Anno, hora, minuto, segundo);
		}

        private static int ParseHora(string unaFecha)
		{
			const char SEPARADOR = ':';
			int index = unaFecha.IndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(0, index));
		}

        private static int ParseMinuto(string unaFecha)
		{
			const char SEPARADOR = ':';
			int indexIni = unaFecha.IndexOf(SEPARADOR);
			int indexFin = unaFecha.LastIndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(indexIni + 1, indexFin - (indexIni + 1)));
		}

		private static int ParseSegundo(string unaFecha)
		{
			const char SEPARADOR = ':';
			int index = unaFecha.LastIndexOf(SEPARADOR);
			return int.Parse(unaFecha.Substring(index + 1));
		}

	}


}