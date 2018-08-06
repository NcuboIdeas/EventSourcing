using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	class ComandoIf : Comando
	{
		private readonly Expresion expresion;
		private readonly Comando comandosDelIf;
		private readonly Comando comandosDelElse;

		public ComandoIf(Expresion expresion, Comando comandosDelIf)
		{
			this.expresion = expresion;
			this.comandosDelIf = comandosDelIf;
			this.comandosDelElse = null;
		}

		public ComandoIf(Expresion expresion, Comando comandoDeIf, Comando comandoDeElse)
		{
			this.expresion = expresion;
			this.comandosDelIf = comandoDeIf;
			this.comandosDelElse = comandoDeElse;
		}

        public override void Ejecutar()
		{
			Objeto valorDeLaExpresion = expresion.ejecutar();
			bool cumpleCondicion = ((EventSourcing.Libraries.Boolean) valorDeLaExpresion).Valor;
			if (cumpleCondicion)
			{
				comandosDelIf.Ejecutar();
			}
			else if (comandosDelElse != null)
			{
				comandosDelElse.Ejecutar();
			}
		}

		public override void ValidarEstaticamente()
		{
			expresion.validarEstaticamente();
            Type tipoExpresion = expresion.calcularTipo();
            if (tipoExpresion != typeof(Puppeteer.EventSourcing.Libraries.Boolean))
            {
                throw new LanguageException("Sólo es posible ejecutar un IF cuando la expresión es de tipo Boolean");
            }
            comandosDelIf.ValidarEstaticamente();
			if (comandosDelElse != null)
			{
				comandosDelElse.ValidarEstaticamente();
			}
		}

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("If (");
			expresion.write(resultado);
			resultado.Append(")\r");

			if (!(comandosDelIf is ComandoBloque))
			{
				tabs++;
			}
			comandosDelIf.Write(resultado, tabs);
			if (!(comandosDelIf is ComandoBloque))
			{
				tabs--;
			}

			if (comandosDelElse != null)
			{
				resultado.Append(GenerarTabs(tabs));
				resultado.Append("Else\r");
				if (!(comandosDelElse is ComandoBloque))
				{
					tabs++;
				}
				comandosDelElse.Write(resultado, tabs);
				if (!(comandosDelElse is ComandoBloque))
				{
					tabs--;
				}
			}
		}
	}
}