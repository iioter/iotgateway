using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public class DriverReturnValueModel
    {
        public object Value { get; set; }
        public string Message { get; set; }
        public VaribaleStatusTypeEnum StatusType { get; set; }
    }
}
