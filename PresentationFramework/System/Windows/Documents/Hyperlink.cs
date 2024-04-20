using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Commands;
using MS.Internal.PresentationFramework;

namespace System.Windows.Documents
{
	// Token: 0x02000626 RID: 1574
	[TextElementEditingBehavior(IsMergeable = false, IsTypographicOnly = false)]
	public class Hyperlink : Span, ICommandSource, IUriContext
	{
		// Token: 0x06004D8A RID: 19850 RVA: 0x0024054C File Offset: 0x0023F54C
		static Hyperlink()
		{
			Hyperlink.RequestNavigateEvent = EventManager.RegisterRoutedEvent("RequestNavigate", RoutingStrategy.Bubble, typeof(RequestNavigateEventHandler), typeof(Hyperlink));
			Hyperlink.ClickEvent = ButtonBase.ClickEvent.AddOwner(typeof(Hyperlink));
			Hyperlink.RequestSetStatusBarEvent = EventManager.RegisterRoutedEvent("RequestSetStatusBar", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Hyperlink));
			Hyperlink.IsHyperlinkPressedProperty = DependencyProperty.Register("IsHyperlinkPressed", typeof(bool), typeof(Hyperlink), new FrameworkPropertyMetadata(false));
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Hyperlink), new FrameworkPropertyMetadata(typeof(Hyperlink)));
			Hyperlink._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Hyperlink));
			ContentElement.FocusableProperty.OverrideMetadata(typeof(Hyperlink), new FrameworkPropertyMetadata(true));
			EventManager.RegisterClassHandler(typeof(Hyperlink), Mouse.QueryCursorEvent, new QueryCursorEventHandler(Hyperlink.OnQueryCursor));
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x00240753 File Offset: 0x0023F753
		public Hyperlink()
		{
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x00240762 File Offset: 0x0023F762
		public Hyperlink(Inline childInline) : base(childInline)
		{
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x00240772 File Offset: 0x0023F772
		public Hyperlink(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x00240784 File Offset: 0x0023F784
		public Hyperlink(TextPointer start, TextPointer end) : base(start, end)
		{
			TextPointer textPointer = base.ContentStart.CreatePointer();
			TextPointer contentEnd = base.ContentEnd;
			while (textPointer.CompareTo(contentEnd) < 0)
			{
				Hyperlink hyperlink = textPointer.GetAdjacentElement(LogicalDirection.Forward) as Hyperlink;
				if (hyperlink != null)
				{
					hyperlink.Reposition(null, null);
				}
				else
				{
					textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x002407E1 File Offset: 0x0023F7E1
		public void DoClick()
		{
			Hyperlink.DoNonUserInitiatedNavigation(this);
		}

		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x06004D90 RID: 19856 RVA: 0x002407E9 File Offset: 0x0023F7E9
		// (set) Token: 0x06004D91 RID: 19857 RVA: 0x002407FB File Offset: 0x0023F7FB
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public ICommand Command
		{
			get
			{
				return (ICommand)base.GetValue(Hyperlink.CommandProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandProperty, value);
			}
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x00240809 File Offset: 0x0023F809
		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Hyperlink)d).OnCommandChanged((ICommand)e.OldValue, (ICommand)e.NewValue);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x0024082E File Offset: 0x0023F82E
		private void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				this.UnhookCommand(oldCommand);
			}
			if (newCommand != null)
			{
				this.HookCommand(newCommand);
			}
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x00240844 File Offset: 0x0023F844
		private void UnhookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.RemoveHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x0024085E File Offset: 0x0023F85E
		private void HookCommand(ICommand command)
		{
			CanExecuteChangedEventManager.AddHandler(command, new EventHandler<EventArgs>(this.OnCanExecuteChanged));
			this.UpdateCanExecute();
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x00240878 File Offset: 0x0023F878
		private void OnCanExecuteChanged(object sender, EventArgs e)
		{
			this.UpdateCanExecute();
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x00240880 File Offset: 0x0023F880
		private void UpdateCanExecute()
		{
			if (this.Command != null)
			{
				this.CanExecute = CommandHelpers.CanExecuteCommandSource(this);
				return;
			}
			this.CanExecute = true;
		}

		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x0024089E File Offset: 0x0023F89E
		// (set) Token: 0x06004D99 RID: 19865 RVA: 0x002408A6 File Offset: 0x0023F8A6
		private bool CanExecute
		{
			get
			{
				return this._canExecute;
			}
			set
			{
				if (this._canExecute != value)
				{
					this._canExecute = value;
					base.CoerceValue(ContentElement.IsEnabledProperty);
				}
			}
		}

		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x06004D9A RID: 19866 RVA: 0x002408C3 File Offset: 0x0023F8C3
		private bool IsEditable
		{
			get
			{
				return base.TextContainer.TextSelection != null && !base.TextContainer.TextSelection.TextEditor.IsReadOnly;
			}
		}

		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06004D9B RID: 19867 RVA: 0x002408EC File Offset: 0x0023F8EC
		protected override bool IsEnabledCore
		{
			get
			{
				return base.IsEnabledCore && this.CanExecute;
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06004D9C RID: 19868 RVA: 0x002408FE File Offset: 0x0023F8FE
		// (set) Token: 0x06004D9D RID: 19869 RVA: 0x0024090B File Offset: 0x0023F90B
		[Bindable(true)]
		[Category("Action")]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object CommandParameter
		{
			get
			{
				return base.GetValue(Hyperlink.CommandParameterProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandParameterProperty, value);
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06004D9E RID: 19870 RVA: 0x00240919 File Offset: 0x0023F919
		// (set) Token: 0x06004D9F RID: 19871 RVA: 0x0024092B File Offset: 0x0023F92B
		[Category("Action")]
		[Bindable(true)]
		public IInputElement CommandTarget
		{
			get
			{
				return (IInputElement)base.GetValue(Hyperlink.CommandTargetProperty);
			}
			set
			{
				base.SetValue(Hyperlink.CommandTargetProperty, value);
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x0024093C File Offset: 0x0023F93C
		internal static object CoerceNavigateUri(DependencyObject d, object value)
		{
			int? value2 = Hyperlink.s_criticalNavigateUriProtectee.Value;
			int hashCode = d.GetHashCode();
			if ((value2.GetValueOrDefault() == hashCode & value2 != null) && Hyperlink.ShouldPreventUriSpoofing)
			{
				value = DependencyProperty.UnsetValue;
			}
			return value;
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06004DA1 RID: 19873 RVA: 0x0024097E File Offset: 0x0023F97E
		// (set) Token: 0x06004DA2 RID: 19874 RVA: 0x00240990 File Offset: 0x0023F990
		[Bindable(true)]
		[CustomCategory("Navigation")]
		[Localizability(LocalizationCategory.Hyperlink)]
		public Uri NavigateUri
		{
			get
			{
				return (Uri)base.GetValue(Hyperlink.NavigateUriProperty);
			}
			set
			{
				base.SetValue(Hyperlink.NavigateUriProperty, value);
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x0024099E File Offset: 0x0023F99E
		// (set) Token: 0x06004DA4 RID: 19876 RVA: 0x002409B0 File Offset: 0x0023F9B0
		[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable)]
		[CustomCategory("Navigation")]
		[Bindable(true)]
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(Hyperlink.TargetNameProperty);
			}
			set
			{
				base.SetValue(Hyperlink.TargetNameProperty, value);
			}
		}

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06004DA5 RID: 19877 RVA: 0x002409BE File Offset: 0x0023F9BE
		// (remove) Token: 0x06004DA6 RID: 19878 RVA: 0x002409CC File Offset: 0x0023F9CC
		public event RequestNavigateEventHandler RequestNavigate
		{
			add
			{
				base.AddHandler(Hyperlink.RequestNavigateEvent, value);
			}
			remove
			{
				base.RemoveHandler(Hyperlink.RequestNavigateEvent, value);
			}
		}

		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06004DA7 RID: 19879 RVA: 0x002409DA File Offset: 0x0023F9DA
		// (remove) Token: 0x06004DA8 RID: 19880 RVA: 0x002409E8 File Offset: 0x0023F9E8
		[Category("Behavior")]
		public event RoutedEventHandler Click
		{
			add
			{
				base.AddHandler(Hyperlink.ClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Hyperlink.ClickEvent, value);
			}
		}

		// Token: 0x06004DA9 RID: 19881 RVA: 0x002409F6 File Offset: 0x0023F9F6
		protected internal override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (base.IsEnabled && (!this.IsEditable || (Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None))
			{
				Hyperlink.OnMouseLeftButtonDown(this, e);
			}
		}

		// Token: 0x06004DAA RID: 19882 RVA: 0x00240A1F File Offset: 0x0023FA1F
		protected internal override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			Hyperlink.OnMouseLeftButtonUp(this, e);
		}

		// Token: 0x06004DAB RID: 19883 RVA: 0x00240A2F File Offset: 0x0023FA2F
		private static void CacheNavigateUri(DependencyObject d, Uri targetUri)
		{
			d.VerifyAccess();
			Hyperlink.s_cachedNavigateUri.Value = targetUri;
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x00240A44 File Offset: 0x0023FA44
		private static void NavigateToUri(IInputElement sourceElement, Uri targetUri, string targetWindow)
		{
			DependencyObject dependencyObject = (DependencyObject)sourceElement;
			dependencyObject.VerifyAccess();
			Uri value = Hyperlink.s_cachedNavigateUri.Value;
			if (value == null || value.Equals(targetUri) || !Hyperlink.ShouldPreventUriSpoofing)
			{
				if (!(sourceElement is Hyperlink))
				{
					targetUri = FixedPage.GetLinkUri(sourceElement, targetUri);
				}
				RequestNavigateEventArgs requestNavigateEventArgs = new RequestNavigateEventArgs(targetUri, targetWindow);
				requestNavigateEventArgs.Source = sourceElement;
				sourceElement.RaiseEvent(requestNavigateEventArgs);
				if (requestNavigateEventArgs.Handled)
				{
					dependencyObject.Dispatcher.BeginInvoke(DispatcherPriority.Send, new SendOrPostCallback(Hyperlink.ClearStatusBarAndCachedUri), sourceElement);
				}
			}
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x00240ACC File Offset: 0x0023FACC
		private static void UpdateStatusBar(object sender)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			Uri targetUri = (Uri)dependencyObject.GetValue(Hyperlink.GetNavigateUriProperty(inputElement));
			Hyperlink.s_criticalNavigateUriProtectee.Value = new int?(dependencyObject.GetHashCode());
			Hyperlink.CacheNavigateUri(dependencyObject, targetUri);
			RequestSetStatusBarEventArgs e = new RequestSetStatusBarEventArgs(targetUri);
			inputElement.RaiseEvent(e);
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x00240B23 File Offset: 0x0023FB23
		private static DependencyProperty GetNavigateUriProperty(object element)
		{
			if (element is Hyperlink)
			{
				return Hyperlink.NavigateUriProperty;
			}
			return FixedPage.NavigateUriProperty;
		}

		// Token: 0x06004DAF RID: 19887 RVA: 0x00240B38 File Offset: 0x0023FB38
		private static void ClearStatusBarAndCachedUri(object sender)
		{
			((IInputElement)sender).RaiseEvent(RequestSetStatusBarEventArgs.Clear);
			Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
			Hyperlink.s_criticalNavigateUriProtectee.Value = null;
		}

		// Token: 0x06004DB0 RID: 19888 RVA: 0x00240B74 File Offset: 0x0023FB74
		protected virtual void OnClick()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = ContentElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			Hyperlink.DoNavigation(this);
			base.RaiseEvent(new RoutedEventArgs(Hyperlink.ClickEvent, this));
			CommandHelpers.ExecuteCommandSource(this);
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x00240BB7 File Offset: 0x0023FBB7
		protected internal override void OnKeyDown(KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Return)
			{
				Hyperlink.OnKeyDown(this, e);
				return;
			}
			base.OnKeyDown(e);
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06004DB2 RID: 19890 RVA: 0x001A5A01 File Offset: 0x001A4A01
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x00240BD9 File Offset: 0x0023FBD9
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new HyperlinkAutomationPeer(this);
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x00240BE1 File Offset: 0x0023FBE1
		// (set) Token: 0x06004DB5 RID: 19893 RVA: 0x00240BE9 File Offset: 0x0023FBE9
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

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06004DB6 RID: 19894 RVA: 0x0022A4E3 File Offset: 0x002294E3
		// (set) Token: 0x06004DB7 RID: 19895 RVA: 0x0022A4F5 File Offset: 0x002294F5
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

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06004DB8 RID: 19896 RVA: 0x00240BF2 File Offset: 0x0023FBF2
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal string Text
		{
			get
			{
				return TextRangeBase.GetTextInternal(base.ContentStart, base.ContentEnd);
			}
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x00240C08 File Offset: 0x0023FC08
		private static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			Hyperlink hyperlink = (Hyperlink)sender;
			if (hyperlink.IsEnabled && hyperlink.IsEditable && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
			{
				e.Cursor = hyperlink.TextContainer.TextSelection.TextEditor._cursor;
				e.Handled = true;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x00240C58 File Offset: 0x0023FC58
		private static bool ShouldPreventUriSpoofing
		{
			get
			{
				if (Hyperlink.s_shouldPreventUriSpoofing.Value == null)
				{
					Hyperlink.s_shouldPreventUriSpoofing.Value = new bool?(false);
				}
				return Hyperlink.s_shouldPreventUriSpoofing.Value.Value;
			}
		}

		// Token: 0x06004DBB RID: 19899 RVA: 0x00240C9C File Offset: 0x0023FC9C
		internal static void OnNavigateUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			IInputElement inputElement = d as IInputElement;
			if (inputElement != null && (Uri)e.NewValue != null)
			{
				FrameworkElement frameworkElement = d as FrameworkElement;
				if (frameworkElement != null && (frameworkElement is Path || frameworkElement is Canvas || frameworkElement is Glyphs || frameworkElement is FixedPage))
				{
					Hyperlink.SetUpNavigationEventHandlers(inputElement);
					frameworkElement.Cursor = Cursors.Hand;
					return;
				}
				FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
				if (frameworkContentElement != null && frameworkContentElement is Hyperlink)
				{
					Hyperlink.SetUpNavigationEventHandlers(inputElement);
				}
			}
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x00240D1C File Offset: 0x0023FD1C
		private static void SetUpNavigationEventHandlers(IInputElement element)
		{
			if (!(element is Hyperlink))
			{
				Hyperlink.SetUpEventHandler(element, UIElement.KeyDownEvent, new KeyEventHandler(Hyperlink.OnKeyDown));
				Hyperlink.SetUpEventHandler(element, UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Hyperlink.OnMouseLeftButtonDown));
				Hyperlink.SetUpEventHandler(element, UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(Hyperlink.OnMouseLeftButtonUp));
			}
			Hyperlink.SetUpEventHandler(element, UIElement.MouseEnterEvent, new MouseEventHandler(Hyperlink.OnMouseEnter));
			Hyperlink.SetUpEventHandler(element, UIElement.MouseLeaveEvent, new MouseEventHandler(Hyperlink.OnMouseLeave));
		}

		// Token: 0x06004DBD RID: 19901 RVA: 0x00240DA4 File Offset: 0x0023FDA4
		private static void SetUpEventHandler(IInputElement element, RoutedEvent routedEvent, Delegate handler)
		{
			element.RemoveHandler(routedEvent, handler);
			element.AddHandler(routedEvent, handler);
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x00240DB6 File Offset: 0x0023FDB6
		private static void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Handled && e.Key == Key.Return)
			{
				Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
				if (e.UserInitiated)
				{
					Hyperlink.DoUserInitiatedNavigation(sender);
				}
				else
				{
					Hyperlink.DoNonUserInitiatedNavigation(sender);
				}
				e.Handled = true;
			}
		}

		// Token: 0x06004DBF RID: 19903 RVA: 0x00240DF4 File Offset: 0x0023FDF4
		private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			inputElement.Focus();
			if (e.ButtonState == MouseButtonState.Pressed)
			{
				Mouse.Capture(inputElement);
				if (inputElement.IsMouseCaptured)
				{
					if (e.ButtonState == MouseButtonState.Pressed)
					{
						dependencyObject.SetValue(Hyperlink.IsHyperlinkPressedProperty, true);
					}
					else
					{
						inputElement.ReleaseMouseCapture();
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x06004DC0 RID: 19904 RVA: 0x00240E54 File Offset: 0x0023FE54
		private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			IInputElement inputElement = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			if (inputElement.IsMouseCaptured)
			{
				inputElement.ReleaseMouseCapture();
			}
			if ((bool)dependencyObject.GetValue(Hyperlink.IsHyperlinkPressedProperty))
			{
				dependencyObject.SetValue(Hyperlink.IsHyperlinkPressedProperty, false);
				if (inputElement.IsMouseOver)
				{
					if (e.UserInitiated)
					{
						Hyperlink.DoUserInitiatedNavigation(sender);
					}
					else
					{
						Hyperlink.DoNonUserInitiatedNavigation(sender);
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x00240EC0 File Offset: 0x0023FEC0
		private static void OnMouseEnter(object sender, MouseEventArgs e)
		{
			Hyperlink.UpdateStatusBar(sender);
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x00240EC8 File Offset: 0x0023FEC8
		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (!((IInputElement)sender).IsMouseOver)
			{
				Hyperlink.ClearStatusBarAndCachedUri(sender);
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x00240EDD File Offset: 0x0023FEDD
		private static void DoUserInitiatedNavigation(object sender)
		{
			Hyperlink.DispatchNavigation(sender);
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x00240EE5 File Offset: 0x0023FEE5
		private static void DoNonUserInitiatedNavigation(object sender)
		{
			Hyperlink.CacheNavigateUri((DependencyObject)sender, null);
			Hyperlink.DispatchNavigation(sender);
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x00240EFC File Offset: 0x0023FEFC
		private static void DispatchNavigation(object sender)
		{
			Hyperlink hyperlink = sender as Hyperlink;
			if (hyperlink != null)
			{
				hyperlink.OnClick();
				return;
			}
			Hyperlink.DoNavigation(sender);
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x00240F20 File Offset: 0x0023FF20
		private static void DoNavigation(object sender)
		{
			IInputElement element = (IInputElement)sender;
			DependencyObject dependencyObject = (DependencyObject)sender;
			Uri targetUri = (Uri)dependencyObject.GetValue(Hyperlink.GetNavigateUriProperty(element));
			string targetWindow = (string)dependencyObject.GetValue(Hyperlink.TargetNameProperty);
			Hyperlink.RaiseNavigate(element, targetUri, targetWindow);
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x00240F64 File Offset: 0x0023FF64
		internal static void RaiseNavigate(IInputElement element, Uri targetUri, string targetWindow)
		{
			if (targetUri != null)
			{
				Hyperlink.NavigateToUri(element, targetUri, targetWindow);
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x00240F77 File Offset: 0x0023FF77
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Hyperlink._dType;
			}
		}

		// Token: 0x04002812 RID: 10258
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(Hyperlink), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnCommandChanged)));

		// Token: 0x04002813 RID: 10259
		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(Hyperlink), new FrameworkPropertyMetadata(null));

		// Token: 0x04002814 RID: 10260
		public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(Hyperlink), new FrameworkPropertyMetadata(null));

		// Token: 0x04002815 RID: 10261
		[CommonDependencyProperty]
		public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register("NavigateUri", typeof(Uri), typeof(Hyperlink), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Hyperlink.OnNavigateUriChanged), new CoerceValueCallback(Hyperlink.CoerceNavigateUri)));

		// Token: 0x04002816 RID: 10262
		public static readonly DependencyProperty TargetNameProperty = DependencyProperty.Register("TargetName", typeof(string), typeof(Hyperlink), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04002819 RID: 10265
		internal static readonly RoutedEvent RequestSetStatusBarEvent;

		// Token: 0x0400281A RID: 10266
		[ThreadStatic]
		private static SecurityCriticalDataForSet<Uri> s_cachedNavigateUri;

		// Token: 0x0400281B RID: 10267
		[ThreadStatic]
		private static SecurityCriticalDataForSet<int?> s_criticalNavigateUriProtectee;

		// Token: 0x0400281C RID: 10268
		private static SecurityCriticalDataForSet<bool?> s_shouldPreventUriSpoofing;

		// Token: 0x0400281D RID: 10269
		private bool _canExecute = true;

		// Token: 0x0400281E RID: 10270
		private static readonly DependencyProperty IsHyperlinkPressedProperty;

		// Token: 0x0400281F RID: 10271
		private static DependencyObjectType _dType;
	}
}
