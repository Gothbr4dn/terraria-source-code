using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;

namespace Terraria.DataStructures
{
	public static class PlayerDrawHeadLayers
	{
		public static void DrawPlayer_0_(ref PlayerDrawHeadSet drawinfo)
		{
		}

		public static void DrawPlayer_00_BackHelmet(ref PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head >= 0 && drawinfo.drawPlayer.head < 282)
			{
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
				if (num >= 0)
				{
					Rectangle hairFrame = drawinfo.HairFrame;
					QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[num].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_01_FaceSkin(ref PlayerDrawHeadSet drawinfo)
		{
			bool flag = drawinfo.drawPlayer.head == 38 || drawinfo.drawPlayer.head == 135 || drawinfo.drawPlayer.head == 269;
			if (!flag && drawinfo.drawPlayer.faceHead > 0 && drawinfo.drawPlayer.faceHead < 22)
			{
				Vector2 faceHeadOffsetFromHelmet = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet();
				QuickCDD(drawinfo.DrawData, drawinfo.cFaceHead, TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + faceHeadOffsetFromHelmet, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				if (drawinfo.drawPlayer.face <= 0 || drawinfo.drawPlayer.face >= 22 || !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
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
				QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			else if (!flag && !drawinfo.drawPlayer.isHatRackDoll)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Players[drawinfo.skinVar, 0].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 1].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				QuickCDD(drawinfo.DrawData, TextureAssets.Players[drawinfo.skinVar, 2].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				if (drawinfo.drawPlayer.yoraiz0rDarkness)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.skinDyePacked, TextureAssets.Extra[67].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
				if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < 22 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
				{
					QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_02_DrawArmorWithFullHair(ref PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.fullHair)
			{
				return;
			}
			Color color = drawinfo.colorArmorHead;
			int shaderTechnique = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shaderTechnique = drawinfo.skinDyePacked;
			}
			QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.HairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			if (!drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_03_HelmetHair(ref PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.hideHair && drawinfo.hatHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				if (!drawinfo.drawPlayer.invis)
				{
					QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_04_CapricornMask(ref PlayerDrawHeadSet drawinfo)
		{
			Rectangle hairFrame = drawinfo.HairFrame;
			hairFrame.Width += 2;
			QuickCDD(drawinfo.DrawData, drawinfo.cHead, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
		}

		public static void DrawPlayer_04_RabbitOrder(ref PlayerDrawHeadSet drawinfo)
		{
			int verticalFrames = 27;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value();
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = rectangle.Size() / 2f;
			int usedGravDir = 1;
			Vector2 vector = DrawPlayer_04_GetHatDrawPosition(ref drawinfo, new Vector2(1f, -26f), usedGravDir);
			int hatStacks = GetHatStacks(ref drawinfo, 4955);
			float num = MathF.PI / 60f;
			float num2 = num * drawinfo.drawPlayer.position.X % (MathF.PI * 2f);
			for (int num3 = hatStacks - 1; num3 >= 0; num3--)
			{
				float x = Vector2.UnitY.RotatedBy(num2 + num * (float)num3).X * ((float)num3 / 30f) * 2f - (float)(num3 * 2 * drawinfo.drawPlayer.direction);
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, vector + new Vector2(x, (float)(num3 * -14) * drawinfo.scale), rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (!drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_04_BadgersHat(ref PlayerDrawHeadSet drawinfo)
		{
			int verticalFrames = 6;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value();
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = rectangle.Size() / 2f;
			int num = 1;
			Vector2 vector = DrawPlayer_04_GetHatDrawPosition(ref drawinfo, new Vector2(0f, -9f), num);
			int hatStacks = GetHatStacks(ref drawinfo, 5004);
			float num2 = MathF.PI / 60f;
			float num3 = num2 * drawinfo.drawPlayer.position.X % (MathF.PI * 2f);
			int num4 = hatStacks * 4 + 2;
			int num5 = 0;
			bool flag = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
			for (int num6 = num4 - 1; num6 >= 0; num6--)
			{
				int num7 = 0;
				if (num6 == num4 - 1)
				{
					rectangle.Y = 0;
					num7 = 2;
				}
				else if (num6 == 0)
				{
					rectangle.Y = rectangle.Height * 5;
				}
				else
				{
					rectangle.Y = rectangle.Height * (num5++ % 4 + 1);
				}
				if (!(rectangle.Y == rectangle.Height * 3 && flag))
				{
					float x = Vector2.UnitY.RotatedBy(num3 + num2 * (float)num6).X * ((float)num6 / 10f) * 4f - (float)num6 * 0.1f * (float)drawinfo.drawPlayer.direction;
					QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, vector + new Vector2(x, (num6 * -4 + num7) * num) * drawinfo.scale, rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		private static Vector2 DrawPlayer_04_GetHatDrawPosition(ref PlayerDrawHeadSet drawinfo, Vector2 hatOffset, int usedGravDir)
		{
			Vector2 vector = new Vector2(drawinfo.drawPlayer.direction, usedGravDir);
			return drawinfo.Position - Main.screenPosition + new Vector2(-drawinfo.bodyFrameMemory.Width / 2 + drawinfo.drawPlayer.width / 2, drawinfo.drawPlayer.height - drawinfo.bodyFrameMemory.Height + 4) + hatOffset * vector * drawinfo.scale + (drawinfo.drawPlayer.headPosition + drawinfo.headVect);
		}

		private static int GetHatStacks(ref PlayerDrawHeadSet drawinfo, int itemId)
		{
			int num = 0;
			int num2 = 0;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == itemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			num2 = 10;
			if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == itemId && drawinfo.drawPlayer.armor[num2].stack > 0)
			{
				num += drawinfo.drawPlayer.armor[num2].stack;
			}
			if (num > 2)
			{
				num = 2;
			}
			return num;
		}

		public static void DrawPlayer_04_HatsWithFullHair(ref PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 259)
			{
				DrawPlayer_04_RabbitOrder(ref drawinfo);
			}
			else
			{
				if (!drawinfo.helmetIsOverFullHair)
				{
					return;
				}
				if (!drawinfo.hideHair)
				{
					Rectangle hairFrame = drawinfo.HairFrame;
					hairFrame.Y -= 336;
					if (hairFrame.Y < 0)
					{
						hairFrame.Y = 0;
					}
					QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
				if (drawinfo.drawPlayer.head != 0)
				{
					Color color = drawinfo.colorArmorHead;
					int shaderTechnique = drawinfo.cHead;
					if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
					{
						color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
						shaderTechnique = drawinfo.skinDyePacked;
					}
					QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				}
			}
		}

		public static void DrawPlayer_05_TallHats(ref PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.helmetIsTall)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				if (drawinfo.drawPlayer.head == 158)
				{
					hairFrame.Height -= 2;
				}
				int num = 0;
				if (hairFrame.Y == hairFrame.Height * 6)
				{
					hairFrame.Height -= 2;
				}
				else if (hairFrame.Y == hairFrame.Height * 7)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 8)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 9)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 10)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 13)
				{
					hairFrame.Height -= 2;
				}
				else if (hairFrame.Y == hairFrame.Height * 14)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 15)
				{
					num = -2;
				}
				else if (hairFrame.Y == hairFrame.Height * 16)
				{
					num = -2;
				}
				hairFrame.Y += num;
				Color color = drawinfo.colorArmorHead;
				int shaderTechnique = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
					shaderTechnique = drawinfo.skinDyePacked;
				}
				QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f + (float)num) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_06_NormalHats(ref PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.head == 270)
			{
				DrawPlayer_04_CapricornMask(ref drawinfo);
			}
			else if (drawinfo.drawPlayer.head == 265)
			{
				DrawPlayer_04_BadgersHat(ref drawinfo);
			}
			else
			{
				if (!drawinfo.helmetIsNormal)
				{
					return;
				}
				Rectangle hairFrame = drawinfo.HairFrame;
				Color color = drawinfo.colorArmorHead;
				int shaderTechnique = drawinfo.cHead;
				if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
				{
					color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
					shaderTechnique = drawinfo.skinDyePacked;
				}
				QuickCDD(drawinfo.DrawData, shaderTechnique, TextureAssets.ArmorHead[drawinfo.drawPlayer.head].get_Value(), drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, hairFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
				if (drawinfo.drawPlayer.head != 271)
				{
					return;
				}
				int num = PlayerDrawLayers.DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
				if (num == 0)
				{
					return;
				}
				Texture2D value = TextureAssets.GlowMask[309].get_Value();
				int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
				if (num == 5)
				{
					frameY = 0;
					if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
					{
						frameY = 2;
					}
				}
				Rectangle value2 = value.Frame(6, 4, num, frameY, -2);
				QuickCDD(drawinfo.DrawData, drawinfo.cHead, value, drawinfo.helmetOffset + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, value2, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_07_JustHair(ref PlayerDrawHeadSet drawinfo)
		{
			if (!drawinfo.helmetIsNormal && !drawinfo.helmetIsOverFullHair && !drawinfo.helmetIsTall && !drawinfo.hideHair)
			{
				Rectangle hairFrame = drawinfo.HairFrame;
				hairFrame.Y -= 336;
				if (hairFrame.Y < 0)
				{
					hairFrame.Y = 0;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.hairShaderPacked, TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset, hairFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_08_FaceAcc(ref PlayerDrawHeadSet drawinfo)
		{
			if (drawinfo.drawPlayer.beard > 0 && (drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head]))
			{
				Vector2 beardDrawOffsetFromHelmet = drawinfo.drawPlayer.GetBeardDrawOffsetFromHelmet();
				Color color = drawinfo.colorArmorHead;
				if (ArmorIDs.Beard.Sets.UseHairColor[drawinfo.drawPlayer.beard])
				{
					color = drawinfo.colorHair;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.cBeard, TextureAssets.AccBeard[drawinfo.drawPlayer.beard].get_Value(), beardDrawOffsetFromHelmet + new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawPlayer.face > 0 && drawinfo.drawPlayer.face < 22 && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
			{
				Vector2 vector = Vector2.Zero;
				if (drawinfo.drawPlayer.face == 19)
				{
					vector = new Vector2(0f, -6f) * drawinfo.drawPlayer.Directions;
				}
				QuickCDD(drawinfo.DrawData, drawinfo.cFace, TextureAssets.AccFace[drawinfo.drawPlayer.face].get_Value(), vector + new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawPlayer.faceFlower > 0 && drawinfo.drawPlayer.faceFlower < 22)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.cFaceFlower, TextureAssets.AccFace[drawinfo.drawPlayer.faceFlower].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawUnicornHorn)
			{
				QuickCDD(drawinfo.DrawData, drawinfo.cUnicornHorn, TextureAssets.Extra[143].get_Value(), new Vector2(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
			if (drawinfo.drawAngelHalo)
			{
				Main.instance.LoadAccFace(7);
				QuickCDD(drawinfo.DrawData, drawinfo.cAngelHalo, TextureAssets.AccFace[7].get_Value(), new Vector2((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.bodyFrameMemory.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.bodyFrameMemory.Height + 4f) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.bodyFrameMemory, new Color(200, 200, 200, 200), drawinfo.drawPlayer.headRotation, drawinfo.headVect, drawinfo.scale, drawinfo.playerEffect, 0f);
			}
		}

		public static void DrawPlayer_RenderAllLayers(ref PlayerDrawHeadSet drawinfo)
		{
			List<DrawData> drawData = drawinfo.DrawData;
			Effect pixelShader = Main.pixelShader;
			_ = Main.projectile;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < drawData.Count; i++)
			{
				DrawData cdd = drawData[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
				if (cdd.texture != null)
				{
					cdd.Draw(spriteBatch);
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawHeadSet drawinfo)
		{
			SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawData, out var lowest, out var highest);
			Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
		}

		public static void QuickCDD(List<DrawData> drawData, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			drawData.Add(new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects));
		}

		public static void QuickCDD(List<DrawData> drawData, int shaderTechnique, Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			DrawData item = new DrawData(texture, position, sourceRectangle, color, rotation, origin, scale, effects);
			item.shader = shaderTechnique;
			drawData.Add(item);
		}
	}
}
