using System;
using System.Collections.Specialized;
using System.Windows.Automation.Peers;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	// Token: 0x020007A9 RID: 1961
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListViewItem))]
	public class ListView : ListBox
	{
		// Token: 0x06006ECB RID: 28363 RVA: 0x002D30A0 File Offset: 0x002D20A0
		static ListView()
		{
			ListBox.SelectionModeProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(SelectionMode.Extended));
			ControlsTraceLogger.AddControl(TelemetryControls.ListView);
		}

		// Token: 0x17001990 RID: 6544
		// (get) Token: 0x06006ECC RID: 28364 RVA: 0x002D310B File Offset: 0x002D210B
		// (set) Token: 0x06006ECD RID: 28365 RVA: 0x002D311D File Offset: 0x002D211D
		public ViewBase View
		{
			get
			{
				return (ViewBase)base.GetValue(ListView.ViewProperty);
			}
			set
			{
				base.SetValue(ListView.ViewProperty, value);
			}
		}

		// Token: 0x06006ECE RID: 28366 RVA: 0x002D312C File Offset: 0x002D212C
		private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListView listView = (ListView)d;
			ViewBase viewBase = (ViewBase)e.OldValue;
			ViewBase viewBase2 = (ViewBase)e.NewValue;
			if (viewBase2 != null)
			{
				if (viewBase2.IsUsed)
				{
					throw new InvalidOperationException(SR.Get("ListView_ViewCannotBeShared"));
				}
				viewBase2.IsUsed = true;
			}
			listView._previousView = viewBase;
			listView.ApplyNewView();
			listView._previousView = viewBase2;
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(listView) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null)
			{
				if (listViewAutomationPeer.ViewAutomationPeer != null)
				{
					listViewAutomationPeer.ViewAutomationPeer.ViewDetached();
				}
				if (viewBase2 != null)
				{
					listViewAutomationPeer.ViewAutomationPeer = viewBase2.GetAutomationPeer(listView);
				}
				else
				{
					listViewAutomationPeer.ViewAutomationPeer = null;
				}
				listViewAutomationPeer.InvalidatePeer();
			}
			if (viewBase != null)
			{
				viewBase.IsUsed = false;
			}
		}

		// Token: 0x06006ECF RID: 28367 RVA: 0x002D31DC File Offset: 0x002D21DC
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			ListViewItem listViewItem = element as ListViewItem;
			if (listViewItem != null)
			{
				ViewBase view = this.View;
				if (view != null)
				{
					listViewItem.SetDefaultStyleKey(view.ItemContainerDefaultStyleKey);
					view.PrepareItem(listViewItem);
					return;
				}
				listViewItem.ClearDefaultStyleKey();
			}
		}

		// Token: 0x06006ED0 RID: 28368 RVA: 0x002D321F File Offset: 0x002D221F
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
		}

		// Token: 0x06006ED1 RID: 28369 RVA: 0x002D3229 File Offset: 0x002D2229
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListViewItem;
		}

		// Token: 0x06006ED2 RID: 28370 RVA: 0x002D3234 File Offset: 0x002D2234
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListViewItem();
		}

		// Token: 0x06006ED3 RID: 28371 RVA: 0x002D323C File Offset: 0x002D223C
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null && listViewAutomationPeer.ViewAutomationPeer != null)
			{
				listViewAutomationPeer.ViewAutomationPeer.ItemsChanged(e);
			}
		}

		// Token: 0x06006ED4 RID: 28372 RVA: 0x002D3274 File Offset: 0x002D2274
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			ListViewAutomationPeer listViewAutomationPeer = new ListViewAutomationPeer(this);
			if (listViewAutomationPeer != null && this.View != null)
			{
				listViewAutomationPeer.ViewAutomationPeer = this.View.GetAutomationPeer(this);
			}
			return listViewAutomationPeer;
		}

		// Token: 0x06006ED5 RID: 28373 RVA: 0x002D32A8 File Offset: 0x002D22A8
		private void ApplyNewView()
		{
			ViewBase view = this.View;
			if (view != null)
			{
				base.DefaultStyleKey = view.DefaultStyleKey;
			}
			else
			{
				base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
			}
			if (base.IsLoaded)
			{
				base.ItemContainerGenerator.Refresh();
			}
		}

		// Token: 0x06006ED6 RID: 28374 RVA: 0x002D32EB File Offset: 0x002D22EB
		internal override void OnThemeChanged()
		{
			if (!base.HasTemplateGeneratedSubTree && this.View != null)
			{
				this.View.OnThemeChanged();
			}
		}

		// Token: 0x0400366C RID: 13932
		public static readonly DependencyProperty ViewProperty = DependencyProperty.Register("View", typeof(ViewBase), typeof(ListView), new PropertyMetadata(new PropertyChangedCallback(ListView.OnViewChanged)));

		// Token: 0x0400366D RID: 13933
		private ViewBase _previousView;
	}
}
