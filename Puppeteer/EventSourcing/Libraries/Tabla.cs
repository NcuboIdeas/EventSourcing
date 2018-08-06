using System.Collections.Generic;
using System.Text;

namespace Puppeteer.EventSourcing.Libraries
{


	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;
    using System.Collections;

	public class Tabla : Objeto, IEnumerable<Celda>
	{
		 private IList<Fila> filas_Renamed;
		 private string nombre;

		public Tabla()
		{
			filas_Renamed = new List<Fila>();
		}

		public virtual void agregarFila(params Celda[] celdas)
		{
			this.filas_Renamed.Add(new Fila(celdas));
			foreach (Celda c in celdas)
			{
				if (c is CeldaSuma)
				{
					((CeldaSuma) c).tabla(this);
				}
			}
		}

        public IEnumerator<Celda> GetEnumerator()
		{
			foreach (Fila f in filas_Renamed)
			{
				foreach (Celda c in f)
				{
					yield return c;
				}
			}
		}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); // Just return the generic version
        }

		public override Boolean esIgualQue(Objeto objeto)
		{
			Tabla tabla = null;
			try
			{
				tabla = (Tabla)objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new BusinessLogicalException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontró un valor de tipo [{1}]", typeof(Tabla).Name, objeto.GetType().Name));
			}
			return this == tabla ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

		public override string print()
		{
			StringBuilder builder = new StringBuilder();
			string encabezadoAnterior = "";
			foreach (Fila fila in filas_Renamed)
			{
				string encabezadoActual = fila.encabezado();
				if (!encabezadoAnterior.Equals(encabezadoActual))
				{
					builder.Append(encabezadoActual + "\r\n");
					encabezadoAnterior = encabezadoActual;
				}
				builder.Append(fila.print());
				builder.Append('\r');
				builder.Append('\n');
			}
			return builder.ToString();
		}

		public virtual string filas()
		{
			string filasSt = "";

			for (int i = 0; i < filas_Renamed.Count; i++)
			{
				Fila fila = filas_Renamed[i];

				if (fila != null)
				{
					filasSt += fila.print();
				}
				if (i + 1 < filas_Renamed.Count)
				{
					filasSt += ",";
				}
			}
			return filasSt;
		}

		public virtual string Nombre()
		{
			return nombre;
		}

		public virtual void defineNombreDeLaTabla(string nombre)
		{
			this.nombre = nombre;
		}

		public virtual Numero cantidadFilas()
		{
			return new Numero(filas_Renamed.Count);
		}

		public virtual Numero cantidadColumnas()
		{
			return new Numero(filas_Renamed[0].cantidadCeldas());
		}

	}

	internal class Fila : Objeto, IEnumerable<Celda>
	{
		private List<Celda> objetos = new List<Celda>();

		public Fila(params Celda[] celdas)
		{
			foreach (Celda c in celdas)
			{
				this.objetos.Add(c);
			}
		}

		internal virtual string encabezado()
		{
			string json = "";
			int totalDeFilas = this.objetos.Count - 1;
			for (int i = 0; i <= totalDeFilas; i++)
			{
				Celda celda = ((Celda) objetos[i]);
				string titulo = "";
				int ancho = celda.Valor.print().Length;
				if (celda is CeldaSuma)
				{
					ancho = ancho - 3;
					titulo = "S(";
				}
				titulo = titulo + "\"" + celda.Nombre;
				titulo = titulo.Substring(0, System.Math.Min(ancho - 1, titulo.Length)) + "\"";
				if (celda is CeldaSuma)
				{
					titulo = titulo + ")";
				}
				while (titulo.Length < ancho)
				{
					titulo += ' ';
				}
				json += titulo;
				bool noEsLaUltimaFila = (i != totalDeFilas);
				if (noEsLaUltimaFila)
				{
					json += ", ";
				}
			}
			return json;
		}

        public IEnumerator<Celda> GetEnumerator()
        {
            return (IEnumerator<Celda>)objetos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); // Just return the generic version
        }

		protected internal virtual Objeto obtenerCelda(int columna)
		{
			return ((Celda) objetos[columna]).Valor;
		}

		protected internal virtual int cantidadCeldas()
		{
			return objetos.Count;
		}

		public override string print()
		{
			string json = "";
			int totalDeFilas = this.objetos.Count - 1;
			for (int i = 0; i <= totalDeFilas; i++)
			{
				json += obtenerCelda(i).print();
				bool noEsLaUltimaFila = (i != totalDeFilas);
				if (noEsLaUltimaFila)
				{
					json += ", ";
				}
			}
			return json;
		}

	}


}