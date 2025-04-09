using IoTGateway.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;

namespace IoTGateway.ViewModel.BasicData.DeviceVMs
{
    public partial class DeviceApiListVM : BasePagedListVM<DeviceApi_View, DeviceApiSearcher>
    {
        protected override IEnumerable<IGridColumn<DeviceApi_View>> InitGridHeader()
        {
            return new List<GridColumn<DeviceApi_View>>{
                this.MakeGridHeader(x => x.DeviceName),
                this.MakeGridHeader(x => x.Index),
                this.MakeGridHeader(x => x.Description),
                this.MakeGridHeader(x => x.DriverName_view),
                this.MakeGridHeader(x => x.AutoStart),
                this.MakeGridHeader(x => x.CgUpload),
                this.MakeGridHeader(x => x.EnforcePeriod),
                this.MakeGridHeader(x => x.CmdPeriod),
                this.MakeGridHeader(x => x.DeviceTypeEnum),
                this.MakeGridHeader(x => x.DeviceName_view),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<DeviceApi_View> GetSearchQuery()
        {
            var query = DC.Set<Device>()
                .CheckContain(Searcher.DeviceName, x => x.DeviceName)
                .CheckEqual(Searcher.DriverId, x => x.DriverId)
                .Select(x => new DeviceApi_View
                {
                    ID = x.ID,
                    DeviceName = x.DeviceName,
                    Index = x.Index,
                    Description = x.Description,
                    DriverName_view = x.Driver.DriverName,
                    AutoStart = x.AutoStart,
                    CgUpload = x.CgUpload,
                    EnforcePeriod = x.EnforcePeriod,
                    CmdPeriod = x.CmdPeriod,
                    DeviceTypeEnum = x.DeviceTypeEnum,
                    DeviceName_view = x.Parent.DeviceName,
                })
                .OrderBy(x => x.ID);
            return query;
        }
    }

    public class DeviceApi_View : Device
    {
        [Display(Name = "DriverName")]
        public String DriverName_view { get; set; }

        [Display(Name = "DeviceName")]
        public String DeviceName_view { get; set; }
    }
}