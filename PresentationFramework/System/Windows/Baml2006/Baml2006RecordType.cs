using System;

namespace System.Windows.Baml2006
{
	// Token: 0x0200040C RID: 1036
	internal enum Baml2006RecordType : byte
	{
		// Token: 0x04001B6C RID: 7020
		Unknown,
		// Token: 0x04001B6D RID: 7021
		DocumentStart,
		// Token: 0x04001B6E RID: 7022
		DocumentEnd,
		// Token: 0x04001B6F RID: 7023
		ElementStart,
		// Token: 0x04001B70 RID: 7024
		ElementEnd,
		// Token: 0x04001B71 RID: 7025
		Property,
		// Token: 0x04001B72 RID: 7026
		PropertyCustom,
		// Token: 0x04001B73 RID: 7027
		PropertyComplexStart,
		// Token: 0x04001B74 RID: 7028
		PropertyComplexEnd,
		// Token: 0x04001B75 RID: 7029
		PropertyArrayStart,
		// Token: 0x04001B76 RID: 7030
		PropertyArrayEnd,
		// Token: 0x04001B77 RID: 7031
		PropertyIListStart,
		// Token: 0x04001B78 RID: 7032
		PropertyIListEnd,
		// Token: 0x04001B79 RID: 7033
		PropertyIDictionaryStart,
		// Token: 0x04001B7A RID: 7034
		PropertyIDictionaryEnd,
		// Token: 0x04001B7B RID: 7035
		LiteralContent,
		// Token: 0x04001B7C RID: 7036
		Text,
		// Token: 0x04001B7D RID: 7037
		TextWithConverter,
		// Token: 0x04001B7E RID: 7038
		RoutedEvent,
		// Token: 0x04001B7F RID: 7039
		ClrEvent,
		// Token: 0x04001B80 RID: 7040
		XmlnsProperty,
		// Token: 0x04001B81 RID: 7041
		XmlAttribute,
		// Token: 0x04001B82 RID: 7042
		ProcessingInstruction,
		// Token: 0x04001B83 RID: 7043
		Comment,
		// Token: 0x04001B84 RID: 7044
		DefTag,
		// Token: 0x04001B85 RID: 7045
		DefAttribute,
		// Token: 0x04001B86 RID: 7046
		EndAttributes,
		// Token: 0x04001B87 RID: 7047
		PIMapping,
		// Token: 0x04001B88 RID: 7048
		AssemblyInfo,
		// Token: 0x04001B89 RID: 7049
		TypeInfo,
		// Token: 0x04001B8A RID: 7050
		TypeSerializerInfo,
		// Token: 0x04001B8B RID: 7051
		AttributeInfo,
		// Token: 0x04001B8C RID: 7052
		StringInfo,
		// Token: 0x04001B8D RID: 7053
		PropertyStringReference,
		// Token: 0x04001B8E RID: 7054
		PropertyTypeReference,
		// Token: 0x04001B8F RID: 7055
		PropertyWithExtension,
		// Token: 0x04001B90 RID: 7056
		PropertyWithConverter,
		// Token: 0x04001B91 RID: 7057
		DeferableContentStart,
		// Token: 0x04001B92 RID: 7058
		DefAttributeKeyString,
		// Token: 0x04001B93 RID: 7059
		DefAttributeKeyType,
		// Token: 0x04001B94 RID: 7060
		KeyElementStart,
		// Token: 0x04001B95 RID: 7061
		KeyElementEnd,
		// Token: 0x04001B96 RID: 7062
		ConstructorParametersStart,
		// Token: 0x04001B97 RID: 7063
		ConstructorParametersEnd,
		// Token: 0x04001B98 RID: 7064
		ConstructorParameterType,
		// Token: 0x04001B99 RID: 7065
		ConnectionId,
		// Token: 0x04001B9A RID: 7066
		ContentProperty,
		// Token: 0x04001B9B RID: 7067
		NamedElementStart,
		// Token: 0x04001B9C RID: 7068
		StaticResourceStart,
		// Token: 0x04001B9D RID: 7069
		StaticResourceEnd,
		// Token: 0x04001B9E RID: 7070
		StaticResourceId,
		// Token: 0x04001B9F RID: 7071
		TextWithId,
		// Token: 0x04001BA0 RID: 7072
		PresentationOptionsAttribute,
		// Token: 0x04001BA1 RID: 7073
		LineNumberAndPosition,
		// Token: 0x04001BA2 RID: 7074
		LinePosition,
		// Token: 0x04001BA3 RID: 7075
		OptimizedStaticResource,
		// Token: 0x04001BA4 RID: 7076
		PropertyWithStaticResourceId,
		// Token: 0x04001BA5 RID: 7077
		LastRecordType
	}
}
