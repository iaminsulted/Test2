using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;

namespace WebSocketSharp.Server
{
	// Token: 0x0200004A RID: 74
	public class WebSocketSessionManager
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x0001BAAC File Offset: 0x00019CAC
		internal WebSocketSessionManager(Logger log)
		{
			this._log = log;
			this._clean = true;
			this._forSweep = new object();
			this._sessions = new Dictionary<string, IWebSocketSession>();
			this._state = ServerState.Ready;
			this._sync = ((ICollection)this._sessions).SyncRoot;
			this._waitTime = TimeSpan.FromSeconds(1.0);
			this.setSweepTimer(60000.0);
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001BB28 File Offset: 0x00019D28
		internal ServerState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001BB44 File Offset: 0x00019D44
		public IEnumerable<string> ActiveIDs
		{
			get
			{
				foreach (KeyValuePair<string, bool> res in this.broadping(WebSocketFrame.EmptyPingBytes))
				{
					bool value = res.Value;
					if (value)
					{
						yield return res.Key;
					}
					res = default(KeyValuePair<string, bool>);
				}
				Dictionary<string, bool>.Enumerator enumerator = default(Dictionary<string, bool>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001BB64 File Offset: 0x00019D64
		public int Count
		{
			get
			{
				object sync = this._sync;
				int count;
				lock (sync)
				{
					count = this._sessions.Count;
				}
				return count;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001BBA8 File Offset: 0x00019DA8
		public IEnumerable<string> IDs
		{
			get
			{
				bool flag = this._state != ServerState.Start;
				IEnumerable<string> result;
				if (flag)
				{
					result = Enumerable.Empty<string>();
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = this._state != ServerState.Start;
						if (flag2)
						{
							result = Enumerable.Empty<string>();
						}
						else
						{
							result = this._sessions.Keys.ToList<string>();
						}
					}
				}
				return result;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001BC28 File Offset: 0x00019E28
		public IEnumerable<string> InactiveIDs
		{
			get
			{
				foreach (KeyValuePair<string, bool> res in this.broadping(WebSocketFrame.EmptyPingBytes))
				{
					bool flag = !res.Value;
					if (flag)
					{
						yield return res.Key;
					}
					res = default(KeyValuePair<string, bool>);
				}
				Dictionary<string, bool>.Enumerator enumerator = default(Dictionary<string, bool>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x17000185 RID: 389
		public IWebSocketSession this[string id]
		{
			get
			{
				bool flag = id == null;
				if (flag)
				{
					throw new ArgumentNullException("id");
				}
				bool flag2 = id.Length == 0;
				if (flag2)
				{
					throw new ArgumentException("An empty string.", "id");
				}
				IWebSocketSession result;
				this.tryGetSession(id, out result);
				return result;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001BC98 File Offset: 0x00019E98
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x0001BCB4 File Offset: 0x00019EB4
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
							this._clean = value;
						}
					}
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001BD34 File Offset: 0x00019F34
		public IEnumerable<IWebSocketSession> Sessions
		{
			get
			{
				bool flag = this._state != ServerState.Start;
				IEnumerable<IWebSocketSession> result;
				if (flag)
				{
					result = Enumerable.Empty<IWebSocketSession>();
				}
				else
				{
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = this._state != ServerState.Start;
						if (flag2)
						{
							result = Enumerable.Empty<IWebSocketSession>();
						}
						else
						{
							result = this._sessions.Values.ToList<IWebSocketSession>();
						}
					}
				}
				return result;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		// (set) Token: 0x06000518 RID: 1304 RVA: 0x0001BDCC File Offset: 0x00019FCC
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
							this._waitTime = value;
						}
					}
				}
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001BE6C File Offset: 0x0001A06C
		private void broadcast(Opcode opcode, byte[] data, Action completed)
		{
			Dictionary<CompressionMethod, byte[]> dictionary = new Dictionary<CompressionMethod, byte[]>();
			try
			{
				foreach (IWebSocketSession webSocketSession in this.Sessions)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						this._log.Error("The service is shutting down.");
						break;
					}
					webSocketSession.Context.WebSocket.Send(opcode, data, dictionary);
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

		// Token: 0x0600051A RID: 1306 RVA: 0x0001BF5C File Offset: 0x0001A15C
		private void broadcast(Opcode opcode, Stream stream, Action completed)
		{
			Dictionary<CompressionMethod, Stream> dictionary = new Dictionary<CompressionMethod, Stream>();
			try
			{
				foreach (IWebSocketSession webSocketSession in this.Sessions)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						this._log.Error("The service is shutting down.");
						break;
					}
					webSocketSession.Context.WebSocket.Send(opcode, stream, dictionary);
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

		// Token: 0x0600051B RID: 1307 RVA: 0x0001C094 File Offset: 0x0001A294
		private void broadcastAsync(Opcode opcode, byte[] data, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, data, completed);
			});
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001C0D8 File Offset: 0x0001A2D8
		private void broadcastAsync(Opcode opcode, Stream stream, Action completed)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				this.broadcast(opcode, stream, completed);
			});
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001C11C File Offset: 0x0001A31C
		private Dictionary<string, bool> broadping(byte[] frameAsBytes)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					this._log.Error("The service is shutting down.");
					break;
				}
				bool value = webSocketSession.Context.WebSocket.Ping(frameAsBytes, this._waitTime);
				dictionary.Add(webSocketSession.ID, value);
			}
			return dictionary;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001C1C4 File Offset: 0x0001A3C4
		private bool canSet(out string message)
		{
			message = null;
			bool flag = this._state == ServerState.Start;
			bool result;
			if (flag)
			{
				message = "The service has already started.";
				result = false;
			}
			else
			{
				bool flag2 = this._state == ServerState.ShuttingDown;
				if (flag2)
				{
					message = "The service is shutting down.";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001C210 File Offset: 0x0001A410
		private static string createID()
		{
			return Guid.NewGuid().ToString("N");
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001C234 File Offset: 0x0001A434
		private void setSweepTimer(double interval)
		{
			this._sweepTimer = new System.Timers.Timer(interval);
			this._sweepTimer.Elapsed += delegate(object sender, ElapsedEventArgs e)
			{
				this.Sweep();
			};
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001C25C File Offset: 0x0001A45C
		private void stop(PayloadData payloadData, bool send)
		{
			byte[] frameAsBytes = send ? WebSocketFrame.CreateCloseFrame(payloadData, false).ToArray() : null;
			object sync = this._sync;
			lock (sync)
			{
				this._state = ServerState.ShuttingDown;
				this._sweepTimer.Enabled = false;
				foreach (IWebSocketSession webSocketSession in this._sessions.Values.ToList<IWebSocketSession>())
				{
					webSocketSession.Context.WebSocket.Close(payloadData, frameAsBytes);
				}
				this._state = ServerState.Stop;
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001C320 File Offset: 0x0001A520
		private bool tryGetSession(string id, out IWebSocketSession session)
		{
			session = null;
			bool flag = this._state != ServerState.Start;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object sync = this._sync;
				lock (sync)
				{
					bool flag2 = this._state != ServerState.Start;
					if (flag2)
					{
						result = false;
					}
					else
					{
						result = this._sessions.TryGetValue(id, out session);
					}
				}
			}
			return result;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001C398 File Offset: 0x0001A598
		internal string Add(IWebSocketSession session)
		{
			object sync = this._sync;
			string result;
			lock (sync)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					result = null;
				}
				else
				{
					string text = WebSocketSessionManager.createID();
					this._sessions.Add(text, session);
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001C3FC File Offset: 0x0001A5FC
		internal void Broadcast(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
		{
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					this._log.Error("The service is shutting down.");
					break;
				}
				webSocketSession.Context.WebSocket.Send(opcode, data, cache);
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001C484 File Offset: 0x0001A684
		internal void Broadcast(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
		{
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					this._log.Error("The service is shutting down.");
					break;
				}
				webSocketSession.Context.WebSocket.Send(opcode, stream, cache);
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001C50C File Offset: 0x0001A70C
		internal Dictionary<string, bool> Broadping(byte[] frameAsBytes, TimeSpan timeout)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			foreach (IWebSocketSession webSocketSession in this.Sessions)
			{
				bool flag = this._state != ServerState.Start;
				if (flag)
				{
					this._log.Error("The service is shutting down.");
					break;
				}
				bool value = webSocketSession.Context.WebSocket.Ping(frameAsBytes, timeout);
				dictionary.Add(webSocketSession.ID, value);
			}
			return dictionary;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001C5B0 File Offset: 0x0001A7B0
		internal bool Remove(string id)
		{
			object sync = this._sync;
			bool result;
			lock (sync)
			{
				result = this._sessions.Remove(id);
			}
			return result;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001C5F4 File Offset: 0x0001A7F4
		internal void Start()
		{
			object sync = this._sync;
			lock (sync)
			{
				this._sweepTimer.Enabled = this._clean;
				this._state = ServerState.Start;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001C64C File Offset: 0x0001A84C
		internal void Stop(ushort code, string reason)
		{
			bool flag = code == 1005;
			if (flag)
			{
				this.stop(PayloadData.Empty, true);
			}
			else
			{
				this.stop(new PayloadData(code, reason), !code.IsReserved());
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001C690 File Offset: 0x0001A890
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

		// Token: 0x0600052B RID: 1323 RVA: 0x0001C700 File Offset: 0x0001A900
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

		// Token: 0x0600052C RID: 1324 RVA: 0x0001C798 File Offset: 0x0001A998
		public void Broadcast(Stream stream, int length)
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
				this.broadcast(Opcode.Binary, array, null);
			}
			else
			{
				this.broadcast(Opcode.Binary, new MemoryStream(array), null);
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001C8A0 File Offset: 0x0001AAA0
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

		// Token: 0x0600052E RID: 1326 RVA: 0x0001C910 File Offset: 0x0001AB10
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

		// Token: 0x0600052F RID: 1327 RVA: 0x0001C9A8 File Offset: 0x0001ABA8
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

		// Token: 0x06000530 RID: 1328 RVA: 0x0001CAB0 File Offset: 0x0001ACB0
		[Obsolete("This method will be removed.")]
		public Dictionary<string, bool> Broadping()
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message);
			}
			return this.Broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
		[Obsolete("This method will be removed.")]
		public Dictionary<string, bool> Broadping(string message)
		{
			bool flag = this._state != ServerState.Start;
			if (flag)
			{
				string message2 = "The current state of the manager is not Start.";
				throw new InvalidOperationException(message2);
			}
			bool flag2 = message.IsNullOrEmpty();
			Dictionary<string, bool> result;
			if (flag2)
			{
				result = this.Broadping(WebSocketFrame.EmptyPingBytes, this._waitTime);
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
				result = this.Broadping(webSocketFrame.ToArray(), this._waitTime);
			}
			return result;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001CBAC File Offset: 0x0001ADAC
		public void CloseSession(string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Close();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001CBEC File Offset: 0x0001ADEC
		public void CloseSession(string id, ushort code, string reason)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Close(code, reason);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001CC2C File Offset: 0x0001AE2C
		public void CloseSession(string id, CloseStatusCode code, string reason)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Close(code, reason);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001CC6C File Offset: 0x0001AE6C
		public bool PingTo(string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			return webSocketSession.Context.WebSocket.Ping();
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001CCAC File Offset: 0x0001AEAC
		public bool PingTo(string message, string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message2 = "The session could not be found.";
				throw new InvalidOperationException(message2);
			}
			return webSocketSession.Context.WebSocket.Ping(message);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001CCF0 File Offset: 0x0001AEF0
		public void SendTo(byte[] data, string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Send(data);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001CD30 File Offset: 0x0001AF30
		public void SendTo(string data, string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Send(data);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001CD70 File Offset: 0x0001AF70
		public void SendTo(Stream stream, int length, string id)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.Send(stream, length);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001CDB0 File Offset: 0x0001AFB0
		public void SendToAsync(byte[] data, string id, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.SendAsync(data, completed);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001CDF0 File Offset: 0x0001AFF0
		public void SendToAsync(string data, string id, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.SendAsync(data, completed);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001CE30 File Offset: 0x0001B030
		public void SendToAsync(Stream stream, int length, string id, Action<bool> completed)
		{
			IWebSocketSession webSocketSession;
			bool flag = !this.TryGetSession(id, out webSocketSession);
			if (flag)
			{
				string message = "The session could not be found.";
				throw new InvalidOperationException(message);
			}
			webSocketSession.Context.WebSocket.SendAsync(stream, length, completed);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001CE74 File Offset: 0x0001B074
		public void Sweep()
		{
			bool sweeping = this._sweeping;
			if (sweeping)
			{
				this._log.Info("The sweeping is already in progress.");
			}
			else
			{
				object forSweep = this._forSweep;
				lock (forSweep)
				{
					bool sweeping2 = this._sweeping;
					if (sweeping2)
					{
						this._log.Info("The sweeping is already in progress.");
						return;
					}
					this._sweeping = true;
				}
				foreach (string key in this.InactiveIDs)
				{
					bool flag = this._state != ServerState.Start;
					if (flag)
					{
						break;
					}
					object sync = this._sync;
					lock (sync)
					{
						bool flag2 = this._state != ServerState.Start;
						if (flag2)
						{
							break;
						}
						IWebSocketSession webSocketSession;
						bool flag3 = this._sessions.TryGetValue(key, out webSocketSession);
						if (flag3)
						{
							WebSocketState connectionState = webSocketSession.ConnectionState;
							bool flag4 = connectionState == WebSocketState.Open;
							if (flag4)
							{
								webSocketSession.Context.WebSocket.Close(CloseStatusCode.Abnormal);
							}
							else
							{
								bool flag5 = connectionState == WebSocketState.Closing;
								if (!flag5)
								{
									this._sessions.Remove(key);
								}
							}
						}
					}
				}
				this._sweeping = false;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001CFFC File Offset: 0x0001B1FC
		public bool TryGetSession(string id, out IWebSocketSession session)
		{
			bool flag = id == null;
			if (flag)
			{
				throw new ArgumentNullException("id");
			}
			bool flag2 = id.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("An empty string.", "id");
			}
			return this.tryGetSession(id, out session);
		}

		// Token: 0x04000242 RID: 578
		private volatile bool _clean;

		// Token: 0x04000243 RID: 579
		private object _forSweep;

		// Token: 0x04000244 RID: 580
		private Logger _log;

		// Token: 0x04000245 RID: 581
		private Dictionary<string, IWebSocketSession> _sessions;

		// Token: 0x04000246 RID: 582
		private volatile ServerState _state;

		// Token: 0x04000247 RID: 583
		private volatile bool _sweeping;

		// Token: 0x04000248 RID: 584
		private System.Timers.Timer _sweepTimer;

		// Token: 0x04000249 RID: 585
		private object _sync;

		// Token: 0x0400024A RID: 586
		private TimeSpan _waitTime;
	}
}
