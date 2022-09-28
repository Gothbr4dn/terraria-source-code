using System.IO;
using Terraria.DataStructures;

namespace Terraria
{
	public class EquipmentLoadout : IFixLoadedData
	{
		public Item[] Armor;

		public Item[] Dye;

		public bool[] Hide;

		public EquipmentLoadout()
		{
			Armor = CreateItemArray(20);
			Dye = CreateItemArray(10);
			Hide = new bool[10];
		}

		private Item[] CreateItemArray(int length)
		{
			Item[] array = new Item[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = new Item();
			}
			return array;
		}

		public void Serialize(BinaryWriter writer)
		{
			ItemSerializationContext context = ItemSerializationContext.SavingAndLoading;
			for (int i = 0; i < Armor.Length; i++)
			{
				Armor[i].Serialize(writer, context);
			}
			for (int j = 0; j < Dye.Length; j++)
			{
				Dye[j].Serialize(writer, context);
			}
			for (int k = 0; k < Hide.Length; k++)
			{
				writer.Write(Hide[k]);
			}
		}

		public void Deserialize(BinaryReader reader, int gameVersion)
		{
			ItemSerializationContext context = ItemSerializationContext.SavingAndLoading;
			for (int i = 0; i < Armor.Length; i++)
			{
				Armor[i].DeserializeFrom(reader, context);
			}
			for (int j = 0; j < Dye.Length; j++)
			{
				Dye[j].DeserializeFrom(reader, context);
			}
			for (int k = 0; k < Hide.Length; k++)
			{
				Hide[k] = reader.ReadBoolean();
			}
		}

		public void Swap(Player player)
		{
			Item[] armor = player.armor;
			for (int i = 0; i < armor.Length; i++)
			{
				Utils.Swap(ref armor[i], ref Armor[i]);
			}
			Item[] dye = player.dye;
			for (int j = 0; j < dye.Length; j++)
			{
				Utils.Swap(ref dye[j], ref Dye[j]);
			}
			bool[] hideVisibleAccessory = player.hideVisibleAccessory;
			for (int k = 0; k < hideVisibleAccessory.Length; k++)
			{
				Utils.Swap(ref hideVisibleAccessory[k], ref Hide[k]);
			}
		}

		public void TryDroppingItems(Player player, IEntitySource source)
		{
			for (int i = 0; i < Armor.Length; i++)
			{
				player.TryDroppingSingleItem(source, Armor[i]);
			}
			for (int j = 0; j < Dye.Length; j++)
			{
				player.TryDroppingSingleItem(source, Dye[j]);
			}
		}

		public void FixLoadedData()
		{
			for (int i = 0; i < Armor.Length; i++)
			{
				Armor[i].FixAgainstExploit();
			}
			for (int j = 0; j < Dye.Length; j++)
			{
				Dye[j].FixAgainstExploit();
			}
			Player.FixLoadedData_EliminiateDuplicateAccessories(Armor);
		}
	}
}
