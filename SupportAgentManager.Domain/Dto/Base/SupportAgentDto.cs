using SupportAgentManager.Common.Domain.Dto;
using SupportAgentManager.Domain.Enums;

namespace SupportAgentManager.Domain.Dto.Base
{
    public class SupportAgentDto : AuditedDbEntityDto
    {
        public string Name { get; set; }

        public SeniorityLevel SeniorityLevel { get; set; }

        public SupportShiftType ShiftType { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int ConcurrentCapacity { get; set; }

        public int ActiveChats { get; set; }
    }
}
