namespace PowerStateManagement
{
    public class BatteryState
    {
        private readonly PowerStateManager.SystemBatteryState batteryState;

        private bool[] spare;

        internal BatteryState(PowerStateManager.SystemBatteryState batteryState)
        {
            this.batteryState = batteryState;
        }

        public bool AcOnLine => this.batteryState.AcOnLine != 0;

        public bool BatteryPresent => this.batteryState.BatteryPresent != 0;

        public bool Charging => this.batteryState.Charging != 0;

        public uint DefaultAlert1 => this.batteryState.DefaultAlert1;

        public uint DefaultAlert2 => this.batteryState.DefaultAlert2;

        public uint EstimatedTime => this.batteryState.EstimatedTime;

        public uint MaxCapacity => this.batteryState.MaxCapacity;

        public int Rate => this.batteryState.Rate;

        public uint RemainingCapacity => this.batteryState.RemainingCapacity;

        public bool[] Spare
        {
            get
            {
                if (this.spare == null)
                {
                    this.spare = new[]
                        {
                            this.batteryState.Spare1 != 0, this.batteryState.Spare2 != 0,
                            this.batteryState.Spare3 != 0, this.batteryState.Spare4 != 0
                        };
                }

                return this.spare;
            }
        }
    }
}