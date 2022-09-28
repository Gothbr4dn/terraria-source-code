using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria.IO;

namespace Terraria.DataStructures
{
	public abstract class SelectionHolder<TCycleType> where TCycleType : class, IConfigKeyHolder
	{
		protected Dictionary<string, TCycleType> Options = new Dictionary<string, TCycleType>();

		protected TCycleType ActiveSelection;

		protected string ActiveSelectionConfigKey;

		protected bool LoadedContent;

		public string ActiveSelectionKeyName { get; private set; }

		public void Initialize()
		{
			Main.Configuration.OnLoad += Wrapped_Configuration_OnLoad;
			Main.Configuration.OnSave += Configuration_Save;
		}

		protected abstract void Configuration_Save(Preferences obj);

		protected abstract void Configuration_OnLoad(Preferences obj);

		protected void Wrapped_Configuration_OnLoad(Preferences obj)
		{
			Configuration_OnLoad(obj);
			if (LoadedContent)
			{
				SetActiveMinimapFromLoadedConfigKey();
			}
		}

		protected abstract void PopulateOptionsAndLoadContent(AssetRequestMode mode);

		public void LoadContent(AssetRequestMode mode)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			PopulateOptionsAndLoadContent(mode);
			LoadedContent = true;
			SetActiveMinimapFromLoadedConfigKey();
		}

		public void CycleSelection()
		{
			TCycleType lastFrame = null;
			Options.Values.FirstOrDefault(delegate(TCycleType frame)
			{
				if (frame == ActiveSelection)
				{
					return true;
				}
				lastFrame = frame;
				return false;
			});
			if (lastFrame == null)
			{
				lastFrame = Options.Values.Last();
			}
			SetActiveFrame(lastFrame);
		}

		public void SetActiveMinimapFromLoadedConfigKey()
		{
			SetActiveFrame(ActiveSelectionConfigKey);
		}

		private void SetActiveFrame(string frameName)
		{
			TCycleType val = Options.FirstOrDefault((KeyValuePair<string, TCycleType> pair) => pair.Key == frameName).Value;
			if (val == null)
			{
				val = Options.Values.First();
			}
			SetActiveFrame(val);
		}

		private void SetActiveFrame(TCycleType frame)
		{
			ActiveSelection = frame;
			ActiveSelectionConfigKey = frame.ConfigKey;
			ActiveSelectionKeyName = frame.NameKey;
		}
	}
}
