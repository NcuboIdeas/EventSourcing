namespace Puppeteer.EventSourcing.Libraries
{

	public sealed class Nulo : Objeto
	{
		public static readonly Nulo NULO = new Nulo();

		private Nulo()
		{

		}

		public override string print()
		{
			return "null";
		}

        public override Boolean esIgualQue(Objeto objeto)
		{
			bool esIgual;
			if (objeto.GetType() != this.GetType())
			{
				esIgual = false;
			}
			else
			{
				esIgual = true;
			}
			return esIgual ? Boolean.True : Boolean.False;
		}

        public override Boolean noEsIgualQue(Objeto objeto)
		{
            return ! esIgualQue(objeto).valor ? Boolean.True : Boolean.False;
		}
	}

}