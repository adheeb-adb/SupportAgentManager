using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Dto.ConfigInformation
{
    /// <summary>
    /// Defines the shift configs of each shift start.end times as strings.
    /// </summary>
    public class ShiftConfigInformation
    {
        public SupportShiftType ShiftType { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}
