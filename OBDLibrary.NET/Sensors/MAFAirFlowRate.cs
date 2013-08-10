using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class MAFAirFlowRate : NumericSensor
    {
        private static MAFAirFlowRate _instance;
        private static readonly object SyncLock = new object();

        private MAFAirFlowRate()
        {
        }

        public static MAFAirFlowRate GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new MAFAirFlowRate();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x10; }
        }

        public override Units Unit
        {
            get { return Units.GramsPerSecond; }
        }

        public override string Label
        {
            get { return "MAF air flow rate (grams/sec)"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 655.35; }
        }

        internal override byte BytesCount
        {
            get { return 2; }
        }

        internal override SensorValueComputationType ComputationType
        {
            get { return SensorValueComputationType.Formula; }
        }

        internal override NumericValue GetComputedValue(string hexValues)
        {
            var splittedValue = SplitRawValue(hexValues);
            var a = splittedValue[2];
            var b = splittedValue[3];
            var value = ((a * 256) + b) / 100.0;
            return (new NumericValue(value));
        }
    }
}

