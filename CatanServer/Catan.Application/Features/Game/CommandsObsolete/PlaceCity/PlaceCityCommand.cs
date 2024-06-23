using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.CommandsObsolete.PlaceCity
{
    public class PlaceCityCommand : IRequest<CityResponse>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public int Position { get; set; }
    }
}
