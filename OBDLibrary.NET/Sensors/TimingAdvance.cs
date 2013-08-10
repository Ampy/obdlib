using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class TimingAdvance : NumericSensor
    {
        private static TimingAdvance _instance;
        private static readonly object SyncLock = new object();

        private TimingAdvance()
        {
        }

        public static TimingAdvance GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new TimingAdvance();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x0E; }
        }

        public override Units Unit
        {
            get { return Units.Degree; }
        }

        public override string Label
        {
            get { return "Timing advance (° relative to #1 cylinder)"; }
        }

        public override double? MinValue
        {
            get { return -64; }
        }

        public override double? MaxValue
        {
            get { return 63.5; }
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
            var value = (splittedValue[2] / 2.0) - 64;
            return (new NumericValue(value));
        }
    }
}

