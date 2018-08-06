using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Interprete
{


	public class Token
	{

		private TokenType type;
		private string valor;

		

		public enum TokenType
		{
			nulo,
			@as,
			procedure,
			ELSE,
            EVAL,
			IF,
            FOR,
			begin,
			end,
			comentarioDeLinea,
			boolTrue,
			boolFalse,
			monto,
			fecha,
			mes,
			hora,
			@decimal,
			numero,
			igual,
			igualdad,
			desigualdad,
			negacionLogica,
			yLogico,
			oLogico,
			suma,
			resta,
			division,
			coma,
			punto,
			puntoComa,
			rParentesis,
			lParentesis,
            dosPuntos,
			print,
			hilera,
			mayor,
			menor,
			mayorIgual,
			menorIgual,
			eof,
			multiplicacion,
			eol,
			id
		}

		public Token(TokenType type, string valor)
		{
			this.type = type;

			if (this.type == TokenType.hilera)
			{
				this.valor = valor.Substring(1, (valor.Length - 1) - 1);
			}
			else
			{
				this.valor = valor;
			}
		}

		public virtual TokenType Type
		{
			get
			{
				return type;
			}
			set
			{
				this.type = value;
			}
		}


		public virtual string Valor
		{
			get
			{
				return valor;
			}
			set
			{
				this.valor = value;
			}
		}


		public override string ToString()
		{
			return "Valor: " + valor + " Tipo: " + type.ToString();
		}
	}

}