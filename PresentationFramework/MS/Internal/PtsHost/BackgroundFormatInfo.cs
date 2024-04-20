using System;
using System.Windows.Threading;
using MS.Internal.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200010A RID: 266
	internal sealed class BackgroundFormatInfo
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x001090F8 File Offset: 0x001080F8
		internal BackgroundFormatInfo(StructuralCache structuralCache)
		{
			this._structuralCache = structuralCache;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00109114 File Offset: 0x00108114
		internal void UpdateBackgroundFormatInfo()
		{
			this._cpInterrupted = -1;
			this._lastCPUninterruptible = 0;
			this._doesFinalDTRCoverRestOfText = false;
			this._cchAllText = this._structuralCache.TextContainer.SymbolCount;
			if (this._structuralCache.DtrList != null)
			{
				int num = 0;
				for (int i = 0; i < this._structuralCache.DtrList.Length - 1; i++)
				{
					num += this._structuralCache.DtrList[i].PositionsAdded - this._structuralCache.DtrList[i].PositionsRemoved;
				}
				DirtyTextRange dirtyTextRange = this._structuralCache.DtrList[this._structuralCache.DtrList.Length - 1];
				if (dirtyTextRange.StartIndex + num + dirtyTextRange.PositionsAdded >= this._cchAllText)
				{
					this._doesFinalDTRCoverRestOfText = true;
					this._lastCPUninterruptible = dirtyTextRange.StartIndex + num;
				}
			}
			else
			{
				this._doesFinalDTRCoverRestOfText = true;
			}
			this._backgroundFormatStopTime = DateTime.UtcNow.AddMilliseconds(200.0);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0010922C File Offset: 0x0010822C
		internal void ThrottleBackgroundFormatting()
		{
			if (this._throttleBackgroundTimer == null)
			{
				this._throttleBackgroundTimer = new DispatcherTimer(DispatcherPriority.Background);
				this._throttleBackgroundTimer.Interval = new TimeSpan(0, 0, 2);
				this._throttleBackgroundTimer.Tick += this.OnThrottleBackgroundTimeout;
			}
			else
			{
				this._throttleBackgroundTimer.Stop();
			}
			this._throttleBackgroundTimer.Start();
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0010928F File Offset: 0x0010828F
		internal void BackgroundFormat(IFlowDocumentFormatter formatter, bool ignoreThrottle)
		{
			if (this._throttleBackgroundTimer == null)
			{
				formatter.OnContentInvalidated(true);
				return;
			}
			if (ignoreThrottle)
			{
				this.OnThrottleBackgroundTimeout(null, EventArgs.Empty);
				return;
			}
			this._pendingBackgroundFormatter = formatter;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x001092B8 File Offset: 0x001082B8
		internal int LastCPUninterruptible
		{
			get
			{
				return this._lastCPUninterruptible;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x001092C0 File Offset: 0x001082C0
		internal DateTime BackgroundFormatStopTime
		{
			get
			{
				return this._backgroundFormatStopTime;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x001092C8 File Offset: 0x001082C8
		internal int CchAllText
		{
			get
			{
				return this._cchAllText;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x001092D0 File Offset: 0x001082D0
		internal static bool IsBackgroundFormatEnabled
		{
			get
			{
				return BackgroundFormatInfo._isBackgroundFormatEnabled;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x001092D7 File Offset: 0x001082D7
		internal bool DoesFinalDTRCoverRestOfText
		{
			get
			{
				return this._doesFinalDTRCoverRestOfText;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600069E RID: 1694 RVA: 0x001092DF File Offset: 0x001082DF
		// (set) Token: 0x0600069F RID: 1695 RVA: 0x001092E7 File Offset: 0x001082E7
		internal int CPInterrupted
		{
			get
			{
				return this._cpInterrupted;
			}
			set
			{
				this._cpInterrupted = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x001092F0 File Offset: 0x001082F0
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x001092F8 File Offset: 0x001082F8
		internal double ViewportHeight
		{
			get
			{
				return this._viewportHeight;
			}
			set
			{
				this._viewportHeight = value;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00109301 File Offset: 0x00108301
		private void OnThrottleBackgroundTimeout(object sender, EventArgs e)
		{
			this._throttleBackgroundTimer.Stop();
			this._throttleBackgroundTimer = null;
			if (this._pendingBackgroundFormatter != null)
			{
				this.BackgroundFormat(this._pendingBackgroundFormatter, true);
				this._pendingBackgroundFormatter = null;
			}
		}

		// Token: 0x0400070F RID: 1807
		private double _viewportHeight;

		// Token: 0x04000710 RID: 1808
		private bool _doesFinalDTRCoverRestOfText;

		// Token: 0x04000711 RID: 1809
		private int _lastCPUninterruptible;

		// Token: 0x04000712 RID: 1810
		private DateTime _backgroundFormatStopTime;

		// Token: 0x04000713 RID: 1811
		private int _cchAllText;

		// Token: 0x04000714 RID: 1812
		private int _cpInterrupted;

		// Token: 0x04000715 RID: 1813
		private static bool _isBackgroundFormatEnabled = true;

		// Token: 0x04000716 RID: 1814
		private StructuralCache _structuralCache;

		// Token: 0x04000717 RID: 1815
		private DateTime _throttleTimeout = DateTime.UtcNow;

		// Token: 0x04000718 RID: 1816
		private DispatcherTimer _throttleBackgroundTimer;

		// Token: 0x04000719 RID: 1817
		private IFlowDocumentFormatter _pendingBackgroundFormatter;

		// Token: 0x0400071A RID: 1818
		private const uint _throttleBackgroundSeconds = 2U;

		// Token: 0x0400071B RID: 1819
		private const uint _stopTimeDelta = 200U;
	}
}
