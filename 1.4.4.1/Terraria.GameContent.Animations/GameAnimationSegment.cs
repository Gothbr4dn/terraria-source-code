using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Animations
{
	public struct GameAnimationSegment
	{
		public SpriteBatch SpriteBatch;

		public Vector2 AnchorPositionOnScreen;

		public int TimeInAnimation;

		public float DisplayOpacity;
	}
}
