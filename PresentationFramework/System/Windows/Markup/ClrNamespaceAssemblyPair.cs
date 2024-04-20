using System;

namespace System.Windows.Markup
{
	// Token: 0x02000522 RID: 1314
	internal struct ClrNamespaceAssemblyPair
	{
		// Token: 0x0600414E RID: 16718 RVA: 0x00217AFE File Offset: 0x00216AFE
		internal ClrNamespaceAssemblyPair(string clrNamespace, string assemblyName)
		{
			this._clrNamespace = clrNamespace;
			this._assemblyName = assemblyName;
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x00217B0E File Offset: 0x00216B0E
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06004150 RID: 16720 RVA: 0x00217B16 File Offset: 0x00216B16
		internal string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
		}

		// Token: 0x040024BC RID: 9404
		private string _assemblyName;

		// Token: 0x040024BD RID: 9405
		private string _clrNamespace;
	}
}
