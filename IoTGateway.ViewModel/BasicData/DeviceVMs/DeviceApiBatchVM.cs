using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


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
