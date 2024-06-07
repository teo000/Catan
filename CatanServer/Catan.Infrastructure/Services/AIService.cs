using Catan.Application.Contracts;
using Catan.Application.Dtos;
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

		//public async Task<Move> MakeAIMove(GameSessionDto gameSession)
		//{
		//	var response = await _httpClient.PostAsJsonAsync("move", gameSession);
		//	response.EnsureSuccessStatusCode();
		//	return await response.Content.ReadFromJsonAsync<Move>();
		//}
	}
}
