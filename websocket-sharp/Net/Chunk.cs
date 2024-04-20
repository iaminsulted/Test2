using System;

namespace WebSocketSharp.Net
{
	// Token: 0x02000035 RID: 53
	internal class Chunk
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x000168A2 File Offset: 0x00014AA2
		public Chunk(byte[] data)
		{
			this._data = data;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x000168B4 File Offset: 0x00014AB4
		public int ReadLeft
		{
			get
			{
				return this._data.Length - this._offset;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000168D8 File Offset: 0x00014AD8
		public int Read(byte[] buffer, int offset, int count)
		{
			int num = this._data.Length - this._offset;
			bool flag = num == 0;
			int result;
			if (flag)
			{
				result = num;
			}
			else
			{
				bool flag2 = count > num;
				if (flag2)
				{
					count = num;
				}
				Buffer.BlockCopy(this._data, this._offset, buffer, offset, count);
				this._offset += count;
				result = count;
			}
			return result;
		}

		// Token: 0x0400018C RID: 396
		private byte[] _data;

		// Token: 0x0400018D RID: 397
		private int _offset;
	}
}
