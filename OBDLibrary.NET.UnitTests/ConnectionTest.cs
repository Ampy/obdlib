using System;
using System.Globalization;
using NUnit.Framework;
using OBD2.Library.Sensors;
using OBD2.Library.Sensors.ResultType;

namespace OBD2.Library.UnitTests
{
	[TestFixture]
	public class ConnectionTest
	{
        static void SensorValueTest(Sensor sensor, string value)
        {
            var numericSensor = sensor as NumericSensor;
            if(numericSensor != null)
            {
                var sensorValue = Connection.GetSensorValue(numericSensor, value);
                Assert.IsNotNull(sensorValue, numericSensor.Label);
                Assert.IsNotNull(sensorValue.Value, numericSensor.Label);
                var strMaxValue = sensorValue.Value.Value.ToString(CultureInfo.InvariantCulture);
                var message =
                    string.Concat("Sensor (", numericSensor.Label, "), raw value: ", value, ", computed value: ",
                                  strMaxValue);
                Assert.LessOrEqual(numericSensor.MinValue, sensorValue.Value.Value, message);
                Assert.GreaterOrEqual(numericSensor.MaxValue, sensorValue.Value.Value, message);
            }
            else
            {
                var typeSensor = sensor as FuelTypeSensor;
                if(typeSensor != null)
                {
                    var sensorValue = Connection.GetSensorValue(typeSensor, value);
                    Assert.IsNotNull(sensorValue, typeSensor.Label);
                    Assert.IsNotNull(sensorValue.Value, typeSensor.Label);
                    var fuelTypeIntValue = (int) sensorValue.Value;
                    var strMaxValue = fuelTypeIntValue.ToString(CultureInfo.InvariantCulture);
                    var mesage = string.Concat("Sensor (", typeSensor.Label, "), raw value: ", value + ", computed value: ",
                                               strMaxValue);
                    Assert.LessOrEqual(typeSensor.MinValue, fuelTypeIntValue, mesage);
                    Assert.GreaterOrEqual(typeSensor.MaxValue, fuelTypeIntValue, mesage);
                }
                else
                {
                    Assert.Fail(string.Concat("Unknown sensor type: ", sensor.GetType().FullName));
                }
            }
        }

	    /// <summary>
        /// 
        /// </summary>
        /// <param name="loopCount"></param>
        /// <param name="max"></param>
        /// <param name="str"></param>
        /// <param name="sensor"></param>
        /// <param name="method">Delegate method to call.</param>
        private static void NestedLoops(int loopCount, int max, string str, Sensor sensor, SensorValueTestDelegate method)
		{
			loopCount--;
            for (var i = 0; i <= max; i++)
                if (loopCount <= 0)
                    method(sensor, string.Concat(str, Constants.SPLIT_VALUE_CHAR, i.ToString("X2")));
                else
                    NestedLoops(loopCount, max,
                                string.Concat(str, Constants.SPLIT_VALUE_CHAR, i.ToString("X2"),
                                              Constants.SPLIT_VALUE_CHAR),
                                sensor, method);
		}

        delegate void SensorValueTestDelegate(Sensor sensor, string value);

		[Test]
		public void GetSensorValueTest ()
		{
			const string split = Constants.SPLIT_VALUE_CHAR;
			var sensorList = Sensor.List;
			foreach (var sensor in sensorList)
			{
			    const int maxValue = byte.MaxValue;
			    if (sensor.ComputationType == SensorValueComputationType.Formula)
			        NestedLoops(sensor.BytesCount, maxValue, string.Concat("00", split, "00"), sensor, SensorValueTest);
			}
		}

