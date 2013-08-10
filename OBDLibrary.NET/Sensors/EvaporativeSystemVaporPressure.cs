using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class EvaporativeSystemVaporPressure : NumericSensor
    {
        private static EvaporativeSystemVaporPressure _instance;
        private static readonly object SyncLock = new object();

        private EvaporativeSystemVaporPressure()
        {
        }

        public static EvaporativeSystemVaporPressure GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new EvaporativeSystemVaporPressure();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x32; }
        }

        public override Units Unit
        {
            get { return Units.Pa; }
        }

        public override string Label
        {
            get { return "Evaporative System Vapor Pressure"; }
        }

        public override double? MinValue
        {
            get { return -8192; }
        }

        public override double? MaxValue
        {
            get { return 8191.75; }
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
            var value = ((a * 256.0) + b) / 4.0;
            return (new NumericValue(value));
        }
    }
}

