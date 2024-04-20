using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Markup.Localizer;
using System.Xml;
using MS.Utility;

namespace MS.Internal.Globalization
{
	// Token: 0x0200019B RID: 411
	internal class InternalBamlLocalizabilityResolver : BamlLocalizabilityResolver
	{
		// Token: 0x06000D97 RID: 3479 RVA: 0x00136416 File Offset: 0x00135416
		internal InternalBamlLocalizabilityResolver(BamlLocalizer localizer, BamlLocalizabilityResolver externalResolver, TextReader comments)
		{
			this._localizer = localizer;
			this._externalResolver = externalResolver;
			this._commentingText = comments;
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00136434 File Offset: 0x00135434
		internal void AddClassAndAssembly(string className, string assemblyName)
		{
			if (assemblyName == null || this._classNameToAssemblyIndex.Contains(className))
			{
				return;
			}
			int num = this._assemblyNames.IndexOf(assemblyName);
			if (num < 0)
			{
				this._assemblyNames.Add(assemblyName);
				num = this._assemblyNames.Count - 1;
			}
			this._classNameToAssemblyIndex.Add(className, num);
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00136494 File Offset: 0x00135494
		internal void InitLocalizabilityCache()
		{
			this._assemblyNames = new FrugalObjectList<string>();
			this._classNameToAssemblyIndex = new Hashtable(8);
			this._classAttributeTable = new Dictionary<string, ElementLocalizability>(8);
			this._propertyAttributeTable = new Dictionary<string, LocalizabilityAttribute>(8);
			this._comments = new InternalBamlLocalizabilityResolver.ElementComments[8];
			this._commentsIndex = 0;
			XmlDocument xmlDocument = null;
			if (this._commentingText != null)
			{
				xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.Load(this._commentingText);
				}
				catch (XmlException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(new BamlLocalizableResourceKey(string.Empty, string.Empty, string.Empty), BamlLocalizerError.InvalidCommentingXml));
					xmlDocument = null;
				}
			}
			this._commentsDocument = xmlDocument;
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00136540 File Offset: 0x00135540
		internal void ReleaseLocalizabilityCache()
		{
			this._propertyAttributeTable = null;
			this._comments = null;
			this._commentsIndex = 0;
			this._commentsDocument = null;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00136560 File Offset: 0x00135560
		internal LocalizabilityGroup GetLocalizabilityComment(BamlStartElementNode node, string localName)
		{
			InternalBamlLocalizabilityResolver.ElementComments elementComments = this.LookupCommentForElement(node);
			for (int i = 0; i < elementComments.LocalizationAttributes.Length; i++)
			{
				if (elementComments.LocalizationAttributes[i].PropertyName == localName)
				{
					return (LocalizabilityGroup)elementComments.LocalizationAttributes[i].Value;
				}
			}
			return null;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x001365B4 File Offset: 0x001355B4
		internal string GetStringComment(BamlStartElementNode node, string localName)
		{
			InternalBamlLocalizabilityResolver.ElementComments elementComments = this.LookupCommentForElement(node);
			for (int i = 0; i < elementComments.LocalizationComments.Length; i++)
			{
				if (elementComments.LocalizationComments[i].PropertyName == localName)
				{
					return (string)elementComments.LocalizationComments[i].Value;
				}
			}
			return null;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00136605 File Offset: 0x00135605
		internal void RaiseErrorNotifyEvent(BamlLocalizerErrorNotifyEventArgs e)
		{
			this._localizer.RaiseErrorNotifyEvent(e);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00136614 File Offset: 0x00135614
		public override ElementLocalizability GetElementLocalizability(string assembly, string className)
		{
			if (this._externalResolver == null || assembly == null || assembly.Length == 0 || className == null || className.Length == 0)
			{
				return new ElementLocalizability(null, this.DefaultAttribute);
			}
			if (this._classAttributeTable.ContainsKey(className))
			{
				return this._classAttributeTable[className];
			}
			ElementLocalizability elementLocalizability = this._externalResolver.GetElementLocalizability(assembly, className);
			if (elementLocalizability == null || elementLocalizability.Attribute == null)
			{
				elementLocalizability = new ElementLocalizability(null, this.DefaultAttribute);
			}
			this._classAttributeTable[className] = elementLocalizability;
			return elementLocalizability;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0013669C File Offset: 0x0013569C
		public override LocalizabilityAttribute GetPropertyLocalizability(string assembly, string className, string property)
		{
			if (this._externalResolver == null || assembly == null || assembly.Length == 0 || className == null || className.Length == 0 || property == null || property.Length == 0)
			{
				return this.DefaultAttribute;
			}
			string key = className + ":" + property;
			if (this._propertyAttributeTable.ContainsKey(key))
			{
				return this._propertyAttributeTable[key];
			}
			LocalizabilityAttribute localizabilityAttribute = this._externalResolver.GetPropertyLocalizability(assembly, className, property);
			if (localizabilityAttribute == null)
			{
				localizabilityAttribute = this.DefaultAttribute;
			}
			this._propertyAttributeTable[key] = localizabilityAttribute;
			return localizabilityAttribute;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00136728 File Offset: 0x00135728
		public override string ResolveFormattingTagToClass(string formattingTag)
		{
			foreach (KeyValuePair<string, ElementLocalizability> keyValuePair in this._classAttributeTable)
			{
				if (keyValuePair.Value.FormattingTag == formattingTag)
				{
					return keyValuePair.Key;
				}
			}
			string text = null;
			if (this._externalResolver != null)
			{
				text = this._externalResolver.ResolveFormattingTagToClass(formattingTag);
				if (!string.IsNullOrEmpty(text))
				{
					if (this._classAttributeTable.ContainsKey(text))
					{
						this._classAttributeTable[text].FormattingTag = formattingTag;
					}
					else
					{
						this._classAttributeTable[text] = new ElementLocalizability(formattingTag, null);
					}
				}
			}
			return text;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x001367EC File Offset: 0x001357EC
		public override string ResolveAssemblyFromClass(string className)
		{
			if (className == null || className.Length == 0)
			{
				return string.Empty;
			}
			if (this._classNameToAssemblyIndex.Contains(className))
			{
				return this._assemblyNames[(int)this._classNameToAssemblyIndex[className]];
			}
			string text = null;
			if (this._externalResolver != null)
			{
				text = this._externalResolver.ResolveAssemblyFromClass(className);
				this.AddClassAndAssembly(className, text);
			}
			return text;
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00136855 File Offset: 0x00135855
		private LocalizabilityAttribute DefaultAttribute
		{
			get
			{
				return new LocalizabilityAttribute(LocalizationCategory.Inherit)
				{
					Modifiability = Modifiability.Inherit,
					Readability = Readability.Inherit
				};
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0013686C File Offset: 0x0013586C
		private InternalBamlLocalizabilityResolver.ElementComments LookupCommentForElement(BamlStartElementNode node)
		{
			if (node.Uid == null)
			{
				return new InternalBamlLocalizabilityResolver.ElementComments();
			}
			for (int i = 0; i < this._comments.Length; i++)
			{
				if (this._comments[i] != null && this._comments[i].ElementId == node.Uid)
				{
					return this._comments[i];
				}
			}
			InternalBamlLocalizabilityResolver.ElementComments elementComments = new InternalBamlLocalizabilityResolver.ElementComments();
			elementComments.ElementId = node.Uid;
			if (this._commentsDocument != null)
			{
				XmlElement xmlElement = InternalBamlLocalizabilityResolver.FindElementByID(this._commentsDocument, node.Uid);
				if (xmlElement != null)
				{
					string attribute = xmlElement.GetAttribute("Attributes");
					this.SetLocalizationAttributes(node, elementComments, attribute);
					attribute = xmlElement.GetAttribute("Comments");
					this.SetLocalizationComments(node, elementComments, attribute);
				}
			}
			if (node.Children != null)
			{
				int num = 0;
				while (num < node.Children.Count && (elementComments.LocalizationComments.Length == 0 || elementComments.LocalizationAttributes.Length == 0))
				{
					BamlTreeNode bamlTreeNode = node.Children[num];
					if (bamlTreeNode.NodeType == BamlNodeType.Property)
					{
						BamlPropertyNode bamlPropertyNode = (BamlPropertyNode)bamlTreeNode;
						if (LocComments.IsLocCommentsProperty(bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName) && elementComments.LocalizationComments.Length == 0)
						{
							this.SetLocalizationComments(node, elementComments, bamlPropertyNode.Value);
						}
						else if (LocComments.IsLocLocalizabilityProperty(bamlPropertyNode.OwnerTypeFullName, bamlPropertyNode.PropertyName) && elementComments.LocalizationAttributes.Length == 0)
						{
							this.SetLocalizationAttributes(node, elementComments, bamlPropertyNode.Value);
						}
					}
					num++;
				}
			}
			this._comments[this._commentsIndex] = elementComments;
			this._commentsIndex = (this._commentsIndex + 1) % this._comments.Length;
			return elementComments;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00136A04 File Offset: 0x00135A04
		private static XmlElement FindElementByID(XmlDocument doc, string uid)
		{
			if (doc != null && doc.DocumentElement != null)
			{
				foreach (object obj in doc.DocumentElement.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.NodeType == XmlNodeType.Element)
					{
						XmlElement xmlElement = (XmlElement)xmlNode;
						if (xmlElement.Name == "LocalizationDirectives" && xmlElement.GetAttribute("Uid") == uid)
						{
							return xmlElement;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00136AA8 File Offset: 0x00135AA8
		private void SetLocalizationAttributes(BamlStartElementNode node, InternalBamlLocalizabilityResolver.ElementComments comments, string attributes)
		{
			if (!string.IsNullOrEmpty(attributes))
			{
				try
				{
					comments.LocalizationAttributes = LocComments.ParsePropertyLocalizabilityAttributes(attributes);
				}
				catch (FormatException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(node), BamlLocalizerError.InvalidLocalizationAttributes));
				}
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x00136AF0 File Offset: 0x00135AF0
		private void SetLocalizationComments(BamlStartElementNode node, InternalBamlLocalizabilityResolver.ElementComments comments, string stringComment)
		{
			if (!string.IsNullOrEmpty(stringComment))
			{
				try
				{
					comments.LocalizationComments = LocComments.ParsePropertyComments(stringComment);
				}
				catch (FormatException)
				{
					this.RaiseErrorNotifyEvent(new BamlLocalizerErrorNotifyEventArgs(BamlTreeMap.GetKey(node), BamlLocalizerError.InvalidLocalizationComments));
				}
			}
		}

		// Token: 0x040009F8 RID: 2552
		private BamlLocalizabilityResolver _externalResolver;

		// Token: 0x040009F9 RID: 2553
		private FrugalObjectList<string> _assemblyNames;

		// Token: 0x040009FA RID: 2554
		private Hashtable _classNameToAssemblyIndex;

		// Token: 0x040009FB RID: 2555
		private Dictionary<string, ElementLocalizability> _classAttributeTable;

		// Token: 0x040009FC RID: 2556
		private Dictionary<string, LocalizabilityAttribute> _propertyAttributeTable;

		// Token: 0x040009FD RID: 2557
		private InternalBamlLocalizabilityResolver.ElementComments[] _comments;

		// Token: 0x040009FE RID: 2558
		private int _commentsIndex;

		// Token: 0x040009FF RID: 2559
		private XmlDocument _commentsDocument;

		// Token: 0x04000A00 RID: 2560
		private BamlLocalizer _localizer;

		// Token: 0x04000A01 RID: 2561
		private TextReader _commentingText;

		// Token: 0x020009CB RID: 2507
		private class ElementComments
		{
			// Token: 0x060083EA RID: 33770 RVA: 0x00324965 File Offset: 0x00323965
			internal ElementComments()
			{
				this.ElementId = null;
				this.LocalizationAttributes = Array.Empty<PropertyComment>();
				this.LocalizationComments = Array.Empty<PropertyComment>();
			}

			// Token: 0x04003FA9 RID: 16297
			internal string ElementId;

			// Token: 0x04003FAA RID: 16298
			internal PropertyComment[] LocalizationAttributes;

			// Token: 0x04003FAB RID: 16299
			internal PropertyComment[] LocalizationComments;
		}
	}
}
