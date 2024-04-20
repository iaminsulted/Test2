using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	// Token: 0x0200086C RID: 2156
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "Resource")]
	public sealed class AnnotationResource : IXmlSerializable, INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		// Token: 0x06007F35 RID: 32565 RVA: 0x0031D3C7 File Offset: 0x0031C3C7
		public AnnotationResource()
		{
			this._id = Guid.NewGuid();
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x0031D3DA File Offset: 0x0031C3DA
		public AnnotationResource(string name) : this()
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
			this._id = Guid.NewGuid();
		}

		// Token: 0x06007F37 RID: 32567 RVA: 0x0031D404 File Offset: 0x0031C404
		public AnnotationResource(Guid id)
		{
			if (Guid.Empty.Equals(id))
			{
				throw new ArgumentException(SR.Get("InvalidGuid"), "id");
			}
			this._id = id;
		}

		// Token: 0x06007F38 RID: 32568 RVA: 0x00109403 File Offset: 0x00108403
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06007F39 RID: 32569 RVA: 0x0031D444 File Offset: 0x0031C444
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core")))
			{
				writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
			}
			writer.WriteAttributeString("Id", XmlConvert.ToString(this._id));
			if (this._name != null)
			{
				writer.WriteAttributeString("Name", this._name);
			}
			if (this._locators != null)
			{
				foreach (ContentLocatorBase contentLocatorBase in this._locators)
				{
					if (contentLocatorBase != null)
					{
						if (contentLocatorBase is ContentLocatorGroup)
						{
							AnnotationResource.LocatorGroupSerializer.Serialize(writer, contentLocatorBase);
						}
						else
						{
							AnnotationResource.ListSerializer.Serialize(writer, contentLocatorBase);
						}
					}
				}
			}
			if (this._contents != null)
			{
				foreach (XmlElement xmlElement in this._contents)
				{
					if (xmlElement != null)
					{
						xmlElement.WriteTo(writer);
					}
				}
			}
		}

		// Token: 0x06007F3A RID: 32570 RVA: 0x0031D564 File Offset: 0x0031C564
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			XmlDocument xmlDocument = new XmlDocument();
			this.ReadAttributes(reader);
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("Resource" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if ("ContentLocatorGroup" == reader.LocalName)
					{
						ContentLocatorBase item = (ContentLocatorBase)AnnotationResource.LocatorGroupSerializer.Deserialize(reader);
						this.InternalLocators.Add(item);
					}
					else if ("ContentLocator" == reader.LocalName)
					{
						ContentLocatorBase item2 = (ContentLocatorBase)AnnotationResource.ListSerializer.Deserialize(reader);
						this.InternalLocators.Add(item2);
					}
					else
					{
						if (XmlNodeType.Element != reader.NodeType)
						{
							throw new XmlException(SR.Get("InvalidXmlContent", new object[]
							{
								"Resource"
							}));
						}
						XmlElement item3 = xmlDocument.ReadNode(reader) as XmlElement;
						this.InternalContents.Add(item3);
					}
				}
			}
			reader.Read();
		}

		// Token: 0x14000167 RID: 359
		// (add) Token: 0x06007F3B RID: 32571 RVA: 0x0031D670 File Offset: 0x0031C670
		// (remove) Token: 0x06007F3C RID: 32572 RVA: 0x0031D689 File Offset: 0x0031C689
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this._propertyChanged, value);
			}
			remove
			{
				this._propertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this._propertyChanged, value);
			}
		}

		// Token: 0x17001D5A RID: 7514
		// (get) Token: 0x06007F3D RID: 32573 RVA: 0x0031D6A2 File Offset: 0x0031C6A2
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17001D5B RID: 7515
		// (get) Token: 0x06007F3E RID: 32574 RVA: 0x0031D6AA File Offset: 0x0031C6AA
		// (set) Token: 0x06007F3F RID: 32575 RVA: 0x0031D6B4 File Offset: 0x0031C6B4
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				bool flag = false;
				if (this._name == null)
				{
					if (value != null)
					{
						flag = true;
					}
				}
				else if (!this._name.Equals(value))
				{
					flag = true;
				}
				this._name = value;
				if (flag)
				{
					this.FireResourceChanged("Name");
				}
			}
		}

		// Token: 0x17001D5C RID: 7516
		// (get) Token: 0x06007F40 RID: 32576 RVA: 0x0031D6F7 File Offset: 0x0031C6F7
		public Collection<ContentLocatorBase> ContentLocators
		{
			get
			{
				return this.InternalLocators;
			}
		}

		// Token: 0x17001D5D RID: 7517
		// (get) Token: 0x06007F41 RID: 32577 RVA: 0x0031D6FF File Offset: 0x0031C6FF
		public Collection<XmlElement> Contents
		{
			get
			{
				return this.InternalContents;
			}
		}

		// Token: 0x17001D5E RID: 7518
		// (get) Token: 0x06007F42 RID: 32578 RVA: 0x0031D707 File Offset: 0x0031C707
		// (set) Token: 0x06007F43 RID: 32579 RVA: 0x0031D70F File Offset: 0x0031C70F
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x17001D5F RID: 7519
		// (get) Token: 0x06007F44 RID: 32580 RVA: 0x0031D718 File Offset: 0x0031C718
		internal static Serializer ListSerializer
		{
			get
			{
				if (AnnotationResource.s_ListSerializer == null)
				{
					AnnotationResource.s_ListSerializer = new Serializer(typeof(ContentLocator));
				}
				return AnnotationResource.s_ListSerializer;
			}
		}

		// Token: 0x17001D60 RID: 7520
		// (get) Token: 0x06007F45 RID: 32581 RVA: 0x0031D73A File Offset: 0x0031C73A
		private AnnotationObservableCollection<ContentLocatorBase> InternalLocators
		{
			get
			{
				if (this._locators == null)
				{
					this._locators = new AnnotationObservableCollection<ContentLocatorBase>();
					this._locators.CollectionChanged += this.OnLocatorsChanged;
				}
				return this._locators;
			}
		}

		// Token: 0x17001D61 RID: 7521
		// (get) Token: 0x06007F46 RID: 32582 RVA: 0x0031D76C File Offset: 0x0031C76C
		private XmlElementCollection InternalContents
		{
			get
			{
				if (this._contents == null)
				{
					this._contents = new XmlElementCollection();
					this._contents.CollectionChanged += this.OnContentsChanged;
				}
				return this._contents;
			}
		}

		// Token: 0x17001D62 RID: 7522
		// (get) Token: 0x06007F47 RID: 32583 RVA: 0x0031D79E File Offset: 0x0031C79E
		private static Serializer LocatorGroupSerializer
		{
			get
			{
				if (AnnotationResource.s_LocatorGroupSerializer == null)
				{
					AnnotationResource.s_LocatorGroupSerializer = new Serializer(typeof(ContentLocatorGroup));
				}
				return AnnotationResource.s_LocatorGroupSerializer;
			}
		}

		// Token: 0x06007F48 RID: 32584 RVA: 0x0031D7C0 File Offset: 0x0031C7C0
		private void ReadAttributes(XmlReader reader)
		{
			Invariant.Assert(reader != null, "No reader passed in.");
			Guid guid = Guid.Empty;
			while (reader.MoveToNextAttribute())
			{
				string value = reader.Value;
				if (value != null)
				{
					string localName = reader.LocalName;
					if (!(localName == "Id"))
					{
						if (!(localName == "Name"))
						{
							if (!Annotation.IsNamespaceDeclaration(reader))
							{
								throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
								{
									reader.LocalName,
									"Resource"
								}));
							}
						}
						else
						{
							this._name = value;
						}
					}
					else
					{
						guid = XmlConvert.ToGuid(value);
					}
				}
			}
			if (Guid.Empty.Equals(guid))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Id",
					"Resource"
				}));
			}
			this._id = guid;
			reader.MoveToContent();
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x0031D89D File Offset: 0x0031C89D
		private void OnLocatorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.FireResourceChanged("Locators");
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x0031D8AA File Offset: 0x0031C8AA
		private void OnContentsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.FireResourceChanged("Contents");
		}

		// Token: 0x06007F4B RID: 32587 RVA: 0x0031D8B7 File Offset: 0x0031C8B7
		private void FireResourceChanged(string name)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x04003B6A RID: 15210
		private Guid _id;

		// Token: 0x04003B6B RID: 15211
		private string _name;

		// Token: 0x04003B6C RID: 15212
		private AnnotationObservableCollection<ContentLocatorBase> _locators;

		// Token: 0x04003B6D RID: 15213
		private XmlElementCollection _contents;

		// Token: 0x04003B6E RID: 15214
		private static Serializer s_ListSerializer;

		// Token: 0x04003B6F RID: 15215
		private static Serializer s_LocatorGroupSerializer;

		// Token: 0x04003B70 RID: 15216
		private bool _owned;

		// Token: 0x04003B71 RID: 15217
		private PropertyChangedEventHandler _propertyChanged;
	}
}
