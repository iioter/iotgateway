using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Rpc.RpcLogVMs
{
    public partial class RpcLogTemplateVM : BaseTemplateVM
    {
        [Display(Name = "发起方")]
        public ExcelPropety RpcSide_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.RpcSide);
        [Display(Name = "开始时间")]
        public ExcelPropety StartTime_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.StartTime);
        public ExcelPropety Device_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.DeviceId);
        [Display(Name = "方法名")]
        public ExcelPropety Method_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.Method);
        [Display(Name = "请求参数")]
        public ExcelPropety Params_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.Params);
        [Display(Name = "结束时间")]
        public ExcelPropety EndTime_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.EndTime);
        [Display(Name = "结果")]
        public ExcelPropety IsSuccess_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.IsSuccess);
        [Display(Name = "描述")]
        public ExcelPropety Description_Excel = ExcelPropety.CreateProperty<RpcLog>(x => x.Description);

	    protected override void InitVM()
        {
            Device_Excel.DataType = ColumnDataType.ComboBox;
            Device_Excel.ListItems = DC.Set<Device>().GetSelectListItems(Wtm, y => y.DeviceName);
        }

    }

    public class RpcLogImportVM : BaseImportVM<RpcLogTemplateVM, RpcLog>
    {

    }

}
