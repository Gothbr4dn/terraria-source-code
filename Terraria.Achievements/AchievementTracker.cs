using Terraria.Social;

namespace Terraria.Achievements
{
	public abstract class AchievementTracker<T> : IAchievementTracker
	{
		protected T _value;

		protected T _maxValue;

		protected string _name;

		private TrackerType _type;

		public T Value => _value;

		public T MaxValue => _maxValue;

		protected AchievementTracker(TrackerType type)
		{
			_type = type;
		}

		void IAchievementTracker.ReportAs(string name)
		{
			_name = name;
		}

		TrackerType IAchievementTracker.GetTrackerType()
		{
			return _type;
		}

		void IAchievementTracker.Clear()
		{
			SetValue(default(T));
		}

		public void SetValue(T newValue, bool reportUpdate = true)
		{
			if (newValue.Equals(_value))
			{
				return;
			}
			_value = newValue;
			if (reportUpdate)
			{
				ReportUpdate();
				if (_value.Equals(_maxValue))
				{
					OnComplete();
				}
			}
		}

		public abstract void ReportUpdate();

		protected abstract void Load();

		void IAchievementTracker.Load()
		{
			Load();
		}

		protected void OnComplete()
		{
			if (SocialAPI.Achievements != null)
			{
				SocialAPI.Achievements.StoreStats();
			}
		}
	}
}
