using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020000F2 RID: 242
	internal static class TraceMarkup
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0010268E File Offset: 0x0010168E
		public static AvTraceDetails AddValueToAddChild
		{
			get
			{
				if (TraceMarkup._AddValueToAddChild == null)
				{
					TraceMarkup._AddValueToAddChild = new AvTraceDetails(1, new string[]
					{
						"Add value to IAddChild"
					});
				}
				return TraceMarkup._AddValueToAddChild;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x001026B5 File Offset: 0x001016B5
		public static AvTraceDetails AddValueToArray
		{
			get
			{
				if (TraceMarkup._AddValueToArray == null)
				{
					TraceMarkup._AddValueToArray = new AvTraceDetails(2, new string[]
					{
						"Add value to an array property"
					});
				}
				return TraceMarkup._AddValueToArray;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x001026DC File Offset: 0x001016DC
		public static AvTraceDetails AddValueToDictionary
		{
			get
			{
				if (TraceMarkup._AddValueToDictionary == null)
				{
					TraceMarkup._AddValueToDictionary = new AvTraceDetails(3, new string[]
					{
						"Add value to a dictionary property"
					});
				}
				return TraceMarkup._AddValueToDictionary;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00102703 File Offset: 0x00101703
		public static AvTraceDetails AddValueToList
		{
			get
			{
				if (TraceMarkup._AddValueToList == null)
				{
					TraceMarkup._AddValueToList = new AvTraceDetails(4, new string[]
					{
						"CanFreezeAdd value to a collection property"
					});
				}
				return TraceMarkup._AddValueToList;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0010272A File Offset: 0x0010172A
		public static AvTraceDetails BeginInit
		{
			get
			{
				if (TraceMarkup._BeginInit == null)
				{
					TraceMarkup._BeginInit = new AvTraceDetails(5, new string[]
					{
						"Start initialization of object (ISupportInitialize.BeginInit)"
					});
				}
				return TraceMarkup._BeginInit;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00102751 File Offset: 0x00101751
		public static AvTraceDetails CreateMarkupExtension
		{
			get
			{
				if (TraceMarkup._CreateMarkupExtension == null)
				{
					TraceMarkup._CreateMarkupExtension = new AvTraceDetails(6, new string[]
					{
						"Create MarkupExtension"
					});
				}
				return TraceMarkup._CreateMarkupExtension;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00102778 File Offset: 0x00101778
		public static AvTraceDetails CreateObject
		{
			get
			{
				if (TraceMarkup._CreateObject == null)
				{
					TraceMarkup._CreateObject = new AvTraceDetails(7, new string[]
					{
						"Create object"
					});
				}
				return TraceMarkup._CreateObject;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0010279F File Offset: 0x0010179F
		public static AvTraceDetails EndInit
		{
			get
			{
				if (TraceMarkup._EndInit == null)
				{
					TraceMarkup._EndInit = new AvTraceDetails(8, new string[]
					{
						"End initialization of object (ISupportInitialize.EndInit)"
					});
				}
				return TraceMarkup._EndInit;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x001027C6 File Offset: 0x001017C6
		public static AvTraceDetails Load
		{
			get
			{
				if (TraceMarkup._Load == null)
				{
					TraceMarkup._Load = new AvTraceDetails(9, new string[]
					{
						"Load xaml/baml"
					});
				}
				return TraceMarkup._Load;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x001027EE File Offset: 0x001017EE
		public static AvTraceDetails ProcessConstructorParameter
		{
			get
			{
				if (TraceMarkup._ProcessConstructorParameter == null)
				{
					TraceMarkup._ProcessConstructorParameter = new AvTraceDetails(10, new string[]
					{
						"Convert constructor parameter"
					});
				}
				return TraceMarkup._ProcessConstructorParameter;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00102816 File Offset: 0x00101816
		public static AvTraceDetails ProvideValue
		{
			get
			{
				if (TraceMarkup._ProvideValue == null)
				{
					TraceMarkup._ProvideValue = new AvTraceDetails(11, new string[]
					{
						"Converted a MarkupExtension"
					});
				}
				return TraceMarkup._ProvideValue;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0010283E File Offset: 0x0010183E
		public static AvTraceDetails SetCPA
		{
			get
			{
				if (TraceMarkup._SetCPA == null)
				{
					TraceMarkup._SetCPA = new AvTraceDetails(12, new string[]
					{
						"Set property value to the ContentProperty"
					});
				}
				return TraceMarkup._SetCPA;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00102866 File Offset: 0x00101866
		public static AvTraceDetails SetPropertyValue
		{
			get
			{
				if (TraceMarkup._SetPropertyValue == null)
				{
					TraceMarkup._SetPropertyValue = new AvTraceDetails(13, new string[]
					{
						"Set property value"
					});
				}
				return TraceMarkup._SetPropertyValue;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0010288E File Offset: 0x0010188E
		public static AvTraceDetails ThrowException
		{
			get
			{
				if (TraceMarkup._ThrowException == null)
				{
					TraceMarkup._ThrowException = new AvTraceDetails(14, new string[]
					{
						"A xaml exception has been thrown"
					});
				}
				return TraceMarkup._ThrowException;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x001028B6 File Offset: 0x001018B6
		public static AvTraceDetails TypeConvert
		{
			get
			{
				if (TraceMarkup._TypeConvert == null)
				{
					TraceMarkup._TypeConvert = new AvTraceDetails(15, new string[]
					{
						"Converted value"
					});
				}
				return TraceMarkup._TypeConvert;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x001028DE File Offset: 0x001018DE
		public static AvTraceDetails TypeConvertFallback
		{
			get
			{
				if (TraceMarkup._TypeConvertFallback == null)
				{
					TraceMarkup._TypeConvertFallback = new AvTraceDetails(16, new string[]
					{
						"CanFreezeAdd value to a collection property"
					});
				}
				return TraceMarkup._TypeConvertFallback;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00102906 File Offset: 0x00101906
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00102927 File Offset: 0x00101927
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00102950 File Offset: 0x00101950
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00102988 File Offset: 0x00101988
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x001029C4 File Offset: 0x001019C4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00102A02 File Offset: 0x00101A02
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00102A21 File Offset: 0x00101A21
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00102A45 File Offset: 0x00101A45
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00102A6D File Offset: 0x00101A6D
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00102A99 File Offset: 0x00101A99
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x00102AC9 File Offset: 0x00101AC9
		public static bool IsEnabled
		{
			get
			{
				return TraceMarkup._avTrace != null && TraceMarkup._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x00102ADE File Offset: 0x00101ADE
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceMarkup._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00102AEA File Offset: 0x00101AEA
		public static void Refresh()
		{
			TraceMarkup._avTrace.Refresh();
		}

		// Token: 0x040006BB RID: 1723
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.MarkupSource, delegate()
		{
			PresentationTraceSources._MarkupSource = null;
		});

		// Token: 0x040006BC RID: 1724
		private static AvTraceDetails _AddValueToAddChild;

		// Token: 0x040006BD RID: 1725
		private static AvTraceDetails _AddValueToArray;

		// Token: 0x040006BE RID: 1726
		private static AvTraceDetails _AddValueToDictionary;

		// Token: 0x040006BF RID: 1727
		private static AvTraceDetails _AddValueToList;

		// Token: 0x040006C0 RID: 1728
		private static AvTraceDetails _BeginInit;

		// Token: 0x040006C1 RID: 1729
		private static AvTraceDetails _CreateMarkupExtension;

		// Token: 0x040006C2 RID: 1730
		private static AvTraceDetails _CreateObject;

		// Token: 0x040006C3 RID: 1731
		private static AvTraceDetails _EndInit;

		// Token: 0x040006C4 RID: 1732
		private static AvTraceDetails _Load;

		// Token: 0x040006C5 RID: 1733
		private static AvTraceDetails _ProcessConstructorParameter;

		// Token: 0x040006C6 RID: 1734
		private static AvTraceDetails _ProvideValue;

		// Token: 0x040006C7 RID: 1735
		private static AvTraceDetails _SetCPA;

		// Token: 0x040006C8 RID: 1736
		private static AvTraceDetails _SetPropertyValue;

		// Token: 0x040006C9 RID: 1737
		private static AvTraceDetails _ThrowException;

		// Token: 0x040006CA RID: 1738
		private static AvTraceDetails _TypeConvert;

		// Token: 0x040006CB RID: 1739
		private static AvTraceDetails _TypeConvertFallback;
	}
}
