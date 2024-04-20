using System;
using System.Windows.Automation.Peers;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Documents;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000830 RID: 2096
	public class DocumentPageView : FrameworkElement, IServiceProvider, IDisposable
	{
		// Token: 0x06007AD6 RID: 31446 RVA: 0x00309D5D File Offset: 0x00308D5D
		public DocumentPageView()
		{
			this._pageZoom = 1.0;
		}

		// Token: 0x06007AD7 RID: 31447 RVA: 0x00309D7C File Offset: 0x00308D7C
		static DocumentPageView()
		{
			UIElement.ClipToBoundsProperty.OverrideMetadata(typeof(DocumentPageView), new PropertyMetadata(BooleanBoxes.TrueBox));
		}

		// Token: 0x17001C6B RID: 7275
		// (get) Token: 0x06007AD8 RID: 31448 RVA: 0x00309E2C File Offset: 0x00308E2C
		// (set) Token: 0x06007AD9 RID: 31449 RVA: 0x00309E34 File Offset: 0x00308E34
		public DocumentPaginator DocumentPaginator
		{
			get
			{
				return this._documentPaginator;
			}
			set
			{
				this.CheckDisposed();
				if (this._documentPaginator != value)
				{
					if (this._documentPaginator != null)
					{
						this._documentPaginator.GetPageCompleted -= this.HandleGetPageCompleted;
						this._documentPaginator.PagesChanged -= this.HandlePagesChanged;
						this.DisposeCurrentPage();
						this.DisposeAsyncPage();
					}
					Invariant.Assert(this._documentPage == null);
					Invariant.Assert(this._documentPageAsync == null);
					this._documentPaginator = value;
					this._textView = null;
					if (this._documentPaginator != null)
					{
						this._documentPaginator.GetPageCompleted += this.HandleGetPageCompleted;
						this._documentPaginator.PagesChanged += this.HandlePagesChanged;
					}
					base.InvalidateMeasure();
				}
			}
		}

		// Token: 0x17001C6C RID: 7276
		// (get) Token: 0x06007ADA RID: 31450 RVA: 0x00309EFB File Offset: 0x00308EFB
		public DocumentPage DocumentPage
		{
			get
			{
				if (this._documentPage != null)
				{
					return this._documentPage;
				}
				return DocumentPage.Missing;
			}
		}

		// Token: 0x17001C6D RID: 7277
		// (get) Token: 0x06007ADB RID: 31451 RVA: 0x00309F11 File Offset: 0x00308F11
		// (set) Token: 0x06007ADC RID: 31452 RVA: 0x00309F23 File Offset: 0x00308F23
		public int PageNumber
		{
			get
			{
				return (int)base.GetValue(DocumentPageView.PageNumberProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.PageNumberProperty, value);
			}
		}

		// Token: 0x17001C6E RID: 7278
		// (get) Token: 0x06007ADD RID: 31453 RVA: 0x00309F36 File Offset: 0x00308F36
		// (set) Token: 0x06007ADE RID: 31454 RVA: 0x00309F48 File Offset: 0x00308F48
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(DocumentPageView.StretchProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.StretchProperty, value);
			}
		}

		// Token: 0x17001C6F RID: 7279
		// (get) Token: 0x06007ADF RID: 31455 RVA: 0x00309F5B File Offset: 0x00308F5B
		// (set) Token: 0x06007AE0 RID: 31456 RVA: 0x00309F6D File Offset: 0x00308F6D
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(DocumentPageView.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(DocumentPageView.StretchDirectionProperty, value);
			}
		}

		// Token: 0x14000152 RID: 338
		// (add) Token: 0x06007AE1 RID: 31457 RVA: 0x00309F80 File Offset: 0x00308F80
		// (remove) Token: 0x06007AE2 RID: 31458 RVA: 0x00309FB8 File Offset: 0x00308FB8
		public event EventHandler PageConnected;

		// Token: 0x14000153 RID: 339
		// (add) Token: 0x06007AE3 RID: 31459 RVA: 0x00309FF0 File Offset: 0x00308FF0
		// (remove) Token: 0x06007AE4 RID: 31460 RVA: 0x0030A028 File Offset: 0x00309028
		public event EventHandler PageDisconnected;

		// Token: 0x06007AE5 RID: 31461 RVA: 0x0030A05D File Offset: 0x0030905D
		protected override void OnDpiChanged(DpiScale oldDpiScaleInfo, DpiScale newDpiScaleInfo)
		{
			this.DisposeCurrentPage();
			this.DisposeAsyncPage();
		}

		// Token: 0x06007AE6 RID: 31462 RVA: 0x0030A06C File Offset: 0x0030906C
		protected sealed override Size MeasureOverride(Size availableSize)
		{
			Size result = default(Size);
			this.CheckDisposed();
			if (this._suspendLayout)
			{
				result = base.DesiredSize;
			}
			else if (this._documentPaginator != null)
			{
				if (this.ShouldReflowContent() && (!double.IsInfinity(availableSize.Width) || !double.IsInfinity(availableSize.Height)))
				{
					Size pageSize = this._documentPaginator.PageSize;
					Size size;
					if (double.IsInfinity(availableSize.Width))
					{
						size = default(Size);
						size.Height = availableSize.Height / this._pageZoom;
						size.Width = size.Height * (pageSize.Width / pageSize.Height);
					}
					else if (double.IsInfinity(availableSize.Height))
					{
						size = default(Size);
						size.Width = availableSize.Width / this._pageZoom;
						size.Height = size.Width * (pageSize.Height / pageSize.Width);
					}
					else
					{
						size = new Size(availableSize.Width / this._pageZoom, availableSize.Height / this._pageZoom);
					}
					if (!DoubleUtil.AreClose(pageSize, size))
					{
						this._documentPaginator.PageSize = size;
					}
				}
				if (this._documentPage == null && this._documentPageAsync == null)
				{
					if (this.PageNumber >= 0)
					{
						if (this._useAsynchronous)
						{
							this._documentPaginator.GetPageAsync(this.PageNumber, this);
						}
						else
						{
							this._documentPageAsync = this._documentPaginator.GetPage(this.PageNumber);
							if (this._documentPageAsync == null)
							{
								this._documentPageAsync = DocumentPage.Missing;
							}
						}
					}
					else
					{
						this._documentPage = DocumentPage.Missing;
					}
				}
				if (this._documentPageAsync != null)
				{
					this.DisposeCurrentPage();
					if (this._documentPageAsync == null)
					{
						this._documentPageAsync = DocumentPage.Missing;
					}
					if (this._pageVisualClone != null)
					{
						this.RemoveDuplicateVisual();
					}
					this._documentPage = this._documentPageAsync;
					if (this._documentPage != DocumentPage.Missing)
					{
						this._documentPage.PageDestroyed += this.HandlePageDestroyed;
						this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
					}
					this._documentPageAsync = null;
					this._newPageConnected = true;
				}
				if (this._documentPage != null && this._documentPage != DocumentPage.Missing)
				{
					Size pageSize = new Size(this._documentPage.Size.Width * this._pageZoom, this._documentPage.Size.Height * this._pageZoom);
					Size size2 = Viewbox.ComputeScaleFactor(availableSize, pageSize, this.Stretch, this.StretchDirection);
					result = new Size(pageSize.Width * size2.Width, pageSize.Height * size2.Height);
				}
				if (this._pageVisualClone != null)
				{
					result = this._visualCloneSize;
				}
			}
			return result;
		}

		// Token: 0x06007AE7 RID: 31463 RVA: 0x0030A338 File Offset: 0x00309338
		protected sealed override Size ArrangeOverride(Size finalSize)
		{
			this.CheckDisposed();
			if (this._pageVisualClone == null)
			{
				if (this._pageHost == null)
				{
					this._pageHost = new DocumentPageHost();
					base.AddVisualChild(this._pageHost);
				}
				Invariant.Assert(this._pageHost != null);
				Visual visual = (this._documentPage == null) ? null : this._documentPage.Visual;
				if (visual == null)
				{
					this._pageHost.PageVisual = null;
					this._pageHost.CachedOffset = default(Point);
					this._pageHost.RenderTransform = null;
					this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, finalSize));
				}
				else
				{
					if (this._pageHost.PageVisual != visual)
					{
						DocumentPageHost.DisconnectPageVisual(visual);
						this._pageHost.PageVisual = visual;
					}
					Size size = this._documentPage.Size;
					Transform transform = Transform.Identity;
					if (base.FlowDirection == FlowDirection.RightToLeft)
					{
						transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, size.Width, 0.0);
					}
					if (!DoubleUtil.IsOne(this._pageZoom))
					{
						ScaleTransform scaleTransform = new ScaleTransform(this._pageZoom, this._pageZoom);
						if (transform == Transform.Identity)
						{
							transform = scaleTransform;
						}
						else
						{
							transform = new MatrixTransform(transform.Value * scaleTransform.Value);
						}
						size = new Size(size.Width * this._pageZoom, size.Height * this._pageZoom);
					}
					Size size2 = Viewbox.ComputeScaleFactor(finalSize, size, this.Stretch, this.StretchDirection);
					if (!DoubleUtil.IsOne(size2.Width) || !DoubleUtil.IsOne(size2.Height))
					{
						ScaleTransform scaleTransform = new ScaleTransform(size2.Width, size2.Height);
						if (transform == Transform.Identity)
						{
							transform = scaleTransform;
						}
						else
						{
							transform = new MatrixTransform(transform.Value * scaleTransform.Value);
						}
						size = new Size(size.Width * size2.Width, size.Height * size2.Height);
					}
					this._pageHost.CachedOffset = new Point((finalSize.Width - size.Width) / 2.0, (finalSize.Height - size.Height) / 2.0);
					this._pageHost.RenderTransform = transform;
					this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, this._documentPage.Size));
				}
				if (this._newPageConnected)
				{
					this.OnPageConnected();
				}
				this.OnTransformChangedAsync();
			}
			else if (this._pageHost.PageVisual != this._pageVisualClone)
			{
				this._pageHost.PageVisual = this._pageVisualClone;
				this._pageHost.Arrange(new Rect(this._pageHost.CachedOffset, finalSize));
			}
			return base.ArrangeOverride(finalSize);
		}

		// Token: 0x06007AE8 RID: 31464 RVA: 0x0030A61F File Offset: 0x0030961F
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._pageHost == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._pageHost;
		}

		// Token: 0x06007AE9 RID: 31465 RVA: 0x0030A650 File Offset: 0x00309650
		protected void Dispose()
		{
			if (!this._disposed)
			{
				this._disposed = true;
				if (this._documentPaginator != null)
				{
					this._documentPaginator.GetPageCompleted -= this.HandleGetPageCompleted;
					this._documentPaginator.PagesChanged -= this.HandlePagesChanged;
					this._documentPaginator.CancelAsync(this);
					this.DisposeCurrentPage();
					this.DisposeAsyncPage();
				}
				Invariant.Assert(this._documentPage == null);
				Invariant.Assert(this._documentPageAsync == null);
				this._documentPaginator = null;
				this._textView = null;
			}
		}

		// Token: 0x06007AEA RID: 31466 RVA: 0x0030A6E4 File Offset: 0x003096E4
		protected object GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			this.CheckDisposed();
			if (this._documentPaginator != null && this._documentPaginator is IServiceProvider)
			{
				if (serviceType == typeof(ITextView))
				{
					if (this._textView == null)
					{
						ITextContainer textContainer = ((IServiceProvider)this._documentPaginator).GetService(typeof(ITextContainer)) as ITextContainer;
						if (textContainer != null)
						{
							this._textView = new DocumentPageTextView(this, textContainer);
						}
					}
					result = this._textView;
				}
				else if (serviceType == typeof(TextContainer) || serviceType == typeof(ITextContainer))
				{
					result = ((IServiceProvider)this._documentPaginator).GetService(serviceType);
				}
			}
			return result;
		}

		// Token: 0x06007AEB RID: 31467 RVA: 0x0030A7B2 File Offset: 0x003097B2
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DocumentPageViewAutomationPeer(this);
		}

		// Token: 0x17001C70 RID: 7280
		// (get) Token: 0x06007AEC RID: 31468 RVA: 0x0030A7BA File Offset: 0x003097BA
		protected bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x17001C71 RID: 7281
		// (get) Token: 0x06007AED RID: 31469 RVA: 0x0030A7C2 File Offset: 0x003097C2
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageHost == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06007AEE RID: 31470 RVA: 0x0030A7D0 File Offset: 0x003097D0
		internal void SetPageZoom(double pageZoom)
		{
			Invariant.Assert(!DoubleUtil.LessThanOrClose(pageZoom, 0.0) && !double.IsInfinity(pageZoom));
			Invariant.Assert(!this._disposed);
			if (!DoubleUtil.AreClose(this._pageZoom, pageZoom))
			{
				this._pageZoom = pageZoom;
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06007AEF RID: 31471 RVA: 0x0030A828 File Offset: 0x00309828
		internal void SuspendLayout()
		{
			this._suspendLayout = true;
			this._pageVisualClone = this.DuplicatePageVisual();
			this._visualCloneSize = base.DesiredSize;
		}

		// Token: 0x06007AF0 RID: 31472 RVA: 0x0030A849 File Offset: 0x00309849
		internal void ResumeLayout()
		{
			this._suspendLayout = false;
			this._pageVisualClone = null;
			base.InvalidateMeasure();
		}

		// Token: 0x06007AF1 RID: 31473 RVA: 0x0030A85F File Offset: 0x0030985F
		internal void DuplicateVisual()
		{
			if (this._documentPage != null && this._pageVisualClone == null)
			{
				this._pageVisualClone = this.DuplicatePageVisual();
				this._visualCloneSize = base.DesiredSize;
				base.InvalidateArrange();
			}
		}

		// Token: 0x06007AF2 RID: 31474 RVA: 0x0030A88F File Offset: 0x0030988F
		internal void RemoveDuplicateVisual()
		{
			if (this._pageVisualClone != null)
			{
				this._pageVisualClone = null;
				base.InvalidateArrange();
			}
		}

		// Token: 0x17001C72 RID: 7282
		// (get) Token: 0x06007AF3 RID: 31475 RVA: 0x0030A8A6 File Offset: 0x003098A6
		// (set) Token: 0x06007AF4 RID: 31476 RVA: 0x0030A8AE File Offset: 0x003098AE
		internal bool UseAsynchronousGetPage
		{
			get
			{
				return this._useAsynchronous;
			}
			set
			{
				this._useAsynchronous = value;
			}
		}

		// Token: 0x17001C73 RID: 7283
		// (get) Token: 0x06007AF5 RID: 31477 RVA: 0x0030A8B7 File Offset: 0x003098B7
		internal DocumentPage DocumentPageInternal
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x06007AF6 RID: 31478 RVA: 0x0030A8BF File Offset: 0x003098BF
		private void HandlePageDestroyed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				base.InvalidateMeasure();
				this.DisposeCurrentPage();
			}
		}

		// Token: 0x06007AF7 RID: 31479 RVA: 0x0030A8D5 File Offset: 0x003098D5
		private void HandleAsyncPageDestroyed(object sender, EventArgs e)
		{
			if (!this._disposed)
			{
				this.DisposeAsyncPage();
			}
		}

		// Token: 0x06007AF8 RID: 31480 RVA: 0x0030A8E8 File Offset: 0x003098E8
		private void HandleGetPageCompleted(object sender, GetPageCompletedEventArgs e)
		{
			if (!this._disposed && e != null && !e.Cancelled && e.Error == null && e.PageNumber == this.PageNumber && e.UserState == this)
			{
				if (this._documentPageAsync != null && this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
				}
				this._documentPageAsync = e.DocumentPage;
				if (this._documentPageAsync == null)
				{
					this._documentPageAsync = DocumentPage.Missing;
				}
				if (this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed += this.HandleAsyncPageDestroyed;
				}
				base.InvalidateMeasure();
			}
		}

		// Token: 0x06007AF9 RID: 31481 RVA: 0x0030A9A8 File Offset: 0x003099A8
		private void HandlePagesChanged(object sender, PagesChangedEventArgs e)
		{
			if (!this._disposed && e != null && this.PageNumber >= e.Start && (e.Count == 2147483647 || this.PageNumber <= e.Start + e.Count))
			{
				this.OnPageContentChanged();
			}
		}

		// Token: 0x06007AFA RID: 31482 RVA: 0x0030A9F6 File Offset: 0x003099F6
		private void OnTransformChangedAsync()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(this.OnTransformChanged), null);
		}

		// Token: 0x06007AFB RID: 31483 RVA: 0x0030AA13 File Offset: 0x00309A13
		private object OnTransformChanged(object arg)
		{
			if (this._textView != null && this._documentPage != null)
			{
				this._textView.OnTransformChanged();
			}
			return null;
		}

		// Token: 0x06007AFC RID: 31484 RVA: 0x0030AA31 File Offset: 0x00309A31
		private void OnPageConnected()
		{
			this._newPageConnected = false;
			if (this._textView != null)
			{
				this._textView.OnPageConnected();
			}
			if (this.PageConnected != null && this._documentPage != null)
			{
				this.PageConnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x06007AFD RID: 31485 RVA: 0x0030AA6E File Offset: 0x00309A6E
		private void OnPageDisconnected()
		{
			if (this._textView != null)
			{
				this._textView.OnPageDisconnected();
			}
			if (this.PageDisconnected != null)
			{
				this.PageDisconnected(this, EventArgs.Empty);
			}
		}

		// Token: 0x06007AFE RID: 31486 RVA: 0x0030AA9C File Offset: 0x00309A9C
		private void OnPageContentChanged()
		{
			base.InvalidateMeasure();
			this.DisposeCurrentPage();
			this.DisposeAsyncPage();
		}

		// Token: 0x06007AFF RID: 31487 RVA: 0x0030AAB0 File Offset: 0x00309AB0
		private void DisposeCurrentPage()
		{
			if (this._documentPage != null)
			{
				if (this._pageHost != null)
				{
					this._pageHost.PageVisual = null;
				}
				if (this._documentPage != DocumentPage.Missing)
				{
					this._documentPage.PageDestroyed -= this.HandlePageDestroyed;
				}
				if (this._documentPage != null)
				{
					((IDisposable)this._documentPage).Dispose();
				}
				this._documentPage = null;
				this.OnPageDisconnected();
			}
		}

		// Token: 0x06007B00 RID: 31488 RVA: 0x0030AB20 File Offset: 0x00309B20
		private void DisposeAsyncPage()
		{
			if (this._documentPageAsync != null)
			{
				if (this._documentPageAsync != DocumentPage.Missing)
				{
					this._documentPageAsync.PageDestroyed -= this.HandleAsyncPageDestroyed;
				}
				if (this._documentPageAsync != null)
				{
					((IDisposable)this._documentPageAsync).Dispose();
				}
				this._documentPageAsync = null;
			}
		}

		// Token: 0x06007B01 RID: 31489 RVA: 0x0030AB73 File Offset: 0x00309B73
		private void CheckDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(typeof(DocumentPageView).ToString());
			}
		}

		// Token: 0x06007B02 RID: 31490 RVA: 0x0030AB94 File Offset: 0x00309B94
		private bool ShouldReflowContent()
		{
			bool result = false;
			if (DocumentViewerBase.GetIsMasterPage(this))
			{
				DocumentViewerBase hostViewer = this.GetHostViewer();
				if (hostViewer != null)
				{
					result = hostViewer.IsMasterPageView(this);
				}
			}
			return result;
		}

		// Token: 0x06007B03 RID: 31491 RVA: 0x0030ABC0 File Offset: 0x00309BC0
		private DocumentViewerBase GetHostViewer()
		{
			DocumentViewerBase result = null;
			if (base.TemplatedParent is DocumentViewerBase)
			{
				result = (DocumentViewerBase)base.TemplatedParent;
			}
			else
			{
				for (Visual visual = VisualTreeHelper.GetParent(this) as Visual; visual != null; visual = (VisualTreeHelper.GetParent(visual) as Visual))
				{
					if (visual is DocumentViewerBase)
					{
						result = (DocumentViewerBase)visual;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06007B04 RID: 31492 RVA: 0x0030AC19 File Offset: 0x00309C19
		private static void OnPageNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Invariant.Assert(d != null && d is DocumentPageView);
			((DocumentPageView)d).OnPageContentChanged();
		}

		// Token: 0x06007B05 RID: 31493 RVA: 0x0030AC3C File Offset: 0x00309C3C
		private DrawingVisual DuplicatePageVisual()
		{
			DrawingVisual drawingVisual = null;
			if (this._pageHost != null && this._pageHost.PageVisual != null && this._documentPage.Size != Size.Empty)
			{
				Rect rectangle = new Rect(this._documentPage.Size);
				rectangle.Width = Math.Min(rectangle.Width, 4096.0);
				rectangle.Height = Math.Min(rectangle.Height, 4096.0);
				drawingVisual = new DrawingVisual();
				try
				{
					if (rectangle.Width > 1.0 && rectangle.Height > 1.0)
					{
						RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)rectangle.Width, (int)rectangle.Height, 96.0, 96.0, PixelFormats.Pbgra32);
						renderTargetBitmap.Render(this._pageHost.PageVisual);
						ImageBrush brush = new ImageBrush(renderTargetBitmap);
						drawingVisual.Opacity = 0.5;
						using (DrawingContext drawingContext = drawingVisual.RenderOpen())
						{
							drawingContext.DrawRectangle(brush, null, rectangle);
						}
					}
				}
				catch (OverflowException)
				{
				}
			}
			return drawingVisual;
		}

		// Token: 0x06007B06 RID: 31494 RVA: 0x0030AD88 File Offset: 0x00309D88
		object IServiceProvider.GetService(Type serviceType)
		{
			return this.GetService(serviceType);
		}

		// Token: 0x06007B07 RID: 31495 RVA: 0x0030AD91 File Offset: 0x00309D91
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x04003A1D RID: 14877
		public static readonly DependencyProperty PageNumberProperty = DependencyProperty.Register("PageNumber", typeof(int), typeof(DocumentPageView), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(DocumentPageView.OnPageNumberChanged)));

		// Token: 0x04003A1E RID: 14878
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(DocumentPageView), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04003A1F RID: 14879
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(DocumentPageView), new FrameworkPropertyMetadata(StretchDirection.DownOnly, FrameworkPropertyMetadataOptions.AffectsMeasure));

		// Token: 0x04003A22 RID: 14882
		private DocumentPaginator _documentPaginator;

		// Token: 0x04003A23 RID: 14883
		private double _pageZoom;

		// Token: 0x04003A24 RID: 14884
		private DocumentPage _documentPage;

		// Token: 0x04003A25 RID: 14885
		private DocumentPage _documentPageAsync;

		// Token: 0x04003A26 RID: 14886
		private DocumentPageTextView _textView;

		// Token: 0x04003A27 RID: 14887
		private DocumentPageHost _pageHost;

		// Token: 0x04003A28 RID: 14888
		private Visual _pageVisualClone;

		// Token: 0x04003A29 RID: 14889
		private Size _visualCloneSize;

		// Token: 0x04003A2A RID: 14890
		private bool _useAsynchronous = true;

		// Token: 0x04003A2B RID: 14891
		private bool _suspendLayout;

		// Token: 0x04003A2C RID: 14892
		private bool _disposed;

		// Token: 0x04003A2D RID: 14893
		private bool _newPageConnected;
	}
}
