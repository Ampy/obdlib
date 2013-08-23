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
                Assert.LessOrEqual(sensorValue.Value.Value, numericSensor.MaxValue, message);
                Assert.GreaterOrEqual(sensorValue.Value.Value, numericSensor.MinValue, message);
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
                    Assert.LessOrEqual(fuelTypeIntValue, typeSensor.MaxValue, mesage);
                    Assert.GreaterOrEqual(fuelTypeIntValue, typeSensor.MinValue, mesage);
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
                    Assert.LessOrEqual(sensorMinValue.Value.Value, numericSensor.MaxValue, message);
                    Assert.GreaterOrEqual(sensorMinValue.Value.Value, numericSensor.MinValue, message);
                    Assert.AreEqual(sensorMinValue.Value.Value, numericSensor.MinValue, message);
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
                        Assert.LessOrEqual(fuelTypeIntValue, typeSensor.MaxValue, message);
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
                var numericSensor = sensor as NumericSensor;
                if (numericSensor != null)
                {
                    var sensorMaxValue = Connection.GetSensorValue(numericSensor, rawValue);
                    Assert.IsNotNull(sensorMaxValue);
                    Assert.IsNotNull(sensorMaxValue.Value);
                    var strMaxValue = sensorMaxValue.Value.Value.ToString(CultureInfo.InvariantCulture);
                    var message = string.Concat("Sensor (", numericSensor.Label, ") raw value: ", rawValue,
                            ", computed value: ", strMaxValue);
                    Assert.LessOrEqual(sensorMaxValue.Value.Value, numericSensor.MaxValue, message);
                    Assert.GreaterOrEqual(sensorMaxValue.Value.Value, numericSensor.MinValue, message);
                    Assert.AreEqual(sensorMaxValue.Value.Value, numericSensor.MaxValue, message);
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
                        var strMaxValue = fuelTypeIntValue.ToString(CultureInfo.InvariantCulture);
                        var message = string.Concat("Sensor (", typeSensor.Label, ") raw value: ", rawValue,
                                                    ", computed value: ", strMaxValue);
                        Assert.GreaterOrEqual(fuelTypeIntValue, typeSensor.MinValue, message);
                    }
                    else
                    {
                        Assert.Fail(string.Concat("Unknown sensor type: ", sensor.GetType().FullName));
                    }
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
            Assert.AreEqual(Sensor.SplitRawValue(testString).Length, 3);

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
