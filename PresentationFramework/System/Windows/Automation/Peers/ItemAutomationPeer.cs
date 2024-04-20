using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000572 RID: 1394
	public abstract class ItemAutomationPeer : AutomationPeer, IVirtualizedItemProvider
	{
		// Token: 0x06004481 RID: 17537 RVA: 0x002214A7 File Offset: 0x002204A7
		protected ItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
		{
			this.Item = item;
			this._itemsControlAutomationPeer = itemsControlAutomationPeer;
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x0021DD4D File Offset: 0x0021CD4D
		// (set) Token: 0x06004483 RID: 17539 RVA: 0x002214C0 File Offset: 0x002204C0
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer wrapperPeer = this.GetWrapperPeer();
				if (wrapperPeer != null)
				{
					wrapperPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x002214EC File Offset: 0x002204EC
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.VirtualizedItem)
			{
				if (VirtualizedItemPatternIdentifiers.Pattern != null)
				{
					if (this.GetWrapperPeer() == null)
					{
						return this;
					}
					if (this.ItemsControlAutomationPeer != null && !this.IsItemInAutomationTree())
					{
						return this;
					}
					if (this.ItemsControlAutomationPeer == null)
					{
						return this;
					}
				}
				return null;
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				UIElementAutomationPeer uielementAutomationPeer = this.GetWrapperPeer() as UIElementAutomationPeer;
				if (uielementAutomationPeer != null)
				{
					return uielementAutomationPeer.GetPattern(patternInterface);
				}
			}
			return null;
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0022154C File Offset: 0x0022054C
		internal UIElement GetWrapper()
		{
			UIElement result = null;
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				ItemsControl itemsControl = (ItemsControl)itemsControlAutomationPeer.Owner;
				if (itemsControl != null)
				{
					object rawItem = this.RawItem;
					if (rawItem != DependencyProperty.UnsetValue)
					{
						if (((IGeneratorHost)itemsControl).IsItemItsOwnContainer(rawItem))
						{
							result = (rawItem as UIElement);
						}
						else
						{
							result = (itemsControl.ItemContainerGenerator.ContainerFromItem(rawItem) as UIElement);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x002215A8 File Offset: 0x002205A8
		internal virtual AutomationPeer GetWrapperPeer()
		{
			AutomationPeer automationPeer = null;
			UIElement wrapper = this.GetWrapper();
			if (wrapper != null)
			{
				automationPeer = UIElementAutomationPeer.CreatePeerForElement(wrapper);
				if (automationPeer == null)
				{
					if (wrapper is FrameworkElement)
					{
						automationPeer = new FrameworkElementAutomationPeer((FrameworkElement)wrapper);
					}
					else
					{
						automationPeer = new UIElementAutomationPeer(wrapper);
					}
				}
			}
			return automationPeer;
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x002215E9 File Offset: 0x002205E9
		internal void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null && !(this is GridViewItemAutomationPeer) && !this.IsItemInAutomationTree())
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0021DBC0 File Offset: 0x0021CBC0
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x00221614 File Offset: 0x00220614
		internal override void AddToParentProxyWeakRefCache()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				itemsControlAutomationPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x00221638 File Offset: 0x00220638
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetVisibleBoundingRectCore();
			}
			return base.GetBoundingRectangle();
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x000FD820 File Offset: 0x000FC820
		protected override string GetItemTypeCore()
		{
			return string.Empty;
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x0022165C File Offset: 0x0022065C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.ForceEnsureChildren();
				return wrapperPeer.GetChildren();
			}
			return null;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x00221684 File Offset: 0x00220684
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x002216B4 File Offset: 0x002206B4
		protected override bool IsOffscreenCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x002216DC File Offset: 0x002206DC
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x00221704 File Offset: 0x00220704
		protected override int GetPositionInSetCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				int num = wrapperPeer.GetPositionInSet();
				if (num == -1)
				{
					num = ItemAutomationPeer.GetPositionInSetFromItemsControl((ItemsControl)this.ItemsControlAutomationPeer.Owner, this.Item);
				}
				return num;
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0022174C File Offset: 0x0022074C
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				int num = wrapperPeer.GetSizeOfSet();
				if (num == -1)
				{
					num = ItemAutomationPeer.GetSizeOfSetFromItemsControl((ItemsControl)this.ItemsControlAutomationPeer.Owner, this.Item);
				}
				return num;
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x00221794 File Offset: 0x00220794
		internal static int GetPositionInSetFromItemsControl(ItemsControl itemsControl, object item)
		{
			ItemCollection items = itemsControl.Items;
			int num = items.IndexOf(item);
			if (itemsControl.IsGrouping)
			{
				int num2;
				num = ItemAutomationPeer.FindPositionInGroup(items.Groups, num, out num2);
			}
			return num + 1;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x002217CC File Offset: 0x002207CC
		internal static int GetSizeOfSetFromItemsControl(ItemsControl itemsControl, object item)
		{
			int result = -1;
			ItemCollection items = itemsControl.Items;
			if (itemsControl.IsGrouping)
			{
				int position = items.IndexOf(item);
				ItemAutomationPeer.FindPositionInGroup(items.Groups, position, out result);
			}
			else
			{
				result = items.Count;
			}
			return result;
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0022180C File Offset: 0x0022080C
		private static int FindPositionInGroup(ReadOnlyObservableCollection<object> collection, int position, out int sizeOfGroup)
		{
			ReadOnlyObservableCollection<object> readOnlyObservableCollection = null;
			sizeOfGroup = -1;
			do
			{
				readOnlyObservableCollection = null;
				foreach (object obj in collection)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = (CollectionViewGroupInternal)obj;
					if (position < collectionViewGroupInternal.ItemCount)
					{
						CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal;
						if (collectionViewGroupInternal2.IsBottomLevel)
						{
							readOnlyObservableCollection = null;
							sizeOfGroup = collectionViewGroupInternal.ItemCount;
							break;
						}
						readOnlyObservableCollection = collectionViewGroupInternal2.Items;
						break;
					}
					else
					{
						position -= collectionViewGroupInternal.ItemCount;
					}
				}
			}
			while ((collection = readOnlyObservableCollection) != null);
			return position;
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x00221894 File Offset: 0x00220894
		protected override string GetItemStatusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x002218C0 File Offset: 0x002208C0
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x002218E8 File Offset: 0x002208E8
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x00221910 File Offset: 0x00220910
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x00221938 File Offset: 0x00220938
		protected override bool IsEnabledCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x00221960 File Offset: 0x00220960
		protected override bool IsPasswordCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00221988 File Offset: 0x00220988
		protected override string GetAutomationIdCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			string result = null;
			object item;
			if (wrapperPeer != null)
			{
				result = wrapperPeer.GetAutomationId();
			}
			else if ((item = this.Item) != null)
			{
				using (RecyclableWrapper recyclableWrapperPeer = this.ItemsControlAutomationPeer.GetRecyclableWrapperPeer(item))
				{
					result = recyclableWrapperPeer.Peer.GetAutomationId();
				}
			}
			return result;
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x002219EC File Offset: 0x002209EC
		protected override string GetNameCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			string text = null;
			object item = this.Item;
			if (wrapperPeer != null)
			{
				text = wrapperPeer.GetName();
			}
			else if (item != null)
			{
				using (RecyclableWrapper recyclableWrapperPeer = this.ItemsControlAutomationPeer.GetRecyclableWrapperPeer(item))
				{
					text = recyclableWrapperPeer.Peer.GetName();
				}
			}
			if (string.IsNullOrEmpty(text) && item != null)
			{
				FrameworkElement frameworkElement = item as FrameworkElement;
				if (frameworkElement != null)
				{
					text = frameworkElement.GetPlainText();
				}
				if (string.IsNullOrEmpty(text))
				{
					text = item.ToString();
				}
			}
			return text;
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x00221A7C File Offset: 0x00220A7C
		protected override bool IsContentElementCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			return wrapperPeer == null || wrapperPeer.IsContentElement();
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x00221A9C File Offset: 0x00220A9C
		protected override bool IsControlElementCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			return wrapperPeer == null || wrapperPeer.IsControlElement();
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x00221ABC File Offset: 0x00220ABC
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x00221AE4 File Offset: 0x00220AE4
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLiveSetting();
			}
			this.ThrowElementNotAvailableException();
			return AutomationLiveSetting.Off;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x00221B0C File Offset: 0x00220B0C
		protected override string GetHelpTextCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetHelpText();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x00221B38 File Offset: 0x00220B38
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x00221B64 File Offset: 0x00220B64
		protected override string GetAccessKeyCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x00221B90 File Offset: 0x00220B90
		protected override Point GetClickablePointCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x00221BCC File Offset: 0x00220BCC
		protected override void SetFocusCore()
		{
			AutomationPeer wrapperPeer = this.GetWrapperPeer();
			if (wrapperPeer != null)
			{
				wrapperPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x00221BF0 File Offset: 0x00220BF0
		internal virtual ItemsControlAutomationPeer GetItemsControlAutomationPeer()
		{
			return this._itemsControlAutomationPeer;
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x00221BF8 File Offset: 0x00220BF8
		// (set) Token: 0x060044A9 RID: 17577 RVA: 0x00221C21 File Offset: 0x00220C21
		public object Item
		{
			get
			{
				ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
				if (itemWeakReference == null)
				{
					return this._item;
				}
				return itemWeakReference.Target;
			}
			private set
			{
				if (value != null && !value.GetType().IsValueType && !FrameworkAppContextSwitches.ItemAutomationPeerKeepsItsItemAlive)
				{
					this._item = new ItemAutomationPeer.ItemWeakReference(value);
					return;
				}
				this._item = value;
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x060044AA RID: 17578 RVA: 0x00221C50 File Offset: 0x00220C50
		private object RawItem
		{
			get
			{
				ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
				if (itemWeakReference == null)
				{
					return this._item;
				}
				object target = itemWeakReference.Target;
				if (target != null)
				{
					return target;
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x00221C84 File Offset: 0x00220C84
		internal void ReuseForItem(object item)
		{
			ItemAutomationPeer.ItemWeakReference itemWeakReference = this._item as ItemAutomationPeer.ItemWeakReference;
			if (itemWeakReference != null)
			{
				if (item != itemWeakReference.Target)
				{
					itemWeakReference.Target = item;
					return;
				}
			}
			else
			{
				this._item = item;
			}
		}

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x060044AC RID: 17580 RVA: 0x00221CB8 File Offset: 0x00220CB8
		// (set) Token: 0x060044AD RID: 17581 RVA: 0x00221CC0 File Offset: 0x00220CC0
		public ItemsControlAutomationPeer ItemsControlAutomationPeer
		{
			get
			{
				return this.GetItemsControlAutomationPeer();
			}
			internal set
			{
				this._itemsControlAutomationPeer = value;
			}
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00221CC9 File Offset: 0x00220CC9
		void IVirtualizedItemProvider.Realize()
		{
			this.RealizeCore();
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x00221CD4 File Offset: 0x00220CD4
		internal virtual void RealizeCore()
		{
			ItemsControlAutomationPeer itemsControlAutomationPeer = this.ItemsControlAutomationPeer;
			if (itemsControlAutomationPeer != null)
			{
				ItemsControl parent = itemsControlAutomationPeer.Owner as ItemsControl;
				if (parent != null)
				{
					if (parent.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
					{
						if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parent))
						{
							itemsControlAutomationPeer.RecentlyRealizedPeers.Add(this);
						}
						parent.OnBringItemIntoView(this.Item);
						return;
					}
					base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(delegate(object arg)
					{
						if (AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parent))
						{
							itemsControlAutomationPeer.RecentlyRealizedPeers.Add(this);
						}
						parent.OnBringItemIntoView(arg);
						return null;
					}), this.Item);
				}
			}
		}

		// Token: 0x04002530 RID: 9520
		private object _item;

		// Token: 0x04002531 RID: 9521
		private ItemsControlAutomationPeer _itemsControlAutomationPeer;

		// Token: 0x02000B1F RID: 2847
		private class ItemWeakReference : WeakReference
		{
			// Token: 0x06008C5D RID: 35933 RVA: 0x0032A9B4 File Offset: 0x003299B4
			public ItemWeakReference(object o) : base(o)
			{
			}
		}
	}
}
