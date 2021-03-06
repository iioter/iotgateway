/* ========================================================================
 * Copyright (c) 2005-2021 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Mono.Options;
using Opc.Ua;
using Opc.Ua.Configuration;
using static Opc.Ua.Utils;

namespace Quickstarts
{
    /// <summary>
    /// The log output implementation of a TextWriter.
    /// </summary>
    public class LogWriter : TextWriter
    {
        private StringBuilder m_builder = new StringBuilder();

        public override void Write(char value)
        {
            m_builder.Append(value);
        }

        public override void WriteLine(char value)
        {
            m_builder.Append(value);
            //LogInfo("{0}", m_builder.ToString());
            m_builder.Clear();
        }

        public override void WriteLine()
        {
            //LogInfo("{0}", m_builder.ToString());
            m_builder.Clear();
        }

        public override void WriteLine(string format, object arg0)
        {
            m_builder.Append(format);
            //LogInfo(m_builder.ToString(), arg0);
            m_builder.Clear();
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            m_builder.Append(format);
            //LogInfo(m_builder.ToString(), arg0, arg1);
            m_builder.Clear();
        }

        public override void WriteLine(string format, params object[] arg)
        {
            m_builder.Append(format);
            //LogInfo(m_builder.ToString(), arg);
            m_builder.Clear();
        }

        public override void Write(string value)
        {
            m_builder.Append(value);
        }

        public override void WriteLine(string value)
        {
            m_builder.Append(value);
            //LogInfo("{0}", m_builder.ToString());
            m_builder.Clear();
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }

    /// <summary>
    /// The error code why the application exit.
    /// </summary>
    public enum ExitCode : int
    {
        Ok = 0,
        ErrorNotStarted = 0x80,
        ErrorRunning = 0x81,
        ErrorException = 0x82,
        ErrorStopping = 0x83,
        ErrorCertificate = 0x84,
        ErrorInvalidCommandLine = 0x100
    };

    /// <summary>
    /// An exception that occured and caused an exit of the application.
    /// </summary>
    public class ErrorExitException : Exception
    {
        public ExitCode ExitCode { get; }

        public ErrorExitException(ExitCode exitCode)
        {
            ExitCode = exitCode;
        }

        public ErrorExitException()
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message) : base(message)
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message, ExitCode exitCode) : base(message)
        {
            ExitCode = exitCode;
        }

        public ErrorExitException(string message, Exception innerException) : base(message, innerException)
        {
            ExitCode = ExitCode.Ok;
        }

        public ErrorExitException(string message, Exception innerException, ExitCode exitCode) : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }

    /// <summary>
    /// A dialog which asks for user input.
    /// </summary>
    public class ApplicationMessageDlg : IApplicationMessageDlg
    {
        private TextWriter m_output;
        private string m_message = string.Empty;
        private bool m_ask;

        public ApplicationMessageDlg(TextWriter output)
        {
            m_output = output;
        }

        public override void Message(string text, bool ask)
        {
            m_message = text;
            m_ask = ask;
        }

        public override async Task<bool> ShowAsync()
        {
            if (m_ask)
            {
                var message = new StringBuilder(m_message);
                message.Append(" (y/n, default y): ");
                //m_output.Write(message.ToString());

                try
                {
                    ConsoleKeyInfo result = Console.ReadKey();
                    //m_output.WriteLine();
                    return await Task.FromResult((result.KeyChar == 'y') ||
                        (result.KeyChar == 'Y') || (result.KeyChar == '\r')).ConfigureAwait(false);
                }
                catch
                {
                    // intentionally fall through
                }
            }
            else
            {
                //m_output.WriteLine(m_message);
            }

            return await Task.FromResult(true).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Helper functions shared in various console applications.
    /// </summary>
    public static class ConsoleUtils
    {
        /// <summary>
        /// Process a command line of the console sample application.
        /// </summary>
        public static string ProcessCommandLine(
            TextWriter output,
            string[] args,
            Mono.Options.OptionSet options,
            ref bool showHelp,
            bool noExtraArgs = true)
        {
            IList<string> extraArgs = null;
            try
            {
                extraArgs = options.Parse(args);
                if (noExtraArgs)
                {
                    foreach (string extraArg in extraArgs)
                    {
                        output.WriteLine("Error: Unknown option: {0}", extraArg);
                        showHelp = true;
                    }
                }
            }
            catch (OptionException e)
            {
                output.WriteLine(e.Message);
                showHelp = true;
            }

            if (showHelp)
            {
                options.WriteOptionDescriptions(output);
                throw new ErrorExitException("Invalid Commandline or help requested.", ExitCode.ErrorInvalidCommandLine);
            }

            return extraArgs.FirstOrDefault();
        }

        /// <summary>
        /// Create an event which is set if a user
        /// enters the Ctrl-C key combination.
        /// </summary>
        public static ManualResetEvent CtrlCHandler()
        {
            var quitEvent = new ManualResetEvent(false);
            try
            {
                Console.CancelKeyPress += (_, eArgs) => {
                    quitEvent.Set();
                    eArgs.Cancel = true;
                };
            }
            catch
            {
                // intentionally left blank
            }
            return quitEvent;
        }
    }
}

