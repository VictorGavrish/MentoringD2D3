namespace Program
{
    using System;

    using PowerStateManagement;

    internal class Program
    {
        private static void Main(string[] args)
        {
            IPowerStateManager pm = new PowerStateManager();

            Console.WriteLine(pm.GetLastSleepTime());
            Console.WriteLine(pm.GetLastWakeTime());
            var batteryState = pm.GetSystemBatteryState();
            var powerInfo = pm.GetSystemPowerInformation();
            try
            {
                pm.ReserveHiberFile(false);
            }
            catch (PowerManagementException pme)
            {
                Console.WriteLine(pme.Message);
            }
            ////pm.Hibernate();
            ////pm.Sleep();
        }
    }
}