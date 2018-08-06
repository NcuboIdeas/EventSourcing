using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class ComandoCall : Comando
	{
		private Expresion expresion;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public ComandoCall(TablaDeSimbolos tablaDeSimbolos, Expresion expresion)
		{
			this.expresion = expresion;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		public override void Ejecutar()
		{
			expresion.ejecutar();
		}

		public override void ValidarEstaticamente()
		{
			if (expresion is NuevaInstancia)
			{
				string nombreProcedimiento = ((NuevaInstancia) expresion).Clase();
				if (!tablaDeSimbolos.ExisteLaDeclaracion(nombreProcedimiento))
				{
					throw new LanguageException(string.Format("No se encontró declarado el procedimiento {0}, debe declararlo primero.", nombreProcedimiento));
				}
			}
			else
			{
				Type tipoExpresion = expresion.calcularTipo();
				expresion.validarEstaticamente();
				if (!tipoExpresion.Equals(typeof(EventSourcing.Libraries.Void)))
				{
					throw new LanguageException(string.Format("Sólo es posible ejecutar procedimientos pero se está intentando ejecutar erróneamente un valor de tipo {0}", tipoExpresion.FullName));
				}
			}
		}

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			expresion.write(resultado);
			resultado.Append(";\r");
		}
	}

}