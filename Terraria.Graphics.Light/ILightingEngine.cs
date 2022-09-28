using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Light
{
	public interface ILightingEngine
	{
		void Rebuild();

		void AddLight(int x, int y, Vector3 color);

		void ProcessArea(Rectangle area);

		Vector3 GetColor(int x, int y);

		void Clear();
	}
}
