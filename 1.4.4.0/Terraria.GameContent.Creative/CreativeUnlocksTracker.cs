using System.IO;

namespace Terraria.GameContent.Creative
{
	public class CreativeUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		public ItemsSacrificedUnlocksTracker ItemSacrifices = new ItemsSacrificedUnlocksTracker();

		public void Save(BinaryWriter writer)
		{
			ItemSacrifices.Save(writer);
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			ItemSacrifices.Load(reader, gameVersionSaveWasMadeOn);
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			ValidateWorld(reader, gameVersionSaveWasMadeOn);
		}

		public void Reset()
		{
			ItemSacrifices.Reset();
		}

		public void OnPlayerJoining(int playerIndex)
		{
			ItemSacrifices.OnPlayerJoining(playerIndex);
		}
	}
}
