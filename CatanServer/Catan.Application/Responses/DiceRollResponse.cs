using Catan.Application.Dtos;
using Catan.Domain.Data;

namespace Catan.Application.Responses
{
    public class DiceRollResponse : BaseResponse
    {
        public DiceRollDto DiceRoll { get; set; }
	}
}