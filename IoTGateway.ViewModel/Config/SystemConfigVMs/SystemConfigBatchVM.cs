using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Config.SystemConfigVMs
{
    public partial class SystemConfigBatchVM : BaseBatchVM<SystemConfig, SystemConfig_BatchEdit>
    {
        public SystemConfigBatchVM()
        {
            ListVM = new SystemConfigListVM();
            LinkedVM = new SystemConfig_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class SystemConfig_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
