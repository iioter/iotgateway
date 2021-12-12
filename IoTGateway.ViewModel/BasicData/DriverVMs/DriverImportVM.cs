using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.BasicData.DriverVMs
{
    public partial class DriverTemplateVM : BaseTemplateVM
    {
        [Display(Name = "驱动名")]
        public ExcelPropety DriverName_Excel = ExcelPropety.CreateProperty<Driver>(x => x.DriverName);
        [Display(Name = "文件名")]
        public ExcelPropety FileName_Excel = ExcelPropety.CreateProperty<Driver>(x => x.FileName);
        [Display(Name = "程序集名")]
        public ExcelPropety AssembleName_Excel = ExcelPropety.CreateProperty<Driver>(x => x.AssembleName);
        [Display(Name = "剩余授权数量")]
        public ExcelPropety AuthorizesNum_Excel = ExcelPropety.CreateProperty<Driver>(x => x.AuthorizesNum);

	    protected override void InitVM()
        {
        }

    }

    public class DriverImportVM : BaseImportVM<DriverTemplateVM, Driver>
    {

    }

}
