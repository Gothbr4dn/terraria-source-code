using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.Map
{
	public class TeleportPylonsMapLayer : IMapLayer
	{
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			List<TeleportPylonInfo> pylons = Main.PylonSystem.Pylons;
			float num = 1f;
			float scaleIfSelected = num * 2f;
			Texture2D value = TextureAssets.Extra[182].get_Value();
			bool num2 = TeleportPylonsSystem.IsPlayerNearAPylon(Main.LocalPlayer) && (Main.DroneCameraTracker == null || !Main.DroneCameraTracker.IsInUse());
			Color color = Color.White;
			if (!num2)
			{
				color = Color.Gray * 0.5f;
			}
			for (int i = 0; i < pylons.Count; i++)
			{
				TeleportPylonInfo info = pylons[i];
				if (context.Draw(value, info.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), color, new SpriteFrame(9, 1, (byte)info.TypeOfPylon, 0)
				{
					PaddingY = 0
				}, num, scaleIfSelected, Alignment.Center).IsMouseOver)
				{
					Main.cancelWormHole = true;
					string text2 = (text = Lang.GetItemNameValue(TETeleportationPylon.GetPylonItemTypeFromTileStyle((int)info.TypeOfPylon)));
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						Main.mouseLeftRelease = false;
						Main.mapFullscreen = false;
						PlayerInput.LockGamepadButtons("MouseLeft");
						Main.PylonSystem.RequestTeleportation(info, Main.LocalPlayer);
						SoundEngine.PlaySound(11);
					}
				}
			}
		}
	}
}
