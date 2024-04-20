using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000133 RID: 307
	internal class PageVisual : DrawingVisual, IContentHost
	{
		// Token: 0x06000867 RID: 2151 RVA: 0x00114F02 File Offset: 0x00113F02
		internal PageVisual(FlowDocumentPage owner)
		{
			this._owner = new WeakReference(owner);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00114F18 File Offset: 0x00113F18
		internal void DrawBackground(Brush backgroundBrush, Rect renderBounds)
		{
			if (this._backgroundBrush != backgroundBrush || this._renderBounds != renderBounds)
			{
				this._backgroundBrush = backgroundBrush;
				this._renderBounds = renderBounds;
				using (DrawingContext drawingContext = base.RenderOpen())
				{
					if (this._backgroundBrush != null)
					{
						drawingContext.DrawRectangle(this._backgroundBrush, null, this._renderBounds);
					}
					else
					{
						drawingContext.DrawRectangle(Brushes.Transparent, null, this._renderBounds);
					}
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x00114F9C File Offset: 0x00113F9C
		// (set) Token: 0x0600086A RID: 2154 RVA: 0x00114FC4 File Offset: 0x00113FC4
		internal Visual Child
		{
			get
			{
				VisualCollection children = base.Children;
				if (children.Count != 0)
				{
					return children[0];
				}
				return null;
			}
			set
			{
				VisualCollection children = base.Children;
				if (children.Count == 0)
				{
					children.Add(value);
					return;
				}
				if (children[0] != value)
				{
					children[0] = value;
				}
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00114FFC File Offset: 0x00113FFC
		internal void ClearDrawingContext()
		{
			DrawingContext drawingContext = base.RenderOpen();
			if (drawingContext != null)
			{
				drawingContext.Close();
			}
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0011501C File Offset: 0x0011401C
		IInputElement IContentHost.InputHitTest(Point point)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.InputHitTest(point);
			}
			return null;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00115048 File Offset: 0x00114048
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				return contentHost.GetRectangles(child);
			}
			return new ReadOnlyCollection<Rect>(new List<Rect>(0));
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x0011507C File Offset: 0x0011407C
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				IContentHost contentHost = this._owner.Target as IContentHost;
				if (contentHost != null)
				{
					return contentHost.HostedElements;
				}
				return null;
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x001150A8 File Offset: 0x001140A8
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			IContentHost contentHost = this._owner.Target as IContentHost;
			if (contentHost != null)
			{
				contentHost.OnChildDesiredSizeChanged(child);
			}
		}

		// Token: 0x040007C1 RID: 1985
		private readonly WeakReference _owner;

		// Token: 0x040007C2 RID: 1986
		private Brush _backgroundBrush;

		// Token: 0x040007C3 RID: 1987
		private Rect _renderBounds;
	}
}
