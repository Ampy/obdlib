using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class RunTimeSinceEngineStart : NumericSensor
    {
        private static RunTimeSinceEngineStart _instance;
        private static readonly object SyncLock = new object();

        private RunTimeSinceEngineStart()
        {
        }

        public static RunTimeSinceEngineStart GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new RunTimeSinceEngineStart();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x1F; }
        }

        public override Units Unit
        {
            get { return Units.Seconds; }
        }

        public override string Label
        {
            get { return "Run time since engine start"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 65535; }
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
            var value = (a * 256) + b;
            return (new NumericValue(value));
        }
    }
}
