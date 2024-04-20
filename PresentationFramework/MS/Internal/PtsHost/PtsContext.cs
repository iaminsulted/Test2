using System;
using System.Collections;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000137 RID: 311
	internal sealed class PtsContext : DispatcherObject, IDisposable
	{
		// Token: 0x06000888 RID: 2184 RVA: 0x00116E6C File Offset: 0x00115E6C
		internal PtsContext(bool isOptimalParagraphEnabled, TextFormattingMode textFormattingMode)
		{
			this._pages = new ArrayList(1);
			this._pageBreakRecords = new ArrayList(1);
			this._unmanagedHandles = new PtsContext.HandleIndex[16];
			this._isOptimalParagraphEnabled = isOptimalParagraphEnabled;
			this.BuildFreeList(1);
			this._ptsHost = PtsCache.AcquireContext(this, textFormattingMode);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00116EC0 File Offset: 0x00115EC0
		public void Dispose()
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				try
				{
					this.Enter();
					for (int i = 0; i < this._pageBreakRecords.Count; i++)
					{
						Invariant.Assert((IntPtr)this._pageBreakRecords[i] != IntPtr.Zero, "Invalid break record object");
						PTS.Validate(PTS.FsDestroyPageBreakRecord(this._ptsHost.Context, (IntPtr)this._pageBreakRecords[i]));
					}
				}
				finally
				{
					this.Leave();
					this._pageBreakRecords = null;
				}
				try
				{
					this.Enter();
					for (int i = 0; i < this._pages.Count; i++)
					{
						Invariant.Assert((IntPtr)this._pages[i] != IntPtr.Zero, "Invalid break record object");
						PTS.Validate(PTS.FsDestroyPage(this._ptsHost.Context, (IntPtr)this._pages[i]));
					}
				}
				finally
				{
					this.Leave();
					this._pages = null;
				}
				if (Invariant.Strict && this._unmanagedHandles != null)
				{
					for (int i = 0; i < this._unmanagedHandles.Length; i++)
					{
						object obj = this._unmanagedHandles[i].Obj;
						if (obj != null)
						{
							Invariant.Assert(obj is BaseParagraph || obj is Section || obj is LineBreakRecord, "One of PTS Client objects is not properly disposed.");
						}
					}
				}
				this._ptsHost = null;
				this._unmanagedHandles = null;
				this._callbackException = null;
				this._disposeCompleted = true;
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00117064 File Offset: 0x00116064
		internal IntPtr CreateHandle(object obj)
		{
			Invariant.Assert(obj != null, "Cannot create handle for non-existing object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			if (this._unmanagedHandles[0].Index == 0L)
			{
				this.Resize();
			}
			long index = this._unmanagedHandles[0].Index;
			checked
			{
				this._unmanagedHandles[0].Index = this._unmanagedHandles[(int)((IntPtr)index)].Index;
				this._unmanagedHandles[(int)((IntPtr)index)].Obj = obj;
				this._unmanagedHandles[(int)((IntPtr)index)].Index = 0L;
				return (IntPtr)index;
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00117110 File Offset: 0x00116110
		internal void ReleaseHandle(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			Invariant.Assert(num > 0L && num < (long)this._unmanagedHandles.Length, "Invalid object handle.");
			checked
			{
				Invariant.Assert(this._unmanagedHandles[(int)((IntPtr)num)].IsHandle(), "Handle has been already released.");
				this._unmanagedHandles[(int)((IntPtr)num)].Obj = null;
				this._unmanagedHandles[(int)((IntPtr)num)].Index = this._unmanagedHandles[0].Index;
				this._unmanagedHandles[0].Index = num;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x001171BC File Offset: 0x001161BC
		internal bool IsValidHandle(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			return num >= 0L && num < (long)this._unmanagedHandles.Length && this._unmanagedHandles[(int)(checked((IntPtr)num))].IsHandle();
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00117208 File Offset: 0x00116208
		internal object HandleToObject(IntPtr handle)
		{
			long num = (long)handle;
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			Invariant.Assert(num > 0L && num < (long)this._unmanagedHandles.Length, "Invalid object handle.");
			checked
			{
				Invariant.Assert(this._unmanagedHandles[(int)((IntPtr)num)].IsHandle(), "Handle has been already released.");
				return this._unmanagedHandles[(int)((IntPtr)num)].Obj;
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0011727B File Offset: 0x0011627B
		internal void Enter()
		{
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			this._ptsHost.EnterContext(this);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0011729C File Offset: 0x0011629C
		internal void Leave()
		{
			Invariant.Assert(!this._disposeCompleted, "PtsContext is already disposed.");
			this._ptsHost.LeaveContext(this);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x001172C0 File Offset: 0x001162C0
		internal void OnPageCreated(SecurityCriticalDataForSet<IntPtr> ptsPage)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			Invariant.Assert(!this._pages.Contains(ptsPage.Value), "Page already exists.");
			this._pages.Add(ptsPage.Value);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00117338 File Offset: 0x00116338
		internal void OnPageDisposed(SecurityCriticalDataForSet<IntPtr> ptsPage, bool disposing, bool enterContext)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			if (disposing)
			{
				this.OnDestroyPage(ptsPage, enterContext);
				return;
			}
			if (!this.Disposed && !base.Dispatcher.HasShutdownStarted)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnDestroyPage), ptsPage);
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x001173A0 File Offset: 0x001163A0
		internal void OnPageBreakRecordCreated(SecurityCriticalDataForSet<IntPtr> br)
		{
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			Invariant.Assert(!this.Disposed, "PtsContext is already disposed.");
			Invariant.Assert(!this._pageBreakRecords.Contains(br.Value), "Break record already exists.");
			this._pageBreakRecords.Add(br.Value);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00117418 File Offset: 0x00116418
		internal void OnPageBreakRecordDisposed(SecurityCriticalDataForSet<IntPtr> br, bool disposing)
		{
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			if (disposing)
			{
				this.OnDestroyBreakRecord(br);
				return;
			}
			if (!this.Disposed && !base.Dispatcher.HasShutdownStarted)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnDestroyBreakRecord), br);
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00117485 File Offset: 0x00116485
		internal bool Disposed
		{
			get
			{
				return this._disposed != 0;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00117490 File Offset: 0x00116490
		internal IntPtr Context
		{
			get
			{
				return this._ptsHost.Context;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x0011749D File Offset: 0x0011649D
		internal bool IsOptimalParagraphEnabled
		{
			get
			{
				return this._isOptimalParagraphEnabled;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x001174A5 File Offset: 0x001164A5
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x001174AD File Offset: 0x001164AD
		internal TextFormatter TextFormatter
		{
			get
			{
				return this._textFormatter;
			}
			set
			{
				this._textFormatter = value;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x001174B6 File Offset: 0x001164B6
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x001174BE File Offset: 0x001164BE
		internal Exception CallbackException
		{
			get
			{
				return this._callbackException;
			}
			set
			{
				this._callbackException = value;
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x001174C8 File Offset: 0x001164C8
		private void BuildFreeList(int freeIndex)
		{
			this._unmanagedHandles[0].Index = (long)freeIndex;
			while (freeIndex < this._unmanagedHandles.Length)
			{
				this._unmanagedHandles[freeIndex].Index = (long)(++freeIndex);
			}
			this._unmanagedHandles[freeIndex - 1].Index = 0L;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00117524 File Offset: 0x00116524
		private void Resize()
		{
			int freeIndex = this._unmanagedHandles.Length;
			PtsContext.HandleIndex[] array = new PtsContext.HandleIndex[this._unmanagedHandles.Length * 2];
			Array.Copy(this._unmanagedHandles, array, this._unmanagedHandles.Length);
			this._unmanagedHandles = array;
			this.BuildFreeList(freeIndex);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0011756C File Offset: 0x0011656C
		private object OnDestroyPage(object args)
		{
			SecurityCriticalDataForSet<IntPtr> ptsPage = (SecurityCriticalDataForSet<IntPtr>)args;
			this.OnDestroyPage(ptsPage, true);
			return null;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0011758C File Offset: 0x0011658C
		private void OnDestroyPage(SecurityCriticalDataForSet<IntPtr> ptsPage, bool enterContext)
		{
			Invariant.Assert(ptsPage.Value != IntPtr.Zero, "Invalid page object.");
			if (!this.Disposed)
			{
				Invariant.Assert(this._pages != null, "Collection of pages does not exist.");
				Invariant.Assert(this._pages.Contains(ptsPage.Value), "Page does not exist.");
				try
				{
					if (enterContext)
					{
						this.Enter();
					}
					PTS.Validate(PTS.FsDestroyPage(this._ptsHost.Context, ptsPage.Value));
				}
				finally
				{
					if (enterContext)
					{
						this.Leave();
					}
					this._pages.Remove(ptsPage.Value);
				}
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00117648 File Offset: 0x00116648
		private object OnDestroyBreakRecord(object args)
		{
			SecurityCriticalDataForSet<IntPtr> securityCriticalDataForSet = (SecurityCriticalDataForSet<IntPtr>)args;
			Invariant.Assert(securityCriticalDataForSet.Value != IntPtr.Zero, "Invalid break record object.");
			if (!this.Disposed)
			{
				Invariant.Assert(this._pageBreakRecords != null, "Collection of break records does not exist.");
				Invariant.Assert(this._pageBreakRecords.Contains(securityCriticalDataForSet.Value), "Break record does not exist.");
				try
				{
					this.Enter();
					PTS.Validate(PTS.FsDestroyPageBreakRecord(this._ptsHost.Context, securityCriticalDataForSet.Value));
				}
				finally
				{
					this.Leave();
					this._pageBreakRecords.Remove(securityCriticalDataForSet.Value);
				}
			}
			return null;
		}

		// Token: 0x040007CC RID: 1996
		private PtsContext.HandleIndex[] _unmanagedHandles;

		// Token: 0x040007CD RID: 1997
		private ArrayList _pages;

		// Token: 0x040007CE RID: 1998
		private ArrayList _pageBreakRecords;

		// Token: 0x040007CF RID: 1999
		private Exception _callbackException;

		// Token: 0x040007D0 RID: 2000
		private PtsHost _ptsHost;

		// Token: 0x040007D1 RID: 2001
		private bool _isOptimalParagraphEnabled;

		// Token: 0x040007D2 RID: 2002
		private TextFormatter _textFormatter;

		// Token: 0x040007D3 RID: 2003
		private int _disposed;

		// Token: 0x040007D4 RID: 2004
		private bool _disposeCompleted;

		// Token: 0x040007D5 RID: 2005
		private const int _defaultHandlesCapacity = 16;

		// Token: 0x020008C4 RID: 2244
		private struct HandleIndex
		{
			// Token: 0x0600815B RID: 33115 RVA: 0x00323165 File Offset: 0x00322165
			internal bool IsHandle()
			{
				return this.Obj != null && this.Index == 0L;
			}

			// Token: 0x04003C43 RID: 15427
			internal long Index;

			// Token: 0x04003C44 RID: 15428
			internal object Obj;
		}
	}
}
