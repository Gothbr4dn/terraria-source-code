using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Graphics;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.Utilities;

namespace Terraria.Initializers
{
	public static class AssetInitializer
	{
		public static void CreateAssetServices(GameServiceContainer services)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			AssetReaderCollection val = new AssetReaderCollection();
			val.RegisterReader((IAssetReader)new PngReader(XnaExtensions.Get<IGraphicsDeviceService>((IServiceProvider)services).GraphicsDevice), new string[1] { ".png" });
			val.RegisterReader((IAssetReader)new XnbReader((IServiceProvider)services), new string[1] { ".xnb" });
			AsyncAssetLoader val2 = new AsyncAssetLoader(val, 20);
			val2.RequireTypeCreationOnTransfer(typeof(Texture2D));
			val2.RequireTypeCreationOnTransfer(typeof(DynamicSpriteFont));
			val2.RequireTypeCreationOnTransfer(typeof(SpriteFont));
			IAssetRepository provider = (IAssetRepository)new AssetRepository((IAssetLoader)new AssetLoader(val), (IAsyncAssetLoader)(object)val2);
			services.AddService(typeof(AssetReaderCollection), val);
			services.AddService(typeof(IAssetRepository), provider);
		}

		public static ResourcePackList CreateResourcePackList(IServiceProvider services)
		{
			GetResourcePacksFolderPathAndConfirmItExists(out var resourcePackJson, out var resourcePackFolder);
			return ResourcePackList.FromJson(resourcePackJson, services, resourcePackFolder);
		}

		public static ResourcePackList CreatePublishableResourcePacksList(IServiceProvider services)
		{
			GetResourcePacksFolderPathAndConfirmItExists(out var resourcePackJson, out var resourcePackFolder);
			return ResourcePackList.Publishable(resourcePackJson, services, resourcePackFolder);
		}

