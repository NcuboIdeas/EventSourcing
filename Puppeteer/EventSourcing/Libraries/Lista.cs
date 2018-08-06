using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Libraries
{

	using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Lista : Objeto
	{
		private List<Objeto> lista;

		public Lista()
		{
			lista = new List<Objeto>();
		}

        public virtual int Count()
        {
            return lista.Count;
        }

        public virtual List<Objeto> getLista()
		{
			return lista;
		}

		public virtual Objeto getObjeto(int puntero)
		{
			return lista[puntero];
		}

		public virtual bool contains(Objeto objeto)
		{
			if (lista.Contains(objeto))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public virtual void remover(Objeto objeto)
		{
			lista.Remove(objeto);
		}

		public virtual void guardarObjeto(Objeto objeto)
		{
			lista.Add(objeto);
		}

		public virtual void guardarLista(List<Objeto> lista)
		{
			lista.AddRange(lista);
		}

		public virtual void guardarTodo(Lista objeto)
		{
			foreach (Objeto lista in objeto.getLista())
			{
				this.lista.Add(lista);
			}
		}
		public virtual void clear()
		{
			lista.Clear();
		}

		public virtual bool Empty
		{
			get
			{
				if (lista.Count == 0)
				{
					return true;
				}
				else if (lista.Count == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		public virtual Hilera demeElTipoObjetoAlQuePertenece()
		{
			Hilera valor = new Hilera(lista[0].GetType().Name);
			return valor;
		}

		public override Objeto sumar(Objeto objeto)
		{
			Lista miLista = new Lista();
			miLista.guardarLista(lista);

			bool elObjetoDelParametroEsInstanciaDeLista = objeto is Lista;
			if (!elObjetoDelParametroEsInstanciaDeLista)
			{
				throw new LanguageException(string.Format("No se puede sumar un {0} a un {1}, ya que ambos de tipos diferentes.", miLista.demeElTipoObjetoAlQuePertenece(), objeto.GetType().Name));
			}

			Lista nuevaLista = new Lista();
			nuevaLista.guardarTodo((Lista) objeto);

			bool noEsElMismoTipoDeObjeto = !miLista.demeElTipoObjetoAlQuePertenece().esIgual(nuevaLista.demeElTipoObjetoAlQuePertenece());
			if (noEsElMismoTipoDeObjeto)
			{
				throw new LanguageException(string.Format("No se puede sumar un {0} a un {1}, ya que ambos de tipos diferentes.", miLista.demeElTipoObjetoAlQuePertenece(), nuevaLista.demeElTipoObjetoAlQuePertenece()));
			}
			miLista.guardarTodo(nuevaLista);
			return miLista;
		}

		public override Objeto restar(Objeto objeto)
		{
			Lista miLista = new Lista();
			miLista.guardarLista(lista);

			bool elObjetoDelParametroEsInstanciaDeLista = objeto is Lista;
			if (elObjetoDelParametroEsInstanciaDeLista)
			{
				throw new LanguageException(string.Format("No se puede restar un {0} a un {1}, ya que ambos de tipos diferentes.", miLista.demeElTipoObjetoAlQuePertenece(), objeto.GetType().Name));
			}
			Lista nuevaLista = new Lista();
			nuevaLista.guardarTodo((Lista) objeto);

			bool noEsElMismoTipoDeObjeto = !miLista.demeElTipoObjetoAlQuePertenece().esIgual(nuevaLista.demeElTipoObjetoAlQuePertenece());
			if (noEsElMismoTipoDeObjeto)
			{
				throw new LanguageException(string.Format("No se puede restar un %s a un '%2', ya que ambos de tipos diferentes.", miLista.demeElTipoObjetoAlQuePertenece(), nuevaLista.demeElTipoObjetoAlQuePertenece()));
			}

			Lista copiaDeMiLista = new Lista();
			copiaDeMiLista.guardarTodo(miLista);
			foreach (Objeto listaRestar in nuevaLista.getLista())
			{
				foreach (Objeto listaElemento in copiaDeMiLista.getLista())
				{
                    if (listaElemento.Equals(listaRestar))
					{
						miLista.remover(listaRestar);
					}
				}
			}
			return miLista;
		}

		public override string print()
		{
			string salida = "{\"" + this.GetType().Name + "\":[";
			for (int i = 0; i < lista.Count; i++)
			{
				Objeto obj = lista[i];
				if (obj != null)
				{
					salida += obj.print();
				}
				if (i + 1 < lista.Count)
				{
					salida += ",";
				}
			}
			salida += "]}";
			return salida;
		}
	}

}