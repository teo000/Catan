using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Game.Commands.CreateGame
{
    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, GameSessionResponse>
    {
        private readonly GameSessionManager _gameSessionManager;
        private readonly IMapper _mapper;
        public CreateGameCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
        {
            _gameSessionManager = gameSessionManager;
            _mapper = mapper;
        }

        public async Task<GameSessionResponse> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var players = new List<Player>();

            foreach (var playerName in request.playerNames)
            {
                var player = Player.Create(playerName);

                if (!player.IsSuccess)
                    return new GameSessionResponse()
                    {
                        Success = false,
                        ValidationErrors = new List<string>() { player.Error }
                    };

                players.Add(player.Value);
            }

            var result = _gameSessionManager.CreateGameSession(players);
            if (!result.IsSuccess)
            {
                return new GameSessionResponse()
                {
                    Success = false,
                    ValidationErrors = new List<string>() { result.Error }
                };
            }

            return new GameSessionResponse()
            {
                Success = true,
				GameSession = _mapper.Map<GameSessionDto>(result.Value)

				//GameSession = new GameSessionDto()
				//{
				//    Id = result.Value.Id,
				//    Players = result.Value.Players,
				//    GameStatus = result.Value.GameStatus.ToString(),
				//    Map = result.Value.GameMap,
				//    TurnPlayerIndex = result.Value.TurnPlayerIndex,
				//    TurnEndTime = result.Value.TurnEndTime,
				//}
			};
        }
    }
}
