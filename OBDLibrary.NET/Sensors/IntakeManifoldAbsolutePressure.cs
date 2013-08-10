using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class IntakeManifoldAbsolutePressure : NumericSensor
    {
        private static IntakeManifoldAbsolutePressure _instance;
        private static readonly object SyncLock = new object();

        private IntakeManifoldAbsolutePressure()
        {
        }

        public static IntakeManifoldAbsolutePressure GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new IntakeManifoldAbsolutePressure();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x0B; }
        }

        public override Units Unit
        {
            get { return Units.kPa; }
        }

        public override string Label
        {
            get { return "Intake manifold absolute pressure (kPa (absolute))"; }
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
            get { return 2; }
        }

        internal override SensorValueComputationType ComputationType
        {
            get
            {
                return SensorValueComputationType.Formula;
            }
        }

        internal override NumericValue GetComputedValue(string hexValues)
        {
            var splittedValue = SplitRawValue(hexValues);
            var value = splittedValue[2];
            return (new NumericValue(value));
        }
    }
}

