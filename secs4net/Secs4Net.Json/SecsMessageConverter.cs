using System;
using Newtonsoft.Json;

namespace Secs4Net.Json
{
	public sealed class SecsMessageConverter : JsonConverter<SecsMessage>
	{
		public override SecsMessage ReadJson(JsonReader reader, Type objectType, SecsMessage existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return reader.ToSecsMessage();
		}

		public override void WriteJson(JsonWriter writer, SecsMessage value, JsonSerializer serializer)
		{
			value.WriteTo(writer);
		}
	}
}
