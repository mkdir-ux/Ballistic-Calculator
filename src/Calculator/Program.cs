using System;
using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace BallisticCalculator
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Ballistic Rangefinder Calculator ===");
                Console.WriteLine("(Type 'b' to go back to start)\n");

                Console.WriteLine("Select object unit: 1 - Inch, 2 - Centimeter, 3 - Meter");
                string unitInput = Console.ReadLine()?.ToLower().Trim();

                if (unitInput == "b") continue;

                if (!int.TryParse(unitInput, out int unitChoice) || unitChoice < 1 || unitChoice > 3)
                {
                    Console.WriteLine("Invalid input. Press any key...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Enter the known size (e.g. 2.5): ");
                string sizeInput = Console.ReadLine()?.ToLower().Trim();

                if (sizeInput == "b") continue;

                if (!double.TryParse(sizeInput, out double rawSize) || rawSize <= 0)
                {
                    Console.WriteLine("Invalid size. Press any key...");
                    Console.ReadKey();
                    continue;
                }

                double sizeInMeters = unitChoice switch
                {
                    1 => rawSize * 0.0254,
                    2 => rawSize * 0.01,
                    3 => rawSize,
                    _ => 0
                };
                double sizeInInches = sizeInMeters / 0.0254;

                Console.WriteLine("\nSelect reticle: 1 - MIL (MRAD), 2 - MOA");
                string reticleInput = Console.ReadLine()?.ToLower().Trim();

                if (reticleInput == "b") continue;

                if (!int.TryParse(reticleInput, out int reticleChoice) || (reticleChoice != 1 && reticleChoice != 2))
                {
                    Console.WriteLine("Invalid choice. Press any key...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write(reticleChoice == 1 ? "Enter size in MILs: " : "Enter size in MOA: ");
                string valInput = Console.ReadLine()?.ToLower().Trim();

                if (valInput == "b") continue;

                if (!double.TryParse(valInput, out double reticleValue) || reticleValue <= 0)
                {
                    Console.WriteLine("Invalid value. Press any key...");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("\n--- Results ---");
                string finalResult = "";
                if (reticleChoice == 1)
                {
                    double distanceMeters = (sizeInMeters * 1000) / reticleValue;
                    finalResult = $"Distance: {distanceMeters:F2} Meters / {(distanceMeters * 1.09361):F2} Yards";
                }
                else
                {
                    double distanceYards = (sizeInInches * 95.5) / reticleValue;
                    finalResult = $"Distance: {distanceYards:F2} Yards / {(distanceYards * 0.9144):F2} Meters";
                }

                Console.WriteLine(finalResult);

                SendDataToNetwork(finalResult);

                Console.WriteLine("\n'Enter' to restart, 'Esc' to exit.");
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) break;
            }
        }
        static void SendDataToNetwork(string message)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var result = client.BeginConnect("127.0.0.1", 8888, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                    if (!success) throw new Exception();

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    using (NetworkStream stream = client.GetStream())
                    {
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("[Network] Sent!");
                    }
                }
            }
            catch
            {
                Console.WriteLine("[Network] No server.");
            }
        }
    }
}