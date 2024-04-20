using System;
using System.Runtime.InteropServices;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x020006DC RID: 1756
	internal class WinEventHandler : IDisposable
	{
		// Token: 0x06005C68 RID: 23656 RVA: 0x002865F0 File Offset: 0x002855F0
		internal WinEventHandler(int eventMin, int eventMax)
		{
			this._eventMin = eventMin;
			this._eventMax = eventMax;
			this._winEventProc.Value = new NativeMethods.WinEventProcDef(this.WinEventDefaultProc);
			this._gchThis = GCHandle.Alloc(this._winEventProc.Value);
			this._shutdownListener = new WinEventHandler.WinEventHandlerShutDownListener(this);
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x0028664C File Offset: 0x0028564C
		~WinEventHandler()
		{
			this.Clear();
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x00286678 File Offset: 0x00285678
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Clear();
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void WinEventProc(int eventId, IntPtr hwnd)
		{
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x00286686 File Offset: 0x00285686
		internal void Clear()
		{
			this.Stop();
			if (this._gchThis.IsAllocated)
			{
				this._gchThis.Free();
			}
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x002866A8 File Offset: 0x002856A8
		internal void Start()
		{
			if (this._gchThis.IsAllocated)
			{
				this._hHook.Value = UnsafeNativeMethods.SetWinEventHook(this._eventMin, this._eventMax, IntPtr.Zero, this._winEventProc.Value, 0U, 0U, 0);
				if (this._hHook.Value == IntPtr.Zero)
				{
					this.Stop();
				}
			}
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x00286710 File Offset: 0x00285710
		internal void Stop()
		{
			if (this._hHook.Value != IntPtr.Zero)
			{
				UnsafeNativeMethods.UnhookWinEvent(this._hHook.Value);
				this._hHook.Value = IntPtr.Zero;
			}
			if (this._shutdownListener != null)
			{
				this._shutdownListener.StopListening();
				this._shutdownListener = null;
			}
		}

		// Token: 0x06005C6F RID: 23663 RVA: 0x0028676F File Offset: 0x0028576F
		private void WinEventDefaultProc(int winEventHook, int eventId, IntPtr hwnd, int idObject, int idChild, int eventThread, int eventTime)
		{
			this.WinEventProc(eventId, hwnd);
		}

		// Token: 0x040030CA RID: 12490
		private int _eventMin;

		// Token: 0x040030CB RID: 12491
		private int _eventMax;

		// Token: 0x040030CC RID: 12492
		private SecurityCriticalDataForSet<IntPtr> _hHook;

		// Token: 0x040030CD RID: 12493
		private SecurityCriticalDataForSet<NativeMethods.WinEventProcDef> _winEventProc;

		// Token: 0x040030CE RID: 12494
		private GCHandle _gchThis;

		// Token: 0x040030CF RID: 12495
		private WinEventHandler.WinEventHandlerShutDownListener _shutdownListener;

		// Token: 0x02000B80 RID: 2944
		private sealed class WinEventHandlerShutDownListener : ShutDownListener
		{
			// Token: 0x06008E2E RID: 36398 RVA: 0x00340788 File Offset: 0x0033F788
			public WinEventHandlerShutDownListener(WinEventHandler target) : base(target, ShutDownEvents.DispatcherShutdown)
			{
			}

			// Token: 0x06008E2F RID: 36399 RVA: 0x00340792 File Offset: 0x0033F792
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				((WinEventHandler)target).Stop();
			}
		}
	}
}
