using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020002DF RID: 735
	internal static class ControlsTraceLogger
	{
		// Token: 0x06001BC8 RID: 7112 RVA: 0x0016A522 File Offset: 0x00169522
		internal static void LogUsedControlsDetails()
		{
			EventSource provider = TraceLoggingProvider.GetProvider();
			if (provider == null)
			{
				return;
			}
			provider.Write(ControlsTraceLogger.ControlsUsed, TelemetryEventSource.MeasuresOptions(), new
			{
				ControlsUsedInApp = ControlsTraceLogger._telemetryControls
			});
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0016A547 File Offset: 0x00169547
		internal static void AddControl(TelemetryControls control)
		{
			ControlsTraceLogger._telemetryControls |= control;
		}

		// Token: 0x04000E5F RID: 3679
		private static readonly string ControlsUsed = "ControlsUsed";

		// Token: 0x04000E60 RID: 3680
		private static TelemetryControls _telemetryControls = TelemetryControls.None;
	}
}
