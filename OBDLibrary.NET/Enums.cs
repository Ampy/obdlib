namespace OBD2.Library
{
    // ReSharper disable UnusedMember.Global
    // ReSharper disable InconsistentNaming
    public enum Units
    {
        KmPerHour,
        Percent,
        CelsiusDegree,
        Degree,
        RPM,
        kPa,
        GramsPerSecond,
        Seconds,
        Number,
        Km,
        FuelType,
        Pa,
        kPaAbsolute,
        Volts
    }
    // ReSharper restore InconsistentNaming

	public enum BaudRate
	{
		B2400 = 2400,
		B4800 = 4800,
		B9600 = 9600,
		B19200 = 19200,
		B38400 = 38400,
		B57600 = 57600,
		B115200 = 115200
	}

    public enum SensorValueMode
    {
        /// <summary>
        /// Show current data
        /// </summary>
        Mode1 = 1,
        /// <summary>
        /// Show freeze frame data
        /// </summary>
        Mode2 = 2
    }

    public enum Fuel
    {
        Undefined = -1,
        Gasoline = 1,
        Methanol = 2,
        Ethanol = 3,
        Diesel = 4,
        LPG = 5,
        CNG = 6,
        Propane = 7,
        Electric = 8,
        BifuelRunningGasoline = 9,
        BifuelRunningMethanol = 10,
        BifuelRunningEthanol = 11,
        BifuelRunningLPG = 12,
        BifuelRunningCNG = 13,
        BifuelRunningProp = 14,
        BifuelRunningElectricity = 15,
        BifuelMixedGasElectric = 16,
        HybridGasoline = 17,
        HybridEthanol = 18,
        HybridDiesel = 19,
        HybridElectric = 20,
        HybridMixedFuel = 21,
        HybridRegenerative = 22
    }

	internal enum SensorValueComputationType
	{
		BitEncoded,
		Formula,
		FuelType
	}
    // ReSharper restore UnusedMember.Global
}

