using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
	public interface IInGameNotification
	{
		object CreationObject { get; }

		bool ShouldBeRemoved { get; }

		void Update();

		void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition);

		void PushAnchor(ref Vector2 positionAnchorBottom);

		void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse);
	}
}
