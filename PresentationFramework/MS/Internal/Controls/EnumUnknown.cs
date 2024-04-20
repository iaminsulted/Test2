using System;
using System.Runtime.InteropServices;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000255 RID: 597
	internal class EnumUnknown : UnsafeNativeMethods.IEnumUnknown
	{
		// Token: 0x06001712 RID: 5906 RVA: 0x0015CC23 File Offset: 0x0015BC23
		internal EnumUnknown(object[] arr)
		{
			this.arr = arr;
			this.loc = 0;
			this.size = ((arr == null) ? 0 : arr.Length);
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x0015CC48 File Offset: 0x0015BC48
		private EnumUnknown(object[] arr, int loc) : this(arr)
		{
			this.loc = loc;
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x0015CC58 File Offset: 0x0015BC58
		int UnsafeNativeMethods.IEnumUnknown.Next(int celt, IntPtr rgelt, IntPtr pceltFetched)
		{
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, 0);
			}
			if (celt < 0)
			{
				return -2147024809;
			}
			int num = 0;
			if (this.loc >= this.size)
			{
				num = 0;
			}
			else
			{
				while (this.loc < this.size && num < celt)
				{
					if (this.arr[this.loc] != null)
					{
						Marshal.WriteIntPtr(rgelt, Marshal.GetIUnknownForObject(this.arr[this.loc]));
						rgelt = (IntPtr)((long)rgelt + (long)sizeof(IntPtr));
						num++;
					}
					this.loc++;
				}
			}
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, num);
			}
			if (num != celt)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0015CD14 File Offset: 0x0015BD14
		int UnsafeNativeMethods.IEnumUnknown.Skip(int celt)
		{
			this.loc += celt;
			if (this.loc >= this.size)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x0015CD35 File Offset: 0x0015BD35
		void UnsafeNativeMethods.IEnumUnknown.Reset()
		{
			this.loc = 0;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x0015CD3E File Offset: 0x0015BD3E
		void UnsafeNativeMethods.IEnumUnknown.Clone(out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = new EnumUnknown(this.arr, this.loc);
		}

		// Token: 0x04000C89 RID: 3209
		private object[] arr;

		// Token: 0x04000C8A RID: 3210
		private int loc;

		// Token: 0x04000C8B RID: 3211
		private int size;
	}
}
