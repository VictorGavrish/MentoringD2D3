namespace PowerStateManagement
{
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    [Guid("31f0f783-729f-40a2-b270-1da29ca9c4b7")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerStateManager
    {
        ulong GetLastSleepTime();

        ulong GetLastWakeTime();

        BatteryState GetSystemBatteryState();

        PowerInformation GetSystemPowerInformation();

        void Hibernate();

        void ReserveHiberFile(bool reserve);

        void Sleep();
    }
}