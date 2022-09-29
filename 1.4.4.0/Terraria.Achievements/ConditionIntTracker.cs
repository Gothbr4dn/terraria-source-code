using Terraria.Social;

namespace Terraria.Achievements
{
	public class ConditionIntTracker : AchievementTracker<int>
	{
		public ConditionIntTracker()
			: base(TrackerType.Int)
		{
		}

		public ConditionIntTracker(int maxValue)
			: base(TrackerType.Int)
		{
			_maxValue = maxValue;
		}

		public override void ReportUpdate()
		{
			if (SocialAPI.Achievements != null && _name != null)
			{
				SocialAPI.Achievements.UpdateIntStat(_name, _value);
			}
		}

		protected override void Load()
		{
		}
	}
}
