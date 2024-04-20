using System;

namespace IngameDebugConsole
{
	// Token: 0x020001EC RID: 492
	public class DebugLogIndexList
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0002DC77 File Offset: 0x0002BE77
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170000FB RID: 251
		public int this[int index]
		{
			get
			{
				return this.indices[index];
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0002DC89 File Offset: 0x0002BE89
		public DebugLogIndexList()
		{
			this.indices = new int[64];
			this.size = 0;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0002DCA8 File Offset: 0x0002BEA8
		public void Add(int index)
		{
			if (this.size == this.indices.Length)
			{
				int[] destinationArray = new int[this.size * 2];
				Array.Copy(this.indices, 0, destinationArray, 0, this.size);
				this.indices = destinationArray;
			}
			int[] array = this.indices;
			int num = this.size;
			this.size = num + 1;
			array[num] = index;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0002DD07 File Offset: 0x0002BF07
		public void Clear()
		{
			this.size = 0;
		}

		// Token: 0x04000AC8 RID: 2760
		private int[] indices;

		// Token: 0x04000AC9 RID: 2761
		private int size;
	}
}
