// WTM默认页面 Wtm buidin page
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkRoleVMs
{
    public class FrameworkRoleSearcher : BaseSearcher
    {
        [Display(Name = "_Admin.RoleCode")]
        public string RoleCode { get; set; }

        [Display(Name = "_Admin.RoleName")]
        public string RoleName { get; set; }
    }
}