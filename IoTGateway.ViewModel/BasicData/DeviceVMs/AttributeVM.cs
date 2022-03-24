using Microsoft.EntityFrameworkCore;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public class AttributeVM : BaseVM
    {
        public string 请求结果 { get; set; }
        public string 设备名称 { get; set; }

        public void Request()
        {
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var device = DC.Set<Device>().Where(x => x.ID == Guid.Parse(FC["id"].ToString())).Include(x => x.Parent).Include(x=>x.DeviceConfigs).Include(x => x.Driver).FirstOrDefault();

                    if (device == null)
                        请求结果 = "复制失败，找不到设备";
                    else
                    {
                        var myMqttClient = Wtm.ServiceProvider.GetService(typeof(MyMqttClient)) as MyMqttClient;
                        myMqttClient.RequestAttributes(device.DeviceName, true, device.DeviceConfigs.Where(x => x.DataSide == DataSide.AnySide).Select(x => x.DeviceConfigName).ToArray());
                    }
                    DC.SaveChanges();
                    transaction.Commit();
                    请求结果 = "请求成功";

                    var pluginManager = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
                    pluginManager?.UpdateDevice(device);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    请求结果 = $"请求超时,{ex}";
                }
            }
        }

        protected override void InitVM()
        {
            var device = DC.Set<Device>().AsNoTracking().Include(x => x.Parent).Where(x => x.ID == Guid.Parse(FC["id"].ToString())).FirstOrDefault();
            设备名称 = $"{device?.Parent?.DeviceName}===>{device?.DeviceName}";

            base.InitVM();
        }
    }
}
