using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001F6 RID: 502
	internal class UIElementIsland : ContainerVisual, IContentHost, IDisposable
	{
		// Token: 0x06001276 RID: 4726 RVA: 0x0014AA28 File Offset: 0x00149A28
		internal UIElementIsland(UIElement child)
		{
			base.SetFlags(true, VisualFlags.IsLayoutIslandRoot);
			this._child = child;
			if (this._child != null)
			{
				Visual visual = VisualTreeHelper.GetParent(this._child) as Visual;
				if (visual != null)
				{
					Invariant.Assert(visual is UIElementIsland, "Parent should always be a UIElementIsland.");
					((UIElementIsland)visual).Dispose();
				}
				base.Children.Add(this._child);
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0014AA9C File Offset: 0x00149A9C
		internal Size DoLayout(Size availableSize, bool horizontalAutoSize, bool verticalAutoSize)
		{
			Size size = default(Size);
			if (this._child != null)
			{
				if (this._child is FrameworkElement && ((FrameworkElement)this._child).Parent != null)
				{
					base.SetValue(FrameworkElement.FlowDirectionProperty, ((FrameworkElement)this._child).Parent.GetValue(FrameworkElement.FlowDirectionProperty));
				}
				try
				{
					this._layoutInProgress = true;
					this._child.Measure(availableSize);
					size.Width = (horizontalAutoSize ? this._child.DesiredSize.Width : availableSize.Width);
					size.Height = (verticalAutoSize ? this._child.DesiredSize.Height : availableSize.Height);
					this._child.Arrange(new Rect(size));
				}
				finally
				{
					this._layoutInProgress = false;
				}
			}
			return size;
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001278 RID: 4728 RVA: 0x0014AB8C File Offset: 0x00149B8C
		internal UIElement Root
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001279 RID: 4729 RVA: 0x0014AB94 File Offset: 0x00149B94
		// (remove) Token: 0x0600127A RID: 4730 RVA: 0x0014ABCC File Offset: 0x00149BCC
		internal event DesiredSizeChangedEventHandler DesiredSizeChanged;

		// Token: 0x0600127B RID: 4731 RVA: 0x0014AC01 File Offset: 0x00149C01
		public void Dispose()
		{
			if (this._child != null)
			{
				base.Children.Clear();
				this._child = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x00109403 File Offset: 0x00108403
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return null;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0014AC23 File Offset: 0x00149C23
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return new ReadOnlyCollection<Rect>(new List<Rect>());
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x0014AC30 File Offset: 0x00149C30
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				List<IInputElement> list = new List<IInputElement>();
				if (this._child != null)
				{
					list.Add(this._child);
				}
				return list.GetEnumerator();
			}
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x0014AC62 File Offset: 0x00149C62
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			Invariant.Assert(child == this._child);
			if (!this._layoutInProgress && this.DesiredSizeChanged != null)
			{
				this.DesiredSizeChanged(this, new DesiredSizeChangedEventArgs(child));
			}
		}

		// Token: 0x04000B2E RID: 2862
		private UIElement _child;

		// Token: 0x04000B2F RID: 2863
		private bool _layoutInProgress;
	}
}
