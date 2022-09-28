using Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes;

namespace Terraria.GameContent.LootSimulation
{
	public class SimulationConditionSetters
	{
		public static FastConditionSetter HardMode = new FastConditionSetter(delegate
		{
			Main.hardMode = true;
		}, delegate
		{
			Main.hardMode = false;
		});

		public static FastConditionSetter ExpertMode = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.GameMode = 1;
			info.runningExpertMode = true;
		}, delegate(SimulatorInfo info)
		{
			Main.GameMode = 0;
			info.runningExpertMode = false;
		});

		public static FastConditionSetter Eclipse = new FastConditionSetter(delegate
		{
			Main.eclipse = true;
		}, delegate
		{
			Main.eclipse = false;
		});

		public static FastConditionSetter BloodMoon = new FastConditionSetter(delegate
		{
			Main.bloodMoon = true;
		}, delegate
		{
			Main.bloodMoon = false;
		});

		public static FastConditionSetter SlainMechBosses = new FastConditionSetter(delegate
		{
			NPC.downedMechBoss1 = (NPC.downedMechBoss2 = (NPC.downedMechBoss3 = (NPC.downedMechBossAny = true)));
		}, delegate
		{
			NPC.downedMechBoss1 = (NPC.downedMechBoss2 = (NPC.downedMechBoss3 = (NPC.downedMechBossAny = false)));
		});

		public static FastConditionSetter SlainPlantera = new FastConditionSetter(delegate
		{
			NPC.downedPlantBoss = true;
		}, delegate
		{
			NPC.downedPlantBoss = false;
		});

		public static StackedConditionSetter ExpertAndHardMode = new StackedConditionSetter(ExpertMode, HardMode);

		public static FastConditionSetter WindyWeather = new FastConditionSetter(delegate
		{
			Main._shouldUseWindyDayMusic = true;
		}, delegate
		{
			Main._shouldUseWindyDayMusic = false;
		});

		public static FastConditionSetter MidDay = new FastConditionSetter(delegate
		{
			Main.dayTime = true;
			Main.time = 27000.0;
		}, delegate(SimulatorInfo info)
		{
			info.ReturnToOriginalDaytime();
		});

		public static FastConditionSetter MidNight = new FastConditionSetter(delegate
		{
			Main.dayTime = false;
			Main.time = 16200.0;
		}, delegate(SimulatorInfo info)
		{
			info.ReturnToOriginalDaytime();
		});

		public static FastConditionSetter SlimeRain = new FastConditionSetter(delegate
		{
			Main.slimeRain = true;
		}, delegate
		{
			Main.slimeRain = false;
		});

		public static StackedConditionSetter WindyExpertHardmodeEndgameEclipseMorning = new StackedConditionSetter(WindyWeather, ExpertMode, HardMode, SlainMechBosses, SlainPlantera, Eclipse, MidDay);

		public static StackedConditionSetter WindyExpertHardmodeEndgameBloodMoonNight = new StackedConditionSetter(WindyWeather, ExpertMode, HardMode, SlainMechBosses, SlainPlantera, BloodMoon, MidNight);

		public static SlimeStaffConditionSetter SlimeStaffTest = new SlimeStaffConditionSetter(100);

		public static LuckyCoinConditionSetter LuckyCoinTest = new LuckyCoinConditionSetter(100);
	}
}
