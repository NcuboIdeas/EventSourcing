using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	class LineaEnBlanco : Linea
	{
		internal override void ejecutar()
		{
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append('\r');
		}
	}

}