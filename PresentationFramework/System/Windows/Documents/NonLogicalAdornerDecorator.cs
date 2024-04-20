using System;

namespace System.Windows.Documents
{
	// Token: 0x020005D6 RID: 1494
	internal class NonLogicalAdornerDecorator : AdornerDecorator
	{
		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06004813 RID: 18451 RVA: 0x0022AFD2 File Offset: 0x00229FD2
		// (set) Token: 0x06004814 RID: 18452 RVA: 0x0022AFDC File Offset: 0x00229FDC
		public override UIElement Child
		{
			get
			{
				return base.IntChild;
			}
			set
			{
				if (base.IntChild != value)
				{
					base.RemoveVisualChild(base.IntChild);
					base.RemoveVisualChild(base.AdornerLayer);
					base.IntChild = value;
					if (value != null)
					{
						base.AddVisualChild(value);
						base.AddVisualChild(base.AdornerLayer);
					}
					base.InvalidateMeasure();
				}
			}
		}
	}
}
