using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    public class RpcLog:TopBasePoco
    {
        [Display(Name = "RpcSide")]
        public RpcSide RpcSide { get; set; }    

        [Display(Name = "StartTime")]
        public DateTime StartTime { get; set; }

        public Device Device { get; set; }

        [Display(Name = "Device")]
        public Guid? DeviceId { get; set; }

        [Display(Name = "Method")]
        public string Method { get; set; }

        [Display(Name = "Parameters")]
        public string Params { get; set; }

        [Display(Name = "EndTime")]
        public DateTime EndTime { get; set; }

        [Display(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public enum RpcSide
    {
        [Display(Name = "ServerSide")]
        ServerSide=1,
        [Display(Name = "ClientSide")]
        ClientSide =1
    }
}
