using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	// Token: 0x02000871 RID: 2161
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "ContentLocatorGroup")]
	public sealed class ContentLocatorGroup : ContentLocatorBase, IXmlSerializable
	{
		// Token: 0x06007F98 RID: 32664 RVA: 0x0031F565 File Offset: 0x0031E565
		public ContentLocatorGroup()
		{
			this._locators = new AnnotationObservableCollection<ContentLocator>();
			this._locators.CollectionChanged += this.OnCollectionChanged;
		}

		// Token: 0x06007F99 RID: 32665 RVA: 0x0031F590 File Offset: 0x0031E590
		public override object Clone()
		{
			ContentLocatorGroup contentLocatorGroup = new ContentLocatorGroup();
			foreach (ContentLocator contentLocator in this._locators)
			{
				contentLocatorGroup.Locators.Add((ContentLocator)contentLocator.Clone());
			}
			return contentLocatorGroup;
		}

		// Token: 0x06007F9A RID: 32666 RVA: 0x00109403 File Offset: 0x00108403
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06007F9B RID: 32667 RVA: 0x0031F5F4 File Offset: 0x0031E5F4
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core") == null)
			{
				writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
			}
			foreach (ContentLocatorBase contentLocatorBase in this._locators)
			{
				if (contentLocatorBase != null)
				{
					AnnotationResource.ListSerializer.Serialize(writer, contentLocatorBase);
				}
			}
		}

		// Token: 0x06007F9C RID: 32668 RVA: 0x0031F67C File Offset: 0x0031E67C
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			Annotation.CheckForNonNamespaceAttribute(reader, "ContentLocatorGroup");
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("ContentLocatorGroup" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if (!("ContentLocator" == reader.LocalName))
					{
						throw new XmlException(SR.Get("InvalidXmlContent", new object[]
						{
							"ContentLocatorGroup"
						}));
					}
					ContentLocator item = (ContentLocator)AnnotationResource.ListSerializer.Deserialize(reader);
					this._locators.Add(item);
				}
			}
			reader.Read();
		}

		// Token: 0x17001D6B RID: 7531
		// (get) Token: 0x06007F9D RID: 32669 RVA: 0x0031F725 File Offset: 0x0031E725
		public Collection<ContentLocator> Locators
		{
			get
			{
				return this._locators;
			}
		}

		// Token: 0x06007F9E RID: 32670 RVA: 0x0031F730 File Offset: 0x0031E730
		internal override ContentLocatorBase Merge(ContentLocatorBase other)
		{
			if (other == null)
			{
				return this;
			}
			ContentLocator contentLocator = null;
			ContentLocatorGroup contentLocatorGroup = other as ContentLocatorGroup;
			if (contentLocatorGroup != null)
			{
				List<ContentLocatorBase> list = new List<ContentLocatorBase>(contentLocatorGroup.Locators.Count * (this.Locators.Count - 1));
				foreach (ContentLocator contentLocator2 in this.Locators)
				{
					foreach (ContentLocator contentLocator3 in contentLocatorGroup.Locators)
					{
						if (contentLocator == null)
						{
							contentLocator = contentLocator3;
						}
						else
						{
							ContentLocator contentLocator4 = (ContentLocator)contentLocator2.Clone();
							contentLocator4.Append(contentLocator3);
							list.Add(contentLocator4);
						}
					}
					contentLocator2.Append(contentLocator);
					contentLocator = null;
				}
				using (List<ContentLocatorBase>.Enumerator enumerator3 = list.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ContentLocatorBase contentLocatorBase = enumerator3.Current;
						ContentLocator item = (ContentLocator)contentLocatorBase;
						this.Locators.Add(item);
					}
					return this;
				}
			}
			ContentLocator contentLocator5 = other as ContentLocator;
			Invariant.Assert(contentLocator5 != null, "other should be of type ContentLocator");
			foreach (ContentLocator contentLocator6 in this.Locators)
			{
				contentLocator6.Append(contentLocator5);
			}
			return this;
		}

		// Token: 0x06007F9F RID: 32671 RVA: 0x0031F8B4 File Offset: 0x0031E8B4
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.FireLocatorChanged("Locators");
		}

		// Token: 0x04003B8D RID: 15245
		private AnnotationObservableCollection<ContentLocator> _locators;
	}
}
