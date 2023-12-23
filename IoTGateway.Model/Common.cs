using System.ComponentModel.DataAnnotations;

namespace IoTGateway.Model
{
    public enum DeviceTypeEnum
    {
        [Display(Name = "Collection Group")]
        Group = 0,

        [Display(Name = "Collection Device")]
        Device = 1
    }
    public enum AccessEnum
    {
        [Display(Name = "Read Only")]
        ReadOnly = 0,
        [Display(Name = "Read and write")]
        ReadAndWrite = 1
    }

    public enum DataSide
    {
        [Display(Name = "Shared Properties")]
        AnySide=0,
        //ServerSide=1,
        [Display(Name = "Client Properties")]
        ClientSide =2,
    }
}