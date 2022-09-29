using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria.UI.Chat
{
	public class TextSnippet
	{
		public string Text;

		public string TextOriginal;

		public Color Color = Color.White;

		public float Scale = 1f;

		public bool CheckForHover;

		public bool DeleteWhole;

		public TextSnippet(string text = "")
		{
			Text = text;
			TextOriginal = text;
		}

		public TextSnippet(string text, Color color, float scale = 1f)
		{
			Text = text;
			TextOriginal = text;
			Color = color;
			Scale = scale;
		}

		public virtual void Update()
		{
		}

		public virtual void OnHover()
		{
		}

		public virtual void OnClick()
		{
		}

		public virtual Color GetVisibleColor()
		{
			return ChatManager.WaveColor(Color);
		}

		public virtual bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
		{
			size = Vector2.Zero;
			return false;
		}

		public virtual TextSnippet CopyMorph(string newText)
		{
			TextSnippet obj = (TextSnippet)MemberwiseClone();
			obj.Text = newText;
			return obj;
		}

		public virtual float GetStringLength(DynamicSpriteFont font)
		{
			return font.MeasureString(Text).X * Scale;
		}

		public override string ToString()
		{
			return "Text: " + Text + " | OriginalText: " + TextOriginal;
		}
	}
}
