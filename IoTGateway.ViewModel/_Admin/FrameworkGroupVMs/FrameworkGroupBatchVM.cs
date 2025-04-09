// WTM默认页面 Wtm buidin page
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkGroupVMs
{
    public class FrameworkGroupBatchVM : BaseBatchVM<FrameworkGroup, BaseVM>
    {
        public FrameworkGroupBatchVM()
        {
            ListVM = new FrameworkGroupListVM();
        }
    }
}