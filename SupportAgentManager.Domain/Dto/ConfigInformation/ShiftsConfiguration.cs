namespace SupportAgentManager.Domain.Dto.ConfigInformation
{
    /// <summary>
    /// Defines the shifts.
    /// </summary>
    public class ShiftsConfiguration
    {
        public ShiftConfigInformation BusinessShift { get; set; }

        public ShiftConfigInformation MidShift { get; set; }

        public ShiftConfigInformation NightShift { get; set; }
    }
}
