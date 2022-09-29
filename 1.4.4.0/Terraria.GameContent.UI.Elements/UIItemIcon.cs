using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIItemIcon : UIElement
	{
		private Item _item;

		private bool _blackedOut;

		public UIItemIcon(Item item, bool blackedOut)
		{
			_item = item;
			Width.Set(32f, 0f);
			Height.Set(32f, 0f);
			_blackedOut = blackedOut;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			ItemSlot.DrawItemIcon(screenPositionForItemCenter: GetDimensions().Center(), item: _item, context: 31, spriteBatch: spriteBatch, scale: _item.scale, sizeLimit: 32f, environmentColor: _blackedOut ? Color.Black : Color.White);
		}
	}
}
