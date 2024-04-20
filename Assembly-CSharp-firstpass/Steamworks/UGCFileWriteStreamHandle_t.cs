using System;

namespace Steamworks
{
	// Token: 0x020001D9 RID: 473
	[Serializable]
	public struct UGCFileWriteStreamHandle_t : IEquatable<UGCFileWriteStreamHandle_t>, IComparable<UGCFileWriteStreamHandle_t>
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x0002BEF7 File Offset: 0x0002A0F7
		public UGCFileWriteStreamHandle_t(ulong value)
		{
			this.m_UGCFileWriteStreamHandle = value;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0002BF00 File Offset: 0x0002A100
		public override string ToString()
		{
			return this.m_UGCFileWriteStreamHandle.ToString();
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002BF0D File Offset: 0x0002A10D
		public override bool Equals(object other)
		{
			return other is UGCFileWriteStreamHandle_t && this == (UGCFileWriteStreamHandle_t)other;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0002BF2A File Offset: 0x0002A12A
		public override int GetHashCode()
		{
			return this.m_UGCFileWriteStreamHandle.GetHashCode();
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0002BF37 File Offset: 0x0002A137
		public static bool operator ==(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return x.m_UGCFileWriteStreamHandle == y.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0002BF47 File Offset: 0x0002A147
		public static bool operator !=(UGCFileWriteStreamHandle_t x, UGCFileWriteStreamHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0002BF53 File Offset: 0x0002A153
		public static explicit operator UGCFileWriteStreamHandle_t(ulong value)
		{
			return new UGCFileWriteStreamHandle_t(value);
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0002BF5B File Offset: 0x0002A15B
		public static explicit operator ulong(UGCFileWriteStreamHandle_t that)
		{
			return that.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0002BF63 File Offset: 0x0002A163
		public bool Equals(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle == other.m_UGCFileWriteStreamHandle;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0002BF73 File Offset: 0x0002A173
		public int CompareTo(UGCFileWriteStreamHandle_t other)
		{
			return this.m_UGCFileWriteStreamHandle.CompareTo(other.m_UGCFileWriteStreamHandle);
		}

		// Token: 0x04000A99 RID: 2713
		public static readonly UGCFileWriteStreamHandle_t Invalid = new UGCFileWriteStreamHandle_t(ulong.MaxValue);

		// Token: 0x04000A9A RID: 2714
		public ulong m_UGCFileWriteStreamHandle;
	}
}
