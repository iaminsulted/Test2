using System;
using System.Collections.Generic;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200013B RID: 315
	internal class PageContext
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0011E7EE File Offset: 0x0011D7EE
		// (set) Token: 0x0600098C RID: 2444 RVA: 0x0011E7F6 File Offset: 0x0011D7F6
		internal PTS.FSRECT PageRect
		{
			get
			{
				return this._pageRect;
			}
			set
			{
				this._pageRect = value;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0011E7FF File Offset: 0x0011D7FF
		internal List<BaseParaClient> FloatingElementList
		{
			get
			{
				return this._floatingElementList;
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0011E807 File Offset: 0x0011D807
		internal void AddFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList == null)
			{
				this._floatingElementList = new List<BaseParaClient>();
			}
			if (!this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Add(floatingElement);
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0011E836 File Offset: 0x0011D836
		internal void RemoveFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Remove(floatingElement);
			}
			if (this._floatingElementList.Count == 0)
			{
				this._floatingElementList = null;
			}
		}

		// Token: 0x040007E7 RID: 2023
		private List<BaseParaClient> _floatingElementList;

		// Token: 0x040007E8 RID: 2024
		private PTS.FSRECT _pageRect;
	}
}
