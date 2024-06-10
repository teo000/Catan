using Catan.Application.Utils;
using Catan.Domain.Common;
using Catan.Domain.Entities;
using System;

namespace Catan.Application.GameManagement
{
	public class LobbyManager
    {
        private Dictionary<string, Lobby> lobbies = new Dictionary<string, Lobby>();
        private GameSessionManager _gameSessionManager;

        public LobbyManager(GameSessionManager gameSessionManager)
        {
            _gameSessionManager = gameSessionManager;
        }

        public Result<Lobby> Get(string joinCode)
        {
            if (lobbies.TryGetValue(joinCode, out var lobby))
            {
                return Result<Lobby>.Success(lobby);
            }
            else
            {
                return Result<Lobby>.Failure("Lobby does not exist in current context");
            }
        }

        public Result<Lobby> Create(Player player)
        {
            string joinCode;
            do
            {
                joinCode = JoinCodeGenerator.GenerateJoinCode();
            } while (Get(joinCode).IsSuccess);

            var newLobby = new Lobby(joinCode, player);
            lobbies.Add(newLobby.JoinCode, newLobby);
            return Result<Lobby>.Success(newLobby);
        }

        public Result<Lobby> Join(string joinCode, Player player)
        {
            var lobbyResult = Get(joinCode);
            if (!lobbyResult.IsSuccess)
                return Result<Lobby>.Failure("Lobby does not exist in current context.");

            var lobby = lobbyResult.Value;
            var joinResult = lobby.Join(player);
            return joinResult;
        }

        public Result<Lobby> StartGame(string joinCode)
        {
            var lobbyResult = Get(joinCode);
            if (!lobbyResult.IsSuccess)
                return Result<Lobby>.Failure("Lobby does not exist in current context.");

            var lobby = lobbyResult.Value;

            var result = _gameSessionManager.CreateGameSession(lobby.Players);

            if (!result.IsSuccess)
                return Result<Lobby>.Failure(result.Error);

            lobby.SetGameSession(result.Value);
            return Result<Lobby>.Success(lobby);
        }

    }
}
