namespace OBD2.Library.Sensors.Values
{
	public class NumericValue: SensorValue
	{
		private readonly double? _value;

        public double? Value
		{
			get
			{
				return _value;
			}
		}

        public NumericValue()
        {
        }

        public NumericValue(double? value)
        {
			_value = value;
		}
	}
}

