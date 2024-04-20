using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200015B RID: 347
	internal class ElementTableKey
	{
		// Token: 0x06000B8C RID: 2956 RVA: 0x0012CBCD File Offset: 0x0012BBCD
		internal ElementTableKey(string xmlNamespace, string baseName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			this._xmlNamespace = xmlNamespace;
			this._baseName = baseName;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0012CC00 File Offset: 0x0012BC00
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (other.GetType() != base.GetType())
			{
				return false;
			}
			ElementTableKey elementTableKey = (ElementTableKey)other;
			return string.CompareOrdinal(this.BaseName, elementTableKey.BaseName) == 0 && string.CompareOrdinal(this.XmlNamespace, elementTableKey.XmlNamespace) == 0;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0012CC57 File Offset: 0x0012BC57
		public override int GetHashCode()
		{
			return this.XmlNamespace.GetHashCode() ^ this.BaseName.GetHashCode();
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B8F RID: 2959 RVA: 0x0012CC70 File Offset: 0x0012BC70
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x0012CC78 File Offset: 0x0012BC78
		internal string BaseName
		{
			get
			{
				return this._baseName;
			}
		}

		// Token: 0x040008CC RID: 2252
		private string _baseName;

		// Token: 0x040008CD RID: 2253
		private string _xmlNamespace;

		// Token: 0x040008CE RID: 2254
		public static readonly string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x040008CF RID: 2255
		public static readonly string FixedMarkupNamespace = "http://schemas.microsoft.com/xps/2005/06";
	}
}
