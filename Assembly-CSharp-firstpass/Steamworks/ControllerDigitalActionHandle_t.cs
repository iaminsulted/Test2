using System;

namespace Steamworks
{
	// Token: 0x020001CA RID: 458
	[Serializable]
	public struct ControllerDigitalActionHandle_t : IEquatable<ControllerDigitalActionHandle_t>, IComparable<ControllerDigitalActionHandle_t>
	{
		// Token: 0x06000E0A RID: 3594 RVA: 0x0002B61A File Offset: 0x0002981A
		public ControllerDigitalActionHandle_t(ulong value)
		{
			this.m_ControllerDigitalActionHandle = value;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002B623 File Offset: 0x00029823
		public override string ToString()
		{
			return this.m_ControllerDigitalActionHandle.ToString();
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0002B630 File Offset: 0x00029830
		public override bool Equals(object other)
		{
			return other is ControllerDigitalActionHandle_t && this == (ControllerDigitalActionHandle_t)other;
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0002B64D File Offset: 0x0002984D
		public override int GetHashCode()
		{
			return this.m_ControllerDigitalActionHandle.GetHashCode();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0002B65A File Offset: 0x0002985A
		public static bool operator ==(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y)
		{
			return x.m_ControllerDigitalActionHandle == y.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0002B66A File Offset: 0x0002986A
		public static bool operator !=(ControllerDigitalActionHandle_t x, ControllerDigitalActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0002B676 File Offset: 0x00029876
		public static explicit operator ControllerDigitalActionHandle_t(ulong value)
		{
			return new ControllerDigitalActionHandle_t(value);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0002B67E File Offset: 0x0002987E
		public static explicit operator ulong(ControllerDigitalActionHandle_t that)
		{
			return that.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0002B686 File Offset: 0x00029886
		public bool Equals(ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle == other.m_ControllerDigitalActionHandle;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x0002B696 File Offset: 0x00029896
		public int CompareTo(ControllerDigitalActionHandle_t other)
		{
			return this.m_ControllerDigitalActionHandle.CompareTo(other.m_ControllerDigitalActionHandle);
		}

		// Token: 0x04000A80 RID: 2688
		public ulong m_ControllerDigitalActionHandle;
	}
}
