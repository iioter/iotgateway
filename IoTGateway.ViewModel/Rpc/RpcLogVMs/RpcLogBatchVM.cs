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
