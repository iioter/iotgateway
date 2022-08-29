using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Plugin;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace IoTGateway.ViewModel.BasicData.DeviceConfigVMs
{
    public partial class DeviceConfigVM : BaseCRUDVM<DeviceConfig>
    {
        public List<ComboSelectListItem> AllDevices { get; set; }
        public List<ComboSelectListItem> AllTypes { get; set; }

        public DeviceConfigVM()
        {
            SetInclude(x => x.Device);
        }

        protected override void InitVM()
        {
            AllDevices = DC.Set<Device>().AsNoTracking().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Device)
                .OrderBy(x => x.Parent.Index).ThenBy(x => x.Parent.DeviceName)
                .OrderBy(x => x.Index).ThenBy(x => x.DeviceName)
                .GetSelectListItems(Wtm, y => y.DeviceName);
            if (Entity.DeviceId != null)
            {
                if (!string.IsNullOrEmpty(Entity.EnumInfo))
                {
                    AllTypes = new List<ComboSelectListItem>();
                    var EnumInfos = JsonSerializer.Deserialize<Dictionary<string, int>>(Entity.EnumInfo);
                    foreach (var EnumInfo in EnumInfos)
                    {
                        var item = new ComboSelectListItem
                        {
                            Text = EnumInfo.Key,
                            Value = EnumInfo.Key,
                            Selected = Entity.Value == EnumInfo.Key ? true : false
                        };
                        AllTypes.Add(item);
                    }
                }
            }
            else if (IoTBackgroundService.ConfigSelectDeviceId != null)
            {
                Entity.DeviceId = IoTBackgroundService.ConfigSelectDeviceId;
            }
        }

        public override void DoAdd()
        {
            base.DoAdd();
            UpdateConfig();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
            UpdateConfig();
        }

        public override void DoDelete()
        {
            //先获取id
            var id = UpdateDevices.FC2Guids(FC);
            var deviceId = DC.Set<DeviceConfig>().Where(x => id.Contains(x.ID)).Select(x => x.DeviceId).FirstOrDefault();
            FC["Entity.DeviceId"] = (StringValues)deviceId.ToString();
            base.DoDelete();
            UpdateConfig();
        }

        private void UpdateConfig()
        {
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            UpdateDevices.UpdateConfig(DC, deviceService, FC);

        }

        public override DuplicatedInfo<DeviceConfig> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(x => x.DeviceId), SimpleField(x => x.DeviceConfigName));
            return rv;
        }
    }
}
