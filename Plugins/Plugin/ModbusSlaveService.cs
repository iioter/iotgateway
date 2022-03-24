using Microsoft.Extensions.Logging;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    public class ModbusSlaveService : IDisposable
    {
        private readonly ILogger<ModbusSlaveService> _logger;
        TcpListener slaveTcpListener;
        private Timer m_simulationTimer;
        private object Lock=new object();
        private ModbusSlave slave;
        private Task task { get; set; } = null;
        public ModbusSlaveService(ILogger<ModbusSlaveService> logger)
        {
            _logger = logger;
            byte slaveId = 1;
            int port = 503;
            IPAddress address = IPAddress.Any;

            // create and start the TCP slave
            slaveTcpListener = new TcpListener(address, port);
            slaveTcpListener.Start();
            slave = ModbusTcpSlave.CreateTcp(slaveId, slaveTcpListener);
            slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
            slave.ListenAsync();
            m_simulationTimer = new Timer(DoSimulation, null, 1000, 1000);
            _logger.LogInformation($"Modbus Server Started");
        }

        private void DoSimulation(object state)
        {
            try
            {
                lock (Lock)
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        if(i != 1|| i != 2|| i != 7)
                            slave.DataStore.HoldingRegisters[i] = (ushort)new Random().Next(0, short.MaxValue);
                        slave.DataStore.InputRegisters[i] = (ushort)new Random().Next(0, short.MaxValue);
                        slave.DataStore.CoilDiscretes[i] = new Random().Next() % 2 == 0;
                        slave.DataStore.InputDiscretes[i] = new Random().Next() % 2 == 0;
                    }
                    slave.DataStore.HoldingRegisters[1] = (ushort)new Random().Next(2000,3000); //前端要用的温度
                    slave.DataStore.HoldingRegisters[2] = (ushort)new Random().Next(4000, 7000);//湿度
                    slave.DataStore.HoldingRegisters[7] = (ushort)new Random().Next(0, 10000);//随机值
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Modbus Server Error", ex);
            }
        }
        public void Dispose()
        {
            _logger.LogError($"Modbus Server Dispose");
            m_simulationTimer.Dispose();
            slaveTcpListener.Stop();
        }
    }
}
