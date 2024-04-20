using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.Net.Cache;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Navigation;
using MS.Internal.Utility;
using MS.Utility;

namespace System.Windows.Navigation
{
	// Token: 0x020005BB RID: 1467
	public sealed class NavigationService : IContentContainer
	{
		// Token: 0x060046BD RID: 18109 RVA: 0x00226BA4 File Offset: 0x00225BA4
		internal NavigationService(INavigator nav)
		{
			this.INavigatorHost = nav;
			if (!(nav is NavigationWindow))
			{
				this.GuidId = Guid.NewGuid();
			}
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00226C00 File Offset: 0x00225C00
		private void ResetPendingNavigationState(NavigationStatus newState)
		{
			JournalNavigationScope journalScope = this.JournalScope;
			if (journalScope != null && journalScope.RootNavigationService != this)
			{
				journalScope.RootNavigationService.BytesRead -= this._bytesRead;
				journalScope.RootNavigationService.MaxBytes -= this._maxBytes;
			}
			this._navStatus = newState;
			this._bytesRead = 0L;
			this._maxBytes = 0L;
			this._navigateQueueItem = null;
			this._request = null;
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x00226C78 File Offset: 0x00225C78
		private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			e.Handled = true;
			string target = e.Target;
			Uri bpu = e.Uri;
			if (bpu != null && !bpu.IsAbsoluteUri)
			{
				DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
				IUriContext uriContext = dependencyObject as IUriContext;
				if (uriContext == null)
				{
					throw new Exception(SR.Get("MustImplementIUriContext", new object[]
					{
						typeof(IUriContext)
					}));
				}
				bpu = BindUriHelper.GetUriToNavigate(dependencyObject, uriContext.BaseUri, e.Uri);
			}
			INavigatorBase navigator = null;
			bool flag = true;
			if (!string.IsNullOrEmpty(target))
			{
				navigator = this.FindTarget(target);
				if (navigator == null && this.JournalScope != null)
				{
					navigator = this.JournalScope.FindTarget(target);
				}
				if (navigator == null)
				{
					NavigationWindow navigationWindow = this.FindNavigationWindow();
					if (navigationWindow != null)
					{
						navigator = NavigationService.FindTargetInNavigationWindow(navigationWindow, target);
					}
					if (navigator == null)
					{
						navigator = NavigationService.FindTargetInApplication(target);
						if (navigator != null)
						{
							flag = ((DispatcherObject)navigator).CheckAccess();
						}
					}
				}
			}
			else
			{
				navigator = this.INavigatorHost;
			}
			if (navigator == null)
			{
				throw new ArgumentException(SR.Get("HyperLinkTargetNotFound"));
			}
			if (flag)
			{
				navigator.Navigate(bpu);
				return;
			}
			((DispatcherObject)navigator).Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback((object unused) => navigator.Navigate(bpu)), null);
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x00226E08 File Offset: 0x00225E08
		private static bool IsSameUri(Uri baseUri, Uri a, Uri b, bool withFragment)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			Uri resolvedUri = BindUriHelper.GetResolvedUri(baseUri, a);
			Uri resolvedUri2 = BindUriHelper.GetResolvedUri(baseUri, b);
			bool flag = resolvedUri.Equals(resolvedUri2);
			if (flag && withFragment)
			{
				flag = (flag && string.Compare(resolvedUri.Fragment, resolvedUri2.Fragment, StringComparison.OrdinalIgnoreCase) == 0);
			}
			return flag;
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x00226E6C File Offset: 0x00225E6C
		private void NavigateToFragmentOrCustomContentState(Uri uri, object navState)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			JournalEntry journalEntry = null;
			if (navigateInfo != null)
			{
				journalEntry = navigateInfo.JournalEntry;
			}
			NavigationMode navigationMode = (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode;
			CustomJournalStateInternal rootViewerState = this.GetRootViewerState(JournalReason.FragmentNavigation);
			string elementId = (uri != null) ? BindUriHelper.GetFragment(uri) : null;
			bool flag = journalEntry != null && journalEntry.CustomContentState != null;
			bool flag2 = this.NavigateToFragment(elementId, !flag);
			if (navigationMode == NavigationMode.Back || navigationMode == NavigationMode.Forward || (flag2 && !NavigationService.IsSameUri(null, this._currentSource, uri, true)))
			{
				try
				{
					this._rootViewerStateToSave = rootViewerState;
					this.UpdateJournal(navigationMode, JournalReason.FragmentNavigation, journalEntry);
				}
				finally
				{
					this._rootViewerStateToSave = null;
				}
				Uri resolvedUri = BindUriHelper.GetResolvedUri(this._currentSource, uri);
				this._currentSource = resolvedUri;
				this._currentCleanSource = BindUriHelper.GetUriRelativeToPackAppBase(uri);
			}
			this._navStatus = NavigationStatus.Navigated;
			this.HandleNavigated(navState, false);
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x00226F50 File Offset: 0x00225F50
		private bool NavigateToFragment(string elementId, bool scrollToTopOnEmptyFragment)
		{
			if (this.FireFragmentNavigation(elementId))
			{
				return true;
			}
			if (!string.IsNullOrEmpty(elementId))
			{
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode((DependencyObject)this._bp, elementId);
				NavigationService.BringIntoView(dependencyObject);
				return dependencyObject != null;
			}
			if (!scrollToTopOnEmptyFragment)
			{
				return false;
			}
			this.ScrollContentToTop();
			return true;
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x00226F8C File Offset: 0x00225F8C
		private void ScrollContentToTop()
		{
			if (this._bp != null)
			{
				FrameworkElement frameworkElement = this._bp as FrameworkElement;
				if (frameworkElement != null)
				{
					IEnumerator logicalChildren = frameworkElement.LogicalChildren;
					if (logicalChildren != null && logicalChildren.MoveNext())
					{
						ScrollViewer scrollViewer = logicalChildren.Current as ScrollViewer;
						if (scrollViewer != null)
						{
							scrollViewer.ScrollToTop();
							return;
						}
					}
				}
				IInputElement inputElement = this._bp as IInputElement;
				if (inputElement != null && ScrollBar.ScrollToTopCommand.CanExecute(null, inputElement))
				{
					ScrollBar.ScrollToTopCommand.Execute(null, inputElement);
					return;
				}
				NavigationService.BringIntoView(this._bp as DependencyObject);
			}
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x00227014 File Offset: 0x00226014
		private static void BringIntoView(DependencyObject elem)
		{
			FrameworkElement frameworkElement = elem as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
				return;
			}
			FrameworkContentElement frameworkContentElement = elem as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.BringIntoView();
			}
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x00227042 File Offset: 0x00226042
		private JournalNavigationScope EnsureJournal()
		{
			if (this._journalScope == null && this._navigatorHost != null)
			{
				this._journalScope = this._navigatorHost.GetJournal(true);
			}
			return this._journalScope;
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0022706C File Offset: 0x0022606C
		private bool IsConsistent(NavigateInfo navInfo)
		{
			return navInfo == null || (navInfo.IsConsistent && (navInfo.JournalEntry == null || navInfo.JournalEntry.NavigationServiceId == this._guidId));
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x0022709D File Offset: 0x0022609D
		private bool IsJournalNavigation(NavigateInfo navInfo)
		{
			return navInfo != null && (navInfo.NavigationMode == NavigationMode.Back || navInfo.NavigationMode == NavigationMode.Forward);
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x002270B8 File Offset: 0x002260B8
		private CustomJournalStateInternal GetRootViewerState(JournalReason journalReason)
		{
			if (this._navigatorHostImpl != null && !(this._bp is Visual))
			{
				IJournalState journalState = this._navigatorHostImpl.FindRootViewer() as IJournalState;
				if (journalState != null)
				{
					return journalState.GetJournalState(journalReason);
				}
			}
			return null;
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x002270F8 File Offset: 0x002260F8
		private bool RestoreRootViewerState(CustomJournalStateInternal rvs)
		{
			Visual visual = this._navigatorHostImpl.FindRootViewer();
			if (visual == null)
			{
				return false;
			}
			IJournalState journalState = visual as IJournalState;
			if (journalState != null)
			{
				journalState.RestoreJournalState(rvs);
			}
			return true;
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x00227128 File Offset: 0x00226128
		internal static INavigatorBase FindTargetInApplication(string targetName)
		{
			if (Application.Current == null)
			{
				return null;
			}
			INavigatorBase navigatorBase = NavigationService.FindTargetInWindowCollection(Application.Current.WindowsInternal.Clone(), targetName);
			if (navigatorBase == null)
			{
				navigatorBase = NavigationService.FindTargetInWindowCollection(Application.Current.NonAppWindowsInternal.Clone(), targetName);
			}
			return navigatorBase;
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00227170 File Offset: 0x00226170
		private static INavigatorBase FindTargetInWindowCollection(WindowCollection wc, string targetName)
		{
			NavigationWindow nw = null;
			DispatcherOperationCallback <>9__0;
			for (int i = 0; i < wc.Count; i++)
			{
				nw = (wc[i] as NavigationWindow);
				if (nw != null)
				{
					INavigatorBase navigatorBase;
					if (nw.CheckAccess())
					{
						navigatorBase = NavigationService.FindTargetInNavigationWindow(nw, targetName);
					}
					else
					{
						Dispatcher dispatcher = nw.Dispatcher;
						DispatcherPriority priority = DispatcherPriority.Send;
						DispatcherOperationCallback method;
						if ((method = <>9__0) == null)
						{
							method = (<>9__0 = ((object unused) => NavigationService.FindTargetInNavigationWindow(nw, targetName)));
						}
						navigatorBase = (INavigator)dispatcher.Invoke(priority, method, null);
					}
					if (navigatorBase != null)
					{
						return navigatorBase;
					}
				}
			}
			return null;
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x00227220 File Offset: 0x00226220
		private static INavigatorBase FindTargetInNavigationWindow(NavigationWindow navigationWindow, string navigatorId)
		{
			if (navigationWindow != null)
			{
				return navigationWindow.NavigationService.FindTarget(navigatorId);
			}
			return null;
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x00227234 File Offset: 0x00226234
		internal void InvalidateJournalNavigationScope()
		{
			if (this._journalScope != null && this._journalScope.Journal.HasUncommittedNavigation)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_CantChangeJournalOwnership"));
			}
			this._journalScope = null;
			for (int i = this.ChildNavigationServices.Count - 1; i >= 0; i--)
			{
				((NavigationService)this.ChildNavigationServices[i]).InvalidateJournalNavigationScope();
			}
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x002272A0 File Offset: 0x002262A0
		internal void OnParentNavigationServiceChanged()
		{
			NavigationService parentNavigationService = this._parentNavigationService;
			NavigationService navigationService = ((DependencyObject)this.INavigatorHost).GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
			if (navigationService == parentNavigationService)
			{
				return;
			}
			if (parentNavigationService != null)
			{
				parentNavigationService.RemoveChild(this);
			}
			if (navigationService != null)
			{
				navigationService.AddChild(this);
			}
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x002272E8 File Offset: 0x002262E8
		internal void AddChild(NavigationService ncChild)
		{
			if (ncChild == this)
			{
				throw new Exception(SR.Get("LoopDetected", new object[]
				{
					this._currentCleanSource
				}));
			}
			Invariant.Assert(ncChild.ParentNavigationService == null);
			Invariant.Assert(ncChild.JournalScope == null || ncChild.IsJournalLevelContainer, "Parentless NavigationService has a reference to a JournalNavigationScope its host navigator doesn't own.");
			this.ChildNavigationServices.Add(ncChild);
			ncChild._parentNavigationService = this;
			if (this.JournalScope != null)
			{
				this.JournalScope.Journal.UpdateView();
			}
			if (this.NavStatus == NavigationStatus.Stopped)
			{
				ncChild.INavigatorHost.StopLoading();
				return;
			}
			if (ncChild.NavStatus != NavigationStatus.Idle && ncChild.NavStatus != NavigationStatus.Stopped && this.NavStatus != NavigationStatus.Idle && this.NavStatus != NavigationStatus.Stopped)
			{
				this.PendingNavigationList.Add(ncChild);
			}
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x002273B4 File Offset: 0x002263B4
		internal void RemoveChild(NavigationService ncChild)
		{
			this.ChildNavigationServices.Remove(ncChild);
			ncChild._parentNavigationService = null;
			if (!ncChild.IsJournalLevelContainer)
			{
				ncChild.InvalidateJournalNavigationScope();
			}
			if (this.JournalScope != null)
			{
				this.JournalScope.Journal.UpdateView();
			}
			if (this.PendingNavigationList.Contains(ncChild))
			{
				this.PendingNavigationList.Remove(ncChild);
				this.HandleLoadCompleted(null);
			}
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x0022741C File Offset: 0x0022641C
		internal NavigationService FindTarget(Guid navigationServiceId)
		{
			if (this.GuidId == navigationServiceId)
			{
				return this;
			}
			foreach (object obj in this.ChildNavigationServices)
			{
				NavigationService navigationService = ((NavigationService)obj).FindTarget(navigationServiceId);
				if (navigationService != null)
				{
					return navigationService;
				}
			}
			return null;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00227490 File Offset: 0x00226490
		internal INavigatorBase FindTarget(string name)
		{
			FrameworkElement frameworkElement = this.INavigatorHost as FrameworkElement;
			if (string.Compare(name, frameworkElement.Name, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return this.INavigatorHost;
			}
			INavigatorBase navigatorBase = null;
			foreach (object obj in this.ChildNavigationServices)
			{
				navigatorBase = ((NavigationService)obj).FindTarget(name);
				if (navigatorBase != null)
				{
					return navigatorBase;
				}
			}
			return navigatorBase;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x0022751C File Offset: 0x0022651C
		internal bool IsContentKeepAlive()
		{
			bool flag = true;
			DependencyObject dependencyObject = this._bp as DependencyObject;
			if (dependencyObject != null)
			{
				flag = JournalEntry.GetKeepAlive(dependencyObject);
				if (!flag)
				{
					object obj = dependencyObject as PageFunctionBase;
					bool flag2 = !this.CanReloadFromUri;
					if (obj == null && flag2)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00227560 File Offset: 0x00226560
		private void SetBaseUri(DependencyObject dobj, Uri fullUri)
		{
			Invariant.Assert(dobj != null && !dobj.IsSealed);
			if ((Uri)dobj.GetValue(BaseUriHelper.BaseUriProperty) == null && fullUri != null)
			{
				dobj.SetValue(BaseUriHelper.BaseUriProperty, fullUri);
			}
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x002275B0 File Offset: 0x002265B0
		private bool UnhookOldTree(object oldTree)
		{
			DependencyObject dependencyObject = oldTree as DependencyObject;
			if (dependencyObject != null && !dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(NavigationService.NavigationServiceProperty, null);
			}
			IInputElement inputElement = oldTree as IInputElement;
			if (inputElement != null && inputElement.IsKeyboardFocusWithin)
			{
				if (dependencyObject != null && this.JournalScope != null)
				{
					DependencyObject dependencyObject2 = (DependencyObject)this.INavigatorHost;
					if (!(bool)dependencyObject2.GetValue(FocusManager.IsFocusScopeProperty))
					{
						dependencyObject2 = FocusManager.GetFocusScope(dependencyObject2);
					}
					FocusManager.SetFocusedElement(dependencyObject2, null);
				}
				Keyboard.PrimaryDevice.Focus(null);
			}
			PageFunctionBase pageFunctionBase = oldTree as PageFunctionBase;
			if (pageFunctionBase != null)
			{
				pageFunctionBase.FinishHandler = null;
			}
			bool result = true;
			if (this.IsContentKeepAlive())
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x00227654 File Offset: 0x00226654
		private bool HookupNewTree(object newTree, NavigateInfo navInfo, Uri newUri)
		{
			if (newTree != null && this.IsJournalNavigation(navInfo))
			{
				navInfo.JournalEntry.RestoreState(newTree);
			}
			PageFunctionReturnInfo pageFunctionReturnInfo = navInfo as PageFunctionReturnInfo;
			PageFunctionBase pageFunctionBase = (pageFunctionReturnInfo != null) ? pageFunctionReturnInfo.FinishingChildPageFunction : null;
			if (pageFunctionBase != null)
			{
				object returnEventArgs = (pageFunctionReturnInfo != null) ? pageFunctionReturnInfo.ReturnEventArgs : null;
				if (newTree != null)
				{
					this.FireChildPageFunctionReturnEvent(newTree, pageFunctionBase, returnEventArgs);
					if (this._navigateQueueItem != null)
					{
						if (pageFunctionReturnInfo.JournalEntry != null)
						{
							pageFunctionReturnInfo.JournalEntry.SaveState(newTree);
						}
						return false;
					}
				}
			}
			if (NavigationService.IsPageFunction(newTree))
			{
				this.SetupPageFunctionHandlers(newTree);
				if ((navInfo == null || navInfo.NavigationMode == NavigationMode.New) && !this._doNotJournalCurrentContent)
				{
					PageFunctionBase pageFunctionBase2 = (PageFunctionBase)newTree;
					if (!pageFunctionBase2._Resume && pageFunctionBase2.ParentPageFunctionId == Guid.Empty && this._bp is PageFunctionBase)
					{
						pageFunctionBase2.ParentPageFunctionId = ((PageFunctionBase)this._bp).PageFunctionId;
					}
				}
			}
			DependencyObject dependencyObject = newTree as DependencyObject;
			if (dependencyObject != null && !dependencyObject.IsSealed)
			{
				dependencyObject.SetValue(NavigationService.NavigationServiceProperty, this);
				if (newUri != null && !BindUriHelper.StartWithFragment(newUri))
				{
					this.SetBaseUri(dependencyObject, newUri);
				}
			}
			this._webBrowser = (newTree as WebBrowser);
			return true;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x00227778 File Offset: 0x00226778
		private bool OnBeforeSwitchContent(object newBP, NavigateInfo navInfo, Uri newUri)
		{
			if (newBP != null && !this.HookupNewTree(newBP, navInfo, newUri))
			{
				return false;
			}
			if (navInfo == null)
			{
				this.UpdateJournal(NavigationMode.New, JournalReason.NewContentNavigation, null);
			}
			else if (navInfo.NavigationMode != NavigationMode.Refresh)
			{
				this.UpdateJournal(navInfo.NavigationMode, JournalReason.NewContentNavigation, navInfo.JournalEntry);
			}
			if (this._navigateQueueItem != null)
			{
				return false;
			}
			if (this.UnhookOldTree(this._bp))
			{
				DisposeTreeQueueItem @object = new DisposeTreeQueueItem(this._bp);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(@object.Dispatch), null);
			}
			return true;
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x002277FF File Offset: 0x002267FF
		internal void VisualTreeAvailable(Visual v)
		{
			if (v != this._oldRootVisual)
			{
				if (this._oldRootVisual != null)
				{
					this._oldRootVisual.SetValue(NavigationService.NavigationServiceProperty, null);
				}
				if (v != null)
				{
					v.SetValue(NavigationService.NavigationServiceProperty, this);
				}
				this._oldRootVisual = v;
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x0022783C File Offset: 0x0022683C
		void IContentContainer.OnContentReady(ContentType contentType, object bp, Uri bpu, object navState)
		{
			Invariant.Assert(bpu == null || bpu.IsAbsoluteUri, "Content URI must be absolute.");
			if (this.IsDisposed)
			{
				return;
			}
			if (!this.IsValidRootElement(bp))
			{
				throw new InvalidOperationException(SR.Get("WrongNavigateRootElement", new object[]
				{
					bp.ToString()
				}));
			}
			this.ResetPendingNavigationState(NavigationStatus.Navigated);
			NavigateInfo navigateInfo = navState as NavigateInfo;
			NavigationMode navigationMode = (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode;
			if (bpu == null)
			{
				bpu = ((navigateInfo == null) ? null : navigateInfo.Source);
			}
			Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(bpu);
			if (this.PreBPReady != null)
			{
				BPReadyEventArgs bpreadyEventArgs = new BPReadyEventArgs(bp, bpu);
				this.PreBPReady(this, bpreadyEventArgs);
				if (bpreadyEventArgs.Cancel)
				{
					this._navStatus = NavigationStatus.Idle;
					return;
				}
			}
			bool flag = false;
			if (bp == this._bp)
			{
				flag = true;
				this._bp = null;
				if (this.BPReady != null)
				{
					this.BPReady(this, new BPReadyEventArgs(null, null));
				}
			}
			else
			{
				if (!this.OnBeforeSwitchContent(bp, navigateInfo, bpu))
				{
					return;
				}
				if (navigationMode != NavigationMode.Refresh)
				{
					if (navigateInfo == null || navigateInfo.JournalEntry == null)
					{
						this._contentId += 1U;
						this._journalEntryGroupState = null;
					}
					else
					{
						this._contentId = navigateInfo.JournalEntry.ContentId;
						this._journalEntryGroupState = navigateInfo.JournalEntry.JEGroupState;
					}
					this._currentSource = bpu;
					this._currentCleanSource = uriRelativeToPackAppBase;
				}
			}
			this._bp = bp;
			if (this.BPReady != null)
			{
				this.BPReady(this, new BPReadyEventArgs(this._bp, bpu));
			}
			this.HandleNavigated(navState, !flag);
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x002279C8 File Offset: 0x002269C8
		void IContentContainer.OnNavigationProgress(Uri sourceUri, long bytesRead, long maxBytes)
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (!sourceUri.Equals(this.Source))
			{
				return;
			}
			NavigationService navigationService = null;
			if (this.JournalScope != null && this.JournalScope.RootNavigationService != this)
			{
				navigationService = this.JournalScope.RootNavigationService;
				navigationService.BytesRead += bytesRead - this._bytesRead;
				navigationService.MaxBytes += maxBytes - this._maxBytes;
			}
			this._bytesRead = bytesRead;
			this._maxBytes = maxBytes;
			this.FireNavigationProgress(sourceUri);
			if (navigationService == null)
			{
				return;
			}
			navigationService.FireNavigationProgress(sourceUri);
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00227A5B File Offset: 0x00226A5B
		void IContentContainer.OnStreamClosed(Uri sourceUri)
		{
			if (!sourceUri.Equals(this.Source))
			{
				return;
			}
			this._asyncObjectConverter = null;
			this.HandleLoadCompleted(null);
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00227A7A File Offset: 0x00226A7A
		public static NavigationService GetNavigationService(DependencyObject dependencyObject)
		{
			if (dependencyObject == null)
			{
				throw new ArgumentNullException("dependencyObject");
			}
			return dependencyObject.GetValue(NavigationService.NavigationServiceProperty) as NavigationService;
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x00227A9C File Offset: 0x00226A9C
		// (set) Token: 0x060046DE RID: 18142 RVA: 0x00227B08 File Offset: 0x00226B08
		public Uri Source
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				if (this._recursiveNavigateList.Count > 0)
				{
					return BindUriHelper.GetUriRelativeToPackAppBase((this._recursiveNavigateList[this._recursiveNavigateList.Count - 1] as NavigateQueueItem).Source);
				}
				if (this._navigateQueueItem != null)
				{
					return BindUriHelper.GetUriRelativeToPackAppBase(this._navigateQueueItem.Source);
				}
				return this._currentCleanSource;
			}
			set
			{
				this.Navigate(value);
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060046DF RID: 18143 RVA: 0x00227B12 File Offset: 0x00226B12
		public Uri CurrentSource
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				return this._currentCleanSource;
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x060046E0 RID: 18144 RVA: 0x00227B24 File Offset: 0x00226B24
		// (set) Token: 0x060046E1 RID: 18145 RVA: 0x00227B36 File Offset: 0x00226B36
		public object Content
		{
			get
			{
				if (this.IsDisposed)
				{
					return null;
				}
				return this._bp;
			}
			set
			{
				this.Navigate(value);
			}
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x00227B40 File Offset: 0x00226B40
		public void AddBackEntry(CustomContentState state)
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this._bp == null)
			{
				throw new InvalidOperationException(SR.Get("InvalidOperation_AddBackEntryNoContent"));
			}
			this._customContentStateToSave = state;
			JournalEntry journalEntry = this.UpdateJournal(NavigationMode.New, JournalReason.AddBackEntry, null);
			this._customContentStateToSave = null;
			if (journalEntry != null && journalEntry.CustomContentState == null)
			{
				this.RemoveBackEntry();
				throw new InvalidOperationException(SR.Get("InvalidOperation_MustImplementIPCCSOrHandleNavigating", new object[]
				{
					(this._bp != null) ? this._bp.GetType().ToString() : "null"
				}));
			}
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00227BD0 File Offset: 0x00226BD0
		public JournalEntry RemoveBackEntry()
		{
			if (this.IsDisposed)
			{
				return null;
			}
			if (this.JournalScope == null)
			{
				return null;
			}
			return this.JournalScope.RemoveBackEntry();
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x00227BF1 File Offset: 0x00226BF1
		public bool Navigate(Uri source)
		{
			return this.Navigate(source, null, false, false);
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x00227BFD File Offset: 0x00226BFD
		public bool Navigate(object root)
		{
			return this.Navigate(root, null);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00227C07 File Offset: 0x00226C07
		public bool Navigate(Uri source, object navigationState)
		{
			return this.Navigate(source, navigationState, false, false);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00227C13 File Offset: 0x00226C13
		public bool Navigate(Uri source, object navigationState, bool sandboxExternalContent)
		{
			return this.Navigate(source, navigationState, sandboxExternalContent, false);
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x00227C20 File Offset: 0x00226C20
		internal bool Navigate(Uri source, object navigationState, bool sandboxExternalContent, bool navigateOnSourceChanged)
		{
			if (this.IsDisposed)
			{
				return false;
			}
			NavigateInfo navigateInfo = navigationState as NavigateInfo;
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationStart, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, new object[]
				{
					(navigateInfo != null) ? navigateInfo.NavigationMode.ToString() : NavigationMode.New.ToString(),
					(source != null) ? ("\"" + source.ToString() + "\"") : "(null)"
				});
			}
			Invariant.Assert(this.IsConsistent(navigateInfo));
			WebRequest webRequest = null;
			bool flag = false;
			Uri uri = null;
			if (source != null)
			{
				if (BindUriHelper.StartWithFragment(source) || BindUriHelper.StartWithFragment(BindUriHelper.GetUriRelativeToPackAppBase(source)))
				{
					uri = BindUriHelper.GetResolvedUri(this._currentSource, source);
					flag = true;
				}
				else
				{
					uri = BindUriHelper.GetResolvedUri(source);
					flag = ((navigateInfo == null || navigateInfo.JournalEntry == null || navigateInfo.JournalEntry.ContentId == this._contentId) && NavigationService.IsSameUri(null, uri, this._currentSource, false));
				}
				if (navigateInfo != null && navigateInfo.NavigationMode == NavigationMode.Refresh)
				{
					flag = false;
				}
				if (!flag)
				{
					webRequest = this.CreateWebRequest(uri, navigateInfo);
					if (webRequest == null)
					{
						return false;
					}
				}
			}
			if (!this.HandleNavigating(uri, null, navigationState, webRequest, navigateOnSourceChanged))
			{
				return false;
			}
			if (source == null && this._bp == null)
			{
				this.ResetPendingNavigationState(NavigationStatus.Idle);
				return true;
			}
			if (flag)
			{
				this.NavigateToFragmentOrCustomContentState(uri, navigationState);
				return true;
			}
			this._navigateQueueItem.PostNavigation();
			return true;
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x00227D96 File Offset: 0x00226D96
		private void InformBrowserAboutStoppedNavigation()
		{
			if (this.Application != null && this.Application.CheckAccess())
			{
				this.Application.PerformNavigationStateChangeTasks(true, false, Application.NavigationStateChange.Stopped);
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x00227DBC File Offset: 0x00226DBC
		public bool Navigate(object root, object navigationState)
		{
			if (this.IsDisposed)
			{
				return false;
			}
			NavigateInfo navigateInfo = navigationState as NavigateInfo;
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationStart, EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, new object[]
				{
					(navigateInfo != null) ? navigateInfo.NavigationMode.ToString() : NavigationMode.New.ToString(),
					(root != null) ? root.ToString() : "(null)"
				});
			}
			Invariant.Assert(this.IsConsistent(navigateInfo));
			if (navigateInfo == null)
			{
				PageFunctionBase pageFunctionBase = root as PageFunctionBase;
				if (pageFunctionBase != null && (pageFunctionBase._Resume || pageFunctionBase._Saver != null))
				{
					throw new InvalidOperationException(SR.Get("InvalidOperation_CannotReenterPageFunction"));
				}
			}
			Uri uri = (navigateInfo == null) ? null : navigateInfo.Source;
			if (!this.HandleNavigating(uri, root, navigationState, null, false))
			{
				return false;
			}
			if (root == this._bp && (navigateInfo == null || navigateInfo.NavigationMode != NavigationMode.Refresh))
			{
				this.NavigateToFragmentOrCustomContentState(uri, navigationState);
				if (this.IsJournalNavigation(navigateInfo))
				{
					this._journalEntryGroupState = navigateInfo.JournalEntry.JEGroupState;
					this._contentId = this._journalEntryGroupState.ContentId;
					this._journalScope.Journal.UpdateView();
				}
				return true;
			}
			this._navigateQueueItem.PostNavigation();
			return true;
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x060046EB RID: 18155 RVA: 0x00227EFC File Offset: 0x00226EFC
		public bool CanGoForward
		{
			get
			{
				return this.JournalScope != null && this.JournalScope.CanGoForward;
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x060046EC RID: 18156 RVA: 0x00227F13 File Offset: 0x00226F13
		public bool CanGoBack
		{
			get
			{
				return this.JournalScope != null && this.JournalScope.CanGoBack;
			}
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x00227F2A File Offset: 0x00226F2A
		public void GoForward()
		{
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("NoForwardEntry"));
			}
			this.JournalScope.GoForward();
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x00227F4F File Offset: 0x00226F4F
		public void GoBack()
		{
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("NoBackEntry"));
			}
			this.JournalScope.GoBack();
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00227F74 File Offset: 0x00226F74
		public void StopLoading()
		{
			this.DoStopLoading(true, true);
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00227F80 File Offset: 0x00226F80
		private void DoStopLoading(bool clearRecursiveNavigations, bool fireEvents)
		{
			bool flag = false;
			object navState = null;
			if (this._asyncObjectConverter != null)
			{
				this._asyncObjectConverter.CancelAsync();
				this._asyncObjectConverter = null;
				Invariant.Assert(this._webResponse != null);
				this._webResponse.Close();
				this._webResponse = null;
			}
			else if (this._navStatus != NavigationStatus.Navigating && this._webResponse != null)
			{
				this._webResponse.Close();
				this._webResponse = null;
			}
			this._navStatus = NavigationStatus.Stopped;
			if (this._navigateQueueItem != null)
			{
				this._navigateQueueItem.Stop();
				if (this.JournalScope != null && clearRecursiveNavigations)
				{
					this.JournalScope.AbortJournalNavigation();
				}
				if (this._request != null)
				{
					try
					{
						WebRequest request = this._request;
						this._request = null;
						request.Abort();
					}
					catch (NotSupportedException)
					{
					}
					catch (NotImplementedException)
					{
					}
				}
				navState = this._navigateQueueItem.NavState;
				this.ResetPendingNavigationState(NavigationStatus.Stopped);
				flag = true;
			}
			if (clearRecursiveNavigations && this._recursiveNavigateList.Count > 0)
			{
				this._recursiveNavigateList.Clear();
				flag = true;
			}
			if (this._navigatorHostImpl != null)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(true);
			}
			bool fireEvents2 = false;
			try
			{
				if (fireEvents && flag)
				{
					this.FireNavigationStopped(navState);
				}
				fireEvents2 = true;
			}
			finally
			{
				int i = 0;
				try
				{
					while (i < this._childNavigationServices.Count)
					{
						((NavigationService)this._childNavigationServices[i]).DoStopLoading(true, fireEvents2);
						i++;
					}
				}
				finally
				{
					if (++i < this._childNavigationServices.Count)
					{
						while (i < this._childNavigationServices.Count)
						{
							((NavigationService)this._childNavigationServices[i]).DoStopLoading(true, false);
							i++;
						}
					}
					this.PendingNavigationList.Clear();
					if (this._parentNavigationService != null && this._parentNavigationService.PendingNavigationList.Contains(this))
					{
						this._parentNavigationService.PendingNavigationList.Remove(this);
						if (fireEvents)
						{
							this._parentNavigationService.HandleLoadCompleted(null);
						}
					}
				}
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0022818C File Offset: 0x0022718C
		public void Refresh()
		{
			if (this.IsDisposed)
			{
				return;
			}
			if (this.CanReloadFromUri)
			{
				this.Navigate(this._currentSource, new NavigateInfo(this._currentSource, NavigationMode.Refresh));
				return;
			}
			if (this._bp != null)
			{
				this.Navigate(this._bp, new NavigateInfo(this._currentSource, NavigationMode.Refresh));
			}
		}

		// Token: 0x1400009B RID: 155
		// (add) Token: 0x060046F2 RID: 18162 RVA: 0x002281E8 File Offset: 0x002271E8
		// (remove) Token: 0x060046F3 RID: 18163 RVA: 0x00228220 File Offset: 0x00227220
		public event NavigationFailedEventHandler NavigationFailed;

		// Token: 0x1400009C RID: 156
		// (add) Token: 0x060046F4 RID: 18164 RVA: 0x00228255 File Offset: 0x00227255
		// (remove) Token: 0x060046F5 RID: 18165 RVA: 0x0022826E File Offset: 0x0022726E
		public event NavigatingCancelEventHandler Navigating
		{
			add
			{
				this._navigating = (NavigatingCancelEventHandler)Delegate.Combine(this._navigating, value);
			}
			remove
			{
				this._navigating = (NavigatingCancelEventHandler)Delegate.Remove(this._navigating, value);
			}
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00228288 File Offset: 0x00227288
		private bool FireNavigating(Uri source, object bp, object navState, WebRequest request)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(source);
			if (bp != null && navigateInfo != null && !(navigateInfo is PageFunctionReturnInfo) && (!(bp is PageFunctionBase) || !(bp as PageFunctionBase)._Resume) && navigateInfo.Source != null && navigateInfo.NavigationMode == NavigationMode.New)
			{
				return this._navigateQueueItem == null;
			}
			CustomContentState customContentState = (navigateInfo != null && navigateInfo.JournalEntry != null) ? navigateInfo.JournalEntry.CustomContentState : null;
			object extraData = (navigateInfo == null) ? navState : null;
			NavigatingCancelEventArgs navigatingCancelEventArgs = new NavigatingCancelEventArgs(uriRelativeToPackAppBase, bp, customContentState, extraData, (navigateInfo == null) ? NavigationMode.New : navigateInfo.NavigationMode, request, this.INavigatorHost, this.IsNavigationInitiator);
			if (this._navigating != null)
			{
				this._navigating(this.INavigatorHost, navigatingCancelEventArgs);
			}
			if (!navigatingCancelEventArgs.Cancel && this.Application != null && this.Application.CheckAccess())
			{
				this.Application.FireNavigating(navigatingCancelEventArgs, this._bp == null);
			}
			this._customContentStateToSave = navigatingCancelEventArgs.ContentStateToSave;
			if (navigatingCancelEventArgs.Cancel && this.JournalScope != null)
			{
				this.JournalScope.AbortJournalNavigation();
			}
			return !navigatingCancelEventArgs.Cancel && !this.IsDisposed;
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x002283BC File Offset: 0x002273BC
		private bool HandleNavigating(Uri source, object content, object navState, WebRequest newRequest, bool navigateOnSourceChanged)
		{
			NavigateInfo navigateInfo = navState as NavigateInfo;
			if (navigateInfo != null && source == null)
			{
				source = navigateInfo.Source;
			}
			NavigateQueueItem navigateQueueItem = new NavigateQueueItem(source, content, (navigateInfo != null) ? navigateInfo.NavigationMode : NavigationMode.New, navState, this);
			this._recursiveNavigateList.Add(navigateQueueItem);
			this._isNavInitiatorValid = false;
			if (this._navigatorHostImpl != null && !navigateOnSourceChanged)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(this.IsJournalNavigation(navigateInfo));
			}
			bool flag = false;
			try
			{
				flag = this.FireNavigating(source, content, navState, newRequest);
			}
			catch
			{
				this.CleanupAfterNavigationCancelled(navigateQueueItem);
				throw;
			}
			if (flag)
			{
				this.DoStopLoading(false, true);
				if (!this._recursiveNavigateList.Contains(navigateQueueItem))
				{
					return false;
				}
				this._recursiveNavigateList.Clear();
				this._navigateQueueItem = navigateQueueItem;
				this._request = newRequest;
				this._navStatus = NavigationStatus.Navigating;
			}
			else
			{
				this.CleanupAfterNavigationCancelled(navigateQueueItem);
			}
			return flag;
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x0022849C File Offset: 0x0022749C
		private void CleanupAfterNavigationCancelled(NavigateQueueItem localNavigateQueueItem)
		{
			if (this.JournalScope != null)
			{
				this.JournalScope.AbortJournalNavigation();
			}
			this._recursiveNavigateList.Remove(localNavigateQueueItem);
			if (this._navigatorHostImpl != null)
			{
				this._navigatorHostImpl.OnSourceUpdatedFromNavService(true);
			}
			this.InformBrowserAboutStoppedNavigation();
		}

		// Token: 0x1400009D RID: 157
		// (add) Token: 0x060046F9 RID: 18169 RVA: 0x002284D7 File Offset: 0x002274D7
		// (remove) Token: 0x060046FA RID: 18170 RVA: 0x002284F0 File Offset: 0x002274F0
		public event NavigatedEventHandler Navigated
		{
			add
			{
				this._navigated = (NavigatedEventHandler)Delegate.Combine(this._navigated, value);
			}
			remove
			{
				this._navigated = (NavigatedEventHandler)Delegate.Remove(this._navigated, value);
			}
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x0022850C File Offset: 0x0022750C
		private void FireNavigated(object navState)
		{
			object extraData = (navState is NavigateInfo) ? null : navState;
			try
			{
				NavigationEventArgs e = new NavigationEventArgs(this.CurrentSource, this.Content, extraData, this._webResponse, this.INavigatorHost, this.IsNavigationInitiator);
				if (this._navigated != null)
				{
					this._navigated(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigated(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x002285A4 File Offset: 0x002275A4
		private void HandleNavigated(object navState, bool navigatedToNewContent)
		{
			BrowserInteropHelper.IsInitialViewerNavigation = false;
			NavigateInfo navigateInfo = navState as NavigateInfo;
			bool flag = false;
			if (navigatedToNewContent && this._currentSource != null)
			{
				flag = !string.IsNullOrEmpty(BindUriHelper.GetFragment(this._currentSource));
			}
			if (navigateInfo != null && navigateInfo.JournalEntry != null)
			{
				JournalEntry journalEntry = navigateInfo.JournalEntry;
				if (journalEntry.CustomContentState != null)
				{
					journalEntry.CustomContentState.Replay(this, navigateInfo.NavigationMode);
					journalEntry.CustomContentState = null;
					if (this._navStatus != NavigationStatus.Navigated)
					{
						return;
					}
				}
				if (journalEntry.RootViewerState != null && this._navigatorHostImpl != null)
				{
					if (!navigatedToNewContent)
					{
						this.RestoreRootViewerState(journalEntry.RootViewerState);
						journalEntry.RootViewerState = null;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				FrameworkContentElement frameworkContentElement = this._bp as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.Loaded += this.OnContentLoaded;
				}
				else
				{
					FrameworkElement frameworkElement = this._bp as FrameworkElement;
					if (frameworkElement != null)
					{
						frameworkElement.Loaded += this.OnContentLoaded;
					}
				}
				this._cancelContentRenderedHandling = false;
			}
			if (this.JournalScope != null)
			{
				NavigateQueueItem navigateQueueItem = this._navigateQueueItem;
				this.JournalScope.Journal.UpdateView();
				if (this._navigateQueueItem != navigateQueueItem)
				{
					return;
				}
			}
			this.ResetPendingNavigationState(NavigationStatus.Navigated);
			this.FireNavigated(navState);
			if (navigatedToNewContent && NavigationService.IsPageFunction(this._bp))
			{
				this.HandlePageFunction(navigateInfo);
			}
			this.HandleLoadCompleted(navState);
		}

		// Token: 0x1400009E RID: 158
		// (add) Token: 0x060046FD RID: 18173 RVA: 0x002286F5 File Offset: 0x002276F5
		// (remove) Token: 0x060046FE RID: 18174 RVA: 0x0022870E File Offset: 0x0022770E
		public event NavigationProgressEventHandler NavigationProgress
		{
			add
			{
				this._navigationProgress = (NavigationProgressEventHandler)Delegate.Combine(this._navigationProgress, value);
			}
			remove
			{
				this._navigationProgress = (NavigationProgressEventHandler)Delegate.Remove(this._navigationProgress, value);
			}
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x00228728 File Offset: 0x00227728
		private void FireNavigationProgress(Uri source)
		{
			UIElement uielement = this.INavigatorHost as UIElement;
			if (uielement != null)
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(uielement);
				if (automationPeer != null)
				{
					NavigationWindowAutomationPeer.RaiseAsyncContentLoadedEvent(automationPeer, this.BytesRead, this.MaxBytes);
				}
			}
			NavigationProgressEventArgs e = new NavigationProgressEventArgs(source, this.BytesRead, this.MaxBytes, this.INavigatorHost);
			try
			{
				if (this._navigationProgress != null)
				{
					this._navigationProgress(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigationProgress(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		// Token: 0x1400009F RID: 159
		// (add) Token: 0x06004700 RID: 18176 RVA: 0x002287D4 File Offset: 0x002277D4
		// (remove) Token: 0x06004701 RID: 18177 RVA: 0x002287ED File Offset: 0x002277ED
		public event LoadCompletedEventHandler LoadCompleted
		{
			add
			{
				this._loadCompleted = (LoadCompletedEventHandler)Delegate.Combine(this._loadCompleted, value);
			}
			remove
			{
				this._loadCompleted = (LoadCompletedEventHandler)Delegate.Remove(this._loadCompleted, value);
			}
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x00228808 File Offset: 0x00227808
		private void FireLoadCompleted(bool isNavInitiator, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationEnd);
			object extraData = (navState is NavigateInfo) ? null : navState;
			NavigationEventArgs e = new NavigationEventArgs(this.CurrentSource, this.Content, extraData, this._webResponse, this.INavigatorHost, isNavInitiator);
			try
			{
				if (this._loadCompleted != null)
				{
					this._loadCompleted(this.INavigatorHost, e);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireLoadCompleted(e);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
		}

		// Token: 0x140000A0 RID: 160
		// (add) Token: 0x06004703 RID: 18179 RVA: 0x002288AC File Offset: 0x002278AC
		// (remove) Token: 0x06004704 RID: 18180 RVA: 0x002288C5 File Offset: 0x002278C5
		public event FragmentNavigationEventHandler FragmentNavigation
		{
			add
			{
				this._fragmentNavigation = (FragmentNavigationEventHandler)Delegate.Combine(this._fragmentNavigation, value);
			}
			remove
			{
				this._fragmentNavigation = (FragmentNavigationEventHandler)Delegate.Remove(this._fragmentNavigation, value);
			}
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x002288E0 File Offset: 0x002278E0
		private bool FireFragmentNavigation(string fragment)
		{
			if (string.IsNullOrEmpty(fragment))
			{
				return false;
			}
			FragmentNavigationEventArgs fragmentNavigationEventArgs = new FragmentNavigationEventArgs(fragment, this.INavigatorHost);
			try
			{
				if (this._fragmentNavigation != null)
				{
					this._fragmentNavigation(this, fragmentNavigationEventArgs);
				}
				if (this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireFragmentNavigation(fragmentNavigationEventArgs);
				}
			}
			catch
			{
				this.DoStopLoading(true, false);
				throw;
			}
			return fragmentNavigationEventArgs.Handled;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x00228960 File Offset: 0x00227960
		private void HandleLoadCompleted(object navState)
		{
			if (navState != null)
			{
				this._navState = navState;
			}
			if (this._asyncObjectConverter != null)
			{
				return;
			}
			if (this.PendingNavigationList.Count != 0 || this._navStatus != NavigationStatus.Navigated)
			{
				return;
			}
			NavigationService parentNavigationService = this.ParentNavigationService;
			this._navStatus = NavigationStatus.Idle;
			bool isNavigationInitiator = this.IsNavigationInitiator;
			this.FireLoadCompleted(isNavigationInitiator, this._navState);
			this._navState = null;
			if (this._webResponse != null)
			{
				this._webResponse.Close();
				this._webResponse = null;
			}
			if (!isNavigationInitiator && parentNavigationService != null)
			{
				parentNavigationService.PendingNavigationList.Remove(this);
				parentNavigationService.HandleLoadCompleted(null);
			}
		}

		// Token: 0x140000A1 RID: 161
		// (add) Token: 0x06004707 RID: 18183 RVA: 0x002289F3 File Offset: 0x002279F3
		// (remove) Token: 0x06004708 RID: 18184 RVA: 0x00228A0C File Offset: 0x00227A0C
		public event NavigationStoppedEventHandler NavigationStopped
		{
			add
			{
				this._stopped = (NavigationStoppedEventHandler)Delegate.Combine(this._stopped, value);
			}
			remove
			{
				this._stopped = (NavigationStoppedEventHandler)Delegate.Remove(this._stopped, value);
			}
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00228A28 File Offset: 0x00227A28
		private void FireNavigationStopped(object navState)
		{
			object extraData = (navState is NavigateInfo) ? null : navState;
			NavigationEventArgs e = new NavigationEventArgs(this.Source, this.Content, extraData, null, this.INavigatorHost, this.IsNavigationInitiator);
			if (this._stopped != null)
			{
				this._stopped(this.INavigatorHost, e);
			}
			if (this.Application != null && this.Application.CheckAccess())
			{
				this.Application.FireNavigationStopped(e);
			}
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x00228AA0 File Offset: 0x00227AA0
		private void OnContentLoaded(object sender, RoutedEventArgs args)
		{
			FrameworkContentElement frameworkContentElement = this._bp as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.Loaded -= this.OnContentLoaded;
			}
			else
			{
				((FrameworkElement)this._bp).Loaded -= this.OnContentLoaded;
			}
			this.OnFirstContentLayout();
			this._cancelContentRenderedHandling = true;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00228AF9 File Offset: 0x00227AF9
		private void ContentRenderedHandler(object sender, EventArgs args)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationContentRendered);
			if (this._cancelContentRenderedHandling)
			{
				this._cancelContentRenderedHandling = false;
				return;
			}
			this.OnFirstContentLayout();
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00228B24 File Offset: 0x00227B24
		private void OnFirstContentLayout()
		{
			if (this.CurrentSource != null)
			{
				string fragment = BindUriHelper.GetFragment(this.CurrentSource);
				if (!string.IsNullOrEmpty(fragment))
				{
					this.NavigateToFragment(fragment, false);
				}
			}
			if (this._journalScope != null)
			{
				JournalEntry currentEntry = this._journalScope.Journal.CurrentEntry;
				if (currentEntry != null && currentEntry.RootViewerState != null)
				{
					this.RestoreRootViewerState(currentEntry.RootViewerState);
					currentEntry.RootViewerState = null;
				}
			}
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00228B94 File Offset: 0x00227B94
		internal void DoNavigate(Uri source, NavigationMode f, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationAsyncWorkItem);
			if (this.IsDisposed)
			{
				return;
			}
			WebResponse webResponse = null;
			try
			{
				if (this._request is PackWebRequest)
				{
					webResponse = WpfWebRequestHelper.GetResponse(this._request);
					if (webResponse == null)
					{
						Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(this._request.RequestUri);
						throw new Exception(SR.Get("GetResponseFailed", new object[]
						{
							uriRelativeToPackAppBase.ToString()
						}));
					}
					this.GetObjectFromResponse(this._request, webResponse, source, navState);
				}
				else
				{
					RequestState state = new RequestState(this._request, source, navState, Dispatcher.CurrentDispatcher);
					this._request.BeginGetResponse(new AsyncCallback(this.HandleWebResponseOnRightDispatcher), state);
				}
			}
			catch (WebException e)
			{
				object extraData = (navState is NavigateInfo) ? null : navState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(source, extraData, this.INavigatorHost, this._request, webResponse, e)))
				{
					throw;
				}
			}
			catch (IOException e2)
			{
				object extraData2 = (navState is NavigateInfo) ? null : navState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(source, extraData2, this.INavigatorHost, this._request, webResponse, e2)))
				{
					throw;
				}
			}
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00228CC8 File Offset: 0x00227CC8
		private bool FireNavigationFailed(NavigationFailedEventArgs e)
		{
			this._navStatus = NavigationStatus.NavigationFailed;
			try
			{
				if (this.NavigationFailed != null)
				{
					this.NavigationFailed(this.INavigatorHost, e);
				}
				if (!e.Handled)
				{
					NavigationWindow navigationWindow = this.FindNavigationWindow();
					if (navigationWindow != null && navigationWindow.NavigationService != this)
					{
						navigationWindow.NavigationService.FireNavigationFailed(e);
					}
				}
				if (!e.Handled && this.Application != null && this.Application.CheckAccess())
				{
					this.Application.FireNavigationFailed(e);
				}
			}
			finally
			{
				if (this._navStatus == NavigationStatus.NavigationFailed)
				{
					this.DoStopLoading(true, false);
				}
			}
			return e.Handled;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00228D74 File Offset: 0x00227D74
		private WebRequest CreateWebRequest(Uri resolvedDestinationUri, NavigateInfo navInfo)
		{
			WebRequest webRequest = null;
			try
			{
				webRequest = PackWebRequestFactory.CreateWebRequest(resolvedDestinationUri);
			}
			catch (NotSupportedException)
			{
				if (AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(this.CurrentSource, resolvedDestinationUri, this.IsTopLevelContainer) == LaunchResult.NotLaunched)
				{
					throw;
				}
			}
			catch (SecurityException)
			{
				throw;
			}
			bool isRefresh = navInfo != null && navInfo.NavigationMode == NavigationMode.Refresh;
			WpfWebRequestHelper.ConfigCachePolicy(webRequest, isRefresh);
			return webRequest;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x00228DE0 File Offset: 0x00227DE0
		private void HandleWebResponseOnRightDispatcher(IAsyncResult ar)
		{
			if (this.IsDisposed)
			{
				return;
			}
			Dispatcher callbackDispatcher = ((RequestState)ar.AsyncState).CallbackDispatcher;
			if (Dispatcher.CurrentDispatcher != callbackDispatcher)
			{
				callbackDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(delegate(object unused)
				{
					this.HandleWebResponse(ar);
					return null;
				}), null);
				return;
			}
			callbackDispatcher.Invoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
			{
				this.HandleWebResponse(ar);
				return null;
			}), null);
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x00228E58 File Offset: 0x00227E58
		private void HandleWebResponse(IAsyncResult ar)
		{
			if (this.IsDisposed)
			{
				return;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationWebResponseReceived);
			RequestState requestState = (RequestState)ar.AsyncState;
			if (requestState.Request != this._request)
			{
				return;
			}
			WebResponse response = null;
			try
			{
				try
				{
					response = WpfWebRequestHelper.EndGetResponse(this._request, ar);
				}
				catch
				{
					throw;
				}
				this.GetObjectFromResponse(this._request, response, requestState.Source, requestState.NavState);
			}
			catch (WebException e)
			{
				object extraData = (requestState.NavState is NavigateInfo) ? null : requestState.NavState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(requestState.Source, extraData, this.INavigatorHost, this._request, response, e)))
				{
					throw;
				}
			}
			catch (IOException e2)
			{
				object extraData2 = (requestState.NavState is NavigateInfo) ? null : requestState.NavState;
				if (!this.FireNavigationFailed(new NavigationFailedEventArgs(requestState.Source, extraData2, this.INavigatorHost, this._request, response, e2)))
				{
					throw;
				}
			}
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00228F74 File Offset: 0x00227F74
		private void GetObjectFromResponse(WebRequest request, WebResponse response, Uri destinationUri, object navState)
		{
			bool flag = false;
			ContentType contentType = WpfWebRequestHelper.GetContentType(response);
			try
			{
				Stream responseStream = response.GetResponseStream();
				if (responseStream == null)
				{
					Uri uriRelativeToPackAppBase = BindUriHelper.GetUriRelativeToPackAppBase(this._request.RequestUri);
					throw new Exception(SR.Get("GetStreamFailed", new object[]
					{
						uriRelativeToPackAppBase.ToString()
					}));
				}
				long contentLength = response.ContentLength;
				Uri uriRelativeToPackAppBase2 = BindUriHelper.GetUriRelativeToPackAppBase(destinationUri);
				NavigateInfo navInfo = navState as NavigateInfo;
				bool sandboxExternalContent = this.SandboxExternalContent && !BaseUriHelper.IsPackApplicationUri(destinationUri) && MimeTypeMapper.XamlMime.AreTypeAndSubTypeEqual(contentType);
				Stream s = new BindStream(responseStream, contentLength, uriRelativeToPackAppBase2, this, Dispatcher.CurrentDispatcher);
				Invariant.Assert(this._webResponse == null && this._asyncObjectConverter == null);
				this._webResponse = response;
				this._asyncObjectConverter = null;
				bool canUseTopLevelBrowser = false;
				object objectAndCloseStream = MimeObjectFactory.GetObjectAndCloseStream(s, contentType, destinationUri, canUseTopLevelBrowser, sandboxExternalContent, true, this.IsJournalNavigation(navInfo), out this._asyncObjectConverter);
				if (objectAndCloseStream != null)
				{
					if (this._request == request)
					{
						((IContentContainer)this).OnContentReady(contentType, objectAndCloseStream, destinationUri, navState);
						flag = true;
					}
				}
				else
				{
					try
					{
						if (!this.IsTopLevelContainer || BrowserInteropHelper.IsInitialViewerNavigation)
						{
							throw new InvalidOperationException(SR.Get("FailedToConvertResource"));
						}
						this.DelegateToBrowser(response is PackWebResponse, destinationUri);
					}
					finally
					{
						this.DrainResponseStreamForPartialCacheFileBug(responseStream);
						responseStream.Close();
						this.ResetPendingNavigationState(this._navStatus);
					}
				}
			}
			finally
			{
				if (!flag)
				{
					response.Close();
					this._webResponse = null;
					if (this._asyncObjectConverter != null)
					{
						this._asyncObjectConverter.CancelAsync();
						this._asyncObjectConverter = null;
					}
				}
			}
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x00229120 File Offset: 0x00228120
		private void DelegateToBrowser(bool isPack, Uri destinationUri)
		{
			try
			{
				if (isPack)
				{
					destinationUri = BaseUriHelper.ConvertPackUriToAbsoluteExternallyVisibleUri(destinationUri);
				}
				if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationLaunchBrowser, EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, destinationUri.ToString());
				}
				AppSecurityManager.SafeLaunchBrowserDemandWhenUnsafe(this.CurrentSource, destinationUri, this.IsTopLevelContainer);
			}
			finally
			{
				this.InformBrowserAboutStoppedNavigation();
			}
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0022918C File Offset: 0x0022818C
		private void DrainResponseStreamForPartialCacheFileBug(Stream s)
		{
			if (this._request is HttpWebRequest && HttpWebRequest.DefaultCachePolicy != null && HttpWebRequest.DefaultCachePolicy is HttpRequestCachePolicy)
			{
				StreamReader streamReader = new StreamReader(s);
				streamReader.ReadToEnd();
				streamReader.Close();
			}
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x002291C0 File Offset: 0x002281C0
		internal void DoNavigate(object bp, NavigationMode navFlags, object navState)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, EventTrace.Event.Wpf_NavigationAsyncWorkItem);
			if (this.IsDisposed)
			{
				return;
			}
			NavigateInfo navigateInfo = navState as NavigateInfo;
			Invariant.Assert(navFlags != NavigationMode.Refresh ^ bp == this._bp, "Navigating to the same object should be handled as fragment navigation, except for Refresh.");
			Uri orgUri = (navigateInfo == null) ? null : navigateInfo.Source;
			Uri resolvedUri = BindUriHelper.GetResolvedUri(null, orgUri);
			((IContentContainer)this).OnContentReady(null, bp, resolvedUri, navState);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00229228 File Offset: 0x00228228
		private JournalEntry UpdateJournal(NavigationMode navigationMode, JournalReason journalReason, JournalEntry destinationJournalEntry)
		{
			JournalEntry journalEntry = null;
			if (!this._doNotJournalCurrentContent)
			{
				journalEntry = this.MakeJournalEntry(journalReason);
			}
			if (journalEntry == null)
			{
				this._doNotJournalCurrentContent = false;
				if ((navigationMode == NavigationMode.Back || navigationMode == NavigationMode.Forward) && this.JournalScope != null)
				{
					this.JournalScope.Journal.CommitJournalNavigation(destinationJournalEntry);
				}
				return null;
			}
			JournalNavigationScope journalNavigationScope = this.EnsureJournal();
			if (journalNavigationScope == null)
			{
				return null;
			}
			PageFunctionBase pageFunctionBase = this._bp as PageFunctionBase;
			if (pageFunctionBase != null && navigationMode == NavigationMode.New && pageFunctionBase.Content == null)
			{
				journalEntry.EntryType = JournalEntryType.UiLess;
			}
			journalNavigationScope.Journal.UpdateCurrentEntry(journalEntry);
			if (navigationMode == NavigationMode.New)
			{
				journalNavigationScope.Journal.RecordNewNavigation();
			}
			else
			{
				journalNavigationScope.Journal.CommitJournalNavigation(destinationJournalEntry);
			}
			this._customContentStateToSave = null;
			return journalEntry;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x002292D4 File Offset: 0x002282D4
		internal JournalEntry MakeJournalEntry(JournalReason journalReason)
		{
			if (this._bp == null)
			{
				return null;
			}
			if (this._journalEntryGroupState == null)
			{
				this._journalEntryGroupState = new JournalEntryGroupState(this._guidId, this._contentId);
			}
			bool flag = this.IsContentKeepAlive();
			PageFunctionBase pageFunctionBase = this._bp as PageFunctionBase;
			JournalEntry journalEntry;
			if (pageFunctionBase != null)
			{
				if (flag)
				{
					journalEntry = new JournalEntryPageFunctionKeepAlive(this._journalEntryGroupState, pageFunctionBase);
				}
				else
				{
					Uri uri = pageFunctionBase.GetValue(BaseUriHelper.BaseUriProperty) as Uri;
					if (uri != null)
					{
						Invariant.Assert(uri.IsAbsoluteUri, "BaseUri for root element should be absolute.");
						Uri markupUri;
						if (this._currentCleanSource != null && !BindUriHelper.StartWithFragment(this._currentCleanSource))
						{
							markupUri = this._currentSource;
						}
						else
						{
							markupUri = uri;
						}
						journalEntry = new JournalEntryPageFunctionUri(this._journalEntryGroupState, pageFunctionBase, markupUri);
					}
					else
					{
						journalEntry = new JournalEntryPageFunctionType(this._journalEntryGroupState, pageFunctionBase);
					}
				}
				journalEntry.Source = this._currentCleanSource;
			}
			else if (flag)
			{
				journalEntry = new JournalEntryKeepAlive(this._journalEntryGroupState, this._currentCleanSource, this._bp);
			}
			else
			{
				journalEntry = new JournalEntryUri(this._journalEntryGroupState, this._currentCleanSource);
			}
			CustomContentState customContentState = this._customContentStateToSave;
			if (customContentState == null)
			{
				IProvideCustomContentState provideCustomContentState = this._bp as IProvideCustomContentState;
				if (provideCustomContentState != null)
				{
					customContentState = provideCustomContentState.GetContentState();
				}
			}
			if (customContentState != null)
			{
				Type type = customContentState.GetType();
				if (!type.IsSerializable)
				{
					throw new SystemException(SR.Get("CustomContentStateMustBeSerializable", new object[]
					{
						type
					}));
				}
				journalEntry.CustomContentState = customContentState;
			}
			if (this._rootViewerStateToSave != null)
			{
				journalEntry.RootViewerState = this._rootViewerStateToSave;
				this._rootViewerStateToSave = null;
			}
			else
			{
				journalEntry.RootViewerState = this.GetRootViewerState(journalReason);
			}
			string text = null;
			if (journalEntry.CustomContentState != null)
			{
				text = journalEntry.CustomContentState.JournalEntryName;
			}
			if (string.IsNullOrEmpty(text))
			{
				DependencyObject dependencyObject = this._bp as DependencyObject;
				if (dependencyObject != null)
				{
					text = (string)dependencyObject.GetValue(JournalEntry.NameProperty);
					if (string.IsNullOrEmpty(text) && dependencyObject is Page)
					{
						text = (dependencyObject as Page).Title;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (this._currentSource != null)
					{
						string fragment = BindUriHelper.GetFragment(this._currentSource);
						if (!string.IsNullOrEmpty(fragment))
						{
							text = text + "#" + fragment;
						}
					}
				}
				else
				{
					NavigationWindow navigationWindow = (this.JournalScope == null) ? null : (this.JournalScope.NavigatorHost as NavigationWindow);
					if (navigationWindow != null && this == navigationWindow.NavigationService && !string.IsNullOrEmpty(navigationWindow.Title))
					{
						if (this.CurrentSource != null)
						{
							text = string.Format(CultureInfo.CurrentCulture, "{0} ({1})", navigationWindow.Title, JournalEntry.GetDisplayName(this._currentSource, SiteOfOriginContainer.SiteOfOrigin));
						}
						else
						{
							text = navigationWindow.Title;
						}
					}
					else if (this.CurrentSource != null)
					{
						text = JournalEntry.GetDisplayName(this._currentSource, SiteOfOriginContainer.SiteOfOrigin);
					}
					else
					{
						text = SR.Get("Untitled");
					}
				}
			}
			journalEntry.Name = text;
			if (journalReason == JournalReason.NewContentNavigation)
			{
				journalEntry.SaveState(this._bp);
			}
			return journalEntry;
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x002295DC File Offset: 0x002285DC
		internal void RequestCustomContentStateOnAppShutdown()
		{
			this._isNavInitiator = false;
			this._isNavInitiatorValid = true;
			this.FireNavigating(null, null, null, null);
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06004719 RID: 18201 RVA: 0x001A0A0C File Offset: 0x0019FA0C
		internal Application Application
		{
			get
			{
				return Application.Current;
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x002295F7 File Offset: 0x002285F7
		// (set) Token: 0x0600471B RID: 18203 RVA: 0x002295FF File Offset: 0x002285FF
		internal bool AllowWindowNavigation
		{
			private get
			{
				return this._allowWindowNavigation;
			}
			set
			{
				this._allowWindowNavigation = value;
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x00229608 File Offset: 0x00228608
		// (set) Token: 0x0600471D RID: 18205 RVA: 0x00229610 File Offset: 0x00228610
		internal long BytesRead
		{
			get
			{
				return this._bytesRead;
			}
			set
			{
				this._bytesRead = value;
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x0600471E RID: 18206 RVA: 0x00229619 File Offset: 0x00228619
		// (set) Token: 0x0600471F RID: 18207 RVA: 0x00229621 File Offset: 0x00228621
		internal long MaxBytes
		{
			get
			{
				return this._maxBytes;
			}
			set
			{
				this._maxBytes = value;
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06004720 RID: 18208 RVA: 0x0022962A File Offset: 0x0022862A
		internal uint ContentId
		{
			get
			{
				return this._contentId;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06004721 RID: 18209 RVA: 0x00229632 File Offset: 0x00228632
		// (set) Token: 0x06004722 RID: 18210 RVA: 0x0022963A File Offset: 0x0022863A
		internal Guid GuidId
		{
			get
			{
				return this._guidId;
			}
			set
			{
				this._guidId = value;
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06004723 RID: 18211 RVA: 0x00229643 File Offset: 0x00228643
		internal NavigationService ParentNavigationService
		{
			get
			{
				return this._parentNavigationService;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06004724 RID: 18212 RVA: 0x0022964B File Offset: 0x0022864B
		internal bool CanReloadFromUri
		{
			get
			{
				return !(this._currentCleanSource == null) && !BindUriHelper.StartWithFragment(this._currentCleanSource) && !BindUriHelper.StartWithFragment(BindUriHelper.GetUriRelativeToPackAppBase(this._currentCleanSource));
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06004725 RID: 18213 RVA: 0x0022967D File Offset: 0x0022867D
		internal ArrayList ChildNavigationServices
		{
			get
			{
				return this._childNavigationServices;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x00229685 File Offset: 0x00228685
		private FinishEventHandler FinishHandler
		{
			get
			{
				if (this._finishHandler == null)
				{
					this._finishHandler = new FinishEventHandler(this.HandleFinish);
				}
				return this._finishHandler;
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06004727 RID: 18215 RVA: 0x002296A7 File Offset: 0x002286A7
		private bool IsTopLevelContainer
		{
			get
			{
				return this.INavigatorHost is NavigationWindow || (this.Application != null && this.Application.CheckAccess() && this.Application.NavService == this);
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06004728 RID: 18216 RVA: 0x002296E0 File Offset: 0x002286E0
		private bool IsJournalLevelContainer
		{
			get
			{
				JournalNavigationScope journalScope = this.JournalScope;
				return journalScope != null && journalScope.RootNavigationService == this;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06004729 RID: 18217 RVA: 0x00229704 File Offset: 0x00228704
		private bool SandboxExternalContent
		{
			get
			{
				DependencyObject dependencyObject = this.INavigatorHost as DependencyObject;
				return dependencyObject != null && (bool)dependencyObject.GetValue(Frame.SandboxExternalContentProperty);
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x0600472A RID: 18218 RVA: 0x00229732 File Offset: 0x00228732
		// (set) Token: 0x0600472B RID: 18219 RVA: 0x0022973C File Offset: 0x0022873C
		internal INavigator INavigatorHost
		{
			get
			{
				return this._navigatorHost;
			}
			set
			{
				RequestNavigateEventHandler handler = new RequestNavigateEventHandler(this.OnRequestNavigate);
				if (this._navigatorHost != null)
				{
					IInputElement inputElement = this._navigatorHost as IInputElement;
					if (inputElement != null)
					{
						inputElement.RemoveHandler(Hyperlink.RequestNavigateEvent, handler);
					}
					IDownloader downloader = this._navigatorHost as IDownloader;
					if (downloader != null)
					{
						downloader.ContentRendered -= this.ContentRenderedHandler;
					}
				}
				if (value != null)
				{
					IInputElement inputElement2 = value as IInputElement;
					if (inputElement2 != null)
					{
						inputElement2.AddHandler(Hyperlink.RequestNavigateEvent, handler);
					}
					IDownloader downloader2 = value as IDownloader;
					if (downloader2 != null)
					{
						downloader2.ContentRendered += this.ContentRenderedHandler;
					}
				}
				this._navigatorHost = value;
				this._navigatorHostImpl = (value as INavigatorImpl);
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x0600472C RID: 18220 RVA: 0x002297E5 File Offset: 0x002287E5
		// (set) Token: 0x0600472D RID: 18221 RVA: 0x002297ED File Offset: 0x002287ED
		internal NavigationStatus NavStatus
		{
			get
			{
				return this._navStatus;
			}
			set
			{
				this._navStatus = value;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x0600472E RID: 18222 RVA: 0x002297F6 File Offset: 0x002287F6
		internal ArrayList PendingNavigationList
		{
			get
			{
				return this._pendingNavigationList;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x0600472F RID: 18223 RVA: 0x002297FE File Offset: 0x002287FE
		internal WebBrowser WebBrowser
		{
			get
			{
				return this._webBrowser;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06004730 RID: 18224 RVA: 0x00229808 File Offset: 0x00228808
		internal bool IsDisposed
		{
			get
			{
				bool flag = false;
				if (this.Application != null && this.Application.CheckAccess() && Application.IsShuttingDown)
				{
					flag = true;
				}
				return this._disposed || flag;
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x00229840 File Offset: 0x00228840
		internal void Dispose()
		{
			this._disposed = true;
			this.StopLoading();
			foreach (object obj in this.ChildNavigationServices)
			{
				((NavigationService)obj).Dispose();
			}
			this._journalScope = null;
			this._bp = null;
			this._currentSource = null;
			this._currentCleanSource = null;
			this._oldRootVisual = null;
			this._childNavigationServices.Clear();
			this._parentNavigationService = null;
			this._webBrowser = null;
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x002298E0 File Offset: 0x002288E0
		private NavigationWindow FindNavigationWindow()
		{
			NavigationService navigationService = this;
			while (navigationService != null && navigationService.INavigatorHost != null)
			{
				NavigationWindow navigationWindow = navigationService.INavigatorHost as NavigationWindow;
				if (navigationWindow != null)
				{
					return navigationWindow;
				}
				navigationService = navigationService.ParentNavigationService;
			}
			return null;
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x00229915 File Offset: 0x00228915
		internal static bool IsPageFunction(object content)
		{
			return content is PageFunctionBase;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00229924 File Offset: 0x00228924
		private void SetupPageFunctionHandlers(object bp)
		{
			PageFunctionBase pageFunctionBase = bp as PageFunctionBase;
			if (bp == null)
			{
				return;
			}
			pageFunctionBase.FinishHandler = this.FinishHandler;
			new ReturnEventSaver()._Detach(pageFunctionBase);
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x00229954 File Offset: 0x00228954
		private void HandlePageFunction(NavigateInfo navInfo)
		{
			PageFunctionBase pageFunctionBase = (PageFunctionBase)this._bp;
			if (this.IsJournalNavigation(navInfo))
			{
				pageFunctionBase._Resume = true;
			}
			if (!pageFunctionBase._Resume)
			{
				pageFunctionBase.CallStart();
			}
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0022998C File Offset: 0x0022898C
		private void HandleFinish(PageFunctionBase endingPF, object ReturnEventArgs)
		{
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.Wpf_NavigationPageFunctionReturn, EventTrace.Keyword.KeywordHosting, EventTrace.Level.Info, endingPF.ToString());
			}
			if (this.JournalScope == null)
			{
				throw new InvalidOperationException(SR.Get("WindowAlreadyClosed"));
			}
			Journal journal = this.JournalScope.Journal;
			PageFunctionBase pageFunctionBase = null;
			int parentPageJournalIndex = JournalEntryPageFunction.GetParentPageJournalIndex(this, journal, endingPF);
			if (endingPF.RemoveFromJournal)
			{
				this.DoRemoveFromJournal(endingPF, parentPageJournalIndex);
			}
			if (parentPageJournalIndex != -1)
			{
				JournalEntryPageFunction journalEntryPageFunction = journal[parentPageJournalIndex] as JournalEntryPageFunction;
				if (journalEntryPageFunction != null)
				{
					pageFunctionBase = journalEntryPageFunction.ResumePageFunction();
					pageFunctionBase.FinishHandler = this.FinishHandler;
					this.FireChildPageFunctionReturnEvent(pageFunctionBase, endingPF, ReturnEventArgs);
				}
			}
			if (this._navigateQueueItem == null)
			{
				if (parentPageJournalIndex != -1 && parentPageJournalIndex < journal.TotalCount && !this.IsDisposed)
				{
					this.NavigateToParentPage(endingPF, pageFunctionBase, ReturnEventArgs, parentPageJournalIndex);
					return;
				}
			}
			else
			{
				if (parentPageJournalIndex < journal.TotalCount)
				{
					((JournalEntryPageFunction)journal[parentPageJournalIndex]).SaveState(pageFunctionBase);
				}
				pageFunctionBase.FinishHandler = null;
			}
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x00229A7C File Offset: 0x00228A7C
		private void FireChildPageFunctionReturnEvent(object parentElem, PageFunctionBase childPF, object ReturnEventArgs)
		{
			ReturnEventSaver saver = childPF._Saver;
			if (saver != null)
			{
				saver._Attach(parentElem, childPF);
				Window window = null;
				DependencyObject dependencyObject = parentElem as DependencyObject;
				if (dependencyObject != null && !dependencyObject.IsSealed)
				{
					dependencyObject.SetValue(NavigationService.NavigationServiceProperty, this);
					DependencyObject dependencyObject2 = this.INavigatorHost as DependencyObject;
					if (dependencyObject2 != null && (window = Window.GetWindow(dependencyObject2)) != null)
					{
						dependencyObject.SetValue(Window.IWindowServiceProperty, window);
					}
				}
				try
				{
					childPF._OnFinish(ReturnEventArgs);
				}
				catch
				{
					this.DoStopLoading(true, false);
					throw;
				}
				finally
				{
					saver._Detach(childPF);
					if (dependencyObject != null && !dependencyObject.IsSealed)
					{
						dependencyObject.ClearValue(NavigationService.NavigationServiceProperty);
						if (window != null)
						{
							dependencyObject.ClearValue(Window.IWindowServiceProperty);
						}
					}
				}
			}
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x00229B40 File Offset: 0x00228B40
		private void DoRemoveFromJournal(PageFunctionBase finishingChildPageFunction, int parentEntryIndex)
		{
			if (!finishingChildPageFunction.RemoveFromJournal)
			{
				return;
			}
			bool flag = false;
			Journal journal = this.JournalScope.Journal;
			int i = parentEntryIndex + 1;
			while (i < journal.TotalCount)
			{
				if (!flag)
				{
					JournalEntryPageFunction journalEntryPageFunction = journal[i] as JournalEntryPageFunction;
					flag = (journalEntryPageFunction != null && journalEntryPageFunction.PageFunctionId == finishingChildPageFunction.PageFunctionId);
				}
				if (flag)
				{
					journal.RemoveEntryInternal(i);
				}
				else
				{
					i++;
				}
			}
			if (flag)
			{
				journal.UpdateView();
			}
			else if (this._bp == finishingChildPageFunction)
			{
				journal.ClearForwardStack();
			}
			this._doNotJournalCurrentContent = true;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x00229BD0 File Offset: 0x00228BD0
		private void NavigateToParentPage(PageFunctionBase finishingChildPageFunction, PageFunctionBase parentPF, object returnEventArgs, int parentIndex)
		{
			JournalEntry journalEntry = this.JournalScope.Journal[parentIndex];
			if (parentPF != null)
			{
				if (journalEntry.EntryType == JournalEntryType.UiLess)
				{
					throw new InvalidOperationException(SR.Get("UiLessPageFunctionNotCallingOnReturn"));
				}
				NavigateInfo navigationState = finishingChildPageFunction.RemoveFromJournal ? new NavigateInfo(journalEntry.Source, NavigationMode.Back, journalEntry) : new NavigateInfo(journalEntry.Source, NavigationMode.New);
				this.Navigate(parentPF, navigationState);
				return;
			}
			else
			{
				PageFunctionReturnInfo navigationState2 = finishingChildPageFunction.RemoveFromJournal ? new PageFunctionReturnInfo(finishingChildPageFunction, journalEntry.Source, NavigationMode.Back, journalEntry, returnEventArgs) : new PageFunctionReturnInfo(finishingChildPageFunction, journalEntry.Source, NavigationMode.New, null, returnEventArgs);
				if (journalEntry is JournalEntryUri)
				{
					this.Navigate(journalEntry.Source, navigationState2);
					return;
				}
				if (journalEntry is JournalEntryKeepAlive)
				{
					object keepAliveRoot = ((JournalEntryKeepAlive)journalEntry).KeepAliveRoot;
					this.Navigate(keepAliveRoot, navigationState2);
				}
				return;
			}
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x00229C98 File Offset: 0x00228C98
		private bool IsValidRootElement(object bp)
		{
			bool result = true;
			if (!this.AllowWindowNavigation && bp != null && bp is Window)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x140000A2 RID: 162
		// (add) Token: 0x0600473B RID: 18235 RVA: 0x00229CC0 File Offset: 0x00228CC0
		// (remove) Token: 0x0600473C RID: 18236 RVA: 0x00229CF8 File Offset: 0x00228CF8
		internal event BPReadyEventHandler BPReady;

		// Token: 0x140000A3 RID: 163
		// (add) Token: 0x0600473D RID: 18237 RVA: 0x00229D30 File Offset: 0x00228D30
		// (remove) Token: 0x0600473E RID: 18238 RVA: 0x00229D68 File Offset: 0x00228D68
		internal event BPReadyEventHandler PreBPReady;

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x0600473F RID: 18239 RVA: 0x00229D9D File Offset: 0x00228D9D
		private JournalNavigationScope JournalScope
		{
			get
			{
				if (this._journalScope == null && this._navigatorHost != null)
				{
					this._journalScope = this._navigatorHost.GetJournal(false);
				}
				return this._journalScope;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06004740 RID: 18240 RVA: 0x00229DC8 File Offset: 0x00228DC8
		private bool IsNavigationInitiator
		{
			get
			{
				if (!this._isNavInitiatorValid)
				{
					this._isNavInitiator = this.IsTopLevelContainer;
					if (this._parentNavigationService != null)
					{
						if (!this._parentNavigationService.PendingNavigationList.Contains(this))
						{
							this._isNavInitiator = true;
						}
					}
					else if (this.IsJournalLevelContainer)
					{
						this._isNavInitiator = true;
					}
					this._isNavInitiatorValid = true;
				}
				return this._isNavInitiator;
			}
		}

		// Token: 0x0400259C RID: 9628
		internal static readonly DependencyProperty NavigationServiceProperty = DependencyProperty.RegisterAttached("NavigationService", typeof(NavigationService), typeof(NavigationService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400259E RID: 9630
		private NavigatingCancelEventHandler _navigating;

		// Token: 0x0400259F RID: 9631
		private NavigatedEventHandler _navigated;

		// Token: 0x040025A0 RID: 9632
		private NavigationProgressEventHandler _navigationProgress;

		// Token: 0x040025A1 RID: 9633
		private LoadCompletedEventHandler _loadCompleted;

		// Token: 0x040025A2 RID: 9634
		private FragmentNavigationEventHandler _fragmentNavigation;

		// Token: 0x040025A3 RID: 9635
		private NavigationStoppedEventHandler _stopped;

		// Token: 0x040025A6 RID: 9638
		private object _bp;

		// Token: 0x040025A7 RID: 9639
		private uint _contentId;

		// Token: 0x040025A8 RID: 9640
		private Uri _currentSource;

		// Token: 0x040025A9 RID: 9641
		private Uri _currentCleanSource;

		// Token: 0x040025AA RID: 9642
		private JournalEntryGroupState _journalEntryGroupState;

		// Token: 0x040025AB RID: 9643
		private bool _doNotJournalCurrentContent;

		// Token: 0x040025AC RID: 9644
		private bool _cancelContentRenderedHandling;

		// Token: 0x040025AD RID: 9645
		private CustomContentState _customContentStateToSave;

		// Token: 0x040025AE RID: 9646
		private CustomJournalStateInternal _rootViewerStateToSave;

		// Token: 0x040025AF RID: 9647
		private WebRequest _request;

		// Token: 0x040025B0 RID: 9648
		private object _navState;

		// Token: 0x040025B1 RID: 9649
		private WebResponse _webResponse;

		// Token: 0x040025B2 RID: 9650
		private XamlReader _asyncObjectConverter;

		// Token: 0x040025B3 RID: 9651
		private bool _isNavInitiator;

		// Token: 0x040025B4 RID: 9652
		private bool _isNavInitiatorValid;

		// Token: 0x040025B5 RID: 9653
		private bool _allowWindowNavigation;

		// Token: 0x040025B6 RID: 9654
		private Guid _guidId = Guid.Empty;

		// Token: 0x040025B7 RID: 9655
		private INavigator _navigatorHost;

		// Token: 0x040025B8 RID: 9656
		private INavigatorImpl _navigatorHostImpl;

		// Token: 0x040025B9 RID: 9657
		private JournalNavigationScope _journalScope;

		// Token: 0x040025BA RID: 9658
		private ArrayList _childNavigationServices = new ArrayList(2);

		// Token: 0x040025BB RID: 9659
		private NavigationService _parentNavigationService;

		// Token: 0x040025BC RID: 9660
		private bool _disposed;

		// Token: 0x040025BD RID: 9661
		private FinishEventHandler _finishHandler;

		// Token: 0x040025BE RID: 9662
		private NavigationStatus _navStatus;

		// Token: 0x040025BF RID: 9663
		private ArrayList _pendingNavigationList = new ArrayList(2);

		// Token: 0x040025C0 RID: 9664
		private ArrayList _recursiveNavigateList = new ArrayList(2);

		// Token: 0x040025C1 RID: 9665
		private NavigateQueueItem _navigateQueueItem;

		// Token: 0x040025C2 RID: 9666
		private long _bytesRead;

		// Token: 0x040025C3 RID: 9667
		private long _maxBytes;

		// Token: 0x040025C4 RID: 9668
		private Visual _oldRootVisual;

		// Token: 0x040025C5 RID: 9669
		private const int _noParentPage = -1;

		// Token: 0x040025C6 RID: 9670
		private WebBrowser _webBrowser;
	}
}
