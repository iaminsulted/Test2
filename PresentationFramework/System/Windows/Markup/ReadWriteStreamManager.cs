using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace System.Windows.Markup
{
	// Token: 0x02000512 RID: 1298
	internal class ReadWriteStreamManager
	{
		// Token: 0x0600407B RID: 16507 RVA: 0x00214148 File Offset: 0x00213148
		internal ReadWriteStreamManager()
		{
			this.ReaderFirstBufferPosition = 0L;
			this.WriterFirstBufferPosition = 0L;
			this.ReaderBufferArrayList = new ArrayList();
			this.WriterBufferArrayList = new ArrayList();
			this._writerStream = new WriterStream(this);
			this._readerStream = new ReaderStream(this);
			this._bufferLock = new ReaderWriterLock();
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x002141A4 File Offset: 0x002131A4
		internal void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			int num;
			int num2;
			byte[] bufferFromFilePosition = this.GetBufferFromFilePosition(this.WritePosition, false, out num, out num2);
			int num3 = this.BufferSize - num;
			int num4;
			int num5;
			if (count > num3)
			{
				num4 = num3;
				num5 = count - num3;
			}
			else
			{
				num5 = 0;
				num4 = count;
			}
			for (int i = 0; i < num4; i++)
			{
				bufferFromFilePosition[num++] = buffer[offset++];
			}
			this.WritePosition += (long)num4;
			if (this.WritePosition > this.WriteLength)
			{
				this.WriteLength = this.WritePosition;
			}
			if (num5 > 0)
			{
				this.Write(buffer, offset, num5);
			}
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x00214244 File Offset: 0x00213244
		internal long WriterSeek(long offset, SeekOrigin loc)
		{
			switch (loc)
			{
			case SeekOrigin.Begin:
				this.WritePosition = (long)((int)offset);
				break;
			case SeekOrigin.Current:
				this.WritePosition = (long)((int)(this.WritePosition + offset));
				break;
			case SeekOrigin.End:
				throw new NotSupportedException(SR.Get("ParserWriterNoSeekEnd"));
			default:
				throw new ArgumentException(SR.Get("ParserWriterUnknownOrigin"));
			}
			if (this.WritePosition > this.WriteLength || this.WritePosition < this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.WritePosition;
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x002142D0 File Offset: 0x002132D0
		internal void UpdateReaderLength(long position)
		{
			if (this.ReadLength > position)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.ReadLength = position;
			if (this.ReadLength > this.WriteLength)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.CheckIfCanRemoveFromArrayList(position, this.WriterBufferArrayList, ref this._writerFirstBufferPosition);
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal void WriterClose()
		{
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x00214324 File Offset: 0x00213324
		internal int Read(byte[] buffer, int offset, int count)
		{
			if ((long)count + this.ReadPosition > this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num;
			int num2;
			byte[] bufferFromFilePosition = this.GetBufferFromFilePosition(this.ReadPosition, true, out num, out num2);
			int num3 = this.BufferSize - num;
			int num4;
			int num5;
			if (count > num3)
			{
				num4 = num3;
				num5 = count - num3;
			}
			else
			{
				num5 = 0;
				num4 = count;
			}
			for (int i = 0; i < num4; i++)
			{
				buffer[offset++] = bufferFromFilePosition[num++];
			}
			this.ReadPosition += (long)num4;
			if (num5 > 0)
			{
				this.Read(buffer, offset, num5);
			}
			return count;
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x002143C4 File Offset: 0x002133C4
		internal int ReadByte()
		{
			byte[] array = new byte[1];
			this.Read(array, 0, 1);
			return (int)array[0];
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x002143E8 File Offset: 0x002133E8
		internal long ReaderSeek(long offset, SeekOrigin loc)
		{
			switch (loc)
			{
			case SeekOrigin.Begin:
				this.ReadPosition = (long)((int)offset);
				break;
			case SeekOrigin.Current:
				this.ReadPosition = (long)((int)(this.ReadPosition + offset));
				break;
			case SeekOrigin.End:
				throw new NotSupportedException(SR.Get("ParserWriterNoSeekEnd"));
			default:
				throw new ArgumentException(SR.Get("ParserWriterUnknownOrigin"));
			}
			if (this.ReadPosition < this.ReaderFirstBufferPosition || this.ReadPosition >= this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.ReadPosition;
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x00214473 File Offset: 0x00213473
		internal void ReaderDoneWithFileUpToPosition(long position)
		{
			this.CheckIfCanRemoveFromArrayList(position, this.ReaderBufferArrayList, ref this._readerFirstBufferPosition);
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x00214488 File Offset: 0x00213488
		private byte[] GetBufferFromFilePosition(long position, bool reader, out int bufferOffset, out int bufferIndex)
		{
			this._bufferLock.AcquireWriterLock(-1);
			ArrayList arrayList;
			long num;
			if (reader)
			{
				arrayList = this.ReaderBufferArrayList;
				num = this.ReaderFirstBufferPosition;
			}
			else
			{
				arrayList = this.WriterBufferArrayList;
				num = this.WriterFirstBufferPosition;
			}
			bufferIndex = (int)((position - num) / (long)this.BufferSize);
			bufferOffset = (int)(position - num - (long)(bufferIndex * this.BufferSize));
			byte[] array;
			if (arrayList.Count <= bufferIndex)
			{
				array = new byte[this.BufferSize];
				this.ReaderBufferArrayList.Add(array);
				this.WriterBufferArrayList.Add(array);
			}
			else
			{
				array = (arrayList[bufferIndex] as byte[]);
			}
			this._bufferLock.ReleaseWriterLock();
			return array;
		}

		// Token: 0x06004085 RID: 16517 RVA: 0x00214534 File Offset: 0x00213534
		private void CheckIfCanRemoveFromArrayList(long position, ArrayList arrayList, ref long firstBufferPosition)
		{
			int num = (int)((position - firstBufferPosition) / (long)this.BufferSize);
			if (num > 0)
			{
				int num2 = num;
				this._bufferLock.AcquireWriterLock(-1);
				firstBufferPosition += (long)(num2 * this.BufferSize);
				arrayList.RemoveRange(0, num);
				this._bufferLock.ReleaseWriterLock();
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x06004086 RID: 16518 RVA: 0x00214582 File Offset: 0x00213582
		internal WriterStream WriterStream
		{
			get
			{
				return this._writerStream;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x06004087 RID: 16519 RVA: 0x0021458A File Offset: 0x0021358A
		internal ReaderStream ReaderStream
		{
			get
			{
				return this._readerStream;
			}
		}

		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06004088 RID: 16520 RVA: 0x00214592 File Offset: 0x00213592
		// (set) Token: 0x06004089 RID: 16521 RVA: 0x0021459A File Offset: 0x0021359A
		internal long ReadPosition
		{
			get
			{
				return this._readPosition;
			}
			set
			{
				this._readPosition = value;
			}
		}

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x0600408A RID: 16522 RVA: 0x002145A3 File Offset: 0x002135A3
		// (set) Token: 0x0600408B RID: 16523 RVA: 0x002145AB File Offset: 0x002135AB
		internal long ReadLength
		{
			get
			{
				return this._readLength;
			}
			set
			{
				this._readLength = value;
			}
		}

		// Token: 0x17000E5F RID: 3679
		// (get) Token: 0x0600408C RID: 16524 RVA: 0x002145B4 File Offset: 0x002135B4
		// (set) Token: 0x0600408D RID: 16525 RVA: 0x002145BC File Offset: 0x002135BC
		internal long WritePosition
		{
			get
			{
				return this._writePosition;
			}
			set
			{
				this._writePosition = value;
			}
		}

		// Token: 0x17000E60 RID: 3680
		// (get) Token: 0x0600408E RID: 16526 RVA: 0x002145C5 File Offset: 0x002135C5
		// (set) Token: 0x0600408F RID: 16527 RVA: 0x002145CD File Offset: 0x002135CD
		internal long WriteLength
		{
			get
			{
				return this._writeLength;
			}
			set
			{
				this._writeLength = value;
			}
		}

		// Token: 0x17000E61 RID: 3681
		// (get) Token: 0x06004090 RID: 16528 RVA: 0x002145D6 File Offset: 0x002135D6
		private int BufferSize
		{
			get
			{
				return 4096;
			}
		}

		// Token: 0x17000E62 RID: 3682
		// (get) Token: 0x06004091 RID: 16529 RVA: 0x002145DD File Offset: 0x002135DD
		// (set) Token: 0x06004092 RID: 16530 RVA: 0x002145E5 File Offset: 0x002135E5
		private long ReaderFirstBufferPosition
		{
			get
			{
				return this._readerFirstBufferPosition;
			}
			set
			{
				this._readerFirstBufferPosition = value;
			}
		}

		// Token: 0x17000E63 RID: 3683
		// (get) Token: 0x06004093 RID: 16531 RVA: 0x002145EE File Offset: 0x002135EE
		// (set) Token: 0x06004094 RID: 16532 RVA: 0x002145F6 File Offset: 0x002135F6
		private long WriterFirstBufferPosition
		{
			get
			{
				return this._writerFirstBufferPosition;
			}
			set
			{
				this._writerFirstBufferPosition = value;
			}
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06004095 RID: 16533 RVA: 0x002145FF File Offset: 0x002135FF
		// (set) Token: 0x06004096 RID: 16534 RVA: 0x00214607 File Offset: 0x00213607
		private ArrayList ReaderBufferArrayList
		{
			get
			{
				return this._readerBufferArrayList;
			}
			set
			{
				this._readerBufferArrayList = value;
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x00214610 File Offset: 0x00213610
		// (set) Token: 0x06004098 RID: 16536 RVA: 0x00214618 File Offset: 0x00213618
		private ArrayList WriterBufferArrayList
		{
			get
			{
				return this._writerBufferArrayList;
			}
			set
			{
				this._writerBufferArrayList = value;
			}
		}

		// Token: 0x04002431 RID: 9265
		private long _readPosition;

		// Token: 0x04002432 RID: 9266
		private long _readLength;

		// Token: 0x04002433 RID: 9267
		private long _writePosition;

		// Token: 0x04002434 RID: 9268
		private long _writeLength;

		// Token: 0x04002435 RID: 9269
		private ReaderWriterLock _bufferLock;

		// Token: 0x04002436 RID: 9270
		private WriterStream _writerStream;

		// Token: 0x04002437 RID: 9271
		private ReaderStream _readerStream;

		// Token: 0x04002438 RID: 9272
		private long _readerFirstBufferPosition;

		// Token: 0x04002439 RID: 9273
		private long _writerFirstBufferPosition;

		// Token: 0x0400243A RID: 9274
		private ArrayList _readerBufferArrayList;

		// Token: 0x0400243B RID: 9275
		private ArrayList _writerBufferArrayList;

		// Token: 0x0400243C RID: 9276
		private const int _bufferSize = 4096;
	}
}
