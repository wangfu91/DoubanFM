using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubanFM.Desktop.API.Models
{
	/// <summary>
	/// Convert int to bool, in case Newtonsoft.Json.JsonSerializationException was thrown.
	/// <see cref="http://stackoverflow.com/questions/14427596/convert-an-int-to-bool-with-json-net"/>
	/// </summary>
	public class BoolConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(((bool)value) ? 1 : 0);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return reader.Value.ToString() == "1";
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool);
		}
	}

}
