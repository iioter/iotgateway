using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;
using PluginInterface;
using Plugin;
using Newtonsoft.Json;
using IoTGateway.DataAccess.Migrations;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using IoTGateway.ViewModel.BasicData.DeviceVariableVMs;
using IoTGateway.ViewModel.BasicData.DeviceConfigVMs;
using System.IO;
using System.Reflection;
using IoTGateway.ViewModel.Config.SystemConfigVMs;
using IoTGateway.ViewModel.BasicData.DeviceVMs;

namespace IoTGateway.ViewModel.BasicData
{
    public class ExportDevicesSetting : BaseVM
    {
        #region GetData
        private List<Device> GetAllDevices()
        {
            var queryResult = DC.Set<Device>().AsNoTracking().AsNoTracking()
                .Include(x => x.Parent).Include(x => x.Driver)
                .OrderBy(x => x.ParentId).ToList();

            return queryResult;
        }

        private List<DeviceConfig> GetAllDeviceConfigs()
        {
            var queryResult = DC.Set<DeviceConfig>().AsNoTracking().Include(x=>x.Device)
                .OrderBy(x => x.Device.DeviceName).ThenBy(x => x.DeviceConfigName).ToList();
            return queryResult;
        }


        private List<DeviceVariable> GetAllDeviceVariables()
        {
            var queryResult = DC.Set<DeviceVariable>().AsNoTracking().Include(x => x.Device)
                .OrderBy(x => x.Device.DeviceName).ThenBy(x => x.Alias).ThenBy(x => x.Method).ThenBy(x => x.Index).ThenBy(x => x.DeviceAddress).ToList();
            return queryResult;
        }

        private List<SystemConfig> GetAllSystemConfigs()
        {
            var queryResult = DC.Set<SystemConfig>().AsNoTracking()
                .AsNoTracking()
                .OrderBy(x => x.ID).ToList();
            return queryResult;
        }
        #endregion

