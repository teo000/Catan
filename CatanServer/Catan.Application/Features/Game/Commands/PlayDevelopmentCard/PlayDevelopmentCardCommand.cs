using Catan.Application.Responses;
using MediatR;

namespace Catan.Application.Features.Game.Commands.PlayDevelopmentCard
{
	public class PlayDevelopmentCardCommand : IRequest<GameSessionResponse>
	{
		public Guid GameId { get; set; }
		public Guid PlayerId { get; set; }
		public string DevelopmentType {  get; set; }
		public int? Position { get; set; }
	}
}
