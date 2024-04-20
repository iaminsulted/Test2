using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace WebSocketSharp.Net
{
	// Token: 0x02000019 RID: 25
	internal class ChunkStream
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x0000B60B File Offset: 0x0000980B
		public ChunkStream(WebHeaderCollection headers)
		{
			this._headers = headers;
			this._chunkSize = -1;
			this._chunks = new List<Chunk>();
			this._saved = new StringBuilder();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B639 File Offset: 0x00009839
		public ChunkStream(byte[] buffer, int offset, int count, WebHeaderCollection headers) : this(headers)
		{
			this.Write(buffer, offset, count);
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000B650 File Offset: 0x00009850
		internal WebHeaderCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000B668 File Offset: 0x00009868
		public int ChunkLeft
		{
			get
			{
				return this._chunkSize - this._chunkRead;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000B688 File Offset: 0x00009888
		public bool WantMore
		{
			get
			{
				return this._state != InputChunkState.End;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000B6A8 File Offset: 0x000098A8
		private int read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			int count2 = this._chunks.Count;
			for (int i = 0; i < count2; i++)
			{
				Chunk chunk = this._chunks[i];
				bool flag = chunk == null;
				if (!flag)
				{
					bool flag2 = chunk.ReadLeft == 0;
					if (flag2)
					{
						this._chunks[i] = null;
					}
					else
					{
						num += chunk.Read(buffer, offset + num, count - num);
						bool flag3 = num == count;
						if (flag3)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000B734 File Offset: 0x00009934
		private static string removeChunkExtension(string value)
		{
			int num = value.IndexOf(';');
			return (num > -1) ? value.Substring(0, num) : value;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B760 File Offset: 0x00009960
		private InputChunkState seekCrLf(byte[] buffer, ref int offset, int length)
		{
			bool flag = !this._sawCr;
			int num;
			if (flag)
			{
				num = offset;
				offset = num + 1;
				bool flag2 = buffer[num] != 13;
				if (flag2)
				{
					ChunkStream.throwProtocolViolation("CR is expected.");
				}
				this._sawCr = true;
				bool flag3 = offset == length;
				if (flag3)
				{
					return InputChunkState.DataEnded;
				}
			}
			num = offset;
			offset = num + 1;
			bool flag4 = buffer[num] != 10;
			if (flag4)
			{
				ChunkStream.throwProtocolViolation("LF is expected.");
			}
			return InputChunkState.None;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B7E0 File Offset: 0x000099E0
		private InputChunkState setChunkSize(byte[] buffer, ref int offset, int length)
		{
			byte b = 0;
			while (offset < length)
			{
				int num = offset;
				offset = num + 1;
				b = buffer[num];
				bool sawCr = this._sawCr;
				if (sawCr)
				{
					bool flag = b != 10;
					if (flag)
					{
						ChunkStream.throwProtocolViolation("LF is expected.");
					}
					break;
				}
				bool flag2 = b == 13;
				if (flag2)
				{
					this._sawCr = true;
				}
				else
				{
					bool flag3 = b == 10;
					if (flag3)
					{
						ChunkStream.throwProtocolViolation("LF is unexpected.");
					}
					bool flag4 = b == 32;
					if (flag4)
					{
						this._gotIt = true;
					}
					bool flag5 = !this._gotIt;
					if (flag5)
					{
						this._saved.Append((char)b);
					}
					bool flag6 = this._saved.Length > 20;
					if (flag6)
					{
						ChunkStream.throwProtocolViolation("The chunk size is too long.");
					}
				}
			}
			bool flag7 = !this._sawCr || b != 10;
			InputChunkState result;
			if (flag7)
			{
				result = InputChunkState.None;
			}
			else
			{
				this._chunkRead = 0;
				try
				{
					this._chunkSize = int.Parse(ChunkStream.removeChunkExtension(this._saved.ToString()), NumberStyles.HexNumber);
				}
				catch
				{
					ChunkStream.throwProtocolViolation("The chunk size cannot be parsed.");
				}
				bool flag8 = this._chunkSize == 0;
				if (flag8)
				{
					this._trailerState = 2;
					result = InputChunkState.Trailer;
				}
				else
				{
					result = InputChunkState.Data;
				}
			}
			return result;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000B940 File Offset: 0x00009B40
		private InputChunkState setTrailer(byte[] buffer, ref int offset, int length)
		{
			bool flag = this._trailerState == 2 && buffer[offset] == 13 && this._saved.Length == 0;
			if (flag)
			{
				offset++;
				bool flag2 = offset < length && buffer[offset] == 10;
				if (flag2)
				{
					offset++;
					return InputChunkState.End;
				}
				offset--;
			}
			while (offset < length && this._trailerState < 4)
			{
				int num = offset;
				offset = num + 1;
				byte b = buffer[num];
				this._saved.Append((char)b);
				bool flag3 = this._saved.Length > 4196;
				if (flag3)
				{
					ChunkStream.throwProtocolViolation("The trailer is too long.");
				}
				bool flag4 = this._trailerState == 1 || this._trailerState == 3;
				if (flag4)
				{
					bool flag5 = b != 10;
					if (flag5)
					{
						ChunkStream.throwProtocolViolation("LF is expected.");
					}
					this._trailerState++;
				}
				else
				{
					bool flag6 = b == 13;
					if (flag6)
					{
						this._trailerState++;
					}
					else
					{
						bool flag7 = b == 10;
						if (flag7)
						{
							ChunkStream.throwProtocolViolation("LF is unexpected.");
						}
						this._trailerState = 0;
					}
				}
			}
			bool flag8 = this._trailerState < 4;
			InputChunkState result;
			if (flag8)
			{
				result = InputChunkState.Trailer;
			}
			else
			{
				this._saved.Length -= 2;
				StringReader stringReader = new StringReader(this._saved.ToString());
				string text;
				while ((text = stringReader.ReadLine()) != null && text.Length > 0)
				{
					this._headers.Add(text);
				}
				result = InputChunkState.End;
			}
			return result;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000BAF3 File Offset: 0x00009CF3
		private static void throwProtocolViolation(string message)
		{
			throw new WebException(message, null, WebExceptionStatus.ServerProtocolViolation, null);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000BB00 File Offset: 0x00009D00
		private void write(byte[] buffer, ref int offset, int length)
		{
			bool flag = this._state == InputChunkState.End;
			if (flag)
			{
				ChunkStream.throwProtocolViolation("The chunks were ended.");
			}
			bool flag2 = this._state == InputChunkState.None;
			if (flag2)
			{
				this._state = this.setChunkSize(buffer, ref offset, length);
				bool flag3 = this._state == InputChunkState.None;
				if (flag3)
				{
					return;
				}
				this._saved.Length = 0;
				this._sawCr = false;
				this._gotIt = false;
			}
			bool flag4 = this._state == InputChunkState.Data && offset < length;
			if (flag4)
			{
				this._state = this.writeData(buffer, ref offset, length);
				bool flag5 = this._state == InputChunkState.Data;
				if (flag5)
				{
					return;
				}
			}
			bool flag6 = this._state == InputChunkState.DataEnded && offset < length;
			if (flag6)
			{
				this._state = this.seekCrLf(buffer, ref offset, length);
				bool flag7 = this._state == InputChunkState.DataEnded;
				if (flag7)
				{
					return;
				}
				this._sawCr = false;
			}
			bool flag8 = this._state == InputChunkState.Trailer && offset < length;
			if (flag8)
			{
				this._state = this.setTrailer(buffer, ref offset, length);
				bool flag9 = this._state == InputChunkState.Trailer;
				if (flag9)
				{
					return;
				}
				this._saved.Length = 0;
			}
			bool flag10 = offset < length;
			if (flag10)
			{
				this.write(buffer, ref offset, length);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000BC48 File Offset: 0x00009E48
		private InputChunkState writeData(byte[] buffer, ref int offset, int length)
		{
			int num = length - offset;
			int num2 = this._chunkSize - this._chunkRead;
			bool flag = num > num2;
			if (flag)
			{
				num = num2;
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(buffer, offset, array, 0, num);
			this._chunks.Add(new Chunk(array));
			offset += num;
			this._chunkRead += num;
			return (this._chunkRead == this._chunkSize) ? InputChunkState.DataEnded : InputChunkState.Data;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000BCC4 File Offset: 0x00009EC4
		internal void ResetBuffer()
		{
			this._chunkRead = 0;
			this._chunkSize = -1;
			this._chunks.Clear();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000BCE4 File Offset: 0x00009EE4
		internal int WriteAndReadBack(byte[] buffer, int offset, int writeCount, int readCount)
		{
			this.Write(buffer, offset, writeCount);
			return this.Read(buffer, offset, readCount);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public int Read(byte[] buffer, int offset, int count)
		{
			bool flag = count <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000BD38 File Offset: 0x00009F38
		public void Write(byte[] buffer, int offset, int count)
		{
			bool flag = count <= 0;
			if (!flag)
			{
				this.write(buffer, ref offset, offset + count);
			}
		}

		// Token: 0x0400009E RID: 158
		private int _chunkRead;

		// Token: 0x0400009F RID: 159
		private int _chunkSize;

		// Token: 0x040000A0 RID: 160
		private List<Chunk> _chunks;

		// Token: 0x040000A1 RID: 161
		private bool _gotIt;

		// Token: 0x040000A2 RID: 162
		private WebHeaderCollection _headers;

		// Token: 0x040000A3 RID: 163
		private StringBuilder _saved;

		// Token: 0x040000A4 RID: 164
		private bool _sawCr;

		// Token: 0x040000A5 RID: 165
		private InputChunkState _state;

		// Token: 0x040000A6 RID: 166
		private int _trailerState;
	}
}
