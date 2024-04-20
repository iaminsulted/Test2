using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal.AppModel;
using MS.Internal.KnownBoxes;
using MS.Internal.Utility;

namespace System.Windows.Navigation
{
	// Token: 0x020005CB RID: 1483
	[ContentProperty]
	[TemplatePart(Name = "PART_NavWinCP", Type = typeof(ContentPresenter))]
	public class NavigationWindow : Window, INavigator, INavigatorBase, INavigatorImpl, IDownloader, IJournalNavigationScopeHost, IUriContext
	{
		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06004781 RID: 18305 RVA: 0x0022A0E8 File Offset: 0x002290E8
		// (set) Token: 0x06004782 RID: 18306 RVA: 0x0022A0FC File Offset: 0x002290FC
		public bool SandboxExternalContent
		{
			get
			{
				return (bool)base.GetValue(NavigationWindow.SandboxExternalContentProperty);
			}
			set
			{
				base.SetValue(NavigationWindow.SandboxExternalContentProperty, value);
			}
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0022A118 File Offset: 0x00229118
		private static void OnSandboxExternalContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			if ((bool)e.NewValue && !(bool)e.OldValue)
			{
				navigationWindow.NavigationService.Refresh();
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x0022A153 File Offset: 0x00229153
		private static object CoerceSandBoxExternalContentValue(DependencyObject d, object value)
		{
			return (bool)value;
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0022A160 File Offset: 0x00229160
		static NavigationWindow()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(typeof(NavigationWindow)));
			ContentControl.ContentProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(null, new CoerceValueCallback(NavigationWindow.CoerceContent)));
			NavigationWindow.SandboxExternalContentProperty.OverrideMetadata(typeof(NavigationWindow), new FrameworkPropertyMetadata(new PropertyChangedCallback(NavigationWindow.OnSandboxExternalContentPropertyChanged), new CoerceValueCallback(NavigationWindow.CoerceSandBoxExternalContentValue)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseBack, new ExecutedRoutedEventHandler(NavigationWindow.OnGoBack), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryGoBack)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseForward, new ExecutedRoutedEventHandler(NavigationWindow.OnGoForward), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryGoForward)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.NavigateJournal, new ExecutedRoutedEventHandler(NavigationWindow.OnNavigateJournal)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.Refresh, new ExecutedRoutedEventHandler(NavigationWindow.OnRefresh), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryRefresh)));
			CommandManager.RegisterClassCommandBinding(typeof(NavigationWindow), new CommandBinding(NavigationCommands.BrowseStop, new ExecutedRoutedEventHandler(NavigationWindow.OnBrowseStop), new CanExecuteRoutedEventHandler(NavigationWindow.OnQueryBrowseStop)));
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0022A3B9 File Offset: 0x002293B9
		public NavigationWindow()
		{
			this.Initialize();
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x0022A3C7 File Offset: 0x002293C7
		internal NavigationWindow(bool inRbw) : base(inRbw)
		{
			this.Initialize();
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x0022A3D6 File Offset: 0x002293D6
		private void Initialize()
		{
			this._navigationService = new NavigationService(this);
			this._navigationService.BPReady += this.OnBPReady;
			this._JNS = new JournalNavigationScope(this);
			this._fFramelet = false;
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06004789 RID: 18313 RVA: 0x0022A40E File Offset: 0x0022940E
		NavigationService IDownloader.Downloader
		{
			get
			{
				return this._navigationService;
			}
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0022A416 File Offset: 0x00229416
		public bool Navigate(Uri source)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(source);
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x0022A42A File Offset: 0x0022942A
		public bool Navigate(Uri source, object extraData)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(source, extraData);
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x0022A43F File Offset: 0x0022943F
		public bool Navigate(object content)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(content);
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x0022A453 File Offset: 0x00229453
		public bool Navigate(object content, object extraData)
		{
			base.VerifyContextAndObjectState();
			return this.NavigationService.Navigate(content, extraData);
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x0022A468 File Offset: 0x00229468
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this._JNS;
		}

		// Token: 0x0600478F RID: 18319 RVA: 0x0022A470 File Offset: 0x00229470
		public void GoForward()
		{
			this._JNS.GoForward();
		}

		// Token: 0x06004790 RID: 18320 RVA: 0x0022A47D File Offset: 0x0022947D
		public void GoBack()
		{
			this._JNS.GoBack();
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x0022A48A File Offset: 0x0022948A
		public void StopLoading()
		{
			base.VerifyContextAndObjectState();
			if (this.InAppShutdown)
			{
				return;
			}
			this.NavigationService.StopLoading();
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0022A4A6 File Offset: 0x002294A6
		public void Refresh()
		{
			base.VerifyContextAndObjectState();
			if (this.InAppShutdown)
			{
				return;
			}
			this.NavigationService.Refresh();
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x0022A4C2 File Offset: 0x002294C2
		public void AddBackEntry(CustomContentState state)
		{
			base.VerifyContextAndObjectState();
			this.NavigationService.AddBackEntry(state);
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0022A4D6 File Offset: 0x002294D6
		public JournalEntry RemoveBackEntry()
		{
			return this._JNS.RemoveBackEntry();
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06004795 RID: 18325 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004796 RID: 18326 RVA: 0x0022A4F5 File Offset: 0x002294F5
		Uri IUriContext.BaseUri
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

		// Token: 0x06004797 RID: 18327 RVA: 0x0022A504 File Offset: 0x00229504
		public override void OnApplyTemplate()
		{
			base.VerifyContextAndObjectState();
			base.OnApplyTemplate();
			FrameworkElement frameworkElement = this.GetVisualChild(0) as FrameworkElement;
			if (this._navigationService != null)
			{
				this._navigationService.VisualTreeAvailable(frameworkElement);
			}
			if (frameworkElement != null && frameworkElement.Name == "NavigationBarRoot")
			{
				if (!this._fFramelet)
				{
					this._fFramelet = true;
					return;
				}
			}
			else if (this._fFramelet)
			{
				this._fFramelet = false;
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0022A572 File Offset: 0x00229572
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool ShouldSerializeContent()
		{
			return this._navigationService == null || !this._navigationService.CanReloadFromUri;
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06004799 RID: 18329 RVA: 0x0022A58C File Offset: 0x0022958C
		public NavigationService NavigationService
		{
			get
			{
				base.VerifyContextAndObjectState();
				return this._navigationService;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x0600479A RID: 18330 RVA: 0x0022A59A File Offset: 0x0022959A
		public IEnumerable BackStack
		{
			get
			{
				return this._JNS.BackStack;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x0600479B RID: 18331 RVA: 0x0022A5A7 File Offset: 0x002295A7
		public IEnumerable ForwardStack
		{
			get
			{
				return this._JNS.ForwardStack;
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x0600479C RID: 18332 RVA: 0x0022A5B4 File Offset: 0x002295B4
		// (set) Token: 0x0600479D RID: 18333 RVA: 0x0022A5CC File Offset: 0x002295CC
		public bool ShowsNavigationUI
		{
			get
			{
				base.VerifyContextAndObjectState();
				return (bool)base.GetValue(NavigationWindow.ShowsNavigationUIProperty);
			}
			set
			{
				base.VerifyContextAndObjectState();
				base.SetValue(NavigationWindow.ShowsNavigationUIProperty, value);
			}
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0022A5E0 File Offset: 0x002295E0
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			if (!navigationWindow._sourceUpdatedFromNavService)
			{
				Uri uriToNavigate = BindUriHelper.GetUriToNavigate(navigationWindow, d.GetValue(BaseUriHelper.BaseUriProperty) as Uri, (Uri)e.NewValue);
				navigationWindow._navigationService.Navigate(uriToNavigate, null, false, true);
			}
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x0022A630 File Offset: 0x00229630
		void INavigatorImpl.OnSourceUpdatedFromNavService(bool journalOrCancel)
		{
			try
			{
				this._sourceUpdatedFromNavService = true;
				base.SetCurrentValueInternal(NavigationWindow.SourceProperty, this._navigationService.Source);
			}
			finally
			{
				this._sourceUpdatedFromNavService = false;
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x0022A674 File Offset: 0x00229674
		// (set) Token: 0x060047A1 RID: 18337 RVA: 0x0022A686 File Offset: 0x00229686
		[DefaultValue(null)]
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(NavigationWindow.SourceProperty);
			}
			set
			{
				base.SetValue(NavigationWindow.SourceProperty, value);
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x060047A2 RID: 18338 RVA: 0x0022A694 File Offset: 0x00229694
		public Uri CurrentSource
		{
			get
			{
				base.VerifyContextAndObjectState();
				if (this._navigationService != null)
				{
					return this._navigationService.CurrentSource;
				}
				return null;
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x060047A3 RID: 18339 RVA: 0x0022A6B1 File Offset: 0x002296B1
		public bool CanGoForward
		{
			get
			{
				return this._JNS.CanGoForward;
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x060047A4 RID: 18340 RVA: 0x0022A6BE File Offset: 0x002296BE
		public bool CanGoBack
		{
			get
			{
				return this._JNS.CanGoBack;
			}
		}

		// Token: 0x140000A4 RID: 164
		// (add) Token: 0x060047A5 RID: 18341 RVA: 0x0022A6CB File Offset: 0x002296CB
		// (remove) Token: 0x060047A6 RID: 18342 RVA: 0x0022A6DF File Offset: 0x002296DF
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigating += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigating -= value;
			}
		}

		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x060047A7 RID: 18343 RVA: 0x0022A6F3 File Offset: 0x002296F3
		// (remove) Token: 0x060047A8 RID: 18344 RVA: 0x0022A707 File Offset: 0x00229707
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationProgress += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationProgress -= value;
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x060047A9 RID: 18345 RVA: 0x0022A71B File Offset: 0x0022971B
		// (remove) Token: 0x060047AA RID: 18346 RVA: 0x0022A72F File Offset: 0x0022972F
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationFailed += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationFailed -= value;
			}
		}

		// Token: 0x140000A7 RID: 167
		// (add) Token: 0x060047AB RID: 18347 RVA: 0x0022A743 File Offset: 0x00229743
		// (remove) Token: 0x060047AC RID: 18348 RVA: 0x0022A757 File Offset: 0x00229757
		public event NavigatedEventHandler Navigated
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigated += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.Navigated -= value;
			}
		}

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x060047AD RID: 18349 RVA: 0x0022A76B File Offset: 0x0022976B
		// (remove) Token: 0x060047AE RID: 18350 RVA: 0x0022A77F File Offset: 0x0022977F
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.LoadCompleted += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.LoadCompleted -= value;
			}
		}

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x060047AF RID: 18351 RVA: 0x0022A793 File Offset: 0x00229793
		// (remove) Token: 0x060047B0 RID: 18352 RVA: 0x0022A7A7 File Offset: 0x002297A7
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationStopped += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.NavigationStopped -= value;
			}
		}

		// Token: 0x140000AA RID: 170
		// (add) Token: 0x060047B1 RID: 18353 RVA: 0x0022A7BB File Offset: 0x002297BB
		// (remove) Token: 0x060047B2 RID: 18354 RVA: 0x0022A7CF File Offset: 0x002297CF
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.FragmentNavigation += value;
			}
			remove
			{
				base.VerifyContextAndObjectState();
				this.NavigationService.FragmentNavigation -= value;
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0022A7E3 File Offset: 0x002297E3
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new NavigationWindowAutomationPeer(this);
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0022A7EB File Offset: 0x002297EB
		protected override void AddChild(object value)
		{
			throw new InvalidOperationException(SR.Get("NoAddChild"));
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x00175B1C File Offset: 0x00174B1C
		protected override void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0022A7FC File Offset: 0x002297FC
		protected override void OnClosed(EventArgs args)
		{
			base.VerifyContextAndObjectState();
			base.OnClosed(args);
			if (this._navigationService != null)
			{
				this._navigationService.Dispose();
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x0022A81E File Offset: 0x0022981E
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			this._JNS.EnsureJournal();
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x0022A831 File Offset: 0x00229831
		void IJournalNavigationScopeHost.VerifyContextAndObjectState()
		{
			base.VerifyContextAndObjectState();
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IJournalNavigationScopeHost.OnJournalAvailable()
		{
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IJournalNavigationScopeHost.GoBackOverride()
		{
			return false;
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IJournalNavigationScopeHost.GoForwardOverride()
		{
			return false;
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x060047BC RID: 18364 RVA: 0x0022A40E File Offset: 0x0022940E
		NavigationService IJournalNavigationScopeHost.NavigationService
		{
			get
			{
				return this._navigationService;
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x0022A839 File Offset: 0x00229839
		Visual INavigatorImpl.FindRootViewer()
		{
			return NavigationHelper.FindRootViewer(this, "PART_NavWinCP");
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x0022A846 File Offset: 0x00229846
		internal Journal Journal
		{
			get
			{
				return this._JNS.Journal;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x060047BF RID: 18367 RVA: 0x0022A468 File Offset: 0x00229468
		internal JournalNavigationScope JournalNavigationScope
		{
			get
			{
				return this._JNS;
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0022A854 File Offset: 0x00229854
		private static object CoerceContent(DependencyObject d, object value)
		{
			NavigationWindow navigationWindow = (NavigationWindow)d;
			if (navigationWindow.NavigationService.Content == value)
			{
				return value;
			}
			navigationWindow.Navigate(value);
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x0022A885 File Offset: 0x00229885
		private void OnBPReady(object sender, BPReadyEventArgs e)
		{
			base.Content = e.Content;
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x0022A893 File Offset: 0x00229893
		private static void OnGoBack(object sender, ExecutedRoutedEventArgs args)
		{
			(sender as NavigationWindow).GoBack();
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x0022A8A0 File Offset: 0x002298A0
		private static void OnQueryGoBack(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = navigationWindow.CanGoBack;
			e.ContinueRouting = !navigationWindow.CanGoBack;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x0022A8CF File Offset: 0x002298CF
		private static void OnGoForward(object sender, ExecutedRoutedEventArgs e)
		{
			(sender as NavigationWindow).GoForward();
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0022A8DC File Offset: 0x002298DC
		private static void OnQueryGoForward(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = navigationWindow.CanGoForward;
			e.ContinueRouting = !navigationWindow.CanGoForward;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x0022A90B File Offset: 0x0022990B
		private static void OnRefresh(object sender, ExecutedRoutedEventArgs e)
		{
			(sender as NavigationWindow).Refresh();
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x0022A918 File Offset: 0x00229918
		private static void OnQueryRefresh(object sender, CanExecuteRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			e.CanExecute = (navigationWindow.Content != null);
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x0022A93B File Offset: 0x0022993B
		private static void OnBrowseStop(object sender, ExecutedRoutedEventArgs e)
		{
			(sender as NavigationWindow).StopLoading();
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x0022A948 File Offset: 0x00229948
		private static void OnQueryBrowseStop(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x0022A954 File Offset: 0x00229954
		private static void OnNavigateJournal(object sender, ExecutedRoutedEventArgs e)
		{
			NavigationWindow navigationWindow = sender as NavigationWindow;
			FrameworkElement frameworkElement = e.Parameter as FrameworkElement;
			if (frameworkElement != null)
			{
				JournalEntry journalEntry = frameworkElement.DataContext as JournalEntry;
				if (journalEntry != null)
				{
					navigationWindow.JournalNavigationScope.NavigateToEntry(journalEntry);
				}
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x060047CB RID: 18379 RVA: 0x00161647 File Offset: 0x00160647
		private bool InAppShutdown
		{
			get
			{
				return Application.IsShuttingDown;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x001FCA42 File Offset: 0x001FBA42
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x060047CD RID: 18381 RVA: 0x0022A993 File Offset: 0x00229993
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return NavigationWindow._dType;
			}
		}

		// Token: 0x040025D9 RID: 9689
		public static readonly DependencyProperty SandboxExternalContentProperty = Frame.SandboxExternalContentProperty.AddOwner(typeof(NavigationWindow));

		// Token: 0x040025DA RID: 9690
		public static readonly DependencyProperty ShowsNavigationUIProperty = DependencyProperty.Register("ShowsNavigationUI", typeof(bool), typeof(NavigationWindow), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x040025DB RID: 9691
		public static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackProperty.AddOwner(typeof(NavigationWindow));

		// Token: 0x040025DC RID: 9692
		public static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackProperty.AddOwner(typeof(NavigationWindow));

		// Token: 0x040025DD RID: 9693
		public static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackProperty.AddOwner(typeof(NavigationWindow));

		// Token: 0x040025DE RID: 9694
		public static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardProperty.AddOwner(typeof(NavigationWindow));

		// Token: 0x040025DF RID: 9695
		public static readonly DependencyProperty SourceProperty = Frame.SourceProperty.AddOwner(typeof(NavigationWindow), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(NavigationWindow.OnSourcePropertyChanged)));

		// Token: 0x040025E0 RID: 9696
		private NavigationService _navigationService;

		// Token: 0x040025E1 RID: 9697
		private JournalNavigationScope _JNS;

		// Token: 0x040025E2 RID: 9698
		private bool _sourceUpdatedFromNavService;

		// Token: 0x040025E3 RID: 9699
		private bool _fFramelet;

		// Token: 0x040025E4 RID: 9700
		private static DependencyObjectType _dType = DependencyObjectType.FromSystemTypeInternal(typeof(NavigationWindow));
	}
}
