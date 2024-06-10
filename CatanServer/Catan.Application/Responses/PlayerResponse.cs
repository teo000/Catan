using Catan.Application.Dtos;

namespace Catan.Application.Responses
{
    public class PlayerResponse : BaseResponse
    {
        public PlayerDto Player { get; set; }
    }
}
