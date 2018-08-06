namespace Puppeteer.EventSourcing.Libraries
{
    using System.Runtime.CompilerServices;
    using LanguageException = Puppeteer.EventSourcing.Interprete.Libraries.LanguageException;

	public class Boolean : Objeto
	{
		internal bool valor;
        private static readonly Boolean TRUE = new Boolean(true);
        private static readonly Boolean FALSE = new Boolean(false);

        public virtual bool Valor
		{
			get
			{
				return valor;
			}
			set
			{
				this.valor = value;
			}
		}

        
        static public Boolean True
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return TRUE;
            }
        }

        static public Boolean False
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return FALSE;
            }
        }

        private Boolean(bool valor)
		{
			this.valor = valor;
		}

		public override string print()
		{
			return valor ? "true" : "false";
		}

		public override string ToString()
		{
			return "" + valor;
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
            Boolean argumento = null;
			try
			{
				argumento = (Boolean) objeto;
			}
			catch (System.InvalidCastException)
			{
				throw new LanguageException(string.Format("En la comparación se esperaba el valor de tipo [{0}] pero se encontro un valor de tipo [{1}]", typeof(Boolean).Name, objeto.GetType().Name));
			}
			return valor == argumento.valor ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
			return  ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}

	}


}