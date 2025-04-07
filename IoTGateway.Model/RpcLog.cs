using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;

namespace IoTGateway.Model
{
    [Comment("RPC日志")]
    public class RpcLog : TopBasePoco
    {
        [Comment("发起方")]
        [Display(Name = "RpcSide")]
        public RpcSide RpcSide { get; set; }

        [Comment("开始时间")]
        [Display(Name = "StartTime")]
        public DateTime StartTime { get; set; }

        public Device Device { get; set; }

        [Comment("所属设备")]
        [Display(Name = "Device")]
        public Guid? DeviceId { get; set; }

        [Comment("方法")]
        [Display(Name = "Method")]
        public string Method { get; set; }

        [Comment("请求参数")]
        [Display(Name = "Parameters")]
        public string Params { get; set; }

        [Comment("结束时间")]
        [Display(Name = "EndTime")]
        public DateTime EndTime { get; set; }

        [Comment("是否成功")]
        [Display(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }

        [Comment("描述")]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }

    public enum RpcSide
    {
        [Display(Name = "ServerSide")]
        ServerSide = 1,

        [Display(Name = "ClientSide")]
        ClientSide = 1
    }
}