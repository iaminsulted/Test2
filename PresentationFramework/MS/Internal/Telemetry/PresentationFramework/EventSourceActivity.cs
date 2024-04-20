using System;
using System.Diagnostics.Contracts;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020002DB RID: 731
	internal sealed class EventSourceActivity : IDisposable
	{
		// Token: 0x06001BAD RID: 7085 RVA: 0x0016A118 File Offset: 0x00169118
		internal EventSourceActivity(EventSource eventSource) : this(eventSource, default(EventSourceOptions))
		{
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0016A135 File Offset: 0x00169135
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions) : this(eventSource, startStopOptions, Guid.Empty)
		{
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0016A144 File Offset: 0x00169144
		internal EventSourceActivity(EventSource eventSource, EventSourceOptions startStopOptions, Guid parentActivityId)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(eventSource != null, "eventSource");
			this._eventSource = eventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivityId;
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0016A17C File Offset: 0x0016917C
		internal EventSourceActivity(EventSourceActivity parentActivity) : this(parentActivity, default(EventSourceOptions))
		{
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0016A199 File Offset: 0x00169199
		internal EventSourceActivity(EventSourceActivity parentActivity, EventSourceOptions startStopOptions)
		{
			this._id = Guid.NewGuid();
			base..ctor();
			Contract.Requires<ArgumentNullException>(parentActivity != null, "parentActivity");
			this._eventSource = parentActivity.EventSource;
			this._startStopOptions = startStopOptions;
			this._parentId = parentActivity.Id;
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x0016A1D9 File Offset: 0x001691D9
		internal EventSource EventSource
		{
			get
			{
				return this._eventSource;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001BB3 RID: 7091 RVA: 0x0016A1E1 File Offset: 0x001691E1
		internal Guid Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0016A1EC File Offset: 0x001691EC
		internal void Start(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Start<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x0016A216 File Offset: 0x00169216
		internal void Start<T>(string eventName, T data)
		{
			this.Start<T>(eventName, ref data);
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0016A224 File Offset: 0x00169224
		internal void Stop(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Stop<EventSourceActivity.EmptyStruct>(eventName, ref instance);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0016A24E File Offset: 0x0016924E
		internal void Stop<T>(string eventName, T data)
		{
			this.Stop<T>(eventName, ref data);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0016A25C File Offset: 0x0016925C
		internal void Write(string eventName)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref eventSourceOptions, ref instance);
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x0016A290 File Offset: 0x00169290
		internal void Write(string eventName, EventSourceOptions options)
		{
			Contract.Requires<ArgumentNullException>(eventName != null, "eventName");
			EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
			this.Write<EventSourceActivity.EmptyStruct>(eventName, ref options, ref instance);
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0016A2BC File Offset: 0x001692BC
		internal void Write<T>(string eventName, T data)
		{
			EventSourceOptions eventSourceOptions = default(EventSourceOptions);
			this.Write<T>(eventName, ref eventSourceOptions, ref data);
		}

		// Token: 0x06001BBB RID: 7099 RVA: 0x0016A2DC File Offset: 0x001692DC
		internal void Write<T>(string eventName, EventSourceOptions options, T data)
		{
			this.Write<T>(eventName, ref options, ref data);
		}

		// Token: 0x06001BBC RID: 7100 RVA: 0x0016A2EC File Offset: 0x001692EC
		public void Dispose()
		{
			if (this._state == EventSourceActivity.State.Started)
			{
				this._state = EventSourceActivity.State.Stopped;
				EventSourceActivity.EmptyStruct instance = EventSourceActivity.EmptyStruct.Instance;
				this._eventSource.Write<EventSourceActivity.EmptyStruct>("Dispose", ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref instance);
			}
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x0016A334 File Offset: 0x00169334
		private void Start<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Initialized)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Started;
			this._startStopOptions.Opcode = EventOpcode.Start;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref this._parentId, ref data);
			this._startStopOptions.Opcode = EventOpcode.Stop;
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x0016A38D File Offset: 0x0016938D
		private void Write<T>(string eventName, ref EventSourceOptions options, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._eventSource.Write<T>(eventName, ref options, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x0016A3B7 File Offset: 0x001693B7
		private void Stop<T>(string eventName, ref T data)
		{
			if (this._state != EventSourceActivity.State.Started)
			{
				throw new InvalidOperationException();
			}
			this._state = EventSourceActivity.State.Stopped;
			this._eventSource.Write<T>(eventName, ref this._startStopOptions, ref this._id, ref EventSourceActivity._emptyGuid, ref data);
		}

		// Token: 0x04000E19 RID: 3609
		private static Guid _emptyGuid;

		// Token: 0x04000E1A RID: 3610
		private readonly EventSource _eventSource;

		// Token: 0x04000E1B RID: 3611
		private EventSourceOptions _startStopOptions;

		// Token: 0x04000E1C RID: 3612
		private Guid _parentId;

		// Token: 0x04000E1D RID: 3613
		private Guid _id;

		// Token: 0x04000E1E RID: 3614
		private EventSourceActivity.State _state;

		// Token: 0x02000A1E RID: 2590
		private enum State
		{
			// Token: 0x040040AF RID: 16559
			Initialized,
			// Token: 0x040040B0 RID: 16560
			Started,
			// Token: 0x040040B1 RID: 16561
			Stopped
		}

		// Token: 0x02000A1F RID: 2591
		[EventData]
		private class EmptyStruct
		{
			// Token: 0x060084FF RID: 34047 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
			private EmptyStruct()
			{
			}

			// Token: 0x17001DEA RID: 7658
			// (get) Token: 0x06008500 RID: 34048 RVA: 0x00327C92 File Offset: 0x00326C92
			internal static EventSourceActivity.EmptyStruct Instance
			{
				get
				{
					if (EventSourceActivity.EmptyStruct._instance == null)
					{
						EventSourceActivity.EmptyStruct._instance = new EventSourceActivity.EmptyStruct();
					}
					return EventSourceActivity.EmptyStruct._instance;
				}
			}

			// Token: 0x040040B2 RID: 16562
			private static EventSourceActivity.EmptyStruct _instance;
		}
	}
}
