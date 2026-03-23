using ModelContextProtocol.Server;
using Plugin;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Connect to a device.
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <returns>true if connection successful</returns>
        [McpServerTool, Description("Connect to a device.")]
        public bool ConnectDevice([Description("name of device")] string deviceName)
        {
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);
            if (deviceThread == null)
                return false;
            return deviceThread.Driver.Connect();
        }

        /// <summary>
        /// Disconnect from a device.
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <returns>true if disconnection successful</returns>
        [McpServerTool, Description("Disconnect from a device.")]
        public bool DisconnectDevice([Description("name of device")] string deviceName)
        {
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);
            if (deviceThread == null)
                return false;
            return deviceThread.Driver.Close();
        }

        /// <summary>
        /// Restart device connection (disconnect and reconnect).
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <returns>true if restart successful</returns>
        [McpServerTool, Description("Restart device connection.")]
        public bool RestartDevice([Description("name of device")] string deviceName)
        {
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);
            if (deviceThread == null)
                return false;
            deviceThread.Driver.Close();
            return deviceThread.Driver.Connect();
        }

        /// <summary>
        /// Get detailed information about a device.
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <returns>dictionary containing device information</returns>
        [McpServerTool, Description("Get detailed device information.")]
        public Dictionary<string, object> GetDeviceInfo([Description("name of device")] string deviceName)
        {
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);
            if (deviceThread == null)
                return new Dictionary<string, object> { { "error", "Device not found" } };

            var device = deviceThread.Device;
            var info = new Dictionary<string, object>
            {
                { "DeviceName", device.DeviceName },
                { "Description", device.Description ?? string.Empty },
                { "AutoStart", device.AutoStart },
                { "ChangeUpload", device.CgUpload },
                { "EnforcePeriod", device.EnforcePeriod },
                { "CmdPeriod", device.CmdPeriod },
                { "DeviceType", device.DeviceTypeEnum.ToString() },
                { "IsConnected", deviceThread.Driver.IsConnected },
                { "Timeout", deviceThread.Driver.Timeout },
                { "MinPeriod", deviceThread.Driver.MinPeriod },
                { "VariableCount", device.DeviceVariables?.Count ?? 0 },
                { "ConfigCount", device.DeviceConfigs?.Count ?? 0 }
            };

            // Add device configs
            if (device.DeviceConfigs != null)
            {
                var configs = new Dictionary<string, string>();
                foreach (var config in device.DeviceConfigs)
                {
                    configs[config.DeviceConfigName] = config.Value;
                }
                info["Configs"] = configs;
            }

            return info;
        }

        /// <summary>
        /// Get the driver information for a device.
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <returns>driver name</returns>
        [McpServerTool, Description("Get device driver information.")]
        public string GetDeviceDriver([Description("name of device")] string deviceName)
        {
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);
            if (deviceThread == null)
                return string.Empty;
            return deviceThread.Device.Driver?.DriverName ?? string.Empty;
        }

        /// <summary>
        /// Write value to a device variable.
        /// </summary>
        /// <param name="deviceName">name of device</param>
        /// <param name="variableName">name of variable</param>
        /// <param name="value">value to write</param>
        /// <returns>result of the write operation</returns>
        [McpServerTool, Description("Write value to a device variable.")]
        public async Task<Dictionary<string, object>> WriteDeviceVariableAsync(
            [Description("name of device")] string deviceName,
            [Description("name of variable")] string variableName,
            [Description("value to write")] object value)
        {
            var result = new Dictionary<string, object>();
            var deviceThread = _deviceService.DeviceThreads.FirstOrDefault(x => x.Device.DeviceName == deviceName);

            if (deviceThread == null)
            {
                result["success"] = false;
                result["error"] = "Device not found";
                return result;
            }

            var deviceVariable = deviceThread.Device.DeviceVariables?.FirstOrDefault(x => x.Name == variableName);
            if (deviceVariable == null)
            {
                result["success"] = false;
                result["error"] = "Variable not found";
                return result;
            }

            if (deviceVariable.ProtectType == ProtectTypeEnum.ReadOnly)
            {
                result["success"] = false;
                result["error"] = "Variable is read-only";
                return result;
            }

            // Ensure connected
            if (!deviceThread.Driver.IsConnected)
            {
                deviceThread.Driver.Connect();
            }

            if (!deviceThread.Driver.IsConnected)
            {
                result["success"] = false;
                result["error"] = "Cannot connect to device";
                return result;
            }

            var ioArgModel = new DriverAddressIoArgModel
            {
                Address = deviceVariable.DeviceAddress,
                Value = value,
                ValueType = deviceVariable.DataType,
                EndianType = deviceVariable.EndianType
            };

            var writeResponse = await deviceThread.Driver.WriteAsync(Guid.NewGuid().ToString(), deviceVariable.Method, ioArgModel);
            result["success"] = writeResponse.IsSuccess;
            result["description"] = writeResponse.Description;

            return result;
        }
    }
}