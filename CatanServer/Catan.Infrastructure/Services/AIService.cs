using AutoMapper;
using Catan.Application.Contracts;
using Catan.Application.Dtos;
using Catan.Application.Models.Moves;
using Catan.Application.Models.Requests;
using Catan.Domain.Common;
using Catan.Domain.Data;
using Catan.Domain.Entities;
using Catan.Domain.Entities.Trades;
using Catan.Domain.Interfaces;
using Catan.Infrastructure.JsonConverters;
using Catan.Infrastructure.Requests;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Catan.Infrastructure.Services;

public class AIService : IAIService
{
	private readonly HttpClient _httpClient;
	private readonly IMapper _mapper;
	private readonly ILogger _logger;

	public AIService(HttpClient httpClient, IMapper mapper, ILogger logger)
	{
		_httpClient = httpClient;
		_mapper = mapper;
		_logger = logger;
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

	public async Task<Result<Dictionary<Resource, int>>> DiscardHalfOfResources(GameSession gameSession, Guid playerId)
	{

		var jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters = { new ResultJsonConverter<Dictionary<string, int>>()}
		};

		var request = new AIMoveRequest
		{
			GameState = _mapper.Map<GameSessionDto>(gameSession),
			PlayerId = playerId,
		};

		try
		{
			string requestContent = JsonSerializer.Serialize(request, jsonOptions);
			_logger.Warn($"Sending discard-half request for player {playerId}");

			var response = await _httpClient.PostAsJsonAsync("discard-half", request, jsonOptions);
			response.EnsureSuccessStatusCode();

			var responseContent = await response.Content.ReadAsStringAsync();
			_logger.Warn($"Received response: {responseContent}");

			var result = JsonSerializer.Deserialize<Result<Dictionary<string, int>>>(responseContent, jsonOptions);
			if (result == null)
			{
				_logger.Error($"Failed to deserialize response");
				return Result<Dictionary<Resource, int>>.Failure("Failed to deserialize response");
			}

			var resourceCount = GameUtils.ConvertToResourceDictionary(result.Value); // de facut custom exception

			return Result<Dictionary<Resource, int>>.Success(resourceCount);
		}
		catch (HttpRequestException httpEx)
		{
			_logger.Error(httpEx, $"HTTP request failed for player {playerId}");
			return Result<Dictionary<Resource, int>>.Failure("HTTP request failed");
		}
		catch (JsonException jsonEx)
		{
			_logger.Error(jsonEx, $"JSON deserialization failed for player {playerId}");
			return Result<Dictionary<Resource, int>>.Failure("Failed to deserialize response");
		}
		catch (Exception ex)
		{
			_logger.Error(ex, $"Unexpected error occurred for player {playerId}");
			return Result<Dictionary<Resource, int>>.Failure("An unexpected error occurred");
		}
	}

	public async Task<Result<bool>> RespondToTrade(GameSession gameSession, Guid playerId, Trade trade)
	{
		var jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters = { new ResultJsonConverter<bool>(),  }
		};

		var request = new AITradeRequest
		{
			GameState = _mapper.Map<GameSessionDto>(gameSession),
			PlayerId = playerId,
			Trade = _mapper.Map<TradeDto>(trade),
		};

		var jsonString = JsonSerializer.Serialize(request, jsonOptions);
		Console.WriteLine("JSON Sent to Microservice:");
		Console.WriteLine(jsonString);

		var response = await _httpClient.PostAsJsonAsync("respond-to-trade", request);
		response.EnsureSuccessStatusCode();

		var responseContent = await response.Content.ReadAsStringAsync();

		Console.WriteLine("JSON Response from Microservice:");
		Console.WriteLine(responseContent);

		try
		{
			var content = JsonSerializer.Deserialize<Result<bool>>(responseContent, jsonOptions);
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

	public async Task<Result<int>> MoveThief(GameSession gameSession, Guid playerId)
	{
		var jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters = { new ResultJsonConverter<int>(), }
		};

		var request = new AIMoveRequest
		{
			GameState = _mapper.Map<GameSessionDto>(gameSession),
			PlayerId = playerId,
		};

		var jsonString = JsonSerializer.Serialize(request, jsonOptions);
		Console.WriteLine("JSON Sent to Microservice:");
		Console.WriteLine(jsonString);

		var response = await _httpClient.PostAsJsonAsync("move-thief", request);
		response.EnsureSuccessStatusCode();

		var responseContent = await response.Content.ReadAsStringAsync();

		Console.WriteLine("JSON Response from Microservice:");
		Console.WriteLine(responseContent);

		try
		{
			var content = JsonSerializer.Deserialize<Result<int>>(responseContent, jsonOptions);
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

	public async Task<Result<List<PlayerTradeRequest>>> InitiatePlayerTrades(GameSession gameSession, Guid playerId)
	{
		var jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			Converters = { new ResultJsonConverter<List<PlayerTradeRequest>>(), new PlayerTradeRequestConverter() }
		};

		var request = new AIMoveRequest
		{
			GameState = _mapper.Map<GameSessionDto>(gameSession),
			PlayerId = playerId,
		};

		var jsonString = JsonSerializer.Serialize(request, jsonOptions);

		var response = await _httpClient.PostAsJsonAsync("initiate-player-trades", request);
		response.EnsureSuccessStatusCode();

		var responseContent = await response.Content.ReadAsStringAsync();

		try
		{
			var content = JsonSerializer.Deserialize<Result<List<PlayerTradeRequest>>>(responseContent, jsonOptions);
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
