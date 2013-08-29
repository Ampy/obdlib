using System;
using System.IO.Ports;
using System.Text;

namespace OBD2.Library
{
    /// <summary>
    /// Fromt: http://dotspatial.codeplex.com/SourceControl/latest#DotSpatial.Positioning/SerialPort.cs
    /// </summary>
    internal class SerialPort : System.IO.Ports.SerialPort
    {
        public SerialPort(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One)
        {
        }

        private SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = Constants.SERIAL_PORT_READ_TIMEOUT_IN_MS;
            WriteTimeout = Constants.SERIAL_PORT_WRITE_TIMEOUT_IN_MS;
            NewLine = "\r\n";
            WriteBufferSize = Constants.SERIAL_PORT_BUFFER_SIZE;
            ReadBufferSize = Constants.SERIAL_PORT_BUFFER_SIZE;
            //ReceivedBytesThreshold = 65535;
            Encoding = Encoding.ASCII;
        }

        public new void Open()
        {
            base.Open();

            /* The .Net SerialStream class has a bug that causes its finalizer to crash when working
             * with virtual COM ports (e.g. FTDI, Prolific, etc.) See the following page for details:
             * http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/8a1825d2-c84b-4620-91e7-3934a4d47330
             * To work around this bug, we suppress the finalizer for the BaseStream and close it ourselves instead.
             * See the Dispose method for the other half of this workaorund.
             */
            GC.SuppressFinalize(BaseStream);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    /* The .Net SerialStream class has a bug that causes its finalizer to crash when working
                     * with virtual COM ports (e.g. FTDI, Prolific, etc.) See the following page for details:
                     * http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/8a1825d2-c84b-4620-91e7-3934a4d47330
                     * To work around this bug, we suppress the finalizer for the BaseStream and close it ourselves instead.
                     * See the Open method for the other half of this workaround.
                     */
                    if (IsOpen)
                    {
                        BaseStream.Close();
                    }
                }
// ReSharper disable EmptyGeneralCatchClause
                catch (Exception)
// ReSharper restore EmptyGeneralCatchClause
                {
                    // The BaseStream is already closed, disposed, or in an invalid state. Ignore and continue disposing.
                }
            }

            base.Dispose(disposing);
        }

        public new static string[] GetPortNames()
        {
            return System.IO.Ports.SerialPort.GetPortNames();
        }
    }
}
