using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalkingTec.Mvvm.Core
{
    [Table("FrameworkUserGroups")]
    public class FrameworkUserGroup : BasePoco, ITenant
    {
        [Required]
        [StringLength(50, ErrorMessage = "Validate.{0}stringmax{1}")]
        public string UserCode { get; set; }

        [Display(Name = "_Admin.Group")]
        [Required]
        [StringLength(50, ErrorMessage = "Validate.{0}stringmax{1}")]
        public string GroupCode { get; set; }

        [Display(Name = "_Admin.Tenant")]
        [StringLength(50, ErrorMessage = "Validate.{0}stringmax{1}")]
        public string TenantCode { get; set; }
    }
}