using IoTGateway.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WalkingTec.Mvvm.Core;

namespace Plugin
{
    public class IoTBackgroundService : BackgroundService
    {
        public static DBTypeEnum DbType;
        public static string connnectSetting;
        public static Guid? VariableSelectDeviceId, ConfigSelectDeviceId;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly IServiceProvider _serviceProvider;

        public IoTBackgroundService(IConfiguration configRoot, IHostApplicationLifetime appLifeTime, IServiceProvider serviceProvider)
        {
            _appLifeTime = appLifeTime;
            _serviceProvider = serviceProvider;

            var connnectSettings = new List<ConnnectSettingsModel>();
            configRoot.Bind("Connections", connnectSettings);
            connnectSetting = connnectSettings[0].Value;

            switch (connnectSettings[0].DbType?.Trim().ToLower())
            {
                case "oracle":
                    DbType = DBTypeEnum.Oracle;
                    break;
                case "mysql":
                    DbType = DBTypeEnum.MySql;
                    break;
                case "pgsql":
                    DbType = DBTypeEnum.PgSql;
                    break;
                case "sqlite":
                    DbType = DBTypeEnum.SQLite;
                    break;
                case "memory":
                    DbType = DBTypeEnum.Memory;
                    break;
            }

            if (DbType == DBTypeEnum.SQLite)
            {
                using var dc = new DataContext(connnectSetting, DbType);
                if (dc.Database.GetPendingMigrations().Any())
                {
                    dc.Database.Migrate();
                }
            }


        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifeTime.ApplicationStarted.Register(OnStarted);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private void OnStarted()
        {
            _ = _serviceProvider.GetRequiredService<DeviceService>();
            _ = _serviceProvider.GetRequiredService<ModbusSlaveService>();
        }
    }
}
