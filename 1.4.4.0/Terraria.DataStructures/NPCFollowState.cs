using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public class NPCFollowState
	{
		private NPC _npc;

		private int? _playerIndexBeingFollowed;

		private Vector2 _floorBreadcrumb;

		public Vector2 BreadcrumbPosition => _floorBreadcrumb;

		public bool IsFollowingPlayer => _playerIndexBeingFollowed.HasValue;

		public Player PlayerBeingFollowed
		{
			get
			{
				if (_playerIndexBeingFollowed.HasValue)
				{
					return Main.player[_playerIndexBeingFollowed.Value];
				}
				return null;
			}
		}

		public void FollowPlayer(int playerIndex)
		{
			_playerIndexBeingFollowed = playerIndex;
			_floorBreadcrumb = Main.player[playerIndex].Bottom;
			_npc.netUpdate = true;
		}

		public void StopFollowing()
		{
			_playerIndexBeingFollowed = null;
			MoveNPCBackHome();
			_npc.netUpdate = true;
		}

		public void Clear(NPC npcToBelongTo)
		{
			_npc = npcToBelongTo;
			_playerIndexBeingFollowed = null;
			_floorBreadcrumb = default(Vector2);
		}

		private bool ShouldSync()
		{
			return _npc.isLikeATownNPC;
		}

		public void WriteTo(BinaryWriter writer)
		{
			int num = (_playerIndexBeingFollowed.HasValue ? _playerIndexBeingFollowed.Value : (-1));
			writer.Write((short)num);
		}

		public void ReadFrom(BinaryReader reader)
		{
			short num = reader.ReadInt16();
			if (Main.player.IndexInRange(num))
			{
				_playerIndexBeingFollowed = num;
			}
		}

		private void MoveNPCBackHome()
		{
			_npc.ai[0] = 20f;
			_npc.ai[1] = 0f;
			_npc.ai[2] = 0f;
			_npc.ai[3] = 0f;
			_npc.netUpdate = true;
		}

		public void Update()
		{
			if (IsFollowingPlayer)
			{
				Player playerBeingFollowed = PlayerBeingFollowed;
				if (!playerBeingFollowed.active || playerBeingFollowed.dead)
				{
					StopFollowing();
					return;
				}
				UpdateBreadcrumbs(playerBeingFollowed);
				Dust.QuickDust(_floorBreadcrumb, Color.Red);
			}
		}

		private void UpdateBreadcrumbs(Player player)
		{
			Vector2? vector = null;
			if (player.velocity.Y == 0f && player.gravDir == 1f)
			{
				vector = player.Bottom;
			}
			int num = 8;
			if (vector.HasValue && Vector2.Distance(vector.Value, _floorBreadcrumb) >= (float)num)
			{
				_floorBreadcrumb = vector.Value;
				_npc.netUpdate = true;
			}
		}
	}
}
