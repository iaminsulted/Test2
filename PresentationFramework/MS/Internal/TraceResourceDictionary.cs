using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020000F1 RID: 241
	internal static class TraceResourceDictionary
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x0010229A File Offset: 0x0010129A
		public static AvTraceDetails AddResource
		{
			get
			{
				if (TraceResourceDictionary._AddResource == null)
				{
					TraceResourceDictionary._AddResource = new AvTraceDetails(1, new string[]
					{
						"Resource has been added to ResourceDictionary"
					});
				}
				return TraceResourceDictionary._AddResource;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x001022C1 File Offset: 0x001012C1
		public static AvTraceDetails RealizeDeferContent
		{
			get
			{
				if (TraceResourceDictionary._RealizeDeferContent == null)
				{
					TraceResourceDictionary._RealizeDeferContent = new AvTraceDetails(2, new string[]
					{
						"Delayed creation of resource"
					});
				}
				return TraceResourceDictionary._RealizeDeferContent;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x001022E8 File Offset: 0x001012E8
		public static AvTraceDetails FoundResourceOnElement
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceOnElement == null)
				{
					TraceResourceDictionary._FoundResourceOnElement = new AvTraceDetails(3, new string[]
					{
						"Found resource item on an element"
					});
				}
				return TraceResourceDictionary._FoundResourceOnElement;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0010230F File Offset: 0x0010130F
		public static AvTraceDetails FoundResourceInStyle
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInStyle == null)
				{
					TraceResourceDictionary._FoundResourceInStyle = new AvTraceDetails(4, new string[]
					{
						"Found resource item in a style"
					});
				}
				return TraceResourceDictionary._FoundResourceInStyle;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x00102336 File Offset: 0x00101336
		public static AvTraceDetails FoundResourceInTemplate
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInTemplate == null)
				{
					TraceResourceDictionary._FoundResourceInTemplate = new AvTraceDetails(5, new string[]
					{
						"Found resource item in a template"
					});
				}
				return TraceResourceDictionary._FoundResourceInTemplate;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x0010235D File Offset: 0x0010135D
		public static AvTraceDetails FoundResourceInThemeStyle
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInThemeStyle == null)
				{
					TraceResourceDictionary._FoundResourceInThemeStyle = new AvTraceDetails(6, new string[]
					{
						"Found resource item in a theme style"
					});
				}
				return TraceResourceDictionary._FoundResourceInThemeStyle;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x00102384 File Offset: 0x00101384
		public static AvTraceDetails FoundResourceInApplication
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInApplication == null)
				{
					TraceResourceDictionary._FoundResourceInApplication = new AvTraceDetails(7, new string[]
					{
						"Found resource item in application"
					});
				}
				return TraceResourceDictionary._FoundResourceInApplication;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x001023AB File Offset: 0x001013AB
		public static AvTraceDetails FoundResourceInTheme
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInTheme == null)
				{
					TraceResourceDictionary._FoundResourceInTheme = new AvTraceDetails(8, new string[]
					{
						"Found resource item in theme"
					});
				}
				return TraceResourceDictionary._FoundResourceInTheme;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x001023D2 File Offset: 0x001013D2
		public static AvTraceDetails ResourceNotFound
		{
			get
			{
				if (TraceResourceDictionary._ResourceNotFound == null)
				{
					TraceResourceDictionary._ResourceNotFound = new AvTraceDetails(9, new string[]
					{
						"Resource not found"
					});
				}
				return TraceResourceDictionary._ResourceNotFound;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x001023FA File Offset: 0x001013FA
		public static AvTraceDetails NewResourceDictionary
		{
			get
			{
				if (TraceResourceDictionary._NewResourceDictionary == null)
				{
					TraceResourceDictionary._NewResourceDictionary = new AvTraceDetails(10, new string[]
					{
						"New resource dictionary set"
					});
				}
				return TraceResourceDictionary._NewResourceDictionary;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00102422 File Offset: 0x00101422
		public static AvTraceDetails FindResource
		{
			get
			{
				if (TraceResourceDictionary._FindResource == null)
				{
					TraceResourceDictionary._FindResource = new AvTraceDetails(11, new string[]
					{
						"Searching for resource"
					});
				}
				return TraceResourceDictionary._FindResource;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x0010244A File Offset: 0x0010144A
		public static AvTraceDetails SetKey
		{
			get
			{
				if (TraceResourceDictionary._SetKey == null)
				{
					TraceResourceDictionary._SetKey = new AvTraceDetails(12, new string[]
					{
						"Deferred resource has been added to ResourceDictionary"
					});
				}
				return TraceResourceDictionary._SetKey;
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00102472 File Offset: 0x00101472
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00102493 File Offset: 0x00101493
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x001024BC File Offset: 0x001014BC
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x001024F4 File Offset: 0x001014F4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00102530 File Offset: 0x00101530
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0010256E File Offset: 0x0010156E
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0010258D File Offset: 0x0010158D
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x001025B1 File Offset: 0x001015B1
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x001025D9 File Offset: 0x001015D9
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00102605 File Offset: 0x00101605
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00102635 File Offset: 0x00101635
		public static bool IsEnabled
		{
			get
			{
				return TraceResourceDictionary._avTrace != null && TraceResourceDictionary._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0010264A File Offset: 0x0010164A
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceResourceDictionary._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00102656 File Offset: 0x00101656
		public static void Refresh()
		{
			TraceResourceDictionary._avTrace.Refresh();
		}

		// Token: 0x040006AE RID: 1710
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.ResourceDictionarySource, delegate()
		{
			PresentationTraceSources._ResourceDictionarySource = null;
		});

		// Token: 0x040006AF RID: 1711
		private static AvTraceDetails _AddResource;

		// Token: 0x040006B0 RID: 1712
		private static AvTraceDetails _RealizeDeferContent;

		// Token: 0x040006B1 RID: 1713
		private static AvTraceDetails _FoundResourceOnElement;

		// Token: 0x040006B2 RID: 1714
		private static AvTraceDetails _FoundResourceInStyle;

		// Token: 0x040006B3 RID: 1715
		private static AvTraceDetails _FoundResourceInTemplate;

		// Token: 0x040006B4 RID: 1716
		private static AvTraceDetails _FoundResourceInThemeStyle;

		// Token: 0x040006B5 RID: 1717
		private static AvTraceDetails _FoundResourceInApplication;

		// Token: 0x040006B6 RID: 1718
		private static AvTraceDetails _FoundResourceInTheme;

		// Token: 0x040006B7 RID: 1719
		private static AvTraceDetails _ResourceNotFound;

		// Token: 0x040006B8 RID: 1720
		private static AvTraceDetails _NewResourceDictionary;

		// Token: 0x040006B9 RID: 1721
		private static AvTraceDetails _FindResource;

		// Token: 0x040006BA RID: 1722
		private static AvTraceDetails _SetKey;
	}
}
