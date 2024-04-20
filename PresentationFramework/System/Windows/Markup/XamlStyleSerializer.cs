using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x02000515 RID: 1301
	internal class XamlStyleSerializer : XamlSerializer
	{
		// Token: 0x060040B9 RID: 16569 RVA: 0x00214700 File Offset: 0x00213700
		internal override object GetDictionaryKey(BamlRecord startRecord, ParserContext parserContext)
		{
			Type result = Style.DefaultTargetType;
			bool flag = false;
			object obj = null;
			int num = 0;
			BamlRecord bamlRecord = startRecord;
			short ownerTypeId = 0;
			while (bamlRecord != null)
			{
				if (bamlRecord.RecordType == BamlRecordType.ElementStart)
				{
					BamlElementStartRecord bamlElementStartRecord = bamlRecord as BamlElementStartRecord;
					if (++num == 1)
					{
						ownerTypeId = bamlElementStartRecord.TypeId;
					}
					else if (num == 2)
					{
						result = parserContext.MapTable.GetTypeFromId(bamlElementStartRecord.TypeId);
						flag = true;
						break;
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.Property && num == 1)
				{
					BamlPropertyRecord bamlPropertyRecord = bamlRecord as BamlPropertyRecord;
					if (parserContext.MapTable.DoesAttributeMatch(bamlPropertyRecord.AttributeId, ownerTypeId, "TargetType"))
					{
						obj = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.PropertyComplexStart || bamlRecord.RecordType == BamlRecordType.PropertyIListStart)
				{
					break;
				}
				bamlRecord = bamlRecord.Next;
			}
			if (obj == null)
			{
				if (!flag)
				{
					this.ThrowException("StyleNoDictionaryKey", parserContext.LineNumber, parserContext.LinePosition);
				}
				return result;
			}
			return obj;
		}

		// Token: 0x060040BA RID: 16570 RVA: 0x002147F4 File Offset: 0x002137F4
		private void ThrowException(string id, int lineNumber, int linePosition)
		{
			string text = SR.Get(id);
			XamlParseException ex;
			if (lineNumber > 0)
			{
				text += " ";
				text += SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentCulture),
					linePosition.ToString(CultureInfo.CurrentCulture)
				});
				ex = new XamlParseException(text, lineNumber, linePosition);
			}
			else
			{
				ex = new XamlParseException(text);
			}
			throw ex;
		}

		// Token: 0x0400243F RID: 9279
		internal const string StyleTagName = "Style";

		// Token: 0x04002440 RID: 9280
		internal const string TargetTypePropertyName = "TargetType";

		// Token: 0x04002441 RID: 9281
		internal const string BasedOnPropertyName = "BasedOn";

		// Token: 0x04002442 RID: 9282
		internal const string VisualTriggersPropertyName = "Triggers";

		// Token: 0x04002443 RID: 9283
		internal const string ResourcesPropertyName = "Resources";

		// Token: 0x04002444 RID: 9284
		internal const string SettersPropertyName = "Setters";

		// Token: 0x04002445 RID: 9285
		internal const string VisualTriggersFullPropertyName = "Style.Triggers";

		// Token: 0x04002446 RID: 9286
		internal const string SettersFullPropertyName = "Style.Setters";

		// Token: 0x04002447 RID: 9287
		internal const string ResourcesFullPropertyName = "Style.Resources";

		// Token: 0x04002448 RID: 9288
		internal const string PropertyTriggerPropertyName = "Property";

		// Token: 0x04002449 RID: 9289
		internal const string PropertyTriggerValuePropertyName = "Value";

		// Token: 0x0400244A RID: 9290
		internal const string PropertyTriggerSourceName = "SourceName";

		// Token: 0x0400244B RID: 9291
		internal const string PropertyTriggerEnterActions = "EnterActions";

		// Token: 0x0400244C RID: 9292
		internal const string PropertyTriggerExitActions = "ExitActions";

		// Token: 0x0400244D RID: 9293
		internal const string DataTriggerBindingPropertyName = "Binding";

		// Token: 0x0400244E RID: 9294
		internal const string EventTriggerEventName = "RoutedEvent";

		// Token: 0x0400244F RID: 9295
		internal const string EventTriggerSourceName = "SourceName";

		// Token: 0x04002450 RID: 9296
		internal const string EventTriggerActions = "Actions";

		// Token: 0x04002451 RID: 9297
		internal const string MultiPropertyTriggerConditionsPropertyName = "Conditions";

		// Token: 0x04002452 RID: 9298
		internal const string SetterTagName = "Setter";

		// Token: 0x04002453 RID: 9299
		internal const string SetterPropertyAttributeName = "Property";

		// Token: 0x04002454 RID: 9300
		internal const string SetterValueAttributeName = "Value";

		// Token: 0x04002455 RID: 9301
		internal const string SetterTargetAttributeName = "TargetName";

		// Token: 0x04002456 RID: 9302
		internal const string SetterEventAttributeName = "Event";

		// Token: 0x04002457 RID: 9303
		internal const string SetterHandlerAttributeName = "Handler";
	}
}
