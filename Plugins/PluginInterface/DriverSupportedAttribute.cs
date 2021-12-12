using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class DriverSupportedAttribute : Attribute
    {
        public string DAPName { get; }
        public DriverSupportedAttribute(string dAPName)
        {
            DAPName = dAPName;
        }

    }
}
