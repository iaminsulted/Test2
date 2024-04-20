using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Documents.Tracing
{
	// Token: 0x020006E8 RID: 1768
	internal class SpellerCOMActionTraceLogger : IDisposable
	{
		// Token: 0x06005CF0 RID: 23792 RVA: 0x0028BB84 File Offset: 0x0028AB84
		public SpellerCOMActionTraceLogger(WinRTSpellerInterop caller, SpellerCOMActionTraceLogger.Actions action)
		{
			this._action = action;
			SpellerCOMActionTraceLogger.InstanceInfo instanceInfo = null;
			object lockObject = SpellerCOMActionTraceLogger._lockObject;
			lock (lockObject)
			{
				if (!SpellerCOMActionTraceLogger._instanceInfos.TryGetValue(caller, out instanceInfo))
				{
					instanceInfo = new SpellerCOMActionTraceLogger.InstanceInfo
					{
						Id = Guid.NewGuid(),
						CumulativeCallTime100Ns = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>(),
						NumCallsMeasured = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>()
					};
					foreach (object obj in Enum.GetValues(typeof(SpellerCOMActionTraceLogger.Actions)))
					{
						SpellerCOMActionTraceLogger.Actions key = (SpellerCOMActionTraceLogger.Actions)obj;
						instanceInfo.CumulativeCallTime100Ns.Add(key, 0L);
						instanceInfo.NumCallsMeasured.Add(key, 0L);
					}
					SpellerCOMActionTraceLogger._instanceInfos.Add(caller, instanceInfo);
				}
			}
			this._instanceInfo = instanceInfo;
			this._beginTicks = DateTime.Now.Ticks;
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x0028BC98 File Offset: 0x0028AC98
		private void UpdateRunningAverageAndLogDebugInfo(long endTicks)
		{
			try
			{
				long num = endTicks - this._beginTicks;
				object lockObject = SpellerCOMActionTraceLogger._lockObject;
				lock (lockObject)
				{
					Dictionary<SpellerCOMActionTraceLogger.Actions, long> numCallsMeasured = this._instanceInfo.NumCallsMeasured;
					SpellerCOMActionTraceLogger.Actions action = this._action;
					long num2 = numCallsMeasured[action];
					numCallsMeasured[action] = num2 + 1L;
					Dictionary<SpellerCOMActionTraceLogger.Actions, long> cumulativeCallTime100Ns = this._instanceInfo.CumulativeCallTime100Ns;
					action = this._action;
					cumulativeCallTime100Ns[action] += num;
				}
				long num3 = (long)Math.Floor(1.0 * (double)this._instanceInfo.CumulativeCallTime100Ns[this._action] / (double)this._instanceInfo.NumCallsMeasured[this._action]);
				if (this._action == SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary || this._action == SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary || num > SpellerCOMActionTraceLogger._timeLimits100Ns[this._action] || num3 > 2L * SpellerCOMActionTraceLogger._timeLimits100Ns[this._action])
				{
					EventSource provider = TraceLoggingProvider.GetProvider();
					EventSourceOptions options = TelemetryEventSource.MeasuresOptions();
					SpellerCOMActionTraceLogger.SpellerCOMTimingData data = new SpellerCOMActionTraceLogger.SpellerCOMTimingData
					{
						TextBoxBaseIdentifier = this._instanceInfo.Id.ToString(),
						SpellerCOMAction = this._action.ToString(),
						CallTimeForCOMCallMs = (long)Math.Floor((double)num * 1.0 / 10000.0),
						RunningAverageCallTimeForCOMCallsMs = (long)Math.Floor((double)num3 * 1.0 / 10000.0)
					};
					if (provider != null)
					{
						provider.Write<SpellerCOMActionTraceLogger.SpellerCOMTimingData>(SpellerCOMActionTraceLogger.SpellerCOMLatencyMeasurement, options, data);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06005CF2 RID: 23794 RVA: 0x0028BE84 File Offset: 0x0028AE84
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.UpdateRunningAverageAndLogDebugInfo(DateTime.Now.Ticks);
				}
				this._disposed = true;
			}
		}

		// Token: 0x06005CF3 RID: 23795 RVA: 0x0028BEB6 File Offset: 0x0028AEB6
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0400313B RID: 12603
		private static readonly string SpellerCOMLatencyMeasurement = "SpellerCOMLatencyMeasurement";

		// Token: 0x0400313C RID: 12604
		private static readonly Dictionary<SpellerCOMActionTraceLogger.Actions, long> _timeLimits100Ns = new Dictionary<SpellerCOMActionTraceLogger.Actions, long>
		{
			{
				SpellerCOMActionTraceLogger.Actions.SpellCheckerCreation,
				2500000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.ComprehensiveCheck,
				500000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.RegisterUserDictionary,
				10000000L
			},
			{
				SpellerCOMActionTraceLogger.Actions.UnregisterUserDictionary,
				10000000L
			}
		};

		// Token: 0x0400313D RID: 12605
		private static WeakDictionary<WinRTSpellerInterop, SpellerCOMActionTraceLogger.InstanceInfo> _instanceInfos = new WeakDictionary<WinRTSpellerInterop, SpellerCOMActionTraceLogger.InstanceInfo>();

		// Token: 0x0400313E RID: 12606
		private static object _lockObject = new object();

		// Token: 0x0400313F RID: 12607
		private SpellerCOMActionTraceLogger.Actions _action;

		// Token: 0x04003140 RID: 12608
		private long _beginTicks;

		// Token: 0x04003141 RID: 12609
		private SpellerCOMActionTraceLogger.InstanceInfo _instanceInfo;

		// Token: 0x04003142 RID: 12610
		private bool _disposed;

		// Token: 0x02000B8D RID: 2957
		public enum Actions
		{
			// Token: 0x04004953 RID: 18771
			SpellCheckerCreation,
			// Token: 0x04004954 RID: 18772
			RegisterUserDictionary,
			// Token: 0x04004955 RID: 18773
			UnregisterUserDictionary,
			// Token: 0x04004956 RID: 18774
			ComprehensiveCheck
		}

		// Token: 0x02000B8E RID: 2958
		private class InstanceInfo
		{
			// Token: 0x17001F2F RID: 7983
			// (get) Token: 0x06008E9C RID: 36508 RVA: 0x003432D5 File Offset: 0x003422D5
			// (set) Token: 0x06008E9D RID: 36509 RVA: 0x003432DD File Offset: 0x003422DD
			public Guid Id { get; set; }

			// Token: 0x17001F30 RID: 7984
			// (get) Token: 0x06008E9E RID: 36510 RVA: 0x003432E6 File Offset: 0x003422E6
			// (set) Token: 0x06008E9F RID: 36511 RVA: 0x003432EE File Offset: 0x003422EE
			public Dictionary<SpellerCOMActionTraceLogger.Actions, long> CumulativeCallTime100Ns { get; set; }

			// Token: 0x17001F31 RID: 7985
			// (get) Token: 0x06008EA0 RID: 36512 RVA: 0x003432F7 File Offset: 0x003422F7
			// (set) Token: 0x06008EA1 RID: 36513 RVA: 0x003432FF File Offset: 0x003422FF
			public Dictionary<SpellerCOMActionTraceLogger.Actions, long> NumCallsMeasured { get; set; }
		}

		// Token: 0x02000B8F RID: 2959
		[EventData]
		private struct SpellerCOMTimingData
		{
			// Token: 0x17001F32 RID: 7986
			// (get) Token: 0x06008EA3 RID: 36515 RVA: 0x00343308 File Offset: 0x00342308
			// (set) Token: 0x06008EA4 RID: 36516 RVA: 0x00343310 File Offset: 0x00342310
			public string TextBoxBaseIdentifier { readonly get; set; }

			// Token: 0x17001F33 RID: 7987
			// (get) Token: 0x06008EA5 RID: 36517 RVA: 0x00343319 File Offset: 0x00342319
			// (set) Token: 0x06008EA6 RID: 36518 RVA: 0x00343321 File Offset: 0x00342321
			public string SpellerCOMAction { readonly get; set; }

			// Token: 0x17001F34 RID: 7988
			// (get) Token: 0x06008EA7 RID: 36519 RVA: 0x0034332A File Offset: 0x0034232A
			// (set) Token: 0x06008EA8 RID: 36520 RVA: 0x00343332 File Offset: 0x00342332
			public long CallTimeForCOMCallMs { readonly get; set; }

			// Token: 0x17001F35 RID: 7989
			// (get) Token: 0x06008EA9 RID: 36521 RVA: 0x0034333B File Offset: 0x0034233B
			// (set) Token: 0x06008EAA RID: 36522 RVA: 0x00343343 File Offset: 0x00342343
			public long RunningAverageCallTimeForCOMCallsMs { readonly get; set; }
		}
	}
}
