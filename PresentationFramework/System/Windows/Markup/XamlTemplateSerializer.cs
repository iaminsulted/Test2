using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x02000516 RID: 1302
	internal class XamlTemplateSerializer : XamlSerializer
	{
		// Token: 0x060040BC RID: 16572 RVA: 0x00214860 File Offset: 0x00213860
		internal override object GetDictionaryKey(BamlRecord startRecord, ParserContext parserContext)
		{
			object obj = null;
			int num = 0;
			BamlRecord bamlRecord = startRecord;
			short num2 = 0;
			while (bamlRecord != null)
			{
				if (bamlRecord.RecordType == BamlRecordType.ElementStart)
				{
					BamlElementStartRecord bamlElementStartRecord = bamlRecord as BamlElementStartRecord;
					if (++num != 1)
					{
						break;
					}
					num2 = bamlElementStartRecord.TypeId;
				}
				else if (bamlRecord.RecordType == BamlRecordType.Property && num == 1)
				{
					BamlPropertyRecord bamlPropertyRecord = bamlRecord as BamlPropertyRecord;
					short num3;
					string a;
					BamlAttributeUsage bamlAttributeUsage;
					parserContext.MapTable.GetAttributeInfoFromId(bamlPropertyRecord.AttributeId, out num3, out a, out bamlAttributeUsage);
					if (num3 == num2)
					{
						if (a == "TargetType")
						{
							obj = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
						}
						else if (a == "DataType")
						{
							object dictionaryKey = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
							Exception ex = TemplateKey.ValidateDataType(dictionaryKey, null);
							if (ex != null)
							{
								this.ThrowException("TemplateBadDictionaryKey", parserContext.LineNumber, parserContext.LinePosition, ex);
							}
							obj = new DataTemplateKey(dictionaryKey);
						}
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.PropertyComplexStart || bamlRecord.RecordType == BamlRecordType.PropertyIListStart || bamlRecord.RecordType == BamlRecordType.ElementEnd)
				{
					break;
				}
				bamlRecord = bamlRecord.Next;
			}
			if (obj == null)
			{
				this.ThrowException("StyleNoDictionaryKey", parserContext.LineNumber, parserContext.LinePosition, null);
			}
			return obj;
		}

		// Token: 0x060040BD RID: 16573 RVA: 0x00214998 File Offset: 0x00213998
		private void ThrowException(string id, int lineNumber, int linePosition, Exception innerException)
		{
			string text = SR.Get(id);
			XamlParseException ex;
			if (lineNumber > 0)
			{
				text += " ";
				text += SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentUICulture),
					linePosition.ToString(CultureInfo.CurrentUICulture)
				});
				ex = new XamlParseException(text, lineNumber, linePosition);
			}
			else
			{
				ex = new XamlParseException(text);
			}
			throw ex;
		}

		// Token: 0x04002458 RID: 9304
		internal const string ControlTemplateTagName = "ControlTemplate";

		// Token: 0x04002459 RID: 9305
		internal const string DataTemplateTagName = "DataTemplate";

		// Token: 0x0400245A RID: 9306
		internal const string HierarchicalDataTemplateTagName = "HierarchicalDataTemplate";

		// Token: 0x0400245B RID: 9307
		internal const string ItemsPanelTemplateTagName = "ItemsPanelTemplate";

		// Token: 0x0400245C RID: 9308
		internal const string TargetTypePropertyName = "TargetType";

		// Token: 0x0400245D RID: 9309
		internal const string DataTypePropertyName = "DataType";

		// Token: 0x0400245E RID: 9310
		internal const string TriggersPropertyName = "Triggers";

		// Token: 0x0400245F RID: 9311
		internal const string ResourcesPropertyName = "Resources";

		// Token: 0x04002460 RID: 9312
		internal const string SettersPropertyName = "Setters";

		// Token: 0x04002461 RID: 9313
		internal const string ItemsSourcePropertyName = "ItemsSource";

		// Token: 0x04002462 RID: 9314
		internal const string ItemTemplatePropertyName = "ItemTemplate";

		// Token: 0x04002463 RID: 9315
		internal const string ItemTemplateSelectorPropertyName = "ItemTemplateSelector";

		// Token: 0x04002464 RID: 9316
		internal const string ItemContainerStylePropertyName = "ItemContainerStyle";

		// Token: 0x04002465 RID: 9317
		internal const string ItemContainerStyleSelectorPropertyName = "ItemContainerStyleSelector";

		// Token: 0x04002466 RID: 9318
		internal const string ItemStringFormatPropertyName = "ItemStringFormat";

		// Token: 0x04002467 RID: 9319
		internal const string ItemBindingGroupPropertyName = "ItemBindingGroup";

		// Token: 0x04002468 RID: 9320
		internal const string AlternationCountPropertyName = "AlternationCount";

		// Token: 0x04002469 RID: 9321
		internal const string ControlTemplateTriggersFullPropertyName = "ControlTemplate.Triggers";

		// Token: 0x0400246A RID: 9322
		internal const string ControlTemplateResourcesFullPropertyName = "ControlTemplate.Resources";

		// Token: 0x0400246B RID: 9323
		internal const string DataTemplateTriggersFullPropertyName = "DataTemplate.Triggers";

		// Token: 0x0400246C RID: 9324
		internal const string DataTemplateResourcesFullPropertyName = "DataTemplate.Resources";

		// Token: 0x0400246D RID: 9325
		internal const string HierarchicalDataTemplateTriggersFullPropertyName = "HierarchicalDataTemplate.Triggers";

		// Token: 0x0400246E RID: 9326
		internal const string HierarchicalDataTemplateItemsSourceFullPropertyName = "HierarchicalDataTemplate.ItemsSource";

		// Token: 0x0400246F RID: 9327
		internal const string HierarchicalDataTemplateItemTemplateFullPropertyName = "HierarchicalDataTemplate.ItemTemplate";

		// Token: 0x04002470 RID: 9328
		internal const string HierarchicalDataTemplateItemTemplateSelectorFullPropertyName = "HierarchicalDataTemplate.ItemTemplateSelector";

		// Token: 0x04002471 RID: 9329
		internal const string HierarchicalDataTemplateItemContainerStyleFullPropertyName = "HierarchicalDataTemplate.ItemContainerStyle";

		// Token: 0x04002472 RID: 9330
		internal const string HierarchicalDataTemplateItemContainerStyleSelectorFullPropertyName = "HierarchicalDataTemplate.ItemContainerStyleSelector";

		// Token: 0x04002473 RID: 9331
		internal const string HierarchicalDataTemplateItemStringFormatFullPropertyName = "HierarchicalDataTemplate.ItemStringFormat";

		// Token: 0x04002474 RID: 9332
		internal const string HierarchicalDataTemplateItemBindingGroupFullPropertyName = "HierarchicalDataTemplate.ItemBindingGroup";

		// Token: 0x04002475 RID: 9333
		internal const string HierarchicalDataTemplateAlternationCountFullPropertyName = "HierarchicalDataTemplate.AlternationCount";

		// Token: 0x04002476 RID: 9334
		internal const string PropertyTriggerPropertyName = "Property";

		// Token: 0x04002477 RID: 9335
		internal const string PropertyTriggerValuePropertyName = "Value";

		// Token: 0x04002478 RID: 9336
		internal const string PropertyTriggerSourceName = "SourceName";

		// Token: 0x04002479 RID: 9337
		internal const string PropertyTriggerEnterActions = "EnterActions";

		// Token: 0x0400247A RID: 9338
		internal const string PropertyTriggerExitActions = "ExitActions";

		// Token: 0x0400247B RID: 9339
		internal const string DataTriggerBindingPropertyName = "Binding";

		// Token: 0x0400247C RID: 9340
		internal const string EventTriggerEventName = "RoutedEvent";

		// Token: 0x0400247D RID: 9341
		internal const string EventTriggerSourceName = "SourceName";

		// Token: 0x0400247E RID: 9342
		internal const string EventTriggerActions = "Actions";

		// Token: 0x0400247F RID: 9343
		internal const string MultiPropertyTriggerConditionsPropertyName = "Conditions";

		// Token: 0x04002480 RID: 9344
		internal const string SetterTagName = "Setter";

		// Token: 0x04002481 RID: 9345
		internal const string SetterPropertyAttributeName = "Property";

		// Token: 0x04002482 RID: 9346
		internal const string SetterValueAttributeName = "Value";

		// Token: 0x04002483 RID: 9347
		internal const string SetterTargetAttributeName = "TargetName";

		// Token: 0x04002484 RID: 9348
		internal const string SetterEventAttributeName = "Event";

		// Token: 0x04002485 RID: 9349
		internal const string SetterHandlerAttributeName = "Handler";
	}
}
