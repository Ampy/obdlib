using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class FuelLevelInput : NumericSensor
    {
        private static FuelLevelInput _instance;
        private static readonly object SyncLock = new object();

        private FuelLevelInput()
        {
        }

        public static FuelLevelInput GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new FuelLevelInput();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x2F; }
        }

        public override Units Unit
        {
            get { return Units.Percent; }
        }

        public override string Label
        {
            get { return "Commanded evaporative purge"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 100; }
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
            var value = splittedValue[2] * 100 / 255;
            return (new NumericValue(value));
        }
    }
}

