using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.BasicData.DeviceConfigVMs
{
    public partial class DeviceConfigTemplateVM : BaseTemplateVM
    {
        [Display(Name = "名称")]
        public ExcelPropety DeviceConfigName_Excel = ExcelPropety.CreateProperty<DeviceConfig>(x => x.DeviceConfigName);
        [Display(Name = "描述")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<DeviceConfig>(x => x.Description);
        [Display(Name = "值")]
        public ExcelPropety Value_Excel = ExcelPropety.CreateProperty<DeviceConfig>(x => x.Value);
        [Display(Name = "备注")]
        public ExcelPropety EnumInfo_Excel = ExcelPropety.CreateProperty<DeviceConfig>(x => x.EnumInfo);
        public ExcelPropety Device_Excel = ExcelPropety.CreateProperty<DeviceConfig>(x => x.DeviceId);

	    protected override void InitVM()
        {
            Device_Excel.DataType = ColumnDataType.ComboBox;
            Device_Excel.ListItems = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }

    public class DeviceConfigImportVM : BaseImportVM<DeviceConfigTemplateVM, DeviceConfig>
    {

    }

}
