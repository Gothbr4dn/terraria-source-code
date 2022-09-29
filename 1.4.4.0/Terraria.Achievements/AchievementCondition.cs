using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.Achievements
{
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class AchievementCondition
	{
		public delegate void AchievementUpdate(AchievementCondition condition);

		public readonly string Name;

		protected IAchievementTracker _tracker;

		[JsonProperty("Completed")]
		private bool _isCompleted;

		public bool IsCompleted => _isCompleted;

		public event AchievementUpdate OnComplete;

		protected AchievementCondition(string name)
		{
			Name = name;
		}

		public virtual void Load(JObject state)
		{
			_isCompleted = (bool)state.get_Item("Completed");
		}

		public virtual void Clear()
		{
			_isCompleted = false;
		}

		public virtual void Complete()
		{
			if (!_isCompleted)
			{
				_isCompleted = true;
				if (this.OnComplete != null)
				{
					this.OnComplete(this);
				}
			}
		}

		protected virtual IAchievementTracker CreateAchievementTracker()
		{
			return null;
		}

		public IAchievementTracker GetAchievementTracker()
		{
			if (_tracker == null)
			{
				_tracker = CreateAchievementTracker();
			}
			return _tracker;
		}
	}
}
