using System;
using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using ReLogic.Content.Sources;
using Terraria.Audio;
using Terraria.IO;
using Terraria.Localization;

namespace Terraria.GameContent
{
	public class AssetSourceController
	{
		private readonly List<IContentSource> _staticSources;

		private readonly IAssetRepository _assetRepository;

		public ResourcePackList ActiveResourcePackList { get; private set; }

		public event Action<ResourcePackList> OnResourcePackChange;

		public AssetSourceController(IAssetRepository assetRepository, IEnumerable<IContentSource> staticSources)
		{
			_assetRepository = assetRepository;
			_staticSources = staticSources.ToList();
			UseResourcePacks(new ResourcePackList());
		}

		public void Refresh()
		{
			foreach (ResourcePack allPack in ActiveResourcePackList.AllPacks)
			{
				allPack.Refresh();
			}
			UseResourcePacks(ActiveResourcePackList);
		}

		public void UseResourcePacks(ResourcePackList resourcePacks)
		{
			if (this.OnResourcePackChange != null)
			{
				this.OnResourcePackChange(resourcePacks);
			}
			ActiveResourcePackList = resourcePacks;
			List<IContentSource> list = new List<IContentSource>(from pack in resourcePacks.EnabledPacks
				orderby pack.SortingOrder
				select pack.GetContentSource());
			list.AddRange(_staticSources);
			foreach (IContentSource item in list)
			{
				item.ClearRejections();
			}
			List<IContentSource> list2 = new List<IContentSource>();
			for (int num = list.Count - 1; num >= 0; num--)
			{
				list2.Add(list[num]);
			}
			_assetRepository.SetSources((IEnumerable<IContentSource>)list, (AssetRequestMode)1);
			LanguageManager.Instance.UseSources(list2);
			Main.audioSystem.UseSources(list2);
			SoundEngine.Reload();
			Main.changeTheTitle = true;
		}
	}
}
