using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wavefront.AUV.API.Enums;
using Wavefront.AUV.API.Interface;

namespace TestProject.models
{
    public class VanillaSensor : IAUVSensor
    {
        public int SensorId { get; set; } = 0;

        public eTemperature TemperatureUnit { get; set; } =  eTemperature.Unknown;

        public ePressure PressureUnit { get; set; } = ePressure.Unknown;

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
