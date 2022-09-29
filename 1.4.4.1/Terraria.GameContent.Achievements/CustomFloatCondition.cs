using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class CustomFloatCondition : AchievementCondition
	{
		[JsonProperty("Value")]
		private float _value;

		private float _maxValue;

		public float Value
		{
			get
			{
				return _value;
			}
			set
			{
				float num = Utils.Clamp(value, 0f, _maxValue);
				if (_tracker != null)
				{
					((ConditionFloatTracker)_tracker).SetValue(num);
				}
				_value = num;
				if (_value == _maxValue)
				{
					Complete();
				}
			}
		}

		private CustomFloatCondition(string name, float maxValue)
			: base(name)
		{
			_maxValue = maxValue;
			_value = 0f;
		}

		public override void Clear()
		{
			_value = 0f;
			base.Clear();
		}

		public override void Load(JObject state)
		{
			base.Load(state);
			_value = (float)state.get_Item("Value");
			if (_tracker != null)
			{
				((ConditionFloatTracker)_tracker).SetValue(_value, reportUpdate: false);
			}
		}

		protected override IAchievementTracker CreateAchievementTracker()
		{
			return new ConditionFloatTracker(_maxValue);
		}

		public static AchievementCondition Create(string name, float maxValue)
		{
			return new CustomFloatCondition(name, maxValue);
		}

		public override void Complete()
		{
			if (_tracker != null)
			{
				((ConditionFloatTracker)_tracker).SetValue(_maxValue);
			}
			_value = _maxValue;
			base.Complete();
		}
	}
}