        #region GenerateWorkSheet
        private IWorkbook GenerateDevicesSheet(IWorkbook book, List<Device> devices)
        {
            if (book == null)
            {
                book = new XSSFWorkbook();
            }
            ISheet sheet = book.CreateSheet("设备维护");

            int currentRow = 0;

            #region 生成表头

            string[] colName = { "名称", "排序", "驱动名", "启动", "变化上传", "归档周期ms", "指令间隔ms", "类型" ,"所属组"};
            IRow row = sheet.CreateRow(currentRow);
            row.HeightInPoints = 20;

            for (int i = 0; i < colName.Length; i++)
            {
                row.CreateCell(i).SetCellValue(colName[i]);
            }

            currentRow++;
            #endregion

            #region 生成数据
            foreach (var device in devices)
            {
                int currentCol = 0;
                IRow rowData = sheet.CreateRow(currentRow);
                rowData.CreateCell(currentCol).SetCellValue(device.DeviceName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.Index);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.Driver?.DriverName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.AutoStart);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.CgUpload);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.EnforcePeriod);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.CmdPeriod);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(PropertyHelper.GetEnumDisplayName(device.DeviceTypeEnum));
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(device.Parent?.DeviceName);
                currentRow++;
            }

            #endregion

            return book;
        }

        private IWorkbook GenerateDeviceConfigsSheet(IWorkbook book, List<DeviceConfig> deviceConfigs)
        {
            if (book == null)
            {
                book = new XSSFWorkbook();
            }
            ISheet sheet = book.CreateSheet("通讯配置");

            int currentRow = 0;

            #region 生成表头

            string[] colName = { "设备名", "名称", "属性侧", "描述", "值", "备注" };
            IRow row = sheet.CreateRow(currentRow);
            row.HeightInPoints = 20;

            for (int i = 0; i < colName.Length; i++)
            {
                row.CreateCell(i).SetCellValue(colName[i]);
            }

            currentRow++;
            #endregion

            #region 生成数据
            foreach (var deviceConfig in deviceConfigs)
            {
                int currentCol = 0;
                IRow rowData = sheet.CreateRow(currentRow);
                rowData.CreateCell(currentCol).SetCellValue(deviceConfig.Device?.DeviceName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceConfig.DeviceConfigName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(PropertyHelper.GetEnumDisplayName(deviceConfig.DataSide));
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceConfig.Description);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceConfig.Value);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceConfig.EnumInfo);
                
                currentRow++;
            }

            #endregion

            return book;
        }

        private IWorkbook GenerateDeviceVariablesSheet(IWorkbook book, List<DeviceVariable> deviceVariables)
        {
            if (book == null)
            {
                book = new XSSFWorkbook();
            }
            ISheet sheet = book.CreateSheet("变量配置");

            int currentRow = 0;

            #region 生成表头

            string[] colName = { "设备名", "变量名", "方法", "地址", "类型", "大小端", "表达式", "别名","上传", "排序" };
            IRow row = sheet.CreateRow(currentRow);
            row.HeightInPoints = 20;

            for (int i = 0; i < colName.Length; i++)
            {
                row.CreateCell(i).SetCellValue(colName[i]);
            }

            currentRow++;
            #endregion

            #region 生成数据
            foreach (var deviceVariable in deviceVariables)
            {
                int currentCol = 0;
                IRow rowData = sheet.CreateRow(currentRow);
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Device.DeviceName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Name);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Method);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.DeviceAddress);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(PropertyHelper.GetEnumDisplayName(deviceVariable.DataType)); //deviceVariable.DataType.ToString();
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(PropertyHelper.GetEnumDisplayName(deviceVariable.EndianType));
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Expressions);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Alias);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.IsUpload);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(deviceVariable.Index);

                currentRow++;
            }

            #endregion

            return book;
        }

        private IWorkbook GenerateSystemConfigSheet(IWorkbook book, List<SystemConfig> systemConfigs)
        {
            if (book == null)
            {
                book = new XSSFWorkbook();
            }
            ISheet sheet = book.CreateSheet("传输配置");

            int currentRow = 0;

            #region 生成表头

            string[] colName = { "网关名称", "ClientId", "输出平台", "Mqtt服务器", "Mqtt端口", "Mqtt用户名", "Mqtt密码" };
            IRow row = sheet.CreateRow(currentRow);
            row.HeightInPoints = 20;

            for (int i = 0; i < colName.Length; i++)
            {
                row.CreateCell(i).SetCellValue(colName[i]);
            }

            currentRow++;
            #endregion

            #region 生成数据
            foreach (var systemConfig in systemConfigs)
            {
                int currentCol = 0;
                IRow rowData = sheet.CreateRow(currentRow);
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.GatewayName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.ClientId);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(PropertyHelper.GetEnumDisplayName(systemConfig.IoTPlatformType));
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.MqttIp);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.MqttPort);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.MqttUName);
                currentCol++;
                rowData.CreateCell(currentCol).SetCellValue(systemConfig.MqttUPwd);
                currentRow++;
            }

            #endregion

            return book;
        }

        #endregion

        public byte[] Export()
        {
            byte[] result = null;

            IWorkbook book = new XSSFWorkbook();

            //Sheet1-设备维护
            var allDevices = GetAllDevices();
            book = GenerateDevicesSheet(book, allDevices);

            //Sheet2-通讯设置
            var allDeviceVariables = GetAllDeviceVariables();
            book = GenerateDeviceVariablesSheet(book, allDeviceVariables);

            //Sheet3-变量配置
            var allDeviceConfigs = GetAllDeviceConfigs();
            book = GenerateDeviceConfigsSheet(book, allDeviceConfigs);

            //Sheet4-传输配置
            var allSystemConfigs = GetAllSystemConfigs();
            book = GenerateSystemConfigSheet(book, allSystemConfigs);

            using MemoryStream ms = new MemoryStream();
            book.Write(ms);
            result = ms.ToArray();

            return result;
        }
    }
}
