using IoTGateway.ViewModel.MqttClient.MqttServerVMs;
using Microsoft.AspNetCore.Mvc;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace IoTGateway.Controllers
{
    [Area("MqttServer")]
    [ActionDescription("客户端状态")]
    public partial class MqttClientController : BaseController
    {
        #region Search

        [ActionDescription("Sys.Search")]
        public ActionResult Index()
        {
            var vm = Wtm.CreateVM<MqttClientListVM>();
            return PartialView(vm);
        }

        [ActionDescription("Sys.Search")]
        [HttpPost]
        public string Search(MqttClientSearcher searcher)
        {
            var vm = Wtm.CreateVM<MqttClientListVM>(passInit: true);
            if (ModelState.IsValid)
            {
                vm.Searcher = searcher;
                return vm.GetJson(false);
            }
            else
            {
                return vm.GetError();
            }
        }

        #endregion Search
    }
}