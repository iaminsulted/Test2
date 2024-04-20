using System;
using System.Collections;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Media;

namespace MS.Internal.Annotations.Anchoring
{
	// Token: 0x020002D4 RID: 724
	internal sealed class PathNode
	{
		// Token: 0x06001B5F RID: 7007 RVA: 0x00168CEE File Offset: 0x00167CEE
		internal PathNode(DependencyObject node)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this._node = node;
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00168D18 File Offset: 0x00167D18
		public override bool Equals(object obj)
		{
			PathNode pathNode = obj as PathNode;
			return pathNode != null && this._node.Equals(pathNode.Node);
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x00168D42 File Offset: 0x00167D42
		public override int GetHashCode()
		{
			if (this._node == null)
			{
				return base.GetHashCode();
			}
			return this._node.GetHashCode();
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001B62 RID: 7010 RVA: 0x00168D5E File Offset: 0x00167D5E
		public DependencyObject Node
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001B63 RID: 7011 RVA: 0x00168D66 File Offset: 0x00167D66
		public IList Children
		{
			get
			{
				return this._children;
			}
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00168D70 File Offset: 0x00167D70
		internal static PathNode BuildPathForElements(ICollection nodes)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			PathNode pathNode = null;
			foreach (object obj in nodes)
			{
				PathNode pathNode2 = PathNode.BuildPathForElement((DependencyObject)obj);
				if (pathNode == null)
				{
					pathNode = pathNode2;
				}
				else
				{
					PathNode.AddBranchToPath(pathNode, pathNode2);
				}
			}
			if (pathNode != null)
			{
				pathNode.FreezeChildren();
			}
			return pathNode;
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x00168DEC File Offset: 0x00167DEC
		internal static DependencyObject GetParent(DependencyObject node)
		{
			DependencyObject dependencyObject = node;
			DependencyObject dependencyObject2;
			for (;;)
			{
				dependencyObject2 = (DependencyObject)dependencyObject.GetValue(PathNode.HiddenParentProperty);
				if (dependencyObject2 == null)
				{
					Visual visual = dependencyObject as Visual;
					if (visual != null)
					{
						dependencyObject2 = VisualTreeHelper.GetParent(visual);
					}
				}
				if (dependencyObject2 == null)
				{
					dependencyObject2 = LogicalTreeHelper.GetParent(dependencyObject);
				}
				if (dependencyObject2 == null || FrameworkElement.DType.IsInstanceOfType(dependencyObject2) || FrameworkContentElement.DType.IsInstanceOfType(dependencyObject2))
				{
					break;
				}
				dependencyObject = dependencyObject2;
			}
			return dependencyObject2;
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00168E50 File Offset: 0x00167E50
		private static PathNode BuildPathForElement(DependencyObject node)
		{
			PathNode pathNode = null;
			while (node != null)
			{
				PathNode pathNode2 = new PathNode(node);
				if (pathNode != null)
				{
					pathNode2.AddChild(pathNode);
				}
				pathNode = pathNode2;
				if (node.ReadLocalValue(AnnotationService.ServiceProperty) != DependencyProperty.UnsetValue)
				{
					break;
				}
				node = PathNode.GetParent(node);
			}
			return pathNode;
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00168E94 File Offset: 0x00167E94
		private static PathNode AddBranchToPath(PathNode path, PathNode branch)
		{
			PathNode pathNode = path;
			PathNode pathNode2 = branch;
			while (pathNode.Node.Equals(pathNode2.Node) && pathNode2._children.Count > 0)
			{
				bool flag = false;
				PathNode pathNode3 = (PathNode)pathNode2._children[0];
				foreach (object obj in pathNode._children)
				{
					PathNode pathNode4 = (PathNode)obj;
					if (pathNode4.Equals(pathNode3))
					{
						flag = true;
						pathNode2 = pathNode3;
						pathNode = pathNode4;
						break;
					}
				}
				if (!flag)
				{
					pathNode.AddChild(pathNode3);
					break;
				}
			}
			return path;
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x00168F4C File Offset: 0x00167F4C
		private void AddChild(object child)
		{
			this._children.Add(child);
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x00168F5C File Offset: 0x00167F5C
		private void FreezeChildren()
		{
			foreach (object obj in this._children)
			{
				((PathNode)obj).FreezeChildren();
			}
			this._children = ArrayList.ReadOnly(this._children);
		}

		// Token: 0x04000E0B RID: 3595
		internal static readonly DependencyProperty HiddenParentProperty = DependencyProperty.RegisterAttached("HiddenParent", typeof(DependencyObject), typeof(PathNode));

		// Token: 0x04000E0C RID: 3596
		private DependencyObject _node;

		// Token: 0x04000E0D RID: 3597
		private ArrayList _children = new ArrayList(1);
	}
}
