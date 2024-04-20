using System;

namespace System.Windows.Markup
{
	// Token: 0x02000481 RID: 1153
	internal enum BamlRecordType : byte
	{
		// Token: 0x04001E4B RID: 7755
		Unknown,
		// Token: 0x04001E4C RID: 7756
		DocumentStart,
		// Token: 0x04001E4D RID: 7757
		DocumentEnd,
		// Token: 0x04001E4E RID: 7758
		ElementStart,
		// Token: 0x04001E4F RID: 7759
		ElementEnd,
		// Token: 0x04001E50 RID: 7760
		Property,
		// Token: 0x04001E51 RID: 7761
		PropertyCustom,
		// Token: 0x04001E52 RID: 7762
		PropertyComplexStart,
		// Token: 0x04001E53 RID: 7763
		PropertyComplexEnd,
		// Token: 0x04001E54 RID: 7764
		PropertyArrayStart,
		// Token: 0x04001E55 RID: 7765
		PropertyArrayEnd,
		// Token: 0x04001E56 RID: 7766
		PropertyIListStart,
		// Token: 0x04001E57 RID: 7767
		PropertyIListEnd,
		// Token: 0x04001E58 RID: 7768
		PropertyIDictionaryStart,
		// Token: 0x04001E59 RID: 7769
		PropertyIDictionaryEnd,
		// Token: 0x04001E5A RID: 7770
		LiteralContent,
		// Token: 0x04001E5B RID: 7771
		Text,
		// Token: 0x04001E5C RID: 7772
		TextWithConverter,
		// Token: 0x04001E5D RID: 7773
		RoutedEvent,
		// Token: 0x04001E5E RID: 7774
		ClrEvent,
		// Token: 0x04001E5F RID: 7775
		XmlnsProperty,
		// Token: 0x04001E60 RID: 7776
		XmlAttribute,
		// Token: 0x04001E61 RID: 7777
		ProcessingInstruction,
		// Token: 0x04001E62 RID: 7778
		Comment,
		// Token: 0x04001E63 RID: 7779
		DefTag,
		// Token: 0x04001E64 RID: 7780
		DefAttribute,
		// Token: 0x04001E65 RID: 7781
		EndAttributes,
		// Token: 0x04001E66 RID: 7782
		PIMapping,
		// Token: 0x04001E67 RID: 7783
		AssemblyInfo,
		// Token: 0x04001E68 RID: 7784
		TypeInfo,
		// Token: 0x04001E69 RID: 7785
		TypeSerializerInfo,
		// Token: 0x04001E6A RID: 7786
		AttributeInfo,
		// Token: 0x04001E6B RID: 7787
		StringInfo,
		// Token: 0x04001E6C RID: 7788
		PropertyStringReference,
		// Token: 0x04001E6D RID: 7789
		PropertyTypeReference,
		// Token: 0x04001E6E RID: 7790
		PropertyWithExtension,
		// Token: 0x04001E6F RID: 7791
		PropertyWithConverter,
		// Token: 0x04001E70 RID: 7792
		DeferableContentStart,
		// Token: 0x04001E71 RID: 7793
		DefAttributeKeyString,
		// Token: 0x04001E72 RID: 7794
		DefAttributeKeyType,
		// Token: 0x04001E73 RID: 7795
		KeyElementStart,
		// Token: 0x04001E74 RID: 7796
		KeyElementEnd,
		// Token: 0x04001E75 RID: 7797
		ConstructorParametersStart,
		// Token: 0x04001E76 RID: 7798
		ConstructorParametersEnd,
		// Token: 0x04001E77 RID: 7799
		ConstructorParameterType,
		// Token: 0x04001E78 RID: 7800
		ConnectionId,
		// Token: 0x04001E79 RID: 7801
		ContentProperty,
		// Token: 0x04001E7A RID: 7802
		NamedElementStart,
		// Token: 0x04001E7B RID: 7803
		StaticResourceStart,
		// Token: 0x04001E7C RID: 7804
		StaticResourceEnd,
		// Token: 0x04001E7D RID: 7805
		StaticResourceId,
		// Token: 0x04001E7E RID: 7806
		TextWithId,
		// Token: 0x04001E7F RID: 7807
		PresentationOptionsAttribute,
		// Token: 0x04001E80 RID: 7808
		LineNumberAndPosition,
		// Token: 0x04001E81 RID: 7809
		LinePosition,
		// Token: 0x04001E82 RID: 7810
		OptimizedStaticResource,
		// Token: 0x04001E83 RID: 7811
		PropertyWithStaticResourceId,
		// Token: 0x04001E84 RID: 7812
		LastRecordType
	}
}
