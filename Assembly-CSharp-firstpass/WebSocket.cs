using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

// Token: 0x02000003 RID: 3
public class WebSocket
{
	// Token: 0x060000F4 RID: 244 RVA: 0x0000C634 File Offset: 0x0000A834
	public WebSocket(Uri url)
	{
		this.mUrl = url;
		string scheme = this.mUrl.Scheme;
		if (!scheme.Equals("ws") && !scheme.Equals("wss"))
		{
			throw new ArgumentException("Unsupported protocol: " + scheme);
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000C690 File Offset: 0x0000A890
	public void SendString(string str)
	{
		this.Send(Encoding.UTF8.GetBytes(str));
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000C6A4 File Offset: 0x0000A8A4
	public string RecvString()
	{
		byte[] array = this.Recv();
		if (array == null)
		{
			return null;
		}
		return Encoding.UTF8.GetString(array);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
	public IEnumerator Connect()
	{
		this.m_Socket = new WebSocketSharp.WebSocket(this.mUrl.ToString(), Array.Empty<string>());
		this.m_Socket.OnMessage += delegate(object sender, MessageEventArgs e)
		{
			this.m_Messages.Enqueue(e.RawData);
		};
		this.m_Socket.OnOpen += delegate(object sender, EventArgs e)
		{
			this.m_IsConnected = true;
		};
		this.m_Socket.OnError += delegate(object sender, ErrorEventArgs e)
		{
			this.m_Error = e.Message;
		};
		this.m_Socket.ConnectAsync();
		while (!this.m_IsConnected && this.m_Error == null)
		{
			yield return 0;
		}
		yield break;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000C6D7 File Offset: 0x0000A8D7
	public void Send(byte[] buffer)
	{
		this.m_Socket.Send(buffer);
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000C6E5 File Offset: 0x0000A8E5
	public byte[] Recv()
	{
		if (this.m_Messages.Count == 0)
		{
			return null;
		}
		return this.m_Messages.Dequeue();
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000C701 File Offset: 0x0000A901
	public void Close()
	{
		this.m_Socket.Close();
	}

	// Token: 0x17000001 RID: 1
	// (get) Token: 0x060000FB RID: 251 RVA: 0x0000C70E File Offset: 0x0000A90E
	public string error
	{
		get
		{
			return this.m_Error;
		}
	}

	// Token: 0x04000026 RID: 38
	private Uri mUrl;

	// Token: 0x04000027 RID: 39
	private WebSocketSharp.WebSocket m_Socket;

	// Token: 0x04000028 RID: 40
	private Queue<byte[]> m_Messages = new Queue<byte[]>();

	// Token: 0x04000029 RID: 41
	private bool m_IsConnected;

	// Token: 0x0400002A RID: 42
	private string m_Error;
}
