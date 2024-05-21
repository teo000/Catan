using Catan.Application.Dtos;
using Catan.Application.Responses;

namespace Catan.Application.Features.Game.Responses
{
    public class GameSessionResponse : BaseResponse
    {
        public GameSessionDto GameSession { get; set; }
    }
}
