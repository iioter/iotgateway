using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Plugin;

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
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            base.DoDelete();
        }
    }
}
