using IoTGateway.Model;
using Plugin;
using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.BasicData.DeviceConfigVMs
{
    public partial class DeviceConfigBatchVM : BaseBatchVM<DeviceConfig, DeviceConfig_BatchEdit>
    {
        public DeviceConfigBatchVM()
        {
            ListVM = new DeviceConfigListVM();
            LinkedVM = new DeviceConfig_BatchEdit();
        }

        public override bool DoBatchDelete()
        {
            var ret = base.DoBatchDelete();
            if (ret)
            {
                var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                UpdateDevices.UpdateConfig(DC, deviceService, FC);
            }
            return ret;
        }

        public override bool DoBatchEdit()
        {
            var ret = base.DoBatchEdit();
            if (ret)
            {
                var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                UpdateDevices.UpdateConfig(DC, deviceService, FC);
            }
            return ret;
        }
    }

    /// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class DeviceConfig_BatchEdit : BaseVM
    {
        [Display(Name = "值")]
        public String Value { get; set; }

        protected override void InitVM()
        {
        }
    }
}