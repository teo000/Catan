using AutoMapper;
using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Application.Moves;
using Catan.Domain.Common;
using Catan.Domain.Entities;
using Catan.Infrastructure.JsonConverters;
using Catan.Infrastructure.Requests;
using System.Net.Http.Json;
using System.Text.Json;

namespace Catan.Infrastructure.Services
{
	public class AIService : IAIService
	{
		private readonly HttpClient _httpClient;
		private readonly IMapper _mapper;

		public AIService(HttpClient httpClient, IMapper mapper)
		{
			_httpClient = httpClient;
			_mapper = mapper;
		}

		public async Task<Result<List<Move>>> MakeAIMove(GameSession gameSession, Guid playerId)
		{
			var jsonOptions = new JsonSerializerOptions
			{
				WriteIndented = true ,
				Converters = { new ResultJsonConverter<List<Move>>(), new MoveJsonConverter() }

			};

			var request = new AIMoveRequest
			{
				GameState = _mapper.Map<GameSessionDto>(gameSession),
				PlayerId = playerId
			};

			var jsonString = JsonSerializer.Serialize(request, jsonOptions);
			Console.WriteLine("JSON Sent to Microservice:");
			Console.WriteLine(jsonString);

			var response = await _httpClient.PostAsJsonAsync("trigger-aimove", request);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();

			Console.WriteLine("JSON Response from Microservice:");
			Console.WriteLine(responseContent);

			try
			{
				var content = JsonSerializer.Deserialize<Result<List<Move>>>(responseContent, jsonOptions);
				if (content != null)
					return content;
				else
					throw new Exception("Deserialized content is null");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error deserializing response: " + ex.Message);
				throw new Exception("AI service error: " + ex.Message);
			}
		}
	}
}
