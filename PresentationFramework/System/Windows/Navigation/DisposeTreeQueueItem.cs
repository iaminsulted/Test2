using System;
using System.Collections;
using System.Windows.Controls;

namespace System.Windows.Navigation
{
	// Token: 0x020005CA RID: 1482
	internal class DisposeTreeQueueItem
	{
		// Token: 0x0600477E RID: 18302 RVA: 0x0022A04B File Offset: 0x0022904B
		internal object Dispatch(object o)
		{
			this.DisposeElement(this._root);
			return null;
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x0022A05C File Offset: 0x0022905C
		internal void DisposeElement(object node)
		{
			DependencyObject dependencyObject = node as DependencyObject;
			if (dependencyObject != null)
			{
				bool flag = false;
				IEnumerator logicalChildren = LogicalTreeHelper.GetLogicalChildren(dependencyObject);
				if (logicalChildren != null)
				{
					while (logicalChildren.MoveNext())
					{
						flag = true;
						object node2 = logicalChildren.Current;
						this.DisposeElement(node2);
					}
				}
				if (!flag)
				{
					ContentControl contentControl = dependencyObject as ContentControl;
					if (contentControl != null && contentControl.ContentIsNotLogical && contentControl.Content != null)
					{
						this.DisposeElement(contentControl.Content);
					}
				}
			}
			IDisposable disposable = node as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0022A0D9 File Offset: 0x002290D9
		internal DisposeTreeQueueItem(object node)
		{
			this._root = node;
		}

		// Token: 0x040025D8 RID: 9688
		private object _root;
	}
}
