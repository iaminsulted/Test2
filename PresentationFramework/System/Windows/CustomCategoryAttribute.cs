using System;
using System.ComponentModel;

namespace System.Windows
{
	// Token: 0x0200033D RID: 829
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
	internal sealed class CustomCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06001F34 RID: 7988 RVA: 0x00171359 File Offset: 0x00170359
		internal CustomCategoryAttribute(string name) : base(name)
		{
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x00171362 File Offset: 0x00170362
		protected override string GetLocalizedString(string value)
		{
			if (string.Compare(value, "Content", StringComparison.Ordinal) == 0)
			{
				return SR.Get("DesignerMetadata_CustomCategory_Content");
			}
			if (string.Compare(value, "Accessibility", StringComparison.Ordinal) == 0)
			{
				return SR.Get("DesignerMetadata_CustomCategory_Accessibility");
			}
			return SR.Get("DesignerMetadata_CustomCategory_Navigation");
		}
	}
}
