namespace Modbus.Device
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using IO;
    using Message;

    using Unme.Common;

    /// <summary>
    ///     Modbus UDP slave device.
    /// </summary>
    public class ModbusUdpSlave : ModbusSlave
    {
        private readonly UdpClient _udpClient;

        private ModbusUdpSlave(byte unitId, UdpClient udpClient)
            : base(unitId, new ModbusIpTransport(new UdpClientAdapter(udpClient)))
        {
            _udpClient = udpClient;
        }

        /// <summary>
        ///     Modbus UDP slave factory method.
        ///     Creates NModbus UDP slave with default
        /// </summary>
        public static ModbusUdpSlave CreateUdp(UdpClient client)
        {
            return new ModbusUdpSlave(Modbus.DefaultIpSlaveUnitId, client);
        }

        /// <summary>
        ///     Modbus UDP slave factory method.
        /// </summary>
        public static ModbusUdpSlave CreateUdp(byte unitId, UdpClient client)
        {
            return new ModbusUdpSlave(unitId, client);
        }

        /// <summary>
        ///     Start slave listening for requests.
        /// </summary>
        public override async Task ListenAsync()
        {
            Debug.WriteLine("Start Modbus Udp Server.");

            try
            {
                while (true)
                {
                    UdpReceiveResult receiveResult = await _udpClient.ReceiveAsync().ConfigureAwait(false);
                    IPEndPoint masterEndPoint = receiveResult.RemoteEndPoint;
                    byte[] frame = receiveResult.Buffer;

                    Debug.WriteLine($"Read Frame completed {frame.Length} bytes");
                    Debug.WriteLine($"RX: {string.Join(", ", frame)}");

                    IModbusMessage request =
                        ModbusMessageFactory.CreateModbusRequest(frame.Slice(6, frame.Length - 6).ToArray());
                    request.TransactionId = (ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(frame, 0));

                    // perform action and build response
                    IModbusMessage response = ApplyRequest(request);
                    response.TransactionId = request.TransactionId;

                    // write response
                    byte[] responseFrame = Transport.BuildMessageFrame(response);
                    Debug.WriteLine($"TX: {string.Join(", ", responseFrame)}");
                    await _udpClient.SendAsync(responseFrame, responseFrame.Length, masterEndPoint).ConfigureAwait(false);
                }
            }
            catch (SocketException se)
            {
                // this hapens when slave stops
                if (se.SocketErrorCode != SocketError.Interrupted)
                {
                    throw;
                }
            }
        }
    }
}
