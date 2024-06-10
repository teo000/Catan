using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Domain.Common;
using System.Net.Http.Json;

namespace Catan.Infrastructure.Services
{
	public class AIService : IAIService
	{
		private readonly HttpClient _httpClient;

		public AIService(IHttpClientFactory httpClientFactory)
		{
			_httpClient = httpClientFactory.CreateClient("aiClient");
		}

		public async Task<Result<bool>> MakeAIMove(GameSessionDto gameSession)
		{
			var response = await _httpClient.PostAsJsonAsync("move", gameSession);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<Result<bool>>();
		}
	}
}
