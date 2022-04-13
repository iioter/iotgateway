using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Rpc.RpcLogVMs
{
    public partial class RpcLogVM : BaseCRUDVM<RpcLog>
    {
        public List<ComboSelectListItem> AllDevices { get; set; }

        public RpcLogVM()
        {
            SetInclude(x => x.Device);
        }

        protected override void InitVM()
        {
            AllDevices = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }

        public override void DoAdd()
        {           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
