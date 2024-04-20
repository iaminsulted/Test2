using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000774 RID: 1908
	internal class DeferredTextReference : DeferredReference
	{
		// Token: 0x060067B4 RID: 26548 RVA: 0x002B5EBA File Offset: 0x002B4EBA
		internal DeferredTextReference(ITextContainer textContainer)
		{
			this._textContainer = textContainer;
		}

		// Token: 0x060067B5 RID: 26549 RVA: 0x002B5ECC File Offset: 0x002B4ECC
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			string textInternal = TextRangeBase.GetTextInternal(this._textContainer.Start, this._textContainer.End);
			TextBox textBox = this._textContainer.Parent as TextBox;
			if (textBox != null)
			{
				textBox.OnDeferredTextReferenceResolved(this, textInternal);
			}
			return textInternal;
		}

		// Token: 0x060067B6 RID: 26550 RVA: 0x00219406 File Offset: 0x00218406
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04003452 RID: 13394
		private readonly ITextContainer _textContainer;
	}
}
