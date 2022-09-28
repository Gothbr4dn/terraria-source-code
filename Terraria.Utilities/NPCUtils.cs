using System;
using Microsoft.Xna.Framework;

namespace Terraria.Utilities
{
	public static class NPCUtils
	{
		public delegate bool SearchFilter<T>(T entity) where T : Entity;

		public delegate void NPCTargetingMethod(NPC searcher, bool faceTarget, Vector2? checkPosition);

		public static class SearchFilters
		{
			public static bool OnlyCrystal(NPC npc)
			{
				if (npc.type == 548)
				{
					return !npc.dontTakeDamageFromHostiles;
				}
				return false;
			}

			public static SearchFilter<Player> OnlyPlayersInCertainDistance(Vector2 position, float maxDistance)
			{
				return (Player player) => player.Distance(position) <= maxDistance;
			}

			public static bool NonBeeNPCs(NPC npc)
			{
				if (npc.type != 211 && npc.type != 210 && npc.type != 222)
				{
					return npc.CanBeChasedBy();
				}
				return false;
			}

			public static SearchFilter<Player> DownwindFromNPC(NPC npc, float maxDistanceX)
			{
				return delegate(Player player)
				{
					float windSpeedCurrent = Main.windSpeedCurrent;
					float num = player.Center.X - npc.Center.X;
					float num2 = Math.Abs(num);
					float num3 = Math.Abs(player.Center.Y - npc.Center.Y);
					return player.active && !player.dead && num3 < 100f && num2 < maxDistanceX && ((num > 0f && windSpeedCurrent > 0f) || (num < 0f && windSpeedCurrent < 0f));
				};
			}
		}

		public enum TargetType
		{
			None,
			NPC,
			Player,
			TankPet
		}

		public struct TargetSearchResults
		{
			private TargetType _nearestTargetType;

			private int _nearestNPCIndex;

			private float _nearestNPCDistance;

			private int _nearestTankIndex;

			private float _nearestTankDistance;

			private float _adjustedTankDistance;

			private TargetType _nearestTankType;

			public int NearestTargetIndex
			{
				get
				{
					switch (_nearestTargetType)
					{
					case TargetType.Player:
					case TargetType.TankPet:
						return _nearestTankIndex;
					case TargetType.NPC:
						return NearestNPC.WhoAmIToTargettingIndex;
					default:
						return -1;
					}
				}
			}

			public Rectangle NearestTargetHitbox => _nearestTargetType switch
			{
				TargetType.Player => NearestTankOwner.Hitbox, 
				TargetType.TankPet => Main.projectile[NearestTankOwner.tankPet].Hitbox, 
				TargetType.NPC => NearestNPC.Hitbox, 
				_ => Rectangle.Empty, 
			};

			public TargetType NearestTargetType => _nearestTargetType;

			public bool FoundTarget => _nearestTargetType != TargetType.None;

			public NPC NearestNPC
			{
				get
				{
					if (_nearestNPCIndex != -1)
					{
						return Main.npc[_nearestNPCIndex];
					}
					return null;
				}
			}

			public bool FoundNPC => _nearestNPCIndex != -1;

			public int NearestNPCIndex => _nearestNPCIndex;

			public float NearestNPCDistance => _nearestNPCDistance;

			public Player NearestTankOwner
			{
				get
				{
					if (_nearestTankIndex != -1)
					{
						return Main.player[_nearestTankIndex];
					}
					return null;
				}
			}

			public bool FoundTank => _nearestTankIndex != -1;

			public int NearestTankOwnerIndex => _nearestTankIndex;

			public float NearestTankDistance => _nearestTankDistance;

			public float AdjustedTankDistance => _adjustedTankDistance;

			public TargetType NearestTankType => _nearestTankType;

			public TargetSearchResults(NPC searcher, int nearestNPCIndex, float nearestNPCDistance, int nearestTankIndex, float nearestTankDistance, float adjustedTankDistance, TargetType tankType)
			{
				_nearestNPCIndex = nearestNPCIndex;
				_nearestNPCDistance = nearestNPCDistance;
				_nearestTankIndex = nearestTankIndex;
				_adjustedTankDistance = adjustedTankDistance;
				_nearestTankDistance = nearestTankDistance;
				_nearestTankType = tankType;
				if (_nearestNPCIndex != -1 && _nearestTankIndex != -1)
				{
					if (_nearestNPCDistance < _adjustedTankDistance)
					{
						_nearestTargetType = TargetType.NPC;
					}
					else
					{
						_nearestTargetType = tankType;
					}
				}
				else if (_nearestNPCIndex != -1)
				{
					_nearestTargetType = TargetType.NPC;
				}
				else if (_nearestTankIndex != -1)
				{
					_nearestTargetType = tankType;
				}
				else
				{
					_nearestTargetType = TargetType.None;
				}
			}
		}

		[Flags]
		public enum TargetSearchFlag
		{
			None = 0,
			NPCs = 1,
			Players = 2,
			All = 3
		}

		public static TargetSearchResults SearchForTarget(Vector2 position, TargetSearchFlag flags = TargetSearchFlag.All, SearchFilter<Player> playerFilter = null, SearchFilter<NPC> npcFilter = null)
		{
			return SearchForTarget(null, position, flags, playerFilter, npcFilter);
		}

