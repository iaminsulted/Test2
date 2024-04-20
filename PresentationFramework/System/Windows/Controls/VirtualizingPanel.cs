using System;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x02000808 RID: 2056
	public abstract class VirtualizingPanel : Panel
	{
		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x06007775 RID: 30581 RVA: 0x002F2CF5 File Offset: 0x002F1CF5
		public bool CanHierarchicallyScrollAndVirtualize
		{
			get
			{
				return this.CanHierarchicallyScrollAndVirtualizeCore;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x06007776 RID: 30582 RVA: 0x00105F35 File Offset: 0x00104F35
		protected virtual bool CanHierarchicallyScrollAndVirtualizeCore
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007777 RID: 30583 RVA: 0x002F2CFD File Offset: 0x002F1CFD
		public double GetItemOffset(UIElement child)
		{
			return this.GetItemOffsetCore(child);
		}

		// Token: 0x06007778 RID: 30584 RVA: 0x0016E9C3 File Offset: 0x0016D9C3
		protected virtual double GetItemOffsetCore(UIElement child)
		{
			return 0.0;
		}

		// Token: 0x06007779 RID: 30585 RVA: 0x002F2D06 File Offset: 0x002F1D06
		public static bool GetIsVirtualizing(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsVirtualizingProperty);
		}

		// Token: 0x0600777A RID: 30586 RVA: 0x002F2D26 File Offset: 0x002F1D26
		public static void SetIsVirtualizing(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsVirtualizingProperty, value);
		}

		// Token: 0x0600777B RID: 30587 RVA: 0x002F2D42 File Offset: 0x002F1D42
		public static VirtualizationMode GetVirtualizationMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationMode)element.GetValue(VirtualizingPanel.VirtualizationModeProperty);
		}

		// Token: 0x0600777C RID: 30588 RVA: 0x002F2D62 File Offset: 0x002F1D62
		public static void SetVirtualizationMode(DependencyObject element, VirtualizationMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.VirtualizationModeProperty, value);
		}

		// Token: 0x0600777D RID: 30589 RVA: 0x002F2D83 File Offset: 0x002F1D83
		public static bool GetIsVirtualizingWhenGrouping(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty);
		}

		// Token: 0x0600777E RID: 30590 RVA: 0x002F2DA3 File Offset: 0x002F1DA3
		public static void SetIsVirtualizingWhenGrouping(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, value);
		}

		// Token: 0x0600777F RID: 30591 RVA: 0x002F2DBF File Offset: 0x002F1DBF
		public static ScrollUnit GetScrollUnit(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollUnit)element.GetValue(VirtualizingPanel.ScrollUnitProperty);
		}

		// Token: 0x06007780 RID: 30592 RVA: 0x002F2DDF File Offset: 0x002F1DDF
		public static void SetScrollUnit(DependencyObject element, ScrollUnit value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.ScrollUnitProperty, value);
		}

		// Token: 0x06007781 RID: 30593 RVA: 0x002F2E00 File Offset: 0x002F1E00
		public static VirtualizationCacheLength GetCacheLength(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationCacheLength)element.GetValue(VirtualizingPanel.CacheLengthProperty);
		}

		// Token: 0x06007782 RID: 30594 RVA: 0x002F2E20 File Offset: 0x002F1E20
		public static void SetCacheLength(DependencyObject element, VirtualizationCacheLength value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.CacheLengthProperty, value);
		}

		// Token: 0x06007783 RID: 30595 RVA: 0x002F2E41 File Offset: 0x002F1E41
		public static VirtualizationCacheLengthUnit GetCacheLengthUnit(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationCacheLengthUnit)element.GetValue(VirtualizingPanel.CacheLengthUnitProperty);
		}

		// Token: 0x06007784 RID: 30596 RVA: 0x002F2E61 File Offset: 0x002F1E61
		public static void SetCacheLengthUnit(DependencyObject element, VirtualizationCacheLengthUnit value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.CacheLengthUnitProperty, value);
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x002F2E82 File Offset: 0x002F1E82
		public static bool GetIsContainerVirtualizable(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsContainerVirtualizableProperty);
		}

		// Token: 0x06007786 RID: 30598 RVA: 0x002F2EA2 File Offset: 0x002F1EA2
		public static void SetIsContainerVirtualizable(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsContainerVirtualizableProperty, value);
		}

		// Token: 0x06007787 RID: 30599 RVA: 0x002F2EBE File Offset: 0x002F1EBE
		internal static bool GetShouldCacheContainerSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return !VirtualizingStackPanel.IsVSP45Compat || (bool)element.GetValue(VirtualizingPanel.ShouldCacheContainerSizeProperty);
		}

		// Token: 0x06007788 RID: 30600 RVA: 0x002F2EE8 File Offset: 0x002F1EE8
		private static bool ValidateCacheSizeBeforeOrAfterViewport(object value)
		{
			VirtualizationCacheLength virtualizationCacheLength = (VirtualizationCacheLength)value;
			return DoubleUtil.GreaterThanOrClose(virtualizationCacheLength.CacheBeforeViewport, 0.0) && DoubleUtil.GreaterThanOrClose(virtualizationCacheLength.CacheAfterViewport, 0.0);
		}

		// Token: 0x06007789 RID: 30601 RVA: 0x002F2F2A File Offset: 0x002F1F2A
		private static object CoerceIsVirtualizingWhenGrouping(DependencyObject d, object baseValue)
		{
			return VirtualizingPanel.GetIsVirtualizing(d) && (bool)baseValue;
		}

		// Token: 0x0600778A RID: 30602 RVA: 0x002F2F44 File Offset: 0x002F1F44
		internal static void OnVirtualizationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = d as ItemsControl;
			if (itemsControl != null)
			{
				Panel itemsHost = itemsControl.ItemsHost;
				if (itemsHost != null)
				{
					itemsHost.InvalidateMeasure();
					ItemsPresenter itemsPresenter = VisualTreeHelper.GetParent(itemsHost) as ItemsPresenter;
					if (itemsPresenter != null)
					{
						itemsPresenter.InvalidateMeasure();
					}
					if (d is TreeView)
					{
						DependencyProperty property = e.Property;
						if (property == VirtualizingStackPanel.IsVirtualizingProperty || property == VirtualizingPanel.IsVirtualizingWhenGroupingProperty || property == VirtualizingStackPanel.VirtualizationModeProperty || property == VirtualizingPanel.ScrollUnitProperty)
						{
							VirtualizingPanel.VirtualizationPropertyChangePropagationRecursive(itemsControl, itemsHost);
						}
					}
				}
			}
		}

		// Token: 0x0600778B RID: 30603 RVA: 0x002F2FB8 File Offset: 0x002F1FB8
		private static void VirtualizationPropertyChangePropagationRecursive(DependencyObject parent, Panel itemsHost)
		{
			UIElementCollection internalChildren = itemsHost.InternalChildren;
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
				if (hierarchicalVirtualizationAndScrollInfo != null)
				{
					TreeViewItem.IsVirtualizingPropagationHelper(parent, (DependencyObject)hierarchicalVirtualizationAndScrollInfo);
					Panel itemsHost2 = hierarchicalVirtualizationAndScrollInfo.ItemsHost;
					if (itemsHost2 != null)
					{
						VirtualizingPanel.VirtualizationPropertyChangePropagationRecursive((DependencyObject)hierarchicalVirtualizationAndScrollInfo, itemsHost2);
					}
				}
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600778C RID: 30604 RVA: 0x002F3014 File Offset: 0x002F2014
		public IItemContainerGenerator ItemContainerGenerator
		{
			get
			{
				return base.Generator;
			}
		}

		// Token: 0x0600778D RID: 30605 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void GenerateChildren()
		{
		}

		// Token: 0x0600778E RID: 30606 RVA: 0x002F301C File Offset: 0x002F201C
		protected void AddInternalChild(UIElement child)
		{
			VirtualizingPanel.AddInternalChild(base.InternalChildren, child);
		}

		// Token: 0x0600778F RID: 30607 RVA: 0x002F302A File Offset: 0x002F202A
		protected void InsertInternalChild(int index, UIElement child)
		{
			VirtualizingPanel.InsertInternalChild(base.InternalChildren, index, child);
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x002F3039 File Offset: 0x002F2039
		protected void RemoveInternalChildRange(int index, int range)
		{
			VirtualizingPanel.RemoveInternalChildRange(base.InternalChildren, index, range);
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x002F3048 File Offset: 0x002F2048
		internal static void AddInternalChild(UIElementCollection children, UIElement child)
		{
			children.AddInternal(child);
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x002F3052 File Offset: 0x002F2052
		internal static void InsertInternalChild(UIElementCollection children, int index, UIElement child)
		{
			children.InsertInternal(index, child);
		}

		// Token: 0x06007793 RID: 30611 RVA: 0x002F305C File Offset: 0x002F205C
		internal static void RemoveInternalChildRange(UIElementCollection children, int index, int range)
		{
			children.RemoveRangeInternal(index, range);
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
		}

		// Token: 0x06007795 RID: 30613 RVA: 0x002F3066 File Offset: 0x002F2066
		public bool ShouldItemsChangeAffectLayout(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			return this.ShouldItemsChangeAffectLayoutCore(areItemChangesLocal, args);
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		protected virtual bool ShouldItemsChangeAffectLayoutCore(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			return true;
		}

		// Token: 0x06007797 RID: 30615 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnClearChildren()
		{
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x002AAF1F File Offset: 0x002A9F1F
		public void BringIndexIntoViewPublic(int index)
		{
			this.BringIndexIntoView(index);
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected internal virtual void BringIndexIntoView(int index)
		{
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x002F3070 File Offset: 0x002F2070
		internal override bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
		{
			NotifyCollectionChangedAction action = args.Action;
			if (action > NotifyCollectionChangedAction.Move)
			{
				base.OnItemsChangedInternal(sender, args);
			}
			this.OnItemsChanged(sender, args);
			return this.ShouldItemsChangeAffectLayout(true, args);
		}

		// Token: 0x0600779B RID: 30619 RVA: 0x002F30A1 File Offset: 0x002F20A1
		internal override void OnClearChildrenInternal()
		{
			this.OnClearChildren();
		}

		// Token: 0x040038D7 RID: 14551
		public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.RegisterAttached("IsVirtualizing", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		// Token: 0x040038D8 RID: 14552
		public static readonly DependencyProperty VirtualizationModeProperty = DependencyProperty.RegisterAttached("VirtualizationMode", typeof(VirtualizationMode), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(VirtualizationMode.Standard, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		// Token: 0x040038D9 RID: 14553
		public static readonly DependencyProperty IsVirtualizingWhenGroupingProperty = DependencyProperty.RegisterAttached("IsVirtualizingWhenGrouping", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged), new CoerceValueCallback(VirtualizingPanel.CoerceIsVirtualizingWhenGrouping)));

		// Token: 0x040038DA RID: 14554
		public static readonly DependencyProperty ScrollUnitProperty = DependencyProperty.RegisterAttached("ScrollUnit", typeof(ScrollUnit), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(ScrollUnit.Item, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		// Token: 0x040038DB RID: 14555
		public static readonly DependencyProperty CacheLengthProperty = DependencyProperty.RegisterAttached("CacheLength", typeof(VirtualizationCacheLength), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(new VirtualizationCacheLength(1.0), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)), new ValidateValueCallback(VirtualizingPanel.ValidateCacheSizeBeforeOrAfterViewport));

		// Token: 0x040038DC RID: 14556
		public static readonly DependencyProperty CacheLengthUnitProperty = DependencyProperty.RegisterAttached("CacheLengthUnit", typeof(VirtualizationCacheLengthUnit), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(VirtualizationCacheLengthUnit.Page, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		// Token: 0x040038DD RID: 14557
		public static readonly DependencyProperty IsContainerVirtualizableProperty = DependencyProperty.RegisterAttached("IsContainerVirtualizable", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true));

		// Token: 0x040038DE RID: 14558
		internal static readonly DependencyProperty ShouldCacheContainerSizeProperty = DependencyProperty.RegisterAttached("ShouldCacheContainerSize", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true));
	}
}
