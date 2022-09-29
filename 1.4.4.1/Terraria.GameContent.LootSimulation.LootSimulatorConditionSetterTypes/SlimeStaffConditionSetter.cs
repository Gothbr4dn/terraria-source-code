namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	public class SlimeStaffConditionSetter : ISimulationConditionSetter
	{
		private int _timesToRun;

		public SlimeStaffConditionSetter(int timesToRunMultiplier)
		{
			_timesToRun = timesToRunMultiplier;
		}

		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			switch (info.npcVictim.netID)
			{
			default:
				return 0;
			case -33:
			case -32:
			case -10:
			case -9:
			case -8:
			case -7:
			case -6:
			case -5:
			case -4:
			case -3:
			case 1:
			case 16:
			case 138:
			case 141:
			case 147:
			case 184:
			case 187:
			case 204:
			case 302:
			case 333:
			case 334:
			case 335:
			case 336:
			case 433:
			case 535:
			case 537:
				return _timesToRun;
			}
		}

		public void Setup(SimulatorInfo info)
		{
		}

		public void TearDown(SimulatorInfo info)
		{
		}
	}
}
