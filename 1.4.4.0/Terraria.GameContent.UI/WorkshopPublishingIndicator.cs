using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.GameContent.UI
{
	public class WorkshopPublishingIndicator
	{
		private float _displayUpPercent;

		private int _frameCounter;

		private bool _shouldPlayEndingSound;

		private Asset<Texture2D> _indicatorTexture;

		private int _timesSoundWasPlayed;

		public void Hide()
		{
			_displayUpPercent = 0f;
			_frameCounter = 0;
			_timesSoundWasPlayed = 0;
			_shouldPlayEndingSound = false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			WorkshopSocialModule workshop = SocialAPI.Workshop;
			if (workshop == null)
			{
				return;
			}
			AWorkshopProgressReporter progressReporter = workshop.ProgressReporter;
			bool hasOngoingTasks = progressReporter.HasOngoingTasks;
			bool num = _displayUpPercent == 1f;
			_displayUpPercent = MathHelper.Clamp(_displayUpPercent + (float)hasOngoingTasks.ToDirectionInt() / 60f, 0f, 1f);
			bool flag = _displayUpPercent == 1f;
			if (num && !flag)
			{
				_shouldPlayEndingSound = true;
			}
			if (_displayUpPercent == 0f)
			{
				return;
			}
			if (_indicatorTexture == null)
			{
				_indicatorTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/InProgress", (AssetRequestMode)1);
			}
			Texture2D value = _indicatorTexture.get_Value();
			int num2 = 6;
			_frameCounter++;
			int num3 = 5;
			int num4 = _frameCounter / num3 % num2;
			Vector2 vector = Main.ScreenSize.ToVector2() + new Vector2(-40f, 40f);
			Vector2 value2 = vector + new Vector2(0f, -80f);
			Vector2 position = Vector2.Lerp(vector, value2, _displayUpPercent);
			Rectangle rectangle = value.Frame(1, 6, 0, num4);
			Vector2 origin = rectangle.Size() / 2f;
			spriteBatch.Draw(value, position, rectangle, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);
			if (progressReporter.TryGetProgress(out var progress) && !float.IsNaN(progress))
			{
				string text = progress.ToString("P");
				DynamicSpriteFont value3 = FontAssets.ItemStack.get_Value();
				int num5 = 1;
				Vector2 origin2 = value3.MeasureString(text) * num5 * new Vector2(0.5f, 1f);
				Utils.DrawBorderStringFourWay(spriteBatch, value3, text, position.X, position.Y - 10f, Color.White, Color.Black, origin2, num5);
			}
			if (num4 == 3 && _frameCounter % num3 == 0)
			{
				if (_shouldPlayEndingSound)
				{
					_shouldPlayEndingSound = false;
					_timesSoundWasPlayed = 0;
					SoundEngine.PlaySound(64);
				}
				if (hasOngoingTasks)
				{
					float volumeScale = Utils.Remap(_timesSoundWasPlayed, 0f, 10f, 1f, 0f);
					SoundEngine.PlaySound(21, -1, -1, 1, volumeScale);
					_timesSoundWasPlayed++;
				}
			}
		}
	}
}
