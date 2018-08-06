using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class PuntoConPunto : Punto
	{
		private Punto instancia;

		public PuntoConPunto(Punto instancia, string metodo, Expresion[] argumentos) : base(metodo, argumentos)
		{
			this.instancia = instancia;
		}

		public PuntoConPunto(Punto instancia, string propiedad) : base(propiedad)
		{
			this.instancia = instancia;
		}

		protected internal override object ObtenerElObjeto()
		{
			Objeto resultado = instancia.ejecutar();
			return resultado;
		}

		internal override Type calcularTipo()
		{
			Type classDeLaInstancia = instancia.calcularTipo();
			Type resultado = CalcularElTipoDeUnCallExpresion(classDeLaInstancia);
			return resultado;
		}

		internal override void write(StringBuilder resultado)
		{
			instancia.write(resultado);

			resultado.Append('.');

			if (!string.ReferenceEquals(Propiedad(), null))
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