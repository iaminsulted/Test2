using System;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	// Token: 0x02000049 RID: 73
	public interface IWebSocketSession
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000508 RID: 1288
		WebSocketState ConnectionState { get; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000509 RID: 1289
		WebSocketContext Context { get; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600050A RID: 1290
		string ID { get; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600050B RID: 1291
		string Protocol { get; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600050C RID: 1292
		DateTime StartTime { get; }
	}
}
