using System;

namespace Steamworks
{
	// Token: 0x020001DA RID: 474
	[Serializable]
	public struct UGCHandle_t : IEquatable<UGCHandle_t>, IComparable<UGCHandle_t>
	{
		// Token: 0x06000EB4 RID: 3764 RVA: 0x0002BF94 File Offset: 0x0002A194
		public UGCHandle_t(ulong value)
		{
			this.m_UGCHandle = value;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0002BF9D File Offset: 0x0002A19D
		public override string ToString()
		{
			return this.m_UGCHandle.ToString();
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0002BFAA File Offset: 0x0002A1AA
		public override bool Equals(object other)
		{
			return other is UGCHandle_t && this == (UGCHandle_t)other;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0002BFC7 File Offset: 0x0002A1C7
		public override int GetHashCode()
		{
			return this.m_UGCHandle.GetHashCode();
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0002BFD4 File Offset: 0x0002A1D4
		public static bool operator ==(UGCHandle_t x, UGCHandle_t y)
		{
			return x.m_UGCHandle == y.m_UGCHandle;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0002BFE4 File Offset: 0x0002A1E4
		public static bool operator !=(UGCHandle_t x, UGCHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public static explicit operator UGCHandle_t(ulong value)
		{
			return new UGCHandle_t(value);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		public static explicit operator ulong(UGCHandle_t that)
		{
			return that.m_UGCHandle;
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0002C000 File Offset: 0x0002A200
		public bool Equals(UGCHandle_t other)
		{
			return this.m_UGCHandle == other.m_UGCHandle;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0002C010 File Offset: 0x0002A210
		public int CompareTo(UGCHandle_t other)
		{
			return this.m_UGCHandle.CompareTo(other.m_UGCHandle);
		}

		// Token: 0x04000A9B RID: 2715
		public static readonly UGCHandle_t Invalid = new UGCHandle_t(ulong.MaxValue);

		// Token: 0x04000A9C RID: 2716
		public ulong m_UGCHandle;
	}
}
