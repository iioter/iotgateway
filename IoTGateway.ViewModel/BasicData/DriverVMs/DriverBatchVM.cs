using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.BasicData.DriverVMs
{
    public partial class DriverBatchVM : BaseBatchVM<Driver, Driver_BatchEdit>
    {
        public DriverBatchVM()
        {
            ListVM = new DriverListVM();
            LinkedVM = new Driver_BatchEdit();
        }

    }

	/// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class Driver_BatchEdit : BaseVM
    {

        protected override void InitVM()
        {
        }

    }

}
