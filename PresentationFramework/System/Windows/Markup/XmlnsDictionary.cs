using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xaml;

namespace System.Windows.Markup
{
	// Token: 0x02000523 RID: 1315
	public class XmlnsDictionary : IDictionary, ICollection, IEnumerable, IXamlNamespaceResolver
	{
		// Token: 0x06004151 RID: 16721 RVA: 0x00217B1E File Offset: 0x00216B1E
		public XmlnsDictionary()
		{
			this.Initialize();
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00217B2C File Offset: 0x00216B2C
		public XmlnsDictionary(XmlnsDictionary xmlnsDictionary)
		{
			if (xmlnsDictionary == null)
			{
				throw new ArgumentNullException("xmlnsDictionary");
			}
			if (xmlnsDictionary != null && xmlnsDictionary.Count > 0)
			{
				this._lastDecl = xmlnsDictionary._lastDecl;
				if (this._nsDeclarations == null)
				{
					this._nsDeclarations = new XmlnsDictionary.NamespaceDeclaration[this._lastDecl + 1];
				}
				this._countDecl = 0;
				for (int i = 0; i <= this._lastDecl; i++)
				{
					if (xmlnsDictionary._nsDeclarations[i].Uri != null)
					{
						this._countDecl++;
					}
					this._nsDeclarations[i].Prefix = xmlnsDictionary._nsDeclarations[i].Prefix;
					this._nsDeclarations[i].Uri = xmlnsDictionary._nsDeclarations[i].Uri;
					this._nsDeclarations[i].ScopeCount = xmlnsDictionary._nsDeclarations[i].ScopeCount;
				}
				return;
			}
			this.Initialize();
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x00217C32 File Offset: 0x00216C32
		public void Add(object prefix, object xmlNamespace)
		{
			if (!(prefix is string) || !(xmlNamespace is string))
			{
				throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
			}
			this.AddNamespace((string)prefix, (string)xmlNamespace);
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00217C66 File Offset: 0x00216C66
		public void Add(string prefix, string xmlNamespace)
		{
			this.AddNamespace(prefix, xmlNamespace);
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00217C70 File Offset: 0x00216C70
		public void Clear()
		{
			this.CheckSealed();
			this._lastDecl = 0;
			this._countDecl = 0;
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00217C86 File Offset: 0x00216C86
		public bool Contains(object key)
		{
			return this.HasNamespace((string)key);
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x00217C94 File Offset: 0x00216C94
		public void Remove(string prefix)
		{
			string xmlNamespace = this.LookupNamespace(prefix);
			this.RemoveNamespace(prefix, xmlNamespace);
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x00217CB1 File Offset: 0x00216CB1
		public void Remove(object prefix)
		{
			this.Remove((string)prefix);
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00217CBF File Offset: 0x00216CBF
		public void CopyTo(DictionaryEntry[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00217CCC File Offset: 0x00216CCC
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl);
			for (int i = 0; i < this._lastDecl; i++)
			{
				if (this._nsDeclarations[i].Uri != null)
				{
					hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
				}
			}
			return hybridDictionary.GetEnumerator();
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00217D37 File Offset: 0x00216D37
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00217D40 File Offset: 0x00216D40
		public void CopyTo(Array array, int index)
		{
			IDictionary namespacesInScope = this.GetNamespacesInScope(XmlnsDictionary.NamespaceScope.All);
			if (namespacesInScope != null)
			{
				namespacesInScope.CopyTo(array, index);
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x00217D60 File Offset: 0x00216D60
		public string GetNamespace(string prefix)
		{
			return this.LookupNamespace(prefix);
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00217D69 File Offset: 0x00216D69
		public IEnumerable<System.Xaml.NamespaceDeclaration> GetNamespacePrefixes()
		{
			if (this._lastDecl > 0)
			{
				int num;
				for (int i = this._lastDecl - 1; i >= 0; i = num - 1)
				{
					yield return new System.Xaml.NamespaceDeclaration(this._nsDeclarations[i].Uri, this._nsDeclarations[i].Prefix);
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00217CCC File Offset: 0x00216CCC
		protected IDictionaryEnumerator GetDictionaryEnumerator()
		{
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl);
			for (int i = 0; i < this._lastDecl; i++)
			{
				if (this._nsDeclarations[i].Uri != null)
				{
					hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
				}
			}
			return hybridDictionary.GetEnumerator();
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00217D79 File Offset: 0x00216D79
		protected IEnumerator GetEnumerator()
		{
			return this.Keys.GetEnumerator();
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x00217D86 File Offset: 0x00216D86
		public void Seal()
		{
			this._sealed = true;
		}

		// Token: 0x06004162 RID: 16738 RVA: 0x00217D90 File Offset: 0x00216D90
		public string LookupNamespace(string prefix)
		{
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Prefix == prefix && this._nsDeclarations[i].Uri != null && this._nsDeclarations[i].Uri != string.Empty)
					{
						return this._nsDeclarations[i].Uri;
					}
				}
			}
			return null;
		}

		// Token: 0x06004163 RID: 16739 RVA: 0x00217E24 File Offset: 0x00216E24
		public string LookupPrefix(string xmlNamespace)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Uri == xmlNamespace)
					{
						return this._nsDeclarations[i].Prefix;
					}
				}
			}
			return null;
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00217E88 File Offset: 0x00216E88
		public string DefaultNamespace()
		{
			string text = this.LookupNamespace(string.Empty);
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00217EAB File Offset: 0x00216EAB
		public void PushScope()
		{
			this.CheckSealed();
			XmlnsDictionary.NamespaceDeclaration[] nsDeclarations = this._nsDeclarations;
			int lastDecl = this._lastDecl;
			nsDeclarations[lastDecl].ScopeCount = nsDeclarations[lastDecl].ScopeCount + 1;
		}

		// Token: 0x06004166 RID: 16742 RVA: 0x00217ED0 File Offset: 0x00216ED0
		public void PopScope()
		{
			this.CheckSealed();
			int scopeCount = this._nsDeclarations[this._lastDecl].ScopeCount;
			int num = this._lastDecl;
			while (num > 0 && this._nsDeclarations[num - 1].ScopeCount == scopeCount)
			{
				num--;
			}
			if (this._nsDeclarations[num].ScopeCount > 0)
			{
				XmlnsDictionary.NamespaceDeclaration[] nsDeclarations = this._nsDeclarations;
				int num2 = num;
				nsDeclarations[num2].ScopeCount = nsDeclarations[num2].ScopeCount - 1;
				this._nsDeclarations[num].Prefix = string.Empty;
				this._nsDeclarations[num].Uri = null;
			}
			this._lastDecl = num;
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06004167 RID: 16743 RVA: 0x00105F35 File Offset: 0x00104F35
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x00217F7A File Offset: 0x00216F7A
		public bool IsReadOnly
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x17000E84 RID: 3716
		public string this[string prefix]
		{
			get
			{
				return this.LookupNamespace(prefix);
			}
			set
			{
				this.AddNamespace(prefix, value);
			}
		}

		// Token: 0x17000E85 RID: 3717
		public object this[object prefix]
		{
			get
			{
				if (!(prefix is string))
				{
					throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
				}
				return this.LookupNamespace((string)prefix);
			}
			set
			{
				if (!(prefix is string) || !(value is string))
				{
					throw new ArgumentException(SR.Get("ParserKeysAreStrings"));
				}
				this.AddNamespace((string)prefix, (string)value);
			}
		}

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x0600416D RID: 16749 RVA: 0x00217FA8 File Offset: 0x00216FA8
		public ICollection Keys
		{
			get
			{
				ArrayList arrayList = new ArrayList(this._lastDecl + 1);
				for (int i = 0; i < this._lastDecl; i++)
				{
					if (this._nsDeclarations[i].Uri != null && !arrayList.Contains(this._nsDeclarations[i].Prefix))
					{
						arrayList.Add(this._nsDeclarations[i].Prefix);
					}
				}
				return arrayList;
			}
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x0600416E RID: 16750 RVA: 0x0021801C File Offset: 0x0021701C
		public ICollection Values
		{
			get
			{
				HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl + 1);
				for (int i = 0; i < this._lastDecl; i++)
				{
					if (this._nsDeclarations[i].Uri != null)
					{
						hybridDictionary[this._nsDeclarations[i].Prefix] = this._nsDeclarations[i].Uri;
					}
				}
				return hybridDictionary.Values;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x00218089 File Offset: 0x00217089
		public int Count
		{
			get
			{
				return this._countDecl;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06004170 RID: 16752 RVA: 0x00218091 File Offset: 0x00217091
		public bool IsSynchronized
		{
			get
			{
				return this._nsDeclarations.IsSynchronized;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0021809E File Offset: 0x0021709E
		public object SyncRoot
		{
			get
			{
				return this._nsDeclarations.SyncRoot;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06004172 RID: 16754 RVA: 0x00217F7A File Offset: 0x00216F7A
		public bool Sealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06004173 RID: 16755 RVA: 0x002180AB File Offset: 0x002170AB
		internal void Unseal()
		{
			this._sealed = false;
		}

		// Token: 0x06004174 RID: 16756 RVA: 0x002180B4 File Offset: 0x002170B4
		private void Initialize()
		{
			this._nsDeclarations = new XmlnsDictionary.NamespaceDeclaration[8];
			this._nsDeclarations[0].Prefix = string.Empty;
			this._nsDeclarations[0].Uri = null;
			this._nsDeclarations[0].ScopeCount = 0;
			this._lastDecl = 0;
			this._countDecl = 0;
		}

		// Token: 0x06004175 RID: 16757 RVA: 0x00218115 File Offset: 0x00217115
		private void CheckSealed()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("ParserDictionarySealed"));
			}
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x00218130 File Offset: 0x00217130
		private void AddNamespace(string prefix, string xmlNamespace)
		{
			this.CheckSealed();
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (prefix == null)
			{
				throw new ArgumentNullException("prefix");
			}
			int scopeCount = this._nsDeclarations[this._lastDecl].ScopeCount;
			if (this._lastDecl > 0)
			{
				int num = this._lastDecl - 1;
				while (num >= 0 && this._nsDeclarations[num].ScopeCount == scopeCount)
				{
					if (string.Equals(this._nsDeclarations[num].Prefix, prefix))
					{
						this._nsDeclarations[num].Uri = xmlNamespace;
						return;
					}
					num--;
				}
				if (this._lastDecl == this._nsDeclarations.Length - 1)
				{
					XmlnsDictionary.NamespaceDeclaration[] array = new XmlnsDictionary.NamespaceDeclaration[this._nsDeclarations.Length * 2];
					Array.Copy(this._nsDeclarations, 0, array, 0, this._nsDeclarations.Length);
					this._nsDeclarations = array;
				}
			}
			this._countDecl++;
			this._nsDeclarations[this._lastDecl].Prefix = prefix;
			this._nsDeclarations[this._lastDecl].Uri = xmlNamespace;
			this._lastDecl++;
			this._nsDeclarations[this._lastDecl].ScopeCount = scopeCount;
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x00218278 File Offset: 0x00217278
		private void RemoveNamespace(string prefix, string xmlNamespace)
		{
			this.CheckSealed();
			if (this._lastDecl > 0)
			{
				if (xmlNamespace == null)
				{
					throw new ArgumentNullException("xmlNamespace");
				}
				if (prefix == null)
				{
					throw new ArgumentNullException("prefix");
				}
				int scopeCount = this._nsDeclarations[this._lastDecl - 1].ScopeCount;
				int num = this._lastDecl - 1;
				while (num >= 0 && this._nsDeclarations[num].ScopeCount == scopeCount)
				{
					if (this._nsDeclarations[num].Prefix == prefix && this._nsDeclarations[num].Uri == xmlNamespace)
					{
						this._nsDeclarations[num].Uri = null;
						this._countDecl--;
					}
					num--;
				}
			}
		}

		// Token: 0x06004178 RID: 16760 RVA: 0x00218348 File Offset: 0x00217348
		private IDictionary GetNamespacesInScope(XmlnsDictionary.NamespaceScope scope)
		{
			int i = 0;
			if (scope != XmlnsDictionary.NamespaceScope.All)
			{
				if (scope == XmlnsDictionary.NamespaceScope.Local)
				{
					i = this._lastDecl;
					int scopeCount = this._nsDeclarations[i].ScopeCount;
					while (this._nsDeclarations[i].ScopeCount == scopeCount)
					{
						i--;
					}
					i++;
				}
			}
			else
			{
				i = 0;
			}
			HybridDictionary hybridDictionary = new HybridDictionary(this._lastDecl - i + 1);
			while (i < this._lastDecl)
			{
				string prefix = this._nsDeclarations[i].Prefix;
				string uri = this._nsDeclarations[i].Uri;
				if (uri.Length > 0 || prefix.Length > 0)
				{
					hybridDictionary[prefix] = uri;
				}
				else
				{
					hybridDictionary.Remove(prefix);
				}
				i++;
			}
			return hybridDictionary;
		}

		// Token: 0x06004179 RID: 16761 RVA: 0x00218408 File Offset: 0x00217408
		private bool HasNamespace(string prefix)
		{
			if (this._lastDecl > 0)
			{
				for (int i = this._lastDecl - 1; i >= 0; i--)
				{
					if (this._nsDeclarations[i].Prefix == prefix && this._nsDeclarations[i].Uri != null)
					{
						return prefix.Length > 0 || this._nsDeclarations[i].Uri.Length > 0;
					}
				}
			}
			return false;
		}

		// Token: 0x040024BE RID: 9406
		private XmlnsDictionary.NamespaceDeclaration[] _nsDeclarations;

		// Token: 0x040024BF RID: 9407
		private int _lastDecl;

		// Token: 0x040024C0 RID: 9408
		private int _countDecl;

		// Token: 0x040024C1 RID: 9409
		private bool _sealed;

		// Token: 0x02000B05 RID: 2821
		private struct NamespaceDeclaration
		{
			// Token: 0x0400477A RID: 18298
			public string Prefix;

			// Token: 0x0400477B RID: 18299
			public string Uri;

			// Token: 0x0400477C RID: 18300
			public int ScopeCount;
		}

		// Token: 0x02000B06 RID: 2822
		private enum NamespaceScope
		{
			// Token: 0x0400477E RID: 18302
			All,
			// Token: 0x0400477F RID: 18303
			Local
		}
	}
}
