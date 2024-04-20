using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Internal.Data;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020003A1 RID: 929
	internal static class StyleHelper
	{
		// Token: 0x060025A7 RID: 9639 RVA: 0x001875AC File Offset: 0x001865AC
		internal static void UpdateStyleCache(FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle, ref Style styleCache)
		{
			if (newStyle != null)
			{
				DependencyObject dependencyObject = fe;
				if (dependencyObject == null)
				{
					dependencyObject = fce;
				}
				newStyle.CheckTargetType(dependencyObject);
				newStyle.Seal();
			}
			styleCache = newStyle;
			StyleHelper.DoStyleInvalidations(fe, fce, oldStyle, newStyle);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, fce, newStyle, StyleHelper.StyleDataField);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x001875EC File Offset: 0x001865EC
		internal static void UpdateThemeStyleCache(FrameworkElement fe, FrameworkContentElement fce, Style oldThemeStyle, Style newThemeStyle, ref Style themeStyleCache)
		{
			if (newThemeStyle != null)
			{
				DependencyObject dependencyObject = fe;
				if (dependencyObject == null)
				{
					dependencyObject = fce;
				}
				newThemeStyle.CheckTargetType(dependencyObject);
				newThemeStyle.Seal();
				if (StyleHelper.IsSetOnContainer(FrameworkElement.OverridesDefaultStyleProperty, ref newThemeStyle.ContainerDependents, true))
				{
					throw new InvalidOperationException(SR.Get("CannotHaveOverridesDefaultStyleInThemeStyle"));
				}
				if (newThemeStyle.HasEventSetters)
				{
					throw new InvalidOperationException(SR.Get("CannotHaveEventHandlersInThemeStyle"));
				}
			}
			themeStyleCache = newThemeStyle;
			Style style = null;
			if (fe != null)
			{
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkElement.DefaultStyleKeyProperty))
				{
					style = fe.Style;
				}
			}
			else if (StyleHelper.ShouldGetValueFromStyle(FrameworkContentElement.DefaultStyleKeyProperty))
			{
				style = fce.Style;
			}
			StyleHelper.DoThemeStyleInvalidations(fe, fce, oldThemeStyle, newThemeStyle, style);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, fce, newThemeStyle, StyleHelper.ThemeStyleDataField);
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00187694 File Offset: 0x00186694
		internal static Style GetThemeStyle(FrameworkElement fe, FrameworkContentElement fce)
		{
			Style style = null;
			object defaultStyleKey;
			bool overridesDefaultStyle;
			Style themeStyle;
			if (fe != null)
			{
				fe.HasThemeStyleEverBeenFetched = true;
				defaultStyleKey = fe.DefaultStyleKey;
				overridesDefaultStyle = fe.OverridesDefaultStyle;
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkElement.DefaultStyleKeyProperty))
				{
					Style style2 = fe.Style;
				}
				themeStyle = fe.ThemeStyle;
			}
			else
			{
				fce.HasThemeStyleEverBeenFetched = true;
				defaultStyleKey = fce.DefaultStyleKey;
				overridesDefaultStyle = fce.OverridesDefaultStyle;
				if (StyleHelper.ShouldGetValueFromStyle(FrameworkContentElement.DefaultStyleKeyProperty))
				{
					Style style3 = fce.Style;
				}
				themeStyle = fce.ThemeStyle;
			}
			if (defaultStyleKey != null && !overridesDefaultStyle)
			{
				DependencyObjectType dtypeThemeStyleKey;
				if (fe != null)
				{
					dtypeThemeStyleKey = fe.DTypeThemeStyleKey;
				}
				else
				{
					dtypeThemeStyleKey = fce.DTypeThemeStyleKey;
				}
				object obj;
				if (dtypeThemeStyleKey != null && dtypeThemeStyleKey.SystemType != null && dtypeThemeStyleKey.SystemType.Equals(defaultStyleKey))
				{
					obj = SystemResources.FindThemeStyle(dtypeThemeStyleKey);
				}
				else
				{
					obj = SystemResources.FindResourceInternal(defaultStyleKey);
				}
				if (obj != null)
				{
					if (!(obj is Style))
					{
						throw new InvalidOperationException(SR.Get("SystemResourceForTypeIsNotStyle", new object[]
						{
							defaultStyleKey
						}));
					}
					style = (Style)obj;
				}
				if (style == null)
				{
					Type type = defaultStyleKey as Type;
					if (type != null)
					{
						PropertyMetadata metadata = FrameworkElement.StyleProperty.GetMetadata(type);
						if (metadata != null)
						{
							style = (metadata.DefaultValue as Style);
						}
					}
				}
			}
			if (themeStyle != style)
			{
				if (fe != null)
				{
					FrameworkElement.OnThemeStyleChanged(fe, themeStyle, style);
				}
				else
				{
					FrameworkContentElement.OnThemeStyleChanged(fce, themeStyle, style);
				}
			}
			return style;
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x001877DF File Offset: 0x001867DF
		internal static void UpdateTemplateCache(FrameworkElement fe, FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate, DependencyProperty templateProperty)
		{
			if (newTemplate != null)
			{
				newTemplate.Seal();
			}
			fe.TemplateCache = newTemplate;
			StyleHelper.DoTemplateInvalidations(fe, oldTemplate);
			StyleHelper.ExecuteOnApplyEnterExitActions(fe, null, newTemplate);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00187800 File Offset: 0x00186800
		internal static void SealTemplate(FrameworkTemplate frameworkTemplate, ref bool isSealed, FrameworkElementFactory templateRoot, TriggerCollection triggers, ResourceDictionary resources, HybridDictionary childIndexFromChildID, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ContainerDependent> containerDependents, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary triggerActions, ref HybridDictionary dataTriggerRecordFromBinding, ref bool hasInstanceValues, ref EventHandlersStore eventHandlersStore)
		{
			if (isSealed)
			{
				return;
			}
			if (frameworkTemplate != null)
			{
				frameworkTemplate.ProcessTemplateBeforeSeal();
			}
			if (templateRoot != null)
			{
				templateRoot.Seal(frameworkTemplate);
			}
			if (triggers != null)
			{
				triggers.Seal();
			}
			if (resources != null)
			{
				resources.IsReadOnly = true;
			}
			if (templateRoot != null)
			{
				StyleHelper.ProcessTemplateContentFromFEF(templateRoot, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
			bool hasLoadedChangeHandler = false;
			StyleHelper.ProcessTemplateTriggers(triggers, frameworkTemplate, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref containerDependents, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues, ref triggerActions, templateRoot, ref eventHandlersStore, ref frameworkTemplate.PropertyTriggersWithActions, ref frameworkTemplate.DataTriggersWithActions, ref hasLoadedChangeHandler);
			frameworkTemplate.HasLoadedChangeHandler = hasLoadedChangeHandler;
			frameworkTemplate.SetResourceReferenceState();
			isSealed = true;
			frameworkTemplate.DetachFromDispatcher();
			if (StyleHelper.IsSetOnContainer(Control.TemplateProperty, ref containerDependents, true) || StyleHelper.IsSetOnContainer(ContentPresenter.TemplateProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					Control.TemplateProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.StyleProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.StyleProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.DefaultStyleKeyProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.DefaultStyleKeyProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.OverridesDefaultStyleProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.OverridesDefaultStyleProperty.Name
				}));
			}
			if (StyleHelper.IsSetOnContainer(FrameworkElement.NameProperty, ref containerDependents, true))
			{
				throw new InvalidOperationException(SR.Get("CannotHavePropertyInTemplate", new object[]
				{
					FrameworkElement.NameProperty.Name
				}));
			}
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x0018799C File Offset: 0x0018699C
		internal static void UpdateTables(ref PropertyValue propertyValue, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildName, ref bool hasInstanceValues)
		{
			int num = StyleHelper.QueryChildIndexFromChildName(propertyValue.ChildName, childIndexFromChildName);
			if (num == -1)
			{
				throw new InvalidOperationException(SR.Get("NameNotFound", new object[]
				{
					propertyValue.ChildName
				}));
			}
			object valueInternal = propertyValue.ValueInternal;
			bool flag = StyleHelper.RequiresInstanceStorage(ref valueInternal);
			propertyValue.ValueInternal = valueInternal;
			childRecordFromChildIndex.EnsureIndex(num);
			ChildRecord childRecord = childRecordFromChildIndex[num];
			int num2 = childRecord.ValueLookupListFromProperty.EnsureEntry(propertyValue.Property.GlobalIndex);
			ChildValueLookup childValueLookup = default(ChildValueLookup);
			childValueLookup.LookupType = (ValueLookupType)propertyValue.ValueType;
			childValueLookup.Conditions = propertyValue.Conditions;
			childValueLookup.Property = propertyValue.Property;
			childValueLookup.Value = propertyValue.ValueInternal;
			childRecord.ValueLookupListFromProperty.Entries[num2].Value.Add(ref childValueLookup);
			childRecordFromChildIndex[num] = childRecord;
			switch (propertyValue.ValueType)
			{
			case PropertyValueType.Set:
				hasInstanceValues = (hasInstanceValues || flag);
				return;
			case PropertyValueType.Trigger:
			case PropertyValueType.PropertyTriggerResource:
				if (propertyValue.Conditions != null)
				{
					for (int i = 0; i < propertyValue.Conditions.Length; i++)
					{
						int sourceChildIndex = propertyValue.Conditions[i].SourceChildIndex;
						triggerSourceRecordFromChildIndex.EnsureIndex(sourceChildIndex);
						ItemStructMap<TriggerSourceRecord> itemStructMap = triggerSourceRecordFromChildIndex[sourceChildIndex];
						if (propertyValue.Conditions[i].Property == null)
						{
							throw new InvalidOperationException(SR.Get("MissingTriggerProperty"));
						}
						int num3 = itemStructMap.EnsureEntry(propertyValue.Conditions[i].Property.GlobalIndex);
						StyleHelper.AddPropertyDependent(num, propertyValue.Property, ref itemStructMap.Entries[num3].Value.ChildPropertyDependents);
						triggerSourceRecordFromChildIndex[sourceChildIndex] = itemStructMap;
					}
					if (propertyValue.ValueType == PropertyValueType.PropertyTriggerResource)
					{
						StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
					}
				}
				if (propertyValue.ValueType != PropertyValueType.PropertyTriggerResource)
				{
					hasInstanceValues = (hasInstanceValues || flag);
					return;
				}
				break;
			case PropertyValueType.DataTrigger:
			case PropertyValueType.DataTriggerResource:
				if (propertyValue.Conditions != null)
				{
					if (dataTriggerRecordFromBinding == null)
					{
						dataTriggerRecordFromBinding = new HybridDictionary();
					}
					for (int j = 0; j < propertyValue.Conditions.Length; j++)
					{
						DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[propertyValue.Conditions[j].Binding];
						if (dataTriggerRecord == null)
						{
							dataTriggerRecord = new DataTriggerRecord();
							dataTriggerRecordFromBinding[propertyValue.Conditions[j].Binding] = dataTriggerRecord;
						}
						StyleHelper.AddPropertyDependent(num, propertyValue.Property, ref dataTriggerRecord.Dependents);
					}
					if (propertyValue.ValueType == PropertyValueType.DataTriggerResource)
					{
						StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
					}
				}
				if (propertyValue.ValueType != PropertyValueType.DataTriggerResource)
				{
					hasInstanceValues = (hasInstanceValues || flag);
					return;
				}
				break;
			case PropertyValueType.TemplateBinding:
			{
				TemplateBindingExtension templateBindingExtension = (TemplateBindingExtension)propertyValue.ValueInternal;
				DependencyProperty property = propertyValue.Property;
				DependencyProperty property2 = templateBindingExtension.Property;
				int index = 0;
				triggerSourceRecordFromChildIndex.EnsureIndex(index);
				ItemStructMap<TriggerSourceRecord> itemStructMap2 = triggerSourceRecordFromChildIndex[index];
				int num4 = itemStructMap2.EnsureEntry(property2.GlobalIndex);
				StyleHelper.AddPropertyDependent(num, property, ref itemStructMap2.Entries[num4].Value.ChildPropertyDependents);
				triggerSourceRecordFromChildIndex[index] = itemStructMap2;
				return;
			}
			case PropertyValueType.Resource:
				StyleHelper.AddResourceDependent(num, propertyValue.Property, propertyValue.ValueInternal, ref resourceDependents);
				break;
			default:
				return;
			}
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x00187CD8 File Offset: 0x00186CD8
		private static bool RequiresInstanceStorage(ref object value)
		{
			MarkupExtension markupExtension = null;
			Freezable freezable = null;
			DeferredReference deferredReference;
			if ((deferredReference = (value as DeferredReference)) != null)
			{
				Type valueType = deferredReference.GetValueType();
				if (valueType != null)
				{
					if (typeof(MarkupExtension).IsAssignableFrom(valueType))
					{
						value = deferredReference.GetValue(BaseValueSourceInternal.Style);
						if ((markupExtension = (value as MarkupExtension)) == null)
						{
							freezable = (value as Freezable);
						}
					}
					else if (typeof(Freezable).IsAssignableFrom(valueType))
					{
						freezable = (Freezable)deferredReference.GetValue(BaseValueSourceInternal.Style);
					}
				}
			}
			else if ((markupExtension = (value as MarkupExtension)) == null)
			{
				freezable = (value as Freezable);
			}
			bool result = false;
			if (markupExtension != null)
			{
				value = markupExtension;
				result = true;
			}
			else if (freezable != null)
			{
				value = freezable;
				if (!freezable.CanFreeze)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x00187D8C File Offset: 0x00186D8C
		internal static void AddContainerDependent(DependencyProperty dp, bool fromVisualTrigger, ref FrugalStructList<ContainerDependent> containerDependents)
		{
			ContainerDependent containerDependent;
			for (int i = 0; i < containerDependents.Count; i++)
			{
				containerDependent = containerDependents[i];
				if (dp == containerDependent.Property)
				{
					containerDependent.FromVisualTrigger = (containerDependent.FromVisualTrigger || fromVisualTrigger);
					return;
				}
			}
			containerDependent = new ContainerDependent
			{
				Property = dp,
				FromVisualTrigger = fromVisualTrigger
			};
			containerDependents.Add(containerDependent);
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00187DE8 File Offset: 0x00186DE8
		internal static void AddEventDependent(int childIndex, EventHandlersStore eventHandlersStore, ref ItemStructList<ChildEventDependent> eventDependents)
		{
			if (eventHandlersStore != null)
			{
				ChildEventDependent childEventDependent = default(ChildEventDependent);
				childEventDependent.ChildIndex = childIndex;
				childEventDependent.EventHandlersStore = eventHandlersStore;
				eventDependents.Add(ref childEventDependent);
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00187E18 File Offset: 0x00186E18
		private static void AddPropertyDependent(int childIndex, DependencyProperty dp, ref FrugalStructList<ChildPropertyDependent> propertyDependents)
		{
			propertyDependents.Add(new ChildPropertyDependent
			{
				ChildIndex = childIndex,
				Property = dp
			});
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x00187E48 File Offset: 0x00186E48
		private static void AddResourceDependent(int childIndex, DependencyProperty dp, object name, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			bool flag = true;
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				ChildPropertyDependent childPropertyDependent = resourceDependents[i];
				if (childPropertyDependent.ChildIndex == childIndex && childPropertyDependent.Property == dp && childPropertyDependent.Name == name)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				resourceDependents.Add(new ChildPropertyDependent
				{
					ChildIndex = childIndex,
					Property = dp,
					Name = name
				});
			}
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x00187EBC File Offset: 0x00186EBC
		internal static void ProcessTemplateContentFromFEF(FrameworkElementFactory factory, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildID, ref bool hasInstanceValues)
		{
			for (int i = 0; i < factory.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = factory.PropertyValues[i];
				StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
			StyleHelper.AddEventDependent(factory._childIndex, factory.EventHandlersStore, ref eventDependents);
			for (factory = factory.FirstChild; factory != null; factory = factory.NextSibling)
			{
				StyleHelper.ProcessTemplateContentFromFEF(factory, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref eventDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
			}
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x00187F38 File Offset: 0x00186F38
		private static void ProcessTemplateTriggers(TriggerCollection triggers, FrameworkTemplate frameworkTemplate, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalStructList<ContainerDependent> containerDependents, ref FrugalStructList<ChildPropertyDependent> resourceDependents, ref ItemStructList<ChildEventDependent> eventDependents, ref HybridDictionary dataTriggerRecordFromBinding, HybridDictionary childIndexFromChildID, ref bool hasInstanceValues, ref HybridDictionary triggerActions, FrameworkElementFactory templateRoot, ref EventHandlersStore eventHandlersStore, ref FrugalMap propertyTriggersWithActions, ref HybridDictionary dataTriggersWithActions, ref bool hasLoadedChangeHandler)
		{
			if (triggers != null)
			{
				int count = triggers.Count;
				for (int i = 0; i < count; i++)
				{
					TriggerBase triggerBase = triggers[i];
					Trigger trigger;
					MultiTrigger multiTrigger;
					DataTrigger dataTrigger;
					MultiDataTrigger multiDataTrigger;
					EventTrigger eventTrigger;
					StyleHelper.DetermineTriggerType(triggerBase, out trigger, out multiTrigger, out dataTrigger, out multiDataTrigger, out eventTrigger);
					if (trigger != null || multiTrigger != null || dataTrigger != null || multiDataTrigger != null)
					{
						TriggerCondition[] triggerConditions = triggerBase.TriggerConditions;
						for (int j = 0; j < triggerConditions.Length; j++)
						{
							triggerConditions[j].SourceChildIndex = StyleHelper.QueryChildIndexFromChildName(triggerConditions[j].SourceName, childIndexFromChildID);
						}
						for (int k = 0; k < triggerBase.PropertyValues.Count; k++)
						{
							PropertyValue propertyValue = triggerBase.PropertyValues[k];
							if (propertyValue.ChildName == "~Self")
							{
								StyleHelper.AddContainerDependent(propertyValue.Property, true, ref containerDependents);
							}
							StyleHelper.UpdateTables(ref propertyValue, ref childRecordFromChildIndex, ref triggerSourceRecordFromChildIndex, ref resourceDependents, ref dataTriggerRecordFromBinding, childIndexFromChildID, ref hasInstanceValues);
						}
						if (triggerBase.HasEnterActions || triggerBase.HasExitActions)
						{
							if (trigger != null)
							{
								StyleHelper.AddPropertyTriggerWithAction(triggerBase, trigger.Property, ref propertyTriggersWithActions);
							}
							else if (multiTrigger != null)
							{
								for (int l = 0; l < multiTrigger.Conditions.Count; l++)
								{
									Condition condition = multiTrigger.Conditions[l];
									StyleHelper.AddPropertyTriggerWithAction(triggerBase, condition.Property, ref propertyTriggersWithActions);
								}
							}
							else if (dataTrigger != null)
							{
								StyleHelper.AddDataTriggerWithAction(triggerBase, dataTrigger.Binding, ref dataTriggersWithActions);
							}
							else
							{
								if (multiDataTrigger == null)
								{
									throw new InvalidOperationException(SR.Get("UnsupportedTriggerInTemplate", new object[]
									{
										triggerBase.GetType().Name
									}));
								}
								for (int m = 0; m < multiDataTrigger.Conditions.Count; m++)
								{
									Condition condition2 = multiDataTrigger.Conditions[m];
									StyleHelper.AddDataTriggerWithAction(triggerBase, condition2.Binding, ref dataTriggersWithActions);
								}
							}
						}
					}
					else
					{
						if (eventTrigger == null)
						{
							throw new InvalidOperationException(SR.Get("UnsupportedTriggerInTemplate", new object[]
							{
								triggerBase.GetType().Name
							}));
						}
						StyleHelper.ProcessEventTrigger(eventTrigger, childIndexFromChildID, ref triggerActions, ref eventDependents, templateRoot, frameworkTemplate, ref eventHandlersStore, ref hasLoadedChangeHandler);
					}
				}
			}
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x00188150 File Offset: 0x00187150
		private static void DetermineTriggerType(TriggerBase triggerBase, out Trigger trigger, out MultiTrigger multiTrigger, out DataTrigger dataTrigger, out MultiDataTrigger multiDataTrigger, out EventTrigger eventTrigger)
		{
			Trigger trigger2;
			trigger = (trigger2 = (triggerBase as Trigger));
			if (trigger2 != null)
			{
				multiTrigger = null;
				dataTrigger = null;
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			MultiTrigger multiTrigger2;
			multiTrigger = (multiTrigger2 = (triggerBase as MultiTrigger));
			if (multiTrigger2 != null)
			{
				dataTrigger = null;
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			DataTrigger dataTrigger2;
			dataTrigger = (dataTrigger2 = (triggerBase as DataTrigger));
			if (dataTrigger2 != null)
			{
				multiDataTrigger = null;
				eventTrigger = null;
				return;
			}
			MultiDataTrigger multiDataTrigger2;
			multiDataTrigger = (multiDataTrigger2 = (triggerBase as MultiDataTrigger));
			if (multiDataTrigger2 != null)
			{
				eventTrigger = null;
				return;
			}
			EventTrigger eventTrigger2;
			eventTrigger = (eventTrigger2 = (triggerBase as EventTrigger));
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x001881CC File Offset: 0x001871CC
		internal static void ProcessEventTrigger(EventTrigger eventTrigger, HybridDictionary childIndexFromChildName, ref HybridDictionary triggerActions, ref ItemStructList<ChildEventDependent> eventDependents, FrameworkElementFactory templateRoot, FrameworkTemplate frameworkTemplate, ref EventHandlersStore eventHandlersStore, ref bool hasLoadedChangeHandler)
		{
			if (eventTrigger == null)
			{
				return;
			}
			List<TriggerAction> list = null;
			bool flag = true;
			bool flag2 = false;
			FrameworkElementFactory frameworkElementFactory = null;
			if (eventTrigger.SourceName == null)
			{
				eventTrigger.TriggerChildIndex = 0;
			}
			else
			{
				int num = StyleHelper.QueryChildIndexFromChildName(eventTrigger.SourceName, childIndexFromChildName);
				if (num == -1)
				{
					throw new InvalidOperationException(SR.Get("EventTriggerTargetNameUnresolvable", new object[]
					{
						eventTrigger.SourceName
					}));
				}
				eventTrigger.TriggerChildIndex = num;
			}
			if (triggerActions == null)
			{
				triggerActions = new HybridDictionary();
			}
			else
			{
				list = (triggerActions[eventTrigger.RoutedEvent] as List<TriggerAction>);
			}
			if (list == null)
			{
				flag = false;
				list = new List<TriggerAction>();
			}
			for (int i = 0; i < eventTrigger.Actions.Count; i++)
			{
				TriggerAction item = eventTrigger.Actions[i];
				list.Add(item);
				flag2 = true;
			}
			if (flag2 && !flag)
			{
				triggerActions[eventTrigger.RoutedEvent] = list;
			}
			if (templateRoot != null || eventTrigger.TriggerChildIndex == 0)
			{
				if (eventTrigger.TriggerChildIndex != 0)
				{
					frameworkElementFactory = StyleHelper.FindFEF(templateRoot, eventTrigger.TriggerChildIndex);
				}
				if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent || eventTrigger.RoutedEvent == FrameworkElement.UnloadedEvent)
				{
					if (eventTrigger.TriggerChildIndex == 0)
					{
						hasLoadedChangeHandler = true;
					}
					else
					{
						frameworkElementFactory.HasLoadedChangeHandler = true;
					}
				}
				StyleHelper.AddDelegateToFireTrigger(eventTrigger.RoutedEvent, eventTrigger.TriggerChildIndex, templateRoot, frameworkElementFactory, ref eventDependents, ref eventHandlersStore);
				return;
			}
			if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent || eventTrigger.RoutedEvent == FrameworkElement.UnloadedEvent)
			{
				FrameworkTemplate.TemplateChildLoadedFlags templateChildLoadedFlags = frameworkTemplate._TemplateChildLoadedDictionary[eventTrigger.TriggerChildIndex] as FrameworkTemplate.TemplateChildLoadedFlags;
				if (templateChildLoadedFlags == null)
				{
					templateChildLoadedFlags = new FrameworkTemplate.TemplateChildLoadedFlags();
					frameworkTemplate._TemplateChildLoadedDictionary[eventTrigger.TriggerChildIndex] = templateChildLoadedFlags;
				}
				if (eventTrigger.RoutedEvent == FrameworkElement.LoadedEvent)
				{
					templateChildLoadedFlags.HasLoadedChangedHandler = true;
				}
				else
				{
					templateChildLoadedFlags.HasUnloadedChangedHandler = true;
				}
			}
			StyleHelper.AddDelegateToFireTrigger(eventTrigger.RoutedEvent, eventTrigger.TriggerChildIndex, ref eventDependents, ref eventHandlersStore);
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x001883A0 File Offset: 0x001873A0
		private static void AddDelegateToFireTrigger(RoutedEvent routedEvent, int childIndex, FrameworkElementFactory templateRoot, FrameworkElementFactory childFef, ref ItemStructList<ChildEventDependent> eventDependents, ref EventHandlersStore eventHandlersStore)
		{
			if (childIndex == 0)
			{
				if (eventHandlersStore == null)
				{
					eventHandlersStore = new EventHandlersStore();
					StyleHelper.AddEventDependent(0, eventHandlersStore, ref eventDependents);
				}
				eventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnContainer, false);
				return;
			}
			if (childFef.EventHandlersStore == null)
			{
				childFef.EventHandlersStore = new EventHandlersStore();
				StyleHelper.AddEventDependent(childIndex, childFef.EventHandlersStore, ref eventDependents);
			}
			childFef.EventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnChild, false);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x0018840B File Offset: 0x0018740B
		private static void AddDelegateToFireTrigger(RoutedEvent routedEvent, int childIndex, ref ItemStructList<ChildEventDependent> eventDependents, ref EventHandlersStore eventHandlersStore)
		{
			if (eventHandlersStore == null)
			{
				eventHandlersStore = new EventHandlersStore();
			}
			StyleHelper.AddEventDependent(childIndex, eventHandlersStore, ref eventDependents);
			eventHandlersStore.AddRoutedEventHandler(routedEvent, StyleHelper.EventTriggerHandlerOnChild, false);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x00188430 File Offset: 0x00187430
		internal static void SealIfSealable(object value)
		{
			ISealable sealable = value as ISealable;
			if (sealable != null && !sealable.IsSealed && sealable.CanSeal)
			{
				sealable.Seal();
			}
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x00188460 File Offset: 0x00187460
		internal static void UpdateInstanceData(UncommonField<HybridDictionary[]> dataField, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle, FrameworkTemplate oldFrameworkTemplate, FrameworkTemplate newFrameworkTemplate, InternalFlags hasGeneratedSubTreeFlag)
		{
			DependencyObject dependencyObject = (fe != null) ? fe : fce;
			if (oldStyle != null || oldFrameworkTemplate != null)
			{
				StyleHelper.ReleaseInstanceData(dataField, dependencyObject, fe, fce, oldStyle, oldFrameworkTemplate, hasGeneratedSubTreeFlag);
			}
			if (newStyle != null || newFrameworkTemplate != null)
			{
				StyleHelper.CreateInstanceData(dataField, dependencyObject, fe, fce, newStyle, newFrameworkTemplate);
				return;
			}
			dataField.ClearValue(dependencyObject);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x001884A8 File Offset: 0x001874A8
		internal static void CreateInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style newStyle, FrameworkTemplate newFrameworkTemplate)
		{
			if (newStyle != null)
			{
				if (newStyle.HasInstanceValues)
				{
					HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues, true, ref newStyle.ChildRecordFromChildIndex);
					return;
				}
			}
			else if (newFrameworkTemplate != null && newFrameworkTemplate.HasInstanceValues)
			{
				HybridDictionary instanceValues2 = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
				StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues2, true, ref newFrameworkTemplate.ChildRecordFromChildIndex);
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00188504 File Offset: 0x00187504
		internal static void CreateInstanceDataForChild(UncommonField<HybridDictionary[]> dataField, DependencyObject container, DependencyObject child, int childIndex, bool hasInstanceValues, ref FrugalStructList<ChildRecord> childRecordFromChildIndex)
		{
			if (hasInstanceValues)
			{
				HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
				StyleHelper.ProcessInstanceValuesForChild(container, child, childIndex, instanceValues, true, ref childRecordFromChildIndex);
			}
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x0018852C File Offset: 0x0018752C
		internal static void ReleaseInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, FrameworkTemplate oldFrameworkTemplate, InternalFlags hasGeneratedSubTreeFlag)
		{
			HybridDictionary[] value = dataField.GetValue(container);
			if (oldStyle != null)
			{
				HybridDictionary instanceValues = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues, oldStyle, oldFrameworkTemplate);
				if (oldStyle.HasInstanceValues)
				{
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues, false, ref oldStyle.ChildRecordFromChildIndex);
					return;
				}
			}
			else if (oldFrameworkTemplate != null)
			{
				HybridDictionary instanceValues2 = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues2, oldStyle, oldFrameworkTemplate);
				if (oldFrameworkTemplate.HasInstanceValues)
				{
					StyleHelper.ProcessInstanceValuesForChild(container, container, 0, instanceValues2, false, ref oldFrameworkTemplate.ChildRecordFromChildIndex);
					return;
				}
			}
			else
			{
				HybridDictionary instanceValues3 = (value != null) ? value[0] : null;
				StyleHelper.ReleaseInstanceDataForDataTriggers(dataField, instanceValues3, oldStyle, oldFrameworkTemplate);
			}
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x001885BE File Offset: 0x001875BE
		internal static HybridDictionary EnsureInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, InstanceStyleData dataType)
		{
			return StyleHelper.EnsureInstanceData(dataField, container, dataType, -1);
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x001885CC File Offset: 0x001875CC
		internal static HybridDictionary EnsureInstanceData(UncommonField<HybridDictionary[]> dataField, DependencyObject container, InstanceStyleData dataType, int initialSize)
		{
			HybridDictionary[] array = dataField.GetValue(container);
			if (array == null)
			{
				array = new HybridDictionary[1];
				dataField.SetValue(container, array);
			}
			if (array[(int)dataType] == null)
			{
				if (initialSize < 0)
				{
					array[(int)dataType] = new HybridDictionary();
				}
				else
				{
					array[(int)dataType] = new HybridDictionary(initialSize);
				}
			}
			return array[(int)dataType];
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00188614 File Offset: 0x00187614
		private static void ProcessInstanceValuesForChild(DependencyObject container, DependencyObject child, int childIndex, HybridDictionary instanceValues, bool apply, ref FrugalStructList<ChildRecord> childRecordFromChildIndex)
		{
			if (childIndex == -1)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(child, out frameworkElement, out frameworkContentElement, false);
				childIndex = ((frameworkElement != null) ? frameworkElement.TemplateChildIndex : ((frameworkContentElement != null) ? frameworkContentElement.TemplateChildIndex : -1));
			}
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				int count = childRecordFromChildIndex[childIndex].ValueLookupListFromProperty.Count;
				for (int i = 0; i < count; i++)
				{
					StyleHelper.ProcessInstanceValuesHelper(ref childRecordFromChildIndex[childIndex].ValueLookupListFromProperty.Entries[i].Value, child, childIndex, instanceValues, apply);
				}
			}
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x001886A0 File Offset: 0x001876A0
		private static void ProcessInstanceValuesHelper(ref ItemStructList<ChildValueLookup> valueLookupList, DependencyObject target, int childIndex, HybridDictionary instanceValues, bool apply)
		{
			for (int i = valueLookupList.Count - 1; i >= 0; i--)
			{
				ValueLookupType lookupType = valueLookupList.List[i].LookupType;
				if (lookupType <= ValueLookupType.Trigger || lookupType == ValueLookupType.DataTrigger)
				{
					DependencyProperty property = valueLookupList.List[i].Property;
					object value = valueLookupList.List[i].Value;
					Freezable freezable;
					if (value is MarkupExtension)
					{
						StyleHelper.ProcessInstanceValue(target, childIndex, instanceValues, property, i, apply);
					}
					else if ((freezable = (value as Freezable)) != null)
					{
						if (!freezable.CheckAccess())
						{
							throw new InvalidOperationException(SR.Get("CrossThreadAccessOfUnshareableFreezable", new object[]
							{
								freezable.GetType().FullName
							}));
						}
						if (!freezable.IsFrozen)
						{
							StyleHelper.ProcessInstanceValue(target, childIndex, instanceValues, property, i, apply);
						}
					}
				}
			}
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x0018876C File Offset: 0x0018776C
		internal static void ProcessInstanceValue(DependencyObject target, int childIndex, HybridDictionary instanceValues, DependencyProperty dp, int i, bool apply)
		{
			InstanceValueKey key = new InstanceValueKey(childIndex, dp.GlobalIndex, i);
			if (apply)
			{
				instanceValues[key] = StyleHelper.NotYetApplied;
				return;
			}
			object obj = instanceValues[key];
			instanceValues.Remove(key);
			Expression expression;
			if ((expression = (obj as Expression)) != null)
			{
				expression.OnDetach(target, dp);
				return;
			}
			Freezable doValue;
			if ((doValue = (obj as Freezable)) != null)
			{
				target.RemoveSelfAsInheritanceContext(doValue, dp);
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x001887D0 File Offset: 0x001877D0
		private static void ReleaseInstanceDataForDataTriggers(UncommonField<HybridDictionary[]> dataField, HybridDictionary instanceValues, Style oldStyle, FrameworkTemplate oldFrameworkTemplate)
		{
			if (instanceValues == null)
			{
				return;
			}
			EventHandler<BindingValueChangedEventArgs> handler;
			if (dataField == StyleHelper.StyleDataField)
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInStyleChanged);
			}
			else if (dataField == StyleHelper.TemplateDataField)
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInTemplateChanged);
			}
			else
			{
				handler = new EventHandler<BindingValueChangedEventArgs>(StyleHelper.OnBindingValueInThemeStyleChanged);
			}
			HybridDictionary hybridDictionary = null;
			if (oldStyle != null)
			{
				hybridDictionary = oldStyle._dataTriggerRecordFromBinding;
			}
			else if (oldFrameworkTemplate != null)
			{
				hybridDictionary = oldFrameworkTemplate._dataTriggerRecordFromBinding;
			}
			if (hybridDictionary != null)
			{
				foreach (object obj in hybridDictionary.Keys)
				{
					StyleHelper.ReleaseInstanceDataForTriggerBinding((BindingBase)obj, instanceValues, handler);
				}
			}
			HybridDictionary hybridDictionary2 = null;
			if (oldStyle != null)
			{
				hybridDictionary2 = oldStyle.DataTriggersWithActions;
			}
			else if (oldFrameworkTemplate != null)
			{
				hybridDictionary2 = oldFrameworkTemplate.DataTriggersWithActions;
			}
			if (hybridDictionary2 != null)
			{
				foreach (object obj2 in hybridDictionary2.Keys)
				{
					StyleHelper.ReleaseInstanceDataForTriggerBinding((BindingBase)obj2, instanceValues, handler);
				}
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x001888EC File Offset: 0x001878EC
		private static void ReleaseInstanceDataForTriggerBinding(BindingBase binding, HybridDictionary instanceValues, EventHandler<BindingValueChangedEventArgs> handler)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)instanceValues[binding];
			if (bindingExpressionBase != null)
			{
				bindingExpressionBase.ValueChanged -= handler;
				bindingExpressionBase.Detach();
				instanceValues.Remove(binding);
			}
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x00188920 File Offset: 0x00187920
		internal static bool ApplyTemplateContent(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElementFactory templateRoot, int lastChildIndex, HybridDictionary childIndexFromChildID, FrameworkTemplate frameworkTemplate)
		{
			bool result = false;
			FrameworkElement frameworkElement = container as FrameworkElement;
			if (templateRoot != null)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseInstVisTreeBegin);
				StyleHelper.CheckForCircularReferencesInTemplateTree(container, frameworkTemplate);
				List<DependencyObject> list = new List<DependencyObject>(lastChildIndex);
				StyleHelper.TemplatedFeChildrenField.SetValue(container, list);
				List<DependencyObject> list2 = null;
				templateRoot.InstantiateTree(dataField, container, container, list, ref list2, ref frameworkTemplate.ResourceDependents);
				if (list2 != null)
				{
					list.AddRange(list2);
				}
				result = true;
				if (frameworkElement != null && EventTrace.IsEnabled(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose))
				{
					string text = frameworkElement.Name;
					if (text == null || text.Length == 0)
					{
						text = container.GetHashCode().ToString(CultureInfo.InvariantCulture);
					}
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientParseInstVisTreeEnd, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, string.Format(CultureInfo.InvariantCulture, "Style.InstantiateSubTree for {0} {1}", container.GetType().Name, text));
				}
			}
			else if (frameworkTemplate != null && frameworkTemplate.HasXamlNodeContent)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseInstVisTreeBegin);
				StyleHelper.CheckForCircularReferencesInTemplateTree(container, frameworkTemplate);
				List<DependencyObject> list3 = new List<DependencyObject>(lastChildIndex);
				StyleHelper.TemplatedFeChildrenField.SetValue(container, list3);
				frameworkTemplate.LoadContent(container, list3);
				result = true;
				if (frameworkElement != null && EventTrace.IsEnabled(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose))
				{
					string text2 = frameworkElement.Name;
					if (text2 == null || text2.Length == 0)
					{
						text2 = container.GetHashCode().ToString(CultureInfo.InvariantCulture);
					}
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientParseInstVisTreeEnd, EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, string.Format(CultureInfo.InvariantCulture, "Style.InstantiateSubTree for {0} {1}", container.GetType().Name, text2));
				}
			}
			else if (frameworkElement != null)
			{
				result = frameworkTemplate.BuildVisualTree(frameworkElement);
			}
			return result;
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x00188AB8 File Offset: 0x00187AB8
		internal static void AddCustomTemplateRoot(FrameworkElement container, UIElement child)
		{
			StyleHelper.AddCustomTemplateRoot(container, child, true, false);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00188AC4 File Offset: 0x00187AC4
		internal static void AddCustomTemplateRoot(FrameworkElement container, UIElement child, bool checkVisualParent, bool mustCacheTreeStateOnChild)
		{
			if (child != null && checkVisualParent)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetParent(child) as FrameworkElement;
				if (frameworkElement != null)
				{
					frameworkElement.TemplateChild = null;
					frameworkElement.InvalidateMeasure();
				}
			}
			container.TemplateChild = child;
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00188AFC File Offset: 0x00187AFC
		private static void CheckForCircularReferencesInTemplateTree(DependencyObject container, FrameworkTemplate frameworkTemplate)
		{
			DependencyObject templatedParent;
			for (DependencyObject dependencyObject = container; dependencyObject != null; dependencyObject = ((dependencyObject is ContentPresenter) ? null : templatedParent))
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
				bool flag = frameworkElement != null;
				if (flag)
				{
					templatedParent = frameworkElement.TemplatedParent;
				}
				else
				{
					templatedParent = frameworkContentElement.TemplatedParent;
				}
				if (dependencyObject != container && templatedParent != null && frameworkTemplate != null && flag && frameworkElement.TemplateInternal == frameworkTemplate && dependencyObject.GetType() == container.GetType())
				{
					string text = flag ? frameworkElement.Name : frameworkContentElement.Name;
					throw new InvalidOperationException(SR.Get("TemplateCircularReferenceFound", new object[]
					{
						text,
						dependencyObject.GetType()
					}));
				}
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00188BAC File Offset: 0x00187BAC
		internal static void ClearGeneratedSubTree(HybridDictionary[] instanceData, FrameworkElement feContainer, FrameworkContentElement fceContainer, FrameworkTemplate oldFrameworkTemplate)
		{
			List<DependencyObject> value;
			if (feContainer != null)
			{
				value = StyleHelper.TemplatedFeChildrenField.GetValue(feContainer);
				StyleHelper.TemplatedFeChildrenField.ClearValue(feContainer);
			}
			else
			{
				value = StyleHelper.TemplatedFeChildrenField.GetValue(fceContainer);
				StyleHelper.TemplatedFeChildrenField.ClearValue(fceContainer);
			}
			DependencyObject dependencyObject = null;
			if (value != null)
			{
				dependencyObject = value[0];
				if (oldFrameworkTemplate != null)
				{
					StyleHelper.ClearTemplateChain(instanceData, feContainer, fceContainer, value, oldFrameworkTemplate);
				}
			}
			if (dependencyObject != null)
			{
				dependencyObject.ClearValue(NameScope.NameScopeProperty);
			}
			StyleHelper.DetachGeneratedSubTree(feContainer, fceContainer);
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00188C1B File Offset: 0x00187C1B
		private static void DetachGeneratedSubTree(FrameworkElement feContainer, FrameworkContentElement fceContainer)
		{
			if (feContainer != null)
			{
				feContainer.TemplateChild = null;
				feContainer.HasTemplateGeneratedSubTree = false;
				return;
			}
			fceContainer.HasTemplateGeneratedSubTree = false;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00188C38 File Offset: 0x00187C38
		private static void ClearTemplateChain(HybridDictionary[] instanceData, FrameworkElement feContainer, FrameworkContentElement fceContainer, List<DependencyObject> templateChain, FrameworkTemplate oldFrameworkTemplate)
		{
			FrameworkObject frameworkObject = new FrameworkObject(feContainer, fceContainer);
			HybridDictionary instanceValues = (instanceData != null) ? instanceData[0] : null;
			int[] array = new int[templateChain.Count];
			for (int i = 0; i < templateChain.Count; i++)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				StyleHelper.SpecialDowncastToFEorFCE(templateChain[i], out frameworkElement, out frameworkContentElement, true);
				if (frameworkElement != null)
				{
					array[i] = frameworkElement.TemplateChildIndex;
					frameworkElement._templatedParent = null;
					frameworkElement.TemplateChildIndex = -1;
				}
				else if (frameworkContentElement != null)
				{
					array[i] = frameworkContentElement.TemplateChildIndex;
					frameworkContentElement._templatedParent = null;
					frameworkContentElement.TemplateChildIndex = -1;
				}
			}
			for (int j = 0; j < templateChain.Count; j++)
			{
				DependencyObject dependencyObject = templateChain[j];
				FrameworkObject child = new FrameworkObject(dependencyObject);
				int childIndex = array[j];
				StyleHelper.ProcessInstanceValuesForChild(feContainer, dependencyObject, array[j], instanceValues, false, ref oldFrameworkTemplate.ChildRecordFromChildIndex);
				StyleHelper.InvalidatePropertiesOnTemplateNode(frameworkObject.DO, child, array[j], ref oldFrameworkTemplate.ChildRecordFromChildIndex, true, oldFrameworkTemplate.VisualTree);
				if (child.StoresParentTemplateValues)
				{
					HybridDictionary value = StyleHelper.ParentTemplateValuesField.GetValue(dependencyObject);
					StyleHelper.ParentTemplateValuesField.ClearValue(dependencyObject);
					child.StoresParentTemplateValues = false;
					foreach (object obj in value)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						DependencyProperty dp = (DependencyProperty)dictionaryEntry.Key;
						if (dictionaryEntry.Value is MarkupExtension)
						{
							StyleHelper.ProcessInstanceValue(dependencyObject, childIndex, instanceValues, dp, -1, false);
						}
						dependencyObject.InvalidateProperty(dp);
					}
				}
			}
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00188DD4 File Offset: 0x00187DD4
		internal static void SpecialDowncastToFEorFCE(DependencyObject d, out FrameworkElement fe, out FrameworkContentElement fce, bool throwIfNeither)
		{
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				fe = (FrameworkElement)d;
				fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				fe = null;
				fce = (FrameworkContentElement)d;
				return;
			}
			if (throwIfNeither && !(d is Visual3D))
			{
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					d.GetType()
				}));
			}
			fe = null;
			fce = null;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00188E44 File Offset: 0x00187E44
		internal static FrameworkElementFactory FindFEF(FrameworkElementFactory root, int childIndex)
		{
			if (root._childIndex == childIndex)
			{
				return root;
			}
			for (FrameworkElementFactory frameworkElementFactory = root.FirstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory.NextSibling)
			{
				FrameworkElementFactory frameworkElementFactory2 = StyleHelper.FindFEF(frameworkElementFactory, childIndex);
				if (frameworkElementFactory2 != null)
				{
					return frameworkElementFactory2;
				}
			}
			return null;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00188E80 File Offset: 0x00187E80
		private static void ExecuteEventTriggerActionsOnContainer(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)sender, out frameworkElement, out frameworkContentElement, false);
			FrameworkTemplate frameworkTemplate = null;
			Style style;
			Style themeStyle;
			if (frameworkElement != null)
			{
				style = frameworkElement.Style;
				themeStyle = frameworkElement.ThemeStyle;
				frameworkTemplate = frameworkElement.TemplateInternal;
			}
			else
			{
				style = frameworkContentElement.Style;
				themeStyle = frameworkContentElement.ThemeStyle;
			}
			if (style != null && style.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, style, null, 0, e.RoutedEvent);
			}
			if (themeStyle != null && themeStyle.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, themeStyle, null, 0, e.RoutedEvent);
			}
			if (frameworkTemplate != null && frameworkTemplate.EventHandlersStore != null)
			{
				StyleHelper.InvokeEventTriggerActions(frameworkElement, frameworkContentElement, null, frameworkTemplate, 0, e.RoutedEvent);
			}
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00188F20 File Offset: 0x00187F20
		private static void ExecuteEventTriggerActionsOnChild(object sender, RoutedEventArgs e)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE((DependencyObject)sender, out frameworkElement, out frameworkContentElement, false);
			DependencyObject templatedParent;
			int templateChildIndex;
			if (frameworkElement != null)
			{
				templatedParent = frameworkElement.TemplatedParent;
				templateChildIndex = frameworkElement.TemplateChildIndex;
			}
			else
			{
				templatedParent = frameworkContentElement.TemplatedParent;
				templateChildIndex = frameworkContentElement.TemplateChildIndex;
			}
			if (templatedParent != null)
			{
				FrameworkElement frameworkElement2;
				FrameworkContentElement fce;
				Helper.DowncastToFEorFCE(templatedParent, out frameworkElement2, out fce, false);
				FrameworkTemplate templateInternal = frameworkElement2.TemplateInternal;
				StyleHelper.InvokeEventTriggerActions(frameworkElement2, fce, null, templateInternal, templateChildIndex, e.RoutedEvent);
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00188F8C File Offset: 0x00187F8C
		private static void InvokeEventTriggerActions(FrameworkElement fe, FrameworkContentElement fce, Style ownerStyle, FrameworkTemplate frameworkTemplate, int childIndex, RoutedEvent Event)
		{
			List<TriggerAction> list;
			if (ownerStyle != null)
			{
				list = ((ownerStyle._triggerActions != null) ? (ownerStyle._triggerActions[Event] as List<TriggerAction>) : null);
			}
			else
			{
				list = ((frameworkTemplate._triggerActions != null) ? (frameworkTemplate._triggerActions[Event] as List<TriggerAction>) : null);
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					TriggerAction triggerAction = list[i];
					int triggerChildIndex = ((EventTrigger)triggerAction.ContainingTrigger).TriggerChildIndex;
					if (childIndex == triggerChildIndex)
					{
						triggerAction.Invoke(fe, fce, ownerStyle, frameworkTemplate, Storyboard.Layers.StyleOrTemplateEventTrigger);
					}
				}
			}
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x0018901C File Offset: 0x0018801C
		internal static object GetChildValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, int childIndex, FrameworkObject child, DependencyProperty dp, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, ref EffectiveValueEntry entry, out ValueLookupType sourceType, FrameworkElementFactory templateRoot)
		{
			object result = DependencyProperty.UnsetValue;
			sourceType = ValueLookupType.Simple;
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[childIndex];
				int num = childRecord.ValueLookupListFromProperty.Search(dp.GlobalIndex);
				if (num >= 0 && childRecord.ValueLookupListFromProperty.Entries[num].Value.Count > 0)
				{
					result = StyleHelper.GetChildValueHelper(dataField, ref childRecord.ValueLookupListFromProperty.Entries[num].Value, dp, container, child, childIndex, true, ref entry, out sourceType, templateRoot);
				}
			}
			return result;
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x001890AC File Offset: 0x001880AC
		private static object GetChildValueHelper(UncommonField<HybridDictionary[]> dataField, ref ItemStructList<ChildValueLookup> valueLookupList, DependencyProperty dp, DependencyObject container, FrameworkObject child, int childIndex, bool styleLookup, ref EffectiveValueEntry entry, out ValueLookupType sourceType, FrameworkElementFactory templateRoot)
		{
			object obj = DependencyProperty.UnsetValue;
			sourceType = ValueLookupType.Simple;
			for (int i = valueLookupList.Count - 1; i >= 0; i--)
			{
				sourceType = valueLookupList.List[i].LookupType;
				switch (valueLookupList.List[i].LookupType)
				{
				case ValueLookupType.Simple:
					obj = valueLookupList.List[i].Value;
					break;
				case ValueLookupType.Trigger:
				case ValueLookupType.PropertyTriggerResource:
				case ValueLookupType.DataTrigger:
				case ValueLookupType.DataTriggerResource:
				{
					bool flag = true;
					if (valueLookupList.List[i].Conditions != null)
					{
						int num = 0;
						while (flag && num < valueLookupList.List[i].Conditions.Length)
						{
							ValueLookupType lookupType = valueLookupList.List[i].LookupType;
							if (lookupType - ValueLookupType.Trigger > 1)
							{
								if (lookupType - ValueLookupType.DataTrigger > 1)
								{
								}
								object state = StyleHelper.GetDataTriggerValue(dataField, container, valueLookupList.List[i].Conditions[num].Binding);
								flag = valueLookupList.List[i].Conditions[num].ConvertAndMatch(state);
							}
							else
							{
								int sourceChildIndex = valueLookupList.List[i].Conditions[num].SourceChildIndex;
								DependencyObject dependencyObject;
								if (sourceChildIndex == 0)
								{
									dependencyObject = container;
								}
								else
								{
									dependencyObject = StyleHelper.GetChild(container, sourceChildIndex);
								}
								DependencyProperty property = valueLookupList.List[i].Conditions[num].Property;
								object state;
								if (dependencyObject != null)
								{
									state = dependencyObject.GetValue(property);
								}
								else
								{
									Type forType;
									if (templateRoot != null)
									{
										forType = StyleHelper.FindFEF(templateRoot, sourceChildIndex).Type;
									}
									else
									{
										forType = (container as FrameworkElement).TemplateInternal.ChildTypeFromChildIndex[sourceChildIndex];
									}
									state = property.GetDefaultValue(forType);
								}
								flag = valueLookupList.List[i].Conditions[num].Match(state);
							}
							num++;
						}
					}
					if (flag)
					{
						if (valueLookupList.List[i].LookupType == ValueLookupType.PropertyTriggerResource || valueLookupList.List[i].LookupType == ValueLookupType.DataTriggerResource)
						{
							object obj2;
							obj = FrameworkElement.FindResourceInternal(child.FE, child.FCE, dp, valueLookupList.List[i].Value, null, true, false, null, false, out obj2);
							StyleHelper.SealIfSealable(obj);
						}
						else
						{
							obj = valueLookupList.List[i].Value;
						}
					}
					break;
				}
				case ValueLookupType.TemplateBinding:
				{
					TemplateBindingExtension templateBindingExtension = (TemplateBindingExtension)valueLookupList.List[i].Value;
					DependencyProperty property2 = templateBindingExtension.Property;
					obj = container.GetValue(property2);
					if (templateBindingExtension.Converter != null)
					{
						DependencyProperty property3 = valueLookupList.List[i].Property;
						CultureInfo compatibleCulture = child.Language.GetCompatibleCulture();
						obj = templateBindingExtension.Converter.Convert(obj, property3.PropertyType, templateBindingExtension.ConverterParameter, compatibleCulture);
					}
					if (obj != DependencyProperty.UnsetValue && !dp.IsValidValue(obj))
					{
						obj = DependencyProperty.UnsetValue;
					}
					break;
				}
				case ValueLookupType.Resource:
				{
					object obj3;
					obj = FrameworkElement.FindResourceInternal(child.FE, child.FCE, dp, valueLookupList.List[i].Value, null, true, false, null, false, out obj3);
					StyleHelper.SealIfSealable(obj);
					break;
				}
				}
				if (obj != DependencyProperty.UnsetValue)
				{
					entry.Value = obj;
					ValueLookupType lookupType2 = valueLookupList.List[i].LookupType;
					if (lookupType2 <= ValueLookupType.Trigger || lookupType2 == ValueLookupType.DataTrigger)
					{
						Freezable freezable;
						if (obj is MarkupExtension)
						{
							obj = StyleHelper.GetInstanceValue(dataField, container, child.FE, child.FCE, childIndex, valueLookupList.List[i].Property, i, ref entry);
						}
						else if ((freezable = (obj as Freezable)) != null && !freezable.IsFrozen)
						{
							obj = StyleHelper.GetInstanceValue(dataField, container, child.FE, child.FCE, childIndex, valueLookupList.List[i].Property, i, ref entry);
						}
					}
				}
				if (obj != DependencyProperty.UnsetValue)
				{
					break;
				}
			}
			return obj;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00189498 File Offset: 0x00188498
		internal static object GetDataTriggerValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, BindingBase binding)
		{
			dataField.GetValue(container);
			HybridDictionary hybridDictionary = StyleHelper.EnsureInstanceData(dataField, container, InstanceStyleData.InstanceValues);
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)hybridDictionary[binding];
			if (bindingExpressionBase == null)
			{
				bindingExpressionBase = BindingExpressionBase.CreateUntargetedBindingExpression(container, binding);
				hybridDictionary[binding] = bindingExpressionBase;
				if (dataField == StyleHelper.StyleDataField)
				{
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInStyleChanged;
				}
				else if (dataField == StyleHelper.TemplateDataField)
				{
					bindingExpressionBase.ResolveNamesInTemplate = true;
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInTemplateChanged;
				}
				else
				{
					bindingExpressionBase.ValueChanged += StyleHelper.OnBindingValueInThemeStyleChanged;
				}
				bindingExpressionBase.Attach(container);
			}
			return bindingExpressionBase.Value;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00189534 File Offset: 0x00188534
		internal static object GetInstanceValue(UncommonField<HybridDictionary[]> dataField, DependencyObject container, FrameworkElement feChild, FrameworkContentElement fceChild, int childIndex, DependencyProperty dp, int i, ref EffectiveValueEntry entry)
		{
			object value = entry.Value;
			DependencyObject dependencyObject = null;
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(container, out frameworkElement, out frameworkContentElement, true);
			HybridDictionary[] array = (dataField != null) ? dataField.GetValue(container) : null;
			HybridDictionary hybridDictionary = (array != null) ? array[0] : null;
			InstanceValueKey key = new InstanceValueKey(childIndex, dp.GlobalIndex, i);
			object obj = (hybridDictionary != null) ? hybridDictionary[key] : null;
			bool flag = (feChild != null) ? feChild.IsRequestingExpression : fceChild.IsRequestingExpression;
			if (obj == null)
			{
				obj = StyleHelper.NotYetApplied;
			}
			Expression expression = obj as Expression;
			if (expression != null && expression.HasBeenDetached)
			{
				obj = StyleHelper.NotYetApplied;
			}
			if (obj == StyleHelper.NotYetApplied)
			{
				dependencyObject = feChild;
				if (dependencyObject == null)
				{
					dependencyObject = fceChild;
				}
				MarkupExtension markupExtension;
				Freezable freezable;
				if ((markupExtension = (value as MarkupExtension)) != null)
				{
					if (flag && !((feChild != null) ? feChild.IsInitialized : fceChild.IsInitialized))
					{
						return DependencyProperty.UnsetValue;
					}
					ProvideValueServiceProvider provideValueServiceProvider = new ProvideValueServiceProvider();
					provideValueServiceProvider.SetData(dependencyObject, dp);
					obj = markupExtension.ProvideValue(provideValueServiceProvider);
				}
				else if ((freezable = (value as Freezable)) != null)
				{
					obj = freezable.Clone();
					dependencyObject.ProvideSelfAsInheritanceContext(obj, dp);
				}
				hybridDictionary[key] = obj;
				if (obj != DependencyProperty.UnsetValue)
				{
					expression = (obj as Expression);
					if (expression != null)
					{
						expression.OnAttach(dependencyObject, dp);
					}
				}
			}
			if (expression != null)
			{
				if (!flag)
				{
					if (dependencyObject == null)
					{
						dependencyObject = feChild;
						if (dependencyObject == null)
						{
							dependencyObject = fceChild;
						}
					}
					entry.ResetValue(DependencyObject.ExpressionInAlternativeStore, true);
					entry.SetExpressionValue(expression.GetValue(dependencyObject, dp), DependencyObject.ExpressionInAlternativeStore);
				}
				else
				{
					entry.Value = obj;
				}
			}
			else
			{
				entry.Value = obj;
			}
			return obj;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x001896C1 File Offset: 0x001886C1
		internal static bool ShouldGetValueFromStyle(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x001896CE File Offset: 0x001886CE
		internal static bool ShouldGetValueFromThemeStyle(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty && dp != FrameworkElement.DefaultStyleKeyProperty && dp != FrameworkElement.OverridesDefaultStyleProperty;
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x001896ED File Offset: 0x001886ED
		internal static bool ShouldGetValueFromTemplate(DependencyProperty dp)
		{
			return dp != FrameworkElement.StyleProperty && dp != FrameworkElement.DefaultStyleKeyProperty && dp != FrameworkElement.OverridesDefaultStyleProperty && dp != Control.TemplateProperty && dp != ContentPresenter.TemplateProperty;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x0018971C File Offset: 0x0018871C
		internal static void DoStyleInvalidations(FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle)
		{
			if (oldStyle != newStyle)
			{
				FrameworkElement frameworkElement = (fe != null) ? fe : fce;
				StyleHelper.UpdateLoadedFlag(frameworkElement, oldStyle, newStyle);
				StyleHelper.UpdateInstanceData(StyleHelper.StyleDataField, fe, fce, oldStyle, newStyle, null, null, (InternalFlags)0U);
				if (newStyle != null && newStyle.HasResourceReferences)
				{
					if (fe != null)
					{
						fe.HasResourceReference = true;
					}
					else
					{
						fce.HasResourceReference = true;
					}
				}
				FrugalStructList<ContainerDependent> frugalStructList = (oldStyle != null) ? oldStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (newStyle != null) ? newStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList3 = default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(frameworkElement, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoStyleResourcesInvalidations(frameworkElement, fe, fce, oldStyle, newStyle);
				if (fe != null)
				{
					fe.OnStyleChanged(oldStyle, newStyle);
					return;
				}
				fce.OnStyleChanged(oldStyle, newStyle);
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x001897C4 File Offset: 0x001887C4
		internal static void DoThemeStyleInvalidations(FrameworkElement fe, FrameworkContentElement fce, Style oldThemeStyle, Style newThemeStyle, Style style)
		{
			if (oldThemeStyle != newThemeStyle && newThemeStyle != style)
			{
				FrameworkElement frameworkElement = (fe != null) ? fe : fce;
				StyleHelper.UpdateLoadedFlag(frameworkElement, oldThemeStyle, newThemeStyle);
				StyleHelper.UpdateInstanceData(StyleHelper.ThemeStyleDataField, fe, fce, oldThemeStyle, newThemeStyle, null, null, (InternalFlags)0U);
				if (newThemeStyle != null && newThemeStyle.HasResourceReferences)
				{
					if (fe != null)
					{
						fe.HasResourceReference = true;
					}
					else
					{
						fce.HasResourceReference = true;
					}
				}
				FrugalStructList<ContainerDependent> frugalStructList = (oldThemeStyle != null) ? oldThemeStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (newThemeStyle != null) ? newThemeStyle.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList3 = (style != null) ? style.ContainerDependents : default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(frameworkElement, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoStyleResourcesInvalidations(frameworkElement, fe, fce, oldThemeStyle, newThemeStyle);
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x0018986C File Offset: 0x0018886C
		internal static void DoTemplateInvalidations(FrameworkElement feContainer, FrameworkTemplate oldFrameworkTemplate)
		{
			HybridDictionary[] value = StyleHelper.TemplateDataField.GetValue(feContainer);
			FrameworkTemplate templateInternal = feContainer.TemplateInternal;
			object obj = templateInternal;
			bool flag = templateInternal != null && templateInternal.HasResourceReferences;
			StyleHelper.UpdateLoadedFlag(feContainer, oldFrameworkTemplate, templateInternal);
			if (oldFrameworkTemplate != obj)
			{
				StyleHelper.UpdateInstanceData(StyleHelper.TemplateDataField, feContainer, null, null, null, oldFrameworkTemplate, templateInternal, InternalFlags.HasTemplateGeneratedSubTree);
				if (obj != null && flag)
				{
					feContainer.HasResourceReference = true;
				}
				StyleHelper.UpdateLoadedFlag(feContainer, oldFrameworkTemplate, templateInternal);
				if (oldFrameworkTemplate != null)
				{
					FrameworkElementFactory visualTree = oldFrameworkTemplate.VisualTree;
				}
				if (templateInternal != null)
				{
					FrameworkElementFactory visualTree2 = templateInternal.VisualTree;
				}
				if (oldFrameworkTemplate != null)
				{
					bool canBuildVisualTree = oldFrameworkTemplate.CanBuildVisualTree;
				}
				bool hasTemplateGeneratedSubTree = feContainer.HasTemplateGeneratedSubTree;
				FrugalStructList<ContainerDependent> frugalStructList = (oldFrameworkTemplate != null) ? oldFrameworkTemplate.ContainerDependents : StyleHelper.EmptyContainerDependents;
				FrugalStructList<ContainerDependent> frugalStructList2 = (templateInternal != null) ? templateInternal.ContainerDependents : StyleHelper.EmptyContainerDependents;
				if (hasTemplateGeneratedSubTree)
				{
					StyleHelper.ClearGeneratedSubTree(value, feContainer, null, oldFrameworkTemplate);
				}
				FrugalStructList<ContainerDependent> frugalStructList3 = default(FrugalStructList<ContainerDependent>);
				StyleHelper.InvalidateContainerDependents(feContainer, ref frugalStructList3, ref frugalStructList, ref frugalStructList2);
				StyleHelper.DoTemplateResourcesInvalidations(feContainer, feContainer, null, oldFrameworkTemplate, obj);
				feContainer.OnTemplateChangedInternal(oldFrameworkTemplate, templateInternal);
				return;
			}
			if (templateInternal != null && feContainer.HasTemplateGeneratedSubTree && templateInternal.VisualTree == null && !templateInternal.HasXamlNodeContent)
			{
				StyleHelper.ClearGeneratedSubTree(value, feContainer, null, oldFrameworkTemplate);
				feContainer.InvalidateMeasure();
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00189988 File Offset: 0x00188988
		internal static void DoStyleResourcesInvalidations(DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, Style oldStyle, Style newStyle)
		{
			if (!((fe != null) ? fe.AncestorChangeInProgress : fce.AncestorChangeInProgress))
			{
				List<ResourceDictionary> resourceDictionariesFromStyle = StyleHelper.GetResourceDictionariesFromStyle(oldStyle);
				List<ResourceDictionary> resourceDictionariesFromStyle2 = StyleHelper.GetResourceDictionariesFromStyle(newStyle);
				if ((resourceDictionariesFromStyle != null && resourceDictionariesFromStyle.Count > 0) || (resourceDictionariesFromStyle2 != null && resourceDictionariesFromStyle2.Count > 0))
				{
					StyleHelper.SetShouldLookupImplicitStyles(new FrameworkObject(fe, fce), resourceDictionariesFromStyle2);
					TreeWalkHelper.InvalidateOnResourcesChange(fe, fce, new ResourcesChangeInfo(resourceDictionariesFromStyle, resourceDictionariesFromStyle2, true, false, container));
				}
			}
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x001899F0 File Offset: 0x001889F0
		internal static void DoTemplateResourcesInvalidations(DependencyObject container, FrameworkElement fe, FrameworkContentElement fce, object oldTemplate, object newTemplate)
		{
			if (!((fe != null) ? fe.AncestorChangeInProgress : fce.AncestorChangeInProgress))
			{
				List<ResourceDictionary> resourceDictionaryFromTemplate = StyleHelper.GetResourceDictionaryFromTemplate(oldTemplate);
				List<ResourceDictionary> resourceDictionaryFromTemplate2 = StyleHelper.GetResourceDictionaryFromTemplate(newTemplate);
				if (resourceDictionaryFromTemplate != resourceDictionaryFromTemplate2)
				{
					StyleHelper.SetShouldLookupImplicitStyles(new FrameworkObject(fe, fce), resourceDictionaryFromTemplate2);
					TreeWalkHelper.InvalidateOnResourcesChange(fe, fce, new ResourcesChangeInfo(resourceDictionaryFromTemplate, resourceDictionaryFromTemplate2, false, true, container));
				}
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00189A44 File Offset: 0x00188A44
		private static void SetShouldLookupImplicitStyles(FrameworkObject fo, List<ResourceDictionary> dictionaries)
		{
			if (dictionaries != null && dictionaries.Count > 0 && !fo.ShouldLookupImplicitStyles)
			{
				for (int i = 0; i < dictionaries.Count; i++)
				{
					if (dictionaries[i].HasImplicitStyles)
					{
						fo.ShouldLookupImplicitStyles = true;
						return;
					}
				}
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00189A90 File Offset: 0x00188A90
		private static List<ResourceDictionary> GetResourceDictionariesFromStyle(Style style)
		{
			List<ResourceDictionary> list = null;
			while (style != null)
			{
				if (style._resources != null)
				{
					if (list == null)
					{
						list = new List<ResourceDictionary>(1);
					}
					list.Add(style._resources);
				}
				style = style.BasedOn;
			}
			return list;
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00189ACC File Offset: 0x00188ACC
		private static List<ResourceDictionary> GetResourceDictionaryFromTemplate(object template)
		{
			ResourceDictionary resourceDictionary = null;
			if (template is FrameworkTemplate)
			{
				resourceDictionary = ((FrameworkTemplate)template)._resources;
			}
			if (resourceDictionary != null)
			{
				return new List<ResourceDictionary>(1)
				{
					resourceDictionary
				};
			}
			return null;
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00189B04 File Offset: 0x00188B04
		internal static void UpdateLoadedFlag(DependencyObject d, Style oldStyle, Style newStyle)
		{
			Invariant.Assert(oldStyle != null || newStyle != null);
			if ((oldStyle == null || !oldStyle.HasLoadedChangeHandler) && newStyle != null && newStyle.HasLoadedChangeHandler)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(d);
				return;
			}
			if (oldStyle != null && oldStyle.HasLoadedChangeHandler && (newStyle == null || !newStyle.HasLoadedChangeHandler))
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(d);
			}
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00189B59 File Offset: 0x00188B59
		internal static void UpdateLoadedFlag(DependencyObject d, FrameworkTemplate oldFrameworkTemplate, FrameworkTemplate newFrameworkTemplate)
		{
			if ((oldFrameworkTemplate == null || !oldFrameworkTemplate.HasLoadedChangeHandler) && newFrameworkTemplate != null && newFrameworkTemplate.HasLoadedChangeHandler)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(d);
				return;
			}
			if (oldFrameworkTemplate != null && oldFrameworkTemplate.HasLoadedChangeHandler && (newFrameworkTemplate == null || !newFrameworkTemplate.HasLoadedChangeHandler))
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(d);
			}
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x00189B94 File Offset: 0x00188B94
		internal static void InvalidateContainerDependents(DependencyObject container, ref FrugalStructList<ContainerDependent> exclusionContainerDependents, ref FrugalStructList<ContainerDependent> oldContainerDependents, ref FrugalStructList<ContainerDependent> newContainerDependents)
		{
			int count = oldContainerDependents.Count;
			for (int i = 0; i < count; i++)
			{
				DependencyProperty property = oldContainerDependents[i].Property;
				if (!StyleHelper.IsSetOnContainer(property, ref exclusionContainerDependents, false))
				{
					container.InvalidateProperty(property);
				}
			}
			count = newContainerDependents.Count;
			if (count > 0)
			{
				FrameworkObject fo = new FrameworkObject(container);
				for (int j = 0; j < count; j++)
				{
					DependencyProperty property2 = newContainerDependents[j].Property;
					if (!StyleHelper.IsSetOnContainer(property2, ref exclusionContainerDependents, false) && !StyleHelper.IsSetOnContainer(property2, ref oldContainerDependents, false))
					{
						StyleHelper.ApplyStyleOrTemplateValue(fo, property2);
					}
				}
			}
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00189C24 File Offset: 0x00188C24
		internal static void ApplyTemplatedParentValue(DependencyObject container, FrameworkObject child, int childIndex, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, DependencyProperty dp, FrameworkElementFactory templateRoot)
		{
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			if (StyleHelper.GetValueFromTemplatedParent(container, childIndex, child, dp, ref childRecordFromChildIndex, templateRoot, ref effectiveValueEntry))
			{
				DependencyObject @do = child.DO;
				@do.UpdateEffectiveValue(@do.LookupEntry(dp.GlobalIndex), dp, dp.GetMetadata(@do.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			}
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00189C90 File Offset: 0x00188C90
		internal static bool IsValueDynamic(DependencyObject container, int childIndex, DependencyProperty dp)
		{
			bool result = false;
			FrameworkObject frameworkObject = new FrameworkObject(container);
			FrameworkTemplate templateInternal = frameworkObject.TemplateInternal;
			if (templateInternal != null)
			{
				FrugalStructList<ChildRecord> childRecordFromChildIndex = templateInternal.ChildRecordFromChildIndex;
				if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
				{
					ChildRecord childRecord = childRecordFromChildIndex[childIndex];
					int num = childRecord.ValueLookupListFromProperty.Search(dp.GlobalIndex);
					if (num >= 0 && childRecord.ValueLookupListFromProperty.Entries[num].Value.Count > 0)
					{
						ChildValueLookup childValueLookup = childRecord.ValueLookupListFromProperty.Entries[num].Value.List[0];
						result = (childValueLookup.LookupType == ValueLookupType.Resource || childValueLookup.LookupType == ValueLookupType.TemplateBinding || (childValueLookup.LookupType == ValueLookupType.Simple && childValueLookup.Value is BindingBase));
					}
				}
			}
			return result;
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00189D6C File Offset: 0x00188D6C
		internal static bool GetValueFromTemplatedParent(DependencyObject container, int childIndex, FrameworkObject child, DependencyProperty dp, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, FrameworkElementFactory templateRoot, ref EffectiveValueEntry entry)
		{
			ValueLookupType valueLookupType = ValueLookupType.Simple;
			object obj = StyleHelper.GetChildValue(StyleHelper.TemplateDataField, container, childIndex, child, dp, ref childRecordFromChildIndex, ref entry, out valueLookupType, templateRoot);
			if (obj != DependencyProperty.UnsetValue)
			{
				if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplateTrigger;
				}
				else
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
				}
				return true;
			}
			if (child.StoresParentTemplateValues)
			{
				HybridDictionary value = StyleHelper.ParentTemplateValuesField.GetValue(child.DO);
				if (value.Contains(dp))
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
					obj = value[dp];
					entry.Value = obj;
					if (obj is MarkupExtension)
					{
						StyleHelper.GetInstanceValue(StyleHelper.TemplateDataField, container, child.FE, child.FCE, childIndex, dp, -1, ref entry);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00189E28 File Offset: 0x00188E28
		internal static void ApplyStyleOrTemplateValue(FrameworkObject fo, DependencyProperty dp)
		{
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			if (StyleHelper.GetValueFromStyleOrTemplate(fo, dp, ref effectiveValueEntry))
			{
				DependencyObject @do = fo.DO;
				@do.UpdateEffectiveValue(@do.LookupEntry(dp.GlobalIndex), dp, dp.GetMetadata(@do.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			}
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00189E8C File Offset: 0x00188E8C
		internal static bool GetValueFromStyleOrTemplate(FrameworkObject fo, DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			ValueLookupType valueLookupType = ValueLookupType.Simple;
			object obj = DependencyProperty.UnsetValue;
			EffectiveValueEntry effectiveValueEntry = entry;
			Style style = fo.Style;
			if (style != null && StyleHelper.ShouldGetValueFromStyle(dp))
			{
				object childValue = StyleHelper.GetChildValue(StyleHelper.StyleDataField, fo.DO, 0, fo, dp, ref style.ChildRecordFromChildIndex, ref effectiveValueEntry, out valueLookupType, null);
				if (childValue != DependencyProperty.UnsetValue)
				{
					if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
					{
						entry = effectiveValueEntry;
						entry.BaseValueSourceInternal = BaseValueSourceInternal.StyleTrigger;
						return true;
					}
					obj = childValue;
				}
			}
			if (StyleHelper.ShouldGetValueFromTemplate(dp))
			{
				FrameworkTemplate templateInternal = fo.TemplateInternal;
				if (templateInternal != null)
				{
					object childValue = StyleHelper.GetChildValue(StyleHelper.TemplateDataField, fo.DO, 0, fo, dp, ref templateInternal.ChildRecordFromChildIndex, ref entry, out valueLookupType, templateInternal.VisualTree);
					if (childValue != DependencyProperty.UnsetValue)
					{
						entry.BaseValueSourceInternal = BaseValueSourceInternal.TemplateTrigger;
						return true;
					}
				}
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				entry = effectiveValueEntry;
				entry.BaseValueSourceInternal = BaseValueSourceInternal.Style;
				return true;
			}
			if (StyleHelper.ShouldGetValueFromThemeStyle(dp))
			{
				Style themeStyle = fo.ThemeStyle;
				if (themeStyle != null)
				{
					object childValue = StyleHelper.GetChildValue(StyleHelper.ThemeStyleDataField, fo.DO, 0, fo, dp, ref themeStyle.ChildRecordFromChildIndex, ref entry, out valueLookupType, null);
					if (childValue != DependencyProperty.UnsetValue)
					{
						if (valueLookupType == ValueLookupType.Trigger || valueLookupType == ValueLookupType.PropertyTriggerResource || valueLookupType == ValueLookupType.DataTrigger || valueLookupType == ValueLookupType.DataTriggerResource)
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.ThemeStyleTrigger;
						}
						else
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.ThemeStyle;
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00189FCC File Offset: 0x00188FCC
		internal static void SortResourceDependents(ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			int count = resourceDependents.Count;
			for (int i = 1; i < count; i++)
			{
				ChildPropertyDependent childPropertyDependent = resourceDependents[i];
				int childIndex = childPropertyDependent.ChildIndex;
				int globalIndex = childPropertyDependent.Property.GlobalIndex;
				int num = i - 1;
				while (num >= 0 && (childIndex < resourceDependents[num].ChildIndex || (childIndex == resourceDependents[num].ChildIndex && globalIndex < resourceDependents[num].Property.GlobalIndex)))
				{
					resourceDependents[num + 1] = resourceDependents[num];
					num--;
				}
				if (num < i - 1)
				{
					resourceDependents[num + 1] = childPropertyDependent;
				}
			}
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x0018A07C File Offset: 0x0018907C
		internal static void InvalidateResourceDependents(DependencyObject container, ResourcesChangeInfo info, ref FrugalStructList<ChildPropertyDependent> resourceDependents, bool invalidateVisualTreeToo)
		{
			List<DependencyObject> value = StyleHelper.TemplatedFeChildrenField.GetValue(container);
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				if (info.Contains(resourceDependents[i].Name, false))
				{
					DependencyObject dependencyObject = null;
					DependencyProperty property = resourceDependents[i].Property;
					int childIndex = resourceDependents[i].ChildIndex;
					if (childIndex == 0)
					{
						dependencyObject = container;
					}
					else if (invalidateVisualTreeToo)
					{
						dependencyObject = StyleHelper.GetChild(value, childIndex);
						if (dependencyObject == null)
						{
							throw new InvalidOperationException(SR.Get("ChildTemplateInstanceDoesNotExist"));
						}
					}
					if (dependencyObject != null)
					{
						dependencyObject.InvalidateProperty(property);
						int globalIndex = property.GlobalIndex;
						while (++i < resourceDependents.Count && resourceDependents[i].ChildIndex == childIndex && resourceDependents[i].Property.GlobalIndex == globalIndex)
						{
						}
						i--;
					}
				}
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x0018A154 File Offset: 0x00189154
		internal static void InvalidateResourceDependentsForChild(DependencyObject container, DependencyObject child, int childIndex, ResourcesChangeInfo info, FrameworkTemplate parentTemplate)
		{
			FrugalStructList<ChildPropertyDependent> resourceDependents = parentTemplate.ResourceDependents;
			int count = resourceDependents.Count;
			for (int i = 0; i < count; i++)
			{
				if (resourceDependents[i].ChildIndex == childIndex && info.Contains(resourceDependents[i].Name, false))
				{
					DependencyProperty property = resourceDependents[i].Property;
					child.InvalidateProperty(property);
					int globalIndex = property.GlobalIndex;
					while (++i < resourceDependents.Count && resourceDependents[i].ChildIndex == childIndex && resourceDependents[i].Property.GlobalIndex == globalIndex)
					{
					}
					i--;
				}
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0018A204 File Offset: 0x00189204
		internal static bool HasResourceDependentsForChild(int childIndex, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			for (int i = 0; i < resourceDependents.Count; i++)
			{
				if (resourceDependents[i].ChildIndex == childIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0018A234 File Offset: 0x00189234
		internal static void InvalidatePropertiesOnTemplateNode(DependencyObject container, FrameworkObject child, int childIndex, ref FrugalStructList<ChildRecord> childRecordFromChildIndex, bool isDetach, FrameworkElementFactory templateRoot)
		{
			if (0 <= childIndex && childIndex < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[childIndex];
				int count = childRecord.ValueLookupListFromProperty.Count;
				if (count > 0)
				{
					for (int i = 0; i < count; i++)
					{
						DependencyProperty property = childRecord.ValueLookupListFromProperty.Entries[i].Value.List[0].Property;
						if (!isDetach)
						{
							StyleHelper.ApplyTemplatedParentValue(container, child, childIndex, ref childRecordFromChildIndex, property, templateRoot);
						}
						else if (property != FrameworkElement.StyleProperty)
						{
							bool flag = true;
							if (property.IsPotentiallyInherited)
							{
								PropertyMetadata metadata = property.GetMetadata(child.DO.DependencyObjectType);
								if (metadata != null && metadata.IsInherited)
								{
									flag = false;
								}
							}
							if (flag)
							{
								child.DO.InvalidateProperty(property);
							}
						}
					}
				}
			}
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x0018A304 File Offset: 0x00189304
		internal static bool IsSetOnContainer(DependencyProperty dp, ref FrugalStructList<ContainerDependent> containerDependents, bool alsoFromTriggers)
		{
			for (int i = 0; i < containerDependents.Count; i++)
			{
				if (dp == containerDependents[i].Property)
				{
					return alsoFromTriggers || !containerDependents[i].FromVisualTrigger;
				}
			}
			return false;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x0018A348 File Offset: 0x00189348
		internal static void OnTriggerSourcePropertyInvalidated(Style ownerStyle, FrameworkTemplate frameworkTemplate, DependencyObject container, DependencyProperty dp, DependencyPropertyChangedEventArgs changedArgs, bool invalidateOnlyContainer, ref FrugalStructList<ItemStructMap<TriggerSourceRecord>> triggerSourceRecordFromChildIndex, ref FrugalMap propertyTriggersWithActions, int sourceChildIndex)
		{
			if (0 <= sourceChildIndex && sourceChildIndex < triggerSourceRecordFromChildIndex.Count)
			{
				ItemStructMap<TriggerSourceRecord> itemStructMap = triggerSourceRecordFromChildIndex[sourceChildIndex];
				int num = itemStructMap.Search(dp.GlobalIndex);
				if (num >= 0)
				{
					TriggerSourceRecord value = itemStructMap.Entries[num].Value;
					StyleHelper.InvalidateDependents(ownerStyle, frameworkTemplate, container, dp, ref value.ChildPropertyDependents, invalidateOnlyContainer);
				}
			}
			object obj = propertyTriggersWithActions[dp.GlobalIndex];
			if (obj != DependencyProperty.UnsetValue)
			{
				TriggerBase triggerBase = obj as TriggerBase;
				if (triggerBase != null)
				{
					StyleHelper.InvokePropertyTriggerActions(triggerBase, container, dp, changedArgs, sourceChildIndex, ownerStyle, frameworkTemplate);
					return;
				}
				List<TriggerBase> list = (List<TriggerBase>)obj;
				for (int i = 0; i < list.Count; i++)
				{
					StyleHelper.InvokePropertyTriggerActions(list[i], container, dp, changedArgs, sourceChildIndex, ownerStyle, frameworkTemplate);
				}
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0018A40C File Offset: 0x0018940C
		private static void InvalidateDependents(Style ownerStyle, FrameworkTemplate frameworkTemplate, DependencyObject container, DependencyProperty dp, ref FrugalStructList<ChildPropertyDependent> dependents, bool invalidateOnlyContainer)
		{
			for (int i = 0; i < dependents.Count; i++)
			{
				DependencyObject dependencyObject = null;
				int childIndex = dependents[i].ChildIndex;
				if (childIndex == 0)
				{
					dependencyObject = container;
				}
				else if (!invalidateOnlyContainer)
				{
					List<DependencyObject> value = StyleHelper.TemplatedFeChildrenField.GetValue(container);
					if (value != null && childIndex <= value.Count)
					{
						dependencyObject = StyleHelper.GetChild(value, childIndex);
					}
				}
				DependencyProperty property = dependents[i].Property;
				bool flag;
				if (dependencyObject != null && dependencyObject.GetValueSource(property, null, out flag) != BaseValueSourceInternal.Local)
				{
					dependencyObject.InvalidateProperty(property, true);
				}
			}
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x0018A494 File Offset: 0x00189494
		private static void InvokeDataTriggerActions(TriggerBase triggerBase, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs bindingChangedArgs, Style style, FrameworkTemplate frameworkTemplate, UncommonField<HybridDictionary[]> dataField)
		{
			DataTrigger dataTrigger = triggerBase as DataTrigger;
			bool oldState;
			bool newState;
			if (dataTrigger != null)
			{
				StyleHelper.EvaluateOldNewStates(dataTrigger, triggerContainer, binding, bindingChangedArgs, dataField, style, frameworkTemplate, out oldState, out newState);
			}
			else
			{
				StyleHelper.EvaluateOldNewStates((MultiDataTrigger)triggerBase, triggerContainer, binding, bindingChangedArgs, dataField, style, frameworkTemplate, out oldState, out newState);
			}
			StyleHelper.InvokeEnterOrExitActions(triggerBase, oldState, newState, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x0018A4E8 File Offset: 0x001894E8
		private static void InvokePropertyTriggerActions(TriggerBase triggerBase, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate)
		{
			Trigger trigger = triggerBase as Trigger;
			bool oldState;
			bool newState;
			if (trigger != null)
			{
				StyleHelper.EvaluateOldNewStates(trigger, triggerContainer, changedProperty, changedArgs, sourceChildIndex, style, frameworkTemplate, out oldState, out newState);
			}
			else
			{
				StyleHelper.EvaluateOldNewStates((MultiTrigger)triggerBase, triggerContainer, changedProperty, changedArgs, sourceChildIndex, style, frameworkTemplate, out oldState, out newState);
			}
			StyleHelper.InvokeEnterOrExitActions(triggerBase, oldState, newState, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0018A53C File Offset: 0x0018953C
		private static void ExecuteOnApplyEnterExitActions(FrameworkElement fe, FrameworkContentElement fce, Style style, UncommonField<HybridDictionary[]> dataField)
		{
			if (style == null)
			{
				return;
			}
			if (style.PropertyTriggersWithActions.Count == 0 && style.DataTriggersWithActions == null)
			{
				return;
			}
			TriggerCollection triggers = style.Triggers;
			StyleHelper.ExecuteOnApplyEnterExitActionsLoop((fe != null) ? fe : fce, triggers, style, null, dataField);
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x0018A57C File Offset: 0x0018957C
		private static void ExecuteOnApplyEnterExitActions(FrameworkElement fe, FrameworkContentElement fce, FrameworkTemplate ft)
		{
			if (ft == null)
			{
				return;
			}
			if (ft != null && ft.PropertyTriggersWithActions.Count == 0 && ft.DataTriggersWithActions == null)
			{
				return;
			}
			TriggerCollection triggersInternal = ft.TriggersInternal;
			StyleHelper.ExecuteOnApplyEnterExitActionsLoop((fe != null) ? fe : fce, triggersInternal, null, ft, StyleHelper.TemplateDataField);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x0018A5C4 File Offset: 0x001895C4
		private static void ExecuteOnApplyEnterExitActionsLoop(DependencyObject triggerContainer, TriggerCollection triggers, Style style, FrameworkTemplate ft, UncommonField<HybridDictionary[]> dataField)
		{
			for (int i = 0; i < triggers.Count; i++)
			{
				TriggerBase triggerBase = triggers[i];
				if ((triggerBase.HasEnterActions || triggerBase.HasExitActions) && (triggerBase.ExecuteEnterActionsOnApply || triggerBase.ExecuteExitActionsOnApply) && StyleHelper.NoSourceNameInTrigger(triggerBase))
				{
					bool currentState = triggerBase.GetCurrentState(triggerContainer, dataField);
					if (currentState && triggerBase.ExecuteEnterActionsOnApply)
					{
						StyleHelper.InvokeActions(triggerBase.EnterActions, triggerBase, triggerContainer, style, ft);
					}
					else if (!currentState && triggerBase.ExecuteExitActionsOnApply)
					{
						StyleHelper.InvokeActions(triggerBase.ExitActions, triggerBase, triggerContainer, style, ft);
					}
				}
			}
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0018A654 File Offset: 0x00189654
		private static bool NoSourceNameInTrigger(TriggerBase triggerBase)
		{
			Trigger trigger = triggerBase as Trigger;
			if (trigger != null)
			{
				return trigger.SourceName == null;
			}
			MultiTrigger multiTrigger = triggerBase as MultiTrigger;
			if (multiTrigger != null)
			{
				for (int i = 0; i < multiTrigger.Conditions.Count; i++)
				{
					if (multiTrigger.Conditions[i].SourceName != null)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x0018A6B0 File Offset: 0x001896B0
		private static void InvokeEnterOrExitActions(TriggerBase triggerBase, bool oldState, bool newState, DependencyObject triggerContainer, Style style, FrameworkTemplate frameworkTemplate)
		{
			TriggerActionCollection actions;
			if (!oldState && newState)
			{
				actions = triggerBase.EnterActions;
			}
			else if (oldState && !newState)
			{
				actions = triggerBase.ExitActions;
			}
			else
			{
				actions = null;
			}
			StyleHelper.InvokeActions(actions, triggerBase, triggerContainer, style, frameworkTemplate);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x0018A6EB File Offset: 0x001896EB
		private static void InvokeActions(TriggerActionCollection actions, TriggerBase triggerBase, DependencyObject triggerContainer, Style style, FrameworkTemplate frameworkTemplate)
		{
			if (actions != null)
			{
				if (StyleHelper.CanInvokeActionsNow(triggerContainer, frameworkTemplate))
				{
					StyleHelper.InvokeActions(triggerBase, triggerContainer, actions, style, frameworkTemplate);
					return;
				}
				StyleHelper.DeferActions(triggerBase, triggerContainer, actions, style, frameworkTemplate);
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0018A714 File Offset: 0x00189714
		private static bool CanInvokeActionsNow(DependencyObject container, FrameworkTemplate frameworkTemplate)
		{
			bool result;
			if (frameworkTemplate != null)
			{
				if (((FrameworkElement)container).HasTemplateGeneratedSubTree)
				{
					ContentPresenter contentPresenter = container as ContentPresenter;
					result = (contentPresenter == null || contentPresenter.TemplateIsCurrent);
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x0018A754 File Offset: 0x00189754
		private static void DeferActions(TriggerBase triggerBase, DependencyObject triggerContainer, TriggerActionCollection actions, Style style, FrameworkTemplate frameworkTemplate)
		{
			DeferredAction item;
			item.TriggerBase = triggerBase;
			item.TriggerActionCollection = actions;
			ConditionalWeakTable<DependencyObject, List<DeferredAction>> conditionalWeakTable;
			if (frameworkTemplate != null)
			{
				conditionalWeakTable = frameworkTemplate.DeferredActions;
				if (conditionalWeakTable == null)
				{
					conditionalWeakTable = new ConditionalWeakTable<DependencyObject, List<DeferredAction>>();
					frameworkTemplate.DeferredActions = conditionalWeakTable;
				}
			}
			else
			{
				conditionalWeakTable = null;
			}
			if (conditionalWeakTable != null)
			{
				List<DeferredAction> list;
				if (!conditionalWeakTable.TryGetValue(triggerContainer, out list))
				{
					list = new List<DeferredAction>();
					conditionalWeakTable.Add(triggerContainer, list);
				}
				list.Add(item);
			}
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0018A7B8 File Offset: 0x001897B8
		internal static void InvokeDeferredActions(DependencyObject triggerContainer, FrameworkTemplate frameworkTemplate)
		{
			List<DeferredAction> list;
			if (frameworkTemplate != null && frameworkTemplate.DeferredActions != null && frameworkTemplate.DeferredActions.TryGetValue(triggerContainer, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					StyleHelper.InvokeActions(list[i].TriggerBase, triggerContainer, list[i].TriggerActionCollection, null, frameworkTemplate);
				}
				frameworkTemplate.DeferredActions.Remove(triggerContainer);
			}
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0018A820 File Offset: 0x00189820
		internal static void InvokeActions(TriggerBase triggerBase, DependencyObject triggerContainer, TriggerActionCollection actions, Style style, FrameworkTemplate frameworkTemplate)
		{
			for (int i = 0; i < actions.Count; i++)
			{
				actions[i].Invoke(triggerContainer as FrameworkElement, triggerContainer as FrameworkContentElement, style, frameworkTemplate, triggerBase.Layer);
			}
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0018A860 File Offset: 0x00189860
		private static void EvaluateOldNewStates(Trigger trigger, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			int num = 0;
			if (trigger.SourceName != null)
			{
				num = StyleHelper.QueryChildIndexFromChildName(trigger.SourceName, frameworkTemplate.ChildIndexFromChildName);
			}
			if (num == sourceChildIndex)
			{
				TriggerCondition[] triggerConditions = trigger.TriggerConditions;
				oldState = triggerConditions[0].Match(changedArgs.OldValue);
				newState = triggerConditions[0].Match(changedArgs.NewValue);
				return;
			}
			oldState = false;
			newState = false;
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x0018A8CC File Offset: 0x001898CC
		private static void EvaluateOldNewStates(DataTrigger dataTrigger, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs bindingChangedArgs, UncommonField<HybridDictionary[]> dataField, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = dataTrigger.TriggerConditions;
			oldState = triggerConditions[0].ConvertAndMatch(bindingChangedArgs.OldValue);
			newState = triggerConditions[0].ConvertAndMatch(bindingChangedArgs.NewValue);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x0018A90C File Offset: 0x0018990C
		private static void EvaluateOldNewStates(MultiTrigger multiTrigger, DependencyObject triggerContainer, DependencyProperty changedProperty, DependencyPropertyChangedEventArgs changedArgs, int sourceChildIndex, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = multiTrigger.TriggerConditions;
			oldState = false;
			newState = false;
			for (int i = 0; i < triggerConditions.Length; i++)
			{
				int num;
				DependencyObject dependencyObject;
				if (triggerConditions[i].SourceChildIndex != 0)
				{
					num = triggerConditions[i].SourceChildIndex;
					dependencyObject = StyleHelper.GetChild(triggerContainer, num);
				}
				else
				{
					num = 0;
					dependencyObject = triggerContainer;
				}
				if (triggerConditions[i].Property == changedProperty && num == sourceChildIndex)
				{
					oldState = triggerConditions[i].Match(changedArgs.OldValue);
					newState = triggerConditions[i].Match(changedArgs.NewValue);
					if (oldState == newState)
					{
						return;
					}
				}
				else
				{
					object value = dependencyObject.GetValue(triggerConditions[i].Property);
					if (!triggerConditions[i].Match(value))
					{
						oldState = false;
						newState = false;
						return;
					}
				}
			}
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x0018A9E0 File Offset: 0x001899E0
		private static void EvaluateOldNewStates(MultiDataTrigger multiDataTrigger, DependencyObject triggerContainer, BindingBase binding, BindingValueChangedEventArgs changedArgs, UncommonField<HybridDictionary[]> dataField, Style style, FrameworkTemplate frameworkTemplate, out bool oldState, out bool newState)
		{
			TriggerCondition[] triggerConditions = multiDataTrigger.TriggerConditions;
			oldState = false;
			newState = false;
			for (int i = 0; i < multiDataTrigger.Conditions.Count; i++)
			{
				BindingBase binding2 = triggerConditions[i].Binding;
				if (binding2 == binding)
				{
					oldState = triggerConditions[i].ConvertAndMatch(changedArgs.OldValue);
					newState = triggerConditions[i].ConvertAndMatch(changedArgs.NewValue);
					if (oldState == newState)
					{
						return;
					}
				}
				else
				{
					object dataTriggerValue = StyleHelper.GetDataTriggerValue(dataField, triggerContainer, binding2);
					if (!triggerConditions[i].ConvertAndMatch(dataTriggerValue))
					{
						oldState = false;
						newState = false;
						return;
					}
				}
			}
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x0018AA7C File Offset: 0x00189A7C
		internal static void AddPropertyTriggerWithAction(TriggerBase triggerBase, DependencyProperty property, ref FrugalMap triggersWithActions)
		{
			object obj = triggersWithActions[property.GlobalIndex];
			if (obj == DependencyProperty.UnsetValue)
			{
				triggersWithActions[property.GlobalIndex] = triggerBase;
			}
			else
			{
				TriggerBase triggerBase2 = obj as TriggerBase;
				if (triggerBase2 != null)
				{
					List<TriggerBase> list = new List<TriggerBase>();
					list.Add(triggerBase2);
					list.Add(triggerBase);
					triggersWithActions[property.GlobalIndex] = list;
				}
				else
				{
					((List<TriggerBase>)obj).Add(triggerBase);
				}
			}
			triggerBase.EstablishLayer();
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x0018AAEC File Offset: 0x00189AEC
		internal static void AddDataTriggerWithAction(TriggerBase triggerBase, BindingBase binding, ref HybridDictionary dataTriggersWithActions)
		{
			if (dataTriggersWithActions == null)
			{
				dataTriggersWithActions = new HybridDictionary();
			}
			object obj = dataTriggersWithActions[binding];
			if (obj == null)
			{
				dataTriggersWithActions[binding] = triggerBase;
			}
			else
			{
				TriggerBase triggerBase2 = obj as TriggerBase;
				if (triggerBase2 != null)
				{
					List<TriggerBase> list = new List<TriggerBase>();
					list.Add(triggerBase2);
					list.Add(triggerBase);
					dataTriggersWithActions[binding] = list;
				}
				else
				{
					((List<TriggerBase>)obj).Add(triggerBase);
				}
			}
			triggerBase.EstablishLayer();
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x0018AB58 File Offset: 0x00189B58
		private static void OnBindingValueInStyleChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			Style style = (frameworkElement != null) ? frameworkElement.Style : frameworkContentElement.Style;
			HybridDictionary dataTriggerRecordFromBinding = style._dataTriggerRecordFromBinding;
			if (dataTriggerRecordFromBinding != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(style, null, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(style, null, targetElement, parentBindingBase, e, StyleHelper.StyleDataField);
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x0018ABE8 File Offset: 0x00189BE8
		private static void OnBindingValueInTemplateChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			FrameworkTemplate templateInternal = frameworkElement.TemplateInternal;
			HybridDictionary hybridDictionary = null;
			if (templateInternal != null)
			{
				hybridDictionary = templateInternal._dataTriggerRecordFromBinding;
			}
			if (hybridDictionary != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)hybridDictionary[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(null, templateInternal, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(null, templateInternal, targetElement, parentBindingBase, e, StyleHelper.TemplateDataField);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x0018AC74 File Offset: 0x00189C74
		private static void OnBindingValueInThemeStyleChanged(object sender, BindingValueChangedEventArgs e)
		{
			BindingExpressionBase bindingExpressionBase = (BindingExpressionBase)sender;
			BindingBase parentBindingBase = bindingExpressionBase.ParentBindingBase;
			DependencyObject targetElement = bindingExpressionBase.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(targetElement, out frameworkElement, out frameworkContentElement, false);
			Style style = (frameworkElement != null) ? frameworkElement.ThemeStyle : frameworkContentElement.ThemeStyle;
			HybridDictionary dataTriggerRecordFromBinding = style._dataTriggerRecordFromBinding;
			if (dataTriggerRecordFromBinding != null && !bindingExpressionBase.IsAttaching)
			{
				DataTriggerRecord dataTriggerRecord = (DataTriggerRecord)dataTriggerRecordFromBinding[parentBindingBase];
				if (dataTriggerRecord != null)
				{
					StyleHelper.InvalidateDependents(style, null, targetElement, null, ref dataTriggerRecord.Dependents, false);
				}
			}
			StyleHelper.InvokeApplicableDataTriggerActions(style, null, targetElement, parentBindingBase, e, StyleHelper.ThemeStyleDataField);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x0018AD04 File Offset: 0x00189D04
		private static void InvokeApplicableDataTriggerActions(Style style, FrameworkTemplate frameworkTemplate, DependencyObject container, BindingBase binding, BindingValueChangedEventArgs e, UncommonField<HybridDictionary[]> dataField)
		{
			HybridDictionary hybridDictionary;
			if (style != null)
			{
				hybridDictionary = style.DataTriggersWithActions;
			}
			else if (frameworkTemplate != null)
			{
				hybridDictionary = frameworkTemplate.DataTriggersWithActions;
			}
			else
			{
				hybridDictionary = null;
			}
			if (hybridDictionary != null)
			{
				object obj = hybridDictionary[binding];
				if (obj != null)
				{
					TriggerBase triggerBase = obj as TriggerBase;
					if (triggerBase != null)
					{
						StyleHelper.InvokeDataTriggerActions(triggerBase, container, binding, e, style, frameworkTemplate, dataField);
						return;
					}
					List<TriggerBase> list = (List<TriggerBase>)obj;
					for (int i = 0; i < list.Count; i++)
					{
						StyleHelper.InvokeDataTriggerActions(list[i], container, binding, e, style, frameworkTemplate, dataField);
					}
				}
			}
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x0018AD84 File Offset: 0x00189D84
		internal static int CreateChildIndexFromChildName(string childName, FrameworkTemplate frameworkTemplate)
		{
			HybridDictionary childIndexFromChildName = frameworkTemplate.ChildIndexFromChildName;
			int lastChildIndex = frameworkTemplate.LastChildIndex;
			if (childIndexFromChildName.Contains(childName))
			{
				throw new ArgumentException(SR.Get("NameScopeDuplicateNamesNotAllowed", new object[]
				{
					childName
				}));
			}
			if (lastChildIndex >= 65535)
			{
				throw new InvalidOperationException(SR.Get("StyleHasTooManyElements"));
			}
			object obj = lastChildIndex;
			childIndexFromChildName[childName] = obj;
			Interlocked.Increment(ref lastChildIndex);
			frameworkTemplate.LastChildIndex = lastChildIndex;
			return (int)obj;
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x0018AE00 File Offset: 0x00189E00
		internal static int QueryChildIndexFromChildName(string childName, HybridDictionary childIndexFromChildName)
		{
			if (childName == "~Self")
			{
				return 0;
			}
			object obj = childIndexFromChildName[childName];
			if (obj == null)
			{
				return -1;
			}
			return (int)obj;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x0018AE30 File Offset: 0x00189E30
		internal static object FindNameInTemplateContent(DependencyObject container, string childName, FrameworkTemplate frameworkTemplate)
		{
			int num = StyleHelper.QueryChildIndexFromChildName(childName, frameworkTemplate.ChildIndexFromChildName);
			if (num != -1)
			{
				return StyleHelper.GetChild(container, num);
			}
			Hashtable value = StyleHelper.TemplatedNonFeChildrenField.GetValue(container);
			if (value != null)
			{
				return value[childName];
			}
			return null;
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0018AE6E File Offset: 0x00189E6E
		internal static DependencyObject GetChild(DependencyObject container, int childIndex)
		{
			return StyleHelper.GetChild(StyleHelper.TemplatedFeChildrenField.GetValue(container), childIndex);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x0018AE81 File Offset: 0x00189E81
		internal static DependencyObject GetChild(List<DependencyObject> styledChildren, int childIndex)
		{
			if (styledChildren == null || childIndex > styledChildren.Count)
			{
				return null;
			}
			if (childIndex < 0)
			{
				throw new ArgumentOutOfRangeException("childIndex");
			}
			return styledChildren[childIndex - 1];
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0018AEA9 File Offset: 0x00189EA9
		internal static void RegisterAlternateExpressionStorage()
		{
			DependencyObject.RegisterForAlternativeExpressionStorage(new AlternativeExpressionStorageCallback(StyleHelper.GetExpressionCore), out StyleHelper._getExpression);
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x0018AEC4 File Offset: 0x00189EC4
		private static Expression GetExpressionCore(DependencyObject d, DependencyProperty dp, PropertyMetadata metadata)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				return frameworkElement.GetExpressionCore(dp, metadata);
			}
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.GetExpressionCore(dp, metadata);
			}
			return null;
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x0018AEF8 File Offset: 0x00189EF8
		internal static Expression GetExpression(DependencyObject d, DependencyProperty dp)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			bool flag = (frameworkElement != null) ? frameworkElement.IsInitialized : (frameworkContentElement == null || frameworkContentElement.IsInitialized);
			if (!flag)
			{
				if (frameworkElement != null)
				{
					frameworkElement.WriteInternalFlag(InternalFlags.IsInitialized, true);
				}
				else if (frameworkContentElement != null)
				{
					frameworkContentElement.WriteInternalFlag(InternalFlags.IsInitialized, true);
				}
			}
			Expression result = StyleHelper._getExpression(d, dp, dp.GetMetadata(d.DependencyObjectType));
			if (!flag)
			{
				if (frameworkElement != null)
				{
					frameworkElement.WriteInternalFlag(InternalFlags.IsInitialized, false);
					return result;
				}
				if (frameworkContentElement != null)
				{
					frameworkContentElement.WriteInternalFlag(InternalFlags.IsInitialized, false);
				}
			}
			return result;
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0018AF88 File Offset: 0x00189F88
		internal static RoutedEventHandlerInfo[] GetChildRoutedEventHandlers(int childIndex, RoutedEvent routedEvent, ref ItemStructList<ChildEventDependent> eventDependents)
		{
			if (childIndex > 0)
			{
				EventHandlersStore eventHandlersStore = null;
				for (int i = 0; i < eventDependents.Count; i++)
				{
					if (eventDependents.List[i].ChildIndex == childIndex)
					{
						eventHandlersStore = eventDependents.List[i].EventHandlersStore;
						break;
					}
				}
				if (eventHandlersStore != null)
				{
					return eventHandlersStore.GetRoutedEventHandlers(routedEvent);
				}
			}
			return null;
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x0018AFE0 File Offset: 0x00189FE0
		internal static bool IsStylingLogicalTree(DependencyProperty dp, object value)
		{
			return dp != ItemsControl.ItemsPanelProperty && dp != FrameworkElement.ContextMenuProperty && dp != FrameworkElement.ToolTipProperty && (value is Visual || value is ContentElement);
		}

		// Token: 0x040011AD RID: 4525
		internal static readonly UncommonField<HybridDictionary[]> StyleDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040011AE RID: 4526
		internal static readonly UncommonField<HybridDictionary[]> TemplateDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040011AF RID: 4527
		internal static readonly UncommonField<HybridDictionary> ParentTemplateValuesField = new UncommonField<HybridDictionary>();

		// Token: 0x040011B0 RID: 4528
		internal static readonly UncommonField<HybridDictionary[]> ThemeStyleDataField = new UncommonField<HybridDictionary[]>();

		// Token: 0x040011B1 RID: 4529
		internal static readonly UncommonField<List<DependencyObject>> TemplatedFeChildrenField = new UncommonField<List<DependencyObject>>();

		// Token: 0x040011B2 RID: 4530
		internal static readonly UncommonField<Hashtable> TemplatedNonFeChildrenField = new UncommonField<Hashtable>();

		// Token: 0x040011B3 RID: 4531
		internal const string SelfName = "~Self";

		// Token: 0x040011B4 RID: 4532
		internal static FrugalStructList<ContainerDependent> EmptyContainerDependents;

		// Token: 0x040011B5 RID: 4533
		internal static readonly object NotYetApplied = new NamedObject("NotYetApplied");

		// Token: 0x040011B6 RID: 4534
		private static AlternativeExpressionStorageCallback _getExpression;

		// Token: 0x040011B7 RID: 4535
		internal static RoutedEventHandler EventTriggerHandlerOnContainer = new RoutedEventHandler(StyleHelper.ExecuteEventTriggerActionsOnContainer);

		// Token: 0x040011B8 RID: 4536
		internal static RoutedEventHandler EventTriggerHandlerOnChild = new RoutedEventHandler(StyleHelper.ExecuteEventTriggerActionsOnChild);

		// Token: 0x040011B9 RID: 4537
		internal const int UnsharedTemplateContentPropertyIndex = -1;
	}
}
