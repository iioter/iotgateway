using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ConfigParameterAttribute : Attribute
    {
        public string Description { get;  }
        public ConfigParameterAttribute(string description)
        {
            Description = description;
        }
    }
}
