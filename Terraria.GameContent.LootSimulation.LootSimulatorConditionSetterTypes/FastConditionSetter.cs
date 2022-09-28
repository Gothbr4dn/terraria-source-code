using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
	public class FastConditionSetter : ISimulationConditionSetter
	{
		private Action<SimulatorInfo> _setup;

		private Action<SimulatorInfo> _tearDown;

		public FastConditionSetter(Action<SimulatorInfo> setup, Action<SimulatorInfo> tearDown)
		{
			_setup = setup;
			_tearDown = tearDown;
		}

		public void Setup(SimulatorInfo info)
		{
			if (_setup != null)
			{
				_setup(info);
			}
		}

		public void TearDown(SimulatorInfo info)
		{
			if (_tearDown != null)
			{
				_tearDown(info);
			}
		}

		public int GetTimesToRunMultiplier(SimulatorInfo info)
		{
			return 1;
		}
	}
}
