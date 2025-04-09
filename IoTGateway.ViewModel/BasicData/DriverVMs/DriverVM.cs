using IoTGateway.Model;
using Plugin;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.BasicData.DriverVMs
{
    public partial class DriverVM : BaseCRUDVM<Driver>
    {
        public DriverVM()
        {
        }

        protected override void InitVM()
        {
        }

        public override void DoAdd()
        {
            var DriverService = Wtm.ServiceProvider.GetService(typeof(DriverService)) as DriverService;
            Entity.AssembleName = DriverService.GetAssembleNameByFileName(Entity.FileName);
            if (string.IsNullOrEmpty(Entity.AssembleName))
            {
                MSD.AddModelError("", "程序集获取失败");
                return;
            }

            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            var DriverService = Wtm.ServiceProvider.GetService(typeof(DriverService)) as DriverService;
            Entity.AssembleName = DriverService.GetAssembleNameByFileName(Entity.FileName);
            if (string.IsNullOrEmpty(Entity.AssembleName))
            {
                MSD.AddModelError("", "程序集获取失败");
                return;
            }
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}