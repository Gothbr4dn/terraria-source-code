using System.IO;
using ReLogic.Content;
using ReLogic.Content.Sources;
using Terraria.GameContent;

namespace Terraria.IO
{
	public class ResourcePackContentValidator
	{
		public void ValidateResourePack(ResourcePack pack)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			if ((int)(AssetReaderCollection)Main.instance.Services.GetService(typeof(AssetReaderCollection)) != 0)
			{
				IContentSource contentSource = pack.GetContentSource();
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				contentSource.GetAllAssetsStartingWith("Images" + directorySeparatorChar);
				VanillaContentValidator.Instance.GetValidImageFilePaths();
			}
		}
	}
}
