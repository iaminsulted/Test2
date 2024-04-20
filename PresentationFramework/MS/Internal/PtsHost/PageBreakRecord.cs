using System;
using System.Threading;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000132 RID: 306
	internal sealed class PageBreakRecord : IDisposable
	{
		// Token: 0x06000861 RID: 2145 RVA: 0x00114DEC File Offset: 0x00113DEC
		internal PageBreakRecord(PtsContext ptsContext, SecurityCriticalDataForSet<IntPtr> br, int pageNumber)
		{
			Invariant.Assert(ptsContext != null, "Invalid PtsContext object.");
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			this._br = br;
			this._pageNumber = pageNumber;
			this._ptsContext = new WeakReference(ptsContext);
			ptsContext.OnPageBreakRecordCreated(this._br);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00114E50 File Offset: 0x00113E50
		~PageBreakRecord()
		{
			this.Dispose(false);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00114E80 File Offset: 0x00113E80
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00114E8F File Offset: 0x00113E8F
		internal IntPtr BreakRecord
		{
			get
			{
				return this._br.Value;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00114E9C File Offset: 0x00113E9C
		internal int PageNumber
		{
			get
			{
				return this._pageNumber;
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00114EA4 File Offset: 0x00113EA4
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				PtsContext ptsContext = this._ptsContext.Target as PtsContext;
				if (ptsContext != null && !ptsContext.Disposed)
				{
					ptsContext.OnPageBreakRecordDisposed(this._br, disposing);
				}
				this._br.Value = IntPtr.Zero;
				this._ptsContext = null;
			}
		}

		// Token: 0x040007BD RID: 1981
		private SecurityCriticalDataForSet<IntPtr> _br;

		// Token: 0x040007BE RID: 1982
		private readonly int _pageNumber;

		// Token: 0x040007BF RID: 1983
		private WeakReference _ptsContext;

		// Token: 0x040007C0 RID: 1984
		private int _disposed;
	}
}
