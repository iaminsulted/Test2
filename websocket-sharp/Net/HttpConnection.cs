using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WebSocketSharp.Net
{
	// Token: 0x0200001F RID: 31
	internal sealed class HttpConnection
	{
		// Token: 0x06000225 RID: 549 RVA: 0x0000E690 File Offset: 0x0000C890
		internal HttpConnection(Socket socket, EndPointListener listener)
		{
			this._socket = socket;
			this._listener = listener;
			NetworkStream networkStream = new NetworkStream(socket, false);
			bool isSecure = listener.IsSecure;
			if (isSecure)
			{
				ServerSslConfiguration sslConfiguration = listener.SslConfiguration;
				SslStream sslStream = new SslStream(networkStream, false, sslConfiguration.ClientCertificateValidationCallback);
				sslStream.AuthenticateAsServer(sslConfiguration.ServerCertificate, sslConfiguration.ClientCertificateRequired, sslConfiguration.EnabledSslProtocols, sslConfiguration.CheckCertificateRevocation);
				this._secure = true;
				this._stream = sslStream;
			}
			else
			{
				this._stream = networkStream;
			}
			this._localEndPoint = socket.LocalEndPoint;
			this._remoteEndPoint = socket.RemoteEndPoint;
			this._sync = new object();
			this._timeout = 90000;
			this._timeoutCanceled = new Dictionary<int, bool>();
			this._timer = new Timer(new TimerCallback(HttpConnection.onTimeout), this, -1, -1);
			this.init();
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000E770 File Offset: 0x0000C970
		public bool IsClosed
		{
			get
			{
				return this._socket == null;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000E78C File Offset: 0x0000C98C
		public bool IsLocal
		{
			get
			{
				return ((IPEndPoint)this._remoteEndPoint).Address.IsLocal();
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000E7B4 File Offset: 0x0000C9B4
		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000E7CC File Offset: 0x0000C9CC
		public IPEndPoint LocalEndPoint
		{
			get
			{
				return (IPEndPoint)this._localEndPoint;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600022A RID: 554 RVA: 0x0000E7EC File Offset: 0x0000C9EC
		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return (IPEndPoint)this._remoteEndPoint;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000E80C File Offset: 0x0000CA0C
		public int Reuses
		{
			get
			{
				return this._reuses;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000E824 File Offset: 0x0000CA24
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000E83C File Offset: 0x0000CA3C
		private void close()
		{
			object sync = this._sync;
			lock (sync)
			{
				bool flag = this._socket == null;
				if (flag)
				{
					return;
				}
				this.disposeTimer();
				this.disposeRequestBuffer();
				this.disposeStream();
				this.closeSocket();
			}
			this.unregisterContext();
			this.removeConnection();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
		private void closeSocket()
		{
			try
			{
				this._socket.Shutdown(SocketShutdown.Both);
			}
			catch
			{
			}
			this._socket.Close();
			this._socket = null;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
		private void disposeRequestBuffer()
		{
			bool flag = this._requestBuffer == null;
			if (!flag)
			{
				this._requestBuffer.Dispose();
				this._requestBuffer = null;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E928 File Offset: 0x0000CB28
		private void disposeStream()
		{
			bool flag = this._stream == null;
			if (!flag)
			{
				this._inputStream = null;
				this._outputStream = null;
				this._stream.Dispose();
				this._stream = null;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E968 File Offset: 0x0000CB68
		private void disposeTimer()
		{
			bool flag = this._timer == null;
			if (!flag)
			{
				try
				{
					this._timer.Change(-1, -1);
				}
				catch
				{
				}
				this._timer.Dispose();
				this._timer = null;
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
		private void init()
		{
			this._context = new HttpListenerContext(this);
			this._inputState = InputState.RequestLine;
			this._inputStream = null;
			this._lineState = LineState.None;
			this._outputStream = null;
			this._position = 0;
			this._requestBuffer = new MemoryStream();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000EA00 File Offset: 0x0000CC00
		private static void onRead(IAsyncResult asyncResult)
		{
			HttpConnection httpConnection = (HttpConnection)asyncResult.AsyncState;
			bool flag = httpConnection._socket == null;
			if (!flag)
			{
				object sync = httpConnection._sync;
				lock (sync)
				{
					bool flag2 = httpConnection._socket == null;
					if (!flag2)
					{
						int num = -1;
						int length = 0;
						try
						{
							int reuses = httpConnection._reuses;
							bool flag3 = !httpConnection._timeoutCanceled[reuses];
							if (flag3)
							{
								httpConnection._timer.Change(-1, -1);
								httpConnection._timeoutCanceled[reuses] = true;
							}
							num = httpConnection._stream.EndRead(asyncResult);
							httpConnection._requestBuffer.Write(httpConnection._buffer, 0, num);
							length = (int)httpConnection._requestBuffer.Length;
						}
						catch (Exception ex)
						{
							bool flag4 = httpConnection._requestBuffer != null && httpConnection._requestBuffer.Length > 0L;
							if (flag4)
							{
								httpConnection.SendError(ex.Message, 400);
								return;
							}
							httpConnection.close();
							return;
						}
						bool flag5 = num <= 0;
						if (flag5)
						{
							httpConnection.close();
						}
						else
						{
							bool flag6 = httpConnection.processInput(httpConnection._requestBuffer.GetBuffer(), length);
							if (flag6)
							{
								bool flag7 = !httpConnection._context.HasError;
								if (flag7)
								{
									httpConnection._context.Request.FinishInitialization();
								}
								bool hasError = httpConnection._context.HasError;
								if (hasError)
								{
									httpConnection.SendError();
								}
								else
								{
									HttpListener httpListener;
									bool flag8 = !httpConnection._listener.TrySearchHttpListener(httpConnection._context.Request.Url, out httpListener);
									if (flag8)
									{
										httpConnection.SendError(null, 404);
									}
									else
									{
										bool flag9 = httpConnection._lastListener != httpListener;
										if (flag9)
										{
											httpConnection.removeConnection();
											bool flag10 = !httpListener.AddConnection(httpConnection);
											if (flag10)
											{
												httpConnection.close();
												return;
											}
											httpConnection._lastListener = httpListener;
										}
										httpConnection._context.Listener = httpListener;
										bool flag11 = !httpConnection._context.Authenticate();
										if (!flag11)
										{
											bool flag12 = httpConnection._context.Register();
											if (flag12)
											{
												httpConnection._contextRegistered = true;
											}
										}
									}
								}
							}
							else
							{
								httpConnection._stream.BeginRead(httpConnection._buffer, 0, 8192, new AsyncCallback(HttpConnection.onRead), httpConnection);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000ECA0 File Offset: 0x0000CEA0
		private static void onTimeout(object state)
		{
			HttpConnection httpConnection = (HttpConnection)state;
			int reuses = httpConnection._reuses;
			bool flag = httpConnection._socket == null;
			if (!flag)
			{
				object sync = httpConnection._sync;
				lock (sync)
				{
					bool flag2 = httpConnection._socket == null;
					if (!flag2)
					{
						bool flag3 = httpConnection._timeoutCanceled[reuses];
						if (!flag3)
						{
							httpConnection.SendError(null, 408);
						}
					}
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000ED28 File Offset: 0x0000CF28
		private bool processInput(byte[] data, int length)
		{
			bool flag = this._currentLine == null;
			if (flag)
			{
				this._currentLine = new StringBuilder(64);
			}
			int num = 0;
			try
			{
				string text;
				while ((text = this.readLineFrom(data, this._position, length, out num)) != null)
				{
					this._position += num;
					bool flag2 = text.Length == 0;
					if (flag2)
					{
						bool flag3 = this._inputState == InputState.RequestLine;
						if (!flag3)
						{
							bool flag4 = this._position > 32768;
							if (flag4)
							{
								this._context.ErrorMessage = "Headers too long";
							}
							this._currentLine = null;
							return true;
						}
					}
					else
					{
						bool flag5 = this._inputState == InputState.RequestLine;
						if (flag5)
						{
							this._context.Request.SetRequestLine(text);
							this._inputState = InputState.Headers;
						}
						else
						{
							this._context.Request.AddHeader(text);
						}
						bool hasError = this._context.HasError;
						if (hasError)
						{
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				this._context.ErrorMessage = ex.Message;
				return true;
			}
			this._position += num;
			bool flag6 = this._position >= 32768;
			bool result;
			if (flag6)
			{
				this._context.ErrorMessage = "Headers too long";
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000EEA4 File Offset: 0x0000D0A4
		private string readLineFrom(byte[] buffer, int offset, int length, out int read)
		{
			read = 0;
			int num = offset;
			while (num < length && this._lineState != LineState.Lf)
			{
				read++;
				byte b = buffer[num];
				bool flag = b == 13;
				if (flag)
				{
					this._lineState = LineState.Cr;
				}
				else
				{
					bool flag2 = b == 10;
					if (flag2)
					{
						this._lineState = LineState.Lf;
					}
					else
					{
						this._currentLine.Append((char)b);
					}
				}
				num++;
			}
			bool flag3 = this._lineState != LineState.Lf;
			string result;
			if (flag3)
			{
				result = null;
			}
			else
			{
				string text = this._currentLine.ToString();
				this._currentLine.Length = 0;
				this._lineState = LineState.None;
				result = text;
			}
			return result;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000EF58 File Offset: 0x0000D158
		private void removeConnection()
		{
			bool flag = this._lastListener != null;
			if (flag)
			{
				this._lastListener.RemoveConnection(this);
			}
			else
			{
				this._listener.RemoveConnection(this);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000EF90 File Offset: 0x0000D190
		private void unregisterContext()
		{
			bool flag = !this._contextRegistered;
			if (!flag)
			{
				this._context.Unregister();
				this._contextRegistered = false;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000EFC0 File Offset: 0x0000D1C0
		internal void Close(bool force)
		{
			bool flag = this._socket == null;
			if (!flag)
			{
				object sync = this._sync;
				lock (sync)
				{
					bool flag2 = this._socket == null;
					if (!flag2)
					{
						if (force)
						{
							bool flag3 = this._outputStream != null;
							if (flag3)
							{
								this._outputStream.Close(true);
							}
							this.close();
						}
						else
						{
							this.GetResponseStream().Close(false);
							bool closeConnection = this._context.Response.CloseConnection;
							if (closeConnection)
							{
								this.close();
							}
							else
							{
								bool flag4 = !this._context.Request.FlushInput();
								if (flag4)
								{
									this.close();
								}
								else
								{
									this.disposeRequestBuffer();
									this.unregisterContext();
									this.init();
									this._reuses++;
									this.BeginReadRequest();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000F0C4 File Offset: 0x0000D2C4
		public void BeginReadRequest()
		{
			bool flag = this._buffer == null;
			if (flag)
			{
				this._buffer = new byte[8192];
			}
			bool flag2 = this._reuses == 1;
			if (flag2)
			{
				this._timeout = 15000;
			}
			try
			{
				this._timeoutCanceled.Add(this._reuses, false);
				this._timer.Change(this._timeout, -1);
				this._stream.BeginRead(this._buffer, 0, 8192, new AsyncCallback(HttpConnection.onRead), this);
			}
			catch
			{
				this.close();
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000F174 File Offset: 0x0000D374
		public void Close()
		{
			this.Close(false);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000F180 File Offset: 0x0000D380
		public RequestStream GetRequestStream(long contentLength, bool chunked)
		{
			object sync = this._sync;
			RequestStream result;
			lock (sync)
			{
				bool flag = this._socket == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = this._inputStream != null;
					if (flag2)
					{
						result = this._inputStream;
					}
					else
					{
						byte[] buffer = this._requestBuffer.GetBuffer();
						int num = (int)this._requestBuffer.Length;
						int count = num - this._position;
						this.disposeRequestBuffer();
						this._inputStream = (chunked ? new ChunkedRequestStream(this._stream, buffer, this._position, count, this._context) : new RequestStream(this._stream, buffer, this._position, count, contentLength));
						result = this._inputStream;
					}
				}
			}
			return result;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000F254 File Offset: 0x0000D454
		public ResponseStream GetResponseStream()
		{
			object sync = this._sync;
			ResponseStream result;
			lock (sync)
			{
				bool flag = this._socket == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					bool flag2 = this._outputStream != null;
					if (flag2)
					{
						result = this._outputStream;
					}
					else
					{
						HttpListener listener = this._context.Listener;
						bool ignoreWriteExceptions = listener == null || listener.IgnoreWriteExceptions;
						this._outputStream = new ResponseStream(this._stream, this._context.Response, ignoreWriteExceptions);
						result = this._outputStream;
					}
				}
			}
			return result;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000F2F8 File Offset: 0x0000D4F8
		public void SendError()
		{
			this.SendError(this._context.ErrorMessage, this._context.ErrorStatus);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000F318 File Offset: 0x0000D518
		public void SendError(string message, int status)
		{
			bool flag = this._socket == null;
			if (!flag)
			{
				object sync = this._sync;
				lock (sync)
				{
					bool flag2 = this._socket == null;
					if (!flag2)
					{
						try
						{
							HttpListenerResponse response = this._context.Response;
							response.StatusCode = status;
							response.ContentType = "text/html";
							StringBuilder stringBuilder = new StringBuilder(64);
							stringBuilder.AppendFormat("<html><body><h1>{0} {1}", status, response.StatusDescription);
							bool flag3 = message != null && message.Length > 0;
							if (flag3)
							{
								stringBuilder.AppendFormat(" ({0})</h1></body></html>", message);
							}
							else
							{
								stringBuilder.Append("</h1></body></html>");
							}
							Encoding utf = Encoding.UTF8;
							byte[] bytes = utf.GetBytes(stringBuilder.ToString());
							response.ContentEncoding = utf;
							response.ContentLength64 = (long)bytes.Length;
							response.Close(bytes, true);
						}
						catch
						{
							this.Close(true);
						}
					}
				}
			}
		}

		// Token: 0x040000C5 RID: 197
		private byte[] _buffer;

		// Token: 0x040000C6 RID: 198
		private const int _bufferLength = 8192;

		// Token: 0x040000C7 RID: 199
		private HttpListenerContext _context;

		// Token: 0x040000C8 RID: 200
		private bool _contextRegistered;

		// Token: 0x040000C9 RID: 201
		private StringBuilder _currentLine;

		// Token: 0x040000CA RID: 202
		private InputState _inputState;

		// Token: 0x040000CB RID: 203
		private RequestStream _inputStream;

		// Token: 0x040000CC RID: 204
		private HttpListener _lastListener;

		// Token: 0x040000CD RID: 205
		private LineState _lineState;

		// Token: 0x040000CE RID: 206
		private EndPointListener _listener;

		// Token: 0x040000CF RID: 207
		private EndPoint _localEndPoint;

		// Token: 0x040000D0 RID: 208
		private ResponseStream _outputStream;

		// Token: 0x040000D1 RID: 209
		private int _position;

		// Token: 0x040000D2 RID: 210
		private EndPoint _remoteEndPoint;

		// Token: 0x040000D3 RID: 211
		private MemoryStream _requestBuffer;

		// Token: 0x040000D4 RID: 212
		private int _reuses;

		// Token: 0x040000D5 RID: 213
		private bool _secure;

		// Token: 0x040000D6 RID: 214
		private Socket _socket;

		// Token: 0x040000D7 RID: 215
		private Stream _stream;

		// Token: 0x040000D8 RID: 216
		private object _sync;

		// Token: 0x040000D9 RID: 217
		private int _timeout;

		// Token: 0x040000DA RID: 218
		private Dictionary<int, bool> _timeoutCanceled;

		// Token: 0x040000DB RID: 219
		private Timer _timer;
	}
}
