using Puppeteer.EventSourcing.DB;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Puppeteer.EventSourcing
{

    using TablaDeSimbolos = Puppeteer.EventSourcing.DB.TablaDeSimbolos;
	using Parser = Puppeteer.EventSourcing.Interprete.Parser;
	using Salida = Puppeteer.EventSourcing.Interprete.Salida;
	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;
	using Programa = Puppeteer.EventSourcing.Interprete.Libraries.Programa;

	public abstract class Actor
	{
		private readonly Parser parser;
		private readonly TablaDeSimbolos tablaDeSimbolos;
		private readonly Salida salida;
        private readonly String persona;
        private Dairy eventStorage = null;


        public Actor(String persona)
		{
            if (String.IsNullOrEmpty(persona)) throw new ArgumentNullException(nameof(persona));
            Assembly library = this.GetType().Assembly;
            tablaDeSimbolos = new TablaDeSimbolos();
            salida = new Salida();
            parser = new Parser(library, tablaDeSimbolos, salida);
            this.persona = persona;
		}

        public void EventSourcingStorage(DatabaseType dbType, string connection)
        {
            if (String.IsNullOrEmpty(connection)) throw new ArgumentNullException(nameof(connection));
            eventStorage = new Dairy(dbType, connection, persona);
            salida.SinSalida();
            eventStorage.RecuperarEstado(this);
            salida.ConSalida();
        }

        public TablaDeSimbolos TablaDeSimbolos
        {
            get
            {
                return tablaDeSimbolos;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string Perform(string script)
        {
            //TODO: Esta fecha hora deberia venir de un server de Hora
            var now = DateTime.Now;
            var ahora = new Libraries.FechaHora(now.Day, now.Month, now.Year, now.Hour, now.Minute, now.Second);
            return Perform(script, ahora);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal string Perform(string script, Libraries.FechaHora ahora)
		{
            if (script == null) throw new ArgumentNullException(nameof(script));

            tablaDeSimbolos.GuardarVariable("Now", ahora);

			parser.EstablecerComando(script);
			Programa programa = parser.Procesar();
            string resultado = programa.Ejecutar();
            if (eventStorage != null)
            {
                string formatedScript = programa.Write();
                eventStorage.EscribirEnDairy(formatedScript);
            }
            return resultado == "{}" ? "" : resultado;
		}

	}

}