using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000513 RID: 1299
	internal class WriterStream : Stream
	{
		// Token: 0x06004099 RID: 16537 RVA: 0x00214621 File Offset: 0x00213621
		internal WriterStream(ReadWriteStreamManager streamManager)
		{
			this._streamManager = streamManager;
		}

		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x0600409A RID: 16538 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600409B RID: 16539 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600409C RID: 16540 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600409D RID: 16541 RVA: 0x00214630 File Offset: 0x00213630
		public override void Close()
		{
			this.StreamManager.WriterClose();
		}

		// Token: 0x0600409E RID: 16542 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public override void Flush()
		{
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600409F RID: 16543 RVA: 0x0021463D File Offset: 0x0021363D
		public override long Length
		{
			get
			{
				return this.StreamManager.WriteLength;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x060040A0 RID: 16544 RVA: 0x0021464A File Offset: 0x0021364A
		// (set) Token: 0x060040A1 RID: 16545 RVA: 0x0012F160 File Offset: 0x0012E160
		public override long Position
		{
			get
			{
				return -1L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060040A2 RID: 16546 RVA: 0x0012F160 File Offset: 0x0012E160
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040A3 RID: 16547 RVA: 0x0012F160 File Offset: 0x0012E160
		public override int ReadByte()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040A4 RID: 16548 RVA: 0x0021464E File Offset: 0x0021364E
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this.StreamManager.WriterSeek(offset, loc);
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x0012F160 File Offset: 0x0012E160
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x0021465D File Offset: 0x0021365D
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.StreamManager.Write(buffer, offset, count);
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x0021466D File Offset: 0x0021366D
		internal void UpdateReaderLength(long position)
		{
			this.StreamManager.UpdateReaderLength(position);
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x060040A8 RID: 16552 RVA: 0x0021467B File Offset: 0x0021367B
		private ReadWriteStreamManager StreamManager
		{
			get
			{
				return this._streamManager;
			}
		}

		// Token: 0x0400243D RID: 9277
		private ReadWriteStreamManager _streamManager;
	}
}
