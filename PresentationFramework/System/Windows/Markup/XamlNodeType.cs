using System;

namespace System.Windows.Markup
{
	// Token: 0x020004E1 RID: 1249
	internal enum XamlNodeType
	{
		// Token: 0x04002383 RID: 9091
		Unknown,
		// Token: 0x04002384 RID: 9092
		DocumentStart,
		// Token: 0x04002385 RID: 9093
		DocumentEnd,
		// Token: 0x04002386 RID: 9094
		ElementStart,
		// Token: 0x04002387 RID: 9095
		ElementEnd,
		// Token: 0x04002388 RID: 9096
		Property,
		// Token: 0x04002389 RID: 9097
		PropertyComplexStart,
		// Token: 0x0400238A RID: 9098
		PropertyComplexEnd,
		// Token: 0x0400238B RID: 9099
		PropertyArrayStart,
		// Token: 0x0400238C RID: 9100
		PropertyArrayEnd,
		// Token: 0x0400238D RID: 9101
		PropertyIListStart,
		// Token: 0x0400238E RID: 9102
		PropertyIListEnd,
		// Token: 0x0400238F RID: 9103
		PropertyIDictionaryStart,
		// Token: 0x04002390 RID: 9104
		PropertyIDictionaryEnd,
		// Token: 0x04002391 RID: 9105
		PropertyWithExtension,
		// Token: 0x04002392 RID: 9106
		PropertyWithType,
		// Token: 0x04002393 RID: 9107
		LiteralContent,
		// Token: 0x04002394 RID: 9108
		Text,
		// Token: 0x04002395 RID: 9109
		RoutedEvent,
		// Token: 0x04002396 RID: 9110
		ClrEvent,
		// Token: 0x04002397 RID: 9111
		XmlnsProperty,
		// Token: 0x04002398 RID: 9112
		XmlAttribute,
		// Token: 0x04002399 RID: 9113
		ProcessingInstruction,
		// Token: 0x0400239A RID: 9114
		Comment,
		// Token: 0x0400239B RID: 9115
		DefTag,
		// Token: 0x0400239C RID: 9116
		DefAttribute,
		// Token: 0x0400239D RID: 9117
		PresentationOptionsAttribute,
		// Token: 0x0400239E RID: 9118
		DefKeyTypeAttribute,
		// Token: 0x0400239F RID: 9119
		EndAttributes,
		// Token: 0x040023A0 RID: 9120
		PIMapping,
		// Token: 0x040023A1 RID: 9121
		UnknownTagStart,
		// Token: 0x040023A2 RID: 9122
		UnknownTagEnd,
		// Token: 0x040023A3 RID: 9123
		UnknownAttribute,
		// Token: 0x040023A4 RID: 9124
		KeyElementStart,
		// Token: 0x040023A5 RID: 9125
		KeyElementEnd,
		// Token: 0x040023A6 RID: 9126
		ConstructorParametersStart,
		// Token: 0x040023A7 RID: 9127
		ConstructorParametersEnd,
		// Token: 0x040023A8 RID: 9128
		ConstructorParameterType,
		// Token: 0x040023A9 RID: 9129
		ContentProperty
	}
}
