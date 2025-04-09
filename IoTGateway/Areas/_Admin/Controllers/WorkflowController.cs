// WTM默认页面 Wtm buidin page
using Microsoft.AspNetCore.Mvc;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.Controllers
{
    [Area("_Admin")]
    [ActionDescription("MenuKey.Workflow")]
    public class WorkflowController : BaseController
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}