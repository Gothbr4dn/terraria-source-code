using System.IO;

namespace Terraria.DataStructures
{
	public struct BinaryWriterHelper
	{
		private long _placeInWriter;

		public void ReservePointToFillLengthLaterByFilling6Bytes(BinaryWriter writer)
		{
			_placeInWriter = writer.BaseStream.Position;
			writer.Write(0u);
			writer.Write((ushort)0);
		}

		public void FillReservedPoint(BinaryWriter writer, ushort dataId)
		{
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position = _placeInWriter;
			long num = position - _placeInWriter - 4;
			writer.Write((int)num);
			writer.Write(dataId);
			writer.BaseStream.Position = position;
		}

		public void FillOnlyIfThereIsLengthOrRevertToSavedPosition(BinaryWriter writer, ushort dataId, out bool wroteSomething)
		{
			wroteSomething = false;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position = _placeInWriter;
			long num = position - _placeInWriter - 4;
			if (num != 0L)
			{
				writer.Write((int)num);
				writer.Write(dataId);
				writer.BaseStream.Position = position;
				wroteSomething = true;
			}
		}
	}
}
