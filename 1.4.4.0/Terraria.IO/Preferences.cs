using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using Terraria.Localization;

namespace Terraria.IO
{
	public class Preferences
	{
		public delegate void TextProcessAction(ref string text);

		private Dictionary<string, object> _data = new Dictionary<string, object>();

		private readonly string _path;

		private readonly JsonSerializerSettings _serializerSettings;

		public readonly bool UseBson;

		private readonly object _lock = new object();

		public bool AutoSave;

		public event Action<Preferences> OnSave;

		public event Action<Preferences> OnLoad;

		public event TextProcessAction OnProcessText;

		public Preferences(string path, bool parseAllTypes = false, bool useBson = false)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			_path = path;
			UseBson = useBson;
			if (parseAllTypes)
			{
				JsonSerializerSettings val = new JsonSerializerSettings();
				val.set_TypeNameHandling((TypeNameHandling)4);
				val.set_MetadataPropertyHandling((MetadataPropertyHandling)1);
				val.set_Formatting((Formatting)1);
				_serializerSettings = val;
			}
			else
			{
				JsonSerializerSettings val2 = new JsonSerializerSettings();
				val2.set_Formatting((Formatting)1);
				_serializerSettings = val2;
			}
		}

		public bool Load()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			lock (_lock)
			{
				if (!File.Exists(_path))
				{
					return false;
				}
				try
				{
					if (!UseBson)
					{
						string text = File.ReadAllText(_path);
						_data = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, _serializerSettings);
					}
					else
					{
						using FileStream fileStream = File.OpenRead(_path);
						BsonReader val = new BsonReader((Stream)fileStream);
						try
						{
							JsonSerializer val2 = JsonSerializer.Create(_serializerSettings);
							_data = val2.Deserialize<Dictionary<string, object>>((JsonReader)(object)val);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
					if (_data == null)
					{
						_data = new Dictionary<string, object>();
					}
					if (this.OnLoad != null)
					{
						this.OnLoad(this);
					}
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public bool Save(bool canCreateFile = true)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			lock (_lock)
			{
				try
				{
					if (this.OnSave != null)
					{
						this.OnSave(this);
					}
					if (!canCreateFile && !File.Exists(_path))
					{
						return false;
					}
					Directory.GetParent(_path).Create();
					if (File.Exists(_path))
					{
						File.SetAttributes(_path, FileAttributes.Normal);
					}
					if (!UseBson)
					{
						string text = JsonConvert.SerializeObject((object)_data, _serializerSettings);
						if (this.OnProcessText != null)
						{
							this.OnProcessText(ref text);
						}
						File.WriteAllText(_path, text);
						File.SetAttributes(_path, FileAttributes.Normal);
					}
					else
					{
						using FileStream fileStream = File.Create(_path);
						BsonWriter val = new BsonWriter((Stream)fileStream);
						try
						{
							File.SetAttributes(_path, FileAttributes.Normal);
							JsonSerializer.Create(_serializerSettings).Serialize((JsonWriter)(object)val, (object)_data);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToWritePreferences", _path));
					Console.WriteLine(ex.ToString());
					return false;
				}
				return true;
			}
		}

		public void Clear()
		{
			_data.Clear();
		}

		public void Put(string name, object value)
		{
			lock (_lock)
			{
				_data[name] = value;
				if (AutoSave)
				{
					Save();
				}
			}
		}

		public bool Contains(string name)
		{
			lock (_lock)
			{
				return _data.ContainsKey(name);
			}
		}

		public T Get<T>(string name, T defaultValue)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			lock (_lock)
			{
				try
				{
					if (_data.TryGetValue(name, out var value))
					{
						if (value is T)
						{
							return (T)value;
						}
						if (value is JObject)
						{
							return JsonConvert.DeserializeObject<T>(((object)(JObject)value).ToString());
						}
						return (T)Convert.ChangeType(value, typeof(T));
					}
					return defaultValue;
				}
				catch
				{
					return defaultValue;
				}
			}
		}

		public void Get<T>(string name, ref T currentValue)
		{
			currentValue = Get(name, currentValue);
		}

		public List<string> GetAllKeys()
		{
			return _data.Keys.ToList();
		}
	}
}
