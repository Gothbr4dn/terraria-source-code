using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	public class TreeTopsInfo
	{
		public class AreaId
		{
			public static SetFactory Factory = new SetFactory(13);

			public const int Forest1 = 0;

			public const int Forest2 = 1;

			public const int Forest3 = 2;

			public const int Forest4 = 3;

			public const int Corruption = 4;

			public const int Jungle = 5;

			public const int Snow = 6;

			public const int Hallow = 7;

			public const int Crimson = 8;

			public const int Desert = 9;

			public const int Ocean = 10;

			public const int GlowingMushroom = 11;

			public const int Underworld = 12;

			public const int Count = 13;
		}

		private int[] _variations = new int[13];

		public void Save(BinaryWriter writer)
		{
			writer.Write(_variations.Length);
			for (int i = 0; i < _variations.Length; i++)
			{
				writer.Write(_variations[i]);
			}
		}

		public void Load(BinaryReader reader, int loadVersion)
		{
			if (loadVersion < 211)
			{
				CopyExistingWorldInfo();
				return;
			}
			int num = reader.ReadInt32();
			for (int i = 0; i < num && i < _variations.Length; i++)
			{
				_variations[i] = reader.ReadInt32();
			}
		}

		public void SyncSend(BinaryWriter writer)
		{
			for (int i = 0; i < _variations.Length; i++)
			{
				writer.Write((byte)_variations[i]);
			}
		}

		public void SyncReceive(BinaryReader reader)
		{
			for (int i = 0; i < _variations.Length; i++)
			{
				int num = _variations[i];
				_variations[i] = reader.ReadByte();
				if (_variations[i] != num)
				{
					DoTreeFX(i);
				}
			}
		}

		public int GetTreeStyle(int areaId)
		{
			return _variations[areaId];
		}

		public void RandomizeTreeStyleBasedOnWorldPosition(UnifiedRandom rand, Vector2 worldPosition)
		{
			Point pt = new Point((int)(worldPosition.X / 16f), (int)(worldPosition.Y / 16f) + 1);
			Tile tileSafely = Framing.GetTileSafely(pt);
			if (tileSafely.active())
			{
				int num = -1;
				if (tileSafely.type == 70)
				{
					num = 11;
				}
				else if (tileSafely.type == 53 && WorldGen.oceanDepths(pt.X, pt.Y))
				{
					num = 10;
				}
				else if (tileSafely.type == 23)
				{
					num = 4;
				}
				else if (tileSafely.type == 199)
				{
					num = 8;
				}
				else if (tileSafely.type == 109 || tileSafely.type == 492)
				{
					num = 7;
				}
				else if (tileSafely.type == 53)
				{
					num = 9;
				}
				else if (tileSafely.type == 147)
				{
					num = 6;
				}
				else if (tileSafely.type == 60)
				{
					num = 5;
				}
				else if (tileSafely.type == 633)
				{
					num = 12;
				}
				else if (tileSafely.type == 2 || tileSafely.type == 477)
				{
					num = ((pt.X >= Main.treeX[0]) ? ((pt.X < Main.treeX[1]) ? 1 : ((pt.X >= Main.treeX[2]) ? 3 : 2)) : 0);
				}
				if (num > -1)
				{
					RandomizeTreeStyle(rand, num);
				}
			}
		}

		public void RandomizeTreeStyle(UnifiedRandom rand, int areaId)
		{
			int num = _variations[areaId];
			bool flag = false;
			while (_variations[areaId] == num)
			{
				switch (areaId)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					_variations[areaId] = rand.Next(6);
					break;
				case 4:
					_variations[areaId] = rand.Next(5);
					break;
				case 5:
					_variations[areaId] = rand.Next(6);
					break;
				case 6:
					_variations[areaId] = rand.NextFromList<int>(0, 1, 2, 21, 22, 3, 31, 32, 4, 41, 42, 5, 6, 7);
					break;
				case 7:
					_variations[areaId] = rand.Next(5);
					break;
				case 8:
					_variations[areaId] = rand.Next(6);
					break;
				case 9:
					_variations[areaId] = rand.Next(5);
					break;
				case 10:
					_variations[areaId] = rand.Next(6);
					break;
				case 11:
					_variations[areaId] = rand.Next(4);
					break;
				case 12:
					_variations[areaId] = rand.Next(6);
					break;
				default:
					flag = true;
					break;
				}
				if (flag)
				{
					break;
				}
			}
			if (num != _variations[areaId])
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(7);
				}
				else
				{
					DoTreeFX(areaId);
				}
			}
		}

		private void DoTreeFX(int areaID)
		{
		}

		public void CopyExistingWorldInfoForWorldGeneration()
		{
			CopyExistingWorldInfo();
		}

		private void CopyExistingWorldInfo()
		{
			_variations[0] = Main.treeStyle[0];
			_variations[1] = Main.treeStyle[1];
			_variations[2] = Main.treeStyle[2];
			_variations[3] = Main.treeStyle[3];
			_variations[4] = WorldGen.corruptBG;
			_variations[5] = WorldGen.jungleBG;
			_variations[6] = WorldGen.snowBG;
			_variations[7] = WorldGen.hallowBG;
			_variations[8] = WorldGen.crimsonBG;
			_variations[9] = WorldGen.desertBG;
			_variations[10] = WorldGen.oceanBG;
			_variations[11] = WorldGen.mushroomBG;
			_variations[12] = WorldGen.underworldBG;
		}
	}
}
