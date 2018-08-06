using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public abstract class Linea : AST
	{
		internal abstract void ejecutar();

		internal abstract void write(StringBuilder resultado);

	}

}