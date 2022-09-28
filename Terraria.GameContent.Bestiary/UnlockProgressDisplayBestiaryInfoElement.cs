using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class UnlockProgressDisplayBestiaryInfoElement : IBestiaryInfoElement
	{
		private BestiaryUnlockProgressReport _progressReport;

		private UIElement _text1;

		private UIElement _text2;

		public UnlockProgressDisplayBestiaryInfoElement(BestiaryUnlockProgressReport progressReport)
		{
			_progressReport = progressReport;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			UIElement uIElement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode)1), null, 12, 7)
			{
				Width = new StyleDimension(-11f, 1f),
				Height = new StyleDimension(109f, 0f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent,
				Left = new StyleDimension(3f, 0f)
			};
			uIElement.PaddingLeft = 4f;
			uIElement.PaddingRight = 4f;
			string arg = Utils.PrettifyPercentDisplay((float)info.UnlockState / 4f, "P2");
			string text = $"{arg} Entry Collected";
			string arg2 = Utils.PrettifyPercentDisplay(_progressReport.CompletionPercent, "P2");
			string text2 = $"{arg2} Bestiary Collected";
			int num = 8;
			UIText uIText = new UIText(text, 0.8f)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				IsWrapped = true,
				PaddingTop = -num,
				PaddingBottom = -num
			};
			UIText uIText2 = new UIText(text2, 0.8f)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				IsWrapped = true,
				PaddingTop = -num,
				PaddingBottom = -num
			};
			_text1 = uIText;
			_text2 = uIText2;
			AddDynamicResize(uIElement, uIText);
			uIElement.Append(uIText);
			uIElement.Append(uIText2);
			return uIElement;
		}

		private void AddDynamicResize(UIElement container, UIText text)
		{
			text.OnInternalTextChange += delegate
			{
				container.Height = new StyleDimension(_text1.MinHeight.Pixels + 4f + _text2.MinHeight.Pixels, 0f);
				_text2.Top = new StyleDimension(_text1.MinHeight.Pixels + 4f, 0f);
			};
		}
	}
}
