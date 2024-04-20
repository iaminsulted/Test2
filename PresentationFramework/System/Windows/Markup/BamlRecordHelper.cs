using System;

namespace System.Windows.Markup
{
	// Token: 0x0200047C RID: 1148
	internal static class BamlRecordHelper
	{
		// Token: 0x06003B3E RID: 15166 RVA: 0x001F5A09 File Offset: 0x001F4A09
		internal static bool IsMapTableRecordType(BamlRecordType bamlRecordType)
		{
			return bamlRecordType - BamlRecordType.PIMapping <= 5;
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x001F5A15 File Offset: 0x001F4A15
		internal static bool IsDebugBamlRecordType(BamlRecordType recordType)
		{
			return recordType == BamlRecordType.LineNumberAndPosition || recordType == BamlRecordType.LinePosition;
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x001F5A24 File Offset: 0x001F4A24
		internal static bool HasDebugExtensionRecord(bool isDebugBamlStream, BamlRecord bamlRecord)
		{
			return isDebugBamlStream && bamlRecord.Next != null && BamlRecordHelper.IsDebugBamlRecordType(bamlRecord.Next.RecordType);
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x001F5A48 File Offset: 0x001F4A48
		internal static bool DoesRecordTypeHaveDebugExtension(BamlRecordType recordType)
		{
			switch (recordType)
			{
			case BamlRecordType.DocumentStart:
			case BamlRecordType.DocumentEnd:
			case BamlRecordType.PropertyCustom:
			case BamlRecordType.PropertyComplexEnd:
			case BamlRecordType.PropertyArrayEnd:
			case BamlRecordType.PropertyIListEnd:
			case BamlRecordType.PropertyIDictionaryEnd:
			case BamlRecordType.LiteralContent:
			case BamlRecordType.Text:
			case BamlRecordType.TextWithConverter:
			case BamlRecordType.RoutedEvent:
			case BamlRecordType.ClrEvent:
			case BamlRecordType.XmlAttribute:
			case BamlRecordType.ProcessingInstruction:
			case BamlRecordType.Comment:
			case BamlRecordType.DefTag:
			case BamlRecordType.DefAttribute:
			case BamlRecordType.EndAttributes:
			case BamlRecordType.AssemblyInfo:
			case BamlRecordType.TypeInfo:
			case BamlRecordType.TypeSerializerInfo:
			case BamlRecordType.AttributeInfo:
			case BamlRecordType.StringInfo:
			case BamlRecordType.PropertyStringReference:
			case BamlRecordType.DeferableContentStart:
			case BamlRecordType.DefAttributeKeyString:
			case BamlRecordType.DefAttributeKeyType:
			case BamlRecordType.KeyElementEnd:
			case BamlRecordType.ConstructorParametersStart:
			case BamlRecordType.ConstructorParametersEnd:
			case BamlRecordType.ConstructorParameterType:
			case BamlRecordType.NamedElementStart:
			case BamlRecordType.StaticResourceEnd:
			case BamlRecordType.StaticResourceId:
			case BamlRecordType.TextWithId:
			case BamlRecordType.LineNumberAndPosition:
			case BamlRecordType.LinePosition:
			case BamlRecordType.OptimizedStaticResource:
			case BamlRecordType.PropertyWithStaticResourceId:
				return false;
			case BamlRecordType.ElementStart:
			case BamlRecordType.ElementEnd:
			case BamlRecordType.Property:
			case BamlRecordType.PropertyComplexStart:
			case BamlRecordType.PropertyArrayStart:
			case BamlRecordType.PropertyIListStart:
			case BamlRecordType.PropertyIDictionaryStart:
			case BamlRecordType.XmlnsProperty:
			case BamlRecordType.PIMapping:
			case BamlRecordType.PropertyTypeReference:
			case BamlRecordType.PropertyWithExtension:
			case BamlRecordType.PropertyWithConverter:
			case BamlRecordType.KeyElementStart:
			case BamlRecordType.ConnectionId:
			case BamlRecordType.ContentProperty:
			case BamlRecordType.StaticResourceStart:
			case BamlRecordType.PresentationOptionsAttribute:
				return true;
			default:
				return false;
			}
		}
	}
}
