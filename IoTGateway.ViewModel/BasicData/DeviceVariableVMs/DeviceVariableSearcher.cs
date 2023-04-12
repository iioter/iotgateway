using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using PluginInterface;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public partial class DeviceVariableSearcher : BaseSearcher
    {
        [Display(Name = "变量名")]
        public String Name { get; set; }
        [Display(Name = "方法")]
        public String Method { get; set; }
        [Display(Name = "地址")]
        public String DeviceAddress { get; set; }
        [Display(Name = "数据类型")]
        public DataTypeEnum? DataType { get; set; }
        public List<ComboSelectListItem> AllDevices { get; set; }
        [Display(Name = "设备名")]
        public Guid? DeviceId { get; set; }
        [Display(Name = "设备别名")]
        public String Alias { get; set; }

        protected override void InitVM()
        {
            AllDevices = DC.Set<Device>().AsNoTracking().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                .OrderBy(x => x.Parent.Index).ThenBy(x => x.Parent.DeviceName)
                .ThenBy(x => x.Index).ThenBy(x => x.Parent.DeviceName)
                .GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }
}
