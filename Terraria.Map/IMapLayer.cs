namespace Terraria.Map
{
	public interface IMapLayer
	{
		void Draw(ref MapOverlayDrawContext context, ref string text);
	}
}
