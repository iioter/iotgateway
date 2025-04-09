using IoTGateway.Model;
using Plugin;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.Config.SystemConfigVMs
{
    public partial class SystemConfigVM : BaseCRUDVM<SystemConfig>
    {
        public SystemConfigVM()
        {
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
            var messageService = Wtm.ServiceProvider.GetService(typeof(MessageService)) as MessageService;
            messageService.StartClientAsync().Wait();
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}