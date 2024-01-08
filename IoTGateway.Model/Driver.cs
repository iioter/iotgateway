using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class Driver : BasePoco
    {
        [Display(Name = "Name")]
        public string DriverName { get; set; }
        [Display(Name = "FileName")]
        public string FileName { get; set; }
        [Display(Name = "AssembleName")]
        public string AssembleName { get; set; }
        [Display(Name = "Number of licenses remaining")]
        public int AuthorizesNum { get; set; }
    }
}