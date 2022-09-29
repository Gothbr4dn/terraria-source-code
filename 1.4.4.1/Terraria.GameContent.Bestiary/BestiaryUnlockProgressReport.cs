namespace Terraria.GameContent.Bestiary
{
	public struct BestiaryUnlockProgressReport
	{
		public int EntriesTotal;

		public float CompletionAmountTotal;

		public float CompletionPercent
		{
			get
			{
				if (EntriesTotal == 0)
				{
					return 1f;
				}
				return CompletionAmountTotal / (float)EntriesTotal;
			}
		}
	}
}
