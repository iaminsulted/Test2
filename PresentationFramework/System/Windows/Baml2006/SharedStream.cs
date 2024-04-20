using System;
using System.ComponentModel;
using System.IO;

namespace System.Windows.Baml2006
{
	// Token: 0x02000411 RID: 1041
	internal class SharedStream : Stream
	{
		// Token: 0x06002D35 RID: 11573 RVA: 0x001AB64E File Offset: 0x001AA64E
		public SharedStream(Stream baseStream)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.Initialize(baseStream, 0L, baseStream.Length);
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x001AB673 File Offset: 0x001AA673
		public SharedStream(Stream baseStream, long offset, long length)
		{
			if (baseStream == null)
			{
				throw new ArgumentNullException("baseStream");
			}
			this.Initialize(baseStream, offset, length);
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x001AB694 File Offset: 0x001AA694
		private void Initialize(Stream baseStream, long offset, long length)
		{
			if (!baseStream.CanSeek)
			{
				throw new ArgumentException("can’t seek on baseStream");
			}
			if (offset < 0L)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0L)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			SharedStream sharedStream = baseStream as SharedStream;
			if (sharedStream != null)
			{
				this._baseStream = sharedStream.BaseStream;
				this._offset = offset + sharedStream._offset;
				this._length = length;
				this._refCount = sharedStream._refCount;
				this._refCount.Value++;
				return;
			}
			this._baseStream = baseStream;
			this._offset = offset;
			this._length = length;
			this._refCount = new SharedStream.RefCount();
			this._refCount.Value++;
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x001AB752 File Offset: 0x001AA752
		public virtual int SharedCount
		{
			get
			{
				return this._refCount.Value;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002D39 RID: 11577 RVA: 0x001AB75F File Offset: 0x001AA75F
		public override bool CanRead
		{
			get
			{
				this.CheckDisposed();
				return this._baseStream.CanRead;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06002D3B RID: 11579 RVA: 0x00105F35 File Offset: 0x00104F35
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x001AB772 File Offset: 0x001AA772
		public override void Flush()
		{
			this.CheckDisposed();
			this._baseStream.Flush();
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06002D3D RID: 11581 RVA: 0x001AB785 File Offset: 0x001AA785
		public override long Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002D3E RID: 11582 RVA: 0x001AB78D File Offset: 0x001AA78D
		// (set) Token: 0x06002D3F RID: 11583 RVA: 0x001AB795 File Offset: 0x001AA795
		public override long Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (value < 0L || value >= this._length)
				{
					throw new ArgumentOutOfRangeException("value", value, string.Empty);
				}
				this._position = value;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002D40 RID: 11584 RVA: 0x001AB7C2 File Offset: 0x001AA7C2
		public virtual bool IsDisposed
		{
			get
			{
				return this._baseStream == null;
			}
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x001AB7D0 File Offset: 0x001AA7D0
		public override int ReadByte()
		{
			this.CheckDisposed();
			Math.Min(this._position + 1L, this._length);
			int result;
			if (this.Sync())
			{
				result = this._baseStream.ReadByte();
				this._position = this._baseStream.Position - this._offset;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x001AB82C File Offset: 0x001AA82C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.CheckDisposed();
			long num = Math.Min(this._position + (long)count, this._length);
			int result = 0;
			if (this.Sync())
			{
				result = this._baseStream.Read(buffer, offset, (int)(num - this._position));
				this._position = this._baseStream.Position - this._offset;
			}
			return result;
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x0012F160 File Offset: 0x0012E160
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x001AB8C4 File Offset: 0x001AA8C4
		public override long Seek(long offset, SeekOrigin origin)
		{
			long num;
			switch (origin)
			{
			case SeekOrigin.Begin:
				num = offset;
				break;
			case SeekOrigin.Current:
				num = this._position + offset;
				break;
			case SeekOrigin.End:
				num = this._length + offset;
				break;
			default:
				throw new InvalidEnumArgumentException("origin", (int)origin, typeof(SeekOrigin));
			}
			if (num < 0L || num >= this._length)
			{
				throw new ArgumentOutOfRangeException("offset", offset, string.Empty);
			}
			this.CheckDisposed();
			this._position = num;
			return this._position;
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x0012F160 File Offset: 0x0012E160
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x001AB94C File Offset: 0x001AA94C
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._baseStream != null)
			{
				this._refCount.Value--;
				if (this._refCount.Value < 1)
				{
					this._baseStream.Close();
				}
				this._refCount = null;
				this._baseStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x001AB9A5 File Offset: 0x001AA9A5
		public Stream BaseStream
		{
			get
			{
				return this._baseStream;
			}
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x001AB9AD File Offset: 0x001AA9AD
		private void CheckDisposed()
		{
			if (this.IsDisposed)
			{
				throw new ObjectDisposedException("BaseStream");
			}
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x001AB9C4 File Offset: 0x001AA9C4
		private bool Sync()
		{
			if (this._position >= 0L && this._position < this._length)
			{
				if (this._position + this._offset != this._baseStream.Position)
				{
					this._baseStream.Seek(this._offset + this._position, SeekOrigin.Begin);
				}
				return true;
			}
			return false;
		}

		// Token: 0x04001BAF RID: 7087
		private Stream _baseStream;

		// Token: 0x04001BB0 RID: 7088
		private long _offset;

		// Token: 0x04001BB1 RID: 7089
		private long _length;

		// Token: 0x04001BB2 RID: 7090
		private long _position;

		// Token: 0x04001BB3 RID: 7091
		private SharedStream.RefCount _refCount;

		// Token: 0x02000AB1 RID: 2737
		private class RefCount
		{
			// Token: 0x040042CB RID: 17099
			public int Value;
		}
	}
}
