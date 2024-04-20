using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;
using MS.Internal.Utility;

namespace System.Windows.Controls
{
	// Token: 0x0200077F RID: 1919
	[DefaultEvent("Navigated")]
	[DefaultProperty("Source")]
	[Localizability(LocalizationCategory.Ignore)]
	[ContentProperty]
	[TemplatePart(Name = "PART_FrameCP", Type = typeof(ContentPresenter))]
	public class Frame : ContentControl, INavigator, INavigatorBase, INavigatorImpl, IJournalNavigationScopeHost, IDownloader, IJournalState, IAddChild, IUriContext
	{
		// Token: 0x06006948 RID: 26952 RVA: 0x002BC754 File Offset: 0x002BB754
		public Frame()
		{
			this.Init();
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x002BC764 File Offset: 0x002BB764
		static Frame()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(typeof(Frame)));
			Frame._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Frame));
			ContentControl.ContentProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(null, new CoerceValueCallback(Frame.CoerceContent)));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			NavigationService.NavigationServiceProperty.OverrideMetadata(typeof(Frame), new FrameworkPropertyMetadata(new PropertyChangedCallback(Frame.OnParentNavigationServiceChanged)));
			ControlsTraceLogger.AddControl(TelemetryControls.Frame);
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x002BC9AC File Offset: 0x002BB9AC
		private static object CoerceContent(DependencyObject d, object value)
		{
			Frame frame = (Frame)d;
			if (frame._navigationService.Content == value)
			{
				return value;
			}
			frame.Navigate(value);
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x0600694B RID: 26955 RVA: 0x002BC9DD File Offset: 0x002BB9DD
		private void Init()
		{
			base.InheritanceBehavior = InheritanceBehavior.SkipToAppNow;
			base.ContentIsNotLogical = true;
			this._navigationService = new NavigationService(this);
			this._navigationService.BPReady += this._OnBPReady;
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x0600694C RID: 26956 RVA: 0x002BCA10 File Offset: 0x002BBA10
		// (set) Token: 0x0600694D RID: 26957 RVA: 0x002BCA18 File Offset: 0x002BBA18
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

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x0600694E RID: 26958 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x0600694F RID: 26959 RVA: 0x0022A4F5 File Offset: 0x002294F5
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

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06006950 RID: 26960 RVA: 0x002BCA21 File Offset: 0x002BBA21
		NavigationService IDownloader.Downloader
		{
			get
			{
				return this._navigationService;
			}
		}

		// Token: 0x14000110 RID: 272
		// (add) Token: 0x06006951 RID: 26961 RVA: 0x002BCA2C File Offset: 0x002BBA2C
		// (remove) Token: 0x06006952 RID: 26962 RVA: 0x002BCA64 File Offset: 0x002BBA64
		public event EventHandler ContentRendered;

		// Token: 0x06006953 RID: 26963 RVA: 0x002BCA9C File Offset: 0x002BBA9C
		protected virtual void OnContentRendered(EventArgs args)
		{
			DependencyObject dependencyObject = base.Content as DependencyObject;
			if (dependencyObject != null)
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement(dependencyObject);
				if (focusedElement != null)
				{
					focusedElement.Focus();
				}
			}
			if (this.ContentRendered != null)
			{
				this.ContentRendered(this, args);
			}
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x002BCAE0 File Offset: 0x002BBAE0
		private static object CoerceSource(DependencyObject d, object value)
		{
			Frame frame = (Frame)d;
			if (frame._sourceUpdatedFromNavService)
			{
				Invariant.Assert(frame._navigationService != null, "_navigationService should never be null here");
				return frame._navigationService.Source;
			}
			return value;
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x002BCB1C File Offset: 0x002BBB1C
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Frame frame = (Frame)d;
			if (!frame._sourceUpdatedFromNavService)
			{
				Uri uriToNavigate = BindUriHelper.GetUriToNavigate(frame, ((IUriContext)frame).BaseUri, (Uri)e.NewValue);
				frame._navigationService.Navigate(uriToNavigate, null, false, true);
			}
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x002BCB64 File Offset: 0x002BBB64
		void INavigatorImpl.OnSourceUpdatedFromNavService(bool journalOrCancel)
		{
			try
			{
				this._sourceUpdatedFromNavService = true;
				base.SetCurrentValueInternal(Frame.SourceProperty, this._navigationService.Source);
			}
			finally
			{
				this._sourceUpdatedFromNavService = false;
			}
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06006957 RID: 26967 RVA: 0x002BCBA8 File Offset: 0x002BBBA8
		// (set) Token: 0x06006958 RID: 26968 RVA: 0x002BCBBA File Offset: 0x002BBBBA
		[Bindable(true)]
		[CustomCategory("Navigation")]
		public Uri Source
		{
			get
			{
				return (Uri)base.GetValue(Frame.SourceProperty);
			}
			set
			{
				base.SetValue(Frame.SourceProperty, value);
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06006959 RID: 26969 RVA: 0x002BCBC8 File Offset: 0x002BBBC8
		// (set) Token: 0x0600695A RID: 26970 RVA: 0x002BCBDA File Offset: 0x002BBBDA
		public NavigationUIVisibility NavigationUIVisibility
		{
			get
			{
				return (NavigationUIVisibility)base.GetValue(Frame.NavigationUIVisibilityProperty);
			}
			set
			{
				base.SetValue(Frame.NavigationUIVisibilityProperty, value);
			}
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x0600695B RID: 26971 RVA: 0x002BCBED File Offset: 0x002BBBED
		// (set) Token: 0x0600695C RID: 26972 RVA: 0x002BCC00 File Offset: 0x002BBC00
		public bool SandboxExternalContent
		{
			get
			{
				return (bool)base.GetValue(Frame.SandboxExternalContentProperty);
			}
			set
			{
				base.SetValue(Frame.SandboxExternalContentProperty, value);
			}
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x002BCC1C File Offset: 0x002BBC1C
		private static void OnSandboxExternalContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Frame frame = (Frame)d;
			if ((bool)e.NewValue && !(bool)e.OldValue)
			{
				frame.NavigationService.Refresh();
			}
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x0022A153 File Offset: 0x00229153
		private static object CoerceSandBoxExternalContentValue(DependencyObject d, object value)
		{
			return (bool)value;
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x002BCC58 File Offset: 0x002BBC58
		private static bool ValidateJournalOwnershipValue(object value)
		{
			JournalOwnership journalOwnership = (JournalOwnership)value;
			return journalOwnership == JournalOwnership.Automatic || journalOwnership == JournalOwnership.UsesParentJournal || journalOwnership == JournalOwnership.OwnsJournal;
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x002BCC79 File Offset: 0x002BBC79
		private static void OnJournalOwnershipPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Frame)d).OnJournalOwnershipPropertyChanged((JournalOwnership)e.NewValue);
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x002BCC94 File Offset: 0x002BBC94
		private void OnJournalOwnershipPropertyChanged(JournalOwnership newValue)
		{
			switch (this._journalOwnership)
			{
			case JournalOwnership.Automatic:
				if (newValue != JournalOwnership.OwnsJournal)
				{
					if (newValue == JournalOwnership.UsesParentJournal)
					{
						this.SwitchToParentJournal();
					}
				}
				else
				{
					this.SwitchToOwnJournal();
				}
				break;
			case JournalOwnership.OwnsJournal:
				if (newValue != JournalOwnership.Automatic && newValue == JournalOwnership.UsesParentJournal)
				{
					this.SwitchToParentJournal();
				}
				break;
			case JournalOwnership.UsesParentJournal:
				if (newValue != JournalOwnership.Automatic)
				{
					if (newValue == JournalOwnership.OwnsJournal)
					{
						this.SwitchToOwnJournal();
					}
				}
				else
				{
					this._navigationService.InvalidateJournalNavigationScope();
				}
				break;
			}
			this._journalOwnership = newValue;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x002BCD08 File Offset: 0x002BBD08
		private static object CoerceJournalOwnership(DependencyObject d, object newValue)
		{
			if (((Frame)d)._journalOwnership == JournalOwnership.OwnsJournal && (JournalOwnership)newValue == JournalOwnership.Automatic)
			{
				return JournalOwnership.OwnsJournal;
			}
			return newValue;
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06006963 RID: 26979 RVA: 0x002BCD28 File Offset: 0x002BBD28
		// (set) Token: 0x06006964 RID: 26980 RVA: 0x002BCD30 File Offset: 0x002BBD30
		public JournalOwnership JournalOwnership
		{
			get
			{
				return this._journalOwnership;
			}
			set
			{
				if (value != this._journalOwnership)
				{
					base.SetValue(Frame.JournalOwnershipProperty, value);
				}
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06006965 RID: 26981 RVA: 0x002BCD4C File Offset: 0x002BBD4C
		public NavigationService NavigationService
		{
			get
			{
				base.VerifyAccess();
				return this._navigationService;
			}
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x002BCD5A File Offset: 0x002BBD5A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new FrameAutomationPeer(this);
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x002BCD62 File Offset: 0x002BBD62
		protected override void AddChild(object value)
		{
			throw new InvalidOperationException(SR.Get("FrameNoAddChild"));
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x00175B1C File Offset: 0x00174B1C
		protected override void AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x002BCD74 File Offset: 0x002BBD74
		private void _OnBPReady(object o, BPReadyEventArgs e)
		{
			base.SetCurrentValueInternal(ContentControl.ContentProperty, e.Content);
			base.InvalidateMeasure();
			if (base.IsLoaded)
			{
				this.PostContentRendered();
				return;
			}
			if (!this._postContentRenderedFromLoadedHandler)
			{
				base.Loaded += this.LoadedHandler;
				this._postContentRenderedFromLoadedHandler = true;
			}
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x002BCDC8 File Offset: 0x002BBDC8
		private void LoadedHandler(object sender, RoutedEventArgs args)
		{
			if (this._postContentRenderedFromLoadedHandler)
			{
				this.PostContentRendered();
				this._postContentRenderedFromLoadedHandler = false;
				base.Loaded -= this.LoadedHandler;
			}
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x002BCDF1 File Offset: 0x002BBDF1
		private void PostContentRendered()
		{
			if (this._contentRenderedCallback != null)
			{
				this._contentRenderedCallback.Abort();
			}
			this._contentRenderedCallback = base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object unused)
			{
				this._contentRenderedCallback = null;
				this.OnContentRendered(EventArgs.Empty);
				return null;
			}), this);
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x002BCE26 File Offset: 0x002BBE26
		private void OnQueryGoBack(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this._ownJournalScope.CanGoBack;
			e.Handled = true;
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x002BCE40 File Offset: 0x002BBE40
		private void OnGoBack(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.GoBack();
			e.Handled = true;
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x002BCE54 File Offset: 0x002BBE54
		private void OnQueryGoForward(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = this._ownJournalScope.CanGoForward;
			e.Handled = true;
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x002BCE6E File Offset: 0x002BBE6E
		private void OnGoForward(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.GoForward();
			e.Handled = true;
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x002BCE84 File Offset: 0x002BBE84
		private void OnNavigateJournal(object sender, ExecutedRoutedEventArgs e)
		{
			FrameworkElement frameworkElement = e.Parameter as FrameworkElement;
			if (frameworkElement != null)
			{
				JournalEntry journalEntry = frameworkElement.DataContext as JournalEntry;
				if (journalEntry != null && this._ownJournalScope.NavigateToEntry(journalEntry))
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x002BCEC4 File Offset: 0x002BBEC4
		private void OnQueryRefresh(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (base.Content != null);
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x002BCED5 File Offset: 0x002BBED5
		private void OnRefresh(object sender, ExecutedRoutedEventArgs e)
		{
			this._navigationService.Refresh();
			e.Handled = true;
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x002BCEE9 File Offset: 0x002BBEE9
		private void OnBrowseStop(object sender, ExecutedRoutedEventArgs e)
		{
			this._ownJournalScope.StopLoading();
			e.Handled = true;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x002BCEFD File Offset: 0x002BBEFD
		internal override object AdjustEventSource(RoutedEventArgs e)
		{
			e.Source = this;
			return this;
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x002BCF07 File Offset: 0x002BBF07
		internal override string GetPlainText()
		{
			if (this.Source != null)
			{
				return this.Source.ToString();
			}
			return string.Empty;
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x002BCF28 File Offset: 0x002BBF28
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool ShouldSerializeContent()
		{
			Invariant.Assert(this._navigationService != null, "_navigationService should never be null here");
			return !this._navigationService.CanReloadFromUri && base.Content != null;
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x002BCF55 File Offset: 0x002BBF55
		private static void OnParentNavigationServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Frame)d).NavigationService.OnParentNavigationServiceChanged();
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x002BCF68 File Offset: 0x002BBF68
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Visual templateChild = this.TemplateChild;
			if (templateChild != null)
			{
				this.NavigationService.VisualTreeAvailable(templateChild);
			}
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x002BCF91 File Offset: 0x002BBF91
		Visual INavigatorImpl.FindRootViewer()
		{
			return NavigationHelper.FindRootViewer(this, "PART_FrameCP");
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x002BCF9E File Offset: 0x002BBF9E
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this.GetJournal(create);
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x002BCFA8 File Offset: 0x002BBFA8
		private JournalNavigationScope GetJournal(bool create)
		{
			Invariant.Assert(this._ownJournalScope != null ^ this._journalOwnership != JournalOwnership.OwnsJournal);
			if (this._ownJournalScope != null)
			{
				return this._ownJournalScope;
			}
			JournalNavigationScope parentJournal = this.GetParentJournal(create);
			if (parentJournal != null)
			{
				base.SetCurrentValueInternal(Frame.JournalOwnershipProperty, JournalOwnership.UsesParentJournal);
				return parentJournal;
			}
			if (create && this._journalOwnership == JournalOwnership.Automatic)
			{
				base.SetCurrentValueInternal(Frame.JournalOwnershipProperty, JournalOwnership.OwnsJournal);
			}
			return this._ownJournalScope;
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x0600697C RID: 27004 RVA: 0x002BD01F File Offset: 0x002BC01F
		public bool CanGoForward
		{
			get
			{
				return this._ownJournalScope != null && this._ownJournalScope.CanGoForward;
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x0600697D RID: 27005 RVA: 0x002BD036 File Offset: 0x002BC036
		public bool CanGoBack
		{
			get
			{
				return this._ownJournalScope != null && this._ownJournalScope.CanGoBack;
			}
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x002BD04D File Offset: 0x002BC04D
		public void AddBackEntry(CustomContentState state)
		{
			base.VerifyAccess();
			this._navigationService.AddBackEntry(state);
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x002BD061 File Offset: 0x002BC061
		public JournalEntry RemoveBackEntry()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			return this._ownJournalScope.RemoveBackEntry();
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x002BD086 File Offset: 0x002BC086
		public bool Navigate(Uri source)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(source);
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x002BD09A File Offset: 0x002BC09A
		public bool Navigate(Uri source, object extraData)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(source, extraData);
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x002BD0AF File Offset: 0x002BC0AF
		public bool Navigate(object content)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(content);
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x002BD0C3 File Offset: 0x002BC0C3
		public bool Navigate(object content, object extraData)
		{
			base.VerifyAccess();
			return this._navigationService.Navigate(content, extraData);
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x002BD0D8 File Offset: 0x002BC0D8
		public void GoForward()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			this._ownJournalScope.GoForward();
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x002BD0FD File Offset: 0x002BC0FD
		public void GoBack()
		{
			if (this._ownJournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_NoJournal"));
			}
			this._ownJournalScope.GoBack();
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x002BD122 File Offset: 0x002BC122
		public void StopLoading()
		{
			base.VerifyAccess();
			this._navigationService.StopLoading();
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x002BD135 File Offset: 0x002BC135
		public void Refresh()
		{
			base.VerifyAccess();
			this._navigationService.Refresh();
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x06006988 RID: 27016 RVA: 0x002BD148 File Offset: 0x002BC148
		public Uri CurrentSource
		{
			get
			{
				return this._navigationService.CurrentSource;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x06006989 RID: 27017 RVA: 0x002BD155 File Offset: 0x002BC155
		public IEnumerable BackStack
		{
			get
			{
				if (this._ownJournalScope != null)
				{
					return this._ownJournalScope.BackStack;
				}
				return null;
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x0600698A RID: 27018 RVA: 0x002BD16C File Offset: 0x002BC16C
		public IEnumerable ForwardStack
		{
			get
			{
				if (this._ownJournalScope != null)
				{
					return this._ownJournalScope.ForwardStack;
				}
				return null;
			}
		}

		// Token: 0x14000111 RID: 273
		// (add) Token: 0x0600698B RID: 27019 RVA: 0x002BD183 File Offset: 0x002BC183
		// (remove) Token: 0x0600698C RID: 27020 RVA: 0x002BD191 File Offset: 0x002BC191
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._navigationService.Navigating += value;
			}
			remove
			{
				this._navigationService.Navigating -= value;
			}
		}

		// Token: 0x14000112 RID: 274
		// (add) Token: 0x0600698D RID: 27021 RVA: 0x002BD19F File Offset: 0x002BC19F
		// (remove) Token: 0x0600698E RID: 27022 RVA: 0x002BD1AD File Offset: 0x002BC1AD
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._navigationService.NavigationProgress += value;
			}
			remove
			{
				this._navigationService.NavigationProgress -= value;
			}
		}

		// Token: 0x14000113 RID: 275
		// (add) Token: 0x0600698F RID: 27023 RVA: 0x002BD1BB File Offset: 0x002BC1BB
		// (remove) Token: 0x06006990 RID: 27024 RVA: 0x002BD1C9 File Offset: 0x002BC1C9
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				this._navigationService.NavigationFailed += value;
			}
			remove
			{
				this._navigationService.NavigationFailed -= value;
			}
		}

		// Token: 0x14000114 RID: 276
		// (add) Token: 0x06006991 RID: 27025 RVA: 0x002BD1D7 File Offset: 0x002BC1D7
		// (remove) Token: 0x06006992 RID: 27026 RVA: 0x002BD1E5 File Offset: 0x002BC1E5
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._navigationService.Navigated += value;
			}
			remove
			{
				this._navigationService.Navigated -= value;
			}
		}

		// Token: 0x14000115 RID: 277
		// (add) Token: 0x06006993 RID: 27027 RVA: 0x002BD1F3 File Offset: 0x002BC1F3
		// (remove) Token: 0x06006994 RID: 27028 RVA: 0x002BD201 File Offset: 0x002BC201
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._navigationService.LoadCompleted += value;
			}
			remove
			{
				this._navigationService.LoadCompleted -= value;
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x06006995 RID: 27029 RVA: 0x002BD20F File Offset: 0x002BC20F
		// (remove) Token: 0x06006996 RID: 27030 RVA: 0x002BD21D File Offset: 0x002BC21D
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._navigationService.NavigationStopped += value;
			}
			remove
			{
				this._navigationService.NavigationStopped -= value;
			}
		}

		// Token: 0x14000117 RID: 279
		// (add) Token: 0x06006997 RID: 27031 RVA: 0x002BD22B File Offset: 0x002BC22B
		// (remove) Token: 0x06006998 RID: 27032 RVA: 0x002BD239 File Offset: 0x002BC239
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._navigationService.FragmentNavigation += value;
			}
			remove
			{
				this._navigationService.FragmentNavigation -= value;
			}
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x0019D93D File Offset: 0x0019C93D
		void IJournalNavigationScopeHost.VerifyContextAndObjectState()
		{
			base.VerifyAccess();
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IJournalNavigationScopeHost.OnJournalAvailable()
		{
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IJournalNavigationScopeHost.GoBackOverride()
		{
			return false;
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IJournalNavigationScopeHost.GoForwardOverride()
		{
			return false;
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x002BD248 File Offset: 0x002BC248
		CustomJournalStateInternal IJournalState.GetJournalState(JournalReason journalReason)
		{
			if (journalReason != JournalReason.NewContentNavigation)
			{
				return null;
			}
			Frame.FramePersistState framePersistState = new Frame.FramePersistState();
			framePersistState.JournalEntry = this._navigationService.MakeJournalEntry(JournalReason.NewContentNavigation);
			framePersistState.NavSvcGuid = this._navigationService.GuidId;
			framePersistState.JournalOwnership = this._journalOwnership;
			if (this._ownJournalScope != null)
			{
				framePersistState.Journal = this._ownJournalScope.Journal;
			}
			return framePersistState;
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x002BD2AC File Offset: 0x002BC2AC
		void IJournalState.RestoreJournalState(CustomJournalStateInternal cjs)
		{
			Frame.FramePersistState framePersistState = (Frame.FramePersistState)cjs;
			this._navigationService.GuidId = framePersistState.NavSvcGuid;
			this.JournalOwnership = framePersistState.JournalOwnership;
			if (this._journalOwnership == JournalOwnership.OwnsJournal)
			{
				Invariant.Assert(framePersistState.Journal != null);
				this._ownJournalScope.Journal = framePersistState.Journal;
			}
			if (framePersistState.JournalEntry != null)
			{
				framePersistState.JournalEntry.Navigate(this, NavigationMode.Back);
			}
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x002BD31B File Offset: 0x002BC31B
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			if (this._ownJournalScope != null)
			{
				this._ownJournalScope.EnsureJournal();
			}
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x002BD338 File Offset: 0x002BC338
		internal override void OnThemeChanged()
		{
			DependencyObject d;
			if (!base.HasTemplateGeneratedSubTree && (d = (base.Content as DependencyObject)) != null)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
				if (frameworkElement != null || frameworkContentElement != null)
				{
					TreeWalkHelper.InvalidateOnResourcesChange(frameworkElement, frameworkContentElement, ResourcesChangeInfo.ThemeChangeInfo);
				}
			}
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x002BD37C File Offset: 0x002BC37C
		private JournalNavigationScope GetParentJournal(bool create)
		{
			JournalNavigationScope result = null;
			NavigationService parentNavigationService = this._navigationService.ParentNavigationService;
			if (parentNavigationService != null)
			{
				result = parentNavigationService.INavigatorHost.GetJournal(create);
			}
			return result;
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x002BD3A8 File Offset: 0x002BC3A8
		private void SwitchToOwnJournal()
		{
			if (this._ownJournalScope == null)
			{
				JournalNavigationScope parentJournal = this.GetParentJournal(false);
				if (parentJournal != null)
				{
					parentJournal.Journal.RemoveEntries(this._navigationService.GuidId);
				}
				this._ownJournalScope = new JournalNavigationScope(this);
				this._navigationService.InvalidateJournalNavigationScope();
				if (base.IsLoaded)
				{
					this._ownJournalScope.EnsureJournal();
				}
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseBack, new ExecutedRoutedEventHandler(this.OnGoBack), new CanExecuteRoutedEventHandler(this.OnQueryGoBack)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseForward, new ExecutedRoutedEventHandler(this.OnGoForward), new CanExecuteRoutedEventHandler(this.OnQueryGoForward)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.NavigateJournal, new ExecutedRoutedEventHandler(this.OnNavigateJournal)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.Refresh, new ExecutedRoutedEventHandler(this.OnRefresh), new CanExecuteRoutedEventHandler(this.OnQueryRefresh)));
				this.AddCommandBinding(new CommandBinding(NavigationCommands.BrowseStop, new ExecutedRoutedEventHandler(this.OnBrowseStop)));
			}
			this._journalOwnership = JournalOwnership.OwnsJournal;
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x002BD4C4 File Offset: 0x002BC4C4
		private void SwitchToParentJournal()
		{
			if (this._ownJournalScope != null)
			{
				this._ownJournalScope = null;
				this._navigationService.InvalidateJournalNavigationScope();
				JournalNavigationScope.ClearDPValues(this);
				foreach (CommandBinding commandBinding in this._commandBindings)
				{
					base.CommandBindings.Remove(commandBinding);
				}
				this._commandBindings = null;
			}
			this._journalOwnership = JournalOwnership.UsesParentJournal;
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x002BD54C File Offset: 0x002BC54C
		private void AddCommandBinding(CommandBinding b)
		{
			base.CommandBindings.Add(b);
			if (this._commandBindings == null)
			{
				this._commandBindings = new List<CommandBinding>(6);
			}
			this._commandBindings.Add(b);
		}

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x060069A5 RID: 27045 RVA: 0x002BD57B File Offset: 0x002BC57B
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Frame._dType;
			}
		}

		// Token: 0x04003504 RID: 13572
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Uri), typeof(Frame), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(Frame.OnSourcePropertyChanged), new CoerceValueCallback(Frame.CoerceSource)));

		// Token: 0x04003505 RID: 13573
		public static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackProperty.AddOwner(typeof(Frame));

		// Token: 0x04003506 RID: 13574
		public static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardProperty.AddOwner(typeof(Frame));

		// Token: 0x04003507 RID: 13575
		public static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackProperty.AddOwner(typeof(Frame));

		// Token: 0x04003508 RID: 13576
		public static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackProperty.AddOwner(typeof(Frame));

		// Token: 0x04003509 RID: 13577
		public static readonly DependencyProperty NavigationUIVisibilityProperty = DependencyProperty.Register("NavigationUIVisibility", typeof(NavigationUIVisibility), typeof(Frame), new PropertyMetadata(NavigationUIVisibility.Automatic));

		// Token: 0x0400350A RID: 13578
		public static readonly DependencyProperty SandboxExternalContentProperty = DependencyProperty.Register("SandboxExternalContent", typeof(bool), typeof(Frame), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Frame.OnSandboxExternalContentPropertyChanged), new CoerceValueCallback(Frame.CoerceSandBoxExternalContentValue)));

		// Token: 0x0400350B RID: 13579
		public static readonly DependencyProperty JournalOwnershipProperty = DependencyProperty.Register("JournalOwnership", typeof(JournalOwnership), typeof(Frame), new FrameworkPropertyMetadata(JournalOwnership.Automatic, new PropertyChangedCallback(Frame.OnJournalOwnershipPropertyChanged), new CoerceValueCallback(Frame.CoerceJournalOwnership)), new ValidateValueCallback(Frame.ValidateJournalOwnershipValue));

		// Token: 0x0400350C RID: 13580
		private bool _postContentRenderedFromLoadedHandler;

		// Token: 0x0400350D RID: 13581
		private DispatcherOperation _contentRenderedCallback;

		// Token: 0x0400350E RID: 13582
		private NavigationService _navigationService;

		// Token: 0x0400350F RID: 13583
		private bool _sourceUpdatedFromNavService;

		// Token: 0x04003510 RID: 13584
		private JournalOwnership _journalOwnership;

		// Token: 0x04003511 RID: 13585
		private JournalNavigationScope _ownJournalScope;

		// Token: 0x04003512 RID: 13586
		private List<CommandBinding> _commandBindings;

		// Token: 0x04003513 RID: 13587
		private static DependencyObjectType _dType;

		// Token: 0x02000BD6 RID: 3030
		[Serializable]
		private class FramePersistState : CustomJournalStateInternal
		{
			// Token: 0x06008F93 RID: 36755 RVA: 0x003449CA File Offset: 0x003439CA
			internal override void PrepareForSerialization()
			{
				if (this.JournalEntry != null && this.JournalEntry.IsAlive())
				{
					this.JournalEntry = null;
				}
				if (this.Journal != null)
				{
					this.Journal.PruneKeepAliveEntries();
				}
			}

			// Token: 0x04004A17 RID: 18967
			internal JournalEntry JournalEntry;

			// Token: 0x04004A18 RID: 18968
			internal Guid NavSvcGuid;

			// Token: 0x04004A19 RID: 18969
			internal JournalOwnership JournalOwnership;

			// Token: 0x04004A1A RID: 18970
			internal Journal Journal;
		}
	}
}
