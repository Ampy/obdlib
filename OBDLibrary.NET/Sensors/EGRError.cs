using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class EGRError : NumericSensor
    {
        private static EGRError _instance;
        private static readonly object SyncLock = new object();

        private EGRError()
        {
        }

        public static EGRError GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new EGRError();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x2D; }
        }

        public override Units Unit
        {
            get { return Units.Percent; }
        }

        public override string Label
        {
            get { return "EGR Error"; }
        }

        public override double? MinValue
        {
            get { return -100; }
        }

        public override double? MaxValue
        {
            get { return 99.21875; }
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
            var value = (splittedValue[2] - 128) * 100.0 / 128.0;
            return (new NumericValue(value));
        }
    }
}

