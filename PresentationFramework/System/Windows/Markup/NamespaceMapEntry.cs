using System;
using System.Diagnostics;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000519 RID: 1305
	[DebuggerDisplay("'{_xmlNamespace}'={_clrNamespace}:{_assemblyName}")]
	public class NamespaceMapEntry
	{
		// Token: 0x06004120 RID: 16672 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		public NamespaceMapEntry()
		{
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x002172DC File Offset: 0x002162DC
		public NamespaceMapEntry(string xmlNamespace, string assemblyName, string clrNamespace)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (assemblyName == null)
			{
				throw new ArgumentNullException("assemblyName");
			}
			if (clrNamespace == null)
			{
				throw new ArgumentNullException("clrNamespace");
			}
			this._xmlNamespace = xmlNamespace;
			this._assemblyName = assemblyName;
			this._clrNamespace = clrNamespace;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x0021732E File Offset: 0x0021632E
		internal NamespaceMapEntry(string xmlNamespace, string assemblyName, string clrNamespace, string assemblyPath) : this(xmlNamespace, assemblyName, clrNamespace)
		{
			this._assemblyPath = assemblyPath;
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06004123 RID: 16675 RVA: 0x00217341 File Offset: 0x00216341
		// (set) Token: 0x06004124 RID: 16676 RVA: 0x00217349 File Offset: 0x00216349
		public string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._xmlNamespace == null)
				{
					this._xmlNamespace = value;
				}
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06004125 RID: 16677 RVA: 0x00217368 File Offset: 0x00216368
		// (set) Token: 0x06004126 RID: 16678 RVA: 0x00217370 File Offset: 0x00216370
		public string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._assemblyName == null)
				{
					this._assemblyName = value;
				}
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06004127 RID: 16679 RVA: 0x0021738F File Offset: 0x0021638F
		// (set) Token: 0x06004128 RID: 16680 RVA: 0x00217397 File Offset: 0x00216397
		public string ClrNamespace
		{
			get
			{
				return this._clrNamespace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._clrNamespace == null)
				{
					this._clrNamespace = value;
				}
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06004129 RID: 16681 RVA: 0x002173B6 File Offset: 0x002163B6
		internal Assembly Assembly
		{
			get
			{
				if (null == this._assembly && this._assemblyName.Length > 0)
				{
					this._assembly = ReflectionHelper.LoadAssembly(this._assemblyName, this._assemblyPath);
				}
				return this._assembly;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x0600412A RID: 16682 RVA: 0x002173F1 File Offset: 0x002163F1
		// (set) Token: 0x0600412B RID: 16683 RVA: 0x002173F9 File Offset: 0x002163F9
		internal string AssemblyPath
		{
			get
			{
				return this._assemblyPath;
			}
			set
			{
				this._assemblyPath = value;
			}
		}

		// Token: 0x040024A0 RID: 9376
		private string _xmlNamespace;

		// Token: 0x040024A1 RID: 9377
		private string _assemblyName;

		// Token: 0x040024A2 RID: 9378
		private string _assemblyPath;

		// Token: 0x040024A3 RID: 9379
		private Assembly _assembly;

		// Token: 0x040024A4 RID: 9380
		private string _clrNamespace;
	}
}
