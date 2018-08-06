using System;
using System.Collections.Generic;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	class LineaDePrograma : Linea
	{
		private readonly IList<Comando> comandos_Renamed;
		private StringBuilder ultimoComandoEjecutado_Renamed;

		public LineaDePrograma(IList<Comando> comandos)
		{
			ultimoComandoEjecutado_Renamed = new StringBuilder();
			this.comandos_Renamed = comandos;
		}

		public virtual string ultimoComandoEjecutado()
		{
			return ultimoComandoEjecutado_Renamed.ToString();
		}

		internal override void ejecutar()
		{
            int index = 0;
			foreach (Comando comando in comandos_Renamed)
			{
				try
				{
					comando.Ejecutar();
				}
				catch (Exception e)
				{
					ultimoComandoEjecutado_Renamed = new StringBuilder();
					comando.Write(ultimoComandoEjecutado_Renamed, 0);
                    if (e.InnerException == null)
                    {
                        throw e;
                    }
                    else
                    {
                        throw e.InnerException;
                    }

                }
                index++;

            }
		}

		public virtual IList<Comando> comandos()
		{
			return comandos_Renamed;
		}

		internal override void write(StringBuilder resultado)
		{
			foreach (Comando comando in comandos_Renamed)
			{
				comando.Write(resultado, 0);
			}
		}

	}

}