using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020007D9 RID: 2009
	public class SoundPlayerAction : TriggerAction, IDisposable
	{
		// Token: 0x0600739E RID: 29598 RVA: 0x002E3CB6 File Offset: 0x002E2CB6
		public void Dispose()
		{
			if (this.m_player != null)
			{
				this.m_player.Dispose();
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600739F RID: 29599 RVA: 0x002E3CCB File Offset: 0x002E2CCB
		// (set) Token: 0x060073A0 RID: 29600 RVA: 0x002E3CDD File Offset: 0x002E2CDD
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(SoundPlayerAction.SourceProperty);
			}
			set
			{
				base.SetValue(SoundPlayerAction.SourceProperty, value);
			}
		}

		// Token: 0x060073A1 RID: 29601 RVA: 0x002E3CEB File Offset: 0x002E2CEB
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SoundPlayerAction)d).OnSourceChangedHelper((Uri)e.NewValue);
		}

		// Token: 0x060073A2 RID: 29602 RVA: 0x002E3D04 File Offset: 0x002E2D04
		private void OnSourceChangedHelper(Uri newValue)
		{
			if (newValue == null || newValue.IsAbsoluteUri)
			{
				this.m_lastRequestedAbsoluteUri = newValue;
			}
			else
			{
				this.m_lastRequestedAbsoluteUri = BaseUriHelper.GetResolvedUri(BaseUriHelper.BaseUri, newValue);
			}
			this.m_player = null;
			this.m_playRequested = false;
			if (this.m_streamLoadInProgress)
			{
				this.m_uriChangedWhileLoadingStream = true;
				return;
			}
			this.BeginLoadStream();
		}

		// Token: 0x060073A3 RID: 29603 RVA: 0x002E3D60 File Offset: 0x002E2D60
		internal sealed override void Invoke(FrameworkElement el, FrameworkContentElement ctntEl, Style targetStyle, FrameworkTemplate targetTemplate, long layer)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x060073A4 RID: 29604 RVA: 0x002E3D60 File Offset: 0x002E2D60
		internal sealed override void Invoke(FrameworkElement el)
		{
			this.PlayWhenLoaded();
		}

		// Token: 0x060073A5 RID: 29605 RVA: 0x002E3D68 File Offset: 0x002E2D68
		private void PlayWhenLoaded()
		{
			if (this.m_streamLoadInProgress)
			{
				this.m_playRequested = true;
				return;
			}
			if (this.m_player != null)
			{
				this.m_player.Play();
			}
		}

		// Token: 0x060073A6 RID: 29606 RVA: 0x002E3D8D File Offset: 0x002E2D8D
		private void BeginLoadStream()
		{
			if (this.m_lastRequestedAbsoluteUri != null)
			{
				this.m_streamLoadInProgress = true;
				Task.Run(delegate()
				{
					Stream asyncResult = WpfWebRequestHelper.CreateRequestAndGetResponseStream(this.m_lastRequestedAbsoluteUri);
					this.LoadStreamCallback(asyncResult);
				});
			}
		}

		// Token: 0x060073A7 RID: 29607 RVA: 0x002E3DB6 File Offset: 0x002E2DB6
		private Stream LoadStreamAsync(Uri uri)
		{
			return WpfWebRequestHelper.CreateRequestAndGetResponseStream(uri);
		}

		// Token: 0x060073A8 RID: 29608 RVA: 0x002E3DC0 File Offset: 0x002E2DC0
		private void LoadStreamCallback(Stream asyncResult)
		{
			DispatcherOperationCallback method = new DispatcherOperationCallback(this.OnLoadStreamCompleted);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Normal, method, asyncResult);
		}

		// Token: 0x060073A9 RID: 29609 RVA: 0x002E3DEC File Offset: 0x002E2DEC
		private object OnLoadStreamCompleted(object asyncResultArg)
		{
			Stream stream = (Stream)asyncResultArg;
			if (this.m_uriChangedWhileLoadingStream)
			{
				this.m_uriChangedWhileLoadingStream = false;
				if (stream != null)
				{
					stream.Dispose();
				}
				this.BeginLoadStream();
			}
			else if (stream != null)
			{
				if (this.m_player == null)
				{
					this.m_player = new SoundPlayer(stream);
				}
				else
				{
					this.m_player.Stream = stream;
				}
				this.m_player.LoadCompleted += this.OnSoundPlayerLoadCompleted;
				this.m_player.LoadAsync();
			}
			return null;
		}

		// Token: 0x060073AA RID: 29610 RVA: 0x002E3E68 File Offset: 0x002E2E68
		private void OnSoundPlayerLoadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (this.m_player == sender)
			{
				if (this.m_uriChangedWhileLoadingStream)
				{
					this.m_player = null;
					this.m_uriChangedWhileLoadingStream = false;
					this.BeginLoadStream();
					return;
				}
				this.m_streamLoadInProgress = false;
				if (this.m_playRequested)
				{
					this.m_playRequested = false;
					this.m_player.Play();
				}
			}
		}

		// Token: 0x040037DC RID: 14300
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(SoundPlayerAction), new FrameworkPropertyMetadata(new PropertyChangedCallback(SoundPlayerAction.OnSourceChanged)));

		// Token: 0x040037DD RID: 14301
		private SoundPlayer m_player;

		// Token: 0x040037DE RID: 14302
		private Uri m_lastRequestedAbsoluteUri;

		// Token: 0x040037DF RID: 14303
		private bool m_streamLoadInProgress;

		// Token: 0x040037E0 RID: 14304
		private bool m_playRequested;

		// Token: 0x040037E1 RID: 14305
		private bool m_uriChangedWhileLoadingStream;

		// Token: 0x02000C1F RID: 3103
		// (Invoke) Token: 0x0600908E RID: 37006
		private delegate Stream LoadStreamCaller(Uri uri);
	}
}
