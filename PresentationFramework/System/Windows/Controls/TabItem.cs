using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007E2 RID: 2018
	[DefaultEvent("IsSelectedChanged")]
	public class TabItem : HeaderedContentControl
	{
		// Token: 0x06007423 RID: 29731 RVA: 0x002E5960 File Offset: 0x002E4960
		static TabItem()
		{
			EventManager.RegisterClassHandler(typeof(TabItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(TabItem.OnAccessKeyPressed));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
			TabItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(TabItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(TabItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(TabItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x06007424 RID: 29732 RVA: 0x002E5AE9 File Offset: 0x002E4AE9
		// (set) Token: 0x06007425 RID: 29733 RVA: 0x002E5AFB File Offset: 0x002E4AFB
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(TabItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(TabItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06007426 RID: 29734 RVA: 0x002E5B10 File Offset: 0x002E4B10
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TabItem tabItem = d as TabItem;
			bool flag = (bool)e.NewValue;
			TabControl tabControlParent = tabItem.TabControlParent;
			if (tabControlParent != null)
			{
				tabControlParent.RaiseIsSelectedChangedAutomationEvent(tabItem, flag);
			}
			if (flag)
			{
				tabItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, tabItem));
			}
			else
			{
				tabItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, tabItem));
			}
			if (flag)
			{
				Binding binding = new Binding("Margin");
				binding.Source = tabItem;
				BindingOperations.SetBinding(tabItem, KeyboardNavigation.DirectionalNavigationMarginProperty, binding);
			}
			else
			{
				BindingOperations.ClearBinding(tabItem, KeyboardNavigation.DirectionalNavigationMarginProperty);
			}
			tabItem.UpdateVisualState();
		}

		// Token: 0x06007427 RID: 29735 RVA: 0x002E5B9F File Offset: 0x002E4B9F
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(true, e);
		}

		// Token: 0x06007428 RID: 29736 RVA: 0x002E5BA9 File Offset: 0x002E4BA9
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(false, e);
		}

		// Token: 0x06007429 RID: 29737 RVA: 0x002D2E48 File Offset: 0x002D1E48
		private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x0600742A RID: 29738 RVA: 0x002E5BB4 File Offset: 0x002E4BB4
		private static object CoerceTabStripPlacement(DependencyObject d, object value)
		{
			TabControl tabControlParent = ((TabItem)d).TabControlParent;
			if (tabControlParent == null)
			{
				return value;
			}
			return tabControlParent.TabStripPlacement;
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x0600742B RID: 29739 RVA: 0x002E5BDD File Offset: 0x002E4BDD
		public Dock TabStripPlacement
		{
			get
			{
				return (Dock)base.GetValue(TabItem.TabStripPlacementProperty);
			}
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x002E5BEF File Offset: 0x002E4BEF
		internal override void OnAncestorChanged()
		{
			base.CoerceValue(TabItem.TabStripPlacementProperty);
		}

		// Token: 0x0600742D RID: 29741 RVA: 0x002E5BFC File Offset: 0x002E4BFC
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (this.IsSelected)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Selected",
					"Unselected"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unselected", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x0600742E RID: 29742 RVA: 0x002E5CA3 File Offset: 0x002E4CA3
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TabItemWrapperAutomationPeer(this);
		}

		// Token: 0x0600742F RID: 29743 RVA: 0x002E5CAB File Offset: 0x002E4CAB
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if ((e.Source == this || !this.IsSelected) && this.SetFocus())
			{
				e.Handled = true;
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06007430 RID: 29744 RVA: 0x002E5CD4 File Offset: 0x002E4CD4
		protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
		{
			base.OnPreviewGotKeyboardFocus(e);
			if (!e.Handled && e.NewFocus == this)
			{
				if (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent)
				{
					if (!this.IsSelected && this.TabControlParent != null)
					{
						base.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
						if (e.OldFocus != Keyboard.FocusedElement)
						{
							e.Handled = true;
							return;
						}
						if (this.GetBoolField(TabItem.BoolField.SetFocusOnContent))
						{
							TabControl tabControlParent = this.TabControlParent;
							if (tabControlParent != null)
							{
								ContentPresenter selectedContentPresenter = tabControlParent.SelectedContentPresenter;
								if (selectedContentPresenter != null)
								{
									tabControlParent.UpdateLayout();
									if (selectedContentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)))
									{
										e.Handled = true;
										return;
									}
								}
							}
						}
					}
				}
				else
				{
					if (!this.IsSelected && this.TabControlParent != null)
					{
						base.SetCurrentValueInternal(TabItem.IsSelectedProperty, BooleanBoxes.TrueBox);
						if (e.OldFocus != Keyboard.FocusedElement)
						{
							e.Handled = true;
						}
					}
					if (!e.Handled && this.GetBoolField(TabItem.BoolField.SetFocusOnContent))
					{
						TabControl tabControlParent2 = this.TabControlParent;
						if (tabControlParent2 != null)
						{
							ContentPresenter selectedContentPresenter2 = tabControlParent2.SelectedContentPresenter;
							if (selectedContentPresenter2 != null)
							{
								tabControlParent2.UpdateLayout();
								if (selectedContentPresenter2.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)))
								{
									e.Handled = true;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x002E5DF9 File Offset: 0x002E4DF9
		protected override void OnAccessKey(AccessKeyEventArgs e)
		{
			this.SetFocus();
		}

		// Token: 0x06007432 RID: 29746 RVA: 0x002E5E04 File Offset: 0x002E4E04
		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					if (newContent == BindingExpressionBase.DisconnectedItem)
					{
						newContent = null;
					}
					tabControlParent.SelectedContent = newContent;
				}
			}
		}

		// Token: 0x06007433 RID: 29747 RVA: 0x002E5E40 File Offset: 0x002E4E40
		protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
		{
			base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					tabControlParent.SelectedContentTemplate = newContentTemplate;
				}
			}
		}

		// Token: 0x06007434 RID: 29748 RVA: 0x002E5E70 File Offset: 0x002E4E70
		protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
		{
			base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
			if (this.IsSelected)
			{
				TabControl tabControlParent = this.TabControlParent;
				if (tabControlParent != null)
				{
					tabControlParent.SelectedContentTemplateSelector = newContentTemplateSelector;
				}
			}
		}

		// Token: 0x06007435 RID: 29749 RVA: 0x002E5EA0 File Offset: 0x002E4EA0
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!e.Handled && e.Scope == null)
			{
				TabItem tabItem = sender as TabItem;
				if (e.Target == null)
				{
					e.Target = tabItem;
					return;
				}
				if (!tabItem.IsSelected)
				{
					e.Scope = tabItem;
					e.Handled = true;
				}
			}
		}

		// Token: 0x06007436 RID: 29750 RVA: 0x002E5EEC File Offset: 0x002E4EEC
		internal bool SetFocus()
		{
			bool result = false;
			if (!this.GetBoolField(TabItem.BoolField.SettingFocus))
			{
				TabItem tabItem = Keyboard.FocusedElement as TabItem;
				bool flag = (FrameworkAppContextSwitches.SelectionPropertiesCanLagBehindSelectionChangedEvent || !base.IsKeyboardFocusWithin) && (tabItem == this || tabItem == null || tabItem.TabControlParent != this.TabControlParent);
				this.SetBoolField(TabItem.BoolField.SettingFocus, true);
				this.SetBoolField(TabItem.BoolField.SetFocusOnContent, flag);
				try
				{
					result = (base.Focus() || flag);
				}
				finally
				{
					this.SetBoolField(TabItem.BoolField.SettingFocus, false);
					this.SetBoolField(TabItem.BoolField.SetFocusOnContent, false);
				}
			}
			return result;
		}

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x06007437 RID: 29751 RVA: 0x002E5F80 File Offset: 0x002E4F80
		private TabControl TabControlParent
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as TabControl;
			}
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002E5F8D File Offset: 0x002E4F8D
		private bool GetBoolField(TabItem.BoolField field)
		{
			return (this._tabItemBoolFieldStore & field) > TabItem.BoolField.DefaultValue;
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x002E5F9A File Offset: 0x002E4F9A
		private void SetBoolField(TabItem.BoolField field, bool value)
		{
			if (value)
			{
				this._tabItemBoolFieldStore |= field;
				return;
			}
			this._tabItemBoolFieldStore &= ~field;
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x0600743A RID: 29754 RVA: 0x002E5FBD File Offset: 0x002E4FBD
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return TabItem._dType;
			}
		}

		// Token: 0x040037FE RID: 14334
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(TabItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(TabItem.OnIsSelectedChanged)));

		// Token: 0x040037FF RID: 14335
		private static readonly DependencyPropertyKey TabStripPlacementPropertyKey = DependencyProperty.RegisterReadOnly("TabStripPlacement", typeof(Dock), typeof(TabItem), new FrameworkPropertyMetadata(Dock.Top, null, new CoerceValueCallback(TabItem.CoerceTabStripPlacement)));

		// Token: 0x04003800 RID: 14336
		public static readonly DependencyProperty TabStripPlacementProperty = TabItem.TabStripPlacementPropertyKey.DependencyProperty;

		// Token: 0x04003801 RID: 14337
		private TabItem.BoolField _tabItemBoolFieldStore;

		// Token: 0x04003802 RID: 14338
		private static DependencyObjectType _dType;

		// Token: 0x02000C22 RID: 3106
		[Flags]
		private enum BoolField
		{
			// Token: 0x04004B34 RID: 19252
			SetFocusOnContent = 16,
			// Token: 0x04004B35 RID: 19253
			SettingFocus = 32,
			// Token: 0x04004B36 RID: 19254
			DefaultValue = 0
		}
	}
}
