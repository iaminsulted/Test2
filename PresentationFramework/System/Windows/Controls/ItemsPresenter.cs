using System;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020007A4 RID: 1956
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class ItemsPresenter : FrameworkElement
	{
		// Token: 0x06006E64 RID: 28260 RVA: 0x002D18C7 File Offset: 0x002D08C7
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			this.AttachToOwner();
		}

		// Token: 0x06006E65 RID: 28261 RVA: 0x002D18D8 File Offset: 0x002D08D8
		public override void OnApplyTemplate()
		{
			Panel panel = this.GetVisualChild(0) as Panel;
			if (panel == null || VisualTreeHelper.GetChildrenCount(panel) > 0)
			{
				throw new InvalidOperationException(SR.Get("ItemsPanelNotSingleNode"));
			}
			this.OnPanelChanged(this, EventArgs.Empty);
			base.OnApplyTemplate();
		}

		// Token: 0x06006E66 RID: 28262 RVA: 0x0029CEF8 File Offset: 0x0029BEF8
		protected override Size MeasureOverride(Size constraint)
		{
			return Helper.MeasureElementWithSingleChild(this, constraint);
		}

		// Token: 0x06006E67 RID: 28263 RVA: 0x0029CF01 File Offset: 0x0029BF01
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return Helper.ArrangeElementWithSingleChild(this, arrangeSize);
		}

		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x06006E68 RID: 28264 RVA: 0x002D1920 File Offset: 0x002D0920
		internal ItemsControl Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x06006E69 RID: 28265 RVA: 0x002D1928 File Offset: 0x002D0928
		internal ItemContainerGenerator Generator
		{
			get
			{
				return this._generator;
			}
		}

		// Token: 0x17001980 RID: 6528
		// (get) Token: 0x06006E6A RID: 28266 RVA: 0x002D1930 File Offset: 0x002D0930
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x17001981 RID: 6529
		// (get) Token: 0x06006E6B RID: 28267 RVA: 0x002D1938 File Offset: 0x002D0938
		// (set) Token: 0x06006E6C RID: 28268 RVA: 0x002D1940 File Offset: 0x002D0940
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ItemsPanelTemplate)value;
			}
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x06006E6D RID: 28269 RVA: 0x002D1938 File Offset: 0x002D0938
		// (set) Token: 0x06006E6E RID: 28270 RVA: 0x002D194E File Offset: 0x002D094E
		private ItemsPanelTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(ItemsPresenter.TemplateProperty, value);
			}
		}

		// Token: 0x06006E6F RID: 28271 RVA: 0x002D195C File Offset: 0x002D095C
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ItemsPanelTemplate)oldTemplate, (ItemsPanelTemplate)newTemplate);
		}

		// Token: 0x06006E70 RID: 28272 RVA: 0x002D1970 File Offset: 0x002D0970
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			StyleHelper.UpdateTemplateCache((ItemsPresenter)d, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, ItemsPresenter.TemplateProperty);
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnTemplateChanged(ItemsPanelTemplate oldTemplate, ItemsPanelTemplate newTemplate)
		{
		}

		// Token: 0x06006E72 RID: 28274 RVA: 0x002D199A File Offset: 0x002D099A
		internal static ItemsPresenter FromPanel(Panel panel)
		{
			if (panel == null)
			{
				return null;
			}
			return panel.TemplatedParent as ItemsPresenter;
		}

		// Token: 0x06006E73 RID: 28275 RVA: 0x002D19AC File Offset: 0x002D09AC
		internal static ItemsPresenter FromGroupItem(GroupItem groupItem)
		{
			if (groupItem == null)
			{
				return null;
			}
			Visual visual = VisualTreeHelper.GetParent(groupItem) as Visual;
			if (visual == null)
			{
				return null;
			}
			return VisualTreeHelper.GetParent(visual) as ItemsPresenter;
		}

		// Token: 0x06006E74 RID: 28276 RVA: 0x002D19DA File Offset: 0x002D09DA
		internal override void OnAncestorChanged()
		{
			if (base.TemplatedParent == null)
			{
				this.UseGenerator(null);
				this.ClearPanel();
			}
			base.OnAncestorChanged();
		}

		// Token: 0x06006E75 RID: 28277 RVA: 0x002D19F8 File Offset: 0x002D09F8
		private void AttachToOwner()
		{
			DependencyObject templatedParent = base.TemplatedParent;
			ItemsControl itemsControl = templatedParent as ItemsControl;
			ItemContainerGenerator generator;
			if (itemsControl != null)
			{
				generator = itemsControl.ItemContainerGenerator;
			}
			else
			{
				GroupItem groupItem = templatedParent as GroupItem;
				ItemsPresenter itemsPresenter = ItemsPresenter.FromGroupItem(groupItem);
				if (itemsPresenter != null)
				{
					itemsControl = itemsPresenter.Owner;
				}
				generator = ((groupItem != null) ? groupItem.Generator : null);
			}
			this._owner = itemsControl;
			this.UseGenerator(generator);
			GroupStyle groupStyle = (this._generator != null) ? this._generator.GroupStyle : null;
			ItemsPanelTemplate itemsPanelTemplate;
			if (groupStyle != null)
			{
				itemsPanelTemplate = groupStyle.Panel;
				if (itemsPanelTemplate == null)
				{
					if (VirtualizingPanel.GetIsVirtualizingWhenGrouping(itemsControl))
					{
						itemsPanelTemplate = GroupStyle.DefaultVirtualizingStackPanel;
					}
					else
					{
						itemsPanelTemplate = GroupStyle.DefaultStackPanel;
					}
				}
			}
			else
			{
				itemsPanelTemplate = ((this._owner != null) ? this._owner.ItemsPanel : null);
			}
			this.Template = itemsPanelTemplate;
		}

		// Token: 0x06006E76 RID: 28278 RVA: 0x002D1AB8 File Offset: 0x002D0AB8
		private void UseGenerator(ItemContainerGenerator generator)
		{
			if (generator == this._generator)
			{
				return;
			}
			if (this._generator != null)
			{
				this._generator.PanelChanged -= this.OnPanelChanged;
			}
			this._generator = generator;
			if (this._generator != null)
			{
				this._generator.PanelChanged += this.OnPanelChanged;
			}
		}

		// Token: 0x06006E77 RID: 28279 RVA: 0x002D1B14 File Offset: 0x002D0B14
		private void OnPanelChanged(object sender, EventArgs e)
		{
			base.InvalidateMeasure();
			if (base.Parent is ScrollViewer)
			{
				ScrollContentPresenter scrollContentPresenter = VisualTreeHelper.GetParent(this) as ScrollContentPresenter;
				if (scrollContentPresenter != null)
				{
					scrollContentPresenter.HookupScrollingComponents();
				}
			}
		}

		// Token: 0x06006E78 RID: 28280 RVA: 0x002D1B4C File Offset: 0x002D0B4C
		private void ClearPanel()
		{
			Panel panel = (this.VisualChildrenCount > 0) ? (this.GetVisualChild(0) as Panel) : null;
			Type right = null;
			if (this.Template != null)
			{
				if (this.Template.VisualTree != null)
				{
					right = this.Template.VisualTree.Type;
				}
				else if (this.Template.HasXamlNodeContent)
				{
					right = this.Template.Template.RootType.UnderlyingType;
				}
			}
			if (panel != null && panel.GetType() == right)
			{
				panel.IsItemsHost = false;
			}
		}

		// Token: 0x04003654 RID: 13908
		internal static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ItemsPanelTemplate), typeof(ItemsPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ItemsPresenter.OnTemplateChanged)));

		// Token: 0x04003655 RID: 13909
		private ItemsControl _owner;

		// Token: 0x04003656 RID: 13910
		private ItemContainerGenerator _generator;

		// Token: 0x04003657 RID: 13911
		private ItemsPanelTemplate _templateCache;
	}
}
