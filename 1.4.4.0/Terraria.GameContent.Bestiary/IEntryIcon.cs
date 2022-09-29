using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Bestiary
{
	public interface IEntryIcon
	{
		void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings);

		void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings);

		bool GetUnlockState(BestiaryUICollectionInfo providedInfo);

		string GetHoverText(BestiaryUICollectionInfo providedInfo);

		IEntryIcon CreateClone();
	}
}
