using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding
{
	public class ShapeData
	{
		private HashSet<Point16> _points;

		public int Count => _points.Count;

		public ShapeData()
		{
			_points = new HashSet<Point16>();
		}

		public ShapeData(ShapeData original)
		{
			_points = new HashSet<Point16>(original._points);
		}

		public void Add(int x, int y)
		{
			_points.Add(new Point16(x, y));
		}

		public void Remove(int x, int y)
		{
			_points.Remove(new Point16(x, y));
		}

		public HashSet<Point16> GetData()
		{
			return _points;
		}

		public void Clear()
		{
			_points.Clear();
		}

		public bool Contains(int x, int y)
		{
			return _points.Contains(new Point16(x, y));
		}

		public void Add(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
		{
			foreach (Point16 datum in shapeData.GetData())
			{
				Add(remoteOrigin.X - localOrigin.X + datum.X, remoteOrigin.Y - localOrigin.Y + datum.Y);
			}
		}

		public void Subtract(ShapeData shapeData, Point localOrigin, Point remoteOrigin)
		{
			foreach (Point16 datum in shapeData.GetData())
			{
				Remove(remoteOrigin.X - localOrigin.X + datum.X, remoteOrigin.Y - localOrigin.Y + datum.Y);
			}
		}

		public static Rectangle GetBounds(Point origin, params ShapeData[] shapes)
		{
			int num = shapes[0]._points.First().X;
			int num2 = num;
			int num3 = shapes[0]._points.First().Y;
			int num4 = num3;
			for (int i = 0; i < shapes.Length; i++)
			{
				foreach (Point16 point in shapes[i]._points)
				{
					num = Math.Max(num, point.X);
					num2 = Math.Min(num2, point.X);
					num3 = Math.Max(num3, point.Y);
					num4 = Math.Min(num4, point.Y);
				}
			}
			return new Rectangle(num2 + origin.X, num4 + origin.Y, num - num2, num3 - num4);
		}
	}
}
