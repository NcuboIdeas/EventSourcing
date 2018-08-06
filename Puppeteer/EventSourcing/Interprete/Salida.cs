using System.Collections.Generic;
using System.Text;

using Puppeteer.EventSourcing.Libraries;

namespace Puppeteer.EventSourcing.Interprete
{

	public class Salida
	{
		private bool escribirSalida = true;
        private bool enUnFor = false;
        private bool necesitaComa = false;
        private StringBuilder salida = new StringBuilder().Append('{');
        private Stack<StringBuilder> nivelesSalida;
        private Stack<bool> nivelesEnUnFor;
        private Stack<bool> nivelesNecesitaComa;
        
        public virtual void Inicio()
		{
			salida = new StringBuilder().Append('{');
            enUnFor = false;
            necesitaComa = false;
        }

        public virtual void Fin()
        {
            if (escribirSalida)
            {
                salida.Append('}');
            }
        }

        public virtual void ConSalida()
		{
			escribirSalida = true;
			Inicio();
		}

        private void PushDelEstado()
        {
            if (nivelesSalida == null)
            {
                nivelesSalida = new Stack<StringBuilder>();
                nivelesEnUnFor = new Stack<bool>();
                nivelesNecesitaComa = new Stack<bool>();
            }
            nivelesSalida.Push(salida);
            nivelesEnUnFor.Push(enUnFor);
            nivelesNecesitaComa.Push(necesitaComa);
        }

        private void PopDelEstado()
        {
            salida = nivelesSalida.Pop();
            enUnFor = nivelesEnUnFor.Pop();
            necesitaComa = nivelesNecesitaComa.Pop();
        }

        public virtual void AbrirFor()
        {
            if (escribirSalida)
            {
                PushDelEstado();
                Inicio();
            }
        }

        public virtual void CerrarFor(string alias)
        {
            if (escribirSalida)
            {
                if (!Vacio())
                {
                    string contenido = salida.Insert(0, '[').Append(']').ToString();
                    PopDelEstado();
                    EscribirPar(alias, contenido);
                }
                else
                {
                    PopDelEstado();
                }
            }
        }

        public virtual void InicioMoveNextDelFor()
        {
            enUnFor = true;
            if (escribirSalida)
            {
                if (!Vacio())
                {
                    salida.Append(',').Append('{');
                }
                necesitaComa = false;
            }
        }

        public virtual void FinMoveNextDelFor()
        {
            if (escribirSalida)
            {
                if (!Vacio())
                {
                    char[] ultimos = new char[2];
                    salida.CopyTo(salida.Length - 2, ultimos, 0, 2);
                    bool elCuerpoDelFORNoHizoSalidaEnEstaIteracion = ultimos[0] == ',' && ultimos[1] == '{';
                    if (elCuerpoDelFORNoHizoSalidaEnEstaIteracion)
                    {
                        salida.Remove(salida.Length - 2, 2);
                    }
                    else
                    {
                        salida.Append('}');
                    }
                }
            }
            enUnFor = false;
        }

        public virtual void SinSalida()
		{
			escribirSalida = false;
			Inicio();
		}

		public virtual bool EstaEscribiendo()
		{
			return escribirSalida;
		}

		public virtual bool Vacio()
		{
            bool result = salida.Length == 1;
            return result;
		}

        public virtual void Append(string alias, Objeto valor)
        {
            string hilera = valor.print();
            EscribirPar(alias, hilera);
        }

        public virtual void AppendStream(string stream)
        {
            if (escribirSalida)
            {
                if (necesitaComa)
                {
                    salida.Append(',');
                }
                salida.Append(stream);
                necesitaComa = true;
            }
        }

        private void EscribirPar(string alias, string texto)
		{
			if (escribirSalida)
			{
                if (necesitaComa)
                {
                    salida.Append(',');
                }
                salida.Append('"');
                salida.Append(alias);
                salida.Append('"');
                salida.Append(':');
                salida.Append(texto);
                necesitaComa = true;
            }
		}

		public virtual void Append(string alias, char texto)
		{
            EscribirPar(alias, "" + texto);
		}

		public override string ToString()
		{
			return escribirSalida ? salida.ToString() : null;
		}
	}
}