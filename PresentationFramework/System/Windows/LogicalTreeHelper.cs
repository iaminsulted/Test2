using System;
using System.Collections;
using MS.Internal.Controls;

namespace System.Windows
{
	// Token: 0x0200037D RID: 893
	public static class LogicalTreeHelper
	{
		// Token: 0x06002420 RID: 9248 RVA: 0x0018191C File Offset: 0x0018091C
		public static DependencyObject FindLogicalNode(DependencyObject logicalTreeNode, string elementName)
		{
			if (logicalTreeNode == null)
			{
				throw new ArgumentNullException("logicalTreeNode");
			}
			if (elementName == null)
			{
				throw new ArgumentNullException("elementName");
			}
			if (elementName == string.Empty)
			{
				throw new ArgumentException(SR.Get("StringEmpty"), "elementName");
			}
			DependencyObject dependencyObject = null;
			IFrameworkInputElement frameworkInputElement = logicalTreeNode as IFrameworkInputElement;
			if (frameworkInputElement != null && frameworkInputElement.Name == elementName)
			{
				dependencyObject = logicalTreeNode;
			}
			if (dependencyObject == null)
			{
				IEnumerator logicalChildren = LogicalTreeHelper.GetLogicalChildren(logicalTreeNode);
				if (logicalChildren != null)
				{
					logicalChildren.Reset();
					while (dependencyObject == null && logicalChildren.MoveNext())
					{
						DependencyObject dependencyObject2 = logicalChildren.Current as DependencyObject;
						if (dependencyObject2 != null)
						{
							dependencyObject = LogicalTreeHelper.FindLogicalNode(dependencyObject2, elementName);
						}
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x001819C0 File Offset: 0x001809C0
		public static DependencyObject GetParent(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.Parent;
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.Parent;
			}
			return null;
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x00181A00 File Offset: 0x00180A00
		public static IEnumerable GetChildren(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return new LogicalTreeHelper.EnumeratorWrapper(frameworkElement.LogicalChildren);
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return new LogicalTreeHelper.EnumeratorWrapper(frameworkContentElement.LogicalChildren);
			}
			return LogicalTreeHelper.EnumeratorWrapper.Empty;
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00181A4C File Offset: 0x00180A4C
		public static IEnumerable GetChildren(FrameworkElement current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			return new LogicalTreeHelper.EnumeratorWrapper(current.LogicalChildren);
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x00181A67 File Offset: 0x00180A67
		public static IEnumerable GetChildren(FrameworkContentElement current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			return new LogicalTreeHelper.EnumeratorWrapper(current.LogicalChildren);
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x00181A84 File Offset: 0x00180A84
		public static void BringIntoView(DependencyObject current)
		{
			if (current == null)
			{
				throw new ArgumentNullException("current");
			}
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				frameworkContentElement.BringIntoView();
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x00181AC0 File Offset: 0x00180AC0
		internal static void AddLogicalChild(DependencyObject parent, object child)
		{
			if (child != null && parent != null)
			{
				FrameworkElement frameworkElement = parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.AddLogicalChild(child);
					return;
				}
				FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.AddLogicalChild(child);
				}
			}
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x00181AF6 File Offset: 0x00180AF6
		internal static void AddLogicalChild(FrameworkElement parentFE, FrameworkContentElement parentFCE, object child)
		{
			if (child != null)
			{
				if (parentFE != null)
				{
					parentFE.AddLogicalChild(child);
					return;
				}
				if (parentFCE != null)
				{
					parentFCE.AddLogicalChild(child);
				}
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x00181B10 File Offset: 0x00180B10
		internal static void RemoveLogicalChild(DependencyObject parent, object child)
		{
			if (child != null && parent != null)
			{
				FrameworkElement frameworkElement = parent as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.RemoveLogicalChild(child);
					return;
				}
				FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
				if (frameworkContentElement != null)
				{
					frameworkContentElement.RemoveLogicalChild(child);
				}
			}
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x00181B46 File Offset: 0x00180B46
		internal static void RemoveLogicalChild(FrameworkElement parentFE, FrameworkContentElement parentFCE, object child)
		{
			if (child != null)
			{
				if (parentFE != null)
				{
					parentFE.RemoveLogicalChild(child);
					return;
				}
				parentFCE.RemoveLogicalChild(child);
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x00181B60 File Offset: 0x00180B60
		internal static IEnumerator GetLogicalChildren(DependencyObject current)
		{
			FrameworkElement frameworkElement = current as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.LogicalChildren;
			}
			FrameworkContentElement frameworkContentElement = current as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.LogicalChildren;
			}
			return EmptyEnumerator.Instance;
		}

		// Token: 0x02000A83 RID: 2691
		private class EnumeratorWrapper : IEnumerable
		{
			// Token: 0x0600866B RID: 34411 RVA: 0x0032A768 File Offset: 0x00329768
			public EnumeratorWrapper(IEnumerator enumerator)
			{
				if (enumerator != null)
				{
					this._enumerator = enumerator;
					return;
				}
				this._enumerator = EmptyEnumerator.Instance;
			}

			// Token: 0x0600866C RID: 34412 RVA: 0x0032A786 File Offset: 0x00329786
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._enumerator;
			}

			// Token: 0x17001E1B RID: 7707
			// (get) Token: 0x0600866D RID: 34413 RVA: 0x0032A78E File Offset: 0x0032978E
			internal static LogicalTreeHelper.EnumeratorWrapper Empty
			{
				get
				{
					if (LogicalTreeHelper.EnumeratorWrapper._emptyInstance == null)
					{
						LogicalTreeHelper.EnumeratorWrapper._emptyInstance = new LogicalTreeHelper.EnumeratorWrapper(null);
					}
					return LogicalTreeHelper.EnumeratorWrapper._emptyInstance;
				}
			}

			// Token: 0x04004199 RID: 16793
			private IEnumerator _enumerator;

			// Token: 0x0400419A RID: 16794
			private static LogicalTreeHelper.EnumeratorWrapper _emptyInstance;
		}
	}
}
