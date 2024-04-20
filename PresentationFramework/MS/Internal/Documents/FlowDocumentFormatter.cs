using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020001C5 RID: 453
	internal class FlowDocumentFormatter : IFlowDocumentFormatter
	{
		// Token: 0x06000F71 RID: 3953 RVA: 0x0013D743 File Offset: 0x0013C743
		internal FlowDocumentFormatter(FlowDocument document)
		{
			this._document = document;
			this._documentPage = new FlowDocumentPage(this._document.StructuralCache);
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x0013D768 File Offset: 0x0013C768
		internal void Format(Size constraint)
		{
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			if (this._document.StructuralCache.IsFormattedOnce)
			{
				if (!this._lastFormatSuccessful)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				if (!this._arrangedAfterFormat && (!this._document.StructuralCache.ForceReformat || !this._document.StructuralCache.DestroyStructure))
				{
					this._documentPage.Arrange(this._documentPage.ContentSize);
					this._documentPage.EnsureValidVisuals();
				}
			}
			this._arrangedAfterFormat = false;
			this._lastFormatSuccessful = false;
			this._isContentFormatValid = false;
			Size pageSize = this.ComputePageSize(constraint);
			Thickness pageMargin = this.ComputePageMargin();
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight = constraint.Height;
					this._documentPage.FormatBottomless(pageSize, pageMargin);
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			this._lastFormatSuccessful = true;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0013D8E8 File Offset: 0x0013C8E8
		internal void Arrange(Size arrangeSize, Rect viewport)
		{
			Invariant.Assert(this._document.StructuralCache.DtrList == null || this._document.StructuralCache.DtrList.Length == 0 || (this._document.StructuralCache.DtrList.Length == 1 && this._document.StructuralCache.BackgroundFormatInfo.DoesFinalDTRCoverRestOfText));
			this._documentPage.Arrange(arrangeSize);
			this._documentPage.EnsureValidVisuals();
			this._arrangedAfterFormat = true;
			if (viewport.IsEmpty)
			{
				viewport = new Rect(0.0, 0.0, arrangeSize.Width, this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight);
			}
			PTS.FSRECT fsrect = new PTS.FSRECT(viewport);
			this._documentPage.UpdateViewport(ref fsrect, true);
			this._isContentFormatValid = true;
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0013D9CF File Offset: 0x0013C9CF
		internal FlowDocumentPage DocumentPage
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000F75 RID: 3957 RVA: 0x0013D9D8 File Offset: 0x0013C9D8
		// (remove) Token: 0x06000F76 RID: 3958 RVA: 0x0013DA10 File Offset: 0x0013CA10
		internal event EventHandler ContentInvalidated;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000F77 RID: 3959 RVA: 0x0013DA48 File Offset: 0x0013CA48
		// (remove) Token: 0x06000F78 RID: 3960 RVA: 0x0013DA80 File Offset: 0x0013CA80
		internal event EventHandler Suspended;

		// Token: 0x06000F79 RID: 3961 RVA: 0x0013DAB8 File Offset: 0x0013CAB8
		private Size ComputePageSize(Size constraint)
		{
			Size result = new Size(this._document.PageWidth, double.PositiveInfinity);
			if (DoubleUtil.IsNaN(result.Width))
			{
				result.Width = constraint.Width;
				double maxPageWidth = this._document.MaxPageWidth;
				if (result.Width > maxPageWidth)
				{
					result.Width = maxPageWidth;
				}
				double minPageWidth = this._document.MinPageWidth;
				if (result.Width < minPageWidth)
				{
					result.Width = minPageWidth;
				}
			}
			if (double.IsPositiveInfinity(result.Width))
			{
				result.Width = 500.0;
			}
			return result;
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0013DB58 File Offset: 0x0013CB58
		private Thickness ComputePageMargin()
		{
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._document);
			Thickness pagePadding = this._document.PagePadding;
			if (DoubleUtil.IsNaN(pagePadding.Left))
			{
				pagePadding.Left = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Top))
			{
				pagePadding.Top = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Right))
			{
				pagePadding.Right = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Bottom))
			{
				pagePadding.Bottom = lineHeightValue;
			}
			return pagePadding;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0013DBD6 File Offset: 0x0013CBD6
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout)
		{
			if (affectsLayout)
			{
				if (!this._arrangedAfterFormat)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				this._isContentFormatValid = false;
			}
			if (this.ContentInvalidated != null)
			{
				this.ContentInvalidated(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0013DC14 File Offset: 0x0013CC14
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end)
		{
			((IFlowDocumentFormatter)this).OnContentInvalidated(affectsLayout);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0013DC1D File Offset: 0x0013CC1D
		void IFlowDocumentFormatter.Suspend()
		{
			if (this.Suspended != null)
			{
				this.Suspended(this, EventArgs.Empty);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000F7E RID: 3966 RVA: 0x0013DC38 File Offset: 0x0013CC38
		bool IFlowDocumentFormatter.IsLayoutDataValid
		{
			get
			{
				return this._documentPage != null && this._document.StructuralCache.IsFormattedOnce && !this._document.StructuralCache.ForceReformat && this._isContentFormatValid && !this._document.StructuralCache.IsContentChangeInProgress && !this._document.StructuralCache.IsFormattingInProgress;
			}
		}

		// Token: 0x04000A9F RID: 2719
		private readonly FlowDocument _document;

		// Token: 0x04000AA0 RID: 2720
		private FlowDocumentPage _documentPage;

		// Token: 0x04000AA1 RID: 2721
		private bool _arrangedAfterFormat;

		// Token: 0x04000AA2 RID: 2722
		private bool _lastFormatSuccessful;

		// Token: 0x04000AA3 RID: 2723
		private const double _defaultWidth = 500.0;

		// Token: 0x04000AA4 RID: 2724
		private bool _isContentFormatValid;
	}
}
