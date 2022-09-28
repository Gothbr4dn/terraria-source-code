using Terraria.DataStructures;

namespace Terraria
{
	public struct NPCSpawnParams
	{
		public float? sizeScaleOverride;

		public int? playerCountForMultiplayerDifficultyOverride;

		public GameModeData gameModeData;

		public float? strengthMultiplierOverride;

		public NPCSpawnParams WithScale(float scaleOverride)
		{
			NPCSpawnParams result = default(NPCSpawnParams);
			result.sizeScaleOverride = scaleOverride;
			result.playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride;
			result.gameModeData = gameModeData;
			result.strengthMultiplierOverride = strengthMultiplierOverride;
			return result;
		}
	}
}
