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

namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public partial class DeviceVariableListVM : BasePagedListVM<DeviceVariable_View, DeviceVariableSearcher>
    {
        public List<TreeSelectListItem> AllDevices { get; set; }
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800).SetBindVisiableColName("detail"),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("DeviceVariable", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }

        protected override void InitListVM()
        {
            AllDevices = DC.Set<Device>().AsNoTracking()
                .OrderBy(x => x.Parent.Index).ThenBy(x => x.Parent.DeviceName)
                .OrderBy(x => x.Index).ThenBy(x => x.DeviceName)
                .GetTreeSelectListItems(Wtm, x => x.DeviceName);
            foreach (var device in AllDevices)
            {
                foreach (var item in device.Children)
                {
                    item.Text = item.Text;
                    item.Icon = "layui-icon layui-icon-link";
                    item.Expended = true;
                }
            }
            base.InitListVM();
        }
        protected override IEnumerable<IGridColumn<DeviceVariable_View>> InitGridHeader()
        {
            return new List<GridColumn<DeviceVariable_View>>{
                this.MakeGridHeader(x => x.Name).SetSort(true).SetWidth(100),
                //this.MakeGridHeader(x => x.Description),
                this.MakeGridHeader(x => x.Method).SetSort(true).SetWidth(160),
                this.MakeGridHeader(x => x.DeviceAddress).SetSort(true).SetWidth(80),
                this.MakeGridHeader(x => x.DataType).SetSort(true).SetWidth(80),
                this.MakeGridHeader(x => x.Value).SetWidth(80),
                this.MakeGridHeader(x => x.CookedValue).SetWidth(80),
                this.MakeGridHeader(x => x.State).SetWidth(80),
                this.MakeGridHeader(x => x.Expressions).SetSort(true).SetWidth(80),
                //this.MakeGridHeader(x => x.ProtectType).SetSort(true),
                this.MakeGridHeader(x => x.DeviceName_view).SetSort(true).SetWidth(90),
                this.MakeGridHeader(x=> "detail").SetHide().SetFormat((a,b)=>{
                    return "false";
                }),
                this.MakeGridHeaderAction(width: 115)
            };
        }

        public override void AfterDoSearcher()
        {
            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            foreach (var item in EntityList)
            {
                var DapThread = deviceService.DeviceThreads.Where(x => x.Device.ID == item.DeviceId).FirstOrDefault();
                if (DapThread?.DeviceValues != null && DapThread.DeviceValues.ContainsKey(item.ID))
                {
                    item.Value = DapThread.DeviceValues[item.ID].Value?.ToString();
                    item.CookedValue = DapThread.DeviceValues[item.ID].CookedValue?.ToString();
                    item.State = DapThread.DeviceValues[item.ID].StatusType.ToString();
                }
            }

        }
        public override IOrderedQueryable<DeviceVariable_View> GetSearchQuery()
        {
            var query = DC.Set<DeviceVariable>()
                .CheckContain(Searcher.Name, x => x.Name)
                .CheckContain(Searcher.Method, x => x.Method)
                .CheckContain(Searcher.DeviceAddress, x => x.DeviceAddress)
                .CheckEqual(Searcher.DataType, x => x.DataType)
                .CheckEqual(Searcher.DeviceId, x => x.DeviceId)
                .Select(x => new DeviceVariable_View
                {
                    ID = x.ID,
                    DeviceId = x.DeviceId,
                    Name = x.Name,
                    Description = x.Description,
                    Method = x.Method,
                    DeviceAddress = x.DeviceAddress,
                    DataType = x.DataType,
                    Expressions = x.Expressions,
                    ProtectType = x.ProtectType,
                    DeviceName_view = x.Device.DeviceName,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class DeviceVariable_View : DeviceVariable
    {
        [Display(Name = "设备名")]
        public String DeviceName_view { get; set; }
        [Display(Name = "原值")]
        public String Value { get; set; }
        [Display(Name = "值")]
        public String CookedValue { get; set; }
        [Display(Name = "状态")]
        public String State { get; set; }

    }
}
