using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using MS.Internal;
using MS.Internal.Annotations;
using MS.Utility;

namespace System.Windows.Annotations
{
	// Token: 0x02000866 RID: 2150
	[XmlRoot(Namespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core", ElementName = "Annotation")]
	public sealed class Annotation : IXmlSerializable
	{
		// Token: 0x06007ED2 RID: 32466 RVA: 0x0031AB4E File Offset: 0x00319B4E
		public Annotation()
		{
			this._id = Guid.Empty;
			this._created = DateTime.MinValue;
			this._modified = DateTime.MinValue;
			this.Init();
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x0031AB80 File Offset: 0x00319B80
		public Annotation(XmlQualifiedName annotationType)
		{
			if (annotationType == null)
			{
				throw new ArgumentNullException("annotationType");
			}
			if (string.IsNullOrEmpty(annotationType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Name");
			}
			if (string.IsNullOrEmpty(annotationType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Namespace");
			}
			this._id = Guid.NewGuid();
			this._typeName = annotationType;
			this._created = DateTime.Now;
			this._modified = this._created;
			this.Init();
		}

		// Token: 0x06007ED4 RID: 32468 RVA: 0x0031AC1C File Offset: 0x00319C1C
		public Annotation(XmlQualifiedName annotationType, Guid id, DateTime creationTime, DateTime lastModificationTime)
		{
			if (annotationType == null)
			{
				throw new ArgumentNullException("annotationType");
			}
			if (string.IsNullOrEmpty(annotationType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Name");
			}
			if (string.IsNullOrEmpty(annotationType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "annotationType.Namespace");
			}
			if (id.Equals(Guid.Empty))
			{
				throw new ArgumentException(SR.Get("InvalidGuid"), "id");
			}
			if (lastModificationTime.CompareTo(creationTime) < 0)
			{
				throw new ArgumentException(SR.Get("ModificationEarlierThanCreation"), "lastModificationTime");
			}
			this._id = id;
			this._typeName = annotationType;
			this._created = creationTime;
			this._modified = lastModificationTime;
			this.Init();
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x00109403 File Offset: 0x00108403
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06007ED6 RID: 32470 RVA: 0x0031ACF0 File Offset: 0x00319CF0
		public void WriteXml(XmlWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.SerializeAnnotationBegin);
			try
			{
				if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core")))
				{
					writer.WriteAttributeString("xmlns", "anc", null, "http://schemas.microsoft.com/windows/annotations/2003/11/core");
				}
				if (string.IsNullOrEmpty(writer.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/base")))
				{
					writer.WriteAttributeString("xmlns", "anb", null, "http://schemas.microsoft.com/windows/annotations/2003/11/base");
				}
				if (this._typeName == null)
				{
					throw new InvalidOperationException(SR.Get("CannotSerializeInvalidInstance"));
				}
				writer.WriteAttributeString("Id", XmlConvert.ToString(this._id));
				writer.WriteAttributeString("CreationTime", XmlConvert.ToString(this._created));
				writer.WriteAttributeString("LastModificationTime", XmlConvert.ToString(this._modified));
				writer.WriteStartAttribute("Type");
				writer.WriteQualifiedName(this._typeName.Name, this._typeName.Namespace);
				writer.WriteEndAttribute();
				if (this._authors != null && this._authors.Count > 0)
				{
					writer.WriteStartElement("Authors", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (string text in this._authors)
					{
						if (text != null)
						{
							writer.WriteElementString("anb", "StringAuthor", "http://schemas.microsoft.com/windows/annotations/2003/11/base", text);
						}
					}
					writer.WriteEndElement();
				}
				if (this._anchors != null && this._anchors.Count > 0)
				{
					writer.WriteStartElement("Anchors", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (AnnotationResource annotationResource in this._anchors)
					{
						if (annotationResource != null)
						{
							Annotation.ResourceSerializer.Serialize(writer, annotationResource);
						}
					}
					writer.WriteEndElement();
				}
				if (this._cargos != null && this._cargos.Count > 0)
				{
					writer.WriteStartElement("Cargos", "http://schemas.microsoft.com/windows/annotations/2003/11/core");
					foreach (AnnotationResource annotationResource2 in this._cargos)
					{
						if (annotationResource2 != null)
						{
							Annotation.ResourceSerializer.Serialize(writer, annotationResource2);
						}
					}
					writer.WriteEndElement();
				}
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.SerializeAnnotationEnd);
			}
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x0031AFA0 File Offset: 0x00319FA0
		public void ReadXml(XmlReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeserializeAnnotationBegin);
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				this.ReadAttributes(reader);
				if (!reader.IsEmptyElement)
				{
					reader.Read();
					while (XmlNodeType.EndElement != reader.NodeType || !("Annotation" == reader.LocalName))
					{
						if ("Anchors" == reader.LocalName)
						{
							Annotation.CheckForNonNamespaceAttribute(reader, "Anchors");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Anchors" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									AnnotationResource item = (AnnotationResource)Annotation.ResourceSerializer.Deserialize(reader);
									this._anchors.Add(item);
								}
							}
							reader.Read();
						}
						else if ("Cargos" == reader.LocalName)
						{
							Annotation.CheckForNonNamespaceAttribute(reader, "Cargos");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Cargos" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									AnnotationResource item2 = (AnnotationResource)Annotation.ResourceSerializer.Deserialize(reader);
									this._cargos.Add(item2);
								}
							}
							reader.Read();
						}
						else
						{
							if (!("Authors" == reader.LocalName))
							{
								throw new XmlException(SR.Get("InvalidXmlContent", new object[]
								{
									"Annotation"
								}));
							}
							Annotation.CheckForNonNamespaceAttribute(reader, "Authors");
							if (!reader.IsEmptyElement)
							{
								reader.Read();
								while (!("Authors" == reader.LocalName) || XmlNodeType.EndElement != reader.NodeType)
								{
									if (!("StringAuthor" == reader.LocalName) || XmlNodeType.Element != reader.NodeType)
									{
										throw new XmlException(SR.Get("InvalidXmlContent", new object[]
										{
											"Annotation"
										}));
									}
									XmlNode xmlNode = xmlDocument.ReadNode(reader);
									if (!reader.IsEmptyElement)
									{
										this._authors.Add(xmlNode.InnerText);
									}
								}
							}
							reader.Read();
						}
					}
				}
				reader.Read();
			}
			finally
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordAnnotation, EventTrace.Event.DeserializeAnnotationEnd);
			}
		}

		// Token: 0x14000164 RID: 356
		// (add) Token: 0x06007ED8 RID: 32472 RVA: 0x0031B1F8 File Offset: 0x0031A1F8
		// (remove) Token: 0x06007ED9 RID: 32473 RVA: 0x0031B230 File Offset: 0x0031A230
		public event AnnotationAuthorChangedEventHandler AuthorChanged;

		// Token: 0x14000165 RID: 357
		// (add) Token: 0x06007EDA RID: 32474 RVA: 0x0031B268 File Offset: 0x0031A268
		// (remove) Token: 0x06007EDB RID: 32475 RVA: 0x0031B2A0 File Offset: 0x0031A2A0
		public event AnnotationResourceChangedEventHandler AnchorChanged;

		// Token: 0x14000166 RID: 358
		// (add) Token: 0x06007EDC RID: 32476 RVA: 0x0031B2D8 File Offset: 0x0031A2D8
		// (remove) Token: 0x06007EDD RID: 32477 RVA: 0x0031B310 File Offset: 0x0031A310
		public event AnnotationResourceChangedEventHandler CargoChanged;

		// Token: 0x17001D4B RID: 7499
		// (get) Token: 0x06007EDE RID: 32478 RVA: 0x0031B345 File Offset: 0x0031A345
		public Guid Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17001D4C RID: 7500
		// (get) Token: 0x06007EDF RID: 32479 RVA: 0x0031B34D File Offset: 0x0031A34D
		public XmlQualifiedName AnnotationType
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17001D4D RID: 7501
		// (get) Token: 0x06007EE0 RID: 32480 RVA: 0x0031B355 File Offset: 0x0031A355
		public DateTime CreationTime
		{
			get
			{
				return this._created;
			}
		}

		// Token: 0x17001D4E RID: 7502
		// (get) Token: 0x06007EE1 RID: 32481 RVA: 0x0031B35D File Offset: 0x0031A35D
		public DateTime LastModificationTime
		{
			get
			{
				return this._modified;
			}
		}

		// Token: 0x17001D4F RID: 7503
		// (get) Token: 0x06007EE2 RID: 32482 RVA: 0x0031B365 File Offset: 0x0031A365
		public Collection<string> Authors
		{
			get
			{
				return this._authors;
			}
		}

		// Token: 0x17001D50 RID: 7504
		// (get) Token: 0x06007EE3 RID: 32483 RVA: 0x0031B36D File Offset: 0x0031A36D
		public Collection<AnnotationResource> Anchors
		{
			get
			{
				return this._anchors;
			}
		}

		// Token: 0x17001D51 RID: 7505
		// (get) Token: 0x06007EE4 RID: 32484 RVA: 0x0031B375 File Offset: 0x0031A375
		public Collection<AnnotationResource> Cargos
		{
			get
			{
				return this._cargos;
			}
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x0031B380 File Offset: 0x0031A380
		internal static bool IsNamespaceDeclaration(XmlReader reader)
		{
			Invariant.Assert(reader != null);
			if (reader.NodeType == XmlNodeType.Attribute)
			{
				if (reader.Prefix.Length == 0)
				{
					if (reader.LocalName == "xmlns")
					{
						return true;
					}
				}
				else if (reader.Prefix == "xmlns" || reader.Prefix == "xml")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x0031B3E8 File Offset: 0x0031A3E8
		internal static void CheckForNonNamespaceAttribute(XmlReader reader, string elementName)
		{
			Invariant.Assert(reader != null, "No reader supplied.");
			Invariant.Assert(elementName != null, "No element name supplied.");
			while (reader.MoveToNextAttribute())
			{
				if (!Annotation.IsNamespaceDeclaration(reader))
				{
					throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
					{
						reader.LocalName,
						elementName
					}));
				}
			}
			reader.MoveToContent();
		}

		// Token: 0x17001D52 RID: 7506
		// (get) Token: 0x06007EE7 RID: 32487 RVA: 0x0031B44D File Offset: 0x0031A44D
		private static Serializer ResourceSerializer
		{
			get
			{
				if (Annotation._ResourceSerializer == null)
				{
					Annotation._ResourceSerializer = new Serializer(typeof(AnnotationResource));
				}
				return Annotation._ResourceSerializer;
			}
		}

		// Token: 0x06007EE8 RID: 32488 RVA: 0x0031B470 File Offset: 0x0031A470
		private void ReadAttributes(XmlReader reader)
		{
			Invariant.Assert(reader != null, "No reader passed in.");
			while (reader.MoveToNextAttribute())
			{
				string value = reader.Value;
				if (!string.IsNullOrEmpty(value))
				{
					string localName = reader.LocalName;
					if (!(localName == "Id"))
					{
						if (!(localName == "CreationTime"))
						{
							if (!(localName == "LastModificationTime"))
							{
								if (!(localName == "Type"))
								{
									if (!Annotation.IsNamespaceDeclaration(reader))
									{
										throw new XmlException(SR.Get("UnexpectedAttribute", new object[]
										{
											reader.LocalName,
											"Annotation"
										}));
									}
								}
								else
								{
									string[] array = value.Split(Annotation._Colon);
									if (array.Length == 1)
									{
										array[0] = array[0].Trim();
										if (string.IsNullOrEmpty(array[0]))
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										this._typeName = new XmlQualifiedName(array[0]);
									}
									else
									{
										if (array.Length != 2)
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										array[0] = array[0].Trim();
										array[1] = array[1].Trim();
										if (string.IsNullOrEmpty(array[0]) || string.IsNullOrEmpty(array[1]))
										{
											throw new FormatException(SR.Get("InvalidAttributeValue", new object[]
											{
												"Type"
											}));
										}
										this._typeName = new XmlQualifiedName(array[1], reader.LookupNamespace(array[0]));
									}
								}
							}
							else
							{
								this._modified = XmlConvert.ToDateTime(value);
							}
						}
						else
						{
							this._created = XmlConvert.ToDateTime(value);
						}
					}
					else
					{
						this._id = XmlConvert.ToGuid(value);
					}
				}
			}
			if (this._id.Equals(Guid.Empty))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Id",
					"Annotation"
				}));
			}
			if (this._created.Equals(DateTime.MinValue))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"CreationTime",
					"Annotation"
				}));
			}
			if (this._modified.Equals(DateTime.MinValue))
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"LastModificationTime",
					"Annotation"
				}));
			}
			if (this._typeName == null)
			{
				throw new XmlException(SR.Get("RequiredAttributeMissing", new object[]
				{
					"Type",
					"Annotation"
				}));
			}
			reader.MoveToContent();
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x0031B70F File Offset: 0x0031A70F
		private void OnCargoChanged(object sender, PropertyChangedEventArgs e)
		{
			this.FireResourceEvent((AnnotationResource)sender, AnnotationAction.Modified, this.CargoChanged);
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x0031B724 File Offset: 0x0031A724
		private void OnCargosChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					AnnotationResource resource = (AnnotationResource)obj;
					this.FireResourceEvent(resource, AnnotationAction.Removed, this.CargoChanged);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object obj2 in list)
				{
					AnnotationResource resource2 = (AnnotationResource)obj2;
					this.FireResourceEvent(resource2, action, this.CargoChanged);
				}
			}
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x0031B848 File Offset: 0x0031A848
		private void OnAnchorChanged(object sender, PropertyChangedEventArgs e)
		{
			this.FireResourceEvent((AnnotationResource)sender, AnnotationAction.Modified, this.AnchorChanged);
		}

		// Token: 0x06007EEC RID: 32492 RVA: 0x0031B860 File Offset: 0x0031A860
		private void OnAnchorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					AnnotationResource resource = (AnnotationResource)obj;
					this.FireResourceEvent(resource, AnnotationAction.Removed, this.AnchorChanged);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object obj2 in list)
				{
					AnnotationResource resource2 = (AnnotationResource)obj2;
					this.FireResourceEvent(resource2, action, this.AnchorChanged);
				}
			}
		}

		// Token: 0x06007EED RID: 32493 RVA: 0x0031B984 File Offset: 0x0031A984
		private void OnAuthorsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			AnnotationAction action = AnnotationAction.Added;
			IList list = null;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				action = AnnotationAction.Added;
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Remove:
				action = AnnotationAction.Removed;
				list = e.OldItems;
				break;
			case NotifyCollectionChangedAction.Replace:
				foreach (object obj in e.OldItems)
				{
					string author = (string)obj;
					this.FireAuthorEvent(author, AnnotationAction.Removed);
				}
				list = e.NewItems;
				break;
			case NotifyCollectionChangedAction.Move:
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			if (list != null)
			{
				foreach (object author2 in list)
				{
					this.FireAuthorEvent(author2, action);
				}
			}
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x0031BA94 File Offset: 0x0031AA94
		private void FireAuthorEvent(object author, AnnotationAction action)
		{
			Invariant.Assert(action >= AnnotationAction.Added && action <= AnnotationAction.Modified, "Unknown AnnotationAction");
			this._modified = DateTime.Now;
			if (this.AuthorChanged != null)
			{
				this.AuthorChanged(this, new AnnotationAuthorChangedEventArgs(this, action, author));
			}
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x0031BAE0 File Offset: 0x0031AAE0
		private void FireResourceEvent(AnnotationResource resource, AnnotationAction action, AnnotationResourceChangedEventHandler handlers)
		{
			Invariant.Assert(action >= AnnotationAction.Added && action <= AnnotationAction.Modified, "Unknown AnnotationAction");
			this._modified = DateTime.Now;
			if (handlers != null)
			{
				handlers(this, new AnnotationResourceChangedEventArgs(this, action, resource));
			}
		}

		// Token: 0x06007EF0 RID: 32496 RVA: 0x0031BB18 File Offset: 0x0031AB18
		private void Init()
		{
			this._cargos = new AnnotationResourceCollection();
			this._cargos.ItemChanged += this.OnCargoChanged;
			this._cargos.CollectionChanged += this.OnCargosChanged;
			this._anchors = new AnnotationResourceCollection();
			this._anchors.ItemChanged += this.OnAnchorChanged;
			this._anchors.CollectionChanged += this.OnAnchorsChanged;
			this._authors = new ObservableCollection<string>();
			this._authors.CollectionChanged += this.OnAuthorsChanged;
		}

		// Token: 0x04003B59 RID: 15193
		private Guid _id;

		// Token: 0x04003B5A RID: 15194
		private XmlQualifiedName _typeName;

		// Token: 0x04003B5B RID: 15195
		private DateTime _created;

		// Token: 0x04003B5C RID: 15196
		private DateTime _modified;

		// Token: 0x04003B5D RID: 15197
		private ObservableCollection<string> _authors;

		// Token: 0x04003B5E RID: 15198
		private AnnotationResourceCollection _cargos;

		// Token: 0x04003B5F RID: 15199
		private AnnotationResourceCollection _anchors;

		// Token: 0x04003B60 RID: 15200
		private static Serializer _ResourceSerializer;

		// Token: 0x04003B61 RID: 15201
		private static readonly char[] _Colon = new char[]
		{
			':'
		};
	}
}
