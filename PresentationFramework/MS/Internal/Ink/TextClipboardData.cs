using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x02000193 RID: 403
	internal class TextClipboardData : ElementsClipboardData
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x00135303 File Offset: 0x00134303
		internal TextClipboardData() : this(null)
		{
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0013530C File Offset: 0x0013430C
		internal TextClipboardData(string text)
		{
			this._text = text;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0013531B File Offset: 0x0013431B
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(DataFormats.UnicodeText, false) || dataObject.GetDataPresent(DataFormats.Text, false) || dataObject.GetDataPresent(DataFormats.OemText, false);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00135347 File Offset: 0x00134347
		protected override bool CanCopy()
		{
			return !string.IsNullOrEmpty(this._text);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00135357 File Offset: 0x00134357
		protected override void DoCopy(IDataObject dataObject)
		{
			dataObject.SetData(DataFormats.UnicodeText, this._text, true);
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0013536C File Offset: 0x0013436C
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.UnicodeText, true) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = (dataObject.GetData(DataFormats.Text, true) as string);
			}
			if (!string.IsNullOrEmpty(text))
			{
				TextBox textBox = new TextBox();
				textBox.Text = text;
				textBox.TextWrapping = TextWrapping.Wrap;
				base.ElementList.Add(textBox);
			}
		}

		// Token: 0x040009DF RID: 2527
		private string _text;
	}
}
