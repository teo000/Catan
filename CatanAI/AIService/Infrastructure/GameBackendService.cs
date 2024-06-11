using AIService.Entities;
using AIService.Entities.Common;
using AIService.Entities.Data;
using AIService.Entities.Game;
using AIService.Entities.Moves;
using AIService.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace AIService.Infrastructure
{
	public class GameBackendService : IGameBackendService
	{
		private readonly HttpClient _httpClient;

		public GameBackendService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<Result<Move>> NotifyMoveAsync(Move move)
		{
			HttpResponseMessage response;

			if (move is PlaceSettlementMove)
			{
				response = await _httpClient.PostAsJsonAsync("place-settlement", move);
			}
			else if (move is EndTurnMove)
			{
				response = await _httpClient.PostAsJsonAsync("Game/end-turn", move);
			}
			else
			{
				throw new InvalidOperationException("Unsupported move type");
			}

			if (!response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return Result<Move>.Failure(content);
			}
			else return Result<Move>.Success(move);
		}

			//public async Task<Result<GameState>> GetGameStateAsync(Guid gameId)
			//{
			//	var response = await _httpClient.GetAsync($"http://game-backend/game-state/{gameId}");
			//	response.EnsureSuccessStatusCode();
			//	return await response.Content.ReadFromJsonAsync<Result<GameState>>();
			//}
	}
}
