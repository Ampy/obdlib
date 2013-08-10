using System;

namespace OBD2.Library.Sensors.Values
{
	public abstract class SensorValue
	{
		private readonly DateTime _timeStamp;

		public DateTime TimeStamp
		{
			get
			{
				return _timeStamp;
			}
		}

	    protected SensorValue ()
		{
			_timeStamp = DateTime.Now;
		}
	}
}

