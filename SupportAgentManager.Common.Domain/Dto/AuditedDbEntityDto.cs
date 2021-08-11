using System;
using System.ComponentModel.DataAnnotations;

namespace SupportAgentManager.Common.Domain.Dto
{
    public class AuditedDbEntityDto : BaseEntityDto
    {
        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int LastUpdatedByUserId { get; set; }

        public DateTime LastUpdatedOnUtc { get; set; }

        [Timestamp]
        public byte[] ConcurrencyKey { get; set; }
    }
}
