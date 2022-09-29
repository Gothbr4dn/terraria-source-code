using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	internal interface IBigProgressBar
	{
		bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info);

		void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch);
	}
}
