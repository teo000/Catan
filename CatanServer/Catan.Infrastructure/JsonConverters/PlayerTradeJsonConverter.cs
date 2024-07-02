using Catan.Application.Models.Requests;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Catan.Infrastructure.JsonConverters
{
	public class PlayerTradeRequestConverter : JsonConverter<PlayerTradeRequest>
	{
		public override PlayerTradeRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			Guid playerToGiveId = Guid.Empty;
			string resourceToGive = null;
			int countToGive = 0;
			Guid playerToReceiveId = Guid.Empty;
			string resourceToReceive = null;
			int countToReceive = 0;

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					break;
				}

				if (reader.TokenType != JsonTokenType.PropertyName)
				{
					throw new JsonException("Expected property name token.");
				}

				string propertyName = reader.GetString();
				reader.Read();

				switch (propertyName)
				{
					case "playerToGiveId":
						playerToGiveId = Guid.Parse(reader.GetString());
						break;
					case "resourceToGive":
						resourceToGive = reader.GetString();
						break;
					case "countToGive":
						countToGive = reader.GetInt32();
						break;
					case "playerToReceiveId":
						playerToReceiveId = Guid.Parse(reader.GetString());
						break;
					case "resourceToReceive":
						resourceToReceive = reader.GetString();
						break;
					case "countToReceive":
						countToReceive = reader.GetInt32();
						break;
					default:
						reader.Skip();
						break;
				}
			}

			return new PlayerTradeRequest
			{
				PlayerToGiveId = playerToGiveId,
				ResourceToGive = resourceToGive,
				CountToGive = countToGive,
				PlayerToReceiveId = playerToReceiveId,
				ResourceToReceive = resourceToReceive,
				CountToReceive = countToReceive
			};
		}

		public override void Write(Utf8JsonWriter writer, PlayerTradeRequest value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			writer.WriteString("PlayerToGiveId", value.PlayerToGiveId.ToString());
			writer.WriteString("ResourceToGive", value.ResourceToGive);
			writer.WriteNumber("CountToGive", value.CountToGive);
			writer.WriteString("PlayerToReceiveId", value.PlayerToReceiveId.ToString());
			writer.WriteString("ResourceToReceive", value.ResourceToReceive);
			writer.WriteNumber("CountToReceive", value.CountToReceive);

			writer.WriteEndObject();
		}
	}
}
