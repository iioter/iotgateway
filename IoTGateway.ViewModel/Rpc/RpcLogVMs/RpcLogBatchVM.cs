using IoTGateway.Model;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.Rpc.RpcLogVMs
{
    public partial class RpcLogBatchVM : BaseBatchVM<RpcLog, RpcLog_BatchEdit>
    {
        public RpcLogBatchVM()
        {
            ListVM = new RpcLogListVM();
            LinkedVM = new RpcLog_BatchEdit();
        }
    }

    /// <summary>
    /// Class to define batch edit fields
    /// </summary>
    public class RpcLog_BatchEdit : BaseVM
    {
        protected override void InitVM()
        {
        }
    }
}