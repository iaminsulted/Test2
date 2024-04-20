using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using MS.Internal.Controls;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x02000838 RID: 2104
	public abstract class GridViewRowPresenterBase : FrameworkElement, IWeakEventListener
	{
		// Token: 0x06007B6A RID: 31594 RVA: 0x0030C2D1 File Offset: 0x0030B2D1
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridViewRowPresenterBase", new object[]
			{
				base.GetType(),
				(this.Columns != null) ? this.Columns.Count : 0
			});
		}

		// Token: 0x17001C86 RID: 7302
		// (get) Token: 0x06007B6B RID: 31595 RVA: 0x0030C30A File Offset: 0x0030B30A
		// (set) Token: 0x06007B6C RID: 31596 RVA: 0x0030C31C File Offset: 0x0030B31C
		public GridViewColumnCollection Columns
		{
			get
			{
				return (GridViewColumnCollection)base.GetValue(GridViewRowPresenterBase.ColumnsProperty);
			}
			set
			{
				base.SetValue(GridViewRowPresenterBase.ColumnsProperty, value);
			}
		}

		// Token: 0x17001C87 RID: 7303
		// (get) Token: 0x06007B6D RID: 31597 RVA: 0x0030C32A File Offset: 0x0030B32A
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (this.InternalChildren.Count == 0)
				{
					return EmptyEnumerator.Instance;
				}
				return this.InternalChildren.GetEnumerator();
			}
		}

		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x06007B6E RID: 31598 RVA: 0x0030C34A File Offset: 0x0030B34A
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					return 0;
				}
				return this._uiElementCollection.Count;
			}
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x0030C361 File Offset: 0x0030B361
		protected override Visual GetVisualChild(int index)
		{
			if (this._uiElementCollection == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._uiElementCollection[index];
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x0030C394 File Offset: 0x0030B394
		internal virtual void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
		{
			if (this.DesiredWidthList != null)
			{
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					if (this.DesiredWidthList.Count > e.ActualIndex)
					{
						this.DesiredWidthList.RemoveAt(e.ActualIndex);
						return;
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					this.DesiredWidthList = null;
				}
			}
		}

		// Token: 0x06007B71 RID: 31601
		internal abstract void OnColumnPropertyChanged(GridViewColumn column, string propertyName);

		// Token: 0x06007B72 RID: 31602 RVA: 0x0030C3F0 File Offset: 0x0030B3F0
		internal void EnsureDesiredWidthList()
		{
			GridViewColumnCollection columns = this.Columns;
			if (columns != null)
			{
				int count = columns.Count;
				if (this.DesiredWidthList == null)
				{
					this.DesiredWidthList = new List<double>(count);
				}
				int num = count - this.DesiredWidthList.Count;
				for (int i = 0; i < num; i++)
				{
					this.DesiredWidthList.Add(double.NaN);
				}
			}
		}

		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x06007B73 RID: 31603 RVA: 0x0030C450 File Offset: 0x0030B450
		// (set) Token: 0x06007B74 RID: 31604 RVA: 0x0030C458 File Offset: 0x0030B458
		internal List<double> DesiredWidthList
		{
			get
			{
				return this._desiredWidthList;
			}
			private set
			{
				this._desiredWidthList = value;
			}
		}

		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x06007B75 RID: 31605 RVA: 0x0030C461 File Offset: 0x0030B461
		// (set) Token: 0x06007B76 RID: 31606 RVA: 0x0030C469 File Offset: 0x0030B469
		internal bool NeedUpdateVisualTree
		{
			get
			{
				return this._needUpdateVisualTree;
			}
			set
			{
				this._needUpdateVisualTree = value;
			}
		}

		// Token: 0x17001C8B RID: 7307
		// (get) Token: 0x06007B77 RID: 31607 RVA: 0x0030C472 File Offset: 0x0030B472
		internal UIElementCollection InternalChildren
		{
			get
			{
				if (this._uiElementCollection == null)
				{
					this._uiElementCollection = new UIElementCollection(this, this);
				}
				return this._uiElementCollection;
			}
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x0030C490 File Offset: 0x0030B490
		private static void ColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridViewRowPresenterBase gridViewRowPresenterBase = (GridViewRowPresenterBase)d;
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)e.OldValue;
			if (gridViewColumnCollection != null)
			{
				InternalCollectionChangedEventManager.RemoveHandler(gridViewColumnCollection, new EventHandler<NotifyCollectionChangedEventArgs>(gridViewRowPresenterBase.ColumnCollectionChanged));
				if (!gridViewColumnCollection.InViewMode && gridViewColumnCollection.Owner == gridViewRowPresenterBase.GetStableAncester())
				{
					gridViewColumnCollection.Owner = null;
				}
			}
			GridViewColumnCollection gridViewColumnCollection2 = (GridViewColumnCollection)e.NewValue;
			if (gridViewColumnCollection2 != null)
			{
				InternalCollectionChangedEventManager.AddHandler(gridViewColumnCollection2, new EventHandler<NotifyCollectionChangedEventArgs>(gridViewRowPresenterBase.ColumnCollectionChanged));
				if (!gridViewColumnCollection2.InViewMode && gridViewColumnCollection2.Owner == null)
				{
					gridViewColumnCollection2.Owner = gridViewRowPresenterBase.GetStableAncester();
				}
			}
			gridViewRowPresenterBase.NeedUpdateVisualTree = true;
			gridViewRowPresenterBase.InvalidateMeasure();
		}

		// Token: 0x06007B79 RID: 31609 RVA: 0x0030C530 File Offset: 0x0030B530
		private FrameworkElement GetStableAncester()
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(base.TemplatedParent);
			if (itemsControl == null)
			{
				return this;
			}
			return itemsControl;
		}

		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x06007B7A RID: 31610 RVA: 0x0030C54F File Offset: 0x0030B54F
		private bool IsPresenterVisualReady
		{
			get
			{
				return base.IsInitialized && !this.NeedUpdateVisualTree;
			}
		}

		// Token: 0x06007B7B RID: 31611 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
		{
			return false;
		}

		// Token: 0x06007B7C RID: 31612 RVA: 0x0030C564 File Offset: 0x0030B564
		private void ColumnCollectionChanged(object sender, NotifyCollectionChangedEventArgs arg)
		{
			GridViewColumnCollectionChangedEventArgs gridViewColumnCollectionChangedEventArgs = arg as GridViewColumnCollectionChangedEventArgs;
			if (gridViewColumnCollectionChangedEventArgs != null && this.IsPresenterVisualReady)
			{
				if (gridViewColumnCollectionChangedEventArgs.Column != null)
				{
					this.OnColumnPropertyChanged(gridViewColumnCollectionChangedEventArgs.Column, gridViewColumnCollectionChangedEventArgs.PropertyName);
					return;
				}
				this.OnColumnCollectionChanged(gridViewColumnCollectionChangedEventArgs);
			}
		}

		// Token: 0x04003A48 RID: 14920
		public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns", typeof(GridViewColumnCollection), typeof(GridViewRowPresenterBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(GridViewRowPresenterBase.ColumnsPropertyChanged)));

		// Token: 0x04003A49 RID: 14921
		internal const double c_PaddingHeaderMinWidth = 2.0;

		// Token: 0x04003A4A RID: 14922
		private UIElementCollection _uiElementCollection;

		// Token: 0x04003A4B RID: 14923
		private bool _needUpdateVisualTree = true;

		// Token: 0x04003A4C RID: 14924
		private List<double> _desiredWidthList;
	}
}
