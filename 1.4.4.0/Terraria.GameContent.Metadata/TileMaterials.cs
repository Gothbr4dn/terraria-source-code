using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Terraria.ID;

namespace Terraria.GameContent.Metadata
{
	public static class TileMaterials
	{
		private static Dictionary<string, TileMaterial> _materialsByName;

		private static readonly TileMaterial[] MaterialsByTileId;

		static TileMaterials()
		{
			MaterialsByTileId = new TileMaterial[693];
			_materialsByName = DeserializeEmbeddedResource<Dictionary<string, TileMaterial>>("Terraria.GameContent.Metadata.MaterialData.Materials.json");
			TileMaterial tileMaterial = _materialsByName["Default"];
			for (int i = 0; i < MaterialsByTileId.Length; i++)
			{
				MaterialsByTileId[i] = tileMaterial;
			}
			foreach (KeyValuePair<string, string> item in DeserializeEmbeddedResource<Dictionary<string, string>>("Terraria.GameContent.Metadata.MaterialData.Tiles.json"))
			{
				string key = item.Key;
				string value = item.Value;
				SetForTileId((ushort)TileID.Search.GetId(key), _materialsByName[value]);
			}
		}

		private static T DeserializeEmbeddedResource<T>(string path)
		{
			using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
			using StreamReader streamReader = new StreamReader(stream);
			return JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());
		}

		public static void SetForTileId(ushort tileId, TileMaterial material)
		{
			MaterialsByTileId[tileId] = material;
		}

		public static TileMaterial GetByTileId(ushort tileId)
		{
			return MaterialsByTileId[tileId];
		}
	}
}
