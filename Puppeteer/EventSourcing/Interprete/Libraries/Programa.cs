using System.Collections.Generic;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	public class Programa : AST
	{
		private readonly Linea[] lineasDePrograma;
		private LineaDePrograma lineaEnEjecucion_Renamed;
		private readonly Salida salida;

        public Programa(Salida salida, Linea[] lineasDePrograma)
		{
			this.salida = salida;
			this.lineasDePrograma = lineasDePrograma;
        }

		public virtual string Ejecutar()
		{
			salida.Inicio();
            foreach (Linea linea in lineasDePrograma)
			{
				if (linea is LineaDePrograma)
				{
					lineaEnEjecucion_Renamed = (LineaDePrograma) linea;
				}
				linea.ejecutar();
            }
            salida.Fin();
			string resultado = salida.ToString();
			return resultado;
		}

		public virtual string LineaEnEjecucion()
		{
			return lineaEnEjecucion_Renamed.ultimoComandoEjecutado();
		}

		public virtual Linea[] LineasDelPrograma()
		{
			return lineasDePrograma;
		}

		public virtual string Write()
		{
			StringBuilder builder = new StringBuilder();
			foreach (Linea linea in lineasDePrograma)
			{
				linea.write(builder);
			}
			return builder.ToString();
		}
	}
}