// WTM默认页面 Wtm buidin page
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkGroupVMs
{
    public class FrameworkGroupSearcher : BaseSearcher
    {
        [Display(Name = "_Admin.GroupCode")]
        public string GroupCode { get; set; }

        [Display(Name = "_Admin.GroupName")]
        public string GroupName { get; set; }
    }
}