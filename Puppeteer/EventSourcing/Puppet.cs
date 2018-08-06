using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puppeteer.EventSourcing
{
    [System.AttributeUsage(
        AttributeTargets.Class | 
        AttributeTargets.Constructor | 
        AttributeTargets.Enum | 
        AttributeTargets.Field | 
        AttributeTargets.Method | 
        AttributeTargets.Property
     )]
    public class Puppet : System.Attribute
    {
    }
}
