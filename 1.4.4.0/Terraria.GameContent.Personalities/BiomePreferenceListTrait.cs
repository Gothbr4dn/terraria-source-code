using System.Collections;
using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	public class BiomePreferenceListTrait : IShopPersonalityTrait, IEnumerable<BiomePreferenceListTrait.BiomePreference>, IEnumerable
	{
		public class BiomePreference
		{
			public AffectionLevel Affection;

			public AShoppingBiome Biome;

			public BiomePreference(AffectionLevel affection, AShoppingBiome biome)
			{
				Affection = affection;
				Biome = biome;
			}
		}

		private List<BiomePreference> _preferences;

		public BiomePreferenceListTrait()
		{
			_preferences = new List<BiomePreference>();
		}

		public void Add(BiomePreference preference)
		{
			_preferences.Add(preference);
		}

		public void Add(AffectionLevel level, AShoppingBiome biome)
		{
			_preferences.Add(new BiomePreference(level, biome));
		}

		public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
		{
			BiomePreference biomePreference = null;
			for (int i = 0; i < _preferences.Count; i++)
			{
				BiomePreference biomePreference2 = _preferences[i];
				if (biomePreference2.Biome.IsInBiome(info.player) && (biomePreference == null || biomePreference.Affection < biomePreference2.Affection))
				{
					biomePreference = biomePreference2;
				}
			}
			if (biomePreference != null)
			{
				ApplyPreference(biomePreference, info, shopHelperInstance);
			}
		}

		private void ApplyPreference(BiomePreference preference, HelperInfo info, ShopHelper shopHelperInstance)
		{
			string nameKey = preference.Biome.NameKey;
			switch (preference.Affection)
			{
			case AffectionLevel.Love:
				shopHelperInstance.LoveBiome(nameKey);
				break;
			case AffectionLevel.Like:
				shopHelperInstance.LikeBiome(nameKey);
				break;
			case AffectionLevel.Dislike:
				shopHelperInstance.DislikeBiome(nameKey);
				break;
			case AffectionLevel.Hate:
				shopHelperInstance.HateBiome(nameKey);
				break;
			}
		}

		public IEnumerator<BiomePreference> GetEnumerator()
		{
			return _preferences.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _preferences.GetEnumerator();
		}
	}
}
