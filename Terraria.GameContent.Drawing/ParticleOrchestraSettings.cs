using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing
{
	public struct ParticleOrchestraSettings
	{
		public Vector2 PositionInWorld;

		public Vector2 MovementVector;

		public int UniqueInfoPiece;

		public byte IndexOfPlayerWhoInvokedThis;

		public const int SerializationSize = 21;

		public void Serialize(BinaryWriter writer)
		{
			writer.WriteVector2(PositionInWorld);
			writer.WriteVector2(MovementVector);
			writer.Write(UniqueInfoPiece);
			writer.Write(IndexOfPlayerWhoInvokedThis);
		}

		public void DeserializeFrom(BinaryReader reader)
		{
			PositionInWorld = reader.ReadVector2();
			MovementVector = reader.ReadVector2();
			UniqueInfoPiece = reader.ReadInt32();
			IndexOfPlayerWhoInvokedThis = reader.ReadByte();
		}
	}
}
