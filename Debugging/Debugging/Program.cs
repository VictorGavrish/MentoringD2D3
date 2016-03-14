namespace Debugging
{
    using System;
    using System.Linq;
    using System.Net.NetworkInformation;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var serial = GetSerial();

            Console.WriteLine(serial);

            Console.ReadLine();
        }

        private static string GetSerial()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().First();
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            var dateBytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());

            var numArray = addressBytes
                .Select((b, index) => b ^ dateBytes[index])
                .Select(x => x < 999 ? x * 10 : x)
                .ToArray();

            var result = string.Join("-", numArray.Select(i => i.ToString()));

            return result;
        }
    }
}