using Catan.Application.Models.Requests;
using Catan.Domain.Common;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Catan.Infrastructure.JsonConverters
{
	public class ResultListPlayerTradeRequestJsonConverter : JsonConverter<Result<List<PlayerTradeRequest>>>
	{
		public override Result<List<PlayerTradeRequest>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using (var jsonDoc = JsonDocument.ParseValue(ref reader))
			{
				var jsonObject = jsonDoc.RootElement;

				var isSuccess = jsonObject.GetProperty("isSuccess").GetBoolean();
				var error = jsonObject.TryGetProperty("error", out var errorElement) && errorElement.ValueKind != JsonValueKind.Null
					? errorElement.GetString()
					: null;

				if (isSuccess)
				{
					var value = jsonObject.TryGetProperty("value", out var valueElement) && valueElement.ValueKind != JsonValueKind.Null
						? JsonSerializer.Deserialize<List<PlayerTradeRequest>>(valueElement.GetRawText(), options)
						: new List<PlayerTradeRequest>();

					return Result<List<PlayerTradeRequest>>.Success(value);
				}
				else
				{
					return Result<List<PlayerTradeRequest>>.Failure(error);
				}
			}
		}

		public override void Write(Utf8JsonWriter writer, Result<List<PlayerTradeRequest>> value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteBoolean("isSuccess", value.IsSuccess);
			writer.WritePropertyName("value");
			JsonSerializer.Serialize(writer, value.Value, options);
			writer.WriteString("error", value.Error);
			writer.WriteEndObject();
		}
	}
}
