using System;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020001BD RID: 445
	internal class DocumentPageHost : FrameworkElement
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x0013C3BF File Offset: 0x0013B3BF
		internal DocumentPageHost()
		{
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x0013C3C8 File Offset: 0x0013B3C8
		internal static void DisconnectPageVisual(Visual pageVisual)
		{
			Visual visual = VisualTreeHelper.GetParent(pageVisual) as Visual;
			if (visual != null)
			{
				ContainerVisual containerVisual = visual as ContainerVisual;
				if (containerVisual == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				DocumentPageHost documentPageHost = VisualTreeHelper.GetParent(containerVisual) as DocumentPageHost;
				if (documentPageHost == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				documentPageHost.PageVisual = null;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0013C42A File Offset: 0x0013B42A
		// (set) Token: 0x06000F13 RID: 3859 RVA: 0x0013C434 File Offset: 0x0013B434
		internal Visual PageVisual
		{
			get
			{
				return this._pageVisual;
			}
			set
			{
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = VisualTreeHelper.GetParent(this._pageVisual) as ContainerVisual;
					Invariant.Assert(containerVisual != null);
					containerVisual.Children.Clear();
					base.RemoveVisualChild(containerVisual);
				}
				this._pageVisual = value;
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = new ContainerVisual();
					base.AddVisualChild(containerVisual);
					containerVisual.Children.Add(this._pageVisual);
					containerVisual.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
				}
			}
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x0013C4B4 File Offset: 0x0013B4B4
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._pageVisual == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return VisualTreeHelper.GetParent(this._pageVisual) as Visual;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0013C4EC File Offset: 0x0013B4EC
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageVisual == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x04000A8B RID: 2699
		internal Point CachedOffset;

		// Token: 0x04000A8C RID: 2700
		private Visual _pageVisual;
	}
}
