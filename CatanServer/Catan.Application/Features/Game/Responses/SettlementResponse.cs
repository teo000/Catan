using Catan.Application.Dtos;
using Catan.Application.Responses;

namespace Catan.Application.Features.Game.Responses
{
	public class SettlementResponse : BaseResponse
    {
        public SettlementDto Settlement { get; set; }
    }
}
