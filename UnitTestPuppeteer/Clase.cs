using Puppeteer.EventSourcing;
using Puppeteer.EventSourcing.Libraries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paquete.SubPaquete
{
    [Puppet]
    public class Clase : Objeto
    {
        public Clase()
        {
            publicField = new Clase("publicField");
            internalField = new Clase("internalField");
        }

        public Clase(List<string> conStrings)
        {
            id = "Para Constructor con List<string>";
        }

        public Clase(List<int> conInts)
        {
            id = "Para Constructor con List<int>";
        }

        public Clase(List<double> conDoubles)
        {
            id = "Para Constructor con List<double>";
        }

        public Clase(List<bool> conBools)
        {
            id = "Para Constructor con List<bool>";
        }

        public Clase(List<Clase> conClases)
        {
            id = "Para Constructor con List<Clase>";
        }

        private string id;
        private Clase(string id)
        {
            this.id = id;
        }

        public override string print()
        {
            if (id == null) throw new Exception("Use el contructor privado de clase para asignarle un texto al objeto " + nameof(Clase));
            return "{\"Clase\":\"" + id + "\"}";
        }

        public int GooRecibirListaDeInts(List<int> ints)
        {
            return ints.Count;
        }

        public int GooRecibirListaDeBools(List<bool> bools)
        {
            return bools.Count;
        }


        public int GooRecibirListaDeDoubles(List<double> doubles)
        {
            return doubles.Count;
        }

        public int GooRecibirListaDeDecimals(List<decimal> decimals)
        {
            return decimals.Count;
        }


        public int GooRecibirListaDeStrings(List<string> strings)
        {
            return strings.Count;
        }


        public int GooRecibirListaDeObjetos(List<Objeto> objetos)
        {
            return objetos.Count;
        }

        public int GooRecibirListaDeClase(List<Clase> objetos)
        {
            return objetos.Count;
        }



        public int GooRecibirListaDeInts(int i, List<int> ints, string s)
        {
            return ints.Count;
        }


        public int GooRecibirListaDeBools(int i, List<bool> bools, string s)
        {
            return bools.Count;
        }


        public int GooRecibirListaDeDoubles(int i, List<double> doubles, string s)
        {
            return doubles.Count;
        }


        public int GooRecibirListaDeStrings(int i, List<string> strings, string s)
        {
            return strings.Count;
        }


        public int GooRecibirListaDeObjetos(int i, List<Objeto> objetos, string s)
        {
            return objetos.Count;
        }


        public List<int> ListaDeInts()
        {
            List<int> lista = new List<int>();
            lista.Add(1);
            lista.Add(2);
            lista.Add(3);
            return lista;
        }



        public List<bool> ListaDeBools()
        {
            List<bool> lista = new List<bool>();
            lista.Add(true);
            lista.Add(false);
            lista.Add(true);
            return lista;
        }


        public List<double> ListaDeDoubles()
        {
            List<double> lista = new List<double>();
            lista.Add(1.2);
            lista.Add(2.34);
            lista.Add(3.456);
            return lista;
        }
        public List<decimal> ListaDeDecimals()
        {
            List<decimal> lista = new List<decimal>();
            lista.Add(1.2M);
            lista.Add(2.34M);
            lista.Add(3.456M);
            return lista;
        }


        public List<string> ListaDeStrings()
        {
            List<string> lista = new List<string>();
            lista.Add("AA");
            lista.Add("BB");
            lista.Add("Cc");
            return lista;
        }


        public List<Objeto> ListaDeObjetos()
        {
            List<Objeto> lista = new List<Objeto>();
            lista.Add(new Clase("AA"));
            lista.Add(new Clase("BB"));
            lista.Add(new Clase("CC"));
            return lista;
        }


        public List<Clase> ListaDeClase()
        {
            List<Clase> lista = new List<Clase>();
            lista.Add(new Clase("AA"));
            lista.Add(new Clase("BB"));
            lista.Add(new Clase("CC"));
            return lista;
        }


        public int GetInt()
        {
            return 7;
        }


        public string GetString()
        {
            return "ABC";
        }


        public double GetDouble()
        {
            return 1.23;
        }


        public bool GetBool()
        {
            return true;
        }


        public Clase GetObjeto()
        {
            return new Clase("Para GetObjeto()");
        }


        public int GetIntConParametro(string unParametro)
        {
            return 7;
        }


        public string XX = "ABCXYZ";


        public int PropertyInt
        {
            get { return 7; }
        }


        public Clase GetObjetoParametro(String str)
        {
            return new Clase(str);
        }


        public Clase GetPublicMetodo(String str)
        {
            return new Clase(str);
        }


        internal Clase GetInternalMetodo(String str)
        {
            return new Clase(str);
        }


        public Clase GetPublicProperty
        {
            get { 
                return new Clase("GetPublicProperty");
            }
        }


        public Clase GetInternalProperty
        {
            get
            {
                return new Clase("GetInternalProperty");
            }
        }

        public Clase publicField;

        internal Clase internalField;

        public decimal Total()
        {
            return 123.456M;
        }

        internal Clase GetMetodoConDecimal(String str, decimal valor)
        {
            return new Clase(str + valor);
        }

        internal Clase GetMetodoConDouble(double valor)
        {
            return new Clase("unValorDouble" + valor.ToString(CultureInfo.GetCultureInfo("en-US")));
        }

        internal Clase GetMetodoConBoolean(String str, bool t, decimal valor)
        {
            return new Clase(str + t + valor);
        }

        public string LHV = "ZZ LHV ZZ";

        public string GetSetLHV { get; set; }

        internal string GetSetInternalLHV { get; set; }

        public decimal Price { get; set; }

        public string PasePorFoo { get; set; } = "No he pasado";
        internal void Foo()
        {
            PasePorFoo = "SI PASE";
        }

        internal Clase Player
        {
            get
            {
                return new Clase("PLAYER");
            }
        }

        internal Clase FooPlayer(string s, Clase c)
        {
            return c;
        }

        internal DateTime Fecha()
        {
            DateTime f = new DateTime(2018, 5, 3, 13, 43, 59);
            return f;
        }

        internal decimal DecimalAmount { get; set; }

        internal decimal GetDecimalAmount(int valor)
        {
            return valor * 1.0M;
        }

        public int LValue = 0;
        public OtraClase OtraClase = new OtraClase();

        public DateTime fechaHora = DateTime.Now;
        public DateTime Ahora()
        {
            return fechaHora;
        }
    }
}
