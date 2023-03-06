using System.Text;
using Wavefront.AUV.API;
using Wavefront.AUV.API.Enums;
using Wavefront.AUV.API.Interface;
using WaveFront.Helpers;
using WaveFront.Interfaces;

namespace WaveFront.lib
{
    public class AUVSensorReader : IAUVSensorReader
    {
        private int _NumDecimalPlaces;
        private string _NumDecimalPlacesFormat;
        private eTemperature _TempUnits { get; set; }
        private ePressure _PressureUnits { get; set; }
        private readonly IList<IAUVSensor> _SensorList;

        public AUVSensorReader()
        {
            _SensorList = AUVSensorsFactory.Build();
            _NumDecimalPlaces = 16;
            _NumDecimalPlacesFormat = "f" + _NumDecimalPlaces.ToString();
        }

        public AUVSensorReader(IList<IAUVSensor> SensorList)
        {
            _SensorList = SensorList;
            _NumDecimalPlaces = 16;
            _NumDecimalPlacesFormat = "f" + _NumDecimalPlaces.ToString();
        }

        public void SetTemperatureUnits(string units)
        {
            if (units.Equals("Celsius", StringComparison.InvariantCultureIgnoreCase) || units.Equals("C", StringComparison.InvariantCultureIgnoreCase))
                _TempUnits = eTemperature.Celsius;
            if (units.Equals("Fahrenheit", StringComparison.InvariantCultureIgnoreCase) || units.Equals("F", StringComparison.InvariantCultureIgnoreCase))
                _TempUnits = eTemperature.Fahrenheit;

        }
        public void SetPressureUnits(string units)
        {
            if (units.Equals("kpa", StringComparison.InvariantCultureIgnoreCase) || units.Equals("k", StringComparison.InvariantCultureIgnoreCase))
                _PressureUnits = ePressure.kPa;
            if (units.Equals("psi", StringComparison.InvariantCultureIgnoreCase) || units.Equals("p", StringComparison.InvariantCultureIgnoreCase))
                _PressureUnits = ePressure.PSI;

        }
        public void SetNumDecimalPlaces(int numDecimalPlaces)
        {
            _NumDecimalPlaces = numDecimalPlaces;
            _NumDecimalPlacesFormat = "f" + _NumDecimalPlaces.ToString();

        }
        public string[] GetSensorData()
        {
            if (_TempUnits == eTemperature.Unknown)
                throw new Exception("A temperature unit is required");
            if (_PressureUnits == ePressure.Unknown)
                throw new Exception("A pressure unit unit is required");

            var SensorData = new string[_SensorList.Count];
            int i = 0;
            var sb = new StringBuilder();

            foreach (var sensor in _SensorList)
            {
                sb.Clear();
                try
                {
                    if (sensor.TemperatureUnit == eTemperature.Unknown)
                    {
                        sb.Append($"SensorId: {sensor.SensorId} Temp: ERROR A sensor temperature unit is required");
                    }
                    else
                    {
                        if (sensor.TemperatureUnit == eTemperature.Fahrenheit && _TempUnits == eTemperature.Celsius)
                            sb.Append($"SensorId: {sensor.SensorId} Temp: {Converter.FahrenheitToCelsius(sensor.GetTemperature()).ToString(_NumDecimalPlacesFormat)}\u00B0C");
                        else if (sensor.TemperatureUnit == eTemperature.Celsius && _TempUnits == eTemperature.Fahrenheit)
                            sb.Append($"SensorId: {sensor.SensorId} Temp: {Converter.CelsiusToFahrenheit(sensor.GetTemperature()).ToString(_NumDecimalPlacesFormat)}\u00B0F");
                        else if (sensor.TemperatureUnit == eTemperature.Fahrenheit)
                            sb.Append($"SensorId: {sensor.SensorId} Temp: {sensor.GetTemperature().ToString(_NumDecimalPlacesFormat)}\u00B0F");
                        else
                            sb.Append($"SensorId: {sensor.SensorId} Temp: {sensor.GetTemperature().ToString(_NumDecimalPlacesFormat)}\u00B0C");
                    }                       
                }
                catch
                {
                    sb.Append($"ERROR");
                }

                try
                {
                    if (sensor.PressureUnit == ePressure.Unknown)
                    {
                        sb.Append($" Pressure: ERROR A sensor pressureunit unit is required");
                    }
                    else
                    {
                        if (sensor.PressureUnit == ePressure.PSI && _PressureUnits == ePressure.kPa)
                            sb.Append($" Pressure: {Converter.PsiToKpa(sensor.GetPressure()).ToString(_NumDecimalPlacesFormat)} kPa");
                        else if (sensor.PressureUnit == ePressure.kPa && _PressureUnits == ePressure.PSI)
                            sb.Append($" Pressure: {Converter.KpaToPsi(sensor.GetPressure()).ToString(_NumDecimalPlacesFormat)} psi");
                        else if (sensor.PressureUnit == ePressure.PSI && _PressureUnits == ePressure.PSI)
                            sb.Append($" Pressure: {sensor.GetPressure().ToString(_NumDecimalPlacesFormat)} psi");
                        else
                            sb.Append($" Pressure: {sensor.GetPressure().ToString(_NumDecimalPlacesFormat)} kPa");
                    }
                    
                }
                catch
                {
                    sb.Append($"ERROR");
                }

                SensorData[i] = sb.ToString();
                i++;
            }

            return SensorData;
        }
    }
}
