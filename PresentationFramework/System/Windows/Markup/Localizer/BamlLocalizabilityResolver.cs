using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x0200053A RID: 1338
	public abstract class BamlLocalizabilityResolver
	{
		// Token: 0x0600423F RID: 16959
		public abstract ElementLocalizability GetElementLocalizability(string assembly, string className);

		// Token: 0x06004240 RID: 16960
		public abstract LocalizabilityAttribute GetPropertyLocalizability(string assembly, string className, string property);

		// Token: 0x06004241 RID: 16961
		public abstract string ResolveFormattingTagToClass(string formattingTag);

		// Token: 0x06004242 RID: 16962
		public abstract string ResolveAssemblyFromClass(string className);
	}
}
