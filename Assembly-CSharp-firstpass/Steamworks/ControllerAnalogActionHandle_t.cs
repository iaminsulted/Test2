using System;

namespace Steamworks
{
	// Token: 0x020001C9 RID: 457
	[Serializable]
	public struct ControllerAnalogActionHandle_t : IEquatable<ControllerAnalogActionHandle_t>, IComparable<ControllerAnalogActionHandle_t>
	{
		// Token: 0x06000E00 RID: 3584 RVA: 0x0002B58B File Offset: 0x0002978B
		public ControllerAnalogActionHandle_t(ulong value)
		{
			this.m_ControllerAnalogActionHandle = value;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0002B594 File Offset: 0x00029794
		public override string ToString()
		{
			return this.m_ControllerAnalogActionHandle.ToString();
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0002B5A1 File Offset: 0x000297A1
		public override bool Equals(object other)
		{
			return other is ControllerAnalogActionHandle_t && this == (ControllerAnalogActionHandle_t)other;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0002B5BE File Offset: 0x000297BE
		public override int GetHashCode()
		{
			return this.m_ControllerAnalogActionHandle.GetHashCode();
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0002B5CB File Offset: 0x000297CB
		public static bool operator ==(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
		{
			return x.m_ControllerAnalogActionHandle == y.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0002B5DB File Offset: 0x000297DB
		public static bool operator !=(ControllerAnalogActionHandle_t x, ControllerAnalogActionHandle_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0002B5E7 File Offset: 0x000297E7
		public static explicit operator ControllerAnalogActionHandle_t(ulong value)
		{
			return new ControllerAnalogActionHandle_t(value);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0002B5EF File Offset: 0x000297EF
		public static explicit operator ulong(ControllerAnalogActionHandle_t that)
		{
			return that.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0002B5F7 File Offset: 0x000297F7
		public bool Equals(ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle == other.m_ControllerAnalogActionHandle;
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x0002B607 File Offset: 0x00029807
		public int CompareTo(ControllerAnalogActionHandle_t other)
		{
			return this.m_ControllerAnalogActionHandle.CompareTo(other.m_ControllerAnalogActionHandle);
		}

		// Token: 0x04000A7F RID: 2687
		public ulong m_ControllerAnalogActionHandle;
	}
}