		[Test]
		public void GetSensorValueMinTest ()
		{
			const string split = Constants.SPLIT_VALUE_CHAR;
            var sensorList = Sensor.List;
            foreach (var sensor in sensorList)
            {
                string hexMinValueB;
                var hexMinValueA = hexMinValueB = byte.MinValue.ToString("X2");                  
                var rawValue = string.Concat("00", split, "00", split, hexMinValueA, split, hexMinValueB);
                var numericSensor = sensor as NumericSensor;
                if (numericSensor != null)
                {
                    var sensorMinValue = Connection.GetSensorValue(numericSensor, rawValue);
                    Assert.IsNotNull(sensorMinValue);
                    Assert.IsNotNull(sensorMinValue.Value);
                    var strMinValue = sensorMinValue.Value.Value.ToString(CultureInfo.InvariantCulture);
                    var message = string.Concat("Sensor (", numericSensor.Label, ") raw value: ", rawValue,
                                                ", computed value: ", strMinValue);
                    Assert.LessOrEqual(numericSensor.MinValue, sensorMinValue.Value.Value, message);
                    Assert.GreaterOrEqual(numericSensor.MaxValue, sensorMinValue.Value.Value, message);
                    Assert.AreEqual(numericSensor.MinValue, sensorMinValue.Value.Value, message);
                }
                else
                {
                    var typeSensor = sensor as FuelTypeSensor;
                    if (typeSensor != null)
                    {
                        var sensorValue = Connection.GetSensorValue(typeSensor, rawValue);
                        Assert.IsNotNull(sensorValue, typeSensor.Label);
                        Assert.IsNotNull(sensorValue.Value, typeSensor.Label);
                        var fuelTypeIntValue = (int)sensorValue.Value;
                        var strMinValue = fuelTypeIntValue.ToString(CultureInfo.InvariantCulture);
                        var message = string.Concat("Sensor (", typeSensor.Label, ") raw value: ", rawValue,
                                                    ", computed value: ", strMinValue);
                        Assert.LessOrEqual(typeSensor.MinValue, fuelTypeIntValue, message);
                    }
                    else
                    {
                        Assert.Fail(string.Concat("Unknown sensor type: ", sensor.GetType().FullName));
                    }
                }
            }
		}

		[Test]
		public void GetSensorValueMaxTest ()
		{
			const string split = Constants.SPLIT_VALUE_CHAR;
            var sensorList = Sensor.List;
            foreach (var sensor in sensorList)
            {
                string hexMaxValueB;
                var hexMaxValueA = hexMaxValueB = byte.MaxValue.ToString("X2");
                var rawValue = string.Concat("00", split, "00", split, hexMaxValueA, split, hexMaxValueB);
                if (sensor is NumericSensor)
                {
                    var sensorMaxValue = Connection.GetSensorValue((NumericSensor)sensor, rawValue);
                    Assert.IsNotNull(sensorMaxValue);
                    Assert.IsNotNull(sensorMaxValue.Value);
                    var strMaxValue = sensorMaxValue.Value.Value.ToString(CultureInfo.InvariantCulture);
                    var message = string.Concat("Sensor (", sensor.Label, ") raw value: ", rawValue,
                            ", computed value: ", strMaxValue);
                    Assert.LessOrEqual(sensor.MinValue, sensorMaxValue.Value.Value, message);
                    Assert.GreaterOrEqual(sensor.MaxValue, sensorMaxValue.Value.Value, message);
                    Assert.AreEqual(sensor.MaxValue, sensorMaxValue.Value.Value, message);
                }
                else if (sensor is FuelTypeSensor)
                {
                    var sensorValue = Connection.GetSensorValue((FuelTypeSensor)sensor, rawValue);
                    Assert.IsNotNull(sensorValue, sensor.Label);
                    Assert.IsNotNull(sensorValue.Value, sensor.Label);
                    var fuelTypeIntValue = (int)sensorValue.Value;
                    var strMaxValue = fuelTypeIntValue.ToString(CultureInfo.InvariantCulture);
                    var message = string.Concat("Sensor (", sensor.Label, ") raw value: ", rawValue,
                                                ", computed value: ", strMaxValue);
                    Assert.GreaterOrEqual(sensor.MaxValue, fuelTypeIntValue, message);
                }
                else
                {
                    Assert.Fail(string.Concat("Unknown sensor type: ", sensor.GetType().FullName));
                }
            }
		}

        [Test]
        public void CleanRawValueTest()
        {
            Assert.IsNullOrEmpty(Connection.CleanRawValue(string.Join(string.Empty, Constants.StringsToRemove)));

            var random = new Random();
            Assert.IsNotNullOrEmpty(Connection.CleanRawValue(string.Join(random.Next(1000).ToString(CultureInfo.InvariantCulture), Constants.StringsToRemove)));

            Assert.IsNotNullOrEmpty(Connection.CleanRawValue("sometext\nsomeotherText"));
        }

        [Test]
        public void SplitRawValueTest()
        {
            const string split = Constants.SPLIT_VALUE_CHAR;
            var testString = "01" + split + "02" + split + "03";
            Assert.IsNotEmpty(Sensor.SplitRawValue(testString));
            Assert.AreEqual(3, Sensor.SplitRawValue(testString).Length);

            testString = split + split + split;
            Assert.IsNotNull(Sensor.SplitRawValue(testString));
            Assert.IsEmpty(Sensor.SplitRawValue(testString));
        }

        [Test]
        public void HasDataTest()
        {
            Assert.IsFalse(Connection.HasData("SomeText " + Constants.NO_DATA + " SomeText "));
            Assert.IsTrue(Connection.HasData("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer tortor mi, commodo vel auctor ac, mollis vitae metu"));
        }
	}
}
