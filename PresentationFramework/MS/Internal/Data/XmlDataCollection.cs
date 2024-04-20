using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x0200024D RID: 589
	internal class XmlDataCollection : ReadOnlyObservableCollection<XmlNode>
	{
		// Token: 0x060016B6 RID: 5814 RVA: 0x0015C044 File Offset: 0x0015B044
		internal XmlDataCollection(XmlDataProvider xmlDataProvider) : base(new ObservableCollection<XmlNode>())
		{
			this._xds = xmlDataProvider;
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060016B7 RID: 5815 RVA: 0x0015C058 File Offset: 0x0015B058
		// (set) Token: 0x060016B8 RID: 5816 RVA: 0x0015C081 File Offset: 0x0015B081
		internal XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				if (this._nsMgr == null && this._xds != null)
				{
					this._nsMgr = this._xds.XmlNamespaceManager;
				}
				return this._nsMgr;
			}
			set
			{
				this._nsMgr = value;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060016B9 RID: 5817 RVA: 0x0015C08A File Offset: 0x0015B08A
		internal XmlDataProvider ParentXmlDataProvider
		{
			get
			{
				return this._xds;
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0015C094 File Offset: 0x0015B094
		internal bool CollectionHasChanged(XmlNodeList nodes)
		{
			int count = base.Count;
			if (count != nodes.Count)
			{
				return true;
			}
			for (int i = 0; i < count; i++)
			{
				if (base[i] != nodes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0015C0D4 File Offset: 0x0015B0D4
		internal void SynchronizeCollection(XmlNodeList nodes)
		{
			if (nodes == null)
			{
				base.Items.Clear();
				return;
			}
			int i = 0;
			while (i < base.Count)
			{
				if (i >= nodes.Count)
				{
					break;
				}
				if (base[i] != nodes[i])
				{
					int num = i + 1;
					while (num < nodes.Count && base[i] != nodes[num])
					{
						num++;
					}
					if (num < nodes.Count)
					{
						while (i < num)
						{
							base.Items.Insert(i, nodes[i]);
							i++;
						}
						i++;
					}
					else
					{
						base.Items.RemoveAt(i);
					}
				}
				else
				{
					i++;
				}
			}
			while (i < base.Count)
			{
				base.Items.RemoveAt(i);
			}
			while (i < nodes.Count)
			{
				base.Items.Insert(i, nodes[i]);
				i++;
			}
		}

		// Token: 0x04000C79 RID: 3193
		private XmlDataProvider _xds;

		// Token: 0x04000C7A RID: 3194
		private XmlNamespaceManager _nsMgr;
	}
}
