namespace Modbus.Data
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///     Modbus message containing data.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public interface IModbusMessageDataCollection
    {
        /// <summary>
        ///     Gets the network bytes.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        byte[] NetworkBytes { get; }

        /// <summary>
        ///     Gets the byte count.
        /// </summary>
        byte ByteCount { get; }
    }
}
