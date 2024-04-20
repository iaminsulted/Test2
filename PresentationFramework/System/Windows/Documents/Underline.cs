using System;

namespace System.Windows.Documents
{
	// Token: 0x020006DA RID: 1754
	public class Underline : Span
	{
		// Token: 0x06005C5D RID: 23645 RVA: 0x00286413 File Offset: 0x00285413
		static Underline()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Underline), new FrameworkPropertyMetadata(typeof(Underline)));
		}

		// Token: 0x06005C5E RID: 23646 RVA: 0x0022C8E5 File Offset: 0x0022B8E5
		public Underline()
		{
		}

		// Token: 0x06005C5F RID: 23647 RVA: 0x0022C8ED File Offset: 0x0022B8ED
		public Underline(Inline childInline) : base(childInline)
		{
		}

		// Token: 0x06005C60 RID: 23648 RVA: 0x0022C8F6 File Offset: 0x0022B8F6
		public Underline(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		// Token: 0x06005C61 RID: 23649 RVA: 0x0022C900 File Offset: 0x0022B900
		public Underline(TextPointer start, TextPointer end) : base(start, end)
		{
		}
	}
}
