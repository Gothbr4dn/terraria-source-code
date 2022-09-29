using System.IO;

namespace Terraria.GameContent
{
	public interface IPersistentPerPlayerContent
	{
		void Save(Player player, BinaryWriter writer);

		void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn);

		void ApplyLoadedDataToOutOfPlayerFields(Player player);

		void ResetDataForNewPlayer(Player player);

		void Reset();
	}
}
