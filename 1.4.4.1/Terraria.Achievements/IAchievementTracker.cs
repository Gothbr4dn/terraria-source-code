namespace Terraria.Achievements
{
	public interface IAchievementTracker
	{
		void ReportAs(string name);

		TrackerType GetTrackerType();

		void Load();

		void Clear();
	}
}
