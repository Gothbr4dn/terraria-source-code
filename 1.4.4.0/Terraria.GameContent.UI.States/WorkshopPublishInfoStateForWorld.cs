using System.Collections.Generic;
using Terraria.IO;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	public class WorkshopPublishInfoStateForWorld : AWorkshopPublishInfoState<WorldFileData>
	{
		public WorkshopPublishInfoStateForWorld(UIState stateToGoBackTo, WorldFileData world)
			: base(stateToGoBackTo, world)
		{
			_instructionsTextKey = "Workshop.WorldPublishDescription";
			_publishedObjectNameDescriptorTexKey = "Workshop.WorldName";
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
				SocialAPI.Workshop.PublishWorld(_dataObject, GetPublishSettings());
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(_previousUIState);
		}

		protected override List<WorkshopTagOption> GetTagsToShow()
		{
			return SocialAPI.Workshop.SupportedTags.WorldTags;
		}

		protected override bool TryFindingTags(out FoundWorkshopEntryInfo info)
		{
			return SocialAPI.Workshop.TryGetInfoForWorld(_dataObject, out info);
		}
	}
}
