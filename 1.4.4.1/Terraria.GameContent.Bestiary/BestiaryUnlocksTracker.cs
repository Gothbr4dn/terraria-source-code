using System.IO;

namespace Terraria.GameContent.Bestiary
{
	public class BestiaryUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		public NPCKillsTracker Kills = new NPCKillsTracker();

		public NPCWasNearPlayerTracker Sights = new NPCWasNearPlayerTracker();

		public NPCWasChatWithTracker Chats = new NPCWasChatWithTracker();

		public void Save(BinaryWriter writer)
		{
			Kills.Save(writer);
			Sights.Save(writer);
			Chats.Save(writer);
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			Kills.Load(reader, gameVersionSaveWasMadeOn);
			Sights.Load(reader, gameVersionSaveWasMadeOn);
			Chats.Load(reader, gameVersionSaveWasMadeOn);
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			Kills.ValidateWorld(reader, gameVersionSaveWasMadeOn);
			Sights.ValidateWorld(reader, gameVersionSaveWasMadeOn);
			Chats.ValidateWorld(reader, gameVersionSaveWasMadeOn);
		}

		public void Reset()
		{
			Kills.Reset();
			Sights.Reset();
			Chats.Reset();
		}

		public void OnPlayerJoining(int playerIndex)
		{
			Kills.OnPlayerJoining(playerIndex);
			Sights.OnPlayerJoining(playerIndex);
			Chats.OnPlayerJoining(playerIndex);
		}

		public void FillBasedOnVersionBefore210()
		{
		}
	}
}
