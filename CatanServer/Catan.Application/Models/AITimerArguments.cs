using Catan.Domain.Entities;

namespace Catan.Application.Models
{
    public class AITimerArguments
    {
        public Guid GameSessionId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
