using System;
using System.Collections.Generic;

namespace Terraria.UI.Gamepad
{
	public class UILinkPage
	{
		public int ID;

		public int PageOnLeft = -1;

		public int PageOnRight = -1;

		public int DefaultPoint;

		public int CurrentPoint;

		public Dictionary<int, UILinkPoint> LinkMap = new Dictionary<int, UILinkPoint>();

		public event Action<int, int> ReachEndEvent;

		public event Action TravelEvent;

		public event Action LeaveEvent;

		public event Action EnterEvent;

		public event Action UpdateEvent;

		public event Func<bool> IsValidEvent;

		public event Func<bool> CanEnterEvent;

		public event Action<int> OnPageMoveAttempt;

		public event Func<string> OnSpecialInteracts;

		public UILinkPage()
		{
		}

		public UILinkPage(int id)
		{
			ID = id;
		}

		public void Update()
		{
			if (this.UpdateEvent != null)
			{
				this.UpdateEvent();
			}
		}

		public void Leave()
		{
			if (this.LeaveEvent != null)
			{
				this.LeaveEvent();
			}
		}

		public void Enter()
		{
			if (this.EnterEvent != null)
			{
				this.EnterEvent();
			}
		}

		public bool IsValid()
		{
			if (this.IsValidEvent != null)
			{
				return this.IsValidEvent();
			}
			return true;
		}

		public bool CanEnter()
		{
			if (this.CanEnterEvent != null)
			{
				return this.CanEnterEvent();
			}
			return true;
		}

		public void TravelUp()
		{
			Travel(LinkMap[CurrentPoint].Up);
		}

		public void TravelDown()
		{
			Travel(LinkMap[CurrentPoint].Down);
		}

		public void TravelLeft()
		{
			Travel(LinkMap[CurrentPoint].Left);
		}

		public void TravelRight()
		{
			Travel(LinkMap[CurrentPoint].Right);
		}

		public void SwapPageLeft()
		{
			if (this.OnPageMoveAttempt != null)
			{
				this.OnPageMoveAttempt(-1);
			}
			UILinkPointNavigator.ChangePage(PageOnLeft);
		}

		public void SwapPageRight()
		{
			if (this.OnPageMoveAttempt != null)
			{
				this.OnPageMoveAttempt(1);
			}
			UILinkPointNavigator.ChangePage(PageOnRight);
		}

		private void Travel(int next)
		{
			if (next < 0)
			{
				if (this.ReachEndEvent != null)
				{
					this.ReachEndEvent(CurrentPoint, next);
					if (this.TravelEvent != null)
					{
						this.TravelEvent();
					}
				}
			}
			else
			{
				UILinkPointNavigator.ChangePoint(next);
				if (this.TravelEvent != null)
				{
					this.TravelEvent();
				}
			}
		}

		public string SpecialInteractions()
		{
			if (this.OnSpecialInteracts != null)
			{
				return this.OnSpecialInteracts();
			}
			return string.Empty;
		}
	}
}
