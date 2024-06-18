using System.Collections.Concurrent;
using AutoMapper;
using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Application.Moves;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Domain.Entities.GamePieces;
using MediatR;

namespace Catan.Application.GameManagement;

public class GameSessionManager
{
	private readonly IAIService _aIService;
	private readonly Lazy<IMapper> _mapper;

	private ConcurrentDictionary<Guid, GameSession> gameSessions = new ConcurrentDictionary<Guid, GameSession>();
	private ConcurrentDictionary<Guid, Timer> sessionTimers = new ConcurrentDictionary<Guid, Timer>();

	public GameSessionManager(IAIService aIService, Lazy<IMapper> mapper)
	{
		_aIService = aIService;
		_mapper = mapper;
	}

	public Result<GameSession> CreateGameSession(List<Player> players)
	{
		var result = GameSession.Create(players);
		if (result.IsSuccess)
		{
			var game = result.Value;
			if (gameSessions.TryAdd(game.Id, game))
			{
				StartTurnTimer(game.Id);
			}
			else
			{
				return Result<GameSession>.Failure("Failed to add game session.");
			}
		}
		return result;
	}

	public Result<GameSession> GetGameSession(Guid guid)
	{
		if (gameSessions.TryGetValue(guid, out var game))
		{
			return Result<GameSession>.Success(game);
		}
		else
		{
			return Result<GameSession>.Failure("Game does not exist in current context");
		}
	}

	public Result<GameSession> RemoveGameSession(Guid guid)
	{
		if (gameSessions.TryRemove(guid, out var game))
		{
			if (sessionTimers.TryRemove(guid, out var timer))
			{
				timer.Dispose();
			}
			return Result<GameSession>.Success(game);
		}
		else
		{
			return Result<GameSession>.Failure("Game does not exist in current context");
		}
	}

	private void StartTurnTimer(Guid sessionId)
	{
		if (sessionTimers.TryRemove(sessionId, out Timer? oldTimer))
		{
			oldTimer.Dispose();
		}

		var timer = new Timer(OnTurnTimeout, sessionId, 1000 * GameInfo.TURN_DURATION, 1000 * GameInfo.TURN_DURATION);
		sessionTimers[sessionId] = timer;

		var sessionResult = GetGameSession(sessionId);
		var session = sessionResult.Value;

		var currentPlayer = session.GetTurnPlayer();
		if (currentPlayer.IsAI)
			Task.Run(() => HandleAIPlayer(session, currentPlayer));
	}

	private void OnTurnTimeout(object? state)
	{
		Console.WriteLine("gata");
		var sessionId = (Guid)state;
		var sessionResult = GetGameSession(sessionId);

		if (!sessionResult.IsSuccess)
		{
			return;
		}

		var session = sessionResult.Value;

		EndPlayerTurn(session);
	}

	public void EndPlayerTurn(GameSession session)
	{
		lock (session)
		{
			var player = session.GetTurnPlayer();
			if (session.IsInBeginningPhase())
			{
				if (player.Roads.Count < session.Round
					|| player.Settlements.Count < session.Round)
					player.Kick();
			}
			else
			{
				if (!session.Dice.RolledThisTurn)
					player.Kick();
			}

			if (session.GetActivePlayers().Count < 2)
				session.MarkAbandoned();

			var winner = session.CheckIfIsWon();
			if (winner is not null)
				session.MarkFinished(winner);


			if (session.GameStatus == GameStatus.InProgress)
			{
				session.EndPlayerTurn();
				StartTurnTimer(session.Id);
			}
		}
	}

	public async Task HandleAIPlayer(GameSession session, Player aIPlayer)
	{
		var mapper = _mapper.Value;

		var aIMovesResult = await _aIService.MakeAIMove(mapper.Map<GameSessionDto>(session));

		if (!aIMovesResult.IsSuccess) 
		{
			aIPlayer.Kick();
			if (session.GetActivePlayers().Count < 2)
				session.MarkAbandoned();

			session.Message = $"{aIPlayer.Name} has been removed due to an internal error.";
			return;
		}

		var aIMoves = aIMovesResult.Value;
		foreach (var move in aIMoves)
		{
			ParseMove(session, aIPlayer, move);
		}


		// handle AI move result here
	}

	private void ParseMove(GameSession session, Player aIPlayer, Move move)
	{
		if (move.GameId != session.Id)
			throw new Exception("AI move caused interal server error.");

		if (!IsMoveTypeDefined(move.MoveType))
			throw new Exception("Move type not supported"); // cred ca ar fi bine sa fac niste logging aici sau ceva 

		var moveType = (MoveType)Enum.Parse(typeof(MoveType), move.MoveType, true);

		if (moveType == MoveType.PlaceSettlement)
			PlaceSettlement(session, aIPlayer, move.Position);
		else if (moveType == MoveType.PlaceRoad)
			PlaceRoad(session, aIPlayer, move.Position);
		else if (moveType == MoveType.PlaceCity)
			PlaceCity(session, aIPlayer, move.Position);
	}

	public Result<Road> PlaceRoad(GameSession session, Player player, int? position)
	{
		if (!position.HasValue) 
		{
			return Result<Road>.Failure("Road position must be specified.");
		}

		var result = session.PlaceRoad(player, position.Value);

		if (!result.IsSuccess || !session.IsInBeginningPhase())
			return result;

		EndPlayerTurn(session);
		return result;
	}

	public Result<Settlement> PlaceSettlement(GameSession session, Player player, int? position)
	{
		if (!position.HasValue)
		{
			return Result<Settlement>.Failure("Settlement position must be specified.");
		}

		return session.PlaceSettlement(player, position.Value);
	}

	public Result<City> PlaceCity(GameSession session, Player player, int? position)
	{
		if (!position.HasValue)
		{
			return Result<City>.Failure("City position must be specified.");
		}

		return session.PlaceCity(player, position.Value);
	}

	private bool IsMoveTypeDefined(string moveType)
	{
		var normalizedMoveType = moveType.ToUpper();

		foreach (var value in Enum.GetValues(typeof(MoveType)))
		{
			if (string.Equals(value.ToString(), normalizedMoveType, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}
}