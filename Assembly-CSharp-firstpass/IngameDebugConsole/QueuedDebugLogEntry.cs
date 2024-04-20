using System;
using UnityEngine;

namespace IngameDebugConsole
{
	// Token: 0x020001EB RID: 491
	public struct QueuedDebugLogEntry
	{
		// Token: 0x06000F64 RID: 3940 RVA: 0x0002DC60 File Offset: 0x0002BE60
		public QueuedDebugLogEntry(string logString, string stackTrace, LogType logType)
		{
			this.logString = logString;
			this.stackTrace = stackTrace;
			this.logType = logType;
		}

		// Token: 0x04000AC5 RID: 2757
		public readonly string logString;

		// Token: 0x04000AC6 RID: 2758
		public readonly string stackTrace;

		// Token: 0x04000AC7 RID: 2759
		public readonly LogType logType;
	}
}
