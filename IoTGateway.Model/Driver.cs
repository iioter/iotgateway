using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("驱动管理")]
    public class Driver : BasePoco
    {
        [Comment("驱动名")]
        [Display(Name = "DriverName")]
        public string DriverName { get; set; }

        [Comment("文件名")]
        [Display(Name = "FileName")]
        public string FileName { get; set; }

        [Comment("程序集名")]
        [Display(Name = "AssembleName")]
        public string AssembleName { get; set; }

        [Comment("剩余授权数")]
        [Display(Name = "Remains")]
        public int AuthorizesNum { get; set; }
    }
}