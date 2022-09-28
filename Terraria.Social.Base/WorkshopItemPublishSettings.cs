using System.Linq;

namespace Terraria.Social.Base
{
	public class WorkshopItemPublishSettings
	{
		public WorkshopTagOption[] UsedTags = new WorkshopTagOption[0];

		public WorkshopItemPublicSettingId Publicity;

		public string PreviewImagePath;

		public string[] GetUsedTagsInternalNames()
		{
			return UsedTags.Select((WorkshopTagOption x) => x.InternalNameForAPIs).ToArray();
		}
	}
}
