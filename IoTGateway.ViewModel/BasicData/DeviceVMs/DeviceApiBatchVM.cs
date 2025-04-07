using IoTGateway.Model;
using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceApiBatchVM : BaseBatchVM<Device, DeviceApi_BatchEdit>
    {
        public DeviceApiBatchVM()
        {
            ListVM = new DeviceApiListVM();
            LinkedVM = new DeviceApi_BatchEdit();
        }
    }

    /// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class DeviceApi_BatchEdit : BaseVM
    {
        [Display(Name = "AutoStart")]
        public Boolean? AutoStart { get; set; }

        [Display(Name = "ChangeUpload")]
        public Boolean? CgUpload { get; set; }

        [Display(Name = "EnforcePeriodms")]
        public UInt32? EnforcePeriod { get; set; }

        [Display(Name = "CmdPeriodms")]
        public UInt32? CmdPeriod { get; set; }

        protected override void InitVM()
        {
        }
    }
}