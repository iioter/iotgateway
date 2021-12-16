namespace Modbus.Device
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using IO;
    using Message;

    /// <summary>
    ///     Modbus slave device.
    /// </summary>
    public abstract class ModbusSlave : ModbusDevice
    {
        internal ModbusSlave(byte unitId, ModbusTransport transport)
            : base(transport)
        {
            DataStore = DataStoreFactory.CreateDefaultDataStore();
            UnitId = unitId;
        }

        /// <summary>
        ///     Raised when a Modbus slave receives a request, before processing request function.
        /// </summary>
        /// <exception cref="InvalidModbusRequestException">The Modbus request was invalid, and an error response the specified exception should be sent.</exception>
        public event EventHandler<ModbusSlaveRequestEventArgs> ModbusSlaveRequestReceived;

        /// <summary>
        ///     Raised when a Modbus slave receives a write request, after processing the write portion of the function.
        /// </summary>
        /// <remarks>For Read/Write Multiple registers (function code 23), this method is raised after writing and before reading.</remarks>
        public event EventHandler<ModbusSlaveRequestEventArgs> WriteComplete;

        /// <summary>
        ///     Gets or sets the data store.
        /// </summary>
        public DataStore DataStore { get; set; }

        /// <summary>
        ///     Gets or sets the unit ID.
        /// </summary>
        public byte UnitId { get; set; }

        /// <summary>
        ///     Start slave listening for requests.
        /// </summary>
        public abstract Task ListenAsync();

        internal static ReadCoilsInputsResponse ReadDiscretes(
            ReadCoilsInputsRequest request,
            DataStore dataStore,
            ModbusDataCollection<bool> dataSource)
        {
            DiscreteCollection data;
            ReadCoilsInputsResponse response;

            data = DataStore.ReadData<DiscreteCollection, bool>(
                dataStore,
                dataSource,
                request.StartAddress,
                request.NumberOfPoints,
                dataStore.SyncRoot);

            response = new ReadCoilsInputsResponse(
                request.FunctionCode,
                request.SlaveAddress,
                data.ByteCount,
                data);

            return response;
        }

        internal static ReadHoldingInputRegistersResponse ReadRegisters(
            ReadHoldingInputRegistersRequest request,
            DataStore dataStore,
            ModbusDataCollection<ushort> dataSource)
        {
            RegisterCollection data;
            ReadHoldingInputRegistersResponse response;

            data = DataStore.ReadData<RegisterCollection, ushort>(
                dataStore,
                dataSource,
                request.StartAddress,
                request.NumberOfPoints,
                dataStore.SyncRoot);

            response = new ReadHoldingInputRegistersResponse(
                request.FunctionCode,
                request.SlaveAddress,
                data);

            return response;
        }

        internal static WriteSingleCoilRequestResponse WriteSingleCoil(
            WriteSingleCoilRequestResponse request,
            DataStore dataStore,
            ModbusDataCollection<bool> dataSource)
        {
            DataStore.WriteData(
                dataStore,
                new DiscreteCollection(request.Data[0] == Modbus.CoilOn),
                dataSource,
                request.StartAddress,
                dataStore.SyncRoot);

            return request;
        }

        internal static WriteMultipleCoilsResponse WriteMultipleCoils(
            WriteMultipleCoilsRequest request,
            DataStore dataStore,
            ModbusDataCollection<bool> dataSource)
        {
            WriteMultipleCoilsResponse response;

            DataStore.WriteData(
                dataStore,
                request.Data.Take(request.NumberOfPoints),
                dataSource,
                request.StartAddress,
                dataStore.SyncRoot);

            response = new WriteMultipleCoilsResponse(
                request.SlaveAddress,
                request.StartAddress,
                request.NumberOfPoints);

            return response;
        }

        internal static WriteSingleRegisterRequestResponse WriteSingleRegister(
            WriteSingleRegisterRequestResponse request,
            DataStore dataStore,
            ModbusDataCollection<ushort> dataSource)
        {
            DataStore.WriteData(
                dataStore,
                request.Data,
                dataSource,
                request.StartAddress,
                dataStore.SyncRoot);

            return request;
        }

        internal static WriteMultipleRegistersResponse WriteMultipleRegisters(
            WriteMultipleRegistersRequest request,
            DataStore dataStore,
            ModbusDataCollection<ushort> dataSource)
        {
            WriteMultipleRegistersResponse response;

            DataStore.WriteData(
                dataStore,
                request.Data,
                dataSource,
                request.StartAddress,
                dataStore.SyncRoot);

            response = new WriteMultipleRegistersResponse(
                request.SlaveAddress,
                request.StartAddress,
                request.NumberOfPoints);

            return response;
        }

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Cast is not unneccessary.")]
        internal IModbusMessage ApplyRequest(IModbusMessage request)
        {
            IModbusMessage response;

            try
            {
                Debug.WriteLine(request.ToString());
                var eventArgs = new ModbusSlaveRequestEventArgs(request);
                ModbusSlaveRequestReceived?.Invoke(this, eventArgs);

                switch (request.FunctionCode)
                {
                    case Modbus.ReadCoils:
                        response = ReadDiscretes(
                            (ReadCoilsInputsRequest)request,
                            DataStore,
                            DataStore.CoilDiscretes);
                        break;
                    case Modbus.ReadInputs:
                        response = ReadDiscretes(
                            (ReadCoilsInputsRequest)request,
                            DataStore,
                            DataStore.InputDiscretes);
                        break;
                    case Modbus.ReadHoldingRegisters:
                        response = ReadRegisters(
                            (ReadHoldingInputRegistersRequest)request,
                            DataStore,
                            DataStore.HoldingRegisters);
                        break;
                    case Modbus.ReadInputRegisters:
                        response = ReadRegisters(
                            (ReadHoldingInputRegistersRequest)request,
                            DataStore,
                            DataStore.InputRegisters);
                        break;
                    case Modbus.Diagnostics:
                        response = request;
                        break;
                    case Modbus.WriteSingleCoil:
                        response = WriteSingleCoil(
                            (WriteSingleCoilRequestResponse)request,
                            DataStore,
                            DataStore.CoilDiscretes);
                        WriteComplete?.Invoke(this, eventArgs);
                        break;
                    case Modbus.WriteSingleRegister:
                        response = WriteSingleRegister(
                            (WriteSingleRegisterRequestResponse)request,
                            DataStore,
                            DataStore.HoldingRegisters);
                        WriteComplete?.Invoke(this, eventArgs);
                        break;
                    case Modbus.WriteMultipleCoils:
                        response = WriteMultipleCoils(
                            (WriteMultipleCoilsRequest)request,
                            DataStore,
                            DataStore.CoilDiscretes);
                        WriteComplete?.Invoke(this, eventArgs);
                        break;
                    case Modbus.WriteMultipleRegisters:
                        response = WriteMultipleRegisters(
                            (WriteMultipleRegistersRequest)request,
                            DataStore,
                            DataStore.HoldingRegisters);
                        WriteComplete?.Invoke(this, eventArgs);
                        break;
                    case Modbus.ReadWriteMultipleRegisters:
                        ReadWriteMultipleRegistersRequest readWriteRequest = (ReadWriteMultipleRegistersRequest)request;
                        WriteMultipleRegisters(
                            readWriteRequest.WriteRequest,
                            DataStore,
                            DataStore.HoldingRegisters);
                        WriteComplete?.Invoke(this, eventArgs);
                        response = ReadRegisters(
                            readWriteRequest.ReadRequest,
                            DataStore,
                            DataStore.HoldingRegisters);
                        break;
                    default:
                        string msg = $"Unsupported function code {request.FunctionCode}.";
                        Debug.WriteLine(msg);
                        throw new InvalidModbusRequestException(Modbus.IllegalFunction);
                }
            }
            catch (InvalidModbusRequestException ex)
            {
                // Catches the exception for an illegal function or a custom exception from the ModbusSlaveRequestReceived event.
                response = new SlaveExceptionResponse(
                    request.SlaveAddress,
                    (byte)(Modbus.ExceptionOffset + request.FunctionCode),
                    ex.ExceptionCode);
            }

            return response;
        }
    }
}
