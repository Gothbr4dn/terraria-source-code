using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Peripherals.RGB;
using Terraria.GameInput;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	public class ChromaHotkeyPainter
	{
		private class ReactiveRGBKey
		{
			public readonly Keys XNAKey;

			public readonly string WhatIsThisKeyFor;

			private readonly Color _color;

			private readonly TimeSpan _duration;

			private TimeSpan _startTime;

			private TimeSpan _expireTime;

			private RgbKey _rgbKey;

			public bool Expired => _expireTime < Main.gameTimeCache.TotalGameTime;

			public ReactiveRGBKey(Keys key, Color color, TimeSpan duration, string whatIsThisKeyFor)
			{
				_color = color;
				XNAKey = key;
				WhatIsThisKeyFor = whatIsThisKeyFor;
				_duration = duration;
				_startTime = Main.gameTimeCache.TotalGameTime;
			}

			public void Update()
			{
				float amount = (float)Utils.GetLerpValue(_startTime.TotalSeconds, _expireTime.TotalSeconds, Main.gameTimeCache.TotalGameTime.TotalSeconds, clamped: true);
				_rgbKey.SetSolid(Color.Lerp(_color, Color.Black, amount));
			}

			public void Clear()
			{
				_rgbKey.Clear();
			}

			public void Unbind()
			{
				Main.Chroma.UnbindKey(XNAKey);
			}

			public void Bind()
			{
				_rgbKey = Main.Chroma.BindKey(XNAKey, WhatIsThisKeyFor);
			}

			public void Refresh()
			{
				_startTime = Main.gameTimeCache.TotalGameTime;
				_expireTime = _startTime;
				_expireTime.Add(_duration);
			}
		}

		private class PaintKey
		{
			private string _triggerName;

			private List<Keys> _xnaKeys;

			private List<RgbKey> _rgbKeys;

			public PaintKey(string triggerName, List<string> keys)
			{
				_triggerName = triggerName;
				_xnaKeys = new List<Keys>();
				foreach (string key in keys)
				{
					if (Enum.TryParse<Keys>(key, ignoreCase: true, out var result))
					{
						_xnaKeys.Add(result);
					}
				}
				_rgbKeys = new List<RgbKey>();
			}

			public void Unbind()
			{
				foreach (RgbKey rgbKey in _rgbKeys)
				{
					Main.Chroma.UnbindKey(rgbKey.Key);
				}
			}

			public void Bind()
			{
				foreach (Keys xnaKey in _xnaKeys)
				{
					_rgbKeys.Add(Main.Chroma.BindKey(xnaKey, _triggerName));
				}
				_rgbKeys = _rgbKeys.Distinct().ToList();
			}

			public void SetSolid(Color color)
			{
				foreach (RgbKey rgbKey in _rgbKeys)
				{
					rgbKey.SetSolid(color);
				}
			}

			public void SetClear()
			{
				foreach (RgbKey rgbKey in _rgbKeys)
				{
					rgbKey.Clear();
				}
			}

			public bool UsesKey(Keys key)
			{
				return _xnaKeys.Contains(key);
			}

			public void SetAlert(Color colorBase, Color colorFlash, float time, float flashesPerSecond)
			{
				if (time == -1f)
				{
					time = 10000f;
				}
				foreach (RgbKey rgbKey in _rgbKeys)
				{
					rgbKey.SetFlashing(colorBase, colorFlash, time, flashesPerSecond);
				}
			}

			public List<Keys> GetXNAKeysInUse()
			{
				return new List<Keys>(_xnaKeys);
			}
		}

		private static class PainterColors
		{
			private const float HOTKEY_COLOR_MULTIPLIER = 1f;

			public static readonly Color MovementKeys = Color.Gray * 1f;

			public static readonly Color QuickMount = Color.RoyalBlue * 1f;

			public static readonly Color QuickGrapple = Color.Lerp(Color.RoyalBlue, Color.Blue, 0.5f) * 1f;

			public static readonly Color QuickHealReady = Color.Pink * 1f;

			public static readonly Color QuickHealReadyUrgent = Color.DeepPink * 1f;

			public static readonly Color QuickHealCooldown = Color.HotPink * 0.5f * 1f;

			public static readonly Color QuickMana = new Color(40, 0, 230) * 1f;

			public static readonly Color Throw = Color.Red * 0.2f * 1f;

			public static readonly Color SmartCursor = Color.Gold;

			public static readonly Color SmartSelect = Color.Goldenrod;

			public static readonly Color DangerKeyBlocked = Color.Red * 1f;
		}

		private readonly Dictionary<string, PaintKey> _keys = new Dictionary<string, PaintKey>();

		private readonly List<ReactiveRGBKey> _reactiveKeys = new List<ReactiveRGBKey>();

		private List<Keys> _xnaKeysInUse = new List<Keys>();

		private Player _player;

		private int _quickHealAlert;

		private List<PaintKey> _wasdKeys = new List<PaintKey>();

		private PaintKey _healKey;

		private PaintKey _mountKey;

		private PaintKey _jumpKey;

		private PaintKey _grappleKey;

		private PaintKey _throwKey;

		private PaintKey _manaKey;

		private PaintKey _buffKey;

		private PaintKey _smartCursorKey;

		private PaintKey _smartSelectKey;

		public bool PotionAlert => _quickHealAlert != 0;

		public void CollectBoundKeys()
		{
			foreach (KeyValuePair<string, PaintKey> key in _keys)
			{
				key.Value.Unbind();
			}
			_keys.Clear();
			foreach (KeyValuePair<string, List<string>> item in PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard].KeyStatus)
			{
				_keys.Add(item.Key, new PaintKey(item.Key, item.Value));
			}
			foreach (KeyValuePair<string, PaintKey> key2 in _keys)
			{
				key2.Value.Bind();
			}
			_wasdKeys = new List<PaintKey>
			{
				_keys["Up"],
				_keys["Down"],
				_keys["Left"],
				_keys["Right"]
			};
			_healKey = _keys["QuickHeal"];
			_mountKey = _keys["QuickMount"];
			_jumpKey = _keys["Jump"];
			_grappleKey = _keys["Grapple"];
			_throwKey = _keys["Throw"];
			_manaKey = _keys["QuickMana"];
			_buffKey = _keys["QuickBuff"];
			_smartCursorKey = _keys["SmartCursor"];
			_smartSelectKey = _keys["SmartSelect"];
			_reactiveKeys.Clear();
			_xnaKeysInUse.Clear();
			foreach (KeyValuePair<string, PaintKey> key3 in _keys)
			{
				_xnaKeysInUse.AddRange(key3.Value.GetXNAKeysInUse());
			}
			_xnaKeysInUse = _xnaKeysInUse.Distinct().ToList();
		}

		[Old("Reactive keys are no longer used so this catch-all method isn't used")]
		public void PressKey(Keys key)
		{
		}

		private ReactiveRGBKey FindReactiveKey(Keys keyTarget)
		{
			return _reactiveKeys.FirstOrDefault((ReactiveRGBKey x) => x.XNAKey == keyTarget);
		}

		public void Update()
		{
			_player = Main.LocalPlayer;
			if (!Main.hasFocus)
			{
				Step_ClearAll();
				return;
			}
			if (PotionAlert)
			{
				foreach (KeyValuePair<string, PaintKey> key in _keys)
				{
					if (key.Key != "QuickHeal")
					{
						key.Value.SetClear();
					}
				}
				Step_QuickHeal();
			}
			else
			{
				Step_Movement();
				Step_QuickHeal();
			}
			if (Main.InGameUI.CurrentState == Main.ManageControlsMenu)
			{
				Step_ClearAll();
				Step_KeybindsMenu();
			}
			Step_UpdateReactiveKeys();
		}

		private void SetGroupColorBase(List<PaintKey> keys, Color color)
		{
			foreach (PaintKey key in keys)
			{
				key.SetSolid(color);
			}
		}

		private void SetGroupClear(List<PaintKey> keys)
		{
			foreach (PaintKey key in keys)
			{
				key.SetClear();
			}
		}

		private void Step_KeybindsMenu()
		{
			SetGroupColorBase(_wasdKeys, PainterColors.MovementKeys);
			_jumpKey.SetSolid(PainterColors.MovementKeys);
			_grappleKey.SetSolid(PainterColors.QuickGrapple);
			_mountKey.SetSolid(PainterColors.QuickMount);
			_quickHealAlert = 0;
			_healKey.SetSolid(PainterColors.QuickHealReady);
			_manaKey.SetSolid(PainterColors.QuickMana);
			_throwKey.SetSolid(PainterColors.Throw);
			_smartCursorKey.SetSolid(PainterColors.SmartCursor);
			_smartSelectKey.SetSolid(PainterColors.SmartSelect);
		}

		private void Step_UpdateReactiveKeys()
		{
			foreach (ReactiveRGBKey key in _reactiveKeys.FindAll((ReactiveRGBKey x) => x.Expired))
			{
				key.Clear();
				if (!_keys.Any((KeyValuePair<string, PaintKey> x) => x.Value.UsesKey(key.XNAKey)))
				{
					key.Unbind();
				}
			}
			_reactiveKeys.RemoveAll((ReactiveRGBKey x) => x.Expired);
			foreach (ReactiveRGBKey reactiveKey in _reactiveKeys)
			{
				reactiveKey.Update();
			}
		}

		private void Step_ClearAll()
		{
			foreach (KeyValuePair<string, PaintKey> key in _keys)
			{
				key.Value.SetClear();
			}
		}

		private void Step_SmartKeys()
		{
			PaintKey smartCursorKey = _smartCursorKey;
			PaintKey smartSelectKey = _smartSelectKey;
			if (_player.DeadOrGhost || _player.frozen || _player.tongued || _player.webbed || _player.stoned || _player.noItems)
			{
				smartCursorKey.SetClear();
				smartSelectKey.SetClear();
				return;
			}
			if (Main.SmartCursorWanted)
			{
				smartCursorKey.SetSolid(PainterColors.SmartCursor);
			}
			else
			{
				smartCursorKey.SetClear();
			}
			if (_player.nonTorch >= 0)
			{
				smartSelectKey.SetSolid(PainterColors.SmartSelect);
			}
			else
			{
				smartSelectKey.SetClear();
			}
		}

		private void Step_Movement()
		{
			List<PaintKey> wasdKeys = _wasdKeys;
			bool flag = _player.frozen || _player.tongued || _player.webbed || _player.stoned;
			if (_player.DeadOrGhost)
			{
				SetGroupClear(wasdKeys);
			}
			else if (flag)
			{
				SetGroupColorBase(wasdKeys, PainterColors.DangerKeyBlocked);
			}
			else
			{
				SetGroupColorBase(wasdKeys, PainterColors.MovementKeys);
			}
		}

		private void Step_Mount()
		{
			PaintKey mountKey = _mountKey;
			if (_player.QuickMount_GetItemToUse() == null || _player.DeadOrGhost)
			{
				mountKey.SetClear();
			}
			else if (_player.frozen || _player.tongued || _player.webbed || _player.stoned || _player.gravDir == -1f || _player.noItems)
			{
				mountKey.SetSolid(PainterColors.DangerKeyBlocked);
				if (_player.gravDir == -1f)
				{
					mountKey.SetSolid(PainterColors.DangerKeyBlocked * 0.6f);
				}
			}
			else
			{
				mountKey.SetSolid(PainterColors.QuickMount);
			}
		}

		private void Step_Grapple()
		{
			PaintKey grappleKey = _grappleKey;
			if (_player.QuickGrapple_GetItemToUse() == null || _player.DeadOrGhost)
			{
				grappleKey.SetClear();
			}
			else if (_player.frozen || _player.tongued || _player.webbed || _player.stoned || _player.noItems)
			{
				grappleKey.SetSolid(PainterColors.DangerKeyBlocked);
			}
			else
			{
				grappleKey.SetSolid(PainterColors.QuickGrapple);
			}
		}

		private void Step_Jump()
		{
			PaintKey jumpKey = _jumpKey;
			if (_player.DeadOrGhost)
			{
				jumpKey.SetClear();
			}
			else if (_player.frozen || _player.tongued || _player.webbed || _player.stoned)
			{
				jumpKey.SetSolid(PainterColors.DangerKeyBlocked);
			}
			else
			{
				jumpKey.SetSolid(PainterColors.MovementKeys);
			}
		}

		private void Step_QuickHeal()
		{
			PaintKey healKey = _healKey;
			if (_player.QuickHeal_GetItemToUse() == null || _player.DeadOrGhost)
			{
				healKey.SetClear();
				_quickHealAlert = 0;
			}
			else if (_player.potionDelay > 0)
			{
				float lerpValue = Utils.GetLerpValue(_player.potionDelayTime, 0f, _player.potionDelay, clamped: true);
				Color solid = Color.Lerp(PainterColors.DangerKeyBlocked, PainterColors.QuickHealCooldown, lerpValue) * lerpValue * lerpValue * lerpValue;
				healKey.SetSolid(solid);
				_quickHealAlert = 0;
			}
			else if (_player.statLife == _player.statLifeMax2)
			{
				healKey.SetClear();
				_quickHealAlert = 0;
			}
			else if ((float)_player.statLife <= (float)_player.statLifeMax2 / 4f)
			{
				if (_quickHealAlert != 1)
				{
					_quickHealAlert = 1;
					healKey.SetAlert(Color.Black, PainterColors.QuickHealReadyUrgent, -1f, 2f);
				}
			}
			else if ((float)_player.statLife <= (float)_player.statLifeMax2 / 2f)
			{
				if (_quickHealAlert != 2)
				{
					_quickHealAlert = 2;
					healKey.SetAlert(Color.Black, PainterColors.QuickHealReadyUrgent, -1f, 2f);
				}
			}
			else
			{
				healKey.SetSolid(PainterColors.QuickHealReady);
				_quickHealAlert = 0;
			}
		}

		private void Step_QuickMana()
		{
			PaintKey manaKey = _manaKey;
			if (_player.QuickMana_GetItemToUse() == null || _player.DeadOrGhost || _player.statMana == _player.statManaMax2)
			{
				manaKey.SetClear();
			}
			else
			{
				manaKey.SetSolid(PainterColors.QuickMana);
			}
		}

		private void Step_Throw()
		{
			PaintKey throwKey = _throwKey;
			_ = _player.HeldItem;
			if (_player.DeadOrGhost || _player.HeldItem.favorited || _player.noThrow > 0)
			{
				throwKey.SetClear();
			}
			else if (_player.frozen || _player.tongued || _player.webbed || _player.stoned || _player.noItems)
			{
				throwKey.SetClear();
			}
			else
			{
				throwKey.SetSolid(PainterColors.Throw);
			}
		}
	}
}
