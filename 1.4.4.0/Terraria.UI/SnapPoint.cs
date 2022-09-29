using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	[DebuggerDisplay("Snap Point - {Name} {Id}")]
	public class SnapPoint
	{
		public string Name;

		private Vector2 _anchor;

		private Vector2 _offset;

		public int Id { get; private set; }

		public Vector2 Position { get; private set; }

		public SnapPoint(string name, int id, Vector2 anchor, Vector2 offset)
		{
			Name = name;
			Id = id;
			_anchor = anchor;
			_offset = offset;
		}

		public void Calculate(UIElement element)
		{
			CalculatedStyle dimensions = element.GetDimensions();
			Position = dimensions.Position() + _offset + _anchor * new Vector2(dimensions.Width, dimensions.Height);
		}

		public void ThisIsAHackThatChangesTheSnapPointsInfo(Vector2 anchor, Vector2 offset, int id)
		{
			_anchor = anchor;
			_offset = offset;
			Id = id;
		}
	}
}
