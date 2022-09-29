using System.IO;

namespace Terraria.DataStructures
{
	public struct TrackedProjectileReference
	{
		public int ProjectileLocalIndex { get; private set; }

		public int ProjectileOwnerIndex { get; private set; }

		public int ProjectileIdentity { get; private set; }

		public int ProjectileType { get; private set; }

		public bool IsTrackingSomething { get; private set; }

		public void Set(Projectile proj)
		{
			ProjectileLocalIndex = proj.whoAmI;
			ProjectileOwnerIndex = proj.owner;
			ProjectileIdentity = proj.identity;
			ProjectileType = proj.type;
			IsTrackingSomething = true;
		}

		public void Clear()
		{
			ProjectileLocalIndex = -1;
			ProjectileOwnerIndex = -1;
			ProjectileIdentity = -1;
			ProjectileType = -1;
			IsTrackingSomething = false;
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write((short)ProjectileOwnerIndex);
			if (ProjectileOwnerIndex != -1)
			{
				writer.Write((short)ProjectileIdentity);
				writer.Write((short)ProjectileType);
			}
		}

		public bool IsTracking(Projectile proj)
		{
			return proj.whoAmI == ProjectileLocalIndex;
		}

		public void TryReading(BinaryReader reader)
		{
			int num = reader.ReadInt16();
			if (num == -1)
			{
				Clear();
				return;
			}
			int expectedIdentity = reader.ReadInt16();
			int expectedType = reader.ReadInt16();
			Projectile projectile = FindMatchingProjectile(num, expectedIdentity, expectedType);
			if (projectile == null)
			{
				Clear();
			}
			else
			{
				Set(projectile);
			}
		}

		private Projectile FindMatchingProjectile(int expectedOwner, int expectedIdentity, int expectedType)
		{
			if (expectedOwner == -1)
			{
				return null;
			}
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.type == expectedType && projectile.owner == expectedOwner && projectile.identity == expectedIdentity)
				{
					return projectile;
				}
			}
			return null;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TrackedProjectileReference other))
			{
				return false;
			}
			return Equals(other);
		}

		public bool Equals(TrackedProjectileReference other)
		{
			if (ProjectileLocalIndex == other.ProjectileLocalIndex && ProjectileOwnerIndex == other.ProjectileOwnerIndex && ProjectileIdentity == other.ProjectileIdentity)
			{
				return ProjectileType == other.ProjectileType;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return (((((ProjectileLocalIndex * 397) ^ ProjectileOwnerIndex) * 397) ^ ProjectileIdentity) * 397) ^ ProjectileType;
		}

		public static bool operator ==(TrackedProjectileReference c1, TrackedProjectileReference c2)
		{
			return c1.Equals(c2);
		}

		public static bool operator !=(TrackedProjectileReference c1, TrackedProjectileReference c2)
		{
			return !c1.Equals(c2);
		}
	}
}
