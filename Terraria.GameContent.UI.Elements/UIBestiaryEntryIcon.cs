using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryEntryIcon : UIElement
	{
		private BestiaryEntry _entry;

		private Asset<Texture2D> _notUnlockedTexture;

		private bool _isPortrait;

		public bool ForceHover;

		private BestiaryUICollectionInfo _collectionInfo;

		public UIBestiaryEntryIcon(BestiaryEntry entry, bool isPortrait)
		{
			_entry = entry;
			IgnoresMouseInteraction = true;
			OverrideSamplerState = Main.DefaultSamplerState;
			UseImmediateMode = true;
			Width.Set(0f, 1f);
			Height.Set(0f, 1f);
			_notUnlockedTexture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked", (AssetRequestMode)1);
			_isPortrait = isPortrait;
			_collectionInfo = _entry.UIInfoProvider.GetEntryUICollectionInfo();
		}

		public override void Update(GameTime gameTime)
		{
			_collectionInfo = _entry.UIInfoProvider.GetEntryUICollectionInfo();
			CalculatedStyle dimensions = GetDimensions();
			bool isHovered = base.IsMouseHovering || ForceHover;
			_entry.Icon.Update(_collectionInfo, dimensions.ToRectangle(), new EntryIconDrawSettings
			{
				iconbox = dimensions.ToRectangle(),
				IsPortrait = _isPortrait,
				IsHovered = isHovered
			});
			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			bool unlockState = _entry.Icon.GetUnlockState(_collectionInfo);
			bool isHovered = base.IsMouseHovering || ForceHover;
			if (unlockState)
			{
				_entry.Icon.Draw(_collectionInfo, spriteBatch, new EntryIconDrawSettings
				{
					iconbox = dimensions.ToRectangle(),
					IsPortrait = _isPortrait,
					IsHovered = isHovered
				});
			}
			else
			{
				Texture2D value = _notUnlockedTexture.get_Value();
				spriteBatch.Draw(value, dimensions.Center(), null, Color.White * 0.15f, 0f, value.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
		}

		public string GetHoverText()
		{
			return _entry.Icon.GetHoverText(_collectionInfo);
		}
	}
}
