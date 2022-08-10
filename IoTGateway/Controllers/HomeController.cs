using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Plugin;
using PluginInterface;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Auth;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Mvc;

namespace IoTGateway.Controllers
{
    public class HomeController : BaseController
    {
        private readonly DeviceService _deviceService;
        public HomeController(DeviceService deviceService)
        {
            _deviceService = deviceService;
        }
        [AllRights]
        public IActionResult Index()
        {
            ViewData["title"] = "IoTGateway";
            return View();
        }

        [AllowAnonymous]
        public IActionResult PIndex()
        {
            return View();
        }

        [AllRights]
        [ActionDescription("FrontPage")]
        public IActionResult FrontPage()
        {
            return PartialView();
        }

        public IActionResult GetDeviceChart()
        {
            var data = new List<ChartData>();


            data.Add(new ChartData
            {
                Value = _deviceService.DeviceThreads.Where(x => !x.Device.AutoStart).Count(),
                Category = "停止",
                Series = "Device"
            });

            data.Add(new ChartData
            {
                Value = _deviceService.DeviceThreads.Where(x => x.Device.AutoStart && x.Driver.IsConnected).Count(),
                Category = "运行",
                Series = "Device",
            });

            data.Add(new ChartData
            {
                Value = _deviceService.DeviceThreads.Where(x => x.Device.AutoStart && !x.Driver.IsConnected).Count(),
                Category = "异常",
                Series = "Device"
            });
            var rv = data.ToChartData();
            return Json(rv);
        }

        public IActionResult GetDeviceVariableChart()
        {
            var data = new List<ChartData>();
            foreach (var deviceThread in _deviceService.DeviceThreads.OrderBy(x => x.Device.DeviceName))
            {
                data.Add(new ChartData
                {
                    Category = deviceThread.Device.DeviceName,
                    Value = deviceThread.DeviceValues.Where(x => x.Value.StatusType != VaribaleStatusTypeEnum.Good).Count(),
                    Series = "Others"
                });

                data.Add(new ChartData
                {
                    Category = deviceThread.Device.DeviceName,
                    Value = deviceThread.DeviceValues.Where(x => x.Value.StatusType == VaribaleStatusTypeEnum.Good).Count(),
                    Series = "Good"
                });

                
            }
            
            var rv = data.ToChartData();
            return Json(rv);
        }        

        public IActionResult GetActionChart()
        {
            var areas = GlobaInfo.AllModule.Select(x => x.Area).Distinct();
            var data = new List<ChartData>();

            foreach (var area in areas)
            {
                var controllers = GlobaInfo.AllModule.Where(x => x.Area == area);
                data.Add(new ChartData
                {
                    Category = "Controllers",
                    Value = controllers.Count(),
                    Series = area?.AreaName ?? "Default"
                });
                data.Add(new ChartData
                {
                    Category = "Actions",
                    Value = controllers.SelectMany(x=>x.Actions).Count(),
                    Series = area?.AreaName ?? "Default"
                });
            }
            var rv = data.ToChartData();
            return Json(rv);
        }

        public IActionResult GetModelChart()
        {
            var models = new List<Type>();

            var pros = Wtm.ConfigInfo.Connections.SelectMany(x => x.DcConstructor.DeclaringType.GetProperties(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance));
            if (pros != null)
            {
                foreach (var pro in pros)
                {
                    if (pro.PropertyType.IsGeneric(typeof(DbSet<>)))
                    {
                        models.Add(pro.PropertyType.GetGenericArguments()[0]);
                    }
                }
            }
            var data = new List<ChartData>();

            foreach (var m in models)
            {
                data.Add(new ChartData
                {
                    Value = m.GetProperties().Count(),
                    Category = m.GetPropertyDisplayName(),
                    Series = "Model"
                }) ;
            }
            var rv = data.ToChartData();
            return Json(rv);
        }

        public IActionResult GetSampleChart()
        {
            var data = new List<ChartData>();
            Random r = new Random();
            int maxi = r.Next(3, 10);
            int maxy = r.Next(3, 10);
            for (int i = 0; i < maxi; i++)
            {
                for (int j = 0; j < maxy; j++)
                {
                    data.Add(new ChartData
                    {
                        Category = "x" + i,
                        Value = r.Next(100, 1000),
                        ValueX = r.Next(200, 2000),
                        Series = "y" + j,
                        Addition = r.Next(100, 1000),

                    });
                }
            }
            var rv = data.ToChartData();
            return Json(rv);
        }


        [AllRights]
        [ActionDescription("Layout")]
        public IActionResult Layout()
        {
            ViewData["debug"] = Wtm.ConfigInfo.IsQuickDebug;
            return PartialView();
        }

        [AllRights]
        public IActionResult UserInfo()
        {
            if (HttpContext.Request.Cookies.TryGetValue(CookieAuthenticationDefaults.CookiePrefix + AuthConstants.CookieAuthName, out string cookieValue))
            {
                var protectedData = Base64UrlTextEncoder.Decode(cookieValue);
                var dataProtectionProvider = HttpContext.RequestServices.GetRequiredService<IDataProtectionProvider>();
                var _dataProtector = dataProtectionProvider
                                        .CreateProtector(
                                            "Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
                                            CookieAuthenticationDefaults.AuthenticationScheme,
                                            "v2");
                var unprotectedData = _dataProtector.Unprotect(protectedData);

                string cookieData = Encoding.UTF8.GetString(unprotectedData);
                return JsonMore(cookieData);
            }
            else
                return JsonMore("No Data");
        }

    }

}
