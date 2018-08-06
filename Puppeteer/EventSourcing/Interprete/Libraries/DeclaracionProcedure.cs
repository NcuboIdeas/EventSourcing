using System;
using System.Text;

namespace Puppeteer.EventSourcing.Interprete.Libraries
{

	using TablaDeSimbolos = DB.TablaDeSimbolos;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;

	public class DeclaracionProcedure : Declaracion
	{
		private Id id;
		private ComandoBloque cuerpoDelProcedimiento;
		private DeclaracionDeParametro[] parametros;
		private readonly TablaDeSimbolos tablaDeSimbolos;

		public DeclaracionProcedure(TablaDeSimbolos tablaDeSimbolos, Id id, DeclaracionDeParametro[] parametros, ComandoBloque cuerpo)
		{
			this.id = id;
			this.cuerpoDelProcedimiento = cuerpo;
			this.parametros = parametros;
			this.tablaDeSimbolos = tablaDeSimbolos;
		}

		public override void guardar()
		{
			tablaDeSimbolos.GuardarDeclaracion(id.Valor, this);
		}

		public virtual void ejecutar()
		{
			cuerpoDelProcedimiento.Ejecutar();
		}

		public virtual int cantidadDeParametros()
		{
			return parametros.Length;
		}

		internal virtual Type calcularTipoDelParametro(int posicion)
		{
			return parametros[posicion].tipoDelParametro();
		}

		public virtual string nombreDelParametro(int posicion)
		{
			return parametros[posicion].nombreDelParametro();
		}

		public override void write(StringBuilder resultado, int tabs)
		{
			resultado.Append(GenerarTabs(tabs));
			resultado.Append("Procedure ");
			id.write(resultado);
			resultado.Append('(');

			for (int i = 0; i < parametros.Length; i++)
			{
				if (i > 0)
				{
					resultado.Append(", ");
				}
				parametros[i].write(resultado);
			}
			resultado.Append(")\r");

			cuerpoDelProcedimiento.Write(resultado, tabs);
		}
	}
}