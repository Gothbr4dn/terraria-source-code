using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent
{
	public struct VoidLensHelper
	{
		private readonly Vector2 _position;

		private readonly float _opacity;

		private readonly int _frameNumber;

		public VoidLensHelper(Projectile proj)
		{
			_position = proj.Center;
			_opacity = proj.Opacity;
			_frameNumber = proj.frame;
		}

		public VoidLensHelper(Vector2 worldPosition, float opacity)
		{
			worldPosition.Y -= 2f;
			_position = worldPosition;
			_opacity = opacity;
			_frameNumber = (int)(((float)Main.tileFrameCounter[491] + _position.X + _position.Y) % 40f) / 5;
		}

		public void Update()
		{
			Lighting.AddLight(_position, 0.4f, 0.2f, 0.9f);
			SpawnVoidLensDust();
		}

		public void SpawnVoidLensDust()
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
					Dust dust2 = Dust.NewDustDirect(_position - vector2 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
					dust2.noGravity = true;
					dust2.noLightEmittence = true;
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

		public void DrawToDrawData(List<DrawData> drawDataList, int selectionMode)
		{
			Main.instance.LoadProjectile(734);
			Asset<Texture2D> val = TextureAssets.Projectile[734];
			Rectangle rectangle = val.Frame(1, 8, 0, _frameNumber);
			Color color = Lighting.GetColor(_position.ToTileCoordinates());
			color = Color.Lerp(color, Color.White, 0.5f);
			color *= _opacity;
			DrawData drawData = new DrawData(val.get_Value(), _position - Main.screenPosition, rectangle, color, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None);
			drawDataList.Add(drawData);
			for (float num = 0f; num < 1f; num += 0.34f)
			{
				DrawData item = drawData;
				item.color = new Color(127, 50, 127, 0) * _opacity;
				item.scale *= 1.1f;
				float x = (Main.GlobalTimeWrappedHourly / 5f * (MathF.PI * 2f)).ToRotationVector2().X;
				item.color *= x * 0.1f + 0.3f;
				item.position += ((Main.GlobalTimeWrappedHourly / 5f + num) * (MathF.PI * 2f)).ToRotationVector2() * (x * 1f + 2f);
				drawDataList.Add(item);
			}
			if (selectionMode != 0)
			{
				int num2 = (color.R + color.G + color.B) / 3;
				if (num2 > 10)
				{
					Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionMode == 2, num2);
					drawData = new DrawData(TextureAssets.Extra[93].get_Value(), _position - Main.screenPosition, rectangle, selectionGlowColor, 0f, rectangle.Size() / 2f, 1f, SpriteEffects.None);
					drawDataList.Add(drawData);
				}
			}
		}
	}
}
