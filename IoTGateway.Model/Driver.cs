using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class Driver : BasePoco
    {
        [Display(Name = "驱动名")]
        public string DriverName { get; set; }
        [Display(Name = "文件名")]
        public string FileName { get; set; }
        [Display(Name = "程序集名")]
        public string AssembleName { get; set; }
        [Display(Name = "剩余授权数量")]
        public int AuthorizesNum { get; set; }
    }
}