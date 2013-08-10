using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class IntakeAirTemperature : NumericSensor
    {
        private static IntakeAirTemperature _instance;
        private static readonly object SyncLock = new object();

        private IntakeAirTemperature()
        {
        }

        public static IntakeAirTemperature GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new IntakeAirTemperature();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x0F; }
        }

        public override Units Unit
        {
            get { return Units.CelsiusDegree; }
        }

        public override string Label
        {
            get { return "Intake air temperature (°C)"; }
        }

        public override double? MinValue
        {
            get { return -40; }
        }

        public override double? MaxValue
        {
            get { return 215; }
        }

        internal override byte BytesCount
        {
            get { return 1; }
        }

        internal override SensorValueComputationType ComputationType
        {
            get { return SensorValueComputationType.Formula; }
        }

        internal override NumericValue GetComputedValue(string hexValues)
        {
            var splittedValue = SplitRawValue(hexValues);
            var value = splittedValue[2] - 40;
            return (new NumericValue(value));
        }
    }
}

