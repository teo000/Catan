using Catan.Domain.Common;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Catan.Infrastructure.JsonConverters;

public class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
	public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using (var jsonDoc = JsonDocument.ParseValue(ref reader))
		{
			var jsonObject = jsonDoc.RootElement;

			var isSuccess = jsonObject.GetProperty("isSuccess").GetBoolean();
			var error = jsonObject.GetProperty("error").GetString();

			if (isSuccess)
			{
				var value = JsonSerializer.Deserialize<T>(jsonObject.GetProperty("value").GetRawText(), options);
				return Result<T>.Success(value);
			}
			else
			{
				return Result<T>.Failure(error);
			}
		}
	}

	public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		writer.WriteBoolean("isSuccess", value.IsSuccess);
		writer.WritePropertyName("value");
		JsonSerializer.Serialize(writer, value.Value, options);
		writer.WriteString("error", value.Error);
		writer.WriteEndObject();
	}
}
