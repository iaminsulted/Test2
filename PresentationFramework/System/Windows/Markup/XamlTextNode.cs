using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004E5 RID: 1253
	[DebuggerDisplay("Text:{_text}")]
	internal class XamlTextNode : XamlNode
	{
		// Token: 0x06003F93 RID: 16275 RVA: 0x00212250 File Offset: 0x00211250
		internal XamlTextNode(int lineNumber, int linePosition, int depth, string textContent, Type converterType) : base(XamlNodeType.Text, lineNumber, linePosition, depth)
		{
			this._text = textContent;
			this._converterType = converterType;
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003F94 RID: 16276 RVA: 0x0021226D File Offset: 0x0021126D
		internal string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06003F95 RID: 16277 RVA: 0x00212275 File Offset: 0x00211275
		internal Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0021227D File Offset: 0x0021127D
		internal void UpdateText(string text)
		{
			this._text = text;
		}

		// Token: 0x040023B0 RID: 9136
		private string _text;

		// Token: 0x040023B1 RID: 9137
		private Type _converterType;
	}
}
