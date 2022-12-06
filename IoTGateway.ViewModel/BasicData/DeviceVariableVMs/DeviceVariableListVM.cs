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

namespace IoTGateway.ViewModel.BasicData.DeviceVariableVMs
{
    public partial class DeviceVariableListVM : BasePagedListVM<DeviceVariable_View, DeviceVariableSearcher>
    {
        public List<TreeSelectListItem> AllDevices { get; set; }
        public List<LayuiTreeItem> DevicesTree { get; set; }
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("DeviceVariable","SetValue","写入值","写入值", GridActionParameterTypesEnum.MultiIds,"BasicData",600).SetIconCls("_wtmicon _wtmicon-xiayibu").SetHideOnToolBar(false).SetShowInRow(false).SetBindVisiableColName("setValue"),
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

            var deviceService = Wtm.ServiceProvider.GetService(typeof(DeviceService)) as DeviceService;
            foreach (var device in AllDevices)
            {
                foreach (var item in device.Children)
                {
                    var deviceThread = deviceService.DeviceThreads.Where(x => x.Device.ID.ToString() == (string)item.Value).FirstOrDefault();
                    if (deviceThread != null)
                        item.Icon = deviceThread.Device.AutoStart ? (deviceThread.Driver.IsConnected ? "layui-icon layui-icon-link" : "layui-icon layui-icon-unlink") : "layui-icon layui-icon-pause";

                    item.Text = " " + item.Text;
                    item.Expended = true;
                    item.Selected = item.Value.ToString() == IoTBackgroundService.VariableSelectDeviceId.ToString();

                }
            }
            DevicesTree = GetLayuiTree(AllDevices);
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
                this.MakeGridHeader(x => x.EndianType).SetSort(true).SetWidth(120),
                this.MakeGridHeader(x => x.Value).SetWidth(80).SetFormat((a,b)=>{
                    return $"<div id='id{a.ID}_Value'>{a.Value}</div>";
                }),
                this.MakeGridHeader(x => x.CookedValue).SetWidth(80).SetFormat((a,b)=>{
                    return $"<div id='id{a.ID}_CookedValue'>{a.CookedValue}</div>";
                }),
                this.MakeGridHeader(x => x.State).SetWidth(80).SetFormat((a,b)=>{
                    return $"<div id='id{a.ID}_State'>{a.State}</div>";
                }),
                this.MakeGridHeader(x => x.Expressions).SetWidth(150),
                //this.MakeGridHeader(x => x.ProtectType).SetSort(true),
                this.MakeGridHeader(x => x.DeviceName_view).SetSort(true).SetWidth(90),
                this.MakeGridHeader(x=> "detail").SetHide().SetFormat((a,b)=>{
                    return "false";
                }),
                this.MakeGridHeader(x=>"setValue").SetHide().SetFormat((a,b)=>{
                    if(a.Device.AutoStart== true)
                        return "true";
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
                    item.Value = DapThread?.DeviceValues[item.ID]?.Value?.ToString();
                    item.CookedValue = DapThread?.DeviceValues[item.ID]?.CookedValue?.ToString();
                    item.State = DapThread?.DeviceValues[item.ID]?.StatusType.ToString();
                }
            }

        }
        public override IOrderedQueryable<DeviceVariable_View> GetSearchQuery()
        {
            if (Searcher.DeviceId != null)
                IoTBackgroundService.VariableSelectDeviceId = Searcher.DeviceId;

            var query = DC.Set<DeviceVariable>().Include(x => x.Device)
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
                    Index = x.Index,
                    Description = x.Description,
                    Method = x.Method,
                    DeviceAddress = x.DeviceAddress,
                    DataType = x.DataType,
                    EndianType = x.EndianType,
                    Expressions = x.Expressions,
                    ProtectType = x.ProtectType,
                    DeviceName_view = x.Device.DeviceName,
                    Device = x.Device
                })
                .OrderBy(x => x.DeviceName_view).ThenBy(x => x.Index);
            return query;
        }

        private List<LayuiTreeItem> GetLayuiTree(List<TreeSelectListItem> tree, int level = 0)
        {
            List<LayuiTreeItem> rv = new List<LayuiTreeItem>();
            foreach (var s in tree)
            {
                var news = new LayuiTreeItem
                {
                    Id = (string)s.Value,
                    Title = s.Text,
                    Icon = s.Icon,
                    Url = s.Url,
                    Expand = s.Expended,
                    Level = level,
                    Checked = s.Selected
                    //Children = new List<LayuiTreeItem>()
                };
                if (s.Children != null && s.Children.Count > 0)
                {
                    news.Children = GetLayuiTree(s.Children, level + 1);
                    if (news.Children.Where(x => x.Checked == true || x.Expand == true).Count() > 0)
                    {
                        news.Expand = true;
                    }
                }
                rv.Add(news);
            }
            return rv;
        }
        public class LayuiTreeItem
        {
            [JsonProperty(PropertyName = "title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "icon")]
            public string Icon { get; set; }

            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "children")]
            public List<LayuiTreeItem> Children { get; set; }

            [JsonProperty(PropertyName = "href")]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "spread")]
            public bool Expand { get; set; }

            [JsonProperty(PropertyName = "checked")]
            public bool Checked { get; set; }

            [JsonProperty(PropertyName = "level")]
            public int Level { get; set; }
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
