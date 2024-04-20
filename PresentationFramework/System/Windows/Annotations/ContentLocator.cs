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
	// Token: 0x02000873 RID: 2163
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "ContentLocator")]
	public sealed class ContentLocator : ContentLocatorBase, IXmlSerializable
	{
		// Token: 0x06007FB1 RID: 32689 RVA: 0x0031FE09 File Offset: 0x0031EE09
		public ContentLocator()
		{
			this._parts = new AnnotationObservableCollection<ContentLocatorPart>();
			this._parts.CollectionChanged += this.OnCollectionChanged;
		}

		// Token: 0x06007FB2 RID: 32690 RVA: 0x0031FE34 File Offset: 0x0031EE34
		public bool StartsWith(ContentLocator locator)
		{
			if (locator == null)
			{
				throw new ArgumentNullException("locator");
			}
			Invariant.Assert(locator.Parts != null, "Locator has null Parts property.");
			if (this.Parts.Count < locator.Parts.Count)
			{
				return false;
			}
			for (int i = 0; i < locator.Parts.Count; i++)
			{
				ContentLocatorPart contentLocatorPart = locator.Parts[i];
				ContentLocatorPart contentLocatorPart2 = this.Parts[i];
				if (contentLocatorPart == null && contentLocatorPart2 != null)
				{
					return false;
				}
				if (!contentLocatorPart.Matches(contentLocatorPart2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007FB3 RID: 32691 RVA: 0x0031FEC0 File Offset: 0x0031EEC0
		public override object Clone()
		{
			ContentLocator contentLocator = new ContentLocator();
			foreach (ContentLocatorPart contentLocatorPart in this.Parts)
			{
				ContentLocatorPart item;
				if (contentLocatorPart != null)
				{
					item = (ContentLocatorPart)contentLocatorPart.Clone();
				}
				else
				{
					item = null;
				}
				contentLocator.Parts.Add(item);
			}
			return contentLocator;
		}

		// Token: 0x06007FB4 RID: 32692 RVA: 0x00109403 File Offset: 0x00108403
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x0031FF30 File Offset: 0x0031EF30
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
			if (writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/base") == null)
			{
				writer.WriteAttributeString("xmlns", "anb", null, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
			}
			foreach (ContentLocatorPart contentLocatorPart in this._parts)
			{
				string text = writer.LookupPrefix(contentLocatorPart.PartType.Namespace);
				if (string.IsNullOrEmpty(text))
				{
					text = "tmp";
				}
				writer.WriteStartElement(text, contentLocatorPart.PartType.Name, contentLocatorPart.PartType.Namespace);
				foreach (KeyValuePair<string, string> keyValuePair in contentLocatorPart.NameValuePairs)
				{
					writer.WriteStartElement("Item", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					writer.WriteAttributeString("Name", keyValuePair.Key);
					writer.WriteAttributeString("Value", keyValuePair.Value);
					writer.WriteEndElement();
				}
				writer.WriteEndElement();
			}
		}

		// Token: 0x06007FB6 RID: 32694 RVA: 0x00320088 File Offset: 0x0031F088
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			Annotation.CheckForNonNamespaceAttribute(reader, "ContentLocator");
			if (!reader.IsEmptyElement)
			{
				reader.Read();
				while (!("ContentLocator" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
				{
					if (XmlNodeType.Element != reader.NodeType)
					{
						throw new XmlException(SR.Get("InvalidXmlContent", new object[]
						{
							"ContentLocator"
						}));
					}
					ContentLocatorPart contentLocatorPart = new ContentLocatorPart(new XmlQualifiedName(reader.LocalName, reader.NamespaceURI));
					if (!reader.IsEmptyElement)
					{
						Annotation.CheckForNonNamespaceAttribute(reader, contentLocatorPart.PartType.Name);
						reader.Read();
						while (XmlNodeType.EndElement != reader.NodeType || !(contentLocatorPart.PartType.Name == reader.LocalName))
						{
							if (!("Item" == reader.LocalName) || !(reader.NamespaceURI == "http://schemas.microsoft.com/windows/annotations/2003/11/core"))
							{
								throw new XmlException(SR.Get("InvalidXmlContent", new object[]
								{
									contentLocatorPart.PartType.Name
								}));
							}
							string text = null;
							string text2 = null;
							while (reader.MoveToNextAttribute())
							{
								string localName = reader.LocalName;
								if (!(localName == "Name"))
								{
									if (!(localName == "Value"))
									{
										if (!Annotation.IsNamespaceDeclaration(reader))
										{
											throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
											{
												reader.LocalName,
												"Item"
											}));
										}
									}
									else
									{
										text2 = reader.Value;
									}
								}
								else
								{
									text = reader.Value;
								}
							}
							if (text == null)
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Name",
									"Item"
								}));
							}
							if (text2 == null)
							{
								throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
								{
									"Value",
									"Item"
								}));
							}
							reader.MoveToContent();
							contentLocatorPart.NameValuePairs.Add(text, text2);
							bool isEmptyElement = reader.IsEmptyElement;
							reader.Read();
							if (!isEmptyElement)
							{
								if (XmlNodeType.EndElement != reader.NodeType || !("Item" == reader.LocalName))
								{
									throw new XmlException(SR.Get("InvalidXmlContent", new object[]
									{
										"Item"
									}));
								}
								reader.Read();
							}
						}
					}
					this._parts.Add(contentLocatorPart);
					reader.Read();
				}
			}
			reader.Read();
		}

		// Token: 0x17001D6F RID: 7535
		// (get) Token: 0x06007FB7 RID: 32695 RVA: 0x00320307 File Offset: 0x0031F307
		public Collection<ContentLocatorPart> Parts
		{
			get
			{
				return this._parts;
			}
		}

		// Token: 0x06007FB8 RID: 32696 RVA: 0x00320310 File Offset: 0x0031F310
		internal IList<ContentLocatorBase> DotProduct(IList<ContentLocatorPart> additionalLocatorParts)
		{
			List<ContentLocatorBase> list;
			if (additionalLocatorParts == null || additionalLocatorParts.Count == 0)
			{
				list = new List<ContentLocatorBase>(1);
				list.Add(this);
			}
			else
			{
				list = new List<ContentLocatorBase>(additionalLocatorParts.Count);
				for (int i = 1; i < additionalLocatorParts.Count; i++)
				{
					ContentLocator contentLocator = (ContentLocator)this.Clone();
					contentLocator.Parts.Add(additionalLocatorParts[i]);
					list.Add(contentLocator);
				}
				this.Parts.Add(additionalLocatorParts[0]);
				list.Insert(0, this);
			}
			return list;
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x00320398 File Offset: 0x0031F398
		internal override ContentLocatorBase Merge(ContentLocatorBase other)
		{
			if (other == null)
			{
				return this;
			}
			ContentLocatorGroup contentLocatorGroup = other as ContentLocatorGroup;
			if (contentLocatorGroup == null)
			{
				this.Append((ContentLocator)other);
				return this;
			}
			ContentLocatorGroup contentLocatorGroup2 = new ContentLocatorGroup();
			ContentLocator contentLocator = null;
			foreach (ContentLocator contentLocator2 in contentLocatorGroup.Locators)
			{
				if (contentLocator == null)
				{
					contentLocator = contentLocator2;
				}
				else
				{
					ContentLocator contentLocator3 = (ContentLocator)this.Clone();
					contentLocator3.Append(contentLocator2);
					contentLocatorGroup2.Locators.Add(contentLocator3);
				}
			}
			if (contentLocator != null)
			{
				this.Append(contentLocator);
				contentLocatorGroup2.Locators.Add(this);
			}
			if (contentLocatorGroup2.Locators.Count == 0)
			{
				return this;
			}
			return contentLocatorGroup2;
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x00320458 File Offset: 0x0031F458
		internal void Append(ContentLocator other)
		{
			Invariant.Assert(other != null, "Parameter 'other' is null.");
			foreach (ContentLocatorPart contentLocatorPart in other.Parts)
			{
				this.Parts.Add((ContentLocatorPart)contentLocatorPart.Clone());
			}
		}

		// Token: 0x06007FBB RID: 32699 RVA: 0x003204C4 File Offset: 0x0031F4C4
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.FireLocatorChanged("Parts");
		}

		// Token: 0x04003B92 RID: 15250
		private AnnotationObservableCollection<ContentLocatorPart> _parts;
	}
}
