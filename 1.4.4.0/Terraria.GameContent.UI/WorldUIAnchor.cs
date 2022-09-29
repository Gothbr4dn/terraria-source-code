using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI
{
	public class WorldUIAnchor
	{
		public enum AnchorType
		{
			Entity,
			Tile,
			Pos,
			None
		}

		public AnchorType type;

		public Entity entity;

		public Vector2 pos = Vector2.Zero;

		public Vector2 size = Vector2.Zero;

		public WorldUIAnchor()
		{
			type = AnchorType.None;
		}

		public WorldUIAnchor(Entity anchor)
		{
			type = AnchorType.Entity;
			entity = anchor;
		}

		public WorldUIAnchor(Vector2 anchor)
		{
			type = AnchorType.Pos;
			pos = anchor;
		}

		public WorldUIAnchor(int topLeftX, int topLeftY, int width, int height)
		{
			type = AnchorType.Tile;
			pos = new Vector2((float)topLeftX + (float)width / 2f, (float)topLeftY + (float)height / 2f) * 16f;
			size = new Vector2(width, height) * 16f;
		}

		public bool InRange(Vector2 target, float tileRangeX, float tileRangeY)
		{
			switch (type)
			{
			case AnchorType.Entity:
				if (Math.Abs(target.X - entity.Center.X) <= tileRangeX * 16f + (float)entity.width / 2f)
				{
					return Math.Abs(target.Y - entity.Center.Y) <= tileRangeY * 16f + (float)entity.height / 2f;
				}
				return false;
			case AnchorType.Pos:
				if (Math.Abs(target.X - pos.X) <= tileRangeX * 16f)
				{
					return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16f;
				}
				return false;
			case AnchorType.Tile:
				if (Math.Abs(target.X - pos.X) <= tileRangeX * 16f + size.X / 2f)
				{
					return Math.Abs(target.Y - pos.Y) <= tileRangeY * 16f + size.Y / 2f;
				}
				return false;
			default:
				return true;
			}
		}
	}
}
