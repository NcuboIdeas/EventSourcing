using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puppeteer.UnitTest
{
    static class Titere
    {
        static internal string Perform(string script)
        {
            TitereActor actor = new TitereActor();
            if (String.IsNullOrEmpty(script)) throw new ArgumentNullException(nameof(script));
            return actor.Perform(script);
        }
    }

    public class TitereActor : Puppeteer.EventSourcing.Actor
    {

        public TitereActor() : base(nameof(TitereActor))
        {

        }
    }
}
