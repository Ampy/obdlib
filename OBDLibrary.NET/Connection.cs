using System;
using System.Globalization;
using System.Threading;
using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library
{
	public sealed class Connection : IDisposable
	{
	    private SerialPort _serialPort;
		private bool _disposed;

        private BaudRate _baudRate = Constants.SERIAL_PORT_DEFAULT_BAUD_RATE;
        public BaudRate BaudRate
        {
            get { return _baudRate; }
            set
            {
                _baudRate = value;
                if (_serialPort != null)
                {
                    _serialPort.BaudRate = Convert.ToInt32(value);
                }
            }
        }

		public Connection (string portName)
		{
		    _serialPort = new SerialPort(portName, Convert.ToInt32(_baudRate));
		}

		public Connection (string portName, BaudRate baudRate)
		{
		    _serialPort = new SerialPort(portName, Convert.ToInt32(baudRate));
		}
		
		public void Open ()
		{
		    _serialPort.NewLine = Constants.SERIAL_PORT_READY_PROMPT_CHAR.ToString(CultureInfo.InvariantCulture);
			_serialPort.Open ();

            SendMessage("ATZ");
            SendMessage("ATE0");
            SendMessage("ATL0");
            SendMessage("ATSP1");
            SendMessage("01 00");
            SendMessage("ATH1");
		}
		
		public void Close ()
		{
			_serialPort.Close ();
		}

		public static string[] PortNames ()
		{
			return SerialPort.GetPortNames ();
		}

        public NumericValue GetSensorValue (SensorValueMode mode, NumericSensor sensor)
        {
            var hexMode = ((int)mode).ToString(Constants.HEX_FORMAT);
            return GetSensorValue(sensor, GetRawValue(string.Concat(hexMode, sensor.PID.ToString(Constants.HEX_FORMAT))));
        }

        public FuelTypeValue GetSensorValue(SensorValueMode mode, FuelTypeSensor sensor)
        {
            var hexMode = ((int)mode).ToString(Constants.HEX_FORMAT);
            return GetSensorValue(sensor, GetRawValue(string.Concat(hexMode, sensor.PID.ToString(Constants.HEX_FORMAT))));
        }

        internal static FuelTypeValue GetSensorValue(FuelTypeSensor sensor, string rawValue)
        {
            return HasData(rawValue) ? sensor.GetComputedValue(CleanRawValue(rawValue)) : new FuelTypeValue();
        }

	    internal static NumericValue GetSensorValue(NumericSensor sensor, string rawValue)
	    {
            return HasData(rawValue) ? sensor.GetComputedValue(CleanRawValue(rawValue)) : new NumericValue();
	    }

	    public string GetRawValueFromPID(string pid)
		{
			return GetRawValue(pid);
		}

        internal static bool HasData(string rawValue)
        {
            return !rawValue.Contains(Constants.NO_DATA);
        }

        internal static string CleanRawValue(string rawValue)
        {
            var returnValue = rawValue;
            foreach (var stringToRemove in Constants.StringsToRemove)
            {
                returnValue = returnValue.Replace(stringToRemove, string.Empty);
            }
            return returnValue;
        }

        private void SendMessage(string message)
        {
            _serialPort.Write(string.Concat(message, Constants.SERIAL_PORT_MESSAGE_TERMINATOR_CHAR));
        }

	    private string ReceiveMessage()
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            var i = 0;
            var buffer = new byte[Constants.SERIAL_PORT_BUFFER_SIZE];
            while (stopWatch.ElapsedMilliseconds < Constants.SERIAL_PORT_RECEIVE_TIMEOUT_IN_MS)
            {
                if (_serialPort.BytesToRead > 0 && _serialPort.Read(buffer, i, 1) > 0)
                {
                    if (buffer[i] == Constants.SERIAL_PORT_READY_PROMPT_CHAR)
                    {
                        var message = new string(
                            _serialPort.Encoding.GetChars(buffer),
                            0,
                            i);

                        // Trim the message to the point just before the last EOL terminator
                        message =
                            message.Substring(0, message.LastIndexOf(Constants.SERIAL_PORT_MESSAGE_TERMINATOR_CHAR) - 1).
                                Trim();

                        return message;
                    }
                    i++;
                }
                Thread.Sleep(1);
            }

            // Reaching this point means we've waited too long for the chip to send us a response
            return null;
        }

	    private string GetRawValue (string pid)
		{
            SendMessage(pid);
			Thread.Sleep (Constants.SLEEP_TIME_IN_MS);
            var cont = true;
            var bff = new byte[Constants.SERIAL_PORT_BUFFER_SIZE];
            var retVal = string.Empty;
            while (cont)
            {
                var count = _serialPort.Read(bff, 0, Constants.SERIAL_PORT_BUFFER_SIZE);
                retVal += System.Text.Encoding.Default.GetString(bff, 0, count);
                if (retVal.Contains(Constants.SERIAL_PORT_READY_PROMPT_CHAR.ToString(CultureInfo.InvariantCulture)))
                {
                    cont = false;
                }
            }
            return retVal;
		}

		public void Dispose ()
		{
			Dispose (true);
		}

	    private void Dispose (bool disposing)
		{
	        if (_disposed) return;
	        if (disposing && _serialPort != null)
	        {
	            _serialPort.Dispose();
	        }
	        _serialPort = null;
	        _disposed = true;
		}
	}
}

