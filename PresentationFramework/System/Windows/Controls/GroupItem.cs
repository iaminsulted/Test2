using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x0200078E RID: 1934
	public class GroupItem : ContentControl, IHierarchicalVirtualizationAndScrollInfo, IContainItemStorage
	{
		// Token: 0x06006B4D RID: 27469 RVA: 0x002C5A54 File Offset: 0x002C4A54
		static GroupItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(typeof(GroupItem)));
			GroupItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GroupItem));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(false));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		// Token: 0x06006B4E RID: 27470 RVA: 0x002C5B12 File Offset: 0x002C4B12
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GroupItemAutomationPeer(this);
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x002C5B1C File Offset: 0x002C4B1C
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._header = (base.GetTemplateChild("PART_Header") as FrameworkElement);
			this._expander = Helper.FindTemplatedDescendant<Expander>(this, this);
			if (this._expander != null)
			{
				ItemsControl parentItemsControl = this.ParentItemsControl;
				if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl))
				{
					Helper.SetItemValuesOnContainer(parentItemsControl, this._expander, parentItemsControl.ItemContainerGenerator.ItemFromContainer(this));
				}
				this._expander.Expanded += GroupItem.OnExpanded;
			}
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x002C5B9C File Offset: 0x002C4B9C
		private static void OnExpanded(object sender, RoutedEventArgs e)
		{
			GroupItem groupItem = sender as GroupItem;
			if (groupItem != null && groupItem._expander != null && groupItem._expander.IsExpanded)
			{
				ItemsControl parentItemsControl = groupItem.ParentItemsControl;
				if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizing(parentItemsControl) && VirtualizingPanel.GetVirtualizationMode(parentItemsControl) == VirtualizationMode.Recycling)
				{
					ItemsPresenter itemsHostPresenter = groupItem.ItemsHostPresenter;
					if (itemsHostPresenter != null)
					{
						groupItem.InvalidateMeasure();
						Helper.InvalidateMeasureOnPath(itemsHostPresenter, groupItem, false);
					}
				}
			}
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x002C5BFB File Offset: 0x002C4BFB
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
			if (this._expander != null)
			{
				this._expander.Expanded -= GroupItem.OnExpanded;
				this._expander = null;
			}
			this._itemsHost = null;
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x002C5C32 File Offset: 0x002C4C32
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			arrangeSize = base.ArrangeOverride(arrangeSize);
			Helper.ComputeCorrectionFactor(this.ParentItemsControl, this, this.ItemsHost, this.HeaderElement);
			return arrangeSize;
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x002C5C58 File Offset: 0x002C4C58
		internal override string GetPlainText()
		{
			CollectionViewGroup collectionViewGroup = base.Content as CollectionViewGroup;
			if (collectionViewGroup != null && collectionViewGroup.Name != null)
			{
				return collectionViewGroup.Name.ToString();
			}
			return base.GetPlainText();
		}

		// Token: 0x170018C4 RID: 6340
		// (get) Token: 0x06006B54 RID: 27476 RVA: 0x002C5C8E File Offset: 0x002C4C8E
		// (set) Token: 0x06006B55 RID: 27477 RVA: 0x002C5C96 File Offset: 0x002C4C96
		internal ItemContainerGenerator Generator
		{
			get
			{
				return this._generator;
			}
			set
			{
				this._generator = value;
			}
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x002C5CA0 File Offset: 0x002C4CA0
		internal void PrepareItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (this.Generator == null)
			{
				return;
			}
			if (this._itemsHost != null)
			{
				this._itemsHost.IsItemsHost = true;
			}
			bool flag = parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl);
			if (this.Generator != null)
			{
				if (!flag)
				{
					this.Generator.Release();
				}
				else
				{
					this.Generator.RemoveAllInternal(true);
				}
			}
			GroupStyle groupStyle = this.Generator.Parent.GroupStyle;
			Style style = groupStyle.ContainerStyle;
			if (style == null && groupStyle.ContainerStyleSelector != null)
			{
				style = groupStyle.ContainerStyleSelector.SelectStyle(item, this);
			}
			if (style != null)
			{
				if (!style.TargetType.IsInstanceOfType(this))
				{
					throw new InvalidOperationException(SR.Get("StyleForWrongType", new object[]
					{
						style.TargetType.Name,
						base.GetType().Name
					}));
				}
				base.Style = style;
				base.WriteInternalFlag2(InternalFlags2.IsStyleSetFromGenerator, true);
			}
			if (base.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
			{
				base.Content = item;
				base.ContentIsItem = true;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentTemplateProperty))
			{
				base.ContentTemplate = groupStyle.HeaderTemplate;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentTemplateSelectorProperty))
			{
				base.ContentTemplateSelector = groupStyle.HeaderTemplateSelector;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentStringFormatProperty))
			{
				base.ContentStringFormat = groupStyle.HeaderStringFormat;
			}
			Helper.ClearVirtualizingElement(this);
			if (flag)
			{
				Helper.SetItemValuesOnContainer(parentItemsControl, this, item);
				if (this._expander != null)
				{
					Helper.SetItemValuesOnContainer(parentItemsControl, this._expander, item);
				}
			}
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x002C5E18 File Offset: 0x002C4E18
		internal void ClearItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (this.Generator == null)
			{
				return;
			}
			if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl))
			{
				Helper.StoreItemValues(parentItemsControl, this, item);
				if (this._expander != null)
				{
					Helper.StoreItemValues(parentItemsControl, this._expander, item);
				}
				VirtualizingPanel virtualizingPanel = this._itemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.OnClearChildrenInternal();
				}
				this.Generator.RemoveAllInternal(true);
			}
			else
			{
				this.Generator.Release();
			}
			base.ClearContentControl(item);
		}

		// Token: 0x170018C5 RID: 6341
		// (get) Token: 0x06006B58 RID: 27480 RVA: 0x002C5E8B File Offset: 0x002C4E8B
		// (set) Token: 0x06006B59 RID: 27481 RVA: 0x002C5E98 File Offset: 0x002C4E98
		HierarchicalVirtualizationConstraints IHierarchicalVirtualizationAndScrollInfo.Constraints
		{
			get
			{
				return GroupItem.HierarchicalVirtualizationConstraintsField.GetValue(this);
			}
			set
			{
				if (value.CacheLengthUnit == VirtualizationCacheLengthUnit.Page)
				{
					throw new InvalidOperationException(SR.Get("PageCacheSizeNotAllowed"));
				}
				GroupItem.HierarchicalVirtualizationConstraintsField.SetValue(this, value);
			}
		}

		// Token: 0x170018C6 RID: 6342
		// (get) Token: 0x06006B5A RID: 27482 RVA: 0x002C5EC0 File Offset: 0x002C4EC0
		HierarchicalVirtualizationHeaderDesiredSizes IHierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes
		{
			get
			{
				FrameworkElement headerElement = this.HeaderElement;
				Size pixelSize = default(Size);
				if (base.IsVisible && headerElement != null)
				{
					pixelSize = headerElement.DesiredSize;
					Helper.ApplyCorrectionFactorToPixelHeaderSize(this.ParentItemsControl, this, this._itemsHost, ref pixelSize);
				}
				return new HierarchicalVirtualizationHeaderDesiredSizes(new Size((double)(DoubleUtil.GreaterThan(pixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(pixelSize.Height, 0.0) ? 1 : 0)), pixelSize);
			}
		}

		// Token: 0x170018C7 RID: 6343
		// (get) Token: 0x06006B5B RID: 27483 RVA: 0x002C5F45 File Offset: 0x002C4F45
		// (set) Token: 0x06006B5C RID: 27484 RVA: 0x002C5F53 File Offset: 0x002C4F53
		HierarchicalVirtualizationItemDesiredSizes IHierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes
		{
			get
			{
				return Helper.ApplyCorrectionFactorToItemDesiredSizes(this, this._itemsHost);
			}
			set
			{
				GroupItem.HierarchicalVirtualizationItemDesiredSizesField.SetValue(this, value);
			}
		}

		// Token: 0x170018C8 RID: 6344
		// (get) Token: 0x06006B5D RID: 27485 RVA: 0x002C5F61 File Offset: 0x002C4F61
		Panel IHierarchicalVirtualizationAndScrollInfo.ItemsHost
		{
			get
			{
				return this._itemsHost;
			}
		}

		// Token: 0x170018C9 RID: 6345
		// (get) Token: 0x06006B5E RID: 27486 RVA: 0x002C5F69 File Offset: 0x002C4F69
		// (set) Token: 0x06006B5F RID: 27487 RVA: 0x002C5F76 File Offset: 0x002C4F76
		bool IHierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization
		{
			get
			{
				return GroupItem.MustDisableVirtualizationField.GetValue(this);
			}
			set
			{
				GroupItem.MustDisableVirtualizationField.SetValue(this, value);
			}
		}

		// Token: 0x170018CA RID: 6346
		// (get) Token: 0x06006B60 RID: 27488 RVA: 0x002C5F84 File Offset: 0x002C4F84
		// (set) Token: 0x06006B61 RID: 27489 RVA: 0x002C5F91 File Offset: 0x002C4F91
		bool IHierarchicalVirtualizationAndScrollInfo.InBackgroundLayout
		{
			get
			{
				return GroupItem.InBackgroundLayoutField.GetValue(this);
			}
			set
			{
				GroupItem.InBackgroundLayoutField.SetValue(this, value);
			}
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x002C5F9F File Offset: 0x002C4F9F
		object IContainItemStorage.ReadItemValue(object item, DependencyProperty dp)
		{
			return Helper.ReadItemValue(this, item, dp.GlobalIndex);
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x002C5FAE File Offset: 0x002C4FAE
		void IContainItemStorage.StoreItemValue(object item, DependencyProperty dp, object value)
		{
			Helper.StoreItemValue(this, item, dp.GlobalIndex, value);
		}

		// Token: 0x06006B64 RID: 27492 RVA: 0x002C5FBE File Offset: 0x002C4FBE
		void IContainItemStorage.ClearItemValue(object item, DependencyProperty dp)
		{
			Helper.ClearItemValue(this, item, dp.GlobalIndex);
		}

		// Token: 0x06006B65 RID: 27493 RVA: 0x002C5FCD File Offset: 0x002C4FCD
		void IContainItemStorage.ClearValue(DependencyProperty dp)
		{
			Helper.ClearItemValueStorage(this, new int[]
			{
				dp.GlobalIndex
			});
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x002C5FE4 File Offset: 0x002C4FE4
		void IContainItemStorage.Clear()
		{
			Helper.ClearItemValueStorage(this);
		}

		// Token: 0x170018CB RID: 6347
		// (get) Token: 0x06006B67 RID: 27495 RVA: 0x002C5FEC File Offset: 0x002C4FEC
		private ItemsControl ParentItemsControl
		{
			get
			{
				DependencyObject dependencyObject = this;
				ItemsControl itemsControl;
				for (;;)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					itemsControl = (dependencyObject as ItemsControl);
					if (itemsControl != null)
					{
						break;
					}
					if (dependencyObject == null)
					{
						goto Block_2;
					}
				}
				return itemsControl;
				Block_2:
				return null;
			}
		}

		// Token: 0x170018CC RID: 6348
		// (get) Token: 0x06006B68 RID: 27496 RVA: 0x002C6014 File Offset: 0x002C5014
		internal IContainItemStorage ParentItemStorageProvider
		{
			get
			{
				DependencyObject parent = VisualTreeHelper.GetParent(this);
				if (parent != null)
				{
					return ItemsControl.GetItemsOwnerInternal(parent) as IContainItemStorage;
				}
				return null;
			}
		}

		// Token: 0x170018CD RID: 6349
		// (get) Token: 0x06006B69 RID: 27497 RVA: 0x002C5F61 File Offset: 0x002C4F61
		// (set) Token: 0x06006B6A RID: 27498 RVA: 0x002C6038 File Offset: 0x002C5038
		internal Panel ItemsHost
		{
			get
			{
				return this._itemsHost;
			}
			set
			{
				this._itemsHost = value;
			}
		}

		// Token: 0x170018CE RID: 6350
		// (get) Token: 0x06006B6B RID: 27499 RVA: 0x002C6041 File Offset: 0x002C5041
		private ItemsPresenter ItemsHostPresenter
		{
			get
			{
				if (this._expander != null)
				{
					return Helper.FindTemplatedDescendant<ItemsPresenter>(this._expander, this._expander);
				}
				return Helper.FindTemplatedDescendant<ItemsPresenter>(this, this);
			}
		}

		// Token: 0x170018CF RID: 6351
		// (get) Token: 0x06006B6C RID: 27500 RVA: 0x002C6064 File Offset: 0x002C5064
		internal Expander Expander
		{
			get
			{
				return this._expander;
			}
		}

		// Token: 0x170018D0 RID: 6352
		// (get) Token: 0x06006B6D RID: 27501 RVA: 0x002C606C File Offset: 0x002C506C
		private FrameworkElement ExpanderHeader
		{
			get
			{
				if (this._expander != null)
				{
					return this._expander.GetTemplateChild("HeaderSite") as FrameworkElement;
				}
				return null;
			}
		}

		// Token: 0x170018D1 RID: 6353
		// (get) Token: 0x06006B6E RID: 27502 RVA: 0x002C6090 File Offset: 0x002C5090
		private FrameworkElement HeaderElement
		{
			get
			{
				FrameworkElement result = null;
				if (this._header != null)
				{
					result = this._header;
				}
				else if (this._expander != null)
				{
					result = this.ExpanderHeader;
				}
				return result;
			}
		}

		// Token: 0x170018D2 RID: 6354
		// (get) Token: 0x06006B6F RID: 27503 RVA: 0x002C60C0 File Offset: 0x002C50C0
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GroupItem._dType;
			}
		}

		// Token: 0x0400359E RID: 13726
		private ItemContainerGenerator _generator;

		// Token: 0x0400359F RID: 13727
		private Panel _itemsHost;

		// Token: 0x040035A0 RID: 13728
		private FrameworkElement _header;

		// Token: 0x040035A1 RID: 13729
		private Expander _expander;

		// Token: 0x040035A2 RID: 13730
		internal static readonly UncommonField<bool> MustDisableVirtualizationField = new UncommonField<bool>();

		// Token: 0x040035A3 RID: 13731
		internal static readonly UncommonField<bool> InBackgroundLayoutField = new UncommonField<bool>();

		// Token: 0x040035A4 RID: 13732
		internal static readonly UncommonField<Thickness> DesiredPixelItemsSizeCorrectionFactorField = new UncommonField<Thickness>();

		// Token: 0x040035A5 RID: 13733
		internal static readonly UncommonField<HierarchicalVirtualizationConstraints> HierarchicalVirtualizationConstraintsField = new UncommonField<HierarchicalVirtualizationConstraints>();

		// Token: 0x040035A6 RID: 13734
		internal static readonly UncommonField<HierarchicalVirtualizationHeaderDesiredSizes> HierarchicalVirtualizationHeaderDesiredSizesField = new UncommonField<HierarchicalVirtualizationHeaderDesiredSizes>();

		// Token: 0x040035A7 RID: 13735
		internal static readonly UncommonField<HierarchicalVirtualizationItemDesiredSizes> HierarchicalVirtualizationItemDesiredSizesField = new UncommonField<HierarchicalVirtualizationItemDesiredSizes>();

		// Token: 0x040035A8 RID: 13736
		private static DependencyObjectType _dType;

		// Token: 0x040035A9 RID: 13737
		private const string ExpanderHeaderPartName = "HeaderSite";
	}
}
