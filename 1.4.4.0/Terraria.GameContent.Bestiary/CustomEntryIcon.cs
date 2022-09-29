using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	public class CustomEntryIcon : IEntryIcon
	{
		private LocalizedText _text;

		private Asset<Texture2D> _textureAsset;

		private Rectangle _sourceRectangle;

		private Func<bool> _unlockCondition;

		public CustomEntryIcon(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
		{
			_text = Language.GetText(nameLanguageKey);
			_textureAsset = Main.Assets.Request<Texture2D>(texturePath, (AssetRequestMode)1);
			_unlockCondition = unlockCondition;
			UpdateUnlockState(state: false);
		}

		public IEntryIcon CreateClone()
		{
			return new CustomEntryIcon(_text.Key, _textureAsset.get_Name(), _unlockCondition);
		}

		public void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings)
		{
			UpdateUnlockState(GetUnlockState(providedInfo));
		}

		public void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
		{
			Rectangle iconbox = settings.iconbox;
			spriteBatch.Draw(_textureAsset.get_Value(), iconbox.Center.ToVector2() + Vector2.One, _sourceRectangle, Color.White, 0f, _sourceRectangle.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		public string GetHoverText(BestiaryUICollectionInfo providedInfo)
		{
			if (GetUnlockState(providedInfo))
			{
				return _text.Value;
			}
			return "???";
		}

		private void UpdateUnlockState(bool state)
		{
			_sourceRectangle = _textureAsset.Frame(2, 1, state.ToInt());
			_sourceRectangle.Inflate(-2, -2);
		}

		public bool GetUnlockState(BestiaryUICollectionInfo providedInfo)
		{
			return providedInfo.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
		}
	}
}
