using System.ComponentModel.DataAnnotations;

namespace IoTGateway.Model
{
    public enum DeviceTypeEnum
    {
        [Display(Name = "Group")]
        Group = 0,

        [Display(Name = "Device")]
        Device = 1
    }
    public enum AccessEnum
    {
        [Display(Name = "ReadOnly")]
        ReadOnly = 0,
        [Display(Name = "ReadWrite")]
        ReadAndWrite = 1
    }

    public enum DataSide
    {
        [Display(Name = "AnySide")]
        AnySide=0,
        //ServerSide=1,
        [Display(Name = "ClientSide")]
        ClientSide =2,
    }
}