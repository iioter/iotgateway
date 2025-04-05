using ModelContextProtocol.Server;
using Plugin;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace IoTGateway.MCP
{
    [McpServerToolType]
    public sealed class DeviceTool
    {
        private readonly DeviceService _deviceService;
        public DeviceTool(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }
        /// <summary>
        /// Get the list of sub-devices.
        /// </summary>
        /// <returns></returns>
        [McpServerTool(Name = "DevicesList"), Description("Get the list of sub-devices.")]
        public IEnumerable<string> DevicesList()
        {
            return _deviceService.DeviceThreads.Select(x => x.Device.DeviceName);
        }

        /// <summary>
        /// Get the current connection status of the sub-device.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        [McpServerTool, Description("Get the current connection status of the sub-device.")]
        public bool? GetDeviceStatus(
            [Description("name of device")] string deviceName
            )
        {
            return _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName)?.Driver.IsConnected;
        }

        /// <summary>
        /// Get the current value of a variable of a sub-device.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [McpServerTool, Description("Get sub-device variables.")]
        [CanBeNull]
        public Dictionary<string, object> GetDeviceVariables(
            [Description("name of device")] string deviceName
            )
        {
            return _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName)?.Device
                .DeviceVariables.ToDictionary(x => x.Name, x => x.CookedValue);
        }

        /// <summary>
        /// Get the current value of a variable of a sub-device.
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        [McpServerTool, Description("Get the current value of a variable of a sub-device.")]
        public object GetDeviceVariable(
            [Description("name of device")] string deviceName,
            [Description("name of variable")] string variableName)
        {
            return _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName)?.Device
                .DeviceVariables.FirstOrDefault(x => x.Name == variableName)?.CookedValue;
        }
    }

}
