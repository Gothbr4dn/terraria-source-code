using System.Collections.Generic;
using Terraria.IO;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	public class WorkshopPublishInfoStateForResourcePack : AWorkshopPublishInfoState<ResourcePack>
	{
		public WorkshopPublishInfoStateForResourcePack(UIState stateToGoBackTo, ResourcePack resourcePack)
			: base(stateToGoBackTo, resourcePack)
		{
			_instructionsTextKey = "Workshop.ResourcePackPublishDescription";
			_publishedObjectNameDescriptorTexKey = "Workshop.ResourcePackName";
		}

		protected override string GetPublishedObjectDisplayName()
		{
			if (_dataObject == null)
			{
				return "null";
			}
			return _dataObject.Name;
		}

		protected override void GoToPublishConfirmation()
		{
			if (SocialAPI.Workshop != null && _dataObject != null)
			{
				SocialAPI.Workshop.PublishResourcePack(_dataObject, GetPublishSettings());
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(_previousUIState);
		}

		protected override List<WorkshopTagOption> GetTagsToShow()
		{
			return SocialAPI.Workshop.SupportedTags.ResourcePackTags;
		}

		protected override bool TryFindingTags(out FoundWorkshopEntryInfo info)
		{
			return SocialAPI.Workshop.TryGetInfoForResourcePack(_dataObject, out info);
		}
	}
}
