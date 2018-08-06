using System;
using System.Reflection;
using System.Text;

using Puppeteer.EventSourcing.Libraries;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class ComandoEval : Comando
	{
		private Expresion expresion;
		private readonly TablaDeSimbolos tablaDeSimbolos;
        private readonly Salida salida;
        private readonly Assembly assembly;

        public ComandoEval(Assembly assembly, TablaDeSimbolos tablaDeSimbolos, Salida salida, Expresion expresion)
		{
			this.expresion = expresion;
			this.tablaDeSimbolos = tablaDeSimbolos;
            this.salida = salida;
            this.assembly = assembly;
		}

		public override void Ejecutar()
		{
			string script = ((Hilera) expresion.ejecutar()).Valor;
            Salida salidaExpresion = new Salida();
            Parser parser = new Parser(assembly, tablaDeSimbolos, salidaExpresion);
            parser.EstablecerComando(script);
            Programa programa = parser.Procesar();
            string resultado = programa.Ejecutar();
            if (!String.IsNullOrEmpty(resultado) && resultado != "{}")
            {
                resultado = resultado.Substring(1, resultado.Length - 2);
                salida.AppendStream(resultado);
            }
        }

        public override void ValidarEstaticamente()
        {
            expresion.validarEstaticamente();
            Type tipoExpresion = expresion.calcularTipo();
            if (tipoExpresion != typeof(Hilera))
            {
                throw new LanguageException("Sólo es posible ejecutar un Eval cuando la expresión es de tipo Hilera");
            }
        }

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
            resultado.Append("Eval (");
			expresion.write(resultado);
			resultado.Append(");\r");
		}
	}

}