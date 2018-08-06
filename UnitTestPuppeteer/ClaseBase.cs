using Puppeteer.EventSourcing;
using Puppeteer.EventSourcing.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puppeteer.UnitTest
{
    [Puppet]
    internal class ClaseBase : Objeto
    {
        private string id;
        protected ClaseBase(string id)
        {
            this.id = id;
        }

        public override string print()
        {
            if (id == null) throw new Exception("Use el contructor privado de clase para asignarle un texto al objeto " + nameof(ClaseBase));
            return "{\"Clase\":\"" + id + "\"}";
        }

        internal int FooCastHaciaClaseBase(ClaseBase claseBase)
        {
            return 1;
        }

        internal ClaseBase FooSinCastHaciaHeredaDeBase(ClaseHeredaDeBase claseBase)
        {
            return claseBase;
        }
    }
}
