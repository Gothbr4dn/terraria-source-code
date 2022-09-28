using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	public class VanillaContentValidator : IContentValidator
	{
		private struct TextureMetaData
		{
			public int Width;

			public int Height;

			public bool Matches(Texture2D texture, out IRejectionReason rejectReason)
			{
				if (texture.Width != Width || texture.Height != Height)
				{
					rejectReason = (IRejectionReason)(object)new ContentRejectionFromSize(Width, Height, texture.Width, texture.Height);
					return false;
				}
				rejectReason = null;
				return true;
			}
		}

		public static VanillaContentValidator Instance;

		private Dictionary<string, TextureMetaData> _info = new Dictionary<string, TextureMetaData>();

		public VanillaContentValidator(string infoFilePath)
		{
			string[] array = Regex.Split(Utils.ReadEmbeddedResource(infoFilePath), "\r\n|\r|\n");
			foreach (string text in array)
			{
				if (!text.StartsWith("//"))
				{
					string[] array2 = text.Split(new char[1] { '\t' });
					if (array2.Length >= 3 && int.TryParse(array2[1], out var result) && int.TryParse(array2[2], out var result2))
					{
						string key = array2[0].Replace('/', '\\');
						_info[key] = new TextureMetaData
						{
							Width = result,
							Height = result2
						};
					}
				}
			}
		}

		public bool AssetIsValid<T>(T content, string contentPath, out IRejectionReason rejectReason) where T : class
		{
			Texture2D texture2D = content as Texture2D;
			rejectReason = null;
			if (texture2D != null)
			{
				if (!_info.TryGetValue(contentPath, out var value))
				{
					return true;
				}
				return value.Matches(texture2D, out rejectReason);
			}
			return true;
		}

		public HashSet<string> GetValidImageFilePaths()
		{
			return new HashSet<string>(_info.Select((KeyValuePair<string, TextureMetaData> x) => x.Key));
		}
	}
}
