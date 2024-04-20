using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B7 RID: 439
	public class ISteamMatchmakingPingResponse
	{
		// Token: 0x06000D44 RID: 3396 RVA: 0x0002A3A4 File Offset: 0x000285A4
		public ISteamMatchmakingPingResponse(ISteamMatchmakingPingResponse.ServerResponded onServerResponded, ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPingResponse.VTable)));
			Marshal.StructureToPtr<ISteamMatchmakingPingResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0002A444 File Offset: 0x00028644
		~ISteamMatchmakingPingResponse()
		{
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pGCHandle.IsAllocated)
			{
				this.m_pGCHandle.Free();
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0002A4A0 File Offset: 0x000286A0
		private void InternalOnServerResponded(gameserveritem_t server)
		{
			this.m_ServerResponded(server);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0002A4AE File Offset: 0x000286AE
		private void InternalOnServerFailedToRespond()
		{
			this.m_ServerFailedToRespond();
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0002A4BB File Offset: 0x000286BB
		public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000A47 RID: 2631
		private ISteamMatchmakingPingResponse.VTable m_VTable;

		// Token: 0x04000A48 RID: 2632
		private IntPtr m_pVTable;

		// Token: 0x04000A49 RID: 2633
		private GCHandle m_pGCHandle;

		// Token: 0x04000A4A RID: 2634
		private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000A4B RID: 2635
		private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x020002C6 RID: 710
		// (Invoke) Token: 0x060012BB RID: 4795
		public delegate void ServerResponded(gameserveritem_t server);

		// Token: 0x020002C7 RID: 711
		// (Invoke) Token: 0x060012BF RID: 4799
		public delegate void ServerFailedToRespond();

		// Token: 0x020002C8 RID: 712
		// (Invoke) Token: 0x060012C3 RID: 4803
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InternalServerResponded(gameserveritem_t server);

		// Token: 0x020002C9 RID: 713
		// (Invoke) Token: 0x060012C7 RID: 4807
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InternalServerFailedToRespond();

		// Token: 0x020002CA RID: 714
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000DC7 RID: 3527
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000DC8 RID: 3528
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}
	}
}
