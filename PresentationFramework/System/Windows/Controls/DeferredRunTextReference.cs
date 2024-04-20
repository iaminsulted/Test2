using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000772 RID: 1906
	internal class DeferredRunTextReference : DeferredReference
	{
		// Token: 0x060067AE RID: 26542 RVA: 0x002B5E61 File Offset: 0x002B4E61
		internal DeferredRunTextReference(Run run)
		{
			this._run = run;
		}

		// Token: 0x060067AF RID: 26543 RVA: 0x002B5E70 File Offset: 0x002B4E70
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return TextRangeBase.GetTextInternal(this._run.ContentStart, this._run.ContentEnd);
		}

		// Token: 0x060067B0 RID: 26544 RVA: 0x00219406 File Offset: 0x00218406
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04003450 RID: 13392
		private readonly Run _run;
	}
}
