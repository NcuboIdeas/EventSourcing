namespace Puppeteer.EventSourcing.Libraries
{

	public class ArchivoDelFileServer : Objeto
	{
		private static string NOMBREGENERADO = "nombreGenerado";
		private static string NOMBREORIGINAL = "nombreOriginal";
		private static string LONGITUD = "longitud";

		public ArchivoDelFileServer(Hilera nombreGenerado, Hilera nombreOriginal, Numero longitud)
		{
			base.setAtributoFijo(NOMBREGENERADO, nombreGenerado);
			base.setAtributoFijo(NOMBREORIGINAL, nombreOriginal);
			base.setAtributoFijo(LONGITUD, longitud);
		}

		public override string print()
		{
			return base.generarTablaConLosAtributos().print();
		}

		public virtual Hilera obtenerNombreOriginal()
		{
			return new Hilera(base.getDatoDelAtributo(NOMBREORIGINAL).ToString());
		}

		public virtual Hilera obtenerNombreGenerado()
		{
			return new Hilera(base.getDatoDelAtributo(NOMBREGENERADO).ToString());
		}

		public virtual Numero obtenerLongitud()
		{
			return new Numero(int.Parse(base.getDatoDelAtributo(LONGITUD).ToString()));
		}
	}

}