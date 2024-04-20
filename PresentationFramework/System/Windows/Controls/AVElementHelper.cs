using System;
using System.IO.Packaging;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200071B RID: 1819
	internal class AVElementHelper
	{
		// Token: 0x06005F96 RID: 24470 RVA: 0x002956A0 File Offset: 0x002946A0
		internal AVElementHelper(MediaElement element)
		{
			this._element = element;
			this._position = new SettableState<TimeSpan>(new TimeSpan(0L));
			this._mediaState = new SettableState<MediaState>(MediaState.Close);
			this._source = new SettableState<Uri>(null);
			this._clock = new SettableState<MediaClock>(null);
			this._speedRatio = new SettableState<double>(1.0);
			this._volume = new SettableState<double>(0.5);
			this._isMuted = new SettableState<bool>(false);
			this._balance = new SettableState<double>(0.0);
			this._isScrubbingEnabled = new SettableState<bool>(false);
			this._mediaPlayer = new MediaPlayer();
			this.HookEvents();
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x0029576C File Offset: 0x0029476C
		internal static AVElementHelper GetHelper(DependencyObject d)
		{
			MediaElement mediaElement = d as MediaElement;
			if (mediaElement != null)
			{
				return mediaElement.Helper;
			}
			throw new ArgumentException(SR.Get("AudioVideo_InvalidDependencyObject"));
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x06005F98 RID: 24472 RVA: 0x00295799 File Offset: 0x00294799
		internal MediaPlayer Player
		{
			get
			{
				return this._mediaPlayer;
			}
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06005F99 RID: 24473 RVA: 0x002957A1 File Offset: 0x002947A1
		// (set) Token: 0x06005F9A RID: 24474 RVA: 0x002957A9 File Offset: 0x002947A9
		internal Uri BaseUri
		{
			get
			{
				return this._baseUri;
			}
			set
			{
				if (value.Scheme != PackUriHelper.UriSchemePack)
				{
					this._baseUri = value;
					return;
				}
				this._baseUri = null;
			}
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x002957CC File Offset: 0x002947CC
		internal void SetUnloadedBehavior(MediaState unloadedBehavior)
		{
			this._unloadedBehavior = unloadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x002957DB File Offset: 0x002947DB
		internal void SetLoadedBehavior(MediaState loadedBehavior)
		{
			this._loadedBehavior = loadedBehavior;
			this.HandleStateChange();
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06005F9D RID: 24477 RVA: 0x002957EA File Offset: 0x002947EA
		internal TimeSpan Position
		{
			get
			{
				if (this._currentState == MediaState.Close)
				{
					return this._position._value;
				}
				return this._mediaPlayer.Position;
			}
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x0029580C File Offset: 0x0029480C
		internal void SetPosition(TimeSpan position)
		{
			this._position._isSet = true;
			this._position._value = position;
			this.HandleStateChange();
		}

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06005F9F RID: 24479 RVA: 0x0029582C File Offset: 0x0029482C
		internal MediaClock Clock
		{
			get
			{
				return this._clock._value;
			}
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x00295839 File Offset: 0x00294839
		internal void SetClock(MediaClock clock)
		{
			this._clock._value = clock;
			this._clock._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06005FA1 RID: 24481 RVA: 0x00295859 File Offset: 0x00294859
		internal double SpeedRatio
		{
			get
			{
				return this._speedRatio._value;
			}
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x00295868 File Offset: 0x00294868
		internal void SetSpeedRatio(double speedRatio)
		{
			this._speedRatio._wasSet = (this._speedRatio._isSet = true);
			this._speedRatio._value = speedRatio;
			this.HandleStateChange();
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x002958A1 File Offset: 0x002948A1
		internal void SetState(MediaState mediaState)
		{
			if (this._loadedBehavior != MediaState.Manual && this._unloadedBehavior != MediaState.Manual)
			{
				throw new NotSupportedException(SR.Get("AudioVideo_CannotControlMedia"));
			}
			this._mediaState._value = mediaState;
			this._mediaState._isSet = true;
			this.HandleStateChange();
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x002958E4 File Offset: 0x002948E4
		internal void SetVolume(double volume)
		{
			this._volume._wasSet = (this._volume._isSet = true);
			this._volume._value = volume;
			this.HandleStateChange();
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x00295920 File Offset: 0x00294920
		internal void SetBalance(double balance)
		{
			this._balance._wasSet = (this._balance._isSet = true);
			this._balance._value = balance;
			this.HandleStateChange();
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x0029595C File Offset: 0x0029495C
		internal void SetIsMuted(bool isMuted)
		{
			this._isMuted._wasSet = (this._isMuted._isSet = true);
			this._isMuted._value = isMuted;
			this.HandleStateChange();
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x00295998 File Offset: 0x00294998
		internal void SetScrubbingEnabled(bool isScrubbingEnabled)
		{
			this._isScrubbingEnabled._wasSet = (this._isScrubbingEnabled._isSet = true);
			this._isScrubbingEnabled._value = isScrubbingEnabled;
			this.HandleStateChange();
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x002959D4 File Offset: 0x002949D4
		private void HookEvents()
		{
			this._mediaPlayer.MediaOpened += this.OnMediaOpened;
			this._mediaPlayer.MediaFailed += this.OnMediaFailed;
			this._mediaPlayer.BufferingStarted += this.OnBufferingStarted;
			this._mediaPlayer.BufferingEnded += this.OnBufferingEnded;
			this._mediaPlayer.MediaEnded += this.OnMediaEnded;
			this._mediaPlayer.ScriptCommand += this.OnScriptCommand;
			this._element.Loaded += this.OnLoaded;
			this._element.Unloaded += this.OnUnloaded;
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x00295A9C File Offset: 0x00294A9C
		private void HandleStateChange()
		{
			MediaState mediaState = this._mediaState._value;
			bool flag = false;
			bool flag2 = false;
			if (this._isLoaded)
			{
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else if (this._loadedBehavior != MediaState.Manual)
				{
					mediaState = this._loadedBehavior;
				}
				else if (this._source._wasSet)
				{
					if (this._loadedBehavior != MediaState.Manual)
					{
						mediaState = MediaState.Play;
					}
					else
					{
						flag2 = true;
					}
				}
			}
			else if (this._unloadedBehavior != MediaState.Manual)
			{
				mediaState = this._unloadedBehavior;
			}
			else
			{
				Invariant.Assert(this._unloadedBehavior == MediaState.Manual);
				if (this._clock._value != null)
				{
					mediaState = MediaState.Manual;
					flag = true;
				}
				else
				{
					flag2 = true;
				}
			}
			bool flag3 = false;
			if (mediaState != MediaState.Close && mediaState != MediaState.Manual)
			{
				Invariant.Assert(!flag);
				if (this._mediaPlayer.Clock != null)
				{
					this._mediaPlayer.Clock = null;
				}
				if (this._currentState == MediaState.Close || this._source._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					if (this._clock._value == null)
					{
						this._mediaPlayer.Open(this.UriFromSourceUri(this._source._value));
					}
					flag3 = true;
				}
			}
			else if (flag)
			{
				if (this._currentState == MediaState.Close || this._clock._isSet)
				{
					if (this._isScrubbingEnabled._wasSet)
					{
						this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
						this._isScrubbingEnabled._isSet = false;
					}
					this._mediaPlayer.Clock = this._clock._value;
					this._clock._isSet = false;
					flag3 = true;
				}
			}
			else if (mediaState == MediaState.Close && this._currentState != MediaState.Close)
			{
				this._mediaPlayer.Clock = null;
				this._mediaPlayer.Close();
				this._currentState = MediaState.Close;
			}
			if (this._currentState != MediaState.Close || flag3)
			{
				if (this._position._isSet)
				{
					this._mediaPlayer.Position = this._position._value;
					this._position._isSet = false;
				}
				if (this._volume._isSet || (flag3 && this._volume._wasSet))
				{
					this._mediaPlayer.Volume = this._volume._value;
					this._volume._isSet = false;
				}
				if (this._balance._isSet || (flag3 && this._balance._wasSet))
				{
					this._mediaPlayer.Balance = this._balance._value;
					this._balance._isSet = false;
				}
				if (this._isMuted._isSet || (flag3 && this._isMuted._wasSet))
				{
					this._mediaPlayer.IsMuted = this._isMuted._value;
					this._isMuted._isSet = false;
				}
				if (this._isScrubbingEnabled._isSet)
				{
					this._mediaPlayer.ScrubbingEnabled = this._isScrubbingEnabled._value;
					this._isScrubbingEnabled._isSet = false;
				}
				if (mediaState == MediaState.Play && this._source._isSet)
				{
					this._mediaPlayer.Play();
					if (!this._speedRatio._wasSet)
					{
						this._mediaPlayer.SpeedRatio = 1.0;
					}
					this._source._isSet = false;
					this._mediaState._isSet = false;
				}
				else if (this._currentState != mediaState || (flag2 && this._mediaState._isSet))
				{
					switch (mediaState)
					{
					case MediaState.Manual:
						goto IL_3BE;
					case MediaState.Play:
						this._mediaPlayer.Play();
						goto IL_3BE;
					case MediaState.Pause:
						this._mediaPlayer.Pause();
						goto IL_3BE;
					case MediaState.Stop:
						this._mediaPlayer.Stop();
						goto IL_3BE;
					}
					Invariant.Assert(false, "Unexpected state request.");
					IL_3BE:
					if (flag2)
					{
						this._mediaState._isSet = false;
					}
				}
				this._currentState = mediaState;
				if (this._speedRatio._isSet || (flag3 && this._speedRatio._wasSet))
				{
					this._mediaPlayer.SpeedRatio = this._speedRatio._value;
					this._speedRatio._isSet = false;
				}
			}
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x00295EBC File Offset: 0x00294EBC
		private Uri UriFromSourceUri(Uri sourceUri)
		{
			if (sourceUri != null)
			{
				if (sourceUri.IsAbsoluteUri)
				{
					return sourceUri;
				}
				if (this.BaseUri != null)
				{
					return new Uri(this.BaseUri, sourceUri);
				}
			}
			return sourceUri;
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x00295EED File Offset: 0x00294EED
		internal static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.IsASubPropertyChange)
			{
				return;
			}
			AVElementHelper.GetHelper(d).MemberOnInvalidateSource(e);
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x00295F08 File Offset: 0x00294F08
		private void MemberOnInvalidateSource(DependencyPropertyChangedEventArgs e)
		{
			if (this._clock._value != null)
			{
				throw new InvalidOperationException(SR.Get("MediaElement_CannotSetSourceOnMediaElementDrivenByClock"));
			}
			this._source._value = (Uri)e.NewValue;
			this._source._wasSet = (this._source._isSet = true);
			this.HandleStateChange();
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x00295F69 File Offset: 0x00294F69
		private void OnMediaFailed(object sender, ExceptionEventArgs args)
		{
			this._element.OnMediaFailed(sender, args);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x00295F78 File Offset: 0x00294F78
		private void OnMediaOpened(object sender, EventArgs args)
		{
			this._element.InvalidateMeasure();
			this._element.OnMediaOpened(sender, args);
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x00295F92 File Offset: 0x00294F92
		private void OnBufferingStarted(object sender, EventArgs args)
		{
			this._element.OnBufferingStarted(sender, args);
		}

		// Token: 0x06005FB0 RID: 24496 RVA: 0x00295FA1 File Offset: 0x00294FA1
		private void OnBufferingEnded(object sender, EventArgs args)
		{
			this._element.OnBufferingEnded(sender, args);
		}

		// Token: 0x06005FB1 RID: 24497 RVA: 0x00295FB0 File Offset: 0x00294FB0
		private void OnMediaEnded(object sender, EventArgs args)
		{
			this._element.OnMediaEnded(sender, args);
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x00295FBF File Offset: 0x00294FBF
		private void OnScriptCommand(object sender, MediaScriptCommandEventArgs args)
		{
			this._element.OnScriptCommand(sender, args);
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x00295FCE File Offset: 0x00294FCE
		private void OnLoaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = true;
			this.HandleStateChange();
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x00295FDD File Offset: 0x00294FDD
		private void OnUnloaded(object sender, RoutedEventArgs args)
		{
			this._isLoaded = false;
			this.HandleStateChange();
		}

		// Token: 0x040031DA RID: 12762
		private MediaPlayer _mediaPlayer;

		// Token: 0x040031DB RID: 12763
		private MediaElement _element;

		// Token: 0x040031DC RID: 12764
		private Uri _baseUri;

		// Token: 0x040031DD RID: 12765
		private MediaState _unloadedBehavior = MediaState.Close;

		// Token: 0x040031DE RID: 12766
		private MediaState _loadedBehavior = MediaState.Play;

		// Token: 0x040031DF RID: 12767
		private MediaState _currentState = MediaState.Close;

		// Token: 0x040031E0 RID: 12768
		private bool _isLoaded;

		// Token: 0x040031E1 RID: 12769
		private SettableState<TimeSpan> _position;

		// Token: 0x040031E2 RID: 12770
		private SettableState<MediaState> _mediaState;

		// Token: 0x040031E3 RID: 12771
		private SettableState<Uri> _source;

		// Token: 0x040031E4 RID: 12772
		private SettableState<MediaClock> _clock;

		// Token: 0x040031E5 RID: 12773
		private SettableState<double> _speedRatio;

		// Token: 0x040031E6 RID: 12774
		private SettableState<double> _volume;

		// Token: 0x040031E7 RID: 12775
		private SettableState<bool> _isMuted;

		// Token: 0x040031E8 RID: 12776
		private SettableState<double> _balance;

		// Token: 0x040031E9 RID: 12777
		private SettableState<bool> _isScrubbingEnabled;
	}
}
