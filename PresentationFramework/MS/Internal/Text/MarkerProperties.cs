using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000324 RID: 804
	internal sealed class MarkerProperties
	{
		// Token: 0x06001DFB RID: 7675 RVA: 0x0016EC08 File Offset: 0x0016DC08
		internal MarkerProperties(List list, int index)
		{
			this._offset = list.MarkerOffset;
			if (double.IsNaN(this._offset))
			{
				double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(list);
				this._offset = -0.5 * lineHeightValue;
			}
			else
			{
				this._offset = -this._offset;
			}
			this._style = list.MarkerStyle;
			this._index = index;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0016EC6E File Offset: 0x0016DC6E
		internal TextMarkerProperties GetTextMarkerProperties(TextParagraphProperties textParaProps)
		{
			return new TextSimpleMarkerProperties(this._style, this._offset, this._index, textParaProps);
		}

		// Token: 0x04000EDF RID: 3807
		private TextMarkerStyle _style;

		// Token: 0x04000EE0 RID: 3808
		private double _offset;

		// Token: 0x04000EE1 RID: 3809
		private int _index;
	}
}
