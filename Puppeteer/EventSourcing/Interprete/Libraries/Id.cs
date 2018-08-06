using Puppeteer.EventSourcing.Libraries;
using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class Id : Expresion
	{
		private string id;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public Id(TablaDeSimbolos tablaDeSimbolos, string id)
		{
			this.tablaDeSimbolos = tablaDeSimbolos;
			this.id = id;
		}

		public virtual string Valor
		{
			get
			{
				return id;
			}
		}

		internal override Type calcularTipo()
		{
            if (tablaDeSimbolos.ExisteLaVariable(id))
            {
                return ejecutar().GetType();
            }
            else
            {
                return Nulo.NULO.GetType();
            }
		}

		public override Objeto ejecutar()
		{
			Objeto valor = tablaDeSimbolos.Valor(id);
            if (valor == null)
            {
                throw new LanguageException(string.Format("La variable {0} no ha sido definida. Verifique si ya creó la variable y que no cometió ningún error en la escritura.", id));
            }
            return valor;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(id);
		}


	}

}