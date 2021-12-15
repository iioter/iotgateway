namespace Modbus.Data
{
    /// <summary>
    ///     Types of data supported by the Modbus protocol.
    /// </summary>
    public enum ModbusDataType
    {
        /// <summary>
        ///     Read/write register.
        /// </summary>
        HoldingRegister,

        /// <summary>
        ///     Readonly register.
        /// </summary>
        InputRegister,

        /// <summary>
        ///     Read/write discrete.
        /// </summary>
        Coil,

        /// <summary>
        ///     Readonly discrete.
        /// </summary>
        Input
    }
}
