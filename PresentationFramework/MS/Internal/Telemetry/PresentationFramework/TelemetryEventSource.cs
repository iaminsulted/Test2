using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020002DC RID: 732
	internal class TelemetryEventSource : EventSource
	{
		// Token: 0x06001BC0 RID: 7104 RVA: 0x0016A3ED File Offset: 0x001693ED
		internal TelemetryEventSource(string eventSourceName) : base(eventSourceName, EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0016A3FC File Offset: 0x001693FC
		protected TelemetryEventSource() : base(EventSourceSettings.EtwSelfDescribingEventFormat, TelemetryEventSource.telemetryTraits)
		{
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x0016A40C File Offset: 0x0016940C
		internal static EventSourceOptions TelemetryOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)35184372088832L
			};
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0016A434 File Offset: 0x00169434
		internal static EventSourceOptions MeasuresOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)70368744177664L
			};
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0016A45C File Offset: 0x0016945C
		internal static EventSourceOptions CriticalDataOptions()
		{
			return new EventSourceOptions
			{
				Keywords = (EventKeywords)140737488355328L
			};
		}

		// Token: 0x04000E1F RID: 3615
		internal const EventKeywords Reserved44Keyword = (EventKeywords)17592186044416L;

		// Token: 0x04000E20 RID: 3616
		internal const EventKeywords TelemetryKeyword = (EventKeywords)35184372088832L;

		// Token: 0x04000E21 RID: 3617
		internal const EventKeywords MeasuresKeyword = (EventKeywords)70368744177664L;

		// Token: 0x04000E22 RID: 3618
		internal const EventKeywords CriticalDataKeyword = (EventKeywords)140737488355328L;

		// Token: 0x04000E23 RID: 3619
		internal const EventTags CoreData = (EventTags)524288;

		// Token: 0x04000E24 RID: 3620
		internal const EventTags InjectXToken = (EventTags)1048576;

		// Token: 0x04000E25 RID: 3621
		internal const EventTags RealtimeLatency = (EventTags)2097152;

		// Token: 0x04000E26 RID: 3622
		internal const EventTags NormalLatency = (EventTags)4194304;

		// Token: 0x04000E27 RID: 3623
		internal const EventTags CriticalPersistence = (EventTags)8388608;

		// Token: 0x04000E28 RID: 3624
		internal const EventTags NormalPersistence = (EventTags)16777216;

		// Token: 0x04000E29 RID: 3625
		internal const EventTags DropPii = (EventTags)33554432;

		// Token: 0x04000E2A RID: 3626
		internal const EventTags HashPii = (EventTags)67108864;

		// Token: 0x04000E2B RID: 3627
		internal const EventTags MarkPii = (EventTags)134217728;

		// Token: 0x04000E2C RID: 3628
		internal const EventFieldTags DropPiiField = (EventFieldTags)67108864;

		// Token: 0x04000E2D RID: 3629
		internal const EventFieldTags HashPiiField = (EventFieldTags)134217728;

		// Token: 0x04000E2E RID: 3630
		private static readonly string[] telemetryTraits = new string[]
		{
			"ETW_GROUP",
			"{4f50731a-89cf-4782-b3e0-dce8c90476ba}"
		};
	}
}
