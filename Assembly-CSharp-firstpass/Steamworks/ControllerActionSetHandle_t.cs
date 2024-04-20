using System;

namespace Steamworks
{
	// Token: 0x020001C8 RID: 456
	[Serializable]
	public struct ControllerActionSetHandle_t : IEquatable<ControllerActionSetHandle_t>, IComparable<ControllerActionSetHandle_t>
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0002B4FC File Offset: 0x000296FC
		public ControllerActionSetHandle_t(ulong value)
		{
			this.m_ControllerActionSetHandle = value;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0002B505 File Offset: 0x00029705
		public override string ToString()
		{
			return this.m_ControllerActionSetHandle.ToString();
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002B512 File Offset: 0x00029712
		public override bool Equals(object other)
		{
			return other is ControllerActionSetHandle_t && this == (ControllerActionSetHandle_t)other;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002B52F File Offset: 0x0002972F
		public override int GetHashCode()
		{
			return this.m_ControllerActionSetHandle.GetHashCode();
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002B53C File Offset: 0x0002973C
		public static bool operator ==(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
		{
			return x.m_ControllerActionSetHandle == y.m_ControllerActionSetHandle;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0002B54C File Offset: 0x0002974C
		public static bool operator !=(ControllerActionSetHandle_t x, ControllerActionSetHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002B558 File Offset: 0x00029758
		public static explicit operator ControllerActionSetHandle_t(ulong value)
		{
			return new ControllerActionSetHandle_t(value);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002B560 File Offset: 0x00029760
		public static explicit operator ulong(ControllerActionSetHandle_t that)
		{
			return that.m_ControllerActionSetHandle;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002B568 File Offset: 0x00029768
		public bool Equals(ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle == other.m_ControllerActionSetHandle;
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002B578 File Offset: 0x00029778
		public int CompareTo(ControllerActionSetHandle_t other)
		{
			return this.m_ControllerActionSetHandle.CompareTo(other.m_ControllerActionSetHandle);
		}

		// Token: 0x04000A7E RID: 2686
		public ulong m_ControllerActionSetHandle;
	}
}
