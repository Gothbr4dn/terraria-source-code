using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Localization;
using Terraria.Social;

namespace Terraria.Achievements
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class Achievement
	{
		public delegate void AchievementCompleted(Achievement achievement);

		private static int _totalAchievements;

		public readonly string Name;

		public readonly LocalizedText FriendlyName;

		public readonly LocalizedText Description;

		public readonly int Id = _totalAchievements++;

		private AchievementCategory _category;

		private IAchievementTracker _tracker;

		[JsonProperty("Conditions")]
		private Dictionary<string, AchievementCondition> _conditions = new Dictionary<string, AchievementCondition>();

		private int _completedCount;

		public AchievementCategory Category => _category;

		public bool HasTracker => _tracker != null;

		public bool IsCompleted => _completedCount == _conditions.Count;

		public event AchievementCompleted OnCompleted;

		public IAchievementTracker GetTracker()
		{
			return _tracker;
		}

		public Achievement(string name)
		{
			Name = name;
			FriendlyName = Language.GetText("Achievements." + name + "_Name");
			Description = Language.GetText("Achievements." + name + "_Description");
		}

		public void ClearProgress()
		{
			_completedCount = 0;
			foreach (KeyValuePair<string, AchievementCondition> condition in _conditions)
			{
				condition.Value.Clear();
			}
			if (_tracker != null)
			{
				_tracker.Clear();
			}
		}

		public void Load(Dictionary<string, JObject> conditions)
		{
			foreach (KeyValuePair<string, JObject> condition in conditions)
			{
				if (_conditions.TryGetValue(condition.Key, out var value))
				{
					value.Load(condition.Value);
					if (value.IsCompleted)
					{
						_completedCount++;
					}
				}
			}
			if (_tracker != null)
			{
				_tracker.Load();
			}
		}

		public void AddCondition(AchievementCondition condition)
		{
			_conditions[condition.Name] = condition;
			condition.OnComplete += OnConditionComplete;
		}

		private void OnConditionComplete(AchievementCondition condition)
		{
			_completedCount++;
			if (_completedCount == _conditions.Count)
			{
				if (_tracker == null && SocialAPI.Achievements != null)
				{
					SocialAPI.Achievements.CompleteAchievement(Name);
				}
				if (this.OnCompleted != null)
				{
					this.OnCompleted(this);
				}
			}
		}

		private void UseTracker(IAchievementTracker tracker)
		{
			tracker.ReportAs("STAT_" + Name);
			_tracker = tracker;
		}

		public void UseTrackerFromCondition(string conditionName)
		{
			UseTracker(GetConditionTracker(conditionName));
		}

		public void UseConditionsCompletedTracker()
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();
			foreach (KeyValuePair<string, AchievementCondition> condition in _conditions)
			{
				conditionsCompletedTracker.AddCondition(condition.Value);
			}
			UseTracker(conditionsCompletedTracker);
		}

		public void UseConditionsCompletedTracker(params string[] conditions)
		{
			ConditionsCompletedTracker conditionsCompletedTracker = new ConditionsCompletedTracker();
			foreach (string key in conditions)
			{
				conditionsCompletedTracker.AddCondition(_conditions[key]);
			}
			UseTracker(conditionsCompletedTracker);
		}

		public void ClearTracker()
		{
			_tracker = null;
		}

		private IAchievementTracker GetConditionTracker(string name)
		{
			return _conditions[name].GetAchievementTracker();
		}

		public void AddConditions(params AchievementCondition[] conditions)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				AddCondition(conditions[i]);
			}
		}

		public AchievementCondition GetCondition(string conditionName)
		{
			if (_conditions.TryGetValue(conditionName, out var value))
			{
				return value;
			}
			return null;
		}

		public void SetCategory(AchievementCategory category)
		{
			_category = category;
		}
	}
}
