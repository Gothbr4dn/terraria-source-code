using System;
using System.Collections.Generic;
using System.IO;
using Steamworks;
using Terraria.IO;
using Terraria.Social.Base;
using Terraria.Utilities;

namespace Terraria.Social.Steam
{
	public class WorkshopHelper
	{
		public class UGCBased
		{
			public struct SteamWorkshopItem
			{
				public string ContentFolderPath;

				public string Description;

				public string PreviewImagePath;

				public string[] Tags;

				public string Title;

				public ERemoteStoragePublishedFileVisibility? Visibility;
			}

			public class Downloader
			{
				public List<string> ResourcePackPaths { get; private set; }

				public List<string> WorldPaths { get; private set; }

				public Downloader()
				{
					ResourcePackPaths = new List<string>();
					WorldPaths = new List<string>();
				}

				public static Downloader Create()
				{
					return new Downloader();
				}

				public List<string> GetListOfSubscribedItemsPaths()
				{
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					PublishedFileId_t[] array = new PublishedFileId_t[SteamUGC.GetNumSubscribedItems()];
					SteamUGC.GetSubscribedItems((PublishedFileId_t[])(object)array, (uint)array.Length);
					ulong num = 0uL;
					string empty = string.Empty;
					uint num2 = 0u;
					List<string> list = new List<string>();
					PublishedFileId_t[] array2 = (PublishedFileId_t[])(object)array;
					for (int i = 0; i < array2.Length; i++)
					{
						if (SteamUGC.GetItemInstallInfo(array2[i], ref num, ref empty, 1024u, ref num2))
						{
							list.Add(empty);
						}
					}
					return list;
				}

				public bool Prepare(WorkshopIssueReporter issueReporter)
				{
					return Refresh(issueReporter);
				}

				public bool Refresh(WorkshopIssueReporter issueReporter)
				{
					ResourcePackPaths.Clear();
					WorldPaths.Clear();
					foreach (string listOfSubscribedItemsPath in GetListOfSubscribedItemsPaths())
					{
						if (listOfSubscribedItemsPath == null)
						{
							continue;
						}
						try
						{
							char directorySeparatorChar = Path.DirectorySeparatorChar;
							string path = listOfSubscribedItemsPath + directorySeparatorChar + "workshop.json";
							if (!File.Exists(path))
							{
								continue;
							}
							string text = AWorkshopEntry.ReadHeader(File.ReadAllText(path));
							if (!(text == "World"))
							{
								if (text == "ResourcePack")
								{
									ResourcePackPaths.Add(listOfSubscribedItemsPath);
								}
							}
							else
							{
								WorldPaths.Add(listOfSubscribedItemsPath);
							}
						}
						catch (Exception exception)
						{
							issueReporter.ReportDownloadProblem("Workshop.ReportIssue_FailedToLoadSubscribedFile", listOfSubscribedItemsPath, exception);
							return false;
						}
					}
					return true;
				}
			}

			public class PublishedItemsFinder
			{
				private Dictionary<ulong, SteamWorkshopItem> _items = new Dictionary<ulong, SteamWorkshopItem>();

				private UGCQueryHandle_t m_UGCQueryHandle;

				private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;

				private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;

				public bool HasItemOfId(ulong id)
				{
					return _items.ContainsKey(id);
				}

				public static PublishedItemsFinder Create()
				{
					PublishedItemsFinder publishedItemsFinder = new PublishedItemsFinder();
					publishedItemsFinder.LoadHooks();
					return publishedItemsFinder;
				}

				private void LoadHooks()
				{
					OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create((APIDispatchDelegate<SteamUGCQueryCompleted_t>)OnSteamUGCQueryCompleted);
					OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create((APIDispatchDelegate<SteamUGCRequestUGCDetailsResult_t>)OnSteamUGCRequestUGCDetailsResult);
				}

				public void Prepare()
				{
					Refresh();
				}

				public void Refresh()
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					CSteamID steamID = SteamUser.GetSteamID();
					m_UGCQueryHandle = SteamUGC.CreateQueryUserUGCRequest(((CSteamID)(ref steamID)).GetAccountID(), (EUserUGCList)0, (EUGCMatchingUGCType)(-1), (EUserUGCListSortOrder)0, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1u);
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
					SteamAPICall_t val = SteamUGC.SendQueryUGCRequest(m_UGCQueryHandle);
					OnSteamUGCQueryCompletedCallResult.Set(val, (APIDispatchDelegate<SteamUGCQueryCompleted_t>)OnSteamUGCQueryCompleted);
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
				}

