using AutoMapper;
using Catan.Application.Dtos;
using Catan.Application.GameManagement;
using Catan.Application.Responses;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using MediatR;

namespace Catan.Application.Features.Game.Commands.DiscardHalf;

public class DiscardHalfCommandHandler : IRequestHandler<DiscardHalfCommand, GameSessionResponse>
{
	private GameSessionManager _gameSessionManager;
	private IMapper _mapper;

	public DiscardHalfCommandHandler(GameSessionManager gameSessionManager, IMapper mapper)
	{
		_gameSessionManager = gameSessionManager;
		_mapper = mapper;
	}

	public async Task<GameSessionResponse> Handle(DiscardHalfCommand request, CancellationToken cancellationToken)
	{
		var validator = new DiscardHalfCommandValidator();
		var validatorResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validatorResult.IsValid)
			return new GameSessionResponse
			{
				Success = false,
				ValidationErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
			};

		var gameSessionResponse = _gameSessionManager.GetGameSession(request.GameId);

		if (!gameSessionResponse.IsSuccess)
		{
			return new GameSessionResponse()
			{
				Success = false,
				ValidationErrors = new List<string>() { gameSessionResponse.Error }
			};
		}

		var gameSession = gameSessionResponse.Value;
		bool playerExists = false;
		Player player = null;

		foreach (var p in gameSession.Players)
		{
			if (p.Id == request.PlayerId)
			{
				playerExists = true;
				player = p;
				break;
			}
		}

		if (!playerExists)
		{
			return new GameSessionResponse()
			{
				Success = false,
				ValidationErrors = new List<string>() { "Player does not exist." }
			};
		}

		if (!player.IsActive)
		{
			return new GameSessionResponse()
			{
				Success = false,
				ValidationErrors = new List<string>() { "Player has been disconnected." }
			};
		}

		var result = gameSession.DiscardHalf(player, GameUtils.ConvertToResourceDictionary( request.ResourceCount));

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
			GameSession = _mapper.Map<GameSessionDto>(gameSession)
		};
	}
	
}


