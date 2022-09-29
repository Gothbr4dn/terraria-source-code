namespace Terraria.GameContent.UI.ResourceSets
{
	public struct PlayerStatsSnapshot
	{
		public int Life;

		public int LifeMax;

		public int LifeFruitCount;

		public float LifePerSegment;

		public int Mana;

		public int ManaMax;

		public float ManaPerSegment;

		public PlayerStatsSnapshot(Player player)
		{
			Life = player.statLife;
			Mana = player.statMana;
			LifeMax = player.statLifeMax2;
			ManaMax = player.statManaMax2;
			float num = 20f;
			int num2 = player.statLifeMax / 20;
			int num3 = (player.statLifeMax - 400) / 5;
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 0)
			{
				num2 = player.statLifeMax / (20 + num3 / 4);
				num = (float)player.statLifeMax / 20f;
			}
			int num4 = player.statLifeMax2 - player.statLifeMax;
			num += (float)(num4 / num2);
			LifeFruitCount = num3;
			LifePerSegment = num;
			ManaPerSegment = 20f;
		}
	}
}
