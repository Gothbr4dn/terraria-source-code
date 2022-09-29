namespace Terraria.DataStructures
{
	public struct NPCStrengthHelper
	{
		private float _strengthOverride;

		private float _gameModeDifficulty;

		private GameModeData _gameModeData;

		public bool IsExpertMode
		{
			get
			{
				if (!(_strengthOverride >= 2f))
				{
					return _gameModeDifficulty >= 2f;
				}
				return true;
			}
		}

		public bool IsMasterMode
		{
			get
			{
				if (!(_strengthOverride >= 3f))
				{
					return _gameModeDifficulty >= 3f;
				}
				return true;
			}
		}

		public bool ExtraDamageForGetGoodWorld
		{
			get
			{
				if (!(_strengthOverride >= 4f))
				{
					return _gameModeDifficulty >= 4f;
				}
				return true;
			}
		}

		public NPCStrengthHelper(GameModeData data, float strength, bool isGetGoodWorld)
		{
			_strengthOverride = strength;
			_gameModeData = data;
			_gameModeDifficulty = 1f;
			if (_gameModeData.IsExpertMode)
			{
				_gameModeDifficulty += 1f;
			}
			if (_gameModeData.IsMasterMode)
			{
				_gameModeDifficulty += 1f;
			}
			if (isGetGoodWorld)
			{
				_gameModeDifficulty += 1f;
			}
		}
	}
}
