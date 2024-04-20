using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AE RID: 430
	public sealed class CallResult<T> : IDisposable
	{
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000D1E RID: 3358 RVA: 0x00029C60 File Offset: 0x00027E60
		// (remove) Token: 0x06000D1F RID: 3359 RVA: 0x00029C98 File Offset: 0x00027E98
		private event CallResult<T>.APIDispatchDelegate m_Func;

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000D20 RID: 3360 RVA: 0x00029CCD File Offset: 0x00027ECD
		public SteamAPICall_t Handle
		{
			get
			{
				return this.m_hAPICall;
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00029CD5 File Offset: 0x00027ED5
		public static CallResult<T> Create(CallResult<T>.APIDispatchDelegate func = null)
		{
			return new CallResult<T>(func);
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00029CDD File Offset: 0x00027EDD
		public CallResult(CallResult<T>.APIDispatchDelegate func = null)
		{
			this.m_Func = func;
			this.BuildCCallbackBase();
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00029D20 File Offset: 0x00027F20
		~CallResult()
		{
			this.Dispose();
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00029D4C File Offset: 0x00027F4C
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Cancel();
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pCCallbackBase.IsAllocated)
			{
				this.m_pCCallbackBase.Free();
			}
			this.m_bDisposed = true;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00029DAC File Offset: 0x00027FAC
		public void Set(SteamAPICall_t hAPICall, CallResult<T>.APIDispatchDelegate func = null)
		{
			if (func != null)
			{
				this.m_Func = func;
			}
			if (this.m_Func == null)
			{
				throw new Exception("CallResult function was null, you must either set it in the CallResult Constructor or in Set()");
			}
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
			}
			this.m_hAPICall = hAPICall;
			if (hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_RegisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)hAPICall);
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00029E2D File Offset: 0x0002802D
		public bool IsActive()
		{
			return this.m_hAPICall != SteamAPICall_t.Invalid;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00029E3F File Offset: 0x0002803F
		public void Cancel()
		{
			if (this.m_hAPICall != SteamAPICall_t.Invalid)
			{
				NativeMethods.SteamAPI_UnregisterCallResult(this.m_pCCallbackBase.AddrOfPinnedObject(), (ulong)this.m_hAPICall);
				this.m_hAPICall = SteamAPICall_t.Invalid;
			}
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00029E79 File Offset: 0x00028079
		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00029E90 File Offset: 0x00028090
		private void OnRunCallback(IntPtr pvParam)
		{
			this.m_hAPICall = SteamAPICall_t.Invalid;
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), false);
			}
			catch (Exception e)
			{
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00029EE4 File Offset: 0x000280E4
		private void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall_)
		{
			if ((SteamAPICall_t)hSteamAPICall_ == this.m_hAPICall)
			{
				this.m_hAPICall = SteamAPICall_t.Invalid;
				try
				{
					this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))), bFailed);
				}
				catch (Exception e)
				{
					CallbackDispatcher.ExceptionHandler(e);
				}
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00029F4C File Offset: 0x0002814C
		private int OnGetCallbackSizeBytes()
		{
			return this.m_size;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00029F54 File Offset: 0x00028154
		private void BuildCCallbackBase()
		{
			this.VTable = new CCallbackBaseVTable
			{
				m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
				m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_GetCallbackSizeBytes = new CCallbackBaseVTable.GetCallbackSizeBytesDel(this.OnGetCallbackSizeBytes)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CCallbackBaseVTable)));
			Marshal.StructureToPtr<CCallbackBaseVTable>(this.VTable, this.m_pVTable, false);
			this.m_CCallbackBase = new CCallbackBase
			{
				m_vfptr = this.m_pVTable,
				m_nCallbackFlags = 0,
				m_iCallback = CallbackIdentities.GetCallbackIdentity(typeof(T))
			};
			this.m_pCCallbackBase = GCHandle.Alloc(this.m_CCallbackBase, GCHandleType.Pinned);
		}

		// Token: 0x04000A2D RID: 2605
		private CCallbackBaseVTable VTable;

		// Token: 0x04000A2E RID: 2606
		private IntPtr m_pVTable = IntPtr.Zero;

		// Token: 0x04000A2F RID: 2607
		private CCallbackBase m_CCallbackBase;

		// Token: 0x04000A30 RID: 2608
		private GCHandle m_pCCallbackBase;

		// Token: 0x04000A32 RID: 2610
		private SteamAPICall_t m_hAPICall = SteamAPICall_t.Invalid;

		// Token: 0x04000A33 RID: 2611
		private readonly int m_size = Marshal.SizeOf(typeof(T));

		// Token: 0x04000A34 RID: 2612
		private bool m_bDisposed;

		// Token: 0x020002B9 RID: 697
		// (Invoke) Token: 0x0600128D RID: 4749
		public delegate void APIDispatchDelegate(T param, bool bIOFailure);
	}
}
