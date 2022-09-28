using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.IO
{
	public class ResourcePackList
	{
		private struct ResourcePackEntry
		{
			public string FileName;

			public bool Enabled;

			public int SortingOrder;

			public ResourcePackEntry(string name, bool enabled, int sortingOrder)
			{
				FileName = name;
				Enabled = enabled;
				SortingOrder = sortingOrder;
			}
		}

		private readonly List<ResourcePack> _resourcePacks = new List<ResourcePack>();

		public IEnumerable<ResourcePack> EnabledPacks => from pack in _resourcePacks
			where pack.IsEnabled
			orderby pack.SortingOrder, pack.Name, pack.Version, pack.FileName
			select pack;

		public IEnumerable<ResourcePack> DisabledPacks => from pack in _resourcePacks
			where !pack.IsEnabled
			orderby pack.Name, pack.Version, pack.FileName
			select pack;

		public IEnumerable<ResourcePack> AllPacks => from pack in _resourcePacks
			orderby pack.Name, pack.Version, pack.FileName
			select pack;

		public ResourcePackList()
		{
		}

		public ResourcePackList(IEnumerable<ResourcePack> resourcePacks)
		{
			_resourcePacks.AddRange(resourcePacks);
		}

		public JArray ToJson()
		{
			List<ResourcePackEntry> list = new List<ResourcePackEntry>(_resourcePacks.Count);
			list.AddRange(_resourcePacks.Select((ResourcePack pack) => new ResourcePackEntry(pack.FileName, pack.IsEnabled, pack.SortingOrder)));
			return JArray.FromObject((object)list);
		}

		public static ResourcePackList FromJson(JArray serializedState, IServiceProvider services, string searchPath)
		{
			if (!Directory.Exists(searchPath))
			{
				return new ResourcePackList();
			}
			List<ResourcePack> resourcePacks = new List<ResourcePack>();
			CreatePacksFromSavedJson(serializedState, services, searchPath, resourcePacks);
			CreatePacksFromZips(services, searchPath, resourcePacks);
			CreatePacksFromDirectories(services, searchPath, resourcePacks);
			CreatePacksFromWorkshopFolders(services, resourcePacks);
			return new ResourcePackList(resourcePacks);
		}

		public static ResourcePackList Publishable(JArray serializedState, IServiceProvider services, string searchPath)
		{
			if (!Directory.Exists(searchPath))
			{
				return new ResourcePackList();
			}
			List<ResourcePack> resourcePacks = new List<ResourcePack>();
			CreatePacksFromZips(services, searchPath, resourcePacks);
			CreatePacksFromDirectories(services, searchPath, resourcePacks);
			return new ResourcePackList(resourcePacks);
		}

		private static void CreatePacksFromSavedJson(JArray serializedState, IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			foreach (ResourcePackEntry item2 in CreatePackEntryListFromJson(serializedState))
			{
				if (item2.FileName == null)
				{
					continue;
				}
				string text = Path.Combine(searchPath, item2.FileName);
				try
				{
					bool flag = File.Exists(text) || Directory.Exists(text);
					ResourcePack.BrandingType branding = ResourcePack.BrandingType.None;
					if (!flag && SocialAPI.Workshop != null && SocialAPI.Workshop.TryGetPath(item2.FileName, out var fullPathFound))
					{
						text = fullPathFound;
						flag = true;
						branding = SocialAPI.Workshop.Branding.ResourcePackBrand;
					}
					if (flag)
					{
						ResourcePack item = new ResourcePack(services, text, branding)
						{
							IsEnabled = item2.Enabled,
							SortingOrder = item2.SortingOrder
						};
						resourcePacks.Add(item);
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		private static void CreatePacksFromDirectories(IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			string[] directories = Directory.GetDirectories(searchPath);
			string folderName;
			foreach (string text in directories)
			{
				try
				{
					folderName = Path.GetFileName(text);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != folderName))
					{
						resourcePacks.Add(new ResourcePack(services, text));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		private static void CreatePacksFromZips(IServiceProvider services, string searchPath, List<ResourcePack> resourcePacks)
		{
			string[] files = Directory.GetFiles(searchPath, "*.zip");
			string fileName;
			foreach (string text in files)
			{
				try
				{
					fileName = Path.GetFileName(text);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != fileName))
					{
						resourcePacks.Add(new ResourcePack(services, text));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", text, arg);
				}
			}
		}

		private static void CreatePacksFromWorkshopFolders(IServiceProvider services, List<ResourcePack> resourcePacks)
		{
			WorkshopSocialModule workshop = SocialAPI.Workshop;
			if (workshop == null)
			{
				return;
			}
			List<string> listOfSubscribedResourcePackPaths = workshop.GetListOfSubscribedResourcePackPaths();
			ResourcePack.BrandingType resourcePackBrand = workshop.Branding.ResourcePackBrand;
			string folderName;
			foreach (string item in listOfSubscribedResourcePackPaths)
			{
				try
				{
					folderName = Path.GetFileName(item);
					if (resourcePacks.All((ResourcePack pack) => pack.FileName != folderName))
					{
						resourcePacks.Add(new ResourcePack(services, item, resourcePackBrand));
					}
				}
				catch (Exception arg)
				{
					Console.WriteLine("Failed to read resource pack {0}: {1}", item, arg);
				}
			}
		}

		private static IEnumerable<ResourcePackEntry> CreatePackEntryListFromJson(JArray serializedState)
		{
			//IL_0014: Expected O, but got Unknown
			try
			{
				if (((JContainer)serializedState).get_Count() != 0)
				{
					return ((JToken)serializedState).ToObject<List<ResourcePackEntry>>();
				}
			}
			catch (JsonReaderException val)
			{
				JsonReaderException arg = val;
				Console.WriteLine("Failed to parse configuration entry for resource pack list. {0}", arg);
			}
			return new List<ResourcePackEntry>();
		}
	}
}
