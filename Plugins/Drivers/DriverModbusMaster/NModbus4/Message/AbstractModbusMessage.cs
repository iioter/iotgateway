namespace Modbus.Message
{
    using System;

    /// <summary>
    ///     Abstract Modbus message.
    /// </summary>
    public abstract class AbstractModbusMessage
    {
        private readonly ModbusMessageImpl _messageImpl;

        /// <summary>
        ///     Abstract Modbus message.
        /// </summary>
        internal AbstractModbusMessage()
        {
            _messageImpl = new ModbusMessageImpl();
        }

        /// <summary>
        ///     Abstract Modbus message.
        /// </summary>
        internal AbstractModbusMessage(byte slaveAddress, byte functionCode)
        {
            _messageImpl = new ModbusMessageImpl(slaveAddress, functionCode);
        }

        public ushort TransactionId
        {
            get { return _messageImpl.TransactionId; }
            set { _messageImpl.TransactionId = value; }
        }

        public byte FunctionCode
        {
            get { return _messageImpl.FunctionCode; }
            set { _messageImpl.FunctionCode = value; }
        }

        public byte SlaveAddress
        {
            get { return _messageImpl.SlaveAddress; }
            set { _messageImpl.SlaveAddress = value; }
        }

        public byte[] MessageFrame
        {
            get { return _messageImpl.MessageFrame; }
        }

        public virtual byte[] ProtocolDataUnit
        {
            get { return _messageImpl.ProtocolDataUnit; }
        }

        public abstract int MinimumFrameSize { get; }

        internal ModbusMessageImpl MessageImpl
        {
            get { return _messageImpl; }
        }

        public void Initialize(byte[] frame)
        {
            if (frame.Length < MinimumFrameSize)
            {
                string msg = $"Message frame must contain at least {MinimumFrameSize} bytes of data.";
                throw new FormatException(msg);
            }

            _messageImpl.Initialize(frame);
            InitializeUnique(frame);
        }

        protected abstract void InitializeUnique(byte[] frame);
    }
}
