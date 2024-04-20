using System;

namespace Steamworks
{
	// Token: 0x020001DD RID: 477
	[Serializable]
	public struct AppId_t : IEquatable<AppId_t>, IComparable<AppId_t>
	{
		// Token: 0x06000ED4 RID: 3796 RVA: 0x0002C15C File Offset: 0x0002A35C
		public AppId_t(uint value)
		{
			this.m_AppId = value;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0002C165 File Offset: 0x0002A365
		public override string ToString()
		{
			return this.m_AppId.ToString();
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0002C172 File Offset: 0x0002A372
		public override bool Equals(object other)
		{
			return other is AppId_t && this == (AppId_t)other;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0002C18F File Offset: 0x0002A38F
		public override int GetHashCode()
		{
			return this.m_AppId.GetHashCode();
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x0002C19C File Offset: 0x0002A39C
		public static bool operator ==(AppId_t x, AppId_t y)
		{
			return x.m_AppId == y.m_AppId;
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x0002C1AC File Offset: 0x0002A3AC
		public static bool operator !=(AppId_t x, AppId_t y)
		{
			return !(x == y);
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
		public static explicit operator AppId_t(uint value)
		{
			return new AppId_t(value);
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002C1C0 File Offset: 0x0002A3C0
		public static explicit operator uint(AppId_t that)
		{
			return that.m_AppId;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0002C1C8 File Offset: 0x0002A3C8
		public bool Equals(AppId_t other)
		{
			return this.m_AppId == other.m_AppId;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002C1D8 File Offset: 0x0002A3D8
		public int CompareTo(AppId_t other)
		{
			return this.m_AppId.CompareTo(other.m_AppId);
		}

		// Token: 0x04000AA0 RID: 2720
		public static readonly AppId_t Invalid = new AppId_t(0U);

		// Token: 0x04000AA1 RID: 2721
		public uint m_AppId;
	}
}
