using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000514 RID: 1300
	internal class ReaderStream : Stream
	{
		// Token: 0x060040A9 RID: 16553 RVA: 0x00214683 File Offset: 0x00213683
		internal ReaderStream(ReadWriteStreamManager streamManager)
		{
			this._streamManager = streamManager;
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x060040AA RID: 16554 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x060040AB RID: 16555 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E6E RID: 3694
		// (get) Token: 0x060040AC RID: 16556 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Close()
		{
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Flush()
		{
		}

		// Token: 0x17000E6F RID: 3695
		// (get) Token: 0x060040AF RID: 16559 RVA: 0x00214692 File Offset: 0x00213692
		public override long Length
		{
			get
			{
				return this.StreamManager.ReadLength;
			}
		}

		// Token: 0x17000E70 RID: 3696
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x0021469F File Offset: 0x0021369F
		// (set) Token: 0x060040B1 RID: 16561 RVA: 0x002146AC File Offset: 0x002136AC
		public override long Position
		{
			get
			{
				return this.StreamManager.ReadPosition;
			}
			set
			{
				this.StreamManager.ReaderSeek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x002146BC File Offset: 0x002136BC
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.StreamManager.Read(buffer, offset, count);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x002146CC File Offset: 0x002136CC
		public override int ReadByte()
		{
			return this.StreamManager.ReadByte();
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x002146D9 File Offset: 0x002136D9
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this.StreamManager.ReaderSeek(offset, loc);
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0012F160 File Offset: 0x0012E160
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x0012F160 File Offset: 0x0012E160
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x002146E8 File Offset: 0x002136E8
		internal void ReaderDoneWithFileUpToPosition(long position)
		{
			this.StreamManager.ReaderDoneWithFileUpToPosition(position);
		}

		// Token: 0x17000E71 RID: 3697
		// (get) Token: 0x060040B8 RID: 16568 RVA: 0x002146F6 File Offset: 0x002136F6
		private ReadWriteStreamManager StreamManager
		{
			get
			{
				return this._streamManager;
			}
		}

		// Token: 0x0400243E RID: 9278
		private ReadWriteStreamManager _streamManager;
	}
}
