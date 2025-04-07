using IoTGateway.Model;
using WalkingTec.Mvvm.Core;

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