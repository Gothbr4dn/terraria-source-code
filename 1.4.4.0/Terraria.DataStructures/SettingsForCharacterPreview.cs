using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public class SettingsForCharacterPreview
	{
		public delegate void CustomAnimationCode(Projectile proj, bool walking);

		public struct SelectionBasedSettings
		{
			public int StartFrame;

			public int FrameCount;

			public int DelayPerFrame;

			public bool BounceLoop;

			public void ApplyTo(Projectile proj)
			{
				if (FrameCount != 0)
				{
					if (proj.frame < StartFrame)
					{
						proj.frame = StartFrame;
					}
					int num = proj.frame - StartFrame;
					int num2 = FrameCount * DelayPerFrame;
					int num3 = num2;
					if (BounceLoop)
					{
						num3 = num2 * 2 - DelayPerFrame * 2;
					}
					if (++proj.frameCounter >= num3)
					{
						proj.frameCounter = 0;
					}
					num = proj.frameCounter / DelayPerFrame;
					if (BounceLoop && num >= FrameCount)
					{
						num = FrameCount * 2 - num - 2;
					}
					proj.frame = StartFrame + num;
				}
			}
		}

		public Vector2 Offset;

		public SelectionBasedSettings Selected;

		public SelectionBasedSettings NotSelected;

		public int SpriteDirection = 1;

		public CustomAnimationCode CustomAnimation;

		public void ApplyTo(Projectile proj, bool walking)
		{
			proj.position += Offset;
			proj.spriteDirection = SpriteDirection;
			proj.direction = SpriteDirection;
			if (walking)
			{
				Selected.ApplyTo(proj);
			}
			else
			{
				NotSelected.ApplyTo(proj);
			}
			if (CustomAnimation != null)
			{
				CustomAnimation(proj, walking);
			}
		}

		public SettingsForCharacterPreview WhenSelected(int? startFrame = null, int? frameCount = null, int? delayPerFrame = null, bool? bounceLoop = null)
		{
			Modify(ref Selected, startFrame, frameCount, delayPerFrame, bounceLoop);
			return this;
		}

		public SettingsForCharacterPreview WhenNotSelected(int? startFrame = null, int? frameCount = null, int? delayPerFrame = null, bool? bounceLoop = null)
		{
			Modify(ref NotSelected, startFrame, frameCount, delayPerFrame, bounceLoop);
			return this;
		}

		private static void Modify(ref SelectionBasedSettings target, int? startFrame, int? frameCount, int? delayPerFrame, bool? bounceLoop)
		{
			if (frameCount.HasValue && frameCount.Value < 1)
			{
				frameCount = 1;
			}
			target.StartFrame = (startFrame.HasValue ? startFrame.Value : target.StartFrame);
			target.FrameCount = (frameCount.HasValue ? frameCount.Value : target.FrameCount);
			target.DelayPerFrame = (delayPerFrame.HasValue ? delayPerFrame.Value : target.DelayPerFrame);
			target.BounceLoop = (bounceLoop.HasValue ? bounceLoop.Value : target.BounceLoop);
		}

		public SettingsForCharacterPreview WithOffset(Vector2 offset)
		{
			Offset = offset;
			return this;
		}

		public SettingsForCharacterPreview WithOffset(float x, float y)
		{
			Offset = new Vector2(x, y);
			return this;
		}

		public SettingsForCharacterPreview WithSpriteDirection(int spriteDirection)
		{
			SpriteDirection = spriteDirection;
			return this;
		}

		public SettingsForCharacterPreview WithCode(CustomAnimationCode customAnimation)
		{
			CustomAnimation = customAnimation;
			return this;
		}
	}
}
