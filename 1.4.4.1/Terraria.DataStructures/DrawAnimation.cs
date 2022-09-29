using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	public class DrawAnimation
	{
		public int Frame;

		public int FrameCount;

		public int TicksPerFrame;

		public int FrameCounter;

		public virtual void Update()
		{
		}

		public virtual Rectangle GetFrame(Texture2D texture, int frameCounterOverride = -1)
		{
			return texture.Frame();
		}
	}
}
