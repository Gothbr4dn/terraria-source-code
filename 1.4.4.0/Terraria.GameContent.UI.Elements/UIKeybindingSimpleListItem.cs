using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIKeybindingSimpleListItem : UIElement
	{
		private Color _color;

		private Func<string> _GetTextFunction;

		public UIKeybindingSimpleListItem(Func<string> getText, Color color)
		{
			_color = color;
			_GetTextFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			Vector2 baseScale = new Vector2(0.8f);
			Color value = (base.IsMouseHovering ? Color.White : Color.Silver);
			value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color = (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
			Vector2 position = vector;
			Utils.DrawSettings2Panel(spriteBatch, position, num2, color);
			position.X += 8f;
			position.Y += 2f + num;
			string text = _GetTextFunction();
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.get_Value(), text, baseScale);
			position.X = dimensions.X + dimensions.Width / 2f - stringSize.X / 2f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text, position, value, 0f, Vector2.Zero, baseScale, num2);
		}
	}
}
