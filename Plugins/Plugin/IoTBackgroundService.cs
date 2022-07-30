using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<IoTBackgroundService> _logger;
        public static DBTypeEnum DBType;
        public static string connnectSetting;
        public static Guid? VariableSelectDeviceId, ConfigSelectDeviceId;
        public IoTBackgroundService(IConfiguration ConfigRoot, ILogger<IoTBackgroundService> logger)
        {
            _logger= logger;
            var connnectSettings = new List<ConnnectSettingsModel>();
            ConfigRoot.Bind("Connections", connnectSettings);
            connnectSetting = connnectSettings[0].Value;
            DBType = GetDBType(connnectSettings[0].DBType);
            //read from Environment
            var IoTGateway_DB = Environment.GetEnvironmentVariable("IoTGateway_DB");
            var IoTGateway_DBType = Environment.GetEnvironmentVariable("IoTGateway_DBType");
            if (!string.IsNullOrEmpty(IoTGateway_DB) && !string.IsNullOrEmpty(IoTGateway_DBType))
            {
                connnectSetting = IoTGateway_DB;
                DBType = GetDBType(IoTGateway_DBType);
            }
            _logger.LogInformation($"IoTBackgroundService connnectSetting:{connnectSetting},DBType:{DBType}");
        }

        private DBTypeEnum GetDBType(string dbtypeStr)
        {
            DBTypeEnum dbType = DBTypeEnum.SQLite;
            switch (dbtypeStr.Trim().ToLower())
            {
                case "oracle":
                    dbType = DBTypeEnum.Oracle;
                    break;
                case "mysql":
                    dbType = DBTypeEnum.MySql;
                    break;
                case "pgsql":
                    dbType = DBTypeEnum.PgSql;
                    break;
                case "sqlite":
                    dbType = DBTypeEnum.SQLite;
                    break;
                case "memory":
                    dbType = DBTypeEnum.Memory;
                    break;
                default:
                    break;
            }
            return dbType;
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
