namespace Modbus.Device
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using IO;
    using Message;

    /// <summary>
    /// Represents an incoming connection from a Modbus master. Contains the slave's logic to process the connection.
    /// </summary>
    internal class ModbusMasterTcpConnection : ModbusDevice, IDisposable
    {
        private readonly TcpClient _client;
        private readonly string _endPoint;
        private readonly Stream _stream;
        private readonly ModbusTcpSlave _slave;
        private readonly Task _requestHandlerTask;

        private readonly byte[] _mbapHeader = new byte[6];
        private byte[] _messageFrame;

        public ModbusMasterTcpConnection(TcpClient client, ModbusTcpSlave slave)
            : base(new ModbusIpTransport(new TcpClientAdapter(client)))
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (slave == null)
            {
                throw new ArgumentNullException(nameof(slave));
            }

            _client = client;
            _endPoint = client.Client.RemoteEndPoint.ToString();
            _stream = client.GetStream();
            _slave = slave;
            _requestHandlerTask = Task.Run((Func<Task>)HandleRequestAsync);
        }

        /// <summary>
        ///     Occurs when a Modbus master TCP connection is closed.
        /// </summary>
        public event EventHandler<TcpConnectionEventArgs> ModbusMasterTcpConnectionClosed;

        public string EndPoint
        {
            get { return _endPoint; }
        }

        public Stream Stream
        {
            get { return _stream; }
        }

        public TcpClient TcpClient
        {
            get { return _client; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stream.Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task HandleRequestAsync()
        {
            while (true)
            {
                Debug.WriteLine($"Begin reading header from Master at IP: {EndPoint}");

                int readBytes = await Stream.ReadAsync(_mbapHeader, 0, 6).ConfigureAwait(false);
                if (readBytes == 0)
                {
                    Debug.WriteLine($"0 bytes read, Master at {EndPoint} has closed Socket connection.");
                    ModbusMasterTcpConnectionClosed?.Invoke(this, new TcpConnectionEventArgs(EndPoint));
                    return;
                }

                ushort frameLength = (ushort)IPAddress.HostToNetworkOrder(BitConverter.ToInt16(_mbapHeader, 4));
                Debug.WriteLine($"Master at {EndPoint} sent header: \"{string.Join(", ", _mbapHeader)}\" with {frameLength} bytes in PDU");

                _messageFrame = new byte[frameLength];
                readBytes = await Stream.ReadAsync(_messageFrame, 0, frameLength).ConfigureAwait(false);
                if (readBytes == 0)
                {
                    Debug.WriteLine($"0 bytes read, Master at {EndPoint} has closed Socket connection.");
                    ModbusMasterTcpConnectionClosed?.Invoke(this, new TcpConnectionEventArgs(EndPoint));
                    return;
                }

                Debug.WriteLine($"Read frame from Master at {EndPoint} completed {readBytes} bytes");
                byte[] frame = _mbapHeader.Concat(_messageFrame).ToArray();
                Debug.WriteLine($"RX from Master at {EndPoint}: {string.Join(", ", frame)}");

                var request = ModbusMessageFactory.CreateModbusRequest(_messageFrame);
                request.TransactionId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

                // perform action and build response
                IModbusMessage response = _slave.ApplyRequest(request);
                response.TransactionId = request.TransactionId;

                // write response
                byte[] responseFrame = Transport.BuildMessageFrame(response);
                Debug.WriteLine($"TX to Master at {EndPoint}: {string.Join(", ", responseFrame)}");
                await Stream.WriteAsync(responseFrame, 0, responseFrame.Length).ConfigureAwait(false);
            }
        }
    }
}
