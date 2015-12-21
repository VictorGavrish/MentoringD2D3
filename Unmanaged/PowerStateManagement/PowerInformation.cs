namespace PowerStateManagement
{
    public class PowerInformation
    {
        private readonly PowerStateManager.SystemPowerInformation powerInformation;

        internal PowerInformation(PowerStateManager.SystemPowerInformation powerInformation)
        {
            this.powerInformation = powerInformation;
        }

        public CoolingMode CoolingMode => this.powerInformation.CoolingMode;

        public uint Idleness => this.powerInformation.Idleness;

        public uint MaxIdlenessAllowed => this.powerInformation.MaxIdlenessAllowed;

        public uint TimeRemaining => this.powerInformation.TimeRemaining;
    }
}