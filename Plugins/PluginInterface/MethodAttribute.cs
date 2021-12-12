using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MethodAttribute: Attribute
    {
        public string Name { get;  }        
        public ProtectTypeEnum Protect_Type { get;  }
        public string Description { get;  }

        public MethodAttribute(string name, ProtectTypeEnum protect_Type = ProtectTypeEnum.ReadOnly, string description = "")
        {
            Name = name;
            Protect_Type = protect_Type;
            Description = description;
        }
    }
}
