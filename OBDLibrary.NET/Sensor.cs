using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OBD2.Library.Sensors.ResultType;

namespace OBD2.Library
{
    /// <summary>
    /// Infos from http://en.wikipedia.org/wiki/OBD-II_PIDs
    /// </summary>
    public abstract class Sensor
    {
        // ReSharper disable UnusedMember.Global
        #region "abstract properties/methods"

        public abstract byte PID { get; }

        public abstract Units Unit { get; }

        public abstract string Label { get; }

        public abstract double? MinValue { get; }

        public abstract double? MaxValue { get; }

        internal abstract byte BytesCount { get; }

        internal abstract SensorValueComputationType ComputationType { get; }

        internal static byte[] SplitRawValue(string value)
        {
            return
                value.Split(new[] {Constants.SPLIT_VALUE_CHAR}, StringSplitOptions.RemoveEmptyEntries)
                     .Select(s => Convert.ToByte(s, 16)).ToArray();
        }
        #endregion

        #region "List & instances"
        private static List<Sensor> _list;

        public static List<Sensor> List
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<Sensor>();
                    var typesList = Assembly.GetExecutingAssembly().GetTypes();
                    var sensorBaseClass = typeof(Sensor);
                    var baseClasses = new List<Type>();
                    foreach (var type in typesList)
                    {
                        if (!type.IsSubclassOf(sensorBaseClass)) continue;
                        baseClasses.Add(type);
                    }

                    foreach (var type in typesList)
                    {
                        foreach (var baseType in baseClasses)
                        {
                            if (!type.IsSubclassOf(baseType)) continue;
                            var methodInfo = type.GetMethod("GetInstance", BindingFlags.Static | BindingFlags.Public);
                            _list.Add((Sensor)methodInfo.Invoke(null, null));
                        }
                    }
                }
                return _list;
            }
        }

        public static Sensor AbsoluteLoadValue
        {
            get { return Sensors.AbsoluteLoadValue.GetInstance(); }
        }

        public static Sensor BarometricPressure
        {
            get { return Sensors.BarometricPressure.GetInstance(); }
        }

        public static Sensor CalculatedEngineLoad
        {
            get { return Sensors.CalculatedEngineLoad.GetInstance(); }
        }

        public static Sensor CommandedEGR
        {
            get { return Sensors.CommandedEGR.GetInstance(); }
        }

        public static Sensor CommandedEvaporativePurge
        {
            get { return Sensors.CommandedEvaporativePurge.GetInstance(); }
        }

        public static Sensor ControlModuleVoltage
        {
            get { return Sensors.ControlModuleVoltage.GetInstance(); }
        }

        public static Sensor DistanceTraveledSinceCodesCleared
        {
            get { return Sensors.DistanceTraveledSinceCodesCleared.GetInstance(); }
        }

        public static Sensor EGRError
        {
            get { return Sensors.EGRError.GetInstance(); }
        }

        public static Sensor EngineCoolantTemperature
        {
            get { return Sensors.EngineCoolantTemperature.GetInstance(); }
        }

        public static NumericSensor EngineRPM
        {
            get { return Sensors.EngineRPM.GetInstance(); }
        }

        public static Sensor EthanolFuelPercentage
        {
            get { return Sensors.EthanolFuelPercentage.GetInstance(); }
        }

        public static Sensor FuelLevelInput
        {
            get { return Sensors.FuelLevelInput.GetInstance(); }
        }

        public static Sensor FuelPressure
        {
            get { return Sensors.FuelPressure.GetInstance(); }
        }

        public static Sensor SensorFuelType
        {
            get { return Sensors.FuelType.GetInstance(); }
        }

        public static Sensor IntakeAirTemperature
        {
            get { return Sensors.IntakeAirTemperature.GetInstance(); }
        }

        public static Sensor IntakeManifoldAbsolutePressure
        {
            get { return Sensors.IntakeManifoldAbsolutePressure.GetInstance(); }
        }

        public static Sensor MAFAirFlowRate
        {
            get { return Sensors.MAFAirFlowRate.GetInstance(); }
        }

        public static Sensor NumberOfWarmUpsSinceCodesCleared
        {
            get { return Sensors.NumberOfWarmUpsSinceCodesCleared.GetInstance(); }
        }

        public static Sensor RunTimeSinceEngineStart
        {
            get { return Sensors.RunTimeSinceEngineStart.GetInstance(); }
        }

        public static Sensor ThrottlePosition
        {
            get { return Sensors.ThrottlePosition.GetInstance(); }
        }

        public static Sensor TimingAdvance
        {
            get { return Sensors.TimingAdvance.GetInstance(); }
        }

        public static NumericSensor VehicleSpeed
        {
            get { return Sensors.VehicleSpeed.GetInstance(); }
        }

        #endregion
        // ReSharper restore UnusedMember.Global
    }
}

