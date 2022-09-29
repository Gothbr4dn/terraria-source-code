using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public class MethodSequenceListItem
	{
		public string Name;

		public MethodSequenceListItem Parent;

		public Func<bool> Method;

		public bool Skip;

		public MethodSequenceListItem(string name, Func<bool> method, MethodSequenceListItem parent = null)
		{
			Name = name;
			Method = method;
			Parent = parent;
		}

		public bool ShouldAct(List<MethodSequenceListItem> sequence)
		{
			if (Skip)
			{
				return false;
			}
			if (!sequence.Contains(this))
			{
				return false;
			}
			if (Parent != null)
			{
				return Parent.ShouldAct(sequence);
			}
			return true;
		}

		public bool Act()
		{
			return Method();
		}

		public static void ExecuteSequence(List<MethodSequenceListItem> sequence)
		{
			foreach (MethodSequenceListItem item in sequence)
			{
				if (item.ShouldAct(sequence) && !item.Act())
				{
					break;
				}
			}
		}

		public override string ToString()
		{
			return "name: " + Name + " skip: " + Skip.ToString() + " parent: " + Parent;
		}
	}
}
