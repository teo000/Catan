using System.Collections.Concurrent;
using AutoMapper;
using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Application.GameManagement.Misc;
using Catan.Application.Moves;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Domain.Entities.GamePieces;
using Catan.Domain.Entities.Misc;
using Catan.Domain.Entities.Trades;
using Catan.Domain.Interfaces;
using MediatR;

namespace Catan.Application.GameManagement;

public class GameSessionManager
{
	private readonly IAIService _aIService;
	private readonly IGameNotifier _gameNotifier;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;
	private ConcurrentDictionary<Guid, GameSession> gameSessions = new ConcurrentDictionary<Guid, GameSession>();
	private ConcurrentDictionary<Guid, Timer> sessionTimers = new ConcurrentDictionary<Guid, Timer>();


	public GameSessionManager(IAIService aIService, IGameNotifier gameNotifier, ILogger logger, IMapper mapper)
	{
		_aIService = aIService;
		_gameNotifier = gameNotifier;
		_logger = logger;
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
		{
			//var arguments = new AITimerArguments() { GameSessionId = session.Id, PlayerId = currentPlayer.Id };
			//var aItimer = new Timer(HandleAI, arguments, 1000, int.MaxValue);

			Task.Run(() => HandleAIPlayer(session, currentPlayer));

		}
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

				//if (session.Dice.GetSummedValue() == 7)
				//{
				//	foreach (var p in session.Players)
				//		if (p.GetCardsNo() >= 7 && !p.DiscardedThisTurn)
				//			player.Kick();
				//}

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

		Task.Run(() =>_gameNotifier.NotifyGameAsync(_mapper.Map<GameSessionDto>(session)));

	}

	private void HandleAI(object? state)
	{
		Console.WriteLine("gata");
		var args = (AITimerArguments)state;

		var sessionResult = GetGameSession(args.GameSessionId);

		if (!sessionResult.IsSuccess)
		{
			return;
		}

		var session = sessionResult.Value;

		var currentPlayer = session.GetTurnPlayer();
		if (args.PlayerId != currentPlayer.Id)
			return;

		Task.Run(() => HandleAIPlayer(session, currentPlayer));

	}

	public async Task HandleAIPlayer(GameSession session, Player aIPlayer)
	{
		if (!session.IsInBeginningPhase())
		{
			await RollDice(session, aIPlayer);
		}

		var aIMovesResult = await _aIService.MakeAIMove(session, aIPlayer.Id);

		if (!aIMovesResult.IsSuccess) 
		{
			aIPlayer.Kick();
			if (session.GetActivePlayers().Count < 2)
				session.MarkAbandoned();

			session.Message = $"{aIPlayer.Name} has been removed due to an internal error.";

			_logger.Warn($"AI Service failed to return any moves with error {aIMovesResult.Error}");

			return;
		}

		var aIMoves = aIMovesResult.Value;
		foreach (var move in aIMoves)
		{
			ParseMove(session, aIPlayer, move);
			await _gameNotifier.NotifyGameAsync(_mapper.Map<GameSessionDto>(session));
		}



		if (!session.IsInBeginningPhase())
			EndPlayerTurn(session);

		// handle AI move result here
	}

	private void ParseMove(GameSession session, Player aIPlayer, Move move)
	{
		if (move.GameId != session.Id)
			throw new Exception("AI move caused interal server error.");

		if (!IsMoveTypeDefined(move.MoveType))
			throw new Exception("Move type not supported");

		var moveType = (MoveType)Enum.Parse(typeof(MoveType), move.MoveType, true);

		if (moveType == MoveType.PlaceSettlement)
		{
			var result = PlaceSettlement(session, aIPlayer, move.Position);
			if (!result.IsSuccess)
				_logger.Warn($"AI Place Settlement error: {result.Error}");
		}
		else if (moveType == MoveType.PlaceRoad)
		{
			var result = PlaceRoad(session, aIPlayer, move.Position);
			if (!result.IsSuccess)
				_logger.Warn($"AI Place Road error: {result.Error}");
		}
		else if (moveType == MoveType.PlaceCity)
		{
			var result = PlaceCity(session, aIPlayer, move.Position);
			if (!result.IsSuccess)
				_logger.Warn($"AI Place City error: {result.Error}");
		}
	}

	public async Task<Result<DiceRoll>> RollDice (GameSession session, Player player) 
	{ 
		var diceRollResult = session.RollDice(player);
		_logger.Warn($"Dice rolled: ${session.Dice.GetSummedValue()} ");

		var aIPlayers = session.Players.Where(p => p.IsAI);

		if (!diceRollResult.IsSuccess || !aIPlayers.Any())
			return diceRollResult;
		
		var dice = diceRollResult.Value;
		if (dice.GetSummedValue() == 7)
			foreach (var aIPlayer in aIPlayers)
				if (aIPlayer.GetCardsNo() >= 7)
				{
					var aIResult = await _aIService.DiscardHalfOfResources(session, aIPlayer.Id); // set timer for this
					var aIDiceRollResult = session.DiscardHalf(aIPlayer, aIResult.Value);

					if (!aIDiceRollResult.IsSuccess)
					{
						_logger.Warn($"AI failed to discard half of resources: {aIDiceRollResult.Error}");
						LogResourceDictionary(aIResult.Value);
					}
					await _gameNotifier.NotifyGameAsync(_mapper.Map<GameSessionDto>(session));

				}

		return diceRollResult;
	
	}

	private void LogResourceDictionary(Dictionary<Resource, int> resourceDictionary)
	{
		
		_logger.Warn("Logging Resource Dictionary:");
		foreach (var kvp in resourceDictionary)
		{
			_logger.Warn($"{kvp.Key}: {kvp.Value}");
		}
	}

	public Result<DevelopmentCard> BuyDevelopmentCard(GameSession session, Player player)
	{
		return session.BuyDevelopmentCard(player);
	}

	public Result<Road> PlaceRoad(GameSession session, Player player, int? position)
	{
		if (!position.HasValue) 
		{
			return Result<Road>.Failure("Road position must be specified.");
		}

		if (player.IsAI)
			_logger.Warn($"AI places road at position: {position}");

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

		if (player.IsAI)
			_logger.Warn($"AI places settlement at position: {position}");

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

	public async Task<Result<PlayerTrade>> AddNewPendingTrade(GameSession session, Player playerToGive, Resource resourceToGive, int countToGive, Player playerToReceive, Resource resourceToReceive, int countToReceive)
	{
		var tradeResult = session.AddNewPendingTrade(playerToGive, resourceToGive, countToGive, playerToReceive, resourceToReceive, countToReceive);

		if (!tradeResult.IsSuccess)
			return tradeResult;

		if (playerToReceive.IsAI)
		{
			var aIResult = await _aIService.RespondToTrade(session, playerToReceive.Id, tradeResult.Value);
			if (!aIResult.IsSuccess)
			{
				_logger.Warn($"AI service failed to respond to trade offer: {aIResult.Error}");
				return tradeResult;
			}

			var aITradeResult = session.RespondToTrade(tradeResult.Value.Id, aIResult.Value);

			if (!aITradeResult.IsSuccess)
				_logger.Warn($"AI failed to respond to trade: {aITradeResult.Error}");

			await _gameNotifier.NotifyGameAsync(_mapper.Map<GameSessionDto>(session));

		}

		return tradeResult;
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