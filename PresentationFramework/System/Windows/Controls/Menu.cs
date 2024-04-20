using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007AD RID: 1965
	public class Menu : MenuBase
	{
		// Token: 0x06006F25 RID: 28453 RVA: 0x002D3C78 File Offset: 0x002D2C78
		static Menu()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(typeof(Menu)));
			Menu._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Menu));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(Menu.GetDefaultPanel()));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(false));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			EventManager.RegisterClassHandler(typeof(Menu), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(Menu.OnAccessKeyPressed));
			ControlsTraceLogger.AddControl(TelemetryControls.Menu);
		}

		// Token: 0x06006F26 RID: 28454 RVA: 0x002D3D9B File Offset: 0x002D2D9B
		private static ItemsPanelTemplate GetDefaultPanel()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(WrapPanel)));
			itemsPanelTemplate.Seal();
			return itemsPanelTemplate;
		}

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x06006F27 RID: 28455 RVA: 0x002D3DB7 File Offset: 0x002D2DB7
		// (set) Token: 0x06006F28 RID: 28456 RVA: 0x002D3DC9 File Offset: 0x002D2DC9
		public bool IsMainMenu
		{
			get
			{
				return (bool)base.GetValue(Menu.IsMainMenuProperty);
			}
			set
			{
				base.SetValue(Menu.IsMainMenuProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006F29 RID: 28457 RVA: 0x002D3DDC File Offset: 0x002D2DDC
		private static void OnIsMainMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Menu menu = d as Menu;
			if ((bool)e.NewValue)
			{
				menu.SetupMainMenu();
				return;
			}
			menu.CleanupMainMenu();
		}

		// Token: 0x06006F2A RID: 28458 RVA: 0x002D3E0B File Offset: 0x002D2E0B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MenuAutomationPeer(this);
		}

		// Token: 0x06006F2B RID: 28459 RVA: 0x002D3E13 File Offset: 0x002D2E13
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (this.IsMainMenu)
			{
				this.SetupMainMenu();
			}
		}

		// Token: 0x06006F2C RID: 28460 RVA: 0x002D3E2A File Offset: 0x002D2E2A
		private void SetupMainMenu()
		{
			if (this._enterMenuModeHandler == null)
			{
				this._enterMenuModeHandler = new KeyboardNavigation.EnterMenuModeEventHandler(this.OnEnterMenuMode);
				KeyboardNavigation.Current.EnterMenuMode += this._enterMenuModeHandler;
			}
		}

		// Token: 0x06006F2D RID: 28461 RVA: 0x002D3E56 File Offset: 0x002D2E56
		private void CleanupMainMenu()
		{
			if (this._enterMenuModeHandler != null)
			{
				KeyboardNavigation.Current.EnterMenuMode -= this._enterMenuModeHandler;
			}
		}

		// Token: 0x06006F2E RID: 28462 RVA: 0x002D3E70 File Offset: 0x002D2E70
		private static object OnGetIsMainMenu(DependencyObject d)
		{
			return BooleanBoxes.Box(((Menu)d).IsMainMenu);
		}

		// Token: 0x06006F2F RID: 28463 RVA: 0x0029D8BE File Offset: 0x0029C8BE
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		// Token: 0x06006F30 RID: 28464 RVA: 0x002D3E84 File Offset: 0x002D2E84
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			switch (e.Key)
			{
			case Key.Left:
			case Key.Right:
				if (base.CurrentSelection != null)
				{
					Panel itemsHost = base.ItemsHost;
					if (itemsHost != null && itemsHost.HasLogicalOrientation && itemsHost.LogicalOrientation == Orientation.Vertical)
					{
						base.CurrentSelection.OpenSubmenuWithKeyboard();
						e.Handled = true;
					}
				}
				break;
			case Key.Up:
			case Key.Down:
				if (base.CurrentSelection != null)
				{
					Panel itemsHost2 = base.ItemsHost;
					if (itemsHost2 == null || !itemsHost2.HasLogicalOrientation || itemsHost2.LogicalOrientation != Orientation.Vertical)
					{
						base.CurrentSelection.OpenSubmenuWithKeyboard();
						e.Handled = true;
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06006F31 RID: 28465 RVA: 0x002D3F38 File Offset: 0x002D2F38
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			if (e.UserInitiated && e.Text == " " && this.IsMainMenu && (base.CurrentSelection == null || !base.CurrentSelection.IsSubmenuOpen))
			{
				base.IsMenuMode = false;
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(this) as HwndSource;
				if (hwndSource != null)
				{
					hwndSource.ShowSystemMenu();
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006F32 RID: 28466 RVA: 0x002D3FB0 File Offset: 0x002D2FB0
		protected override void HandleMouseButton(MouseButtonEventArgs e)
		{
			base.HandleMouseButton(e);
			if (e.Handled)
			{
				return;
			}
			if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right)
			{
				return;
			}
			if (base.IsMenuMode)
			{
				FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
				if (frameworkElement != null && (frameworkElement == this || frameworkElement.TemplatedParent == this))
				{
					base.IsMenuMode = false;
					e.Handled = true;
				}
			}
		}

		// Token: 0x06006F33 RID: 28467 RVA: 0x002D4014 File Offset: 0x002D3014
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = base.FocusItem(info, itemNavigateArgs);
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				MenuItem menuItem = info.Container as MenuItem;
				if (menuItem != null && menuItem.Role == MenuItemRole.TopLevelHeader && menuItem.IsSubmenuOpen)
				{
					menuItem.NavigateToStart(itemNavigateArgs);
				}
			}
			return result;
		}

		// Token: 0x06006F34 RID: 28468 RVA: 0x002D405D File Offset: 0x002D305D
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
			{
				e.Scope = sender;
				e.Handled = true;
			}
		}

		// Token: 0x06006F35 RID: 28469 RVA: 0x002D4080 File Offset: 0x002D3080
		private bool OnEnterMenuMode(object sender, EventArgs e)
		{
			if (Mouse.Captured != null)
			{
				return false;
			}
			PresentationSource presentationSource = sender as PresentationSource;
			PresentationSource presentationSource2 = PresentationSource.CriticalFromVisual(this);
			if (presentationSource == presentationSource2)
			{
				for (int i = 0; i < base.Items.Count; i++)
				{
					MenuItem menuItem = base.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem != null && !(base.Items[i] is Separator) && menuItem.Focus())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x001FD464 File Offset: 0x001FC464
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x002D40F1 File Offset: 0x002D30F1
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Menu._dType;
			}
		}

		// Token: 0x04003684 RID: 13956
		public static readonly DependencyProperty IsMainMenuProperty = DependencyProperty.Register("IsMainMenu", typeof(bool), typeof(Menu), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Menu.OnIsMainMenuChanged)));

		// Token: 0x04003685 RID: 13957
		private KeyboardNavigation.EnterMenuModeEventHandler _enterMenuModeHandler;

		// Token: 0x04003686 RID: 13958
		private static DependencyObjectType _dType;
	}
}