		public static TargetSearchResults SearchForTarget(NPC searcher, TargetSearchFlag flags = TargetSearchFlag.All, SearchFilter<Player> playerFilter = null, SearchFilter<NPC> npcFilter = null)
		{
			return SearchForTarget(searcher, searcher.Center, flags, playerFilter, npcFilter);
		}

		public static TargetSearchResults SearchForTarget(NPC searcher, Vector2 position, TargetSearchFlag flags = TargetSearchFlag.All, SearchFilter<Player> playerFilter = null, SearchFilter<NPC> npcFilter = null)
		{
			float num = float.MaxValue;
			int nearestNPCIndex = -1;
			float num2 = float.MaxValue;
			float nearestTankDistance = float.MaxValue;
			int nearestTankIndex = -1;
			TargetType tankType = TargetType.Player;
			if (flags.HasFlag(TargetSearchFlag.NPCs))
			{
				for (int i = 0; i < 200; i++)
				{
					NPC nPC = Main.npc[i];
					if (nPC.active && nPC.whoAmI != searcher.whoAmI && (npcFilter == null || npcFilter(nPC)))
					{
						float num3 = Vector2.DistanceSquared(position, nPC.Center);
						if (num3 < num)
						{
							nearestNPCIndex = i;
							num = num3;
						}
					}
				}
			}
			if (flags.HasFlag(TargetSearchFlag.Players))
			{
				for (int j = 0; j < 255; j++)
				{
					Player player = Main.player[j];
					if (!player.active || player.dead || player.ghost || (playerFilter != null && !playerFilter(player)))
					{
						continue;
					}
					float num4 = Vector2.Distance(position, player.Center);
					float num5 = num4 - (float)player.aggro;
					bool flag = searcher != null && player.npcTypeNoAggro[searcher.type];
					if (searcher != null && flag && searcher.direction == 0)
					{
						num5 += 1000f;
					}
					if (num5 < num2)
					{
						nearestTankIndex = j;
						num2 = num5;
						nearestTankDistance = num4;
						tankType = TargetType.Player;
					}
					if (player.tankPet >= 0 && !flag)
					{
						Vector2 center = Main.projectile[player.tankPet].Center;
						num4 = Vector2.Distance(position, center);
						num5 = num4 - 200f;
						if (num5 < num2 && num5 < 200f && Collision.CanHit(position, 0, 0, center, 0, 0))
						{
							nearestTankIndex = j;
							num2 = num5;
							nearestTankDistance = num4;
							tankType = TargetType.TankPet;
						}
					}
				}
			}
			return new TargetSearchResults(searcher, nearestNPCIndex, (float)Math.Sqrt(num), nearestTankIndex, nearestTankDistance, num2, tankType);
		}

		public static void TargetClosestOldOnesInvasion(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			TargetSearchResults searchResults = SearchForTarget(searcher, TargetSearchFlag.All, SearchFilters.OnlyPlayersInCertainDistance(searcher.Center, 200f), SearchFilters.OnlyCrystal);
			if (searchResults.FoundTarget)
			{
				searcher.target = searchResults.NearestTargetIndex;
				searcher.targetRect = searchResults.NearestTargetHitbox;
				if (searcher.ShouldFaceTarget(ref searchResults) && faceTarget)
				{
					searcher.FaceTarget();
				}
			}
		}

		public static void TargetClosestNonBees(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			TargetSearchResults searchResults = SearchForTarget(searcher, TargetSearchFlag.All, null, SearchFilters.NonBeeNPCs);
			if (searchResults.FoundTarget)
			{
				searcher.target = searchResults.NearestTargetIndex;
				searcher.targetRect = searchResults.NearestTargetHitbox;
				if (searcher.ShouldFaceTarget(ref searchResults) && faceTarget)
				{
					searcher.FaceTarget();
				}
			}
		}

		public static void TargetClosestDownwindFromNPC(NPC searcher, float distanceMaxX, bool faceTarget = true, Vector2? checkPosition = null)
		{
			TargetSearchResults searchResults = SearchForTarget(searcher, TargetSearchFlag.Players, SearchFilters.DownwindFromNPC(searcher, distanceMaxX));
			if (searchResults.FoundTarget)
			{
				searcher.target = searchResults.NearestTargetIndex;
				searcher.targetRect = searchResults.NearestTargetHitbox;
				if (searcher.ShouldFaceTarget(ref searchResults) && faceTarget)
				{
					searcher.FaceTarget();
				}
			}
		}

		public static void TargetClosestCommon(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			searcher.TargetClosest(faceTarget);
		}

		public static void TargetClosestBetsy(NPC searcher, bool faceTarget = true, Vector2? checkPosition = null)
		{
			TargetSearchResults searchResults = SearchForTarget(searcher, TargetSearchFlag.All, null, SearchFilters.OnlyCrystal);
			if (searchResults.FoundTarget)
			{
				TargetType value = searchResults.NearestTargetType;
				if (searchResults.FoundTank && !searchResults.NearestTankOwner.dead)
				{
					value = TargetType.Player;
				}
				searcher.target = searchResults.NearestTargetIndex;
				searcher.targetRect = searchResults.NearestTargetHitbox;
				if (searcher.ShouldFaceTarget(ref searchResults, value) && faceTarget)
				{
					searcher.FaceTarget();
				}
			}
		}
	}
}
