namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	public class LuckyCoinConditionSetter : ISimulationConditionSetter
	{
		private int _timesToRun;

		public LuckyCoinConditionSetter(int timesToRunMultiplier)
		{
			_timesToRun = timesToRunMultiplier;
		}

		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			int netID = info.npcVictim.netID;
			if (netID != 216 && netID != 491)
			{
				return 0;
			}
			return _timesToRun;
		}

		public void Setup(SimulatorInfo info)
		{
		}

		public void TearDown(SimulatorInfo info)
		{
		}
	}
}
