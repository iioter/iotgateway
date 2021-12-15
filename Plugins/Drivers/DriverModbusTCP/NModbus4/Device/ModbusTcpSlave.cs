namespace Modbus.Device
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;
#if TIMER
    using System.Timers;
#endif
    using IO;

    /// <summary>
    ///     Modbus TCP slave device.
    /// </summary>
    public class ModbusTcpSlave : ModbusSlave
    {
        private const int TimeWaitResponse = 1000;
        private readonly object _serverLock = new object();

        private readonly ConcurrentDictionary<string, ModbusMasterTcpConnection> _masters =
            new ConcurrentDictionary<string, ModbusMasterTcpConnection>();

        private TcpListener _server;
#if TIMER
        private Timer _timer;
#endif
        private ModbusTcpSlave(byte unitId, TcpListener tcpListener)
            : base(unitId, new EmptyTransport())
        {
            if (tcpListener == null)
            {
                throw new ArgumentNullException(nameof(tcpListener));
            }

            _server = tcpListener;
        }

#if TIMER
        private ModbusTcpSlave(byte unitId, TcpListener tcpListener, double timeInterval)
            : base(unitId, new EmptyTransport())
        {
            if (tcpListener == null)
            {
                throw new ArgumentNullException(nameof(tcpListener));
            }

            _server = tcpListener;
            _timer = new Timer(timeInterval);
            _timer.Elapsed += OnTimer;
            _timer.Enabled = true;
        }
#endif

        /// <summary>
        ///     Gets the Modbus TCP Masters connected to this Modbus TCP Slave.
        /// </summary>
        public ReadOnlyCollection<TcpClient> Masters
        {
            get
            {
                return new ReadOnlyCollection<TcpClient>(_masters.Values.Select(mc => mc.TcpClient).ToList());
            }
        }

        /// <summary>
        ///     Gets the server.
        /// </summary>
        /// <value>The server.</value>
        /// <remarks>
        ///     This property is not thread safe, it should only be consumed within a lock.
        /// </remarks>
        private TcpListener Server
        {
            get
            {
                if (_server == null)
                {
                    throw new ObjectDisposedException("Server");
                }

                return _server;
            }
        }

        /// <summary>
        ///     Modbus TCP slave factory method.
        /// </summary>
        public static ModbusTcpSlave CreateTcp(byte unitId, TcpListener tcpListener)
        {
            return new ModbusTcpSlave(unitId, tcpListener);
        }

#if TIMER
        /// <summary>
        ///     Creates ModbusTcpSlave with timer which polls connected clients every
        ///     <paramref name="pollInterval"/> milliseconds on that they are connected.
        /// </summary>
        public static ModbusTcpSlave CreateTcp(byte unitId, TcpListener tcpListener, double pollInterval)
        {
            return new ModbusTcpSlave(unitId, tcpListener, pollInterval);
        }
#endif

        /// <summary>
        ///     Start slave listening for requests.
        /// </summary>
        public override async Task ListenAsync()
        {
            Debug.WriteLine("Start Modbus Tcp Server.");
            // TODO: add state {stoped, listening} and check it before starting
            Server.Start();

            while (true)
            {
                TcpClient client = await Server.AcceptTcpClientAsync().ConfigureAwait(false);
                var masterConnection = new ModbusMasterTcpConnection(client, this);
                masterConnection.ModbusMasterTcpConnectionClosed += OnMasterConnectionClosedHandler;
                _masters.TryAdd(client.Client.RemoteEndPoint.ToString(), masterConnection);
            }
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        /// <remarks>Dispose is thread-safe.</remarks>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // double-check locking
                if (_server != null)
                {
                    lock (_serverLock)
                    {
                        if (_server != null)
                        {
                            _server.Stop();
                            _server = null;

#if TIMER
                            if (_timer != null)
                            {
                                _timer.Dispose();
                                _timer = null;
                            }
#endif

                            foreach (var key in _masters.Keys)
                            {
                                ModbusMasterTcpConnection connection;

                                if (_masters.TryRemove(key, out connection))
                                {
                                    connection.ModbusMasterTcpConnectionClosed -= OnMasterConnectionClosedHandler;
                                    connection.Dispose();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool IsSocketConnected(Socket socket)
        {
            bool poll = socket.Poll(TimeWaitResponse, SelectMode.SelectRead);
            bool available = (socket.Available == 0);
            return poll && available;
        }

#if TIMER
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            foreach (var master in _masters.ToList())
            {
                if (IsSocketConnected(master.Value.TcpClient.Client) == false)
                {
                    master.Value.Dispose();
                }
            }
        }
#endif
        private void OnMasterConnectionClosedHandler(object sender, TcpConnectionEventArgs e)
        {
            ModbusMasterTcpConnection connection;

            if (!_masters.TryRemove(e.EndPoint, out connection))
            {
                string msg = $"EndPoint {e.EndPoint} cannot be removed, it does not exist.";
                throw new ArgumentException(msg);
            }

            Debug.WriteLine($"Removed Master {e.EndPoint}");
        }
    }
}
