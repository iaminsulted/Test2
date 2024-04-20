using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.Ink
{
	// Token: 0x02000194 RID: 404
	internal class XamlClipboardData : ElementsClipboardData
	{
		// Token: 0x06000D77 RID: 3447 RVA: 0x001353D8 File Offset: 0x001343D8
		internal XamlClipboardData()
		{
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x001353E0 File Offset: 0x001343E0
		internal XamlClipboardData(UIElement[] elements) : base(elements)
		{
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x001353E9 File Offset: 0x001343E9
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(DataFormats.Xaml, false);
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x001353F7 File Offset: 0x001343F7
		protected override bool CanCopy()
		{
			return base.Elements != null && base.Elements.Count != 0;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00135414 File Offset: 0x00134414
		protected override void DoCopy(IDataObject dataObject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UIElement obj in base.Elements)
			{
				string value = XamlWriter.Save(obj);
				stringBuilder.Append(value);
			}
			dataObject.SetData(DataFormats.Xaml, stringBuilder.ToString());
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00135484 File Offset: 0x00134484
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.Xaml) as string;
			if (!string.IsNullOrEmpty(text))
			{
				UIElement uielement = XamlReader.Load(new XmlTextReader(new StringReader(text)), true) as UIElement;
				if (uielement != null)
				{
					base.ElementList.Add(uielement);
				}
			}
		}
	}
}
