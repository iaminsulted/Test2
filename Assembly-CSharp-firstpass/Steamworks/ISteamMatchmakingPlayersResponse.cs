using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B8 RID: 440
	public class ISteamMatchmakingPlayersResponse
	{
		// Token: 0x06000D49 RID: 3401 RVA: 0x0002A4C8 File Offset: 0x000286C8
		public ISteamMatchmakingPlayersResponse(ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList, ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond, ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_AddPlayerToList = onAddPlayerToList;
			this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
			this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
			this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable
			{
				m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
				m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
				m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPlayersResponse.VTable)));
			Marshal.StructureToPtr<ISteamMatchmakingPlayersResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0002A584 File Offset: 0x00028784
		~ISteamMatchmakingPlayersResponse()
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

		// Token: 0x06000D4B RID: 3403 RVA: 0x0002A5E0 File Offset: 0x000287E0
		private void InternalOnAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed)
		{
			this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0002A5F5 File Offset: 0x000287F5
		private void InternalOnPlayersFailedToRespond()
		{
			this.m_PlayersFailedToRespond();
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0002A602 File Offset: 0x00028802
		private void InternalOnPlayersRefreshComplete()
		{
			this.m_PlayersRefreshComplete();
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0002A60F File Offset: 0x0002880F
		public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000A4C RID: 2636
		private ISteamMatchmakingPlayersResponse.VTable m_VTable;

		// Token: 0x04000A4D RID: 2637
		private IntPtr m_pVTable;

		// Token: 0x04000A4E RID: 2638
		private GCHandle m_pGCHandle;

		// Token: 0x04000A4F RID: 2639
		private ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;

		// Token: 0x04000A50 RID: 2640
		private ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;

		// Token: 0x04000A51 RID: 2641
		private ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

		// Token: 0x020002CB RID: 715
		// (Invoke) Token: 0x060012CC RID: 4812
		public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

		// Token: 0x020002CC RID: 716
		// (Invoke) Token: 0x060012D0 RID: 4816
		public delegate void PlayersFailedToRespond();

		// Token: 0x020002CD RID: 717
		// (Invoke) Token: 0x060012D4 RID: 4820
		public delegate void PlayersRefreshComplete();

		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x060012D8 RID: 4824
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalAddPlayerToList(IntPtr pchName, int nScore, float flTimePlayed);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x060012DC RID: 4828
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalPlayersFailedToRespond();

		// Token: 0x020002D0 RID: 720
		// (Invoke) Token: 0x060012E0 RID: 4832
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalPlayersRefreshComplete();

		// Token: 0x020002D1 RID: 721
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000DC9 RID: 3529
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;

			// Token: 0x04000DCA RID: 3530
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			// Token: 0x04000DCB RID: 3531
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}
	}
}
