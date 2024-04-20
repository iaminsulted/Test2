using System;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	// Token: 0x02000047 RID: 71
	public abstract class WebSocketServiceHost
	{
		// Token: 0x060004F2 RID: 1266 RVA: 0x0001B78C File Offset: 0x0001998C
		protected WebSocketServiceHost(string path, Logger log)
		{
			this._path = path;
			this._log = log;
			this._sessions = new WebSocketSessionManager(log);
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0001B7B0 File Offset: 0x000199B0
		internal ServerState State
		{
			get
			{
				return this._sessions.State;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0001B7D0 File Offset: 0x000199D0
		protected Logger Log
		{
			get
			{
				return this._log;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001B7E8 File Offset: 0x000199E8
		// (set) Token: 0x060004F6 RID: 1270 RVA: 0x0001B805 File Offset: 0x00019A05
		public bool KeepClean
		{
			get
			{
				return this._sessions.KeepClean;
			}
			set
			{
				this._sessions.KeepClean = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001B818 File Offset: 0x00019A18
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001B830 File Offset: 0x00019A30
		public WebSocketSessionManager Sessions
		{
			get
			{
				return this._sessions;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060004F9 RID: 1273
		public abstract Type BehaviorType { get; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001B848 File Offset: 0x00019A48
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x0001B865 File Offset: 0x00019A65
		public TimeSpan WaitTime
		{
			get
			{
				return this._sessions.WaitTime;
			}
			set
			{
				this._sessions.WaitTime = value;
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001B875 File Offset: 0x00019A75
		internal void Start()
		{
			this._sessions.Start();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001B884 File Offset: 0x00019A84
		internal void StartSession(WebSocketContext context)
		{
			this.CreateSession().Start(context, this._sessions);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001B89A File Offset: 0x00019A9A
		internal void Stop(ushort code, string reason)
		{
			this._sessions.Stop(code, reason);
		}

		// Token: 0x060004FF RID: 1279
		protected abstract WebSocketBehavior CreateSession();

		// Token: 0x0400023D RID: 573
		private Logger _log;

		// Token: 0x0400023E RID: 574
		private string _path;

		// Token: 0x0400023F RID: 575
		private WebSocketSessionManager _sessions;
	}
}
