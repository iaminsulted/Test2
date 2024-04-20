using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020000F3 RID: 243
	internal static class TraceHwndHost
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x00102B22 File Offset: 0x00101B22
		public static AvTraceDetails HwndHostIn3D
		{
			get
			{
				if (TraceHwndHost._HwndHostIn3D == null)
				{
					TraceHwndHost._HwndHostIn3D = new AvTraceDetails(1, new string[]
					{
						"An HwndHost may not be embedded in a 3D scene."
					});
				}
				return TraceHwndHost._HwndHostIn3D;
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00102B49 File Offset: 0x00101B49
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00102B6A File Offset: 0x00101B6A
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00102B90 File Offset: 0x00101B90
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x00102BC8 File Offset: 0x00101BC8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00102C04 File Offset: 0x00101C04
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00102C42 File Offset: 0x00101C42
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00102C61 File Offset: 0x00101C61
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00102C85 File Offset: 0x00101C85
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00102CAD File Offset: 0x00101CAD
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00102CD9 File Offset: 0x00101CD9
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x00102D09 File Offset: 0x00101D09
		public static bool IsEnabled
		{
			get
			{
				return TraceHwndHost._avTrace != null && TraceHwndHost._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00102D1E File Offset: 0x00101D1E
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceHwndHost._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00102D2A File Offset: 0x00101D2A
		public static void Refresh()
		{
			TraceHwndHost._avTrace.Refresh();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00102D36 File Offset: 0x00101D36
		static TraceHwndHost()
		{
			TraceHwndHost._avTrace.EnabledByDebugger = true;
		}

		// Token: 0x040006CC RID: 1740
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.HwndHostSource, delegate()
		{
			PresentationTraceSources._HwndHostSource = null;
		});

		// Token: 0x040006CD RID: 1741
		private static AvTraceDetails _HwndHostIn3D;
	}
}
