using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Libraries
{


	public sealed class Monedas
	{
        public enum SimboloMoneda
        {
            CRC,
            MXN,
            SVC,
            HNL,
            NIO,
            PAB,
            GTQ,
            COP,
            ECS,
            VEF,
            PEN,
            BOB,
            USD,
            CLP,
            ARS,
            PYG,
            BRL,
            DOP,
            PRUSD,
            CUP,
            UYU
        }

        public static readonly Monedas CRC;
        public static readonly Monedas MXN;
        public static readonly Monedas SVC;
        public static readonly Monedas HNL;
        public static readonly Monedas NIO;
        public static readonly Monedas PAB;
        public static readonly Monedas GTQ;
        public static readonly Monedas COP;
        public static readonly Monedas ECS;
        public static readonly Monedas VEF;
        public static readonly Monedas PEN;
        public static readonly Monedas BOB;
        public static readonly Monedas USD;
        public static readonly Monedas CLP;
        public static readonly Monedas ARS;
        public static readonly Monedas PYG;
        public static readonly Monedas BRL;
        public static readonly Monedas DOP;
        public static readonly Monedas PRUSD;
        public static readonly Monedas CUP;
        public static readonly Monedas UYU;
        
        private SimboloMoneda moneda;

		private string localizacion_Renamed;

		private string simbolo_Renamed;

        private static Dictionary<string, Monedas> monedas;

        private Monedas(SimboloMoneda simboloMoneda, string localizacion, string nombre, string simbolo)
		{
			this.localizacion_Renamed = localizacion;
			this.simbolo_Renamed = simbolo;
            this.moneda= simboloMoneda;
		}

		public string simbolo()
		{
			return simbolo_Renamed;
		}

		public string localizacion()
		{
			return localizacion_Renamed;
		}

        public string nombre()
		{
			return this.moneda.ToString();
		}

        public static bool contieneLaMoneda(string monedaAEvaluar)
        {
            string aEvaluarUnSensitive = monedaAEvaluar.ToUpper();
            foreach (string moneda in System.Enum.GetNames(typeof(SimboloMoneda)))
            {
                if (moneda.ToUpper() == aEvaluarUnSensitive)
                {
                    return true;
                }
            }
            return false;
        }

		public static Monedas obtenerTipoDeMonto(Hilera hilera)
		{
			//TODO El PRUSD esta mal.. pues tiene 5 letras y aqui se usan fijos 3.
            string tipo = hilera.Valor.Trim().Substring(0, 3).ToUpper();
            return monedas[tipo];
		}

		public static Monedas valueOf(string name)
		{
            return monedas[name.ToUpper()];
		}

        static Monedas()
        {
            monedas = new Dictionary<string, Monedas>();

            CRC = new Monedas(SimboloMoneda.CRC, "es_CR", "Colon", "CRC");
            MXN = new Monedas(SimboloMoneda.MXN, "es_MX", "Peso", "$");
            SVC = new Monedas(SimboloMoneda.SVC, "es_SV", "Colon", "¢");
            HNL = new Monedas(SimboloMoneda.HNL, "es_HN", "Lempira", "(L)");
            NIO = new Monedas(SimboloMoneda.NIO, "es_NI", "Cordoba", "(C$)");
            PAB = new Monedas(SimboloMoneda.PAB, "es_PA", "Balboa", "(฿)");
            GTQ = new Monedas(SimboloMoneda.GTQ, "es_GT", "Quetzal", "(Q)");
            COP = new Monedas(SimboloMoneda.COP, "es_CO", "Peso", "($)");
            ECS = new Monedas(SimboloMoneda.ECS, "es_EC", "Sucre", "(S/.)");
            VEF = new Monedas(SimboloMoneda.VEF, "es_VE", "Bolivar", "(Bs.)");
            PEN = new Monedas(SimboloMoneda.PEN, "es_PE", "Nuevo sol", "(S/.)");
            BOB = new Monedas(SimboloMoneda.BOB, "es_BO", "Boliviano", "(Bs)");
            USD = new Monedas(SimboloMoneda.USD, "en_US", "Dollar", "$");
            CLP = new Monedas(SimboloMoneda.CLP, "es_CL", "Peso", "($)");
            ARS = new Monedas(SimboloMoneda.ARS, "es_AR", "Peso", "($)");
            PYG = new Monedas(SimboloMoneda.PYG, "es_PY", "Guarani", "(₲)");
            BRL = new Monedas(SimboloMoneda.BRL, "pt_BR", "Real", "(R$)");
            DOP = new Monedas(SimboloMoneda.DOP, "es_DO", "Peso", "(RD$)");
            PRUSD = new Monedas(SimboloMoneda.PRUSD, "es_PR", "Dollar", "($)");
            CUP = new Monedas(SimboloMoneda.CUP, "es_CU", "Peso", "($)");
            UYU = new Monedas(SimboloMoneda.UYU, "es_UY", "Peso", "($)");

            monedas.Add(CRC.nombre(),CRC);
            monedas.Add(MXN.nombre(), MXN);
            monedas.Add(SVC.nombre(), SVC);
            monedas.Add(HNL.nombre(), HNL);
            monedas.Add(NIO.nombre(), NIO);
            monedas.Add(PAB.nombre(), PAB);
            monedas.Add(GTQ.nombre(), GTQ);
            monedas.Add(COP.nombre(), COP);
            monedas.Add(ECS.nombre(), ECS);
            monedas.Add(VEF.nombre(), VEF);
            monedas.Add(PEN.nombre(), PEN);
            monedas.Add(BOB.nombre(), BOB);
            monedas.Add(USD.nombre(), USD);
            monedas.Add(CLP.nombre(), CLP);
            monedas.Add(ARS.nombre(), ARS);
            monedas.Add(PYG.nombre(), PYG);
            monedas.Add(BRL.nombre(), BRL);
            monedas.Add(DOP.nombre(), DOP);
            monedas.Add(PRUSD.nombre(), PRUSD);
            monedas.Add(CUP.nombre(), CUP);
            monedas.Add(UYU.nombre(), UYU);
        }
	}
}