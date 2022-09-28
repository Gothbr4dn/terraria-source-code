using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
	public class NPCWasChatWithTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		private object _entryCreationLock = new object();

		private HashSet<string> _chattedWithPlayer;

		public NPCWasChatWithTracker()
		{
			_chattedWithPlayer = new HashSet<string>();
		}

		public void RegisterChatStartWith(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			bool flag = !_chattedWithPlayer.Contains(bestiaryCreditId);
			lock (_entryCreationLock)
			{
				_chattedWithPlayer.Add(bestiaryCreditId);
			}
			if (Main.netMode == 2 && flag)
			{
				NetManager.Instance.Broadcast(NetBestiaryModule.SerializeChat(npc.netID));
			}
		}

		public void SetWasChatWithDirectly(string persistentId)
		{
			lock (_entryCreationLock)
			{
				_chattedWithPlayer.Add(persistentId);
			}
		}

		public bool GetWasChatWith(NPC npc)
		{
			string bestiaryCreditId = npc.GetBestiaryCreditId();
			return _chattedWithPlayer.Contains(bestiaryCreditId);
		}

		public bool GetWasChatWith(string persistentId)
		{
			return _chattedWithPlayer.Contains(persistentId);
		}

		public void Save(BinaryWriter writer)
		{
			lock (_entryCreationLock)
			{
				writer.Write(_chattedWithPlayer.Count);
				foreach (string item in _chattedWithPlayer)
				{
					writer.Write(item);
				}
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string item = reader.ReadString();
				_chattedWithPlayer.Add(item);
			}
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
			}
		}

		public void Reset()
		{
			_chattedWithPlayer.Clear();
		}

		public void OnPlayerJoining(int playerIndex)
		{
			foreach (string item in _chattedWithPlayer)
			{
				if (ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(item, out var value))
				{
					NetManager.Instance.SendToClient(NetBestiaryModule.SerializeChat(value), playerIndex);
				}
			}
		}
	}
}
