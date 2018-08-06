namespace Puppeteer.EventSourcing.Libraries
{

	  public abstract class Atributo
	  {

		private string nombre_Renamed;
		private string nombreEnMayusculas_Renamed;
		private Objeto valor_Renamed;

		internal Atributo(string nombre, Objeto instancia)
		{
			this.nombre_Renamed = nombre;
			this.nombreEnMayusculas_Renamed = nombre.ToUpper();
			this.valor_Renamed = instancia;
		}

		internal virtual string nombre()
		{
			return nombre_Renamed;
		}

		internal virtual string nombreEnMayusculas()
		{
			return nombreEnMayusculas_Renamed;
		}

		internal virtual Objeto valor()
		{
			return valor_Renamed;
		}

		public override bool Equals(object o)
		{
			if (((Atributo)o).nombreEnMayusculas_Renamed.Equals(nombreEnMayusculas_Renamed))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			int hash = 7;
			hash = 97 * hash + nombre_Renamed.ToLower().GetHashCode();
			return hash;
		}
	  }
}