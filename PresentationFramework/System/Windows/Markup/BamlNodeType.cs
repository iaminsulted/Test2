using System;

namespace System.Windows.Markup
{
	// Token: 0x0200047A RID: 1146
	internal enum BamlNodeType
	{
		// Token: 0x04001DE0 RID: 7648
		None,
		// Token: 0x04001DE1 RID: 7649
		StartDocument,
		// Token: 0x04001DE2 RID: 7650
		EndDocument,
		// Token: 0x04001DE3 RID: 7651
		ConnectionId,
		// Token: 0x04001DE4 RID: 7652
		StartElement,
		// Token: 0x04001DE5 RID: 7653
		EndElement,
		// Token: 0x04001DE6 RID: 7654
		Property,
		// Token: 0x04001DE7 RID: 7655
		ContentProperty,
		// Token: 0x04001DE8 RID: 7656
		XmlnsProperty,
		// Token: 0x04001DE9 RID: 7657
		StartComplexProperty,
		// Token: 0x04001DEA RID: 7658
		EndComplexProperty,
		// Token: 0x04001DEB RID: 7659
		LiteralContent,
		// Token: 0x04001DEC RID: 7660
		Text,
		// Token: 0x04001DED RID: 7661
		RoutedEvent,
		// Token: 0x04001DEE RID: 7662
		Event,
		// Token: 0x04001DEF RID: 7663
		IncludeReference,
		// Token: 0x04001DF0 RID: 7664
		DefAttribute,
		// Token: 0x04001DF1 RID: 7665
		PresentationOptionsAttribute,
		// Token: 0x04001DF2 RID: 7666
		PIMapping,
		// Token: 0x04001DF3 RID: 7667
		StartConstructor,
		// Token: 0x04001DF4 RID: 7668
		EndConstructor
	}
}
