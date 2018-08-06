using Puppeteer.EventSourcing.Libraries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	class ComandoPrint : Comando
	{
		private readonly Expresion expression;
		private readonly Salida salida;
        private readonly String alias;

        public ComandoPrint(Salida salida, Expresion expression, String alias)
		{
			this.salida = salida;
			this.expression = expression;
            this.alias = alias;
		}

		public override void Ejecutar()
		{
			if (!salida.EstaEscribiendo())
			{
				return;
			}
			var resultado = expression.ejecutar();
            if (resultado == null)
            {
                throw new LanguageException("La salida de un Print no puede ser nula");
            }
            salida.Append(alias, resultado);
		}

		public override void ValidarEstaticamente()
		{
			Type tipoExpresion = expression.calcularTipo();
			if (tipoExpresion.Equals(typeof(void)))
			{
				throw new LanguageException(string.Format("Al parecer intenta mostar un procedimiento o accion, pero que el comando show solo permite mostrar valores"));
			}
			expression.validarEstaticamente();
		}

		internal override void Write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("Print ");
			expression.write(resultado);
            resultado.Append(' ');
            resultado.Append(alias);
            resultado.Append(";");
		}

	}

}