using System.IO;

namespace Terraria.GameContent
{
	public interface IPersistentPerWorldContent
	{
		void Save(BinaryWriter writer);

		void Load(BinaryReader reader, int gameVersionSaveWasMadeOn);

		void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn);

		void Reset();
	}
}
