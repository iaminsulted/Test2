using System;

namespace System.Windows.Documents
{
	// Token: 0x02000610 RID: 1552
	internal abstract class FixedSOMPageElement : FixedSOMContainer
	{
		// Token: 0x06004B8E RID: 19342 RVA: 0x002389BA File Offset: 0x002379BA
		public FixedSOMPageElement(FixedSOMPage page)
		{
			this._page = page;
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x06004B8F RID: 19343 RVA: 0x002389C9 File Offset: 0x002379C9
		public FixedSOMPage FixedSOMPage
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x06004B90 RID: 19344
		public abstract bool IsRTL { get; }

		// Token: 0x04002797 RID: 10135
		protected FixedSOMPage _page;
	}
}
