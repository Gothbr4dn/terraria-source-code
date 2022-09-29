using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LootSimulation
{
	public class SimulatorInfo
	{
		public Player player;

		private double _originalDayTimeCounter;

		private bool _originalDayTimeFlag;

		private Vector2 _originalPlayerPosition;

		public bool runningExpertMode;

		public LootSimulationItemCounter itemCounter;

		public NPC npcVictim;

		public SimulatorInfo()
		{
			player = new Player();
			_originalDayTimeCounter = Main.time;
			_originalDayTimeFlag = Main.dayTime;
			_originalPlayerPosition = player.position;
			runningExpertMode = false;
		}

		public void ReturnToOriginalDaytime()
		{
			Main.dayTime = _originalDayTimeFlag;
			Main.time = _originalDayTimeCounter;
		}

		public void AddItem(int itemId, int amount)
		{
			itemCounter.AddItem(itemId, amount, runningExpertMode);
		}

		public void ReturnToOriginalPlayerPosition()
		{
			player.position = _originalPlayerPosition;
		}
	}
}
