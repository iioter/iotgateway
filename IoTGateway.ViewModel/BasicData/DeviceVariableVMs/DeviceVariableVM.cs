using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IoTGateway.DataAccess.Migrations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Plugin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using PluginInterface;

namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public partial class DeviceVariableVM : BaseCRUDVM<DeviceVariable>
    {
        public List<ComboSelectListItem> AllDevices { get; set; }
        public List<ComboSelectListItem> AllMethods { get; set; }

        public DeviceVariableVM()
        {
            SetInclude(x => x.Device);
        }

        protected override void InitVM()
        {
            if (this.ControllerName.ToLower().Contains("add"))
            {
                this.Entity.IsUpload = true;
                this.Entity.ProtectType =  ProtectTypeEnum.ReadWrite;
            }
            AllDevices = DC.Set<Device>().AsNoTracking().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                .OrderBy(x => x.Parent.Index).ThenBy(x => x.Parent.DeviceName)
                .OrderBy(x => x.Index).ThenBy(x => x.DeviceName)
                .GetSelectListItems(Wtm, y => y.DeviceName);
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            if (Entity.DeviceId != null)
            {
                AllMethods = deviceService.GetDriverMethods(Entity.DeviceId);
            }
            else if (IoTBackgroundService.VariableSelectDeviceId != null)
            {
                Entity.DeviceId = IoTBackgroundService.VariableSelectDeviceId;
                AllMethods = deviceService.GetDriverMethods(Entity.DeviceId);
            }

            if (AllMethods?.Count() > 0)
                AllMethods[0].Selected = true;
        }

        public override void DoAdd()
        {
            base.DoAdd();
            UpdateVaribale();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
            UpdateVaribale();
        }

        public override void DoDelete()
        {
            //先获取id
            var id = UpdateDevices.FC2Guids(FC);
            var deviceId = DC.Set<DeviceVariable>().Where(x => id.Contains(x.ID)).Select(x => x.DeviceId).FirstOrDefault();
            FC["Entity.DeviceId"] = (StringValues)deviceId.ToString();
            base.DoDelete();
            UpdateVaribale();
        }


        private void UpdateVaribale()
        {
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            UpdateDevices.UpdateVaribale(DC, deviceService, FC);

        }
        public override DuplicatedInfo<DeviceVariable> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(x => x.DeviceId), SimpleField(x => x.Name), SimpleField(x => x.Alias));
            return rv;
        }
    }
}
