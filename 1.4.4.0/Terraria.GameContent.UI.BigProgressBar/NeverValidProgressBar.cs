using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class NeverValidProgressBar : IBigProgressBar
	{
		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			return false;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
		}
	}
}
