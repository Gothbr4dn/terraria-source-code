using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetLiquidModule : NetModule
	{
		private class ChunkChanges
		{
			public HashSet<int> DirtiedPackedTileCoords;

			public int ChunkX;

			public int ChunkY;

			public ChunkChanges(int x, int y)
			{
				ChunkX = x;
				ChunkY = y;
				DirtiedPackedTileCoords = new HashSet<int>();
			}

			public bool BroadcastingCondition(int clientIndex)
			{
				return Netplay.Clients[clientIndex].TileSections[ChunkX, ChunkY];
			}
		}

		private static List<int> _changesForPlayerCache = new List<int>();

		private static Dictionary<Point, ChunkChanges> _changesByChunkCoords = new Dictionary<Point, ChunkChanges>();

		public static NetPacket Serialize(HashSet<int> changes)
		{
			NetPacket result = NetModule.CreatePacket<NetLiquidModule>(changes.Count * 6 + 2);
			result.Writer.Write((ushort)changes.Count);
			foreach (int change in changes)
			{
				int num = (change >> 16) & 0xFFFF;
				int num2 = change & 0xFFFF;
				result.Writer.Write(change);
				result.Writer.Write(Main.tile[num, num2].liquid);
				result.Writer.Write(Main.tile[num, num2].liquidType());
			}
			return result;
		}

		public static NetPacket SerializeForPlayer(int playerIndex)
		{
			_changesForPlayerCache.Clear();
			foreach (KeyValuePair<Point, ChunkChanges> changesByChunkCoord in _changesByChunkCoords)
			{
				if (changesByChunkCoord.Value.BroadcastingCondition(playerIndex))
				{
					_changesForPlayerCache.AddRange(changesByChunkCoord.Value.DirtiedPackedTileCoords);
				}
			}
			NetPacket result = NetModule.CreatePacket<NetLiquidModule>(_changesForPlayerCache.Count * 6 + 2);
			result.Writer.Write((ushort)_changesForPlayerCache.Count);
			foreach (int item in _changesForPlayerCache)
			{
				int num = (item >> 16) & 0xFFFF;
				int num2 = item & 0xFFFF;
				result.Writer.Write(item);
				result.Writer.Write(Main.tile[num, num2].liquid);
				result.Writer.Write(Main.tile[num, num2].liquidType());
			}
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			int num = reader.ReadUInt16();
			for (int i = 0; i < num; i++)
			{
				int num2 = reader.ReadInt32();
				byte liquid = reader.ReadByte();
				byte liquidType = reader.ReadByte();
				int num3 = (num2 >> 16) & 0xFFFF;
				int num4 = num2 & 0xFFFF;
				Tile tile = Main.tile[num3, num4];
				if (tile != null)
				{
					tile.liquid = liquid;
					tile.liquidType(liquidType);
				}
			}
			return true;
		}

		public static void CreateAndBroadcastByChunk(HashSet<int> dirtiedPackedTileCoords)
		{
			PrepareChunks(dirtiedPackedTileCoords);
			PrepareAndSendToEachPlayerSeparately();
		}

		private static void PrepareAndSendToEachPlayerSeparately()
		{
			for (int i = 0; i < 256; i++)
			{
				if (Netplay.Clients[i].IsConnected())
				{
					NetManager.Instance.SendToClient(SerializeForPlayer(i), i);
				}
			}
		}

		private static void BroadcastEachChunkSeparately()
		{
			foreach (KeyValuePair<Point, ChunkChanges> changesByChunkCoord in _changesByChunkCoords)
			{
				NetManager.Instance.Broadcast(Serialize(changesByChunkCoord.Value.DirtiedPackedTileCoords), changesByChunkCoord.Value.BroadcastingCondition);
			}
		}

		private static void PrepareChunks(HashSet<int> dirtiedPackedTileCoords)
		{
			foreach (KeyValuePair<Point, ChunkChanges> changesByChunkCoord in _changesByChunkCoords)
			{
				changesByChunkCoord.Value.DirtiedPackedTileCoords.Clear();
			}
			DistributeChangesIntoChunks(dirtiedPackedTileCoords);
		}

		private static void BroadcastAllChanges(HashSet<int> dirtiedPackedTileCoords)
		{
			NetManager.Instance.Broadcast(Serialize(dirtiedPackedTileCoords));
		}

		private static void DistributeChangesIntoChunks(HashSet<int> dirtiedPackedTileCoords)
		{
			Point key = default(Point);
			foreach (int dirtiedPackedTileCoord in dirtiedPackedTileCoords)
			{
				int x = (dirtiedPackedTileCoord >> 16) & 0xFFFF;
				int y = dirtiedPackedTileCoord & 0xFFFF;
				key.X = Netplay.GetSectionX(x);
				key.Y = Netplay.GetSectionY(y);
				if (!_changesByChunkCoords.TryGetValue(key, out var value))
				{
					value = new ChunkChanges(key.X, key.Y);
					_changesByChunkCoords[key] = value;
				}
				value.DirtiedPackedTileCoords.Add(dirtiedPackedTileCoord);
			}
		}
	}
}
