using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;

	public class ComandoBloque : Comando
	{
		private Comando[] comandos;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public ComandoBloque(TablaDeSimbolos tablaDeSimbolos, Comando[] comandos)
		{
			this.comandos = comandos;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		public virtual Comando[] Comandos
		{
			get
			{
				return comandos;
			}
		}

		public override void Ejecutar()
		{
			tablaDeSimbolos.AbrirBloque();
			 foreach (Comando comando in comandos)
			 {
				 comando.Ejecutar();
			 }
			 tablaDeSimbolos.CerrarBloque();
		}

		public override void ValidarEstaticamente()
		{
			tablaDeSimbolos.AbrirBloque();
			foreach (Comando comando in comandos)
			{
				comando.ValidarEstaticamente();
			}
			tablaDeSimbolos.CerrarBloque();
		}

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("{\r");
			tabs++;
			foreach (Comando comando in comandos)
			{
				comando.Write(resultado, tabs);
			}
			tabs--;
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("}\r");
		}
	}

}