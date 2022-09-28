using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using Terraria.Utilities;

namespace Terraria.GameContent.RGB
{
	public class EyeballShader : ChromaShader
	{
		private struct Ring
		{
			public readonly Vector4 Color;

			public readonly float Distance;

			public Ring(Vector4 color, float distance)
			{
				Color = color;
				Distance = distance;
			}
		}

		private enum EyelidState
		{
			Closed,
			Opening,
			Open,
			Closing
		}

		private static readonly Ring[] Rings = new Ring[5]
		{
			new Ring(Color.Black.ToVector4(), 0f),
			new Ring(Color.Black.ToVector4(), 0.4f),
			new Ring(new Color(17, 220, 237).ToVector4(), 0.5f),
			new Ring(new Color(17, 120, 237).ToVector4(), 0.6f),
			new Ring(Vector4.One, 0.65f)
		};

		private readonly Vector4 _eyelidColor = new Color(108, 110, 75).ToVector4();

		private float _eyelidProgress;

		private Vector2 _pupilOffset = Vector2.Zero;

		private Vector2 _targetOffset = Vector2.Zero;

		private readonly UnifiedRandom _random = new UnifiedRandom();

		private float _timeUntilPupilMove;

		private float _eyelidStateTime;

		private readonly bool _isSpawning;

		private EyelidState _eyelidState;

		public EyeballShader(bool isSpawning)
		{
			_isSpawning = isSpawning;
		}

		public override void Update(float elapsedTime)
		{
			UpdateEyelid(elapsedTime);
			bool num = _timeUntilPupilMove <= 0f;
			_pupilOffset = (_targetOffset + _pupilOffset) * 0.5f;
			_timeUntilPupilMove -= elapsedTime;
			if (num)
			{
				float num2 = (float)_random.NextDouble() * (MathF.PI * 2f);
				float num3;
				if (_isSpawning)
				{
					_timeUntilPupilMove = (float)_random.NextDouble() * 0.4f + 0.3f;
					num3 = (float)_random.NextDouble() * 0.7f;
				}
				else
				{
					_timeUntilPupilMove = (float)_random.NextDouble() * 0.4f + 0.6f;
					num3 = (float)_random.NextDouble() * 0.3f;
				}
				_targetOffset = new Vector2((float)Math.Cos(num2), (float)Math.Sin(num2)) * num3;
			}
		}

		private void UpdateEyelid(float elapsedTime)
		{
			float num = 0.5f;
			float num2 = 6f;
			if (_isSpawning)
			{
				if (NPC.MoonLordCountdown >= NPC.MaxMoonLordCountdown - 10)
				{
					_eyelidStateTime = 0f;
					_eyelidState = EyelidState.Closed;
				}
				num = (float)NPC.MoonLordCountdown / (float)NPC.MaxMoonLordCountdown * 10f + 0.5f;
				num2 = 2f;
			}
			_eyelidStateTime += elapsedTime;
			switch (_eyelidState)
			{
			case EyelidState.Closed:
				_eyelidProgress = 0f;
				if (_eyelidStateTime > num)
				{
					_eyelidStateTime = 0f;
					_eyelidState = EyelidState.Opening;
				}
				break;
			case EyelidState.Opening:
				_eyelidProgress = _eyelidStateTime / 0.4f;
				if (_eyelidStateTime > 0.4f)
				{
					_eyelidStateTime = 0f;
					_eyelidState = EyelidState.Open;
				}
				break;
			case EyelidState.Open:
				_eyelidProgress = 1f;
				if (_eyelidStateTime > num2)
				{
					_eyelidStateTime = 0f;
					_eyelidState = EyelidState.Closing;
				}
				break;
			case EyelidState.Closing:
				_eyelidProgress = 1f - _eyelidStateTime / 0.4f;
				if (_eyelidStateTime > 0.4f)
				{
					_eyelidStateTime = 0f;
					_eyelidState = EyelidState.Closed;
				}
				break;
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector2 vector = new Vector2(1.5f, 0.5f);
			Vector2 vector2 = vector + _pupilOffset;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector2 vector3 = canvasPositionOfIndex - vector;
				Vector4 vector4 = Vector4.One;
				float num = (vector2 - canvasPositionOfIndex).Length();
				for (int j = 1; j < Rings.Length; j++)
				{
					Ring ring = Rings[j];
					Ring ring2 = Rings[j - 1];
					if (num < ring.Distance)
					{
						vector4 = Vector4.Lerp(ring2.Color, ring.Color, (num - ring2.Distance) / (ring.Distance - ring2.Distance));
						break;
					}
				}
				float num2 = (float)Math.Sqrt(1f - 0.4f * vector3.Y * vector3.Y) * 5f;
				float num3 = Math.Abs(vector3.X) - num2 * (1.1f * _eyelidProgress - 0.1f);
				if (num3 > 0f)
				{
					vector4 = Vector4.Lerp(vector4, _eyelidColor, Math.Min(1f, num3 * 10f));
				}
				fragment.SetColor(i, vector4);
			}
		}
	}
}
