using OBD2.Library.Sensors.ResultType;
using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors
{
    public class FuelType : FuelTypeSensor
    {
        private static FuelType _instance;
        private static readonly object SyncLock = new object();

        private FuelType()
        {
        }

        public static FuelType GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        // ReSharper disable PossibleMultipleWriteAccessInDoubleCheckLocking
                        _instance = new FuelType();
                        // ReSharper restore PossibleMultipleWriteAccessInDoubleCheckLocking
                    }
                }
            }
            return _instance;
        }

        public override byte PID
        {
            get { return 0x51; }
        }

        public override Units Unit
        {
            get { return Units.FuelType; }
        }

        public override string Label
        {
            get { return "Fuel Type"; }
        }

        public override double? MinValue
        {
            get { return 0; }
        }

        public override double? MaxValue
        {
            get { return 765; }
        }

        internal override byte BytesCount
        {
            get { return 1; }
        }

        internal override SensorValueComputationType ComputationType
        {
            get { return SensorValueComputationType.FuelType; }
        }

        internal override FuelTypeValue GetComputedValue(string hexValues)
        {
            var splittedValue = SplitRawValue(hexValues);
            var result = splittedValue[2]*3;
            var sensorFuelType = (Fuel)result;
            return (new FuelTypeValue(sensorFuelType));
        }
    }
}

