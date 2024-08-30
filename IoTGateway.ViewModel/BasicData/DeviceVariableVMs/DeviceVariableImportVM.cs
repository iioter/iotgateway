using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;
using Microsoft.Extensions.Primitives;
using Plugin;
using PluginInterface;


namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public partial class DeviceVariableTemplateVM : BaseTemplateVM
    {
        [Display(Name = "变量名")]
        public ExcelPropety Name_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Name);
        [Display(Name = "排序")]
        public ExcelPropety Index_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Index);
        [Display(Name = "描述")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Description);
        [Display(Name = "方法")]
        public ExcelPropety Method_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Method);
        [Display(Name = "地址")]
        public ExcelPropety DeviceAddress_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.DeviceAddress);
        [Display(Name = "数据类型")]
        public ExcelPropety DataType_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.DataType);
        [Display(Name = "触发")]
        public ExcelPropety IsTrigger_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.IsTrigger);
        [Display(Name = "大小端")]
        public ExcelPropety EndianType_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.EndianType);
        [Display(Name = "表达式")]
        public ExcelPropety Expressions_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Expressions);
        [Display(Name = "上传")]
        public ExcelPropety IsUpload_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.IsUpload);
        [Display(Name = "权限")]
        public ExcelPropety ProtectType_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.ProtectType);
        public ExcelPropety Device_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.DeviceId);
        [Display(Name = "设备别名")]
        public ExcelPropety Alias_Excel = ExcelPropety.CreateProperty<DeviceVariable>(x => x.Alias);

        protected override void InitVM()
        {
            Device_Excel.DataType = ColumnDataType.ComboBox;
            Device_Excel.ListItems = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }
    }

    public class DeviceVariableImportVM : BaseImportVM<DeviceVariableTemplateVM, DeviceVariable>
    {
        public override bool BatchSaveData()
        {
            var result = base.BatchSaveData();

            FC["Ids"] = new StringValues(EntityList.Select(x => x.DeviceId.ToString()).ToArray());
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            UpdateDevices.UpdateDevice(DC, deviceService, FC);

            return result;
        }
    }

}
