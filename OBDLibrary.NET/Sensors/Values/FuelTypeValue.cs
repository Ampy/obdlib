namespace OBD2.Library.Sensors.Values
{
    public class FuelTypeValue : SensorValue
    {
        private readonly Fuel _value;

        public Fuel Value
		{
			get
			{
				return _value;
			}
		}

        public FuelTypeValue()
        {
        }

        public FuelTypeValue(Fuel value)
        {
			_value = value;
		}
    }
}
