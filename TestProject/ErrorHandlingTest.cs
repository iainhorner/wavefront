using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.models;
using Wavefront.AUV.API.Enums;
using Wavefront.AUV.API.Interface;
using WaveFront.lib;

namespace TestProject
{
    [TestClass]
    public class ErrorHandlingTest
    {
        [TestMethod]
        public void Test_Missing_Sensor_Units()
        {
            var AUVSensors = new List<IAUVSensor>()
            {
               new VanillaSensor()
            };

            var SensorReader = new AUVSensorReader(AUVSensors);

            SensorReader.SetTemperatureUnits("C");
            SensorReader.SetPressureUnits("P");

            var SensorData = SensorReader.GetSensorData();

            Assert.AreEqual(SensorData[0], "SensorId: 0 Temp: ERROR A sensor temperature unit is required Pressure: ERROR A sensor pressureunit unit is required");
        }

        [TestMethod]
        public void Test_Missing_Required_Temp_Units()
        {
            var AUVSensors = new List<IAUVSensor>()
            {
               new VanillaSensor(){ PressureUnit = ePressure.PSI, TemperatureUnit = eTemperature.Celsius}
            };

            var SensorReader = new AUVSensorReader(AUVSensors);


            SensorReader.SetPressureUnits("P");

            try
            {
                var SensorData = SensorReader.GetSensorData();
               
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "A temperature unit is required"); ;
            }
        }

        [TestMethod]
        public void Test_Missing_Required_Pressure_Units()
        {
            var AUVSensors = new List<IAUVSensor>()
            {
               new VanillaSensor(){ PressureUnit = ePressure.PSI, TemperatureUnit = eTemperature.Celsius}
            };

            var SensorReader = new AUVSensorReader(AUVSensors);


            SensorReader.SetTemperatureUnits("C");

            try
            {
                var SensorData = SensorReader.GetSensorData();

            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "A pressure unit unit is required"); ;
            }
        }
    }
}
