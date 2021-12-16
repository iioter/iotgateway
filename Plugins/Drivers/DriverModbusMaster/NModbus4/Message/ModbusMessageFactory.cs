namespace Modbus.Message
{
    using System;

    /// <summary>
    ///     Modbus message factory.
    /// </summary>
    public static class ModbusMessageFactory
    {
        /// <summary>
        ///     Minimum request frame length.
        /// </summary>
        private const int MinRequestFrameLength = 3;

        /// <summary>
        ///     Create a Modbus message.
        /// </summary>
        /// <typeparam name="T">Modbus message type.</typeparam>
        /// <param name="frame">Bytes of Modbus frame.</param>
        /// <returns>New Modbus message based on type and frame bytes.</returns>
        public static T CreateModbusMessage<T>(byte[] frame)
            where T : IModbusMessage, new()
        {
            IModbusMessage message = new T();
            message.Initialize(frame);

            return (T)message;
        }

        /// <summary>
        ///     Create a Modbus request.
        /// </summary>
        /// <param name="frame">Bytes of Modbus frame.</param>
        /// <returns>Modbus request.</returns>
        public static IModbusMessage CreateModbusRequest(byte[] frame)
        {
            if (frame.Length < MinRequestFrameLength)
            {
                string msg = $"Argument 'frame' must have a length of at least {MinRequestFrameLength} bytes.";
                throw new FormatException(msg);
            }

            IModbusMessage request;
            byte functionCode = frame[1];

            switch (functionCode)
            {
                case Modbus.ReadCoils:
                case Modbus.ReadInputs:
                    request = CreateModbusMessage<ReadCoilsInputsRequest>(frame);
                    break;
                case Modbus.ReadHoldingRegisters:
                case Modbus.ReadInputRegisters:
                    request = CreateModbusMessage<ReadHoldingInputRegistersRequest>(frame);
                    break;
                case Modbus.WriteSingleCoil:
                    request = CreateModbusMessage<WriteSingleCoilRequestResponse>(frame);
                    break;
                case Modbus.WriteSingleRegister:
                    request = CreateModbusMessage<WriteSingleRegisterRequestResponse>(frame);
                    break;
                case Modbus.Diagnostics:
                    request = CreateModbusMessage<DiagnosticsRequestResponse>(frame);
                    break;
                case Modbus.WriteMultipleCoils:
                    request = CreateModbusMessage<WriteMultipleCoilsRequest>(frame);
                    break;
                case Modbus.WriteMultipleRegisters:
                    request = CreateModbusMessage<WriteMultipleRegistersRequest>(frame);
                    break;
                case Modbus.ReadWriteMultipleRegisters:
                    request = CreateModbusMessage<ReadWriteMultipleRegistersRequest>(frame);
                    break;
                default:
                    string msg = $"Unsupported function code {functionCode}";
                    throw new ArgumentException(msg, nameof(frame));
            }

            return request;
        }
    }
}
