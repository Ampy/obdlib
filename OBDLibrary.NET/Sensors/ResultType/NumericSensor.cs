using OBD2.Library.Sensors.Values;

namespace OBD2.Library.Sensors.ResultType
{
    public abstract class NumericSensor : Sensor
    {
        internal abstract NumericValue GetComputedValue(string hexValues);
    }
}
