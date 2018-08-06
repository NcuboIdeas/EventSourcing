using Puppeteer.EventSourcing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puppeteer.UnitTest
{
    [Puppet]
    internal class ClaseHeredaDeBase : ClaseBase
    {
        ClaseHeredaDeBase() : base(nameof(ClaseHeredaDeBase))
        {

        }

    }
}
