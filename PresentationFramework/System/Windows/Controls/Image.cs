using System;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MS.Internal.PresentationFramework;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x02000793 RID: 1939
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Image : FrameworkElement, IUriContext, IProvidePropertyFallback
	{
		// Token: 0x170018ED RID: 6381
		// (get) Token: 0x06006BCA RID: 27594 RVA: 0x002C6E46 File Offset: 0x002C5E46
		// (set) Token: 0x06006BCB RID: 27595 RVA: 0x002C6E58 File Offset: 0x002C5E58
		public ImageSource Source
		{
			get
			{
				return (ImageSource)base.GetValue(Image.SourceProperty);
			}
			set
			{
				base.SetValue(Image.SourceProperty, value);
			}
		}

		// Token: 0x170018EE RID: 6382
		// (get) Token: 0x06006BCC RID: 27596 RVA: 0x002C6E66 File Offset: 0x002C5E66
		// (set) Token: 0x06006BCD RID: 27597 RVA: 0x002C6E78 File Offset: 0x002C5E78
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(Image.StretchProperty);
			}
			set
			{
				base.SetValue(Image.StretchProperty, value);
			}
		}

		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x06006BCE RID: 27598 RVA: 0x002C6E8B File Offset: 0x002C5E8B
		// (set) Token: 0x06006BCF RID: 27599 RVA: 0x002C6E9D File Offset: 0x002C5E9D
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(Image.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(Image.StretchDirectionProperty, value);
			}
		}

		// Token: 0x1400011E RID: 286
		// (add) Token: 0x06006BD0 RID: 27600 RVA: 0x002C6EB0 File Offset: 0x002C5EB0
		// (remove) Token: 0x06006BD1 RID: 27601 RVA: 0x002C6EBE File Offset: 0x002C5EBE
		public event EventHandler<ExceptionRoutedEventArgs> ImageFailed
		{
			add
			{
				base.AddHandler(Image.ImageFailedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Image.ImageFailedEvent, value);
			}
		}

		// Token: 0x1400011F RID: 287
		// (add) Token: 0x06006BD2 RID: 27602 RVA: 0x002C6ECC File Offset: 0x002C5ECC
		// (remove) Token: 0x06006BD3 RID: 27603 RVA: 0x002C6EDA File Offset: 0x002C5EDA
		public event DpiChangedEventHandler DpiChanged
		{
			add
			{
				base.AddHandler(Image.DpiChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Image.DpiChangedEvent, value);
			}
		}

		// Token: 0x06006BD4 RID: 27604 RVA: 0x002C6EE8 File Offset: 0x002C5EE8
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ImageAutomationPeer(this);
		}

		// Token: 0x06006BD5 RID: 27605 RVA: 0x002C6EF0 File Offset: 0x002C5EF0
		protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
		{
			this._hasDpiChangedEverFired = true;
			base.RaiseEvent(new DpiChangedEventArgs(oldDpi, newDpi, Image.DpiChangedEvent, this));
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x002C6F0C File Offset: 0x002C5F0C
		protected override Size MeasureOverride(Size constraint)
		{
			if (!this._hasDpiChangedEverFired)
			{
				this._hasDpiChangedEverFired = true;
				DpiScale dpi = base.GetDpi();
				this.OnDpiChanged(dpi, dpi);
			}
			return this.MeasureArrangeHelper(constraint);
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x002C6F3E File Offset: 0x002C5F3E
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return this.MeasureArrangeHelper(arrangeSize);
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x002C6F48 File Offset: 0x002C5F48
		protected override void OnRender(DrawingContext dc)
		{
			ImageSource source = this.Source;
			if (source == null)
			{
				return;
			}
			dc.DrawImage(source, new Rect(default(Point), base.RenderSize));
		}

		// Token: 0x170018F0 RID: 6384
		// (get) Token: 0x06006BD9 RID: 27609 RVA: 0x002C6F7B File Offset: 0x002C5F7B
		// (set) Token: 0x06006BDA RID: 27610 RVA: 0x002C6F83 File Offset: 0x002C5F83
		Uri IUriContext.BaseUri
		{
			get
			{
				return this.BaseUri;
			}
			set
			{
				this.BaseUri = value;
			}
		}

		// Token: 0x170018F1 RID: 6385
		// (get) Token: 0x06006BDB RID: 27611 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06006BDC RID: 27612 RVA: 0x0022A4F5 File Offset: 0x002294F5
		protected virtual Uri BaseUri
		{
			get
			{
				return (Uri)base.GetValue(BaseUriHelper.BaseUriProperty);
			}
			set
			{
				base.SetValue(BaseUriHelper.BaseUriProperty, value);
			}
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x002C6F8C File Offset: 0x002C5F8C
		private Size MeasureArrangeHelper(Size inputSize)
		{
			ImageSource source = this.Source;
			Size size = default(Size);
			if (source == null)
			{
				return size;
			}
			try
			{
				Image.UpdateBaseUri(this, source);
				size = source.Size;
			}
			catch (Exception errorException)
			{
				base.SetCurrentValue(Image.SourceProperty, null);
				base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, errorException));
			}
			Size size2 = Viewbox.ComputeScaleFactor(inputSize, size, this.Stretch, this.StretchDirection);
			return new Size(size.Width * size2.Width, size.Height * size2.Height);
		}

		// Token: 0x170018F2 RID: 6386
		// (get) Token: 0x06006BDE RID: 27614 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x06006BDF RID: 27615 RVA: 0x002C7028 File Offset: 0x002C6028
		static Image()
		{
			Image.ImageFailedEvent = EventManager.RegisterRoutedEvent("ImageFailed", RoutingStrategy.Bubble, typeof(EventHandler<ExceptionRoutedEventArgs>), typeof(Image));
			Style defaultValue = Image.CreateDefaultStyles();
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(defaultValue));
			Image.StretchProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
			Image.StretchDirectionProperty.OverrideMetadata(typeof(Image), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
			Image.DpiChangedEvent = Window.DpiChangedEvent.AddOwner(typeof(Image));
			ControlsTraceLogger.AddControl(TelemetryControls.Image);
		}

		// Token: 0x06006BE0 RID: 27616 RVA: 0x002C7148 File Offset: 0x002C6148
		private static Style CreateDefaultStyles()
		{
			Style style = new Style(typeof(Image), null);
			style.Setters.Add(new Setter(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight));
			style.Seal();
			return style;
		}

		// Token: 0x06006BE1 RID: 27617 RVA: 0x002C717B File Offset: 0x002C617B
		private void OnSourceDownloaded(object sender, EventArgs e)
		{
			this.DetachBitmapSourceEvents();
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x002C718F File Offset: 0x002C618F
		private void OnSourceFailed(object sender, ExceptionEventArgs e)
		{
			this.DetachBitmapSourceEvents();
			base.SetCurrentValue(Image.SourceProperty, null);
			base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, e.ErrorException));
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x002C71BA File Offset: 0x002C61BA
		private void AttachBitmapSourceEvents(BitmapSource bitmapSource)
		{
			Image.DownloadCompletedEventManager.AddHandler(bitmapSource, new EventHandler<EventArgs>(this.OnSourceDownloaded));
			Image.DownloadFailedEventManager.AddHandler(bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
			Image.DecodeFailedEventManager.AddHandler(bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
			this._bitmapSource = bitmapSource;
		}

		// Token: 0x06006BE4 RID: 27620 RVA: 0x002C71FC File Offset: 0x002C61FC
		private void DetachBitmapSourceEvents()
		{
			if (this._bitmapSource != null)
			{
				Image.DownloadCompletedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<EventArgs>(this.OnSourceDownloaded));
				Image.DownloadFailedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
				Image.DecodeFailedEventManager.RemoveHandler(this._bitmapSource, new EventHandler<ExceptionEventArgs>(this.OnSourceFailed));
				this._bitmapSource = null;
			}
		}

		// Token: 0x06006BE5 RID: 27621 RVA: 0x002C7260 File Offset: 0x002C6260
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!e.IsASubPropertyChange)
			{
				Image image = (Image)d;
				ImageSource imageSource = (ImageSource)e.OldValue;
				ImageSource imageSource2 = (ImageSource)e.NewValue;
				Image.UpdateBaseUri(d, imageSource2);
				image.DetachBitmapSourceEvents();
				BitmapSource bitmapSource = imageSource2 as BitmapSource;
				if (bitmapSource != null && bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					image.AttachBitmapSourceEvents(bitmapSource);
				}
			}
		}

		// Token: 0x06006BE6 RID: 27622 RVA: 0x002C72C8 File Offset: 0x002C62C8
		private static void UpdateBaseUri(DependencyObject d, ImageSource source)
		{
			if (source is IUriContext && !source.IsFrozen && ((IUriContext)source).BaseUri == null && BaseUriHelper.GetBaseUriCore(d) != null)
			{
				((IUriContext)source).BaseUri = BaseUriHelper.GetBaseUriCore(d);
			}
		}

		// Token: 0x06006BE7 RID: 27623 RVA: 0x002C7317 File Offset: 0x002C6317
		bool IProvidePropertyFallback.CanProvidePropertyFallback(string property)
		{
			return string.CompareOrdinal(property, "Source") == 0;
		}

		// Token: 0x06006BE8 RID: 27624 RVA: 0x002C7329 File Offset: 0x002C6329
		object IProvidePropertyFallback.ProvidePropertyFallback(string property, Exception cause)
		{
			if (string.CompareOrdinal(property, "Source") == 0)
			{
				base.RaiseEvent(new ExceptionRoutedEventArgs(Image.ImageFailedEvent, this, cause));
			}
			return null;
		}

		// Token: 0x040035C5 RID: 13765
		[CommonDependencyProperty]
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(Image), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(Image.OnSourceChanged), null), null);

		// Token: 0x040035C7 RID: 13767
		[CommonDependencyProperty]
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(Image));

		// Token: 0x040035C8 RID: 13768
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(Image));

		// Token: 0x040035CA RID: 13770
		private BitmapSource _bitmapSource;

		// Token: 0x040035CB RID: 13771
		private bool _hasDpiChangedEverFired;

		// Token: 0x02000BF2 RID: 3058
		private class DownloadCompletedEventManager : WeakEventManager
		{
			// Token: 0x06008FCF RID: 36815 RVA: 0x0015A0BF File Offset: 0x001590BF
			private DownloadCompletedEventManager()
			{
			}

			// Token: 0x06008FD0 RID: 36816 RVA: 0x003455D9 File Offset: 0x003445D9
			public static void AddHandler(BitmapSource source, EventHandler<EventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadCompletedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x06008FD1 RID: 36817 RVA: 0x003455F5 File Offset: 0x003445F5
			public static void RemoveHandler(BitmapSource source, EventHandler<EventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadCompletedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x06008FD2 RID: 36818 RVA: 0x001E9F8E File Offset: 0x001E8F8E
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<EventArgs>();
			}

			// Token: 0x06008FD3 RID: 36819 RVA: 0x00345611 File Offset: 0x00344611
			protected override void StartListening(object source)
			{
				((BitmapSource)source).DownloadCompleted += this.OnDownloadCompleted;
			}

			// Token: 0x06008FD4 RID: 36820 RVA: 0x0034562C File Offset: 0x0034462C
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DownloadCompleted -= this.OnDownloadCompleted;
				}
			}

			// Token: 0x17001F70 RID: 8048
			// (get) Token: 0x06008FD5 RID: 36821 RVA: 0x00345664 File Offset: 0x00344664
			private static Image.DownloadCompletedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DownloadCompletedEventManager);
					Image.DownloadCompletedEventManager downloadCompletedEventManager = (Image.DownloadCompletedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (downloadCompletedEventManager == null)
					{
						downloadCompletedEventManager = new Image.DownloadCompletedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, downloadCompletedEventManager);
					}
					return downloadCompletedEventManager;
				}
			}

			// Token: 0x06008FD6 RID: 36822 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
			private void OnDownloadCompleted(object sender, EventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}

		// Token: 0x02000BF3 RID: 3059
		private class DownloadFailedEventManager : WeakEventManager
		{
			// Token: 0x06008FD7 RID: 36823 RVA: 0x0015A0BF File Offset: 0x001590BF
			private DownloadFailedEventManager()
			{
			}

			// Token: 0x06008FD8 RID: 36824 RVA: 0x00345699 File Offset: 0x00344699
			public static void AddHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadFailedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x06008FD9 RID: 36825 RVA: 0x003456B5 File Offset: 0x003446B5
			public static void RemoveHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DownloadFailedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x06008FDA RID: 36826 RVA: 0x003456D1 File Offset: 0x003446D1
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<ExceptionEventArgs>();
			}

			// Token: 0x06008FDB RID: 36827 RVA: 0x003456D8 File Offset: 0x003446D8
			protected override void StartListening(object source)
			{
				((BitmapSource)source).DownloadFailed += this.OnDownloadFailed;
			}

			// Token: 0x06008FDC RID: 36828 RVA: 0x003456F4 File Offset: 0x003446F4
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DownloadFailed -= this.OnDownloadFailed;
				}
			}

			// Token: 0x17001F71 RID: 8049
			// (get) Token: 0x06008FDD RID: 36829 RVA: 0x0034572C File Offset: 0x0034472C
			private static Image.DownloadFailedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DownloadFailedEventManager);
					Image.DownloadFailedEventManager downloadFailedEventManager = (Image.DownloadFailedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (downloadFailedEventManager == null)
					{
						downloadFailedEventManager = new Image.DownloadFailedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, downloadFailedEventManager);
					}
					return downloadFailedEventManager;
				}
			}

			// Token: 0x06008FDE RID: 36830 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
			private void OnDownloadFailed(object sender, ExceptionEventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}

		// Token: 0x02000BF4 RID: 3060
		private class DecodeFailedEventManager : WeakEventManager
		{
			// Token: 0x06008FDF RID: 36831 RVA: 0x0015A0BF File Offset: 0x001590BF
			private DecodeFailedEventManager()
			{
			}

			// Token: 0x06008FE0 RID: 36832 RVA: 0x00345761 File Offset: 0x00344761
			public static void AddHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DecodeFailedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
			}

			// Token: 0x06008FE1 RID: 36833 RVA: 0x0034577D File Offset: 0x0034477D
			public static void RemoveHandler(BitmapSource source, EventHandler<ExceptionEventArgs> handler)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				Image.DecodeFailedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
			}

			// Token: 0x06008FE2 RID: 36834 RVA: 0x003456D1 File Offset: 0x003446D1
			protected override WeakEventManager.ListenerList NewListenerList()
			{
				return new WeakEventManager.ListenerList<ExceptionEventArgs>();
			}

			// Token: 0x06008FE3 RID: 36835 RVA: 0x00345799 File Offset: 0x00344799
			protected override void StartListening(object source)
			{
				((BitmapSource)source).DecodeFailed += this.OnDecodeFailed;
			}

			// Token: 0x06008FE4 RID: 36836 RVA: 0x003457B4 File Offset: 0x003447B4
			protected override void StopListening(object source)
			{
				BitmapSource bitmapSource = (BitmapSource)source;
				if (bitmapSource.CheckAccess() && !bitmapSource.IsFrozen)
				{
					bitmapSource.DecodeFailed -= this.OnDecodeFailed;
				}
			}

			// Token: 0x17001F72 RID: 8050
			// (get) Token: 0x06008FE5 RID: 36837 RVA: 0x003457EC File Offset: 0x003447EC
			private static Image.DecodeFailedEventManager CurrentManager
			{
				get
				{
					Type typeFromHandle = typeof(Image.DecodeFailedEventManager);
					Image.DecodeFailedEventManager decodeFailedEventManager = (Image.DecodeFailedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
					if (decodeFailedEventManager == null)
					{
						decodeFailedEventManager = new Image.DecodeFailedEventManager();
						WeakEventManager.SetCurrentManager(typeFromHandle, decodeFailedEventManager);
					}
					return decodeFailedEventManager;
				}
			}

			// Token: 0x06008FE6 RID: 36838 RVA: 0x0015C2E5 File Offset: 0x0015B2E5
			private void OnDecodeFailed(object sender, ExceptionEventArgs args)
			{
				base.DeliverEvent(sender, args);
			}
		}
	}
}
