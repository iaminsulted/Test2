using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020002DD RID: 733
	internal static class TraceLoggingProvider
	{
		// Token: 0x06001BC6 RID: 7110 RVA: 0x0016A4A0 File Offset: 0x001694A0
		internal static EventSource GetProvider()
		{
			if (TraceLoggingProvider._logger == null)
			{
				object lockObject = TraceLoggingProvider._lockObject;
				lock (lockObject)
				{
					if (TraceLoggingProvider._logger == null)
					{
						try
						{
							TraceLoggingProvider._logger = new TelemetryEventSource(TraceLoggingProvider.ProviderName);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
			return TraceLoggingProvider._logger;
		}

		// Token: 0x04000E2F RID: 3631
		private static EventSource _logger;

		// Token: 0x04000E30 RID: 3632
		private static object _lockObject = new object();

		// Token: 0x04000E31 RID: 3633
		private static readonly string ProviderName = "Microsoft.DOTNET.WPF.PresentationFramework";
	}
}
