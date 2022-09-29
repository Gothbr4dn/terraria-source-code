namespace Terraria.Graphics.CameraModifiers
{
	public interface ICameraModifier
	{
		string UniqueIdentity { get; }

		bool Finished { get; }

		void Update(ref CameraInfo cameraPosition);
	}
}
