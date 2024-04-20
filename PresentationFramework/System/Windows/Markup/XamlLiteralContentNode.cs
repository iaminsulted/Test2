using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004F1 RID: 1265
	[DebuggerDisplay("Cont:{_content}")]
	internal class XamlLiteralContentNode : XamlNode
	{
		// Token: 0x06003FD6 RID: 16342 RVA: 0x00212728 File Offset: 0x00211728
		internal XamlLiteralContentNode(int lineNumber, int linePosition, int depth, string content) : base(XamlNodeType.LiteralContent, lineNumber, linePosition, depth)
		{
			this._content = content;
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x06003FD7 RID: 16343 RVA: 0x0021273D File Offset: 0x0021173D
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x040023D9 RID: 9177
		private string _content;
	}
}
