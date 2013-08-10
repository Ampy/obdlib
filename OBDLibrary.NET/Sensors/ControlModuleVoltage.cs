using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class ControlModuleVoltage : NumericSensor
    {
        private static ControlModuleVoltage _instance;
        private static readonly object SyncLock = new object();

        private ControlModuleVoltage()
        {
        }

        public static ControlModuleVoltage GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new ControlModuleVoltage();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x42; }
        }

        public override Units Unit
        {
            get { return Units.Volts; }
        }

        public override string Label
        {
            get { return "Control module voltage"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 65.535; }
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
            var value = ((a * 256) + b) / 1000.0;
            return (new NumericValue(value));
        }
    }
}

