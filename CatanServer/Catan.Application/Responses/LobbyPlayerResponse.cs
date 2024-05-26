using Catan.Application.Dtos;

namespace Catan.Application.Responses
{
    public class LobbyPlayerResponse : BaseResponse
    {
        public LobbyDto Lobby { get; set; }
        public Guid PlayerId { get; set; }
    }
}
