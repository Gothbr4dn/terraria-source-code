using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.GameInput
{
	public class LockOnHelper
	{
		public enum LockOnMode
		{
			FocusTarget,
			TargetClosest,
			ThreeDS
		}

		private const float LOCKON_RANGE = 2000f;

		private const int LOCKON_HOLD_LIFETIME = 40;

		public static LockOnMode UseMode = LockOnMode.ThreeDS;

		private static bool _enabled;

		private static bool _canLockOn;

		private static List<int> _targets = new List<int>();

		private static int _pickedTarget;

		private static int _lifeTimeCounter;

		private static int _lifeTimeArrowDisplay;

		private static int _threeDSTarget = -1;

		private static int _targetClosestTarget = -1;

		public static bool ForceUsability = false;

		private static float[,] _drawProgress = new float[200, 2];

		public static NPC AimedTarget
		{
			get
			{
				if (_pickedTarget == -1 || _targets.Count < 1)
				{
					return null;
				}
				return Main.npc[_targets[_pickedTarget]];
			}
		}

		public static Vector2 PredictedPosition
		{
			get
			{
				NPC aimedTarget = AimedTarget;
				if (aimedTarget == null)
				{
					return Vector2.Zero;
				}
				Vector2 vector = aimedTarget.Center;
				if (NPC.GetNPCLocation(_targets[_pickedTarget], seekHead: true, averageDirection: false, out var index, out var pos))
				{
					vector = pos;
					vector += Main.npc[index].Distance(Main.player[Main.myPlayer].Center) / 2000f * Main.npc[index].velocity * 45f;
				}
				Player player = Main.player[Main.myPlayer];
				int num = ItemID.Sets.LockOnAimAbove[player.inventory[player.selectedItem].type];
				while (num > 0 && vector.Y > 100f)
				{
					Point point = vector.ToTileCoordinates();
					point.Y -= 4;
					if (!WorldGen.InWorld(point.X, point.Y, 10) || WorldGen.SolidTile(point.X, point.Y))
					{
						break;
					}
					vector.Y -= 16f;
					num--;
				}
				float? num2 = ItemID.Sets.LockOnAimCompensation[player.inventory[player.selectedItem].type];
				if (num2.HasValue)
				{
					vector.Y -= aimedTarget.height / 2;
					Vector2 v = vector - player.Center;
					Vector2 vector2 = v.SafeNormalize(Vector2.Zero);
					vector2.Y -= 1f;
					float num3 = v.Length();
					num3 = (float)Math.Pow(num3 / 700f, 2.0) * 700f;
					vector.Y += vector2.Y * num3 * num2.Value * 1f;
					vector.X += (0f - vector2.X) * num3 * num2.Value * 1f;
				}
				return vector;
			}
		}

		public static bool Enabled => _enabled;

		public static void CycleUseModes()
		{
			switch (UseMode)
			{
			case LockOnMode.FocusTarget:
				UseMode = LockOnMode.TargetClosest;
				break;
			case LockOnMode.TargetClosest:
				UseMode = LockOnMode.ThreeDS;
				break;
			case LockOnMode.ThreeDS:
				UseMode = LockOnMode.TargetClosest;
				break;
			}
		}

		public static void Update()
		{
			_canLockOn = false;
			if (!CanUseLockonSystem())
			{
				SetActive(on: false);
				return;
			}
			if (--_lifeTimeArrowDisplay < 0)
			{
				_lifeTimeArrowDisplay = 0;
			}
			FindMostViableTarget(LockOnMode.ThreeDS, ref _threeDSTarget);
			FindMostViableTarget(LockOnMode.TargetClosest, ref _targetClosestTarget);
			if (PlayerInput.Triggers.JustPressed.LockOn && !PlayerInput.WritingText)
			{
				_lifeTimeCounter = 40;
				_lifeTimeArrowDisplay = 30;
				HandlePressing();
			}
			if (!_enabled)
			{
				return;
			}
			if (UseMode == LockOnMode.FocusTarget && PlayerInput.Triggers.Current.LockOn)
			{
				if (_lifeTimeCounter <= 0)
				{
					SetActive(on: false);
					return;
				}
				_lifeTimeCounter--;
			}
			NPC aimedTarget = AimedTarget;
			if (!ValidTarget(aimedTarget))
			{
				SetActive(on: false);
			}
			if (UseMode == LockOnMode.TargetClosest)
			{
				SetActive(on: false);
				SetActive(CanEnable());
			}
			if (_enabled)
			{
				Player player = Main.player[Main.myPlayer];
				Vector2 predictedPosition = PredictedPosition;
				bool flag = false;
				if (ShouldLockOn(player) && (ItemID.Sets.LockOnIgnoresCollision[player.inventory[player.selectedItem].type] || Collision.CanHit(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHit(player.Center, 0, 0, aimedTarget.Center, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, aimedTarget.Center, 0, 0)))
				{
					flag = true;
				}
				if (flag)
				{
					_canLockOn = true;
				}
			}
		}

		public static bool CanUseLockonSystem()
		{
			if (!ForceUsability)
			{
				return PlayerInput.UsingGamepad;
			}
			return true;
		}

		public static void SetUP()
		{
			if (_canLockOn)
			{
				_ = AimedTarget;
				SetLockPosition(Main.ReverseGravitySupport(PredictedPosition - Main.screenPosition));
			}
		}

		public static void SetDOWN()
		{
			if (_canLockOn)
			{
				ResetLockPosition();
			}
		}

		private static bool ShouldLockOn(Player p)
		{
			int type = p.inventory[p.selectedItem].type;
			if (type == 496)
			{
				return false;
			}
			return true;
		}

		public static void Toggle(bool forceOff = false)
		{
			_lifeTimeCounter = 40;
			_lifeTimeArrowDisplay = 30;
			HandlePressing();
			if (forceOff)
			{
				_enabled = false;
			}
		}

		private static void FindMostViableTarget(LockOnMode context, ref int targetVar)
		{
			targetVar = -1;
			if (UseMode == context && CanUseLockonSystem())
			{
				List<int> t = new List<int>();
				int t2 = -1;
				Utils.Swap(ref t, ref _targets);
				Utils.Swap(ref t2, ref _pickedTarget);
				RefreshTargets(Main.MouseWorld, 2000f);
				GetClosestTarget(Main.MouseWorld);
				Utils.Swap(ref t, ref _targets);
				Utils.Swap(ref t2, ref _pickedTarget);
				if (t2 >= 0)
				{
					targetVar = t[t2];
				}
				t.Clear();
			}
		}

		private static void HandlePressing()
		{
			if (UseMode == LockOnMode.TargetClosest)
			{
				SetActive(!_enabled);
			}
			else if (UseMode == LockOnMode.ThreeDS)
			{
				if (!_enabled)
				{
					SetActive(on: true);
				}
				else
				{
					CycleTargetThreeDS();
				}
			}
			else if (!_enabled)
			{
				SetActive(on: true);
			}
			else
			{
				CycleTargetFocus();
			}
		}

		private static void CycleTargetFocus()
		{
			int num = _targets[_pickedTarget];
			RefreshTargets(Main.MouseWorld, 2000f);
			if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]))
			{
				SetActive(on: false);
				return;
			}
			_pickedTarget = 0;
			for (int i = 0; i < _targets.Count; i++)
			{
				if (_targets[i] > num)
				{
					_pickedTarget = i;
					break;
				}
			}
		}

		private static void CycleTargetThreeDS()
		{
			int num = _targets[_pickedTarget];
			RefreshTargets(Main.MouseWorld, 2000f);
			GetClosestTarget(Main.MouseWorld);
			if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]) || num == _targets[_pickedTarget])
			{
				SetActive(on: false);
			}
		}

		private static bool CanEnable()
		{
			if (Main.player[Main.myPlayer].dead)
			{
				return false;
			}
			return true;
		}

		private static void SetActive(bool on)
		{
			if (on)
			{
				if (CanEnable())
				{
					RefreshTargets(Main.MouseWorld, 2000f);
					GetClosestTarget(Main.MouseWorld);
					if (_pickedTarget >= 0)
					{
						_enabled = true;
					}
				}
			}
			else
			{
				_enabled = false;
				_targets.Clear();
				_lifeTimeCounter = 0;
				_threeDSTarget = -1;
				_targetClosestTarget = -1;
			}
		}

		private static void RefreshTargets(Vector2 position, float radius)
		{
			_targets.Clear();
			Rectangle rectangle = Utils.CenteredRectangle(Main.player[Main.myPlayer].Center, new Vector2(1920f, 1200f));
			_ = Main.player[Main.myPlayer].Center;
			Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC nPC = Main.npc[i];
				if (ValidTarget(nPC) && !(nPC.Distance(position) > radius) && rectangle.Intersects(nPC.Hitbox) && !(Lighting.GetSubLight(nPC.Center).Length() / 3f < 0.03f))
				{
					_targets.Add(i);
				}
			}
		}

		private static void GetClosestTarget(Vector2 position)
		{
			_pickedTarget = -1;
			float num = -1f;
			if (UseMode == LockOnMode.ThreeDS)
			{
				Vector2 center = Main.player[Main.myPlayer].Center;
				Vector2 value = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
				for (int i = 0; i < _targets.Count; i++)
				{
					int num2 = _targets[i];
					NPC obj = Main.npc[num2];
					float num3 = Vector2.Dot(obj.DirectionFrom(center), value);
					if (ValidTarget(obj) && (_pickedTarget == -1 || !(num3 <= num)))
					{
						_pickedTarget = i;
						num = num3;
					}
				}
				return;
			}
			for (int j = 0; j < _targets.Count; j++)
			{
				int num4 = _targets[j];
				NPC nPC = Main.npc[num4];
				if (ValidTarget(nPC) && (_pickedTarget == -1 || !(nPC.Distance(position) >= num)))
				{
					_pickedTarget = j;
					num = nPC.Distance(position);
				}
			}
		}

		private static bool ValidTarget(NPC n)
		{
			if (n == null || !n.active || n.dontTakeDamage || n.friendly || n.isLikeATownNPC || n.life < 1 || n.immortal)
			{
				return false;
			}
			if (n.aiStyle == 25 && n.ai[0] == 0f)
			{
				return false;
			}
			return true;
		}

		private static void SetLockPosition(Vector2 position)
		{
			PlayerInput.LockOnCachePosition();
			Main.mouseX = (PlayerInput.MouseX = (int)position.X);
			Main.mouseY = (PlayerInput.MouseY = (int)position.Y);
		}

		private static void ResetLockPosition()
		{
			PlayerInput.LockOnUnCachePosition();
			Main.mouseX = PlayerInput.MouseX;
			Main.mouseY = PlayerInput.MouseY;
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (Main.gameMenu)
			{
				return;
			}
			Texture2D value = TextureAssets.LockOnCursor.get_Value();
			Rectangle rectangle = new Rectangle(0, 0, value.Width, 12);
			Rectangle rectangle2 = new Rectangle(0, 16, value.Width, 12);
			Color t = Main.OurFavoriteColor.MultiplyRGBA(new Color(0.75f, 0.75f, 0.75f, 1f));
			t.A = 220;
			Color t2 = Main.OurFavoriteColor;
			t2.A = 220;
			float num = 0.94f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f)) * 0.06f;
			t2 *= num;
			t *= num;
			Utils.Swap(ref t, ref t2);
			Color color = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
			Color color2 = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
			float gravDir = Main.player[Main.myPlayer].gravDir;
			float num2 = 1f;
			float num3 = 0.1f;
			float num4 = 0.8f;
			float num5 = 1f;
			float num6 = 10f;
			float num7 = 10f;
			bool flag = false;
			for (int i = 0; i < _drawProgress.GetLength(0); i++)
			{
				int num8 = 0;
				if (_pickedTarget != -1 && _targets.Count > 0 && i == _targets[_pickedTarget])
				{
					num8 = 2;
				}
				else if ((flag && _targets.Contains(i)) || (UseMode == LockOnMode.ThreeDS && _threeDSTarget == i) || (UseMode == LockOnMode.TargetClosest && _targetClosestTarget == i))
				{
					num8 = 1;
				}
				_drawProgress[i, 0] = MathHelper.Clamp(_drawProgress[i, 0] + ((num8 == 1) ? num3 : (0f - num3)), 0f, 1f);
				_drawProgress[i, 1] = MathHelper.Clamp(_drawProgress[i, 1] + ((num8 == 2) ? num3 : (0f - num3)), 0f, 1f);
				float num9 = _drawProgress[i, 0];
				if (num9 > 0f)
				{
					float num10 = 1f - num9 * num9;
					Vector2 pos = Main.npc[i].Top + new Vector2(0f, 0f - num7 - num10 * num6) * gravDir - Main.screenPosition;
					pos = Main.ReverseGravitySupport(pos, Main.npc[i].height);
					spriteBatch.Draw(value, pos, rectangle, color * num9, 0f, rectangle.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, SpriteEffects.None, 0f);
					spriteBatch.Draw(value, pos, rectangle2, color2 * num9 * num9, 0f, rectangle2.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num4 * (1f + num9) / 2f, SpriteEffects.None, 0f);
				}
				float num11 = _drawProgress[i, 1];
				if (num11 > 0f)
				{
					int num12 = Main.npc[i].width;
					if (Main.npc[i].height > num12)
					{
						num12 = Main.npc[i].height;
					}
					num12 += 20;
					if ((float)num12 < 70f)
					{
						num5 *= (float)num12 / 70f;
					}
					float num13 = 3f;
					Vector2 vector = Main.npc[i].Center;
					if (_targets.Count >= 0 && _pickedTarget >= 0 && _pickedTarget < _targets.Count && i == _targets[_pickedTarget] && NPC.GetNPCLocation(i, seekHead: true, averageDirection: false, out var _, out var pos2))
					{
						vector = pos2;
					}
					for (int j = 0; (float)j < num13; j++)
					{
						float num14 = MathF.PI * 2f / num13 * (float)j + Main.GlobalTimeWrappedHourly * (MathF.PI * 2f) * 0.25f;
						Vector2 vector2 = new Vector2(0f, num12 / 2).RotatedBy(num14);
						Vector2 pos3 = vector + vector2 - Main.screenPosition;
						pos3 = Main.ReverseGravitySupport(pos3);
						float rotation = num14 * (float)((gravDir == 1f) ? 1 : (-1)) + MathF.PI * (float)((gravDir == 1f) ? 1 : 0);
						spriteBatch.Draw(value, pos3, rectangle, t * num11, rotation, rectangle.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f, SpriteEffects.None, 0f);
						spriteBatch.Draw(value, pos3, rectangle2, t2 * num11 * num11, rotation, rectangle2.Size() / 2f, new Vector2(0.58f, 1f) * num2 * num5 * (1f + num11) / 2f, SpriteEffects.None, 0f);
					}
				}
			}
		}
	}
}
