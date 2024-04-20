using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004F2 RID: 1266
	[DebuggerDisplay("Attr:{_value}")]
	internal class XamlAttributeNode : XamlNode
	{
		// Token: 0x06003FD8 RID: 16344 RVA: 0x00212745 File Offset: 0x00211745
		internal XamlAttributeNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth, string value) : base(tokenType, lineNumber, linePosition, depth)
		{
			this._value = value;
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0021275A File Offset: 0x0021175A
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040023DA RID: 9178
		private string _value;
	}
}
