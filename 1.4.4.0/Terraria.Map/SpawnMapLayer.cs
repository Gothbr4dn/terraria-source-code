using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map
{
	public class SpawnMapLayer : IMapLayer
	{
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			Player localPlayer = Main.LocalPlayer;
			Vector2 position = new Vector2(localPlayer.SpawnX, localPlayer.SpawnY);
			if (context.Draw(position: new Vector2(Main.spawnTileX, Main.spawnTileY), texture: TextureAssets.SpawnPoint.get_Value(), alignment: Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnPoint");
			}
			if (localPlayer.SpawnX != -1 && context.Draw(TextureAssets.SpawnBed.get_Value(), position, Alignment.Bottom).IsMouseOver)
			{
				text = Language.GetTextValue("UI.SpawnBed");
			}
		}
	}
}
