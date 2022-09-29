using System;

namespace Terraria.Utilities
{
	public sealed class OldAttribute : Attribute
	{
		private string message;

		public string Message => message;

		public OldAttribute()
		{
			message = "";
		}

		public OldAttribute(string message)
		{
			this.message = message;
		}
	}
}
