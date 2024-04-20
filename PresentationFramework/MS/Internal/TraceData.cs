using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;

namespace MS.Internal
{
	// Token: 0x020000EF RID: 239
	internal static class TraceData
	{
		// Token: 0x06000481 RID: 1153 RVA: 0x00100345 File Offset: 0x000FF345
		public static AvTraceDetails CannotCreateDefaultValueConverter(params object[] args)
		{
			if (TraceData._CannotCreateDefaultValueConverter == null)
			{
				TraceData._CannotCreateDefaultValueConverter = new AvTraceDetails(1, new string[]
				{
					"Cannot create default converter to perform '{2}' conversions between types '{0}' and '{1}'. Consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._CannotCreateDefaultValueConverter, args);
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00100372 File Offset: 0x000FF372
		public static AvTraceDetails NoMentor
		{
			get
			{
				if (TraceData._NoMentor == null)
				{
					TraceData._NoMentor = new AvTraceDetails(2, new string[]
					{
						"Cannot find governing FrameworkElement or FrameworkContentElement for target element."
					});
				}
				return TraceData._NoMentor;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00100399 File Offset: 0x000FF399
		public static AvTraceDetails NoDataContext
		{
			get
			{
				if (TraceData._NoDataContext == null)
				{
					TraceData._NoDataContext = new AvTraceDetails(3, new string[]
					{
						"UpdateCannot find element that provides DataContext."
					});
				}
				return TraceData._NoDataContext;
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x001003C0 File Offset: 0x000FF3C0
		public static AvTraceDetails NoSource(params object[] args)
		{
			if (TraceData._NoSource == null)
			{
				TraceData._NoSource = new AvTraceDetails(4, new string[]
				{
					"Cannot find source for binding with reference '{0}'."
				});
			}
			return new AvTraceFormat(TraceData._NoSource, args);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x001003ED File Offset: 0x000FF3ED
		public static AvTraceDetails BadValueAtTransfer
		{
			get
			{
				if (TraceData._BadValueAtTransfer == null)
				{
					TraceData._BadValueAtTransfer = new AvTraceDetails(5, new string[]
					{
						"Value produced by BindingExpression is not valid for target property."
					});
				}
				return TraceData._BadValueAtTransfer;
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00100414 File Offset: 0x000FF414
		public static AvTraceDetails BadConverterForTransfer(params object[] args)
		{
			if (TraceData._BadConverterForTransfer == null)
			{
				TraceData._BadConverterForTransfer = new AvTraceDetails(6, new string[]
				{
					"'{0}' converter failed to convert value '{1}' (type '{2}'); fallback value will be used, if available."
				});
			}
			return new AvTraceFormat(TraceData._BadConverterForTransfer, args);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00100441 File Offset: 0x000FF441
		public static AvTraceDetails BadConverterForUpdate(params object[] args)
		{
			if (TraceData._BadConverterForUpdate == null)
			{
				TraceData._BadConverterForUpdate = new AvTraceDetails(7, new string[]
				{
					"ConvertBack cannot convert value '{0}' (type '{1}')."
				});
			}
			return new AvTraceFormat(TraceData._BadConverterForUpdate, args);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0010046E File Offset: 0x000FF46E
		public static AvTraceDetails WorkerUpdateFailed
		{
			get
			{
				if (TraceData._WorkerUpdateFailed == null)
				{
					TraceData._WorkerUpdateFailed = new AvTraceDetails(8, new string[]
					{
						"Cannot save value from target back to source."
					});
				}
				return TraceData._WorkerUpdateFailed;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x00100495 File Offset: 0x000FF495
		public static AvTraceDetails RequiresExplicitCulture
		{
			get
			{
				if (TraceData._RequiresExplicitCulture == null)
				{
					TraceData._RequiresExplicitCulture = new AvTraceDetails(9, new string[]
					{
						"Binding for property cannot use the target element's Language for conversion; if a culture is required, ConverterCulture must be explicitly specified on the Binding."
					});
				}
				return TraceData._RequiresExplicitCulture;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x001004BD File Offset: 0x000FF4BD
		public static AvTraceDetails NoValueToTransfer
		{
			get
			{
				if (TraceData._NoValueToTransfer == null)
				{
					TraceData._NoValueToTransfer = new AvTraceDetails(10, new string[]
					{
						"Cannot retrieve value using the binding and no valid fallback value exists; using default instead."
					});
				}
				return TraceData._NoValueToTransfer;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x001004E5 File Offset: 0x000FF4E5
		public static AvTraceDetails FallbackConversionFailed(params object[] args)
		{
			if (TraceData._FallbackConversionFailed == null)
			{
				TraceData._FallbackConversionFailed = new AvTraceDetails(11, new string[]
				{
					"Fallback value '{0}' (type '{1}') cannot be converted for use in '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._FallbackConversionFailed, args);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00100513 File Offset: 0x000FF513
		public static AvTraceDetails TargetNullValueConversionFailed(params object[] args)
		{
			if (TraceData._TargetNullValueConversionFailed == null)
			{
				TraceData._TargetNullValueConversionFailed = new AvTraceDetails(12, new string[]
				{
					"TargetNullValue '{0}' (type '{1}') cannot be converted for use in '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._TargetNullValueConversionFailed, args);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00100541 File Offset: 0x000FF541
		public static AvTraceDetails BindingGroupNameMatchFailed(params object[] args)
		{
			if (TraceData._BindingGroupNameMatchFailed == null)
			{
				TraceData._BindingGroupNameMatchFailed = new AvTraceDetails(13, new string[]
				{
					"No BindingGroup found with name matching '{0}'."
				});
			}
			return new AvTraceFormat(TraceData._BindingGroupNameMatchFailed, args);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x0010056F File Offset: 0x000FF56F
		public static AvTraceDetails BindingGroupWrongProperty(params object[] args)
		{
			if (TraceData._BindingGroupWrongProperty == null)
			{
				TraceData._BindingGroupWrongProperty = new AvTraceDetails(14, new string[]
				{
					"BindingGroup used as a value of property '{0}' on object of type '{1}'.  This may disable its normal behavior."
				});
			}
			return new AvTraceFormat(TraceData._BindingGroupWrongProperty, args);
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0010059D File Offset: 0x000FF59D
		public static AvTraceDetails BindingGroupMultipleInheritance
		{
			get
			{
				if (TraceData._BindingGroupMultipleInheritance == null)
				{
					TraceData._BindingGroupMultipleInheritance = new AvTraceDetails(15, new string[]
					{
						"BindingGroup used as a value of multiple properties.  This disables its normal behavior."
					});
				}
				return TraceData._BindingGroupMultipleInheritance;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x001005C5 File Offset: 0x000FF5C5
		public static AvTraceDetails SharesProposedValuesRequriesImplicitBindingGroup(params object[] args)
		{
			if (TraceData._SharesProposedValuesRequriesImplicitBindingGroup == null)
			{
				TraceData._SharesProposedValuesRequriesImplicitBindingGroup = new AvTraceDetails(16, new string[]
				{
					"Binding expression '{0}' with BindingGroupName '{1}' has joined BindingGroup '{2}' with SharesProposedValues='true'.  The SharesProposedValues feature only works for binding expressions that implicitly join a binding group."
				});
			}
			return new AvTraceFormat(TraceData._SharesProposedValuesRequriesImplicitBindingGroup, args);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x001005F3 File Offset: 0x000FF5F3
		public static AvTraceDetails CannotGetClrRawValue(params object[] args)
		{
			if (TraceData._CannotGetClrRawValue == null)
			{
				TraceData._CannotGetClrRawValue = new AvTraceDetails(17, new string[]
				{
					"Cannot get '{0}' value (type '{1}') from '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._CannotGetClrRawValue, args);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00100621 File Offset: 0x000FF621
		public static AvTraceDetails CannotSetClrRawValue(params object[] args)
		{
			if (TraceData._CannotSetClrRawValue == null)
			{
				TraceData._CannotSetClrRawValue = new AvTraceDetails(18, new string[]
				{
					"'{3}' is not a valid value for '{0}' of '{2}'."
				});
			}
			return new AvTraceFormat(TraceData._CannotSetClrRawValue, args);
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x0010064F File Offset: 0x000FF64F
		public static AvTraceDetails MissingDataItem
		{
			get
			{
				if (TraceData._MissingDataItem == null)
				{
					TraceData._MissingDataItem = new AvTraceDetails(19, new string[]
					{
						"BindingExpression has no source data item. This could happen when currency is moved to a null data item or moved off the list."
					});
				}
				return TraceData._MissingDataItem;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00100677 File Offset: 0x000FF677
		public static AvTraceDetails MissingInfo
		{
			get
			{
				if (TraceData._MissingInfo == null)
				{
					TraceData._MissingInfo = new AvTraceDetails(20, new string[]
					{
						"BindingExpression cannot retrieve value due to missing information."
					});
				}
				return TraceData._MissingInfo;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0010069F File Offset: 0x000FF69F
		public static AvTraceDetails NullDataItem
		{
			get
			{
				if (TraceData._NullDataItem == null)
				{
					TraceData._NullDataItem = new AvTraceDetails(21, new string[]
					{
						"BindingExpression cannot retrieve value from null data item. This could happen when binding is detached or when binding to a Nullable type that has no value."
					});
				}
				return TraceData._NullDataItem;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x001006C7 File Offset: 0x000FF6C7
		public static AvTraceDetails DefaultValueConverterFailed(params object[] args)
		{
			if (TraceData._DefaultValueConverterFailed == null)
			{
				TraceData._DefaultValueConverterFailed = new AvTraceDetails(22, new string[]
				{
					"Cannot convert '{0}' from type '{1}' to type '{2}' with default conversions; consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._DefaultValueConverterFailed, args);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x001006F5 File Offset: 0x000FF6F5
		public static AvTraceDetails DefaultValueConverterFailedForCulture(params object[] args)
		{
			if (TraceData._DefaultValueConverterFailedForCulture == null)
			{
				TraceData._DefaultValueConverterFailedForCulture = new AvTraceDetails(23, new string[]
				{
					"Cannot convert '{0}' from type '{1}' to type '{2}' for '{3}' culture with default conversions; consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._DefaultValueConverterFailedForCulture, args);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00100723 File Offset: 0x000FF723
		public static AvTraceDetails StyleAndStyleSelectorDefined(params object[] args)
		{
			if (TraceData._StyleAndStyleSelectorDefined == null)
			{
				TraceData._StyleAndStyleSelectorDefined = new AvTraceDetails(24, new string[]
				{
					"Both '{0}Style' and '{0}StyleSelector' are set;  '{0}StyleSelector' will be ignored."
				});
			}
			return new AvTraceFormat(TraceData._StyleAndStyleSelectorDefined, args);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00100751 File Offset: 0x000FF751
		public static AvTraceDetails TemplateAndTemplateSelectorDefined(params object[] args)
		{
			if (TraceData._TemplateAndTemplateSelectorDefined == null)
			{
				TraceData._TemplateAndTemplateSelectorDefined = new AvTraceDetails(25, new string[]
				{
					"Both '{0}Template' and '{0}TemplateSelector' are set;  '{0}TemplateSelector' will be ignored."
				});
			}
			return new AvTraceFormat(TraceData._TemplateAndTemplateSelectorDefined, args);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0010077F File Offset: 0x000FF77F
		public static AvTraceDetails ItemTemplateForDirectItem
		{
			get
			{
				if (TraceData._ItemTemplateForDirectItem == null)
				{
					TraceData._ItemTemplateForDirectItem = new AvTraceDetails(26, new string[]
					{
						"ItemTemplate and ItemTemplateSelector are ignored for items already of the ItemsControl's container type"
					});
				}
				return TraceData._ItemTemplateForDirectItem;
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x001007A7 File Offset: 0x000FF7A7
		public static AvTraceDetails BadMultiConverterForUpdate(params object[] args)
		{
			if (TraceData._BadMultiConverterForUpdate == null)
			{
				TraceData._BadMultiConverterForUpdate = new AvTraceDetails(27, new string[]
				{
					"'{0}' MultiValueConverter failed to convert back value '{1}' (type '{2}'). Check the converter's ConvertBack method."
				});
			}
			return new AvTraceFormat(TraceData._BadMultiConverterForUpdate, args);
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x001007D5 File Offset: 0x000FF7D5
		public static AvTraceDetails MultiValueConverterMissingForTransfer
		{
			get
			{
				if (TraceData._MultiValueConverterMissingForTransfer == null)
				{
					TraceData._MultiValueConverterMissingForTransfer = new AvTraceDetails(28, new string[]
					{
						"MultiBinding failed because it has no valid Converter."
					});
				}
				return TraceData._MultiValueConverterMissingForTransfer;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x001007FD File Offset: 0x000FF7FD
		public static AvTraceDetails MultiValueConverterMissingForUpdate
		{
			get
			{
				if (TraceData._MultiValueConverterMissingForUpdate == null)
				{
					TraceData._MultiValueConverterMissingForUpdate = new AvTraceDetails(29, new string[]
					{
						"MultiBinding cannot update value on source item because there is no valid Converter."
					});
				}
				return TraceData._MultiValueConverterMissingForUpdate;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00100825 File Offset: 0x000FF825
		public static AvTraceDetails MultiValueConverterMismatch
		{
			get
			{
				if (TraceData._MultiValueConverterMismatch == null)
				{
					TraceData._MultiValueConverterMismatch = new AvTraceDetails(30, new string[]
					{
						"MultiValueConverter did not return the same number of values as the count of inner bindings."
					});
				}
				return TraceData._MultiValueConverterMismatch;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0010084D File Offset: 0x000FF84D
		public static AvTraceDetails MultiBindingHasNoConverter
		{
			get
			{
				if (TraceData._MultiBindingHasNoConverter == null)
				{
					TraceData._MultiBindingHasNoConverter = new AvTraceDetails(31, new string[]
					{
						"Cannot set MultiBinding because MultiValueConverter must be specified."
					});
				}
				return TraceData._MultiBindingHasNoConverter;
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00100875 File Offset: 0x000FF875
		public static AvTraceDetails UnsetValueInMultiBindingExpressionUpdate(params object[] args)
		{
			if (TraceData._UnsetValueInMultiBindingExpressionUpdate == null)
			{
				TraceData._UnsetValueInMultiBindingExpressionUpdate = new AvTraceDetails(32, new string[]
				{
					"'{0}' MultiValueConverter returned UnsetValue after converting '{1}' for source binding '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._UnsetValueInMultiBindingExpressionUpdate, args);
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x001008A3 File Offset: 0x000FF8A3
		public static AvTraceDetails ObjectDataProviderHasNoSource
		{
			get
			{
				if (TraceData._ObjectDataProviderHasNoSource == null)
				{
					TraceData._ObjectDataProviderHasNoSource = new AvTraceDetails(33, new string[]
					{
						"ObjectDataProvider needs either an ObjectType or ObjectInstance."
					});
				}
				return TraceData._ObjectDataProviderHasNoSource;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x001008CB File Offset: 0x000FF8CB
		public static AvTraceDetails ObjDPCreateFailed
		{
			get
			{
				if (TraceData._ObjDPCreateFailed == null)
				{
					TraceData._ObjDPCreateFailed = new AvTraceDetails(34, new string[]
					{
						"ObjectDataProvider cannot create object"
					});
				}
				return TraceData._ObjDPCreateFailed;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x001008F3 File Offset: 0x000FF8F3
		public static AvTraceDetails ObjDPInvokeFailed
		{
			get
			{
				if (TraceData._ObjDPInvokeFailed == null)
				{
					TraceData._ObjDPInvokeFailed = new AvTraceDetails(35, new string[]
					{
						"ObjectDataProvider: Failure trying to invoke method on type"
					});
				}
				return TraceData._ObjDPInvokeFailed;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0010091B File Offset: 0x000FF91B
		public static AvTraceDetails RefPreviousNotInContext
		{
			get
			{
				if (TraceData._RefPreviousNotInContext == null)
				{
					TraceData._RefPreviousNotInContext = new AvTraceDetails(36, new string[]
					{
						"Cannot find previous element for use as RelativeSource because there is no parent in generated context."
					});
				}
				return TraceData._RefPreviousNotInContext;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x00100943 File Offset: 0x000FF943
		public static AvTraceDetails RefNoWrapperInChildren
		{
			get
			{
				if (TraceData._RefNoWrapperInChildren == null)
				{
					TraceData._RefNoWrapperInChildren = new AvTraceDetails(37, new string[]
					{
						"Cannot find previous element for use as RelativeSource because children cannot be found for parent element."
					});
				}
				return TraceData._RefNoWrapperInChildren;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0010096B File Offset: 0x000FF96B
		public static AvTraceDetails RefAncestorTypeNotSpecified
		{
			get
			{
				if (TraceData._RefAncestorTypeNotSpecified == null)
				{
					TraceData._RefAncestorTypeNotSpecified = new AvTraceDetails(38, new string[]
					{
						"Reference error: cannot find ancestor element; no AncestorType was specified on RelativeSource."
					});
				}
				return TraceData._RefAncestorTypeNotSpecified;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00100993 File Offset: 0x000FF993
		public static AvTraceDetails RefAncestorLevelInvalid
		{
			get
			{
				if (TraceData._RefAncestorLevelInvalid == null)
				{
					TraceData._RefAncestorLevelInvalid = new AvTraceDetails(39, new string[]
					{
						"Reference error: cannot find ancestor element; AncestorLevel on RelativeSource must be greater than 0."
					});
				}
				return TraceData._RefAncestorLevelInvalid;
			}
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x001009BB File Offset: 0x000FF9BB
		public static AvTraceDetails ClrReplaceItem(params object[] args)
		{
			if (TraceData._ClrReplaceItem == null)
			{
				TraceData._ClrReplaceItem = new AvTraceDetails(40, new string[]
				{
					"BindingExpression path error: '{0}' property not found on '{2}' '{1}'."
				});
			}
			return new AvTraceFormat(TraceData._ClrReplaceItem, args);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x001009E9 File Offset: 0x000FF9E9
		public static AvTraceDetails NullItem(params object[] args)
		{
			if (TraceData._NullItem == null)
			{
				TraceData._NullItem = new AvTraceDetails(41, new string[]
				{
					"BindingExpression path error: '{0}' property not found for '{1}' because data item is null. This could happen because the data provider has not produced any data yet."
				});
			}
			return new AvTraceFormat(TraceData._NullItem, args);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00100A17 File Offset: 0x000FFA17
		public static AvTraceDetails PlaceholderItem(params object[] args)
		{
			if (TraceData._PlaceholderItem == null)
			{
				TraceData._PlaceholderItem = new AvTraceDetails(42, new string[]
				{
					"BindingExpression path error: '{0}' property not found for '{1}' because data item is the NewItemPlaceholder."
				});
			}
			return new AvTraceFormat(TraceData._PlaceholderItem, args);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00100A45 File Offset: 0x000FFA45
		public static AvTraceDetails DataErrorInfoFailed(params object[] args)
		{
			if (TraceData._DataErrorInfoFailed == null)
			{
				TraceData._DataErrorInfoFailed = new AvTraceDetails(43, new string[]
				{
					"Cannot obtain IDataErrorInfo.Error[{0}] from source of type {1} - {2} '{3}'"
				});
			}
			return new AvTraceFormat(TraceData._DataErrorInfoFailed, args);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00100A73 File Offset: 0x000FFA73
		public static AvTraceDetails DisallowTwoWay(params object[] args)
		{
			if (TraceData._DisallowTwoWay == null)
			{
				TraceData._DisallowTwoWay = new AvTraceDetails(44, new string[]
				{
					"Binding mode has been changed to OneWay because source property '{0}.{1}' has a non-public setter."
				});
			}
			return new AvTraceFormat(TraceData._DisallowTwoWay, args);
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00100AA1 File Offset: 0x000FFAA1
		public static AvTraceDetails XmlBindingToNonXml
		{
			get
			{
				if (TraceData._XmlBindingToNonXml == null)
				{
					TraceData._XmlBindingToNonXml = new AvTraceDetails(45, new string[]
					{
						"BindingExpression with XPath cannot bind to non-XML object."
					});
				}
				return TraceData._XmlBindingToNonXml;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00100AC9 File Offset: 0x000FFAC9
		public static AvTraceDetails XmlBindingToNonXmlCollection
		{
			get
			{
				if (TraceData._XmlBindingToNonXmlCollection == null)
				{
					TraceData._XmlBindingToNonXmlCollection = new AvTraceDetails(46, new string[]
					{
						"BindingExpression with XPath cannot bind to a collection with non-XML objects."
					});
				}
				return TraceData._XmlBindingToNonXmlCollection;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00100AF1 File Offset: 0x000FFAF1
		public static AvTraceDetails CannotGetXmlNodeCollection
		{
			get
			{
				if (TraceData._CannotGetXmlNodeCollection == null)
				{
					TraceData._CannotGetXmlNodeCollection = new AvTraceDetails(47, new string[]
					{
						"XML binding failed. Cannot obtain result node collection because of bad source node or bad Path."
					});
				}
				return TraceData._CannotGetXmlNodeCollection;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00100B19 File Offset: 0x000FFB19
		public static AvTraceDetails BadXPath(params object[] args)
		{
			if (TraceData._BadXPath == null)
			{
				TraceData._BadXPath = new AvTraceDetails(48, new string[]
				{
					"XPath '{0}' returned no results on XmlNode '{1}'"
				});
			}
			return new AvTraceFormat(TraceData._BadXPath, args);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00100B47 File Offset: 0x000FFB47
		public static AvTraceDetails XmlDPInlineDocError
		{
			get
			{
				if (TraceData._XmlDPInlineDocError == null)
				{
					TraceData._XmlDPInlineDocError = new AvTraceDetails(49, new string[]
					{
						"XmlDataProvider cannot load inline document because of load or parse error in XML."
					});
				}
				return TraceData._XmlDPInlineDocError;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00100B6F File Offset: 0x000FFB6F
		public static AvTraceDetails XmlNamespaceNotSet
		{
			get
			{
				if (TraceData._XmlNamespaceNotSet == null)
				{
					TraceData._XmlNamespaceNotSet = new AvTraceDetails(50, new string[]
					{
						"XmlDataProvider has inline XML that does not explicitly set its XmlNamespace (xmlns='')."
					});
				}
				return TraceData._XmlNamespaceNotSet;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00100B97 File Offset: 0x000FFB97
		public static AvTraceDetails XmlDPAsyncDocError
		{
			get
			{
				if (TraceData._XmlDPAsyncDocError == null)
				{
					TraceData._XmlDPAsyncDocError = new AvTraceDetails(51, new string[]
					{
						"XmlDataProvider cannot load asynchronous document from Source because of load or parse error in XML stream."
					});
				}
				return TraceData._XmlDPAsyncDocError;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00100BBF File Offset: 0x000FFBBF
		public static AvTraceDetails XmlDPSelectNodesFailed
		{
			get
			{
				if (TraceData._XmlDPSelectNodesFailed == null)
				{
					TraceData._XmlDPSelectNodesFailed = new AvTraceDetails(52, new string[]
					{
						"Cannot select nodes because XPath for Binding is not valid"
					});
				}
				return TraceData._XmlDPSelectNodesFailed;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00100BE7 File Offset: 0x000FFBE7
		public static AvTraceDetails CollectionViewIsUnsupported
		{
			get
			{
				if (TraceData._CollectionViewIsUnsupported == null)
				{
					TraceData._CollectionViewIsUnsupported = new AvTraceDetails(53, new string[]
					{
						"Using CollectionView directly is not fully supported. The basic features work, although with some inefficiencies, but advanced features may encounter known bugs. Consider using a derived class to avoid these problems."
					});
				}
				return TraceData._CollectionViewIsUnsupported;
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00100C0F File Offset: 0x000FFC0F
		public static AvTraceDetails CollectionChangedWithoutNotification(params object[] args)
		{
			if (TraceData._CollectionChangedWithoutNotification == null)
			{
				TraceData._CollectionChangedWithoutNotification = new AvTraceDetails(54, new string[]
				{
					"Collection of type '{0}' has been changed without raising a CollectionChanged event. Support for this is incomplete and inconsistent, and will be removed completely in a future version of WPF. Consider either (a) implementing INotifyCollectionChanged, or (b) avoiding changes to this type of collection."
				});
			}
			return new AvTraceFormat(TraceData._CollectionChangedWithoutNotification, args);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00100C3D File Offset: 0x000FFC3D
		public static AvTraceDetails CannotSort(params object[] args)
		{
			if (TraceData._CannotSort == null)
			{
				TraceData._CannotSort = new AvTraceDetails(55, new string[]
				{
					"Cannot sort by '{0}'"
				});
			}
			return new AvTraceFormat(TraceData._CannotSort, args);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00100C6B File Offset: 0x000FFC6B
		public static AvTraceDetails CreatedExpression(params object[] args)
		{
			if (TraceData._CreatedExpression == null)
			{
				TraceData._CreatedExpression = new AvTraceDetails(56, new string[]
				{
					"Created {0} for {1}"
				});
			}
			return new AvTraceFormat(TraceData._CreatedExpression, args);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00100C99 File Offset: 0x000FFC99
		public static AvTraceDetails CreatedExpressionInParent(params object[] args)
		{
			if (TraceData._CreatedExpressionInParent == null)
			{
				TraceData._CreatedExpressionInParent = new AvTraceDetails(57, new string[]
				{
					"Created {0} for {1} within {2}"
				});
			}
			return new AvTraceFormat(TraceData._CreatedExpressionInParent, args);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00100CC7 File Offset: 0x000FFCC7
		public static AvTraceDetails BindingPath(params object[] args)
		{
			if (TraceData._BindingPath == null)
			{
				TraceData._BindingPath = new AvTraceDetails(58, new string[]
				{
					" Path: {0}"
				});
			}
			return new AvTraceFormat(TraceData._BindingPath, args);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00100CF5 File Offset: 0x000FFCF5
		public static AvTraceDetails BindingXPathAndPath(params object[] args)
		{
			if (TraceData._BindingXPathAndPath == null)
			{
				TraceData._BindingXPathAndPath = new AvTraceDetails(59, new string[]
				{
					" XPath: {0} Path: {1}"
				});
			}
			return new AvTraceFormat(TraceData._BindingXPathAndPath, args);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00100D23 File Offset: 0x000FFD23
		public static AvTraceDetails ResolveDefaultMode(params object[] args)
		{
			if (TraceData._ResolveDefaultMode == null)
			{
				TraceData._ResolveDefaultMode = new AvTraceDetails(60, new string[]
				{
					"{0}: Default mode resolved to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ResolveDefaultMode, args);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00100D51 File Offset: 0x000FFD51
		public static AvTraceDetails ResolveDefaultUpdate(params object[] args)
		{
			if (TraceData._ResolveDefaultUpdate == null)
			{
				TraceData._ResolveDefaultUpdate = new AvTraceDetails(61, new string[]
				{
					"{0}: Default update trigger resolved to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ResolveDefaultUpdate, args);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00100D7F File Offset: 0x000FFD7F
		public static AvTraceDetails AttachExpression(params object[] args)
		{
			if (TraceData._AttachExpression == null)
			{
				TraceData._AttachExpression = new AvTraceDetails(62, new string[]
				{
					"{0}: Attach to {1}.{2} (hash={3})"
				});
			}
			return new AvTraceFormat(TraceData._AttachExpression, args);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00100DAD File Offset: 0x000FFDAD
		public static AvTraceDetails DetachExpression(params object[] args)
		{
			if (TraceData._DetachExpression == null)
			{
				TraceData._DetachExpression = new AvTraceDetails(63, new string[]
				{
					"{0}: Detach"
				});
			}
			return new AvTraceFormat(TraceData._DetachExpression, args);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00100DDB File Offset: 0x000FFDDB
		public static AvTraceDetails UseMentor(params object[] args)
		{
			if (TraceData._UseMentor == null)
			{
				TraceData._UseMentor = new AvTraceDetails(64, new string[]
				{
					"{0}: Use Framework mentor {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseMentor, args);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00100E09 File Offset: 0x000FFE09
		public static AvTraceDetails DeferAttachToContext(params object[] args)
		{
			if (TraceData._DeferAttachToContext == null)
			{
				TraceData._DeferAttachToContext = new AvTraceDetails(65, new string[]
				{
					"{0}: Resolve source deferred"
				});
			}
			return new AvTraceFormat(TraceData._DeferAttachToContext, args);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00100E37 File Offset: 0x000FFE37
		public static AvTraceDetails SourceRequiresTreeContext(params object[] args)
		{
			if (TraceData._SourceRequiresTreeContext == null)
			{
				TraceData._SourceRequiresTreeContext = new AvTraceDetails(66, new string[]
				{
					"{0}: {1} requires tree context"
				});
			}
			return new AvTraceFormat(TraceData._SourceRequiresTreeContext, args);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00100E65 File Offset: 0x000FFE65
		public static AvTraceDetails AttachToContext(params object[] args)
		{
			if (TraceData._AttachToContext == null)
			{
				TraceData._AttachToContext = new AvTraceDetails(67, new string[]
				{
					"{0}: Resolving source {1}"
				});
			}
			return new AvTraceFormat(TraceData._AttachToContext, args);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00100E93 File Offset: 0x000FFE93
		public static AvTraceDetails PathRequiresTreeContext(params object[] args)
		{
			if (TraceData._PathRequiresTreeContext == null)
			{
				TraceData._PathRequiresTreeContext = new AvTraceDetails(68, new string[]
				{
					"{0}: Path '{1}' requires namespace information"
				});
			}
			return new AvTraceFormat(TraceData._PathRequiresTreeContext, args);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00100EC1 File Offset: 0x000FFEC1
		public static AvTraceDetails NoMentorExtended(params object[] args)
		{
			if (TraceData._NoMentorExtended == null)
			{
				TraceData._NoMentorExtended = new AvTraceDetails(69, new string[]
				{
					"{0}: Framework mentor not found"
				});
			}
			return new AvTraceFormat(TraceData._NoMentorExtended, args);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00100EEF File Offset: 0x000FFEEF
		public static AvTraceDetails ContextElement(params object[] args)
		{
			if (TraceData._ContextElement == null)
			{
				TraceData._ContextElement = new AvTraceDetails(70, new string[]
				{
					"{0}: Found data context element: {1} ({2})"
				});
			}
			return new AvTraceFormat(TraceData._ContextElement, args);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00100F1D File Offset: 0x000FFF1D
		public static AvTraceDetails NullDataContext(params object[] args)
		{
			if (TraceData._NullDataContext == null)
			{
				TraceData._NullDataContext = new AvTraceDetails(71, new string[]
				{
					"{0}: DataContext is null"
				});
			}
			return new AvTraceFormat(TraceData._NullDataContext, args);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00100F4B File Offset: 0x000FFF4B
		public static AvTraceDetails RelativeSource(params object[] args)
		{
			if (TraceData._RelativeSource == null)
			{
				TraceData._RelativeSource = new AvTraceDetails(72, new string[]
				{
					" RelativeSource.{0} found {1}"
				});
			}
			return new AvTraceFormat(TraceData._RelativeSource, args);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00100F79 File Offset: 0x000FFF79
		public static AvTraceDetails AncestorLookup(params object[] args)
		{
			if (TraceData._AncestorLookup == null)
			{
				TraceData._AncestorLookup = new AvTraceDetails(73, new string[]
				{
					"  Lookup ancestor of type {0}: queried {1}"
				});
			}
			return new AvTraceFormat(TraceData._AncestorLookup, args);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00100FA7 File Offset: 0x000FFFA7
		public static AvTraceDetails ElementNameQuery(params object[] args)
		{
			if (TraceData._ElementNameQuery == null)
			{
				TraceData._ElementNameQuery = new AvTraceDetails(74, new string[]
				{
					"  Lookup name {0}: queried {1}"
				});
			}
			return new AvTraceFormat(TraceData._ElementNameQuery, args);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00100FD5 File Offset: 0x000FFFD5
		public static AvTraceDetails ElementNameQueryTemplate(params object[] args)
		{
			if (TraceData._ElementNameQueryTemplate == null)
			{
				TraceData._ElementNameQueryTemplate = new AvTraceDetails(75, new string[]
				{
					"  Lookup name {0}: queried template of {1}"
				});
			}
			return new AvTraceFormat(TraceData._ElementNameQueryTemplate, args);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00101003 File Offset: 0x00100003
		public static AvTraceDetails UseCVS(params object[] args)
		{
			if (TraceData._UseCVS == null)
			{
				TraceData._UseCVS = new AvTraceDetails(76, new string[]
				{
					"{0}: Use View from {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseCVS, args);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00101031 File Offset: 0x00100031
		public static AvTraceDetails UseDataProvider(params object[] args)
		{
			if (TraceData._UseDataProvider == null)
			{
				TraceData._UseDataProvider = new AvTraceDetails(77, new string[]
				{
					"{0}: Use Data from {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseDataProvider, args);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0010105F File Offset: 0x0010005F
		public static AvTraceDetails ActivateItem(params object[] args)
		{
			if (TraceData._ActivateItem == null)
			{
				TraceData._ActivateItem = new AvTraceDetails(78, new string[]
				{
					"{0}: Activate with root item {1}"
				});
			}
			return new AvTraceFormat(TraceData._ActivateItem, args);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0010108D File Offset: 0x0010008D
		public static AvTraceDetails Deactivate(params object[] args)
		{
			if (TraceData._Deactivate == null)
			{
				TraceData._Deactivate = new AvTraceDetails(79, new string[]
				{
					"{0}: Deactivate"
				});
			}
			return new AvTraceFormat(TraceData._Deactivate, args);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x001010BB File Offset: 0x001000BB
		public static AvTraceDetails GetRawValue(params object[] args)
		{
			if (TraceData._GetRawValue == null)
			{
				TraceData._GetRawValue = new AvTraceDetails(80, new string[]
				{
					"{0}: TransferValue - got raw value {1}"
				});
			}
			return new AvTraceFormat(TraceData._GetRawValue, args);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x001010E9 File Offset: 0x001000E9
		public static AvTraceDetails ConvertDBNull(params object[] args)
		{
			if (TraceData._ConvertDBNull == null)
			{
				TraceData._ConvertDBNull = new AvTraceDetails(81, new string[]
				{
					"{0}: TransferValue - converted DBNull to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ConvertDBNull, args);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00101117 File Offset: 0x00100117
		public static AvTraceDetails UserConverter(params object[] args)
		{
			if (TraceData._UserConverter == null)
			{
				TraceData._UserConverter = new AvTraceDetails(82, new string[]
				{
					"{0}: TransferValue - user's converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._UserConverter, args);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00101145 File Offset: 0x00100145
		public static AvTraceDetails NullConverter(params object[] args)
		{
			if (TraceData._NullConverter == null)
			{
				TraceData._NullConverter = new AvTraceDetails(83, new string[]
				{
					"{0}: TransferValue - null-value conversion produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._NullConverter, args);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00101173 File Offset: 0x00100173
		public static AvTraceDetails DefaultConverter(params object[] args)
		{
			if (TraceData._DefaultConverter == null)
			{
				TraceData._DefaultConverter = new AvTraceDetails(84, new string[]
				{
					"{0}: TransferValue - implicit converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._DefaultConverter, args);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x001011A1 File Offset: 0x001001A1
		public static AvTraceDetails FormattedValue(params object[] args)
		{
			if (TraceData._FormattedValue == null)
			{
				TraceData._FormattedValue = new AvTraceDetails(85, new string[]
				{
					"{0}: TransferValue - string formatting produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._FormattedValue, args);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x001011CF File Offset: 0x001001CF
		public static AvTraceDetails FormattingFailed(params object[] args)
		{
			if (TraceData._FormattingFailed == null)
			{
				TraceData._FormattingFailed = new AvTraceDetails(86, new string[]
				{
					"{0}: TransferValue - string formatting failed, using format '{1}'"
				});
			}
			return new AvTraceFormat(TraceData._FormattingFailed, args);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x001011FD File Offset: 0x001001FD
		public static AvTraceDetails BadValueAtTransferExtended(params object[] args)
		{
			if (TraceData._BadValueAtTransferExtended == null)
			{
				TraceData._BadValueAtTransferExtended = new AvTraceDetails(87, new string[]
				{
					"{0}: TransferValue - value {1} is not valid for target"
				});
			}
			return new AvTraceFormat(TraceData._BadValueAtTransferExtended, args);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0010122B File Offset: 0x0010022B
		public static AvTraceDetails UseFallback(params object[] args)
		{
			if (TraceData._UseFallback == null)
			{
				TraceData._UseFallback = new AvTraceDetails(88, new string[]
				{
					"{0}: TransferValue - using fallback/default value {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseFallback, args);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00101259 File Offset: 0x00100259
		public static AvTraceDetails TransferValue(params object[] args)
		{
			if (TraceData._TransferValue == null)
			{
				TraceData._TransferValue = new AvTraceDetails(89, new string[]
				{
					"{0}: TransferValue - using final value {1}"
				});
			}
			return new AvTraceFormat(TraceData._TransferValue, args);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00101287 File Offset: 0x00100287
		public static AvTraceDetails UpdateRawValue(params object[] args)
		{
			if (TraceData._UpdateRawValue == null)
			{
				TraceData._UpdateRawValue = new AvTraceDetails(90, new string[]
				{
					"{0}: Update - got raw value {1}"
				});
			}
			return new AvTraceFormat(TraceData._UpdateRawValue, args);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x001012B5 File Offset: 0x001002B5
		public static AvTraceDetails ValidationRuleFailed(params object[] args)
		{
			if (TraceData._ValidationRuleFailed == null)
			{
				TraceData._ValidationRuleFailed = new AvTraceDetails(91, new string[]
				{
					"{0}: Update - {1} failed"
				});
			}
			return new AvTraceFormat(TraceData._ValidationRuleFailed, args);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x001012E3 File Offset: 0x001002E3
		public static AvTraceDetails UserConvertBack(params object[] args)
		{
			if (TraceData._UserConvertBack == null)
			{
				TraceData._UserConvertBack = new AvTraceDetails(92, new string[]
				{
					"{0}: Update - user's converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._UserConvertBack, args);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00101311 File Offset: 0x00100311
		public static AvTraceDetails DefaultConvertBack(params object[] args)
		{
			if (TraceData._DefaultConvertBack == null)
			{
				TraceData._DefaultConvertBack = new AvTraceDetails(93, new string[]
				{
					"{0}: Update - implicit converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._DefaultConvertBack, args);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0010133F File Offset: 0x0010033F
		public static AvTraceDetails Update(params object[] args)
		{
			if (TraceData._Update == null)
			{
				TraceData._Update = new AvTraceDetails(94, new string[]
				{
					"{0}: Update - using final value {1}"
				});
			}
			return new AvTraceFormat(TraceData._Update, args);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0010136D File Offset: 0x0010036D
		public static AvTraceDetails GotEvent(params object[] args)
		{
			if (TraceData._GotEvent == null)
			{
				TraceData._GotEvent = new AvTraceDetails(95, new string[]
				{
					"{0}: Got {1} event from {2}"
				});
			}
			return new AvTraceFormat(TraceData._GotEvent, args);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0010139B File Offset: 0x0010039B
		public static AvTraceDetails GotPropertyChanged(params object[] args)
		{
			if (TraceData._GotPropertyChanged == null)
			{
				TraceData._GotPropertyChanged = new AvTraceDetails(96, new string[]
				{
					"{0}: Got PropertyChanged event from {1} for {2}"
				});
			}
			return new AvTraceFormat(TraceData._GotPropertyChanged, args);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x001013C9 File Offset: 0x001003C9
		public static AvTraceDetails PriorityTransfer(params object[] args)
		{
			if (TraceData._PriorityTransfer == null)
			{
				TraceData._PriorityTransfer = new AvTraceDetails(97, new string[]
				{
					"{0}: TransferValue '{1}' from child {2} - {3}"
				});
			}
			return new AvTraceFormat(TraceData._PriorityTransfer, args);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x001013F7 File Offset: 0x001003F7
		public static AvTraceDetails ChildNotAttached(params object[] args)
		{
			if (TraceData._ChildNotAttached == null)
			{
				TraceData._ChildNotAttached = new AvTraceDetails(98, new string[]
				{
					"{0}: One or more children have not resolved sources"
				});
			}
			return new AvTraceFormat(TraceData._ChildNotAttached, args);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00101425 File Offset: 0x00100425
		public static AvTraceDetails GetRawValueMulti(params object[] args)
		{
			if (TraceData._GetRawValueMulti == null)
			{
				TraceData._GetRawValueMulti = new AvTraceDetails(99, new string[]
				{
					"{0}: TransferValue - got raw value {1}: {2}"
				});
			}
			return new AvTraceFormat(TraceData._GetRawValueMulti, args);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00101453 File Offset: 0x00100453
		public static AvTraceDetails UserConvertBackMulti(params object[] args)
		{
			if (TraceData._UserConvertBackMulti == null)
			{
				TraceData._UserConvertBackMulti = new AvTraceDetails(100, new string[]
				{
					"{0}: Update - multiconverter produced value {1}: {2}"
				});
			}
			return new AvTraceFormat(TraceData._UserConvertBackMulti, args);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00101481 File Offset: 0x00100481
		public static AvTraceDetails GetValue(params object[] args)
		{
			if (TraceData._GetValue == null)
			{
				TraceData._GetValue = new AvTraceDetails(101, new string[]
				{
					"{0}: GetValue at level {1} from {2} using {3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetValue, args);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x001014AF File Offset: 0x001004AF
		public static AvTraceDetails SetValue(params object[] args)
		{
			if (TraceData._SetValue == null)
			{
				TraceData._SetValue = new AvTraceDetails(102, new string[]
				{
					"{0}: SetValue at level {1} to {2} using {3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._SetValue, args);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x001014DD File Offset: 0x001004DD
		public static AvTraceDetails ReplaceItemShort(params object[] args)
		{
			if (TraceData._ReplaceItemShort == null)
			{
				TraceData._ReplaceItemShort = new AvTraceDetails(103, new string[]
				{
					"{0}: Replace item at level {1} with {2}"
				});
			}
			return new AvTraceFormat(TraceData._ReplaceItemShort, args);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0010150B File Offset: 0x0010050B
		public static AvTraceDetails ReplaceItemLong(params object[] args)
		{
			if (TraceData._ReplaceItemLong == null)
			{
				TraceData._ReplaceItemLong = new AvTraceDetails(104, new string[]
				{
					"{0}: Replace item at level {1} with {2}, using accessor {3}"
				});
			}
			return new AvTraceFormat(TraceData._ReplaceItemLong, args);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00101539 File Offset: 0x00100539
		public static AvTraceDetails GetInfo_Reuse(params object[] args)
		{
			if (TraceData._GetInfo_Reuse == null)
			{
				TraceData._GetInfo_Reuse = new AvTraceDetails(105, new string[]
				{
					"{0}:   Item at level {1} has same type - reuse accessor {2}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Reuse, args);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00101567 File Offset: 0x00100567
		public static AvTraceDetails GetInfo_Null(params object[] args)
		{
			if (TraceData._GetInfo_Null == null)
			{
				TraceData._GetInfo_Null = new AvTraceDetails(106, new string[]
				{
					"{0}:   Item at level {1} is null - no accessor"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Null, args);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00101595 File Offset: 0x00100595
		public static AvTraceDetails GetInfo_Cache(params object[] args)
		{
			if (TraceData._GetInfo_Cache == null)
			{
				TraceData._GetInfo_Cache = new AvTraceDetails(107, new string[]
				{
					"{0}:   At level {1} using cached accessor for {2}.{3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Cache, args);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x001015C3 File Offset: 0x001005C3
		public static AvTraceDetails GetInfo_Property(params object[] args)
		{
			if (TraceData._GetInfo_Property == null)
			{
				TraceData._GetInfo_Property = new AvTraceDetails(108, new string[]
				{
					"{0}:   At level {1} - for {2}.{3} found accessor {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Property, args);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x001015F1 File Offset: 0x001005F1
		public static AvTraceDetails GetInfo_Indexer(params object[] args)
		{
			if (TraceData._GetInfo_Indexer == null)
			{
				TraceData._GetInfo_Indexer = new AvTraceDetails(109, new string[]
				{
					"{0}:   At level {1} - for {2}[{3}] found accessor {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Indexer, args);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0010161F File Offset: 0x0010061F
		public static AvTraceDetails XmlContextNode(params object[] args)
		{
			if (TraceData._XmlContextNode == null)
			{
				TraceData._XmlContextNode = new AvTraceDetails(110, new string[]
				{
					"{0}: Context for XML binding set to {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlContextNode, args);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0010164D File Offset: 0x0010064D
		public static AvTraceDetails XmlNewCollection(params object[] args)
		{
			if (TraceData._XmlNewCollection == null)
			{
				TraceData._XmlNewCollection = new AvTraceDetails(111, new string[]
				{
					"{0}: Building collection from {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlNewCollection, args);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0010167B File Offset: 0x0010067B
		public static AvTraceDetails XmlSynchronizeCollection(params object[] args)
		{
			if (TraceData._XmlSynchronizeCollection == null)
			{
				TraceData._XmlSynchronizeCollection = new AvTraceDetails(112, new string[]
				{
					"{0}: Synchronizing collection with {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlSynchronizeCollection, args);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x001016A9 File Offset: 0x001006A9
		public static AvTraceDetails SelectNodes(params object[] args)
		{
			if (TraceData._SelectNodes == null)
			{
				TraceData._SelectNodes = new AvTraceDetails(113, new string[]
				{
					"{0}: SelectNodes at {1} using XPath {2}: {3}"
				});
			}
			return new AvTraceFormat(TraceData._SelectNodes, args);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x001016D7 File Offset: 0x001006D7
		public static AvTraceDetails BeginQuery(params object[] args)
		{
			if (TraceData._BeginQuery == null)
			{
				TraceData._BeginQuery = new AvTraceDetails(114, new string[]
				{
					"{0}: Begin query ({1})"
				});
			}
			return new AvTraceFormat(TraceData._BeginQuery, args);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00101705 File Offset: 0x00100705
		public static AvTraceDetails QueryFinished(params object[] args)
		{
			if (TraceData._QueryFinished == null)
			{
				TraceData._QueryFinished = new AvTraceDetails(115, new string[]
				{
					"{0}: Query finished ({1}) with data {2} and error {3}"
				});
			}
			return new AvTraceFormat(TraceData._QueryFinished, args);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00101733 File Offset: 0x00100733
		public static AvTraceDetails QueryResult(params object[] args)
		{
			if (TraceData._QueryResult == null)
			{
				TraceData._QueryResult = new AvTraceDetails(116, new string[]
				{
					"{0}: Update result (on UI thread) with data {1}"
				});
			}
			return new AvTraceFormat(TraceData._QueryResult, args);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00101761 File Offset: 0x00100761
		public static AvTraceDetails XmlLoadSource(params object[] args)
		{
			if (TraceData._XmlLoadSource == null)
			{
				TraceData._XmlLoadSource = new AvTraceDetails(117, new string[]
				{
					"{0}: Request download ({1}) from {2}"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadSource, args);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0010178F File Offset: 0x0010078F
		public static AvTraceDetails XmlLoadDoc(params object[] args)
		{
			if (TraceData._XmlLoadDoc == null)
			{
				TraceData._XmlLoadDoc = new AvTraceDetails(118, new string[]
				{
					"{0}: Load document from stream"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadDoc, args);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x001017BD File Offset: 0x001007BD
		public static AvTraceDetails XmlLoadInline(params object[] args)
		{
			if (TraceData._XmlLoadInline == null)
			{
				TraceData._XmlLoadInline = new AvTraceDetails(119, new string[]
				{
					"{0}: Load inline document"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadInline, args);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x001017EB File Offset: 0x001007EB
		public static AvTraceDetails XmlBuildCollection(params object[] args)
		{
			if (TraceData._XmlBuildCollection == null)
			{
				TraceData._XmlBuildCollection = new AvTraceDetails(120, new string[]
				{
					"{0}: Build XmlNode collection"
				});
			}
			return new AvTraceFormat(TraceData._XmlBuildCollection, args);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00101819 File Offset: 0x00100819
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0010183A File Offset: 0x0010083A
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00101860 File Offset: 0x00100860
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00101898 File Offset: 0x00100898
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x001018D4 File Offset: 0x001008D4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00101912 File Offset: 0x00100912
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00101931 File Offset: 0x00100931
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00101955 File Offset: 0x00100955
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0010197D File Offset: 0x0010097D
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x001019A9 File Offset: 0x001009A9
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000503 RID: 1283 RVA: 0x001019D9 File Offset: 0x001009D9
		public static bool IsEnabled
		{
			get
			{
				return TraceData._avTrace != null && TraceData._avTrace.IsEnabled;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x001019EE File Offset: 0x001009EE
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceData._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x001019FA File Offset: 0x001009FA
		public static void Refresh()
		{
			TraceData._avTrace.Refresh();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00101A08 File Offset: 0x00100A08
		static TraceData()
		{
			TraceData._avTrace.TraceExtraMessages += TraceData.OnTrace;
			TraceData._avTrace.EnabledByDebugger = true;
			TraceData._avTrace.SuppressGeneratedParameters = true;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00101A6B File Offset: 0x00100A6B
		public static bool IsExtendedTraceEnabled(object element, TraceDataLevel level)
		{
			return TraceData.IsEnabled && PresentationTraceSources.GetTraceLevel(element) >= (PresentationTraceLevel)level;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00101A84 File Offset: 0x00100A84
		public static void OnTrace(AvTraceBuilder traceBuilder, object[] parameters, int start)
		{
			for (int i = start; i < parameters.Length; i++)
			{
				object obj = parameters[i];
				string text = obj as string;
				traceBuilder.Append(" ");
				if (text != null)
				{
					traceBuilder.Append(text);
				}
				else if (obj != null)
				{
					traceBuilder.Append(obj.GetType().Name);
					traceBuilder.Append(":");
					TraceData.Describe(traceBuilder, obj);
				}
				else
				{
					traceBuilder.Append("null");
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00101AF4 File Offset: 0x00100AF4
		public static void Describe(AvTraceBuilder traceBuilder, object o)
		{
			if (o == null)
			{
				traceBuilder.Append("null");
				return;
			}
			if (o is BindingExpression)
			{
				BindingExpression bindingExpression = o as BindingExpression;
				TraceData.Describe(traceBuilder, bindingExpression.ParentBinding);
				traceBuilder.Append("; DataItem=");
				TraceData.DescribeSourceObject(traceBuilder, bindingExpression.DataItem);
				traceBuilder.Append("; ");
				TraceData.DescribeTarget(traceBuilder, bindingExpression.TargetElement, bindingExpression.TargetProperty);
				return;
			}
			if (o is Binding)
			{
				Binding binding = o as Binding;
				if (binding.Path != null)
				{
					traceBuilder.AppendFormat("Path={0}", binding.Path.Path);
					return;
				}
				if (binding.XPath != null)
				{
					traceBuilder.AppendFormat("XPath={0}", binding.XPath);
					return;
				}
				traceBuilder.Append("(no path)");
				return;
			}
			else
			{
				if (o is BindingExpressionBase)
				{
					BindingExpressionBase bindingExpressionBase = o as BindingExpressionBase;
					TraceData.DescribeTarget(traceBuilder, bindingExpressionBase.TargetElement, bindingExpressionBase.TargetProperty);
					return;
				}
				if (o is DependencyObject)
				{
					TraceData.DescribeSourceObject(traceBuilder, o);
					return;
				}
				traceBuilder.AppendFormat("'{0}'", AvTrace.ToStringHelper(o));
				return;
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00101BF8 File Offset: 0x00100BF8
		public static void DescribeSourceObject(AvTraceBuilder traceBuilder, object o)
		{
			if (o == null)
			{
				traceBuilder.Append("null");
				return;
			}
			FrameworkElement frameworkElement = o as FrameworkElement;
			if (frameworkElement != null)
			{
				traceBuilder.AppendFormat("'{0}' (Name='{1}')", frameworkElement.GetType().Name, frameworkElement.Name);
				return;
			}
			traceBuilder.AppendFormat("'{0}' (HashCode={1})", o.GetType().Name, o.GetHashCode());
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00101C5C File Offset: 0x00100C5C
		public static string DescribeSourceObject(object o)
		{
			AvTraceBuilder avTraceBuilder = new AvTraceBuilder(null);
			TraceData.DescribeSourceObject(avTraceBuilder, o);
			return avTraceBuilder.ToString();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00101C70 File Offset: 0x00100C70
		public static void DescribeTarget(AvTraceBuilder traceBuilder, DependencyObject targetElement, DependencyProperty targetProperty)
		{
			if (targetElement != null)
			{
				traceBuilder.Append("target element is ");
				TraceData.DescribeSourceObject(traceBuilder, targetElement);
				if (targetProperty != null)
				{
					traceBuilder.Append("; ");
				}
			}
			if (targetProperty != null)
			{
				traceBuilder.AppendFormat("target property is '{0}' (type '{1}')", targetProperty.Name, targetProperty.PropertyType.Name);
			}
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00101CBF File Offset: 0x00100CBF
		public static string DescribeTarget(DependencyObject targetElement, DependencyProperty targetProperty)
		{
			AvTraceBuilder avTraceBuilder = new AvTraceBuilder(null);
			TraceData.DescribeTarget(avTraceBuilder, targetElement, targetProperty);
			return avTraceBuilder.ToString();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00101CD4 File Offset: 0x00100CD4
		public static string Identify(object o)
		{
			if (o == null)
			{
				return "<null>";
			}
			Type type = o.GetType();
			if (type.IsPrimitive || type.IsEnum)
			{
				return TraceData.Format("'{0}'", new object[]
				{
					o
				});
			}
			string text = o as string;
			if (text != null)
			{
				return TraceData.Format("'{0}'", new object[]
				{
					AvTrace.AntiFormat(text)
				});
			}
			NamedObject namedObject = o as NamedObject;
			if (namedObject != null)
			{
				return AvTrace.AntiFormat(namedObject.ToString());
			}
			ICollection collection = o as ICollection;
			if (collection != null)
			{
				return TraceData.Format("{0} (hash={1} Count={2})", new object[]
				{
					type.Name,
					AvTrace.GetHashCodeHelper(o),
					collection.Count
				});
			}
			return TraceData.Format("{0} (hash={1})", new object[]
			{
				type.Name,
				AvTrace.GetHashCodeHelper(o)
			});
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00101DB8 File Offset: 0x00100DB8
		public static string IdentifyWeakEvent(Type type)
		{
			string text = type.Name;
			if (text.EndsWith("EventManager", StringComparison.Ordinal))
			{
				text = text.Substring(0, text.Length - "EventManager".Length);
			}
			return text;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00101DF4 File Offset: 0x00100DF4
		public static string IdentifyAccessor(object accessor)
		{
			DependencyProperty dependencyProperty = accessor as DependencyProperty;
			if (dependencyProperty != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					dependencyProperty.GetType().Name,
					dependencyProperty.Name
				});
			}
			PropertyInfo propertyInfo = accessor as PropertyInfo;
			if (propertyInfo != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					propertyInfo.GetType().Name,
					propertyInfo.Name
				});
			}
			PropertyDescriptor propertyDescriptor = accessor as PropertyDescriptor;
			if (propertyDescriptor != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					propertyDescriptor.GetType().Name,
					propertyDescriptor.Name
				});
			}
			return TraceData.Identify(accessor);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00101EA3 File Offset: 0x00100EA3
		public static string IdentifyException(Exception ex)
		{
			if (ex == null)
			{
				return "<no error>";
			}
			return TraceData.Format("{0} ({1})", new object[]
			{
				ex.GetType().Name,
				AvTrace.AntiFormat(ex.Message)
			});
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00101EDA File Offset: 0x00100EDA
		private static string Format(string format, params object[] args)
		{
			return string.Format(TypeConverterHelper.InvariantEnglishUS, format, args);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00101EE8 File Offset: 0x00100EE8
		public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding, Exception exception = null)
		{
			object[] array;
			if (exception == null)
			{
				(array = new object[1])[0] = binding;
			}
			else
			{
				object[] array2 = new object[2];
				array2[0] = binding;
				array = array2;
				array2[1] = exception;
			}
			object[] parameters = array;
			string text = TraceData._avTrace.Trace(eventType, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
			if (text != null && BindingDiagnostics.IsEnabled)
			{
				object obj;
				if (exception == null)
				{
					obj = null;
				}
				else
				{
					(obj = new object[1])[0] = exception;
				}
				object[] parameters2 = obj;
				BindingDiagnostics.NotifyBindingFailed(new BindingFailedEventArgs(eventType, traceDetails.Id, text, binding, parameters2));
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00101F64 File Offset: 0x00100F64
		public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, Exception exception = null)
		{
			object obj;
			if (exception == null)
			{
				obj = null;
			}
			else
			{
				(obj = new object[1])[0] = exception;
			}
			object[] array = obj;
			TraceData.TraceAndNotify(eventType, traceDetails, null, array, array);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00101F8C File Offset: 0x00100F8C
		public static void TraceAndNotify(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding, object[] traceParameters, object[] eventParameters = null)
		{
			string text = TraceData._avTrace.Trace(eventType, traceDetails.Id, traceDetails.Message, traceDetails.Labels, traceParameters);
			if (text != null && BindingDiagnostics.IsEnabled)
			{
				BindingDiagnostics.NotifyBindingFailed(new BindingFailedEventArgs(eventType, traceDetails.Id, text, binding, eventParameters));
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00101FD7 File Offset: 0x00100FD7
		public static void TraceAndNotifyWithNoParameters(TraceEventType eventType, AvTraceDetails traceDetails, BindingExpressionBase binding)
		{
			TraceData.TraceAndNotify(eventType, traceDetails, binding, null, null);
		}

		// Token: 0x04000630 RID: 1584
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.DataBindingSource, delegate()
		{
			PresentationTraceSources._DataBindingSource = null;
		});

		// Token: 0x04000631 RID: 1585
		private static AvTraceDetails _CannotCreateDefaultValueConverter;

		// Token: 0x04000632 RID: 1586
		private static AvTraceDetails _NoMentor;

		// Token: 0x04000633 RID: 1587
		private static AvTraceDetails _NoDataContext;

		// Token: 0x04000634 RID: 1588
		private static AvTraceDetails _NoSource;

		// Token: 0x04000635 RID: 1589
		private static AvTraceDetails _BadValueAtTransfer;

		// Token: 0x04000636 RID: 1590
		private static AvTraceDetails _BadConverterForTransfer;

		// Token: 0x04000637 RID: 1591
		private static AvTraceDetails _BadConverterForUpdate;

		// Token: 0x04000638 RID: 1592
		private static AvTraceDetails _WorkerUpdateFailed;

		// Token: 0x04000639 RID: 1593
		private static AvTraceDetails _RequiresExplicitCulture;

		// Token: 0x0400063A RID: 1594
		private static AvTraceDetails _NoValueToTransfer;

		// Token: 0x0400063B RID: 1595
		private static AvTraceDetails _FallbackConversionFailed;

		// Token: 0x0400063C RID: 1596
		private static AvTraceDetails _TargetNullValueConversionFailed;

		// Token: 0x0400063D RID: 1597
		private static AvTraceDetails _BindingGroupNameMatchFailed;

		// Token: 0x0400063E RID: 1598
		private static AvTraceDetails _BindingGroupWrongProperty;

		// Token: 0x0400063F RID: 1599
		private static AvTraceDetails _BindingGroupMultipleInheritance;

		// Token: 0x04000640 RID: 1600
		private static AvTraceDetails _SharesProposedValuesRequriesImplicitBindingGroup;

		// Token: 0x04000641 RID: 1601
		private static AvTraceDetails _CannotGetClrRawValue;

		// Token: 0x04000642 RID: 1602
		private static AvTraceDetails _CannotSetClrRawValue;

		// Token: 0x04000643 RID: 1603
		private static AvTraceDetails _MissingDataItem;

		// Token: 0x04000644 RID: 1604
		private static AvTraceDetails _MissingInfo;

		// Token: 0x04000645 RID: 1605
		private static AvTraceDetails _NullDataItem;

		// Token: 0x04000646 RID: 1606
		private static AvTraceDetails _DefaultValueConverterFailed;

		// Token: 0x04000647 RID: 1607
		private static AvTraceDetails _DefaultValueConverterFailedForCulture;

		// Token: 0x04000648 RID: 1608
		private static AvTraceDetails _StyleAndStyleSelectorDefined;

		// Token: 0x04000649 RID: 1609
		private static AvTraceDetails _TemplateAndTemplateSelectorDefined;

		// Token: 0x0400064A RID: 1610
		private static AvTraceDetails _ItemTemplateForDirectItem;

		// Token: 0x0400064B RID: 1611
		private static AvTraceDetails _BadMultiConverterForUpdate;

		// Token: 0x0400064C RID: 1612
		private static AvTraceDetails _MultiValueConverterMissingForTransfer;

		// Token: 0x0400064D RID: 1613
		private static AvTraceDetails _MultiValueConverterMissingForUpdate;

		// Token: 0x0400064E RID: 1614
		private static AvTraceDetails _MultiValueConverterMismatch;

		// Token: 0x0400064F RID: 1615
		private static AvTraceDetails _MultiBindingHasNoConverter;

		// Token: 0x04000650 RID: 1616
		private static AvTraceDetails _UnsetValueInMultiBindingExpressionUpdate;

		// Token: 0x04000651 RID: 1617
		private static AvTraceDetails _ObjectDataProviderHasNoSource;

		// Token: 0x04000652 RID: 1618
		private static AvTraceDetails _ObjDPCreateFailed;

		// Token: 0x04000653 RID: 1619
		private static AvTraceDetails _ObjDPInvokeFailed;

		// Token: 0x04000654 RID: 1620
		private static AvTraceDetails _RefPreviousNotInContext;

		// Token: 0x04000655 RID: 1621
		private static AvTraceDetails _RefNoWrapperInChildren;

		// Token: 0x04000656 RID: 1622
		private static AvTraceDetails _RefAncestorTypeNotSpecified;

		// Token: 0x04000657 RID: 1623
		private static AvTraceDetails _RefAncestorLevelInvalid;

		// Token: 0x04000658 RID: 1624
		private static AvTraceDetails _ClrReplaceItem;

		// Token: 0x04000659 RID: 1625
		private static AvTraceDetails _NullItem;

		// Token: 0x0400065A RID: 1626
		private static AvTraceDetails _PlaceholderItem;

		// Token: 0x0400065B RID: 1627
		private static AvTraceDetails _DataErrorInfoFailed;

		// Token: 0x0400065C RID: 1628
		private static AvTraceDetails _DisallowTwoWay;

		// Token: 0x0400065D RID: 1629
		private static AvTraceDetails _XmlBindingToNonXml;

		// Token: 0x0400065E RID: 1630
		private static AvTraceDetails _XmlBindingToNonXmlCollection;

		// Token: 0x0400065F RID: 1631
		private static AvTraceDetails _CannotGetXmlNodeCollection;

		// Token: 0x04000660 RID: 1632
		private static AvTraceDetails _BadXPath;

		// Token: 0x04000661 RID: 1633
		private static AvTraceDetails _XmlDPInlineDocError;

		// Token: 0x04000662 RID: 1634
		private static AvTraceDetails _XmlNamespaceNotSet;

		// Token: 0x04000663 RID: 1635
		private static AvTraceDetails _XmlDPAsyncDocError;

		// Token: 0x04000664 RID: 1636
		private static AvTraceDetails _XmlDPSelectNodesFailed;

		// Token: 0x04000665 RID: 1637
		private static AvTraceDetails _CollectionViewIsUnsupported;

		// Token: 0x04000666 RID: 1638
		private static AvTraceDetails _CollectionChangedWithoutNotification;

		// Token: 0x04000667 RID: 1639
		private static AvTraceDetails _CannotSort;

		// Token: 0x04000668 RID: 1640
		private static AvTraceDetails _CreatedExpression;

		// Token: 0x04000669 RID: 1641
		private static AvTraceDetails _CreatedExpressionInParent;

		// Token: 0x0400066A RID: 1642
		private static AvTraceDetails _BindingPath;

		// Token: 0x0400066B RID: 1643
		private static AvTraceDetails _BindingXPathAndPath;

		// Token: 0x0400066C RID: 1644
		private static AvTraceDetails _ResolveDefaultMode;

		// Token: 0x0400066D RID: 1645
		private static AvTraceDetails _ResolveDefaultUpdate;

		// Token: 0x0400066E RID: 1646
		private static AvTraceDetails _AttachExpression;

		// Token: 0x0400066F RID: 1647
		private static AvTraceDetails _DetachExpression;

		// Token: 0x04000670 RID: 1648
		private static AvTraceDetails _UseMentor;

		// Token: 0x04000671 RID: 1649
		private static AvTraceDetails _DeferAttachToContext;

		// Token: 0x04000672 RID: 1650
		private static AvTraceDetails _SourceRequiresTreeContext;

		// Token: 0x04000673 RID: 1651
		private static AvTraceDetails _AttachToContext;

		// Token: 0x04000674 RID: 1652
		private static AvTraceDetails _PathRequiresTreeContext;

		// Token: 0x04000675 RID: 1653
		private static AvTraceDetails _NoMentorExtended;

		// Token: 0x04000676 RID: 1654
		private static AvTraceDetails _ContextElement;

		// Token: 0x04000677 RID: 1655
		private static AvTraceDetails _NullDataContext;

		// Token: 0x04000678 RID: 1656
		private static AvTraceDetails _RelativeSource;

		// Token: 0x04000679 RID: 1657
		private static AvTraceDetails _AncestorLookup;

		// Token: 0x0400067A RID: 1658
		private static AvTraceDetails _ElementNameQuery;

		// Token: 0x0400067B RID: 1659
		private static AvTraceDetails _ElementNameQueryTemplate;

		// Token: 0x0400067C RID: 1660
		private static AvTraceDetails _UseCVS;

		// Token: 0x0400067D RID: 1661
		private static AvTraceDetails _UseDataProvider;

		// Token: 0x0400067E RID: 1662
		private static AvTraceDetails _ActivateItem;

		// Token: 0x0400067F RID: 1663
		private static AvTraceDetails _Deactivate;

		// Token: 0x04000680 RID: 1664
		private static AvTraceDetails _GetRawValue;

		// Token: 0x04000681 RID: 1665
		private static AvTraceDetails _ConvertDBNull;

		// Token: 0x04000682 RID: 1666
		private static AvTraceDetails _UserConverter;

		// Token: 0x04000683 RID: 1667
		private static AvTraceDetails _NullConverter;

		// Token: 0x04000684 RID: 1668
		private static AvTraceDetails _DefaultConverter;

		// Token: 0x04000685 RID: 1669
		private static AvTraceDetails _FormattedValue;

		// Token: 0x04000686 RID: 1670
		private static AvTraceDetails _FormattingFailed;

		// Token: 0x04000687 RID: 1671
		private static AvTraceDetails _BadValueAtTransferExtended;

		// Token: 0x04000688 RID: 1672
		private static AvTraceDetails _UseFallback;

		// Token: 0x04000689 RID: 1673
		private static AvTraceDetails _TransferValue;

		// Token: 0x0400068A RID: 1674
		private static AvTraceDetails _UpdateRawValue;

		// Token: 0x0400068B RID: 1675
		private static AvTraceDetails _ValidationRuleFailed;

		// Token: 0x0400068C RID: 1676
		private static AvTraceDetails _UserConvertBack;

		// Token: 0x0400068D RID: 1677
		private static AvTraceDetails _DefaultConvertBack;

		// Token: 0x0400068E RID: 1678
		private static AvTraceDetails _Update;

		// Token: 0x0400068F RID: 1679
		private static AvTraceDetails _GotEvent;

		// Token: 0x04000690 RID: 1680
		private static AvTraceDetails _GotPropertyChanged;

		// Token: 0x04000691 RID: 1681
		private static AvTraceDetails _PriorityTransfer;

		// Token: 0x04000692 RID: 1682
		private static AvTraceDetails _ChildNotAttached;

		// Token: 0x04000693 RID: 1683
		private static AvTraceDetails _GetRawValueMulti;

		// Token: 0x04000694 RID: 1684
		private static AvTraceDetails _UserConvertBackMulti;

		// Token: 0x04000695 RID: 1685
		private static AvTraceDetails _GetValue;

		// Token: 0x04000696 RID: 1686
		private static AvTraceDetails _SetValue;

		// Token: 0x04000697 RID: 1687
		private static AvTraceDetails _ReplaceItemShort;

		// Token: 0x04000698 RID: 1688
		private static AvTraceDetails _ReplaceItemLong;

		// Token: 0x04000699 RID: 1689
		private static AvTraceDetails _GetInfo_Reuse;

		// Token: 0x0400069A RID: 1690
		private static AvTraceDetails _GetInfo_Null;

		// Token: 0x0400069B RID: 1691
		private static AvTraceDetails _GetInfo_Cache;

		// Token: 0x0400069C RID: 1692
		private static AvTraceDetails _GetInfo_Property;

		// Token: 0x0400069D RID: 1693
		private static AvTraceDetails _GetInfo_Indexer;

		// Token: 0x0400069E RID: 1694
		private static AvTraceDetails _XmlContextNode;

		// Token: 0x0400069F RID: 1695
		private static AvTraceDetails _XmlNewCollection;

		// Token: 0x040006A0 RID: 1696
		private static AvTraceDetails _XmlSynchronizeCollection;

		// Token: 0x040006A1 RID: 1697
		private static AvTraceDetails _SelectNodes;

		// Token: 0x040006A2 RID: 1698
		private static AvTraceDetails _BeginQuery;

		// Token: 0x040006A3 RID: 1699
		private static AvTraceDetails _QueryFinished;

		// Token: 0x040006A4 RID: 1700
		private static AvTraceDetails _QueryResult;

		// Token: 0x040006A5 RID: 1701
		private static AvTraceDetails _XmlLoadSource;

		// Token: 0x040006A6 RID: 1702
		private static AvTraceDetails _XmlLoadDoc;

		// Token: 0x040006A7 RID: 1703
		private static AvTraceDetails _XmlLoadInline;

		// Token: 0x040006A8 RID: 1704
		private static AvTraceDetails _XmlBuildCollection;
	}
}
