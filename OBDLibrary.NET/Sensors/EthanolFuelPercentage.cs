using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class EthanolFuelPercentage : NumericSensor
    {
        private static EthanolFuelPercentage _instance;
        private static readonly object SyncLock = new object();

        private EthanolFuelPercentage()
        {
        }

        public static EthanolFuelPercentage GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new EthanolFuelPercentage();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x52; }
        }

        public override Units Unit
        {
            get { return Units.Percent; }
        }

        public override string Label
        {
            get { return "Ethanol fuel %"; }
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
            var value = splittedValue[2] * 100.0 / 255.0;
            return (new NumericValue(value));
        }
    }
}
