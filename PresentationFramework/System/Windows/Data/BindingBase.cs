using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows.Data
{
	// Token: 0x0200044B RID: 1099
	[Localizability(LocalizationCategory.None, Modifiability = Modifiability.Unmodifiable, Readability = Readability.Unreadable)]
	[MarkupExtensionReturnType(typeof(object))]
	public abstract class BindingBase : MarkupExtension
	{
		// Token: 0x0600358F RID: 13711 RVA: 0x001DE1AD File Offset: 0x001DD1AD
		internal BindingBase()
		{
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003590 RID: 13712 RVA: 0x001DE1C0 File Offset: 0x001DD1C0
		// (set) Token: 0x06003591 RID: 13713 RVA: 0x001DE1CE File Offset: 0x001DD1CE
		public object FallbackValue
		{
			get
			{
				return this.GetValue(BindingBase.Feature.FallbackValue, DependencyProperty.UnsetValue);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.FallbackValue, value);
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x001DE1DE File Offset: 0x001DD1DE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeFallbackValue()
		{
			return this.HasValue(BindingBase.Feature.FallbackValue);
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003593 RID: 13715 RVA: 0x001DE1E7 File Offset: 0x001DD1E7
		// (set) Token: 0x06003594 RID: 13716 RVA: 0x001DE1F6 File Offset: 0x001DD1F6
		[DefaultValue(null)]
		public string StringFormat
		{
			get
			{
				return (string)this.GetValue(BindingBase.Feature.StringFormat, null);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.StringFormat, value, null);
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003595 RID: 13717 RVA: 0x001DE207 File Offset: 0x001DD207
		// (set) Token: 0x06003596 RID: 13718 RVA: 0x001DE215 File Offset: 0x001DD215
		public object TargetNullValue
		{
			get
			{
				return this.GetValue(BindingBase.Feature.TargetNullValue, DependencyProperty.UnsetValue);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.TargetNullValue, value);
			}
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x001DE225 File Offset: 0x001DD225
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTargetNullValue()
		{
			return this.HasValue(BindingBase.Feature.TargetNullValue);
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x001DE22E File Offset: 0x001DD22E
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x001DE241 File Offset: 0x001DD241
		[DefaultValue("")]
		public string BindingGroupName
		{
			get
			{
				return (string)this.GetValue(BindingBase.Feature.BindingGroupName, string.Empty);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.BindingGroupName, value, string.Empty);
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600359A RID: 13722 RVA: 0x001DE256 File Offset: 0x001DD256
		// (set) Token: 0x0600359B RID: 13723 RVA: 0x001DE26A File Offset: 0x001DD26A
		[DefaultValue(0)]
		public int Delay
		{
			get
			{
				return (int)this.GetValue(BindingBase.Feature.Delay, 0);
			}
			set
			{
				this.CheckSealed();
				this.SetValue(BindingBase.Feature.Delay, value, 0);
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x001DE288 File Offset: 0x001DD288
		public sealed override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				return this;
			}
			DependencyObject dependencyObject;
			DependencyProperty dependencyProperty;
			Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out dependencyObject, out dependencyProperty);
			if (dependencyObject == null || dependencyProperty == null)
			{
				return this;
			}
			return this.CreateBindingExpression(dependencyObject, dependencyProperty);
		}

		// Token: 0x0600359D RID: 13725
		internal abstract BindingExpressionBase CreateBindingExpressionOverride(DependencyObject targetObject, DependencyProperty targetProperty, BindingExpressionBase owner);

		// Token: 0x0600359E RID: 13726 RVA: 0x001DE2B5 File Offset: 0x001DD2B5
		internal bool TestFlag(BindingBase.BindingFlags flag)
		{
			return (this._flags & flag) > BindingBase.BindingFlags.OneTime;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x001DE2C2 File Offset: 0x001DD2C2
		internal void SetFlag(BindingBase.BindingFlags flag)
		{
			this._flags |= flag;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x001DE2D2 File Offset: 0x001DD2D2
		internal void ClearFlag(BindingBase.BindingFlags flag)
		{
			this._flags &= ~flag;
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x001DE2E3 File Offset: 0x001DD2E3
		internal void ChangeFlag(BindingBase.BindingFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x001DE306 File Offset: 0x001DD306
		internal BindingBase.BindingFlags GetFlagsWithinMask(BindingBase.BindingFlags mask)
		{
			return this._flags & mask;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x001DE310 File Offset: 0x001DD310
		internal void ChangeFlagsWithinMask(BindingBase.BindingFlags mask, BindingBase.BindingFlags flags)
		{
			this._flags = ((this._flags & ~mask) | (flags & mask));
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x001DE325 File Offset: 0x001DD325
		internal static BindingBase.BindingFlags FlagsFrom(BindingMode bindingMode)
		{
			switch (bindingMode)
			{
			case BindingMode.TwoWay:
				return BindingBase.BindingFlags.TwoWay;
			case BindingMode.OneWay:
				return BindingBase.BindingFlags.OneWay;
			case BindingMode.OneTime:
				return BindingBase.BindingFlags.OneTime;
			case BindingMode.OneWayToSource:
				return BindingBase.BindingFlags.OneWayToSource;
			case BindingMode.Default:
				return BindingBase.BindingFlags.PropDefault;
			default:
				return BindingBase.BindingFlags.IllegalInput;
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x001DE352 File Offset: 0x001DD352
		internal static BindingBase.BindingFlags FlagsFrom(UpdateSourceTrigger updateSourceTrigger)
		{
			switch (updateSourceTrigger)
			{
			case UpdateSourceTrigger.Default:
				return BindingBase.BindingFlags.UpdateDefault;
			case UpdateSourceTrigger.PropertyChanged:
				return BindingBase.BindingFlags.OneTime;
			case UpdateSourceTrigger.LostFocus:
				return BindingBase.BindingFlags.UpdateOnLostFocus;
			case UpdateSourceTrigger.Explicit:
				return BindingBase.BindingFlags.UpdateExplicitly;
			default:
				return BindingBase.BindingFlags.IllegalInput;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x001DE385 File Offset: 0x001DD385
		internal BindingBase.BindingFlags Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual CultureInfo ConverterCultureInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x060035A8 RID: 13736 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x001DE38D File Offset: 0x001DD38D
		internal BindingExpressionBase CreateBindingExpression(DependencyObject targetObject, DependencyProperty targetProperty)
		{
			this._isSealed = true;
			return this.CreateBindingExpressionOverride(targetObject, targetProperty, null);
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x001DE39F File Offset: 0x001DD39F
		internal BindingExpressionBase CreateBindingExpression(DependencyObject targetObject, DependencyProperty targetProperty, BindingExpressionBase owner)
		{
			this._isSealed = true;
			return this.CreateBindingExpressionOverride(targetObject, targetProperty, owner);
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x001DE3B1 File Offset: 0x001DD3B1
		internal void CheckSealed()
		{
			if (this._isSealed)
			{
				throw new InvalidOperationException(SR.Get("ChangeSealedBinding"));
			}
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x001DE3CC File Offset: 0x001DD3CC
		internal ValidationRule GetValidationRule(Type type)
		{
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions) && type == typeof(ExceptionValidationRule))
			{
				return ExceptionValidationRule.Instance;
			}
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors) && type == typeof(DataErrorValidationRule))
			{
				return DataErrorValidationRule.Instance;
			}
			if (this.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors) && type == typeof(NotifyDataErrorValidationRule))
			{
				return NotifyDataErrorValidationRule.Instance;
			}
			return this.LookupValidationRule(type);
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual ValidationRule LookupValidationRule(Type type)
		{
			return null;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x001DE450 File Offset: 0x001DD450
		internal static ValidationRule LookupValidationRule(Type type, Collection<ValidationRule> collection)
		{
			if (collection == null)
			{
				return null;
			}
			for (int i = 0; i < collection.Count; i++)
			{
				if (type.IsInstanceOfType(collection[i]))
				{
					return collection[i];
				}
			}
			return null;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x001DE48C File Offset: 0x001DD48C
		internal BindingBase Clone(BindingMode mode)
		{
			BindingBase bindingBase = this.CreateClone();
			this.InitializeClone(bindingBase, mode);
			return bindingBase;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x001DE4AC File Offset: 0x001DD4AC
		internal virtual void InitializeClone(BindingBase clone, BindingMode mode)
		{
			clone._flags = this._flags;
			this.CopyValue(BindingBase.Feature.FallbackValue, clone);
			clone._isSealed = this._isSealed;
			this.CopyValue(BindingBase.Feature.StringFormat, clone);
			this.CopyValue(BindingBase.Feature.TargetNullValue, clone);
			this.CopyValue(BindingBase.Feature.BindingGroupName, clone);
			clone.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, BindingBase.FlagsFrom(mode));
		}

		// Token: 0x060035B2 RID: 13746
		internal abstract BindingBase CreateClone();

		// Token: 0x060035B3 RID: 13747 RVA: 0x001DE4FE File Offset: 0x001DD4FE
		internal bool HasValue(BindingBase.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x001DE50C File Offset: 0x001DD50C
		internal object GetValue(BindingBase.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x001DE51B File Offset: 0x001DD51B
		internal void SetValue(BindingBase.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x001DE52A File Offset: 0x001DD52A
		internal void SetValue(BindingBase.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x001DE54F File Offset: 0x001DD54F
		internal void ClearValue(BindingBase.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x001DE55D File Offset: 0x001DD55D
		internal void CopyValue(BindingBase.Feature id, BindingBase clone)
		{
			if (this.HasValue(id))
			{
				clone.SetValue(id, this.GetValue(id, null));
			}
		}

		// Token: 0x04001CAE RID: 7342
		private BindingBase.BindingFlags _flags = BindingBase.BindingFlags.Default;

		// Token: 0x04001CAF RID: 7343
		private bool _isSealed;

		// Token: 0x04001CB0 RID: 7344
		private UncommonValueTable _values;

		// Token: 0x02000AC8 RID: 2760
		[Flags]
		internal enum BindingFlags : uint
		{
			// Token: 0x04004669 RID: 18025
			OneWay = 1U,
			// Token: 0x0400466A RID: 18026
			TwoWay = 3U,
			// Token: 0x0400466B RID: 18027
			OneWayToSource = 2U,
			// Token: 0x0400466C RID: 18028
			OneTime = 0U,
			// Token: 0x0400466D RID: 18029
			PropDefault = 4U,
			// Token: 0x0400466E RID: 18030
			NotifyOnTargetUpdated = 8U,
			// Token: 0x0400466F RID: 18031
			NotifyOnSourceUpdated = 8388608U,
			// Token: 0x04004670 RID: 18032
			NotifyOnValidationError = 2097152U,
			// Token: 0x04004671 RID: 18033
			UpdateDefault = 3072U,
			// Token: 0x04004672 RID: 18034
			UpdateOnPropertyChanged = 0U,
			// Token: 0x04004673 RID: 18035
			UpdateOnLostFocus = 1024U,
			// Token: 0x04004674 RID: 18036
			UpdateExplicitly = 2048U,
			// Token: 0x04004675 RID: 18037
			PathGeneratedInternally = 8192U,
			// Token: 0x04004676 RID: 18038
			ValidatesOnExceptions = 16777216U,
			// Token: 0x04004677 RID: 18039
			ValidatesOnDataErrors = 33554432U,
			// Token: 0x04004678 RID: 18040
			ValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x04004679 RID: 18041
			PropagationMask = 7U,
			// Token: 0x0400467A RID: 18042
			UpdateMask = 3072U,
			// Token: 0x0400467B RID: 18043
			Default = 536873988U,
			// Token: 0x0400467C RID: 18044
			IllegalInput = 67108864U
		}

		// Token: 0x02000AC9 RID: 2761
		internal enum Feature
		{
			// Token: 0x0400467E RID: 18046
			FallbackValue,
			// Token: 0x0400467F RID: 18047
			StringFormat,
			// Token: 0x04004680 RID: 18048
			TargetNullValue,
			// Token: 0x04004681 RID: 18049
			BindingGroupName,
			// Token: 0x04004682 RID: 18050
			Delay,
			// Token: 0x04004683 RID: 18051
			XPath,
			// Token: 0x04004684 RID: 18052
			Culture,
			// Token: 0x04004685 RID: 18053
			AsyncState,
			// Token: 0x04004686 RID: 18054
			ObjectSource,
			// Token: 0x04004687 RID: 18055
			RelativeSource,
			// Token: 0x04004688 RID: 18056
			ElementSource,
			// Token: 0x04004689 RID: 18057
			Converter,
			// Token: 0x0400468A RID: 18058
			ConverterParameter,
			// Token: 0x0400468B RID: 18059
			ValidationRules,
			// Token: 0x0400468C RID: 18060
			ExceptionFilterCallback,
			// Token: 0x0400468D RID: 18061
			AttachedPropertiesInPath,
			// Token: 0x0400468E RID: 18062
			LastFeatureId
		}
	}
}
