using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wavefront.AUV.API.Enums;
using Wavefront.AUV.API.Interface;

namespace TestProject.models
{
    public class CelsiusSensor : IAUVSensor
    {
        public int SensorId { get; set; }

        public eTemperature TemperatureUnit => eTemperature.Celsius;

        public ePressure PressureUnit { get; set; }

        public double GetPressure()
        {
            return 1.123456789012345;
        }

        public double GetTemperature()
        {
            return 30.123456789012345;
        }
    }
}
