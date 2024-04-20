using System;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x020001B6 RID: 438
	internal class LocalizabilityGroup
	{
		// Token: 0x06000E3E RID: 3646 RVA: 0x001388C4 File Offset: 0x001378C4
		internal LocalizabilityGroup()
		{
			this.Modifiability = (Modifiability)(-1);
			this.Readability = (Readability)(-1);
			this.Category = (LocalizationCategory)(-1);
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x001388E4 File Offset: 0x001378E4
		internal LocalizabilityAttribute Override(LocalizabilityAttribute attribute)
		{
			Modifiability modifiability = attribute.Modifiability;
			Readability readability = attribute.Readability;
			LocalizationCategory category = attribute.Category;
			bool flag = false;
			if (this.Modifiability != (Modifiability)(-1))
			{
				modifiability = this.Modifiability;
				flag = true;
			}
			if (this.Readability != (Readability)(-1))
			{
				readability = this.Readability;
				flag = true;
			}
			if (this.Category != (LocalizationCategory)(-1))
			{
				category = this.Category;
				flag = true;
			}
			if (flag)
			{
				attribute = new LocalizabilityAttribute(category);
				attribute.Modifiability = modifiability;
				attribute.Readability = readability;
			}
			return attribute;
		}

		// Token: 0x04000A42 RID: 2626
		private const int InvalidValue = -1;

		// Token: 0x04000A43 RID: 2627
		internal Modifiability Modifiability;

		// Token: 0x04000A44 RID: 2628
		internal Readability Readability;

		// Token: 0x04000A45 RID: 2629
		internal LocalizationCategory Category;
	}
}
