using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	class ComandoNulo : Comando
	{
		private string lineaComentada;
		private Declaracion declaracion;

		public ComandoNulo(Declaracion declaracion)
		{
			this.declaracion = declaracion;
		}

		public ComandoNulo(string lineaComentada)
		{
			this.lineaComentada = lineaComentada;
		}

		public ComandoNulo()
		{
		}

		public override void Ejecutar()
		{

		}

		public override void ValidarEstaticamente()
		{
		}
		internal override void Write(StringBuilder resultado, int tabs)
		{
			if (string.ReferenceEquals(lineaComentada, null) && declaracion == null)
			{
				resultado.Append('\r');
			}
			else if (!string.ReferenceEquals(lineaComentada, null))
			{
				resultado.Append(lineaComentada);
				resultado.Append('\r');
			}
			else if (declaracion != null)
			{
				tabs++;
				declaracion.write(resultado, tabs);
				tabs--;
			}
		}

	}

}