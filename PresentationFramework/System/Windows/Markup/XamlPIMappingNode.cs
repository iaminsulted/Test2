using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x020004F7 RID: 1271
	[DebuggerDisplay("PIMap:{_xmlns}={_clrns};{_assy}")]
	internal class XamlPIMappingNode : XamlNode
	{
		// Token: 0x06003FE5 RID: 16357 RVA: 0x0021281A File Offset: 0x0021181A
		internal XamlPIMappingNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string clrNamespace, string assemblyName) : base(XamlNodeType.PIMapping, lineNumber, linePosition, depth)
		{
			this._xmlns = xmlNamespace;
			this._clrns = clrNamespace;
			this._assy = assemblyName;
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06003FE6 RID: 16358 RVA: 0x0021283F File Offset: 0x0021183F
		internal string XmlNamespace
		{
			get
			{
				return this._xmlns;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x06003FE7 RID: 16359 RVA: 0x00212847 File Offset: 0x00211847
		internal string ClrNamespace
		{
			get
			{
				return this._clrns;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x06003FE8 RID: 16360 RVA: 0x0021284F File Offset: 0x0021184F
		internal string AssemblyName
		{
			get
			{
				return this._assy;
			}
		}

		// Token: 0x040023E4 RID: 9188
		private string _xmlns;

		// Token: 0x040023E5 RID: 9189
		private string _clrns;

		// Token: 0x040023E6 RID: 9190
		private string _assy;
	}
}
