/*
 * File: JsonData.cs
 * Desc: Encapsulate MiniJSON & LitJson.JsonData
 * Created by night.yan(yanningning@gmail.com)  at 2014.03.25
 */

using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.IO;


namespace LitJson
{
	public enum JsonType
	{
		None,

		Object,
		Array,
		String,
		Int,
		Long,
		Double,
		Boolean
	}

	public class JsonData : IList, IDictionary
	{
		#region Fields
		private IList<JsonData> inst_array;
		private bool inst_boolean;
		private double inst_double;
		private int inst_int;
		private long inst_long;
		private IDictionary<string, JsonData> inst_object;
		private string inst_string;
		private string json;
		private JsonType type;

		// Used to implement the IOrderedDictionary interface
		private IList<KeyValuePair<string, JsonData>> object_list;
		#endregion


		#region Properties
		public int Count
		{
			get { return EnsureCollection().Count; }
		}

		public bool IsArray
		{
			get { return type == JsonType.Array; }
		}

		public bool IsBoolean
		{
			get { return type == JsonType.Boolean; }
		}

		public bool IsDouble
		{
			get { return type == JsonType.Double; }
		}

		public bool IsInt
		{
			get { return type == JsonType.Int; }
		}

		public bool IsLong
		{
			get { return type == JsonType.Long; }
		}

		public bool IsObject
		{
			get { return type == JsonType.Object; }
		}

		public bool IsString
		{
			get { return type == JsonType.String; }
		}

		public JsonType GetJsonType()
		{
			return type;
		}
		#endregion


		#region Constructors
		public JsonData()
		{
			type = JsonType.None;
		}

		public JsonData(bool boolean)
		{
			this.JsonDataBool(boolean);
		}

		public JsonData(double number)
		{
			this.JsonDataDouble(number);
		}

		public JsonData(int number)
		{
			this.JsonDataInt(number);
		}

		public JsonData(long number)
		{
			this.JsonDataLong(number);
		}

		public JsonData(string str)
		{
			this.JsonDataString(str);
		}

		public JsonData(object obj)
		{
			if (obj is Boolean)
			{
				this.JsonDataBool((bool)obj);
			}
			else if (obj is Double)
			{
				this.JsonDataDouble((double)obj);
			}
			else if (obj is Int32)
			{
				this.JsonDataInt((int)obj);
			}
			else if (obj is Int64)
			{
				this.JsonDataLong((long)obj);
			}
			else if (obj is String)
			{
				this.JsonDataString((string)obj);
			}
			else if (obj is IList)
			{
				this.JsonDataList((IList)obj);
			}
			else if (obj is IDictionary)
			{
				this.JsonDataDictionary((IDictionary)obj);
			}
			else
			{
				throw new ArgumentException("Unable to wrap the given object with JsonData");
			}
		}

