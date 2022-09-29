using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent
{
	public class ContentRejectionFromSize : IRejectionReason
	{
		private int _neededWidth;

		private int _neededHeight;

		private int _actualWidth;

		private int _actualHeight;

		public ContentRejectionFromSize(int neededWidth, int neededHeight, int actualWidth, int actualHeight)
		{
			_neededWidth = neededWidth;
			_neededHeight = neededHeight;
			_actualWidth = actualWidth;
			_actualHeight = actualHeight;
		}

		public string GetReason()
		{
			return Language.GetTextValueWith("AssetRejections.BadSize", new
			{
				NeededWidth = _neededWidth,
				NeededHeight = _neededHeight,
				ActualWidth = _actualWidth,
				ActualHeight = _actualHeight
			});
		}
	}
}
