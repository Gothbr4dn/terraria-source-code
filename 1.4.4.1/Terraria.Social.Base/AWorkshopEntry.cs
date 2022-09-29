using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Social.Base
{
	public abstract class AWorkshopEntry
	{
		public const int CurrentWorkshopPublishVersion = 1;

		public const string ContentTypeName_World = "World";

		public const string ContentTypeName_ResourcePack = "ResourcePack";

		protected const string HeaderFileName = "Workshop.json";

		protected const string ContentTypeJsonCategoryField = "ContentType";

		protected const string WorkshopPublishedVersionField = "WorkshopPublishedVersion";

		protected const string WorkshopEntryField = "SteamEntryId";

		protected const string TagsField = "Tags";

		protected const string PreviewImageField = "PreviewImagePath";

		protected const string PublictyField = "Publicity";

		protected static readonly JsonSerializerSettings SerializerSettings;

		public static string ReadHeader(string jsonText)
		{
			JToken val = default(JToken);
			if (!JObject.Parse(jsonText).TryGetValue("ContentType", ref val))
			{
				return null;
			}
			return val.ToObject<string>();
		}

		protected static string CreateHeaderJson(string contentTypeName, ulong workshopEntryId, string[] tags, WorkshopItemPublicSettingId publicity, string previewImagePath)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			new JObject();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["WorkshopPublishedVersion"] = 1;
			dictionary["ContentType"] = contentTypeName;
			dictionary["SteamEntryId"] = workshopEntryId;
			if (tags != null && tags.Length != 0)
			{
				dictionary["Tags"] = JArray.FromObject((object)tags);
			}
			dictionary["Publicity"] = publicity;
			return JsonConvert.SerializeObject((object)dictionary, SerializerSettings);
		}

		public static bool TryReadingManifest(string filePath, out FoundWorkshopEntryInfo info)
		{
			info = null;
			if (!File.Exists(filePath))
			{
				return false;
			}
			string text = File.ReadAllText(filePath);
			info = new FoundWorkshopEntryInfo();
			Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(text, SerializerSettings);
			if (dictionary == null)
			{
				return false;
			}
			if (!TryGet<ulong>(dictionary, "SteamEntryId", out info.workshopEntryId))
			{
				return false;
			}
			if (!TryGet<int>(dictionary, "WorkshopPublishedVersion", out var outputValue))
			{
				outputValue = 1;
			}
			info.publishedVersion = outputValue;
			if (TryGet<JArray>(dictionary, "Tags", out var outputValue2))
			{
				info.tags = ((JToken)outputValue2).ToObject<string[]>();
			}
			if (TryGet<int>(dictionary, "Publicity", out var outputValue3))
			{
				info.publicity = (WorkshopItemPublicSettingId)outputValue3;
			}
			TryGet<string>(dictionary, "PreviewImagePath", out info.previewImagePath);
			return true;
		}

		protected static bool TryGet<T>(Dictionary<string, object> dict, string name, out T outputValue)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			outputValue = default(T);
			try
			{
				if (dict.TryGetValue(name, out var value))
				{
					if (value is T)
					{
						outputValue = (T)value;
						return true;
					}
					if (value is JObject)
					{
						outputValue = JsonConvert.DeserializeObject<T>(((object)(JObject)value).ToString());
						return true;
					}
					outputValue = (T)Convert.ChangeType(value, typeof(T));
					return true;
				}
				return false;
			}
			catch
			{
				return false;
			}
		}

		static AWorkshopEntry()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			JsonSerializerSettings val = new JsonSerializerSettings();
			val.set_TypeNameHandling((TypeNameHandling)0);
			val.set_MetadataPropertyHandling((MetadataPropertyHandling)1);
			val.set_Formatting((Formatting)1);
			SerializerSettings = val;
		}
	}
}
