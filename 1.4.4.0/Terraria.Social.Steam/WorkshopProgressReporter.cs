using System.Collections.Generic;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	public class WorkshopProgressReporter : AWorkshopProgressReporter
	{
		private List<WorkshopHelper.UGCBased.APublisherInstance> _publisherInstances;

		public override bool HasOngoingTasks => _publisherInstances.Count > 0;

		public WorkshopProgressReporter(List<WorkshopHelper.UGCBased.APublisherInstance> publisherInstances)
		{
			_publisherInstances = publisherInstances;
		}

		public override bool TryGetProgress(out float progress)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < _publisherInstances.Count; i++)
			{
				if (_publisherInstances[i].TryGetProgress(out var progress2))
				{
					num += progress2;
					num2 += 1f;
				}
			}
			progress = 0f;
			if (num2 == 0f)
			{
				return false;
			}
			progress = num / num2;
			return true;
		}
	}
}
