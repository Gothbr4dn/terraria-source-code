using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent
{
	public struct PotionOfReturnGateHelper
	{
		public enum GateType
		{
			EntryPoint,
			ExitPoint
		}

		private readonly Vector2 _position;

		private readonly float _opacity;

		private readonly int _frameNumber;

		private readonly GateType _gateType;

		public PotionOfReturnGateHelper(GateType gateType, Vector2 worldPosition, float opacity)
		{
			_gateType = gateType;
			worldPosition.Y -= 2f;
			_position = worldPosition;
			_opacity = opacity;
			int num = (int)(((float)Main.tileFrameCounter[491] + _position.X + _position.Y) % 40f) / 5;
			if (gateType == GateType.ExitPoint)
			{
				num = 7 - num;
			}
			_frameNumber = num;
		}

		public void Update()
		{
			Lighting.AddLight(_position, 0.4f, 0.2f, 0.9f);
			SpawnReturnPortalDust();
		}

		public void SpawnReturnPortalDust()
		{
			if (_gateType == GateType.EntryPoint)
			{
				if (Main.rand.Next(3) == 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						Vector2 vector = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
						vector *= new Vector2(0.5f, 1f);
						Dust dust = Dust.NewDustDirect(_position - vector * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
						dust.noGravity = true;
						dust.noLightEmittence = true;
						dust.position = _position - vector.SafeNormalize(Vector2.Zero) * Main.rand.Next(10, 21);
						dust.velocity = vector.RotatedBy(1.5707963705062866) * 2f;
						dust.scale = 0.5f + Main.rand.NextFloat();
						dust.fadeIn = 0.5f;
						dust.customData = this;
						dust.position += dust.velocity * 10f;
						dust.velocity *= -1f;
					}
					else
					{
						Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
						vector2 *= new Vector2(0.5f, 1f);
						Dust dust2 = Dust.NewDustDirect(_position - vector2 * 30f, 0, 0, 240);
						dust2.noGravity = true;
						dust2.noLight = true;
						dust2.position = _position - vector2.SafeNormalize(Vector2.Zero) * Main.rand.Next(5, 10);
						dust2.velocity = vector2.RotatedBy(-1.5707963705062866) * 3f;
						dust2.scale = 0.5f + Main.rand.NextFloat();
						dust2.fadeIn = 0.5f;
						dust2.customData = this;
						dust2.position += dust2.velocity * 10f;
						dust2.velocity *= -1f;
					}
				}
			}
			else if (Main.rand.Next(3) == 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector3 *= new Vector2(0.5f, 1f);
					Dust dust3 = Dust.NewDustDirect(_position - vector3 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
					dust3.noGravity = true;
					dust3.noLightEmittence = true;
					dust3.position = _position;
					dust3.velocity = vector3.RotatedBy(-0.7853981852531433) * 2f;
					dust3.scale = 0.5f + Main.rand.NextFloat();
					dust3.fadeIn = 0.5f;
					dust3.customData = this;
					dust3.position += vector3 * new Vector2(20f);
				}
				else
				{
					Vector2 vector4 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector4 *= new Vector2(0.5f, 1f);
					Dust dust4 = Dust.NewDustDirect(_position - vector4 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
					dust4.noGravity = true;
					dust4.noLightEmittence = true;
					dust4.position = _position;
					dust4.velocity = vector4.RotatedBy(-0.7853981852531433) * 2f;
					dust4.scale = 0.5f + Main.rand.NextFloat();
					dust4.fadeIn = 0.5f;
					dust4.customData = this;
					dust4.position += vector4 * new Vector2(20f);
				}
			}
		}

		public void DrawToDrawData(List<DrawData> drawDataList, int selectionMode)
		{
			short num = (short)((_gateType == GateType.EntryPoint) ? 183 : 184);
			Asset<Texture2D> val = TextureAssets.Extra[num];
			Rectangle rectangle = val.Frame(1, 8, 0, _frameNumber);
			Color color = Lighting.GetColor(_position.ToTileCoordinates());
			color = Color.Lerp(color, Color.White, 0.5f);
			color *= _opacity;
			DrawData drawData = new DrawData(val.get_Value(), _position - Main.screenPosition, rectangle, color, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None);
			drawDataList.Add(drawData);
			for (float num2 = 0f; num2 < 1f; num2 += 0.34f)
			{
				DrawData item = drawData;
				item.color = new Color(127, 50, 127, 0) * _opacity;
				item.scale *= 1.1f;
				float x = (Main.GlobalTimeWrappedHourly / 5f * (MathF.PI * 2f)).ToRotationVector2().X;
				item.color *= x * 0.1f + 0.3f;
				item.position += ((Main.GlobalTimeWrappedHourly / 5f + num2) * (MathF.PI * 2f)).ToRotationVector2() * (x * 1f + 2f);
				drawDataList.Add(item);
			}
			if (selectionMode != 0)
			{
				int num3 = (color.R + color.G + color.B) / 3;
				if (num3 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionMode == 2, num3);
					Texture2D value = TextureAssets.Extra[242].get_Value();
					Rectangle value2 = value.Frame(1, 8, 0, _frameNumber);
					drawData = new DrawData(value, _position - Main.screenPosition, value2, selectionGlowColor, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None);
					drawDataList.Add(drawData);
				}
			}
		}
	}
}
