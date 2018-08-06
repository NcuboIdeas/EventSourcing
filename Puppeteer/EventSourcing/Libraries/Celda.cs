namespace Puppeteer.EventSourcing.Libraries
{

	public class Celda : Objeto
	{
		private string nombre;
		private Objeto valor;

		public Celda(string nombre, Objeto valor)
		{
			this.nombre = nombre;
			this.valor = valor;
		}

		public Celda(Objeto valor)
		{
			this.nombre = "";
			this.valor = valor;
		}

		public virtual string Nombre
		{
			get
			{
				return nombre;
			}
		}


		public virtual Objeto Valor
		{
			get
			{
				return valor;
			}
		}

		public override string print()
		{
			string resultado = "";

			Objeto seDebeLlamarAgetValorParaQueLaHerenciaResuelva = Valor;
			Objeto valor = seDebeLlamarAgetValorParaQueLaHerenciaResuelva;

			bool elValorEnLaCeldaEsUnaTabla = valor.GetType() == typeof(Tabla);
			bool elValorEnLaCeldaEsTablaAtributos = valor.cantidadAtributos() > 0;
			if (elValorEnLaCeldaEsUnaTabla)
			{
				Tabla tabla = (Tabla)valor;
				if (nombre.Length == 0)
				{
					nombre = tabla.Nombre();
				}
				resultado = "\"" + this.nombre + "\":[" + tabla.filas() + "]";
			}
			else if (elValorEnLaCeldaEsTablaAtributos)
			{
				resultado = "\"" + this.nombre + "\":" + this.valor.print();
			}
			else if (nombre.Length > 0)
			{
				resultado = "\"" + this.nombre + "\":\"" + this.valor.ToString() + "\"";
			}
			else
			{
				resultado = this.valor.print();
			}
			return resultado;
		}

	}

}