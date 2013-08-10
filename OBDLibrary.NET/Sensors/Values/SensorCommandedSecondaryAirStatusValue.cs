using System;

namespace OBD2.Library
{
	/// <summary>
	/// Sensor commanded secondary air status values.
	/// </summary>
	/// Mode 1 PID 12: A request for this PID returns a single byte of data which describes the secondary air status. Only one bit should ever be set.
	//A0     Upstream of catalytic converter
	//A1     Downstream of catalytic converter
	//A2     From the outside atmosphere or off
	//A3-A7  Always zero
	public class SensorCommandedSecondaryAirStatusValues: SensorValue
	{
		private bool _UpstreamOfCatalyticConverter;
		private bool _DownstreamOfCatalyticConverter;
		private bool _FromTheOutsideAtmosphere;

		public SensorCommandedSecondaryAirStatusValues () : base()
		{

		}
	}
}

