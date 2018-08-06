using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public abstract class Declaracion : AST
	{
		public abstract void guardar();

		public abstract void write(StringBuilder resultado, int tabs);

	}

}