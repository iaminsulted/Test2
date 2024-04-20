using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MS.Internal.Documents
{
	// Token: 0x020001BC RID: 444
	internal class DocumentGridPage : FrameworkElement, IDisposable
	{
		// Token: 0x06000EFD RID: 3837 RVA: 0x0013BD98 File Offset: 0x0013AD98
		public DocumentGridPage(DocumentPaginator paginator)
		{
			this._paginator = paginator;
			this._paginator.GetPageCompleted += this.OnGetPageCompleted;
			this.Init();
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000EFE RID: 3838 RVA: 0x0013BE2D File Offset: 0x0013AE2D
		public DocumentPage DocumentPage
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView.DocumentPage;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0013BE40 File Offset: 0x0013AE40
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0013BE53 File Offset: 0x0013AE53
		public int PageNumber
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView.PageNumber;
			}
			set
			{
				this.CheckDisposed();
				if (this._documentPageView.PageNumber != value)
				{
					this._documentPageView.PageNumber = value;
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0013BE75 File Offset: 0x0013AE75
		public DocumentPageView DocumentPageView
		{
			get
			{
				this.CheckDisposed();
				return this._documentPageView;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000F02 RID: 3842 RVA: 0x0013BE83 File Offset: 0x0013AE83
		// (set) Token: 0x06000F03 RID: 3843 RVA: 0x0013BE91 File Offset: 0x0013AE91
		public bool ShowPageBorders
		{
			get
			{
				this.CheckDisposed();
				return this._showPageBorders;
			}
			set
			{
				this.CheckDisposed();
				if (this._showPageBorders != value)
				{
					this._showPageBorders = value;
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0013BEAF File Offset: 0x0013AEAF
		public bool IsPageLoaded
		{
			get
			{
				this.CheckDisposed();
				return this._loaded;
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000F05 RID: 3845 RVA: 0x0013BEC0 File Offset: 0x0013AEC0
		// (remove) Token: 0x06000F06 RID: 3846 RVA: 0x0013BEF8 File Offset: 0x0013AEF8
		public event EventHandler PageLoaded;

		// Token: 0x06000F07 RID: 3847 RVA: 0x0013BF30 File Offset: 0x0013AF30
		protected override Visual GetVisualChild(int index)
		{
			this.CheckDisposed();
			if (this.VisualChildrenCount == 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			switch (index)
			{
			case 0:
				return this._dropShadowRight;
			case 1:
				return this._dropShadowBottom;
			case 2:
				return this._pageBorder;
			default:
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000F08 RID: 3848 RVA: 0x0013BFA9 File Offset: 0x0013AFA9
		protected override int VisualChildrenCount
		{
			get
			{
				if (!this._disposed && this.hasAddedChildren)
				{
					return 3;
				}
				return 0;
			}
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0013BFC0 File Offset: 0x0013AFC0
		protected sealed override Size MeasureOverride(Size availableSize)
		{
			this.CheckDisposed();
			if (!this.hasAddedChildren)
			{
				base.AddVisualChild(this._dropShadowRight);
				base.AddVisualChild(this._dropShadowBottom);
				base.AddVisualChild(this._pageBorder);
				this.hasAddedChildren = true;
			}
			if (this.ShowPageBorders)
			{
				this._pageBorder.BorderThickness = this._pageBorderVisibleThickness;
				this._pageBorder.Background = Brushes.White;
				this._dropShadowRight.Opacity = 0.35;
				this._dropShadowBottom.Opacity = 0.35;
			}
			else
			{
				this._pageBorder.BorderThickness = this._pageBorderInvisibleThickness;
				this._pageBorder.Background = Brushes.Transparent;
				this._dropShadowRight.Opacity = 0.0;
				this._dropShadowBottom.Opacity = 0.0;
			}
			this._dropShadowRight.Measure(availableSize);
			this._dropShadowBottom.Measure(availableSize);
			this._pageBorder.Measure(availableSize);
			if (this.DocumentPage.Size != Size.Empty && this.DocumentPage.Size.Width != 0.0)
			{
				this._documentPageView.SetPageZoom(availableSize.Width / this.DocumentPage.Size.Width);
			}
			return availableSize;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0013C124 File Offset: 0x0013B124
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			this.CheckDisposed();
			this._pageBorder.Arrange(new Rect(new Point(0.0, 0.0), arrangeSize));
			this._dropShadowRight.Arrange(new Rect(new Point(arrangeSize.Width, 5.0), new Size(5.0, Math.Max(0.0, arrangeSize.Height - 5.0))));
			this._dropShadowBottom.Arrange(new Rect(new Point(5.0, arrangeSize.Height), new Size(arrangeSize.Width, 5.0)));
			base.ArrangeOverride(arrangeSize);
			return arrangeSize;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0013C1F4 File Offset: 0x0013B1F4
		private void Init()
		{
			this._documentPageView = new DocumentPageView();
			this._documentPageView.ClipToBounds = true;
			this._documentPageView.StretchDirection = StretchDirection.Both;
			this._documentPageView.PageNumber = int.MaxValue;
			this._pageBorder = new Border();
			this._pageBorder.BorderBrush = Brushes.Black;
			this._pageBorder.Child = this._documentPageView;
			this._dropShadowRight = new Rectangle();
			this._dropShadowRight.Fill = Brushes.Black;
			this._dropShadowRight.Opacity = 0.35;
			this._dropShadowBottom = new Rectangle();
			this._dropShadowBottom.Fill = Brushes.Black;
			this._dropShadowBottom.Opacity = 0.35;
			this._loaded = false;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0013C2C8 File Offset: 0x0013B2C8
		private void OnGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!this._disposed && e != null && !e.Cancelled && e.Error == null && e.PageNumber != 2147483647 && e.PageNumber == this.PageNumber && e.DocumentPage != null && e.DocumentPage != DocumentPage.Missing)
			{
				this._loaded = true;
				if (this.PageLoaded != null)
				{
					this.PageLoaded(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0013C340 File Offset: 0x0013B340
		protected void Dispose()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				if (this._paginator != null)
				{
					this._paginator.GetPageCompleted -= this.OnGetPageCompleted;
					this._paginator = null;
				}
				IDisposable documentPageView = this._documentPageView;
				if (documentPageView != null)
				{
					documentPageView.Dispose();
				}
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0013C392 File Offset: 0x0013B392
		private void CheckDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(typeof(DocumentPageView).ToString());
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x0013C3B1 File Offset: 0x0013B3B1
		void IDisposable.Dispose()
		{
			GC.SuppressFinalize(this);
			this.Dispose();
		}

		// Token: 0x04000A7E RID: 2686
		private bool hasAddedChildren;

		// Token: 0x04000A7F RID: 2687
		private DocumentPaginator _paginator;

		// Token: 0x04000A80 RID: 2688
		private DocumentPageView _documentPageView;

		// Token: 0x04000A81 RID: 2689
		private Rectangle _dropShadowRight;

		// Token: 0x04000A82 RID: 2690
		private Rectangle _dropShadowBottom;

		// Token: 0x04000A83 RID: 2691
		private Border _pageBorder;

		// Token: 0x04000A84 RID: 2692
		private bool _showPageBorders;

		// Token: 0x04000A85 RID: 2693
		private bool _loaded;

		// Token: 0x04000A86 RID: 2694
		private const double _dropShadowOpacity = 0.35;

		// Token: 0x04000A87 RID: 2695
		private const double _dropShadowWidth = 5.0;

		// Token: 0x04000A88 RID: 2696
		private readonly Thickness _pageBorderVisibleThickness = new Thickness(1.0, 1.0, 1.0, 1.0);

		// Token: 0x04000A89 RID: 2697
		private readonly Thickness _pageBorderInvisibleThickness = new Thickness(0.0, 0.0, 0.0, 0.0);

		// Token: 0x04000A8A RID: 2698
		private bool _disposed;
	}
}
