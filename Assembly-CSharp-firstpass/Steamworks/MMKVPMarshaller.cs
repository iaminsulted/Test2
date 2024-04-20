using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B4 RID: 436
	public class MMKVPMarshaller
	{
		// Token: 0x06000D39 RID: 3385 RVA: 0x0002A144 File Offset: 0x00028344
		public MMKVPMarshaller(MatchMakingKeyValuePair_t[] filters)
		{
			if (filters == null)
			{
				return;
			}
			int num = Marshal.SizeOf(typeof(MatchMakingKeyValuePair_t));
			this.m_pNativeArray = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * filters.Length);
			this.m_pArrayEntries = Marshal.AllocHGlobal(num * filters.Length);
			for (int i = 0; i < filters.Length; i++)
			{
				Marshal.StructureToPtr<MatchMakingKeyValuePair_t>(filters[i], new IntPtr(this.m_pArrayEntries.ToInt64() + (long)(i * num)), false);
			}
			Marshal.WriteIntPtr(this.m_pNativeArray, this.m_pArrayEntries);
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x0002A1DC File Offset: 0x000283DC
		~MMKVPMarshaller()
		{
			if (this.m_pArrayEntries != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pArrayEntries);
			}
			if (this.m_pNativeArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pNativeArray);
			}
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0002A23C File Offset: 0x0002843C
		public static implicit operator IntPtr(MMKVPMarshaller that)
		{
			return that.m_pNativeArray;
		}

		// Token: 0x04000A3F RID: 2623
		private IntPtr m_pNativeArray;

		// Token: 0x04000A40 RID: 2624
		private IntPtr m_pArrayEntries;
	}
}
