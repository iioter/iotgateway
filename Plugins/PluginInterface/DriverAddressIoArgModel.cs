using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface
{
    public class DriverAddressIoArgModel
    {
        public Guid ID { get; set; }
        public string Address { get; set; }
        public DataTypeEnum ValueType { get; set; }
    }
}
