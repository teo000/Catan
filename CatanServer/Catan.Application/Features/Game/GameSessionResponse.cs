using Catan.Application.Responses;

namespace Catan.Application.Features.Game
{
    public class GameSessionResponse : BaseResponse
    {
        public GameSessionDto GameSession { get; set; }
    }
}
