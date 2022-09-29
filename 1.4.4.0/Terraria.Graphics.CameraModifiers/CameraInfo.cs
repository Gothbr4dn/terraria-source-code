using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	public struct CameraInfo
	{
		public Vector2 CameraPosition;

		public Vector2 OriginalCameraCenter;

		public Vector2 OriginalCameraPosition;

		public CameraInfo(Vector2 position)
		{
			OriginalCameraPosition = position;
			OriginalCameraCenter = position + Main.ScreenSize.ToVector2() / 2f;
			CameraPosition = OriginalCameraPosition;
		}
	}
}
