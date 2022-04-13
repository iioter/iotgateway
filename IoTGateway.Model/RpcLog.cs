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
        [Display(Name = "发起方")]
        public RpcSide RpcSide { get; set; }    

        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        public Device Device { get; set; }

        [Display(Name = "设备名")]
        public Guid? DeviceId { get; set; }

        [Display(Name = "方法名")]
        public string Method { get; set; }

        [Display(Name = "请求参数")]
        public string Params { get; set; }

        [Display(Name = "结束时间")]
        public DateTime EndTime { get; set; }

        [Display(Name = "结果")]
        public bool IsSuccess { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }
    }

    public enum RpcSide
    {
        [Display(Name ="服务端请求")]
        ServerSide=1,
        [Display(Name = "客户端请求")]
        ClientSide =1
    }
}
