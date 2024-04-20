using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AD RID: 429
	public sealed class Callback<T> : IDisposable
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06000D10 RID: 3344 RVA: 0x00029930 File Offset: 0x00027B30
		// (remove) Token: 0x06000D11 RID: 3345 RVA: 0x00029968 File Offset: 0x00027B68
		private event Callback<T>.DispatchDelegate m_Func;

		// Token: 0x06000D12 RID: 3346 RVA: 0x0002999D File Offset: 0x00027B9D
		public static Callback<T> Create(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, false);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000299A6 File Offset: 0x00027BA6
		public static Callback<T> CreateGameServer(Callback<T>.DispatchDelegate func)
		{
			return new Callback<T>(func, true);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000299AF File Offset: 0x00027BAF
		public Callback(Callback<T>.DispatchDelegate func, bool bGameServer = false)
		{
			this.m_bGameServer = bGameServer;
			this.BuildCCallbackBase();
			this.Register(func);
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x000299EC File Offset: 0x00027BEC
		~Callback()
		{
			this.Dispose();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00029A18 File Offset: 0x00027C18
		public void Dispose()
		{
			if (this.m_bDisposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			this.Unregister();
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

		// Token: 0x06000D17 RID: 3351 RVA: 0x00029A78 File Offset: 0x00027C78
		public void Register(Callback<T>.DispatchDelegate func)
		{
			if (func == null)
			{
				throw new Exception("Callback function must not be null.");
			}
			if ((this.m_CCallbackBase.m_nCallbackFlags & 1) == 1)
			{
				this.Unregister();
			}
			if (this.m_bGameServer)
			{
				this.SetGameserverFlag();
			}
			this.m_Func = func;
			NativeMethods.SteamAPI_RegisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject(), CallbackIdentities.GetCallbackIdentity(typeof(T)));
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00029ADD File Offset: 0x00027CDD
		public void Unregister()
		{
			NativeMethods.SteamAPI_UnregisterCallback(this.m_pCCallbackBase.AddrOfPinnedObject());
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00029AEF File Offset: 0x00027CEF
		public void SetGameserverFlag()
		{
			CCallbackBase ccallbackBase = this.m_CCallbackBase;
			ccallbackBase.m_nCallbackFlags |= 2;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00029B08 File Offset: 0x00027D08
		private void OnRunCallback(IntPtr pvParam)
		{
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (Exception e)
			{
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00029B50 File Offset: 0x00027D50
		private void OnRunCallResult(IntPtr pvParam, bool bFailed, ulong hSteamAPICall)
		{
			try
			{
				this.m_Func((T)((object)Marshal.PtrToStructure(pvParam, typeof(T))));
			}
			catch (Exception e)
			{
				CallbackDispatcher.ExceptionHandler(e);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00029B98 File Offset: 0x00027D98
		private int OnGetCallbackSizeBytes()
		{
			return this.m_size;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00029BA0 File Offset: 0x00027DA0
		private void BuildCCallbackBase()
		{
			this.VTable = new CCallbackBaseVTable
			{
				m_RunCallResult = new CCallbackBaseVTable.RunCRDel(this.OnRunCallResult),
				m_RunCallback = new CCallbackBaseVTable.RunCBDel(this.OnRunCallback),
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

		// Token: 0x04000A25 RID: 2597
		private CCallbackBaseVTable VTable;

		// Token: 0x04000A26 RID: 2598
		private IntPtr m_pVTable = IntPtr.Zero;

		// Token: 0x04000A27 RID: 2599
		private CCallbackBase m_CCallbackBase;

		// Token: 0x04000A28 RID: 2600
		private GCHandle m_pCCallbackBase;

		// Token: 0x04000A2A RID: 2602
		private bool m_bGameServer;

		// Token: 0x04000A2B RID: 2603
		private readonly int m_size = Marshal.SizeOf(typeof(T));

		// Token: 0x04000A2C RID: 2604
		private bool m_bDisposed;

		// Token: 0x020002B8 RID: 696
		// (Invoke) Token: 0x06001289 RID: 4745
		public delegate void DispatchDelegate(T param);
	}
}
