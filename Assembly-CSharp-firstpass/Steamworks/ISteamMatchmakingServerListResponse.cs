using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B6 RID: 438
	public class ISteamMatchmakingServerListResponse
	{
		// Token: 0x06000D3E RID: 3390 RVA: 0x0002A250 File Offset: 0x00028450
		public ISteamMatchmakingServerListResponse(ISteamMatchmakingServerListResponse.ServerResponded onServerResponded, ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond, ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
		{
			if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_RefreshComplete = onRefreshComplete;
			this.m_VTable = new ISteamMatchmakingServerListResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
				m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingServerListResponse.VTable)));
			Marshal.StructureToPtr<ISteamMatchmakingServerListResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0002A30C File Offset: 0x0002850C
		~ISteamMatchmakingServerListResponse()
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

		// Token: 0x06000D40 RID: 3392 RVA: 0x0002A368 File Offset: 0x00028568
		private void InternalOnServerResponded(HServerListRequest hRequest, int iServer)
		{
			this.m_ServerResponded(hRequest, iServer);
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0002A377 File Offset: 0x00028577
		private void InternalOnServerFailedToRespond(HServerListRequest hRequest, int iServer)
		{
			this.m_ServerFailedToRespond(hRequest, iServer);
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0002A386 File Offset: 0x00028586
		private void InternalOnRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			this.m_RefreshComplete(hRequest, response);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0002A395 File Offset: 0x00028595
		public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000A41 RID: 2625
		private ISteamMatchmakingServerListResponse.VTable m_VTable;

		// Token: 0x04000A42 RID: 2626
		private IntPtr m_pVTable;

		// Token: 0x04000A43 RID: 2627
		private GCHandle m_pGCHandle;

		// Token: 0x04000A44 RID: 2628
		private ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000A45 RID: 2629
		private ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x04000A46 RID: 2630
		private ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

		// Token: 0x020002BF RID: 703
		// (Invoke) Token: 0x060012A2 RID: 4770
		public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

		// Token: 0x020002C0 RID: 704
		// (Invoke) Token: 0x060012A6 RID: 4774
		public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

		// Token: 0x020002C1 RID: 705
		// (Invoke) Token: 0x060012AA RID: 4778
		public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020002C2 RID: 706
		// (Invoke) Token: 0x060012AE RID: 4782
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InternalServerResponded(HServerListRequest hRequest, int iServer);

		// Token: 0x020002C3 RID: 707
		// (Invoke) Token: 0x060012B2 RID: 4786
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InternalServerFailedToRespond(HServerListRequest hRequest, int iServer);

		// Token: 0x020002C4 RID: 708
		// (Invoke) Token: 0x060012B6 RID: 4790
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InternalRefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x020002C5 RID: 709
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000DC4 RID: 3524
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000DC5 RID: 3525
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;

			// Token: 0x04000DC6 RID: 3526
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
		}
	}
}
