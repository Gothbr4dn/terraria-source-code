using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.DataStructures
{
	public struct PlayerDrawHeadSet
	{
		public List<DrawData> DrawData;

		public List<int> Dust;

		public List<int> Gore;

		public Player drawPlayer;

		public int cHead;

		public int cFace;

		public int cFaceHead;

		public int cFaceFlower;

		public int cUnicornHorn;

		public int cAngelHalo;

		public int cBeard;

		public int skinVar;

		public int hairShaderPacked;

		public int skinDyePacked;

		public float scale;

		public Color colorEyeWhites;

		public Color colorEyes;

		public Color colorHair;

		public Color colorHead;

		public Color colorArmorHead;

		public Color colorDisplayDollSkin;

		public SpriteEffects playerEffect;

		public Vector2 headVect;

		public Rectangle bodyFrameMemory;

		public bool fullHair;

		public bool hatHair;

		public bool hideHair;

		public bool helmetIsTall;

		public bool helmetIsOverFullHair;

		public bool helmetIsNormal;

		public bool drawUnicornHorn;

		public bool drawAngelHalo;

		public Vector2 Position;

		public Vector2 hairOffset;

		public Vector2 helmetOffset;

		public Rectangle HairFrame
		{
			get
			{
				Rectangle result = bodyFrameMemory;
				result.Height--;
				return result;
			}
		}

		public void BoringSetup(Player drawPlayer2, List<DrawData> drawData, List<int> dust, List<int> gore, float X, float Y, float Alpha, float Scale)
		{
			DrawData = drawData;
			Dust = dust;
			Gore = gore;
			drawPlayer = drawPlayer2;
			Position = drawPlayer.position;
			cHead = 0;
			cFace = 0;
			cUnicornHorn = 0;
			cAngelHalo = 0;
			cBeard = 0;
			drawUnicornHorn = false;
			drawAngelHalo = false;
			skinVar = drawPlayer.skinVariant;
			hairShaderPacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
			{
				hairShaderPacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			skinDyePacked = drawPlayer.skinDyePacked;
			if (drawPlayer.face > 0 && drawPlayer.face < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			cHead = drawPlayer.cHead;
			cFace = drawPlayer.cFace;
			cFaceHead = drawPlayer.cFaceHead;
			cFaceFlower = drawPlayer.cFaceFlower;
			cUnicornHorn = drawPlayer.cUnicornHorn;
			cAngelHalo = drawPlayer.cAngelHalo;
			cBeard = drawPlayer.cBeard;
			drawUnicornHorn = drawPlayer.hasUnicornHorn;
			drawAngelHalo = drawPlayer.hasAngelHalo;
			Main.instance.LoadHair(drawPlayer.hair);
			scale = Scale;
			colorEyeWhites = Main.quickAlpha(Color.White, Alpha);
			colorEyes = Main.quickAlpha(drawPlayer.eyeColor, Alpha);
			colorHair = Main.quickAlpha(drawPlayer.GetHairColor(useLighting: false), Alpha);
			colorHead = Main.quickAlpha(drawPlayer.skinColor, Alpha);
			colorArmorHead = Main.quickAlpha(Color.White, Alpha);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Main.quickAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, Alpha);
			}
			else
			{
				colorDisplayDollSkin = colorHead;
			}
			playerEffect = SpriteEffects.None;
			if (drawPlayer.direction < 0)
			{
				playerEffect = SpriteEffects.FlipHorizontally;
			}
			headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
			bodyFrameMemory = drawPlayer.bodyFrame;
			bodyFrameMemory.Y = 0;
			Position = Main.screenPosition;
			Position.X += X;
			Position.Y += Y;
			Position.X -= 6f;
			Position.Y -= 4f;
			Position.Y -= drawPlayer.HeightMapOffset;
			if (drawPlayer.head > 0 && drawPlayer.head < 282)
			{
				Main.instance.LoadArmorHead(drawPlayer.head);
				int num = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
				if (num >= 0)
				{
					Main.instance.LoadArmorHead(num);
				}
			}
			if (drawPlayer.face > 0 && drawPlayer.face < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.face);
			}
			if (drawPlayer.faceHead > 0 && drawPlayer.faceHead < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.faceHead);
			}
			if (drawPlayer.faceFlower > 0 && drawPlayer.faceFlower < 22)
			{
				Main.instance.LoadAccFace(drawPlayer.faceFlower);
			}
			if (drawPlayer.beard > 0 && drawPlayer.beard < 5)
			{
				Main.instance.LoadAccBeard(drawPlayer.beard);
			}
			drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out var _, out helmetIsOverFullHair);
			hairOffset = drawPlayer.GetHairDrawOffset(drawPlayer.hair, hatHair);
			hairOffset.Y *= drawPlayer.Directions.Y;
			helmetOffset = drawPlayer.GetHelmetDrawOffset();
			helmetOffset.Y *= drawPlayer.Directions.Y;
			helmetIsTall = drawPlayer.head == 14 || drawPlayer.head == 56 || drawPlayer.head == 158;
			helmetIsNormal = !helmetIsTall && !helmetIsOverFullHair && drawPlayer.head > 0 && drawPlayer.head < 282 && drawPlayer.head != 28;
		}
	}
}
