using Catan.Domain.Entities;

namespace Catan.Application.GameManagement.Misc
{
    public class AITimerArguments
    {
        public Guid GameSessionId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
