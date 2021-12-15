namespace Modbus.Message
{
    using Data;

    public abstract class AbstractModbusMessageWithData<TData> : AbstractModbusMessage
        where TData : IModbusMessageDataCollection
    {
        internal AbstractModbusMessageWithData()
        {
        }

        internal AbstractModbusMessageWithData(byte slaveAddress, byte functionCode)
            : base(slaveAddress, functionCode)
        {
        }

        public TData Data
        {
            get { return (TData)MessageImpl.Data; }
            set { MessageImpl.Data = value; }
        }
    }
}
