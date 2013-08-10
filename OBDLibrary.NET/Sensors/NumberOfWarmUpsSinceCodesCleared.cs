using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class NumberOfWarmUpsSinceCodesCleared : NumericSensor
    {
        private static NumberOfWarmUpsSinceCodesCleared _instance;
        private static readonly object SyncLock = new object();

        private NumberOfWarmUpsSinceCodesCleared()
        {
        }

        public static NumberOfWarmUpsSinceCodesCleared GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new NumberOfWarmUpsSinceCodesCleared();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x30; }
        }

        public override Units Unit
        {
            get { return Units.Number; }
        }

        public override string Label
        {
            get { return "# of warm-ups since codes cleared"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 255; }
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
            var value = splittedValue[2];
            return (new NumericValue(value));
        }
    }
}