		private void JsonDataBool(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		private void JsonDataDouble(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		private void JsonDataInt(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		private void JsonDataLong(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		private void JsonDataString(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		private void JsonDataList(IList obj)
		{
			this.type = JsonType.Array;
			List<object> lst = obj as List<object>;
			inst_array = new List<JsonData>(lst.Count);
			foreach (object item in lst)
			{
				inst_array.Add(this.ToJsonData(item));
			}
		}

		private void JsonDataDictionary(IDictionary obj)
		{
			this.type = JsonType.Object;
			Dictionary<string, object> dict = obj as Dictionary<string, object>;
			inst_object = new Dictionary<string, JsonData>(dict.Count);
			object_list = new List<KeyValuePair<string, JsonData>>(dict.Count);

			KeyValuePair<string, JsonData> entry;
			JsonData value;
			foreach (KeyValuePair<string, object> item in dict)
			{
				value = this.ToJsonData(item.Value);
				entry = new KeyValuePair<string, JsonData>(item.Key, value);
				inst_object.Add(entry);
				object_list.Add(entry);
			}
		}

		private object ToObject(object obj)
		{
			if (obj is JsonData)
			{
				JsonData jd = obj as JsonData;
				switch (jd.type)
				{
					case JsonType.Boolean:
						return jd.inst_boolean;

					case JsonType.Double:
						return jd.inst_double;

					case JsonType.Int:
						return jd.inst_int;

					case JsonType.Long:
						return jd.inst_long;

					case JsonType.String:
						return jd.inst_string;

					case JsonType.Array:
						List<object> list = new List<object>();
						foreach (JsonData item in jd.inst_array)
						{
							list.Add(this.ToObject(item));
						}
						return list;

					case JsonType.Object:
						Dictionary<string, object> dict = new Dictionary<string, object>();
						foreach (KeyValuePair<string, JsonData> item in jd.inst_object)
						{
							dict[item.Key] = this.ToObject(item.Value);
						}
						return dict;

					default:
						return null;
				}
			}
			else
			{
				return obj;
			}
		}
		#endregion


		#region Implicit Conversions
		public static implicit operator JsonData(Boolean data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(Double data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(Int32 data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(Int64 data)
		{
			return new JsonData(data);
		}

		public static implicit operator JsonData(String data)
		{
			return new JsonData(data);
		}
		#endregion


		#region Explicit Conversions
		public static explicit operator Boolean(JsonData data)
		{
			if (data.type != JsonType.Boolean)
				throw new InvalidCastException(
					"Instance of JsonData doesn't hold a double");

			return data.inst_boolean;
		}

		public static explicit operator Double(JsonData data)
		{
			if (data.type != JsonType.Double)
				throw new InvalidCastException(
					"Instance of JsonData doesn't hold a double");

			return data.inst_double;
		}

		public static explicit operator Int32(JsonData data)
		{
			if (data.type != JsonType.Int)
				throw new InvalidCastException(
					"Instance of JsonData doesn't hold an int");

			return data.inst_int;
		}

		public static explicit operator Int64(JsonData data)
		{
			if (data.type != JsonType.Long)
				throw new InvalidCastException(
					"Instance of JsonData doesn't hold an int");

			return data.inst_long;
		}

		public static explicit operator String(JsonData data)
		{
			if (data.type != JsonType.String)
				throw new InvalidCastException(
					"Instance of JsonData doesn't hold a string");

			return data.inst_string;
		}
		#endregion


		#region IDictionary Indexer
		object IDictionary.this[object key]
		{
			get
			{
				return EnsureDictionary()[key];
			}

			set
			{
				if (!(key is String))
					throw new ArgumentException(
						"The key has to be a string");

				JsonData data = ToJsonData(value);

				this[(string)key] = data;
			}
		}
		#endregion


		#region IList Indexer
		object IList.this[int index]
		{
			get
			{
				return EnsureList()[index];
			}

			set
			{
				EnsureList();
				JsonData data = ToJsonData(value);

				this[index] = data;
			}
		}
		#endregion


		#region Public Indexers
		public JsonData this[string prop_name]
		{
			get
			{
				EnsureDictionary();
				return inst_object[prop_name];
			}

			set
			{
				EnsureDictionary();

				KeyValuePair<string, JsonData> entry =
					new KeyValuePair<string, JsonData>(prop_name, value);

				if (inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < object_list.Count; i++)
					{
						if (object_list[i].Key == prop_name)
						{
							object_list[i] = entry;
							break;
						}
					}
				}
				else
					object_list.Add(entry);

				inst_object[prop_name] = value;

				json = null;
			}
		}

		public JsonData this[int index]
		{
			get
			{
				EnsureCollection();

				if (type == JsonType.Array)
					return inst_array[index];

				return object_list[index].Value;
			}

			set
			{
				EnsureCollection();

				if (type == JsonType.Array)
				{
					inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> entry = object_list[index];
					KeyValuePair<string, JsonData> new_entry =
						new KeyValuePair<string, JsonData>(entry.Key, value);

					object_list[index] = new_entry;
					inst_object[entry.Key] = value;
				}

				json = null;
			}
		}
		#endregion


		#region Public Methods
		public int Add(object value)
		{
			JsonData data = ToJsonData(value);
			json = null;
			return EnsureList().Add(data);
		}

		public void Clear()
		{
			json = null;

			if (IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}

			if (IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		public bool Contains(object key)
		{
			return EnsureDictionary().Contains(key);
		}

		public IDictionaryEnumerator Enumerator()
		{
			return EnsureDictionary().GetEnumerator();
		}

		public bool Equals(JsonData x)
		{
			if (x == null)
				return false;

			if (x.type != this.type)
				return false;

			switch (this.type)
			{
				case JsonType.None:
					return true;

				case JsonType.Object:
					return this.inst_object.Equals(x.inst_object);

				case JsonType.Array:
					return this.inst_array.Equals(x.inst_array);

				case JsonType.String:
					return this.inst_string.Equals(x.inst_string);

				case JsonType.Int:
					return this.inst_int.Equals(x.inst_int);

				case JsonType.Long:
					return this.inst_long.Equals(x.inst_long);

				case JsonType.Double:
					return this.inst_double.Equals(x.inst_double);

				case JsonType.Boolean:
					return this.inst_boolean.Equals(x.inst_boolean);
			}

			return false;
		}

		public string ToJson()
		{
			//if (json == null) // 当前对象修改之后，需要让父对象知道，否则父对象的json是旧的
			{
				object obj = this.ToObject(this);
				json = MiniJSON.Serialize(obj);
			}
			return json;
		}

		public override string ToString()
		{
			switch (type)
			{
				case JsonType.Array:
					return "JsonData array";

				case JsonType.Boolean:
					return inst_boolean.ToString();

				case JsonType.Double:
					return inst_double.ToString();

				case JsonType.Int:
					return inst_int.ToString();

				case JsonType.Long:
					return inst_long.ToString();

				case JsonType.Object:
					return "JsonData object";

				case JsonType.String:
					return inst_string;
			}

			return "Uninitialized JsonData";
		}
		
		public static bool IsNull(JsonData jd)
		{
			if (jd == null || jd.type == JsonType.None)
				return true;
			return false;
		}
		#endregion


		#region Private Methods
		private ICollection EnsureCollection()
		{
			if (type == JsonType.Array)
				return (ICollection)inst_array;

			if (type == JsonType.Object)
				return (ICollection)inst_object;

			throw new InvalidOperationException(
				"The JsonData instance has to be initialized first");
		}

		private IDictionary EnsureDictionary()
		{
			if (type == JsonType.Object)
				return (IDictionary)inst_object;

			if (type != JsonType.None)
				throw new InvalidOperationException(
					"Instance of JsonData is not a dictionary");

			type = JsonType.Object;
			inst_object = new Dictionary<string, JsonData>();
			object_list = new List<KeyValuePair<string, JsonData>>();

			return (IDictionary)inst_object;
		}

		private IList EnsureList()
		{
			if (type == JsonType.Array)
				return (IList)inst_array;

			if (type != JsonType.None)
				throw new InvalidOperationException(
					"Instance of JsonData is not a list");

			type = JsonType.Array;
			inst_array = new List<JsonData>();

			return (IList)inst_array;
		}

		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
				return null;

			if (obj is JsonData)
				return (JsonData)obj;

			return new JsonData(obj);
		}

		#endregion


		//////////// Interface Properties & Methods /////
		#region ICollection Properties & Methods
		int ICollection.Count
		{
			get
			{
				return Count;
			}
		}
		bool ICollection.IsSynchronized
		{
			get
			{
				return EnsureCollection().IsSynchronized;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				return EnsureCollection().SyncRoot;
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			EnsureCollection().CopyTo(array, index);
		}
		#endregion


		#region IDictionary Properties & Methods
		bool IDictionary.IsFixedSize
		{
			get
			{
				return EnsureDictionary().IsFixedSize;
			}
		}

		bool IDictionary.IsReadOnly
		{
			get
			{
				return EnsureDictionary().IsReadOnly;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				EnsureDictionary();
				IList<string> keys = new List<string>();

				foreach (KeyValuePair<string, JsonData> entry in
						 object_list)
				{
					keys.Add(entry.Key);
				}

				return (ICollection)keys;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				EnsureDictionary();
				IList<JsonData> values = new List<JsonData>();

				foreach (KeyValuePair<string, JsonData> entry in
						 object_list)
				{
					values.Add(entry.Value);
				}

				return (ICollection)values;
			}
		}

		void IDictionary.Add(object key, object value)
		{
			JsonData data = ToJsonData(value);

			EnsureDictionary().Add(key, data);

			KeyValuePair<string, JsonData> entry =
				new KeyValuePair<string, JsonData>((string)key, data);
			object_list.Add(entry);

			json = null;
		}

		void IDictionary.Clear()
		{
			EnsureDictionary().Clear();
			object_list.Clear();
			json = null;
		}

		bool IDictionary.Contains(object key)
		{
			return EnsureDictionary().Contains(key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			//return ((IOrderedDictionary) this).GetEnumerator ();
			EnsureDictionary();

			return new OrderedDictionaryEnumerator(
				object_list.GetEnumerator());
		}

		void IDictionary.Remove(object key)
		{
			EnsureDictionary().Remove(key);

			for (int i = 0; i < object_list.Count; i++)
			{
				if (object_list[i].Key == (string)key)
				{
					object_list.RemoveAt(i);
					break;
				}
			}

			json = null;
		}
		#endregion


		#region IEnumerable Methods
		IEnumerator IEnumerable.GetEnumerator()
		{
			return EnsureCollection().GetEnumerator();
		}
		#endregion


		#region IList Properties & Methods
		bool IList.IsFixedSize
		{
			get
			{
				return EnsureList().IsFixedSize;
			}
		}

		bool IList.IsReadOnly
		{
			get
			{
				return EnsureList().IsReadOnly;
			}
		}

		int IList.Add(object value)
		{
			return Add(value);
		}

		void IList.Clear()
		{
			EnsureList().Clear();
			json = null;
		}

		bool IList.Contains(object value)
		{
			return EnsureList().Contains(value);
		}

		int IList.IndexOf(object value)
		{
			return EnsureList().IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			EnsureList().Insert(index, value);
			json = null;
		}

		void IList.Remove(object value)
		{
			EnsureList().Remove(value);
			json = null;
		}

		void IList.RemoveAt(int index)
		{
			EnsureList().RemoveAt(index);
			json = null;
		}
		#endregion
	}


	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator
	{
		IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;


		public object Current
		{
			get { return Entry; }
		}

		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> curr = list_enumerator.Current;
				return new DictionaryEntry(curr.Key, curr.Value);
			}
		}

		public object Key
		{
			get { return list_enumerator.Current.Key; }
		}

		public object Value
		{
			get { return list_enumerator.Current.Value; }
		}


		public OrderedDictionaryEnumerator(
			IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			list_enumerator = enumerator;
		}


		public bool MoveNext()
		{
			return list_enumerator.MoveNext();
		}

		public void Reset()
		{
			list_enumerator.Reset();
		}
	}
}
