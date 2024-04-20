using System;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x02000685 RID: 1669
	internal static class SelectionHighlightInfo
	{
		// Token: 0x060052B5 RID: 21173 RVA: 0x00258EB0 File Offset: 0x00257EB0
		static SelectionHighlightInfo()
		{
			SelectionHighlightInfo._objectMaskBrush.Opacity = 0.5;
			SelectionHighlightInfo._objectMaskBrush.Freeze();
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x060052B6 RID: 21174 RVA: 0x00258EDE File Offset: 0x00257EDE
		internal static Brush ForegroundBrush
		{
			get
			{
				return SystemColors.HighlightTextBrush;
			}
		}

		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x060052B7 RID: 21175 RVA: 0x00258EE5 File Offset: 0x00257EE5
		internal static Brush BackgroundBrush
		{
			get
			{
				return SystemColors.HighlightBrush;
			}
		}

		// Token: 0x17001387 RID: 4999
		// (get) Token: 0x060052B8 RID: 21176 RVA: 0x00258EEC File Offset: 0x00257EEC
		internal static Brush ObjectMaskBrush
		{
			get
			{
				return SelectionHighlightInfo._objectMaskBrush;
			}
		}

		// Token: 0x04002EB6 RID: 11958
		private static readonly Brush _objectMaskBrush = new SolidColorBrush(SystemColors.HighlightColor);
	}
}
