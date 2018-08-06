using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class IdConPunto : Punto
	{

        private string instancia_Renamed;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public IdConPunto(TablaDeSimbolos tablaDeSimbolos, Id instancia, string metodo, Expresion[] argumentos) : base(metodo, argumentos)
		{
			this.instancia_Renamed = instancia.Valor;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		public IdConPunto(TablaDeSimbolos tablaDeSimbolos, Id instancia, string propiedad) : base(propiedad)
		{
			this.instancia_Renamed = instancia.Valor;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		internal virtual string instancia()
		{
			return instancia_Renamed;
		}

		internal virtual Type[] firmaNecesariaParaElMetodo()
		{
			Expresion[] argumentos = this.Argumentos();
			Type[] resultado = new Type[argumentos.Length];
			for (int i = 0; i < argumentos.Length; i++)
			{
				resultado[i] = argumentos[i].calcularTipo();
			}
			return resultado;
		}

		internal override Type calcularTipo()
		{
			Type resultado = CalcularElTipoDeUnCallExpresion(ObtenerElObjeto().GetType());
			return resultado;
		}

		protected internal override object ObtenerElObjeto()
		{
			if (!tablaDeSimbolos.ExisteLaVariable(instancia_Renamed))
			{
				throw new LanguageException("La variable '" + instancia_Renamed + "' es desconocida");
			}
			Objeto objetoInstancia = tablaDeSimbolos.Valor(instancia_Renamed);

			return (object) objetoInstancia;
		}

		internal override void write(StringBuilder resultado)
		{
			resultado.Append(instancia_Renamed);
			resultado.Append('.');

			if (Propiedad() != null)
			{
				resultado.Append(Propiedad());
			}
			else
			{
				Expresion[] argumentos = this.Argumentos();
				resultado.Append(Metodo());
				resultado.Append('(');
				for (int i = 0; i < argumentos.Length; i++)
				{
					if (i > 0)
					{
						resultado.Append(", ");
					}
					argumentos[i].write(resultado);
				}
				resultado.Append(')');

			}
		}
	}

}