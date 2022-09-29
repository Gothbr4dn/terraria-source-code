using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	public struct SpriteFrame
	{
		public int PaddingX;

		public int PaddingY;

		private byte _currentColumn;

		private byte _currentRow;

		public readonly byte ColumnCount;

		public readonly byte RowCount;

		public byte CurrentColumn
		{
			get
			{
				return _currentColumn;
			}
			set
			{
				_currentColumn = value;
			}
		}

		public byte CurrentRow
		{
			get
			{
				return _currentRow;
			}
			set
			{
				_currentRow = value;
			}
		}

		public SpriteFrame(byte columns, byte rows)
		{
			PaddingX = 2;
			PaddingY = 2;
			_currentColumn = 0;
			_currentRow = 0;
			ColumnCount = columns;
			RowCount = rows;
		}

		public SpriteFrame(byte columns, byte rows, byte currentColumn, byte currentRow)
		{
			PaddingX = 2;
			PaddingY = 2;
			_currentColumn = currentColumn;
			_currentRow = currentRow;
			ColumnCount = columns;
			RowCount = rows;
		}

		public SpriteFrame With(byte columnToUse, byte rowToUse)
		{
			SpriteFrame result = this;
			result.CurrentColumn = columnToUse;
			result.CurrentRow = rowToUse;
			return result;
		}

		public Rectangle GetSourceRectangle(Texture2D texture)
		{
			int num = texture.Width / (int)ColumnCount;
			int num2 = texture.Height / (int)RowCount;
			return new Rectangle(CurrentColumn * num, CurrentRow * num2, num - ((ColumnCount != 1) ? PaddingX : 0), num2 - ((RowCount != 1) ? PaddingY : 0));
		}
	}
}
