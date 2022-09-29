using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.Initializers
{
	public static class PlayerDataInitializer
	{
		public static void Load()
		{
			TextureAssets.Players = new Asset<Texture2D>[12, 16];
			LoadStarterMale();
			LoadStarterFemale();
			LoadStickerMale();
			LoadStickerFemale();
			LoadGangsterMale();
			LoadGangsterFemale();
			LoadCoatMale();
			LoadDressFemale();
			LoadDressMale();
			LoadCoatFemale();
			LoadDisplayDollMale();
			LoadDisplayDollFemale();
		}

		private static void LoadVariant(int ID, int[] pieceIDs)
		{
			for (int i = 0; i < pieceIDs.Length; i++)
			{
				TextureAssets.Players[ID, pieceIDs[i]] = Main.Assets.Request<Texture2D>("Images/Player_" + ID + "_" + pieceIDs[i], (AssetRequestMode)2);
			}
		}

		private static void CopyVariant(int to, int from)
		{
			for (int i = 0; i < 16; i++)
			{
				TextureAssets.Players[to, i] = TextureAssets.Players[from, i];
			}
		}

		private static void LoadStarterMale()
		{
			LoadVariant(0, new int[15]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				10, 11, 12, 13, 15
			});
			TextureAssets.Players[0, 14] = Asset<Texture2D>.Empty;
		}

		private static void LoadStickerMale()
		{
			CopyVariant(1, 0);
			LoadVariant(1, new int[6] { 4, 6, 8, 11, 12, 13 });
		}

		private static void LoadGangsterMale()
		{
			CopyVariant(2, 0);
			LoadVariant(2, new int[6] { 4, 6, 8, 11, 12, 13 });
		}

		private static void LoadCoatMale()
		{
			CopyVariant(3, 0);
			LoadVariant(3, new int[7] { 4, 6, 8, 11, 12, 13, 14 });
		}

		private static void LoadDressMale()
		{
			CopyVariant(8, 0);
			LoadVariant(8, new int[7] { 4, 6, 8, 11, 12, 13, 14 });
		}

		private static void LoadStarterFemale()
		{
			CopyVariant(4, 0);
			LoadVariant(4, new int[11]
			{
				3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
				13
			});
		}

		private static void LoadStickerFemale()
		{
			CopyVariant(5, 4);
			LoadVariant(5, new int[6] { 4, 6, 8, 11, 12, 13 });
		}

		private static void LoadGangsterFemale()
		{
			CopyVariant(6, 4);
			LoadVariant(6, new int[6] { 4, 6, 8, 11, 12, 13 });
		}

		private static void LoadCoatFemale()
		{
			CopyVariant(7, 4);
			LoadVariant(7, new int[7] { 4, 6, 8, 11, 12, 13, 14 });
		}

		private static void LoadDressFemale()
		{
			CopyVariant(9, 4);
			LoadVariant(9, new int[6] { 4, 6, 8, 11, 12, 13 });
		}

		private static void LoadDisplayDollMale()
		{
			CopyVariant(10, 0);
			LoadVariant(10, new int[7] { 0, 2, 3, 5, 7, 9, 10 });
			Asset<Texture2D> val = TextureAssets.Players[10, 2];
			TextureAssets.Players[10, 2] = val;
			TextureAssets.Players[10, 1] = val;
			TextureAssets.Players[10, 4] = val;
			TextureAssets.Players[10, 6] = val;
			TextureAssets.Players[10, 11] = val;
			TextureAssets.Players[10, 12] = val;
			TextureAssets.Players[10, 13] = val;
			TextureAssets.Players[10, 8] = val;
			TextureAssets.Players[10, 15] = val;
		}

		private static void LoadDisplayDollFemale()
		{
			CopyVariant(11, 10);
			LoadVariant(11, new int[5] { 3, 5, 7, 9, 10 });
			Asset<Texture2D> val = TextureAssets.Players[10, 2];
			TextureAssets.Players[11, 2] = val;
			TextureAssets.Players[11, 1] = val;
			TextureAssets.Players[11, 4] = val;
			TextureAssets.Players[11, 6] = val;
			TextureAssets.Players[11, 11] = val;
			TextureAssets.Players[11, 12] = val;
			TextureAssets.Players[11, 13] = val;
			TextureAssets.Players[11, 8] = val;
			TextureAssets.Players[11, 15] = val;
		}
	}
}
