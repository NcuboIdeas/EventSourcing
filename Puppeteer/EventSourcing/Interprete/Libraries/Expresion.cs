using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

    public abstract class Expresion : AST
    {

        protected bool coersionHilera = false;

        public bool CoersionHilera
        {
            get
            {
                return coersionHilera;
            }
            set
            {
                coersionHilera = value;
            }
        }

        public abstract Objeto ejecutar();

		internal abstract Type calcularTipo();

		internal virtual void validarEstaticamente()
		{
		}

		internal abstract void write(StringBuilder resultado);

	}


}