using System;

namespace System.Windows.Documents
{
	// Token: 0x0200062C RID: 1580
	public class Italic : Span
	{
		// Token: 0x06004E1D RID: 19997 RVA: 0x00243388 File Offset: 0x00242388
		static Italic()
		{
			FrameworkContentElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Italic), new FrameworkPropertyMetadata(typeof(Italic)));
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0022C8E5 File Offset: 0x0022B8E5
		public Italic()
		{
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0022C8ED File Offset: 0x0022B8ED
		public Italic(Inline childInline) : base(childInline)
		{
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0022C8F6 File Offset: 0x0022B8F6
		public Italic(Inline childInline, TextPointer insertionPosition) : base(childInline, insertionPosition)
		{
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0022C900 File Offset: 0x0022B900
		public Italic(TextPointer start, TextPointer end) : base(start, end)
		{
		}
	}
}
