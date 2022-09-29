using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.UI
{
	public class IssueReportsIndicator
	{
		private float _displayUpPercent;

		private bool _shouldBeShowing;

		private Asset<Texture2D> _buttonTexture;

		private Asset<Texture2D> _buttonOutlineTexture;

		public void AttemptLettingPlayerKnow()
		{
			Setup();
			_shouldBeShowing = true;
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode);
		}

		public void Hide()
		{
			_shouldBeShowing = false;
			_displayUpPercent = 0f;
		}

		private void OpenUI()
		{
			Setup();
			Main.OpenReportsMenu();
		}

		private void Setup()
		{
			_buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton", (AssetRequestMode)1);
			_buttonOutlineTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton_Outline", (AssetRequestMode)1);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			bool shouldBeShowing = _shouldBeShowing;
			_displayUpPercent = MathHelper.Clamp(_displayUpPercent + (float)shouldBeShowing.ToDirectionInt(), 0f, 1f);
			if (_displayUpPercent == 0f)
			{
				return;
			}
			Texture2D value = _buttonTexture.get_Value();
			Vector2 vector = Main.ScreenSize.ToVector2() + new Vector2(40f, -80f);
			Vector2 value2 = vector + new Vector2(-80f, 0f);
			Vector2 vector2 = Vector2.Lerp(vector, value2, _displayUpPercent);
			Rectangle rectangle = value.Frame();
			Vector2 origin = rectangle.Size() / 2f;
			bool flag = false;
			if (Utils.CenteredRectangle(vector2, rectangle.Size()).Contains(Main.MouseScreen.ToPoint()))
			{
				flag = true;
				string textValue = Language.GetTextValue("UI.IssueReporterHasThingsToShow");
				Main.instance.MouseText(textValue, 0, 0);
				if (Main.mouseLeft)
				{
					OpenUI();
					Hide();
					return;
				}
			}
			float scale = 1f;
			spriteBatch.Draw(value, vector2, rectangle, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);
			if (flag)
			{
				Texture2D value3 = _buttonOutlineTexture.get_Value();
				Rectangle rectangle2 = value3.Frame();
				spriteBatch.Draw(value3, vector2, rectangle2, Color.White, 0f, rectangle2.Size() / 2f, scale, SpriteEffects.None, 0f);
			}
		}
	}
}
