using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.IO;

namespace Terraria.WorldBuilding
{
	public class WorldGenConfiguration : GameConfiguration
	{
		private readonly JObject _biomeRoot;

		private readonly JObject _passRoot;

		public WorldGenConfiguration(JObject configurationRoot)
			: base(configurationRoot)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			_003F val = (JObject)configurationRoot.get_Item("Biomes");
			if ((int)val == 0)
			{
				val = new JObject();
			}
			_biomeRoot = (JObject)val;
			_003F val2 = (JObject)configurationRoot.get_Item("Passes");
			if ((int)val2 == 0)
			{
				val2 = new JObject();
			}
			_passRoot = (JObject)val2;
		}

		public T CreateBiome<T>() where T : MicroBiome, new()
		{
			return CreateBiome<T>(typeof(T).Name);
		}

		public T CreateBiome<T>(string name) where T : MicroBiome, new()
		{
			JToken val = default(JToken);
			if (_biomeRoot.TryGetValue(name, ref val))
			{
				return val.ToObject<T>();
			}
			return new T();
		}

		public GameConfiguration GetPassConfiguration(string name)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			JToken val = default(JToken);
			if (_passRoot.TryGetValue(name, ref val))
			{
				return new GameConfiguration((JObject)val);
			}
			return new GameConfiguration(new JObject());
		}

		public static WorldGenConfiguration FromEmbeddedPath(string path)
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
			using StreamReader streamReader = new StreamReader(stream);
			return new WorldGenConfiguration(JsonConvert.DeserializeObject<JObject>(streamReader.ReadToEnd()));
		}
	}
}
