using System;

namespace WebSocketSharp.Net
{
	// Token: 0x02000034 RID: 52
	internal class ReadBufferState
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x000167C2 File Offset: 0x000149C2
		public ReadBufferState(byte[] buffer, int offset, int count, HttpStreamAsyncResult asyncResult)
		{
			this._buffer = buffer;
			this._offset = offset;
			this._count = count;
			this._initialCount = count;
			this._asyncResult = asyncResult;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000167F0 File Offset: 0x000149F0
		// (set) Token: 0x060003BB RID: 955 RVA: 0x00016808 File Offset: 0x00014A08
		public HttpStreamAsyncResult AsyncResult
		{
			get
			{
				return this._asyncResult;
			}
			set
			{
				this._asyncResult = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003BC RID: 956 RVA: 0x00016814 File Offset: 0x00014A14
		// (set) Token: 0x060003BD RID: 957 RVA: 0x0001682C File Offset: 0x00014A2C
		public byte[] Buffer
		{
			get
			{
				return this._buffer;
			}
			set
			{
				this._buffer = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003BE RID: 958 RVA: 0x00016838 File Offset: 0x00014A38
		// (set) Token: 0x060003BF RID: 959 RVA: 0x00016850 File Offset: 0x00014A50
		public int Count
		{
			get
			{
				return this._count;
			}
			set
			{
				this._count = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001685C File Offset: 0x00014A5C
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00016874 File Offset: 0x00014A74
		public int InitialCount
		{
			get
			{
				return this._initialCount;
			}
			set
			{
				this._initialCount = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00016880 File Offset: 0x00014A80
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00016898 File Offset: 0x00014A98
		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x04000187 RID: 391
		private HttpStreamAsyncResult _asyncResult;

		// Token: 0x04000188 RID: 392
		private byte[] _buffer;

		// Token: 0x04000189 RID: 393
		private int _count;

		// Token: 0x0400018A RID: 394
		private int _initialCount;

		// Token: 0x0400018B RID: 395
		private int _offset;
	}
}
