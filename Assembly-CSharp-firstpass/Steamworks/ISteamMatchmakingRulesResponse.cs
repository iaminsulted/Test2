using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001B9 RID: 441
	public class ISteamMatchmakingRulesResponse
	{
		// Token: 0x06000D4F RID: 3407 RVA: 0x0002A61C File Offset: 0x0002881C
		public ISteamMatchmakingRulesResponse(ISteamMatchmakingRulesResponse.RulesResponded onRulesResponded, ISteamMatchmakingRulesResponse.RulesFailedToRespond onRulesFailedToRespond, ISteamMatchmakingRulesResponse.RulesRefreshComplete onRulesRefreshComplete)
		{
			if (onRulesResponded == null || onRulesFailedToRespond == null || onRulesRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_RulesResponded = onRulesResponded;
			this.m_RulesFailedToRespond = onRulesFailedToRespond;
			this.m_RulesRefreshComplete = onRulesRefreshComplete;
			this.m_VTable = new ISteamMatchmakingRulesResponse.VTable
			{
				m_VTRulesResponded = new ISteamMatchmakingRulesResponse.InternalRulesResponded(this.InternalOnRulesResponded),
				m_VTRulesFailedToRespond = new ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond(this.InternalOnRulesFailedToRespond),
				m_VTRulesRefreshComplete = new ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete(this.InternalOnRulesRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingRulesResponse.VTable)));
			Marshal.StructureToPtr<ISteamMatchmakingRulesResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0002A6D8 File Offset: 0x000288D8
		~ISteamMatchmakingRulesResponse()
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

		// Token: 0x06000D51 RID: 3409 RVA: 0x0002A734 File Offset: 0x00028934
		private void InternalOnRulesResponded(IntPtr pchRule, IntPtr pchValue)
		{
			this.m_RulesResponded(InteropHelp.PtrToStringUTF8(pchRule), InteropHelp.PtrToStringUTF8(pchValue));
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x0002A74D File Offset: 0x0002894D
		private void InternalOnRulesFailedToRespond()
		{
			this.m_RulesFailedToRespond();
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x0002A75A File Offset: 0x0002895A
		private void InternalOnRulesRefreshComplete()
		{
			this.m_RulesRefreshComplete();
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x0002A767 File Offset: 0x00028967
		public static explicit operator IntPtr(ISteamMatchmakingRulesResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000A52 RID: 2642
		private ISteamMatchmakingRulesResponse.VTable m_VTable;

		// Token: 0x04000A53 RID: 2643
		private IntPtr m_pVTable;

		// Token: 0x04000A54 RID: 2644
		private GCHandle m_pGCHandle;

		// Token: 0x04000A55 RID: 2645
		private ISteamMatchmakingRulesResponse.RulesResponded m_RulesResponded;

		// Token: 0x04000A56 RID: 2646
		private ISteamMatchmakingRulesResponse.RulesFailedToRespond m_RulesFailedToRespond;

		// Token: 0x04000A57 RID: 2647
		private ISteamMatchmakingRulesResponse.RulesRefreshComplete m_RulesRefreshComplete;

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x060012E5 RID: 4837
		public delegate void RulesResponded(string pchRule, string pchValue);

		// Token: 0x020002D3 RID: 723
		// (Invoke) Token: 0x060012E9 RID: 4841
		public delegate void RulesFailedToRespond();

		// Token: 0x020002D4 RID: 724
		// (Invoke) Token: 0x060012ED RID: 4845
		public delegate void RulesRefreshComplete();

		// Token: 0x020002D5 RID: 725
		// (Invoke) Token: 0x060012F1 RID: 4849
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalRulesResponded(IntPtr pchRule, IntPtr pchValue);

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x060012F5 RID: 4853
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalRulesFailedToRespond();

		// Token: 0x020002D7 RID: 727
		// (Invoke) Token: 0x060012F9 RID: 4857
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate void InternalRulesRefreshComplete();

		// Token: 0x020002D8 RID: 728
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000DCC RID: 3532
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesResponded m_VTRulesResponded;

			// Token: 0x04000DCD RID: 3533
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesFailedToRespond m_VTRulesFailedToRespond;

			// Token: 0x04000DCE RID: 3534
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingRulesResponse.InternalRulesRefreshComplete m_VTRulesRefreshComplete;
		}
	}
}
