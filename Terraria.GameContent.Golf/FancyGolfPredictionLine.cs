using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Graphics;

namespace Terraria.GameContent.Golf
{
	public class FancyGolfPredictionLine
	{
		private class PredictionEntity : Entity
		{
		}

		private readonly List<Vector2> _positions;

		private readonly Entity _entity = new PredictionEntity();

		private readonly int _iterations;

		private readonly Color[] _colors = new Color[2]
		{
			Color.White,
			Color.Gray
		};

		private readonly BasicDebugDrawer _drawer = new BasicDebugDrawer(Main.instance.GraphicsDevice);

		private float _time;

		public FancyGolfPredictionLine(int iterations)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			_positions = new List<Vector2>(iterations * 2 + 1);
			_iterations = iterations;
		}

		public void Update(Entity golfBall, Vector2 impactVelocity, float roughLandResistance)
		{
			bool flag = Main.tileSolid[379];
			Main.tileSolid[379] = false;
			_positions.Clear();
			_time += 1f / 60f;
			_entity.position = golfBall.position;
			_entity.width = golfBall.width;
			_entity.height = golfBall.height;
			GolfHelper.HitGolfBall(_entity, impactVelocity, roughLandResistance);
			_positions.Add(_entity.position);
			float angularVelocity = 0f;
			for (int i = 0; i < _iterations; i++)
			{
				GolfHelper.StepGolfBall(_entity, ref angularVelocity);
				_positions.Add(_entity.position);
			}
			Main.tileSolid[379] = flag;
		}

		public void Draw(Camera camera, SpriteBatch spriteBatch, float chargeProgress)
		{
			_drawer.Begin(camera.GameViewMatrix.TransformationMatrix);
			_ = _positions.Count;
			Texture2D value = TextureAssets.Extra[33].get_Value();
			Vector2 vector = new Vector2(3.5f, 3.5f);
			Vector2 origin = value.Size() / 2f;
			Vector2 vector2 = vector - camera.UnscaledPosition;
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < _positions.Count - 1; i++)
			{
				GetSectionLength(i, out var length, out var _);
				if (length != 0f)
				{
					for (; num < num2 + length; num += 4f)
					{
						float num3 = (num - num2) / length + (float)i;
						Vector2 position = GetPosition((num - num2) / length + (float)i);
						Color color = GetColor2(num3);
						color *= MathHelper.Clamp(2f - 2f * num3 / (float)(_positions.Count - 1), 0f, 1f);
						spriteBatch.Draw(value, position + vector2, null, color, 0f, origin, GetScale(num), SpriteEffects.None, 0f);
					}
					num2 += length;
				}
			}
			_drawer.End();
		}

		private Color GetColor(float travelledLength)
		{
			float num = travelledLength % 200f / 200f;
			num *= (float)_colors.Length;
			num -= _time * MathF.PI * 1.5f;
			num %= (float)_colors.Length;
			if (num < 0f)
			{
				num += (float)_colors.Length;
			}
			int num2 = (int)Math.Floor(num);
			int num3 = num2 + 1;
			num2 = Utils.Clamp(num2 % _colors.Length, 0, _colors.Length - 1);
			num3 = Utils.Clamp(num3 % _colors.Length, 0, _colors.Length - 1);
			float amount = num - (float)num2;
			Color color = Color.Lerp(_colors[num2], _colors[num3], amount);
			color.A = 64;
			return color * 0.6f;
		}

		private Color GetColor2(float index)
		{
			float num = index * 0.5f - _time * MathF.PI * 1.5f;
			int num2 = (int)Math.Floor(num) % _colors.Length;
			if (num2 < 0)
			{
				num2 += _colors.Length;
			}
			int num3 = (num2 + 1) % _colors.Length;
			float amount = num - (float)Math.Floor(num);
			Color color = Color.Lerp(_colors[num2], _colors[num3], amount);
			color.A = 64;
			return color * 0.6f;
		}

		private float GetScale(float travelledLength)
		{
			return 0.2f + Utils.GetLerpValue(0.8f, 1f, (float)Math.Cos(travelledLength / 50f + _time * -MathF.PI) * 0.5f + 0.5f, clamped: true) * 0.15f;
		}

		private void GetSectionLength(int startIndex, out float length, out float rotation)
		{
			int num = startIndex + 1;
			if (num >= _positions.Count)
			{
				num = _positions.Count - 1;
			}
			length = Vector2.Distance(_positions[startIndex], _positions[num]);
			rotation = (_positions[num] - _positions[startIndex]).ToRotation();
		}

		private Vector2 GetPosition(float indexProgress)
		{
			int num = (int)Math.Floor(indexProgress);
			int num2 = num + 1;
			if (num2 >= _positions.Count)
			{
				num2 = _positions.Count - 1;
			}
			float amount = indexProgress - (float)num;
			return Vector2.Lerp(_positions[num], _positions[num2], amount);
		}
	}
}
