using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	public class PersonalityDatabase
	{
		private Dictionary<int, PersonalityProfile> _personalityProfiles;

		private PersonalityProfile _trashEntry = new PersonalityProfile();

		public PersonalityDatabase()
		{
			_personalityProfiles = new Dictionary<int, PersonalityProfile>();
		}

		public void Register(int npcId, IShopPersonalityTrait trait)
		{
			if (!_personalityProfiles.ContainsKey(npcId))
			{
				_personalityProfiles[npcId] = new PersonalityProfile();
			}
			_personalityProfiles[npcId].ShopModifiers.Add(trait);
		}

		public void Register(IShopPersonalityTrait trait, params int[] npcIds)
		{
			for (int i = 0; i < npcIds.Length; i++)
			{
				Register(trait, npcIds[i]);
			}
		}

		public PersonalityProfile GetByNPCID(int npcId)
		{
			if (_personalityProfiles.TryGetValue(npcId, out var value))
			{
				return value;
			}
			return _trashEntry;
		}
	}
}
