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
        public object Value { get; set; }
        public DataTypeEnum ValueType { get; set; }

        public EndianEnum EndianType { get; set; }
        public override string ToString()
        {
            return $"变量ID:{ID},Address:{Address},Value:{Value},ValueType:{ValueType},Endian:{EndianType}";
        }
    }
}
