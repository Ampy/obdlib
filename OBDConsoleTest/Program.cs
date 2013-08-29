using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using OBD2.Library;
using OBD2.Library.Sensors.ResultType;

namespace OBDConsoleTest
{
    static class Program
	{
        const string SEPARATOR_CHAR = ";";
        const string TIMESTAMP_FORMAT = "yyyyMMddHHmmss.fff";
        const string FILENAME_DATEFORMAT = "yyyyMMddHHmmss";
        const int DELAY_IN_MS = 100;
        static int _counter;

		static void Main ()
		{
            Console.Clear();
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += CancelEventHandler;

            var portNames = Connection.PortNames();
            if (portNames != null && portNames.Length > 0)
            {
                Console.WriteLine("Available port names:");
                foreach (var name in portNames)
                {
                    Console.WriteLine(name);
                }
            }

            Console.WriteLine("Enter port name: ");
            var portName = Console.ReadLine();

			var pidList = new List<NumericSensor> {Sensor.VehicleSpeed, Sensor.EngineRPM};

		    var logFileName = string.Concat("Log", DateTime.Now.ToString(FILENAME_DATEFORMAT), ".txt");
            var logFilePath = Path.Combine(Environment.CurrentDirectory, logFileName);

		    var contents = string.Concat(
		        "Mode",
		        SEPARATOR_CHAR,
		        "PID",
		        SEPARATOR_CHAR,
		        "Value",
		        SEPARATOR_CHAR,
		        "TimeStamp",
		        SEPARATOR_CHAR,
		        Environment.NewLine
		        );
            File.AppendAllText(logFilePath, contents);

            using (var connection = new Connection(portName, BaudRate.B38400))
            {
				connection.Open ();

                while (true)
                {
                    foreach (var sensor in pidList)
                    {
                        var pid = sensor;
                        if (pid == null) continue;
                        var sensorValue = connection.GetSensorValue(SensorValueMode.Mode1, pid);
                        if (sensorValue.Value.HasValue)
                        {
                            contents = string.Concat((int) SensorValueMode.Mode1, SEPARATOR_CHAR, pid.PID.ToString("X2"), SEPARATOR_CHAR, sensorValue.Value.Value.ToString(CultureInfo.InvariantCulture), SEPARATOR_CHAR, sensorValue.TimeStamp.ToString(TIMESTAMP_FORMAT), SEPARATOR_CHAR, Environment.NewLine);

                            File.AppendAllText(logFilePath, contents);
                            _counter++;
                        }
                        Thread.Sleep(DELAY_IN_MS);
                    }
                }
			}
		}

        private static void CancelEventHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine(string.Concat("Values logged count: ", _counter));
        }
	}
}
