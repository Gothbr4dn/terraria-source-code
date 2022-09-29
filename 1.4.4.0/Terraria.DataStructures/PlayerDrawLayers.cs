using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.DataStructures
{
	public static class PlayerDrawLayers
	{
		public static void DrawPlayer_extra_TorsoPlus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y += drawinfo.torsoOffset;
			drawinfo.ItemLocation.Y += drawinfo.torsoOffset;
		}

		public static void DrawPlayer_extra_TorsoMinus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y -= drawinfo.torsoOffset;
			drawinfo.ItemLocation.Y -= drawinfo.torsoOffset;
		}

		public static void DrawPlayer_extra_MountPlus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y += (int)drawinfo.mountOffSet / 2;
		}

		public static void DrawPlayer_extra_MountMinus(ref PlayerDrawSet drawinfo)
		{
			drawinfo.Position.Y -= (int)drawinfo.mountOffSet / 2;
		}

		public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawinfo, CompositePlayerDrawContext context, DrawData data)
		{
			drawinfo.DrawDataCache.Add(data);
			switch (context)
			{
			case CompositePlayerDrawContext.BackShoulder:
			case CompositePlayerDrawContext.BackArm:
			case CompositePlayerDrawContext.FrontArm:
			case CompositePlayerDrawContext.FrontShoulder:
			{
				if (drawinfo.armGlowColor.PackedValue == 0)
				{
					break;
				}
				DrawData item2 = data;
				item2.color = drawinfo.armGlowColor;
				Rectangle value2 = item2.sourceRect.Value;
				value2.Y += 224;
				item2.sourceRect = value2;
				if (drawinfo.drawPlayer.body == 227)
				{
					Vector2 position2 = item2.position;
					for (int j = 0; j < 2; j++)
					{
						Vector2 vector2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
						item2.position = position2 + vector2;
						if (j == 0)
						{
							drawinfo.DrawDataCache.Add(item2);
						}
					}
				}
				drawinfo.DrawDataCache.Add(item2);
				break;
			}
			case CompositePlayerDrawContext.Torso:
			{
				if (drawinfo.bodyGlowColor.PackedValue == 0)
				{
					break;
				}
				DrawData item = data;
				item.color = drawinfo.bodyGlowColor;
				Rectangle value = item.sourceRect.Value;
				value.Y += 224;
				item.sourceRect = value;
				if (drawinfo.drawPlayer.body == 227)
				{
					Vector2 position = item.position;
					for (int i = 0; i < 2; i++)
					{
						Vector2 vector = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
						item.position = position + vector;
						if (i == 0)
						{
							drawinfo.DrawDataCache.Add(item);
						}
					}
				}
				drawinfo.DrawDataCache.Add(item);
				break;
			}
			}
			if (context == CompositePlayerDrawContext.FrontShoulder && drawinfo.drawPlayer.head == 269)
			{
				Vector2 position3 = drawinfo.helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
				DrawData item3 = new DrawData(TextureAssets.Extra[214].get_Value(), position3, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item3.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item3);
				item3 = new DrawData(TextureAssets.GlowMask[308].get_Value(), position3, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item3.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item3);
			}
			if (context == CompositePlayerDrawContext.FrontArm && drawinfo.drawPlayer.body == 205)
			{
				Color color = new Color(100, 100, 100, 0);
				ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
				int num = 4;
				for (int k = 0; k < num; k++)
				{
					float num2 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
					float num3 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
					DrawData item4 = data;
					Rectangle value3 = item4.sourceRect.Value;
					value3.Y += 224;
					item4.sourceRect = value3;
					item4.position.X += num2;
					item4.position.Y += num3;
					item4.color = color;
					drawinfo.DrawDataCache.Add(item4);
				}
			}
		}

		public static void DrawPlayer_01_BackHair(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.hideHair && drawinfo.backHairDraw)
			{
				Vector2 position = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset;
				if (drawinfo.drawPlayer.head == -1 || drawinfo.fullHair || drawinfo.drawsBackHairWithoutHeadgear)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
				else if (drawinfo.hatHair)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_02_MountBehindPlayer(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Active)
			{
				DrawMeowcartTrail(ref drawinfo);
				DrawTiedBalloons(ref drawinfo);
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 0, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 1, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			}
		}

		public static void DrawPlayer_03_Carpet(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.carpetFrame >= 0)
			{
				Color colorArmorLegs = drawinfo.colorArmorLegs;
				float num = 0f;
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					num = 10f;
				}
				DrawData item = new DrawData(TextureAssets.FlyingCarpet.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 28f * drawinfo.drawPlayer.gravDir + num)), new Rectangle(0, TextureAssets.FlyingCarpet.Height() / 6 * drawinfo.drawPlayer.carpetFrame, TextureAssets.FlyingCarpet.Width(), TextureAssets.FlyingCarpet.Height() / 6), colorArmorLegs, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.FlyingCarpet.Width() / 2, TextureAssets.FlyingCarpet.Height() / 8), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cCarpet;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_03_PortableStool(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.portableStoolInfo.IsInUse)
			{
				Texture2D value = TextureAssets.Extra[102].get_Value();
				Vector2 position = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height + 28f));
				Rectangle rectangle = value.Frame();
				Vector2 origin = rectangle.Size() * new Vector2(0.5f, 1f);
				DrawData item = new DrawData(value, position, rectangle, drawinfo.colorArmorLegs, drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cPortableStool;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_04_ElectrifiedDebuffBack(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
			{
				return;
			}
			Texture2D value = TextureAssets.GlowMask[25].get_Value();
			int num = drawinfo.drawPlayer.miscCounter / 5;
			for (int i = 0; i < 2; i++)
			{
				num %= 7;
				if (num <= 1 || num >= 5)
				{
					DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2(value.Width / 2, value.Height / 14), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				num += 3;
			}
		}

		public static void DrawPlayer_05_ForbiddenSetRing(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.setForbidden && drawinfo.shadow == 0f)
			{
				Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
				Texture2D value = TextureAssets.Extra[74].get_Value();
				Texture2D value2 = TextureAssets.GlowMask[217].get_Value();
				bool num = !drawinfo.drawPlayer.setForbiddenCooldownLocked;
				int num2 = 0;
				num2 = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * (MathF.PI * 2f)).ToRotationVector2().Y * 6f);
				float num3 = ((float)drawinfo.drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 4f;
				Color color2 = new Color(80, 70, 40, 0) * (num3 / 8f + 0.5f) * 0.8f;
				if (!num)
				{
					num2 = 0;
					num3 = 2f;
					color2 = new Color(80, 70, 40, 0) * 0.3f;
					color = color.MultiplyRGB(new Color(0.5f, 0.5f, 1f));
				}
				Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
				int num4 = 10;
				int num5 = 20;
				if (drawinfo.drawPlayer.head == 238)
				{
					num4 += 4;
					num5 += 4;
				}
				vector += new Vector2(-drawinfo.drawPlayer.direction * num4, (float)(-num5) * drawinfo.drawPlayer.gravDir + (float)num2 * drawinfo.drawPlayer.gravDir);
				DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
				for (float num6 = 0f; num6 < 4f; num6 += 1f)
				{
					item = new DrawData(value2, vector + (num6 * (MathF.PI / 2f)).ToRotationVector2() * num3, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_01_3_BackHead(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < 282)
			{
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
				if (num >= 0)
				{
					Vector2 helmetOffset = drawinfo.helmetOffset;
					DrawData item = new DrawData(TextureAssets.ArmorHead[num].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_01_2_JimsCloak(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.legs == 60 && !drawinfo.isSitting && !drawinfo.drawPlayer.invis && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				DrawData item = new DrawData(TextureAssets.Extra[153].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_05_2_SafemanSun(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 238 && drawinfo.shadow == 0f)
			{
				Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
				Texture2D value = TextureAssets.Extra[152].get_Value();
				Texture2D value2 = TextureAssets.Extra[152].get_Value();
				int num = 0;
				num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * (MathF.PI * 2f)).ToRotationVector2().Y * 6f);
				float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 4f;
				Color color2 = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
				Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
				int num3 = 8;
				int num4 = 20;
				num3 += 4;
				num4 += 4;
				vector += new Vector2(-drawinfo.drawPlayer.direction * num3, (float)(-num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
				DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				for (float num5 = 0f; num5 < 4f; num5 += 1f)
				{
					item = new DrawData(value2, vector + (num5 * (MathF.PI / 2f)).ToRotationVector2() * num2, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_06_WebbedDebuffBack(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y != 0f)
			{
				Color color = drawinfo.colorArmorBody * 0.75f;
				Texture2D value = TextureAssets.Extra[32].get_Value();
				DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_07_LeinforsHairShampoo(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.leinforsHair && (drawinfo.fullHair || drawinfo.hatHair || drawinfo.drawsBackHairWithoutHeadgear || drawinfo.drawPlayer.head == -1 || drawinfo.drawPlayer.head == 0) && drawinfo.drawPlayer.hair != 12 && drawinfo.shadow == 0f && Main.rgbToHsl(drawinfo.colorHead).Z > 0.2f)
			{
				if (Main.rand.Next(20) == 0 && !drawinfo.hatHair)
				{
					Rectangle r = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * -20f), new Vector2(20f, 14f));
					int num = Dust.NewDust(r.TopLeft(), r.Width, r.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num].fadeIn = 1f;
					Main.dust[num].velocity *= 0.1f;
					Main.dust[num].noLight = true;
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
					drawinfo.DustCache.Add(num);
				}
				if (Main.rand.Next(40) == 0 && drawinfo.hatHair)
				{
					Rectangle r2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(drawinfo.drawPlayer.direction * -10, drawinfo.drawPlayer.gravDir * -10f), new Vector2(5f, 5f));
					int num2 = Dust.NewDust(r2.TopLeft(), r2.Width, r2.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num2].fadeIn = 1f;
					Main.dust[num2].velocity *= 0.1f;
					Main.dust[num2].noLight = true;
					Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
					drawinfo.DustCache.Add(num2);
				}
				if (drawinfo.drawPlayer.velocity.X != 0f && drawinfo.backHairDraw && Main.rand.Next(15) == 0)
				{
					Rectangle r3 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(drawinfo.drawPlayer.direction * -14, 0f), new Vector2(4f, 30f));
					int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num3].fadeIn = 1f;
					Main.dust[num3].velocity *= 0.1f;
					Main.dust[num3].noLight = true;
					Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
					drawinfo.DustCache.Add(num3);
				}
			}
		}

		public static bool DrawPlayer_08_PlayerVisuallyHasFullArmorSet(PlayerDrawSet drawinfo, int head, int body, int legs)
		{
			if (drawinfo.drawPlayer.head == head && drawinfo.drawPlayer.body == body)
			{
				return drawinfo.drawPlayer.legs == legs;
			}
			return false;
		}

		public static void DrawPlayer_08_Backpacks(ref PlayerDrawSet drawinfo)
		{
			if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 266, 235, 218))
			{
				Vector2 vec = new Vector2(-2f + -2f * drawinfo.drawPlayer.Directions.X, 0f) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2);
				vec = vec.Floor();
				Texture2D value = TextureAssets.Extra[212].get_Value();
				Rectangle value2 = value.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
				Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
				immuneAlphaPure *= drawinfo.drawPlayer.stealth;
				DrawData item = new DrawData(value, vec, value2, immuneAlphaPure, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 268, 237, 222))
			{
				Vector2 vec2 = new Vector2(-9f + 1f * drawinfo.drawPlayer.Directions.X, -4f * drawinfo.drawPlayer.Directions.Y) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2);
				vec2 = vec2.Floor();
				Texture2D value3 = TextureAssets.Extra[213].get_Value();
				Rectangle value4 = value3.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
				Color immuneAlphaPure2 = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
				immuneAlphaPure2 *= drawinfo.drawPlayer.stealth;
				DrawData item = new DrawData(value3, vec2, value4, immuneAlphaPure2, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.heldItem.type == 4818 && drawinfo.drawPlayer.ownedProjectileCounts[902] == 0)
			{
				int num = 8;
				Vector2 vector = new Vector2(0f, 8f);
				Vector2 vec3 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + vector;
				vec3 = vec3.Floor();
				DrawData item = new DrawData(TextureAssets.BackPack[num].get_Value(), vec3, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.backpack > 0 && drawinfo.drawPlayer.backpack < 37 && !drawinfo.drawPlayer.mount.Active)
			{
				Vector2 vector2 = new Vector2(0f, 8f);
				Vector2 vec4 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + vector2;
				vec4 = vec4.Floor();
				DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.backpack].get_Value(), vec4, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBackpack;
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				if (drawinfo.heldItem.type != 1178 && drawinfo.heldItem.type != 779 && drawinfo.heldItem.type != 5134 && drawinfo.heldItem.type != 1295 && drawinfo.heldItem.type != 1910 && !drawinfo.drawPlayer.turtleArmor && drawinfo.drawPlayer.body != 106 && drawinfo.drawPlayer.body != 170)
				{
					return;
				}
				int type = drawinfo.heldItem.type;
				int num2 = 1;
				float num3 = -4f;
				float num4 = -8f;
				int shader = 0;
				if (drawinfo.drawPlayer.turtleArmor)
				{
					num2 = 4;
					shader = drawinfo.cBody;
				}
				else if (drawinfo.drawPlayer.body == 106)
				{
					num2 = 6;
					shader = drawinfo.cBody;
				}
				else if (drawinfo.drawPlayer.body == 170)
				{
					num2 = 7;
					shader = drawinfo.cBody;
				}
				else
				{
					switch (type)
					{
					case 1178:
						num2 = 1;
						break;
					case 779:
						num2 = 2;
						break;
					case 5134:
						num2 = 9;
						break;
					case 1295:
						num2 = 3;
						break;
					case 1910:
						num2 = 5;
						break;
					}
				}
				Vector2 vector3 = new Vector2(0f, 8f);
				Vector2 vec5 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + vector3;
				vec5 = vec5.Floor();
				Vector2 vec6 = drawinfo.Position - Main.screenPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2((-9f + num3) * (float)drawinfo.drawPlayer.direction, (2f + num4) * drawinfo.drawPlayer.gravDir) + vector3;
				vec6 = vec6.Floor();
				switch (num2)
				{
				case 7:
				{
					DrawData item = new DrawData(TextureAssets.BackPack[num2].get_Value(), vec5, new Rectangle(0, drawinfo.drawPlayer.bodyFrame.Y, TextureAssets.BackPack[num2].Width(), drawinfo.drawPlayer.bodyFrame.Height), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)TextureAssets.BackPack[num2].Width() * 0.5f, drawinfo.bodyVect.Y), 1f, drawinfo.playerEffect);
					item.shader = shader;
					drawinfo.DrawDataCache.Add(item);
					break;
				}
				case 4:
				case 6:
				{
					DrawData item = new DrawData(TextureAssets.BackPack[num2].get_Value(), vec5, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = shader;
					drawinfo.DrawDataCache.Add(item);
					break;
				}
				default:
				{
					DrawData item = new DrawData(TextureAssets.BackPack[num2].get_Value(), vec6, new Rectangle(0, 0, TextureAssets.BackPack[num2].Width(), TextureAssets.BackPack[num2].Height()), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.BackPack[num2].Width() / 2, TextureAssets.BackPack[num2].Height() / 2), 1f, drawinfo.playerEffect);
					item.shader = shader;
					drawinfo.DrawDataCache.Add(item);
					break;
				}
				}
			}
		}

		public static void DrawPlayer_08_1_Tails(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.tail > 0 && drawinfo.drawPlayer.tail < 37 && !drawinfo.drawPlayer.mount.Active)
			{
				Vector2 zero = Vector2.Zero;
				if (drawinfo.isSitting)
				{
					zero.Y += -2f;
				}
				Vector2 vector = new Vector2(0f, 8f);
				Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + vector;
				vec = vec.Floor();
				DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.tail].get_Value(), vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cTail;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_10_BackAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.back <= 0 || drawinfo.drawPlayer.back >= 37)
			{
				return;
			}
			if (drawinfo.drawPlayer.front >= 1 && drawinfo.drawPlayer.front <= 4)
			{
				int num = drawinfo.drawPlayer.bodyFrame.Y / 56;
				if (num < 1 || num > 5)
				{
					drawinfo.armorAdjust = 10;
				}
				else
				{
					if (drawinfo.drawPlayer.front == 1)
					{
						drawinfo.armorAdjust = 0;
					}
					if (drawinfo.drawPlayer.front == 2)
					{
						drawinfo.armorAdjust = 8;
					}
					if (drawinfo.drawPlayer.front == 3)
					{
						drawinfo.armorAdjust = 0;
					}
					if (drawinfo.drawPlayer.front == 4)
					{
						drawinfo.armorAdjust = 8;
					}
				}
			}
			Vector2 zero = Vector2.Zero;
			Vector2 vector = new Vector2(0f, 8f);
			Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + new Vector2(0f, -4f) + vector;
			vec = vec.Floor();
			DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.back].get_Value(), vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBack;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.back == 36)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				Rectangle value = bodyFrame;
				value.Width = 2;
				int num2 = 0;
				int num3 = bodyFrame.Width / 2;
				int num4 = 2;
				if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
				{
					num2 = bodyFrame.Width - 2;
					num4 = -2;
				}
				for (int i = 0; i < num3; i++)
				{
					value.X = bodyFrame.X + 2 * i;
					Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
					immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
					item = new DrawData(TextureAssets.GlowMask[332].get_Value(), vec + new Vector2(num2 + i * num4, 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBack;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_09_Wings(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.dead || drawinfo.hideEntirePlayer)
			{
				return;
			}
			Vector2 directions = drawinfo.drawPlayer.Directions;
			Vector2 vector = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.Size / 2f;
			Vector2 vector2 = new Vector2(0f, 7f);
			vector = drawinfo.Position - Main.screenPosition + new Vector2(drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2) + vector2;
			if (drawinfo.drawPlayer.wings <= 0)
			{
				return;
			}
			Main.instance.LoadWings(drawinfo.drawPlayer.wings);
			DrawData item;
			if (drawinfo.drawPlayer.wings == 22)
			{
				if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					return;
				}
				Main.instance.LoadItemFlames(1866);
				Color colorArmorBody = drawinfo.colorArmorBody;
				int num = 26;
				int num2 = -9;
				Vector2 vector3 = vector + new Vector2(num2, num) * directions;
				if (drawinfo.shadow == 0f && drawinfo.drawPlayer.grappling[0] == -1)
				{
					for (int i = 0; i < 7; i++)
					{
						Color color = new Color(250 - i * 10, 250 - i * 10, 250 - i * 10, 150 - i * 10);
						Vector2 vector4 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
						drawinfo.stealth *= drawinfo.stealth;
						drawinfo.stealth *= 1f - drawinfo.shadow;
						color = new Color((int)((float)(int)color.R * drawinfo.stealth), (int)((float)(int)color.G * drawinfo.stealth), (int)((float)(int)color.B * drawinfo.stealth), (int)((float)(int)color.A * drawinfo.stealth));
						vector4.X = drawinfo.drawPlayer.itemFlamePos[i].X;
						vector4.Y = 0f - drawinfo.drawPlayer.itemFlamePos[i].Y;
						vector4 *= 0.5f;
						Vector2 position = (vector3 + vector4).Floor();
						item = new DrawData(TextureAssets.ItemFlame[1866].get_Value(), position, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 - 2), color, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14), 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vector3.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			if (drawinfo.drawPlayer.wings == 28)
			{
				if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					Color colorArmorBody2 = drawinfo.colorArmorBody;
					Vector2 vector5 = new Vector2(0f, 19f);
					Vector2 vec = vector + vector5 * directions;
					Texture2D value = TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value();
					Rectangle rectangle = value.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter / 5 % 4);
					rectangle.Width -= 2;
					rectangle.Height -= 2;
					item = new DrawData(value, vec.Floor(), rectangle, Color.Lerp(colorArmorBody2, Color.White, 1f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
					value = TextureAssets.Extra[38].get_Value();
					item = new DrawData(value, vec.Floor(), rectangle, Color.Lerp(colorArmorBody2, Color.White, 0.5f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			if (drawinfo.drawPlayer.wings == 45)
			{
				if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					return;
				}
				DrawStarboardRainbowTrail(ref drawinfo, vector, directions);
				Color color2 = new Color(255, 255, 255, 255);
				int num3 = 22;
				int num4 = 0;
				Vector2 vec2 = vector + new Vector2(num4, num3) * directions;
				Color color3 = color2 * (1f - drawinfo.shadow);
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vec2.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color3, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.shadow == 0f)
				{
					float num5 = ((float)drawinfo.drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 4f;
					Color color4 = new Color(70, 70, 70, 0) * (num5 / 8f + 0.5f) * 0.4f;
					for (float num6 = 0f; num6 < MathF.PI * 2f; num6 += MathF.PI / 2f)
					{
						item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vec2.Floor() + num6.ToRotationVector2() * num5, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color4, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12), 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				return;
			}
			if (drawinfo.drawPlayer.wings == 34)
			{
				if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					drawinfo.stealth *= drawinfo.stealth;
					drawinfo.stealth *= 1f - drawinfo.shadow;
					Color color5 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
					Vector2 vector6 = new Vector2(0f, 0f);
					Texture2D value2 = TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value();
					Vector2 vec3 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + vector6 * drawinfo.drawPlayer.Directions - Vector2.UnitX * drawinfo.drawPlayer.direction * 4f;
					Rectangle rectangle2 = value2.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
					rectangle2.Width -= 2;
					rectangle2.Height -= 2;
					item = new DrawData(value2, vec3.Floor(), rectangle2, color5, drawinfo.drawPlayer.bodyRotation, rectangle2.Size() / 2f, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			if (drawinfo.drawPlayer.wings == 40)
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				Color color6 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
				Vector2 vector7 = new Vector2(-4f, 0f);
				Texture2D value3 = TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value();
				Vector2 vector8 = vector + vector7 * directions;
				for (int j = 0; j < 1; j++)
				{
					SpriteEffects spriteEffects = drawinfo.playerEffect;
					Vector2 scale = new Vector2(1f);
					Vector2 zero = Vector2.Zero;
					zero.X = drawinfo.drawPlayer.direction * 3;
					if (j == 1)
					{
						spriteEffects ^= SpriteEffects.FlipHorizontally;
						scale = new Vector2(0.7f, 1f);
						zero.X += (float)(-drawinfo.drawPlayer.direction) * 6f;
					}
					Vector2 vector9 = drawinfo.drawPlayer.velocity * -1.5f;
					int num7 = 0;
					int num8 = 8;
					float num9 = 4f;
					if (drawinfo.drawPlayer.velocity.Y == 0f)
					{
						num7 = 8;
						num8 = 14;
						num9 = 3f;
					}
					for (int k = num7; k < num8; k++)
					{
						Vector2 vec4 = vector8;
						Rectangle rectangle3 = value3.Frame(1, 14, 0, k);
						rectangle3.Width -= 2;
						rectangle3.Height -= 2;
						int num10 = (k - num7) % (int)num9;
						Vector2 vector10 = new Vector2(0f, 0.5f).RotatedBy((drawinfo.drawPlayer.miscCounterNormalized * (2f + (float)num10) + (float)num10 * 0.5f + (float)j * 1.3f) * (MathF.PI * 2f)) * (num10 + 1);
						vec4 += vector10;
						vec4 += vector9 * ((float)num10 / num9);
						vec4 += zero;
						item = new DrawData(value3, vec4.Floor(), rectangle3, color6, drawinfo.drawPlayer.bodyRotation, rectangle3.Size() / 2f, scale, spriteEffects);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				return;
			}
			if (drawinfo.drawPlayer.wings == 39)
			{
				if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
				{
					drawinfo.stealth *= drawinfo.stealth;
					drawinfo.stealth *= 1f - drawinfo.shadow;
					Color colorArmorBody3 = drawinfo.colorArmorBody;
					Vector2 vector11 = new Vector2(-6f, -7f);
					Texture2D value4 = TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value();
					Vector2 vec5 = vector + vector11 * directions;
					Rectangle rectangle4 = value4.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
					rectangle4.Width -= 2;
					rectangle4.Height -= 2;
					item = new DrawData(value4, vec5.Floor(), rectangle4, colorArmorBody3, drawinfo.drawPlayer.bodyRotation, rectangle4.Size() / 2f, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			int num11 = 0;
			int num12 = 0;
			int num13 = 4;
			if (drawinfo.drawPlayer.wings == 43)
			{
				num12 = -5;
				num11 = -7;
				num13 = 7;
			}
			else if (drawinfo.drawPlayer.wings == 44)
			{
				num13 = 7;
			}
			else if (drawinfo.drawPlayer.wings == 5)
			{
				num12 = 4;
				num11 -= 4;
			}
			else if (drawinfo.drawPlayer.wings == 27)
			{
				num12 = 4;
			}
			Color color7 = drawinfo.colorArmorBody;
			if (drawinfo.drawPlayer.wings == 9 || drawinfo.drawPlayer.wings == 29)
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				color7 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
			}
			if (drawinfo.drawPlayer.wings == 10)
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				color7 = new Color((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(175f * drawinfo.stealth));
			}
			if (drawinfo.drawPlayer.wings == 11 && color7.A > Main.gFade)
			{
				color7.A = Main.gFade;
			}
			if (drawinfo.drawPlayer.wings == 31)
			{
				color7.A = (byte)(220f * drawinfo.stealth);
			}
			if (drawinfo.drawPlayer.wings == 32)
			{
				color7.A = (byte)(127f * drawinfo.stealth);
			}
			if (drawinfo.drawPlayer.wings == 6)
			{
				color7.A = (byte)(160f * drawinfo.stealth);
				color7 *= 0.9f;
			}
			Vector2 vector12 = vector + new Vector2(num12 - 9, num11 + 2) * directions;
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13), color7, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13 / 2), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.wings == 43 && drawinfo.shadow == 0f)
			{
				Vector2 vector13 = vector12;
				Vector2 origin = new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13 / 2);
				Rectangle value5 = new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num13);
				for (int l = 0; l < 2; l++)
				{
					item = new DrawData(position: vector13 + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f), texture: TextureAssets.GlowMask[272].get_Value(), sourceRect: value5, color: new Color(230, 230, 230, 60), rotation: drawinfo.drawPlayer.bodyRotation, origin: origin, scale: 1f, effect: drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.drawPlayer.wings == 23)
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				item = new DrawData(color: new Color((int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth)), texture: TextureAssets.Flames[8].get_Value(), position: vector12.Floor(), sourceRect: new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), rotation: drawinfo.drawPlayer.bodyRotation, origin: new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), scale: 1f, effect: drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.wings == 27)
			{
				item = new DrawData(TextureAssets.GlowMask[92].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.wings == 44)
			{
				PlayerRainbowWingsTextureContent playerRainbowWings = TextureAssets.RenderTargets.PlayerRainbowWings;
				playerRainbowWings.Request();
				if (playerRainbowWings.IsReady)
				{
					RenderTarget2D target = playerRainbowWings.GetTarget();
					item = new DrawData(target, vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), new Color(255, 255, 255, 255) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (drawinfo.drawPlayer.wings == 30)
			{
				item = new DrawData(TextureAssets.GlowMask[181].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.wings == 38)
			{
				Color color9 = drawinfo.ArkhalisColor * drawinfo.stealth * (1f - drawinfo.shadow);
				item = new DrawData(TextureAssets.GlowMask[251].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color9, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				for (int num14 = drawinfo.drawPlayer.shadowPos.Length - 2; num14 >= 0; num14--)
				{
					Color color10 = color9;
					color10.A = 0;
					color10 *= MathHelper.Lerp(1f, 0f, (float)num14 / 3f);
					color10 *= 0.1f;
					Vector2 vector14 = drawinfo.drawPlayer.shadowPos[num14] - drawinfo.drawPlayer.position;
					for (float num15 = 0f; num15 < 1f; num15 += 0.01f)
					{
						Vector2 vector15 = new Vector2(2f, 0f).RotatedBy(num15 / 0.04f * (MathF.PI * 2f));
						item = new DrawData(TextureAssets.GlowMask[251].get_Value(), vector15 + vector14 * num15 + vector12, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color10 * (1f - num15), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
			else if (drawinfo.drawPlayer.wings == 29)
			{
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1.06f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.wings == 36)
			{
				item = new DrawData(TextureAssets.GlowMask[213].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1.06f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				Vector2 spinningpoint = new Vector2(0f, 2f - drawinfo.shadow * 2f);
				for (int m = 0; m < 4; m++)
				{
					item = new DrawData(TextureAssets.GlowMask[213].get_Value(), spinningpoint.RotatedBy(MathF.PI / 2f * (float)m) + vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(127, 127, 127, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (drawinfo.drawPlayer.wings == 31)
			{
				Color color11 = new Color(255, 255, 255, 0);
				color11 = Color.Lerp(Color.HotPink, Color.Crimson, (float)Math.Cos(MathF.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 100f)) * 0.4f + 0.5f);
				color11.A = 0;
				for (int n = 0; n < 4; n++)
				{
					Vector2 vector16 = new Vector2((float)Math.Cos(MathF.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 60f)) * 0.5f + 0.5f, 0f).RotatedBy((float)n * (MathF.PI / 2f)) * 1f;
					item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vector12.Floor() + vector16, new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color11 * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color11 * drawinfo.stealth * (1f - drawinfo.shadow) * 1f, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.wings == 32)
			{
				item = new DrawData(TextureAssets.GlowMask[183].get_Value(), vector12.Floor(), new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8), 1.06f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_12_1_BalloonFronts(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.balloonFront <= 0 || drawinfo.drawPlayer.balloonFront >= 20)
			{
				return;
			}
			DrawData item;
			if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloonFront])
			{
				item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBalloonFront;
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			int num = ((Main.hasFocus && (!Main.ingameOptionsWindow || !Main.autoPause)) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
			Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
			if (drawinfo.drawPlayer.direction != 1)
			{
				vector.X = (float)drawinfo.drawPlayer.width - vector.X;
			}
			if (drawinfo.drawPlayer.gravDir != 1f)
			{
				vector.Y -= drawinfo.drawPlayer.height;
			}
			Vector2 vector2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
			Vector2 vector3 = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + vector.X), (int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir));
			vector3 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height) + vector2;
			vector3 = vector3.Floor();
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].get_Value(), vector3, new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2(26 + drawinfo.drawPlayer.direction * 4, 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloonFront;
			drawinfo.DrawDataCache.Add(item);
		}

		public static void DrawPlayer_11_Balloons(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.balloon <= 0 || drawinfo.drawPlayer.balloon >= 20)
			{
				return;
			}
			DrawData item;
			if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloon])
			{
				item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBalloon;
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			int num = ((Main.hasFocus && (!Main.ingameOptionsWindow || !Main.autoPause)) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
			Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
			if (drawinfo.drawPlayer.direction != 1)
			{
				vector.X = (float)drawinfo.drawPlayer.width - vector.X;
			}
			if (drawinfo.drawPlayer.gravDir != 1f)
			{
				vector.Y -= drawinfo.drawPlayer.height;
			}
			Vector2 vector2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
			Vector2 vector3 = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + vector.X), (int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir));
			vector3 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height) + vector2;
			vector3 = vector3.Floor();
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].get_Value(), vector3, new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2(26 + drawinfo.drawPlayer.direction * 4, 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloon;
			drawinfo.DrawDataCache.Add(item);
		}

		public static void DrawPlayer_12_Skin(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeTorso)
			{
				DrawPlayer_12_Skin_Composite(ref drawinfo);
				return;
			}
			if (drawinfo.isSitting)
			{
				drawinfo.hidesBottomSkin = true;
			}
			if (!drawinfo.hidesTopSkin)
			{
				drawinfo.Position.Y += drawinfo.torsoOffset;
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
				drawinfo.Position.Y -= drawinfo.torsoOffset;
			}
			if (!drawinfo.hidesBottomSkin && !IsBottomOverridden(ref drawinfo))
			{
				if (drawinfo.isSitting)
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].get_Value(), drawinfo.colorLegs);
					return;
				}
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static bool IsBottomOverridden(ref PlayerDrawSet drawinfo)
		{
			if (ShouldOverrideLegs_CheckPants(ref drawinfo))
			{
				return true;
			}
			if (ShouldOverrideLegs_CheckShoes(ref drawinfo))
			{
				return true;
			}
			return false;
		}

		public static bool ShouldOverrideLegs_CheckPants(ref PlayerDrawSet drawinfo)
		{
			switch (drawinfo.drawPlayer.legs)
			{
			case 67:
			case 106:
			case 138:
			case 140:
			case 143:
			case 217:
			case 222:
			case 226:
			case 228:
				return true;
			default:
				return false;
			}
		}

		public static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawinfo)
		{
			sbyte shoe = drawinfo.drawPlayer.shoe;
			if (shoe == 15)
			{
				return true;
			}
			return false;
		}

		public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.hidesTopSkin && !drawinfo.drawPlayer.invis)
			{
				Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
				vector.Y += drawinfo.torsoOffset;
				Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				vector2.Y -= 2f;
				vector += vector2 * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
				float bodyRotation = drawinfo.drawPlayer.bodyRotation;
				Vector2 vector3 = vector;
				Vector2 vector4 = vector;
				Vector2 bodyVect = drawinfo.bodyVect;
				Vector2 bodyVect2 = drawinfo.bodyVect;
				Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
				vector3 += compositeOffset_BackArm;
				_ = bodyVect + compositeOffset_BackArm;
				Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
				bodyVect2 += compositeOffset_FrontArm;
				_ = vector4 + compositeOffset_FrontArm;
				if (drawinfo.drawFloatingTube)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].get_Value(), vector, new Rectangle(0, 0, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cFloatingTube
					});
				}
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].get_Value(), vector, drawinfo.compTorsoFrame, drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.skinDyePacked
				});
			}
			if (!drawinfo.hidesBottomSkin && !drawinfo.drawPlayer.invis && !IsBottomOverridden(ref drawinfo))
			{
				if (drawinfo.isSitting)
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].get_Value(), drawinfo.colorLegs, drawinfo.skinDyePacked);
				}
				else
				{
					DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					DrawData item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
		}

		public static void DrawPlayer_12_SkinComposite_BackArmShirt(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			vector.Y += drawinfo.torsoOffset;
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 vector3 = vector;
			Vector2 position = vector;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			vector3 += compositeOffset_BackArm;
			position += drawinfo.backShoulderOffset;
			bodyVect += compositeOffset_BackArm;
			float rotation = bodyRotation + drawinfo.compositeBackArmRotation;
			bool flag = !drawinfo.drawPlayer.invis;
			bool flag2 = !drawinfo.drawPlayer.invis;
			bool flag3 = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < 248;
			bool flag4 = !drawinfo.hidesTopSkin;
			bool flag5 = false;
			if (flag3)
			{
				flag &= drawinfo.missingHand;
				if (flag2 && drawinfo.missingArm)
				{
					if (flag4)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					if (!flag5 && flag4)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
						flag5 = true;
					}
					flag2 = false;
				}
				if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].get_Value();
					if (!drawinfo.hideCompositeShoulders)
					{
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackShoulder, new DrawData(value, position, drawinfo.compBackShoulderFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.cBody
						});
					}
					DrawPlayer_12_1_BalloonFronts(ref drawinfo);
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArm, new DrawData(value, vector3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					});
				}
			}
			if (flag)
			{
				if (flag4)
				{
					if (flag2)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					if (!flag5 && flag4)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
						flag5 = true;
					}
				}
				if (!flag3)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					DrawPlayer_12_1_BalloonFronts(ref drawinfo);
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].get_Value(), vector3, drawinfo.compBackArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
				}
			}
			if (drawinfo.drawPlayer.handoff > 0 && drawinfo.drawPlayer.handoff < 16)
			{
				Texture2D value2 = TextureAssets.AccHandsOffComposite[drawinfo.drawPlayer.handoff].get_Value();
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArmAccessory, new DrawData(value2, vector3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cHandOff
				});
			}
			if (drawinfo.drawPlayer.drawingFootball)
			{
				Main.instance.LoadProjectile(861);
				Texture2D value3 = TextureAssets.Projectile[861].get_Value();
				Rectangle rectangle = value3.Frame(1, 4);
				Vector2 origin = rectangle.Size() / 2f;
				Vector2 position2 = vector3 + new Vector2(drawinfo.drawPlayer.direction * -2, drawinfo.drawPlayer.gravDir * 4f);
				drawinfo.DrawDataCache.Add(new DrawData(value3, position2, rectangle, drawinfo.colorArmorBody, bodyRotation + MathF.PI / 4f * (float)drawinfo.drawPlayer.direction, origin, 0.8f, drawinfo.playerEffect));
			}
		}

		public static void DrawPlayer_13_Leggings(ref PlayerDrawSet drawinfo)
		{
			Vector2 legsOffset = drawinfo.legsOffset;
			if (drawinfo.isSitting && drawinfo.drawPlayer.legs != 140 && drawinfo.drawPlayer.legs != 217)
			{
				if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < 236 && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
				{
					if (!drawinfo.drawPlayer.invis)
					{
						DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].get_Value(), drawinfo.colorArmorLegs, drawinfo.cLegs);
						if (drawinfo.legsGlowMask != -1)
						{
							DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].get_Value(), drawinfo.legsGlowColor, drawinfo.cLegs);
						}
					}
				}
				else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 11].get_Value(), drawinfo.colorPants);
					DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 12].get_Value(), drawinfo.colorShoes);
				}
			}
			else if (drawinfo.drawPlayer.legs == 140)
			{
				if (!drawinfo.drawPlayer.invis && !drawinfo.drawPlayer.mount.Active)
				{
					Texture2D value = TextureAssets.Extra[73].get_Value();
					bool flag = drawinfo.drawPlayer.legFrame.Y != drawinfo.drawPlayer.legFrame.Height || Main.gameMenu;
					int num = drawinfo.drawPlayer.miscCounter / 3 % 8;
					if (flag)
					{
						num = drawinfo.drawPlayer.miscCounter / 4 % 8;
					}
					Rectangle rectangle = new Rectangle(18 * flag.ToInt(), num * 26, 16, 24);
					float num2 = 12f;
					if (drawinfo.drawPlayer.bodyFrame.Height != 0)
					{
						num2 = 12f - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height].Y;
					}
					if (drawinfo.drawPlayer.Directions.Y == -1f)
					{
						num2 -= 6f;
					}
					Vector2 scale = new Vector2(1f, 1f);
					Vector2 vector = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 0.5f + 0.5f * drawinfo.drawPlayer.gravDir);
					_ = drawinfo.drawPlayer.direction;
					Vector2 vec = vector + new Vector2(0f, (0f - num2) * drawinfo.drawPlayer.gravDir) - Main.screenPosition + drawinfo.drawPlayer.legPosition;
					if (drawinfo.isSitting)
					{
						vec.Y += drawinfo.seatYOffset;
					}
					vec += legsOffset;
					vec = vec.Floor();
					DrawData item = new DrawData(value, vec, rectangle, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, rectangle.Size() * new Vector2(0.5f, 0.5f - drawinfo.drawPlayer.gravDir * 0.5f), scale, drawinfo.playerEffect);
					item.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (drawinfo.drawPlayer.legs > 0 && drawinfo.drawPlayer.legs < 236 && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				if (drawinfo.drawPlayer.invis)
				{
					return;
				}
				DrawData item = new DrawData(TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].get_Value(), legsOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.legsGlowMask == -1)
				{
					return;
				}
				if (drawinfo.legsGlowMask == 274)
				{
					for (int i = 0; i < 2; i++)
					{
						item = new DrawData(position: legsOffset + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, texture: TextureAssets.GlowMask[drawinfo.legsGlowMask].get_Value(), sourceRect: drawinfo.drawPlayer.legFrame, color: drawinfo.legsGlowColor, rotation: drawinfo.drawPlayer.legRotation, origin: drawinfo.legVect, scale: 1f, effect: drawinfo.playerEffect);
						item.shader = drawinfo.cLegs;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				else
				{
					item = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].get_Value(), legsOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
			{
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 11].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorPants, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 12].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShoes, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		private static void DrawSittingLongCoats(ref PlayerDrawSet drawinfo, int specialLegCoat, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false)
		{
			Vector2 legsOffset = drawinfo.legsOffset;
			Vector2 position = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
			Rectangle legFrame = drawinfo.drawPlayer.legFrame;
			position += legsOffset;
			position.X += 2 * drawinfo.drawPlayer.direction;
			legFrame.Y = legFrame.Height * 5;
			if (specialLegCoat == 160 || specialLegCoat == 173)
			{
				legFrame = drawinfo.drawPlayer.legFrame;
			}
			DrawData item = new DrawData(textureToDraw, position, legFrame, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = shaderIndex;
			drawinfo.DrawDataCache.Add(item);
		}

		private static void DrawSittingLegs(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false)
		{
			Vector2 legsOffset = drawinfo.legsOffset;
			Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
			Rectangle legFrame = drawinfo.drawPlayer.legFrame;
			vector.Y -= 2f;
			vector.Y += drawinfo.seatYOffset;
			vector += legsOffset;
			int num = 2;
			int num2 = 42;
			int num3 = 2;
			int num4 = 2;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			bool flag = drawinfo.drawPlayer.legs == 101 || drawinfo.drawPlayer.legs == 102 || drawinfo.drawPlayer.legs == 118 || drawinfo.drawPlayer.legs == 99;
			if (drawinfo.drawPlayer.wearsRobe && !flag)
			{
				num = 0;
				num4 = 0;
				num2 = 6;
				vector.Y += 4f;
				legFrame.Y = legFrame.Height * 5;
			}
			switch (drawinfo.drawPlayer.legs)
			{
			case 214:
			case 215:
			case 216:
				num = -6;
				num4 = 2;
				num5 = 2;
				num3 = 4;
				num2 = 6;
				legFrame = drawinfo.drawPlayer.legFrame;
				vector.Y += 2f;
				break;
			case 106:
			case 143:
			case 226:
				num = 0;
				num4 = 0;
				num2 = 6;
				vector.Y += 4f;
				legFrame.Y = legFrame.Height * 5;
				break;
			case 132:
				num = -2;
				num7 = 2;
				break;
			case 193:
			case 194:
				if (drawinfo.drawPlayer.body == 218)
				{
					num = -2;
					num7 = 2;
					vector.Y += 2f;
				}
				break;
			case 210:
				if (glowmask)
				{
					Vector2 vector2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
					vector += vector2;
				}
				break;
			}
			for (int num8 = num3; num8 >= 0; num8--)
			{
				Vector2 position = vector + new Vector2(num, 2f) * new Vector2(drawinfo.drawPlayer.direction, 1f);
				Rectangle value = legFrame;
				value.Y += num8 * 2;
				value.Y += num2;
				value.Height -= num2;
				value.Height -= num8 * 2;
				if (num8 != num3)
				{
					value.Height = 2;
				}
				position.X += drawinfo.drawPlayer.direction * num4 * num8 + num6 * drawinfo.drawPlayer.direction;
				if (num8 != 0)
				{
					position.X += num7 * drawinfo.drawPlayer.direction;
				}
				position.Y += num2;
				position.Y += num5;
				DrawData item = new DrawData(textureToDraw, position, value, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = shaderIndex;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_14_Shoes(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.shoe <= 0 || drawinfo.drawPlayer.shoe >= 27 || ShouldOverrideLegs_CheckPants(ref drawinfo))
			{
				return;
			}
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].get_Value(), drawinfo.colorArmorLegs, drawinfo.cShoe);
				return;
			}
			DrawData item = new DrawData(TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cShoe;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.shoe == 25 || drawinfo.drawPlayer.shoe == 26)
			{
				DrawPlayer_14_2_GlassSlipperSparkles(ref drawinfo);
			}
		}

		public static void DrawPlayer_14_2_GlassSlipperSparkles(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.shadow == 0f)
			{
				if (Main.rand.Next(60) == 0)
				{
					Rectangle r = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * 16f), new Vector2(20f, 8f));
					int num = Dust.NewDust(r.TopLeft(), r.Width, r.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num].fadeIn = 1f;
					Main.dust[num].velocity *= 0.1f;
					Main.dust[num].noLight = true;
					Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cShoe, drawinfo.drawPlayer);
					drawinfo.DustCache.Add(num);
				}
				if (drawinfo.drawPlayer.velocity.X != 0f && Main.rand.Next(10) == 0)
				{
					Rectangle r2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(drawinfo.drawPlayer.direction * -2, drawinfo.drawPlayer.gravDir * 16f), new Vector2(6f, 8f));
					int num2 = Dust.NewDust(r2.TopLeft(), r2.Width, r2.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
					Main.dust[num2].fadeIn = 1f;
					Main.dust[num2].velocity *= 0.1f;
					Main.dust[num2].noLight = true;
					Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cShoe, drawinfo.drawPlayer);
					drawinfo.DustCache.Add(num2);
				}
			}
		}

		public static void DrawPlayer_15_SkinLongCoat(ref PlayerDrawSet drawinfo)
		{
			if ((drawinfo.skinVar == 3 || drawinfo.skinVar == 8 || drawinfo.skinVar == 7) && (drawinfo.drawPlayer.body <= 0 || drawinfo.drawPlayer.body >= 248) && !drawinfo.drawPlayer.invis)
			{
				if (drawinfo.isSitting)
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 14].get_Value(), drawinfo.colorShirt);
					return;
				}
				DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 14].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShirt, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_16_ArmorLongCoat(ref PlayerDrawSet drawinfo)
		{
			int num = -1;
			switch (drawinfo.drawPlayer.body)
			{
			case 200:
				num = 149;
				break;
			case 202:
				num = 151;
				break;
			case 201:
				num = 150;
				break;
			case 209:
				num = 160;
				break;
			case 207:
				num = 161;
				break;
			case 198:
				num = 162;
				break;
			case 182:
				num = 163;
				break;
			case 168:
				num = 164;
				break;
			case 73:
				num = 170;
				break;
			case 52:
				num = ((!drawinfo.drawPlayer.Male) ? 172 : 171);
				break;
			case 187:
				num = 173;
				break;
			case 205:
				num = 174;
				break;
			case 53:
				num = ((!drawinfo.drawPlayer.Male) ? 176 : 175);
				break;
			case 210:
				num = ((!drawinfo.drawPlayer.Male) ? 177 : 178);
				break;
			case 211:
				num = ((!drawinfo.drawPlayer.Male) ? 181 : 182);
				break;
			case 218:
				num = 195;
				break;
			case 222:
				num = ((!drawinfo.drawPlayer.Male) ? 200 : 201);
				break;
			case 225:
				num = 206;
				break;
			case 236:
				num = 221;
				break;
			case 237:
				num = 223;
				break;
			case 89:
				num = 186;
				break;
			}
			if (num != -1)
			{
				Main.instance.LoadArmorLegs(num);
				if (drawinfo.isSitting && num != 195)
				{
					DrawSittingLongCoats(ref drawinfo, num, TextureAssets.ArmorLeg[num].get_Value(), drawinfo.colorArmorBody, drawinfo.cBody);
					return;
				}
				DrawData item = new DrawData(TextureAssets.ArmorLeg[num].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_17_Torso(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeTorso)
			{
				DrawPlayer_17_TorsoComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < 248)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
				{
					Texture2D texture = (drawinfo.drawPlayer.Male ? TextureAssets.ArmorBody[drawinfo.drawPlayer.body].get_Value() : TextureAssets.FemaleBody[drawinfo.drawPlayer.body].get_Value());
					DrawData item = new DrawData(texture, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
					if (drawinfo.bodyGlowMask != -1)
					{
						item = new DrawData(TextureAssets.GlowMask[drawinfo.bodyGlowMask].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), bodyFrame, drawinfo.bodyGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cBody;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
				{
					DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					DrawData item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				DrawData item;
				if (!drawinfo.drawPlayer.Male)
				{
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				else
				{
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
					item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_17_TorsoComposite(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 vector3 = vector;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			_ = vector3 + compositeOffset_BackArm;
			bodyVect += compositeOffset_BackArm;
			if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < 248)
			{
				if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].get_Value();
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value, vector, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					});
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].get_Value(), vector, drawinfo.compBackShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), vector, drawinfo.compBackShoulderFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].get_Value(), vector, drawinfo.compTorsoFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), vector, drawinfo.compTorsoFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			}
			if (drawinfo.drawFloatingTube)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].get_Value(), vector, new Rectangle(0, 56, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cFloatingTube
				});
			}
		}

		public static void DrawPlayer_18_OffhandAcc(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.usesCompositeBackHandAcc && drawinfo.drawPlayer.handoff > 0 && drawinfo.drawPlayer.handoff < 16)
			{
				DrawData item = new DrawData(TextureAssets.AccHandsOff[drawinfo.drawPlayer.handoff].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHandOff;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_JimsDroneRadio(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.HeldItem.type == 5451 && drawinfo.drawPlayer.itemAnimation == 0)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				Texture2D value = TextureAssets.Extra[261].get_Value();
				DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + drawinfo.drawPlayer.direction * 2, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + 14f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), bodyFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWaist;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_19_WaistAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.waist > 0 && drawinfo.drawPlayer.waist < 17)
			{
				Rectangle value = drawinfo.drawPlayer.legFrame;
				if (ArmorIDs.Waist.Sets.UsesTorsoFraming[drawinfo.drawPlayer.waist])
				{
					value = drawinfo.drawPlayer.bodyFrame;
				}
				DrawData item = new DrawData(TextureAssets.AccWaist[drawinfo.drawPlayer.waist].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, value, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWaist;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_20_NeckAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.neck > 0 && drawinfo.drawPlayer.neck < 12)
			{
				DrawData item = new DrawData(TextureAssets.AccNeck[drawinfo.drawPlayer.neck].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cNeck;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_21_Head(ref PlayerDrawSet drawinfo)
		{
			Vector2 helmetOffset = drawinfo.helmetOffset;
			DrawPlayer_21_Head_TheFace(ref drawinfo);
			bool flag = drawinfo.drawPlayer.head == 14 || drawinfo.drawPlayer.head == 56 || drawinfo.drawPlayer.head == 114 || drawinfo.drawPlayer.head == 158 || drawinfo.drawPlayer.head == 69 || drawinfo.drawPlayer.head == 180;
			bool flag2 = drawinfo.drawPlayer.head == 28;
			bool flag3 = drawinfo.drawPlayer.head == 39 || drawinfo.drawPlayer.head == 38;
			Vector2 vector = new Vector2(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4);
			Vector2 position = (drawinfo.Position - Main.screenPosition + vector).Floor() + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
			if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically))
			{
				int num = drawinfo.drawPlayer.bodyFrame.Height - drawinfo.hairFrontFrame.Height;
				position.Y += num;
			}
			position += drawinfo.hairOffset;
			if (drawinfo.fullHair)
			{
				Color color = drawinfo.colorArmorHead;
				int shader = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
					shader = drawinfo.skinDyePacked;
				}
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = shader;
				drawinfo.DrawDataCache.Add(item);
				if (!drawinfo.drawPlayer.invis)
				{
					item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.hatHair && !drawinfo.drawPlayer.invis)
			{
				DrawData item = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.head == 270)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				bodyFrame.Width += 2;
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (flag)
			{
				Rectangle bodyFrame2 = drawinfo.drawPlayer.bodyFrame;
				Vector2 headVect = drawinfo.headVect;
				if (drawinfo.drawPlayer.gravDir == 1f)
				{
					if (bodyFrame2.Y != 0)
					{
						bodyFrame2.Y -= 2;
						headVect.Y += 2f;
					}
					bodyFrame2.Height -= 8;
				}
				else if (bodyFrame2.Y != 0)
				{
					bodyFrame2.Y -= 2;
					headVect.Y -= 10f;
					bodyFrame2.Height -= 8;
				}
				Color color2 = drawinfo.colorArmorHead;
				int shader2 = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					color2 = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
					shader2 = drawinfo.skinDyePacked;
				}
				DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame2, color2, drawinfo.drawPlayer.headRotation, headVect, 1f, drawinfo.playerEffect);
				item.shader = shader2;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.head == 259)
			{
				int verticalFrames = 27;
				Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value();
				Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
				Vector2 origin = rectangle.Size() / 2f;
				int num2 = drawinfo.drawPlayer.babyBird.ToInt();
				Vector2 vector2 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2(1 + num2 * 2, -26 + drawinfo.drawPlayer.babyBird.ToInt() * -6));
				int hatStacks = GetHatStacks(ref drawinfo, 4955);
				float num3 = MathF.PI / 60f;
				float num4 = num3 * drawinfo.drawPlayer.position.X % (MathF.PI * 2f);
				for (int num5 = hatStacks - 1; num5 >= 0; num5--)
				{
					float x = Vector2.UnitY.RotatedBy(num4 + num3 * (float)num5).X * ((float)num5 / 30f) * 2f - (float)(num5 * 2 * drawinfo.drawPlayer.direction);
					DrawData item = new DrawData(value, vector2 + new Vector2(x, (float)(num5 * -14) * drawinfo.drawPlayer.gravDir), rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cHead;
					drawinfo.DrawDataCache.Add(item);
				}
				if (!drawinfo.drawPlayer.invis)
				{
					DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.hairDyePacked;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			else if (drawinfo.drawPlayer.head > 0 && drawinfo.drawPlayer.head < 282 && !flag2)
			{
				if (!(drawinfo.drawPlayer.invis && flag3))
				{
					if (drawinfo.drawPlayer.head == 13)
					{
						int hatStacks2 = GetHatStacks(ref drawinfo, 205);
						float num6 = MathF.PI / 60f;
						float num7 = num6 * drawinfo.drawPlayer.position.X % (MathF.PI * 2f);
						for (int i = 0; i < hatStacks2; i++)
						{
							float num8 = Vector2.UnitY.RotatedBy(num7 + num6 * (float)i).X * ((float)i / 30f) * 2f;
							DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num8, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f - (float)(4 * i))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
					else if (drawinfo.drawPlayer.head == 265)
					{
						int verticalFrames2 = 6;
						Texture2D value2 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value();
						Rectangle rectangle2 = value2.Frame(1, verticalFrames2, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
						Vector2 origin2 = rectangle2.Size() / 2f;
						Vector2 vector3 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2(0f, -9f));
						int hatStacks3 = GetHatStacks(ref drawinfo, 5004);
						float num9 = MathF.PI / 60f;
						float num10 = num9 * drawinfo.drawPlayer.position.X % (MathF.PI * 2f);
						int num11 = hatStacks3 * 4 + 2;
						int num12 = 0;
						bool flag4 = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
						for (int num13 = num11 - 1; num13 >= 0; num13--)
						{
							int num14 = 0;
							if (num13 == num11 - 1)
							{
								rectangle2.Y = 0;
								num14 = 2;
							}
							else if (num13 == 0)
							{
								rectangle2.Y = rectangle2.Height * 5;
							}
							else
							{
								rectangle2.Y = rectangle2.Height * (num12++ % 4 + 1);
							}
							if (!(rectangle2.Y == rectangle2.Height * 3 && flag4))
							{
								float x2 = Vector2.UnitY.RotatedBy(num10 + num9 * (float)num13).X * ((float)num13 / 10f) * 4f - (float)num13 * 0.1f * (float)drawinfo.drawPlayer.direction;
								DrawData item = new DrawData(value2, vector3 + new Vector2(x2, (float)(num13 * -4 + num14) * drawinfo.drawPlayer.gravDir), rectangle2, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin2, 1f, drawinfo.playerEffect);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
					}
					else
					{
						Rectangle bodyFrame3 = drawinfo.drawPlayer.bodyFrame;
						Vector2 headVect2 = drawinfo.headVect;
						if (drawinfo.drawPlayer.gravDir == 1f)
						{
							bodyFrame3.Height -= 4;
						}
						else
						{
							headVect2.Y -= 4f;
							bodyFrame3.Height -= 4;
						}
						Color color3 = drawinfo.colorArmorHead;
						int shader3 = drawinfo.cHead;
						if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
						{
							color3 = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
							shader3 = drawinfo.skinDyePacked;
						}
						DrawData item = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame3, color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
						item.shader = shader3;
						drawinfo.DrawDataCache.Add(item);
						if (drawinfo.headGlowMask != -1)
						{
							if (drawinfo.headGlowMask == 309)
							{
								int num15 = DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
								if (num15 != 0)
								{
									int num16 = 0;
									num16 += drawinfo.drawPlayer.bodyFrame.Y / 56;
									if (num16 >= Main.OffsetsPlayerHeadgear.Length)
									{
										num16 = 0;
									}
									Vector2 vector4 = Main.OffsetsPlayerHeadgear[num16];
									vector4.Y -= 2f;
									vector4 *= (float)(-drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt());
									Texture2D value3 = TextureAssets.GlowMask[drawinfo.headGlowMask].get_Value();
									int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
									if (num15 == 5)
									{
										frameY = 0;
										if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
										{
											frameY = 2;
										}
									}
									Rectangle value4 = value3.Frame(6, 4, num15, frameY, -2);
									item = new DrawData(value3, vector4 + helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, value4, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
									item.shader = drawinfo.cHead;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							else if (drawinfo.headGlowMask == 273)
							{
								for (int j = 0; j < 2; j++)
								{
									item = new DrawData(position: new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, texture: TextureAssets.GlowMask[drawinfo.headGlowMask].get_Value(), sourceRect: bodyFrame3, color: drawinfo.headGlowColor, rotation: drawinfo.drawPlayer.headRotation, origin: headVect2, scale: 1f, effect: drawinfo.playerEffect);
									item.shader = drawinfo.cHead;
									drawinfo.DrawDataCache.Add(item);
								}
							}
							else
							{
								item = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame3, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
						if (drawinfo.drawPlayer.head == 211)
						{
							Color color4 = new Color(100, 100, 100, 0);
							ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4 + 100);
							int num17 = 4;
							for (int k = 0; k < num17; k++)
							{
								float x3 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
								float y = (float)Utils.RandomInt(ref seed, -14, 1) * 0.15f;
								item = new DrawData(TextureAssets.GlowMask[241].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + new Vector2(x3, y), drawinfo.drawPlayer.bodyFrame, color4, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
								item.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item);
							}
						}
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.face < 0 || !ArmorIDs.Face.Sets.PreventHairDraw[drawinfo.drawPlayer.face]))
			{
				DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.beard > 0 && (drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head]))
			{
				Vector2 beardDrawOffsetFromHelmet = drawinfo.drawPlayer.GetBeardDrawOffsetFromHelmet();
				Color color5 = drawinfo.colorArmorHead;
				if (ArmorIDs.Beard.Sets.UseHairColor[drawinfo.drawPlayer.beard])
				{
					color5 = drawinfo.colorHair;
				}
				DrawData item = new DrawData(TextureAssets.AccBeard[drawinfo.drawPlayer.beard].get_Value(), beardDrawOffsetFromHelmet + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color5, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBeard;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.head == 205)
			{
				DrawData drawData = new DrawData(TextureAssets.Extra[77].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.head == 214 && !drawinfo.drawPlayer.invis)
			{
				Rectangle bodyFrame4 = drawinfo.drawPlayer.bodyFrame;
				bodyFrame4.Y = 0;
				float num18 = (float)drawinfo.drawPlayer.miscCounter / 300f;
				Color color6 = new Color(0, 0, 0, 0);
				float num19 = 0.8f;
				float num20 = 0.9f;
				if (num18 >= num19)
				{
					color6 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(num19, num20, num18, clamped: true));
				}
				if (num18 >= num20)
				{
					color6 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(1f, num20, num18, clamped: true));
				}
				color6 *= drawinfo.stealth * (1f - drawinfo.shadow);
				DrawData item = new DrawData(TextureAssets.Extra[90].get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height], bodyFrame4, color6, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.head == 137)
			{
				DrawData item = new DrawData(TextureAssets.JackHat.get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, new Color(255, 255, 255, 255), drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				for (int l = 0; l < 7; l++)
				{
					Color color7 = new Color(110 - l * 10, 110 - l * 10, 110 - l * 10, 110 - l * 10);
					Vector2 vector5 = new Vector2((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
					vector5.X = drawinfo.drawPlayer.itemFlamePos[l].X;
					vector5.Y = drawinfo.drawPlayer.itemFlamePos[l].Y;
					vector5 *= 0.5f;
					item = new DrawData(TextureAssets.JackHat.get_Value(), helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector5, drawinfo.drawPlayer.bodyFrame, color7, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
			if (drawinfo.drawPlayer.babyBird)
			{
				Rectangle bodyFrame5 = drawinfo.drawPlayer.bodyFrame;
				bodyFrame5.Y = 0;
				Vector2 vector6 = Vector2.Zero;
				Color color8 = drawinfo.colorArmorHead;
				if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
				{
					Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
					color8 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
					vector6 = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
				}
				DrawData item = new DrawData(TextureAssets.Extra[100].get_Value(), vector6 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, bodyFrame5, color8, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static int DrawPlayer_Head_GetTVScreen(Player plr)
		{
			if (NPC.AnyDanger())
			{
				return 4;
			}
			if (plr.statLife <= plr.statLifeMax2 / 4)
			{
				return 1;
			}
			if (plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneGraveyard)
			{
				return 0;
			}
			if (plr.wet)
			{
				return 2;
			}
			if (plr.townNPCs >= 3f || BirthdayParty.PartyIsUp || LanternNight.LanternsUp)
			{
				return 5;
			}
			return 3;
		}

		private static int GetHatStacks(ref PlayerDrawSet drawinfo, int hatItemId)
		{
			int num = 0;
			int num2 = 0;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			num2 = 10;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			if (num > 2)
			{
				num = 2;
			}
			return num;
		}

		private static Vector2 DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref PlayerDrawSet drawinfo, ref Vector2 helmetOffset, Vector2 hatOffset)
		{
			Vector2 vector = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.Directions;
			Vector2 vec = drawinfo.Position - Main.screenPosition + helmetOffset + new Vector2(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4) + hatOffset * drawinfo.drawPlayer.Directions + vector;
			vec = vec.Floor();
			vec += drawinfo.drawPlayer.headPosition + drawinfo.headVect;
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				vec.Y += 12f;
			}
			return vec.Floor();
		}

		private static void DrawPlayer_21_Head_TheFace(ref PlayerDrawSet drawinfo)
		{
			bool flag = drawinfo.drawPlayer.head == 38 || drawinfo.drawPlayer.head == 135 || drawinfo.drawPlayer.head == 269;
			if (!flag && drawinfo.drawPlayer.faceHead > 0 && drawinfo.drawPlayer.faceHead < 22)
			{
				Vector2 faceHeadOffsetFromHelmet = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet();
				DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + faceHeadOffsetFromHelmet, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFaceHead;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.drawPlayer.face <= 0 || !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
				{
					return;
				}
				float num = 0f;
				if (drawinfo.drawPlayer.face == 5)
				{
					sbyte faceHead = drawinfo.drawPlayer.faceHead;
					if ((uint)(faceHead - 10) <= 3u)
					{
						num = 2 * drawinfo.drawPlayer.direction;
					}
				}
				item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (!drawinfo.drawPlayer.invis && !flag)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 1].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 2].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				Asset<Texture2D> val = TextureAssets.Players[drawinfo.skinVar, 15];
				if (val.get_IsLoaded())
				{
					Vector2 vector = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
					vector.Y -= 2f;
					vector *= (float)(-drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt());
					Rectangle value = val.Frame(1, 3, 0, drawinfo.drawPlayer.eyeHelper.EyeFrameToShow);
					drawData = new DrawData(val.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector, value, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				if (drawinfo.drawPlayer.yoraiz0rDarkness)
				{
					drawData = new DrawData(TextureAssets.Extra[67].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
				{
					item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cFace;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_21_1_Magiluminescence(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.shadow == 0f && drawinfo.drawPlayer.neck == 11 && !drawinfo.hideEntirePlayer)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				Color value = new Color(140, 140, 35, 12);
				float amount = (float)(colorArmorBody.R + colorArmorBody.G + colorArmorBody.B) / 3f / 255f;
				value = Color.Lerp(value, Color.Transparent, amount);
				if (!(value == Color.Transparent))
				{
					DrawData item = new DrawData(TextureAssets.GlowMask[310].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, value, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cNeck;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_22_FaceAcc(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = Vector2.Zero;
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
			{
				vector = new Vector2(28f, -2f);
			}
			vector *= drawinfo.drawPlayer.Directions;
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < 22 && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
			{
				Vector2 vector2 = Vector2.Zero;
				if (drawinfo.drawPlayer.face == 19)
				{
					vector2 = new Vector2(0f, -6f) * drawinfo.drawPlayer.Directions;
				}
				DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), vector2 + vector + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.faceFlower > 0 && drawinfo.drawPlayer.faceFlower < 22)
			{
				DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceFlower].get_Value(), vector + drawinfo.helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFaceFlower;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawUnicornHorn)
			{
				DrawData item = new DrawData(TextureAssets.Extra[143].get_Value(), vector + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cUnicornHorn;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawAngelHalo)
			{
				Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(200, 200, 200, 150), drawinfo.shadow);
				immuneAlphaPure *= drawinfo.drawPlayer.stealth;
				Main.instance.LoadAccFace(7);
				DrawData item = new DrawData(TextureAssets.AccFace[7].get_Value(), vector + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, immuneAlphaPure, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cAngelHalo;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawTiedBalloons(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Type == 34)
			{
				Texture2D value = TextureAssets.Extra[141].get_Value();
				Vector2 vector = new Vector2(0f, 4f);
				Color colorMount = drawinfo.colorMount;
				int frameY = (int)(Main.GlobalTimeWrappedHourly * 3f + drawinfo.drawPlayer.position.X / 50f) % 3;
				Rectangle value2 = value.Frame(1, 3, 0, frameY);
				Vector2 origin = new Vector2(value2.Width / 2, value2.Height);
				float rotation = (0f - drawinfo.drawPlayer.velocity.X) * 0.1f - drawinfo.drawPlayer.fullRotation;
				DrawData item = new DrawData(value, drawinfo.drawPlayer.MountedCenter + vector - Main.screenPosition, value2, colorMount, rotation, origin, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawStarboardRainbowTrail(ref PlayerDrawSet drawinfo, Vector2 commonWingPosPreFloor, Vector2 dirsVec)
		{
			if (drawinfo.shadow != 0f)
			{
				return;
			}
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
			float num2 = 0f;
			for (int num3 = num; num3 > 0; num3--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
				num2 += Vector2.Distance(value2: drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1).Position, value1: advancedShadow.Position);
			}
			float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Main.instance.LoadProjectile(250);
			Texture2D value = TextureAssets.Projectile[250].get_Value();
			float x = 1.7f;
			Vector2 origin = new Vector2(value.Width / 2, value.Height / 2);
			Vector2 vector = new Vector2(drawinfo.drawPlayer.width, drawinfo.drawPlayer.height) / 2f;
			Color white = Color.White;
			white.A = 64;
			Vector2 vector2 = vector;
			vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(0f, -4f);
			if (dirsVec.Y < 0f)
			{
				vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(0f, 4f);
			}
			for (int num5 = num; num5 > 0; num5--)
			{
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num5);
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num5 - 1);
				Vector2 pos = advancedShadow2.Position + vector2 + advancedShadow2.HeadgearOffset;
				Vector2 pos2 = advancedShadow3.Position + vector2 + advancedShadow3.HeadgearOffset;
				pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
				pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
				float num6 = (pos2 - pos).ToRotation() - MathF.PI / 2f;
				num6 = MathF.PI / 2f * (float)drawinfo.drawPlayer.direction;
				float num7 = Math.Abs(pos2.X - pos.X);
				Vector2 scale = new Vector2(x, num7 / (float)value.Height);
				float num8 = 1f - (float)num5 / (float)num;
				num8 *= num8;
				num8 *= Utils.GetLerpValue(0f, 4f, num7, clamped: true);
				num8 *= 0.5f;
				num8 *= num8;
				Color color = white * num8 * num4;
				if (!(color == Color.Transparent))
				{
					DrawData item = new DrawData(value, pos - Main.screenPosition, null, color, num6, origin, scale, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
					for (float num9 = 0.25f; num9 < 1f; num9 += 0.25f)
					{
						item = new DrawData(value, Vector2.Lerp(pos, pos2, num9) - Main.screenPosition, null, color, num6, origin, scale, drawinfo.playerEffect);
						item.shader = drawinfo.cWings;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
		}

		public static void DrawMeowcartTrail(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Type == 33)
			{
				int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 20);
				float num2 = 0f;
				for (int num3 = num; num3 > 0; num3--)
				{
					EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
					num2 += Vector2.Distance(value2: drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1).Position, value1: advancedShadow.Position);
				}
				float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
				Main.instance.LoadProjectile(250);
				Texture2D value = TextureAssets.Projectile[250].get_Value();
				float x = 1.5f;
				Vector2 origin = new Vector2(value.Width / 2, 0f);
				Vector2 vector = new Vector2(drawinfo.drawPlayer.width, drawinfo.drawPlayer.height) / 2f;
				Vector2 vector2 = new Vector2(-drawinfo.drawPlayer.direction * 10, 15f);
				Color white = Color.White;
				white.A = 127;
				Vector2 vector3 = vector + vector2;
				vector3 = Vector2.Zero;
				Vector2 vector4 = drawinfo.drawPlayer.RotatedRelativePoint(drawinfo.drawPlayer.Center + vector3 + vector2) - drawinfo.drawPlayer.position;
				for (int num5 = num; num5 > 0; num5--)
				{
					EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num5);
					EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num5 - 1);
					Vector2 pos = advancedShadow2.Position + vector3;
					Vector2 pos2 = advancedShadow3.Position + vector3;
					pos += vector4;
					pos2 += vector4;
					pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
					pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
					float rotation = (pos2 - pos).ToRotation() - MathF.PI / 2f;
					float num6 = Vector2.Distance(pos, pos2);
					Vector2 scale = new Vector2(x, num6 / (float)value.Height);
					float num7 = 1f - (float)num5 / (float)num;
					num7 *= num7;
					Color color = white * num7 * num4;
					DrawData item = new DrawData(value, pos - Main.screenPosition, null, color, rotation, origin, scale, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_23_MountFront(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.mount.Active)
			{
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 2, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
				drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			}
		}

		public static void DrawPlayer_24_Pulley(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.pulley && drawinfo.drawPlayer.itemAnimation == 0)
			{
				if (drawinfo.drawPlayer.pulleyDir == 2)
				{
					int num = -25;
					int num2 = 0;
					float rotation = 0f;
					DrawData item = new DrawData(TextureAssets.Pulley.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num2 * drawinfo.drawPlayer.direction, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation, new Vector2(TextureAssets.Pulley.Width() / 2, TextureAssets.Pulley.Height() / 4), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				else
				{
					int num3 = -26;
					int num4 = 10;
					float rotation2 = 0.35f * (float)(-drawinfo.drawPlayer.direction);
					DrawData item = new DrawData(TextureAssets.Pulley.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num4 * drawinfo.drawPlayer.direction, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num3 * drawinfo.drawPlayer.gravDir)), new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation2, new Vector2(TextureAssets.Pulley.Width() / 2, TextureAssets.Pulley.Height() / 4), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_25_Shield(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.shield <= 0 || drawinfo.drawPlayer.shield >= 10)
			{
				return;
			}
			Vector2 zero = Vector2.Zero;
			if (drawinfo.drawPlayer.shieldRaised)
			{
				zero.Y -= 4f * drawinfo.drawPlayer.gravDir;
			}
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			Vector2 zero2 = Vector2.Zero;
			Vector2 bodyVect = drawinfo.bodyVect;
			if (bodyFrame.Width != TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value().Width)
			{
				bodyFrame.Width = TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value().Width;
				bodyVect.X += bodyFrame.Width - TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value().Width;
				if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
				{
					bodyVect.X = (float)bodyFrame.Width - bodyVect.X;
				}
			}
			DrawData item;
			if (drawinfo.drawPlayer.shieldRaised)
			{
				float num = (float)Math.Sin(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f));
				float x = 2.5f + 1.5f * num;
				Color colorArmorBody = drawinfo.colorArmorBody;
				colorArmorBody.A = 0;
				colorArmorBody *= 0.45f - num * 0.15f;
				for (float num2 = 0f; num2 < 4f; num2 += 1f)
				{
					item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value(), zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero + new Vector2(x, 0f).RotatedBy(num2 / 4f * (MathF.PI * 2f)), bodyFrame, colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cShield;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value(), zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cShield;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.shieldRaised)
			{
				Color colorArmorBody2 = drawinfo.colorArmorBody;
				float num3 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * MathF.PI);
				colorArmorBody2.A = (byte)((float)(int)colorArmorBody2.A * (0.5f + 0.5f * num3));
				colorArmorBody2 *= 0.5f + 0.5f * num3;
				item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value(), zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero, bodyFrame, colorArmorBody2, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cShield;
			}
			if (drawinfo.drawPlayer.shieldRaised && drawinfo.drawPlayer.shieldParryTimeLeft > 0)
			{
				float num4 = (float)drawinfo.drawPlayer.shieldParryTimeLeft / 20f;
				float num5 = 1.5f * num4;
				Vector2 vector = zero2 + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2) + zero;
				Color colorArmorBody3 = drawinfo.colorArmorBody;
				float num6 = 1f;
				Vector2 vector2 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition;
				Vector2 vector3 = vector - vector2;
				vector += vector3 * num5;
				num6 += num5;
				colorArmorBody3.A = (byte)((float)(int)colorArmorBody3.A * (1f - num4));
				colorArmorBody3 *= 1f - num4;
				item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].get_Value(), vector, bodyFrame, colorArmorBody3, drawinfo.drawPlayer.bodyRotation, bodyVect, num6, drawinfo.playerEffect);
				item.shader = drawinfo.cShield;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.mount.Cart)
			{
				drawinfo.DrawDataCache.Reverse(drawinfo.DrawDataCache.Count - 2, 2);
			}
		}

		public static void DrawPlayer_26_SolarShield(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.solarShields > 0 && drawinfo.shadow == 0f && !drawinfo.drawPlayer.dead)
			{
				Texture2D value = TextureAssets.Extra[61 + drawinfo.drawPlayer.solarShields - 1].get_Value();
				Color color = new Color(255, 255, 255, 127);
				float num = (drawinfo.drawPlayer.solarShieldPos[0] * new Vector2(1f, 0.5f)).ToRotation();
				if (drawinfo.drawPlayer.direction == -1)
				{
					num += MathF.PI;
				}
				num += MathF.PI / 50f * (float)drawinfo.drawPlayer.direction;
				DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.solarShieldPos[0], null, color, num, value.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_27_HeldItem(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.JustDroppedAnItem)
			{
				return;
			}
			if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
			{
				drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
			}
			Item heldItem = drawinfo.heldItem;
			int num = heldItem.type;
			if (drawinfo.drawPlayer.UsingBiomeTorches)
			{
				switch (num)
				{
				case 8:
					num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
					break;
				case 966:
					num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
					break;
				}
			}
			float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
			Main.instance.LoadItem(num);
			Texture2D value = TextureAssets.Item[num].get_Value();
			Vector2 position = new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y));
			Rectangle itemDrawFrame = drawinfo.drawPlayer.GetItemDrawFrame(num);
			drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
			if (num == 678)
			{
				drawinfo.itemColor = Color.White;
			}
			if (drawinfo.drawPlayer.shroomiteStealth && heldItem.ranged)
			{
				float num2 = drawinfo.drawPlayer.stealth;
				if ((double)num2 < 0.03)
				{
					num2 = 0.03f;
				}
				float num3 = (1f + num2 * 10f) / 11f;
				drawinfo.itemColor = new Color((byte)((float)(int)drawinfo.itemColor.R * num2), (byte)((float)(int)drawinfo.itemColor.G * num2), (byte)((float)(int)drawinfo.itemColor.B * num3), (byte)((float)(int)drawinfo.itemColor.A * num2));
			}
			if (drawinfo.drawPlayer.setVortex && heldItem.ranged)
			{
				float num4 = drawinfo.drawPlayer.stealth;
				if ((double)num4 < 0.03)
				{
					num4 = 0.03f;
				}
				_ = (1f + num4 * 10f) / 11f;
				drawinfo.itemColor = drawinfo.itemColor.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num4)));
			}
			bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
			bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
			if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
			{
				flag2 = false;
			}
			if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || !(flag || flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
			{
				return;
			}
			_ = drawinfo.drawPlayer.name;
			Color color = new Color(250, 250, 250, heldItem.alpha);
			Vector2 vector = Vector2.Zero;
			switch (num)
			{
			case 5094:
			case 5095:
				vector = new Vector2(4f, -4f) * drawinfo.drawPlayer.Directions;
				break;
			case 5096:
			case 5097:
				vector = new Vector2(6f, -6f) * drawinfo.drawPlayer.Directions;
				break;
			case 46:
			{
				float amount = Utils.Remap(drawinfo.itemColor.ToVector3().Length() / 1.731f, 0.3f, 0.5f, 1f, 0f);
				color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 127) * 0.7f, amount);
				break;
			}
			}
			if (num == 3823)
			{
				vector = new Vector2(7 * drawinfo.drawPlayer.direction, -7f * drawinfo.drawPlayer.gravDir);
			}
			if (num == 3827)
			{
				vector = new Vector2(13 * drawinfo.drawPlayer.direction, -13f * drawinfo.drawPlayer.gravDir);
				color = heldItem.GetAlpha(drawinfo.itemColor);
				color = Color.Lerp(color, Color.White, 0.6f);
				color.A = 66;
			}
			Vector2 origin = new Vector2((float)itemDrawFrame.Width * 0.5f - (float)itemDrawFrame.Width * 0.5f * (float)drawinfo.drawPlayer.direction, itemDrawFrame.Height);
			if (heldItem.useStyle == 9 && drawinfo.drawPlayer.itemAnimation > 0)
			{
				Vector2 vector2 = new Vector2(0.5f, 0.4f);
				if (heldItem.type == 5009 || heldItem.type == 5042)
				{
					vector2 = new Vector2(0.26f, 0.5f);
					if (drawinfo.drawPlayer.direction == -1)
					{
						vector2.X = 1f - vector2.X;
					}
				}
				origin = itemDrawFrame.Size() * vector2;
			}
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				origin.Y = (float)itemDrawFrame.Height - origin.Y;
			}
			origin += vector;
			float num5 = drawinfo.drawPlayer.itemRotation;
			if (heldItem.useStyle == 8)
			{
				ref float x = ref position.X;
				float num6 = x;
				_ = drawinfo.drawPlayer.direction;
				x = num6 - 0f;
				num5 -= MathF.PI / 2f * (float)drawinfo.drawPlayer.direction;
				origin.Y = 2f;
				origin.X += 2 * drawinfo.drawPlayer.direction;
			}
			if (num == 425 || num == 507)
			{
				if (drawinfo.drawPlayer.gravDir == 1f)
				{
					if (drawinfo.drawPlayer.direction == 1)
					{
						drawinfo.itemEffect = SpriteEffects.FlipVertically;
					}
					else
					{
						drawinfo.itemEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					}
				}
				else if (drawinfo.drawPlayer.direction == 1)
				{
					drawinfo.itemEffect = SpriteEffects.None;
				}
				else
				{
					drawinfo.itemEffect = SpriteEffects.FlipHorizontally;
				}
			}
			if ((num == 946 || num == 4707) && num5 != 0f)
			{
				position.Y -= 22f * drawinfo.drawPlayer.gravDir;
				num5 = -1.57f * (float)(-drawinfo.drawPlayer.direction) * drawinfo.drawPlayer.gravDir;
			}
			ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem);
			DrawData item;
			switch (num)
			{
			case 3476:
			{
				Texture2D value3 = TextureAssets.Extra[64].get_Value();
				Rectangle rectangle2 = value3.Frame(1, 9, 0, drawinfo.drawPlayer.miscCounter % 54 / 6);
				Vector2 vector4 = new Vector2(rectangle2.Width / 2 * drawinfo.drawPlayer.direction, 0f);
				Vector2 origin3 = rectangle2.Size() / 2f;
				item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor(), rectangle2, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				value3 = TextureAssets.GlowMask[195].get_Value();
				item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor(), rectangle2, new Color(250, 250, 250, heldItem.alpha) * 0.5f, drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			case 4049:
			{
				Texture2D value2 = TextureAssets.Extra[92].get_Value();
				Rectangle rectangle = value2.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter % 20 / 5);
				Vector2 vector3 = new Vector2(rectangle.Width / 2 * drawinfo.drawPlayer.direction, 0f);
				vector3 += new Vector2(-10 * drawinfo.drawPlayer.direction, 8f * drawinfo.drawPlayer.gravDir);
				Vector2 origin2 = rectangle.Size() / 2f;
				item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + vector3).Floor(), rectangle, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			case 3779:
			{
				Texture2D texture2D = value;
				Rectangle rectangle3 = texture2D.Frame();
				Vector2 vector5 = new Vector2(rectangle3.Width / 2 * drawinfo.drawPlayer.direction, 0f);
				Vector2 origin4 = rectangle3.Size() / 2f;
				float num7 = ((float)drawinfo.drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 1f + 0f;
				Color color2 = new Color(120, 40, 222, 0) * (num7 / 2f * 0.3f + 0.85f) * 0.5f;
				num7 = 2f;
				for (float num8 = 0f; num8 < 4f; num8 += 1f)
				{
					item = new DrawData(TextureAssets.GlowMask[218].get_Value(), (drawinfo.ItemLocation - Main.screenPosition + vector5).Floor() + (num8 * (MathF.PI / 2f)).ToRotationVector2() * num7, rectangle3, color2, drawinfo.drawPlayer.itemRotation, origin4, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(texture2D, (drawinfo.ItemLocation - Main.screenPosition + vector5).Floor(), rectangle3, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin4, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			}
			if (heldItem.useStyle == 5)
			{
				if (Item.staff[num])
				{
					float num9 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
					float num10 = 0f;
					float num11 = 0f;
					Vector2 origin5 = new Vector2(0f, itemDrawFrame.Height);
					if (num == 3210)
					{
						num10 = 8 * -drawinfo.drawPlayer.direction;
						num11 = 2 * (int)drawinfo.drawPlayer.gravDir;
					}
					if (num == 3870)
					{
						Vector2 vector6 = (drawinfo.drawPlayer.itemRotation + MathF.PI / 4f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
						num10 = (int)vector6.X;
						num11 = (int)vector6.Y;
					}
					if (num == 3787)
					{
						num11 = (int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num9));
					}
					if (num == 3209)
					{
						Vector2 vector7 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
						num10 = vector7.X;
						num11 = vector7.Y;
					}
					if (drawinfo.drawPlayer.gravDir == -1f)
					{
						if (drawinfo.drawPlayer.direction == -1)
						{
							num9 += 1.57f;
							origin5 = new Vector2(itemDrawFrame.Width, 0f);
							num10 -= (float)itemDrawFrame.Width;
						}
						else
						{
							num9 -= 1.57f;
							origin5 = Vector2.Zero;
						}
					}
					else if (drawinfo.drawPlayer.direction == -1)
					{
						origin5 = new Vector2(itemDrawFrame.Width, itemDrawFrame.Height);
						num10 -= (float)itemDrawFrame.Width;
					}
					item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num9, origin5, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
					if (num == 3870)
					{
						item = new DrawData(TextureAssets.GlowMask[238].get_Value(), new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num10), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num11)), itemDrawFrame, new Color(255, 255, 255, 127), num9, origin5, adjustedItemScale, drawinfo.itemEffect);
						drawinfo.DrawDataCache.Add(item);
					}
					return;
				}
				if (num == 5118)
				{
					float rotation = drawinfo.drawPlayer.itemRotation + 1.57f * (float)drawinfo.drawPlayer.direction;
					Vector2 vector8 = new Vector2((float)itemDrawFrame.Width * 0.5f, (float)itemDrawFrame.Height * 0.5f);
					Vector2 origin6 = new Vector2((float)itemDrawFrame.Width * 0.5f, itemDrawFrame.Height);
					Vector2 spinningpoint = new Vector2(10f, 4f) * drawinfo.drawPlayer.Directions;
					spinningpoint = spinningpoint.RotatedBy(drawinfo.drawPlayer.itemRotation);
					item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X + spinningpoint.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y + spinningpoint.Y)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), rotation, origin6, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
					return;
				}
				int num12 = 10;
				Vector2 vector9 = new Vector2(itemDrawFrame.Width / 2, itemDrawFrame.Height / 2);
				Vector2 vector10 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
				num12 = (int)vector10.X;
				vector9.Y = vector10.Y;
				Vector2 origin7 = new Vector2(-num12, itemDrawFrame.Height / 2);
				if (drawinfo.drawPlayer.direction == -1)
				{
					origin7 = new Vector2(itemDrawFrame.Width + num12, itemDrawFrame.Height / 2);
				}
				item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector9.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector9.Y)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				if (heldItem.color != default(Color))
				{
					item = new DrawData(value, new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector9.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector9.Y)), itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				if (heldItem.glowMask != -1)
				{
					item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].get_Value(), new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector9.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector9.Y)), itemDrawFrame, new Color(250, 250, 250, heldItem.alpha), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				if (num == 3788)
				{
					float num13 = ((float)drawinfo.drawPlayer.miscCounter / 75f * (MathF.PI * 2f)).ToRotationVector2().X * 1f + 0f;
					Color color3 = new Color(80, 40, 252, 0) * (num13 / 2f * 0.3f + 0.85f) * 0.5f;
					for (float num14 = 0f; num14 < 4f; num14 += 1f)
					{
						item = new DrawData(TextureAssets.GlowMask[220].get_Value(), new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector9.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector9.Y)) + (num14 * (MathF.PI / 2f) + drawinfo.drawPlayer.itemRotation).ToRotationVector2() * num13, null, color3, drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
						drawinfo.DrawDataCache.Add(item);
					}
				}
				return;
			}
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				item = new DrawData(value, position, itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				if (heldItem.color != default(Color))
				{
					item = new DrawData(value, position, itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				if (heldItem.glowMask != -1)
				{
					item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].get_Value(), position, itemDrawFrame, new Color(250, 250, 250, heldItem.alpha), num5, origin, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			item = new DrawData(value, position, itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			if (heldItem.color != default(Color))
			{
				item = new DrawData(value, position, itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), num5, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (heldItem.glowMask != -1)
			{
				item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].get_Value(), position, itemDrawFrame, color, num5, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (!heldItem.flame || drawinfo.shadow != 0f)
			{
				return;
			}
			try
			{
				Main.instance.LoadItemFlames(num);
				if (TextureAssets.ItemFlame[num].get_IsLoaded())
				{
					Color color4 = new Color(100, 100, 100, 0);
					int num15 = 7;
					float num16 = 1f;
					float num17 = 0f;
					switch (num)
					{
					case 3045:
						color4 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
						break;
					case 5293:
						color4 = new Color(50, 50, 100, 20);
						break;
					case 5353:
						color4 = new Color(255, 255, 255, 200);
						break;
					case 4952:
						num15 = 3;
						num16 = 0.6f;
						color4 = new Color(50, 50, 50, 0);
						break;
					case 5322:
						color4 = new Color(100, 100, 100, 150);
						num17 = -2 * drawinfo.drawPlayer.direction;
						break;
					}
					for (int i = 0; i < num15; i++)
					{
						float num18 = drawinfo.drawPlayer.itemFlamePos[i].X * adjustedItemScale * num16;
						float num19 = drawinfo.drawPlayer.itemFlamePos[i].Y * adjustedItemScale * num16;
						item = new DrawData(TextureAssets.ItemFlame[num].get_Value(), new Vector2((int)(position.X + num18 + num17), (int)(position.Y + num19)), itemDrawFrame, color4, num5, origin, adjustedItemScale, drawinfo.itemEffect);
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
			catch
			{
			}
		}

		public static void DrawPlayer_28_ArmOverItem(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.usesCompositeTorso)
			{
				DrawPlayer_28_ArmOverItemComposite(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < 248)
			{
				Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
				int num = drawinfo.armorAdjust;
				bodyFrame.X += num;
				bodyFrame.Width -= num;
				if (drawinfo.drawPlayer.direction == -1)
				{
					num = 0;
				}
				if (drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.body == 21 || drawinfo.drawPlayer.body == 22))
				{
					return;
				}
				DrawData item;
				if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
				{
					_ = drawinfo.drawPlayer.body;
					DrawData drawData;
					if (drawinfo.missingArm)
					{
						drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
						drawData.shader = drawinfo.skinDyePacked;
						item = drawData;
						drawinfo.DrawDataCache.Add(item);
					}
					drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(TextureAssets.ArmorArm[drawinfo.drawPlayer.body].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
				if (drawinfo.armGlowMask != -1)
				{
					item = new DrawData(TextureAssets.GlowMask[drawinfo.armGlowMask].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), bodyFrame, drawinfo.armGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
				}
				if (drawinfo.drawPlayer.body == 205)
				{
					Color color = new Color(100, 100, 100, 0);
					ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
					int num2 = 4;
					for (int i = 0; i < num2; i++)
					{
						float num3 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
						float num4 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
						item = new DrawData(TextureAssets.GlowMask[240].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + num3, (float)(drawinfo.drawPlayer.bodyFrame.Height / 2) + num4), bodyFrame, color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cBody;
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
				item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_28_ArmOverItemComposite(ref PlayerDrawSet drawinfo)
		{
			Vector2 vector = new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * -drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically).ToDirectionInt();
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
			Vector2 bodyVect = drawinfo.bodyVect;
			Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
			bodyVect += compositeOffset_FrontArm;
			vector += compositeOffset_FrontArm;
			Vector2 position = vector + drawinfo.frontShoulderOffset;
			if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
			{
				vector += new Vector2((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1), (!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1));
			}
			_ = drawinfo.drawPlayer.invis;
			bool num = drawinfo.drawPlayer.body > 0 && drawinfo.drawPlayer.body < 248;
			int num2 = (drawinfo.compShoulderOverFrontArm ? 1 : 0);
			int num3 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			int num4 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
			bool flag = !drawinfo.hidesTopSkin;
			if (num)
			{
				if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
				{
					Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].get_Value();
					for (int i = 0; i < 2; i++)
					{
						if (!drawinfo.drawPlayer.invis && i == num4 && flag)
						{
							if (drawinfo.missingArm)
							{
								drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
								{
									shader = drawinfo.skinDyePacked
								});
							}
							if (drawinfo.missingHand)
							{
								drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
								{
									shader = drawinfo.skinDyePacked
								});
							}
						}
						if (i == num2 && !drawinfo.hideCompositeShoulders)
						{
							DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
						if (i == num3)
						{
							DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.cBody
							});
						}
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis)
			{
				for (int j = 0; j < 2; j++)
				{
					if (j == num2)
					{
						if (flag)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), position, drawinfo.compFrontShoulderFrame, drawinfo.colorBodySkin, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].get_Value(), position, drawinfo.compFrontShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].get_Value(), position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
						if (drawinfo.drawPlayer.head == 269)
						{
							Vector2 position2 = drawinfo.helmetOffset + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
							DrawData item = new DrawData(TextureAssets.Extra[214].get_Value(), position2, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
							item = new DrawData(TextureAssets.GlowMask[308].get_Value(), position2, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item);
						}
					}
					if (j == num3)
					{
						if (flag)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].get_Value(), vector, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					}
				}
			}
			if (drawinfo.drawPlayer.handon > 0 && drawinfo.drawPlayer.handon < 24)
			{
				Texture2D value2 = TextureAssets.AccHandsOnComposite[drawinfo.drawPlayer.handon].get_Value();
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArmAccessory, new DrawData(value2, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cHandOn
				});
			}
		}

		public static void DrawPlayer_29_OnhandAcc(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.usesCompositeFrontHandAcc && drawinfo.drawPlayer.handon > 0 && drawinfo.drawPlayer.handon < 24)
			{
				DrawData item = new DrawData(TextureAssets.AccHandsOn[drawinfo.drawPlayer.handon].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHandOn;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_30_BladedGlove(ref PlayerDrawSet drawinfo)
		{
			Item heldItem = drawinfo.heldItem;
			if (heldItem.type <= -1 || !Item.claw[heldItem.type] || drawinfo.shadow != 0f)
			{
				return;
			}
			Main.instance.LoadItem(heldItem.type);
			Asset<Texture2D> val = TextureAssets.Item[heldItem.type];
			if (!drawinfo.drawPlayer.frozen && (drawinfo.drawPlayer.itemAnimation > 0 || (heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley)) && heldItem.type > 0 && !drawinfo.drawPlayer.dead && !heldItem.noUseGraphic && (!drawinfo.drawPlayer.wet || !heldItem.noWet))
			{
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					DrawData item = new DrawData(val.get_Value(), new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)), new Rectangle(0, 0, val.Width(), val.Height()), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)val.Width() * 0.5f - (float)val.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, 0f), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				else
				{
					DrawData item = new DrawData(val.get_Value(), new Vector2((int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)), new Rectangle(0, 0, val.Width(), val.Height()), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)val.Width() * 0.5f - (float)val.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, val.Height()), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_31_ProjectileOverArm(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && drawinfo.heldProjOverHand)
			{
				drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
			}
		}

		public static void DrawPlayer_32_FrontAcc(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front > 0 && drawinfo.drawPlayer.front < 13 && !drawinfo.drawPlayer.mount.Active)
			{
				Vector2 zero = Vector2.Zero;
				DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].get_Value(), zero + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_32_FrontAcc_FrontPart(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front <= 0 || drawinfo.drawPlayer.front >= 13)
			{
				return;
			}
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = bodyFrame.Width / 2;
			bodyFrame.Width -= num;
			Vector2 bodyVect = drawinfo.bodyVect;
			if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
			{
				bodyVect.X -= num;
			}
			Vector2 vector = Vector2.Zero + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
			DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].get_Value(), vector, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFront;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.front == 12)
			{
				Rectangle rectangle = bodyFrame;
				Rectangle value = rectangle;
				value.Width = 2;
				int num2 = 0;
				int num3 = rectangle.Width / 2;
				int num4 = 2;
				if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
				{
					num2 = rectangle.Width - 2;
					num4 = -2;
				}
				for (int i = 0; i < num3; i++)
				{
					value.X = rectangle.X + 2 * i;
					Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
					immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
					item = new DrawData(TextureAssets.GlowMask[331].get_Value(), vector + new Vector2(num2 + i * num4, 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cFront;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_32_FrontAcc_BackPart(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.front <= 0 || drawinfo.drawPlayer.front >= 13)
			{
				return;
			}
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = bodyFrame.Width / 2;
			bodyFrame.Width -= num;
			bodyFrame.X += num;
			Vector2 bodyVect = drawinfo.bodyVect;
			if (!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
			{
				bodyVect.X -= num;
			}
			Vector2 vector = Vector2.Zero + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2);
			DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].get_Value(), vector, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFront;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.front == 12)
			{
				Rectangle rectangle = bodyFrame;
				Rectangle value = rectangle;
				value.Width = 2;
				int num2 = 0;
				int num3 = rectangle.Width / 2;
				int num4 = 2;
				if (drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally))
				{
					num2 = rectangle.Width - 2;
					num4 = -2;
				}
				for (int i = 0; i < num3; i++)
				{
					value.X = rectangle.X + 2 * i;
					Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
					immuneAlpha *= (float)(int)drawinfo.colorArmorBody.A / 255f;
					item = new DrawData(TextureAssets.GlowMask[331].get_Value(), vector + new Vector2(num2 + i * num4, 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cFront;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_33_FrozenOrWebbedDebuff(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.shimmering)
			{
				if (drawinfo.drawPlayer.frozen && drawinfo.shadow == 0f)
				{
					Color colorArmorBody = drawinfo.colorArmorBody;
					colorArmorBody.R = (byte)((double)(int)colorArmorBody.R * 0.55);
					colorArmorBody.G = (byte)((double)(int)colorArmorBody.G * 0.55);
					colorArmorBody.B = (byte)((double)(int)colorArmorBody.B * 0.55);
					colorArmorBody.A = (byte)((double)(int)colorArmorBody.A * 0.55);
					DrawData item = new DrawData(TextureAssets.Frozen.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), new Rectangle(0, 0, TextureAssets.Frozen.Width(), TextureAssets.Frozen.Height()), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2(TextureAssets.Frozen.Width() / 2, TextureAssets.Frozen.Height() / 2), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				else if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y == 0f)
				{
					Color color = drawinfo.colorArmorBody * 0.75f;
					Texture2D value = TextureAssets.Extra[31].get_Value();
					int num = drawinfo.drawPlayer.height / 2;
					DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + (float)num)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}

		public static void DrawPlayer_34_ElectrifiedDebuffFront(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
			{
				return;
			}
			Texture2D value = TextureAssets.GlowMask[25].get_Value();
			int num = drawinfo.drawPlayer.miscCounter / 5;
			for (int i = 0; i < 2; i++)
			{
				num %= 7;
				if (num > 1 && num < 5)
				{
					DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2(value.Width / 2, value.Height / 14), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				num += 3;
			}
		}

		public static void DrawPlayer_35_IceBarrier(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.iceBarrier && drawinfo.shadow == 0f)
			{
				int num = TextureAssets.IceBarrier.Height() / 12;
				Color white = Color.White;
				DrawData item = new DrawData(TextureAssets.IceBarrier.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2(drawinfo.drawPlayer.bodyFrame.Width / 2, drawinfo.drawPlayer.bodyFrame.Height / 2), new Rectangle(0, num * drawinfo.drawPlayer.iceBarrierFrame, TextureAssets.IceBarrier.Width(), num), white, 0f, new Vector2(TextureAssets.Frozen.Width() / 2, TextureAssets.Frozen.Height() / 2), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_36_CTG(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.shadow != 0f || (byte)drawinfo.drawPlayer.ownedLargeGems <= 0)
			{
				return;
			}
			bool flag = false;
			BitsByte ownedLargeGems = drawinfo.drawPlayer.ownedLargeGems;
			float num = 0f;
			for (int i = 0; i < 7; i++)
			{
				if (ownedLargeGems[i])
				{
					num += 1f;
				}
			}
			float num2 = 1f - num * 0.06f;
			float num3 = (num - 1f) * 4f;
			switch ((int)num)
			{
			case 2:
				num3 += 10f;
				break;
			case 3:
				num3 += 8f;
				break;
			case 4:
				num3 += 6f;
				break;
			case 5:
				num3 += 6f;
				break;
			case 6:
				num3 += 2f;
				break;
			case 7:
				num3 += 0f;
				break;
			}
			float num4 = (float)drawinfo.drawPlayer.miscCounter / 300f * (MathF.PI * 2f);
			if (!(num > 0f))
			{
				return;
			}
			float num5 = MathF.PI * 2f / num;
			float num6 = 0f;
			Vector2 vector = new Vector2(1.3f, 0.65f);
			if (!flag)
			{
				vector = Vector2.One;
			}
			List<DrawData> list = new List<DrawData>();
			for (int j = 0; j < 7; j++)
			{
				if (!ownedLargeGems[j])
				{
					num6 += 1f;
					continue;
				}
				Vector2 vector2 = (num4 + num5 * ((float)j - num6)).ToRotationVector2();
				float num7 = num2;
				if (flag)
				{
					num7 = MathHelper.Lerp(num2 * 0.7f, 1f, vector2.Y / 2f + 0.5f);
				}
				Texture2D value = TextureAssets.Gem[j].get_Value();
				DrawData item = new DrawData(value, new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - 80f)) + vector2 * vector * num3, null, new Color(250, 250, 250, (int)Main.mouseTextColor / 2), 0f, value.Size() / 2f, ((float)(int)Main.mouseTextColor / 1000f + 0.8f) * num7, SpriteEffects.None);
				list.Add(item);
			}
			if (flag)
			{
				list.Sort(DelegateMethods.CompareDrawSorterByYScale);
			}
			drawinfo.DrawDataCache.AddRange(list);
		}

		public static void DrawPlayer_37_BeetleBuff(ref PlayerDrawSet drawinfo)
		{
			if ((!drawinfo.drawPlayer.beetleOffense && !drawinfo.drawPlayer.beetleDefense) || drawinfo.shadow != 0f)
			{
				return;
			}
			for (int i = 0; i < drawinfo.drawPlayer.beetleOrbs; i++)
			{
				DrawData item;
				for (int j = 0; j < 5; j++)
				{
					Color colorArmorBody = drawinfo.colorArmorBody;
					float num = (float)j * 0.1f;
					num = 0.5f - num;
					colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num);
					colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num);
					colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num);
					colorArmorBody.A = (byte)((float)(int)colorArmorBody.A * num);
					Vector2 vector = -drawinfo.drawPlayer.beetleVel[i] * j;
					item = new DrawData(TextureAssets.Beetle.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i] + vector, new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), colorArmorBody, 0f, new Vector2(TextureAssets.Beetle.Width() / 2, TextureAssets.Beetle.Height() / 6), 1f, drawinfo.playerEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(TextureAssets.Beetle.get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i], new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), drawinfo.colorArmorBody, 0f, new Vector2(TextureAssets.Beetle.Width() / 2, TextureAssets.Beetle.Height() / 6), 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}

		public static void DrawPlayer_38_EyebrellaCloud(ref PlayerDrawSet drawinfo)
		{
			if (drawinfo.drawPlayer.eyebrellaCloud && drawinfo.shadow == 0f)
			{
				Texture2D value = TextureAssets.Projectile[238].get_Value();
				int frameY = drawinfo.drawPlayer.miscCounter % 18 / 6;
				Rectangle value2 = value.Frame(1, 6, 0, frameY);
				Vector2 origin = new Vector2(value2.Width / 2, value2.Height / 2);
				Vector2 vector = new Vector2(0f, -70f);
				Vector2 vector2 = drawinfo.drawPlayer.MountedCenter - new Vector2(0f, (float)drawinfo.drawPlayer.height * 0.5f) + vector - Main.screenPosition;
				Color color = Lighting.GetColor((drawinfo.drawPlayer.Top + new Vector2(0f, -30f)).ToTileCoordinates());
				int num = 170;
				int g;
				int b;
				int r = (g = (b = num));
				if (color.R < num)
				{
					r = color.R;
				}
				if (color.G < num)
				{
					g = color.G;
				}
				if (color.B < num)
				{
					b = color.B;
				}
				Color color2 = new Color(r, g, b, 100);
				float num2 = (float)(drawinfo.drawPlayer.miscCounter % 50) / 50f;
				float num3 = 3f;
				DrawData item;
				for (int i = 0; i < 2; i++)
				{
					Vector2 vector3 = new Vector2((i == 0) ? (0f - num3) : num3, 0f).RotatedBy(num2 * (MathF.PI * 2f) * ((i == 0) ? 1f : (-1f)));
					item = new DrawData(value, vector2 + vector3, value2, color2 * 0.65f, 0f, origin, 1f, (drawinfo.drawPlayer.gravDir == -1f) ? SpriteEffects.FlipVertically : SpriteEffects.None);
					item.shader = drawinfo.cHead;
					item.ignorePlayerRotation = true;
					drawinfo.DrawDataCache.Add(item);
				}
				item = new DrawData(value, vector2, value2, color2, 0f, origin, 1f, (drawinfo.drawPlayer.gravDir == -1f) ? SpriteEffects.FlipVertically : SpriteEffects.None);
				item.shader = drawinfo.cHead;
				item.ignorePlayerRotation = true;
				drawinfo.DrawDataCache.Add(item);
			}
		}

		private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2(6 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 2 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipVertically)) ? 1 : (-1)));
		}

		private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
		{
			return new Vector2(-5 * ((!drawinfo.playerEffect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1)), 0f);
		}

		public static void DrawPlayer_TransformDrawData(ref PlayerDrawSet drawinfo)
		{
			_ = drawinfo.rotation;
			_ = 0f;
			Vector2 vector = drawinfo.Position - Main.screenPosition + drawinfo.rotationOrigin;
			Vector2 vector2 = drawinfo.drawPlayer.position + drawinfo.rotationOrigin;
			Matrix matrix = Matrix.CreateRotationZ(drawinfo.rotation);
			for (int i = 0; i < drawinfo.DustCache.Count; i++)
			{
				Vector2 position = Main.dust[drawinfo.DustCache[i]].position - vector2;
				position = Vector2.Transform(position, matrix);
				Main.dust[drawinfo.DustCache[i]].position = position + vector2;
			}
			for (int j = 0; j < drawinfo.GoreCache.Count; j++)
			{
				Vector2 position2 = Main.gore[drawinfo.GoreCache[j]].position - vector2;
				position2 = Vector2.Transform(position2, matrix);
				Main.gore[drawinfo.GoreCache[j]].position = position2 + vector2;
			}
			for (int k = 0; k < drawinfo.DrawDataCache.Count; k++)
			{
				DrawData value = drawinfo.DrawDataCache[k];
				if (!value.ignorePlayerRotation)
				{
					Vector2 position3 = value.position - vector;
					position3 = Vector2.Transform(position3, matrix);
					value.position = position3 + vector;
					value.rotation += drawinfo.rotation;
					drawinfo.DrawDataCache[k] = value;
				}
			}
		}

		public static void DrawPlayer_ScaleDrawData(ref PlayerDrawSet drawinfo, float scale)
		{
			if (scale != 1f)
			{
				Vector2 vector = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 1f) - Main.screenPosition;
				for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
				{
					DrawData value = drawinfo.DrawDataCache[i];
					Vector2 vector2 = value.position - vector;
					value.position = vector + vector2 * scale;
					value.scale *= scale;
					drawinfo.DrawDataCache[i] = value;
				}
			}
		}

		public static void DrawPlayer_AddSelectionGlow(ref PlayerDrawSet drawinfo)
		{
			if (!(drawinfo.selectionGlowColor == Color.Transparent))
			{
				Color selectionGlowColor = drawinfo.selectionGlowColor;
				List<DrawData> list = new List<DrawData>();
				list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, -2f), selectionGlowColor));
				list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, 2f), selectionGlowColor));
				list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(2f, 0f), selectionGlowColor));
				list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(-2f, 0f), selectionGlowColor));
				list.AddRange(drawinfo.DrawDataCache);
				drawinfo.DrawDataCache = list;
			}
		}

		public static void DrawPlayer_MakeIntoFirstFractalAfterImage(ref PlayerDrawSet drawinfo)
		{
			if (!drawinfo.drawPlayer.isFirstFractalAfterImage)
			{
				if (drawinfo.drawPlayer.HeldItem.type == 4722)
				{
					_ = drawinfo.drawPlayer.itemAnimation > 0;
				}
				else
					_ = 0;
				return;
			}
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData value = drawinfo.DrawDataCache[i];
				value.color *= drawinfo.drawPlayer.firstFractalAfterImageOpacity;
				value.color.A = (byte)((float)(int)value.color.A * 0.8f);
				drawinfo.DrawDataCache[i] = value;
			}
		}

		public static void DrawPlayer_RenderAllLayers(ref PlayerDrawSet drawinfo)
		{
			int num = -1;
			List<DrawData> drawDataCache = drawinfo.DrawDataCache;
			Effect pixelShader = Main.pixelShader;
			Projectile[] projectile = Main.projectile;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i <= drawDataCache.Count; i++)
			{
				if (drawinfo.projectileDrawPosition == i)
				{
					if (!ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[projectile[drawinfo.drawPlayer.heldProj].type])
					{
						projectile[drawinfo.drawPlayer.heldProj].gfxOffY = drawinfo.drawPlayer.gfxOffY;
					}
					if (num != 0)
					{
						pixelShader.CurrentTechnique.Passes[0].Apply();
						num = 0;
					}
					try
					{
						Main.instance.DrawProj(drawinfo.drawPlayer.heldProj);
					}
					catch
					{
						projectile[drawinfo.drawPlayer.heldProj].active = false;
					}
				}
				if (i != drawDataCache.Count)
				{
					DrawData cdd = drawDataCache[i];
					if (!cdd.sourceRect.HasValue)
					{
						cdd.sourceRect = cdd.texture.Frame();
					}
					PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
					num = cdd.shader;
					if (cdd.texture != null)
					{
						cdd.Draw(spriteBatch);
					}
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawSet drawinfo)
		{
			SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawDataCache, out var lowest, out var highest);
			Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
		}

		private static bool IsArmorDrawnWhenInvisible(int torsoID)
		{
			if ((uint)(torsoID - 21) <= 1u)
			{
				return false;
			}
			return true;
		}

		private static DrawData[] GetFlatColoredCloneData(ref PlayerDrawSet drawinfo, Vector2 offset, Color color)
		{
			int colorOnlyShaderIndex = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
			DrawData[] array = new DrawData[drawinfo.DrawDataCache.Count];
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData drawData = drawinfo.DrawDataCache[i];
				drawData.position += offset;
				drawData.shader = colorOnlyShaderIndex;
				drawData.color = color;
				array[i] = drawData;
			}
			return array;
		}
	}
}
