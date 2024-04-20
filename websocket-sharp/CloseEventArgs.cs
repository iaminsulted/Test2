using System;

namespace WebSocketSharp
{
	// Token: 0x02000004 RID: 4
	public class CloseEventArgs : EventArgs
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000045D1 File Offset: 0x000027D1
		internal CloseEventArgs()
		{
			this._payloadData = PayloadData.Empty;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000045E6 File Offset: 0x000027E6
		internal CloseEventArgs(ushort code) : this(code, null)
		{
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000045E6 File Offset: 0x000027E6
		internal CloseEventArgs(CloseStatusCode code) : this((ushort)code, null)
		{
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000045F2 File Offset: 0x000027F2
		internal CloseEventArgs(PayloadData payloadData)
		{
			this._payloadData = payloadData;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004603 File Offset: 0x00002803
		internal CloseEventArgs(ushort code, string reason)
		{
			this._payloadData = new PayloadData(code, reason);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000461A File Offset: 0x0000281A
		internal CloseEventArgs(CloseStatusCode code, string reason) : this((ushort)code, reason)
		{
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00004628 File Offset: 0x00002828
		internal PayloadData PayloadData
		{
			get
			{
				return this._payloadData;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00004640 File Offset: 0x00002840
		public ushort Code
		{
			get
			{
				return this._payloadData.Code;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00004660 File Offset: 0x00002860
		public string Reason
		{
			get
			{
				return this._payloadData.Reason ?? string.Empty;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004688 File Offset: 0x00002888
		// (set) Token: 0x06000081 RID: 129 RVA: 0x000046A0 File Offset: 0x000028A0
		public bool WasClean
		{
			get
			{
				return this._clean;
			}
			internal set
			{
				this._clean = value;
			}
		}

		// Token: 0x04000008 RID: 8
		private bool _clean;

		// Token: 0x04000009 RID: 9
		private PayloadData _payloadData;
	}
}
