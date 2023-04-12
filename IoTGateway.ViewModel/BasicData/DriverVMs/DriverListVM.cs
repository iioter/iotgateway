using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.BasicData.DriverVMs
{
    public partial class DriverListVM : BasePagedListVM<Driver_View, DriverSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Driver", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("Driver", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("Driver", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<Driver_View>> InitGridHeader()
        {
            return new List<GridColumn<Driver_View>>{
                this.MakeGridHeader(x => x.DriverName),
                this.MakeGridHeader(x => x.FileName),
                this.MakeGridHeader(x => x.AssembleName),
                this.MakeGridHeader(x => x.AuthorizesNum),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<Driver_View> GetSearchQuery()
        {
            var query = DC.Set<Driver>()
                .CheckContain(Searcher.DriverName, x=>x.DriverName)
                .Select(x => new Driver_View
                {
				    ID = x.ID,
                    DriverName = x.DriverName,
                    FileName = x.FileName,
                    AssembleName = x.AssembleName,
                    AuthorizesNum = x.AuthorizesNum,
                })
                .OrderBy(x => x.FileName);
            return query;
        }

    }

    public class Driver_View : Driver{

    }
}
