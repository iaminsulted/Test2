using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using MS.Internal.Annotations;
using MS.Internal.Annotations.Anchoring;

namespace System.Windows.Annotations
{
	// Token: 0x02000872 RID: 2162
	public sealed class ContentLocatorPart : INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		// Token: 0x06007FA0 RID: 32672 RVA: 0x0031F8C4 File Offset: 0x0031E8C4
		public ContentLocatorPart(XmlQualifiedName partType)
		{
			if (partType == null)
			{
				throw new ArgumentNullException("partType");
			}
			if (string.IsNullOrEmpty(partType.Name))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "partType.Name");
			}
			if (string.IsNullOrEmpty(partType.Namespace))
			{
				throw new ArgumentException(SR.Get("TypeNameMustBeSpecified"), "partType.Namespace");
			}
			this._type = partType;
			this._nameValues = new ObservableDictionary();
			this._nameValues.PropertyChanged += this.OnPropertyChanged;
		}

		// Token: 0x06007FA1 RID: 32673 RVA: 0x0031F958 File Offset: 0x0031E958
		public override bool Equals(object obj)
		{
			ContentLocatorPart contentLocatorPart = obj as ContentLocatorPart;
			if (contentLocatorPart == this)
			{
				return true;
			}
			if (contentLocatorPart == null)
			{
				return false;
			}
			if (!this._type.Equals(contentLocatorPart.PartType))
			{
				return false;
			}
			if (contentLocatorPart.NameValuePairs.Count != this._nameValues.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, string> keyValuePair in this._nameValues)
			{
				string b;
				if (!contentLocatorPart._nameValues.TryGetValue(keyValuePair.Key, out b))
				{
					return false;
				}
				if (keyValuePair.Value != b)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007FA2 RID: 32674 RVA: 0x0031A200 File Offset: 0x00319200
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06007FA3 RID: 32675 RVA: 0x0031FA14 File Offset: 0x0031EA14
		public object Clone()
		{
			ContentLocatorPart contentLocatorPart = new ContentLocatorPart(this._type);
			foreach (KeyValuePair<string, string> keyValuePair in this._nameValues)
			{
				contentLocatorPart.NameValuePairs.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return contentLocatorPart;
		}

		// Token: 0x17001D6C RID: 7532
		// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x0031FA80 File Offset: 0x0031EA80
		public IDictionary<string, string> NameValuePairs
		{
			get
			{
				return this._nameValues;
			}
		}

		// Token: 0x17001D6D RID: 7533
		// (get) Token: 0x06007FA5 RID: 32677 RVA: 0x0031FA88 File Offset: 0x0031EA88
		public XmlQualifiedName PartType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1400016B RID: 363
		// (add) Token: 0x06007FA6 RID: 32678 RVA: 0x0031FA90 File Offset: 0x0031EA90
		// (remove) Token: 0x06007FA7 RID: 32679 RVA: 0x0031FA99 File Offset: 0x0031EA99
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x06007FA8 RID: 32680 RVA: 0x0031FAA4 File Offset: 0x0031EAA4
		internal bool Matches(ContentLocatorPart part)
		{
			bool flag = false;
			string value;
			this._nameValues.TryGetValue("IncludeOverlaps", out value);
			if (!bool.TryParse(value, out flag) || !flag)
			{
				return this.Equals(part);
			}
			if (part == this)
			{
				return true;
			}
			if (!this._type.Equals(part.PartType))
			{
				return false;
			}
			int num;
			int num2;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(this, out num, out num2);
			int num3;
			int num4;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(part, out num3, out num4);
			return (num == num3 && num2 == num4) || (num != int.MinValue && ((num3 >= num && num3 <= num2) || (num3 < num && num4 >= num)));
		}

		// Token: 0x06007FA9 RID: 32681 RVA: 0x0031FB38 File Offset: 0x0031EB38
		internal string GetQueryFragment(XmlNamespaceManager namespaceManager)
		{
			bool flag = false;
			string value;
			this._nameValues.TryGetValue("IncludeOverlaps", out value);
			if (bool.TryParse(value, out flag) && flag)
			{
				return this.GetOverlapQueryFragment(namespaceManager);
			}
			return this.GetExactQueryFragment(namespaceManager);
		}

		// Token: 0x17001D6E RID: 7534
		// (get) Token: 0x06007FAA RID: 32682 RVA: 0x0031FB75 File Offset: 0x0031EB75
		// (set) Token: 0x06007FAB RID: 32683 RVA: 0x0031FB7D File Offset: 0x0031EB7D
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

		// Token: 0x06007FAC RID: 32684 RVA: 0x0031FB86 File Offset: 0x0031EB86
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs("NameValuePairs"));
			}
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x0031FBA8 File Offset: 0x0031EBA8
		private string GetOverlapQueryFragment(XmlNamespaceManager namespaceManager)
		{
			string text = namespaceManager.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core");
			string text2 = namespaceManager.LookupPrefix(this.PartType.Namespace);
			string text3 = (text2 == null) ? "" : (text2 + ":");
			text3 = string.Concat(new string[]
			{
				text3,
				TextSelectionProcessor.CharacterRangeElementName.Name,
				"/",
				text,
				":Item"
			});
			int num;
			int num2;
			TextSelectionProcessor.GetMaxMinLocatorPartValues(this, out num, out num2);
			string text4 = num.ToString(NumberFormatInfo.InvariantInfo);
			string text5 = num2.ToString(NumberFormatInfo.InvariantInfo);
			return string.Concat(new string[]
			{
				text3,
				"[starts-with(@Name, \"Segment\") and  ((substring-before(@Value,\",\") >= ",
				text4,
				" and substring-before(@Value,\",\") <= ",
				text5,
				") or   (substring-before(@Value,\",\") < ",
				text4,
				" and substring-after(@Value,\",\") >= ",
				text4,
				"))]"
			});
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x0031FC90 File Offset: 0x0031EC90
		private string GetExactQueryFragment(XmlNamespaceManager namespaceManager)
		{
			string str = namespaceManager.LookupPrefix("http://schemas.microsoft.com/windows/annotations/2003/11/core");
			string text = namespaceManager.LookupPrefix(this.PartType.Namespace);
			string text2 = (text == null) ? "" : (text + ":");
			text2 += this.PartType.Name;
			bool flag = false;
			foreach (KeyValuePair<string, string> keyValuePair in this.NameValuePairs)
			{
				if (flag)
				{
					text2 = text2 + "/parent::*/" + str + ":Item[";
				}
				else
				{
					flag = true;
					text2 = text2 + "/" + str + ":Item[";
				}
				text2 = string.Concat(new string[]
				{
					text2,
					"@Name=\"",
					keyValuePair.Key,
					"\" and @Value=\"",
					keyValuePair.Value,
					"\"]"
				});
			}
			if (flag)
			{
				text2 += "/parent::*";
			}
			return text2;
		}

		// Token: 0x1400016C RID: 364
		// (add) Token: 0x06007FAF RID: 32687 RVA: 0x0031FD9C File Offset: 0x0031ED9C
		// (remove) Token: 0x06007FB0 RID: 32688 RVA: 0x0031FDD4 File Offset: 0x0031EDD4
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x04003B8E RID: 15246
		private bool _owned;

		// Token: 0x04003B8F RID: 15247
		private XmlQualifiedName _type;

		// Token: 0x04003B90 RID: 15248
		private ObservableDictionary _nameValues;
	}
}
