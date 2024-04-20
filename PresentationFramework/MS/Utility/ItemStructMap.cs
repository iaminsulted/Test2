using System;

namespace MS.Utility
{
	// Token: 0x020000E8 RID: 232
	internal struct ItemStructMap<T>
	{
		// Token: 0x06000426 RID: 1062 RVA: 0x000FF2A8 File Offset: 0x000FE2A8
		public int EnsureEntry(int key)
		{
			int num = this.Search(key);
			if (num < 0)
			{
				if (this.Entries == null)
				{
					this.Entries = new ItemStructMap<T>.Entry[4];
				}
				num = ~num;
				ItemStructMap<T>.Entry[] array = this.Entries;
				if (this.Count + 1 > this.Entries.Length)
				{
					array = new ItemStructMap<T>.Entry[this.Entries.Length * 2];
					Array.Copy(this.Entries, 0, array, 0, num);
				}
				Array.Copy(this.Entries, num, array, num + 1, this.Count - num);
				this.Entries = array;
				this.Entries[num] = ItemStructMap<T>.EmptyEntry;
				this.Entries[num].Key = key;
				this.Count++;
			}
			return num;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000FF364 File Offset: 0x000FE364
		public int Search(int key)
		{
			int num = int.MaxValue;
			int num2 = 0;
			if (this.Count > 4)
			{
				int i = 0;
				int num3 = this.Count - 1;
				while (i <= num3)
				{
					num2 = (num3 + i) / 2;
					num = this.Entries[num2].Key;
					if (key == num)
					{
						return num2;
					}
					if (key < num)
					{
						num3 = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.Count; j++)
				{
					num2 = j;
					num = this.Entries[num2].Key;
					if (key == num)
					{
						return num2;
					}
					if (key < num)
					{
						break;
					}
				}
			}
			if (key > num)
			{
				num2++;
			}
			return ~num2;
		}

		// Token: 0x04000610 RID: 1552
		private const int SearchTypeThreshold = 4;

		// Token: 0x04000611 RID: 1553
		public ItemStructMap<T>.Entry[] Entries;

		// Token: 0x04000612 RID: 1554
		public int Count;

		// Token: 0x04000613 RID: 1555
		private static ItemStructMap<T>.Entry EmptyEntry;

		// Token: 0x020008A7 RID: 2215
		public struct Entry
		{
			// Token: 0x04003C08 RID: 15368
			public int Key;

			// Token: 0x04003C09 RID: 15369
			public T Value;
		}
	}
}
