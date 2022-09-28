using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	public class SpelunkerProjectileHelper
	{
		private HashSet<Vector2> _positionsChecked = new HashSet<Vector2>();

		private HashSet<Point> _tilesChecked = new HashSet<Point>();

		private Rectangle _clampBox;

		private int _frameCounter;

		public void OnPreUpdateAllProjectiles()
		{
			_clampBox = new Rectangle(2, 2, Main.maxTilesX - 2, Main.maxTilesY - 2);
			if (++_frameCounter >= 10)
			{
				_frameCounter = 0;
				_tilesChecked.Clear();
				_positionsChecked.Clear();
			}
		}

		public void AddSpotToCheck(Vector2 spot)
		{
			if (_positionsChecked.Add(spot))
			{
				CheckSpot(spot);
			}
		}

		private void CheckSpot(Vector2 Center)
		{
			int num = (int)Center.X / 16;
			int num2 = (int)Center.Y / 16;
			int num3 = Utils.Clamp(num - 30, _clampBox.Left, _clampBox.Right);
			int num4 = Utils.Clamp(num + 30, _clampBox.Left, _clampBox.Right);
			int num5 = Utils.Clamp(num2 - 30, _clampBox.Top, _clampBox.Bottom);
			int num6 = Utils.Clamp(num2 + 30, _clampBox.Top, _clampBox.Bottom);
			Point item = default(Point);
			Vector2 position = default(Vector2);
			for (int i = num3; i <= num4; i++)
			{
				for (int j = num5; j <= num6; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.active() && Main.IsTileSpelunkable(tile) && !(new Vector2(num - i, num2 - j).Length() > 30f))
					{
						item.X = i;
						item.Y = j;
						if (_tilesChecked.Add(item) && Main.rand.Next(4) == 0)
						{
							position.X = i * 16;
							position.Y = j * 16;
							Dust dust = Dust.NewDustDirect(position, 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
							dust.fadeIn = 0.75f;
							dust.velocity *= 0.1f;
							dust.noLight = true;
						}
					}
				}
			}
		}
	}
}