				private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
				{
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Invalid comparison between Unknown and I4
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0040: Unknown result type (might be due to invalid IL or missing references)
					//IL_0084: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					_items.Clear();
					if (bIOFailure || (int)pCallback.m_eResult != 1)
					{
						SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
						return;
					}
					SteamUGCDetails_t val = default(SteamUGCDetails_t);
					for (uint num = 0u; num < pCallback.m_unNumResultsReturned; num++)
					{
						SteamUGC.GetQueryUGCResult(m_UGCQueryHandle, num, ref val);
						ulong publishedFileId = val.m_nPublishedFileId.m_PublishedFileId;
						SteamWorkshopItem steamWorkshopItem = default(SteamWorkshopItem);
						steamWorkshopItem.Title = ((SteamUGCDetails_t)(ref val)).get_m_rgchTitle();
						steamWorkshopItem.Description = ((SteamUGCDetails_t)(ref val)).get_m_rgchDescription();
						SteamWorkshopItem value = steamWorkshopItem;
						_items.Add(publishedFileId, value);
					}
					SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
				}

				private void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure)
				{
				}
			}

			public abstract class APublisherInstance
			{
				public delegate void FinishedPublishingAction(APublisherInstance instance);

				protected WorkshopItemPublicSettingId _publicity;

				protected SteamWorkshopItem _entryData;

				protected PublishedFileId_t _publishedFileID;

				private UGCUpdateHandle_t _updateHandle;

				private CallResult<CreateItemResult_t> _createItemHook;

				private CallResult<SubmitItemUpdateResult_t> _updateItemHook;

				private FinishedPublishingAction _endAction;

				private WorkshopIssueReporter _issueReporter;

				public void PublishContent(PublishedItemsFinder finder, WorkshopIssueReporter issueReporter, FinishedPublishingAction endAction, string itemTitle, string itemDescription, string contentFolderPath, string previewImagePath, WorkshopItemPublicSettingId publicity, string[] tags)
				{
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0044: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
					_issueReporter = issueReporter;
					_endAction = endAction;
					_createItemHook = CallResult<CreateItemResult_t>.Create((APIDispatchDelegate<CreateItemResult_t>)CreateItemResult);
					_updateItemHook = CallResult<SubmitItemUpdateResult_t>.Create((APIDispatchDelegate<SubmitItemUpdateResult_t>)UpdateItemResult);
					ERemoteStoragePublishedFileVisibility visibility = GetVisibility(publicity);
					_entryData = new SteamWorkshopItem
					{
						Title = itemTitle,
						Description = itemDescription,
						ContentFolderPath = contentFolderPath,
						Tags = tags,
						PreviewImagePath = previewImagePath,
						Visibility = visibility
					};
					ulong? num = null;
					char directorySeparatorChar = Path.DirectorySeparatorChar;
					if (AWorkshopEntry.TryReadingManifest(contentFolderPath + directorySeparatorChar + "workshop.json", out var info))
					{
						num = info.workshopEntryId;
					}
					if (num.HasValue && finder.HasItemOfId(num.Value))
					{
						_publishedFileID = new PublishedFileId_t(num.Value);
						PreventUpdatingCertainThings();
						UpdateItem();
					}
					else
					{
						CreateItem();
					}
				}

				private void PreventUpdatingCertainThings()
				{
					_entryData.Title = null;
					_entryData.Description = null;
				}

				private ERemoteStoragePublishedFileVisibility GetVisibility(WorkshopItemPublicSettingId publicityId)
				{
					return (ERemoteStoragePublishedFileVisibility)(publicityId switch
					{
						WorkshopItemPublicSettingId.FriendsOnly => 1, 
						WorkshopItemPublicSettingId.Public => 0, 
						_ => 2, 
					});
				}

				private void CreateItem()
				{
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
					SteamAPICall_t val = SteamUGC.CreateItem(SteamUtils.GetAppID(), (EWorkshopFileType)0);
					_createItemHook.Set(val, (APIDispatchDelegate<CreateItemResult_t>)CreateItemResult);
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
				}

				private void CreateItemResult(CreateItemResult_t param, bool bIOFailure)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					//IL_002c: Invalid comparison between Unknown and I4
					//IL_002f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0030: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
					{
						_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
						_endAction(this);
					}
					else if ((int)param.m_eResult == 1)
					{
						_publishedFileID = param.m_nPublishedFileId;
						UpdateItem();
					}
					else
					{
						_issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", ((object)(EResult)(ref param.m_eResult)).ToString());
						_endAction(this);
					}
				}

				protected abstract string GetHeaderText();

				protected abstract void PrepareContentForUpdate();

