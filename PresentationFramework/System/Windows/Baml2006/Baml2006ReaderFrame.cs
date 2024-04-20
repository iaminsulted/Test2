using System;
using System.Collections.Generic;
using System.Xaml;
using MS.Internal.Xaml.Context;

namespace System.Windows.Baml2006
{
	// Token: 0x02000408 RID: 1032
	internal class Baml2006ReaderFrame : XamlFrame
	{
		// Token: 0x06002D08 RID: 11528 RVA: 0x001AB149 File Offset: 0x001AA149
		public Baml2006ReaderFrame()
		{
			this.DelayedConnectionId = -1;
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x001AB158 File Offset: 0x001AA158
		public Baml2006ReaderFrame(Baml2006ReaderFrame source)
		{
			this.XamlType = source.XamlType;
			this.Member = source.Member;
			if (source._namespaces != null)
			{
				this._namespaces = new Dictionary<string, string>(source._namespaces);
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x001AB191 File Offset: 0x001AA191
		public override XamlFrame Clone()
		{
			return new Baml2006ReaderFrame(this);
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x001AB199 File Offset: 0x001AA199
		// (set) Token: 0x06002D0C RID: 11532 RVA: 0x001AB1A1 File Offset: 0x001AA1A1
		public XamlType XamlType { get; set; }

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x001AB1AA File Offset: 0x001AA1AA
		// (set) Token: 0x06002D0E RID: 11534 RVA: 0x001AB1B2 File Offset: 0x001AA1B2
		public XamlMember Member { get; set; }

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002D0F RID: 11535 RVA: 0x001AB1BB File Offset: 0x001AA1BB
		// (set) Token: 0x06002D10 RID: 11536 RVA: 0x001AB1C3 File Offset: 0x001AA1C3
		public KeyRecord Key { get; set; }

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002D11 RID: 11537 RVA: 0x001AB1CC File Offset: 0x001AA1CC
		// (set) Token: 0x06002D12 RID: 11538 RVA: 0x001AB1D4 File Offset: 0x001AA1D4
		public int DelayedConnectionId { get; set; }

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002D13 RID: 11539 RVA: 0x001AB1DD File Offset: 0x001AA1DD
		// (set) Token: 0x06002D14 RID: 11540 RVA: 0x001AB1E5 File Offset: 0x001AA1E5
		public XamlMember ContentProperty { get; set; }

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x001AB1EE File Offset: 0x001AA1EE
		// (set) Token: 0x06002D16 RID: 11542 RVA: 0x001AB1F6 File Offset: 0x001AA1F6
		public bool FreezeFreezables { get; set; }

		// Token: 0x06002D17 RID: 11543 RVA: 0x001AB1FF File Offset: 0x001AA1FF
		public void AddNamespace(string prefix, string xamlNs)
		{
			if (this._namespaces == null)
			{
				this._namespaces = new Dictionary<string, string>();
			}
			this._namespaces.Add(prefix, xamlNs);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x001AB221 File Offset: 0x001AA221
		public void SetNamespaces(Dictionary<string, string> namespaces)
		{
			this._namespaces = namespaces;
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x001AB22A File Offset: 0x001AA22A
		public bool TryGetNamespaceByPrefix(string prefix, out string xamlNs)
		{
			if (this._namespaces != null && this._namespaces.TryGetValue(prefix, out xamlNs))
			{
				return true;
			}
			xamlNs = null;
			return false;
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x001AB24C File Offset: 0x001AA24C
		public bool TryGetPrefixByNamespace(string xamlNs, out string prefix)
		{
			if (this._namespaces != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in this._namespaces)
				{
					if (keyValuePair.Value == xamlNs)
					{
						prefix = keyValuePair.Key;
						return true;
					}
				}
			}
			prefix = null;
			return false;
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x001AB2C4 File Offset: 0x001AA2C4
		public override void Reset()
		{
			this.XamlType = null;
			this.Member = null;
			if (this._namespaces != null)
			{
				this._namespaces.Clear();
			}
			this.Flags = Baml2006ReaderFrameFlags.None;
			this.IsDeferredContent = false;
			this.Key = null;
			this.DelayedConnectionId = -1;
			this.ContentProperty = null;
		}

		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x001AB315 File Offset: 0x001AA315
		// (set) Token: 0x06002D1D RID: 11549 RVA: 0x001AB31D File Offset: 0x001AA31D
		public Baml2006ReaderFrameFlags Flags { get; set; }

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x001AB326 File Offset: 0x001AA326
		// (set) Token: 0x06002D1F RID: 11551 RVA: 0x001AB32E File Offset: 0x001AA32E
		public bool IsDeferredContent { get; set; }

		// Token: 0x04001B5C RID: 7004
		protected Dictionary<string, string> _namespaces;
	}
}
