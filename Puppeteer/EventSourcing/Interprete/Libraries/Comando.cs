using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public abstract class Comando : AST
	{
		public abstract void Ejecutar();

		public abstract void ValidarEstaticamente();

		internal abstract void Write(StringBuilder resultado, int tabs);

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			Write(builder, 0);
			return builder.ToString();
		}
	}

}