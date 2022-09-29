using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Golf;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.DataStructures
{
	public struct PlayerDrawSet
	{
		public List<DrawData> DrawDataCache;

		public List<int> DustCache;

		public List<int> GoreCache;

		public Player drawPlayer;

		public float shadow;

		public Vector2 Position;

		public int projectileDrawPosition;

		public Vector2 ItemLocation;

		public int armorAdjust;

		public bool missingHand;

		public bool missingArm;

		public bool heldProjOverHand;

		public int skinVar;

		public bool fullHair;

		public bool drawsBackHairWithoutHeadgear;

		public bool hatHair;

		public bool hideHair;

		public int hairDyePacked;

		public int skinDyePacked;

		public float mountOffSet;

		public int cHead;

		public int cBody;

		public int cLegs;

		public int cHandOn;

		public int cHandOff;

		public int cBack;

		public int cFront;

		public int cShoe;

		public int cWaist;

		public int cShield;

		public int cNeck;

		public int cFace;

		public int cBalloon;

		public int cWings;

		public int cCarpet;

		public int cPortableStool;

		public int cFloatingTube;

		public int cUnicornHorn;

		public int cAngelHalo;

		public int cBeard;

		public int cLeinShampoo;

		public int cBackpack;

		public int cTail;

		public int cFaceHead;

		public int cFaceFlower;

		public int cBalloonFront;

		public SpriteEffects playerEffect;

		public SpriteEffects itemEffect;

		public Color colorHair;

		public Color colorEyeWhites;

		public Color colorEyes;

		public Color colorHead;

		public Color colorBodySkin;

		public Color colorLegs;

		public Color colorShirt;

		public Color colorUnderShirt;

		public Color colorPants;

		public Color colorShoes;

		public Color colorArmorHead;

		public Color colorArmorBody;

		public Color colorMount;

		public Color colorArmorLegs;

		public Color colorElectricity;

		public Color colorDisplayDollSkin;

		public int headGlowMask;

		public int bodyGlowMask;

		public int armGlowMask;

		public int legsGlowMask;

		public Color headGlowColor;

		public Color bodyGlowColor;

		public Color armGlowColor;

		public Color legsGlowColor;

		public Color ArkhalisColor;

		public float stealth;

		public Vector2 legVect;

		public Vector2 bodyVect;

		public Vector2 headVect;

		public Color selectionGlowColor;

		public float torsoOffset;

		public bool hidesTopSkin;

		public bool hidesBottomSkin;

		public float rotation;

		public Vector2 rotationOrigin;

		public Rectangle hairFrontFrame;

		public Rectangle hairBackFrame;

		public bool backHairDraw;

		public Color itemColor;

		public bool usesCompositeTorso;

		public bool usesCompositeFrontHandAcc;

		public bool usesCompositeBackHandAcc;

		public bool compShoulderOverFrontArm;

		public Rectangle compBackShoulderFrame;

		public Rectangle compFrontShoulderFrame;

		public Rectangle compBackArmFrame;

		public Rectangle compFrontArmFrame;

		public Rectangle compTorsoFrame;

		public float compositeBackArmRotation;

		public float compositeFrontArmRotation;

		public bool hideCompositeShoulders;

		public Vector2 frontShoulderOffset;

		public Vector2 backShoulderOffset;

		public WeaponDrawOrder weaponDrawOrder;

		public bool weaponOverFrontArm;

		public bool isSitting;

		public bool isSleeping;

		public float seatYOffset;

		public int sittingIndex;

		public bool drawFrontAccInNeckAccLayer;

		public Item heldItem;

		public bool drawFloatingTube;

		public bool drawUnicornHorn;

		public bool drawAngelHalo;

		public Color floatingTubeColor;

		public Vector2 hairOffset;

		public Vector2 helmetOffset;

		public Vector2 legsOffset;

		public bool hideEntirePlayer;

		public Vector2 Center => new Vector2(Position.X + (float)(drawPlayer.width / 2), Position.Y + (float)(drawPlayer.height / 2));

		public void BoringSetup(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
		{
			DrawDataCache = drawData;
			DustCache = dust;
			GoreCache = gore;
			drawPlayer = player;
			shadow = shadowOpacity;
			this.rotation = rotation;
			this.rotationOrigin = rotationOrigin;
			heldItem = player.lastVisualizedSelectedItem;
			cHead = drawPlayer.cHead;
			cBody = drawPlayer.cBody;
			cLegs = drawPlayer.cLegs;
			if (drawPlayer.wearsRobe)
			{
				cLegs = cBody;
			}
			cHandOn = drawPlayer.cHandOn;
			cHandOff = drawPlayer.cHandOff;
			cBack = drawPlayer.cBack;
			cFront = drawPlayer.cFront;
			cShoe = drawPlayer.cShoe;
			cWaist = drawPlayer.cWaist;
			cShield = drawPlayer.cShield;
			cNeck = drawPlayer.cNeck;
			cFace = drawPlayer.cFace;
			cBalloon = drawPlayer.cBalloon;
			cWings = drawPlayer.cWings;
			cCarpet = drawPlayer.cCarpet;
			cPortableStool = drawPlayer.cPortableStool;
			cFloatingTube = drawPlayer.cFloatingTube;
			cUnicornHorn = drawPlayer.cUnicornHorn;
			cAngelHalo = drawPlayer.cAngelHalo;
			cLeinShampoo = drawPlayer.cLeinShampoo;
			cBackpack = drawPlayer.cBackpack;
			cTail = drawPlayer.cTail;
			cFaceHead = drawPlayer.cFaceHead;
			cFaceFlower = drawPlayer.cFaceFlower;
			cBalloonFront = drawPlayer.cBalloonFront;
			cBeard = drawPlayer.cBeard;
			isSitting = drawPlayer.sitting.isSitting;
			seatYOffset = 0f;
			sittingIndex = 0;
			Vector2 posOffset = Vector2.Zero;
			drawPlayer.sitting.GetSittingOffsetInfo(drawPlayer, out posOffset, out seatYOffset);
			if (isSitting)
			{
				sittingIndex = drawPlayer.sitting.sittingIndex;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 17)
			{
				isSitting = true;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 23)
			{
				isSitting = true;
			}
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 45)
			{
				isSitting = true;
			}
			isSleeping = drawPlayer.sleeping.isSleeping;
			Position = drawPosition;
			Position += new Vector2(drawPlayer.MountXOffset * (float)drawPlayer.direction, 0f);
			if (isSitting)
			{
				torsoOffset = seatYOffset;
				Position += posOffset;
			}
			else
			{
				sittingIndex = -1;
			}
			if (isSleeping)
			{
				this.rotationOrigin = player.Size / 2f;
				drawPlayer.sleeping.GetSleepingOffsetInfo(drawPlayer, out var posOffset2);
				Position += posOffset2;
			}
			weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
			if (heldItem.type == 4952)
			{
				weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			}
			if (GolfHelper.IsPlayerHoldingClub(player) && player.itemAnimation > player.itemAnimationMax)
			{
				weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
			}
			projectileDrawPosition = -1;
			ItemLocation = Position + (drawPlayer.itemLocation - drawPlayer.position);
			armorAdjust = 0;
			missingHand = false;
			missingArm = false;
			heldProjOverHand = false;
			skinVar = drawPlayer.skinVariant;
			if (drawPlayer.body == 77 || drawPlayer.body == 103 || drawPlayer.body == 41 || drawPlayer.body == 100 || drawPlayer.body == 10 || drawPlayer.body == 11 || drawPlayer.body == 12 || drawPlayer.body == 13 || drawPlayer.body == 14 || drawPlayer.body == 43 || drawPlayer.body == 15 || drawPlayer.body == 16 || drawPlayer.body == 20 || drawPlayer.body == 39 || drawPlayer.body == 50 || drawPlayer.body == 38 || drawPlayer.body == 40 || drawPlayer.body == 57 || drawPlayer.body == 44 || drawPlayer.body == 52 || drawPlayer.body == 53 || drawPlayer.body == 68 || drawPlayer.body == 81 || drawPlayer.body == 85 || drawPlayer.body == 88 || drawPlayer.body == 98 || drawPlayer.body == 86 || drawPlayer.body == 87 || drawPlayer.body == 99 || drawPlayer.body == 165 || drawPlayer.body == 166 || drawPlayer.body == 167 || drawPlayer.body == 171 || drawPlayer.body == 45 || drawPlayer.body == 168 || drawPlayer.body == 169 || drawPlayer.body == 42 || drawPlayer.body == 180 || drawPlayer.body == 181 || drawPlayer.body == 183 || drawPlayer.body == 186 || drawPlayer.body == 187 || drawPlayer.body == 188 || drawPlayer.body == 64 || drawPlayer.body == 189 || drawPlayer.body == 191 || drawPlayer.body == 192 || drawPlayer.body == 198 || drawPlayer.body == 199 || drawPlayer.body == 202 || drawPlayer.body == 203 || drawPlayer.body == 58 || drawPlayer.body == 59 || drawPlayer.body == 60 || drawPlayer.body == 61 || drawPlayer.body == 62 || drawPlayer.body == 63 || drawPlayer.body == 36 || drawPlayer.body == 104 || drawPlayer.body == 184 || drawPlayer.body == 74 || drawPlayer.body == 78 || drawPlayer.body == 185 || drawPlayer.body == 196 || drawPlayer.body == 197 || drawPlayer.body == 182 || drawPlayer.body == 87 || drawPlayer.body == 76 || drawPlayer.body == 209 || drawPlayer.body == 168 || drawPlayer.body == 210 || drawPlayer.body == 211 || drawPlayer.body == 213)
			{
				missingHand = true;
			}
			int body = drawPlayer.body;
			if (body == 83)
			{
				missingArm = false;
			}
			else
			{
				missingArm = true;
			}
			if (drawPlayer.heldProj >= 0 && shadow == 0f)
			{
				body = Main.projectile[drawPlayer.heldProj].type;
				if (body == 460 || body == 535 || body == 600)
				{
					heldProjOverHand = true;
				}
			}
			drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out backHairDraw, out drawsBackHairWithoutHeadgear);
			hairDyePacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
			{
				hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			skinDyePacked = player.skinDyePacked;
			if (drawPlayer.mount.Active && drawPlayer.mount.Type == 52)
			{
				AdjustmentsForWolfMount();
			}
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				Point point = Center.ToTileCoordinates();
				if (Main.InSmartCursorHighlightArea(point.X, point.Y, out var actuallySelected))
				{
					Color color = Lighting.GetColor(point.X, point.Y);
					int num = (color.R + color.G + color.B) / 3;
					if (num > 10)
					{
						selectionGlowColor = Colors.GetSelectionGlowColor(actuallySelected, num);
					}
				}
			}
			mountOffSet = drawPlayer.HeightOffsetVisual;
			Position.Y -= mountOffSet;
			if (drawPlayer.mount.Active)
			{
				Mount.currentShader = (drawPlayer.mount.Cart ? drawPlayer.cMinecart : drawPlayer.cMount);
			}
			else
			{
				Mount.currentShader = 0;
			}
			playerEffect = SpriteEffects.None;
			itemEffect = SpriteEffects.FlipHorizontally;
			colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(), shadow);
			colorEyeWhites = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), Color.White), shadow);
			colorEyes = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.eyeColor), shadow);
			colorHead = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.skinColor), shadow);
			colorBodySkin = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.skinColor), shadow);
			colorLegs = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.skinColor), shadow);
			colorShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.shirtColor), shadow);
			colorUnderShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.underShirtColor), shadow);
			colorPants = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.pantsColor), shadow);
			colorShoes = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), shadow);
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.25) / 16, Color.White), shadow);
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), shadow);
			colorMount = colorArmorBody;
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
			floatingTubeColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
			colorElectricity = new Color(255, 255, 255, 100);
			colorDisplayDollSkin = colorBodySkin;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			headGlowMask = -1;
			bodyGlowMask = -1;
			armGlowMask = -1;
			legsGlowMask = -1;
			headGlowColor = Color.Transparent;
			bodyGlowColor = Color.Transparent;
			armGlowColor = Color.Transparent;
			legsGlowColor = Color.Transparent;
			switch (drawPlayer.head)
			{
			case 169:
				num2++;
				break;
			case 170:
				num3++;
				break;
			case 171:
				num4++;
				break;
			case 189:
				num5++;
				break;
			}
			switch (drawPlayer.body)
			{
			case 175:
				num2++;
				break;
			case 176:
				num3++;
				break;
			case 177:
				num4++;
				break;
			case 190:
				num5++;
				break;
			}
			switch (drawPlayer.legs)
			{
			case 110:
				num2++;
				break;
			case 111:
				num3++;
				break;
			case 112:
				num4++;
				break;
			case 130:
				num5++;
				break;
			}
			num2 = 3;
			num3 = 3;
			num4 = 3;
			num5 = 3;
			ArkhalisColor = drawPlayer.underShirtColor;
			ArkhalisColor.A = 180;
			if (drawPlayer.head == 169)
			{
				headGlowMask = 15;
				byte b = (byte)(62.5f * (float)(1 + num2));
				headGlowColor = new Color(b, b, b, 0);
			}
			else if (drawPlayer.head == 216)
			{
				headGlowMask = 256;
				byte b2 = 127;
				headGlowColor = new Color(b2, b2, b2, 0);
			}
			else if (drawPlayer.head == 210)
			{
				headGlowMask = 242;
				byte b3 = 127;
				headGlowColor = new Color(b3, b3, b3, 0);
			}
			else if (drawPlayer.head == 214)
			{
				headGlowMask = 245;
				headGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.head == 240)
			{
				headGlowMask = 273;
				headGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.head == 267)
			{
				headGlowMask = 301;
				headGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.head == 268)
			{
				headGlowMask = 302;
				float num6 = (float)(int)Main.mouseTextColor / 255f;
				num6 *= num6;
				headGlowColor = new Color(255, 255, 255) * num6;
			}
			else if (drawPlayer.head == 269)
			{
				headGlowMask = 304;
				headGlowColor = new Color(200, 200, 200);
			}
			else if (drawPlayer.head == 270)
			{
				headGlowMask = 305;
				headGlowColor = new Color(200, 200, 200, 150);
			}
			else if (drawPlayer.head == 271)
			{
				headGlowMask = 309;
				headGlowColor = Color.White;
			}
			else if (drawPlayer.head == 170)
			{
				headGlowMask = 16;
				byte b4 = (byte)(62.5f * (float)(1 + num3));
				headGlowColor = new Color(b4, b4, b4, 0);
			}
			else if (drawPlayer.head == 189)
			{
				headGlowMask = 184;
				byte b5 = (byte)(62.5f * (float)(1 + num5));
				headGlowColor = new Color(b5, b5, b5, 0);
				colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color(b5, b5, b5, 255), shadow);
			}
			else if (drawPlayer.head == 171)
			{
				byte b6 = (byte)(62.5f * (float)(1 + num4));
				colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color(b6, b6, b6, 255), shadow);
			}
			else if (drawPlayer.head == 175)
			{
				headGlowMask = 41;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (drawPlayer.head == 193)
			{
				headGlowMask = 209;
				headGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.head == 109)
			{
				headGlowMask = 208;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (drawPlayer.head == 178)
			{
				headGlowMask = 96;
				headGlowColor = new Color(255, 255, 255, 0);
			}
			if (drawPlayer.body == 175)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 13;
				}
				else
				{
					bodyGlowMask = 18;
				}
				byte b7 = (byte)(62.5f * (float)(1 + num2));
				bodyGlowColor = new Color(b7, b7, b7, 0);
			}
			else if (drawPlayer.body == 208)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 246;
				}
				else
				{
					bodyGlowMask = 247;
				}
				armGlowMask = 248;
				bodyGlowColor = ArkhalisColor;
				armGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.body == 227)
			{
				bodyGlowColor = new Color(230, 230, 230, 60);
				armGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.body == 237)
			{
				float num7 = (float)(int)Main.mouseTextColor / 255f;
				num7 *= num7;
				bodyGlowColor = new Color(255, 255, 255) * num7;
			}
			else if (drawPlayer.body == 238)
			{
				bodyGlowColor = new Color(255, 255, 255);
				armGlowColor = new Color(255, 255, 255);
			}
			else if (drawPlayer.body == 239)
			{
				bodyGlowColor = new Color(200, 200, 200, 150);
				armGlowColor = new Color(200, 200, 200, 150);
			}
			else if (drawPlayer.body == 190)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 185;
				}
				else
				{
					bodyGlowMask = 186;
				}
				armGlowMask = 188;
				byte b8 = (byte)(62.5f * (float)(1 + num5));
				bodyGlowColor = new Color(b8, b8, b8, 0);
				armGlowColor = new Color(b8, b8, b8, 0);
				colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color(b8, b8, b8, 255), shadow);
			}
			else if (drawPlayer.body == 176)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 14;
				}
				else
				{
					bodyGlowMask = 19;
				}
				armGlowMask = 12;
				byte b9 = (byte)(62.5f * (float)(1 + num3));
				bodyGlowColor = new Color(b9, b9, b9, 0);
				armGlowColor = new Color(b9, b9, b9, 0);
			}
			else if (drawPlayer.body == 194)
			{
				bodyGlowMask = 210;
				armGlowMask = 211;
				bodyGlowColor = new Color(255, 255, 255, 127);
				armGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.body == 177)
			{
				byte b10 = (byte)(62.5f * (float)(1 + num4));
				colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color(b10, b10, b10, 255), shadow);
			}
			else if (drawPlayer.body == 179)
			{
				if (drawPlayer.Male)
				{
					bodyGlowMask = 42;
				}
				else
				{
					bodyGlowMask = 43;
				}
				armGlowMask = 44;
				bodyGlowColor = new Color(255, 255, 255, 0);
				armGlowColor = new Color(255, 255, 255, 0);
			}
			if (drawPlayer.legs == 111)
			{
				legsGlowMask = 17;
				byte b11 = (byte)(62.5f * (float)(1 + num3));
				legsGlowColor = new Color(b11, b11, b11, 0);
			}
			else if (drawPlayer.legs == 157)
			{
				legsGlowMask = 249;
				legsGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.legs == 158)
			{
				legsGlowMask = 250;
				legsGlowColor = ArkhalisColor;
			}
			else if (drawPlayer.legs == 210)
			{
				legsGlowMask = 274;
				legsGlowColor = new Color(230, 230, 230, 60);
			}
			else if (drawPlayer.legs == 222)
			{
				legsGlowMask = 303;
				float num8 = (float)(int)Main.mouseTextColor / 255f;
				num8 *= num8;
				legsGlowColor = new Color(255, 255, 255) * num8;
			}
			else if (drawPlayer.legs == 225)
			{
				legsGlowMask = 306;
				legsGlowColor = new Color(200, 200, 200, 150);
			}
			else if (drawPlayer.legs == 226)
			{
				legsGlowMask = 307;
				legsGlowColor = new Color(200, 200, 200, 150);
			}
			else if (drawPlayer.legs == 110)
			{
				legsGlowMask = 199;
				byte b12 = (byte)(62.5f * (float)(1 + num2));
				legsGlowColor = new Color(b12, b12, b12, 0);
			}
			else if (drawPlayer.legs == 112)
			{
				byte b13 = (byte)(62.5f * (float)(1 + num4));
				colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color(b13, b13, b13, 255), shadow);
			}
			else if (drawPlayer.legs == 134)
			{
				legsGlowMask = 212;
				legsGlowColor = new Color(255, 255, 255, 127);
			}
			else if (drawPlayer.legs == 130)
			{
				byte b14 = (byte)(127 * (1 + num5));
				legsGlowMask = 187;
				legsGlowColor = new Color(b14, b14, b14, 0);
				colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color(b14, b14, b14, 255), shadow);
			}
			float alphaReduction = shadow;
			headGlowColor = drawPlayer.GetImmuneAlphaPure(headGlowColor, alphaReduction);
			bodyGlowColor = drawPlayer.GetImmuneAlphaPure(bodyGlowColor, alphaReduction);
			armGlowColor = drawPlayer.GetImmuneAlphaPure(armGlowColor, alphaReduction);
			legsGlowColor = drawPlayer.GetImmuneAlphaPure(legsGlowColor, alphaReduction);
			if (drawPlayer.head > 0 && drawPlayer.head < 282)
			{
				Main.instance.LoadArmorHead(drawPlayer.head);
				int num9 = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
				if (num9 >= 0)
				{
					Main.instance.LoadArmorHead(num9);
				}
			}
			if (drawPlayer.body > 0 && drawPlayer.body < 248)
			{
				Main.instance.LoadArmorBody(drawPlayer.body);
			}
			if (drawPlayer.legs > 0 && drawPlayer.legs < 236)
			{
				Main.instance.LoadArmorLegs(drawPlayer.legs);
			}
			if (drawPlayer.handon > 0 && drawPlayer.handon < 24)
			{
				Main.instance.LoadAccHandsOn(drawPlayer.handon);
			}
			if (drawPlayer.handoff > 0 && drawPlayer.handoff < 16)
			{
				Main.instance.LoadAccHandsOff(drawPlayer.handoff);
			}
			if (drawPlayer.back > 0 && drawPlayer.back < 37)
			{
				Main.instance.LoadAccBack(drawPlayer.back);
			}
			if (drawPlayer.front > 0 && drawPlayer.front < 13)
			{
				Main.instance.LoadAccFront(drawPlayer.front);
			}
			if (drawPlayer.shoe > 0 && drawPlayer.shoe < 27)
			{
				Main.instance.LoadAccShoes(drawPlayer.shoe);
			}
			if (drawPlayer.waist > 0 && drawPlayer.waist < 17)
			{
				Main.instance.LoadAccWaist(drawPlayer.waist);
			}
			if (drawPlayer.shield > 0 && drawPlayer.shield < 10)
			{
				Main.instance.LoadAccShield(drawPlayer.shield);
			}
			if (drawPlayer.neck > 0 && drawPlayer.neck < 12)
			{
				Main.instance.LoadAccNeck(drawPlayer.neck);
			}
			if (drawPlayer.face > 0 && drawPlayer.face < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			if (drawPlayer.balloon > 0 && drawPlayer.balloon < 20)
			{
				Main.instance.LoadAccBalloon(drawPlayer.balloon);
			}
			if (drawPlayer.backpack > 0 && drawPlayer.backpack < 37)
			{
				Main.instance.LoadAccBack(drawPlayer.backpack);
			}
			if (drawPlayer.tail > 0 && drawPlayer.tail < 37)
			{
				Main.instance.LoadAccBack(drawPlayer.tail);
			}
			if (drawPlayer.faceHead > 0 && drawPlayer.faceHead < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.faceHead);
			}
			if (drawPlayer.faceFlower > 0 && drawPlayer.faceFlower < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.faceFlower);
			}
			if (drawPlayer.balloonFront > 0 && drawPlayer.balloonFront < 20)
			{
				Main.instance.LoadAccBalloon(drawPlayer.balloonFront);
			}
			if (drawPlayer.beard > 0 && drawPlayer.beard < 5)
			{
				Main.instance.LoadAccBeard(drawPlayer.beard);
			}
			Main.instance.LoadHair(drawPlayer.hair);
			if (drawPlayer.eyebrellaCloud)
			{
				Main.instance.LoadProjectile(238);
			}
			if (drawPlayer.isHatRackDoll)
			{
				colorLegs = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorHead = Color.Transparent;
				colorHair = Color.Transparent;
				colorEyes = Color.Transparent;
				colorEyeWhites = Color.Transparent;
			}
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				if (drawPlayer.isFullbright)
				{
					colorHead = Color.White;
					colorBodySkin = Color.White;
					colorLegs = Color.White;
					colorEyes = Color.White;
					colorEyeWhites = Color.White;
					colorArmorHead = Color.White;
					colorArmorBody = Color.White;
					colorArmorLegs = Color.White;
					colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR;
				}
				else
				{
					colorDisplayDollSkin = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR), shadow);
				}
			}
			if (!drawPlayer.isDisplayDollOrInanimate)
			{
				if ((drawPlayer.head == 78 || drawPlayer.head == 79 || drawPlayer.head == 80) && drawPlayer.body == 51 && drawPlayer.legs == 47)
				{
					float num10 = (float)(int)Main.mouseTextColor / 200f - 0.3f;
					if (shadow != 0f)
					{
						num10 = 0f;
					}
					colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num10);
					colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num10);
					colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num10);
					colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num10);
					colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num10);
					colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num10);
					colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num10);
					colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num10);
					colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num10);
				}
				if (drawPlayer.head == 193 && drawPlayer.body == 194 && drawPlayer.legs == 134)
				{
					float num11 = 0.6f - drawPlayer.ghostFade * 0.3f;
					if (shadow != 0f)
					{
						num11 = 0f;
					}
					colorArmorHead.R = (byte)((float)(int)colorArmorHead.R * num11);
					colorArmorHead.G = (byte)((float)(int)colorArmorHead.G * num11);
					colorArmorHead.B = (byte)((float)(int)colorArmorHead.B * num11);
					colorArmorBody.R = (byte)((float)(int)colorArmorBody.R * num11);
					colorArmorBody.G = (byte)((float)(int)colorArmorBody.G * num11);
					colorArmorBody.B = (byte)((float)(int)colorArmorBody.B * num11);
					colorArmorLegs.R = (byte)((float)(int)colorArmorLegs.R * num11);
					colorArmorLegs.G = (byte)((float)(int)colorArmorLegs.G * num11);
					colorArmorLegs.B = (byte)((float)(int)colorArmorLegs.B * num11);
				}
				if (shadow > 0f)
				{
					colorLegs = Color.Transparent;
					colorBodySkin = Color.Transparent;
					colorHead = Color.Transparent;
					colorHair = Color.Transparent;
					colorEyes = Color.Transparent;
					colorEyeWhites = Color.Transparent;
				}
			}
			float num12 = 1f;
			float num13 = 1f;
			float num14 = 1f;
			float num15 = 1f;
			if (drawPlayer.honey && Main.rand.Next(30) == 0 && shadow == 0f)
			{
				Dust dust2 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 152, 0f, 0f, 150);
				dust2.velocity.Y = 0.3f;
				dust2.velocity.X *= 0.1f;
				dust2.scale += (float)Main.rand.Next(3, 4) * 0.1f;
				dust2.alpha = 100;
				dust2.noGravity = true;
				dust2.velocity += drawPlayer.velocity * 0.1f;
				DustCache.Add(dust2.dustIndex);
			}
			if (drawPlayer.dryadWard && drawPlayer.velocity.X != 0f && Main.rand.Next(4) == 0)
			{
				Dust dust3 = Dust.NewDustDirect(new Vector2(drawPlayer.position.X - 2f, drawPlayer.position.Y + (float)drawPlayer.height - 2f), drawPlayer.width + 4, 4, 163, 0f, 0f, 100, default(Color), 1.5f);
				dust3.noGravity = true;
				dust3.noLight = true;
				dust3.velocity *= 0f;
				DustCache.Add(dust3.dustIndex);
			}
			if (drawPlayer.poisoned)
			{
				if (Main.rand.Next(50) == 0 && shadow == 0f)
				{
					Dust dust4 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 46, 0f, 0f, 150, default(Color), 0.2f);
					dust4.noGravity = true;
					dust4.fadeIn = 1.9f;
					DustCache.Add(dust4.dustIndex);
				}
				num12 *= 0.65f;
				num14 *= 0.75f;
			}
			if (drawPlayer.venom)
			{
				if (Main.rand.Next(10) == 0 && shadow == 0f)
				{
					Dust dust5 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 171, 0f, 0f, 100, default(Color), 0.5f);
					dust5.noGravity = true;
					dust5.fadeIn = 1.5f;
					DustCache.Add(dust5.dustIndex);
				}
				num13 *= 0.45f;
				num12 *= 0.75f;
			}
			if (drawPlayer.onFire)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust6 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust6.noGravity = true;
					dust6.velocity *= 1.8f;
					dust6.velocity.Y -= 0.5f;
					DustCache.Add(dust6.dustIndex);
				}
				num14 *= 0.6f;
				num13 *= 0.7f;
			}
			if (drawPlayer.onFire3)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust7 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust7.noGravity = true;
					dust7.velocity *= 1.8f;
					dust7.velocity.Y -= 0.5f;
					DustCache.Add(dust7.dustIndex);
				}
				num14 *= 0.6f;
				num13 *= 0.7f;
			}
			if (drawPlayer.dripping && shadow == 0f && Main.rand.Next(4) != 0)
			{
				Vector2 position = Position;
				position.X -= 2f;
				position.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					Dust dust8 = Dust.NewDustDirect(position, drawPlayer.width + 4, drawPlayer.height + 2, 211, 0f, 0f, 50, default(Color), 0.8f);
					if (Main.rand.Next(2) == 0)
					{
						dust8.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust8.alpha += 25;
					}
					dust8.noLight = true;
					dust8.velocity *= 0.2f;
					dust8.velocity.Y += 0.2f;
					dust8.velocity += drawPlayer.velocity;
					DustCache.Add(dust8.dustIndex);
				}
				else
				{
					Dust dust9 = Dust.NewDustDirect(position, drawPlayer.width + 8, drawPlayer.height + 8, 211, 0f, 0f, 50, default(Color), 1.1f);
					if (Main.rand.Next(2) == 0)
					{
						dust9.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust9.alpha += 25;
					}
					dust9.noLight = true;
					dust9.noGravity = true;
					dust9.velocity *= 0.2f;
					dust9.velocity.Y += 1f;
					dust9.velocity += drawPlayer.velocity;
					DustCache.Add(dust9.dustIndex);
				}
			}
			if (drawPlayer.drippingSlime)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, 255, 100);
				if (Main.rand.Next(4) != 0 && shadow == 0f)
				{
					Vector2 position2 = Position;
					position2.X -= 2f;
					position2.Y -= 2f;
					if (Main.rand.Next(2) == 0)
					{
						Dust dust10 = Dust.NewDustDirect(position2, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
						if (Main.rand.Next(2) == 0)
						{
							dust10.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust10.alpha += 25;
						}
						dust10.noLight = true;
						dust10.velocity *= 0.2f;
						dust10.velocity.Y += 0.2f;
						dust10.velocity += drawPlayer.velocity;
						DustCache.Add(dust10.dustIndex);
					}
				}
				num12 *= 0.8f;
				num13 *= 0.8f;
			}
			if (drawPlayer.drippingSparkleSlime)
			{
				int alpha2 = 100;
				if (Main.rand.Next(4) != 0 && shadow == 0f)
				{
					Vector2 position3 = Position;
					position3.X -= 2f;
					position3.Y -= 2f;
					if (Main.rand.Next(4) == 0)
					{
						Color newColor2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
						newColor2.A /= 2;
						Dust dust11 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha2, newColor2, 0.65f);
						if (Main.rand.Next(2) == 0)
						{
							dust11.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust11.alpha += 25;
						}
						dust11.noLight = true;
						dust11.velocity *= 0.2f;
						dust11.velocity += drawPlayer.velocity * 0.7f;
						dust11.fadeIn = 0.8f;
						DustCache.Add(dust11.dustIndex);
					}
					if (Main.rand.Next(30) == 0)
					{
						Color color2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
						color2.A /= 2;
						Dust dust12 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 43, 0f, 0f, 254, new Color(127, 127, 127, 0), 0.45f);
						dust12.noLight = true;
						dust12.velocity.X *= 0f;
						dust12.velocity *= 0.03f;
						dust12.fadeIn = 0.6f;
						DustCache.Add(dust12.dustIndex);
					}
				}
				num12 *= 0.94f;
				num13 *= 0.82f;
			}
			if (drawPlayer.ichor)
			{
				num14 = 0f;
			}
			if (drawPlayer.electrified && shadow == 0f && Main.rand.Next(3) == 0)
			{
				Dust dust13 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 226, 0f, 0f, 100, default(Color), 0.5f);
				dust13.velocity *= 1.6f;
				dust13.velocity.Y -= 1f;
				dust13.position = Vector2.Lerp(dust13.position, drawPlayer.Center, 0.5f);
				DustCache.Add(dust13.dustIndex);
			}
			if (drawPlayer.burned)
			{
				if (shadow == 0f)
				{
					Dust dust14 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 2f);
					dust14.noGravity = true;
					dust14.velocity *= 1.8f;
					dust14.velocity.Y -= 0.75f;
					DustCache.Add(dust14.dustIndex);
				}
				num12 = 1f;
				num14 *= 0.6f;
				num13 *= 0.7f;
			}
			if (drawPlayer.onFrostBurn)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust15 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 135, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust15.noGravity = true;
					dust15.velocity *= 1.8f;
					dust15.velocity.Y -= 0.5f;
					DustCache.Add(dust15.dustIndex);
				}
				num12 *= 0.5f;
				num13 *= 0.7f;
			}
			if (drawPlayer.onFrostBurn2)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust16 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 135, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust16.noGravity = true;
					dust16.velocity *= 1.8f;
					dust16.velocity.Y -= 0.5f;
					DustCache.Add(dust16.dustIndex);
				}
				num12 *= 0.5f;
				num13 *= 0.7f;
			}
			if (drawPlayer.onFire2)
			{
				if (Main.rand.Next(4) == 0 && shadow == 0f)
				{
					Dust dust17 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 75, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust17.noGravity = true;
					dust17.velocity *= 1.8f;
					dust17.velocity.Y -= 0.5f;
					DustCache.Add(dust17.dustIndex);
				}
				num14 *= 0.6f;
				num13 *= 0.7f;
			}
			if (drawPlayer.noItems)
			{
				num13 *= 0.8f;
				num12 *= 0.65f;
			}
			if (drawPlayer.blind)
			{
				num13 *= 0.65f;
				num12 *= 0.7f;
			}
			if (drawPlayer.bleed)
			{
				num13 *= 0.9f;
				num14 *= 0.9f;
				if (!drawPlayer.dead && Main.rand.Next(30) == 0 && shadow == 0f)
				{
					Dust dust18 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 5);
					dust18.velocity.Y += 0.5f;
					dust18.velocity *= 0.25f;
					DustCache.Add(dust18.dustIndex);
				}
			}
			if (shadow == 0f && drawPlayer.palladiumRegen && drawPlayer.statLife < drawPlayer.statLifeMax2 && Main.instance.IsActive && !Main.gamePaused && drawPlayer.miscCounter % 10 == 0 && shadow == 0f)
			{
				Vector2 position4 = default(Vector2);
				position4.X = Position.X + (float)Main.rand.Next(drawPlayer.width);
				position4.Y = Position.Y + (float)Main.rand.Next(drawPlayer.height);
				position4.X = Position.X + (float)(drawPlayer.width / 2) - 6f;
				position4.Y = Position.Y + (float)(drawPlayer.height / 2) - 6f;
				position4.X -= Main.rand.Next(-10, 11);
				position4.Y -= Main.rand.Next(-20, 21);
				int item = Gore.NewGore(position4, new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-20, -10) * 0.1f), 331, (float)Main.rand.Next(80, 120) * 0.01f);
				GoreCache.Add(item);
			}
			if (shadow == 0f && drawPlayer.loveStruck && Main.instance.IsActive && !Main.gamePaused && Main.rand.Next(5) == 0)
			{
				Vector2 vector = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
				vector.Normalize();
				vector.X *= 0.66f;
				int num16 = Gore.NewGore(Position + new Vector2(Main.rand.Next(drawPlayer.width + 1), Main.rand.Next(drawPlayer.height + 1)), vector * Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
				Main.gore[num16].sticky = false;
				Main.gore[num16].velocity *= 0.4f;
				Main.gore[num16].velocity.Y -= 0.6f;
				GoreCache.Add(num16);
			}
			if (drawPlayer.stinky && Main.instance.IsActive && !Main.gamePaused)
			{
				num12 *= 0.7f;
				num14 *= 0.55f;
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					Vector2 vector2 = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
					vector2.Normalize();
					vector2.X *= 0.66f;
					vector2.Y = Math.Abs(vector2.Y);
					Vector2 vector3 = vector2 * Main.rand.Next(3, 5) * 0.25f;
					int num17 = Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, 188, vector3.X, vector3.Y * 0.5f, 100, default(Color), 1.5f);
					Main.dust[num17].velocity *= 0.1f;
					Main.dust[num17].velocity.Y -= 0.5f;
					DustCache.Add(num17);
				}
			}
			if (drawPlayer.slowOgreSpit && Main.instance.IsActive && !Main.gamePaused)
			{
				num12 *= 0.6f;
				num14 *= 0.45f;
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					int type = Utils.SelectRandom<int>(Main.rand, 4, 256);
					Dust dust19 = Main.dust[Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, type, 0f, 0f, 100)];
					dust19.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
					dust19.fadeIn = 0.5f;
					dust19.velocity *= 0.05f;
					dust19.noLight = true;
					if (dust19.type == 4)
					{
						dust19.color = new Color(80, 170, 40, 120);
					}
					DustCache.Add(dust19.dustIndex);
				}
				if (Main.rand.Next(5) == 0 && shadow == 0f)
				{
					int num18 = Gore.NewGore(Position + new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat()) * drawPlayer.Size, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 1024, 1025, 1026), 0.65f);
					Main.gore[num18].velocity *= 0.05f;
					GoreCache.Add(num18);
				}
			}
			if (Main.instance.IsActive && !Main.gamePaused && shadow == 0f)
			{
				float num19 = (float)drawPlayer.miscCounter / 180f;
				float num20 = 0f;
				float num21 = 10f;
				int type2 = 90;
				int num22 = 0;
				for (int i = 0; i < 3; i++)
				{
					switch (i)
					{
					case 0:
						if (drawPlayer.nebulaLevelLife < 1)
						{
							continue;
						}
						num20 = MathF.PI * 2f / (float)drawPlayer.nebulaLevelLife;
						num22 = drawPlayer.nebulaLevelLife;
						break;
					case 1:
						if (drawPlayer.nebulaLevelMana < 1)
						{
							continue;
						}
						num20 = MathF.PI * -2f / (float)drawPlayer.nebulaLevelMana;
						num22 = drawPlayer.nebulaLevelMana;
						num19 = (float)(-drawPlayer.miscCounter) / 180f;
						num21 = 20f;
						type2 = 88;
						break;
					case 2:
						if (drawPlayer.nebulaLevelDamage < 1)
						{
							continue;
						}
						num20 = MathF.PI * 2f / (float)drawPlayer.nebulaLevelDamage;
						num22 = drawPlayer.nebulaLevelDamage;
						num19 = (float)drawPlayer.miscCounter / 180f;
						num21 = 30f;
						type2 = 86;
						break;
					}
					for (int j = 0; j < num22; j++)
					{
						Dust dust20 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, type2, 0f, 0f, 100, default(Color), 1.5f);
						dust20.noGravity = true;
						dust20.velocity = Vector2.Zero;
						dust20.position = drawPlayer.Center + Vector2.UnitY * drawPlayer.gfxOffY + (num19 * (MathF.PI * 2f) + num20 * (float)j).ToRotationVector2() * num21;
						dust20.customData = drawPlayer;
						DustCache.Add(dust20.dustIndex);
					}
				}
			}
			if (drawPlayer.witheredArmor && Main.instance.IsActive && !Main.gamePaused)
			{
				num13 *= 0.5f;
				num12 *= 0.75f;
			}
			if (drawPlayer.witheredWeapon && drawPlayer.itemAnimation > 0 && heldItem.damage > 0 && Main.instance.IsActive && !Main.gamePaused && Main.rand.Next(3) == 0)
			{
				Dust dust21 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 272, 0f, 0f, 50, default(Color), 0.5f);
				dust21.velocity *= 1.6f;
				dust21.velocity.Y -= 1f;
				dust21.position = Vector2.Lerp(dust21.position, drawPlayer.Center, 0.5f);
				DustCache.Add(dust21.dustIndex);
			}
			_ = drawPlayer.shimmering;
			if (num12 != 1f || num13 != 1f || num14 != 1f || num15 != 1f)
			{
				if (drawPlayer.onFire || drawPlayer.onFire2 || drawPlayer.onFrostBurn || drawPlayer.onFire3 || drawPlayer.onFrostBurn2)
				{
					colorEyeWhites = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorEyes = drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, shadow);
					colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), shadow);
					colorHead = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorBodySkin = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorShirt = drawPlayer.GetImmuneAlpha(drawPlayer.shirtColor, shadow);
					colorUnderShirt = drawPlayer.GetImmuneAlpha(drawPlayer.underShirtColor, shadow);
					colorPants = drawPlayer.GetImmuneAlpha(drawPlayer.pantsColor, shadow);
					colorLegs = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
					colorShoes = drawPlayer.GetImmuneAlpha(drawPlayer.shoeColor, shadow);
					colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
					if (drawPlayer.isDisplayDollOrInanimate)
					{
						colorDisplayDollSkin = drawPlayer.GetImmuneAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, shadow);
					}
				}
				else
				{
					colorEyeWhites = Main.buffColor(colorEyeWhites, num12, num13, num14, num15);
					colorEyes = Main.buffColor(colorEyes, num12, num13, num14, num15);
					colorHair = Main.buffColor(colorHair, num12, num13, num14, num15);
					colorHead = Main.buffColor(colorHead, num12, num13, num14, num15);
					colorBodySkin = Main.buffColor(colorBodySkin, num12, num13, num14, num15);
					colorShirt = Main.buffColor(colorShirt, num12, num13, num14, num15);
					colorUnderShirt = Main.buffColor(colorUnderShirt, num12, num13, num14, num15);
					colorPants = Main.buffColor(colorPants, num12, num13, num14, num15);
					colorLegs = Main.buffColor(colorLegs, num12, num13, num14, num15);
					colorShoes = Main.buffColor(colorShoes, num12, num13, num14, num15);
					colorArmorHead = Main.buffColor(colorArmorHead, num12, num13, num14, num15);
					colorArmorBody = Main.buffColor(colorArmorBody, num12, num13, num14, num15);
					colorArmorLegs = Main.buffColor(colorArmorLegs, num12, num13, num14, num15);
					if (drawPlayer.isDisplayDollOrInanimate)
					{
						colorDisplayDollSkin = Main.buffColor(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, num12, num13, num14, num15);
					}
				}
			}
			if (drawPlayer.socialGhost)
			{
				colorEyeWhites = Color.Transparent;
				colorEyes = Color.Transparent;
				colorHair = Color.Transparent;
				colorHead = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorShirt = Color.Transparent;
				colorUnderShirt = Color.Transparent;
				colorPants = Color.Transparent;
				colorShoes = Color.Transparent;
				colorLegs = Color.Transparent;
				if (colorArmorHead.A > Main.gFade)
				{
					colorArmorHead.A = Main.gFade;
				}
				if (colorArmorBody.A > Main.gFade)
				{
					colorArmorBody.A = Main.gFade;
				}
				if (colorArmorLegs.A > Main.gFade)
				{
					colorArmorLegs.A = Main.gFade;
				}
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Color.Transparent;
				}
			}
			if (drawPlayer.socialIgnoreLight)
			{
				float num23 = 1f;
				colorEyeWhites = Color.White * num23;
				colorEyes = drawPlayer.eyeColor * num23;
				colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White);
				colorHead = drawPlayer.skinColor * num23;
				colorBodySkin = drawPlayer.skinColor * num23;
				colorShirt = drawPlayer.shirtColor * num23;
				colorUnderShirt = drawPlayer.underShirtColor * num23;
				colorPants = drawPlayer.pantsColor * num23;
				colorShoes = drawPlayer.shoeColor * num23;
				colorLegs = drawPlayer.skinColor * num23;
				colorArmorHead = Color.White;
				colorArmorBody = Color.White;
				colorArmorLegs = Color.White;
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * num23;
				}
			}
			if (drawPlayer.opacityForAnimation != 1f)
			{
				shadow = 1f - drawPlayer.opacityForAnimation;
				float opacityForAnimation = drawPlayer.opacityForAnimation;
				opacityForAnimation *= opacityForAnimation;
				colorEyeWhites = Color.White * opacityForAnimation;
				colorEyes = drawPlayer.eyeColor * opacityForAnimation;
				colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White) * opacityForAnimation;
				colorHead = drawPlayer.skinColor * opacityForAnimation;
				colorBodySkin = drawPlayer.skinColor * opacityForAnimation;
				colorShirt = drawPlayer.shirtColor * opacityForAnimation;
				colorUnderShirt = drawPlayer.underShirtColor * opacityForAnimation;
				colorPants = drawPlayer.pantsColor * opacityForAnimation;
				colorShoes = drawPlayer.shoeColor * opacityForAnimation;
				colorLegs = drawPlayer.skinColor * opacityForAnimation;
				colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * opacityForAnimation;
				}
			}
			stealth = 1f;
			if (heldItem.type == 3106)
			{
				float num24 = drawPlayer.stealth;
				if ((double)num24 < 0.03)
				{
					num24 = 0.03f;
				}
				float num25 = (1f + num24 * 10f) / 11f;
				if (num24 < 0f)
				{
					num24 = 0f;
				}
				if (!(num24 < 1f - shadow) && shadow > 0f)
				{
					num24 = shadow * 0.5f;
				}
				stealth = num25;
				colorArmorHead = new Color((byte)((float)(int)colorArmorHead.R * num24), (byte)((float)(int)colorArmorHead.G * num24), (byte)((float)(int)colorArmorHead.B * num25), (byte)((float)(int)colorArmorHead.A * num24));
				colorArmorBody = new Color((byte)((float)(int)colorArmorBody.R * num24), (byte)((float)(int)colorArmorBody.G * num24), (byte)((float)(int)colorArmorBody.B * num25), (byte)((float)(int)colorArmorBody.A * num24));
				colorArmorLegs = new Color((byte)((float)(int)colorArmorLegs.R * num24), (byte)((float)(int)colorArmorLegs.G * num24), (byte)((float)(int)colorArmorLegs.B * num25), (byte)((float)(int)colorArmorLegs.A * num24));
				num24 *= num24;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num24);
				colorEyes = Color.Multiply(colorEyes, num24);
				colorHair = Color.Multiply(colorHair, num24);
				colorHead = Color.Multiply(colorHead, num24);
				colorBodySkin = Color.Multiply(colorBodySkin, num24);
				colorShirt = Color.Multiply(colorShirt, num24);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num24);
				colorPants = Color.Multiply(colorPants, num24);
				colorShoes = Color.Multiply(colorShoes, num24);
				colorLegs = Color.Multiply(colorLegs, num24);
				colorMount = Color.Multiply(colorMount, num24);
				headGlowColor = Color.Multiply(headGlowColor, num24);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num24);
				armGlowColor = Color.Multiply(armGlowColor, num24);
				legsGlowColor = Color.Multiply(legsGlowColor, num24);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num24);
				}
			}
			else if (drawPlayer.shroomiteStealth)
			{
				float num26 = drawPlayer.stealth;
				if ((double)num26 < 0.03)
				{
					num26 = 0.03f;
				}
				float num27 = (1f + num26 * 10f) / 11f;
				if (num26 < 0f)
				{
					num26 = 0f;
				}
				if (!(num26 < 1f - shadow) && shadow > 0f)
				{
					num26 = shadow * 0.5f;
				}
				stealth = num27;
				colorArmorHead = new Color((byte)((float)(int)colorArmorHead.R * num26), (byte)((float)(int)colorArmorHead.G * num26), (byte)((float)(int)colorArmorHead.B * num27), (byte)((float)(int)colorArmorHead.A * num26));
				colorArmorBody = new Color((byte)((float)(int)colorArmorBody.R * num26), (byte)((float)(int)colorArmorBody.G * num26), (byte)((float)(int)colorArmorBody.B * num27), (byte)((float)(int)colorArmorBody.A * num26));
				colorArmorLegs = new Color((byte)((float)(int)colorArmorLegs.R * num26), (byte)((float)(int)colorArmorLegs.G * num26), (byte)((float)(int)colorArmorLegs.B * num27), (byte)((float)(int)colorArmorLegs.A * num26));
				num26 *= num26;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num26);
				colorEyes = Color.Multiply(colorEyes, num26);
				colorHair = Color.Multiply(colorHair, num26);
				colorHead = Color.Multiply(colorHead, num26);
				colorBodySkin = Color.Multiply(colorBodySkin, num26);
				colorShirt = Color.Multiply(colorShirt, num26);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num26);
				colorPants = Color.Multiply(colorPants, num26);
				colorShoes = Color.Multiply(colorShoes, num26);
				colorLegs = Color.Multiply(colorLegs, num26);
				colorMount = Color.Multiply(colorMount, num26);
				headGlowColor = Color.Multiply(headGlowColor, num26);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num26);
				armGlowColor = Color.Multiply(armGlowColor, num26);
				legsGlowColor = Color.Multiply(legsGlowColor, num26);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num26);
				}
			}
			else if (drawPlayer.setVortex)
			{
				float num28 = drawPlayer.stealth;
				if ((double)num28 < 0.03)
				{
					num28 = 0.03f;
				}
				if (num28 < 0f)
				{
					num28 = 0f;
				}
				if (!(num28 < 1f - shadow) && shadow > 0f)
				{
					num28 = shadow * 0.5f;
				}
				stealth = num28;
				Color secondColor = new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num28));
				colorArmorHead = colorArmorHead.MultiplyRGBA(secondColor);
				colorArmorBody = colorArmorBody.MultiplyRGBA(secondColor);
				colorArmorLegs = colorArmorLegs.MultiplyRGBA(secondColor);
				num28 *= num28;
				colorEyeWhites = Color.Multiply(colorEyeWhites, num28);
				colorEyes = Color.Multiply(colorEyes, num28);
				colorHair = Color.Multiply(colorHair, num28);
				colorHead = Color.Multiply(colorHead, num28);
				colorBodySkin = Color.Multiply(colorBodySkin, num28);
				colorShirt = Color.Multiply(colorShirt, num28);
				colorUnderShirt = Color.Multiply(colorUnderShirt, num28);
				colorPants = Color.Multiply(colorPants, num28);
				colorShoes = Color.Multiply(colorShoes, num28);
				colorLegs = Color.Multiply(colorLegs, num28);
				colorMount = Color.Multiply(colorMount, num28);
				headGlowColor = Color.Multiply(headGlowColor, num28);
				bodyGlowColor = Color.Multiply(bodyGlowColor, num28);
				armGlowColor = Color.Multiply(armGlowColor, num28);
				legsGlowColor = Color.Multiply(legsGlowColor, num28);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num28);
				}
			}
			if (hideEntirePlayer)
			{
				stealth = 1f;
				colorDisplayDollSkin = (legsGlowColor = (armGlowColor = (bodyGlowColor = (headGlowColor = (colorLegs = (colorShoes = (colorPants = (colorUnderShirt = (colorShirt = (colorBodySkin = (colorHead = (colorHair = (colorEyes = (colorEyeWhites = (colorArmorLegs = (colorArmorBody = (colorArmorHead = Color.Transparent)))))))))))))))));
			}
			if (drawPlayer.gravDir == 1f)
			{
				if (drawPlayer.direction == 1)
				{
					playerEffect = SpriteEffects.None;
					itemEffect = SpriteEffects.None;
				}
				else
				{
					playerEffect = SpriteEffects.FlipHorizontally;
					itemEffect = SpriteEffects.FlipHorizontally;
				}
				if (!drawPlayer.dead)
				{
					drawPlayer.legPosition.Y = 0f;
					drawPlayer.headPosition.Y = 0f;
					drawPlayer.bodyPosition.Y = 0f;
				}
			}
			else
			{
				if (drawPlayer.direction == 1)
				{
					playerEffect = SpriteEffects.FlipVertically;
					itemEffect = SpriteEffects.FlipVertically;
				}
				else
				{
					playerEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
					itemEffect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
				if (!drawPlayer.dead)
				{
					drawPlayer.legPosition.Y = 6f;
					drawPlayer.headPosition.Y = 6f;
					drawPlayer.bodyPosition.Y = 6f;
				}
			}
			switch (heldItem.type)
			{
			case 3182:
			case 3184:
			case 3185:
			case 3782:
				itemEffect ^= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				break;
			case 5118:
				if (player.gravDir < 0f)
				{
					itemEffect ^= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
				}
				break;
			}
			legVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.75f);
			bodyVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
			headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
			if ((drawPlayer.merman || drawPlayer.forceMerman) && !drawPlayer.hideMerman)
			{
				drawPlayer.headRotation = drawPlayer.velocity.Y * (float)drawPlayer.direction * 0.1f;
				if ((double)drawPlayer.headRotation < -0.3)
				{
					drawPlayer.headRotation = -0.3f;
				}
				if ((double)drawPlayer.headRotation > 0.3)
				{
					drawPlayer.headRotation = 0.3f;
				}
			}
			else if (!drawPlayer.dead)
			{
				drawPlayer.headRotation = 0f;
			}
			Rectangle bodyFrame = drawPlayer.bodyFrame;
			bodyFrame = drawPlayer.bodyFrame;
			bodyFrame.Y -= 336;
			if (bodyFrame.Y < 0)
			{
				bodyFrame.Y = 0;
			}
			hairFrontFrame = bodyFrame;
			hairBackFrame = bodyFrame;
			if (hideHair)
			{
				hairFrontFrame.Height = 0;
				hairBackFrame.Height = 0;
			}
			else if (backHairDraw)
			{
				int height = 26;
				hairFrontFrame.Height = height;
			}
			hidesTopSkin = drawPlayer.body == 82 || drawPlayer.body == 83 || drawPlayer.body == 93 || drawPlayer.body == 21 || drawPlayer.body == 22;
			hidesBottomSkin = drawPlayer.body == 93 || drawPlayer.legs == 20 || drawPlayer.legs == 21;
			drawFloatingTube = drawPlayer.hasFloatingTube && !hideEntirePlayer;
			drawUnicornHorn = drawPlayer.hasUnicornHorn;
			drawAngelHalo = drawPlayer.hasAngelHalo;
			drawFrontAccInNeckAccLayer = false;
			if (drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height == 5)
			{
				drawFrontAccInNeckAccLayer = drawPlayer.front > 0 && drawPlayer.front < 13 && ArmorIDs.Front.Sets.DrawsInNeckLayer[drawPlayer.front];
			}
			hairOffset = drawPlayer.GetHairDrawOffset(drawPlayer.hair, hatHair);
			helmetOffset = drawPlayer.GetHelmetDrawOffset();
			legsOffset = drawPlayer.GetLegsDrawOffset();
			CreateCompositeData();
		}

		private void AdjustmentsForWolfMount()
		{
			hideEntirePlayer = true;
			weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			Vector2 vector = new Vector2(10 + drawPlayer.direction * 14, 12f);
			Vector2 vector2 = Position + vector;
			Position.X -= drawPlayer.direction * 10;
			bool flag = drawPlayer.heldProj != -1 || heldItem.useStyle == 5;
			bool num = heldItem.useStyle == 2;
			bool flag2 = heldItem.useStyle == 9;
			bool flag3 = drawPlayer.itemAnimation > 0;
			bool flag4 = heldItem.fishingPole != 0;
			bool flag5 = heldItem.useStyle == 14;
			bool flag6 = heldItem.useStyle == 8;
			bool flag7 = heldItem.holdStyle == 1;
			bool flag8 = heldItem.holdStyle == 2;
			bool flag9 = heldItem.holdStyle == 5;
			if (num)
			{
				ItemLocation += new Vector2(drawPlayer.direction * 14, -4f);
			}
			else if (!flag4)
			{
				if (flag2)
				{
					ItemLocation += (flag3 ? new Vector2(drawPlayer.direction * 18, -4f) : new Vector2(drawPlayer.direction * 14, -18f));
				}
				else if (flag9)
				{
					ItemLocation += new Vector2(drawPlayer.direction * 17, -8f);
				}
				else if (flag7 && drawPlayer.itemAnimation == 0)
				{
					ItemLocation += new Vector2(drawPlayer.direction * 14, -6f);
				}
				else if (flag8 && drawPlayer.itemAnimation == 0)
				{
					ItemLocation += new Vector2(drawPlayer.direction * 17, 4f);
				}
				else if (flag6)
				{
					ItemLocation = vector2 + new Vector2(drawPlayer.direction * 12, 2f);
				}
				else if (flag5)
				{
					ItemLocation += new Vector2(drawPlayer.direction * 5, -2f);
				}
				else if (flag)
				{
					ItemLocation += new Vector2(drawPlayer.direction * 4, -4f);
				}
				else
				{
					ItemLocation = vector2;
				}
			}
		}

		private void CreateCompositeData()
		{
			frontShoulderOffset = Vector2.Zero;
			backShoulderOffset = Vector2.Zero;
			usesCompositeTorso = drawPlayer.body > 0 && drawPlayer.body < 248 && ArmorIDs.Body.Sets.UsesNewFramingCode[drawPlayer.body];
			usesCompositeFrontHandAcc = drawPlayer.handon > 0 && drawPlayer.handon < 24 && ArmorIDs.HandOn.Sets.UsesNewFramingCode[drawPlayer.handon];
			usesCompositeBackHandAcc = drawPlayer.handoff > 0 && drawPlayer.handoff < 16 && ArmorIDs.HandOff.Sets.UsesNewFramingCode[drawPlayer.handoff];
			if (drawPlayer.body < 1)
			{
				usesCompositeTorso = true;
			}
			if (!usesCompositeTorso)
			{
				return;
			}
			Point pt = new Point(1, 1);
			Point pt2 = new Point(0, 1);
			Point pt3 = default(Point);
			Point frameIndex = default(Point);
			Point frameIndex2 = default(Point);
			int num = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
			compShoulderOverFrontArm = true;
			hideCompositeShoulders = false;
			bool flag = true;
			if (drawPlayer.body > 0)
			{
				flag = ArmorIDs.Body.Sets.showsShouldersWhileJumping[drawPlayer.body];
			}
			bool flag2 = false;
			if (drawPlayer.handon > 0)
			{
				flag2 = ArmorIDs.HandOn.Sets.UsesOldFramingTexturesForWalking[drawPlayer.handon];
			}
			bool flag3 = !flag2;
			switch (num)
			{
			case 0:
				frameIndex2.X = 2;
				flag3 = true;
				break;
			case 1:
				frameIndex2.X = 3;
				compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 2:
				frameIndex2.X = 4;
				compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 3:
				frameIndex2.X = 5;
				compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 4:
				frameIndex2.X = 6;
				compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 5:
				frameIndex2.X = 2;
				frameIndex2.Y = 1;
				pt3.X = 1;
				compShoulderOverFrontArm = false;
				flag3 = true;
				if (!flag)
				{
					hideCompositeShoulders = true;
				}
				break;
			case 6:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frameIndex2.X = 4;
				frameIndex2.Y = 1;
				break;
			case 11:
			case 12:
			case 13:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			case 14:
				frameIndex2.X = 5;
				frameIndex2.Y = 1;
				break;
			case 15:
			case 16:
				frameIndex2.X = 6;
				frameIndex2.Y = 1;
				break;
			case 17:
				frameIndex2.X = 5;
				frameIndex2.Y = 1;
				break;
			case 18:
			case 19:
				frameIndex2.X = 3;
				frameIndex2.Y = 1;
				break;
			}
			CreateCompositeData_DetermineShoulderOffsets(drawPlayer.body, num);
			backShoulderOffset *= new Vector2(drawPlayer.direction, drawPlayer.gravDir);
			frontShoulderOffset *= new Vector2(drawPlayer.direction, drawPlayer.gravDir);
			if (drawPlayer.body > 0 && ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[drawPlayer.body])
			{
				compShoulderOverFrontArm = false;
			}
			usesCompositeFrontHandAcc = flag3;
			frameIndex.X = frameIndex2.X;
			frameIndex.Y = frameIndex2.Y + 2;
			UpdateCompositeArm(drawPlayer.compositeFrontArm, ref compositeFrontArmRotation, ref frameIndex2, 7);
			UpdateCompositeArm(drawPlayer.compositeBackArm, ref compositeBackArmRotation, ref frameIndex, 8);
			if (!drawPlayer.Male)
			{
				pt.Y += 2;
				pt2.Y += 2;
				pt3.Y += 2;
			}
			compBackShoulderFrame = CreateCompositeFrameRect(pt);
			compFrontShoulderFrame = CreateCompositeFrameRect(pt2);
			compBackArmFrame = CreateCompositeFrameRect(frameIndex);
			compFrontArmFrame = CreateCompositeFrameRect(frameIndex2);
			compTorsoFrame = CreateCompositeFrameRect(pt3);
		}

		private void CreateCompositeData_DetermineShoulderOffsets(int armor, int targetFrameNumber)
		{
			int num = 0;
			switch (armor)
			{
			case 55:
				num = 1;
				break;
			case 71:
				num = 2;
				break;
			case 204:
				num = 3;
				break;
			case 183:
				num = 4;
				break;
			case 201:
				num = 5;
				break;
			case 101:
				num = 6;
				break;
			case 207:
				num = 7;
				break;
			}
			switch (num)
			{
			case 1:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			case 2:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			case 3:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 4:
				switch (targetFrameNumber)
				{
				case 6:
					frontShoulderOffset.X = -2f;
					break;
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -4f;
					break;
				case 11:
				case 12:
				case 13:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
					frontShoulderOffset.X = 2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 14:
				case 17:
					break;
				}
				break;
			case 5:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 6:
				switch (targetFrameNumber)
				{
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 14:
				case 15:
				case 16:
				case 17:
					frontShoulderOffset.X = 2f;
					break;
				}
				break;
			case 7:
				switch (targetFrameNumber)
				{
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					frontShoulderOffset.X = -2f;
					break;
				case 11:
				case 12:
				case 13:
				case 14:
					frontShoulderOffset.X = -2f;
					break;
				case 18:
				case 19:
					frontShoulderOffset.X = -2f;
					break;
				case 15:
				case 16:
				case 17:
					break;
				}
				break;
			}
		}

		private Rectangle CreateCompositeFrameRect(Point pt)
		{
			return new Rectangle(pt.X * 40, pt.Y * 56, 40, 56);
		}

		private void UpdateCompositeArm(Player.CompositeArmData data, ref float rotation, ref Point frameIndex, int targetX)
		{
			if (data.enabled)
			{
				rotation = data.rotation;
				switch (data.stretch)
				{
				case Player.CompositeArmStretchAmount.Full:
					frameIndex.X = targetX;
					frameIndex.Y = 0;
					break;
				case Player.CompositeArmStretchAmount.ThreeQuarters:
					frameIndex.X = targetX;
					frameIndex.Y = 1;
					break;
				case Player.CompositeArmStretchAmount.Quarter:
					frameIndex.X = targetX;
					frameIndex.Y = 2;
					break;
				case Player.CompositeArmStretchAmount.None:
					frameIndex.X = targetX;
					frameIndex.Y = 3;
					break;
				}
			}
			else
			{
				rotation = 0f;
			}
		}
	}
}
