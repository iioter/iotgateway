using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WalkingTec.Mvvm.Core;

namespace Plugin
{
    public class IoTBackgroundService : BackgroundService
    {
        public static DBTypeEnum DBType;
        public static string connnectSetting;
        public static Guid? VariableSelectDeviceId, ConfigSelectDeviceId;
        public IoTBackgroundService(IConfiguration ConfigRoot)
        {
            var connnectSettings = new List<ConnnectSettingsModel>();
            ConfigRoot.Bind("Connections", connnectSettings);
            connnectSetting = connnectSettings[0].Value;

            switch (connnectSettings[0].DBType.Trim().ToLower())
            {
                case "oracle":
                    DBType = DBTypeEnum.Oracle;
                    break;
                case "mysql":
                    DBType = DBTypeEnum.MySql;
                    break;
                case "pgsql":
                    DBType = DBTypeEnum.PgSql;
                    break;
                case "sqlite":
                    DBType = DBTypeEnum.SQLite;
                    break;
                case "memory":
                    DBType = DBTypeEnum.Memory;
                    break;
                default:
                    break;
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
