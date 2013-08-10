using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class EngineRPM : NumericSensor
    {

        private static EngineRPM _instance;
        private static readonly object SyncLock = new object();

        private EngineRPM()
        {
        }

        public static EngineRPM GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new EngineRPM();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x0C; }
        }

        public override Units Unit
        {
            get { return Units.RPM; }
        }

        public override string Label
        {
            get { return "Engine RPM"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 16383.75; }
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
            var value = ((a * 256) + b) / 4.0;
            return (new NumericValue(value));
        }
    }
}

