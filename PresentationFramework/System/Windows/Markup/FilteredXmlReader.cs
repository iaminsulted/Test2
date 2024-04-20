using System;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x020004C1 RID: 1217
	internal class FilteredXmlReader : XmlTextReader
	{
		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x002013AC File Offset: 0x002003AC
		public override int AttributeCount
		{
			get
			{
				int attributeCount = base.AttributeCount;
				if (this.haveUid)
				{
					return attributeCount - 1;
				}
				return attributeCount;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06003E84 RID: 16004 RVA: 0x002013CD File Offset: 0x002003CD
		public override bool HasAttributes
		{
			get
			{
				return this.AttributeCount != 0;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		public override string this[int attributeIndex]
		{
			get
			{
				return this.GetAttribute(attributeIndex);
			}
		}

		// Token: 0x17000DD4 RID: 3540
		public override string this[string attributeName]
		{
			get
			{
				return this.GetAttribute(attributeName);
			}
		}

		// Token: 0x17000DD5 RID: 3541
		public override string this[string localName, string namespaceUri]
		{
			get
			{
				return this.GetAttribute(localName, namespaceUri);
			}
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x002013F4 File Offset: 0x002003F4
		public override string GetAttribute(int attributeIndex)
		{
			throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoIndexAttributeAccess"));
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x00201405 File Offset: 0x00200405
		public override string GetAttribute(string attributeName)
		{
			if (attributeName == this.uidQualifiedName)
			{
				return null;
			}
			return base.GetAttribute(attributeName);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x0020141E File Offset: 0x0020041E
		public override string GetAttribute(string localName, string namespaceUri)
		{
			if (localName == "Uid" && namespaceUri == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				return null;
			}
			return base.GetAttribute(localName, namespaceUri);
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x002013F4 File Offset: 0x002003F4
		public override void MoveToAttribute(int attributeIndex)
		{
			throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoIndexAttributeAccess"));
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00201444 File Offset: 0x00200444
		public override bool MoveToAttribute(string attributeName)
		{
			return !(attributeName == this.uidQualifiedName) && base.MoveToAttribute(attributeName);
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x0020145D File Offset: 0x0020045D
		public override bool MoveToAttribute(string localName, string namespaceUri)
		{
			return (!(localName == "Uid") || !(namespaceUri == "http://schemas.microsoft.com/winfx/2006/xaml")) && base.MoveToAttribute(localName, namespaceUri);
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x00201484 File Offset: 0x00200484
		public override bool MoveToFirstAttribute()
		{
			bool previousSuccessValue = base.MoveToFirstAttribute();
			return this.CheckForUidOrNamespaceRedef(previousSuccessValue);
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x002014A4 File Offset: 0x002004A4
		public override bool MoveToNextAttribute()
		{
			bool previousSuccessValue = base.MoveToNextAttribute();
			return this.CheckForUidOrNamespaceRedef(previousSuccessValue);
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x002014C1 File Offset: 0x002004C1
		public override bool Read()
		{
			bool flag = base.Read();
			if (flag)
			{
				this.CheckForUidAttribute();
			}
			return flag;
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x002014D2 File Offset: 0x002004D2
		internal FilteredXmlReader(string xmlFragment, XmlNodeType fragmentType, XmlParserContext context) : base(xmlFragment, fragmentType, context)
		{
			this.haveUid = false;
			this.uidPrefix = "def";
			this.uidQualifiedName = this.uidPrefix + ":Uid";
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00201505 File Offset: 0x00200505
		private void CheckForUidAttribute()
		{
			if (base.GetAttribute(this.uidQualifiedName) != null)
			{
				this.haveUid = true;
				return;
			}
			this.haveUid = false;
		}

		// Token: 0x06003E93 RID: 16019 RVA: 0x00201524 File Offset: 0x00200524
		private bool CheckForUidOrNamespaceRedef(bool previousSuccessValue)
		{
			bool flag = previousSuccessValue;
			if (flag && base.LocalName == "Uid" && base.NamespaceURI == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				this.CheckForPrefixUpdate();
				flag = base.MoveToNextAttribute();
			}
			this.CheckForNamespaceRedef();
			return flag;
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0020156E File Offset: 0x0020056E
		private void CheckForPrefixUpdate()
		{
			if (base.Prefix != this.uidPrefix)
			{
				this.uidPrefix = base.Prefix;
				this.uidQualifiedName = this.uidPrefix + ":Uid";
				this.CheckForUidAttribute();
			}
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x002015AC File Offset: 0x002005AC
		private void CheckForNamespaceRedef()
		{
			if (base.Prefix == "xmlns" && base.LocalName != this.uidPrefix && base.Value == "http://schemas.microsoft.com/winfx/2006/xaml")
			{
				throw new InvalidOperationException(SR.Get("ParserFilterXmlReaderNoDefinitionPrefixChangeAllowed"));
			}
		}

		// Token: 0x04001F27 RID: 7975
		private const string uidLocalName = "Uid";

		// Token: 0x04001F28 RID: 7976
		private const string uidNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";

		// Token: 0x04001F29 RID: 7977
		private const string defaultPrefix = "def";

		// Token: 0x04001F2A RID: 7978
		private string uidPrefix;

		// Token: 0x04001F2B RID: 7979
		private string uidQualifiedName;

		// Token: 0x04001F2C RID: 7980
		private bool haveUid;
	}
}
