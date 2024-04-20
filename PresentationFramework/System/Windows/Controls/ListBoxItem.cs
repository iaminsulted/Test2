using System;
using System.ComponentModel;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x020007A8 RID: 1960
	[DefaultEvent("Selected")]
	public class ListBoxItem : ContentControl
	{
		// Token: 0x06006EB5 RID: 28341 RVA: 0x002D2C38 File Offset: 0x002D1C38
		static ListBoxItem()
		{
			ListBoxItem.SelectedEvent = Selector.SelectedEvent.AddOwner(typeof(ListBoxItem));
			ListBoxItem.UnselectedEvent = Selector.UnselectedEvent.AddOwner(typeof(ListBoxItem));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(typeof(ListBoxItem)));
			ListBoxItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ListBoxItem));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(ListBoxItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			Selector.IsSelectionActivePropertyKey.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(ListBoxItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x1700198C RID: 6540
		// (get) Token: 0x06006EB6 RID: 28342 RVA: 0x002D2DAE File Offset: 0x002D1DAE
		// (set) Token: 0x06006EB7 RID: 28343 RVA: 0x002D2DC0 File Offset: 0x002D1DC0
		[Bindable(true)]
		[Category("Appearance")]
		public bool IsSelected
		{
			get
			{
				return (bool)base.GetValue(ListBoxItem.IsSelectedProperty);
			}
			set
			{
				base.SetValue(ListBoxItem.IsSelectedProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06006EB8 RID: 28344 RVA: 0x002D2DD4 File Offset: 0x002D1DD4
		private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListBoxItem listBoxItem = d as ListBoxItem;
			bool flag = (bool)e.NewValue;
			Selector parentSelector = listBoxItem.ParentSelector;
			if (parentSelector != null)
			{
				parentSelector.RaiseIsSelectedChangedAutomationEvent(listBoxItem, flag);
			}
			if (flag)
			{
				listBoxItem.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, listBoxItem));
			}
			else
			{
				listBoxItem.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, listBoxItem));
			}
			listBoxItem.UpdateVisualState();
		}

		// Token: 0x06006EB9 RID: 28345 RVA: 0x002D2E34 File Offset: 0x002D1E34
		protected virtual void OnSelected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(true, e);
		}

		// Token: 0x06006EBA RID: 28346 RVA: 0x002D2E3E File Offset: 0x002D1E3E
		protected virtual void OnUnselected(RoutedEventArgs e)
		{
			this.HandleIsSelectedChanged(false, e);
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x002D2E48 File Offset: 0x002D1E48
		private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x14000133 RID: 307
		// (add) Token: 0x06006EBC RID: 28348 RVA: 0x002D2E51 File Offset: 0x002D1E51
		// (remove) Token: 0x06006EBD RID: 28349 RVA: 0x002D2E5F File Offset: 0x002D1E5F
		public event RoutedEventHandler Selected
		{
			add
			{
				base.AddHandler(ListBoxItem.SelectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.SelectedEvent, value);
			}
		}

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x06006EBE RID: 28350 RVA: 0x002D2E6D File Offset: 0x002D1E6D
		// (remove) Token: 0x06006EBF RID: 28351 RVA: 0x002D2E7B File Offset: 0x002D1E7B
		public event RoutedEventHandler Unselected
		{
			add
			{
				base.AddHandler(ListBoxItem.UnselectedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ListBoxItem.UnselectedEvent, value);
			}
		}

		// Token: 0x06006EC0 RID: 28352 RVA: 0x002D2E8C File Offset: 0x002D1E8C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, (base.Content is Control) ? "Normal" : "Disabled", useTransitions);
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
				if (Selector.GetIsSelectionActive(this))
				{
					VisualStateManager.GoToState(this, "Selected", useTransitions);
				}
				else
				{
					VisualStates.GoToState(this, useTransitions, new string[]
					{
						"SelectedUnfocused",
						"Selected"
					});
				}
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

		// Token: 0x06006EC1 RID: 28353 RVA: 0x002D2F5E File Offset: 0x002D1F5E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ListBoxItemWrapperAutomationPeer(this);
		}

		// Token: 0x06006EC2 RID: 28354 RVA: 0x002D2F66 File Offset: 0x002D1F66
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Left);
			}
			base.OnMouseLeftButtonDown(e);
		}

		// Token: 0x06006EC3 RID: 28355 RVA: 0x002D2F85 File Offset: 0x002D1F85
		protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
		{
			if (!e.Handled)
			{
				e.Handled = true;
				this.HandleMouseButtonDown(MouseButton.Right);
			}
			base.OnMouseRightButtonDown(e);
		}

		// Token: 0x06006EC4 RID: 28356 RVA: 0x002D2FA4 File Offset: 0x002D1FA4
		private void HandleMouseButtonDown(MouseButton mouseButton)
		{
			if (Selector.UiGetIsSelectable(this) && base.Focus())
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null)
				{
					parentListBox.NotifyListItemClicked(this, mouseButton);
				}
			}
		}

		// Token: 0x06006EC5 RID: 28357 RVA: 0x002D2FD4 File Offset: 0x002D1FD4
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			if (base.IsMouseOver)
			{
				ListBox parentListBox = this.ParentListBox;
				if (parentListBox != null && Mouse.LeftButton == MouseButtonState.Pressed)
				{
					parentListBox.NotifyListItemMouseDragged(this);
				}
			}
			base.OnMouseEnter(e);
		}

		// Token: 0x06006EC6 RID: 28358 RVA: 0x002D3024 File Offset: 0x002D2024
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			if (this.parentNotifyDraggedOperation != null)
			{
				this.parentNotifyDraggedOperation.Abort();
				this.parentNotifyDraggedOperation = null;
			}
			base.OnMouseLeave(e);
		}

		// Token: 0x06006EC7 RID: 28359 RVA: 0x002D3048 File Offset: 0x002D2048
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			ItemsControl itemsControl = null;
			if (VisualTreeHelper.GetParent(this) == null && base.IsKeyboardFocusWithin)
			{
				itemsControl = ItemsControl.GetItemsOwner(oldParent);
			}
			base.OnVisualParentChanged(oldParent);
			if (itemsControl != null)
			{
				itemsControl.Focus();
			}
		}

		// Token: 0x1700198D RID: 6541
		// (get) Token: 0x06006EC8 RID: 28360 RVA: 0x002D307F File Offset: 0x002D207F
		private ListBox ParentListBox
		{
			get
			{
				return this.ParentSelector as ListBox;
			}
		}

		// Token: 0x1700198E RID: 6542
		// (get) Token: 0x06006EC9 RID: 28361 RVA: 0x002D308C File Offset: 0x002D208C
		internal Selector ParentSelector
		{
			get
			{
				return ItemsControl.ItemsControlFromItemContainer(this) as Selector;
			}
		}

		// Token: 0x1700198F RID: 6543
		// (get) Token: 0x06006ECA RID: 28362 RVA: 0x002D3099 File Offset: 0x002D2099
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ListBoxItem._dType;
			}
		}

		// Token: 0x04003667 RID: 13927
		public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(ListBoxItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ListBoxItem.OnIsSelectedChanged)));

		// Token: 0x0400366A RID: 13930
		private DispatcherOperation parentNotifyDraggedOperation;

		// Token: 0x0400366B RID: 13931
		private static DependencyObjectType _dType;
	}
}
