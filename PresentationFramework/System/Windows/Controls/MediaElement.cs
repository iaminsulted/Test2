using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007AC RID: 1964
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class MediaElement : FrameworkElement, IUriContext
	{
		// Token: 0x06006EDB RID: 28379 RVA: 0x002D3326 File Offset: 0x002D2326
		public MediaElement()
		{
			this.Initialize();
		}

		// Token: 0x06006EDC RID: 28380 RVA: 0x002D3334 File Offset: 0x002D2334
		static MediaElement()
		{
			MediaElement.MediaFailedEvent = EventManager.RegisterRoutedEvent("MediaFailed", RoutingStrategy.Bubble, typeof(EventHandler<ExceptionRoutedEventArgs>), typeof(MediaElement));
			MediaElement.MediaOpenedEvent = EventManager.RegisterRoutedEvent("MediaOpened", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.BufferingStartedEvent = EventManager.RegisterRoutedEvent("BufferingStarted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.BufferingEndedEvent = EventManager.RegisterRoutedEvent("BufferingEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			MediaElement.ScriptCommandEvent = EventManager.RegisterRoutedEvent("ScriptCommand", RoutingStrategy.Bubble, typeof(EventHandler<MediaScriptCommandRoutedEventArgs>), typeof(MediaElement));
			MediaElement.MediaEndedEvent = EventManager.RegisterRoutedEvent("MediaEnded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MediaElement));
			Style defaultValue = MediaElement.CreateDefaultStyles();
			FrameworkElement.StyleProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(defaultValue));
			MediaElement.StretchProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(Stretch.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));
			MediaElement.StretchDirectionProperty.OverrideMetadata(typeof(MediaElement), new FrameworkPropertyMetadata(StretchDirection.Both, FrameworkPropertyMetadataOptions.AffectsMeasure));
			ControlsTraceLogger.AddControl(TelemetryControls.MediaElement);
		}

		// Token: 0x06006EDD RID: 28381 RVA: 0x002D365F File Offset: 0x002D265F
		private static Style CreateDefaultStyles()
		{
			Style style = new Style(typeof(MediaElement), null);
			style.Setters.Add(new Setter(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight));
			style.Seal();
			return style;
		}

		// Token: 0x17001991 RID: 6545
		// (get) Token: 0x06006EDE RID: 28382 RVA: 0x002D3692 File Offset: 0x002D2692
		// (set) Token: 0x06006EDF RID: 28383 RVA: 0x002D36A4 File Offset: 0x002D26A4
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(MediaElement.SourceProperty);
			}
			set
			{
				base.SetValue(MediaElement.SourceProperty, value);
			}
		}

		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x06006EE0 RID: 28384 RVA: 0x002D36B2 File Offset: 0x002D26B2
		// (set) Token: 0x06006EE1 RID: 28385 RVA: 0x002D36BF File Offset: 0x002D26BF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MediaClock Clock
		{
			get
			{
				return this._helper.Clock;
			}
			set
			{
				this._helper.SetClock(value);
			}
		}

		// Token: 0x06006EE2 RID: 28386 RVA: 0x002D36CD File Offset: 0x002D26CD
		public void Play()
		{
			this._helper.SetState(MediaState.Play);
		}

		// Token: 0x06006EE3 RID: 28387 RVA: 0x002D36DB File Offset: 0x002D26DB
		public void Pause()
		{
			this._helper.SetState(MediaState.Pause);
		}

		// Token: 0x06006EE4 RID: 28388 RVA: 0x002D36E9 File Offset: 0x002D26E9
		public void Stop()
		{
			this._helper.SetState(MediaState.Stop);
		}

		// Token: 0x06006EE5 RID: 28389 RVA: 0x002D36F7 File Offset: 0x002D26F7
		public void Close()
		{
			this._helper.SetState(MediaState.Close);
		}

		// Token: 0x17001993 RID: 6547
		// (get) Token: 0x06006EE6 RID: 28390 RVA: 0x002D3705 File Offset: 0x002D2705
		// (set) Token: 0x06006EE7 RID: 28391 RVA: 0x002D3717 File Offset: 0x002D2717
		public Stretch Stretch
		{
			get
			{
				return (Stretch)base.GetValue(MediaElement.StretchProperty);
			}
			set
			{
				base.SetValue(MediaElement.StretchProperty, value);
			}
		}

		// Token: 0x17001994 RID: 6548
		// (get) Token: 0x06006EE8 RID: 28392 RVA: 0x002D372A File Offset: 0x002D272A
		// (set) Token: 0x06006EE9 RID: 28393 RVA: 0x002D373C File Offset: 0x002D273C
		public StretchDirection StretchDirection
		{
			get
			{
				return (StretchDirection)base.GetValue(MediaElement.StretchDirectionProperty);
			}
			set
			{
				base.SetValue(MediaElement.StretchDirectionProperty, value);
			}
		}

		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x06006EEA RID: 28394 RVA: 0x002D374F File Offset: 0x002D274F
		// (set) Token: 0x06006EEB RID: 28395 RVA: 0x002D3761 File Offset: 0x002D2761
		public double Volume
		{
			get
			{
				return (double)base.GetValue(MediaElement.VolumeProperty);
			}
			set
			{
				base.SetValue(MediaElement.VolumeProperty, value);
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x06006EEC RID: 28396 RVA: 0x002D3774 File Offset: 0x002D2774
		// (set) Token: 0x06006EED RID: 28397 RVA: 0x002D3786 File Offset: 0x002D2786
		public double Balance
		{
			get
			{
				return (double)base.GetValue(MediaElement.BalanceProperty);
			}
			set
			{
				base.SetValue(MediaElement.BalanceProperty, value);
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x06006EEE RID: 28398 RVA: 0x002D3799 File Offset: 0x002D2799
		// (set) Token: 0x06006EEF RID: 28399 RVA: 0x002D37AB File Offset: 0x002D27AB
		public bool IsMuted
		{
			get
			{
				return (bool)base.GetValue(MediaElement.IsMutedProperty);
			}
			set
			{
				base.SetValue(MediaElement.IsMutedProperty, value);
			}
		}

		// Token: 0x17001998 RID: 6552
		// (get) Token: 0x06006EF0 RID: 28400 RVA: 0x002D37B9 File Offset: 0x002D27B9
		// (set) Token: 0x06006EF1 RID: 28401 RVA: 0x002D37CB File Offset: 0x002D27CB
		public bool ScrubbingEnabled
		{
			get
			{
				return (bool)base.GetValue(MediaElement.ScrubbingEnabledProperty);
			}
			set
			{
				base.SetValue(MediaElement.ScrubbingEnabledProperty, value);
			}
		}

		// Token: 0x17001999 RID: 6553
		// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x002D37D9 File Offset: 0x002D27D9
		// (set) Token: 0x06006EF3 RID: 28403 RVA: 0x002D37EB File Offset: 0x002D27EB
		public MediaState UnloadedBehavior
		{
			get
			{
				return (MediaState)base.GetValue(MediaElement.UnloadedBehaviorProperty);
			}
			set
			{
				base.SetValue(MediaElement.UnloadedBehaviorProperty, value);
			}
		}

		// Token: 0x1700199A RID: 6554
		// (get) Token: 0x06006EF4 RID: 28404 RVA: 0x002D37FE File Offset: 0x002D27FE
		// (set) Token: 0x06006EF5 RID: 28405 RVA: 0x002D3810 File Offset: 0x002D2810
		public MediaState LoadedBehavior
		{
			get
			{
				return (MediaState)base.GetValue(MediaElement.LoadedBehaviorProperty);
			}
			set
			{
				base.SetValue(MediaElement.LoadedBehaviorProperty, value);
			}
		}

		// Token: 0x1700199B RID: 6555
		// (get) Token: 0x06006EF6 RID: 28406 RVA: 0x002D3823 File Offset: 0x002D2823
		public bool CanPause
		{
			get
			{
				return this._helper.Player.CanPause;
			}
		}

		// Token: 0x1700199C RID: 6556
		// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x002D3835 File Offset: 0x002D2835
		public bool IsBuffering
		{
			get
			{
				return this._helper.Player.IsBuffering;
			}
		}

		// Token: 0x1700199D RID: 6557
		// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x002D3847 File Offset: 0x002D2847
		public double DownloadProgress
		{
			get
			{
				return this._helper.Player.DownloadProgress;
			}
		}

		// Token: 0x1700199E RID: 6558
		// (get) Token: 0x06006EF9 RID: 28409 RVA: 0x002D3859 File Offset: 0x002D2859
		public double BufferingProgress
		{
			get
			{
				return this._helper.Player.BufferingProgress;
			}
		}

		// Token: 0x1700199F RID: 6559
		// (get) Token: 0x06006EFA RID: 28410 RVA: 0x002D386B File Offset: 0x002D286B
		public int NaturalVideoHeight
		{
			get
			{
				return this._helper.Player.NaturalVideoHeight;
			}
		}

		// Token: 0x170019A0 RID: 6560
		// (get) Token: 0x06006EFB RID: 28411 RVA: 0x002D387D File Offset: 0x002D287D
		public int NaturalVideoWidth
		{
			get
			{
				return this._helper.Player.NaturalVideoWidth;
			}
		}

		// Token: 0x170019A1 RID: 6561
		// (get) Token: 0x06006EFC RID: 28412 RVA: 0x002D388F File Offset: 0x002D288F
		public bool HasAudio
		{
			get
			{
				return this._helper.Player.HasAudio;
			}
		}

		// Token: 0x170019A2 RID: 6562
		// (get) Token: 0x06006EFD RID: 28413 RVA: 0x002D38A1 File Offset: 0x002D28A1
		public bool HasVideo
		{
			get
			{
				return this._helper.Player.HasVideo;
			}
		}

		// Token: 0x170019A3 RID: 6563
		// (get) Token: 0x06006EFE RID: 28414 RVA: 0x002D38B3 File Offset: 0x002D28B3
		public Duration NaturalDuration
		{
			get
			{
				return this._helper.Player.NaturalDuration;
			}
		}

		// Token: 0x170019A4 RID: 6564
		// (get) Token: 0x06006EFF RID: 28415 RVA: 0x002D38C5 File Offset: 0x002D28C5
		// (set) Token: 0x06006F00 RID: 28416 RVA: 0x002D38D2 File Offset: 0x002D28D2
		public TimeSpan Position
		{
			get
			{
				return this._helper.Position;
			}
			set
			{
				this._helper.SetPosition(value);
			}
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x06006F01 RID: 28417 RVA: 0x002D38E0 File Offset: 0x002D28E0
		// (set) Token: 0x06006F02 RID: 28418 RVA: 0x002D38ED File Offset: 0x002D28ED
		public double SpeedRatio
		{
			get
			{
				return this._helper.SpeedRatio;
			}
			set
			{
				this._helper.SetSpeedRatio(value);
			}
		}

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x06006F03 RID: 28419 RVA: 0x002D38FB File Offset: 0x002D28FB
		// (remove) Token: 0x06006F04 RID: 28420 RVA: 0x002D3909 File Offset: 0x002D2909
		public event EventHandler<ExceptionRoutedEventArgs> MediaFailed
		{
			add
			{
				base.AddHandler(MediaElement.MediaFailedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaFailedEvent, value);
			}
		}

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x06006F05 RID: 28421 RVA: 0x002D3917 File Offset: 0x002D2917
		// (remove) Token: 0x06006F06 RID: 28422 RVA: 0x002D3925 File Offset: 0x002D2925
		public event RoutedEventHandler MediaOpened
		{
			add
			{
				base.AddHandler(MediaElement.MediaOpenedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaOpenedEvent, value);
			}
		}

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x06006F07 RID: 28423 RVA: 0x002D3933 File Offset: 0x002D2933
		// (remove) Token: 0x06006F08 RID: 28424 RVA: 0x002D3941 File Offset: 0x002D2941
		public event RoutedEventHandler BufferingStarted
		{
			add
			{
				base.AddHandler(MediaElement.BufferingStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.BufferingStartedEvent, value);
			}
		}

		// Token: 0x14000138 RID: 312
		// (add) Token: 0x06006F09 RID: 28425 RVA: 0x002D394F File Offset: 0x002D294F
		// (remove) Token: 0x06006F0A RID: 28426 RVA: 0x002D395D File Offset: 0x002D295D
		public event RoutedEventHandler BufferingEnded
		{
			add
			{
				base.AddHandler(MediaElement.BufferingEndedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.BufferingEndedEvent, value);
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06006F0B RID: 28427 RVA: 0x002D396B File Offset: 0x002D296B
		// (remove) Token: 0x06006F0C RID: 28428 RVA: 0x002D3979 File Offset: 0x002D2979
		public event EventHandler<MediaScriptCommandRoutedEventArgs> ScriptCommand
		{
			add
			{
				base.AddHandler(MediaElement.ScriptCommandEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.ScriptCommandEvent, value);
			}
		}

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06006F0D RID: 28429 RVA: 0x002D3987 File Offset: 0x002D2987
		// (remove) Token: 0x06006F0E RID: 28430 RVA: 0x002D3995 File Offset: 0x002D2995
		public event RoutedEventHandler MediaEnded
		{
			add
			{
				base.AddHandler(MediaElement.MediaEndedEvent, value);
			}
			remove
			{
				base.RemoveHandler(MediaElement.MediaEndedEvent, value);
			}
		}

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x06006F0F RID: 28431 RVA: 0x002D39A3 File Offset: 0x002D29A3
		// (set) Token: 0x06006F10 RID: 28432 RVA: 0x002D39B0 File Offset: 0x002D29B0
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._helper.BaseUri;
			}
			set
			{
				this._helper.BaseUri = value;
			}
		}

		// Token: 0x06006F11 RID: 28433 RVA: 0x002D39BE File Offset: 0x002D29BE
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MediaElementAutomationPeer(this);
		}

		// Token: 0x06006F12 RID: 28434 RVA: 0x002D39C6 File Offset: 0x002D29C6
		protected override Size MeasureOverride(Size availableSize)
		{
			return this.MeasureArrangeHelper(availableSize);
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x002D39C6 File Offset: 0x002D29C6
		protected override Size ArrangeOverride(Size finalSize)
		{
			return this.MeasureArrangeHelper(finalSize);
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x002D39D0 File Offset: 0x002D29D0
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (this._helper.Player == null)
			{
				return;
			}
			drawingContext.DrawVideo(this._helper.Player, new Rect(default(Point), base.RenderSize));
		}

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x06006F15 RID: 28437 RVA: 0x002D3A10 File Offset: 0x002D2A10
		internal AVElementHelper Helper
		{
			get
			{
				return this._helper;
			}
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x002D3A18 File Offset: 0x002D2A18
		private void Initialize()
		{
			this._helper = new AVElementHelper(this);
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x002D3A28 File Offset: 0x002D2A28
		private Size MeasureArrangeHelper(Size inputSize)
		{
			MediaPlayer player = this._helper.Player;
			if (player == null)
			{
				return default(Size);
			}
			Size contentSize = new Size((double)player.NaturalVideoWidth, (double)player.NaturalVideoHeight);
			Size size = Viewbox.ComputeScaleFactor(inputSize, contentSize, this.Stretch, this.StretchDirection);
			return new Size(contentSize.Width * size.Width, contentSize.Height * size.Height);
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x002D3A9C File Offset: 0x002D2A9C
		private static void VolumePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetVolume((double)e.NewValue);
			}
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x002D3AD4 File Offset: 0x002D2AD4
		private static void BalancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetBalance((double)e.NewValue);
			}
		}

		// Token: 0x06006F1A RID: 28442 RVA: 0x002D3B0C File Offset: 0x002D2B0C
		private static void IsMutedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetIsMuted((bool)e.NewValue);
			}
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x002D3B44 File Offset: 0x002D2B44
		private static void ScrubbingEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetScrubbingEnabled((bool)e.NewValue);
			}
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x002D3B7C File Offset: 0x002D2B7C
		private static void UnloadedBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetUnloadedBehavior((MediaState)e.NewValue);
			}
		}

		// Token: 0x06006F1D RID: 28445 RVA: 0x002D3BB4 File Offset: 0x002D2BB4
		private static void LoadedBehaviorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			MediaElement mediaElement = (MediaElement)d;
			if (mediaElement != null)
			{
				mediaElement._helper.SetLoadedBehavior((MediaState)e.NewValue);
			}
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x002D3BEC File Offset: 0x002D2BEC
		internal void OnMediaFailed(object sender, ExceptionEventArgs args)
		{
			base.RaiseEvent(new ExceptionRoutedEventArgs(MediaElement.MediaFailedEvent, this, args.ErrorException));
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x002D3C05 File Offset: 0x002D2C05
		internal void OnMediaOpened(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.MediaOpenedEvent, this));
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x002D3C18 File Offset: 0x002D2C18
		internal void OnBufferingStarted(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.BufferingStartedEvent, this));
		}

		// Token: 0x06006F21 RID: 28449 RVA: 0x002D3C2B File Offset: 0x002D2C2B
		internal void OnBufferingEnded(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.BufferingEndedEvent, this));
		}

		// Token: 0x06006F22 RID: 28450 RVA: 0x002D3C3E File Offset: 0x002D2C3E
		internal void OnMediaEnded(object sender, EventArgs args)
		{
			base.RaiseEvent(new RoutedEventArgs(MediaElement.MediaEndedEvent, this));
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x002D3C51 File Offset: 0x002D2C51
		internal void OnScriptCommand(object sender, MediaScriptCommandEventArgs args)
		{
			base.RaiseEvent(new MediaScriptCommandRoutedEventArgs(MediaElement.ScriptCommandEvent, this, args.ParameterType, args.ParameterValue));
		}

		// Token: 0x04003674 RID: 13940
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(MediaElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(AVElementHelper.OnSourceChanged)));

		// Token: 0x04003675 RID: 13941
		public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register("Volume", typeof(double), typeof(MediaElement), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.VolumePropertyChanged)));

		// Token: 0x04003676 RID: 13942
		public static readonly DependencyProperty BalanceProperty = DependencyProperty.Register("Balance", typeof(double), typeof(MediaElement), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.BalancePropertyChanged)));

		// Token: 0x04003677 RID: 13943
		public static readonly DependencyProperty IsMutedProperty = DependencyProperty.Register("IsMuted", typeof(bool), typeof(MediaElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.IsMutedPropertyChanged)));

		// Token: 0x04003678 RID: 13944
		public static readonly DependencyProperty ScrubbingEnabledProperty = DependencyProperty.Register("ScrubbingEnabled", typeof(bool), typeof(MediaElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.ScrubbingEnabledPropertyChanged)));

		// Token: 0x04003679 RID: 13945
		public static readonly DependencyProperty UnloadedBehaviorProperty = DependencyProperty.Register("UnloadedBehavior", typeof(MediaState), typeof(MediaElement), new FrameworkPropertyMetadata(MediaState.Close, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.UnloadedBehaviorPropertyChanged)));

		// Token: 0x0400367A RID: 13946
		public static readonly DependencyProperty LoadedBehaviorProperty = DependencyProperty.Register("LoadedBehavior", typeof(MediaState), typeof(MediaElement), new FrameworkPropertyMetadata(MediaState.Play, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(MediaElement.LoadedBehaviorPropertyChanged)));

		// Token: 0x0400367B RID: 13947
		public static readonly DependencyProperty StretchProperty = Viewbox.StretchProperty.AddOwner(typeof(MediaElement));

		// Token: 0x0400367C RID: 13948
		public static readonly DependencyProperty StretchDirectionProperty = Viewbox.StretchDirectionProperty.AddOwner(typeof(MediaElement));

		// Token: 0x04003683 RID: 13955
		private AVElementHelper _helper;
	}
}
