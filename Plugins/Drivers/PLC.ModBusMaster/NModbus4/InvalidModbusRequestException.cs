namespace Modbus
{
    using System;
#if NET46
    using System.Runtime.Serialization;
#endif
    /// <summary>
    ///     An exception that provides the exception code that will be sent in response to an invalid Modbus request.
    /// </summary>
#if NET46
    [Serializable]
#endif
    public class InvalidModbusRequestException : Exception
    {
        private readonly byte _exceptionCode;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModbusRequestException" /> class with a specified Modbus exception code.
        /// </summary>
        /// <param name="exceptionCode">The Modbus exception code to provide to the slave.</param>
        public InvalidModbusRequestException(byte exceptionCode)
            : this(GetMessage(exceptionCode), exceptionCode)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModbusRequestException" /> class with a specified error message and Modbus exception code.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exceptionCode">The Modbus exception code to provide to the slave.</param>
        public InvalidModbusRequestException(string message, byte exceptionCode)
            : this(message, exceptionCode, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModbusRequestException" /> class with a specified Modbus exception code and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="exceptionCode">The Modbus exception code to provide to the slave.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidModbusRequestException(byte exceptionCode, Exception innerException)
            : this(GetMessage(exceptionCode), exceptionCode, innerException)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModbusRequestException" /> class with a specified Modbus exception code and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="exceptionCode">The Modbus exception code to provide to the slave.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidModbusRequestException(string message, byte exceptionCode, Exception innerException)
            : base(message, innerException)
        {
            _exceptionCode = exceptionCode;
        }

#if NET46
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidModbusRequestException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidModbusRequestException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _exceptionCode = info.GetByte(nameof(ExceptionCode));
        }
#endif

        /// <summary>
        ///     Gets the Modbus exception code to provide to the slave.
        /// </summary>
        public byte ExceptionCode
        {
            get { return _exceptionCode; }
        }

#if NET46
        /// <summary>Sets the <see cref="SerializationInfo" /> object with the Modbus exception code and additional exception information.</summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ExceptionCode", this._exceptionCode, typeof(byte));
        }
#endif

        private static string GetMessage(byte exceptionCode)
        {
            return $"Modbus exception code {exceptionCode}.";
        }
    }
}
