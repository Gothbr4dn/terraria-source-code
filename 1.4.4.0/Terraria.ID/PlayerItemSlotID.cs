namespace Terraria.ID
{
	public static class PlayerItemSlotID
	{
		public static readonly int Inventory0;

		public static readonly int InventoryMouseItem;

		public static readonly int Armor0;

		public static readonly int Dye0;

		public static readonly int Misc0;

		public static readonly int MiscDye0;

		public static readonly int Bank1_0;

		public static readonly int Bank2_0;

		public static readonly int TrashItem;

		public static readonly int Bank3_0;

		public static readonly int Bank4_0;

		public static readonly int Loadout1_Armor_0;

		public static readonly int Loadout1_Dye_0;

		public static readonly int Loadout2_Armor_0;

		public static readonly int Loadout2_Dye_0;

		public static readonly int Loadout3_Armor_0;

		public static readonly int Loadout3_Dye_0;

		private static int _nextSlotId;

		static PlayerItemSlotID()
		{
			Inventory0 = AllocateSlots(58);
			InventoryMouseItem = AllocateSlots(1);
			Armor0 = AllocateSlots(20);
			Dye0 = AllocateSlots(10);
			Misc0 = AllocateSlots(5);
			MiscDye0 = AllocateSlots(5);
			Bank1_0 = AllocateSlots(40);
			Bank2_0 = AllocateSlots(40);
			TrashItem = AllocateSlots(1);
			Bank3_0 = AllocateSlots(40);
			Bank4_0 = AllocateSlots(40);
			Loadout1_Armor_0 = AllocateSlots(20);
			Loadout1_Dye_0 = AllocateSlots(10);
			Loadout2_Armor_0 = AllocateSlots(20);
			Loadout2_Dye_0 = AllocateSlots(10);
			Loadout3_Armor_0 = AllocateSlots(20);
			Loadout3_Dye_0 = AllocateSlots(10);
		}

		private static int AllocateSlots(int amount)
		{
			int nextSlotId = _nextSlotId;
			_nextSlotId += amount;
			return nextSlotId;
		}
	}
}
