using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xaml;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.Hashing.PresentationFramework;

namespace MS.Internal
{
	// Token: 0x020000F5 RID: 245
	internal static class Helper
	{
		// Token: 0x06000585 RID: 1413 RVA: 0x001030B1 File Offset: 0x001020B1
		internal static object ResourceFailureThrow(object key)
		{
			return new Helper.FindResourceHelper(key).TryCatchWhen();
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x001030C0 File Offset: 0x001020C0
		internal static object FindTemplateResourceFromAppOrSystem(DependencyObject target, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			if (Application.Current != null)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj = Application.Current.FindResourceInternal(keys[i]);
					if (obj != null)
					{
						bestMatch = i;
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			if (bestMatch >= exactMatch)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj2 = SystemResources.FindResourceInternal(keys[i]);
					if (obj2 != null)
					{
						bestMatch = i;
						result = obj2;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00103134 File Offset: 0x00102134
		internal static DependencyObject FindMentor(DependencyObject d)
		{
			while (d != null)
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
				if (frameworkElement != null)
				{
					return frameworkElement;
				}
				if (frameworkContentElement != null)
				{
					return frameworkContentElement;
				}
				d = d.InheritanceContext;
			}
			return null;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00103164 File Offset: 0x00102164
		internal static bool HasDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, false, true);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0010316F File Offset: 0x0010216F
		internal static bool HasDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, true, true);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0010317A File Offset: 0x0010217A
		internal static bool HasUnmodifiedDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, false, false);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00103185 File Offset: 0x00102185
		internal static bool HasUnmodifiedDefaultOrInheritedValue(DependencyObject d, DependencyProperty dp)
		{
			return Helper.HasDefaultOrInheritedValueImpl(d, dp, true, false);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00103190 File Offset: 0x00102190
		private static bool HasDefaultOrInheritedValueImpl(DependencyObject d, DependencyProperty dp, bool checkInherited, bool ignoreModifiers)
		{
			PropertyMetadata metadata = dp.GetMetadata(d);
			bool flag;
			BaseValueSourceInternal valueSource = d.GetValueSource(dp, metadata, out flag);
			if (valueSource == BaseValueSourceInternal.Default || (checkInherited && valueSource == BaseValueSourceInternal.Inherited))
			{
				if (ignoreModifiers && (d is FrameworkElement || d is FrameworkContentElement))
				{
					flag = false;
				}
				return !flag;
			}
			return false;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x001031D8 File Offset: 0x001021D8
		internal static void DowncastToFEorFCE(DependencyObject d, out FrameworkElement fe, out FrameworkContentElement fce, bool throwIfNeither)
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
			if (throwIfNeither)
			{
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					d.GetType()
				}));
			}
			fe = null;
			fce = null;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00103240 File Offset: 0x00102240
		internal static void CheckStyleAndStyleSelector(string name, DependencyProperty styleProperty, DependencyProperty styleSelectorProperty, DependencyObject d)
		{
			if (TraceData.IsEnabled)
			{
				object obj = d.ReadLocalValue(styleSelectorProperty);
				if (obj != DependencyProperty.UnsetValue && (obj is StyleSelector || obj is ResourceReferenceExpression))
				{
					object obj2 = d.ReadLocalValue(styleProperty);
					if (obj2 != DependencyProperty.UnsetValue && (obj2 is Style || obj2 is ResourceReferenceExpression))
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.StyleAndStyleSelectorDefined(new object[]
						{
							name
						}), null, new object[]
						{
							d
						}, null);
					}
				}
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x001032B5 File Offset: 0x001022B5
		internal static void CheckTemplateAndTemplateSelector(string name, DependencyProperty templateProperty, DependencyProperty templateSelectorProperty, DependencyObject d)
		{
			if (TraceData.IsEnabled && Helper.IsTemplateSelectorDefined(templateSelectorProperty, d) && Helper.IsTemplateDefined(templateProperty, d))
			{
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.TemplateAndTemplateSelectorDefined(new object[]
				{
					name
				}), null, new object[]
				{
					d
				}, null);
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x001032F4 File Offset: 0x001022F4
		internal static bool IsTemplateSelectorDefined(DependencyProperty templateSelectorProperty, DependencyObject d)
		{
			object obj = d.ReadLocalValue(templateSelectorProperty);
			return obj != DependencyProperty.UnsetValue && obj != null && (obj is DataTemplateSelector || obj is ResourceReferenceExpression);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0010332C File Offset: 0x0010232C
		internal static bool IsTemplateDefined(DependencyProperty templateProperty, DependencyObject d)
		{
			object obj = d.ReadLocalValue(templateProperty);
			return obj != DependencyProperty.UnsetValue && obj != null && (obj is FrameworkTemplate || obj is ResourceReferenceExpression);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00103364 File Offset: 0x00102364
		internal static object FindNameInTemplate(string name, DependencyObject templatedParent)
		{
			FrameworkElement frameworkElement = templatedParent as FrameworkElement;
			return frameworkElement.TemplateInternal.FindName(name, frameworkElement);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00103388 File Offset: 0x00102388
		internal static IGeneratorHost GeneratorHostForElement(DependencyObject element)
		{
			DependencyObject dependencyObject = null;
			DependencyObject dependencyObject2 = null;
			while (element != null)
			{
				while (element != null)
				{
					dependencyObject = element;
					element = Helper.GetTemplatedParent(element);
					if (dependencyObject is ContentPresenter)
					{
						ComboBox comboBox = element as ComboBox;
						if (comboBox != null)
						{
							return comboBox;
						}
					}
				}
				Visual visual = dependencyObject as Visual;
				if (visual != null)
				{
					dependencyObject2 = VisualTreeHelper.GetParent(visual);
					element = (dependencyObject2 as GridViewRowPresenterBase);
				}
				else
				{
					dependencyObject2 = null;
				}
			}
			if (dependencyObject2 != null)
			{
				ItemsControl itemsOwner = ItemsControl.GetItemsOwner(dependencyObject2);
				if (itemsOwner != null)
				{
					return itemsOwner;
				}
			}
			return null;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x001033F0 File Offset: 0x001023F0
		internal static DependencyObject GetTemplatedParent(DependencyObject d)
		{
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(d, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				return frameworkElement.TemplatedParent;
			}
			if (frameworkContentElement != null)
			{
				return frameworkContentElement.TemplatedParent;
			}
			return null;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00103420 File Offset: 0x00102420
		internal static XmlDataProvider XmlDataProviderForElement(DependencyObject d)
		{
			IGeneratorHost generatorHost = Helper.GeneratorHostForElement(d);
			ItemCollection itemCollection = (generatorHost != null) ? generatorHost.View : null;
			ICollectionView collectionView = (itemCollection != null) ? itemCollection.CollectionView : null;
			XmlDataCollection xmlDataCollection = (collectionView != null) ? (collectionView.SourceCollection as XmlDataCollection) : null;
			if (xmlDataCollection == null)
			{
				return null;
			}
			return xmlDataCollection.ParentXmlDataProvider;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0010346C File Offset: 0x0010246C
		internal static Size MeasureElementWithSingleChild(UIElement element, Size constraint)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(element) > 0) ? (VisualTreeHelper.GetChild(element, 0) as UIElement) : null;
			if (uielement != null)
			{
				uielement.Measure(constraint);
				return uielement.DesiredSize;
			}
			return default(Size);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x001034AC File Offset: 0x001024AC
		internal static Size ArrangeElementWithSingleChild(UIElement element, Size arrangeSize)
		{
			UIElement uielement = (VisualTreeHelper.GetChildrenCount(element) > 0) ? (VisualTreeHelper.GetChild(element, 0) as UIElement) : null;
			if (uielement != null)
			{
				uielement.Arrange(new Rect(arrangeSize));
			}
			return arrangeSize;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x001034E2 File Offset: 0x001024E2
		internal static bool IsDoubleValid(double value)
		{
			return !double.IsInfinity(value) && !double.IsNaN(value);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x001034F8 File Offset: 0x001024F8
		internal static void CheckCanReceiveMarkupExtension(MarkupExtension markupExtension, IServiceProvider serviceProvider, out DependencyObject targetDependencyObject, out DependencyProperty targetDependencyProperty)
		{
			targetDependencyObject = null;
			targetDependencyProperty = null;
			IProvideValueTarget provideValueTarget = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
			if (provideValueTarget == null)
			{
				return;
			}
			object targetObject = provideValueTarget.TargetObject;
			if (targetObject == null)
			{
				return;
			}
			Type type = targetObject.GetType();
			object targetProperty = provideValueTarget.TargetProperty;
			if (targetProperty != null)
			{
				targetDependencyProperty = (targetProperty as DependencyProperty);
				if (targetDependencyProperty != null)
				{
					targetDependencyObject = (targetObject as DependencyObject);
					return;
				}
				MemberInfo memberInfo = targetProperty as MemberInfo;
				if (memberInfo != null)
				{
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					EventHandler<XamlSetMarkupExtensionEventArgs> eventHandler = Helper.LookupSetMarkupExtensionHandler(type);
					if (eventHandler != null && propertyInfo != null)
					{
						IXamlSchemaContextProvider xamlSchemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
						if (xamlSchemaContextProvider != null)
						{
							XamlType xamlType = xamlSchemaContextProvider.SchemaContext.GetXamlType(type);
							if (xamlType != null)
							{
								XamlMember member = xamlType.GetMember(propertyInfo.Name);
								if (member != null)
								{
									XamlSetMarkupExtensionEventArgs xamlSetMarkupExtensionEventArgs = new XamlSetMarkupExtensionEventArgs(member, markupExtension, serviceProvider);
									eventHandler(targetObject, xamlSetMarkupExtensionEventArgs);
									if (xamlSetMarkupExtensionEventArgs.Handled)
									{
										return;
									}
								}
							}
						}
					}
					Type type2;
					if (propertyInfo != null)
					{
						type2 = propertyInfo.PropertyType;
					}
					else
					{
						type2 = ((MethodInfo)memberInfo).GetParameters()[1].ParameterType;
					}
					if (!typeof(MarkupExtension).IsAssignableFrom(type2) || !type2.IsAssignableFrom(markupExtension.GetType()))
					{
						throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingOnClrProp", new object[]
						{
							markupExtension.GetType().Name,
							memberInfo.Name,
							type.Name
						}));
					}
				}
				else if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) || !typeof(Collection<BindingBase>).IsAssignableFrom(targetProperty.GetType()))
				{
					throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingInCollection", new object[]
					{
						markupExtension.GetType().Name,
						targetProperty.GetType().Name
					}));
				}
			}
			else if (!typeof(BindingBase).IsAssignableFrom(markupExtension.GetType()) || !typeof(Collection<BindingBase>).IsAssignableFrom(type))
			{
				throw new System.Windows.Markup.XamlParseException(SR.Get("MarkupExtensionDynamicOrBindingInCollection", new object[]
				{
					markupExtension.GetType().Name,
					type.Name
				}));
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0010373C File Offset: 0x0010273C
		private static EventHandler<XamlSetMarkupExtensionEventArgs> LookupSetMarkupExtensionHandler(Type type)
		{
			if (typeof(Setter).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Setter.ReceiveMarkupExtension);
			}
			if (typeof(DataTrigger).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(DataTrigger.ReceiveMarkupExtension);
			}
			if (typeof(Condition).IsAssignableFrom(type))
			{
				return new EventHandler<XamlSetMarkupExtensionEventArgs>(Condition.ReceiveMarkupExtension);
			}
			return null;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x001037A7 File Offset: 0x001027A7
		internal static string GetEffectiveStringFormat(string stringFormat)
		{
			if (stringFormat.IndexOf('{') < 0)
			{
				stringFormat = "{0:" + stringFormat + "}";
			}
			return stringFormat;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x001037C8 File Offset: 0x001027C8
		internal static object ReadItemValue(DependencyObject owner, object item, int dpIndex)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> itemValues = Helper.GetItemValues(owner, item);
				if (itemValues != null)
				{
					for (int i = 0; i < itemValues.Count; i++)
					{
						if (itemValues[i].Key == dpIndex)
						{
							return itemValues[i].Value;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00103818 File Offset: 0x00102818
		internal static void StoreItemValue(DependencyObject owner, object item, int dpIndex, object value)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> list = Helper.EnsureItemValues(owner, item);
				bool flag = false;
				KeyValuePair<int, object> keyValuePair = new KeyValuePair<int, object>(dpIndex, value);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Key == dpIndex)
					{
						list[i] = keyValuePair;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(keyValuePair);
				}
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00103878 File Offset: 0x00102878
		internal static void ClearItemValue(DependencyObject owner, object item, int dpIndex)
		{
			if (item != null)
			{
				List<KeyValuePair<int, object>> itemValues = Helper.GetItemValues(owner, item);
				if (itemValues != null)
				{
					for (int i = 0; i < itemValues.Count; i++)
					{
						if (itemValues[i].Key == dpIndex)
						{
							itemValues.RemoveAt(i);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x001038BE File Offset: 0x001028BE
		internal static List<KeyValuePair<int, object>> GetItemValues(DependencyObject owner, object item)
		{
			return Helper.GetItemValues(owner, item, Helper.ItemValueStorageField.GetValue(owner));
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x001038D4 File Offset: 0x001028D4
		internal static List<KeyValuePair<int, object>> GetItemValues(DependencyObject owner, object item, WeakDictionary<object, List<KeyValuePair<int, object>>> itemValueStorage)
		{
			List<KeyValuePair<int, object>> result = null;
			if (itemValueStorage != null)
			{
				itemValueStorage.TryGetValue(item, out result);
			}
			return result;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x001038F4 File Offset: 0x001028F4
		internal static List<KeyValuePair<int, object>> EnsureItemValues(DependencyObject owner, object item)
		{
			WeakDictionary<object, List<KeyValuePair<int, object>>> weakDictionary = Helper.EnsureItemValueStorage(owner);
			List<KeyValuePair<int, object>> list = Helper.GetItemValues(owner, item, weakDictionary);
			if (list == null && HashHelper.HasReliableHashCode(item))
			{
				list = new List<KeyValuePair<int, object>>(3);
				weakDictionary[item] = list;
			}
			return list;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0010392C File Offset: 0x0010292C
		internal static WeakDictionary<object, List<KeyValuePair<int, object>>> EnsureItemValueStorage(DependencyObject owner)
		{
			WeakDictionary<object, List<KeyValuePair<int, object>>> weakDictionary = Helper.ItemValueStorageField.GetValue(owner);
			if (weakDictionary == null)
			{
				weakDictionary = new WeakDictionary<object, List<KeyValuePair<int, object>>>();
				Helper.ItemValueStorageField.SetValue(owner, weakDictionary);
			}
			return weakDictionary;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0010395C File Offset: 0x0010295C
		internal static void SetItemValuesOnContainer(DependencyObject owner, DependencyObject container, object item)
		{
			int[] itemValueStorageIndices = Helper.ItemValueStorageIndices;
			List<KeyValuePair<int, object>> list = Helper.GetItemValues(owner, item) ?? new List<KeyValuePair<int, object>>();
			foreach (int num in itemValueStorageIndices)
			{
				DependencyProperty dependencyProperty = DependencyProperty.RegisteredPropertyList.List[num];
				object obj = DependencyProperty.UnsetValue;
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].Key == num)
					{
						obj = list[j].Value;
						break;
					}
				}
				if (dependencyProperty != null)
				{
					if (obj != DependencyProperty.UnsetValue)
					{
						Helper.ModifiedItemValue modifiedItemValue = obj as Helper.ModifiedItemValue;
						if (modifiedItemValue == null)
						{
							container.SetValue(dependencyProperty, obj);
						}
						else if (modifiedItemValue.IsCoercedWithCurrentValue)
						{
							container.SetCurrentValue(dependencyProperty, modifiedItemValue.Value);
						}
					}
					else if (container != container.GetValue(ItemContainerGenerator.ItemForItemContainerProperty))
					{
						EntryIndex entryIndex = container.LookupEntry(num);
						EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dependencyProperty);
						if (entryIndex.Found)
						{
							effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
							if (effectiveValueEntry.IsCoercedWithCurrentValue)
							{
								container.InvalidateProperty(dependencyProperty, false);
								entryIndex = container.LookupEntry(num);
								if (entryIndex.Found)
								{
									effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
								}
							}
						}
						if (entryIndex.Found && (effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local || effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.ParentTemplate) && !effectiveValueEntry.HasModifiers)
						{
							container.ClearValue(dependencyProperty);
						}
					}
				}
				else if (obj != DependencyProperty.UnsetValue)
				{
					EntryIndex entryIndex2 = container.LookupEntry(num);
					container.SetEffectiveValue(entryIndex2, null, num, null, obj, BaseValueSourceInternal.Local);
				}
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00103B04 File Offset: 0x00102B04
		internal static void StoreItemValues(IContainItemStorage owner, DependencyObject container, object item)
		{
			int[] itemValueStorageIndices = Helper.ItemValueStorageIndices;
			DependencyObject owner2 = (DependencyObject)owner;
			foreach (int num in itemValueStorageIndices)
			{
				EntryIndex entryIndex = container.LookupEntry(num);
				if (entryIndex.Found)
				{
					EffectiveValueEntry effectiveValueEntry = container.EffectiveValues[(int)entryIndex.Index];
					if ((effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local || effectiveValueEntry.BaseValueSourceInternal == BaseValueSourceInternal.ParentTemplate) && !effectiveValueEntry.HasModifiers)
					{
						Helper.StoreItemValue(owner2, item, num, effectiveValueEntry.Value);
					}
					else if (effectiveValueEntry.IsCoercedWithCurrentValue)
					{
						Helper.StoreItemValue(owner2, item, num, new Helper.ModifiedItemValue(effectiveValueEntry.ModifiedValue.CoercedValue, FullValueSource.IsCoercedWithCurrentValue));
					}
					else
					{
						Helper.ClearItemValue(owner2, item, num);
					}
				}
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00103BBD File Offset: 0x00102BBD
		internal static void ClearItemValueStorage(DependencyObject owner)
		{
			Helper.ItemValueStorageField.ClearValue(owner);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00103BCA File Offset: 0x00102BCA
		internal static void ClearItemValueStorage(DependencyObject owner, int[] dpIndices)
		{
			Helper.ClearItemValueStorageRecursive(Helper.ItemValueStorageField.GetValue(owner), dpIndices);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00103BE0 File Offset: 0x00102BE0
		private static void ClearItemValueStorageRecursive(WeakDictionary<object, List<KeyValuePair<int, object>>> itemValueStorage, int[] dpIndices)
		{
			if (itemValueStorage != null)
			{
				foreach (List<KeyValuePair<int, object>> list in itemValueStorage.Values)
				{
					for (int i = 0; i < list.Count; i++)
					{
						KeyValuePair<int, object> keyValuePair = list[i];
						if (keyValuePair.Key == Helper.ItemValueStorageField.GlobalIndex)
						{
							Helper.ClearItemValueStorageRecursive((WeakDictionary<object, List<KeyValuePair<int, object>>>)keyValuePair.Value, dpIndices);
						}
						for (int j = 0; j < dpIndices.Length; j++)
						{
							if (keyValuePair.Key == dpIndices[j])
							{
								list.RemoveAt(i--);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00103C98 File Offset: 0x00102C98
		internal static void ApplyCorrectionFactorToPixelHeaderSize(ItemsControl scrollingItemsControl, FrameworkElement virtualizingElement, Panel itemsHost, ref Size headerSize)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return;
			}
			if (itemsHost != null && itemsHost.IsVisible)
			{
				headerSize.Height = Math.Max(GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement).Top, headerSize.Height);
			}
			else
			{
				headerSize.Height = Math.Max(virtualizingElement.DesiredSize.Height, headerSize.Height);
			}
			headerSize.Width = Math.Max(virtualizingElement.DesiredSize.Width, headerSize.Width);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00103D20 File Offset: 0x00102D20
		internal static HierarchicalVirtualizationItemDesiredSizes ApplyCorrectionFactorToItemDesiredSizes(FrameworkElement virtualizingElement, Panel itemsHost)
		{
			HierarchicalVirtualizationItemDesiredSizes value = GroupItem.HierarchicalVirtualizationItemDesiredSizesField.GetValue(virtualizingElement);
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return value;
			}
			if (itemsHost != null && itemsHost.IsVisible)
			{
				Size pixelSize = value.PixelSize;
				Size pixelSizeInViewport = value.PixelSizeInViewport;
				Size pixelSizeBeforeViewport = value.PixelSizeBeforeViewport;
				Size pixelSizeAfterViewport = value.PixelSizeAfterViewport;
				bool flag = false;
				Thickness value2 = new Thickness(0.0);
				Size desiredSize = virtualizingElement.DesiredSize;
				if (DoubleUtil.GreaterThan(pixelSize.Height, 0.0))
				{
					value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					pixelSize.Height += value2.Bottom;
					flag = true;
				}
				pixelSize.Width = Math.Max(desiredSize.Width, pixelSize.Width);
				if (DoubleUtil.AreClose(value.PixelSizeAfterViewport.Height, 0.0) && DoubleUtil.AreClose(value.PixelSizeInViewport.Height, 0.0) && DoubleUtil.GreaterThan(value.PixelSizeBeforeViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeBeforeViewport.Height += value2.Bottom;
					flag = true;
				}
				pixelSizeBeforeViewport.Width = Math.Max(desiredSize.Width, pixelSizeBeforeViewport.Width);
				if (DoubleUtil.AreClose(value.PixelSizeAfterViewport.Height, 0.0) && DoubleUtil.GreaterThan(value.PixelSizeInViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeInViewport.Height += value2.Bottom;
					flag = true;
				}
				pixelSizeInViewport.Width = Math.Max(desiredSize.Width, pixelSizeInViewport.Width);
				if (DoubleUtil.GreaterThan(value.PixelSizeAfterViewport.Height, 0.0))
				{
					if (!flag)
					{
						value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
					}
					pixelSizeAfterViewport.Height += value2.Bottom;
				}
				pixelSizeAfterViewport.Width = Math.Max(desiredSize.Width, pixelSizeAfterViewport.Width);
				value = new HierarchicalVirtualizationItemDesiredSizes(value.LogicalSize, value.LogicalSizeInViewport, value.LogicalSizeBeforeViewport, value.LogicalSizeAfterViewport, pixelSize, pixelSizeInViewport, pixelSizeBeforeViewport, pixelSizeAfterViewport);
			}
			return value;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00103F98 File Offset: 0x00102F98
		internal static void ComputeCorrectionFactor(ItemsControl scrollingItemsControl, FrameworkElement virtualizingElement, Panel itemsHost, FrameworkElement headerElement)
		{
			if (!VirtualizingStackPanel.IsVSP45Compat)
			{
				return;
			}
			Rect rect = new Rect(default(Point), virtualizingElement.DesiredSize);
			bool flag = false;
			if (itemsHost != null)
			{
				Thickness value = default(Thickness);
				if (itemsHost.IsVisible)
				{
					Rect rect2 = itemsHost.TransformToAncestor(virtualizingElement).TransformBounds(new Rect(default(Point), itemsHost.DesiredSize));
					value.Top = rect2.Top;
					value.Bottom = rect.Bottom - rect2.Bottom;
					if (value.Bottom < 0.0)
					{
						value.Bottom = 0.0;
					}
				}
				Thickness value2 = GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GetValue(virtualizingElement);
				if (!DoubleUtil.AreClose(value.Top, value2.Top) || !DoubleUtil.AreClose(value.Bottom, value2.Bottom))
				{
					flag = true;
					GroupItem.DesiredPixelItemsSizeCorrectionFactorField.SetValue(virtualizingElement, value);
				}
			}
			if (flag && scrollingItemsControl != null)
			{
				itemsHost = scrollingItemsControl.ItemsHost;
				if (itemsHost != null)
				{
					VirtualizingStackPanel virtualizingStackPanel = itemsHost as VirtualizingStackPanel;
					if (virtualizingStackPanel != null)
					{
						virtualizingStackPanel.AnchoredInvalidateMeasure();
						return;
					}
					itemsHost.InvalidateMeasure();
				}
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x001040B4 File Offset: 0x001030B4
		internal static void ClearVirtualizingElement(IHierarchicalVirtualizationAndScrollInfo virtualizingElement)
		{
			virtualizingElement.ItemDesiredSizes = default(HierarchicalVirtualizationItemDesiredSizes);
			virtualizingElement.MustDisableVirtualization = false;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x001040D8 File Offset: 0x001030D8
		internal static T FindTemplatedDescendant<T>(FrameworkElement searchStart, FrameworkElement templatedParent) where T : FrameworkElement
		{
			T t = default(T);
			int childrenCount = VisualTreeHelper.GetChildrenCount(searchStart);
			int num = 0;
			while (num < childrenCount && t == null)
			{
				FrameworkElement frameworkElement = VisualTreeHelper.GetChild(searchStart, num) as FrameworkElement;
				if (frameworkElement != null && frameworkElement.TemplatedParent == templatedParent)
				{
					T t2 = frameworkElement as T;
					if (t2 != null)
					{
						t = t2;
					}
					else
					{
						t = Helper.FindTemplatedDescendant<T>(frameworkElement, templatedParent);
					}
				}
				num++;
			}
			return t;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00104148 File Offset: 0x00103148
		internal static T FindVisualAncestor<T>(DependencyObject element, Func<DependencyObject, bool> shouldContinueFunc) where T : DependencyObject
		{
			while (element != null)
			{
				element = VisualTreeHelper.GetParent(element);
				T t = element as T;
				if (t != null)
				{
					return t;
				}
				if (!shouldContinueFunc(element))
				{
					break;
				}
			}
			return default(T);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0010418A File Offset: 0x0010318A
		internal static void InvalidateMeasureOnPath(DependencyObject pathStartElement, DependencyObject pathEndElement, bool duringMeasure)
		{
			Helper.InvalidateMeasureOnPath(pathStartElement, pathEndElement, duringMeasure, false);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00104198 File Offset: 0x00103198
		internal static void InvalidateMeasureOnPath(DependencyObject pathStartElement, DependencyObject pathEndElement, bool duringMeasure, bool includePathEnd)
		{
			DependencyObject dependencyObject = pathStartElement;
			while (dependencyObject != null && (includePathEnd || dependencyObject != pathEndElement))
			{
				UIElement uielement = dependencyObject as UIElement;
				if (uielement != null)
				{
					if (duringMeasure)
					{
						uielement.InvalidateMeasureInternal();
					}
					else
					{
						uielement.InvalidateMeasure();
					}
				}
				if (dependencyObject == pathEndElement)
				{
					break;
				}
				dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x001041DC File Offset: 0x001031DC
		internal static void InvalidateMeasureForSubtree(DependencyObject d)
		{
			UIElement uielement = d as UIElement;
			if (uielement != null)
			{
				if (uielement.MeasureDirty)
				{
					return;
				}
				uielement.InvalidateMeasureInternal();
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(d);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(d, i);
				if (child != null)
				{
					Helper.InvalidateMeasureForSubtree(child);
				}
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00104228 File Offset: 0x00103228
		internal static bool IsAnyAncestorOf(DependencyObject ancestor, DependencyObject element)
		{
			return ancestor != null && element != null && Helper.FindAnyAncestor(element, (DependencyObject d) => d == ancestor) != null;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00104264 File Offset: 0x00103264
		internal static DependencyObject FindAnyAncestor(DependencyObject element, Predicate<DependencyObject> predicate)
		{
			while (element != null)
			{
				element = Helper.GetAnyParent(element);
				if (element != null && predicate(element))
				{
					return element;
				}
			}
			return null;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00104284 File Offset: 0x00103284
		internal static DependencyObject GetAnyParent(DependencyObject element)
		{
			DependencyObject dependencyObject = null;
			if (!(element is ContentElement))
			{
				dependencyObject = VisualTreeHelper.GetParent(element);
			}
			if (dependencyObject == null)
			{
				dependencyObject = LogicalTreeHelper.GetParent(element);
			}
			return dependencyObject;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x001042B0 File Offset: 0x001032B0
		internal static bool IsDefaultValue(DependencyProperty dp, DependencyObject element)
		{
			bool flag;
			return element.GetValueSource(dp, null, out flag) == BaseValueSourceInternal.Default;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x001042CA File Offset: 0x001032CA
		internal static bool IsComposing(DependencyObject d, DependencyProperty dp)
		{
			return dp == TextBox.TextProperty && Helper.IsComposing(d as TextBoxBase);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x001042E4 File Offset: 0x001032E4
		internal static bool IsComposing(TextBoxBase tbb)
		{
			if (tbb == null)
			{
				return false;
			}
			TextEditor textEditor = tbb.TextEditor;
			if (textEditor == null)
			{
				return false;
			}
			TextStore textStore = textEditor.TextStore;
			return textStore != null && textStore.IsEffectivelyComposing;
		}

		// Token: 0x040006D6 RID: 1750
		private static readonly Type NullableType = Type.GetType("System.Nullable`1");

		// Token: 0x040006D7 RID: 1751
		private static readonly UncommonField<WeakDictionary<object, List<KeyValuePair<int, object>>>> ItemValueStorageField = new UncommonField<WeakDictionary<object, List<KeyValuePair<int, object>>>>();

		// Token: 0x040006D8 RID: 1752
		private static readonly int[] ItemValueStorageIndices = new int[]
		{
			Helper.ItemValueStorageField.GlobalIndex,
			TreeViewItem.IsExpandedProperty.GlobalIndex,
			Expander.IsExpandedProperty.GlobalIndex,
			GroupItem.DesiredPixelItemsSizeCorrectionFactorField.GlobalIndex,
			VirtualizingStackPanel.ItemsHostInsetProperty.GlobalIndex
		};

		// Token: 0x020008AE RID: 2222
		private class FindResourceHelper
		{
			// Token: 0x060080C8 RID: 32968 RVA: 0x0032249E File Offset: 0x0032149E
			internal object TryCatchWhen()
			{
				Dispatcher.CurrentDispatcher.WrappedInvoke(new DispatcherOperationCallback(this.DoTryCatchWhen), null, 1, new DispatcherOperationCallback(this.CatchHandler));
				return this._resource;
			}

			// Token: 0x060080C9 RID: 32969 RVA: 0x003224CB File Offset: 0x003214CB
			private object DoTryCatchWhen(object arg)
			{
				throw new ResourceReferenceKeyNotFoundException(SR.Get("MarkupExtensionResourceNotFound", new object[]
				{
					this._name
				}), this._name);
			}

			// Token: 0x060080CA RID: 32970 RVA: 0x003224F1 File Offset: 0x003214F1
			private object CatchHandler(object arg)
			{
				this._resource = DependencyProperty.UnsetValue;
				return null;
			}

			// Token: 0x060080CB RID: 32971 RVA: 0x003224FF File Offset: 0x003214FF
			public FindResourceHelper(object name)
			{
				this._name = name;
				this._resource = null;
			}

			// Token: 0x04003C10 RID: 15376
			private object _name;

			// Token: 0x04003C11 RID: 15377
			private object _resource;
		}

		// Token: 0x020008AF RID: 2223
		private class ModifiedItemValue
		{
			// Token: 0x060080CC RID: 32972 RVA: 0x00322515 File Offset: 0x00321515
			public ModifiedItemValue(object value, FullValueSource valueSource)
			{
				this._value = value;
				this._valueSource = valueSource;
			}

			// Token: 0x17001D7E RID: 7550
			// (get) Token: 0x060080CD RID: 32973 RVA: 0x0032252B File Offset: 0x0032152B
			public object Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x17001D7F RID: 7551
			// (get) Token: 0x060080CE RID: 32974 RVA: 0x00322533 File Offset: 0x00321533
			public bool IsCoercedWithCurrentValue
			{
				get
				{
					return (this._valueSource & FullValueSource.IsCoercedWithCurrentValue) > (FullValueSource)0;
				}
			}

			// Token: 0x04003C12 RID: 15378
			private object _value;

			// Token: 0x04003C13 RID: 15379
			private FullValueSource _valueSource;
		}
	}
}
