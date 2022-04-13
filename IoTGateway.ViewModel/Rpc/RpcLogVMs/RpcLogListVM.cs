using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Rpc.RpcLogVMs
{
    public partial class RpcLogListVM : BasePagedListVM<RpcLog_View, RpcLogSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                //this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"Rpc", dialogWidth: 800),
                //this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "Rpc", dialogWidth: 800),
                this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Rpc", dialogWidth: 800),
                this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "Rpc", dialogWidth: 800),
                //this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "Rpc", dialogWidth: 800),
                this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "Rpc", dialogWidth: 800),
                //this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "Rpc", dialogWidth: 800),
                this.MakeStandardAction("RpcLog", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "Rpc"),
            };
        }


        protected override IEnumerable<IGridColumn<RpcLog_View>> InitGridHeader()
        {
            return new List<GridColumn<RpcLog_View>>{
                this.MakeGridHeader(x => x.RpcSide),
                this.MakeGridHeader(x => x.StartTime).SetWidth(150),
                this.MakeGridHeader(x => x.Duration),
                this.MakeGridHeader(x => x.DeviceName_view),
                this.MakeGridHeader(x => x.Method),
                this.MakeGridHeader(x => x.Params),
                this.MakeGridHeader(x => x.IsSuccess).SetHeader("是否成功"),
                this.MakeGridHeader(x => x.Description),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<RpcLog_View> GetSearchQuery()
        {
            var query = DC.Set<RpcLog>()
                .CheckEqual(Searcher.RpcSide, x => x.RpcSide)
                .CheckBetween(Searcher.StartTime?.GetStartTime(), Searcher.StartTime?.GetEndTime(), x => x.StartTime, includeMax: false)
                .CheckEqual(Searcher.DeviceId, x => x.DeviceId)
                .CheckContain(Searcher.Method, x => x.Method)
                .CheckContain(Searcher.Params, x => x.Params)
                .CheckEqual(Searcher.IsSuccess, x => x.IsSuccess)
                .Select(x => new RpcLog_View
                {
                    ID = x.ID,
                    RpcSide = x.RpcSide,
                    StartTime = x.StartTime,
                    DeviceName_view = x.Device.DeviceName,
                    Method = x.Method,
                    Params = x.Params,
                    EndTime = x.EndTime,
                    IsSuccess = x.IsSuccess,
                    Description = x.Description,
                })
                .OrderByDescending(x => x.StartTime);
            return query;
        }
        public override void AfterDoSearcher()
        {
            foreach (var entity in EntityList)
            {
                try
                {
                    TimeSpan ts1 = new TimeSpan(((DateTime)entity.StartTime).Ticks);
                    TimeSpan ts2 = new TimeSpan(((DateTime)entity.EndTime).Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    entity.Duration = Math.Round(ts.TotalMilliseconds, 2);
                }
                catch (Exception)
                {

                }
            }
            base.AfterDoSearcher();
        }

    }


    public class RpcLog_View : RpcLog
    {
        [Display(Name = "设备名")]
        public String DeviceName_view { get; set; }
        [Display(Name = "持续时间(ms)")]
        public double Duration { get; set; }

    }
}
