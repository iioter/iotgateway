using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.ViewModel.BasicData.DeviceConfigVMs
{
    public partial class DeviceConfigSearcher : BaseSearcher
    {
        [Display(Name = "名称")]
        public String DeviceConfigName { get; set; }
        [Display(Name = "属性侧")]
        public DataSide? DataSide { get; set; }
        [Display(Name = "值")]
        public String Value { get; set; }
        public List<ComboSelectListItem> AllDevices { get; set; }
        [Display(Name = "设备名")]
        public Guid? DeviceId { get; set; }

        protected override void InitVM()
        {
            AllDevices = DC.Set<Device>().AsNoTracking().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                .OrderBy(x => x.Parent.Index).ThenBy(x => x.Parent.DeviceName)
                .ThenBy(x => x.Index).ThenBy(x => x.Parent.DeviceName)
                .GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }
}
