using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Xml;

namespace MS.Internal.Annotations
{
	// Token: 0x020002C4 RID: 708
	internal sealed class XmlElementCollection : ObservableCollection<XmlElement>
	{
		// Token: 0x06001A58 RID: 6744 RVA: 0x0016373B File Offset: 0x0016273B
		public XmlElementCollection()
		{
			this._xmlDocsRefCounts = new Dictionary<XmlDocument, int>();
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00163750 File Offset: 0x00162750
		protected override void ClearItems()
		{
			foreach (XmlElement element in this)
			{
				this.UnregisterForElement(element);
			}
			base.ClearItems();
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x001637A0 File Offset: 0x001627A0
		protected override void RemoveItem(int index)
		{
			XmlElement element = base[index];
			this.UnregisterForElement(element);
			base.RemoveItem(index);
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x001637C4 File Offset: 0x001627C4
		protected override void InsertItem(int index, XmlElement item)
		{
			if (item != null && base.Contains(item))
			{
				throw new ArgumentException(SR.Get("XmlNodeAlreadyOwned", new object[]
				{
					"change",
					"change"
				}), "item");
			}
			base.InsertItem(index, item);
			this.RegisterForElement(item);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x00163818 File Offset: 0x00162818
		protected override void SetItem(int index, XmlElement item)
		{
			if (item != null && base.Contains(item))
			{
				throw new ArgumentException(SR.Get("XmlNodeAlreadyOwned", new object[]
				{
					"change",
					"change"
				}), "item");
			}
			XmlElement element = base[index];
			this.UnregisterForElement(element);
			base.Items[index] = item;
			this.OnCollectionReset();
			this.RegisterForElement(item);
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x00163888 File Offset: 0x00162888
		private void UnregisterForElement(XmlElement element)
		{
			if (element == null)
			{
				return;
			}
			Invariant.Assert(this._xmlDocsRefCounts.ContainsKey(element.OwnerDocument), "Not registered on XmlElement");
			Dictionary<XmlDocument, int> xmlDocsRefCounts = this._xmlDocsRefCounts;
			XmlDocument ownerDocument = element.OwnerDocument;
			int num = xmlDocsRefCounts[ownerDocument];
			xmlDocsRefCounts[ownerDocument] = num - 1;
			if (this._xmlDocsRefCounts[element.OwnerDocument] == 0)
			{
				element.OwnerDocument.NodeChanged -= this.OnNodeChanged;
				element.OwnerDocument.NodeInserted -= this.OnNodeChanged;
				element.OwnerDocument.NodeRemoved -= this.OnNodeChanged;
				this._xmlDocsRefCounts.Remove(element.OwnerDocument);
			}
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x0016393C File Offset: 0x0016293C
		private void RegisterForElement(XmlElement element)
		{
			if (element == null)
			{
				return;
			}
			if (!this._xmlDocsRefCounts.ContainsKey(element.OwnerDocument))
			{
				this._xmlDocsRefCounts[element.OwnerDocument] = 1;
				XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnNodeChanged);
				element.OwnerDocument.NodeChanged += value;
				element.OwnerDocument.NodeInserted += value;
				element.OwnerDocument.NodeRemoved += value;
				return;
			}
			Dictionary<XmlDocument, int> xmlDocsRefCounts = this._xmlDocsRefCounts;
			XmlDocument ownerDocument = element.OwnerDocument;
			int num = xmlDocsRefCounts[ownerDocument];
			xmlDocsRefCounts[ownerDocument] = num + 1;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x001639C4 File Offset: 0x001629C4
		private void OnNodeChanged(object sender, XmlNodeChangedEventArgs args)
		{
			Invariant.Assert(this._xmlDocsRefCounts.ContainsKey(sender as XmlDocument), "Not expecting a notification from this sender");
			XmlNode xmlNode = args.Node;
			while (xmlNode != null)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null && base.Contains(xmlElement))
				{
					this.OnCollectionReset();
					return;
				}
				XmlAttribute xmlAttribute = xmlNode as XmlAttribute;
				if (xmlAttribute != null)
				{
					xmlNode = xmlAttribute.OwnerElement;
				}
				else
				{
					xmlNode = xmlNode.ParentNode;
				}
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00163A30 File Offset: 0x00162A30
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x04000DB3 RID: 3507
		private Dictionary<XmlDocument, int> _xmlDocsRefCounts;
	}
}
