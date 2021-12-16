namespace Modbus.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Utility;

    /// <summary>
    ///     Collection of 16 bit registers.
    /// </summary>
    public class RegisterCollection : Collection<ushort>, IModbusMessageDataCollection
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterCollection" /> class.
        /// </summary>
        public RegisterCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterCollection" /> class.
        /// </summary>
        /// <param name="bytes">Array for register collection.</param>
        public RegisterCollection(byte[] bytes)
            : this((IList<ushort>)ModbusUtility.NetworkBytesToHostUInt16(bytes))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterCollection" /> class.
        /// </summary>
        /// <param name="registers">Array for register collection.</param>
        public RegisterCollection(params ushort[] registers)
            : this((IList<ushort>)registers)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterCollection" /> class.
        /// </summary>
        /// <param name="registers">List for register collection.</param>
        public RegisterCollection(IList<ushort> registers)
            : base(registers.IsReadOnly ? new List<ushort>(registers) : registers)
        {
        }

        public byte[] NetworkBytes
        {
            get
            {
                var bytes = new MemoryStream(ByteCount);

                foreach (ushort register in this)
                {
                    var b = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)register));
                    bytes.Write(b, 0, b.Length);
                }

                return bytes.ToArray();
            }
        }

        /// <summary>
        ///     Gets the byte count.
        /// </summary>
        public byte ByteCount
        {
            get { return (byte)(Count * 2); }
        }

        /// <summary>
        ///     Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.
        /// </returns>
        public override string ToString()
        {
            return string.Concat("{", string.Join(", ", this.Select(v => v.ToString()).ToArray()), "}");
        }
    }
}
