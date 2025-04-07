using IoTGateway.Model;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceTemplateVM : BaseTemplateVM
    {
        [Display(Name = "名称")]
        public ExcelPropety DeviceName_Excel = ExcelPropety.CreateProperty<Device>(x => x.DeviceName);

        [Display(Name = "排序")]
        public ExcelPropety Index_Excel = ExcelPropety.CreateProperty<Device>(x => x.Index);

        [Display(Name = "描述")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<Device>(x => x.Description);

        public ExcelPropety Driver_Excel = ExcelPropety.CreateProperty<Device>(x => x.DriverId);

        [Display(Name = "自启动")]
        public ExcelPropety AutoStart_Excel = ExcelPropety.CreateProperty<Device>(x => x.AutoStart);

        [Display(Name = "类型")]
        public ExcelPropety DeviceTypeEnum_Excel = ExcelPropety.CreateProperty<Device>(x => x.DeviceTypeEnum);

        [Display(Name = "_Admin.Parent")]
        public ExcelPropety Parent_Excel = ExcelPropety.CreateProperty<Device>(x => x.ParentId);

        protected override void InitVM()
        {
            Driver_Excel.DataType = ColumnDataType.ComboBox;
            Driver_Excel.ListItems = DC.Set<Driver>().GetSelectListItems(Wtm, y => y.DriverName);
            Parent_Excel.DataType = ColumnDataType.ComboBox;
            Parent_Excel.ListItems = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }
    }

    public class DeviceImportVM : BaseImportVM<DeviceTemplateVM, Device>
    {
    }
}