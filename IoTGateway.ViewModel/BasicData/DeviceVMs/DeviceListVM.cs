using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;
using Microsoft.Extensions.Primitives;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceListVM : BasePagedListVM<Device_View, DeviceSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                this.MakeAction("Device","Copy","设备复制","设备复制", GridActionParameterTypesEnum.SingleId,"BasicData",600).SetIconCls("layui-icon layui-icon-template-1").SetPromptMessage("你确定复制设备，包括配置参数和变量？").SetDialogTitle("复制设备确认").SetHideOnToolBar(true).SetShowInRow(true).SetBindVisiableColName("copy"),
                this.MakeAction("Device","Attribute","请求属性","请求属性", GridActionParameterTypesEnum.SingleId,"BasicData",600).SetIconCls("layui-icon layui-icon-download-circle").SetPromptMessage("你确定请求客户端属性和共享属性吗？").SetDialogTitle("请求属性确认").SetHideOnToolBar(true).SetShowInRow(true).SetBindVisiableColName("attribute"),
                this.MakeAction("Device","CreateGroup","创建组","创建组", GridActionParameterTypesEnum.NoId,"BasicData",600).SetIconCls("_wtmicon _wtmicon-zuzhiqunzu").SetDialogTitle("创建组").SetShowInRow(false),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Create, "创建设备","BasicData", dialogWidth: 800,name:"创建设备").SetIconCls("layui-icon layui-icon-senior"),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "BasicData", dialogWidth: 800),
                //this.MakeStandardAction("Device", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "BasicData", dialogWidth: 800),
                this.MakeStandardAction("Device", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "BasicData"),
            };
        }


        protected override IEnumerable<IGridColumn<Device_View>> InitGridHeader()
        {
            return new List<GridColumn<Device_View>>{
                this.MakeGridHeader(x => x.DeviceName),
                this.MakeGridHeader(x => x.Index),
                //this.MakeGridHeader(x => x.Description),
                this.MakeGridHeader(x => x.DriverName_view),
                this.MakeGridHeader(x => x.AutoStart),
                this.MakeGridHeader(x => x.CgUpload),
                this.MakeGridHeader(x => x.EnforcePeriod),
                this.MakeGridHeader(x => x.DeviceTypeEnum),
                //this.MakeGridHeader(x => x.DeviceName_view),
                this.MakeGridHeader(x=>"copy").SetHide().SetFormat((a,b)=>{
                    if(a.DeviceTypeEnum== DeviceTypeEnum.Device)
                        return "true";
                     return "false";
                }),
                this.MakeGridHeader(x=>"attribute").SetHide().SetFormat((a,b)=>{
                    if(a.DeviceTypeEnum== DeviceTypeEnum.Device)
                        return "true";
                     return "false";
                }),
                this.MakeGridHeaderAction(width: 300)
            };
        }

        public override IOrderedQueryable<Device_View> GetSearchQuery()
        {
            var data = DC.Set<Device>().AsNoTracking().Where(x => x.DeviceTypeEnum == DeviceTypeEnum.Group).OrderBy(x => x.Index).ThenBy(x => x.DeviceName).ToList();

            var dataRet = new List<Device_View>();

            int order = 0;
            foreach (var x in data)
            {
                var itemF = new Device_View
                {
                    ID = x.ID,
                    Index = x.Index,
                    DeviceName = x.DeviceName,
                    Description = x.Description,
                    DeviceTypeEnum = x.DeviceTypeEnum,
                    DriverName_view = x.Driver?.DriverName,
                    ExtraOrder = order
                };
                dataRet.Add(itemF);
                order++;


                StringValues Ids = new();
                if (FC.ContainsKey("Ids[]"))
                {
                    Ids = (StringValues)FC["Ids[]"];
                }
                var childrens = DC.Set<Device>().AsNoTracking().Where(y => y.ParentId == x.ID).Include(x => x.Driver).OrderBy(x => x.Index).ThenBy(x => x.DeviceName).ToList();
                if (Ids.Count != 0)
                    childrens = childrens.Where(x => Ids.Contains(x.ID.ToString())).ToList();

                foreach (var y in childrens)
                {
                    var itemC = new Device_View
                    {
                        ID = y.ID,
                        Index = y.Index,
                        DeviceName = "&nbsp;&nbsp;&nbsp;&nbsp;" + y.DeviceName,
                        AutoStart = y.AutoStart,
                        CgUpload = y.CgUpload,
                        EnforcePeriod = y.EnforcePeriod,
                        Description = y.Description,
                        DeviceTypeEnum = y.DeviceTypeEnum,
                        DriverName_view = y.Driver?.DriverName,
                        DeviceName_view = itemF.DeviceName,
                        ExtraOrder = order
                    };
                    dataRet.Add(itemC);
                }
                order++;
            }

            return dataRet.AsQueryable<Device_View>().OrderBy(x => x.ExtraOrder);
        }

    }

    public class Device_View : Device
    {
        [Display(Name = "驱动名")]
        public String DriverName_view { get; set; }
        [Display(Name = "父级名")]
        public String DeviceName_view { get; set; }
        public int ExtraOrder { get; set; }
    }
}
