using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace OBD2.Library.UnitTests
{
    [TestFixture]
    public class SensorTest
    {
		[Test]
        public void DuplicatedPIDTest()
		{
            var duplicatedSensorList = new Dictionary<byte, List<string>>();
            var sensorList = Sensor.List;
            foreach(var sensor in sensorList)
            {
                foreach(var sensor2 in sensorList)
                {
                    if (sensor2.GetType().Name == sensor.GetType().Name || sensor2.PID != sensor.PID) continue;

                    if(!duplicatedSensorList.ContainsKey(sensor.PID))
                    {
                        duplicatedSensorList.Add(sensor.PID, new List<string>());
                    }
                    duplicatedSensorList[sensor.PID].Add(sensor.GetType().Name);
                }
            }
            if (duplicatedSensorList.Keys.Count>0)
            {
                var message = new StringBuilder();
                foreach(var pid in duplicatedSensorList.Keys)
                {
					message.AppendLine(string.Concat(pid.ToString("X2"), " in ", string.Join(", ", duplicatedSensorList[pid].ToArray() )));
                }
                Assert.Fail(string.Concat("Duplicated sensor PIDs: ", message));
            }
            else
            {
                Assert.Pass();
            }
		}
    }
}
