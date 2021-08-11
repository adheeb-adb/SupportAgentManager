using SupportAgentManager.Common.Domain.Entities;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Entities
{
    public class SupportAgent : AuditedDbEntity
    {
        public string Name { get; set; }

        public SeniorityLevel SeniorityLevel { get; set; }

        public SupportShiftType ShiftType { get; set; }
    }
}
