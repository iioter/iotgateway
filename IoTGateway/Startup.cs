using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using Plugin;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using WalkingTec.Mvvm.Core.Support.FileHandlers;
using WalkingTec.Mvvm.Mvc;

namespace IoTGateway
{
    public class Startup
    {
        public IConfiguration ConfigRoot { get; }

        public Startup(IWebHostEnvironment env, IConfiguration config)
        {
            ConfigRoot = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddWtmSession(360000, ConfigRoot);
            services.AddWtmCrossDomain(ConfigRoot);
            services.AddWtmAuthentication(ConfigRoot);
            services.AddWtmHttpClient(ConfigRoot);
            services.AddWtmSwagger();
            services.AddWtmMultiLanguages(ConfigRoot);

            services.AddMvc(options =>
            {
                options.UseWtmMvcOptions();
            })
            .AddJsonOptions(options =>
            {
                options.UseWtmJsonOptions();
            })

            .ConfigureApiBehaviorOptions(options =>
            {
                options.UseWtmApiOptions();
            })
            .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
            .AddWtmDataAnnotationsLocalization(typeof(Program));

            services.AddWtmContext(ConfigRoot, (options) =>
            {
                options.DataPrivileges = DataPrivilegeSettings();
                options.CsSelector = CSSelector;
                options.FileSubDirSelector = SubDirSelector;
                options.ReloadUserFunc = ReloadUser;
            });

            //MQTTServer
            services.AddHostedMqttServer(mqttServer =>
            {
                mqttServer.WithoutDefaultEndpoint();
            })
                .AddMqttConnectionHandler()
                .AddConnections();


            services.AddHostedService<IoTBackgroundService>();
            services.AddSingleton<DeviceService>();
            services.AddSingleton<DriverService>();
            services.AddSingleton<UAService>();
            services.AddSingleton<MyMqttClient>();
            services.AddSingleton<ModbusSlaveService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IOptionsMonitor<Configs> configs, DeviceService deviceService, ModbusSlaveService modbusSlaveService)
        {
            IconFontsHelper.GenerateIconFont();

            var pvd = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString(""),
                //设置不限制content-type 该设置可以下载所有类型的文件，但是不建议这么设置，因为不安全
                //下面设置可以下载apk和nupkg类型的文件
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    { ".html", "text/html" },
                    { ".glb", "model/gltf-binary" },
                    { ".json", " application/json" },
                    { ".js", "application/javascript" },
                    { ".css", "text/css" },
                    { ".wasm", "application/wasm" },
                    { ".png", "image/png" },
                    { ".jpg", "image/jpg" },
                    { ".woff", "application/font-woff" },
                    { ".woff2", "application/font-woff" },
                    { ".ico", "image/x-icon" },
                })
            };

            app.UseExceptionHandler(configs.CurrentValue.ErrorHandler);
            app.UseStaticFiles(pvd);
            app.UseWtmStaticFiles();
            app.UseRouting();
            app.UseWtmMultiLanguages();
            app.UseWtmCrossDomain();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseWtmSwagger();
            app.UseWtm();

            app.UseEndpoints(endpoints =>
            {
                //MqttServerWebSocket
                endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt", options =>
                {
                    options.WebSockets.SubProtocolSelector = MqttSubProtocolSelector.SelectSubProtocol;
                });

                endpoints.MapControllerRoute(
                   name: "areaRoute",
                   pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseWtmContext();


        }

        /// <summary>
        /// Wtm will call this function to dynamiclly set connection string
        /// 框架会调用这个函数来动态设定每次访问需要链接的数据库
        /// </summary>
        /// <param name="context">ActionContext</param>
        /// <returns>Connection string key name</returns>
        public string CSSelector(ActionExecutingContext context)
        {
            //To override the default logic of choosing connection string,
            //change this function to return different connection string key
            //根据context返回不同的连接字符串的名称
            return null;
        }

        /// <summary>
        /// Set data privileges that system supports
        /// 设置系统支持的数据权限
        /// </summary>
        /// <returns>data privileges list</returns>
        public List<IDataPrivilege> DataPrivilegeSettings()
        {
            List<IDataPrivilege> pris = new List<IDataPrivilege>();
            //Add data privilege to specific type
            //指定哪些模型需要数据权限
            return pris;
        }

        /// <summary>
        /// Set sub directory of uploaded files
        /// 动态设置上传文件的子目录
        /// </summary>
        /// <param name="fh">IWtmFileHandler</param>
        /// <returns>subdir name</returns>
        public string SubDirSelector(IWtmFileHandler fh)
        {
            return null;
        }

        /// <summary>
        /// Custom Reload user process when cache is not available
        /// 设置自定义的方法重新读取用户信息，这个方法会在用户缓存失效的时候调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public LoginUserInfo ReloadUser(WTMContext context, string account)
        {
            return null;
        }
    }
}
