using System.IO;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	public class PlayerDeathReason
	{
		private int _sourcePlayerIndex = -1;

		private int _sourceNPCIndex = -1;

		private int _sourceProjectileLocalIndex = -1;

		private int _sourceOtherIndex = -1;

		private int _sourceProjectileType;

		private int _sourceItemType;

		private int _sourceItemPrefix;

		private string _sourceCustomReason;

		public int? SourceProjectileType
		{
			get
			{
				if (_sourceProjectileLocalIndex == -1)
				{
					return null;
				}
				return _sourceProjectileType;
			}
		}

		public bool TryGetCausingEntity(out Entity entity)
		{
			entity = null;
			if (Main.npc.IndexInRange(_sourceNPCIndex))
			{
				entity = Main.npc[_sourceNPCIndex];
				return true;
			}
			if (Main.projectile.IndexInRange(_sourceProjectileLocalIndex))
			{
				entity = Main.projectile[_sourceProjectileLocalIndex];
				return true;
			}
			if (Main.player.IndexInRange(_sourcePlayerIndex))
			{
				entity = Main.player[_sourcePlayerIndex];
				return true;
			}
			return false;
		}

		public static PlayerDeathReason LegacyEmpty()
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = 254
			};
		}

		public static PlayerDeathReason LegacyDefault()
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = 255
			};
		}

		public static PlayerDeathReason ByNPC(int index)
		{
			return new PlayerDeathReason
			{
				_sourceNPCIndex = index
			};
		}

		public static PlayerDeathReason ByCustomReason(string reasonInEnglish)
		{
			return new PlayerDeathReason
			{
				_sourceCustomReason = reasonInEnglish
			};
		}

		public static PlayerDeathReason ByPlayer(int index)
		{
			return new PlayerDeathReason
			{
				_sourcePlayerIndex = index,
				_sourceItemType = Main.player[index].inventory[Main.player[index].selectedItem].type,
				_sourceItemPrefix = Main.player[index].inventory[Main.player[index].selectedItem].prefix
			};
		}

		public static PlayerDeathReason ByOther(int type)
		{
			return new PlayerDeathReason
			{
				_sourceOtherIndex = type
			};
		}

		public static PlayerDeathReason ByProjectile(int playerIndex, int projectileIndex)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason
			{
				_sourcePlayerIndex = playerIndex,
				_sourceProjectileLocalIndex = projectileIndex,
				_sourceProjectileType = Main.projectile[projectileIndex].type
			};
			if (playerIndex >= 0 && playerIndex <= 255)
			{
				playerDeathReason._sourceItemType = Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].type;
				playerDeathReason._sourceItemPrefix = Main.player[playerIndex].inventory[Main.player[playerIndex].selectedItem].prefix;
			}
			return playerDeathReason;
		}

		public NetworkText GetDeathText(string deadPlayerName)
		{
			if (_sourceCustomReason != null)
			{
				return NetworkText.FromLiteral(_sourceCustomReason);
			}
			return Lang.CreateDeathMessage(deadPlayerName, _sourcePlayerIndex, _sourceNPCIndex, _sourceProjectileLocalIndex, _sourceOtherIndex, _sourceProjectileType, _sourceItemType);
		}

		public void WriteSelfTo(BinaryWriter writer)
		{
			BitsByte bitsByte = (byte)0;
			bitsByte[0] = _sourcePlayerIndex != -1;
			bitsByte[1] = _sourceNPCIndex != -1;
			bitsByte[2] = _sourceProjectileLocalIndex != -1;
			bitsByte[3] = _sourceOtherIndex != -1;
			bitsByte[4] = _sourceProjectileType != 0;
			bitsByte[5] = _sourceItemType != 0;
			bitsByte[6] = _sourceItemPrefix != 0;
			bitsByte[7] = _sourceCustomReason != null;
			writer.Write(bitsByte);
			if (bitsByte[0])
			{
				writer.Write((short)_sourcePlayerIndex);
			}
			if (bitsByte[1])
			{
				writer.Write((short)_sourceNPCIndex);
			}
			if (bitsByte[2])
			{
				writer.Write((short)_sourceProjectileLocalIndex);
			}
			if (bitsByte[3])
			{
				writer.Write((byte)_sourceOtherIndex);
			}
			if (bitsByte[4])
			{
				writer.Write((short)_sourceProjectileType);
			}
			if (bitsByte[5])
			{
				writer.Write((short)_sourceItemType);
			}
			if (bitsByte[6])
			{
				writer.Write((byte)_sourceItemPrefix);
			}
			if (bitsByte[7])
			{
				writer.Write(_sourceCustomReason);
			}
		}

		public static PlayerDeathReason FromReader(BinaryReader reader)
		{
			PlayerDeathReason playerDeathReason = new PlayerDeathReason();
			BitsByte bitsByte = reader.ReadByte();
			if (bitsByte[0])
			{
				playerDeathReason._sourcePlayerIndex = reader.ReadInt16();
			}
			if (bitsByte[1])
			{
				playerDeathReason._sourceNPCIndex = reader.ReadInt16();
			}
			if (bitsByte[2])
			{
				playerDeathReason._sourceProjectileLocalIndex = reader.ReadInt16();
			}
			if (bitsByte[3])
			{
				playerDeathReason._sourceOtherIndex = reader.ReadByte();
			}
			if (bitsByte[4])
			{
				playerDeathReason._sourceProjectileType = reader.ReadInt16();
			}
			if (bitsByte[5])
			{
				playerDeathReason._sourceItemType = reader.ReadInt16();
			}
			if (bitsByte[6])
			{
				playerDeathReason._sourceItemPrefix = reader.ReadByte();
			}
			if (bitsByte[7])
			{
				playerDeathReason._sourceCustomReason = reader.ReadString();
			}
			return playerDeathReason;
		}
	}
}