		public static void GetResourcePacksFolderPathAndConfirmItExists(out JArray resourcePackJson, out string resourcePackFolder)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			resourcePackJson = Main.Configuration.Get<JArray>("ResourcePacks", new JArray());
			resourcePackFolder = Path.Combine(Main.SavePath, "ResourcePacks");
			Utils.TryCreatingDirectory(resourcePackFolder);
		}

		public static void LoadSplashAssets(bool asyncLoadForSounds)
		{
			TextureAssets.SplashTexture16x9 = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_1", (AssetRequestMode)1);
			TextureAssets.SplashTexture4x3 = LoadAsset<Texture2D>("Images\\logo_" + new UnifiedRandom().Next(1, 9), (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoResonanace = LoadAsset<Texture2D>("Images\\SplashScreens\\ResonanceArray", (AssetRequestMode)1);
			int num = new UnifiedRandom().Next(1, 11);
			TextureAssets.SplashTextureLegoBack = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_0", (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoTree = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_1", (AssetRequestMode)1);
			TextureAssets.SplashTextureLegoFront = LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_2", (AssetRequestMode)1);
			TextureAssets.Item[75] = LoadAsset<Texture2D>("Images\\Item_" + (short)75, (AssetRequestMode)1);
			TextureAssets.LoadingSunflower = LoadAsset<Texture2D>("Images\\UI\\Sunflower_Loading", (AssetRequestMode)1);
		}

		public static void LoadAssetsWhileInInitialBlackScreen()
		{
			LoadFonts((AssetRequestMode)1);
			LoadTextures((AssetRequestMode)1);
			LoadRenderTargetAssets((AssetRequestMode)1);
			LoadSounds((AssetRequestMode)1);
		}

		public static void Load(bool asyncLoad)
		{
		}

		private static void LoadFonts(AssetRequestMode mode)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			FontAssets.ItemStack = LoadAsset<DynamicSpriteFont>("Fonts/Item_Stack", mode);
			FontAssets.MouseText = LoadAsset<DynamicSpriteFont>("Fonts/Mouse_Text", mode);
			FontAssets.DeathText = LoadAsset<DynamicSpriteFont>("Fonts/Death_Text", mode);
			FontAssets.CombatText[0] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Text", mode);
			FontAssets.CombatText[1] = LoadAsset<DynamicSpriteFont>("Fonts/Combat_Crit", mode);
		}

		private static void LoadSounds(AssetRequestMode mode)
		{
			SoundEngine.Load(Main.instance.Services);
		}

		private static void LoadRenderTargetAssets(AssetRequestMode mode)
		{
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerRainbowWings = new PlayerRainbowWingsTextureContent());
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerTitaniumStormBuff = new PlayerTitaniumStormBuffTextureContent());
			RegisterRenderTargetAsset(TextureAssets.RenderTargets.QueenSlimeMount = new PlayerQueenSlimeMountTextureContent());
		}

		private static void RegisterRenderTargetAsset(INeedRenderTargetContent content)
		{
			Main.ContentThatNeedsRenderTargets.Add(content);
		}

		private static void LoadTextures(AssetRequestMode mode)
		{
			//IL_0659: Unknown result type (might be due to invalid IL or missing references)
			//IL_066f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_068d: Unknown result type (might be due to invalid IL or missing references)
			//IL_069d: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_070a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0727: Unknown result type (might be due to invalid IL or missing references)
			//IL_074f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0784: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0823: Unknown result type (might be due to invalid IL or missing references)
			//IL_0858: Unknown result type (might be due to invalid IL or missing references)
			//IL_088d: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_08df: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_090f: Unknown result type (might be due to invalid IL or missing references)
			//IL_091f: Unknown result type (might be due to invalid IL or missing references)
			//IL_092f: Unknown result type (might be due to invalid IL or missing references)
			//IL_093f: Unknown result type (might be due to invalid IL or missing references)
			//IL_094f: Unknown result type (might be due to invalid IL or missing references)
			//IL_095f: Unknown result type (might be due to invalid IL or missing references)
			//IL_096f: Unknown result type (might be due to invalid IL or missing references)
			//IL_097f: Unknown result type (might be due to invalid IL or missing references)
			//IL_098f: Unknown result type (might be due to invalid IL or missing references)
			//IL_099f: Unknown result type (might be due to invalid IL or missing references)
			//IL_09af: Unknown result type (might be due to invalid IL or missing references)
			//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_09df: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cf3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d57: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dde: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e02: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e3e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
			//IL_100c: Unknown result type (might be due to invalid IL or missing references)
			//IL_101c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1032: Unknown result type (might be due to invalid IL or missing references)
			//IL_1044: Unknown result type (might be due to invalid IL or missing references)
			//IL_1056: Unknown result type (might be due to invalid IL or missing references)
			//IL_1068: Unknown result type (might be due to invalid IL or missing references)
			//IL_1074: Unknown result type (might be due to invalid IL or missing references)
			//IL_1084: Unknown result type (might be due to invalid IL or missing references)
			//IL_1094: Unknown result type (might be due to invalid IL or missing references)
			//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1104: Unknown result type (might be due to invalid IL or missing references)
			//IL_1116: Unknown result type (might be due to invalid IL or missing references)
			//IL_1128: Unknown result type (might be due to invalid IL or missing references)
			//IL_113a: Unknown result type (might be due to invalid IL or missing references)
			//IL_114c: Unknown result type (might be due to invalid IL or missing references)
			//IL_115e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1170: Unknown result type (might be due to invalid IL or missing references)
			//IL_1182: Unknown result type (might be due to invalid IL or missing references)
			//IL_11a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11db: Unknown result type (might be due to invalid IL or missing references)
			//IL_120b: Unknown result type (might be due to invalid IL or missing references)
			//IL_123a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1259: Unknown result type (might be due to invalid IL or missing references)
			//IL_1286: Unknown result type (might be due to invalid IL or missing references)
			//IL_1298: Unknown result type (might be due to invalid IL or missing references)
			//IL_12aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_12e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_12fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_130c: Unknown result type (might be due to invalid IL or missing references)
			//IL_131c: Unknown result type (might be due to invalid IL or missing references)
			//IL_132c: Unknown result type (might be due to invalid IL or missing references)
			//IL_133c: Unknown result type (might be due to invalid IL or missing references)
			//IL_134c: Unknown result type (might be due to invalid IL or missing references)
			//IL_135c: Unknown result type (might be due to invalid IL or missing references)
			//IL_136c: Unknown result type (might be due to invalid IL or missing references)
			//IL_137c: Unknown result type (might be due to invalid IL or missing references)
			//IL_138c: Unknown result type (might be due to invalid IL or missing references)
			//IL_139c: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_13bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_13dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_1414: Unknown result type (might be due to invalid IL or missing references)
			//IL_1431: Unknown result type (might be due to invalid IL or missing references)
			//IL_1441: Unknown result type (might be due to invalid IL or missing references)
			//IL_1451: Unknown result type (might be due to invalid IL or missing references)
			//IL_1461: Unknown result type (might be due to invalid IL or missing references)
			//IL_1477: Unknown result type (might be due to invalid IL or missing references)
			//IL_1489: Unknown result type (might be due to invalid IL or missing references)
			//IL_149b: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_14bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_14d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_150f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1537: Unknown result type (might be due to invalid IL or missing references)
			//IL_156c: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_15d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_15f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1603: Unknown result type (might be due to invalid IL or missing references)
			//IL_1613: Unknown result type (might be due to invalid IL or missing references)
			//IL_1623: Unknown result type (might be due to invalid IL or missing references)
			//IL_1633: Unknown result type (might be due to invalid IL or missing references)
			//IL_1643: Unknown result type (might be due to invalid IL or missing references)
			//IL_1653: Unknown result type (might be due to invalid IL or missing references)
			//IL_1663: Unknown result type (might be due to invalid IL or missing references)
			//IL_1673: Unknown result type (might be due to invalid IL or missing references)
			//IL_1683: Unknown result type (might be due to invalid IL or missing references)
			//IL_1693: Unknown result type (might be due to invalid IL or missing references)
			//IL_16a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_16f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1703: Unknown result type (might be due to invalid IL or missing references)
			//IL_1713: Unknown result type (might be due to invalid IL or missing references)
			//IL_1723: Unknown result type (might be due to invalid IL or missing references)
			//IL_1733: Unknown result type (might be due to invalid IL or missing references)
			//IL_1743: Unknown result type (might be due to invalid IL or missing references)
			//IL_1753: Unknown result type (might be due to invalid IL or missing references)
			//IL_1763: Unknown result type (might be due to invalid IL or missing references)
			//IL_1773: Unknown result type (might be due to invalid IL or missing references)
			//IL_1783: Unknown result type (might be due to invalid IL or missing references)
			//IL_1793: Unknown result type (might be due to invalid IL or missing references)
			//IL_17a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_17b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_17c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_17d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_17e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_17f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1808: Unknown result type (might be due to invalid IL or missing references)
			//IL_1818: Unknown result type (might be due to invalid IL or missing references)
			//IL_1828: Unknown result type (might be due to invalid IL or missing references)
			//IL_1838: Unknown result type (might be due to invalid IL or missing references)
			//IL_1848: Unknown result type (might be due to invalid IL or missing references)
			//IL_1858: Unknown result type (might be due to invalid IL or missing references)
			//IL_1868: Unknown result type (might be due to invalid IL or missing references)
			//IL_1890: Unknown result type (might be due to invalid IL or missing references)
			//IL_18a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_18f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1908: Unknown result type (might be due to invalid IL or missing references)
			//IL_1918: Unknown result type (might be due to invalid IL or missing references)
			//IL_1928: Unknown result type (might be due to invalid IL or missing references)
			//IL_1938: Unknown result type (might be due to invalid IL or missing references)
			//IL_1948: Unknown result type (might be due to invalid IL or missing references)
			//IL_1958: Unknown result type (might be due to invalid IL or missing references)
			//IL_1968: Unknown result type (might be due to invalid IL or missing references)
			//IL_1978: Unknown result type (might be due to invalid IL or missing references)
			//IL_1988: Unknown result type (might be due to invalid IL or missing references)
			//IL_1998: Unknown result type (might be due to invalid IL or missing references)
			//IL_19a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_19b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_19d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_19f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a08: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a18: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a28: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a38: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a48: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a58: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a68: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a78: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a88: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a98: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ab8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ac8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ad8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ae8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b08: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b18: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b28: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b38: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b48: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b68: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b78: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b88: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b98: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ba8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1bb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c15: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c32: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c42: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c52: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c62: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c72: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c82: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ca2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cbd: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < TextureAssets.Item.Length; i++)
			{
				int num = ItemID.Sets.TextureCopyLoad[i];
				if (num != -1)
				{
					TextureAssets.Item[i] = TextureAssets.Item[num];
				}
				else
				{
					TextureAssets.Item[i] = LoadAsset<Texture2D>("Images/Item_" + i, (AssetRequestMode)0);
				}
			}
			for (int j = 0; j < TextureAssets.Npc.Length; j++)
			{
				TextureAssets.Npc[j] = LoadAsset<Texture2D>("Images/NPC_" + j, (AssetRequestMode)0);
			}
			for (int k = 0; k < TextureAssets.Projectile.Length; k++)
			{
				TextureAssets.Projectile[k] = LoadAsset<Texture2D>("Images/Projectile_" + k, (AssetRequestMode)0);
			}
			for (int l = 0; l < TextureAssets.Gore.Length; l++)
			{
				TextureAssets.Gore[l] = LoadAsset<Texture2D>("Images/Gore_" + l, (AssetRequestMode)0);
			}
			for (int m = 0; m < TextureAssets.Wall.Length; m++)
			{
				TextureAssets.Wall[m] = LoadAsset<Texture2D>("Images/Wall_" + m, (AssetRequestMode)0);
			}
			for (int n = 0; n < TextureAssets.Tile.Length; n++)
			{
				TextureAssets.Tile[n] = LoadAsset<Texture2D>("Images/Tiles_" + n, (AssetRequestMode)0);
			}
			for (int num2 = 0; num2 < TextureAssets.ItemFlame.Length; num2++)
			{
				TextureAssets.ItemFlame[num2] = LoadAsset<Texture2D>("Images/ItemFlame_" + num2, (AssetRequestMode)0);
			}
			for (int num3 = 0; num3 < TextureAssets.Wings.Length; num3++)
			{
				TextureAssets.Wings[num3] = LoadAsset<Texture2D>("Images/Wings_" + num3, (AssetRequestMode)0);
			}
			for (int num4 = 0; num4 < TextureAssets.PlayerHair.Length; num4++)
			{
				TextureAssets.PlayerHair[num4] = LoadAsset<Texture2D>("Images/Player_Hair_" + (num4 + 1), (AssetRequestMode)0);
			}
			for (int num5 = 0; num5 < TextureAssets.PlayerHairAlt.Length; num5++)
			{
				TextureAssets.PlayerHairAlt[num5] = LoadAsset<Texture2D>("Images/Player_HairAlt_" + (num5 + 1), (AssetRequestMode)0);
			}
			for (int num6 = 0; num6 < TextureAssets.ArmorHead.Length; num6++)
			{
				TextureAssets.ArmorHead[num6] = LoadAsset<Texture2D>("Images/Armor_Head_" + num6, (AssetRequestMode)0);
			}
			for (int num7 = 0; num7 < TextureAssets.FemaleBody.Length; num7++)
			{
				TextureAssets.FemaleBody[num7] = LoadAsset<Texture2D>("Images/Female_Body_" + num7, (AssetRequestMode)0);
			}
			for (int num8 = 0; num8 < TextureAssets.ArmorBody.Length; num8++)
			{
				TextureAssets.ArmorBody[num8] = LoadAsset<Texture2D>("Images/Armor_Body_" + num8, (AssetRequestMode)0);
			}
			for (int num9 = 0; num9 < TextureAssets.ArmorBodyComposite.Length; num9++)
			{
				TextureAssets.ArmorBodyComposite[num9] = LoadAsset<Texture2D>("Images/Armor/Armor_" + num9, (AssetRequestMode)0);
			}
			for (int num10 = 0; num10 < TextureAssets.ArmorArm.Length; num10++)
			{
				TextureAssets.ArmorArm[num10] = LoadAsset<Texture2D>("Images/Armor_Arm_" + num10, (AssetRequestMode)0);
			}
			for (int num11 = 0; num11 < TextureAssets.ArmorLeg.Length; num11++)
			{
				TextureAssets.ArmorLeg[num11] = LoadAsset<Texture2D>("Images/Armor_Legs_" + num11, (AssetRequestMode)0);
			}
			for (int num12 = 0; num12 < TextureAssets.AccHandsOn.Length; num12++)
			{
				TextureAssets.AccHandsOn[num12] = LoadAsset<Texture2D>("Images/Acc_HandsOn_" + num12, (AssetRequestMode)0);
			}
			for (int num13 = 0; num13 < TextureAssets.AccHandsOff.Length; num13++)
			{
				TextureAssets.AccHandsOff[num13] = LoadAsset<Texture2D>("Images/Acc_HandsOff_" + num13, (AssetRequestMode)0);
			}
			for (int num14 = 0; num14 < TextureAssets.AccHandsOnComposite.Length; num14++)
			{
				TextureAssets.AccHandsOnComposite[num14] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOn_" + num14, (AssetRequestMode)0);
			}
			for (int num15 = 0; num15 < TextureAssets.AccHandsOffComposite.Length; num15++)
			{
				TextureAssets.AccHandsOffComposite[num15] = LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOff_" + num15, (AssetRequestMode)0);
			}
			for (int num16 = 0; num16 < TextureAssets.AccBack.Length; num16++)
			{
				TextureAssets.AccBack[num16] = LoadAsset<Texture2D>("Images/Acc_Back_" + num16, (AssetRequestMode)0);
			}
			for (int num17 = 0; num17 < TextureAssets.AccFront.Length; num17++)
			{
				TextureAssets.AccFront[num17] = LoadAsset<Texture2D>("Images/Acc_Front_" + num17, (AssetRequestMode)0);
			}
			for (int num18 = 0; num18 < TextureAssets.AccShoes.Length; num18++)
			{
				TextureAssets.AccShoes[num18] = LoadAsset<Texture2D>("Images/Acc_Shoes_" + num18, (AssetRequestMode)0);
			}
			for (int num19 = 0; num19 < TextureAssets.AccWaist.Length; num19++)
			{
				TextureAssets.AccWaist[num19] = LoadAsset<Texture2D>("Images/Acc_Waist_" + num19, (AssetRequestMode)0);
			}
			for (int num20 = 0; num20 < TextureAssets.AccShield.Length; num20++)
			{
				TextureAssets.AccShield[num20] = LoadAsset<Texture2D>("Images/Acc_Shield_" + num20, (AssetRequestMode)0);
			}
			for (int num21 = 0; num21 < TextureAssets.AccNeck.Length; num21++)
			{
				TextureAssets.AccNeck[num21] = LoadAsset<Texture2D>("Images/Acc_Neck_" + num21, (AssetRequestMode)0);
			}
			for (int num22 = 0; num22 < TextureAssets.AccFace.Length; num22++)
			{
				TextureAssets.AccFace[num22] = LoadAsset<Texture2D>("Images/Acc_Face_" + num22, (AssetRequestMode)0);
			}
			for (int num23 = 0; num23 < TextureAssets.AccBalloon.Length; num23++)
			{
				TextureAssets.AccBalloon[num23] = LoadAsset<Texture2D>("Images/Acc_Balloon_" + num23, (AssetRequestMode)0);
			}
			for (int num24 = 0; num24 < TextureAssets.AccBeard.Length; num24++)
			{
				TextureAssets.AccBeard[num24] = LoadAsset<Texture2D>("Images/Acc_Beard_" + num24, (AssetRequestMode)0);
			}
			for (int num25 = 0; num25 < TextureAssets.Background.Length; num25++)
			{
				TextureAssets.Background[num25] = LoadAsset<Texture2D>("Images/Background_" + num25, (AssetRequestMode)0);
			}
			TextureAssets.FlameRing = LoadAsset<Texture2D>("Images/FlameRing", (AssetRequestMode)0);
			TextureAssets.TileCrack = LoadAsset<Texture2D>("Images\\TileCracks", mode);
			TextureAssets.ChestStack[0] = LoadAsset<Texture2D>("Images\\ChestStack_0", mode);
			TextureAssets.ChestStack[1] = LoadAsset<Texture2D>("Images\\ChestStack_1", mode);
			TextureAssets.SmartDig = LoadAsset<Texture2D>("Images\\SmartDig", mode);
			TextureAssets.IceBarrier = LoadAsset<Texture2D>("Images\\IceBarrier", mode);
			TextureAssets.Frozen = LoadAsset<Texture2D>("Images\\Frozen", mode);
			for (int num26 = 0; num26 < TextureAssets.Pvp.Length; num26++)
			{
				TextureAssets.Pvp[num26] = LoadAsset<Texture2D>("Images\\UI\\PVP_" + num26, mode);
			}
			for (int num27 = 0; num27 < TextureAssets.EquipPage.Length; num27++)
			{
				TextureAssets.EquipPage[num27] = LoadAsset<Texture2D>("Images\\UI\\DisplaySlots_" + num27, mode);
			}
			TextureAssets.HouseBanner = LoadAsset<Texture2D>("Images\\UI\\House_Banner", mode);
			for (int num28 = 0; num28 < TextureAssets.CraftToggle.Length; num28++)
			{
				TextureAssets.CraftToggle[num28] = LoadAsset<Texture2D>("Images\\UI\\Craft_Toggle_" + num28, mode);
			}
			for (int num29 = 0; num29 < TextureAssets.InventorySort.Length; num29++)
			{
				TextureAssets.InventorySort[num29] = LoadAsset<Texture2D>("Images\\UI\\Sort_" + num29, mode);
			}
			for (int num30 = 0; num30 < TextureAssets.TextGlyph.Length; num30++)
			{
				TextureAssets.TextGlyph[num30] = LoadAsset<Texture2D>("Images\\UI\\Glyphs_" + num30, mode);
			}
			for (int num31 = 0; num31 < TextureAssets.HotbarRadial.Length; num31++)
			{
				TextureAssets.HotbarRadial[num31] = LoadAsset<Texture2D>("Images\\UI\\HotbarRadial_" + num31, mode);
			}
			for (int num32 = 0; num32 < TextureAssets.InfoIcon.Length; num32++)
			{
				TextureAssets.InfoIcon[num32] = LoadAsset<Texture2D>("Images\\UI\\InfoIcon_" + num32, mode);
			}
			for (int num33 = 0; num33 < TextureAssets.Reforge.Length; num33++)
			{
				TextureAssets.Reforge[num33] = LoadAsset<Texture2D>("Images\\UI\\Reforge_" + num33, mode);
			}
			for (int num34 = 0; num34 < TextureAssets.Camera.Length; num34++)
			{
				TextureAssets.Camera[num34] = LoadAsset<Texture2D>("Images\\UI\\Camera_" + num34, mode);
			}
			for (int num35 = 0; num35 < TextureAssets.WireUi.Length; num35++)
			{
				TextureAssets.WireUi[num35] = LoadAsset<Texture2D>("Images\\UI\\Wires_" + num35, mode);
			}
			TextureAssets.BuilderAcc = LoadAsset<Texture2D>("Images\\UI\\BuilderIcons", mode);
			TextureAssets.QuicksIcon = LoadAsset<Texture2D>("Images\\UI\\UI_quickicon1", mode);
			TextureAssets.CraftUpButton = LoadAsset<Texture2D>("Images\\RecUp", mode);
			TextureAssets.CraftDownButton = LoadAsset<Texture2D>("Images\\RecDown", mode);
			TextureAssets.ScrollLeftButton = LoadAsset<Texture2D>("Images\\RecLeft", mode);
			TextureAssets.ScrollRightButton = LoadAsset<Texture2D>("Images\\RecRight", mode);
			TextureAssets.OneDropLogo = LoadAsset<Texture2D>("Images\\OneDropLogo", mode);
			TextureAssets.Pulley = LoadAsset<Texture2D>("Images\\PlayerPulley", mode);
			TextureAssets.Timer = LoadAsset<Texture2D>("Images\\Timer", mode);
			TextureAssets.EmoteMenuButton = LoadAsset<Texture2D>("Images\\UI\\Emotes", mode);
			TextureAssets.BestiaryMenuButton = LoadAsset<Texture2D>("Images\\UI\\Bestiary", mode);
			TextureAssets.Wof = LoadAsset<Texture2D>("Images\\WallOfFlesh", mode);
			TextureAssets.WallOutline = LoadAsset<Texture2D>("Images\\Wall_Outline", mode);
			TextureAssets.Fade = LoadAsset<Texture2D>("Images\\fade-out", mode);
			TextureAssets.Ghost = LoadAsset<Texture2D>("Images\\Ghost", mode);
			TextureAssets.EvilCactus = LoadAsset<Texture2D>("Images\\Evil_Cactus", mode);
			TextureAssets.GoodCactus = LoadAsset<Texture2D>("Images\\Good_Cactus", mode);
			TextureAssets.CrimsonCactus = LoadAsset<Texture2D>("Images\\Crimson_Cactus", mode);
			TextureAssets.WraithEye = LoadAsset<Texture2D>("Images\\Wraith_Eyes", mode);
			TextureAssets.Firefly = LoadAsset<Texture2D>("Images\\Firefly", mode);
			TextureAssets.FireflyJar = LoadAsset<Texture2D>("Images\\FireflyJar", mode);
			TextureAssets.Lightningbug = LoadAsset<Texture2D>("Images\\LightningBug", mode);
			TextureAssets.LightningbugJar = LoadAsset<Texture2D>("Images\\LightningBugJar", mode);
			for (int num36 = 1; num36 <= 3; num36++)
			{
				TextureAssets.JellyfishBowl[num36 - 1] = LoadAsset<Texture2D>("Images\\jellyfishBowl" + num36, mode);
			}
			TextureAssets.GlowSnail = LoadAsset<Texture2D>("Images\\GlowSnail", mode);
			TextureAssets.IceQueen = LoadAsset<Texture2D>("Images\\IceQueen", mode);
			TextureAssets.SantaTank = LoadAsset<Texture2D>("Images\\SantaTank", mode);
			TextureAssets.JackHat = LoadAsset<Texture2D>("Images\\JackHat", mode);
			TextureAssets.TreeFace = LoadAsset<Texture2D>("Images\\TreeFace", mode);
			TextureAssets.PumpkingFace = LoadAsset<Texture2D>("Images\\PumpkingFace", mode);
			TextureAssets.ReaperEye = LoadAsset<Texture2D>("Images\\Reaper_Eyes", mode);
			TextureAssets.MapDeath = LoadAsset<Texture2D>("Images\\MapDeath", mode);
			TextureAssets.DukeFishron = LoadAsset<Texture2D>("Images\\DukeFishron", mode);
			TextureAssets.MiniMinotaur = LoadAsset<Texture2D>("Images\\MiniMinotaur", mode);
			TextureAssets.Map = LoadAsset<Texture2D>("Images\\Map", mode);
			for (int num37 = 0; num37 < TextureAssets.MapBGs.Length; num37++)
			{
				TextureAssets.MapBGs[num37] = LoadAsset<Texture2D>("Images\\MapBG" + (num37 + 1), mode);
			}
			TextureAssets.Hue = LoadAsset<Texture2D>("Images\\Hue", mode);
			TextureAssets.ColorSlider = LoadAsset<Texture2D>("Images\\ColorSlider", mode);
			TextureAssets.ColorBar = LoadAsset<Texture2D>("Images\\ColorBar", mode);
			TextureAssets.ColorBlip = LoadAsset<Texture2D>("Images\\ColorBlip", mode);
			TextureAssets.ColorHighlight = LoadAsset<Texture2D>("Images\\UI\\Slider_Highlight", mode);
			TextureAssets.LockOnCursor = LoadAsset<Texture2D>("Images\\UI\\LockOn_Cursor", mode);
			TextureAssets.Rain = LoadAsset<Texture2D>("Images\\Rain", mode);
			for (int num38 = 0; num38 < 353; num38++)
			{
				TextureAssets.GlowMask[num38] = LoadAsset<Texture2D>("Images\\Glow_" + num38, mode);
			}
			for (int num39 = 0; num39 < TextureAssets.HighlightMask.Length; num39++)
			{
				if (TileID.Sets.HasOutlines[num39])
				{
					TextureAssets.HighlightMask[num39] = LoadAsset<Texture2D>("Images\\Misc\\TileOutlines\\Tiles_" + num39, mode);
				}
			}
			for (int num40 = 0; num40 < 264; num40++)
			{
				TextureAssets.Extra[num40] = LoadAsset<Texture2D>("Images\\Extra_" + num40, mode);
			}
			for (int num41 = 0; num41 < 4; num41++)
			{
				TextureAssets.Coin[num41] = LoadAsset<Texture2D>("Images\\Coin_" + num41, mode);
			}
			TextureAssets.MagicPixel = LoadAsset<Texture2D>("Images\\MagicPixel", mode);
			TextureAssets.SettingsPanel = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel", mode);
			TextureAssets.SettingsPanel2 = LoadAsset<Texture2D>("Images\\UI\\Settings_Panel_2", mode);
			for (int num42 = 0; num42 < TextureAssets.XmasTree.Length; num42++)
			{
				TextureAssets.XmasTree[num42] = LoadAsset<Texture2D>("Images\\Xmas_" + num42, mode);
			}
			for (int num43 = 0; num43 < 6; num43++)
			{
				TextureAssets.Clothes[num43] = LoadAsset<Texture2D>("Images\\Clothes_" + num43, mode);
			}
			for (int num44 = 0; num44 < TextureAssets.Flames.Length; num44++)
			{
				TextureAssets.Flames[num44] = LoadAsset<Texture2D>("Images\\Flame_" + num44, mode);
			}
			for (int num45 = 0; num45 < 8; num45++)
			{
				TextureAssets.MapIcon[num45] = LoadAsset<Texture2D>("Images\\Map_" + num45, mode);
			}
			for (int num46 = 0; num46 < TextureAssets.Underworld.Length; num46++)
			{
				TextureAssets.Underworld[num46] = LoadAsset<Texture2D>("Images/Backgrounds/Underworld " + num46, (AssetRequestMode)0);
			}
			TextureAssets.Dest[0] = LoadAsset<Texture2D>("Images\\Dest1", mode);
			TextureAssets.Dest[1] = LoadAsset<Texture2D>("Images\\Dest2", mode);
			TextureAssets.Dest[2] = LoadAsset<Texture2D>("Images\\Dest3", mode);
			TextureAssets.Actuator = LoadAsset<Texture2D>("Images\\Actuator", mode);
			TextureAssets.Wire = LoadAsset<Texture2D>("Images\\Wires", mode);
			TextureAssets.Wire2 = LoadAsset<Texture2D>("Images\\Wires2", mode);
			TextureAssets.Wire3 = LoadAsset<Texture2D>("Images\\Wires3", mode);
			TextureAssets.Wire4 = LoadAsset<Texture2D>("Images\\Wires4", mode);
			TextureAssets.WireNew = LoadAsset<Texture2D>("Images\\WiresNew", mode);
			TextureAssets.FlyingCarpet = LoadAsset<Texture2D>("Images\\FlyingCarpet", mode);
			TextureAssets.Hb1 = LoadAsset<Texture2D>("Images\\HealthBar1", mode);
			TextureAssets.Hb2 = LoadAsset<Texture2D>("Images\\HealthBar2", mode);
			for (int num47 = 0; num47 < TextureAssets.NpcHead.Length; num47++)
			{
				TextureAssets.NpcHead[num47] = LoadAsset<Texture2D>("Images\\NPC_Head_" + num47, mode);
			}
			for (int num48 = 0; num48 < TextureAssets.NpcHeadBoss.Length; num48++)
			{
				TextureAssets.NpcHeadBoss[num48] = LoadAsset<Texture2D>("Images\\NPC_Head_Boss_" + num48, mode);
			}
			for (int num49 = 1; num49 < TextureAssets.BackPack.Length; num49++)
			{
				TextureAssets.BackPack[num49] = LoadAsset<Texture2D>("Images\\BackPack_" + num49, mode);
			}
			for (int num50 = 1; num50 < 355; num50++)
			{
				TextureAssets.Buff[num50] = LoadAsset<Texture2D>("Images\\Buff_" + num50, mode);
			}
			Main.instance.LoadBackground(0);
			Main.instance.LoadBackground(49);
			TextureAssets.MinecartMount = LoadAsset<Texture2D>("Images\\Mount_Minecart", mode);
			for (int num51 = 0; num51 < TextureAssets.RudolphMount.Length; num51++)
			{
				TextureAssets.RudolphMount[num51] = LoadAsset<Texture2D>("Images\\Rudolph_" + num51, mode);
			}
			TextureAssets.BunnyMount = LoadAsset<Texture2D>("Images\\Mount_Bunny", mode);
			TextureAssets.PigronMount = LoadAsset<Texture2D>("Images\\Mount_Pigron", mode);
			TextureAssets.SlimeMount = LoadAsset<Texture2D>("Images\\Mount_Slime", mode);
			TextureAssets.TurtleMount = LoadAsset<Texture2D>("Images\\Mount_Turtle", mode);
			TextureAssets.UnicornMount = LoadAsset<Texture2D>("Images\\Mount_Unicorn", mode);
			TextureAssets.BasiliskMount = LoadAsset<Texture2D>("Images\\Mount_Basilisk", mode);
			TextureAssets.MinecartMechMount[0] = LoadAsset<Texture2D>("Images\\Mount_MinecartMech", mode);
			TextureAssets.MinecartMechMount[1] = LoadAsset<Texture2D>("Images\\Mount_MinecartMechGlow", mode);
			TextureAssets.CuteFishronMount[0] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron1", mode);
			TextureAssets.CuteFishronMount[1] = LoadAsset<Texture2D>("Images\\Mount_CuteFishron2", mode);
			TextureAssets.MinecartWoodMount = LoadAsset<Texture2D>("Images\\Mount_MinecartWood", mode);
			TextureAssets.DesertMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartDesert", mode);
			TextureAssets.FishMinecartMount = LoadAsset<Texture2D>("Images\\Mount_MinecartMineCarp", mode);
			TextureAssets.BeeMount[0] = LoadAsset<Texture2D>("Images\\Mount_Bee", mode);
			TextureAssets.BeeMount[1] = LoadAsset<Texture2D>("Images\\Mount_BeeWings", mode);
			TextureAssets.UfoMount[0] = LoadAsset<Texture2D>("Images\\Mount_UFO", mode);
			TextureAssets.UfoMount[1] = LoadAsset<Texture2D>("Images\\Mount_UFOGlow", mode);
			TextureAssets.DrillMount[0] = LoadAsset<Texture2D>("Images\\Mount_DrillRing", mode);
			TextureAssets.DrillMount[1] = LoadAsset<Texture2D>("Images\\Mount_DrillSeat", mode);
			TextureAssets.DrillMount[2] = LoadAsset<Texture2D>("Images\\Mount_DrillDiode", mode);
			TextureAssets.DrillMount[3] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillRing", mode);
			TextureAssets.DrillMount[4] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillSeat", mode);
			TextureAssets.DrillMount[5] = LoadAsset<Texture2D>("Images\\Mount_Glow_DrillDiode", mode);
			TextureAssets.ScutlixMount[0] = LoadAsset<Texture2D>("Images\\Mount_Scutlix", mode);
			TextureAssets.ScutlixMount[1] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyes", mode);
			TextureAssets.ScutlixMount[2] = LoadAsset<Texture2D>("Images\\Mount_ScutlixEyeGlow", mode);
			for (int num52 = 0; num52 < TextureAssets.Gem.Length; num52++)
			{
				TextureAssets.Gem[num52] = LoadAsset<Texture2D>("Images\\Gem_" + num52, mode);
			}
			for (int num53 = 0; num53 < 41; num53++)
			{
				TextureAssets.Cloud[num53] = LoadAsset<Texture2D>("Images\\Cloud_" + num53, mode);
			}
			for (int num54 = 0; num54 < 4; num54++)
			{
				TextureAssets.Star[num54] = LoadAsset<Texture2D>("Images\\Star_" + num54, mode);
			}
			for (int num55 = 0; num55 < 15; num55++)
			{
				TextureAssets.Liquid[num55] = LoadAsset<Texture2D>("Images\\Liquid_" + num55, mode);
				TextureAssets.LiquidSlope[num55] = LoadAsset<Texture2D>("Images\\LiquidSlope_" + num55, mode);
			}
			Main.instance.waterfallManager.LoadContent();
			TextureAssets.NpcToggle[0] = LoadAsset<Texture2D>("Images\\House_1", mode);
			TextureAssets.NpcToggle[1] = LoadAsset<Texture2D>("Images\\House_2", mode);
			TextureAssets.HbLock[0] = LoadAsset<Texture2D>("Images\\Lock_0", mode);
			TextureAssets.HbLock[1] = LoadAsset<Texture2D>("Images\\Lock_1", mode);
			TextureAssets.blockReplaceIcon[0] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_0", mode);
			TextureAssets.blockReplaceIcon[1] = LoadAsset<Texture2D>("Images\\UI\\BlockReplace_1", mode);
			TextureAssets.Grid = LoadAsset<Texture2D>("Images\\Grid", mode);
			TextureAssets.Trash = LoadAsset<Texture2D>("Images\\Trash", mode);
			TextureAssets.Cd = LoadAsset<Texture2D>("Images\\CoolDown", mode);
			TextureAssets.Logo = LoadAsset<Texture2D>("Images\\Logo", mode);
			TextureAssets.Logo2 = LoadAsset<Texture2D>("Images\\Logo2", mode);
			TextureAssets.Logo3 = LoadAsset<Texture2D>("Images\\Logo3", mode);
			TextureAssets.Logo4 = LoadAsset<Texture2D>("Images\\Logo4", mode);
			TextureAssets.Dust = LoadAsset<Texture2D>("Images\\Dust", mode);
			TextureAssets.Sun = LoadAsset<Texture2D>("Images\\Sun", mode);
			TextureAssets.Sun2 = LoadAsset<Texture2D>("Images\\Sun2", mode);
			TextureAssets.Sun3 = LoadAsset<Texture2D>("Images\\Sun3", mode);
			TextureAssets.BlackTile = LoadAsset<Texture2D>("Images\\Black_Tile", mode);
			TextureAssets.Heart = LoadAsset<Texture2D>("Images\\Heart", mode);
			TextureAssets.Heart2 = LoadAsset<Texture2D>("Images\\Heart2", mode);
			TextureAssets.Bubble = LoadAsset<Texture2D>("Images\\Bubble", mode);
			TextureAssets.Flame = LoadAsset<Texture2D>("Images\\Flame", mode);
			TextureAssets.Mana = LoadAsset<Texture2D>("Images\\Mana", mode);
			for (int num56 = 0; num56 < TextureAssets.Cursors.Length; num56++)
			{
				TextureAssets.Cursors[num56] = LoadAsset<Texture2D>("Images\\UI\\Cursor_" + num56, mode);
			}
			TextureAssets.CursorRadial = LoadAsset<Texture2D>("Images\\UI\\Radial", mode);
			TextureAssets.Ninja = LoadAsset<Texture2D>("Images\\Ninja", mode);
			TextureAssets.AntLion = LoadAsset<Texture2D>("Images\\AntlionBody", mode);
			TextureAssets.SpikeBase = LoadAsset<Texture2D>("Images\\Spike_Base", mode);
			TextureAssets.Wood[0] = LoadAsset<Texture2D>("Images\\Tiles_5_0", mode);
			TextureAssets.Wood[1] = LoadAsset<Texture2D>("Images\\Tiles_5_1", mode);
			TextureAssets.Wood[2] = LoadAsset<Texture2D>("Images\\Tiles_5_2", mode);
			TextureAssets.Wood[3] = LoadAsset<Texture2D>("Images\\Tiles_5_3", mode);
			TextureAssets.Wood[4] = LoadAsset<Texture2D>("Images\\Tiles_5_4", mode);
			TextureAssets.Wood[5] = LoadAsset<Texture2D>("Images\\Tiles_5_5", mode);
			TextureAssets.Wood[6] = LoadAsset<Texture2D>("Images\\Tiles_5_6", mode);
			TextureAssets.SmileyMoon = LoadAsset<Texture2D>("Images\\Moon_Smiley", mode);
			TextureAssets.PumpkinMoon = LoadAsset<Texture2D>("Images\\Moon_Pumpkin", mode);
			TextureAssets.SnowMoon = LoadAsset<Texture2D>("Images\\Moon_Snow", mode);
			for (int num57 = 0; num57 < TextureAssets.CageTop.Length; num57++)
			{
				TextureAssets.CageTop[num57] = LoadAsset<Texture2D>("Images\\CageTop_" + num57, mode);
			}
			for (int num58 = 0; num58 < TextureAssets.Moon.Length; num58++)
			{
				TextureAssets.Moon[num58] = LoadAsset<Texture2D>("Images\\Moon_" + num58, mode);
			}
			for (int num59 = 0; num59 < TextureAssets.TreeTop.Length; num59++)
			{
				TextureAssets.TreeTop[num59] = LoadAsset<Texture2D>("Images\\Tree_Tops_" + num59, mode);
			}
			for (int num60 = 0; num60 < TextureAssets.TreeBranch.Length; num60++)
			{
				TextureAssets.TreeBranch[num60] = LoadAsset<Texture2D>("Images\\Tree_Branches_" + num60, mode);
			}
			TextureAssets.ShroomCap = LoadAsset<Texture2D>("Images\\Shroom_Tops", mode);
			TextureAssets.InventoryBack = LoadAsset<Texture2D>("Images\\Inventory_Back", mode);
			TextureAssets.InventoryBack2 = LoadAsset<Texture2D>("Images\\Inventory_Back2", mode);
			TextureAssets.InventoryBack3 = LoadAsset<Texture2D>("Images\\Inventory_Back3", mode);
			TextureAssets.InventoryBack4 = LoadAsset<Texture2D>("Images\\Inventory_Back4", mode);
			TextureAssets.InventoryBack5 = LoadAsset<Texture2D>("Images\\Inventory_Back5", mode);
			TextureAssets.InventoryBack6 = LoadAsset<Texture2D>("Images\\Inventory_Back6", mode);
			TextureAssets.InventoryBack7 = LoadAsset<Texture2D>("Images\\Inventory_Back7", mode);
			TextureAssets.InventoryBack8 = LoadAsset<Texture2D>("Images\\Inventory_Back8", mode);
			TextureAssets.InventoryBack9 = LoadAsset<Texture2D>("Images\\Inventory_Back9", mode);
			TextureAssets.InventoryBack10 = LoadAsset<Texture2D>("Images\\Inventory_Back10", mode);
			TextureAssets.InventoryBack11 = LoadAsset<Texture2D>("Images\\Inventory_Back11", mode);
			TextureAssets.InventoryBack12 = LoadAsset<Texture2D>("Images\\Inventory_Back12", mode);
			TextureAssets.InventoryBack13 = LoadAsset<Texture2D>("Images\\Inventory_Back13", mode);
			TextureAssets.InventoryBack14 = LoadAsset<Texture2D>("Images\\Inventory_Back14", mode);
			TextureAssets.InventoryBack15 = LoadAsset<Texture2D>("Images\\Inventory_Back15", mode);
			TextureAssets.InventoryBack16 = LoadAsset<Texture2D>("Images\\Inventory_Back16", mode);
			TextureAssets.InventoryBack17 = LoadAsset<Texture2D>("Images\\Inventory_Back17", mode);
			TextureAssets.InventoryBack18 = LoadAsset<Texture2D>("Images\\Inventory_Back18", mode);
			TextureAssets.InventoryBack19 = LoadAsset<Texture2D>("Images\\Inventory_Back19", mode);
			TextureAssets.HairStyleBack = LoadAsset<Texture2D>("Images\\HairStyleBack", mode);
			TextureAssets.ClothesStyleBack = LoadAsset<Texture2D>("Images\\ClothesStyleBack", mode);
			TextureAssets.InventoryTickOff = LoadAsset<Texture2D>("Images\\Inventory_Tick_Off", mode);
			TextureAssets.InventoryTickOn = LoadAsset<Texture2D>("Images\\Inventory_Tick_On", mode);
			TextureAssets.TextBack = LoadAsset<Texture2D>("Images\\Text_Back", mode);
			TextureAssets.Chat = LoadAsset<Texture2D>("Images\\Chat", mode);
			TextureAssets.Chat2 = LoadAsset<Texture2D>("Images\\Chat2", mode);
			TextureAssets.ChatBack = LoadAsset<Texture2D>("Images\\Chat_Back", mode);
			TextureAssets.Team = LoadAsset<Texture2D>("Images\\Team", mode);
			PlayerDataInitializer.Load();
			TextureAssets.Chaos = LoadAsset<Texture2D>("Images\\Chaos", mode);
			TextureAssets.EyeLaser = LoadAsset<Texture2D>("Images\\Eye_Laser", mode);
			TextureAssets.BoneEyes = LoadAsset<Texture2D>("Images\\Bone_Eyes", mode);
			TextureAssets.BoneLaser = LoadAsset<Texture2D>("Images\\Bone_Laser", mode);
			TextureAssets.LightDisc = LoadAsset<Texture2D>("Images\\Light_Disc", mode);
			TextureAssets.Confuse = LoadAsset<Texture2D>("Images\\Confuse", mode);
			TextureAssets.Probe = LoadAsset<Texture2D>("Images\\Probe", mode);
			TextureAssets.SunOrb = LoadAsset<Texture2D>("Images\\SunOrb", mode);
			TextureAssets.SunAltar = LoadAsset<Texture2D>("Images\\SunAltar", mode);
			TextureAssets.XmasLight = LoadAsset<Texture2D>("Images\\XmasLight", mode);
			TextureAssets.Beetle = LoadAsset<Texture2D>("Images\\BeetleOrb", mode);
			for (int num61 = 0; num61 < 17; num61++)
			{
				TextureAssets.Chains[num61] = LoadAsset<Texture2D>("Images\\Chains_" + num61, mode);
			}
			TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", mode);
			TextureAssets.FishingLine = LoadAsset<Texture2D>("Images\\FishingLine", mode);
			TextureAssets.Chain = LoadAsset<Texture2D>("Images\\Chain", mode);
			TextureAssets.Chain2 = LoadAsset<Texture2D>("Images\\Chain2", mode);
			TextureAssets.Chain3 = LoadAsset<Texture2D>("Images\\Chain3", mode);
			TextureAssets.Chain4 = LoadAsset<Texture2D>("Images\\Chain4", mode);
			TextureAssets.Chain5 = LoadAsset<Texture2D>("Images\\Chain5", mode);
			TextureAssets.Chain6 = LoadAsset<Texture2D>("Images\\Chain6", mode);
			TextureAssets.Chain7 = LoadAsset<Texture2D>("Images\\Chain7", mode);
			TextureAssets.Chain8 = LoadAsset<Texture2D>("Images\\Chain8", mode);
			TextureAssets.Chain9 = LoadAsset<Texture2D>("Images\\Chain9", mode);
			TextureAssets.Chain10 = LoadAsset<Texture2D>("Images\\Chain10", mode);
			TextureAssets.Chain11 = LoadAsset<Texture2D>("Images\\Chain11", mode);
			TextureAssets.Chain12 = LoadAsset<Texture2D>("Images\\Chain12", mode);
			TextureAssets.Chain13 = LoadAsset<Texture2D>("Images\\Chain13", mode);
			TextureAssets.Chain14 = LoadAsset<Texture2D>("Images\\Chain14", mode);
			TextureAssets.Chain15 = LoadAsset<Texture2D>("Images\\Chain15", mode);
			TextureAssets.Chain16 = LoadAsset<Texture2D>("Images\\Chain16", mode);
			TextureAssets.Chain17 = LoadAsset<Texture2D>("Images\\Chain17", mode);
			TextureAssets.Chain18 = LoadAsset<Texture2D>("Images\\Chain18", mode);
			TextureAssets.Chain19 = LoadAsset<Texture2D>("Images\\Chain19", mode);
			TextureAssets.Chain20 = LoadAsset<Texture2D>("Images\\Chain20", mode);
			TextureAssets.Chain21 = LoadAsset<Texture2D>("Images\\Chain21", mode);
			TextureAssets.Chain22 = LoadAsset<Texture2D>("Images\\Chain22", mode);
			TextureAssets.Chain23 = LoadAsset<Texture2D>("Images\\Chain23", mode);
			TextureAssets.Chain24 = LoadAsset<Texture2D>("Images\\Chain24", mode);
			TextureAssets.Chain25 = LoadAsset<Texture2D>("Images\\Chain25", mode);
			TextureAssets.Chain26 = LoadAsset<Texture2D>("Images\\Chain26", mode);
			TextureAssets.Chain27 = LoadAsset<Texture2D>("Images\\Chain27", mode);
			TextureAssets.Chain28 = LoadAsset<Texture2D>("Images\\Chain28", mode);
			TextureAssets.Chain29 = LoadAsset<Texture2D>("Images\\Chain29", mode);
			TextureAssets.Chain30 = LoadAsset<Texture2D>("Images\\Chain30", mode);
			TextureAssets.Chain31 = LoadAsset<Texture2D>("Images\\Chain31", mode);
			TextureAssets.Chain32 = LoadAsset<Texture2D>("Images\\Chain32", mode);
			TextureAssets.Chain33 = LoadAsset<Texture2D>("Images\\Chain33", mode);
			TextureAssets.Chain34 = LoadAsset<Texture2D>("Images\\Chain34", mode);
			TextureAssets.Chain35 = LoadAsset<Texture2D>("Images\\Chain35", mode);
			TextureAssets.Chain36 = LoadAsset<Texture2D>("Images\\Chain36", mode);
			TextureAssets.Chain37 = LoadAsset<Texture2D>("Images\\Chain37", mode);
			TextureAssets.Chain38 = LoadAsset<Texture2D>("Images\\Chain38", mode);
			TextureAssets.Chain39 = LoadAsset<Texture2D>("Images\\Chain39", mode);
			TextureAssets.Chain40 = LoadAsset<Texture2D>("Images\\Chain40", mode);
			TextureAssets.Chain41 = LoadAsset<Texture2D>("Images\\Chain41", mode);
			TextureAssets.Chain42 = LoadAsset<Texture2D>("Images\\Chain42", mode);
			TextureAssets.Chain43 = LoadAsset<Texture2D>("Images\\Chain43", mode);
			TextureAssets.EyeLaserSmall = LoadAsset<Texture2D>("Images\\Eye_Laser_Small", mode);
			TextureAssets.BoneArm = LoadAsset<Texture2D>("Images\\Arm_Bone", mode);
			TextureAssets.PumpkingArm = LoadAsset<Texture2D>("Images\\PumpkingArm", mode);
			TextureAssets.PumpkingCloak = LoadAsset<Texture2D>("Images\\PumpkingCloak", mode);
			TextureAssets.BoneArm2 = LoadAsset<Texture2D>("Images\\Arm_Bone_2", mode);
			for (int num62 = 1; num62 < TextureAssets.GemChain.Length; num62++)
			{
				TextureAssets.GemChain[num62] = LoadAsset<Texture2D>("Images\\GemChain_" + num62, mode);
			}
			for (int num63 = 1; num63 < TextureAssets.Golem.Length; num63++)
			{
				TextureAssets.Golem[num63] = LoadAsset<Texture2D>("Images\\GolemLights" + num63, mode);
			}
			TextureAssets.GolfSwingBarFill = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarFill", mode);
			TextureAssets.GolfSwingBarPanel = LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarPanel", mode);
			TextureAssets.SpawnPoint = LoadAsset<Texture2D>("Images\\UI\\SpawnPoint", mode);
			TextureAssets.SpawnBed = LoadAsset<Texture2D>("Images\\UI\\SpawnBed", mode);
			TextureAssets.MapPing = LoadAsset<Texture2D>("Images\\UI\\MapPing", mode);
			TextureAssets.GolfBallArrow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow", mode);
			TextureAssets.GolfBallArrowShadow = LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow_Shadow", mode);
			TextureAssets.GolfBallOutline = LoadAsset<Texture2D>("Images\\Misc\\GolfBallOutline", mode);
			Main.ResourceSetsManager.LoadContent(mode);
			Main.MinimapFrameManagerInstance.LoadContent(mode);
			Main.AchievementAdvisor.LoadContent();
		}

		private static Asset<T> LoadAsset<T>(string assetName, AssetRequestMode mode) where T : class
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Main.Assets.Request<T>(assetName, mode);
		}
	}
}
