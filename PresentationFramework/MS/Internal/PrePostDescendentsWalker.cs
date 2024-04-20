using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020000FB RID: 251
	internal class PrePostDescendentsWalker<T> : DescendentsWalker<T>
	{
		// Token: 0x060005FB RID: 1531 RVA: 0x00105735 File Offset: 0x00104735
		public PrePostDescendentsWalker(TreeWalkPriority priority, VisitedCallback<T> preCallback, VisitedCallback<T> postCallback, T data) : base(priority, preCallback, data)
		{
			this._postCallback = postCallback;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00105748 File Offset: 0x00104748
		public override void StartWalk(DependencyObject startNode, bool skipStartNode)
		{
			try
			{
				base.StartWalk(startNode, skipStartNode);
			}
			finally
			{
				if (!skipStartNode && this._postCallback != null && (FrameworkElement.DType.IsInstanceOfType(startNode) || FrameworkContentElement.DType.IsInstanceOfType(startNode)))
				{
					this._postCallback(startNode, base.Data, this._priority == TreeWalkPriority.VisualTree);
				}
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x001057B4 File Offset: 0x001047B4
		protected override void _VisitNode(DependencyObject d, bool visitedViaVisualTree)
		{
			try
			{
				base._VisitNode(d, visitedViaVisualTree);
			}
			finally
			{
				if (this._postCallback != null)
				{
					this._postCallback(d, base.Data, visitedViaVisualTree);
				}
			}
		}

		// Token: 0x040006DD RID: 1757
		private VisitedCallback<T> _postCallback;
	}
}
