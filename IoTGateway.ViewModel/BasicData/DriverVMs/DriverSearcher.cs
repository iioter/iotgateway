using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using IoTGateway.Model;


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
