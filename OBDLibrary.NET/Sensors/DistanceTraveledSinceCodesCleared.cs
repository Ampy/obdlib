using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class DistanceTraveledSinceCodesCleared : NumericSensor
    {
        private static DistanceTraveledSinceCodesCleared _instance;
        private static readonly object SyncLock = new object();

        private DistanceTraveledSinceCodesCleared()
        {
        }

        public static DistanceTraveledSinceCodesCleared GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new DistanceTraveledSinceCodesCleared();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x31; }
        }

        public override Units Unit
        {
            get { return Units.Km; }
        }

        public override string Label
        {
            get { return "Distance traveled since codes cleared"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 65280; }
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
            var values = new byte[BytesCount];
            for (var i = 0; i < BytesCount; i++)
                values[i] = splittedValue[i + 2];
            var value = (values[0] * 255) + values[1];
            return (new NumericValue(value));
        }
    }
}

