using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	public struct DrawData
	{
		public Texture2D texture;

		public Vector2 position;

		public Rectangle destinationRectangle;

		public Rectangle? sourceRect;

		public Color color;

		public float rotation;

		public Vector2 origin;

		public Vector2 scale;

		public SpriteEffects effect;

		public int shader;

		public bool ignorePlayerRotation;

		public readonly bool useDestinationRectangle;

		public static Rectangle? nullRectangle;

		public DrawData(Texture2D texture, Vector2 position, Color color)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			destinationRectangle = default(Rectangle);
			sourceRect = nullRectangle;
			rotation = 0f;
			origin = Vector2.Zero;
			scale = Vector2.One;
			effect = SpriteEffects.None;
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			destinationRectangle = default(Rectangle);
			this.sourceRect = sourceRect;
			rotation = 0f;
			origin = Vector2.Zero;
			scale = Vector2.One;
			effect = SpriteEffects.None;
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.position = position;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.scale = new Vector2(scale, scale);
			this.effect = effect;
			destinationRectangle = default(Rectangle);
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.position = position;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.scale = scale;
			this.effect = effect;
			destinationRectangle = default(Rectangle);
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.color = color;
			position = Vector2.Zero;
			sourceRect = nullRectangle;
			rotation = 0f;
			origin = Vector2.Zero;
			scale = Vector2.One;
			effect = SpriteEffects.None;
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.color = color;
			position = Vector2.Zero;
			this.sourceRect = sourceRect;
			rotation = 0f;
			origin = Vector2.Zero;
			scale = Vector2.One;
			effect = SpriteEffects.None;
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.effect = effect;
			position = Vector2.Zero;
			scale = Vector2.One;
			shader = 0;
			ignorePlayerRotation = false;
			useDestinationRectangle = false;
		}

		public void Draw(SpriteBatch sb)
		{
			if (useDestinationRectangle)
			{
				sb.Draw(texture, destinationRectangle, sourceRect, color, rotation, origin, effect, 0f);
			}
			else
			{
				sb.Draw(texture, position, sourceRect, color, rotation, origin, scale, effect, 0f);
			}
		}
	}
}
