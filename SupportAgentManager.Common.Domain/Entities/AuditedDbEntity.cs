using System;

namespace SupportAgentManager.Common.Domain.Entities
{
    public abstract class AuditedDbEntity : BaseEntity
    {
        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int LastUpdatedByUserId { get; set; }

        public DateTime LastUpdatedOnUtc { get; set; }
    }
}
