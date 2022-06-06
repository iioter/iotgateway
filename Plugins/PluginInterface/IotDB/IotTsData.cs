using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.IotDB
{
    /// <summary>
    /// IotDB时序数据库，测点数据json定义
    /// </summary>
    public class IotTsData
    {
        public string device { get; set; }
        public List<string> measurements { get; set; }
        public List<object> values { get; set; }
        public long timestamp { get; set; }
    }
}
