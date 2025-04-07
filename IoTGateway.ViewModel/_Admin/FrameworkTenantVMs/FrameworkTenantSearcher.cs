using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace WalkingTec.Mvvm.Mvc.Admin.ViewModels.FrameworkTenantVMs
{
    public partial class FrameworkTenantSearcher : BaseSearcher
    {
        [Display(Name = "_Admin.TenantCode")]
        public String TCode { get; set; }

        [Display(Name = "_Admin.TenantName")]
        public String TName { get; set; }

        [Display(Name = "_Admin.TenantDomain")]
        public String TDomain { get; set; }

        protected override void InitVM()
        {
        }
    }
}