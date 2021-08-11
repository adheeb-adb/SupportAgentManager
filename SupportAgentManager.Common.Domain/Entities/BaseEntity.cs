using System.ComponentModel.DataAnnotations;

namespace SupportAgentManager.Common.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] ConcurrencyKey { get; set; }
    }
}
