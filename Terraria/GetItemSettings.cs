using System;

namespace Terraria
{
	public struct GetItemSettings
	{
		public static GetItemSettings InventoryEntityToPlayerInventorySettings = new GetItemSettings(LongText: false, NoText: true);

		public static GetItemSettings NPCEntityToPlayerInventorySettings = new GetItemSettings(LongText: true);

		public static GetItemSettings LootAllSettings = default(GetItemSettings);

		public static GetItemSettings LootAllSettingsRegularChest = new GetItemSettings(LongText: false, NoText: false, CanGoIntoVoidVault: true);

		public static GetItemSettings PickupItemFromWorld = new GetItemSettings(LongText: false, NoText: false, CanGoIntoVoidVault: true);

		public static GetItemSettings GetItemInDropItemCheck = new GetItemSettings(LongText: false, NoText: true);

		public static GetItemSettings InventoryUIToInventorySettings = default(GetItemSettings);

		public static GetItemSettings InventoryUIToInventorySettingsShowAsNew = new GetItemSettings(LongText: false, NoText: true, CanGoIntoVoidVault: false, MakeNewAndShiny);

		public static GetItemSettings ItemCreatedFromItemUsage = default(GetItemSettings);

		public readonly bool LongText;

		public readonly bool NoText;

		public readonly bool CanGoIntoVoidVault;

		public readonly Action<Item> StepAfterHandlingSlotNormally;

		public GetItemSettings(bool LongText = false, bool NoText = false, bool CanGoIntoVoidVault = false, Action<Item> StepAfterHandlingSlotNormally = null)
		{
			this.LongText = LongText;
			this.NoText = NoText;
			this.CanGoIntoVoidVault = CanGoIntoVoidVault;
			this.StepAfterHandlingSlotNormally = StepAfterHandlingSlotNormally;
		}

		public void HandlePostAction(Item item)
		{
			if (StepAfterHandlingSlotNormally != null)
			{
				StepAfterHandlingSlotNormally(item);
			}
		}

		private static void MakeNewAndShiny(Item item)
		{
			item.newAndShiny = true;
		}
	}
}
