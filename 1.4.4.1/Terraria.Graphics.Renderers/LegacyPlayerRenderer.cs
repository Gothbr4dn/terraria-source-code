using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Graphics.Renderers
{
	public class LegacyPlayerRenderer : IPlayerRenderer
	{
		private readonly List<DrawData> _drawData = new List<DrawData>();

		private readonly List<int> _dust = new List<int>();

		private readonly List<int> _gore = new List<int>();

		public static SamplerState MountedSamplerState
		{
			get
			{
				if (!Main.drawToScreen)
				{
					return SamplerState.AnisotropicClamp;
				}
				return SamplerState.LinearClamp;
			}
		}

		public void DrawPlayers(Camera camera, IEnumerable<Player> players)
		{
			foreach (Player player in players)
			{
				DrawPlayerFull(camera, player);
			}
		}

		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			if (!drawPlayer.ShouldNotDraw)
			{
				_drawData.Clear();
				_dust.Clear();
				_gore.Clear();
				PlayerDrawHeadSet drawinfo = default(PlayerDrawHeadSet);
				drawinfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position.X, position.Y, alpha, scale);
				PlayerDrawHeadLayers.DrawPlayer_00_BackHelmet(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_01_FaceSkin(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_02_DrawArmorWithFullHair(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_03_HelmetHair(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_04_HatsWithFullHair(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_05_TallHats(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_06_NormalHats(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_07_JustHair(ref drawinfo);
				PlayerDrawHeadLayers.DrawPlayer_08_FaceAcc(ref drawinfo);
				CreateOutlines(alpha, scale, borderColor);
				PlayerDrawHeadLayers.DrawPlayer_RenderAllLayers(ref drawinfo);
			}
		}

		private void CreateOutlines(float alpha, float scale, Color borderColor)
		{
			if (!(borderColor != Color.Transparent))
			{
				return;
			}
			List<DrawData> collection = new List<DrawData>(_drawData);
			List<DrawData> list = new List<DrawData>(_drawData);
			float num = 2f * scale;
			Color color = borderColor;
			color *= alpha * alpha;
			Color black = Color.Black;
			black *= alpha * alpha;
			int colorOnlyShaderIndex = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
			for (int i = 0; i < list.Count; i++)
			{
				DrawData value = list[i];
				value.shader = colorOnlyShaderIndex;
				value.color = black;
				list[i] = value;
			}
			int num2 = 2;
			Vector2 vector;
			for (int j = -num2; j <= num2; j++)
			{
				for (int k = -num2; k <= num2; k++)
				{
					if (Math.Abs(j) + Math.Abs(k) == num2)
					{
						vector = new Vector2((float)j * num, (float)k * num);
						for (int l = 0; l < list.Count; l++)
						{
							DrawData item = list[l];
							item.position += vector;
							_drawData.Add(item);
						}
					}
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				DrawData value2 = list[m];
				value2.shader = colorOnlyShaderIndex;
				value2.color = color;
				list[m] = value2;
			}
			vector = Vector2.Zero;
			num2 = 1;
			for (int n = -num2; n <= num2; n++)
			{
				for (int num3 = -num2; num3 <= num2; num3++)
				{
					if (Math.Abs(n) + Math.Abs(num3) == num2)
					{
						vector = new Vector2((float)n * num, (float)num3 * num);
						for (int num4 = 0; num4 < list.Count; num4++)
						{
							DrawData item2 = list[num4];
							item2.position += vector;
							_drawData.Add(item2);
						}
					}
				}
			}
			_drawData.AddRange(collection);
		}

		public void DrawPlayer(Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow = 0f, float scale = 1f)
		{
			if (drawPlayer.ShouldNotDraw)
			{
				return;
			}
			PlayerDrawSet drawInfo = default(PlayerDrawSet);
			_drawData.Clear();
			_dust.Clear();
			_gore.Clear();
			drawInfo.BoringSetup(drawPlayer, _drawData, _dust, _gore, position, shadow, rotation, rotationOrigin);
			DrawPlayer_UseNormalLayers(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_TransformDrawData(ref drawInfo);
			if (scale != 1f)
			{
				PlayerDrawLayers.DrawPlayer_ScaleDrawData(ref drawInfo, scale);
			}
			PlayerDrawLayers.DrawPlayer_RenderAllLayers(ref drawInfo);
			if (!drawInfo.drawPlayer.mount.Active || !drawInfo.drawPlayer.UsingSuperCart)
			{
				return;
			}
			for (int i = 0; i < 1000; i++)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == drawInfo.drawPlayer.whoAmI && Main.projectile[i].type == 591)
				{
					Main.instance.DrawProj(i);
				}
			}
		}

		private static void DrawPlayer_MountTransformation(ref PlayerDrawSet drawInfo)
		{
			PlayerDrawLayers.DrawPlayer_02_MountBehindPlayer(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_23_MountFront(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_26_SolarShield(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountMinus(ref drawInfo);
		}

		private static void DrawPlayer_UseNormalLayers(ref PlayerDrawSet drawInfo)
		{
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_2_JimsCloak(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_02_MountBehindPlayer(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_03_Carpet(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_03_PortableStool(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_04_ElectrifiedDebuffBack(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_05_ForbiddenSetRing(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_05_2_SafemanSun(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_06_WebbedDebuffBack(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_07_LeinforsHairShampoo(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_08_Backpacks(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_08_1_Tails(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_09_Wings(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_BackHair(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_10_BackAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_01_3_BackHead(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_11_Balloons(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_12_Skin(ref drawInfo);
			if (drawInfo.drawPlayer.wearsRobe && drawInfo.drawPlayer.body != 166)
			{
				PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
			}
			else
			{
				PlayerDrawLayers.DrawPlayer_13_Leggings(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_14_Shoes(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_15_SkinLongCoat(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_16_ArmorLongCoat(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_17_Torso(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_18_OffhandAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_19_WaistAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_20_NeckAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_21_Head(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_21_1_Magiluminescence(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_22_FaceAcc(ref drawInfo);
			if (drawInfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
				PlayerDrawLayers.DrawPlayer_extra_TorsoPlus(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_23_MountFront(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_24_Pulley(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_JimsDroneRadio(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_32_FrontAcc_BackPart(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_25_Shield(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountPlus(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_26_SolarShield(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_extra_MountMinus(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_28_ArmOverItem(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_29_OnhandAcc(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_30_BladedGlove(ref drawInfo);
			if (!drawInfo.drawFrontAccInNeckAccLayer)
			{
				PlayerDrawLayers.DrawPlayer_32_FrontAcc_FrontPart(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_extra_TorsoMinus(ref drawInfo);
			if (drawInfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm)
			{
				PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
			}
			PlayerDrawLayers.DrawPlayer_31_ProjectileOverArm(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_33_FrozenOrWebbedDebuff(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_34_ElectrifiedDebuffFront(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_35_IceBarrier(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_36_CTG(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_37_BeetleBuff(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_38_EyebrellaCloud(ref drawInfo);
			PlayerDrawLayers.DrawPlayer_MakeIntoFirstFractalAfterImage(ref drawInfo);
		}

		private void DrawPlayerFull(Camera camera, Player drawPlayer)
		{
			SpriteBatch spriteBatch = camera.SpriteBatch;
			SamplerState samplerState = camera.Sampler;
			if (drawPlayer.mount.Active && drawPlayer.fullRotation != 0f)
			{
				samplerState = MountedSamplerState;
			}
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, samplerState, DepthStencilState.None, camera.Rasterizer, null, camera.GameViewMatrix.TransformationMatrix);
			if (Main.gamePaused)
			{
				drawPlayer.PlayerFrame();
			}
			if (drawPlayer.ghost)
			{
				for (int i = 0; i < 3; i++)
				{
					Vector2 vector = drawPlayer.shadowPos[i];
					vector = drawPlayer.position - drawPlayer.velocity * (2 + i * 2);
					DrawGhost(camera, drawPlayer, vector, 0.5f + 0.2f * (float)i);
				}
				DrawGhost(camera, drawPlayer, drawPlayer.position);
			}
			else
			{
				if (drawPlayer.inventory[drawPlayer.selectedItem].flame || drawPlayer.head == 137 || drawPlayer.wings == 22)
				{
					drawPlayer.itemFlameCount--;
					if (drawPlayer.itemFlameCount <= 0)
					{
						drawPlayer.itemFlameCount = 5;
						for (int j = 0; j < 7; j++)
						{
							drawPlayer.itemFlamePos[j].X = (float)Main.rand.Next(-10, 11) * 0.15f;
							drawPlayer.itemFlamePos[j].Y = (float)Main.rand.Next(-10, 1) * 0.35f;
						}
					}
				}
				if (drawPlayer.armorEffectDrawShadowEOCShield)
				{
					int num = drawPlayer.eocDash / 4;
					if (num > 3)
					{
						num = 3;
					}
					for (int k = 0; k < num; k++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[k], drawPlayer.shadowRotation[k], drawPlayer.shadowOrigin[k], 0.5f + 0.2f * (float)k);
					}
				}
				Vector2 position = default(Vector2);
				if (drawPlayer.invis)
				{
					drawPlayer.armorEffectDrawOutlines = false;
					drawPlayer.armorEffectDrawShadow = false;
					drawPlayer.armorEffectDrawShadowSubtle = false;
					position = drawPlayer.position;
					if (drawPlayer.aggro <= -750)
					{
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 1f);
					}
					else
					{
						drawPlayer.invis = false;
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
						drawPlayer.invis = true;
					}
				}
				if (drawPlayer.armorEffectDrawOutlines)
				{
					_ = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.075f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num2 = drawPlayer.ghostFade * 5f;
					for (int l = 0; l < 4; l++)
					{
						float num3;
						float num4;
						switch (l)
						{
						default:
							num3 = num2;
							num4 = 0f;
							break;
						case 1:
							num3 = 0f - num2;
							num4 = 0f;
							break;
						case 2:
							num3 = 0f;
							num4 = num2;
							break;
						case 3:
							num3 = 0f;
							num4 = 0f - num2;
							break;
						}
						position = new Vector2(drawPlayer.position.X + num3, drawPlayer.position.Y + drawPlayer.gfxOffY + num4);
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
					}
				}
				if (drawPlayer.armorEffectDrawOutlinesForbidden)
				{
					_ = drawPlayer.position;
					if (!Main.gamePaused)
					{
						drawPlayer.ghostFade += drawPlayer.ghostDir * 0.025f;
					}
					if ((double)drawPlayer.ghostFade < 0.1)
					{
						drawPlayer.ghostDir = 1f;
						drawPlayer.ghostFade = 0.1f;
					}
					else if ((double)drawPlayer.ghostFade > 0.9)
					{
						drawPlayer.ghostDir = -1f;
						drawPlayer.ghostFade = 0.9f;
					}
					float num5 = drawPlayer.ghostFade * 5f;
					for (int m = 0; m < 4; m++)
					{
						float num6;
						float num7;
						switch (m)
						{
						default:
							num6 = num5;
							num7 = 0f;
							break;
						case 1:
							num6 = 0f - num5;
							num7 = 0f;
							break;
						case 2:
							num6 = 0f;
							num7 = num5;
							break;
						case 3:
							num6 = 0f;
							num7 = 0f - num5;
							break;
						}
						position = new Vector2(drawPlayer.position.X + num6, drawPlayer.position.Y + drawPlayer.gfxOffY + num7);
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, drawPlayer.ghostFade);
					}
				}
				if (drawPlayer.armorEffectDrawShadowBasilisk)
				{
					int num8 = (int)(drawPlayer.basiliskCharge * 3f);
					for (int n = 0; n < num8; n++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[n], drawPlayer.shadowRotation[n], drawPlayer.shadowOrigin[n], 0.5f + 0.2f * (float)n);
					}
				}
				else if (drawPlayer.armorEffectDrawShadow)
				{
					for (int num9 = 0; num9 < 3; num9++)
					{
						DrawPlayer(camera, drawPlayer, drawPlayer.shadowPos[num9], drawPlayer.shadowRotation[num9], drawPlayer.shadowOrigin[num9], 0.5f + 0.2f * (float)num9);
					}
				}
				if (drawPlayer.armorEffectDrawShadowLokis)
				{
					for (int num10 = 0; num10 < 3; num10++)
					{
						DrawPlayer(camera, drawPlayer, Vector2.Lerp(drawPlayer.shadowPos[num10], drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY), 0.5f), drawPlayer.shadowRotation[num10], drawPlayer.shadowOrigin[num10], MathHelper.Lerp(1f, 0.5f + 0.2f * (float)num10, 0.5f));
					}
				}
				if (drawPlayer.armorEffectDrawShadowSubtle)
				{
					for (int num11 = 0; num11 < 4; num11++)
					{
						position.X = drawPlayer.position.X + (float)Main.rand.Next(-20, 21) * 0.1f;
						position.Y = drawPlayer.position.Y + (float)Main.rand.Next(-20, 21) * 0.1f + drawPlayer.gfxOffY;
						DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.9f);
					}
				}
				if (drawPlayer.shadowDodge)
				{
					drawPlayer.shadowDodgeCount += 1f;
					if (drawPlayer.shadowDodgeCount > 30f)
					{
						drawPlayer.shadowDodgeCount = 30f;
					}
				}
				else
				{
					drawPlayer.shadowDodgeCount -= 1f;
					if (drawPlayer.shadowDodgeCount < 0f)
					{
						drawPlayer.shadowDodgeCount = 0f;
					}
				}
				if (drawPlayer.shadowDodgeCount > 0f)
				{
					_ = drawPlayer.position;
					position.X = drawPlayer.position.X + drawPlayer.shadowDodgeCount;
					position.Y = drawPlayer.position.Y + drawPlayer.gfxOffY;
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
					position.X = drawPlayer.position.X - drawPlayer.shadowDodgeCount;
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, 0.5f + (float)Main.rand.Next(-10, 11) * 0.005f);
				}
				if (drawPlayer.brainOfConfusionDodgeAnimationCounter > 0)
				{
					Vector2 vector2 = drawPlayer.position + new Vector2(0f, drawPlayer.gfxOffY);
					float lerpValue = Utils.GetLerpValue(300f, 270f, drawPlayer.brainOfConfusionDodgeAnimationCounter);
					float y = MathHelper.Lerp(2f, 120f, lerpValue);
					if (lerpValue >= 0f && lerpValue <= 1f)
					{
						for (float num12 = 0f; num12 < MathF.PI * 2f; num12 += MathF.PI / 3f)
						{
							position = vector2 + new Vector2(0f, y).RotatedBy(MathF.PI * 2f * lerpValue * 0.5f + num12);
							DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin, lerpValue);
						}
					}
				}
				position = drawPlayer.position;
				position.Y += drawPlayer.gfxOffY;
				if (drawPlayer.stoned)
				{
					DrawPlayerStoned(camera, drawPlayer, position);
				}
				else if (!drawPlayer.invis)
				{
					DrawPlayer(camera, drawPlayer, position, drawPlayer.fullRotation, drawPlayer.fullRotationOrigin);
				}
			}
			spriteBatch.End();
		}

		private void DrawPlayerStoned(Camera camera, Player drawPlayer, Vector2 position)
		{
			if (!drawPlayer.dead)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				spriteEffects = ((drawPlayer.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				camera.SpriteBatch.Draw(TextureAssets.Extra[37].get_Value(), new Vector2((int)(position.X - camera.UnscaledPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2)), (int)(position.Y - camera.UnscaledPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 8f)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), null, Lighting.GetColor((int)((double)position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), 0f, new Vector2(TextureAssets.Extra[37].Width() / 2, TextureAssets.Extra[37].Height() / 2), 1f, spriteEffects, 0f);
			}
		}

		private void DrawGhost(Camera camera, Player drawPlayer, Vector2 position, float shadow = 0f)
		{
			byte mouseTextColor = Main.mouseTextColor;
			SpriteEffects effects = ((drawPlayer.direction != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
			Color immuneAlpha = drawPlayer.GetImmuneAlpha(Lighting.GetColor((int)((double)drawPlayer.position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)drawPlayer.position.Y + (double)drawPlayer.height * 0.5) / 16, new Color((int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100, (int)mouseTextColor / 2 + 100)), shadow);
			immuneAlpha.A = (byte)((float)(int)immuneAlpha.A * (1f - Math.Max(0.5f, shadow - 0.5f)));
			Rectangle value = new Rectangle(0, TextureAssets.Ghost.Height() / 4 * drawPlayer.ghostFrame, TextureAssets.Ghost.Width(), TextureAssets.Ghost.Height() / 4);
			Vector2 origin = new Vector2((float)value.Width * 0.5f, (float)value.Height * 0.5f);
			camera.SpriteBatch.Draw(TextureAssets.Ghost.get_Value(), new Vector2((int)(position.X - camera.UnscaledPosition.X + (float)(value.Width / 2)), (int)(position.Y - camera.UnscaledPosition.Y + (float)(value.Height / 2))), value, immuneAlpha, 0f, origin, 1f, effects, 0f);
		}
	}
}
