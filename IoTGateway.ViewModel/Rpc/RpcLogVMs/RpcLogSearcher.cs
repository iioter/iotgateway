using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Rpc.RpcLogVMs
{
    public partial class RpcLogSearcher : BaseSearcher
    {
        [Display(Name = "发起方")]
        public RpcSide? RpcSide { get; set; }
        [Display(Name = "开始时间")]
        public DateRange StartTime { get; set; }
        public List<ComboSelectListItem> AllDevices { get; set; }

        [Display(Name = "设备名")]
        public Guid? DeviceId { get; set; }
        [Display(Name = "方法名")]
        public String Method { get; set; }
        [Display(Name = "请求参数")]
        public String Params { get; set; }
        [Display(Name = "结果")]
        public Boolean? IsSuccess { get; set; }

        protected override void InitVM()
        {
            AllDevices = DC.Set<Device>().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device).GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }
}
