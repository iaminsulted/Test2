using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000521 RID: 1313
	internal class XmlnsCache
	{
		// Token: 0x06004145 RID: 16709 RVA: 0x002177AE File Offset: 0x002167AE
		internal XmlnsCache()
		{
			this._compatTable = new Dictionary<string, string>();
			this._compatTableReverse = new Dictionary<string, string>();
			this._cacheTable = new HybridDictionary();
			this._uriToAssemblyNameTable = new HybridDictionary();
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x002177E4 File Offset: 0x002167E4
		internal List<ClrNamespaceAssemblyPair> GetMappingArray(string xmlns)
		{
			List<ClrNamespaceAssemblyPair> list = null;
			lock (this)
			{
				list = (this._cacheTable[xmlns] as List<ClrNamespaceAssemblyPair>);
				if (list == null)
				{
					if (this._uriToAssemblyNameTable[xmlns] != null)
					{
						string[] array = this._uriToAssemblyNameTable[xmlns] as string[];
						Assembly[] array2 = new Assembly[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = ReflectionHelper.LoadAssembly(array[i], null);
						}
						this._cacheTable[xmlns] = this.GetClrnsToAssemblyNameMappingList(array2, xmlns);
					}
					else
					{
						Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
						this._cacheTable[xmlns] = this.GetClrnsToAssemblyNameMappingList(assemblies, xmlns);
						this.ProcessXmlnsCompatibleWithAttributes(assemblies);
					}
					list = (this._cacheTable[xmlns] as List<ClrNamespaceAssemblyPair>);
				}
			}
			return list;
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x002178D4 File Offset: 0x002168D4
		internal void SetUriToAssemblyNameMapping(string namespaceUri, string[] asmNameList)
		{
			this._uriToAssemblyNameTable[namespaceUri] = asmNameList;
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x002178E4 File Offset: 0x002168E4
		internal string GetNewXmlnamespace(string oldXmlnamespace)
		{
			string result;
			if (this._compatTable.TryGetValue(oldXmlnamespace, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x00217904 File Offset: 0x00216904
		private Attribute[] GetAttributes(Assembly asm, Type attrType)
		{
			return Attribute.GetCustomAttributes(asm, attrType);
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x00217910 File Offset: 0x00216910
		private void GetNamespacesFromDefinitionAttr(Attribute attr, out string xmlns, out string clrns)
		{
			XmlnsDefinitionAttribute xmlnsDefinitionAttribute = (XmlnsDefinitionAttribute)attr;
			xmlns = xmlnsDefinitionAttribute.XmlNamespace;
			clrns = xmlnsDefinitionAttribute.ClrNamespace;
		}

		// Token: 0x0600414B RID: 16715 RVA: 0x00217934 File Offset: 0x00216934
		private void GetNamespacesFromCompatAttr(Attribute attr, out string oldXmlns, out string newXmlns)
		{
			XmlnsCompatibleWithAttribute xmlnsCompatibleWithAttribute = (XmlnsCompatibleWithAttribute)attr;
			oldXmlns = xmlnsCompatibleWithAttribute.OldNamespace;
			newXmlns = xmlnsCompatibleWithAttribute.NewNamespace;
		}

		// Token: 0x0600414C RID: 16716 RVA: 0x00217958 File Offset: 0x00216958
		private List<ClrNamespaceAssemblyPair> GetClrnsToAssemblyNameMappingList(Assembly[] asmList, string xmlnsRequested)
		{
			List<ClrNamespaceAssemblyPair> list = new List<ClrNamespaceAssemblyPair>();
			for (int i = 0; i < asmList.Length; i++)
			{
				string fullName = asmList[i].FullName;
				Attribute[] attributes = this.GetAttributes(asmList[i], typeof(XmlnsDefinitionAttribute));
				for (int j = 0; j < attributes.Length; j++)
				{
					string text = null;
					string text2 = null;
					this.GetNamespacesFromDefinitionAttr(attributes[j], out text, out text2);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new ArgumentException(SR.Get("ParserAttributeArgsLow", new object[]
						{
							"XmlnsDefinitionAttribute"
						}));
					}
					if (string.CompareOrdinal(xmlnsRequested, text) == 0)
					{
						list.Add(new ClrNamespaceAssemblyPair(text2, fullName));
					}
				}
			}
			return list;
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00217A0C File Offset: 0x00216A0C
		private void ProcessXmlnsCompatibleWithAttributes(Assembly[] asmList)
		{
			for (int i = 0; i < asmList.Length; i++)
			{
				Attribute[] attributes = this.GetAttributes(asmList[i], typeof(XmlnsCompatibleWithAttribute));
				for (int j = 0; j < attributes.Length; j++)
				{
					string text = null;
					string text2 = null;
					this.GetNamespacesFromCompatAttr(attributes[j], out text, out text2);
					if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
					{
						throw new ArgumentException(SR.Get("ParserAttributeArgsLow", new object[]
						{
							"XmlnsCompatibleWithAttribute"
						}));
					}
					if (this._compatTable.ContainsKey(text) && this._compatTable[text] != text2)
					{
						throw new InvalidOperationException(SR.Get("ParserCompatDuplicate", new object[]
						{
							text,
							this._compatTable[text]
						}));
					}
					this._compatTable[text] = text2;
					this._compatTableReverse[text2] = text;
				}
			}
		}

		// Token: 0x040024B8 RID: 9400
		private HybridDictionary _cacheTable;

		// Token: 0x040024B9 RID: 9401
		private Dictionary<string, string> _compatTable;

		// Token: 0x040024BA RID: 9402
		private Dictionary<string, string> _compatTableReverse;

		// Token: 0x040024BB RID: 9403
		private HybridDictionary _uriToAssemblyNameTable;
	}
}
