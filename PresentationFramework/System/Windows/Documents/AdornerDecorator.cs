using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020005D7 RID: 1495
	public class AdornerDecorator : Decorator
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x0022B035 File Offset: 0x0022A035
		public AdornerDecorator()
		{
			this._adornerLayer = new AdornerLayer();
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06004817 RID: 18455 RVA: 0x0022B048 File Offset: 0x0022A048
		public AdornerLayer AdornerLayer
		{
			get
			{
				return this._adornerLayer;
			}
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0022B050 File Offset: 0x0022A050
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);
			if (VisualTreeHelper.GetParent(this._adornerLayer) != null)
			{
				this._adornerLayer.Measure(constraint);
			}
			return result;
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0022B072 File Offset: 0x0022A072
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			if (VisualTreeHelper.GetParent(this._adornerLayer) != null)
			{
				this._adornerLayer.Arrange(new Rect(finalSize));
			}
			return result;
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x0600481A RID: 18458 RVA: 0x0022B099 File Offset: 0x0022A099
		// (set) Token: 0x0600481B RID: 18459 RVA: 0x0022B0A4 File Offset: 0x0022A0A4
		public override UIElement Child
		{
			get
			{
				return base.Child;
			}
			set
			{
				Visual child = base.Child;
				if (child == value)
				{
					return;
				}
				if (value == null)
				{
					base.Child = null;
					base.RemoveVisualChild(this._adornerLayer);
					return;
				}
				base.Child = value;
				if (child == null)
				{
					base.AddVisualChild(this._adornerLayer);
				}
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x0600481C RID: 18460 RVA: 0x0022B0EA File Offset: 0x0022A0EA
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.Child != null)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x0022B0F8 File Offset: 0x0022A0F8
		protected override Visual GetVisualChild(int index)
		{
			if (base.Child == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.Child;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._adornerLayer;
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x0600481E RID: 18462 RVA: 0x001FC2AC File Offset: 0x001FB2AC
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x040025F7 RID: 9719
		private readonly AdornerLayer _adornerLayer;
	}
}
