using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Terraria.Social.Base
{
	public class ServerJoinRequestsManager
	{
		private readonly List<UserJoinToServerRequest> _requests;

		public readonly ReadOnlyCollection<UserJoinToServerRequest> CurrentRequests;

		public event ServerJoinRequestEvent OnRequestAdded;

		public event ServerJoinRequestEvent OnRequestRemoved;

		public ServerJoinRequestsManager()
		{
			_requests = new List<UserJoinToServerRequest>();
			CurrentRequests = new ReadOnlyCollection<UserJoinToServerRequest>(_requests);
		}

		public void Update()
		{
			for (int num = _requests.Count - 1; num >= 0; num--)
			{
				if (!_requests[num].IsValid())
				{
					RemoveRequestAtIndex(num);
				}
			}
		}

		public void Add(UserJoinToServerRequest request)
		{
			for (int num = _requests.Count - 1; num >= 0; num--)
			{
				if (_requests[num].Equals(request))
				{
					RemoveRequestAtIndex(num);
				}
			}
			_requests.Add(request);
			request.OnAccepted += delegate
			{
				RemoveRequest(request);
			};
			request.OnRejected += delegate
			{
				RemoveRequest(request);
			};
			if (this.OnRequestAdded != null)
			{
				this.OnRequestAdded(request);
			}
		}

		private void RemoveRequestAtIndex(int i)
		{
			UserJoinToServerRequest request = _requests[i];
			_requests.RemoveAt(i);
			if (this.OnRequestRemoved != null)
			{
				this.OnRequestRemoved(request);
			}
		}

		private void RemoveRequest(UserJoinToServerRequest request)
		{
			if (_requests.Remove(request) && this.OnRequestRemoved != null)
			{
				this.OnRequestRemoved(request);
			}
		}
	}
}
