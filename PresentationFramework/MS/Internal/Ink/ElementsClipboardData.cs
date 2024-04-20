using System;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x02000184 RID: 388
	internal abstract class ElementsClipboardData : ClipboardData
	{
		// Token: 0x06000CD2 RID: 3282 RVA: 0x001316C6 File Offset: 0x001306C6
		internal ElementsClipboardData()
		{
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x001316CE File Offset: 0x001306CE
		internal ElementsClipboardData(UIElement[] elements)
		{
			if (elements != null)
			{
				this.ElementList = new List<UIElement>(elements);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000CD4 RID: 3284 RVA: 0x001316E5 File Offset: 0x001306E5
		internal List<UIElement> Elements
		{
			get
			{
				if (this.ElementList != null)
				{
					return this._elementList;
				}
				return new List<UIElement>();
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x001316FB File Offset: 0x001306FB
		// (set) Token: 0x06000CD6 RID: 3286 RVA: 0x00131703 File Offset: 0x00130703
		protected List<UIElement> ElementList
		{
			get
			{
				return this._elementList;
			}
			set
			{
				this._elementList = value;
			}
		}

		// Token: 0x040009A4 RID: 2468
		private List<UIElement> _elementList;
	}
}
