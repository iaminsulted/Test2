using System;

namespace Steamworks
{
	// Token: 0x020001E2 RID: 482
	[Serializable]
	public struct UGCUpdateHandle_t : IEquatable<UGCUpdateHandle_t>, IComparable<UGCUpdateHandle_t>
	{
		// Token: 0x06000F0B RID: 3851 RVA: 0x0002C46B File Offset: 0x0002A66B
		public UGCUpdateHandle_t(ulong value)
		{
			this.m_UGCUpdateHandle = value;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0002C474 File Offset: 0x0002A674
		public override string ToString()
		{
			return this.m_UGCUpdateHandle.ToString();
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0002C481 File Offset: 0x0002A681
		public override bool Equals(object other)
		{
			return other is UGCUpdateHandle_t && this == (UGCUpdateHandle_t)other;
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0002C49E File Offset: 0x0002A69E
		public override int GetHashCode()
		{
			return this.m_UGCUpdateHandle.GetHashCode();
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0002C4AB File Offset: 0x0002A6AB
		public static bool operator ==(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return x.m_UGCUpdateHandle == y.m_UGCUpdateHandle;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x0002C4BB File Offset: 0x0002A6BB
		public static bool operator !=(UGCUpdateHandle_t x, UGCUpdateHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0002C4C7 File Offset: 0x0002A6C7
		public static explicit operator UGCUpdateHandle_t(ulong value)
		{
			return new UGCUpdateHandle_t(value);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x0002C4CF File Offset: 0x0002A6CF
		public static explicit operator ulong(UGCUpdateHandle_t that)
		{
			return that.m_UGCUpdateHandle;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x0002C4D7 File Offset: 0x0002A6D7
		public bool Equals(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle == other.m_UGCUpdateHandle;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0002C4E7 File Offset: 0x0002A6E7
		public int CompareTo(UGCUpdateHandle_t other)
		{
			return this.m_UGCUpdateHandle.CompareTo(other.m_UGCUpdateHandle);
		}

		// Token: 0x04000AAA RID: 2730
		public static readonly UGCUpdateHandle_t Invalid = new UGCUpdateHandle_t(ulong.MaxValue);

		// Token: 0x04000AAB RID: 2731
		public ulong m_UGCUpdateHandle;
	}
}
