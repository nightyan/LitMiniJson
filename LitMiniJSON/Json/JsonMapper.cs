/*
 * File: JsonMapper.cs
 * Desc: Encapsulate MiniJSON with LitJson.JsonMapper interface
 * Created by night.yan(yanningning@gmail.com)  at 2014.03.25
 */

namespace LitJson
{
	public class JsonMapper
	{
		public static JsonData ToObject(string json)
		{
			object obj = MiniJSON.Deserialize(json);
			JsonData jsonData = obj == null ? null : new JsonData(obj);
			return jsonData;
		}

		public static string Serialize(object obj)
		{
			return MiniJSON.Serialize(obj);
		}

		public static object Deserialize(string json)
		{
			return MiniJSON.Deserialize(json);
		}

	}
}
