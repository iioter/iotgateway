namespace Modbus.Device
{
    using Message;
    using System;

    /// <summary>
    ///     Modbus Slave request event args containing information on the message.
    /// </summary>
    public class ModbusSlaveRequestEventArgs : EventArgs
    {
        private readonly IModbusMessage _message;

        internal ModbusSlaveRequestEventArgs(IModbusMessage message)
        {
            _message = message;
        }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        public IModbusMessage Message
        {
            get { return _message; }
        }
    }
}