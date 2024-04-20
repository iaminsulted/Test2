using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using System.Xml;

namespace System.Windows.Data
{
	// Token: 0x0200046E RID: 1134
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class XmlNamespaceMappingCollection : XmlNamespaceManager, ICollection<XmlNamespaceMapping>, IEnumerable<XmlNamespaceMapping>, IEnumerable, IAddChildInternal, IAddChild
	{
		// Token: 0x06003A65 RID: 14949 RVA: 0x001F08AE File Offset: 0x001EF8AE
		public XmlNamespaceMappingCollection() : base(new NameTable())
		{
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x001F08BB File Offset: 0x001EF8BB
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x001F08C4 File Offset: 0x001EF8C4
		protected virtual void AddChild(object value)
		{
			XmlNamespaceMapping xmlNamespaceMapping = value as XmlNamespaceMapping;
			if (xmlNamespaceMapping == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMapping", new object[]
				{
					value.GetType().FullName
				}), "value");
			}
			this.Add(xmlNamespaceMapping);
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x001F0911 File Offset: 0x001EF911
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x001F091A File Offset: 0x001EF91A
		protected virtual void AddText(string text)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06003A6A RID: 14954 RVA: 0x001F0934 File Offset: 0x001EF934
		public void Add(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			this.AddNamespace(mapping.Prefix, mapping.Uri.OriginalString);
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x001F0990 File Offset: 0x001EF990
		public void Clear()
		{
			int count = this.Count;
			XmlNamespaceMapping[] array = new XmlNamespaceMapping[count];
			this.CopyTo(array, 0);
			for (int i = 0; i < count; i++)
			{
				this.Remove(array[i]);
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x001F09CC File Offset: 0x001EF9CC
		public bool Contains(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			return this.LookupNamespace(mapping.Prefix) == mapping.Uri.OriginalString;
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x001F0A2C File Offset: 0x001EFA2C
		public void CopyTo(XmlNamespaceMapping[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = arrayIndex;
			int num2 = array.Length;
			foreach (object obj in this)
			{
				XmlNamespaceMapping xmlNamespaceMapping = (XmlNamespaceMapping)obj;
				if (num >= num2)
				{
					throw new ArgumentException(SR.Get("Collection_CopyTo_NumberOfElementsExceedsArrayLength", new object[]
					{
						"arrayIndex",
						"array"
					}));
				}
				array[num] = xmlNamespaceMapping;
				num++;
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x001F0AC4 File Offset: 0x001EFAC4
		public bool Remove(XmlNamespaceMapping mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (mapping.Uri == null)
			{
				throw new ArgumentException(SR.Get("RequiresXmlNamespaceMappingUri"), "mapping");
			}
			if (this.Contains(mapping))
			{
				this.RemoveNamespace(mapping.Prefix, mapping.Uri.OriginalString);
				return true;
			}
			return false;
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06003A6F RID: 14959 RVA: 0x001F0B2C File Offset: 0x001EFB2C
		public int Count
		{
			get
			{
				int num = 0;
				foreach (object obj in this)
				{
					XmlNamespaceMapping xmlNamespaceMapping = (XmlNamespaceMapping)obj;
					num++;
				}
				return num;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x001F0B80 File Offset: 0x001EFB80
		public override IEnumerator GetEnumerator()
		{
			return this.ProtectedGetEnumerator();
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x001F0B80 File Offset: 0x001EFB80
		IEnumerator<XmlNamespaceMapping> IEnumerable<XmlNamespaceMapping>.GetEnumerator()
		{
			return this.ProtectedGetEnumerator();
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x001F0B88 File Offset: 0x001EFB88
		protected IEnumerator<XmlNamespaceMapping> ProtectedGetEnumerator()
		{
			IEnumerator enumerator = this.BaseEnumerator;
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text = (string)obj;
				if (!(text == "xmlns") && !(text == "xml"))
				{
					string text2 = this.LookupNamespace(text);
					if (!(text == string.Empty) || !(text2 == string.Empty))
					{
						Uri uri = new Uri(text2, UriKind.RelativeOrAbsolute);
						XmlNamespaceMapping xmlNamespaceMapping = new XmlNamespaceMapping(text, uri);
						yield return xmlNamespaceMapping;
					}
				}
			}
			yield break;
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06003A74 RID: 14964 RVA: 0x001F0B97 File Offset: 0x001EFB97
		private IEnumerator BaseEnumerator
		{
			get
			{
				return base.GetEnumerator();
			}
		}
	}
}
