using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.GameContent.Animations
{
	public class StardewValleyAnimation
	{
		private List<IAnimationSegment> _segments = new List<IAnimationSegment>();

		public StardewValleyAnimation()
		{
			ComposeAnimation();
		}

		private void ComposeAnimation()
		{
			Asset<Texture2D> val = TextureAssets.Extra[247];
			Rectangle rectangle = val.Frame();
			DrawData data = new DrawData(val.get_Value(), Vector2.Zero, rectangle, Color.White, 0f, rectangle.Size() * new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None);
			int targetTime = 128;
			int num = 60;
			int num2 = 360;
			int duration = 60;
			int num3 = 4;
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(val, targetTime, data, Vector2.Zero).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect(GetMatrixForAnimation, "StardewValleyFade", 8, num3).WithPanX(new Segments.Panning
			{
				Delay = 128f,
				Duration = num2 - 120 + num - 60,
				AmountOverTime = 0.55f,
				StartAmount = -0.4f
			}).WithPanY(new Segments.Panning
			{
				StartAmount = 0f
			})).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num))
				.Then(new Actions.Sprites.Wait(num2))
				.Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
			_segments.Add(item);
			Asset<Texture2D> val2 = TextureAssets.Extra[249];
			Rectangle rectangle2 = val2.Frame(1, 8);
			DrawData data2 = new DrawData(val2.get_Value(), Vector2.Zero, rectangle2, Color.White, 0f, rectangle2.Size() * new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None);
			Segments.AnimationSegmentWithActions<Segments.LooseSprite> item2 = new Segments.SpriteSegment(val2, targetTime, data2, Vector2.Zero).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num)).With(new Actions.Sprites.SetFrameSequence(100, new Point[8]
			{
				new Point(0, 0),
				new Point(0, 1),
				new Point(0, 2),
				new Point(0, 3),
				new Point(0, 4),
				new Point(0, 5),
				new Point(0, 6),
				new Point(0, 7)
			}, num3, 0, 0))
				.Then(new Actions.Sprites.Wait(num2))
				.Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
			_segments.Add(item2);
		}

		private Matrix GetMatrixForAnimation()
		{
			return Main.Transform;
		}

		public void Draw(SpriteBatch spriteBatch, int timeInAnimation, Vector2 positionInScreen)
		{
			GameAnimationSegment gameAnimationSegment = default(GameAnimationSegment);
			gameAnimationSegment.SpriteBatch = spriteBatch;
			gameAnimationSegment.AnchorPositionOnScreen = positionInScreen;
			gameAnimationSegment.TimeInAnimation = timeInAnimation;
			gameAnimationSegment.DisplayOpacity = 1f;
			GameAnimationSegment info = gameAnimationSegment;
			for (int i = 0; i < _segments.Count; i++)
			{
				_segments[i].Draw(ref info);
			}
		}
	}
}
