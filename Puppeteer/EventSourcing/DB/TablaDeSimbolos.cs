using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Puppeteer.EventSourcing.DB
{


	using DeclaracionProcedure = Puppeteer.EventSourcing.Interprete.Libraries.DeclaracionProcedure;
	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;
	using Celda = Puppeteer.EventSourcing.Libraries.Celda;
	using Hilera = Puppeteer.EventSourcing.Libraries.Hilera;
	using Nulo = Puppeteer.EventSourcing.Libraries.Nulo;
	using Objeto = Puppeteer.EventSourcing.Libraries.Objeto;
	using Tabla = Puppeteer.EventSourcing.Libraries.Tabla;

    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(TablaDeSimbolosDebugView))]
    public class TablaDeSimbolos
	{

		private IDictionary<string, SimboloVariable> tablaDeVariables = new Dictionary<string, SimboloVariable>();
		private IDictionary<string, SimboloDeclaracion> tablaDeDeclaraciones = new Dictionary<string, SimboloDeclaracion>();

		private int nivel = 0;
		private const char SEPARADOR_NIVEL = ':';
        //private TablaDeSimbolosDebugView debugView;

        public TablaDeSimbolos()
        {
            //debugView = new TablaDeSimbolosDebugView((IDictionary) tablaDeVariables);
        }

        public virtual void AbrirBloque()
		{
			nivel++;
		}

        internal IDictionary TablaDeVariables
        {
            get
            {
                return (IDictionary) tablaDeVariables;
            }
        }

        internal int Count
        {
            get
            {
                return tablaDeVariables.Keys.Count;
            }
        }

		public virtual void CerrarBloque()
		{
			if (nivel == 0)
			{
				throw new Exception("Se esta intentando cerrar un bloque que no ha sido abierto");
			}
			List<string> aRemover = new List<string>();
            string posfijoDeLosKeys = "" + SEPARADOR_NIVEL + nivel;
            foreach (string variable in tablaDeVariables.Keys)
			{
                if (variable.EndsWith(posfijoDeLosKeys))
                {
                    aRemover.Add(variable);
                }
            }
			foreach (string variable in aRemover)
			{
				tablaDeVariables.Remove(variable);
			}
			nivel--;
		}


		public virtual void GuardarVariable(string instancia, Objeto dato)
		{
            string keyInstancia = null;
            bool estabaAlmacenado = false;
            for (int n = nivel; n >= 0; n--)
            {
                keyInstancia = instancia.ToLower() + SEPARADOR_NIVEL + n;
                estabaAlmacenado = tablaDeVariables.ContainsKey(keyInstancia);
                if (estabaAlmacenado) break;
            }
            if (estabaAlmacenado && dato.GetType() != Nulo.NULO.GetType())
			{
                    SimboloVariable simboloAlmacenado = tablaDeVariables[keyInstancia];
                    Type tipoAnterior = simboloAlmacenado.objeto.GetType();
                    Type tipoNuevo = dato.GetType();
                    bool sonDelMismoTipo = tipoAnterior == Nulo.NULO.GetType() || tipoNuevo == tipoAnterior ||
                                           tipoNuevo.IsSubclassOf(tipoAnterior.BaseType);
					if (!sonDelMismoTipo)
					{
						throw new LanguageException("A la instacia " + instancia + " solo se le pueden asignar " + simboloAlmacenado.objeto.GetType().Name + " y se le está tratando de asignar: " + dato.GetType().Name);
					}
					simboloAlmacenado.objeto = dato;
			}
			else
			{
				string nombreDeLainstancia = instancia.ToLower();
				SimboloVariable s = new SimboloVariable(this, nivel, instancia, dato);
				tablaDeVariables[nombreDeLainstancia + SEPARADOR_NIVEL + nivel] = s;
			}
		}

		public virtual void GuardarDeclaracion(string declaracion, DeclaracionProcedure procedure)
		{
			SimboloDeclaracion s = new SimboloDeclaracion(this, nivel, declaracion, procedure);
			tablaDeDeclaraciones[declaracion.ToLower()] = s;
		}

		public virtual bool ExisteLaVariable(string nombreInstancia)
		{
			string instancia = nombreInstancia.ToLower();
			for (int i = 0; i <= nivel; i++)
			{
				bool existe = tablaDeVariables.ContainsKey(instancia + SEPARADOR_NIVEL + i);
				if (existe)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool ExisteLaDeclaracion(string nombreDeclaracion)
		{
			string instancia = nombreDeclaracion.ToLower();
			bool existe = tablaDeDeclaraciones.ContainsKey(instancia);
			return existe;
		}

		private SimboloVariable BuscarVariable(string nombreInstancia)
		{
			string instancia = nombreInstancia.ToLower();
			for (int i = nivel; i >= 0; i--)
			{
				string keyInstancia = instancia + SEPARADOR_NIVEL + i;
				if (tablaDeVariables.ContainsKey(keyInstancia))
				{
                    SimboloVariable s = tablaDeVariables[keyInstancia];
                    return s;
				}
			}
			return null;
		}

		public virtual Objeto Valor(string nombreInstancia)
		{
            SimboloVariable variable = BuscarVariable(nombreInstancia);
			return variable == null ? null : variable.objeto;
		}


		private SimboloDeclaracion Declaracion(string nombreDeclaracion)
		{
			string instancia = nombreDeclaracion.ToLower();
			SimboloDeclaracion s = tablaDeDeclaraciones[instancia];
			return s;
		}

		public virtual DeclaracionProcedure Procedure(string nombreDeclaracion)
		{
			SimboloDeclaracion d = Declaracion(nombreDeclaracion);
			return d.procedure;
		}

		private abstract class Simbolo
		{
			private readonly TablaDeSimbolos outerInstance;

			public Simbolo(TablaDeSimbolos outerInstance)
			{
				this.outerInstance = outerInstance;
			}

			public override abstract bool Equals(object o);

			public override abstract int GetHashCode();
		}

		private class SimboloVariable : Simbolo
		{
			private readonly TablaDeSimbolos outerInstance;

			internal int nivel;
			internal string nombreDeVariable;
			internal Objeto objeto;

			internal SimboloVariable(TablaDeSimbolos outerInstance, int nivel, string nombreDeVariable, Objeto objeto) : base(outerInstance)
			{
				this.outerInstance = outerInstance;
				this.nivel = nivel;
				this.nombreDeVariable = nombreDeVariable.ToLower();
				this.objeto = objeto;
			}

			public override bool Equals(object o)
			{
				bool esInstanciaDeLlave = o is Simbolo;
				if (!esInstanciaDeLlave)
				{
					return false;
				}
				bool esElmismoBloque = ((SimboloVariable) o).nivel == this.nivel;
				if (!esElmismoBloque)
				{
					return false;
				}
				bool esElmismoNombre = ((SimboloVariable) o).nombreDeVariable.Equals(this.nombreDeVariable);
				return esElmismoNombre;
			}

			public override int GetHashCode()
			{
				int hash = 7;
				hash = 97 * hash + this.nivel + this.nombreDeVariable.GetHashCode();
				return hash;
			}
		}

		private class SimboloDeclaracion : Simbolo
		{
			private readonly TablaDeSimbolos outerInstance;

			internal int nivel;
			internal string nombreDeDeclaracion;
			internal DeclaracionProcedure procedure;

			internal SimboloDeclaracion(TablaDeSimbolos outerInstance, int nivel, string nombreDeVariable, DeclaracionProcedure procedure) : base(outerInstance)
			{
				this.outerInstance = outerInstance;
				this.nivel = nivel;
				this.nombreDeDeclaracion = nombreDeVariable.ToLower();
				this.procedure = procedure;
			}

			public override bool Equals(object o)
			{
				bool esInstanciaDeLlave = o is Simbolo;
				if (!esInstanciaDeLlave)
				{
					return false;
				}
				bool esElmismoBloque = ((SimboloDeclaracion) o).nivel == this.nivel;
				if (!esElmismoBloque)
				{
					return false;
				}
				bool esElmismoNombre = ((SimboloDeclaracion) o).nombreDeDeclaracion.Equals(this.nombreDeDeclaracion);
				return esElmismoNombre;
			}


			public override int GetHashCode()
			{
				int hash = 7;
				hash = 97 * hash + this.nivel + this.nombreDeDeclaracion.GetHashCode();
				return hash;
			}
		}

        internal class TablaDeSimbolosDebugView
        {
            private IDictionary hashtable;    
            internal TablaDeSimbolosDebugView(TablaDeSimbolos tabla)
            {
                this.hashtable = tabla.TablaDeVariables;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            internal KeyValuePairs[] Keys
            {
                get
                {
                    KeyValuePairs[] keys = new KeyValuePairs[hashtable.Count];

                    int i = 0;
                    foreach (string key in hashtable.Keys)
                    {
                        keys[i] = new KeyValuePairs(hashtable, key, ((SimboloVariable)hashtable[key]).objeto);
                        i++;
                    }
                    return keys;
                }
            }
        }

        [DebuggerDisplay("{value}", Name = "{key}")]
        internal class KeyValuePairs
        {
            private IDictionary dictionary;
            private object key;
            private object value;

            internal KeyValuePairs(IDictionary dictionary, object key, object value)
            {
                this.value = value;
                this.key = key;
                this.dictionary = dictionary;
            }
        }

    }

}