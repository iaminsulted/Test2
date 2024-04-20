using System;
using System.Collections;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MS.Internal.KnownBoxes;

namespace MS.Internal.AppModel
{
	// Token: 0x0200028A RID: 650
	internal class JournalNavigationScope : DependencyObject, INavigator, INavigatorBase
	{
		// Token: 0x06001893 RID: 6291 RVA: 0x0016109B File Offset: 0x0016009B
		internal JournalNavigationScope(IJournalNavigationScopeHost host)
		{
			this._host = host;
			this._rootNavSvc = host.NavigationService;
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x001610B6 File Offset: 0x001600B6
		// (set) Token: 0x06001895 RID: 6293 RVA: 0x001610C3 File Offset: 0x001600C3
		public Uri Source
		{
			get
			{
				return this._host.Source;
			}
			set
			{
				this._host.Source = value;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001896 RID: 6294 RVA: 0x001610D1 File Offset: 0x001600D1
		public Uri CurrentSource
		{
			get
			{
				return this._host.CurrentSource;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x001610DE File Offset: 0x001600DE
		// (set) Token: 0x06001898 RID: 6296 RVA: 0x001610EB File Offset: 0x001600EB
		public object Content
		{
			get
			{
				return this._host.Content;
			}
			set
			{
				this._host.Content = value;
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x001610F9 File Offset: 0x001600F9
		public bool Navigate(Uri source)
		{
			return this._host.Navigate(source);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00161107 File Offset: 0x00160107
		public bool Navigate(Uri source, object extraData)
		{
			return this._host.Navigate(source, extraData);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00161116 File Offset: 0x00160116
		public bool Navigate(object content)
		{
			return this._host.Navigate(content);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00161124 File Offset: 0x00160124
		public bool Navigate(object content, object extraData)
		{
			return this._host.Navigate(content, extraData);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00161133 File Offset: 0x00160133
		public void StopLoading()
		{
			this._host.StopLoading();
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00161140 File Offset: 0x00160140
		public void Refresh()
		{
			this._host.Refresh();
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x0600189F RID: 6303 RVA: 0x0016114D File Offset: 0x0016014D
		// (remove) Token: 0x060018A0 RID: 6304 RVA: 0x0016115B File Offset: 0x0016015B
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._host.Navigating += value;
			}
			remove
			{
				this._host.Navigating -= value;
			}
		}

		// Token: 0x14000034 RID: 52
		// (add) Token: 0x060018A1 RID: 6305 RVA: 0x00161169 File Offset: 0x00160169
		// (remove) Token: 0x060018A2 RID: 6306 RVA: 0x00161177 File Offset: 0x00160177
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._host.NavigationProgress += value;
			}
			remove
			{
				this._host.NavigationProgress -= value;
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x060018A3 RID: 6307 RVA: 0x00161185 File Offset: 0x00160185
		// (remove) Token: 0x060018A4 RID: 6308 RVA: 0x00161193 File Offset: 0x00160193
		public event NavigationFailedEventHandler NavigationFailed
		{
			add
			{
				this._host.NavigationFailed += value;
			}
			remove
			{
				this._host.NavigationFailed -= value;
			}
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x060018A5 RID: 6309 RVA: 0x001611A1 File Offset: 0x001601A1
		// (remove) Token: 0x060018A6 RID: 6310 RVA: 0x001611AF File Offset: 0x001601AF
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._host.Navigated += value;
			}
			remove
			{
				this._host.Navigated -= value;
			}
		}

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060018A7 RID: 6311 RVA: 0x001611BD File Offset: 0x001601BD
		// (remove) Token: 0x060018A8 RID: 6312 RVA: 0x001611CB File Offset: 0x001601CB
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._host.LoadCompleted += value;
			}
			remove
			{
				this._host.LoadCompleted -= value;
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x060018A9 RID: 6313 RVA: 0x001611D9 File Offset: 0x001601D9
		// (remove) Token: 0x060018AA RID: 6314 RVA: 0x001611E7 File Offset: 0x001601E7
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._host.NavigationStopped += value;
			}
			remove
			{
				this._host.NavigationStopped -= value;
			}
		}

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x060018AB RID: 6315 RVA: 0x001611F5 File Offset: 0x001601F5
		// (remove) Token: 0x060018AC RID: 6316 RVA: 0x00161203 File Offset: 0x00160203
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._host.FragmentNavigation += value;
			}
			remove
			{
				this._host.FragmentNavigation -= value;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060018AD RID: 6317 RVA: 0x00161211 File Offset: 0x00160211
		public bool CanGoForward
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoForward;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060018AE RID: 6318 RVA: 0x0016123B File Offset: 0x0016023B
		public bool CanGoBack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this._journal != null && !this.InAppShutdown && this._journal.CanGoBack;
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00161268 File Offset: 0x00160268
		public void GoForward()
		{
			if (!this.CanGoForward)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			if (!this._host.GoForwardOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginForwardNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x001612C0 File Offset: 0x001602C0
		public void GoBack()
		{
			if (!this.CanGoBack)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			if (!this._host.GoBackOverride())
			{
				JournalEntry journalEntry = this.Journal.BeginBackNavigation();
				if (journalEntry == null)
				{
					this._rootNavSvc.StopLoading();
					return;
				}
				this.NavigateToEntry(journalEntry);
			}
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00161315 File Offset: 0x00160315
		public void AddBackEntry(CustomContentState state)
		{
			this._host.VerifyContextAndObjectState();
			this._rootNavSvc.AddBackEntry(state);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0016132E File Offset: 0x0016032E
		public JournalEntry RemoveBackEntry()
		{
			this._host.VerifyContextAndObjectState();
			if (this._journal != null)
			{
				return this._journal.RemoveBackEntry();
			}
			return null;
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060018B3 RID: 6323 RVA: 0x00161350 File Offset: 0x00160350
		public IEnumerable BackStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.BackStack;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x00161368 File Offset: 0x00160368
		public IEnumerable ForwardStack
		{
			get
			{
				this._host.VerifyContextAndObjectState();
				return this.Journal.ForwardStack;
			}
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x000F93D3 File Offset: 0x000F83D3
		JournalNavigationScope INavigator.GetJournal(bool create)
		{
			return this;
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x00161380 File Offset: 0x00160380
		internal void EnsureJournal()
		{
			Journal journal = this.Journal;
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0016138C File Offset: 0x0016038C
		internal bool CanInvokeJournalEntry(int entryId)
		{
			if (this._journal == null)
			{
				return false;
			}
			int num = this._journal.FindIndexForEntryWithId(entryId);
			if (num == -1)
			{
				return false;
			}
			JournalEntry entry = this._journal[num];
			return this._journal.IsNavigable(entry);
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x001613D0 File Offset: 0x001603D0
		internal bool NavigateToEntry(int index)
		{
			JournalEntry entry = this.Journal[index];
			return this.NavigateToEntry(entry);
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x001613F4 File Offset: 0x001603F4
		internal bool NavigateToEntry(JournalEntry entry)
		{
			if (entry == null)
			{
				return false;
			}
			if (!this.Journal.IsNavigable(entry))
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			NavigationMode navigationMode = this.Journal.GetNavigationMode(entry);
			bool flag = false;
			try
			{
				flag = entry.Navigate(navigationService.INavigatorHost, navigationMode);
			}
			finally
			{
				if (!flag)
				{
					this.AbortJournalNavigation();
				}
			}
			return flag;
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x00161464 File Offset: 0x00160464
		internal void AbortJournalNavigation()
		{
			if (this._journal != null)
			{
				this._journal.AbortJournalNavigation();
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00161479 File Offset: 0x00160479
		internal INavigatorBase FindTarget(string name)
		{
			return this._rootNavSvc.FindTarget(name);
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00161487 File Offset: 0x00160487
		internal static void ClearDPValues(DependencyObject navigator)
		{
			navigator.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.FalseBox);
			navigator.SetValue(JournalNavigationScope.BackStackPropertyKey, null);
			navigator.SetValue(JournalNavigationScope.ForwardStackPropertyKey, null);
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x001614C1 File Offset: 0x001604C1
		// (set) Token: 0x060018BE RID: 6334 RVA: 0x001614DC File Offset: 0x001604DC
		internal Journal Journal
		{
			get
			{
				if (this._journal == null)
				{
					this.Journal = new Journal();
				}
				return this._journal;
			}
			set
			{
				this._journal = value;
				this._journal.Filter = new JournalEntryFilter(this.IsEntryNavigable);
				this._journal.BackForwardStateChange += this.OnBackForwardStateChange;
				DependencyObject dependencyObject = (DependencyObject)this._host;
				dependencyObject.SetValue(JournalNavigationScope.BackStackPropertyKey, this._journal.BackStack);
				dependencyObject.SetValue(JournalNavigationScope.ForwardStackPropertyKey, this._journal.ForwardStack);
				this._host.OnJournalAvailable();
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x0016155F File Offset: 0x0016055F
		internal NavigationService RootNavigationService
		{
			get
			{
				return this._rootNavSvc;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00161567 File Offset: 0x00160567
		internal INavigatorBase NavigatorHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x00161570 File Offset: 0x00160570
		private void OnBackForwardStateChange(object sender, EventArgs e)
		{
			DependencyObject dependencyObject = (DependencyObject)this._host;
			bool flag = false;
			bool flag2 = this._journal.CanGoBack;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoBackProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoBackPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			flag2 = this._journal.CanGoForward;
			if (flag2 != (bool)dependencyObject.GetValue(JournalNavigationScope.CanGoForwardProperty))
			{
				dependencyObject.SetValue(JournalNavigationScope.CanGoForwardPropertyKey, BooleanBoxes.Box(flag2));
				flag = true;
			}
			if (flag)
			{
				CommandManager.InvalidateRequerySuggested();
			}
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x001615F8 File Offset: 0x001605F8
		private bool IsEntryNavigable(JournalEntry entry)
		{
			if (entry == null || !entry.IsNavigable())
			{
				return false;
			}
			NavigationService navigationService = this._rootNavSvc.FindTarget(entry.NavigationServiceId);
			return navigationService != null && (navigationService.ContentId == entry.ContentId || entry.JEGroupState.GroupExitEntry == entry);
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x00161647 File Offset: 0x00160647
		private bool InAppShutdown
		{
			get
			{
				return Application.IsShuttingDown;
			}
		}

		// Token: 0x04000D58 RID: 3416
		private static readonly DependencyPropertyKey CanGoBackPropertyKey = DependencyProperty.RegisterReadOnly("CanGoBack", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04000D59 RID: 3417
		internal static readonly DependencyProperty CanGoBackProperty = JournalNavigationScope.CanGoBackPropertyKey.DependencyProperty;

		// Token: 0x04000D5A RID: 3418
		private static readonly DependencyPropertyKey CanGoForwardPropertyKey = DependencyProperty.RegisterReadOnly("CanGoForward", typeof(bool), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04000D5B RID: 3419
		internal static readonly DependencyProperty CanGoForwardProperty = JournalNavigationScope.CanGoForwardPropertyKey.DependencyProperty;

		// Token: 0x04000D5C RID: 3420
		private static readonly DependencyPropertyKey BackStackPropertyKey = DependencyProperty.RegisterReadOnly("BackStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x04000D5D RID: 3421
		internal static readonly DependencyProperty BackStackProperty = JournalNavigationScope.BackStackPropertyKey.DependencyProperty;

		// Token: 0x04000D5E RID: 3422
		private static readonly DependencyPropertyKey ForwardStackPropertyKey = DependencyProperty.RegisterReadOnly("ForwardStack", typeof(IEnumerable), typeof(JournalNavigationScope), new FrameworkPropertyMetadata(null));

		// Token: 0x04000D5F RID: 3423
		internal static readonly DependencyProperty ForwardStackProperty = JournalNavigationScope.ForwardStackPropertyKey.DependencyProperty;

		// Token: 0x04000D60 RID: 3424
		private IJournalNavigationScopeHost _host;

		// Token: 0x04000D61 RID: 3425
		private NavigationService _rootNavSvc;

		// Token: 0x04000D62 RID: 3426
		private Journal _journal;
	}
}
