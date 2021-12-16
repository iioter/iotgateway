namespace Modbus.IO
{
    using System;
    using Message;

    public class EmptyTransport : ModbusTransport
    {
        internal override byte[] ReadRequest()
        {
            throw new NotImplementedException();
        }

        internal override IModbusMessage ReadResponse<T>()
        {
            throw new NotImplementedException();
        }

        internal override byte[] BuildMessageFrame(Message.IModbusMessage message)
        {
            throw new NotImplementedException();
        }

        internal override void Write(IModbusMessage message)
        {
            throw new NotImplementedException();
        }

        internal override void OnValidateResponse(IModbusMessage request, IModbusMessage response)
        {
            throw new NotImplementedException();
        }
    }
}
