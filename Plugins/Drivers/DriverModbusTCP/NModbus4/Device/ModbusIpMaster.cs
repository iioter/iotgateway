namespace Modbus.Device
{
    using System;
    using System.Diagnostics.CodeAnalysis;
#if SERIAL
    using System.IO.Ports;
#endif
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using IO;

    /// <summary>
    ///    Modbus IP master device.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
    public class ModbusIpMaster : ModbusMaster
    {
        /// <summary>
        ///     Modbus IP master device.
        /// </summary>
        /// <param name="transport">Transport used by this master.</param>
        private ModbusIpMaster(ModbusTransport transport)
            : base(transport)
        {
        }

        /// <summary>
        ///    Modbus IP master factory method.
        /// </summary>
        /// <returns>New instance of Modbus IP master device using provided TCP client.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
        public static ModbusIpMaster CreateIp(TcpClient tcpClient)
        {
            if (tcpClient == null)
            {
                throw new ArgumentNullException(nameof(tcpClient));
            }

            return CreateIp(new TcpClientAdapter(tcpClient));
        }

        /// <summary>
        ///    Modbus IP master factory method.
        /// </summary>
        /// <returns>New instance of Modbus IP master device using provided UDP client.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
        public static ModbusIpMaster CreateIp(UdpClient udpClient)
        {
            if (udpClient == null)
            {
                throw new ArgumentNullException(nameof(udpClient));
            }

            if (!udpClient.Client.Connected)
            {
                throw new InvalidOperationException(Resources.UdpClientNotConnected);
            }

            return CreateIp(new UdpClientAdapter(udpClient));
        }

#if SERIAL
        /// <summary>
        ///     Modbus IP master factory method.
        /// </summary>
        /// <returns>New instance of Modbus IP master device using provided serial port.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
        public static ModbusIpMaster CreateIp(SerialPort serialPort)
        {
            if (serialPort == null)
            {
                throw new ArgumentNullException(nameof(serialPort));
            }

            return CreateIp(new SerialPortAdapter(serialPort));
        }
#endif

        /// <summary>
        ///     Modbus IP master factory method.
        /// </summary>
        /// <returns>New instance of Modbus IP master device using provided stream resource.</returns>
        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Breaking change.")]
        public static ModbusIpMaster CreateIp(IStreamResource streamResource)
        {
            if (streamResource == null)
            {
                throw new ArgumentNullException(nameof(streamResource));
            }

            return new ModbusIpMaster(new ModbusIpTransport(streamResource));
        }

        /// <summary>
        ///    Reads from 1 to 2000 contiguous coils status.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of coils to read.</param>
        /// <returns>Coils status.</returns>
        public bool[] ReadCoils(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadCoils(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Asynchronously reads from 1 to 2000 contiguous coils status.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of coils to read.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        public Task<bool[]> ReadCoilsAsync(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadCoilsAsync(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Reads from 1 to 2000 contiguous discrete input status.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of discrete inputs to read.</param>
        /// <returns>Discrete inputs status.</returns>
        public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadInputs(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Asynchronously reads from 1 to 2000 contiguous discrete input status.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of discrete inputs to read.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        public Task<bool[]> ReadInputsAsync(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadInputsAsync(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Reads contiguous block of holding registers.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>Holding registers status.</returns>
        public ushort[] ReadHoldingRegisters(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadHoldingRegisters(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Asynchronously reads contiguous block of holding registers.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        public Task<ushort[]> ReadHoldingRegistersAsync(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadHoldingRegistersAsync(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Reads contiguous block of input registers.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>Input registers status.</returns>
        public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadInputRegisters(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Asynchronously reads contiguous block of input registers.
        /// </summary>
        /// <param name="startAddress">Address to begin reading.</param>
        /// <param name="numberOfPoints">Number of holding registers to read.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        public Task<ushort[]> ReadInputRegistersAsync(ushort startAddress, ushort numberOfPoints)
        {
            return base.ReadInputRegistersAsync(Modbus.DefaultIpSlaveUnitId, startAddress, numberOfPoints);
        }

        /// <summary>
        ///    Writes a single coil value.
        /// </summary>
        /// <param name="coilAddress">Address to write value to.</param>
        /// <param name="value">Value to write.</param>
        public void WriteSingleCoil(ushort coilAddress, bool value)
        {
            base.WriteSingleCoil(Modbus.DefaultIpSlaveUnitId, coilAddress, value);
        }

        /// <summary>
        ///    Asynchronously writes a single coil value.
        /// </summary>
        /// <param name="coilAddress">Address to write value to.</param>
        /// <param name="value">Value to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public Task WriteSingleCoilAsync(ushort coilAddress, bool value)
        {
            return base.WriteSingleCoilAsync(Modbus.DefaultIpSlaveUnitId, coilAddress, value);
        }

        /// <summary>
        ///     Write a single holding register.
        /// </summary>
        /// <param name="registerAddress">Address to write.</param>
        /// <param name="value">Value to write.</param>
        public void WriteSingleRegister(ushort registerAddress, ushort value)
        {
            base.WriteSingleRegister(Modbus.DefaultIpSlaveUnitId, registerAddress, value);
        }

        /// <summary>
        ///    Asynchronously writes a single holding register.
        /// </summary>
        /// <param name="registerAddress">Address to write.</param>
        /// <param name="value">Value to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public Task WriteSingleRegisterAsync(ushort registerAddress, ushort value)
        {
            return base.WriteSingleRegisterAsync(Modbus.DefaultIpSlaveUnitId, registerAddress, value);
        }

        /// <summary>
        ///     Write a block of 1 to 123 contiguous registers.
        /// </summary>
        /// <param name="startAddress">Address to begin writing values.</param>
        /// <param name="data">Values to write.</param>
        public void WriteMultipleRegisters(ushort startAddress, ushort[] data)
        {
            base.WriteMultipleRegisters(Modbus.DefaultIpSlaveUnitId, startAddress, data);
        }

        /// <summary>
        ///    Asynchronously writes a block of 1 to 123 contiguous registers.
        /// </summary>
        /// <param name="startAddress">Address to begin writing values.</param>
        /// <param name="data">Values to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public Task WriteMultipleRegistersAsync(ushort startAddress, ushort[] data)
        {
            return base.WriteMultipleRegistersAsync(Modbus.DefaultIpSlaveUnitId, startAddress, data);
        }

        /// <summary>
        ///     Force each coil in a sequence of coils to a provided value.
        /// </summary>
        /// <param name="startAddress">Address to begin writing values.</param>
        /// <param name="data">Values to write.</param>
        public void WriteMultipleCoils(ushort startAddress, bool[] data)
        {
            base.WriteMultipleCoils(Modbus.DefaultIpSlaveUnitId, startAddress, data);
        }

        /// <summary>
        ///    Asynchronously writes a sequence of coils.
        /// </summary>
        /// <param name="startAddress">Address to begin writing values.</param>
        /// <param name="data">Values to write.</param>
        /// <returns>A task that represents the asynchronous write operation</returns>
        public Task WriteMultipleCoilsAsync(ushort startAddress, bool[] data)
        {
            return base.WriteMultipleCoilsAsync(Modbus.DefaultIpSlaveUnitId, startAddress, data);
        }

        /// <summary>
        ///     Performs a combination of one read operation and one write operation in a single MODBUS transaction.
        ///     The write operation is performed before the read.
        ///     Message uses default TCP slave id of 0.
        /// </summary>
        /// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
        /// <param name="numberOfPointsToRead">Number of registers to read.</param>
        /// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
        /// <param name="writeData">Register values to write.</param>
        public ushort[] ReadWriteMultipleRegisters(
            ushort startReadAddress,
            ushort numberOfPointsToRead,
            ushort startWriteAddress,
            ushort[] writeData)
        {
            return base.ReadWriteMultipleRegisters(
                Modbus.DefaultIpSlaveUnitId,
                startReadAddress,
                numberOfPointsToRead,
                startWriteAddress,
                writeData);
        }

        /// <summary>
        ///    Asynchronously performs a combination of one read operation and one write operation in a single Modbus transaction.
        ///    The write operation is performed before the read.
        /// </summary>
        /// <param name="startReadAddress">Address to begin reading (Holding registers are addressed starting at 0).</param>
        /// <param name="numberOfPointsToRead">Number of registers to read.</param>
        /// <param name="startWriteAddress">Address to begin writing (Holding registers are addressed starting at 0).</param>
        /// <param name="writeData">Register values to write.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<ushort[]> ReadWriteMultipleRegistersAsync(
            ushort startReadAddress,
            ushort numberOfPointsToRead,
            ushort startWriteAddress,
            ushort[] writeData)
        {
            return base.ReadWriteMultipleRegistersAsync(
                Modbus.DefaultIpSlaveUnitId,
                startReadAddress,
                numberOfPointsToRead,
                startWriteAddress,
                writeData);
        }
    }
}
