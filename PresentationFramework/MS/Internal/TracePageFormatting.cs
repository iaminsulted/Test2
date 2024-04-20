using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020000F0 RID: 240
	internal static class TracePageFormatting
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00101FE3 File Offset: 0x00100FE3
		public static AvTraceDetails FormatPage
		{
			get
			{
				if (TracePageFormatting._FormatPage == null)
				{
					TracePageFormatting._FormatPage = new AvTraceDetails(1, new string[]
					{
						"Formatting page"
					});
				}
				return TracePageFormatting._FormatPage;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0010200A File Offset: 0x0010100A
		public static AvTraceDetails PageFormattingError
		{
			get
			{
				if (TracePageFormatting._PageFormattingError == null)
				{
					TracePageFormatting._PageFormattingError = new AvTraceDetails(2, new string[]
					{
						"Error. Page formatting engine could not complete the formatting operation."
					});
				}
				return TracePageFormatting._PageFormattingError;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x00102031 File Offset: 0x00101031
		public static AvTraceDetails UnableToFreezeFreezableSubProperty
		{
			get
			{
				if (TracePageFormatting._UnableToFreezeFreezableSubProperty == null)
				{
					TracePageFormatting._UnableToFreezeFreezableSubProperty = new AvTraceDetails(3, new string[]
					{
						"CanFreeze is returning false because a DependencyProperty on the Freezable has a value that is a Freezable that has also returned false from CanFreeze"
					});
				}
				return TracePageFormatting._UnableToFreezeFreezableSubProperty;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x00102058 File Offset: 0x00101058
		public static AvTraceDetails UnableToFreezeAnimatedProperties
		{
			get
			{
				if (TracePageFormatting._UnableToFreezeAnimatedProperties == null)
				{
					TracePageFormatting._UnableToFreezeAnimatedProperties = new AvTraceDetails(4, new string[]
					{
						"CanFreeze is returning false because at least one DependencyProperty on the Freezable is animated."
					});
				}
				return TracePageFormatting._UnableToFreezeAnimatedProperties;
			}
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0010207F File Offset: 0x0010107F
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x001020A0 File Offset: 0x001010A0
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x001020C8 File Offset: 0x001010C8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00102100 File Offset: 0x00101100
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0010213C File Offset: 0x0010113C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0010217A File Offset: 0x0010117A
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00102199 File Offset: 0x00101199
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x001021BD File Offset: 0x001011BD
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x001021E5 File Offset: 0x001011E5
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00102211 File Offset: 0x00101211
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x00102241 File Offset: 0x00101241
		public static bool IsEnabled
		{
			get
			{
				return TracePageFormatting._avTrace != null && TracePageFormatting._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x00102256 File Offset: 0x00101256
		public static bool IsEnabledOverride
		{
			get
			{
				return TracePageFormatting._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00102262 File Offset: 0x00101262
		public static void Refresh()
		{
			TracePageFormatting._avTrace.Refresh();
		}

		// Token: 0x040006A9 RID: 1705
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.DocumentsSource, delegate()
		{
			PresentationTraceSources._DocumentsSource = null;
		});

		// Token: 0x040006AA RID: 1706
		private static AvTraceDetails _FormatPage;

		// Token: 0x040006AB RID: 1707
		private static AvTraceDetails _PageFormattingError;

		// Token: 0x040006AC RID: 1708
		private static AvTraceDetails _UnableToFreezeFreezableSubProperty;

		// Token: 0x040006AD RID: 1709
		private static AvTraceDetails _UnableToFreezeAnimatedProperties;
	}
}
