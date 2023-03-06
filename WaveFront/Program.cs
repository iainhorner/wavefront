
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaveFront.Helpers;
using WaveFront.Interfaces;
using WaveFront.lib;
namespace FunnelWebApp
{

    class Program
    {
        private static IConfiguration configuration;

        static void Main(string[] args)
        {
            try
            {

                var services = new ServiceCollection();
                ConfigureServices(services);

                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                    configuration = serviceProvider.GetService<IConfiguration>();


                IAUVSensorReader SensorReader = new AUVSensorReader();

                var confTempUnits = configuration.GetValue<string>("Settings:TempUnits");
                bool NoTempFound = true;
                while (NoTempFound)
                {
                    string? tempUnit = TempUnitMenu(confTempUnits);
                    switch (tempUnit.ToUpper())
                    {
                        case "C":
                            SensorReader.SetTemperatureUnits(tempUnit);
                            NoTempFound = false;
                            break;
                        case "F":
                            SensorReader.SetTemperatureUnits(tempUnit);
                            NoTempFound = false;
                            break;
                        case "":
                            SensorReader.SetTemperatureUnits(confTempUnits);
                            NoTempFound = false;
                            break;
                        default:
                            Console.Write("\n\r");
                            ConsoleText.WriteColor($"ERROR: Please enter [C],[F] or [Enter] to use default unit of {confTempUnits}", ("{ERROR:}", ConsoleColor.Red));
                            Console.Write("\n\r\n\r");
                            break;
                    }
                }

                var confPressureUnits = configuration.GetValue<string>("Settings:PressureUnits");
                bool NoPressureUnitFound = true;
                while (NoPressureUnitFound)
                {
                    string? pressureUnit = PressureUnitMenu(confPressureUnits);
                    switch (pressureUnit.ToUpper())
                    {
                        case "P":
                            SensorReader.SetPressureUnits(pressureUnit);
                            NoPressureUnitFound = false;
                            break;
                        case "K":
                            SensorReader.SetPressureUnits(pressureUnit);
                            NoPressureUnitFound = false;
                            break;
                        case "":
                            SensorReader.SetPressureUnits(confPressureUnits);
                            NoPressureUnitFound = false;
                            break;
                        default:
                            Console.Write("\n\r");
                            ConsoleText.WriteColor($"ERROR: Please enter [P],[K] or [Enter] to use default unit of {confPressureUnits}", ("{ERROR:}", ConsoleColor.Red));
                            Console.Write("\n\r\n\r");
                            break;
                    }
                }

                var numDecimalPlaces = (configuration.GetValue<int>("Settings:NumDecimalPlaces") == 0) ? 16 : configuration.GetValue<int>("Settings:NumDecimalPlaces");
                SensorReader.SetNumDecimalPlaces(numDecimalPlaces);

                RunSensorReader(SensorReader, configuration);

                Console.WriteLine("Press Enter to quit.");
                Console.Write("\n\r\n\r");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void RunSensorReader(IAUVSensorReader aUVSensorReader, IConfiguration configuration)
        {
            try
            {
                var timerSeconds = (configuration.GetValue<int>("Settings:PollIntervalSeconds") == 0) ? 1 : configuration.GetValue<int>("Settings:PollIntervalSeconds");
                var timer = new PeriodicTimer(TimeSpan.FromSeconds(timerSeconds));

                while (await timer.WaitForNextTickAsync())
                {
                    foreach (var sensorData in aUVSensorReader.GetSensorData())
                    {
                        ConsoleText.WriteColor(sensorData, ("{SensorId:}", ConsoleColor.Yellow), ("{Temp:}", ConsoleColor.Yellow), ("{Pressure:}", ConsoleColor.Yellow), ("{ERROR}", ConsoleColor.Red));
                        Console.Write("\n\r");
                    }
                    Console.Write("\n\r");
                    Console.Write("\n\r");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureServices(ServiceCollection services)

        {
            var appSettingsFile = "appsettings.json";

            // Build configuration
            configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettingsFile, optional: false, reloadOnChange: true)
            .Build();

            services.AddSingleton(configuration);
        }

        private static string? TempUnitMenu(string? confTempUnits)
        {
            Console.WriteLine("Enter required temperature units:");
            Console.WriteLine("Celsius [c]");
            Console.WriteLine("Fahrenheit [f]");
            Console.WriteLine($"Use default unit of {confTempUnits} [Enter]");

            var tempUnit = Console.ReadLine();
            return tempUnit;
        }

        private static string? PressureUnitMenu(string? confPressureUnits)
        {
            Console.WriteLine("Enter required pressure units:");
            Console.WriteLine("psi [p]");
            Console.WriteLine("kPa [k]");
            Console.WriteLine($"Use default unit of {confPressureUnits} [Enter]");

            var pressureUnit = Console.ReadLine();
            return pressureUnit;
        }
    }
}
