namespace Terraria.UI
{
	public class LegacyGameInterfaceLayer : GameInterfaceLayer
	{
		private GameInterfaceDrawMethod _drawMethod;

		public LegacyGameInterfaceLayer(string name, GameInterfaceDrawMethod drawMethod, InterfaceScaleType scaleType = InterfaceScaleType.Game)
			: base(name, scaleType)
		{
			_drawMethod = drawMethod;
		}

		protected override bool DrawSelf()
		{
			return _drawMethod();
		}
	}
}
