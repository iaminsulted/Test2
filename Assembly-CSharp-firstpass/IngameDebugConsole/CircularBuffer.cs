using System;

namespace IngameDebugConsole
{
	// Token: 0x020001E6 RID: 486
	public class CircularBuffer<T>
	{
		// Token: 0x170000F6 RID: 246
		public T this[int index]
		{
			get
			{
				return this.arr[(this.index + index) % this.arr.Length];
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0002C6E1 File Offset: 0x0002A8E1
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0002C6E9 File Offset: 0x0002A8E9
		public int Count { get; private set; }

		// Token: 0x06000F38 RID: 3896 RVA: 0x0002C6F2 File Offset: 0x0002A8F2
		public CircularBuffer(int capacity)
		{
			this.arr = new T[capacity];
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0002C708 File Offset: 0x0002A908
		public void Add(T value)
		{
			int num;
			if (this.Count < this.arr.Length)
			{
				T[] array = this.arr;
				num = this.Count;
				this.Count = num + 1;
				array[num] = value;
				return;
			}
			this.arr[this.index] = value;
			num = this.index + 1;
			this.index = num;
			if (num >= this.arr.Length)
			{
				this.index = 0;
			}
		}

		// Token: 0x04000AB0 RID: 2736
		private T[] arr;

		// Token: 0x04000AB1 RID: 2737
		private int index;
	}
}
