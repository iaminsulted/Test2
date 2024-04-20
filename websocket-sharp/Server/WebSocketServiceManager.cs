using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace WebSocketSharp.Server
{
	// Token: 0x0200004C RID: 76
	public class WebSocketServiceManager
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x0001D050 File Offset: 0x0001B250
		internal WebSocketServiceManager(Logger log)
		{
			this._log = log;
			this._clean = true;
			this._hosts = new Dictionary<string, WebSocketServiceHost>();
			this._state = ServerState.Ready;
			this._sync = ((ICollection)this._hosts).SyncRoot;
			this._waitTime = TimeSpan.FromSeconds(1.0);
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0001D0B0 File Offset: 0x0001B2B0
		public int Count
		{
			get
			{
				object sync = this._sync;
				int count;
				lock (sync)
				{
					count = this._hosts.Count;
				}
				return count;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001D0F4 File Offset: 0x0001B2F4
		public IEnumerable<WebSocketServiceHost> Hosts
		{
			get
			{
				object sync = this._sync;
				IEnumerable<WebSocketServiceHost> result;
				lock (sync)
				{
					result = this._hosts.Values.ToList<WebSocketServiceHost>();
				}
				return result;
			}
		}

		// Token: 0x1700018B RID: 395
		public WebSocketServiceHost this[string path]
		{
			get
			{
				bool flag = path == null;
				if (flag)
				{
					throw new ArgumentNullException("path");
				}
				bool flag2 = path.Length == 0;
				if (flag2)
				{
					throw new ArgumentException("An empty string.", "path");
				}
				bool flag3 = path[0] != '/';
				if (flag3)
				{
					throw new ArgumentException("Not an absolute path.", "path");
				}
				bool flag4 = path.IndexOfAny(new char[]
				{
					'?',
					'#'
				}) > -1;
				if (flag4)
				{
					string message = "It includes either or both query and fragment components.";
					throw new ArgumentException(message, "path");
				}
				WebSocketServiceHost result;
				this.InternalTryGetServiceHost(path, out result);
				return result;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0001D200 File Offset: 0x0001B400
		public bool KeepClean
		{
			get
			{
				return this._clean;
			}
			set
			{
				string message;
				bool flag = !this.canSet(out message);
				if (flag)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = !this.canSet(out message);
						if (flag2)
						{
							this._log.Warn(message);
						}
						else
						{
							foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
							{
								webSocketServiceHost.KeepClean = value;
							}
							this._clean = value;
						}
					}
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001D2D0 File Offset: 0x0001B4D0
		public IEnumerable<string> Paths
		{
			get
			{
				object sync = this._sync;
				IEnumerable<string> result;
				lock (sync)
				{
					result = this._hosts.Keys.ToList<string>();
				}
				return result;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001D318 File Offset: 0x0001B518
		[Obsolete("This property will be removed.")]
		public int SessionCount
		{
			get
			{
				int num = 0;
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						break;
					}
					num += webSocketServiceHost.Sessions.Count;
				}
				return num;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001D390 File Offset: 0x0001B590
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x0001D3A8 File Offset: 0x0001B5A8
		public TimeSpan WaitTime
		{
			get
			{
				return this._waitTime;
			}
			set
			{
				bool flag = value <= TimeSpan.Zero;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("value", "Zero or less.");
				}
				string message;
				bool flag2 = !this.canSet(out message);
				if (flag2)
				{
					this._log.Warn(message);
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag3 = !this.canSet(out message);
						if (flag3)
						{
							this._log.Warn(message);
						}
						else
						{
							foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
							{
								webSocketServiceHost.WaitTime = value;
							}
							this._waitTime = value;
						}
					}
				}
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001D498 File Offset: 0x0001B698
		private void broadcast(Opcode opcode, byte[] data, Action completed)
		{
			Dictionary<CompressionMethod, byte[]> dictionary = new Dictionary<CompressionMethod, byte[]>();
			try
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						this._log.Error("The server is shutting down.");
						break;
					}
					webSocketServiceHost.Sessions.Broadcast(opcode, data, dictionary);
				}
				bool flag2 = completed != null;
				if (flag2)
				{
					completed();
				}
			}
			catch (Exception ex)
			{
				this._log.Error(ex.Message);
				this._log.Debug(ex.ToString());
			}
			finally
			{
				dictionary.Clear();
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001D584 File Offset: 0x0001B784
		private void broadcast(Opcode opcode, Stream stream, Action completed)
		{
			Dictionary<CompressionMethod, Stream> dictionary = new Dictionary<CompressionMethod, Stream>();
			try
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						this._log.Error("The server is shutting down.");
						break;
					}
					webSocketServiceHost.Sessions.Broadcast(opcode, stream, dictionary);
				}
				bool flag2 = completed != null;
				if (flag2)
				{
					completed();
				}
			}
			catch (Exception ex)
			{
				this._log.Error(ex.Message);
				this._log.Debug(ex.ToString());
			}
			finally
			{
				foreach (Stream stream2 in dictionary.Values)
				{
					stream2.Dispose();
				}
				dictionary.Clear();
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001D6B8 File Offset: 0x0001B8B8
		private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, data, completed);
			});
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001D6FC File Offset: 0x0001B8FC
		private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, stream, completed);
			});
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001D740 File Offset: 0x0001B940
		private Dictionary<string, Dictionary<string, bool>> broadping(byte[] frameAsBytes, TimeSpan timeout)
		{
			Dictionary<string, Dictionary<string, bool>> dictionary = new Dictionary<string, Dictionary<string, bool>>();
			foreach (WebSocketServiceHost webSocketServiceHost in this.Hosts)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					this._log.Error("The server is shutting down.");
					break;
				}
				Dictionary<string, bool> value = webSocketServiceHost.Sessions.Broadping(frameAsBytes, timeout);
				dictionary.Add(webSocketServiceHost.Path, value);
			}
			return dictionary;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001D7DC File Offset: 0x0001B9DC
		private bool canSet(out string message)
		{
			message = null;
			bool flag = this._state == ServerState.Start;
			bool result;
			if (flag)
			{
				message = "The server has already started.";
				result = false;
			}
			else
			{
				bool flag2 = this._state == ServerState.ShuttingDown;
				if (flag2)
				{
					message = "The server is shutting down.";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001D828 File Offset: 0x0001BA28
		internal void Add<TBehavior>(string path, Func<TBehavior> creator) where TBehavior : WebSocketBehavior
		{
			path = path.TrimSlashFromEnd();
			object sync = this._sync;
			lock (sync)
			{
				WebSocketServiceHost webSocketServiceHost;
				bool flag = this._hosts.TryGetValue(path, out webSocketServiceHost);
				if (flag)
				{
					throw new ArgumentException("Already in use.", "path");
				}
				webSocketServiceHost = new WebSocketServiceHost<TBehavior>(path, creator, null, this._log);
				bool flag2 = !this._clean;
				if (flag2)
				{
					webSocketServiceHost.KeepClean = false;
				}
				bool flag3 = this._waitTime != webSocketServiceHost.WaitTime;
				if (flag3)
				{
					webSocketServiceHost.WaitTime = this._waitTime;
				}
				bool flag4 = this._state == ServerState.Start;
				if (flag4)
				{
					webSocketServiceHost.Start();
				}
				this._hosts.Add(path, webSocketServiceHost);
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001D8FC File Offset: 0x0001BAFC
		internal bool InternalTryGetServiceHost(string path, out WebSocketServiceHost host)
		{
			path = path.TrimSlashFromEnd();
			object sync = this._sync;
			bool result;
			lock (sync)
			{
				result = this._hosts.TryGetValue(path, out host);
			}
			return result;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001D94C File Offset: 0x0001BB4C
		internal void Start()
		{
			object sync = this._sync;
			lock (sync)
			{
				foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
				{
					webSocketServiceHost.Start();
				}
				this._state = ServerState.Start;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001D9D8 File Offset: 0x0001BBD8
		internal void Stop(ushort code, string reason)
		{
			object sync = this._sync;
			lock (sync)
			{
				this._state = ServerState.ShuttingDown;
				foreach (WebSocketServiceHost webSocketServiceHost in this._hosts.Values)
				{
					webSocketServiceHost.Stop(code, reason);
				}
				this._state = ServerState.Stop;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001DA70 File Offset: 0x0001BC70
		public void AddService<TBehavior>(string path, Action<TBehavior> initializer) where TBehavior : WebSocketBehavior, new()
		{
			bool flag = path == null;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = path.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "path");
			}
			bool flag3 = path[0] != '/';
			if (flag3)
			{
				throw new ArgumentException("Not an absolute path.", "path");
			}
			bool flag4 = path.IndexOfAny(new char[]
			{
				'?',
				'#'
			}) > -1;
			if (flag4)
			{
				string message = "It includes either or both query and fragment components.";
				throw new ArgumentException(message, "path");
			}
			path = path.TrimSlashFromEnd();
			object sync = this._sync;
			lock (sync)
			{
				WebSocketServiceHost webSocketServiceHost;
				bool flag5 = this._hosts.TryGetValue(path, out webSocketServiceHost);
				if (flag5)
				{
					throw new ArgumentException("Already in use.", "path");
				}
				webSocketServiceHost = new WebSocketServiceHost<TBehavior>(path, () => Activator.CreateInstance<TBehavior>(), initializer, this._log);
				bool flag6 = !this._clean;
				if (flag6)
				{
					webSocketServiceHost.KeepClean = false;
				}
				bool flag7 = this._waitTime != webSocketServiceHost.WaitTime;
				if (flag7)
				{
					webSocketServiceHost.WaitTime = this._waitTime;
				}
				bool flag8 = this._state == ServerState.Start;
				if (flag8)
				{
					webSocketServiceHost.Start();
				}
				this._hosts.Add(path, webSocketServiceHost);
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001DBF4 File Offset: 0x0001BDF4
		[Obsolete("This method will be removed.")]
		public void Broadcast(byte[] data)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			bool flag3 = (long)data.Length <= (long)WebSocket.FragmentLength;
			if (flag3)
			{
				this.broadcast(Opcode.Binary, data, null);
			}
			else
			{
				this.broadcast(Opcode.Binary, new MemoryStream(data), null);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001DC64 File Offset: 0x0001BE64
		[Obsolete("This method will be removed.")]
		public void Broadcast(string data)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			byte[] array;
			bool flag3 = !data.TryGetUTF8EncodedBytes(out array);
			if (flag3)
			{
				string message2 = "It could not be UTF-8-encoded.";
				throw new ArgumentException(message2, "data");
			}
			bool flag4 = (long)array.Length <= (long)WebSocket.FragmentLength;
			if (flag4)
			{
				this.broadcast(Opcode.Text, array, null);
			}
			else
			{
				this.broadcast(Opcode.Text, new MemoryStream(array), null);
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0001DCFC File Offset: 0x0001BEFC
		[Obsolete("This method will be removed.")]
		public void BroadcastAsync(byte[] data, Action completed)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			bool flag3 = (long)data.Length <= (long)WebSocket.FragmentLength;
			if (flag3)
			{
				this.broadcastAsync(Opcode.Binary, data, completed);
			}
			else
			{
				this.broadcastAsync(Opcode.Binary, new MemoryStream(data), completed);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		[Obsolete("This method will be removed.")]
		public void BroadcastAsync(string data, Action completed)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = data == null;
			if (flag2)
			{
				throw new ArgumentNullException("data");
			}
			byte[] array;
			bool flag3 = !data.TryGetUTF8EncodedBytes(out array);
			if (flag3)
			{
				string message2 = "It could not be UTF-8-encoded.";
				throw new ArgumentException(message2, "data");
			}
			bool flag4 = (long)array.Length <= (long)WebSocket.FragmentLength;
			if (flag4)
			{
				this.broadcastAsync(Opcode.Text, array, completed);
			}
			else
			{
				this.broadcastAsync(Opcode.Text, new MemoryStream(array), completed);
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001DE04 File Offset: 0x0001C004
		[Obsolete("This method will be removed.")]
		public void BroadcastAsync(Stream stream, int length, Action completed)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			bool flag2 = stream == null;
			if (flag2)
			{
				throw new ArgumentNullException("stream");
			}
			bool flag3 = !stream.CanRead;
			if (flag3)
			{
				string message2 = "It cannot be read.";
				throw new ArgumentException(message2, "stream");
			}
			bool flag4 = length < 1;
			if (flag4)
			{
				string message3 = "Less than 1.";
				throw new ArgumentException(message3, "length");
			}
			byte[] array = stream.ReadBytes(length);
			int num = array.Length;
			bool flag5 = num == 0;
			if (flag5)
			{
				string message4 = "No data could be read from it.";
				throw new ArgumentException(message4, "stream");
			}
			bool flag6 = num < length;
			if (flag6)
			{
				this._log.Warn(string.Format("Only {0} byte(s) of data could be read from the stream.", num));
			}
			bool flag7 = num <= WebSocket.FragmentLength;
			if (flag7)
			{
				this.broadcastAsync(Opcode.Binary, array, completed);
			}
			else
			{
				this.broadcastAsync(Opcode.Binary, new MemoryStream(array), completed);
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0001DF0C File Offset: 0x0001C10C
		[Obsolete("This method will be removed.")]
		public Dictionary<string, Dictionary<string, bool>> Broadping()
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			return this.broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001DF50 File Offset: 0x0001C150
		[Obsolete("This method will be removed.")]
		public Dictionary<string, Dictionary<string, bool>> Broadping(string message)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message2 = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message2);
			}
			bool flag2 = message.IsNullOrEmpty();
			Dictionary<string, Dictionary<string, bool>> result;
			if (flag2)
			{
				result = this.broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
			}
			else
			{
				byte[] array;
				bool flag3 = !message.TryGetUTF8EncodedBytes(out array);
				if (flag3)
				{
					string message3 = "It could not be UTF-8-encoded.";
					throw new ArgumentException(message3, "message");
				}
				bool flag4 = array.Length > 125;
				if (flag4)
				{
					string message4 = "Its size is greater than 125 bytes.";
					throw new ArgumentOutOfRangeException("message", message4);
				}
				WebSocketFrame webSocketFrame = WebSocketFrame.CreatePingFrame(array, false);
				result = this.broadping(webSocketFrame.ToArray(), this._waitTime);
			}
			return result;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001E008 File Offset: 0x0001C208
		public void Clear()
		{
			List<WebSocketServiceHost> list = null;
			object sync = this._sync;
			lock (sync)
			{
				list = this._hosts.Values.ToList<WebSocketServiceHost>();
				this._hosts.Clear();
			}
			foreach (WebSocketServiceHost webSocketServiceHost in list)
			{
				bool flag = webSocketServiceHost.State == ServerState.Start;
				if (flag)
				{
					webSocketServiceHost.Stop(1001, string.Empty);
				}
			}
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001E0BC File Offset: 0x0001C2BC
		public bool RemoveService(string path)
		{
			bool flag = path == null;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = path.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "path");
			}
			bool flag3 = path[0] != '/';
			if (flag3)
			{
				throw new ArgumentException("Not an absolute path.", "path");
			}
			bool flag4 = path.IndexOfAny(new char[]
			{
				'?',
				'#'
			}) > -1;
			if (flag4)
			{
				string message = "It includes either or both query and fragment components.";
				throw new ArgumentException(message, "path");
			}
			path = path.TrimSlashFromEnd();
			object sync = this._sync;
			WebSocketServiceHost webSocketServiceHost;
			lock (sync)
			{
				bool flag5 = !this._hosts.TryGetValue(path, out webSocketServiceHost);
				if (flag5)
				{
					return false;
				}
				this._hosts.Remove(path);
			}
			bool flag6 = webSocketServiceHost.State == ServerState.Start;
			if (flag6)
			{
				webSocketServiceHost.Stop(1001, string.Empty);
			}
			return true;
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001E1D8 File Offset: 0x0001C3D8
		public bool TryGetServiceHost(string path, out WebSocketServiceHost host)
		{
			bool flag = path == null;
			if (flag)
			{
				throw new ArgumentNullException("path");
			}
			bool flag2 = path.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "path");
			}
			bool flag3 = path[0] != '/';
			if (flag3)
			{
				throw new ArgumentException("Not an absolute path.", "path");
			}
			bool flag4 = path.IndexOfAny(new char[]
			{
				'?',
				'#'
			}) > -1;
			if (flag4)
			{
				string message = "It includes either or both query and fragment components.";
				throw new ArgumentException(message, "path");
			}
			return this.InternalTryGetServiceHost(path, out host);
		}

		// Token: 0x04000250 RID: 592
		private volatile bool _clean;

		// Token: 0x04000251 RID: 593
		private Dictionary<string, WebSocketServiceHost> _hosts;

		// Token: 0x04000252 RID: 594
		private Logger _log;

		// Token: 0x04000253 RID: 595
		private volatile ServerState _state;

		// Token: 0x04000254 RID: 596
		private object _sync;

		// Token: 0x04000255 RID: 597
		private TimeSpan _waitTime;
	}
}
