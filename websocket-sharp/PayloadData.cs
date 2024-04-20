using System;
using System.Collections;
using System.Collections.Generic;

namespace WebSocketSharp
{
	// Token: 0x0200000C RID: 12
	internal class PayloadData : IEnumerable<byte>, IEnumerable
	{
		// Token: 0x06000117 RID: 279 RVA: 0x000090E8 File Offset: 0x000072E8
		internal PayloadData()
		{
			this._code = 1005;
			this._reason = string.Empty;
			this._data = WebSocket.EmptyBytes;
			this._codeSet = true;
			this._reasonSet = true;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00009121 File Offset: 0x00007321
		internal PayloadData(byte[] data) : this(data, (long)data.Length)
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000912F File Offset: 0x0000732F
		internal PayloadData(byte[] data, long length)
		{
			this._data = data;
			this._length = length;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00009148 File Offset: 0x00007348
		internal PayloadData(ushort code, string reason)
		{
			this._code = code;
			this._reason = (reason ?? string.Empty);
			this._data = code.Append(reason);
			this._length = (long)this._data.Length;
			this._codeSet = true;
			this._reasonSet = true;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000091A0 File Offset: 0x000073A0
		internal ushort Code
		{
			get
			{
				bool flag = !this._codeSet;
				if (flag)
				{
					this._code = ((this._length > 1L) ? this._data.SubArray(0, 2).ToUInt16(ByteOrder.Big) : 1005);
					this._codeSet = true;
				}
				return this._code;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000091F8 File Offset: 0x000073F8
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00009210 File Offset: 0x00007410
		internal long ExtensionDataLength
		{
			get
			{
				return this._extDataLength;
			}
			set
			{
				this._extDataLength = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600011E RID: 286 RVA: 0x0000921C File Offset: 0x0000741C
		internal bool HasReservedCode
		{
			get
			{
				return this._length > 1L && this.Code.IsReserved();
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00009248 File Offset: 0x00007448
		internal string Reason
		{
			get
			{
				bool flag = !this._reasonSet;
				if (flag)
				{
					this._reason = ((this._length > 2L) ? this._data.SubArray(2L, this._length - 2L).UTF8Decode() : string.Empty);
					this._reasonSet = true;
				}
				return this._reason;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000120 RID: 288 RVA: 0x000092A8 File Offset: 0x000074A8
		public byte[] ApplicationData
		{
			get
			{
				return (this._extDataLength > 0L) ? this._data.SubArray(this._extDataLength, this._length - this._extDataLength) : this._data;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000121 RID: 289 RVA: 0x000092EC File Offset: 0x000074EC
		public byte[] ExtensionData
		{
			get
			{
				return (this._extDataLength > 0L) ? this._data.SubArray(0L, this._extDataLength) : WebSocket.EmptyBytes;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009324 File Offset: 0x00007524
		public ulong Length
		{
			get
			{
				return (ulong)this._length;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000933C File Offset: 0x0000753C
		internal void Mask(byte[] key)
		{
			for (long num = 0L; num < this._length; num += 1L)
			{
				checked
				{
					this._data[(int)((IntPtr)num)] = (this._data[(int)((IntPtr)num)] ^ key[(int)((IntPtr)(num % 4L))]);
				}
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000937C File Offset: 0x0000757C
		public IEnumerator<byte> GetEnumerator()
		{
			foreach (byte b in this._data)
			{
				yield return b;
			}
			byte[] array = null;
			yield break;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000938C File Offset: 0x0000758C
		public byte[] ToArray()
		{
			return this._data;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000093A4 File Offset: 0x000075A4
		public override string ToString()
		{
			return BitConverter.ToString(this._data);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000093C4 File Offset: 0x000075C4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000060 RID: 96
		private ushort _code;

		// Token: 0x04000061 RID: 97
		private bool _codeSet;

		// Token: 0x04000062 RID: 98
		private byte[] _data;

		// Token: 0x04000063 RID: 99
		private long _extDataLength;

		// Token: 0x04000064 RID: 100
		private long _length;

		// Token: 0x04000065 RID: 101
		private string _reason;

		// Token: 0x04000066 RID: 102
		private bool _reasonSet;

		// Token: 0x04000067 RID: 103
		public static readonly PayloadData Empty = new PayloadData();

		// Token: 0x04000068 RID: 104
		public static readonly ulong MaxLength = 9223372036854775807UL;
	}
}
