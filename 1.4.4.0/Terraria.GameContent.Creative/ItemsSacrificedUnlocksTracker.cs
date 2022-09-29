using System.Collections.Generic;
using System.IO;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	public class ItemsSacrificedUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		public const int POSITIVE_SACRIFICE_COUNT_CAP = 9999;

		private Dictionary<string, int> _sacrificeCountByItemPersistentId;

		private Dictionary<int, int> _sacrificesCountByItemIdCache;

		public int LastEditId { get; private set; }

		public ItemsSacrificedUnlocksTracker()
		{
			_sacrificeCountByItemPersistentId = new Dictionary<string, int>();
			_sacrificesCountByItemIdCache = new Dictionary<int, int>();
			LastEditId = 0;
		}

		public int GetSacrificeCount(int itemId)
		{
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
			{
				itemId = value;
			}
			_sacrificesCountByItemIdCache.TryGetValue(itemId, out var value2);
			return value2;
		}

		public void FillListOfItemsThatCanBeObtainedInfinitely(List<int> toObtainInfinitely)
		{
			foreach (KeyValuePair<int, int> item in _sacrificesCountByItemIdCache)
			{
				if (TryGetSacrificeNumbers(item.Key, out var _, out var amountNeededTotal) && item.Value >= amountNeededTotal)
				{
					toObtainInfinitely.Add(item.Key);
				}
			}
		}

		public bool TryGetSacrificeNumbers(int itemId, out int amountWeHave, out int amountNeededTotal)
		{
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
			{
				itemId = value;
			}
			amountWeHave = (amountNeededTotal = 0);
			if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(itemId, out amountNeededTotal))
			{
				return false;
			}
			_sacrificesCountByItemIdCache.TryGetValue(itemId, out amountWeHave);
			return true;
		}

		public void RegisterItemSacrifice(int itemId, int amount)
		{
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
			{
				itemId = value;
			}
			if (ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(itemId, out var value2))
			{
				_sacrificeCountByItemPersistentId.TryGetValue(value2, out var value3);
				value3 += amount;
				int value4 = Utils.Clamp(value3, 0, 9999);
				_sacrificeCountByItemPersistentId[value2] = value4;
				_sacrificesCountByItemIdCache[itemId] = value4;
				MarkContentsDirty();
			}
		}

		public void SetSacrificeCountDirectly(string persistentId, int sacrificeCount)
		{
			int value = Utils.Clamp(sacrificeCount, 0, 9999);
			_sacrificeCountByItemPersistentId[persistentId] = value;
			if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(persistentId, out var value2))
			{
				_sacrificesCountByItemIdCache[value2] = value;
				MarkContentsDirty();
			}
		}

		public void Save(BinaryWriter writer)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(_sacrificeCountByItemPersistentId);
			writer.Write(dictionary.Count);
			foreach (KeyValuePair<string, int> item in dictionary)
			{
				writer.Write(item.Key);
				writer.Write(item.Value);
			}
		}

		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string key = reader.ReadString();
				int value = reader.ReadInt32();
				if (ContentSamples.ItemNetIdsByPersistentIds.TryGetValue(key, out var value2))
				{
					if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(value2, out var value3))
					{
						value2 = value3;
					}
					_sacrificesCountByItemIdCache[value2] = value;
					if (ContentSamples.ItemPersistentIdsByNetIds.TryGetValue(value2, out var value4))
					{
						key = value4;
					}
				}
				_sacrificeCountByItemPersistentId[key] = value;
			}
		}

		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadString();
				reader.ReadInt32();
			}
		}

		public void Reset()
		{
			_sacrificeCountByItemPersistentId.Clear();
			_sacrificesCountByItemIdCache.Clear();
			MarkContentsDirty();
		}

		public void OnPlayerJoining(int playerIndex)
		{
		}

		public void MarkContentsDirty()
		{
			LastEditId++;
		}
	}
}
