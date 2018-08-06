namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public abstract class AST
	{
		private static string tabsGenerados = "";
		private static int anteriorTamano = 0;

		protected internal static string GenerarTabs(int cantidad)
		{
			if (anteriorTamano != cantidad)
			{
				tabsGenerados = (new string(new char[cantidad])).Replace('\0', '\t');
				anteriorTamano = cantidad;
			}
			return tabsGenerados;
		}
	}

}