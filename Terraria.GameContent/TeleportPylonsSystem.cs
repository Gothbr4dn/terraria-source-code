using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Tile_Entities;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.GameContent
{
	public class TeleportPylonsSystem : IOnPlayerJoining
	{
		private List<TeleportPylonInfo> _pylons = new List<TeleportPylonInfo>();

		private List<TeleportPylonInfo> _pylonsOld = new List<TeleportPylonInfo>();

		private int _cooldownForUpdatingPylonsList;

		private const int CooldownTimePerPylonsListUpdate = int.MaxValue;

		private SceneMetrics _sceneMetrics = new SceneMetrics();

		public List<TeleportPylonInfo> Pylons => _pylons;

		public void Update()
		{
			if (Main.netMode != 1)
			{
				if (_cooldownForUpdatingPylonsList > 0)
				{
					_cooldownForUpdatingPylonsList--;
					return;
				}
				_cooldownForUpdatingPylonsList = int.MaxValue;
				UpdatePylonsListAndBroadcastChanges();
			}
		}

		public bool HasPylonOfType(TeleportPylonType pylonType)
		{
			return _pylons.Any((TeleportPylonInfo x) => x.TypeOfPylon == pylonType);
		}

		public bool HasAnyPylon()
		{
			return _pylons.Count > 0;
		}

		public void RequestImmediateUpdate()
		{
			if (Main.netMode != 1)
			{
				_cooldownForUpdatingPylonsList = int.MaxValue;
				UpdatePylonsListAndBroadcastChanges();
			}
		}

		private void UpdatePylonsListAndBroadcastChanges()
		{
			Utils.Swap(ref _pylons, ref _pylonsOld);
			_pylons.Clear();
			foreach (TileEntity value in TileEntity.ByPosition.Values)
			{
				if (value is TETeleportationPylon tETeleportationPylon && tETeleportationPylon.TryGetPylonType(out var pylonType))
				{
					TeleportPylonInfo teleportPylonInfo = default(TeleportPylonInfo);
					teleportPylonInfo.PositionInTiles = tETeleportationPylon.Position;
					teleportPylonInfo.TypeOfPylon = pylonType;
					TeleportPylonInfo item = teleportPylonInfo;
					_pylons.Add(item);
				}
			}
			IEnumerable<TeleportPylonInfo> enumerable = _pylonsOld.Except(_pylons);
			foreach (TeleportPylonInfo item2 in _pylons.Except(_pylonsOld))
			{
				NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item2, NetTeleportPylonModule.SubPacketType.PylonWasAdded));
			}
			foreach (TeleportPylonInfo item3 in enumerable)
			{
				NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item3, NetTeleportPylonModule.SubPacketType.PylonWasRemoved));
			}
		}

		public void AddForClient(TeleportPylonInfo info)
		{
			if (!_pylons.Contains(info))
			{
				_pylons.Add(info);
			}
		}

		public void RemoveForClient(TeleportPylonInfo info)
		{
			_pylons.RemoveAll((TeleportPylonInfo x) => x.Equals(info));
		}

		public void HandleTeleportRequest(TeleportPylonInfo info, int playerIndex)
		{
			Player player = Main.player[playerIndex];
			string key = null;
			bool flag = true;
			if (flag)
			{
				flag &= IsPlayerNearAPylon(player);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
				}
			}
			if (flag)
			{
				int necessaryNPCCount = HowManyNPCsDoesPylonNeed(info, player);
				flag &= DoesPylonHaveEnoughNPCsAroundIt(info, necessaryNPCCount);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCs";
				}
			}
			if (flag)
			{
				flag &= !NPC.AnyDanger(quickBossNPCCheck: false, ignorePillarsAndMoonlordCountdown: true);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseThereIsDanger";
				}
			}
			if (flag)
			{
				if (!NPC.downedPlantBoss && (double)info.PositionInTiles.Y > Main.worldSurface && Framing.GetTileSafely(info.PositionInTiles.X, info.PositionInTiles.Y).wall == 87)
				{
					flag = false;
				}
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseAccessingLihzahrdTempleEarly";
				}
			}
			if (flag)
			{
				_sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
				{
					VisualScanArea = null,
					BiomeScanCenterPositionInWorld = info.PositionInTiles.ToWorldCoordinates(),
					ScanOreFinderData = false
				});
				flag = DoesPylonAcceptTeleportation(info, player);
				if (!flag)
				{
					key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
				}
			}
			if (flag)
			{
				bool flag2 = false;
				int num = 0;
				for (int i = 0; i < _pylons.Count; i++)
				{
					TeleportPylonInfo info2 = _pylons[i];
					if (!player.InInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y, TileReachCheckSettings.Pylons))
					{
						continue;
					}
					if (num < 1)
					{
						num = 1;
					}
					int necessaryNPCCount2 = HowManyNPCsDoesPylonNeed(info2, player);
					if (DoesPylonHaveEnoughNPCsAroundIt(info2, necessaryNPCCount2))
					{
						if (num < 2)
						{
							num = 2;
						}
						_sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
						{
							VisualScanArea = null,
							BiomeScanCenterPositionInWorld = info2.PositionInTiles.ToWorldCoordinates(),
							ScanOreFinderData = false
						});
						if (DoesPylonAcceptTeleportation(info2, player))
						{
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					flag = false;
					key = num switch
					{
						1 => "Net.CannotTeleportToPylonBecauseNotEnoughNPCsAtCurrentPylon", 
						2 => "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements", 
						_ => "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon", 
					};
				}
			}
			if (flag)
			{
				Vector2 newPos = info.PositionInTiles.ToWorldCoordinates() - new Vector2(0f, player.HeightOffsetBoost);
				int num2 = 9;
				int typeOfPylon = (int)info.TypeOfPylon;
				int number = 0;
				player.Teleport(newPos, num2, typeOfPylon);
				player.velocity = Vector2.Zero;
				if (Main.netMode == 2)
				{
					RemoteClient.CheckSection(player.whoAmI, player.position);
					NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, num2, number, typeOfPylon);
				}
			}
			else
			{
				ChatHelper.SendChatMessageToClient(NetworkText.FromKey(key), new Color(255, 240, 20), playerIndex);
			}
		}

		public static bool IsPlayerNearAPylon(Player player)
		{
			return player.IsTileTypeInInteractionRange(597, TileReachCheckSettings.Pylons);
		}

		private bool DoesPylonHaveEnoughNPCsAroundIt(TeleportPylonInfo info, int necessaryNPCCount)
		{
			if (necessaryNPCCount <= 0)
			{
				return true;
			}
			Point16 positionInTiles = info.PositionInTiles;
			return DoesPositionHaveEnoughNPCs(necessaryNPCCount, positionInTiles);
		}

		public static bool DoesPositionHaveEnoughNPCs(int necessaryNPCCount, Point16 centerPoint)
		{
			Rectangle rectangle = new Rectangle(centerPoint.X - Main.buffScanAreaWidth / 2, centerPoint.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight);
			int num = necessaryNPCCount;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (!nPC.active || !nPC.isLikeATownNPC || nPC.homeless || !rectangle.Contains(nPC.homeTileX, nPC.homeTileY))
				{
					continue;
				}
				Vector2 value = new Vector2(nPC.homeTileX, nPC.homeTileY);
				Vector2 value2 = new Vector2(nPC.Center.X / 16f, nPC.Center.Y / 16f);
				if (Vector2.Distance(value, value2) < 100f)
				{
					num--;
					if (num == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void RequestTeleportation(TeleportPylonInfo info, Player player)
		{
			NetManager.Instance.SendToServerOrLoopback(NetTeleportPylonModule.SerializeUseRequest(info));
		}

		private bool DoesPylonAcceptTeleportation(TeleportPylonInfo info, Player player)
		{
			if (Main.netMode != 2 && Main.DroneCameraTracker != null && Main.DroneCameraTracker.IsInUse())
			{
				return false;
			}
			switch (info.TypeOfPylon)
			{
			case TeleportPylonType.SurfacePurity:
			{
				bool flag = (double)info.PositionInTiles.Y <= Main.worldSurface;
				if (Main.remixWorld)
				{
					flag = (double)info.PositionInTiles.Y > Main.rockLayer && info.PositionInTiles.Y < Main.maxTilesY - 350;
				}
				bool flag2 = info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
				if (!flag || flag2)
				{
					return false;
				}
				if (_sceneMetrics.EnoughTilesForJungle || _sceneMetrics.EnoughTilesForSnow || _sceneMetrics.EnoughTilesForDesert || _sceneMetrics.EnoughTilesForGlowingMushroom || _sceneMetrics.EnoughTilesForHallow || _sceneMetrics.EnoughTilesForCrimson || _sceneMetrics.EnoughTilesForCorruption)
				{
					return false;
				}
				return true;
			}
			case TeleportPylonType.Jungle:
				return _sceneMetrics.EnoughTilesForJungle;
			case TeleportPylonType.Snow:
				return _sceneMetrics.EnoughTilesForSnow;
			case TeleportPylonType.Desert:
				return _sceneMetrics.EnoughTilesForDesert;
			case TeleportPylonType.Beach:
			{
				bool flag3 = (double)info.PositionInTiles.Y <= Main.worldSurface && (double)info.PositionInTiles.Y > Main.worldSurface * 0.3499999940395355;
				return (info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380) && flag3;
			}
			case TeleportPylonType.GlowingMushroom:
				if (Main.remixWorld && info.PositionInTiles.Y >= Main.maxTilesY - 200)
				{
					return false;
				}
				return _sceneMetrics.EnoughTilesForGlowingMushroom;
			case TeleportPylonType.Hallow:
				return _sceneMetrics.EnoughTilesForHallow;
			case TeleportPylonType.Underground:
				return (double)info.PositionInTiles.Y >= Main.worldSurface;
			case TeleportPylonType.Victory:
				return true;
			default:
				return true;
			}
		}

		private int HowManyNPCsDoesPylonNeed(TeleportPylonInfo info, Player player)
		{
			TeleportPylonType typeOfPylon = info.TypeOfPylon;
			if (typeOfPylon != TeleportPylonType.Victory)
			{
				return 2;
			}
			return 0;
		}

		public void Reset()
		{
			_pylons.Clear();
			_cooldownForUpdatingPylonsList = 0;
		}

		public void OnPlayerJoining(int playerIndex)
		{
			foreach (TeleportPylonInfo pylon in _pylons)
			{
				NetManager.Instance.SendToClient(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(pylon, NetTeleportPylonModule.SubPacketType.PylonWasAdded), playerIndex);
			}
		}

		public static void SpawnInWorldDust(int tileStyle, Rectangle dustBox)
		{
			float r = 1f;
			float g = 1f;
			float b = 1f;
			switch ((byte)tileStyle)
			{
			case 0:
				r = 0.05f;
				g = 0.8f;
				b = 0.3f;
				break;
			case 1:
				r = 0.7f;
				g = 0.8f;
				b = 0.05f;
				break;
			case 2:
				r = 0.5f;
				g = 0.3f;
				b = 0.7f;
				break;
			case 3:
				r = 0.4f;
				g = 0.4f;
				b = 0.6f;
				break;
			case 4:
				r = 0.2f;
				g = 0.2f;
				b = 0.95f;
				break;
			case 5:
				r = 0.85f;
				g = 0.45f;
				b = 0.1f;
				break;
			case 6:
				r = 1f;
				g = 1f;
				b = 1.2f;
				break;
			case 7:
				r = 0.4f;
				g = 0.7f;
				b = 1.2f;
				break;
			case 8:
				r = 0.7f;
				g = 0.7f;
				b = 0.7f;
				break;
			}
			int num = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 43, 0f, 0f, 254, new Color(r, g, b, 1f), 0.5f);
			Main.dust[num].velocity *= 0.1f;
			Main.dust[num].velocity.Y -= 0.2f;
		}
	}
}
