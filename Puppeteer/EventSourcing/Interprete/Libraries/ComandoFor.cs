using System;
using System.Text;

using Puppeteer.EventSourcing.DB;
using Puppeteer.EventSourcing.Libraries;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	class ComandoFor : Comando
	{
        private readonly Salida salida;
        private readonly string variable;
        private readonly TablaDeSimbolos tablaDeSimbolos;
		private readonly Expresion expresion;
		private readonly Comando cuerpo;

		public ComandoFor(Salida salida, TablaDeSimbolos tablaDeSimbolos, string variable, Expresion expresion, Comando cuerpo)
		{
            this.salida = salida;
            this.tablaDeSimbolos = tablaDeSimbolos;
            this.variable = variable;
			this.expresion = expresion;
            this.cuerpo = cuerpo;
		}

        public override void Ejecutar()
		{
            Lista valoresDeLaExpresion = (Lista) expresion.ejecutar();
            tablaDeSimbolos.AbrirBloque();
            salida.AbrirFor();
            for (int i = 0; i < valoresDeLaExpresion.Count(); i++)
            {
                tablaDeSimbolos.GuardarVariable(variable, Nulo.NULO);
                salida.InicioMoveNextDelFor();
                tablaDeSimbolos.GuardarVariable(variable, valoresDeLaExpresion.getObjeto(i));
                cuerpo.Ejecutar();
                salida.FinMoveNextDelFor();
            }
            salida.CerrarFor(variable);
            tablaDeSimbolos.CerrarBloque();
        }

		public override void ValidarEstaticamente()
		{
			expresion.validarEstaticamente();
            Type tipoExpresion = expresion.calcularTipo();
            if ( ! tipoExpresion.Equals(typeof(Lista)) )
            {
                throw new LanguageException("Sólo es posible ejecutar un For cuando la expresión es de tipo Lista");
            }
            tablaDeSimbolos.AbrirBloque();
            cuerpo.ValidarEstaticamente();
            tablaDeSimbolos.CerrarBloque();
        }

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("For ( ");
            resultado.Append(variable);
            resultado.Append(" : ");
            expresion.write(resultado);
			resultado.Append(" )\r");
            if (!(cuerpo is ComandoBloque))
            {
                tabs++;
            }
            cuerpo.Write(resultado, tabs);
            if (!(cuerpo is ComandoBloque))
            {
                tabs--;
            }
		}
	}
}