using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class CustomIntCondition : AchievementCondition
	{
		[JsonProperty("Value")]
		private int _value;

		private int _maxValue;

		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				int num = Utils.Clamp(value, 0, _maxValue);
				if (_tracker != null)
				{
					((ConditionIntTracker)_tracker).SetValue(num);
				}
				_value = num;
				if (_value == _maxValue)
				{
					Complete();
				}
			}
		}

		private CustomIntCondition(string name, int maxValue)
			: base(name)
		{
			_maxValue = maxValue;
			_value = 0;
		}

		public override void Clear()
		{
			_value = 0;
			base.Clear();
		}

		public override void Load(JObject state)
		{
			base.Load(state);
			_value = (int)state.get_Item("Value");
			if (_tracker != null)
			{
				((ConditionIntTracker)_tracker).SetValue(_value, reportUpdate: false);
			}
		}

		protected override IAchievementTracker CreateAchievementTracker()
		{
			return new ConditionIntTracker(_maxValue);
		}

		public static AchievementCondition Create(string name, int maxValue)
		{
			return new CustomIntCondition(name, maxValue);
		}

		public override void Complete()
		{
			if (_tracker != null)
			{
				((ConditionIntTracker)_tracker).SetValue(_maxValue);
			}
			_value = _maxValue;
			base.Complete();
		}
	}
}
