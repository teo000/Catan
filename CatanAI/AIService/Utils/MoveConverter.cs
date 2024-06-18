using AIService.Entities.Moves;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AIService.Utils
{
	public class MoveConverter : JsonConverter<Move>
	{
		public override Move Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
			{
				var rootElement = doc.RootElement;
				var moveType = rootElement.GetProperty("moveType").GetString();

				return moveType switch
				{
					nameof(PlaceSettlementMove) => JsonSerializer.Deserialize<PlaceSettlementMove>(rootElement.GetRawText(), options),
					nameof(PlaceRoadMove) => JsonSerializer.Deserialize<PlaceRoadMove>(rootElement.GetRawText(), options),
					_ => throw new NotSupportedException($"Move type {moveType} is not supported")
				};
			}
		}

		public override void Write(Utf8JsonWriter writer, Move value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
		}
	}
}
