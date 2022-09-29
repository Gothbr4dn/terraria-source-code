using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Renderers
{
	public interface IPlayerRenderer
	{
		void DrawPlayers(Camera camera, IEnumerable<Player> players);

		void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color));

		void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f);
	}
}
