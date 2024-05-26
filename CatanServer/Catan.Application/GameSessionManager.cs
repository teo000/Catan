using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;

namespace Catan.Application
{
	public class GameSessionManager : IGameSessionManager
	{
		private Dictionary<Guid, GameSession> gameSessions = new Dictionary<Guid, GameSession>();
		private readonly Dictionary<Guid, Timer> sessionTimers = new Dictionary<Guid, Timer>();

		public Result<GameSession> CreateGameSession(List<Player> players)
		{
			var result = GameSession.Create(players);
			if (result.IsSuccess)
			{
				var game = result.Value;
				gameSessions.Add(game.Id, game);
				
				StartTurnTimer(game.Id);
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
			if (gameSessions.TryGetValue(guid, out var game))
			{
				gameSessions.Remove(guid);
				return Result<GameSession>.Success(game);
			}
			else
			{
				return Result<GameSession>.Failure("Game does not exist in current context");
			}
		}

		private void StartTurnTimer(Guid sessionId)
		{
			if (sessionTimers.TryGetValue(sessionId, out Timer? oldTimer))
			{
				oldTimer.Dispose();
			}

			var timer = new Timer(OnTurnTimeout, sessionId, 1000 * GameInfo.TURN_DURATION, 1000 * GameInfo.TURN_DURATION);
			sessionTimers[sessionId] = timer;
		}

		public void EndPlayerTurn(GameSession session)
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
				session.MarkFinished();


			if (session.GameStatus == GameStatus.InProgress)
			{
				session.EndPlayerTurn();
				StartTurnTimer(session.Id);
			}
		}

		private void OnTurnTimeout(object? state)
		{
			Console.WriteLine("gata");
			var sessionId = (Guid)state;
			var sessionResult =  GetGameSession(sessionId);

			if (!sessionResult.IsSuccess)
			{
				return;
			}

			var session = sessionResult.Value;

			EndPlayerTurn(session);
		}
	}
}
