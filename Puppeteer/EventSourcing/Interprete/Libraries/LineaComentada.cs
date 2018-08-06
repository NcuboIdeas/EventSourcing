using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	class LineaComentada : Linea
	{
		private readonly string comentario;

		public LineaComentada(string comentario)
		{
			this.comentario = comentario;
		}

		internal override void ejecutar()
		{
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(comentario);
			resultado.Append('\r');
		}
	}

}