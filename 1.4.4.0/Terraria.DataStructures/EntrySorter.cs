using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	public class EntrySorter<TEntryType, TStepType> : IComparer<TEntryType> where TEntryType : new()where TStepType : IEntrySortStep<TEntryType>
	{
		public List<TStepType> Steps = new List<TStepType>();

		private int _prioritizedStep;

		public void AddSortSteps(List<TStepType> sortSteps)
		{
			Steps.AddRange(sortSteps);
		}

		public int Compare(TEntryType x, TEntryType y)
		{
			int num = 0;
			if (_prioritizedStep != -1)
			{
				num = Steps[_prioritizedStep].Compare(x, y);
				if (num != 0)
				{
					return num;
				}
			}
			for (int i = 0; i < Steps.Count; i++)
			{
				if (i != _prioritizedStep)
				{
					num = Steps[i].Compare(x, y);
					if (num != 0)
					{
						return num;
					}
				}
			}
			return num;
		}

		public void SetPrioritizedStepIndex(int index)
		{
			_prioritizedStep = index;
		}

		public string GetDisplayName()
		{
			return Language.GetTextValue(Steps[_prioritizedStep].GetDisplayNameKey());
		}
	}
}
