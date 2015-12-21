namespace Program
{
    using System;

    using PowerStateManagement;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var pm = new PowerStateManager();

            Console.WriteLine(pm.GetLastSleepTime());
            Console.WriteLine(pm.GetLastWakeTime());
            var batteryState = pm.GetSystemBatteryState();
            var powerInfo = pm.GetSystemPowerInformation();
            pm.ReserveHiberFile(false);
            ////pm.Hibernate();
            ////pm.Sleep();
        }
    }
}