using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OBD2.Library
{
    public class SensorSingleValue: SensorValue
    {
        private readonly double? _value;

        public double? Value
        {
            get
            {
                return _value;
            }
        }

        public SensorSingleValue(double? value)
		{
			_timeStamp = DateTime.Now;
			_value = value;
		}
    }
}
