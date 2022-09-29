using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class BigProgressBarSystem
	{
		private IBigProgressBar _currentBar;

		private CommonBossBigProgressBar _bossBar = new CommonBossBigProgressBar();

		private BigProgressBarInfo _info;

		private static TwinsBigProgressBar _twinsBar = new TwinsBigProgressBar();

		private static EaterOfWorldsProgressBar _eaterOfWorldsBar = new EaterOfWorldsProgressBar();

		private static BrainOfCthuluBigProgressBar _brainOfCthuluBar = new BrainOfCthuluBigProgressBar();

		private static GolemHeadProgressBar _golemBar = new GolemHeadProgressBar();

		private static MoonLordProgressBar _moonlordBar = new MoonLordProgressBar();

		private static SolarFlarePillarBigProgressBar _solarPillarBar = new SolarFlarePillarBigProgressBar();

		private static VortexPillarBigProgressBar _vortexPillarBar = new VortexPillarBigProgressBar();

		private static NebulaPillarBigProgressBar _nebulaPillarBar = new NebulaPillarBigProgressBar();

		private static StardustPillarBigProgressBar _stardustPillarBar = new StardustPillarBigProgressBar();

		private static NeverValidProgressBar _neverValid = new NeverValidProgressBar();

		private static PirateShipBigProgressBar _pirateShipBar = new PirateShipBigProgressBar();

		private static MartianSaucerBigProgressBar _martianSaucerBar = new MartianSaucerBigProgressBar();

		private static DeerclopsBigProgressBar _deerclopsBar = new DeerclopsBigProgressBar();

		public static bool ShowText = true;

		private Dictionary<int, IBigProgressBar> _bossBarsByNpcNetId = new Dictionary<int, IBigProgressBar>
		{
			{ 125, _twinsBar },
			{ 126, _twinsBar },
			{ 13, _eaterOfWorldsBar },
			{ 14, _eaterOfWorldsBar },
			{ 15, _eaterOfWorldsBar },
			{ 266, _brainOfCthuluBar },
			{ 245, _golemBar },
			{ 246, _golemBar },
			{ 249, _neverValid },
			{ 517, _solarPillarBar },
			{ 422, _vortexPillarBar },
			{ 507, _nebulaPillarBar },
			{ 493, _stardustPillarBar },
			{ 398, _moonlordBar },
			{ 396, _moonlordBar },
			{ 397, _moonlordBar },
			{ 548, _neverValid },
			{ 549, _neverValid },
			{ 491, _pirateShipBar },
			{ 492, _pirateShipBar },
			{ 440, _neverValid },
			{ 395, _martianSaucerBar },
			{ 393, _martianSaucerBar },
			{ 394, _martianSaucerBar },
			{ 68, _neverValid },
			{ 668, _deerclopsBar }
		};

		private const string _preferencesKey = "ShowBossBarHealthText";

		public void Initialize()
		{
			Main.Configuration.OnLoad += Configuration_OnLoad;
			Main.Configuration.OnSave += Configuration_Save;
		}

		public void Update()
		{
			if (_currentBar == null)
			{
				TryFindingNPCToTrack();
			}
			if (_currentBar != null && !_currentBar.ValidateAndCollectNecessaryInfo(ref _info))
			{
				_currentBar = null;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (_currentBar != null)
			{
				_currentBar.Draw(ref _info, spriteBatch);
			}
		}

		private void TryFindingNPCToTrack()
		{
			Rectangle value = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
			value.Inflate(5000, 5000);
			float num = float.PositiveInfinity;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.Hitbox.Intersects(value))
				{
					float num2 = nPC.Distance(Main.LocalPlayer.Center);
					if (num > num2 && TryTracking(i))
					{
						num = num2;
					}
				}
			}
		}

		public bool TryTracking(int npcIndex)
		{
			if (npcIndex < 0 || npcIndex > 200)
			{
				return false;
			}
			NPC nPC = Main.npc[npcIndex];
			if (!nPC.active)
			{
				return false;
			}
			BigProgressBarInfo bigProgressBarInfo = default(BigProgressBarInfo);
			bigProgressBarInfo.npcIndexToAimAt = npcIndex;
			BigProgressBarInfo info = bigProgressBarInfo;
			IBigProgressBar bigProgressBar = _bossBar;
			if (_bossBarsByNpcNetId.TryGetValue(nPC.netID, out var value))
			{
				bigProgressBar = value;
			}
			if (!bigProgressBar.ValidateAndCollectNecessaryInfo(ref info))
			{
				return false;
			}
			_currentBar = bigProgressBar;
			info.showText = true;
			_info = info;
			return true;
		}

		private void Configuration_Save(Preferences obj)
		{
			obj.Put("ShowBossBarHealthText", ShowText);
		}

		private void Configuration_OnLoad(Preferences obj)
		{
			ShowText = Main.Configuration.Get("ShowBossBarHealthText", ShowText);
		}

		public static void ToggleShowText()
		{
			ShowText = !ShowText;
		}
	}
}
