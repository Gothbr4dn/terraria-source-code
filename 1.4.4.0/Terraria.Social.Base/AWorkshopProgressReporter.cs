namespace Terraria.Social.Base
{
	public abstract class AWorkshopProgressReporter
	{
		public abstract bool HasOngoingTasks { get; }

		public abstract bool TryGetProgress(out float progress);
	}
}
