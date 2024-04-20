using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Documents
{
	// Token: 0x0200060D RID: 1549
	internal sealed class FixedSOMPage : FixedSOMContainer
	{
		// Token: 0x06004B65 RID: 19301 RVA: 0x00236F00 File Offset: 0x00235F00
		public void AddFixedBlock(FixedSOMFixedBlock fixedBlock)
		{
			base.Add(fixedBlock);
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x00236F00 File Offset: 0x00235F00
		public void AddTable(FixedSOMTable table)
		{
			base.Add(table);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x00236F09 File Offset: 0x00235F09
		public override void SetRTFProperties(FixedElement element)
		{
			if (this._cultureInfo != null)
			{
				element.SetValue(FrameworkContentElement.LanguageProperty, XmlLanguage.GetLanguage(this._cultureInfo.IetfLanguageTag));
			}
		}

		// Token: 0x17001155 RID: 4437
		// (get) Token: 0x06004B68 RID: 19304 RVA: 0x00236F2E File Offset: 0x00235F2E
		internal override FixedElement.ElementType[] ElementTypes
		{
			get
			{
				return new FixedElement.ElementType[]
				{
					FixedElement.ElementType.Section
				};
			}
		}

		// Token: 0x17001156 RID: 4438
		// (get) Token: 0x06004B69 RID: 19305 RVA: 0x00236F3B File Offset: 0x00235F3B
		// (set) Token: 0x06004B6A RID: 19306 RVA: 0x00236F43 File Offset: 0x00235F43
		internal List<FixedNode> MarkupOrder
		{
			get
			{
				return this._markupOrder;
			}
			set
			{
				this._markupOrder = value;
			}
		}

		// Token: 0x17001157 RID: 4439
		// (set) Token: 0x06004B6B RID: 19307 RVA: 0x00236F4C File Offset: 0x00235F4C
		internal CultureInfo CultureInfo
		{
			set
			{
				this._cultureInfo = value;
			}
		}

		// Token: 0x04002781 RID: 10113
		private List<FixedNode> _markupOrder;

		// Token: 0x04002782 RID: 10114
		private CultureInfo _cultureInfo;
	}
}
