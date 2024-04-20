using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Shell
{
	// Token: 0x020003ED RID: 1005
	[ContentProperty("JumpItems")]
	public sealed class JumpList : ISupportInitialize
	{
		// Token: 0x06002B12 RID: 11026 RVA: 0x001A0E8C File Offset: 0x0019FE8C
		public static void AddToRecentCategory(string itemPath)
		{
			Verify.FileExists(itemPath, "itemPath");
			itemPath = Path.GetFullPath(itemPath);
			NativeMethods2.SHAddToRecentDocs(itemPath);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x001A0EA7 File Offset: 0x0019FEA7
		public static void AddToRecentCategory(JumpPath jumpPath)
		{
			Verify.IsNotNull<JumpPath>(jumpPath, "jumpPath");
			JumpList.AddToRecentCategory(jumpPath.Path);
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x001A0EC0 File Offset: 0x0019FEC0
		public static void AddToRecentCategory(JumpTask jumpTask)
		{
			Verify.IsNotNull<JumpTask>(jumpTask, "jumpTask");
			if (Utilities.IsOSWindows7OrNewer)
			{
				IShellLinkW shellLinkW = JumpList.CreateLinkFromJumpTask(jumpTask, false);
				try
				{
					if (shellLinkW != null)
					{
						NativeMethods2.SHAddToRecentDocs(shellLinkW);
					}
				}
				finally
				{
					Utilities.SafeRelease<IShellLinkW>(ref shellLinkW);
				}
			}
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x001A0F0C File Offset: 0x0019FF0C
		public static void SetJumpList(Application application, JumpList value)
		{
			Verify.IsNotNull<Application>(application, "application");
			object obj = JumpList.s_lock;
			lock (obj)
			{
				JumpList jumpList;
				if (JumpList.s_applicationMap.TryGetValue(application, out jumpList) && jumpList != null)
				{
					jumpList._application = null;
				}
				JumpList.s_applicationMap[application] = value;
				if (value != null)
				{
					value._application = application;
				}
			}
			if (value != null)
			{
				value.ApplyFromApplication();
			}
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x001A0F88 File Offset: 0x0019FF88
		public static JumpList GetJumpList(Application application)
		{
			Verify.IsNotNull<Application>(application, "application");
			JumpList result;
			JumpList.s_applicationMap.TryGetValue(application, out result);
			return result;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x001A0FAF File Offset: 0x0019FFAF
		public JumpList() : this(null, false, false)
		{
			this._initializing = null;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x001A0FC6 File Offset: 0x0019FFC6
		public JumpList(IEnumerable<JumpItem> items, bool showFrequent, bool showRecent)
		{
			if (items != null)
			{
				this._jumpItems = new List<JumpItem>(items);
			}
			else
			{
				this._jumpItems = new List<JumpItem>();
			}
			this.ShowFrequentCategory = showFrequent;
			this.ShowRecentCategory = showRecent;
			this._initializing = new bool?(false);
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06002B19 RID: 11033 RVA: 0x001A1004 File Offset: 0x001A0004
		// (set) Token: 0x06002B1A RID: 11034 RVA: 0x001A100C File Offset: 0x001A000C
		public bool ShowFrequentCategory { get; set; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x001A1015 File Offset: 0x001A0015
		// (set) Token: 0x06002B1C RID: 11036 RVA: 0x001A101D File Offset: 0x001A001D
		public bool ShowRecentCategory { get; set; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x001A1026 File Offset: 0x001A0026
		public List<JumpItem> JumpItems
		{
			get
			{
				return this._jumpItems;
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x001A102E File Offset: 0x001A002E
		private bool IsUnmodified
		{
			get
			{
				return this._initializing == null && this.JumpItems.Count == 0 && !this.ShowRecentCategory && !this.ShowFrequentCategory;
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x001A105D File Offset: 0x001A005D
		public void BeginInit()
		{
			if (!this.IsUnmodified)
			{
				throw new InvalidOperationException(SR.Get("JumpList_CantNestBeginInitCalls"));
			}
			this._initializing = new bool?(true);
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x001A1084 File Offset: 0x001A0084
		public void EndInit()
		{
			bool? initializing = this._initializing;
			bool flag = true;
			if (!(initializing.GetValueOrDefault() == flag & initializing != null))
			{
				throw new NotSupportedException(SR.Get("JumpList_CantCallUnbalancedEndInit"));
			}
			this._initializing = new bool?(false);
			this.ApplyFromApplication();
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06002B21 RID: 11041 RVA: 0x001A10D0 File Offset: 0x001A00D0
		private static string _RuntimeId
		{
			get
			{
				string result;
				HRESULT hrLeft = NativeMethods2.GetCurrentProcessExplicitAppUserModelID(out result);
				if (hrLeft == HRESULT.E_FAIL)
				{
					hrLeft = HRESULT.S_OK;
					result = null;
				}
				hrLeft.ThrowIfFailed();
				return result;
			}
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x001A1104 File Offset: 0x001A0104
		public void Apply()
		{
			bool? initializing = this._initializing;
			bool flag = true;
			if (initializing.GetValueOrDefault() == flag & initializing != null)
			{
				throw new InvalidOperationException(SR.Get("JumpList_CantApplyUntilEndInit"));
			}
			this._initializing = new bool?(false);
			this.ApplyList();
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x001A1150 File Offset: 0x001A0150
		private void ApplyFromApplication()
		{
			bool? initializing = this._initializing;
			bool flag = true;
			if (!(initializing.GetValueOrDefault() == flag & initializing != null) && !this.IsUnmodified)
			{
				this._initializing = new bool?(false);
			}
			if (this._application == Application.Current)
			{
				initializing = this._initializing;
				flag = false;
				if (initializing.GetValueOrDefault() == flag & initializing != null)
				{
					this.ApplyList();
				}
			}
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x001A11C0 File Offset: 0x001A01C0
		private void ApplyList()
		{
			Verify.IsApartmentState(ApartmentState.STA);
			if (!Utilities.IsOSWindows7OrNewer)
			{
				this.RejectEverything();
				return;
			}
			List<List<JumpList._ShellObjectPair>> list = null;
			List<JumpList._ShellObjectPair> list2 = null;
			ICustomDestinationList customDestinationList = (ICustomDestinationList)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("77f10cf0-3db5-4966-b520-b7c54fd35ed6")));
			List<JumpItem> list3;
			List<JumpList._RejectedJumpItemPair> list4;
			try
			{
				string runtimeId = JumpList._RuntimeId;
				if (!string.IsNullOrEmpty(runtimeId))
				{
					customDestinationList.SetAppID(runtimeId);
				}
				Guid guid = new Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9");
				uint num;
				list2 = JumpList.GenerateJumpItems((IObjectArray)customDestinationList.BeginList(out num, ref guid));
				list3 = new List<JumpItem>(this.JumpItems.Count);
				list4 = new List<JumpList._RejectedJumpItemPair>(this.JumpItems.Count);
				list = new List<List<JumpList._ShellObjectPair>>
				{
					new List<JumpList._ShellObjectPair>()
				};
				foreach (JumpItem jumpItem in this.JumpItems)
				{
					if (jumpItem == null)
					{
						list4.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = jumpItem,
							Reason = JumpItemRejectionReason.InvalidItem
						});
					}
					else
					{
						object obj = null;
						try
						{
							obj = JumpList.GetShellObjectForJumpItem(jumpItem);
							if (obj == null)
							{
								list4.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.InvalidItem,
									JumpItem = jumpItem
								});
							}
							else if (JumpList.ListContainsShellObject(list2, obj))
							{
								list4.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.RemovedByUser,
									JumpItem = jumpItem
								});
							}
							else
							{
								JumpList._ShellObjectPair item = new JumpList._ShellObjectPair
								{
									JumpItem = jumpItem,
									ShellObject = obj
								};
								if (string.IsNullOrEmpty(jumpItem.CustomCategory))
								{
									list[0].Add(item);
								}
								else
								{
									bool flag = false;
									foreach (List<JumpList._ShellObjectPair> list5 in list)
									{
										if (list5.Count > 0 && list5[0].JumpItem.CustomCategory == jumpItem.CustomCategory)
										{
											list5.Add(item);
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										list.Add(new List<JumpList._ShellObjectPair>
										{
											item
										});
									}
								}
								obj = null;
							}
						}
						finally
						{
							Utilities.SafeRelease<object>(ref obj);
						}
					}
				}
				list.Reverse();
				if (this.ShowFrequentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.FREQUENT);
				}
				if (this.ShowRecentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.RECENT);
				}
				foreach (List<JumpList._ShellObjectPair> list6 in list)
				{
					if (list6.Count > 0)
					{
						string customCategory = list6[0].JumpItem.CustomCategory;
						JumpList.AddCategory(customDestinationList, customCategory, list6, list3, list4);
					}
				}
				customDestinationList.CommitList();
			}
			catch
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.RejectingJumpItemsBecauseCatastrophicFailure);
				}
				this.RejectEverything();
				return;
			}
			finally
			{
				Utilities.SafeRelease<ICustomDestinationList>(ref customDestinationList);
				if (list != null)
				{
					foreach (List<JumpList._ShellObjectPair> list7 in list)
					{
						JumpList._ShellObjectPair.ReleaseShellObjects(list7);
					}
				}
				JumpList._ShellObjectPair.ReleaseShellObjects(list2);
			}
			list3.Reverse();
			this._jumpItems = list3;
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			EventHandler<JumpItemsRemovedEventArgs> jumpItemsRemovedByUser = this.JumpItemsRemovedByUser;
			if (list4.Count > 0 && jumpItemsRejected != null)
			{
				List<JumpItem> list8 = new List<JumpItem>(list4.Count);
				List<JumpItemRejectionReason> list9 = new List<JumpItemRejectionReason>(list4.Count);
				foreach (JumpList._RejectedJumpItemPair rejectedJumpItemPair in list4)
				{
					list8.Add(rejectedJumpItemPair.JumpItem);
					list9.Add(rejectedJumpItemPair.Reason);
				}
				jumpItemsRejected(this, new JumpItemsRejectedEventArgs(list8, list9));
			}
			if (list2.Count > 0 && jumpItemsRemovedByUser != null)
			{
				List<JumpItem> list10 = new List<JumpItem>(list2.Count);
				foreach (JumpList._ShellObjectPair shellObjectPair in list2)
				{
					if (shellObjectPair.JumpItem != null)
					{
						list10.Add(shellObjectPair.JumpItem);
					}
				}
				if (list10.Count > 0)
				{
					jumpItemsRemovedByUser(this, new JumpItemsRemovedEventArgs(list10));
				}
			}
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x001A16B4 File Offset: 0x001A06B4
		private static bool ListContainsShellObject(List<JumpList._ShellObjectPair> removedList, object shellObject)
		{
			if (removedList.Count == 0)
			{
				return false;
			}
			IShellItem shellItem = shellObject as IShellItem;
			if (shellItem != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair in removedList)
				{
					IShellItem shellItem2 = shellObjectPair.ShellObject as IShellItem;
					if (shellItem2 != null && shellItem.Compare(shellItem2, SICHINT.CANONICAL | SICHINT.TEST_FILESYSPATH_IF_NOT_EQUAL) == 0)
					{
						return true;
					}
				}
				return false;
			}
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellLinkW != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair2 in removedList)
				{
					IShellLinkW shellLinkW2 = shellObjectPair2.ShellObject as IShellLinkW;
					if (shellLinkW2 != null)
					{
						string a = JumpList.ShellLinkToString(shellLinkW2);
						string b = JumpList.ShellLinkToString(shellLinkW);
						if (a == b)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x001A17A4 File Offset: 0x001A07A4
		private static object GetShellObjectForJumpItem(JumpItem jumpItem)
		{
			JumpPath jumpPath = jumpItem as JumpPath;
			JumpTask jumpTask = jumpItem as JumpTask;
			if (jumpPath != null)
			{
				return JumpList.CreateItemFromJumpPath(jumpPath);
			}
			if (jumpTask != null)
			{
				return JumpList.CreateLinkFromJumpTask(jumpTask, true);
			}
			return null;
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x001A17D8 File Offset: 0x001A07D8
		private static List<JumpList._ShellObjectPair> GenerateJumpItems(IObjectArray shellObjects)
		{
			List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
			Guid guid = new Guid("00000000-0000-0000-C000-000000000046");
			uint count = shellObjects.GetCount();
			for (uint num = 0U; num < count; num += 1U)
			{
				object at = shellObjects.GetAt(num, ref guid);
				JumpItem jumpItem = null;
				try
				{
					jumpItem = JumpList.GetJumpItemForShellObject(at);
				}
				catch (Exception ex)
				{
					if (ex is NullReferenceException || ex is SEHException)
					{
						throw;
					}
				}
				list.Add(new JumpList._ShellObjectPair
				{
					ShellObject = at,
					JumpItem = jumpItem
				});
			}
			return list;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x001A1868 File Offset: 0x001A0868
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList)
		{
			JumpList.AddCategory(cdl, category, jumpItems, successList, rejectionList, true);
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x001A1878 File Offset: 0x001A0878
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList, bool isHeterogenous)
		{
			IObjectCollection objectCollection = (IObjectCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("2d3468c1-36a7-43b6-ac24-d3f02fd9607a")));
			foreach (JumpList._ShellObjectPair shellObjectPair in jumpItems)
			{
				objectCollection.AddObject(shellObjectPair.ShellObject);
			}
			HRESULT hrLeft;
			if (string.IsNullOrEmpty(category))
			{
				hrLeft = cdl.AddUserTasks(objectCollection);
			}
			else
			{
				hrLeft = cdl.AppendCategory(category, objectCollection);
			}
			if (hrLeft.Succeeded)
			{
				int num = jumpItems.Count;
				while (--num >= 0)
				{
					successList.Add(jumpItems[num].JumpItem);
				}
				return;
			}
			if (isHeterogenous && hrLeft == HRESULT.DESTS_E_NO_MATCHING_ASSOC_HANDLER)
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.RejectingJumpListCategoryBecauseNoRegisteredHandler(new object[]
					{
						category
					}));
				}
				Utilities.SafeRelease<IObjectCollection>(ref objectCollection);
				List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
				foreach (JumpList._ShellObjectPair shellObjectPair2 in jumpItems)
				{
					if (shellObjectPair2.JumpItem is JumpPath)
					{
						rejectionList.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = shellObjectPair2.JumpItem,
							Reason = JumpItemRejectionReason.NoRegisteredHandler
						});
					}
					else
					{
						list.Add(shellObjectPair2);
					}
				}
				if (list.Count > 0)
				{
					JumpList.AddCategory(cdl, category, list, successList, rejectionList, false);
					return;
				}
			}
			else
			{
				foreach (JumpList._ShellObjectPair shellObjectPair3 in jumpItems)
				{
					rejectionList.Add(new JumpList._RejectedJumpItemPair
					{
						JumpItem = shellObjectPair3.JumpItem,
						Reason = JumpItemRejectionReason.InvalidItem
					});
				}
			}
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x001A1A54 File Offset: 0x001A0A54
		private static IShellLinkW CreateLinkFromJumpTask(JumpTask jumpTask, bool allowSeparators)
		{
			if (string.IsNullOrEmpty(jumpTask.Title) && (!allowSeparators || !string.IsNullOrEmpty(jumpTask.CustomCategory)))
			{
				return null;
			}
			IShellLinkW shellLinkW = (IShellLinkW)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046")));
			IShellLinkW result;
			try
			{
				string path = JumpList._FullName;
				if (!string.IsNullOrEmpty(jumpTask.ApplicationPath))
				{
					path = jumpTask.ApplicationPath;
				}
				shellLinkW.SetPath(path);
				if (!string.IsNullOrEmpty(jumpTask.WorkingDirectory))
				{
					shellLinkW.SetWorkingDirectory(jumpTask.WorkingDirectory);
				}
				if (!string.IsNullOrEmpty(jumpTask.Arguments))
				{
					shellLinkW.SetArguments(jumpTask.Arguments);
				}
				if (jumpTask.IconResourceIndex != -1)
				{
					string pszIconPath = JumpList._FullName;
					if (!string.IsNullOrEmpty(jumpTask.IconResourcePath))
					{
						if (jumpTask.IconResourcePath.Length >= 260)
						{
							return null;
						}
						pszIconPath = jumpTask.IconResourcePath;
					}
					shellLinkW.SetIconLocation(pszIconPath, jumpTask.IconResourceIndex);
				}
				if (!string.IsNullOrEmpty(jumpTask.Description))
				{
					shellLinkW.SetDescription(jumpTask.Description);
				}
				IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
				PROPVARIANT propvariant = new PROPVARIANT();
				try
				{
					PKEY pkey = default(PKEY);
					if (!string.IsNullOrEmpty(jumpTask.Title))
					{
						propvariant.SetValue(jumpTask.Title);
						pkey = PKEY.Title;
					}
					else
					{
						propvariant.SetValue(true);
						pkey = PKEY.AppUserModel_IsDestListSeparator;
					}
					propertyStore.SetValue(ref pkey, propvariant);
				}
				finally
				{
					Utilities.SafeDispose<PROPVARIANT>(ref propvariant);
				}
				propertyStore.Commit();
				IShellLinkW shellLinkW2 = shellLinkW;
				shellLinkW = null;
				result = shellLinkW2;
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				Utilities.SafeRelease<IShellLinkW>(ref shellLinkW);
			}
			return result;
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x001A1C14 File Offset: 0x001A0C14
		private static IShellItem2 CreateItemFromJumpPath(JumpPath jumpPath)
		{
			try
			{
				return ShellUtil.GetShellItemForPath(Path.GetFullPath(jumpPath.Path));
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x001A1C4C File Offset: 0x001A0C4C
		private static JumpItem GetJumpItemForShellObject(object shellObject)
		{
			IShellItem2 shellItem = shellObject as IShellItem2;
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellItem != null)
			{
				return new JumpPath
				{
					Path = shellItem.GetDisplayName((SIGDN)2147647488U)
				};
			}
			if (shellLinkW != null)
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				shellLinkW.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
				StringBuilder stringBuilder2 = new StringBuilder(1024);
				shellLinkW.GetArguments(stringBuilder2, stringBuilder2.Capacity);
				StringBuilder stringBuilder3 = new StringBuilder(1024);
				shellLinkW.GetDescription(stringBuilder3, stringBuilder3.Capacity);
				StringBuilder stringBuilder4 = new StringBuilder(260);
				int iconResourceIndex;
				shellLinkW.GetIconLocation(stringBuilder4, stringBuilder4.Capacity, out iconResourceIndex);
				StringBuilder stringBuilder5 = new StringBuilder(260);
				shellLinkW.GetWorkingDirectory(stringBuilder5, stringBuilder5.Capacity);
				JumpTask jumpTask = new JumpTask
				{
					ApplicationPath = stringBuilder.ToString(),
					Arguments = stringBuilder2.ToString(),
					Description = stringBuilder3.ToString(),
					IconResourceIndex = iconResourceIndex,
					IconResourcePath = stringBuilder4.ToString(),
					WorkingDirectory = stringBuilder5.ToString()
				};
				using (PROPVARIANT propvariant = new PROPVARIANT())
				{
					IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
					PKEY title = PKEY.Title;
					propertyStore.GetValue(ref title, propvariant);
					jumpTask.Title = (propvariant.GetValue() ?? "");
				}
				return jumpTask;
			}
			return null;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x001A1DB0 File Offset: 0x001A0DB0
		private static string ShellLinkToString(IShellLinkW shellLink)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			shellLink.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
			string text = null;
			using (PROPVARIANT propvariant = new PROPVARIANT())
			{
				IPropertyStore propertyStore = (IPropertyStore)shellLink;
				PKEY title = PKEY.Title;
				propertyStore.GetValue(ref title, propvariant);
				text = (propvariant.GetValue() ?? "");
			}
			StringBuilder stringBuilder2 = new StringBuilder(1024);
			shellLink.GetArguments(stringBuilder2, stringBuilder2.Capacity);
			return stringBuilder.ToString().ToUpperInvariant() + text.ToUpperInvariant() + stringBuilder2.ToString();
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x001A1E54 File Offset: 0x001A0E54
		private void RejectEverything()
		{
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			if (jumpItemsRejected == null)
			{
				this._jumpItems.Clear();
				return;
			}
			if (this._jumpItems.Count > 0)
			{
				List<JumpItemRejectionReason> list = new List<JumpItemRejectionReason>(this._jumpItems.Count);
				for (int i = 0; i < this._jumpItems.Count; i++)
				{
					list.Add(JumpItemRejectionReason.InvalidItem);
				}
				JumpItemsRejectedEventArgs e = new JumpItemsRejectedEventArgs(this._jumpItems, list);
				this._jumpItems.Clear();
				jumpItemsRejected(this, e);
			}
		}

		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06002B2F RID: 11055 RVA: 0x001A1ED4 File Offset: 0x001A0ED4
		// (remove) Token: 0x06002B30 RID: 11056 RVA: 0x001A1F0C File Offset: 0x001A0F0C
		public event EventHandler<JumpItemsRejectedEventArgs> JumpItemsRejected;

		// Token: 0x14000075 RID: 117
		// (add) Token: 0x06002B31 RID: 11057 RVA: 0x001A1F44 File Offset: 0x001A0F44
		// (remove) Token: 0x06002B32 RID: 11058 RVA: 0x001A1F7C File Offset: 0x001A0F7C
		public event EventHandler<JumpItemsRemovedEventArgs> JumpItemsRemovedByUser;

		// Token: 0x04001A9F RID: 6815
		private static readonly object s_lock = new object();

		// Token: 0x04001AA0 RID: 6816
		private static readonly Dictionary<Application, JumpList> s_applicationMap = new Dictionary<Application, JumpList>();

		// Token: 0x04001AA1 RID: 6817
		private Application _application;

		// Token: 0x04001AA2 RID: 6818
		private bool? _initializing;

		// Token: 0x04001AA3 RID: 6819
		private List<JumpItem> _jumpItems;

		// Token: 0x04001AA6 RID: 6822
		private static readonly string _FullName = UnsafeNativeMethods.GetModuleFileName(default(HandleRef));

		// Token: 0x02000AA1 RID: 2721
		private class _RejectedJumpItemPair
		{
			// Token: 0x17001E52 RID: 7762
			// (get) Token: 0x060086FF RID: 34559 RVA: 0x0032C0A8 File Offset: 0x0032B0A8
			// (set) Token: 0x06008700 RID: 34560 RVA: 0x0032C0B0 File Offset: 0x0032B0B0
			public JumpItem JumpItem { get; set; }

			// Token: 0x17001E53 RID: 7763
			// (get) Token: 0x06008701 RID: 34561 RVA: 0x0032C0B9 File Offset: 0x0032B0B9
			// (set) Token: 0x06008702 RID: 34562 RVA: 0x0032C0C1 File Offset: 0x0032B0C1
			public JumpItemRejectionReason Reason { get; set; }
		}

		// Token: 0x02000AA2 RID: 2722
		private class _ShellObjectPair
		{
			// Token: 0x17001E54 RID: 7764
			// (get) Token: 0x06008704 RID: 34564 RVA: 0x0032C0CA File Offset: 0x0032B0CA
			// (set) Token: 0x06008705 RID: 34565 RVA: 0x0032C0D2 File Offset: 0x0032B0D2
			public JumpItem JumpItem { get; set; }

			// Token: 0x17001E55 RID: 7765
			// (get) Token: 0x06008706 RID: 34566 RVA: 0x0032C0DB File Offset: 0x0032B0DB
			// (set) Token: 0x06008707 RID: 34567 RVA: 0x0032C0E3 File Offset: 0x0032B0E3
			public object ShellObject { get; set; }

			// Token: 0x06008708 RID: 34568 RVA: 0x0032C0EC File Offset: 0x0032B0EC
			public static void ReleaseShellObjects(List<JumpList._ShellObjectPair> list)
			{
				if (list != null)
				{
					foreach (JumpList._ShellObjectPair shellObjectPair in list)
					{
						object shellObject = shellObjectPair.ShellObject;
						shellObjectPair.ShellObject = null;
						Utilities.SafeRelease<object>(ref shellObject);
					}
				}
			}
		}
	}
}
