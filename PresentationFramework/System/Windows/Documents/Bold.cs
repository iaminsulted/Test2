using System;

namespace System.Windows.Documents
{
	// Token: 0x020005DD RID: 1501
	public class Bold : Span
	{
		// Token: 0x06004885 RID: 18565 RVA: 0x0022C8C0 File Offset: 0x0022B8C0
		static Bold()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Bold), new FrameworkPropertyMetadata(typeof(Bold)));
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x0022C8E5 File Offset: 0x0022B8E5
		public Bold()
		{
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x0022C8ED File Offset: 0x0022B8ED
		public Bold(Inline childInline) : base(childInline)
		{
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x0022C8F6 File Offset: 0x0022B8F6
		public Bold(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x0022C900 File Offset: 0x0022B900
		public Bold(TextPointer start, TextPointer end) : base(start, end)
		{
		}
	}
}
