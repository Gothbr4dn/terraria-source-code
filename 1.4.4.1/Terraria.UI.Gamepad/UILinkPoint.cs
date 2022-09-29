using System;
using Microsoft.Xna.Framework;

namespace Terraria.UI.Gamepad
{
	public class UILinkPoint
	{
		public int ID;

		public bool Enabled;

		public Vector2 Position;

		public int Left;

		public int Right;

		public int Up;

		public int Down;

		public int Page { get; private set; }

		public event Func<string> OnSpecialInteracts;

		public UILinkPoint(int id, bool enabled, int left, int right, int up, int down)
		{
			ID = id;
			Enabled = enabled;
			Left = left;
			Right = right;
			Up = up;
			Down = down;
		}

		public void SetPage(int page)
		{
			Page = page;
		}

		public void Unlink()
		{
			Left = -3;
			Right = -4;
			Up = -1;
			Down = -2;
		}

		public string SpecialInteractions()
		{
			if (this.OnSpecialInteracts != null)
			{
				return this.OnSpecialInteracts();
			}
			return string.Empty;
		}
	}
}
