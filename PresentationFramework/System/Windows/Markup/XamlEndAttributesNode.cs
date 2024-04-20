using System;

namespace System.Windows.Markup
{
	// Token: 0x020004FF RID: 1279
	internal class XamlEndAttributesNode : XamlNode
	{
		// Token: 0x06003FF0 RID: 16368 RVA: 0x002128EF File Offset: 0x002118EF
		internal XamlEndAttributesNode(int lineNumber, int linePosition, int depth, bool compact) : base(XamlNodeType.EndAttributes, lineNumber, linePosition, depth)
		{
			this._compact = compact;
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00212904 File Offset: 0x00211904
		internal bool IsCompact
		{
			get
			{
				return this._compact;
			}
		}

		// Token: 0x040023E7 RID: 9191
		private bool _compact;
	}
}
