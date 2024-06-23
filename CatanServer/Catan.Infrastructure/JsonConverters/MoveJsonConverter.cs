using Catan.Application.Moves;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Catan.Infrastructure.JsonConverters;

public class MoveJsonConverter : JsonConverter<Move>
{
	public override Move Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using (JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader))
		{
			JsonElement jsonObject = jsonDoc.RootElement;

			return new Move
			{
				GameId = jsonObject.GetProperty("gameId").GetGuid(),
				MoveType = jsonObject.GetProperty("moveType").GetString(),
				Position = jsonObject.TryGetProperty("position", out JsonElement positionElement) ? positionElement.GetInt32() : (int?)null
			};
		}
	}

	public override void Write(Utf8JsonWriter writer, Move value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("gameId");
		writer.WriteStringValue(value.GameId);
		writer.WritePropertyName("moveType");
		writer.WriteStringValue(value.MoveType);
		if (value.Position.HasValue)
		{
			writer.WritePropertyName("position");
			writer.WriteNumberValue(value.Position.Value);
		}
		writer.WriteEndObject();
	}
}