using System;

namespace System.Windows.Markup.Localizer
{
	// Token: 0x02000542 RID: 1346
	public enum BamlLocalizerError
	{
		// Token: 0x04002504 RID: 9476
		DuplicateUid,
		// Token: 0x04002505 RID: 9477
		DuplicateElement,
		// Token: 0x04002506 RID: 9478
		IncompleteElementPlaceholder,
		// Token: 0x04002507 RID: 9479
		InvalidCommentingXml,
		// Token: 0x04002508 RID: 9480
		InvalidLocalizationAttributes,
		// Token: 0x04002509 RID: 9481
		InvalidLocalizationComments,
		// Token: 0x0400250A RID: 9482
		InvalidUid,
		// Token: 0x0400250B RID: 9483
		MismatchedElements,
		// Token: 0x0400250C RID: 9484
		SubstitutionAsPlaintext,
		// Token: 0x0400250D RID: 9485
		UidMissingOnChildElement,
		// Token: 0x0400250E RID: 9486
		UnknownFormattingTag
	}
}
