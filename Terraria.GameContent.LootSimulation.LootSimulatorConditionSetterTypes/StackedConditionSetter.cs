namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	public class StackedConditionSetter : ISimulationConditionSetter
	{
		private ISimulationConditionSetter[] _setters;

		public StackedConditionSetter(params ISimulationConditionSetter[] setters)
		{
			_setters = setters;
		}

		public void Setup(SimulatorInfo info)
		{
			for (int i = 0; i < _setters.Length; i++)
			{
				_setters[i].Setup(info);
			}
		}

		public void TearDown(SimulatorInfo info)
		{
			for (int i = 0; i < _setters.Length; i++)
			{
				_setters[i].TearDown(info);
			}
		}

		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			return 1;
		}
	}
}
