using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000462 RID: 1122
	[ContentProperty("Bindings")]
	public class MultiBinding : BindingBase, IAddChild
	{
		// Token: 0x06003966 RID: 14694 RVA: 0x001ED05C File Offset: 0x001EC05C
		public MultiBinding()
		{
			this._bindingCollection = new BindingCollection(this, new BindingCollectionChangedCallback(this.OnBindingCollectionChanged));
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x001ED07C File Offset: 0x001EC07C
		void IAddChild.AddChild(object value)
		{
			BindingBase bindingBase = value as BindingBase;
			if (bindingBase != null)
			{
				this.Bindings.Add(bindingBase);
				return;
			}
			throw new ArgumentException(SR.Get("ChildHasWrongType", new object[]
			{
				base.GetType().Name,
				"BindingBase",
				value.GetType().FullName
			}), "value");
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00175B1C File Offset: 0x00174B1C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x001ED0DE File Offset: 0x001EC0DE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public Collection<BindingBase> Bindings
		{
			get
			{
				return this._bindingCollection;
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x001ED0E6 File Offset: 0x001EC0E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeBindings()
		{
			return this.Bindings != null && this.Bindings.Count > 0;
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x001ED100 File Offset: 0x001EC100
		// (set) Token: 0x0600396C RID: 14700 RVA: 0x001ED13C File Offset: 0x001EC13C
		[DefaultValue(BindingMode.Default)]
		public BindingMode Mode
		{
			get
			{
				switch (base.GetFlagsWithinMask(BindingBase.BindingFlags.PropagationMask))
				{
				case BindingBase.BindingFlags.OneTime:
					return BindingMode.OneTime;
				case BindingBase.BindingFlags.OneWay:
					return BindingMode.OneWay;
				case BindingBase.BindingFlags.OneWayToSource:
					return BindingMode.OneWayToSource;
				case BindingBase.BindingFlags.TwoWay:
					return BindingMode.TwoWay;
				case BindingBase.BindingFlags.PropDefault:
					return BindingMode.Default;
				default:
					return BindingMode.TwoWay;
				}
			}
			set
			{
				base.CheckSealed();
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.PropagationMask, BindingBase.FlagsFrom(value));
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x0600396D RID: 14701 RVA: 0x001ED154 File Offset: 0x001EC154
		// (set) Token: 0x0600396E RID: 14702 RVA: 0x001ED19D File Offset: 0x001EC19D
		[DefaultValue(UpdateSourceTrigger.PropertyChanged)]
		public UpdateSourceTrigger UpdateSourceTrigger
		{
			get
			{
				BindingBase.BindingFlags flagsWithinMask = base.GetFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault);
				if (flagsWithinMask <= BindingBase.BindingFlags.UpdateOnLostFocus)
				{
					if (flagsWithinMask == BindingBase.BindingFlags.OneTime)
					{
						return UpdateSourceTrigger.PropertyChanged;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateOnLostFocus)
					{
						return UpdateSourceTrigger.LostFocus;
					}
				}
				else
				{
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateExplicitly)
					{
						return UpdateSourceTrigger.Explicit;
					}
					if (flagsWithinMask == BindingBase.BindingFlags.UpdateDefault)
					{
						return UpdateSourceTrigger.Default;
					}
				}
				return UpdateSourceTrigger.Default;
			}
			set
			{
				base.CheckSealed();
				base.ChangeFlagsWithinMask(BindingBase.BindingFlags.UpdateDefault, BindingBase.FlagsFrom(value));
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600396F RID: 14703 RVA: 0x001DDBD4 File Offset: 0x001DCBD4
		// (set) Token: 0x06003970 RID: 14704 RVA: 0x001DDBE1 File Offset: 0x001DCBE1
		[DefaultValue(false)]
		public bool NotifyOnSourceUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnSourceUpdated, value);
				}
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x001DDC03 File Offset: 0x001DCC03
		// (set) Token: 0x06003972 RID: 14706 RVA: 0x001DDC0C File Offset: 0x001DCC0C
		[DefaultValue(false)]
		public bool NotifyOnTargetUpdated
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnTargetUpdated, value);
				}
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06003973 RID: 14707 RVA: 0x001DDC26 File Offset: 0x001DCC26
		// (set) Token: 0x06003974 RID: 14708 RVA: 0x001DDC33 File Offset: 0x001DCC33
		[DefaultValue(false)]
		public bool NotifyOnValidationError
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.NotifyOnValidationError) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.NotifyOnValidationError, value);
				}
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06003975 RID: 14709 RVA: 0x001ED1B6 File Offset: 0x001EC1B6
		// (set) Token: 0x06003976 RID: 14710 RVA: 0x001DDC65 File Offset: 0x001DCC65
		[DefaultValue(null)]
		public IMultiValueConverter Converter
		{
			get
			{
				return (IMultiValueConverter)base.GetValue(BindingBase.Feature.Converter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Converter, value, null);
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x001DDC77 File Offset: 0x001DCC77
		// (set) Token: 0x06003978 RID: 14712 RVA: 0x001DDC82 File Offset: 0x001DCC82
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return base.GetValue(BindingBase.Feature.ConverterParameter, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.ConverterParameter, value, null);
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06003979 RID: 14713 RVA: 0x001DDC94 File Offset: 0x001DCC94
		// (set) Token: 0x0600397A RID: 14714 RVA: 0x001DDCA3 File Offset: 0x001DCCA3
		[DefaultValue(null)]
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture
		{
			get
			{
				return (CultureInfo)base.GetValue(BindingBase.Feature.Culture, null);
			}
			set
			{
				base.CheckSealed();
				base.SetValue(BindingBase.Feature.Culture, value, null);
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x0600397B RID: 14715 RVA: 0x001DD90D File Offset: 0x001DC90D
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				if (!base.HasValue(BindingBase.Feature.ValidationRules))
				{
					base.SetValue(BindingBase.Feature.ValidationRules, new ValidationRuleCollection());
				}
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x001ED1C6 File Offset: 0x001EC1C6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeValidationRules()
		{
			return base.HasValue(BindingBase.Feature.ValidationRules) && this.ValidationRules.Count > 0;
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x001DDEAE File Offset: 0x001DCEAE
		// (set) Token: 0x0600397E RID: 14718 RVA: 0x001DDEBE File Offset: 0x001DCEBE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
		{
			get
			{
				return (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			}
			set
			{
				base.SetValue(BindingBase.Feature.ExceptionFilterCallback, value, null);
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x001DD950 File Offset: 0x001DC950
		// (set) Token: 0x06003980 RID: 14720 RVA: 0x001DD95D File Offset: 0x001DC95D
		[DefaultValue(false)]
		public bool ValidatesOnExceptions
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnExceptions) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnExceptions, value);
				}
			}
		}

		// Token: 0x17000C6F RID: 3183
		// (get) Token: 0x06003981 RID: 14721 RVA: 0x001DD97F File Offset: 0x001DC97F
		// (set) Token: 0x06003982 RID: 14722 RVA: 0x001DD98C File Offset: 0x001DC98C
		[DefaultValue(false)]
		public bool ValidatesOnDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnDataErrors) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnDataErrors, value);
				}
			}
		}

		// Token: 0x17000C70 RID: 3184
		// (get) Token: 0x06003983 RID: 14723 RVA: 0x001DD9AE File Offset: 0x001DC9AE
		// (set) Token: 0x06003984 RID: 14724 RVA: 0x001DD9BB File Offset: 0x001DC9BB
		[DefaultValue(true)]
		public bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors);
			}
			set
			{
				if (base.TestFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors) != value)
				{
					base.CheckSealed();
					base.ChangeFlag(BindingBase.BindingFlags.ValidatesOnNotifyDataErrors, value);
				}
			}
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x001ED1E4 File Offset: 0x001EC1E4
		internal override BindingExpressionBase CreateBindingExpressionOverride(DependencyObject target, DependencyProperty dp, BindingExpressionBase owner)
		{
			if (this.Converter == null && string.IsNullOrEmpty(base.StringFormat))
			{
				throw new InvalidOperationException(SR.Get("MultiBindingHasNoConverter"));
			}
			for (int i = 0; i < this.Bindings.Count; i++)
			{
				MultiBinding.CheckTrigger(this.Bindings[i]);
			}
			return MultiBindingExpression.CreateBindingExpression(target, dp, this, owner);
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x001DDED5 File Offset: 0x001DCED5
		internal override ValidationRule LookupValidationRule(Type type)
		{
			return BindingBase.LookupValidationRule(type, this.ValidationRulesInternal);
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x001DDEE4 File Offset: 0x001DCEE4
		internal object DoFilterException(object bindExpr, Exception exception)
		{
			UpdateSourceExceptionFilterCallback updateSourceExceptionFilterCallback = (UpdateSourceExceptionFilterCallback)base.GetValue(BindingBase.Feature.ExceptionFilterCallback, null);
			if (updateSourceExceptionFilterCallback != null)
			{
				return updateSourceExceptionFilterCallback(bindExpr, exception);
			}
			return exception;
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x001ED248 File Offset: 0x001EC248
		internal static void CheckTrigger(BindingBase bb)
		{
			Binding binding = bb as Binding;
			if (binding != null && binding.UpdateSourceTrigger != UpdateSourceTrigger.PropertyChanged && binding.UpdateSourceTrigger != UpdateSourceTrigger.Default)
			{
				throw new InvalidOperationException(SR.Get("NoUpdateSourceTriggerForInnerBindingOfMultiBinding"));
			}
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x001ED280 File Offset: 0x001EC280
		internal override BindingBase CreateClone()
		{
			return new MultiBinding();
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x001ED288 File Offset: 0x001EC288
		internal override void InitializeClone(BindingBase baseClone, BindingMode mode)
		{
			MultiBinding multiBinding = (MultiBinding)baseClone;
			base.CopyValue(BindingBase.Feature.Converter, multiBinding);
			base.CopyValue(BindingBase.Feature.ConverterParameter, multiBinding);
			base.CopyValue(BindingBase.Feature.Culture, multiBinding);
			base.CopyValue(BindingBase.Feature.ValidationRules, multiBinding);
			base.CopyValue(BindingBase.Feature.ExceptionFilterCallback, multiBinding);
			for (int i = 0; i < this._bindingCollection.Count; i++)
			{
				multiBinding._bindingCollection.Add(this._bindingCollection[i].Clone(mode));
			}
			base.InitializeClone(baseClone, mode);
		}

		// Token: 0x17000C71 RID: 3185
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x001DE065 File Offset: 0x001DD065
		internal override Collection<ValidationRule> ValidationRulesInternal
		{
			get
			{
				return (ValidationRuleCollection)base.GetValue(BindingBase.Feature.ValidationRules, null);
			}
		}

		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x001ED303 File Offset: 0x001EC303
		internal override CultureInfo ConverterCultureInternal
		{
			get
			{
				return this.ConverterCulture;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x001ED30B File Offset: 0x001EC30B
		internal override bool ValidatesOnNotifyDataErrorsInternal
		{
			get
			{
				return this.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x0600398E RID: 14734 RVA: 0x001ED313 File Offset: 0x001EC313
		private void OnBindingCollectionChanged()
		{
			base.CheckSealed();
		}

		// Token: 0x04001D6C RID: 7532
		private BindingCollection _bindingCollection;
	}
}
