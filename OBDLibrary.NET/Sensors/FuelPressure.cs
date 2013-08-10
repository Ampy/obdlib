using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class FuelPressure : NumericSensor
    {
        private static FuelPressure _instance;
        private static readonly object SyncLock = new object();

        private FuelPressure()
        {
        }

        public static FuelPressure GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new FuelPressure();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x0A; }
        }

        public override Units Unit
        {
            get { return Units.kPa; }
        }

        public override string Label
        {
            get { return "Fuel pressure (kPa (gauge))"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 765; }
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
            var value = splittedValue[2] * 3;
            return (new NumericValue(value));
        }
    }
}

