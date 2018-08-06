using System.Collections.Generic;

namespace Puppeteer.EventSourcing.Libraries
{

	using BusinessLogicalException = Puppeteer.EventSourcing.Interprete.Libraries.BusinessLogicalException;

	public sealed class Meses
	{
		public static readonly Meses ENE = new Meses("Jan", InnerEnum.ENE, "January",1);
		public static readonly Meses FEB = new Meses("Feb", InnerEnum.FEB, "February",2);
		public static readonly Meses MAR = new Meses("Mar", InnerEnum.MAR, "March",3);
		public static readonly Meses ABR = new Meses("Apr", InnerEnum.ABR, "April",4);
		public static readonly Meses MAY = new Meses("May", InnerEnum.MAY, "May",5);
		public static readonly Meses JUN = new Meses("Jun", InnerEnum.JUN, "June",6);
		public static readonly Meses JUL = new Meses("Jul", InnerEnum.JUL, "July",7);
		public static readonly Meses AGO = new Meses("Ago", InnerEnum.AGO, "Agoust",8);
		public static readonly Meses SET = new Meses("Set", InnerEnum.SET, "September",9);
		public static readonly Meses OCT = new Meses("Oct", InnerEnum.OCT, "October",10);
		public static readonly Meses NOV = new Meses("Nov", InnerEnum.NOV, "November",11);
		public static readonly Meses DIC = new Meses("Dec", InnerEnum.DIC, "December",12);

		private static readonly IList<Meses> valueList = new List<Meses>();

		static Meses()
		{
			valueList.Add(ENE);
			valueList.Add(FEB);
			valueList.Add(MAR);
			valueList.Add(ABR);
			valueList.Add(MAY);
			valueList.Add(JUN);
			valueList.Add(JUL);
			valueList.Add(AGO);
			valueList.Add(SET);
			valueList.Add(OCT);
			valueList.Add(NOV);
			valueList.Add(DIC);
		}

		public enum InnerEnum
		{
			ENE,
			FEB,
			MAR,
			ABR,
			MAY,
			JUN,
			JUL,
			AGO,
			SET,
			OCT,
			NOV,
			DIC
		}

		private readonly string nameValue;
		private readonly int ordinalValue;
		private readonly InnerEnum innerEnumValue;
		private static int nextOrdinal = 0;

		private string nombreCompleto_Renamed;
		private int numero_Renamed;

		private Meses(string name, InnerEnum innerEnum, string nombreCompleto, int numero)
		{
			this.nombreCompleto_Renamed = nombreCompleto;
			this.numero_Renamed = numero;

			nameValue = name;
			ordinalValue = nextOrdinal++;
			innerEnumValue = innerEnum;
		}

        public string name ()
        {
            return nameValue;
        }

		public string nombreCompleto()
		{
			return this.nombreCompleto_Renamed;
		}

		public int numero()
		{
			return this.numero_Renamed;
		}

		public static Meses obtenerMes(int numeroBuscado)
		{
			foreach (Meses mes in Meses.values())
			{
				if (mes.numero_Renamed == numeroBuscado)
				{
					return mes;
				}
			}
			throw new BusinessLogicalException("numero de mes no encontrado " + numeroBuscado);
		}

		public Meses siguienteMes()
		{

			return obtenerMes(this.numero_Renamed + 1);
		}

		private static readonly string[] NOMBRES = new string[]{Meses.ENE.name(), Meses.FEB.name(), Meses.MAR.name(), Meses.ABR.name(), Meses.MAY.name(), Meses.JUN.name(), Meses.JUL.name(), Meses.AGO.name(), Meses.SET.name(), Meses.OCT.name(), Meses.NOV.name(), Meses.DIC.name()};
		public static bool contieneElMes(string mesEnUpperCase)
		{
			return NOMBRES[0].Equals(mesEnUpperCase) || NOMBRES[1].Equals(mesEnUpperCase) || NOMBRES[2].Equals(mesEnUpperCase) || NOMBRES[3].Equals(mesEnUpperCase) || NOMBRES[4].Equals(mesEnUpperCase) || NOMBRES[5].Equals(mesEnUpperCase) || NOMBRES[6].Equals(mesEnUpperCase) || NOMBRES[7].Equals(mesEnUpperCase) || NOMBRES[8].Equals(mesEnUpperCase) || NOMBRES[9].Equals(mesEnUpperCase) || NOMBRES[10].Equals(mesEnUpperCase) || NOMBRES[11].Equals(mesEnUpperCase);


		}

		public override string ToString()
		{
			return "Nombre: " + this.name() + " Nombre Completo: " + this.nombreCompleto_Renamed + " Numero: " + this.numero_Renamed;
		}

		public Meses anteriorMes()
		{
			return obtenerMes(this.numero_Renamed - 1);
		}


		public static IList<Meses> values()
		{
			return valueList;
		}

		public InnerEnum InnerEnumValue()
		{
			return innerEnumValue;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static Meses valueOf(string name)
		{
			foreach (Meses enumInstance in Meses.values())
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}