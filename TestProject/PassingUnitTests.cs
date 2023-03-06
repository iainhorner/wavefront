using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.models;
using Wavefront.AUV.API.Interface;
using WaveFront.Interfaces;
using WaveFront.lib;

namespace TestProject
{
    [TestClass]
    public class PassingUnitTests
    {
        private readonly IList<IAUVSensor> AUVSensors;
        private readonly IAUVSensorReader SensorReader;
        public PassingUnitTests()
        {
            AUVSensors = new List<IAUVSensor>()
            {
               new CelsiusSensor {
                   SensorId = 1,
                   PressureUnit = Wavefront.AUV.API.Enums.ePressure.PSI
               }
            };

            SensorReader = new AUVSensorReader(AUVSensors);
        }
        [TestMethod]
        public void Test_Psi_Celsius_Default_DecimalPlaces()
        {

            SensorReader.SetTemperatureUnits("C");
            SensorReader.SetPressureUnits("P");

            var SensorData = SensorReader.GetSensorData();

            Assert.AreEqual(SensorData[0],"SensorId: 1 Temp: 30.1234567890123444°C Pressure: 1.1234567890123450 psi");
        }
        [TestMethod]
        public void Test_Psi_Fahrenheit_Default_DecimalPlaces()
        {

            SensorReader.SetTemperatureUnits("F");
            SensorReader.SetPressureUnits("P");

            var SensorData = SensorReader.GetSensorData();

            Assert.AreEqual(SensorData[0], "SensorId: 1 Temp: 86.2222222202222213°F Pressure: 1.1234567890123450 psi");
        }

        [TestMethod]
        public void Test_Kpa_Fahrenheit_Default_DecimalPlaces()
        {

            SensorReader.SetTemperatureUnits("F");
            SensorReader.SetPressureUnits("K");

            var SensorData = SensorReader.GetSensorData();

            Assert.AreEqual(SensorData[0], "SensorId: 1 Temp: 86.2222222202222213°F Pressure: 7.7462345602401186 kPa");
        }

        [TestMethod]
        public void Test_Psi_Celsius_TwoDecimalPlaces()
        {

            SensorReader.SetTemperatureUnits("C");
            SensorReader.SetPressureUnits("P");
            SensorReader.SetNumDecimalPlaces(2);

            var SensorData = SensorReader.GetSensorData();

            Assert.AreEqual(SensorData[0], "SensorId: 1 Temp: 30.12°C Pressure: 1.12 psi");
        }
    }
}