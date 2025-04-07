using IoTGateway.Model;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceApiTemplateVM : BaseTemplateVM
    {
        [Display(Name = "DeviceName")]
        public ExcelPropety DeviceName_Excel = ExcelPropety.CreateProperty<Device>(x => x.DeviceName);

        [Display(Name = "Sort")]
        public ExcelPropety Index_Excel = ExcelPropety.CreateProperty<Device>(x => x.Index);

        [Display(Name = "Description")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<Device>(x => x.Description);

        public ExcelPropety Driver_Excel = ExcelPropety.CreateProperty<Device>(x => x.DriverId);

        [Display(Name = "AutoStart")]
        public ExcelPropety AutoStart_Excel = ExcelPropety.CreateProperty<Device>(x => x.AutoStart);

        [Display(Name = "ChangeUpload")]
        public ExcelPropety CgUpload_Excel = ExcelPropety.CreateProperty<Device>(x => x.CgUpload);

        [Display(Name = "EnforcePeriodms")]
        public ExcelPropety EnforcePeriod_Excel = ExcelPropety.CreateProperty<Device>(x => x.EnforcePeriod);

        [Display(Name = "CmdPeriodms")]
        public ExcelPropety CmdPeriod_Excel = ExcelPropety.CreateProperty<Device>(x => x.CmdPeriod);

        [Display(Name = "Type")]
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

    public class DeviceApiImportVM : BaseImportVM<DeviceApiTemplateVM, Device>
    {
    }
}