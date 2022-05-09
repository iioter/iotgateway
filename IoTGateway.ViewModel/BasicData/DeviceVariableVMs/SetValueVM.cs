using Microsoft.EntityFrameworkCore;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using System.ComponentModel.DataAnnotations;

namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public class SetValueVM : BaseVM
    {
        public string 设备名 { get; set; }
        public string 变量名 { get; set; }
        public string 类型 { get; set; }
        public string 当前原始值 { get; set; }
        public string 当前计算值 { get; set; }
        public string 状态 { get; set; }
        public string 设定原始值 { get; set; }
        public string 设置结果 { get; set; }

        public void Set()
        {
            try
            {
                var variable = DC.Set<DeviceVariable>().Where(x => x.ID == Guid.Parse(FC["id"].ToString())).AsNoTracking().Include(x => x.Device).FirstOrDefault();
                设备名 = variable.Device.DeviceName;
                变量名 = variable.Name;
                类型 = variable.DataType.GetEnumDisplayName();

                var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                var dapThread = deviceService.DeviceThreads.Where(x => x._device.ID == variable.DeviceId).FirstOrDefault();

                if (dapThread?.DeviceValues != null && dapThread.DeviceValues.ContainsKey(variable.ID))
                {
                    当前原始值 = dapThread.DeviceValues[variable.ID].Value?.ToString();
                    当前计算值 = dapThread.DeviceValues[variable.ID].CookedValue?.ToString();
                    状态 = dapThread.DeviceValues[variable.ID].StatusType.ToString();
                }

                if (variable == null || variable.Device == null || dapThread == null)
                    设置结果 = "设置失败，找不到设备(变量)";

                else
                {
                    PluginInterface.RpcRequest request = new PluginInterface.RpcRequest()
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        DeviceName = variable.Device.DeviceName,
                        Method = "write",
                        Params = new Dictionary<string, object>() { { variable.Name, 设定原始值 } }
                    };
                    dapThread.MyMqttClient_OnExcRpc(this, request);
                    设置结果 = "设置成功";
                }
            }
            catch (Exception ex)
            {

                设置结果 = $"设置失败,{ex}";
            }
        }

        protected override void InitVM()
        {
            var variable = DC.Set<DeviceVariable>().Where(x => x.ID == Guid.Parse(FC["id"].ToString())).AsNoTracking().Include(x => x.Device).FirstOrDefault();
            设备名 = variable.Device.DeviceName;
            变量名 = variable.Name;
            类型 = variable.DataType.GetEnumDisplayName();

            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            var dapThread = deviceService.DeviceThreads.Where(x => x._device.ID == variable.DeviceId).FirstOrDefault();

            if (dapThread?.DeviceValues != null && dapThread.DeviceValues.ContainsKey(variable.ID))
            {
                当前原始值 = dapThread.DeviceValues[variable.ID].Value?.ToString();
                当前计算值 = dapThread.DeviceValues[variable.ID].CookedValue?.ToString();
                状态 = dapThread.DeviceValues[variable.ID].StatusType.ToString();
            }

            base.InitVM();
        }
    }
}
