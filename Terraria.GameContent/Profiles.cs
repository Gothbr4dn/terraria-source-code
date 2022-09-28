using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent
{
	public class Profiles
	{
		public class StackedNPCProfile : ITownNPCProfile
		{
			internal ITownNPCProfile[] _profiles;

			public StackedNPCProfile(params ITownNPCProfile[] profilesInOrderOfVariants)
			{
				_profiles = profilesInOrderOfVariants;
			}

			public int RollVariation()
			{
				return 0;
			}

			public string GetNameForVariant(NPC npc)
			{
				int num = 0;
				if (_profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return _profiles[num].GetNameForVariant(npc);
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				int num = 0;
				if (_profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return _profiles[num].GetTextureNPCShouldUse(npc);
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				int num = 0;
				if (_profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return _profiles[num].GetHeadTextureIndex(npc);
			}
		}

		public class LegacyNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private int _defaultVariationHeadIndex;

			internal Asset<Texture2D> _defaultNoAlt;

			internal Asset<Texture2D> _defaultParty;

			public LegacyNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex, bool includeDefault = true, bool uniquePartyTexture = true)
			{
				_rootFilePath = npcFileTitleFilePath;
				_defaultVariationHeadIndex = defaultHeadIndex;
				_defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + (includeDefault ? "_Default" : ""), (AssetRequestMode)0);
				if (uniquePartyTexture)
				{
					_defaultParty = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + (includeDefault ? "_Default_Party" : "_Party"), (AssetRequestMode)0);
				}
				else
				{
					_defaultParty = _defaultNoAlt;
				}
			}

			public int RollVariation()
			{
				return 0;
			}

			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				{
					return _defaultNoAlt;
				}
				if (npc.altTexture == 1)
				{
					return _defaultParty;
				}
				return _defaultNoAlt;
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _defaultVariationHeadIndex;
			}
		}

		public class TransformableNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private int _defaultVariationHeadIndex;

			internal Asset<Texture2D> _defaultNoAlt;

			internal Asset<Texture2D> _defaultTransformed;

			internal Asset<Texture2D> _defaultCredits;

			public TransformableNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex, bool includeCredits = true)
			{
				_rootFilePath = npcFileTitleFilePath;
				_defaultVariationHeadIndex = defaultHeadIndex;
				_defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", (AssetRequestMode)0);
				_defaultTransformed = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Transformed", (AssetRequestMode)0);
				if (includeCredits)
				{
					_defaultCredits = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Credits", (AssetRequestMode)0);
				}
			}

			public int RollVariation()
			{
				return 0;
			}

			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.altTexture == 3 && _defaultCredits != null)
				{
					return _defaultCredits;
				}
				if (npc.IsABestiaryIconDummy)
				{
					return _defaultNoAlt;
				}
				if (npc.altTexture == 2)
				{
					return _defaultTransformed;
				}
				return _defaultNoAlt;
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _defaultVariationHeadIndex;
			}
		}

		public class VariantNPCProfile : ITownNPCProfile
		{
			private string _rootFilePath;

			private string _npcBaseName;

			private int[] _variantHeadIDs;

			private string[] _variants;

			internal Dictionary<string, Asset<Texture2D>> _variantTextures = new Dictionary<string, Asset<Texture2D>>();

			public VariantNPCProfile(string npcFileTitleFilePath, string npcBaseName, int[] variantHeadIds, params string[] variantTextureNames)
			{
				_rootFilePath = npcFileTitleFilePath;
				_npcBaseName = npcBaseName;
				_variantHeadIDs = variantHeadIds;
				_variants = variantTextureNames;
				string[] variants = _variants;
				foreach (string text in variants)
				{
					string text2 = _rootFilePath + "_" + text;
					_variantTextures[text2] = Main.Assets.Request<Texture2D>(text2, (AssetRequestMode)0);
				}
			}

			public VariantNPCProfile SetPartyTextures(params string[] variantTextureNames)
			{
				foreach (string text in variantTextureNames)
				{
					string text2 = _rootFilePath + "_" + text + "_Party";
					_variantTextures[text2] = Main.Assets.Request<Texture2D>(text2, (AssetRequestMode)0);
				}
				return this;
			}

			public int RollVariation()
			{
				return Main.rand.Next(_variants.Length);
			}

			public string GetNameForVariant(NPC npc)
			{
				return Language.RandomFromCategory(_npcBaseName + "Names_" + _variants[npc.townNpcVariationIndex], WorldGen.genRand).Value;
			}

			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				string text = _rootFilePath + "_" + _variants[npc.townNpcVariationIndex];
				if (npc.IsABestiaryIconDummy)
				{
					return _variantTextures[text];
				}
				if (npc.altTexture == 1 && _variantTextures.ContainsKey(text + "_Party"))
				{
					return _variantTextures[text + "_Party"];
				}
				return _variantTextures[text];
			}

			public int GetHeadTextureIndex(NPC npc)
			{
				return _variantHeadIDs[npc.townNpcVariationIndex];
			}
		}
	}
}
