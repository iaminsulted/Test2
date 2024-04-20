using System;

namespace Steamworks
{
	// Token: 0x020001CB RID: 459
	[Serializable]
	public struct ControllerHandle_t : IEquatable<ControllerHandle_t>, IComparable<ControllerHandle_t>
	{
		// Token: 0x06000E14 RID: 3604 RVA: 0x0002B6A9 File Offset: 0x000298A9
		public ControllerHandle_t(ulong value)
		{
			this.m_ControllerHandle = value;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x0002B6B2 File Offset: 0x000298B2
		public override string ToString()
		{
			return this.m_ControllerHandle.ToString();
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0002B6BF File Offset: 0x000298BF
		public override bool Equals(object other)
		{
			return other is ControllerHandle_t && this == (ControllerHandle_t)other;
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x0002B6DC File Offset: 0x000298DC
		public override int GetHashCode()
		{
			return this.m_ControllerHandle.GetHashCode();
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x0002B6E9 File Offset: 0x000298E9
		public static bool operator ==(ControllerHandle_t x, ControllerHandle_t y)
		{
			return x.m_ControllerHandle == y.m_ControllerHandle;
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x0002B6F9 File Offset: 0x000298F9
		public static bool operator !=(ControllerHandle_t x, ControllerHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0002B705 File Offset: 0x00029905
		public static explicit operator ControllerHandle_t(ulong value)
		{
			return new ControllerHandle_t(value);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0002B70D File Offset: 0x0002990D
		public static explicit operator ulong(ControllerHandle_t that)
		{
			return that.m_ControllerHandle;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0002B715 File Offset: 0x00029915
		public bool Equals(ControllerHandle_t other)
		{
			return this.m_ControllerHandle == other.m_ControllerHandle;
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x0002B725 File Offset: 0x00029925
		public int CompareTo(ControllerHandle_t other)
		{
			return this.m_ControllerHandle.CompareTo(other.m_ControllerHandle);
		}

		// Token: 0x04000A81 RID: 2689
		public ulong m_ControllerHandle;
	}
}
