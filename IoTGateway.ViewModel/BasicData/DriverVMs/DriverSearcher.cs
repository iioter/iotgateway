using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.ViewModel.BasicData.DriverVMs
{
    public partial class DriverSearcher : BaseSearcher
    {
        [Display(Name = "驱动名")]
        public String DriverName { get; set; }

        protected override void InitVM()
        {
        }
    }
}