				private void UpdateItem()
				{
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					//IL_0039: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00de: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0108: Unknown result type (might be due to invalid IL or missing references)
					string headerText = GetHeaderText();
					if (!TryWritingManifestToFolder(_entryData.ContentFolderPath, headerText))
					{
						_endAction(this);
						return;
					}
					PrepareContentForUpdate();
					UGCUpdateHandle_t val = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), _publishedFileID);
					if (_entryData.Title != null)
					{
						SteamUGC.SetItemTitle(val, _entryData.Title);
					}
					if (_entryData.Description != null)
					{
						SteamUGC.SetItemDescription(val, _entryData.Description);
					}
					SteamUGC.SetItemContent(val, _entryData.ContentFolderPath);
					SteamUGC.SetItemTags(val, (IList<string>)_entryData.Tags);
					if (_entryData.PreviewImagePath != null)
					{
						SteamUGC.SetItemPreview(val, _entryData.PreviewImagePath);
					}
					if (_entryData.Visibility.HasValue)
					{
						SteamUGC.SetItemVisibility(val, _entryData.Visibility.Value);
					}
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
					SteamAPICall_t val2 = SteamUGC.SubmitItemUpdate(val, "");
					_updateHandle = val;
					_updateItemHook.Set(val2, (APIDispatchDelegate<SubmitItemUpdateResult_t>)UpdateItemResult);
					CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
				}

				private void UpdateItemResult(SubmitItemUpdateResult_t param, bool bIOFailure)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					//IL_002c: Unknown result type (might be due to invalid IL or missing references)
					//IL_002f: Invalid comparison between Unknown and I4
					//IL_0031: Unknown result type (might be due to invalid IL or missing references)
					//IL_0033: Invalid comparison between Unknown and I4
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Invalid comparison between Unknown and I4
					//IL_0039: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Invalid comparison between Unknown and I4
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Invalid comparison between Unknown and I4
					//IL_0048: Unknown result type (might be due to invalid IL or missing references)
					//IL_004b: Invalid comparison between Unknown and I4
					//IL_0050: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Invalid comparison between Unknown and I4
					if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
					{
						_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
						_endAction(this);
						return;
					}
					EResult eResult = param.m_eResult;
					if ((int)eResult <= 9)
					{
						if ((int)eResult != 1)
						{
							if ((int)eResult != 8)
							{
								if ((int)eResult != 9)
								{
									goto IL_0079;
								}
								_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_CouldNotFindFolderToUpload");
							}
							else
							{
								_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_InvalidParametersForPublishing");
							}
						}
						else
						{
							SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + _publishedFileID.m_PublishedFileId, (EActivateGameOverlayToWebPageMode)0);
						}
					}
					else if ((int)eResult != 15)
					{
						if ((int)eResult != 25)
						{
							if ((int)eResult != 33)
							{
								goto IL_0079;
							}
							_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_SteamFileLockFailed");
						}
						else
						{
							_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_LimitExceeded");
						}
					}
					else
					{
						_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_AccessDeniedBecauseUserDoesntOwnLicenseForApp");
					}
					goto IL_00f5;
					IL_00f5:
					_endAction(this);
					return;
					IL_0079:
					_issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", ((object)(EResult)(ref param.m_eResult)).ToString());
					goto IL_00f5;
				}

				private bool TryWritingManifestToFolder(string folderPath, string manifestText)
				{
					char directorySeparatorChar = Path.DirectorySeparatorChar;
					string path = folderPath + directorySeparatorChar + "workshop.json";
					bool result = true;
					try
					{
						File.WriteAllText(path, manifestText);
						return result;
					}
					catch (Exception exception)
					{
						_issueReporter.ReportManifestCreationProblem("Workshop.ReportIssue_CouldNotCreateResourcePackManifestFile", exception);
						return false;
					}
				}

				public bool TryGetProgress(out float progress)
				{
					//IL_0008: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					progress = 0f;
					if (_updateHandle == default(UGCUpdateHandle_t))
					{
						return false;
					}
					ulong num = default(ulong);
					ulong num2 = default(ulong);
					SteamUGC.GetItemUpdateProgress(_updateHandle, ref num, ref num2);
					if (num2 == 0L)
					{
						return false;
					}
					progress = (float)((double)num / (double)num2);
					return true;
				}
			}

			public class ResourcePackPublisherInstance : APublisherInstance
			{
				private ResourcePack _resourcePack;

				public ResourcePackPublisherInstance(ResourcePack resourcePack)
				{
					_resourcePack = resourcePack;
				}

				protected override string GetHeaderText()
				{
					return TexturePackWorkshopEntry.GetHeaderTextFor(_resourcePack, _publishedFileID.m_PublishedFileId, _entryData.Tags, _publicity, _entryData.PreviewImagePath);
				}

				protected override void PrepareContentForUpdate()
				{
				}
			}

			public class WorldPublisherInstance : APublisherInstance
			{
				private WorldFileData _world;

				public WorldPublisherInstance(WorldFileData world)
				{
					_world = world;
				}

				protected override string GetHeaderText()
				{
					return WorldWorkshopEntry.GetHeaderTextFor(_world, _publishedFileID.m_PublishedFileId, _entryData.Tags, _publicity, _entryData.PreviewImagePath);
				}

				protected override void PrepareContentForUpdate()
				{
					if (_world.IsCloudSave)
					{
						string path = _world.Path;
						string contentFolderPath = _entryData.ContentFolderPath;
						char directorySeparatorChar = Path.DirectorySeparatorChar;
						FileUtilities.CopyToLocal(path, contentFolderPath + directorySeparatorChar + "world.wld");
					}
					else
					{
						string path2 = _world.Path;
						string contentFolderPath2 = _entryData.ContentFolderPath;
						char directorySeparatorChar = Path.DirectorySeparatorChar;
						FileUtilities.Copy(path2, contentFolderPath2 + directorySeparatorChar + "world.wld", cloud: false);
					}
				}
			}

			public const string ManifestFileName = "workshop.json";
		}
	}
}
