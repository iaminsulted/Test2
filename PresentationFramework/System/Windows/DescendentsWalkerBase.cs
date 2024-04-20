using System;
using System.Windows.Media;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x02000359 RID: 857
	internal class DescendentsWalkerBase
	{
		// Token: 0x06002072 RID: 8306 RVA: 0x00175794 File Offset: 0x00174794
		protected DescendentsWalkerBase(TreeWalkPriority priority)
		{
			this._startNode = null;
			this._priority = priority;
			this._recursionDepth = 0;
			this._nodes = default(FrugalStructList<DependencyObject>);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x001757C0 File Offset: 0x001747C0
		internal bool WasVisited(DependencyObject d)
		{
			DependencyObject dependencyObject = d;
			while (dependencyObject != this._startNode && dependencyObject != null)
			{
				DependencyObject dependencyObject2;
				if (FrameworkElement.DType.IsInstanceOfType(dependencyObject))
				{
					FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
					dependencyObject2 = frameworkElement.Parent;
					DependencyObject parent = VisualTreeHelper.GetParent(frameworkElement);
					if (parent != null && dependencyObject2 != null && parent != dependencyObject2)
					{
						return this._nodes.Contains(dependencyObject);
					}
					if (parent != null)
					{
						dependencyObject = parent;
						continue;
					}
				}
				else
				{
					FrameworkContentElement frameworkContentElement = dependencyObject as FrameworkContentElement;
					dependencyObject2 = ((frameworkContentElement != null) ? frameworkContentElement.Parent : null);
				}
				dependencyObject = dependencyObject2;
			}
			return dependencyObject != null;
		}

		// Token: 0x04000FDD RID: 4061
		internal DependencyObject _startNode;

		// Token: 0x04000FDE RID: 4062
		internal TreeWalkPriority _priority;

		// Token: 0x04000FDF RID: 4063
		internal FrugalStructList<DependencyObject> _nodes;

		// Token: 0x04000FE0 RID: 4064
		internal int _recursionDepth;
	}
}
