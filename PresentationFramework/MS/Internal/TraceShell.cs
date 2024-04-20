using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020000F4 RID: 244
	internal static class TraceShell
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00102D6D File Offset: 0x00101D6D
		public static AvTraceDetails NotOnWindows7
		{
			get
			{
				if (TraceShell._NotOnWindows7 == null)
				{
					TraceShell._NotOnWindows7 = new AvTraceDetails(1, new string[]
					{
						"Shell integration features are not being applied because the host OS does not support the feature."
					});
				}
				return TraceShell._NotOnWindows7;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00102D94 File Offset: 0x00101D94
		public static AvTraceDetails ExplorerTaskbarTimeout
		{
			get
			{
				if (TraceShell._ExplorerTaskbarTimeout == null)
				{
					TraceShell._ExplorerTaskbarTimeout = new AvTraceDetails(2, new string[]
					{
						"Communication with Explorer timed out while trying to update the taskbar item for the window."
					});
				}
				return TraceShell._ExplorerTaskbarTimeout;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00102DBB File Offset: 0x00101DBB
		public static AvTraceDetails ExplorerTaskbarRetrying
		{
			get
			{
				if (TraceShell._ExplorerTaskbarRetrying == null)
				{
					TraceShell._ExplorerTaskbarRetrying = new AvTraceDetails(3, new string[]
					{
						"Making another attempt to update the taskbar."
					});
				}
				return TraceShell._ExplorerTaskbarRetrying;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00102DE2 File Offset: 0x00101DE2
		public static AvTraceDetails ExplorerTaskbarNotRunning
		{
			get
			{
				if (TraceShell._ExplorerTaskbarNotRunning == null)
				{
					TraceShell._ExplorerTaskbarNotRunning = new AvTraceDetails(4, new string[]
					{
						"Halting attempts at Shell integration with the taskbar because it appears that Explorer is not running."
					});
				}
				return TraceShell._ExplorerTaskbarNotRunning;
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00102E09 File Offset: 0x00101E09
		public static AvTraceDetails NativeTaskbarError(params object[] args)
		{
			if (TraceShell._NativeTaskbarError == null)
			{
				TraceShell._NativeTaskbarError = new AvTraceDetails(5, new string[]
				{
					"The native ITaskbarList3 interface failed a method call with error {0}."
				});
			}
			return new AvTraceFormat(TraceShell._NativeTaskbarError, args);
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00102E36 File Offset: 0x00101E36
		public static AvTraceDetails RejectingJumpItemsBecauseCatastrophicFailure
		{
			get
			{
				if (TraceShell._RejectingJumpItemsBecauseCatastrophicFailure == null)
				{
					TraceShell._RejectingJumpItemsBecauseCatastrophicFailure = new AvTraceDetails(6, new string[]
					{
						"Failed to apply items to the JumpList because the native interfaces failed."
					});
				}
				return TraceShell._RejectingJumpItemsBecauseCatastrophicFailure;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00102E5D File Offset: 0x00101E5D
		public static AvTraceDetails RejectingJumpListCategoryBecauseNoRegisteredHandler(params object[] args)
		{
			if (TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler == null)
			{
				TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler = new AvTraceDetails(7, new string[]
				{
					"Rejecting the category {0} from the jump list because this application is not registered for file types contained in the list.  JumpPath items will be removed and the operation will be retried."
				});
			}
			return new AvTraceFormat(TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler, args);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00102E8A File Offset: 0x00101E8A
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00102EAB File Offset: 0x00101EAB
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00102ED4 File Offset: 0x00101ED4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00102F0C File Offset: 0x00101F0C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00102F48 File Offset: 0x00101F48
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00102F86 File Offset: 0x00101F86
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00102FA5 File Offset: 0x00101FA5
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00102FC9 File Offset: 0x00101FC9
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00102FF1 File Offset: 0x00101FF1
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0010301D File Offset: 0x0010201D
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0010304D File Offset: 0x0010204D
		public static bool IsEnabled
		{
			get
			{
				return TraceShell._avTrace != null && TraceShell._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x00103062 File Offset: 0x00102062
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceShell._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0010306E File Offset: 0x0010206E
		public static void Refresh()
		{
			TraceShell._avTrace.Refresh();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0010307A File Offset: 0x0010207A
		static TraceShell()
		{
			TraceShell._avTrace.EnabledByDebugger = true;
		}

		// Token: 0x040006CE RID: 1742
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.ShellSource, delegate()
		{
			PresentationTraceSources._ShellSource = null;
		});

		// Token: 0x040006CF RID: 1743
		private static AvTraceDetails _NotOnWindows7;

		// Token: 0x040006D0 RID: 1744
		private static AvTraceDetails _ExplorerTaskbarTimeout;

		// Token: 0x040006D1 RID: 1745
		private static AvTraceDetails _ExplorerTaskbarRetrying;

		// Token: 0x040006D2 RID: 1746
		private static AvTraceDetails _ExplorerTaskbarNotRunning;

		// Token: 0x040006D3 RID: 1747
		private static AvTraceDetails _NativeTaskbarError;

		// Token: 0x040006D4 RID: 1748
		private static AvTraceDetails _RejectingJumpItemsBecauseCatastrophicFailure;

		// Token: 0x040006D5 RID: 1749
		private static AvTraceDetails _RejectingJumpListCategoryBecauseNoRegisteredHandler;
	}
}
