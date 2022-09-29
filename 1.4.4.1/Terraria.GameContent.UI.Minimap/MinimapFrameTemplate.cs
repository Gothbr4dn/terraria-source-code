using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.Minimap
{
	public class MinimapFrameTemplate
	{
		private string name;

		private Vector2 frameOffset;

		private Vector2 resetPosition;

		private Vector2 zoomInPosition;

		private Vector2 zoomOutPosition;

		public MinimapFrameTemplate(string name, Vector2 frameOffset, Vector2 resetPosition, Vector2 zoomInPosition, Vector2 zoomOutPosition)
		{
			this.name = name;
			this.frameOffset = frameOffset;
			this.resetPosition = resetPosition;
			this.zoomInPosition = zoomInPosition;
			this.zoomOutPosition = zoomOutPosition;
		}

		public MinimapFrame CreateInstance(AssetRequestMode mode)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			MinimapFrame minimapFrame = new MinimapFrame(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapFrame", mode), frameOffset);
			minimapFrame.NameKey = name;
			minimapFrame.ConfigKey = name;
			minimapFrame.SetResetButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_Reset", mode), resetPosition);
			minimapFrame.SetZoomOutButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_ZoomOut", mode), zoomOutPosition);
			minimapFrame.SetZoomInButton(LoadAsset<Texture2D>("Images\\UI\\Minimap\\" + name + "\\MinimapButton_ZoomIn", mode), zoomInPosition);
			return minimapFrame;
		}

		private static Asset<T> LoadAsset<T>(string assetName, AssetRequestMode mode) where T : class
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Main.Assets.Request<T>(assetName, mode);
		}
	}
}
