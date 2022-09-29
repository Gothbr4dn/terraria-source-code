using Terraria.Social;

namespace Terraria.Achievements
{
	public class ConditionFloatTracker : AchievementTracker<float>
	{
		public ConditionFloatTracker(float maxValue)
			: base(TrackerType.Float)
		{
			_maxValue = maxValue;
		}

		public ConditionFloatTracker()
			: base(TrackerType.Float)
		{
		}

		public override void ReportUpdate()
		{
			if (SocialAPI.Achievements != null && _name != null)
			{
				SocialAPI.Achievements.UpdateFloatStat(_name, _value);
			}
		}

		protected override void Load()
		{
		}
	}
}
