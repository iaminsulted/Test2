using System;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x0200063D RID: 1597
	[TrimSurroundingWhitespace]
	public class LineBreak : Inline
	{
		// Token: 0x06004F10 RID: 20240 RVA: 0x00243005 File Offset: 0x00242005
		public LineBreak()
		{
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x002434BC File Offset: 0x002424BC
		public LineBreak(TextPointer insertionPosition)
		{
			if (insertionPosition != null)
			{
				insertionPosition.TextContainer.BeginChange();
			}
			try
			{
				if (insertionPosition != null)
				{
					insertionPosition.InsertInline(this);
				}
			}
			finally
			{
				if (insertionPosition != null)
				{
					insertionPosition.TextContainer.EndChange();
				}
			}
		}
	}
}
