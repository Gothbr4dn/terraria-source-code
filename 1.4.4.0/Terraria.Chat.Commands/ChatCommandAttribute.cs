using System;

namespace Terraria.Chat.Commands
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ChatCommandAttribute : Attribute
	{
		public readonly string Name;

		public ChatCommandAttribute(string name)
		{
			Name = name;
		}
	}
}
