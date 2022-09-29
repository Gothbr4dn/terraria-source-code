using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	public abstract class Entity
	{
		public int whoAmI;

		public bool active;

		internal long entityId;

		public Vector2 position;

		public Vector2 velocity;

		public Vector2 oldPosition;

		public Vector2 oldVelocity;

		public int oldDirection;

		public int direction = 1;

		public int width;

		public int height;

		public bool wet;

		public bool shimmerWet;

		public bool honeyWet;

		public byte wetCount;

		public bool lavaWet;

		public virtual Vector2 VisualPosition => position;

		public Vector2 Center
		{
			get
			{
				return new Vector2(position.X + (float)(width / 2), position.Y + (float)(height / 2));
			}
			set
			{
				position = new Vector2(value.X - (float)(width / 2), value.Y - (float)(height / 2));
			}
		}

		public Vector2 Left
		{
			get
			{
				return new Vector2(position.X, position.Y + (float)(height / 2));
			}
			set
			{
				position = new Vector2(value.X, value.Y - (float)(height / 2));
			}
		}

		public Vector2 Right
		{
			get
			{
				return new Vector2(position.X + (float)width, position.Y + (float)(height / 2));
			}
			set
			{
				position = new Vector2(value.X - (float)width, value.Y - (float)(height / 2));
			}
		}

		public Vector2 Top
		{
			get
			{
				return new Vector2(position.X + (float)(width / 2), position.Y);
			}
			set
			{
				position = new Vector2(value.X - (float)(width / 2), value.Y);
			}
		}

		public Vector2 TopLeft
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		public Vector2 TopRight
		{
			get
			{
				return new Vector2(position.X + (float)width, position.Y);
			}
			set
			{
				position = new Vector2(value.X - (float)width, value.Y);
			}
		}

		public Vector2 Bottom
		{
			get
			{
				return new Vector2(position.X + (float)(width / 2), position.Y + (float)height);
			}
			set
			{
				position = new Vector2(value.X - (float)(width / 2), value.Y - (float)height);
			}
		}

		public Vector2 BottomLeft
		{
			get
			{
				return new Vector2(position.X, position.Y + (float)height);
			}
			set
			{
				position = new Vector2(value.X, value.Y - (float)height);
			}
		}

		public Vector2 BottomRight
		{
			get
			{
				return new Vector2(position.X + (float)width, position.Y + (float)height);
			}
			set
			{
				position = new Vector2(value.X - (float)width, value.Y - (float)height);
			}
		}

		public Vector2 Size
		{
			get
			{
				return new Vector2(width, height);
			}
			set
			{
				width = (int)value.X;
				height = (int)value.Y;
			}
		}

		public Rectangle Hitbox
		{
			get
			{
				return new Rectangle((int)position.X, (int)position.Y, width, height);
			}
			set
			{
				position = new Vector2(value.X, value.Y);
				width = value.Width;
				height = value.Height;
			}
		}

		public float AngleTo(Vector2 Destination)
		{
			return (float)Math.Atan2(Destination.Y - Center.Y, Destination.X - Center.X);
		}

		public float AngleFrom(Vector2 Source)
		{
			return (float)Math.Atan2(Center.Y - Source.Y, Center.X - Source.X);
		}

		public float Distance(Vector2 Other)
		{
			return Vector2.Distance(Center, Other);
		}

		public float DistanceSQ(Vector2 Other)
		{
			return Vector2.DistanceSquared(Center, Other);
		}

		public Vector2 DirectionTo(Vector2 Destination)
		{
			return Vector2.Normalize(Destination - Center);
		}

		public Vector2 DirectionFrom(Vector2 Source)
		{
			return Vector2.Normalize(Center - Source);
		}

		public bool WithinRange(Vector2 Target, float MaxRange)
		{
			return Vector2.DistanceSquared(Center, Target) <= MaxRange * MaxRange;
		}
	}
}
