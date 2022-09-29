using System;

namespace Terraria.Social.Base
{
	public abstract class UserJoinToServerRequest
	{
		internal string UserDisplayName { get; private set; }

		internal string UserFullIdentifier { get; private set; }

		public event Action OnAccepted;

		public event Action OnRejected;

		public UserJoinToServerRequest(string userDisplayName, string fullIdentifier)
		{
			UserDisplayName = userDisplayName;
			UserFullIdentifier = fullIdentifier;
		}

		public void Accept()
		{
			if (this.OnAccepted != null)
			{
				this.OnAccepted();
			}
		}

		public void Reject()
		{
			if (this.OnRejected != null)
			{
				this.OnRejected();
			}
		}

		public abstract bool IsValid();

		public abstract string GetUserWrapperText();
	}
}
