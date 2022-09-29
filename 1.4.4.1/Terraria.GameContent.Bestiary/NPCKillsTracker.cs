using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
	public class NPCKillsTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		private object _entryCreationLock = new object();

		public const int POSITIVE_KILL_COUNT_CAP = 999999999;

		private Dictionary<string, int> _killCountsByNpcId;

		public NPCKillsTracker()
		{
			_killCountsByNpcId = new Dictionary<string, int>();
		}

		public void RegisterKill(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			_killCountsByNpcId.TryGetValue(bestiaryCreditId, out var value);
			value++;
			lock (_entryCreationLock)
			{
				_killCountsByNpcId[bestiaryCreditId] = Utils.Clamp(value, 0, 999999999);
			}
			if (Main.netMode == 2)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeKillCount(npc.netID, value));
			}
		}

		public int GetKillCount(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return GetKillCount(bestiaryCreditId);
		}

		public void SetKillCountDirectly(string persistentId, int killCount)
		{
			lock (_entryCreationLock)
			{
				_killCountsByNpcId[persistentId] = Utils.Clamp(killCount, 0, 999999999);
			}
		}

		public int GetKillCount(string persistentId)
		{
			_killCountsByNpcId.TryGetValue(persistentId, out var value);
			return value;
		}

		public void Save(BinaryWriter writer)
		{
			lock (_killCountsByNpcId)
			{
				writer.Write(_killCountsByNpcId.Count);
				foreach (KeyValuePair<string, int> item in _killCountsByNpcId)
				{
					writer.Write(item.Key);
					writer.Write(item.Value);
				}
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				_killCountsByNpcId[key] = value;
			}
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
				reader.ReadInt32();
			}
		}

		public void Reset()
		{
			_killCountsByNpcId.Clear();
		}

		public void OnPlayerJoining(int playerIndex)
		{
			foreach (KeyValuePair<string, int> item in _killCountsByNpcId)
			{
				if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(item.Key, out var value))
				{
					NetManager.Instance.SendToClient(NetBestiaryModule.SerializeKillCount(value, item.Value), playerIndex);
				}
			}
		}
	}
}
