using System.Collections.Concurrent;
using AutoMapper;
using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Domain.Entities.GamePieces;

public class GameSessionManager
{
	private readonly Lazy<IAIService> _aIService;
	private readonly Lazy<IMapper> _mapper;

	private ConcurrentDictionary<Guid, GameSession> gameSessions = new ConcurrentDictionary<Guid, GameSession>();
	private ConcurrentDictionary<Guid, Timer> sessionTimers = new ConcurrentDictionary<Guid, Timer>();

	public GameSessionManager(Lazy<IAIService> aIService, Lazy<IMapper> mapper)
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
			var currentPlayer = session.GetTurnPlayer();
			if (currentPlayer.IsAI)
				Task.Run(() => HandleAIPlayer(session));
		}
	}

	private async Task HandleAIPlayer(GameSession session)
	{
		var aIService = _aIService.Value;
		var mapper = _mapper.Value;

		var aIMoveResult = await aIService.MakeAIMove(mapper.Map<GameSessionDto>(session));
		// handle AI move result here
	}

	public Result<Road> PlaceRoad(GameSession session, Player player, int position)
	{
		var result = session.PlaceRoad(player, position);

		if (!result.IsSuccess || !session.IsInBeginningPhase())
			return result;

		EndPlayerTurn(session);
		return result;
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
}
