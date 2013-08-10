using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors.ResultType
{
    public abstract class FuelTypeSensor : Sensor
    {
        internal abstract FuelTypeValue GetComputedValue(string hexValues);
    }
}
