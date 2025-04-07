using IoTGateway.Model;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceApiVM : BaseCRUDVM<Device>
    {
        public DeviceApiVM()
        {
            SetInclude(x => x.Driver);
            SetInclude(x => x.Parent);
        }

        protected override void InitVM()
        {
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