using System;
using System.Windows.Documents;

namespace System.Windows.Media
{
	// Token: 0x0200042A RID: 1066
	public class AdornerHitTestResult : PointHitTestResult
	{
		// Token: 0x060033E3 RID: 13283 RVA: 0x001D9B1B File Offset: 0x001D8B1B
		internal AdornerHitTestResult(Visual visual, Point pt, Adorner adorner) : base(visual, pt)
		{
			this._adorner = adorner;
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x001D9B2C File Offset: 0x001D8B2C
		public Adorner Adorner
		{
			get
			{
				return this._adorner;
			}
		}

		// Token: 0x04001C42 RID: 7234
		private readonly Adorner _adorner;
	}
}
