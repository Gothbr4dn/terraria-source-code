namespace Terraria.DataStructures
{
	public struct NPCStrengthHelper
	{
		private float _strength;

		private GameModeData _gameModeData;

		public bool IsExpertMode
		{
			get
			{
				if (!(_strength >= 2f))
				{
					return _gameModeData.IsExpertMode;
				}
				return true;
			}
		}

		public bool IsMasterMode
		{
			get
			{
				if (!(_strength >= 3f))
				{
					return _gameModeData.IsMasterMode;
				}
				return true;
			}
		}

		public NPCStrengthHelper(GameModeData data, float strength)
		{
			_strength = strength;
			_gameModeData = data;
		}
	}
}
