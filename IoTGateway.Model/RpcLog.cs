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
        [Display(Name = "Initiator")]
        public RpcSide RpcSide { get; set; }    

        [Display(Name = "Starting Time")]
        public DateTime StartTime { get; set; }

        public Device Device { get; set; }

        [Display(Name = "ID")]
        public Guid? DeviceId { get; set; }

        [Display(Name = "Method")]
        public string Method { get; set; }

        [Display(Name = "Request Parameters")]
        public string Params { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Result")]
        public bool IsSuccess { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public enum RpcSide
    {
        [Display(Name = "Server Request")]
        ServerSide=1,
        [Display(Name = "Client Request")]
        ClientSide =1
    }
}
