using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent
{
	public class CoinLossRevengeSystem
	{
		public class RevengeMarker
		{
			private static int _uniqueIDCounter = 0;

			private static readonly int _expirationCompCopper = Item.buyPrice(0, 0, 0, 1);

			private static readonly int _expirationCompSilver = Item.buyPrice(0, 0, 1);

			private static readonly int _expirationCompGold = Item.buyPrice(0, 1);

			private static readonly int _expirationCompPlat = Item.buyPrice(1);

			private const int ONE_MINUTE = 3600;

			private const int ENEMY_BOX_WIDTH = 2160;

			private const int ENEMY_BOX_HEIGHT = 1440;

			public static readonly Vector2 EnemyBoxSize = new Vector2(2160f, 1440f);

			private readonly Vector2 _location;

			private readonly Rectangle _hitbox;

			private readonly int _npcNetID;

			private readonly float _npcHPPercent;

			private readonly float _baseValue;

			private readonly int _coinsValue;

			private readonly int _npcTypeAgainstDiscouragement;

			private readonly int _npcAIStyleAgainstDiscouragement;

			private readonly int _expirationTime;

			private readonly bool _spawnedFromStatue;

			private readonly int _uniqueID;

			private bool _forceExpire;

			private bool _attemptedRespawn;

			public bool RespawnAttemptLocked => _attemptedRespawn;

			public int UniqueID => _uniqueID;

			public void SetToExpire()
			{
				_forceExpire = true;
			}

			public void SetRespawnAttemptLock(bool state)
			{
				_attemptedRespawn = state;
			}

			public RevengeMarker(Vector2 coords, int npcNetId, float npcHPPercent, int npcType, int npcAiStyle, int coinValue, float baseValue, bool spawnedFromStatue, int gameTime, int uniqueID = -1)
			{
				_location = coords;
				_npcNetID = npcNetId;
				_npcHPPercent = npcHPPercent;
				_npcTypeAgainstDiscouragement = npcType;
				_npcAIStyleAgainstDiscouragement = npcAiStyle;
				_coinsValue = coinValue;
				_baseValue = baseValue;
				_spawnedFromStatue = spawnedFromStatue;
				_hitbox = Utils.CenteredRectangle(_location, EnemyBoxSize);
				_expirationTime = CalculateExpirationTime(gameTime, coinValue);
				if (uniqueID == -1)
				{
					_uniqueID = _uniqueIDCounter++;
				}
				else
				{
					_uniqueID = uniqueID;
				}
			}

			public bool IsInvalid()
			{
				int nPCInvasionGroup = NPC.GetNPCInvasionGroup(_npcTypeAgainstDiscouragement);
				switch (nPCInvasionGroup)
				{
				case 1:
				case 2:
				case 3:
				case 4:
					return nPCInvasionGroup != Main.invasionType;
				case -3:
					return !DD2Event.Ongoing;
				case -2:
					if (Main.pumpkinMoon)
					{
						return Main.dayTime;
					}
					return true;
				case -1:
					if (Main.snowMoon)
					{
						return Main.dayTime;
					}
					return true;
				default:
					switch (_npcTypeAgainstDiscouragement)
					{
					case 158:
					case 159:
					case 162:
					case 166:
					case 251:
					case 253:
					case 460:
					case 461:
					case 462:
					case 463:
					case 466:
					case 467:
					case 468:
					case 469:
					case 477:
					case 478:
					case 479:
						if (!Main.eclipse || !Main.dayTime)
						{
							return true;
						}
						break;
					}
					return false;
				}
			}

			public bool IsExpired(int gameTime)
			{
				if (!_forceExpire)
				{
					return _expirationTime <= gameTime;
				}
				return true;
			}

			private int CalculateExpirationTime(int gameCacheTime, int coinValue)
			{
				int num = 0;
				num = ((coinValue < _expirationCompSilver) ? ((int)MathHelper.Lerp(0f, 3600f, Utils.GetLerpValue(_expirationCompCopper, _expirationCompSilver, coinValue))) : ((coinValue < _expirationCompGold) ? ((int)MathHelper.Lerp(36000f, 108000f, Utils.GetLerpValue(_expirationCompSilver, _expirationCompGold, coinValue))) : ((coinValue >= _expirationCompPlat) ? 432000 : ((int)MathHelper.Lerp(108000f, 216000f, Utils.GetLerpValue(_expirationCompSilver, _expirationCompGold, coinValue))))));
				num += 18000;
				return gameCacheTime + num;
			}

			public bool Intersects(Rectangle rectInner, Rectangle rectOuter)
			{
				return rectOuter.Intersects(_hitbox);
			}

			public void SpawnEnemy()
			{
				int num = NPC.NewNPC(new EntitySource_RevengeSystem(), (int)_location.X, (int)_location.Y, _npcNetID);
				NPC nPC = Main.npc[num];
				if (_npcNetID < 0)
				{
					nPC.SetDefaults(_npcNetID);
				}
				if (NPCID.Sets.SpecialSpawningRules.TryGetValue(_npcNetID, out var value) && value == 0)
				{
					Point point = nPC.position.ToTileCoordinates();
					nPC.ai[0] = point.X;
					nPC.ai[1] = point.Y;
					nPC.netUpdate = true;
				}
				nPC.timeLeft += 3600;
				nPC.extraValue = _coinsValue;
				nPC.value = _baseValue;
				nPC.SpawnedFromStatue = _spawnedFromStatue;
				float num2 = Math.Max(0.5f, _npcHPPercent);
				nPC.life = (int)((float)nPC.lifeMax * num2);
				if (num < 200)
				{
					if (Main.netMode == 0)
					{
						nPC.moneyPing(_location);
					}
					else
					{
						NetMessage.SendData(23, -1, -1, null, num);
						NetMessage.SendData(92, -1, -1, null, num, _coinsValue, _location.X, _location.Y);
					}
				}
				if (DisplayCaching)
				{
					Main.NewText("Spawned " + nPC.GivenOrTypeName);
				}
			}

			public bool WouldNPCBeDiscouraged(Player playerTarget)
			{
				switch (_npcAIStyleAgainstDiscouragement)
				{
				case 2:
					return NPC.DespawnEncouragement_AIStyle2_FloatingEye_IsDiscouraged(_npcTypeAgainstDiscouragement, playerTarget.position);
				case 3:
					return !NPC.DespawnEncouragement_AIStyle3_Fighters_NotDiscouraged(_npcTypeAgainstDiscouragement, playerTarget.position, null);
				case 6:
				{
					bool flag = false;
					switch (_npcTypeAgainstDiscouragement)
					{
					case 513:
						flag = !playerTarget.ZoneUndergroundDesert;
						break;
					case 10:
					case 39:
					case 95:
					case 117:
					case 510:
						flag = true;
						break;
					}
					if (flag)
					{
						return (double)playerTarget.position.Y < Main.worldSurface * 16.0;
					}
					return false;
				}
				default:
					return _npcNetID switch
					{
						253 => !Main.eclipse, 
						490 => Main.dayTime, 
						_ => false, 
					};
				}
			}

			public bool DrawMapIcon(SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, int gameTime)
			{
				Vector2 vector = _location / 16f - mapTopLeft;
				vector *= mapScale;
				vector += mapX2Y2AndOff;
				if (mapRect.HasValue && !mapRect.Value.Contains(vector.ToPoint()))
				{
					return false;
				}
				Texture2D value = TextureAssets.MapDeath.get_Value();
				value = ((_coinsValue < 100) ? TextureAssets.Coin[0].get_Value() : ((_coinsValue < 10000) ? TextureAssets.Coin[1].get_Value() : ((_coinsValue >= 1000000) ? TextureAssets.Coin[3].get_Value() : TextureAssets.Coin[2].get_Value())));
				Rectangle rectangle = value.Frame(1, 8);
				spriteBatch.Draw(value, vector, rectangle, Color.White, 0f, rectangle.Size() / 2f, drawScale, SpriteEffects.None, 0f);
				return Utils.CenteredRectangle(vector, rectangle.Size() * drawScale).Contains(Main.MouseScreen.ToPoint());
			}

			public void UseMouseOver(SpriteBatch spriteBatch, ref string mouseTextString, float drawScale = 1f)
			{
				mouseTextString = "";
				Vector2 vector = Main.MouseScreen / drawScale + new Vector2(-28f) + new Vector2(4f, 0f);
				ItemSlot.DrawMoney(spriteBatch, "", vector.X, vector.Y, Utils.CoinsSplit(_coinsValue), horizontal: true);
			}

			public void WriteSelfTo(BinaryWriter writer)
			{
				writer.Write(_uniqueID);
				writer.WriteVector2(_location);
				writer.Write(_npcNetID);
				writer.Write(_npcHPPercent);
				writer.Write(_npcTypeAgainstDiscouragement);
				writer.Write(_npcAIStyleAgainstDiscouragement);
				writer.Write(_coinsValue);
				writer.Write(_baseValue);
				writer.Write(_spawnedFromStatue);
			}
		}

		public static bool DisplayCaching = false;

		public static int MinimumCoinsForCaching = Item.buyPrice(0, 0, 10);

		private const int PLAYER_BOX_WIDTH_INNER = 1968;

		private const int PLAYER_BOX_HEIGHT_INNER = 1200;

		private const int PLAYER_BOX_WIDTH_OUTER = 2608;

		private const int PLAYER_BOX_HEIGHT_OUTER = 1840;

		private static readonly Vector2 _playerBoxSizeInner = new Vector2(1968f, 1200f);

		private static readonly Vector2 _playerBoxSizeOuter = new Vector2(2608f, 1840f);

		private List<RevengeMarker> _markers;

		private readonly object _markersLock = new object();

		private int _gameTime;

		public void AddMarkerFromReader(BinaryReader reader)
		{
			int uniqueID = reader.ReadInt32();
			Vector2 coords = reader.ReadVector2();
			int npcNetId = reader.ReadInt32();
			float npcHPPercent = reader.ReadSingle();
			int npcType = reader.ReadInt32();
			int npcAiStyle = reader.ReadInt32();
			int coinValue = reader.ReadInt32();
			float baseValue = reader.ReadSingle();
			bool spawnedFromStatue = reader.ReadBoolean();
			RevengeMarker marker = new RevengeMarker(coords, npcNetId, npcHPPercent, npcType, npcAiStyle, coinValue, baseValue, spawnedFromStatue, _gameTime, uniqueID);
			AddMarker(marker);
		}

		private void AddMarker(RevengeMarker marker)
		{
			lock (_markersLock)
			{
				_markers.Add(marker);
			}
		}

		public void DestroyMarker(int markerUniqueID)
		{
			lock (_markersLock)
			{
				_markers.RemoveAll((RevengeMarker x) => x.UniqueID == markerUniqueID);
			}
		}

		public CoinLossRevengeSystem()
		{
			_markers = new List<RevengeMarker>();
		}

		public void CacheEnemy(NPC npc)
		{
			if (npc.boss || npc.realLife != -1 || npc.rarity > 0 || npc.extraValue < MinimumCoinsForCaching || npc.position.X < Main.leftWorld + 640f + 16f || npc.position.X + (float)npc.width > Main.rightWorld - 640f - 32f || npc.position.Y < Main.topWorld + 640f + 16f || npc.position.Y > Main.bottomWorld - 640f - 32f - (float)npc.height)
			{
				return;
			}
			int num = npc.netID;
			if (NPCID.Sets.RespawnEnemyID.TryGetValue(num, out var value))
			{
				num = value;
			}
			if (num != 0)
			{
				RevengeMarker marker = new RevengeMarker(npc.Center, num, npc.GetLifePercent(), npc.type, npc.aiStyle, npc.extraValue, npc.value, npc.SpawnedFromStatue, _gameTime);
				AddMarker(marker);
				if (Main.netMode == 2)
				{
					NetMessage.SendCoinLossRevengeMarker(marker);
				}
				if (DisplayCaching)
				{
					Main.NewText("Cached " + npc.GivenOrTypeName);
				}
			}
		}

		public void Reset()
		{
			lock (_markersLock)
			{
				_markers.Clear();
			}
			_gameTime = 0;
		}

		public void Update()
		{
			_gameTime++;
			if (Main.netMode == 1 && _gameTime % 60 == 0)
			{
				RemoveExpiredOrInvalidMarkers();
			}
		}

		public void CheckRespawns()
		{
			lock (_markersLock)
			{
				if (_markers.Count == 0)
				{
					return;
				}
			}
			List<Tuple<int, Rectangle, Rectangle>> list = new List<Tuple<int, Rectangle, Rectangle>>();
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead)
				{
					list.Add(Tuple.Create(i, Utils.CenteredRectangle(player.Center, _playerBoxSizeInner), Utils.CenteredRectangle(player.Center, _playerBoxSizeOuter)));
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			RemoveExpiredOrInvalidMarkers();
			lock (_markersLock)
			{
				List<RevengeMarker> list2 = new List<RevengeMarker>();
				for (int j = 0; j < _markers.Count; j++)
				{
					RevengeMarker revengeMarker = _markers[j];
					bool flag = false;
					Tuple<int, Rectangle, Rectangle> tuple = null;
					foreach (Tuple<int, Rectangle, Rectangle> item in list)
					{
						if (revengeMarker.Intersects(item.Item2, item.Item3))
						{
							tuple = item;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						revengeMarker.SetRespawnAttemptLock(state: false);
					}
					else if (!revengeMarker.RespawnAttemptLocked)
					{
						revengeMarker.SetRespawnAttemptLock(state: true);
						if (revengeMarker.WouldNPCBeDiscouraged(Main.player[tuple.Item1]))
						{
							revengeMarker.SetToExpire();
							continue;
						}
						revengeMarker.SpawnEnemy();
						list2.Add(revengeMarker);
					}
				}
				_markers = _markers.Except(list2).ToList();
			}
		}

		private void RemoveExpiredOrInvalidMarkers()
		{
			lock (_markersLock)
			{
				IEnumerable<RevengeMarker> enumerable = _markers.Where((RevengeMarker x) => x.IsExpired(_gameTime));
				IEnumerable<RevengeMarker> enumerable2 = _markers.Where((RevengeMarker x) => x.IsInvalid());
				_markers.RemoveAll((RevengeMarker x) => x.IsInvalid());
				_markers.RemoveAll((RevengeMarker x) => x.IsExpired(_gameTime));
			}
		}

		public RevengeMarker DrawMapIcons(SpriteBatch spriteBatch, Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale, float drawScale, ref string unused)
		{
			RevengeMarker result = null;
			lock (_markersLock)
			{
				foreach (RevengeMarker marker in _markers)
				{
					if (marker.DrawMapIcon(spriteBatch, mapTopLeft, mapX2Y2AndOff, mapRect, mapScale, drawScale, _gameTime))
					{
						result = marker;
					}
				}
				return result;
			}
		}

		public void SendAllMarkersToPlayer(int plr)
		{
			lock (_markersLock)
			{
				foreach (RevengeMarker marker in _markers)
				{
					NetMessage.SendCoinLossRevengeMarker(marker, plr);
				}
			}
		}
	}
}
