using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class EmoteButton : UIElement
	{
		private Asset<Texture2D> _texture;

		private Asset<Texture2D> _textureBorder;

		private int _emoteIndex;

		private bool _hovered;

		private int _frameCounter;

		public EmoteButton(int emoteIndex)
		{
			_texture = Main.Assets.Request<Texture2D>("Images/Extra_" + (short)48, (AssetRequestMode)1);
			_textureBorder = Main.Assets.Request<Texture2D>("Images/UI/EmoteBubbleBorder", (AssetRequestMode)1);
			_emoteIndex = emoteIndex;
			Rectangle frame = GetFrame();
			Width.Set(frame.Width, 0f);
			Height.Set(frame.Height, 0f);
		}

		private Rectangle GetFrame()
		{
			int num = ((_frameCounter >= 10) ? 1 : 0);
			return _texture.Frame(8, 39, _emoteIndex % 4 * 2 + num, _emoteIndex / 4 + 1);
		}

		private void UpdateFrame()
		{
			if (++_frameCounter >= 20)
			{
				_frameCounter = 0;
			}
		}

		public override void Update(GameTime gameTime)
		{
			UpdateFrame();
			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Vector2 vector = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
			Rectangle frame = GetFrame();
			Rectangle value = frame;
			value.X = _texture.Width() / 8;
			value.Y = 0;
			Vector2 origin = frame.Size() / 2f;
			Color white = Color.White;
			Color color = Color.Black;
			if (_hovered)
			{
				color = Main.OurFavoriteColor;
			}
			spriteBatch.Draw(_texture.get_Value(), vector, value, white, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(_texture.get_Value(), vector, frame, white, 0f, origin, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(_textureBorder.get_Value(), vector - Vector2.One * 2f, null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (_hovered)
			{
				string name = EmoteID.Search.GetName(_emoteIndex);
				string cursorText = "/" + Language.GetTextValue("EmojiName." + name);
				Main.instance.MouseText(cursorText, 0, 0);
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			_hovered = false;
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			EmoteBubble.MakeLocalPlayerEmote(_emoteIndex);
			IngameFancyUI.Close();
		}
	}
}
