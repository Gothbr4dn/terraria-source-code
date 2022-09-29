using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	public class FancyClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
	{
		private float _currentPlayerLife;

		private float _lifePerHeart;

		private int _playerLifeFruitCount;

		private int _lastHeartFillingIndex;

		private int _lastHeartPanelIndex;

		private int _heartCountRow1;

		private int _heartCountRow2;

		private int _starCount;

		private int _lastStarFillingIndex;

		private float _manaPerStar;

		private float _currentPlayerMana;

		private Asset<Texture2D> _heartLeft;

		private Asset<Texture2D> _heartMiddle;

		private Asset<Texture2D> _heartRight;

		private Asset<Texture2D> _heartRightFancy;

		private Asset<Texture2D> _heartFill;

		private Asset<Texture2D> _heartFillHoney;

		private Asset<Texture2D> _heartSingleFancy;

		private Asset<Texture2D> _starTop;

		private Asset<Texture2D> _starMiddle;

		private Asset<Texture2D> _starBottom;

		private Asset<Texture2D> _starSingle;

		private Asset<Texture2D> _starFill;

		private bool _hoverLife;

		private bool _hoverMana;

		private bool _drawText;

		public string NameKey { get; private set; }

		public string ConfigKey { get; private set; }

		public FancyClassicPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			NameKey = nameKey;
			ConfigKey = configKey;
			if (configKey == "NewWithText")
			{
				_drawText = true;
			}
			else
			{
				_drawText = false;
			}
			string text = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
			_heartLeft = Main.Assets.Request<Texture2D>(text + "\\Heart_Left", mode);
			_heartMiddle = Main.Assets.Request<Texture2D>(text + "\\Heart_Middle", mode);
			_heartRight = Main.Assets.Request<Texture2D>(text + "\\Heart_Right", mode);
			_heartRightFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Right_Fancy", mode);
			_heartFill = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill", mode);
			_heartFillHoney = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill_B", mode);
			_heartSingleFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Single_Fancy", mode);
			_starTop = Main.Assets.Request<Texture2D>(text + "\\Star_A", mode);
			_starMiddle = Main.Assets.Request<Texture2D>(text + "\\Star_B", mode);
			_starBottom = Main.Assets.Request<Texture2D>(text + "\\Star_C", mode);
			_starSingle = Main.Assets.Request<Texture2D>(text + "\\Star_Single", mode);
			_starFill = Main.Assets.Request<Texture2D>(text + "\\Star_Fill", mode);
		}

		public void Draw()
		{
			Player localPlayer = Main.LocalPlayer;
			SpriteBatch spriteBatch = Main.spriteBatch;
			PrepareFields(localPlayer);
			DrawLifeBar(spriteBatch);
			DrawManaBar(spriteBatch);
		}

		private void DrawLifeBar(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(Main.screenWidth - 300 + 4, 15f);
			if (_drawText)
			{
				vector.Y += 6f;
				DrawLifeBarText(spriteBatch, vector + new Vector2(-4f, 3f));
			}
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f);
			resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f) + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_hoverLife = isHovered;
		}

		private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
		{
			Vector2 vector = topLeftAnchor + new Vector2(130f, -24f);
			Player localPlayer = Main.LocalPlayer;
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
			Vector2 vector2 = FontAssets.MouseText.get_Value().MeasureString(text);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), Lang.inter[0].Value, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), localPlayer.statLife + "/" + localPlayer.statLifeMax2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.get_Value().MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
		}

		private void DrawManaBar(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(Main.screenWidth - 40, 22f);
			_ = _starCount;
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = StarPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 16f);
			resourceDrawSettings.GetTextureMethod = StarFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitY * -2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_hoverMana = isHovered;
		}

		private static void DrawManaText(SpriteBatch spriteBatch)
		{
			Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(Lang.inter[2].Value);
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			int num = 50;
			if (vector.X >= 45f)
			{
				num = (int)vector.X + 5;
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), Lang.inter[2].Value, new Vector2(Main.screenWidth - num, 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		}

		private void HeartPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _heartLeft;
			drawScale = 1f;
			if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
			{
				sprite = _heartSingleFancy;
				offset = new Vector2(-4f, -4f);
			}
			else if (elementIndex == lastElementIndex && lastElementIndex == _lastHeartPanelIndex)
			{
				sprite = _heartRightFancy;
				offset = new Vector2(-8f, -4f);
			}
			else if (elementIndex == lastElementIndex)
			{
				sprite = _heartRight;
			}
			else if (elementIndex != firstElementIndex)
			{
				sprite = _heartMiddle;
			}
		}

		private void HeartFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _heartLeft;
			if (elementIndex < _playerLifeFruitCount)
			{
				sprite = _heartFillHoney;
			}
			else
			{
				sprite = _heartFill;
			}
			float num = (drawScale = Utils.GetLerpValue(_lifePerHeart * (float)elementIndex, _lifePerHeart * (float)(elementIndex + 1), _currentPlayerLife, clamped: true));
			if (elementIndex == _lastHeartFillingIndex && num > 0f)
			{
				drawScale += Main.cursorScale - 1f;
			}
		}

		private void StarPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _starTop;
			drawScale = 1f;
			if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
			{
				sprite = _starSingle;
			}
			else if (elementIndex == lastElementIndex)
			{
				sprite = _starBottom;
				offset = new Vector2(0f, 0f);
			}
			else if (elementIndex != firstElementIndex)
			{
				sprite = _starMiddle;
			}
		}

		private void StarFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
		{
			sourceRect = null;
			offset = Vector2.Zero;
			sprite = _starFill;
			float num = (drawScale = Utils.GetLerpValue(_manaPerStar * (float)elementIndex, _manaPerStar * (float)(elementIndex + 1), _currentPlayerMana, clamped: true));
			if (elementIndex == _lastStarFillingIndex && num > 0f)
			{
				drawScale += Main.cursorScale - 1f;
			}
		}

		private void PrepareFields(Player player)
		{
			PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
			_playerLifeFruitCount = playerStatsSnapshot.LifeFruitCount;
			_lifePerHeart = playerStatsSnapshot.LifePerSegment;
			_currentPlayerLife = playerStatsSnapshot.Life;
			_manaPerStar = playerStatsSnapshot.ManaPerSegment;
			_heartCountRow1 = Utils.Clamp((int)((float)playerStatsSnapshot.LifeMax / _lifePerHeart), 0, 10);
			_heartCountRow2 = Utils.Clamp((int)((float)(playerStatsSnapshot.LifeMax - 200) / _lifePerHeart), 0, 10);
			int num = (_lastHeartFillingIndex = (int)((float)playerStatsSnapshot.Life / _lifePerHeart));
			_lastHeartPanelIndex = _heartCountRow1 + _heartCountRow2 - 1;
			_starCount = (int)((float)playerStatsSnapshot.ManaMax / _manaPerStar);
			_currentPlayerMana = playerStatsSnapshot.Mana;
			_lastStarFillingIndex = (int)(_currentPlayerMana / _manaPerStar);
		}

		public void TryToHover()
		{
			if (_hoverLife)
			{
				CommonResourceBarMethods.DrawLifeMouseOver();
			}
			if (_hoverMana)
			{
				CommonResourceBarMethods.DrawManaMouseOver();
			}
		}
	}
}
