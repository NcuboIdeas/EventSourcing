using Puppeteer.EventSourcing.DB;
using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Libraries
{
    using System;
    using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

    public abstract class Objeto
    {
        private List<Atributo> arrayAtributo;
        private readonly string uuid;

        protected Objeto()
        {
            var anUUID = System.Guid.NewGuid();
            uuid = "ID" + anUUID.ToString().Replace('-', 'x');
        }

        public string UUID
        {
            get
            {
                return uuid;
            }
        }

        public Tabla generarTablaConLosAtributos()
		{
			int cantidadDeAtributos = arrayAtributo == null ? 0 : this.arrayAtributo.Count;
			Tabla tabla = new Tabla();
			tabla.defineNombreDeLaTabla(this.GetType().Name);

			Celda[] celdas = new Celda[cantidadDeAtributos];
			for (int i = 0; i < cantidadDeAtributos; i++)
			{
				Atributo atributo = arrayAtributo[i];
				celdas [i] = new Celda(atributo.nombre(),atributo.valor());
			}
			tabla.agregarFila(celdas);
			return tabla;
		}

		public Objeto getDatoDelAtributo(string nombreDeAtributo)
		{
			string nombreUnsensitive = nombreDeAtributo.ToUpper();
			if (arrayAtributo != null)
			{
				foreach (Atributo atributo in arrayAtributo)
				{
				bool esElMismoNombre = atributo.nombreEnMayusculas().Equals(nombreUnsensitive);
				if (esElMismoNombre)
				{
					return atributo.valor();
				}
				}
			}
			throw new LanguageException(string.Format("No se ha encontrado el atributo: {0} verifique el nombre", nombreDeAtributo));
		}

		private Atributo getAtributo(string nombreDeAtributo)
		{
			string nombreUnsensitive = nombreDeAtributo.ToUpper();
			if (arrayAtributo != null)
			{
				foreach (Atributo atributo in arrayAtributo)
				{
				bool esElMismoNombre = atributo.nombreEnMayusculas().Equals(nombreUnsensitive);
				if (esElMismoNombre)
				{
					return atributo;
				}
				}
			}
			throw new LanguageException(string.Format("No se ha encontrado el atributo: {0} verifique el nombre", nombreDeAtributo));
		}

		public bool tieneElAtributo(string nombreDeAtributo)
		{
			 if (arrayAtributo == null)
			 {
				 return false;
			 }
			 string nombreUnsensitive = nombreDeAtributo.ToUpper();
			 foreach (Atributo atributo in arrayAtributo)
			 {
				 bool esElMismoNombre = atributo.nombreEnMayusculas().Equals(nombreUnsensitive);
				 if (esElMismoNombre)
				 {
					 return true;
				 }
			 }
			 return false;
		}

		public virtual void setAtributo(string nombre, Objeto valor)
		{
			if (tieneElAtributo(nombre))
			{
				Atributo atributo = getAtributo(nombre);
				if (atributo is AtributoDinamico)
				{
					setAtributoDinamico(nombre, valor);
				}
				else
				{
					setAtributoFijo(atributo.nombre(), valor);
				}
			}
			else
			{
				setAtributoDinamico(nombre, valor);
			}
		}

		private void setAtributoDinamico(string nombre, Objeto instancia)
		{
			AtributoDinamico atributo = new AtributoDinamico(nombre, instancia);
			if (arrayAtributo == null)
			{
				arrayAtributo = new List<Atributo>();
			}
			if (arrayAtributo.Contains(atributo))
			{
				int index = arrayAtributo.IndexOf(atributo);
				Atributo dato = getAtributo(nombre);
				if (instancia.GetType() == Nulo.NULO.GetType())
				{
					arrayAtributo.RemoveAt(index);
				}
				else if (dato.valor().GetType() != instancia.GetType())
				{
					throw new LanguageException("Al atributo '" + nombre + "' solo se le pueden asignar valores de tipo '" + dato.valor().GetType().Name + "'.");
				}
				else
				{
					arrayAtributo[index] = atributo;
				}
			}
			else
			{
				if (instancia.GetType() != Nulo.NULO.GetType())
				{
					arrayAtributo.Add(atributo);
				}
			}
		}

		public virtual void setAtributoFijo(string nombre, Objeto valor)
		{
			AtributoFijo atributo = new AtributoFijo(nombre, valor);
			if (arrayAtributo == null)
			{
				arrayAtributo = new List<Atributo>();
			}
			if (arrayAtributo.Contains(atributo))
			{
				Objeto dato = getDatoDelAtributo(nombre);
				if (valor.GetType() == Nulo.NULO.GetType())
				{
					throw new LanguageException("Al atributo Fijo'" + nombre + "' No se le puede asignar un valor Nulo, debe asignarle un valor de tipo '" + dato.GetType().Name + "'.");
				}
				else if (dato.GetType() != valor.GetType())
				{
					throw new LanguageException("Al atributo '" + nombre + "' solo le puede asignar un valor de tipo '" + dato.GetType().Name + "'.");
				}
				else
				{
					int index = arrayAtributo.IndexOf(atributo);
					arrayAtributo[index] = atributo;
				}
			}
			else
			{
				if (valor.GetType() != Nulo.NULO.GetType())
				{
					arrayAtributo.Add(atributo);
				}
				else
				{
					throw new LanguageException("Al atributo Fijo'" + nombre + "' No se le puede asignar un valor Nulo, debe asignarle un valor de tipo '" + valor.GetType().Name + "'.");
				}
			}
		}

		public virtual string print()
		{
			throw new LanguageException("El método 'public overide string Print()' no ha sido implementado para " + this.GetType().FullName);
		}

		public override string ToString()
		{
			throw new LanguageException(string.Format("El método toSring no ha sido implementado para {0}", this.GetType().FullName));
		}

		public virtual Objeto sumar(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método sumar no ha sido implementado para ", this.GetType().FullName));
		}

		public virtual Objeto restar(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método restar no ha sido implementado para ", this.GetType().FullName));
		}

		public virtual Objeto multiplicar(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método multiplicar no ha sido implementado para ", this.GetType().FullName));
		}

		public virtual Objeto dividir(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método dividir no ha sido implementado para ", this.GetType().FullName));
		}

        protected internal virtual Boolean igual(Objeto objeto)
		{
			return this.Equals(objeto) ? Boolean.True : Boolean.False;
		}
        public override bool Equals(object obj)
        {
            return obj.GetType().IsAssignableFrom(this.GetType()) && this.uuid.Equals( ((Objeto)obj).uuid);
        }

        /*public static bool operator == (Objeto obj1, Objeto obj2)
        {
            return obj1.uuid.Equals(obj2.uuid);
        }

        public static bool operator != (Objeto obj1, Objeto obj2)
        {
            return ! obj1.uuid.Equals(obj2.uuid);
        }*/

        public virtual Boolean esIgualQue(Objeto objeto)
		{
			return this.igual(objeto);
		}

        public virtual Boolean noEsIgualQue(Objeto objeto)
		{
			return ! this.igual(objeto).valor ? Boolean.True : Boolean.False;
		}

        public virtual Boolean esMayorQue(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método esMayorQue no ha sido implementado para ", this.GetType().FullName));
		}

        public virtual Boolean esMenorQue(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método esMenorQue no ha sido implementado para ", this.GetType().FullName));
		}

        public virtual Boolean esMayorOIgualQue(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método esMayorOIgualQue no ha sido implementado para ", this.GetType().FullName));
		}

        public virtual Boolean esMenorOIgualQue(Objeto objeto)
		{
			throw new LanguageException(string.Format("El método esMenorOIgualQue no ha sido implementado para ", this.GetType().FullName));
		}

		protected internal virtual int cantidadAtributos()
		{
			return arrayAtributo == null ? 0 : arrayAtributo.Count;
		}

        public override int GetHashCode()
        {
            return 1907758594 + uuid.GetHashCode();
        }
    }
}