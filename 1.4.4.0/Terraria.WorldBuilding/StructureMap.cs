using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.WorldBuilding
{
	public class StructureMap
	{
		private readonly List<Rectangle> _structures = new List<Rectangle>(2048);

		private readonly List<Rectangle> _protectedStructures = new List<Rectangle>(2048);

		private readonly object _lock = new object();

		public bool CanPlace(Rectangle area, int padding = 0)
		{
			return CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
		}

		public bool CanPlace(Rectangle area, bool[] validTiles, int padding = 0)
		{
			lock (_lock)
			{
				if (area.X < 0 || area.Y < 0 || area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1)
				{
					return false;
				}
				Rectangle rectangle = new Rectangle(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);
				for (int i = 0; i < _protectedStructures.Count; i++)
				{
					if (rectangle.Intersects(_protectedStructures[i]))
					{
						return false;
					}
				}
				for (int j = rectangle.X; j < rectangle.X + rectangle.Width; j++)
				{
					for (int k = rectangle.Y; k < rectangle.Y + rectangle.Height; k++)
					{
						if (Main.tile[j, k].active())
						{
							ushort type = Main.tile[j, k].type;
							if (!validTiles[type])
							{
								return false;
							}
						}
					}
				}
				return true;
			}
		}

		public Rectangle GetBoundingBox()
		{
			lock (_lock)
			{
				if (_structures.Count == 0)
				{
					return Rectangle.Empty;
				}
				Point point = new Point(_structures.Min((Rectangle rect) => rect.Left), _structures.Min((Rectangle rect) => rect.Top));
				Point point2 = new Point(_structures.Max((Rectangle rect) => rect.Right), _structures.Max((Rectangle rect) => rect.Bottom));
				return new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			}
		}

		public void AddStructure(Rectangle area, int padding = 0)
		{
			lock (_lock)
			{
				area.Inflate(padding, padding);
				_structures.Add(area);
			}
		}

		public void AddProtectedStructure(Rectangle area, int padding = 0)
		{
			lock (_lock)
			{
				area.Inflate(padding, padding);
				_structures.Add(area);
				_protectedStructures.Add(area);
			}
		}

		public void Reset()
		{
			lock (_lock)
			{
				_protectedStructures.Clear();
			}
		}
	}
}
