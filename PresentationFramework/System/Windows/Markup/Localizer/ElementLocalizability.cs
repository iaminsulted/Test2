using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053B RID: 1339
	public class ElementLocalizability
	{
		// Token: 0x06004244 RID: 16964 RVA: 0x000F7BD9 File Offset: 0x000F6BD9
		public ElementLocalizability()
		{
		}

		// Token: 0x06004245 RID: 16965 RVA: 0x0021B318 File Offset: 0x0021A318
		public ElementLocalizability(string formattingTag, LocalizabilityAttribute attribute)
		{
			this._formattingTag = formattingTag;
			this._attribute = attribute;
		}

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x06004246 RID: 16966 RVA: 0x0021B32E File Offset: 0x0021A32E
		// (set) Token: 0x06004247 RID: 16967 RVA: 0x0021B336 File Offset: 0x0021A336
		public string FormattingTag
		{
			get
			{
				return this._formattingTag;
			}
			set
			{
				this._formattingTag = value;
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x0021B33F File Offset: 0x0021A33F
		// (set) Token: 0x06004249 RID: 16969 RVA: 0x0021B347 File Offset: 0x0021A347
		public LocalizabilityAttribute Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				this._attribute = value;
			}
		}

		// Token: 0x040024F3 RID: 9459
		private string _formattingTag;

		// Token: 0x040024F4 RID: 9460
		private LocalizabilityAttribute _attribute;
	}
}
