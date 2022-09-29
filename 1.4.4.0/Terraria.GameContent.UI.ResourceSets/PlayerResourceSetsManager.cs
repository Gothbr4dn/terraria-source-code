using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria.IO;

namespace Terraria.GameContent.UI.ResourceSets
{
	public class PlayerResourceSetsManager
	{
		private Dictionary<string, IPlayerResourcesDisplaySet> _sets = new Dictionary<string, IPlayerResourcesDisplaySet>();

		private IPlayerResourcesDisplaySet _activeSet;

		private string _activeSetConfigKey;

		private bool _loadedContent;

		public string ActiveSetKeyName { get; private set; }

		public void Initialize()
		{
			Main.Configuration.OnLoad += Configuration_OnLoad;
			Main.Configuration.OnSave += Configuration_OnSave;
		}

		private void Configuration_OnLoad(Preferences obj)
		{
			_activeSetConfigKey = Main.Configuration.Get("PlayerResourcesSet", "New");
			if (_loadedContent)
			{
				SetActiveFromLoadedConfigKey();
			}
		}

		private void Configuration_OnSave(Preferences obj)
		{
			_ = _sets.FirstOrDefault((KeyValuePair<string, IPlayerResourcesDisplaySet> pair) => pair.Value == _activeSet).Key;
			obj.Put("PlayerResourcesSet", _activeSetConfigKey);
		}

		public void LoadContent(AssetRequestMode mode)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			_sets["New"] = new FancyClassicPlayerResourcesDisplaySet("New", "New", "FancyClassic", mode);
			_sets["Default"] = new ClassicPlayerResourcesDisplaySet("Default", "Default");
			_sets["HorizontalBarsWithFullText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithFullText", "HorizontalBarsWithFullText", "HorizontalBars", mode);
			_sets["HorizontalBarsWithText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithText", "HorizontalBarsWithText", "HorizontalBars", mode);
			_sets["HorizontalBars"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBars", "HorizontalBars", "HorizontalBars", mode);
			_sets["NewWithText"] = new FancyClassicPlayerResourcesDisplaySet("NewWithText", "NewWithText", "FancyClassic", mode);
			_loadedContent = true;
			SetActiveFromLoadedConfigKey();
		}

		public void SetActiveFromLoadedConfigKey()
		{
			SetActive(_activeSetConfigKey);
		}

		private void SetActive(string name)
		{
			IPlayerResourcesDisplaySet playerResourcesDisplaySet = _sets.FirstOrDefault((KeyValuePair<string, IPlayerResourcesDisplaySet> pair) => pair.Key == name).Value;
			if (playerResourcesDisplaySet == null)
			{
				playerResourcesDisplaySet = _sets.Values.First();
			}
			SetActiveFrame(playerResourcesDisplaySet);
		}

		private void SetActiveFrame(IPlayerResourcesDisplaySet set)
		{
			_activeSet = set;
			_activeSetConfigKey = set.ConfigKey;
			ActiveSetKeyName = set.NameKey;
		}

		public void TryToHoverOverResources()
		{
			_activeSet.TryToHover();
		}

		public void Draw()
		{
			_activeSet.Draw();
		}

		public void CycleResourceSet()
		{
			IPlayerResourcesDisplaySet lastFrame = null;
			_sets.Values.FirstOrDefault(delegate(IPlayerResourcesDisplaySet frame)
			{
				if (frame == _activeSet)
				{
					return true;
				}
				lastFrame = frame;
				return false;
			});
			if (lastFrame == null)
			{
				lastFrame = _sets.Values.Last();
			}
			SetActiveFrame(lastFrame);
		}
	}
}
