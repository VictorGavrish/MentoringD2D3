namespace PowerStateManagement
{
    using System;

    public class BatteryState
    {
        private readonly PowerStateManager.SystemBatteryState batteryState;

        private bool[] spare;

        internal BatteryState(PowerStateManager.SystemBatteryState batteryState)
        {
            this.batteryState = batteryState;
        }

        public bool AcOnLine => Convert.ToBoolean(this.batteryState.AcOnLine);

        public bool BatteryPresent => Convert.ToBoolean(this.batteryState.BatteryPresent);

        public bool Charging => Convert.ToBoolean(this.batteryState.Charging);

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
                            Convert.ToBoolean(this.batteryState.Spare1), Convert.ToBoolean(this.batteryState.Spare2), 
                            Convert.ToBoolean(this.batteryState.Spare3), Convert.ToBoolean(this.batteryState.Spare4)
                        };
                }

                return this.spare;
            }
        }
    }
}