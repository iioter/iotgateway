using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Config.SystemConfigVMs
{
    public partial class SystemConfigListVM : BasePagedListVM<SystemConfig_View, SystemConfigSearcher>
    {
        protected override List<GridAction> InitGridAction()
        {
            return new List<GridAction>
            {
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.Create, Localizer["Sys.Create"],"Config", dialogWidth: 800),
                this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.Edit, Localizer["Sys.Edit"], "Config", dialogWidth: 800),
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.Delete, Localizer["Sys.Delete"], "Config", dialogWidth: 800),
                this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.Details, Localizer["Sys.Details"], "Config", dialogWidth: 800),
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.BatchEdit, Localizer["Sys.BatchEdit"], "Config", dialogWidth: 800),
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.BatchDelete, Localizer["Sys.BatchDelete"], "Config", dialogWidth: 800),
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.Import, Localizer["Sys.Import"], "Config", dialogWidth: 800),
                //this.MakeStandardAction("SystemConfig", GridActionStandardTypesEnum.ExportExcel, Localizer["Sys.Export"], "Config"),
            };
        }


        protected override IEnumerable<IGridColumn<SystemConfig_View>> InitGridHeader()
        {
            return new List<GridColumn<SystemConfig_View>>{
                this.MakeGridHeader(x => x.GatewayName),
                this.MakeGridHeader(x => x.IoTPlatformType),
                this.MakeGridHeader(x => x.MqttIp),
                this.MakeGridHeader(x => x.MqttPort),
                this.MakeGridHeader(x => x.MqttUName),
                this.MakeGridHeader(x => x.MqttUPwd),
                this.MakeGridHeaderAction(width: 200)
            };
        }

        public override IOrderedQueryable<SystemConfig_View> GetSearchQuery()
        {
            var query = DC.Set<SystemConfig>()
                .Select(x => new SystemConfig_View
                {
				    ID = x.ID,
                    GatewayName = x.GatewayName,
                    MqttIp = x.MqttIp,
                    MqttPort = x.MqttPort,
                    MqttUName = x.MqttUName,
                    MqttUPwd = x.MqttUPwd,
                    IoTPlatformType = x.IoTPlatformType,
                })
                .OrderBy(x => x.ID);
            return query;
        }

    }

    public class SystemConfig_View : SystemConfig{

    }
}
