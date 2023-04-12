using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using Modbus.Data;
using Modbus.Device;

namespace Plugin
{
    public class ModbusSlaveService : IDisposable
    {
        private readonly ILogger<ModbusSlaveService> _logger;
        readonly TcpListener _slaveTcpListener;
        private readonly Timer _mSimulationTimer;
        private readonly object _lock = new();
        private readonly ModbusSlave _slave;
        private Task _task;
        public ModbusSlaveService(ILogger<ModbusSlaveService> logger)
        {
            _logger = logger;
            byte slaveId = 1;
            int port = 503;
            IPAddress address = IPAddress.Any;

            // create and start the TCP slave
            _slaveTcpListener = new TcpListener(address, port);
            _slaveTcpListener.Start();
            _slave = ModbusTcpSlave.CreateTcp(slaveId, _slaveTcpListener);
            _slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
            _slave.ListenAsync();
            _mSimulationTimer = new Timer(DoSimulation, null, 1000, 1000);
            _logger.LogInformation($"Modbus Server Started");
        }

        private void DoSimulation(object state)
        {
            try
            {
                lock (_lock)
                {
                    for (int i = 1; i <= 20; i++)
                    {
                        if (i != 1 || i != 2 || i != 7)
                            _slave.DataStore.HoldingRegisters[i] = (ushort)new Random().Next(0, short.MaxValue);
                        _slave.DataStore.InputRegisters[i] = (ushort)new Random().Next(0, short.MaxValue);
                        _slave.DataStore.CoilDiscretes[i] = new Random().Next() % 2 == 0;
                        _slave.DataStore.InputDiscretes[i] = new Random().Next() % 2 == 0;
                    }
                    _slave.DataStore.HoldingRegisters[1] = (ushort)new Random().Next(2000, 3000); //前端要用的温度
                    _slave.DataStore.HoldingRegisters[2] = (ushort)new Random().Next(4000, 7000);//湿度
                    _slave.DataStore.HoldingRegisters[7] = (ushort)new Random().Next(0, 10000);//随机值
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
            _mSimulationTimer.Dispose();
            _slaveTcpListener.Stop();
        }
    }
}
