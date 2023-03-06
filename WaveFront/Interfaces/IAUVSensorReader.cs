namespace WaveFront.Interfaces
{
    public interface IAUVSensorReader
    {
        string[] GetSensorData();
        void SetNumDecimalPlaces(int numDecimalPlaces);
        void SetPressureUnits(string units);
        void SetTemperatureUnits(string units);
    }
}