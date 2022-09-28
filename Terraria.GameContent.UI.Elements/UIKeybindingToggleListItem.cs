using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIKeybindingToggleListItem : UIElement
	{
		private Color _color;

		private Func<string> _TextDisplayFunction;

		private Func<bool> _IsOnFunction;

		private Asset<Texture2D> _toggleTexture;

		public UIKeybindingToggleListItem(Func<string> getText, Func<bool> getStatus, Color color)
		{
			_color = color;
			_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", (AssetRequestMode)1);
			_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
			_IsOnFunction = ((getStatus != null) ? getStatus : ((Func<bool>)(() => false)));
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			Vector2 baseScale = new Vector2(0.8f);
			Color value = (false ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver));
			value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color = (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
			Vector2 position = vector;
			Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
			position.X += 8f;
			position.Y += 2f + num;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), _TextDisplayFunction(), position, value, 0f, Vector2.Zero, baseScale, num2);
			position.X -= 17f;
			Rectangle value2 = new Rectangle(_IsOnFunction() ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
			Vector2 vector2 = new Vector2(value2.Width, 0f);
			spriteBatch.Draw(position: new Vector2(dimensions.X + dimensions.Width - vector2.X - 10f, dimensions.Y + 2f + num), texture: _toggleTexture.get_Value(), sourceRectangle: value2, color: Color.White, rotation: 0f, origin: Vector2.Zero, scale: Vector2.One, effects: SpriteEffects.None, layerDepth: 0f);
		}
	}
}
