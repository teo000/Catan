using Catan.Application.Dtos;

namespace Catan.Application.Responses
{
    public class GameSessionResponse : BaseResponse
    {
        public GameSessionDto GameSession { get; set; }
    }
}
