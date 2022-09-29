using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria.GameContent;

namespace Terraria.IO
{
	public class ResourcePack
	{
		public enum BrandingType
		{
			None,
			SteamWorkshop
		}

		public readonly string FullPath;

		public readonly string FileName;

		private readonly IServiceProvider _services;

		public readonly bool IsCompressed;

		public readonly BrandingType Branding;

		private readonly ZipFile _zipFile;

		private Texture2D _icon;

		private IContentSource _contentSource;

		private bool _needsReload = true;

		private const string ICON_FILE_NAME = "icon.png";

		private const string PACK_FILE_NAME = "pack.json";

		public Texture2D Icon
		{
			get
			{
				if (_icon == null)
				{
					_icon = CreateIcon();
				}
				return _icon;
			}
		}

		public string Name { get; private set; }

		public string Description { get; private set; }

		public string Author { get; private set; }

		public ResourcePackVersion Version { get; private set; }

		public bool IsEnabled { get; set; }

		public int SortingOrder { get; set; }

		public ResourcePack(IServiceProvider services, string path, BrandingType branding = BrandingType.None)
		{
			if (File.Exists(path))
			{
				IsCompressed = true;
			}
			else if (!Directory.Exists(path))
			{
				throw new FileNotFoundException("Unable to find file or folder for resource pack at: " + path);
			}
			FileName = Path.GetFileName(path);
			_services = services;
			FullPath = path;
			Branding = branding;
			if (IsCompressed)
			{
				_zipFile = ZipFile.Read(path);
			}
			LoadManifest();
		}

		public void Refresh()
		{
			_needsReload = true;
		}

		public IContentSource GetContentSource()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			if (_needsReload)
			{
				if (IsCompressed)
				{
					_contentSource = (IContentSource)new ZipContentSource(FullPath, "Content");
				}
				else
				{
					_contentSource = (IContentSource)new FileSystemContentSource(Path.Combine(FullPath, "Content"));
				}
				_contentSource.set_ContentValidator((IContentValidator)(object)VanillaContentValidator.Instance);
				_needsReload = false;
			}
			return _contentSource;
		}

		private Texture2D CreateIcon()
		{
			if (!HasFile("icon.png"))
			{
				return XnaExtensions.Get<IAssetRepository>(_services).Request<Texture2D>("Images/UI/DefaultResourcePackIcon", (AssetRequestMode)1).get_Value();
			}
			using Stream stream = OpenStream("icon.png");
			return XnaExtensions.Get<AssetReaderCollection>(_services).Read<Texture2D>(stream, ".png");
		}

		private void LoadManifest()
		{
			if (!HasFile("pack.json"))
			{
				throw new FileNotFoundException(string.Format("Resource Pack at \"{0}\" must contain a {1} file.", FullPath, "pack.json"));
			}
			JObject val;
			using (Stream stream = OpenStream("pack.json"))
			{
				using StreamReader streamReader = new StreamReader(stream);
				val = JObject.Parse(streamReader.ReadToEnd());
			}
			Name = Extensions.Value<string>((IEnumerable<JToken>)val.get_Item("Name"));
			Description = Extensions.Value<string>((IEnumerable<JToken>)val.get_Item("Description"));
			Author = Extensions.Value<string>((IEnumerable<JToken>)val.get_Item("Author"));
			Version = val.get_Item("Version").ToObject<ResourcePackVersion>();
		}

		private Stream OpenStream(string fileName)
		{
			if (!IsCompressed)
			{
				return File.OpenRead(Path.Combine(FullPath, fileName));
			}
			ZipEntry obj = _zipFile.get_Item(fileName);
			MemoryStream memoryStream = new MemoryStream((int)obj.get_UncompressedSize());
			obj.Extract((Stream)memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		private bool HasFile(string fileName)
		{
			if (!IsCompressed)
			{
				return File.Exists(Path.Combine(FullPath, fileName));
			}
			return _zipFile.ContainsEntry(fileName);
		}
	}
}
