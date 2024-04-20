using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020005D5 RID: 1493
	public abstract class Adorner : FrameworkElement
	{
		// Token: 0x06004806 RID: 18438 RVA: 0x0022AE82 File Offset: 0x00229E82
		protected Adorner(UIElement adornedElement)
		{
			if (adornedElement == null)
			{
				throw new ArgumentNullException("adornedElement");
			}
			this._adornedElement = adornedElement;
			this._isClipEnabled = false;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Normal, new DispatcherOperationCallback(Adorner.CreateFlowDirectionBinding), this);
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0022AEC0 File Offset: 0x00229EC0
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = new Size(this.AdornedElement.RenderSize.Width, this.AdornedElement.RenderSize.Height);
			int visualChildrenCount = this.VisualChildrenCount;
			for (int i = 0; i < visualChildrenCount; i++)
			{
				UIElement uielement = this.GetVisualChild(i) as UIElement;
				if (uielement != null)
				{
					uielement.Measure(size);
				}
			}
			return size;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00109403 File Offset: 0x00108403
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return null;
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x001136C4 File Offset: 0x001126C4
		public virtual GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			return transform;
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x0600480A RID: 18442 RVA: 0x0022AF28 File Offset: 0x00229F28
		// (set) Token: 0x0600480B RID: 18443 RVA: 0x0022AF30 File Offset: 0x00229F30
		internal Geometry AdornerClip
		{
			get
			{
				return base.Clip;
			}
			set
			{
				base.Clip = value;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x0600480C RID: 18444 RVA: 0x0022AF39 File Offset: 0x00229F39
		// (set) Token: 0x0600480D RID: 18445 RVA: 0x0022AF41 File Offset: 0x00229F41
		internal Transform AdornerTransform
		{
			get
			{
				return base.RenderTransform;
			}
			set
			{
				base.RenderTransform = value;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x0600480E RID: 18446 RVA: 0x0022AF4A File Offset: 0x00229F4A
		public UIElement AdornedElement
		{
			get
			{
				return this._adornedElement;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x0022AF52 File Offset: 0x00229F52
		// (set) Token: 0x06004810 RID: 18448 RVA: 0x0022AF5A File Offset: 0x00229F5A
		public bool IsClipEnabled
		{
			get
			{
				return this._isClipEnabled;
			}
			set
			{
				this._isClipEnabled = value;
				base.InvalidateArrange();
				AdornerLayer.GetAdornerLayer(this._adornedElement).InvalidateArrange();
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0022AF7C File Offset: 0x00229F7C
		private static object CreateFlowDirectionBinding(object o)
		{
			Adorner adorner = (Adorner)o;
			Binding binding = new Binding("FlowDirection");
			binding.Mode = BindingMode.OneWay;
			binding.Source = adorner.AdornedElement;
			adorner.SetBinding(FrameworkElement.FlowDirectionProperty, binding);
			return null;
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0022AFBC File Offset: 0x00229FBC
		internal virtual bool NeedsUpdate(Size oldSize)
		{
			return !DoubleUtil.AreClose(this.AdornedElement.RenderSize, oldSize);
		}

		// Token: 0x040025F5 RID: 9717
		private readonly UIElement _adornedElement;

		// Token: 0x040025F6 RID: 9718
		private bool _isClipEnabled;
	}
}
