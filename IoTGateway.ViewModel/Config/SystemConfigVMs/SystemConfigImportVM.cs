using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


namespace IoTGateway.ViewModel.Config.SystemConfigVMs
{
    public partial class SystemConfigTemplateVM : BaseTemplateVM
    {
        [Display(Name = "网关名称")]
        public ExcelPropety GatewayName_Excel = ExcelPropety.CreateProperty<SystemConfig>(x => x.GatewayName);
        [Display(Name = "Mqtt服务器")]
        public ExcelPropety MqttIp_Excel = ExcelPropety.CreateProperty<SystemConfig>(x => x.MqttIp);
        [Display(Name = "Mqtt端口")]
        public ExcelPropety MqttPort_Excel = ExcelPropety.CreateProperty<SystemConfig>(x => x.MqttPort);
        [Display(Name = "Mqtt用户名")]
        public ExcelPropety MqttUName_Excel = ExcelPropety.CreateProperty<SystemConfig>(x => x.MqttUName);
        [Display(Name = "Mqtt密码")]
        public ExcelPropety MqttUPwd_Excel = ExcelPropety.CreateProperty<SystemConfig>(x => x.MqttUPwd);

	    protected override void InitVM()
        {
        }

    }

    public class SystemConfigImportVM : BaseImportVM<SystemConfigTemplateVM, SystemConfig>
    {

    }

}
