namespace Terraria.GameContent
{
	public struct PlayerEyeHelper
	{
		private enum EyeFrame
		{
			EyeOpen,
			EyeHalfClosed,
			EyeClosed
		}

		private enum EyeState
		{
			NormalBlinking,
			InStorm,
			InBed,
			JustTookDamage,
			IsModeratelyDamaged,
			IsBlind,
			IsTipsy,
			IsPoisoned
		}

		private EyeState _state;

		private int _timeInState;

		private const int TimeToActDamaged = 20;

		public int EyeFrameToShow { get; private set; }

		public void Update(Player player)
		{
			SetStateByPlayerInfo(player);
			UpdateEyeFrameToShow(player);
			_timeInState++;
		}

		private void UpdateEyeFrameToShow(Player player)
		{
			EyeFrame eyeFrameToShow = EyeFrame.EyeOpen;
			switch (_state)
			{
			case EyeState.NormalBlinking:
			{
				int num = _timeInState % 240 - 234;
				eyeFrameToShow = ((num >= 4) ? EyeFrame.EyeHalfClosed : ((num < 2) ? ((num >= 0) ? EyeFrame.EyeHalfClosed : EyeFrame.EyeOpen) : EyeFrame.EyeClosed));
				break;
			}
			case EyeState.InStorm:
				eyeFrameToShow = ((_timeInState % 120 - 114 < 0) ? EyeFrame.EyeHalfClosed : EyeFrame.EyeClosed);
				break;
			case EyeState.IsModeratelyDamaged:
			case EyeState.IsTipsy:
			case EyeState.IsPoisoned:
				eyeFrameToShow = ((_timeInState % 120 - 100 < 0) ? EyeFrame.EyeHalfClosed : EyeFrame.EyeClosed);
				break;
			case EyeState.InBed:
			{
				EyeFrame eyeFrame = (DoesPlayerCountAsModeratelyDamaged(player) ? EyeFrame.EyeHalfClosed : EyeFrame.EyeOpen);
				_timeInState = player.sleeping.timeSleeping;
				eyeFrameToShow = ((_timeInState >= 60) ? ((_timeInState < 120) ? EyeFrame.EyeHalfClosed : EyeFrame.EyeClosed) : eyeFrame);
				break;
			}
			case EyeState.IsBlind:
				eyeFrameToShow = EyeFrame.EyeClosed;
				break;
			case EyeState.JustTookDamage:
				eyeFrameToShow = EyeFrame.EyeClosed;
				break;
			}
			EyeFrameToShow = (int)eyeFrameToShow;
		}

		private void SetStateByPlayerInfo(Player player)
		{
			if (player.blackout || player.blind)
			{
				SwitchToState(EyeState.IsBlind);
			}
			else
			{
				if (_state == EyeState.JustTookDamage && _timeInState < 20)
				{
					return;
				}
				if (player.sleeping.isSleeping)
				{
					bool resetStateTimerEvenIfAlreadyInState = player.itemAnimation > 0;
					SwitchToState(EyeState.InBed, resetStateTimerEvenIfAlreadyInState);
					return;
				}
				if (DoesPlayerCountAsModeratelyDamaged(player))
				{
					SwitchToState(EyeState.IsModeratelyDamaged);
					return;
				}
				if (player.tipsy)
				{
					SwitchToState(EyeState.IsTipsy);
					return;
				}
				if (player.poisoned || player.venom || player.starving)
				{
					SwitchToState(EyeState.IsPoisoned);
					return;
				}
				bool flag = player.ZoneSandstorm || (player.ZoneSnow && Main.IsItRaining);
				if (player.behindBackWall)
				{
					flag = false;
				}
				if (flag)
				{
					SwitchToState(EyeState.InStorm);
				}
				else
				{
					SwitchToState(EyeState.NormalBlinking);
				}
			}
		}

		private void SwitchToState(EyeState newState, bool resetStateTimerEvenIfAlreadyInState = false)
		{
			if (_state != newState || resetStateTimerEvenIfAlreadyInState)
			{
				_state = newState;
				_timeInState = 0;
			}
		}

		private bool DoesPlayerCountAsModeratelyDamaged(Player player)
		{
			return (float)player.statLife <= (float)player.statLifeMax2 * 0.25f;
		}

		public void BlinkBecausePlayerGotHurt()
		{
			SwitchToState(EyeState.JustTookDamage, resetStateTimerEvenIfAlreadyInState: true);
		}
	}
}
