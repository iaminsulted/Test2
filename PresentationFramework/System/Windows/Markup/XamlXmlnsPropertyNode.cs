using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004F6 RID: 1270
	[DebuggerDisplay("Xmlns:{_prefix)={_xmlNamespace}")]
	internal class XamlXmlnsPropertyNode : XamlNode
	{
		// Token: 0x06003FE2 RID: 16354 RVA: 0x002127ED File Offset: 0x002117ED
		internal XamlXmlnsPropertyNode(int lineNumber, int linePosition, int depth, string prefix, string xmlNamespace) : base(XamlNodeType.XmlnsProperty, lineNumber, linePosition, depth)
		{
			this._prefix = prefix;
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x0021280A File Offset: 0x0021180A
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06003FE4 RID: 16356 RVA: 0x00212812 File Offset: 0x00211812
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x040023E2 RID: 9186
		private string _prefix;

		// Token: 0x040023E3 RID: 9187
		private string _xmlNamespace;
	}
}
