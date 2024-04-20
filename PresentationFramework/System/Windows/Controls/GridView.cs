using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	// Token: 0x02000784 RID: 1924
	[ContentProperty("Columns")]
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	public class GridView : ViewBase, IAddChild
	{
		// Token: 0x06006A38 RID: 27192 RVA: 0x002C1805 File Offset: 0x002C0805
		void IAddChild.AddChild(object column)
		{
			this.AddChild(column);
		}

		// Token: 0x06006A39 RID: 27193 RVA: 0x002C1810 File Offset: 0x002C0810
		protected virtual void AddChild(object column)
		{
			GridViewColumn gridViewColumn = column as GridViewColumn;
			if (gridViewColumn != null)
			{
				this.Columns.Add(gridViewColumn);
				return;
			}
			throw new InvalidOperationException(SR.Get("ListView_IllegalChildrenType"));
		}

		// Token: 0x06006A3A RID: 27194 RVA: 0x002C1843 File Offset: 0x002C0843
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		// Token: 0x06006A3B RID: 27195 RVA: 0x002C1805 File Offset: 0x002C0805
		protected virtual void AddText(string text)
		{
			this.AddChild(text);
		}

		// Token: 0x06006A3C RID: 27196 RVA: 0x002C184C File Offset: 0x002C084C
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridView", new object[]
			{
				base.GetType(),
				this.Columns.Count
			});
		}

		// Token: 0x06006A3D RID: 27197 RVA: 0x002C187A File Offset: 0x002C087A
		protected internal override IViewAutomationPeer GetAutomationPeer(ListView parent)
		{
			return new GridViewAutomationPeer(this, parent);
		}

		// Token: 0x17001888 RID: 6280
		// (get) Token: 0x06006A3E RID: 27198 RVA: 0x002C1883 File Offset: 0x002C0883
		public static ResourceKey GridViewScrollViewerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewScrollViewerStyleKey;
			}
		}

		// Token: 0x17001889 RID: 6281
		// (get) Token: 0x06006A3F RID: 27199 RVA: 0x002C188A File Offset: 0x002C088A
		public static ResourceKey GridViewStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewStyleKey;
			}
		}

		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x06006A40 RID: 27200 RVA: 0x002C1891 File Offset: 0x002C0891
		public static ResourceKey GridViewItemContainerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewItemContainerStyleKey;
			}
		}

		// Token: 0x06006A41 RID: 27201 RVA: 0x002C1898 File Offset: 0x002C0898
		public static GridViewColumnCollection GetColumnCollection(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (GridViewColumnCollection)element.GetValue(GridView.ColumnCollectionProperty);
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x002C18B8 File Offset: 0x002C08B8
		public static void SetColumnCollection(DependencyObject element, GridViewColumnCollection collection)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(GridView.ColumnCollectionProperty, collection);
		}

		// Token: 0x06006A43 RID: 27203 RVA: 0x002C18D4 File Offset: 0x002C08D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ShouldSerializeColumnCollection(DependencyObject obj)
		{
			ListViewItem listViewItem = obj as ListViewItem;
			if (listViewItem != null)
			{
				ListView listView = listViewItem.ParentSelector as ListView;
				if (listView != null)
				{
					GridView gridView = listView.View as GridView;
					if (gridView != null)
					{
						return listViewItem.ReadLocalValue(GridView.ColumnCollectionProperty) as GridViewColumnCollection != gridView.Columns;
					}
				}
			}
			return true;
		}

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x06006A44 RID: 27204 RVA: 0x002C1926 File Offset: 0x002C0926
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public GridViewColumnCollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = new GridViewColumnCollection();
					this._columns.Owner = this;
					this._columns.InViewMode = true;
				}
				return this._columns;
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x06006A45 RID: 27205 RVA: 0x002C1959 File Offset: 0x002C0959
		// (set) Token: 0x06006A46 RID: 27206 RVA: 0x002C196B File Offset: 0x002C096B
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridView.ColumnHeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContainerStyleProperty, value);
			}
		}

		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06006A47 RID: 27207 RVA: 0x002C1979 File Offset: 0x002C0979
		// (set) Token: 0x06006A48 RID: 27208 RVA: 0x002C198B File Offset: 0x002C098B
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridView.ColumnHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateProperty, value);
			}
		}

		// Token: 0x06006A49 RID: 27209 RVA: 0x002C199C File Offset: 0x002C099C
		private static void OnColumnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		// Token: 0x1700188E RID: 6286
		// (get) Token: 0x06006A4A RID: 27210 RVA: 0x002C19C5 File Offset: 0x002C09C5
		// (set) Token: 0x06006A4B RID: 27211 RVA: 0x002C19D7 File Offset: 0x002C09D7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridView.ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06006A4C RID: 27212 RVA: 0x002C199C File Offset: 0x002C099C
		private static void OnColumnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		// Token: 0x1700188F RID: 6287
		// (get) Token: 0x06006A4D RID: 27213 RVA: 0x002C19E5 File Offset: 0x002C09E5
		// (set) Token: 0x06006A4E RID: 27214 RVA: 0x002C19F7 File Offset: 0x002C09F7
		public string ColumnHeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridView.ColumnHeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderStringFormatProperty, value);
			}
		}

		// Token: 0x17001890 RID: 6288
		// (get) Token: 0x06006A4F RID: 27215 RVA: 0x002C1A05 File Offset: 0x002C0A05
		// (set) Token: 0x06006A50 RID: 27216 RVA: 0x002C1A17 File Offset: 0x002C0A17
		public bool AllowsColumnReorder
		{
			get
			{
				return (bool)base.GetValue(GridView.AllowsColumnReorderProperty);
			}
			set
			{
				base.SetValue(GridView.AllowsColumnReorderProperty, value);
			}
		}

		// Token: 0x17001891 RID: 6289
		// (get) Token: 0x06006A51 RID: 27217 RVA: 0x002C1A25 File Offset: 0x002C0A25
		// (set) Token: 0x06006A52 RID: 27218 RVA: 0x002C1A37 File Offset: 0x002C0A37
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(GridView.ColumnHeaderContextMenuProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContextMenuProperty, value);
			}
		}

		// Token: 0x17001892 RID: 6290
		// (get) Token: 0x06006A53 RID: 27219 RVA: 0x002C1A45 File Offset: 0x002C0A45
		// (set) Token: 0x06006A54 RID: 27220 RVA: 0x002C1A52 File Offset: 0x002C0A52
		public object ColumnHeaderToolTip
		{
			get
			{
				return base.GetValue(GridView.ColumnHeaderToolTipProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderToolTipProperty, value);
			}
		}

		// Token: 0x06006A55 RID: 27221 RVA: 0x002C1A60 File Offset: 0x002C0A60
		protected internal override void PrepareItem(ListViewItem item)
		{
			base.PrepareItem(item);
			GridView.SetColumnCollection(item, this._columns);
		}

		// Token: 0x06006A56 RID: 27222 RVA: 0x002C1A75 File Offset: 0x002C0A75
		protected internal override void ClearItem(ListViewItem item)
		{
			item.ClearValue(GridView.ColumnCollectionProperty);
			base.ClearItem(item);
		}

		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06006A57 RID: 27223 RVA: 0x002C1A89 File Offset: 0x002C0A89
		protected internal override object DefaultStyleKey
		{
			get
			{
				return GridView.GridViewStyleKey;
			}
		}

		// Token: 0x17001894 RID: 6292
		// (get) Token: 0x06006A58 RID: 27224 RVA: 0x002C1A90 File Offset: 0x002C0A90
		protected internal override object ItemContainerDefaultStyleKey
		{
			get
			{
				return GridView.GridViewItemContainerStyleKey;
			}
		}

		// Token: 0x06006A59 RID: 27225 RVA: 0x002C1A98 File Offset: 0x002C0A98
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			base.OnInheritanceContextChangedCore(args);
			if (this._columns != null)
			{
				foreach (GridViewColumn gridViewColumn in this._columns)
				{
					gridViewColumn.OnInheritanceContextChanged(args);
				}
			}
		}

		// Token: 0x06006A5A RID: 27226 RVA: 0x002C1AF4 File Offset: 0x002C0AF4
		internal override void OnThemeChanged()
		{
			if (this._columns != null)
			{
				for (int i = 0; i < this._columns.Count; i++)
				{
					this._columns[i].OnThemeChanged();
				}
			}
		}

		// Token: 0x17001895 RID: 6293
		// (get) Token: 0x06006A5B RID: 27227 RVA: 0x002C1B30 File Offset: 0x002C0B30
		// (set) Token: 0x06006A5C RID: 27228 RVA: 0x002C1B38 File Offset: 0x002C0B38
		internal GridViewHeaderRowPresenter HeaderRowPresenter
		{
			get
			{
				return this._gvheaderRP;
			}
			set
			{
				this._gvheaderRP = value;
			}
		}

		// Token: 0x0400353B RID: 13627
		public static readonly DependencyProperty ColumnCollectionProperty = DependencyProperty.RegisterAttached("ColumnCollection", typeof(GridViewColumnCollection), typeof(GridView));

		// Token: 0x0400353C RID: 13628
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = DependencyProperty.Register("ColumnHeaderContainerStyle", typeof(Style), typeof(GridView));

		// Token: 0x0400353D RID: 13629
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateChanged)));

		// Token: 0x0400353E RID: 13630
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateSelectorChanged)));

		// Token: 0x0400353F RID: 13631
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(GridView));

		// Token: 0x04003540 RID: 13632
		public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register("AllowsColumnReorder", typeof(bool), typeof(GridView), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		// Token: 0x04003541 RID: 13633
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(GridView));

		// Token: 0x04003542 RID: 13634
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = DependencyProperty.Register("ColumnHeaderToolTip", typeof(object), typeof(GridView));

		// Token: 0x04003543 RID: 13635
		private GridViewColumnCollection _columns;

		// Token: 0x04003544 RID: 13636
		private GridViewHeaderRowPresenter _gvheaderRP;
	}
}
