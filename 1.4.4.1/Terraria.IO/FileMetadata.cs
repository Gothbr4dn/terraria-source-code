using System;
using System.IO;

namespace Terraria.IO
{
	public class FileMetadata
	{
		public const ulong MAGIC_NUMBER = 27981915666277746uL;

		public const int SIZE = 20;

		public FileType Type;

		public uint Revision;

		public bool IsFavorite;

		private FileMetadata()
		{
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(0x6369676F6C6572uL | ((ulong)Type << 56));
			writer.Write(Revision);
			writer.Write((ulong)(int)(((uint)IsFavorite.ToInt() & 1u) | 0u));
		}

		public void IncrementAndWrite(BinaryWriter writer)
		{
			Revision++;
			Write(writer);
		}

		public static FileMetadata FromCurrentSettings(FileType type)
		{
			return new FileMetadata
			{
				Type = type,
				Revision = 0u,
				IsFavorite = false
			};
		}

		public static FileMetadata Read(BinaryReader reader, FileType expectedType)
		{
			FileMetadata fileMetadata = new FileMetadata();
			fileMetadata.Read(reader);
			if (fileMetadata.Type != expectedType)
			{
				throw new FormatException("Expected type \"" + Enum.GetName(typeof(FileType), expectedType) + "\" but found \"" + Enum.GetName(typeof(FileType), fileMetadata.Type) + "\".");
			}
			return fileMetadata;
		}

		private void Read(BinaryReader reader)
		{
			ulong num = reader.ReadUInt64();
			if ((num & 0xFFFFFFFFFFFFFFL) != 27981915666277746L)
			{
				throw new FormatException("Expected Re-Logic file format.");
			}
			byte b = (byte)((num >> 56) & 0xFF);
			FileType fileType = FileType.None;
			FileType[] array = (FileType[])Enum.GetValues(typeof(FileType));
			for (int i = 0; i < array.Length; i++)
			{
				if ((uint)array[i] == b)
				{
					fileType = array[i];
					break;
				}
			}
			if (fileType == FileType.None)
			{
				throw new FormatException("Found invalid file type.");
			}
			Type = fileType;
			Revision = reader.ReadUInt32();
			ulong num2 = reader.ReadUInt64();
			IsFavorite = (num2 & 1) == 1;
		}
	}
}
