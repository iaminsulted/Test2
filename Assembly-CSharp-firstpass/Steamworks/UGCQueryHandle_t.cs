using System;

namespace Steamworks
{
	// Token: 0x020001E1 RID: 481
	[Serializable]
	public struct UGCQueryHandle_t : IEquatable<UGCQueryHandle_t>, IComparable<UGCQueryHandle_t>
	{
		// Token: 0x06000F00 RID: 3840 RVA: 0x0002C3CE File Offset: 0x0002A5CE
		public UGCQueryHandle_t(ulong value)
		{
			this.m_UGCQueryHandle = value;
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0002C3D7 File Offset: 0x0002A5D7
		public override string ToString()
		{
			return this.m_UGCQueryHandle.ToString();
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0002C3E4 File Offset: 0x0002A5E4
		public override bool Equals(object other)
		{
			return other is UGCQueryHandle_t && this == (UGCQueryHandle_t)other;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0002C401 File Offset: 0x0002A601
		public override int GetHashCode()
		{
			return this.m_UGCQueryHandle.GetHashCode();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0002C40E File Offset: 0x0002A60E
		public static bool operator ==(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return x.m_UGCQueryHandle == y.m_UGCQueryHandle;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0002C41E File Offset: 0x0002A61E
		public static bool operator !=(UGCQueryHandle_t x, UGCQueryHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0002C42A File Offset: 0x0002A62A
		public static explicit operator UGCQueryHandle_t(ulong value)
		{
			return new UGCQueryHandle_t(value);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0002C432 File Offset: 0x0002A632
		public static explicit operator ulong(UGCQueryHandle_t that)
		{
			return that.m_UGCQueryHandle;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0002C43A File Offset: 0x0002A63A
		public bool Equals(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle == other.m_UGCQueryHandle;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0002C44A File Offset: 0x0002A64A
		public int CompareTo(UGCQueryHandle_t other)
		{
			return this.m_UGCQueryHandle.CompareTo(other.m_UGCQueryHandle);
		}

		// Token: 0x04000AA8 RID: 2728
		public static readonly UGCQueryHandle_t Invalid = new UGCQueryHandle_t(ulong.MaxValue);

		// Token: 0x04000AA9 RID: 2729
		public ulong m_UGCQueryHandle;
	}
}
