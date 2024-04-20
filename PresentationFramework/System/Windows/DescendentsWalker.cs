using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace System.Windows
{
	// Token: 0x02000357 RID: 855
	internal class DescendentsWalker<T> : DescendentsWalkerBase
	{
		// Token: 0x06002060 RID: 8288 RVA: 0x00175238 File Offset: 0x00174238
		public DescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> callback) : this(priority, callback, default(T))
		{
		}

		// Token: 0x06002061 RID: 8289 RVA: 0x00175256 File Offset: 0x00174256
		public DescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> callback, T data) : base(priority)
		{
			this._callback = callback;
			this._data = data;
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x0017526D File Offset: 0x0017426D
		public void StartWalk(DependencyObject startNode)
		{
			this.StartWalk(startNode, false);
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x00175278 File Offset: 0x00174278
		public virtual void StartWalk(DependencyObject startNode, bool skipStartNode)
		{
			this._startNode = startNode;
			bool flag = true;
			if (!skipStartNode && (FrameworkElement.DType.IsInstanceOfType(this._startNode) || FrameworkContentElement.DType.IsInstanceOfType(this._startNode)))
			{
				flag = this._callback(this._startNode, this._data, this._priority == TreeWalkPriority.VisualTree);
			}
			if (flag)
			{
				this.IterateChildren(this._startNode);
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x001752E8 File Offset: 0x001742E8
		private void IterateChildren(DependencyObject d)
		{
			this._recursionDepth++;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				FrameworkElement frameworkElement = (FrameworkElement)d;
				bool hasLogicalChildren = frameworkElement.HasLogicalChildren;
				if (this._priority == TreeWalkPriority.VisualTree)
				{
					this.WalkFrameworkElementVisualThenLogicalChildren(frameworkElement, hasLogicalChildren);
				}
				else if (this._priority == TreeWalkPriority.LogicalTree)
				{
					this.WalkFrameworkElementLogicalThenVisualChildren(frameworkElement, hasLogicalChildren);
				}
			}
			else if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				FrameworkContentElement frameworkContentElement = d as FrameworkContentElement;
				if (frameworkContentElement.HasLogicalChildren)
				{
					this.WalkLogicalChildren(null, frameworkContentElement, frameworkContentElement.LogicalChildren);
				}
			}
			else
			{
				Visual visual = d as Visual;
				if (visual != null)
				{
					this.WalkVisualChildren(visual);
				}
				else
				{
					Visual3D visual3D = d as Visual3D;
					if (visual3D != null)
					{
						this.WalkVisualChildren(visual3D);
					}
				}
			}
			this._recursionDepth--;
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x001753A4 File Offset: 0x001743A4
		private void WalkVisualChildren(Visual v)
		{
			v.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisual2DOr3DChildrenCount = v.InternalVisual2DOr3DChildrenCount;
				for (int i = 0; i < internalVisual2DOr3DChildrenCount; i++)
				{
					DependencyObject dependencyObject = v.InternalGet2DOr3DVisualChild(i);
					if (dependencyObject != null)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(dependencyObject, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				v.IsVisualChildrenIterationInProgress = false;
			}
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x001753FC File Offset: 0x001743FC
		private void WalkVisualChildren(Visual3D v)
		{
			v.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisual2DOr3DChildrenCount = v.InternalVisual2DOr3DChildrenCount;
				for (int i = 0; i < internalVisual2DOr3DChildrenCount; i++)
				{
					DependencyObject dependencyObject = v.InternalGet2DOr3DVisualChild(i);
					if (dependencyObject != null)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(dependencyObject, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				v.IsVisualChildrenIterationInProgress = false;
			}
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00175454 File Offset: 0x00174454
		private void WalkLogicalChildren(FrameworkElement feParent, FrameworkContentElement fceParent, IEnumerator logicalChildren)
		{
			if (feParent != null)
			{
				feParent.IsLogicalChildrenIterationInProgress = true;
			}
			else
			{
				fceParent.IsLogicalChildrenIterationInProgress = true;
			}
			try
			{
				if (logicalChildren != null)
				{
					while (logicalChildren.MoveNext())
					{
						object obj = logicalChildren.Current;
						DependencyObject dependencyObject = obj as DependencyObject;
						if (dependencyObject != null)
						{
							bool visitedViaVisualTree = false;
							this.VisitNode(dependencyObject, visitedViaVisualTree);
						}
					}
				}
			}
			finally
			{
				if (feParent != null)
				{
					feParent.IsLogicalChildrenIterationInProgress = false;
				}
				else
				{
					fceParent.IsLogicalChildrenIterationInProgress = false;
				}
			}
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x001754C4 File Offset: 0x001744C4
		private void WalkFrameworkElementVisualThenLogicalChildren(FrameworkElement feParent, bool hasLogicalChildren)
		{
			this.WalkVisualChildren(feParent);
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(feParent);
			if (value != null)
			{
				foreach (Popup fe in value)
				{
					bool visitedViaVisualTree = false;
					this.VisitNode(fe, visitedViaVisualTree);
				}
			}
			feParent.IsLogicalChildrenIterationInProgress = true;
			try
			{
				if (hasLogicalChildren)
				{
					IEnumerator logicalChildren = feParent.LogicalChildren;
					if (logicalChildren != null)
					{
						while (logicalChildren.MoveNext())
						{
							object obj = logicalChildren.Current;
							FrameworkElement frameworkElement = obj as FrameworkElement;
							if (frameworkElement != null)
							{
								if (VisualTreeHelper.GetParent(frameworkElement) != frameworkElement.Parent)
								{
									bool visitedViaVisualTree2 = false;
									this.VisitNode(frameworkElement, visitedViaVisualTree2);
								}
							}
							else
							{
								FrameworkContentElement frameworkContentElement = obj as FrameworkContentElement;
								if (frameworkContentElement != null)
								{
									bool visitedViaVisualTree3 = false;
									this.VisitNode(frameworkContentElement, visitedViaVisualTree3);
								}
							}
						}
					}
				}
			}
			finally
			{
				feParent.IsLogicalChildrenIterationInProgress = false;
			}
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x001755B4 File Offset: 0x001745B4
		private void WalkFrameworkElementLogicalThenVisualChildren(FrameworkElement feParent, bool hasLogicalChildren)
		{
			if (hasLogicalChildren)
			{
				this.WalkLogicalChildren(feParent, null, feParent.LogicalChildren);
			}
			feParent.IsVisualChildrenIterationInProgress = true;
			try
			{
				int internalVisualChildrenCount = feParent.InternalVisualChildrenCount;
				for (int i = 0; i < internalVisualChildrenCount; i++)
				{
					Visual visual = feParent.InternalGetVisualChild(i);
					if (visual != null && FrameworkElement.DType.IsInstanceOfType(visual) && VisualTreeHelper.GetParent(visual) != ((FrameworkElement)visual).Parent)
					{
						bool visitedViaVisualTree = true;
						this.VisitNode(visual, visitedViaVisualTree);
					}
				}
			}
			finally
			{
				feParent.IsVisualChildrenIterationInProgress = false;
			}
			List<Popup> value = Popup.RegisteredPopupsField.GetValue(feParent);
			if (value != null)
			{
				foreach (Popup fe in value)
				{
					bool visitedViaVisualTree2 = false;
					this.VisitNode(fe, visitedViaVisualTree2);
				}
			}
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x00175694 File Offset: 0x00174694
		private void VisitNode(FrameworkElement fe, bool visitedViaVisualTree)
		{
			if (this._recursionDepth > ContextLayoutManager.s_LayoutRecursionLimit)
			{
				throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
			}
			int num = this._nodes.IndexOf(fe);
			if (num != -1)
			{
				this._nodes.RemoveAt(num);
				return;
			}
			DependencyObject parent = VisualTreeHelper.GetParent(fe);
			DependencyObject parent2 = fe.Parent;
			if (parent != null && parent2 != null && parent != parent2)
			{
				this._nodes.Add(fe);
			}
			this._VisitNode(fe, visitedViaVisualTree);
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x0017570C File Offset: 0x0017470C
		private void VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			if (this._recursionDepth > ContextLayoutManager.s_LayoutRecursionLimit)
			{
				throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
			}
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this.VisitNode(d as FrameworkElement, visitedViaVisualTree);
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._VisitNode(d, visitedViaVisualTree);
				return;
			}
			this.IterateChildren(d);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x0017576E File Offset: 0x0017476E
		protected virtual void _VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			if (this._callback(d, this._data, visitedViaVisualTree))
			{
				this.IterateChildren(d);
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600206D RID: 8301 RVA: 0x0017578C File Offset: 0x0017478C
		protected T Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x04000FDB RID: 4059
		private VisitedCallback<T> _callback;

		// Token: 0x04000FDC RID: 4060
		private T _data;
	}
}
