using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Microsoft.EntityFrameworkCore;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    static class DeleteDevices
    {
        public static DeleteRet doDelete(DeviceService pluginManager,IDataContext DC,List<Guid> Ids)
        {
            DeleteRet deleteRet = new() { IsSuccess = false };
            using (var transaction = DC.BeginTransaction())
            {
                try
                {
                    var daps = DC.Set<Device>().Include(x => x.Children).Where(x => Ids.Contains(x.ID)).ToList();

                    foreach (var dap in daps)
                    {
                        if (dap == null)
                        {
                            deleteRet.Message = "采集点不存在，可能已经被删除了";
                            return deleteRet;
                        }
                        else if (dap.DeviceTypeEnum == DeviceTypeEnum.Group && dap.Children.Count() != 0)
                        {
                            deleteRet.Message = "组内还有设备，暂不支持组删除";
                            return deleteRet;
                        }
                        else
                        {
                            var dapConfigs = DC.Set<DeviceConfig>().Where(x => x.DeviceId == dap.ID).ToList();
                            var dapVariables = DC.Set<DeviceVariable>().Where(x => x.DeviceId == dap.ID).ToList();
                            DC.Set<DeviceConfig>().RemoveRange(dapConfigs);
                            DC.Set<DeviceVariable>().RemoveRange(dapVariables);
                        }
                        pluginManager.RemoveDeviceThread(dap);
                    }
                    DC.Set<Device>().RemoveRange(daps);
                    DC.SaveChanges();
                    transaction.Commit();
                    deleteRet.IsSuccess=true;                    
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    deleteRet.Message = $"其他错误,{ex}";
                }
            }


            return deleteRet;
        }
    }

    public class DeleteRet
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
