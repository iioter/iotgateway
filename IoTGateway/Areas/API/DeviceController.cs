using System.Linq;
using System.Threading.Tasks;
using IoTGateway.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plugin;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Mvc;

namespace IoTGateway.Areas.API
{
    /// <summary>
    /// 设备和数据查询api
    /// </summary>
    [Area("API")]
    [ActionDescription("MenuKey.ActionLog")]
    public class DeviceController : BaseController
    {
        private readonly ILogger<DeviceController> _logger;
        private readonly DeviceService _deviceService;
        public DeviceController(ILogger<DeviceController>  logger, DeviceService deviceService)
        {
            _logger = logger;
            _deviceService = deviceService;

        }
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        [Public]
        [HttpGet("Device/GetDevices")]
        public async Task<IActionResult>  GetDevices()
        {
            return Ok(await DC.Set<Device>().Include(x => x.Driver).Where(x => x.ParentId != null).AsNoTracking()
                .OrderBy(x => x.Index).ToListAsync());

        }
    }
}
