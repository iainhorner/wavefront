using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveFront.Helpers
{
    public static class Converter
    {
        public static double CelsiusToFahrenheit(double value)
        {
            return value * 9 / 5 + 32;
        }

        public static double FahrenheitToCelsius(double value)
        {
            return (value - 32) * 5 / 9;
        }

        public static double PsiToKpa(double value)
        {
            return value * 6.895;
        }

        public static double KpaToPsi(double value)
        {
            return value / 6.895;
        }
    }
}
