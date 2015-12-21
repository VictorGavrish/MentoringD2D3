namespace PowerStateManagement
{
    using System;
    using System.Runtime.InteropServices;

    public class PowerStateManager
    {
        private enum PowerInformationLevel
        {
            LastSleepTime = 15, 

            LastWakeTime = 14, 

            SystemBatteryState = 5, 

            SystemPowerInformation = 12, 

            SystemReserveHiberFile = 10
        }

        public ulong GetLastSleepTime()
        {
            ulong ticks;
            var status = CallNtPowerInformation(
                PowerInformationLevel.LastSleepTime, 
                IntPtr.Zero, 
                0, 
                out ticks, 
                Marshal.SizeOf<ulong>());

            return status == NtStatus.Success ? ticks : 0;
        }

        public ulong GetLastWakeTime()
        {
            ulong ticks;
            var status = CallNtPowerInformation(
                PowerInformationLevel.LastWakeTime, 
                IntPtr.Zero, 
                0, 
                out ticks, 
                Marshal.SizeOf<ulong>());

            return status == NtStatus.Success ? ticks : 0;
        }

        public BatteryState GetSystemBatteryState()
        {
            SystemBatteryState state;
            var status = CallNtPowerInformation(
                PowerInformationLevel.SystemBatteryState, 
                IntPtr.Zero, 
                0, 
                out state, 
                Marshal.SizeOf<SystemBatteryState>());

            if (status != NtStatus.Success)
            {
                throw new PowerManagementException($"Unable to retrive system battery state. Reason: {status}");
            }

            return new BatteryState(state);
        }

        public PowerInformation GetSystemPowerInformation()
        {
            SystemPowerInformation info;
            var status = CallNtPowerInformation(
                PowerInformationLevel.SystemPowerInformation, 
                IntPtr.Zero, 
                0, 
                out info, 
                Marshal.SizeOf<SystemPowerInformation>());

            if (status != NtStatus.Success)
            {
                throw new PowerManagementException($"Unable to retrive system power information. Reason: {status}");
            }

            return new PowerInformation(info);
        }

        public void Hibernate()
        {
            var success = SetSuspendState(true, false, false);

            if (!success)
            {
                var error = (NtStatus)Marshal.GetLastWin32Error();
                throw new PowerManagementException($"Failed to enter hybernate mode. Reason: {error}");
            }
        }

        public void ReserveHiberFile(bool reserve)
        {
            var status = CallNtPowerInformation(
                PowerInformationLevel.SystemReserveHiberFile, 
                ref reserve, 
                Marshal.SizeOf<bool>(), 
                IntPtr.Zero, 
                0);

            if (status != NtStatus.Success)
            {
                throw new PowerManagementException($"Failed reserving hibernation file. Reason: {status}");
            }
        }

        public void Sleep()
        {
            var success = SetSuspendState(false, false, false);

            if (!success)
            {
                var error = (NtStatus)Marshal.GetLastWin32Error();
                throw new PowerManagementException($"Failed to enter sleep mode. Reason: {error}");
            }
        }

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern NtStatus CallNtPowerInformation(
            PowerInformationLevel informationLevel, 
            ref bool inputBuffer, 
            int inputBufferSize, 
            IntPtr outputBuffer, 
            int outputBufferSize);

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern NtStatus CallNtPowerInformation(
            PowerInformationLevel informationLevel, 
            IntPtr inputBuffer, 
            int inputBufferSize, 
            out ulong outputBuffer, 
            int outputBufferSize);

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern NtStatus CallNtPowerInformation(
            PowerInformationLevel informationLevel, 
            IntPtr inputBuffer, 
            int inputBufferSize, 
            out SystemPowerInformation outputBuffer, 
            int outputBufferSize);

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern NtStatus CallNtPowerInformation(
            PowerInformationLevel informationLevel, 
            IntPtr inputBuffer, 
            int inputBufferSize, 
            out SystemBatteryState outputBuffer, 
            int outputBufferSize);

        [DllImport("Powrprof.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SystemBatteryState
        {
            internal readonly byte AcOnLine;

            internal readonly byte BatteryPresent;

            internal readonly byte Charging;

            internal readonly byte Discharging;

            internal readonly byte Spare1;

            internal readonly byte Spare2;

            internal readonly byte Spare3;

            internal readonly byte Spare4;

            internal readonly uint MaxCapacity;

            internal readonly uint RemainingCapacity;

            internal readonly int Rate;

            internal readonly uint EstimatedTime;

            internal readonly uint DefaultAlert1;

            internal readonly uint DefaultAlert2;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SystemPowerInformation
        {
            internal readonly uint MaxIdlenessAllowed;

            internal readonly uint Idleness;

            internal readonly uint TimeRemaining;

            internal readonly CoolingMode CoolingMode;
        }
    }
}