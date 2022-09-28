using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Achievements;
using Terraria.GameInput;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.UI
{
	public class InGameNotificationsTracker
	{
		private static List<IInGameNotification> _notifications = new List<IInGameNotification>();

		public static void Initialize()
		{
			Main.Achievements.OnAchievementCompleted += AddCompleted;
			SocialAPI.JoinRequests.OnRequestAdded += JoinRequests_OnRequestAdded;
			SocialAPI.JoinRequests.OnRequestRemoved += JoinRequests_OnRequestRemoved;
		}

		private static void JoinRequests_OnRequestAdded(UserJoinToServerRequest request)
		{
			AddJoinRequest(request);
		}

		private static void JoinRequests_OnRequestRemoved(UserJoinToServerRequest request)
		{
			for (int num = _notifications.Count - 1; num >= 0; num--)
			{
				if (_notifications[num].CreationObject == request)
				{
					_notifications.RemoveAt(num);
				}
			}
		}

		public static void DrawInGame(SpriteBatch sb)
		{
			float num = Main.screenHeight - 40;
			if (PlayerInput.UsingGamepad)
			{
				num -= 25f;
			}
			Vector2 positionAnchorBottom = new Vector2(Main.screenWidth / 2, num);
			foreach (IInGameNotification notification in _notifications)
			{
				notification.DrawInGame(sb, positionAnchorBottom);
				notification.PushAnchor(ref positionAnchorBottom);
				if (positionAnchorBottom.Y < -100f)
				{
					break;
				}
			}
		}

		public static void DrawInIngameOptions(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointIdLocalIndexToUse)
		{
			int num = 4;
			int num2 = area.Height / 5 - num;
			Rectangle area2 = new Rectangle(area.X, area.Y, area.Width - 6, num2);
			int num3 = 0;
			foreach (IInGameNotification notification in _notifications)
			{
				notification.DrawInNotificationsArea(spriteBatch, area2, ref gamepadPointIdLocalIndexToUse);
				area2.Y += num2 + num;
				num3++;
				if (num3 >= 5)
				{
					break;
				}
			}
		}

		public static void AddCompleted(Achievement achievement)
		{
			if (Main.netMode != 2)
			{
				_notifications.Add(new InGamePopups.AchievementUnlockedPopup(achievement));
			}
		}

		public static void AddJoinRequest(UserJoinToServerRequest request)
		{
			if (Main.netMode != 2)
			{
				_notifications.Add(new InGamePopups.PlayerWantsToJoinGamePopup(request));
			}
		}

		public static void Clear()
		{
			_notifications.Clear();
		}

		public static void Update()
		{
			for (int i = 0; i < _notifications.Count; i++)
			{
				_notifications[i].Update();
				if (_notifications[i].ShouldBeRemoved)
				{
					_notifications.Remove(_notifications[i]);
					i--;
				}
			}
		}
	}
}
