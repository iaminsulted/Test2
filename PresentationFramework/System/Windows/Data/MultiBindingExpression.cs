using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000463 RID: 1123
	public sealed class MultiBindingExpression : BindingExpressionBase, IDataBindEngineClient
	{
		// Token: 0x0600398F RID: 14735 RVA: 0x001ED31C File Offset: 0x001EC31C
		private MultiBindingExpression(MultiBinding binding, BindingExpressionBase owner) : base(binding, owner)
		{
			int count = binding.Bindings.Count;
			this._tempValues = new object[count];
			this._tempTypes = new Type[count];
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x001ED360 File Offset: 0x001EC360
		void IDataBindEngineClient.TransferValue()
		{
			this.TransferValue();
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x001DE61C File Offset: 0x001DD61C
		void IDataBindEngineClient.UpdateValue()
		{
			base.UpdateValue();
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x001ED368 File Offset: 0x001EC368
		bool IDataBindEngineClient.AttachToContext(bool lastChance)
		{
			this.AttachToContext(lastChance);
			return !base.TransferIsDeferred;
		}

		// Token: 0x06003993 RID: 14739 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		void IDataBindEngineClient.VerifySourceReference(bool lastChance)
		{
		}

		// Token: 0x06003994 RID: 14740 RVA: 0x001ED37A File Offset: 0x001EC37A
		void IDataBindEngineClient.OnTargetUpdated()
		{
			this.OnTargetUpdated();
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x001DE6AB File Offset: 0x001DD6AB
		DependencyObject IDataBindEngineClient.TargetElement
		{
			get
			{
				if (base.UsingMentor)
				{
					return Helper.FindMentor(base.TargetElement);
				}
				return base.TargetElement;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x001ED382 File Offset: 0x001EC382
		public MultiBinding ParentMultiBinding
		{
			get
			{
				return (MultiBinding)base.ParentBindingBase;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x001ED38F File Offset: 0x001EC38F
		public ReadOnlyCollection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return new ReadOnlyCollection<BindingExpressionBase>(this.MutableBindingExpressions);
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x001ED39C File Offset: 0x001EC39C
		public override void UpdateSource()
		{
			if (this.MutableBindingExpressions.Count == 0)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			base.NeedsUpdate = true;
			base.Update();
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001ED3C9 File Offset: 0x001EC3C9
		public override void UpdateTarget()
		{
			if (this.MutableBindingExpressions.Count == 0)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			this.UpdateTarget(true);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600399A RID: 14746 RVA: 0x001ED3EF File Offset: 0x001EC3EF
		internal override bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return this.ParentMultiBinding.UpdateSourceTrigger == UpdateSourceTrigger.Default;
			}
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x001ED400 File Offset: 0x001EC400
		internal static MultiBindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, MultiBinding binding, BindingExpressionBase owner)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			MultiBindingExpression multiBindingExpression = new MultiBindingExpression(binding, owner);
			multiBindingExpression.ResolvePropertyDefaultSettings(binding.Mode, binding.UpdateSourceTrigger, frameworkPropertyMetadata);
			return multiBindingExpression;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x001ED470 File Offset: 0x001EC470
		private void AttachToContext(bool lastChance)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			this._converter = this.ParentMultiBinding.Converter;
			if (this._converter == null && string.IsNullOrEmpty(base.EffectiveStringFormat) && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.MultiBindingHasNoConverter, this, new object[]
				{
					this.ParentMultiBinding
				}, null);
			}
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.AttachToContext(new object[]
				{
					TraceData.Identify(this),
					lastChance ? " (last chance)" : string.Empty
				}), this);
			}
			base.TransferIsDeferred = true;
			bool flag2 = true;
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.MutableBindingExpressions[i].StatusInternal == BindingStatusInternal.Unattached)
				{
					flag2 = false;
				}
			}
			if (!flag2 && !lastChance)
			{
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ChildNotAttached(new object[]
					{
						TraceData.Identify(this)
					}), this);
				}
				return;
			}
			if (base.UsesLanguage)
			{
				WeakDependencySource[] commonSources = new WeakDependencySource[]
				{
					new WeakDependencySource(base.TargetElement, FrameworkElement.LanguageProperty)
				};
				WeakDependencySource[] newSources = BindingExpressionBase.CombineSources(-1, this.MutableBindingExpressions, this.MutableBindingExpressions.Count, null, commonSources);
				base.ChangeSources(newSources);
			}
			bool flag3 = base.IsOneWayToSource;
			object newValue;
			if (base.ShouldUpdateWithCurrentValue(targetElement, out newValue))
			{
				flag3 = true;
				base.ChangeValue(newValue, false);
				base.NeedsUpdate = true;
			}
			base.SetStatus(BindingStatusInternal.Active);
			if (!flag3)
			{
				this.UpdateTarget(false);
				return;
			}
			base.UpdateValue();
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x001ED5F0 File Offset: 0x001EC5F0
		public override ValidationError ValidationError
		{
			get
			{
				ValidationError validationError = base.ValidationError;
				if (validationError == null)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						validationError = this.MutableBindingExpressions[i].ValidationError;
						if (validationError != null)
						{
							break;
						}
					}
				}
				return validationError;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600399E RID: 14750 RVA: 0x001ED634 File Offset: 0x001EC634
		public override bool HasError
		{
			get
			{
				bool hasError = base.HasError;
				if (!hasError)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						if (this.MutableBindingExpressions[i].HasError)
						{
							return true;
						}
					}
				}
				return hasError;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x0600399F RID: 14751 RVA: 0x001ED678 File Offset: 0x001EC678
		public override bool HasValidationError
		{
			get
			{
				bool hasValidationError = base.HasValidationError;
				if (!hasValidationError)
				{
					for (int i = 0; i < this.MutableBindingExpressions.Count; i++)
					{
						if (this.MutableBindingExpressions[i].HasValidationError)
						{
							return true;
						}
					}
				}
				return hasValidationError;
			}
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x001ED6BC File Offset: 0x001EC6BC
		internal override bool AttachOverride(DependencyObject d, DependencyProperty dp)
		{
			if (!base.AttachOverride(d, dp))
			{
				return false;
			}
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return false;
			}
			if (base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.AddHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			base.TransferIsDeferred = true;
			int count = this.ParentMultiBinding.Bindings.Count;
			for (int i = 0; i < count; i++)
			{
				this.AttachBindingExpression(i, false);
			}
			this.AttachToContext(false);
			if (base.TransferIsDeferred)
			{
				base.Engine.AddTask(this, TaskOps.AttachToContext);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.DeferAttachToContext(new object[]
					{
						TraceData.Identify(this)
					}), this);
				}
			}
			return true;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x001ED76C File Offset: 0x001EC76C
		internal override void DetachOverride()
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.RemoveHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			for (int i = this.MutableBindingExpressions.Count - 1; i >= 0; i--)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
					bindingExpressionBase.Detach();
					this.MutableBindingExpressions.RemoveAt(i);
				}
			}
			base.ChangeSources(null);
			base.DetachOverride();
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x001ED7E8 File Offset: 0x001EC7E8
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (0 <= num && base.IsDynamic)
			{
				base.NeedsDataTransfer = true;
				this.Transfer();
			}
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001ED81C File Offset: 0x001EC81C
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (num >= 0)
			{
				WeakDependencySource[] commonSources = null;
				if (base.UsesLanguage)
				{
					commonSources = new WeakDependencySource[]
					{
						new WeakDependencySource(base.TargetElement, FrameworkElement.LanguageProperty)
					};
				}
				WeakDependencySource[] newSources2 = BindingExpressionBase.CombineSources(num, this.MutableBindingExpressions, this.MutableBindingExpressions.Count, newSources, commonSources);
				base.ChangeSources(newSources2);
			}
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001ED880 File Offset: 0x001EC880
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (num >= 0 && targetElement != null)
			{
				bindingExpression.Detach();
				this.AttachBindingExpression(num, true);
			}
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x001ED8B8 File Offset: 0x001EC8B8
		internal override void UpdateBindingGroup(BindingGroup bg)
		{
			int i = 0;
			int num = this.MutableBindingExpressions.Count - 1;
			while (i < num)
			{
				this.MutableBindingExpressions[i].UpdateBindingGroup(bg);
				i++;
			}
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001ED8F4 File Offset: 0x001EC8F4
		internal override object ConvertProposedValue(object value)
		{
			object unsetValue;
			if (!this.ConvertProposedValueImpl(value, out unsetValue))
			{
				unsetValue = DependencyProperty.UnsetValue;
				ValidationError validationError = new ValidationError(ConversionValidationRule.Instance, this, SR.Get("Validation_ConversionFailed", new object[]
				{
					value
				}), null);
				base.UpdateValidationError(validationError, false);
			}
			return unsetValue;
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x001ED93C File Offset: 0x001EC93C
		private bool ConvertProposedValueImpl(object value, out object result)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				result = DependencyProperty.UnsetValue;
				return false;
			}
			result = this.GetValuesForChildBindings(value);
			if (base.IsDetached)
			{
				return false;
			}
			if (result == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			object[] array = (object[])result;
			if (array == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.BadMultiConverterForUpdate(new object[]
					{
						this.Converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this, null);
				}
				result = DependencyProperty.UnsetValue;
				return false;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				for (int i = 0; i < array.Length; i++)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UserConvertBackMulti(new object[]
					{
						TraceData.Identify(this),
						i,
						TraceData.Identify(array[i])
					}), this);
				}
			}
			int num = this.MutableBindingExpressions.Count;
			if (array.Length != num && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Information, TraceData.MultiValueConverterMismatch, this, new object[]
				{
					this.Converter.GetType().Name,
					num,
					array.Length,
					TraceData.DescribeTarget(targetElement, base.TargetProperty)
				}, null);
			}
			if (array.Length < num)
			{
				num = array.Length;
			}
			bool result2 = true;
			for (int j = 0; j < num; j++)
			{
				value = array[j];
				if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue)
				{
					BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[j];
					bindingExpressionBase.SetValue(targetElement, base.TargetProperty, value);
					value = bindingExpressionBase.GetRawProposedValue();
					if (!bindingExpressionBase.Validate(value, ValidationStep.RawProposedValue))
					{
						value = DependencyProperty.UnsetValue;
					}
					value = bindingExpressionBase.ConvertProposedValue(value);
				}
				else if (value == DependencyProperty.UnsetValue && TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Information, TraceData.UnsetValueInMultiBindingExpressionUpdate(new object[]
					{
						this.Converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						j,
						this._tempTypes[j]
					}), this, null);
				}
				if (value == DependencyProperty.UnsetValue)
				{
					result2 = false;
				}
				array[j] = value;
			}
			Array.Clear(this._tempTypes, 0, this._tempTypes.Length);
			result = array;
			return result2;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x001EDB74 File Offset: 0x001ECB74
		private object GetValuesForChildBindings(object rawValue)
		{
			if (this.Converter == null)
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.MultiValueConverterMissingForUpdate, this, null);
				}
				return DependencyProperty.UnsetValue;
			}
			CultureInfo culture = base.GetCulture();
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpression bindingExpression = this.MutableBindingExpressions[i] as BindingExpression;
				if (bindingExpression != null && bindingExpression.UseDefaultValueConverter)
				{
					this._tempTypes[i] = bindingExpression.ConverterSourceType;
				}
				else
				{
					this._tempTypes[i] = base.TargetProperty.PropertyType;
				}
			}
			return this.Converter.ConvertBack(rawValue, this._tempTypes, this.ParentMultiBinding.ConverterParameter, culture);
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x001EDC20 File Offset: 0x001ECC20
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object obj = bindingGroup.GetValue(this);
				if (obj != DependencyProperty.UnsetValue)
				{
					obj = this.ConvertProposedValue(obj);
					object[] array;
					if (obj == DependencyProperty.UnsetValue)
					{
						result = false;
					}
					else if ((array = (obj as object[])) != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] == DependencyProperty.UnsetValue)
							{
								result = false;
							}
						}
					}
				}
				this.StoreValueInBindingGroup(obj, bindingGroup);
			}
			else
			{
				bindingGroup.UseSourceValue(this);
			}
			return result;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x001EDC94 File Offset: 0x001ECC94
		internal override object UpdateSource(object convertedValue)
		{
			if (convertedValue == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return convertedValue;
			}
			object[] array = convertedValue as object[];
			int num = this.MutableBindingExpressions.Count;
			if (array.Length < num)
			{
				num = array.Length;
			}
			base.BeginSourceUpdate();
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				if (obj != Binding.DoNothing)
				{
					BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
					bindingExpressionBase.UpdateSource(obj);
					if (bindingExpressionBase.StatusInternal == BindingStatusInternal.UpdateSourceError)
					{
						base.SetStatus(BindingStatusInternal.UpdateSourceError);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				base.IsInUpdate = false;
			}
			base.EndSourceUpdate();
			this.OnSourceUpdated();
			return convertedValue;
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x001EDD30 File Offset: 0x001ECD30
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object value = bindingGroup.GetValue(this);
				this.UpdateSource(value);
				if (value == DependencyProperty.UnsetValue)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001EDD64 File Offset: 0x001ECD64
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			bindingGroup.SetValue(this, value);
			object[] array = value as object[];
			if (array != null)
			{
				int num = this.MutableBindingExpressions.Count;
				if (array.Length < num)
				{
					num = array.Length;
				}
				for (int i = 0; i < num; i++)
				{
					this.MutableBindingExpressions[i].StoreValueInBindingGroup(array[i], bindingGroup);
				}
				return;
			}
			for (int j = this.MutableBindingExpressions.Count - 1; j >= 0; j--)
			{
				this.MutableBindingExpressions[j].StoreValueInBindingGroup(DependencyProperty.UnsetValue, bindingGroup);
			}
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x001EDDEC File Offset: 0x001ECDEC
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			if (value == Binding.DoNothing)
			{
				return true;
			}
			if (value == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			bool result = base.Validate(value, validationStep);
			if (validationStep != ValidationStep.RawProposedValue)
			{
				object[] array = value as object[];
				int num = this.MutableBindingExpressions.Count;
				if (array.Length < num)
				{
					num = array.Length;
				}
				for (int i = 0; i < num; i++)
				{
					value = array[i];
					if (value != DependencyProperty.UnsetValue && value != Binding.DoNothing && !this.MutableBindingExpressions[i].Validate(value, validationStep))
					{
						array[i] = DependencyProperty.UnsetValue;
					}
				}
			}
			return result;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001EDE7C File Offset: 0x001ECE7C
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			if (!base.NeedsValidation)
			{
				return true;
			}
			if (validationStep <= ValidationStep.CommittedValue)
			{
				object value = bindingGroup.GetValue(this);
				bool flag = this.Validate(value, validationStep);
				if (flag && validationStep == ValidationStep.CommittedValue)
				{
					base.NeedsValidation = false;
				}
				return flag;
			}
			throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
			{
				validationStep,
				bindingGroup
			}));
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x001EDEDC File Offset: 0x001ECEDC
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			values = null;
			object rawProposedValue = this.GetRawProposedValue();
			if (!this.Validate(rawProposedValue, ValidationStep.RawProposedValue))
			{
				return false;
			}
			object valuesForChildBindings = this.GetValuesForChildBindings(rawProposedValue);
			if (base.IsDetached || valuesForChildBindings == DependencyProperty.UnsetValue || valuesForChildBindings == null)
			{
				return false;
			}
			int num = this.MutableBindingExpressions.Count;
			object[] array = (object[])valuesForChildBindings;
			if (array.Length < num)
			{
				num = array.Length;
			}
			values = new Collection<BindingExpressionBase.ProposedValue>();
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				object obj = array[i];
				if (obj != Binding.DoNothing)
				{
					if (obj == DependencyProperty.UnsetValue)
					{
						flag = false;
					}
					else
					{
						BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
						bindingExpressionBase.Value = obj;
						if (bindingExpressionBase.NeedsValidation)
						{
							Collection<BindingExpressionBase.ProposedValue> collection;
							bool flag2 = bindingExpressionBase.ValidateAndConvertProposedValue(out collection);
							if (collection != null)
							{
								int j = 0;
								int count = collection.Count;
								while (j < count)
								{
									values.Add(collection[j]);
									j++;
								}
							}
							flag = (flag && flag2);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x001EDFDC File Offset: 0x001ECFDC
		internal override object GetSourceItem(object newValue)
		{
			if (newValue == null)
			{
				return null;
			}
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				if (ItemsControl.EqualsEx(this.MutableBindingExpressions[i].GetValue(null, null), newValue))
				{
					return this.MutableBindingExpressions[i].GetSourceItem(newValue);
				}
			}
			return null;
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060039B1 RID: 14769 RVA: 0x001EE035 File Offset: 0x001ED035
		private Collection<BindingExpressionBase> MutableBindingExpressions
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060039B2 RID: 14770 RVA: 0x001EE03D File Offset: 0x001ED03D
		// (set) Token: 0x060039B3 RID: 14771 RVA: 0x001EE045 File Offset: 0x001ED045
		private IMultiValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				this._converter = value;
			}
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x001EE050 File Offset: 0x001ED050
		private BindingExpressionBase AttachBindingExpression(int i, bool replaceExisting)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return null;
			}
			BindingBase bindingBase = this.ParentMultiBinding.Bindings[i];
			MultiBinding.CheckTrigger(bindingBase);
			BindingExpressionBase bindingExpressionBase = bindingBase.CreateBindingExpression(targetElement, base.TargetProperty, this);
			if (replaceExisting)
			{
				this.MutableBindingExpressions[i] = bindingExpressionBase;
			}
			else
			{
				this.MutableBindingExpressions.Add(bindingExpressionBase);
			}
			bindingExpressionBase.Attach(targetElement, base.TargetProperty);
			return bindingExpressionBase;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x001EE0BC File Offset: 0x001ED0BC
		internal override void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.GotPropertyChanged(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(d),
					property.Name
				}), null);
			}
			bool flag = true;
			base.TransferIsDeferred = true;
			if (base.UsesLanguage && d == base.TargetElement && property == FrameworkElement.LanguageProperty)
			{
				base.InvalidateCulture();
				base.NeedsDataTransfer = true;
			}
			if (base.IsDetached)
			{
				return;
			}
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
					DependencySource[] sources = bindingExpressionBase.GetSources();
					if (sources != null)
					{
						foreach (DependencySource dependencySource in sources)
						{
							if (dependencySource.DependencyObject == d && dependencySource.DependencyProperty == property)
							{
								bindingExpressionBase.OnPropertyInvalidation(d, args);
								break;
							}
						}
					}
					if (bindingExpressionBase.IsDisconnected)
					{
						flag = false;
					}
				}
			}
			base.TransferIsDeferred = false;
			if (flag)
			{
				this.Transfer();
				return;
			}
			this.Disconnect();
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00105F35 File Offset: 0x00104F35
		internal override bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x001EE1D1 File Offset: 0x001ED1D1
		internal override void OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"LostFocus",
					TraceData.Identify(sender)
				}), null);
			}
			base.Update();
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x001EE210 File Offset: 0x001ED210
		private void UpdateTarget(bool includeInnerBindings)
		{
			base.TransferIsDeferred = true;
			if (includeInnerBindings)
			{
				foreach (BindingExpressionBase bindingExpressionBase in this.MutableBindingExpressions)
				{
					bindingExpressionBase.UpdateTarget();
				}
			}
			base.TransferIsDeferred = false;
			base.NeedsDataTransfer = true;
			this.Transfer();
			base.NeedsUpdate = false;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x001EE280 File Offset: 0x001ED280
		private void Transfer()
		{
			if (base.NeedsDataTransfer && base.StatusInternal != BindingStatusInternal.Unattached && !base.TransferIsDeferred)
			{
				this.TransferValue();
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001EE2A0 File Offset: 0x001ED2A0
		private void TransferValue()
		{
			base.IsInTransfer = true;
			base.NeedsDataTransfer = false;
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null)
			{
				bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
				object obj = DependencyProperty.UnsetValue;
				object obj2 = this._tempValues;
				CultureInfo culture = base.GetCulture();
				int count = this.MutableBindingExpressions.Count;
				for (int i = 0; i < count; i++)
				{
					this._tempValues[i] = this.MutableBindingExpressions[i].GetValue(targetElement, base.TargetProperty);
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetRawValueMulti(new object[]
						{
							TraceData.Identify(this),
							i,
							TraceData.Identify(this._tempValues[i])
						}), this);
					}
				}
				if (this.Converter != null)
				{
					obj2 = this.Converter.Convert(this._tempValues, base.TargetProperty.PropertyType, this.ParentMultiBinding.ConverterParameter, culture);
					if (base.IsDetached)
					{
						return;
					}
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UserConverter(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj2)
						}), this);
					}
				}
				else if (base.EffectiveStringFormat != null)
				{
					for (int j = 0; j < this._tempValues.Length; j++)
					{
						if (this._tempValues[j] == DependencyProperty.UnsetValue)
						{
							obj2 = DependencyProperty.UnsetValue;
							break;
						}
					}
				}
				else
				{
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.MultiValueConverterMissingForTransfer, this, null);
						goto IL_37A;
					}
					goto IL_37A;
				}
				if (base.EffectiveStringFormat == null || obj2 == Binding.DoNothing || obj2 == DependencyProperty.UnsetValue)
				{
					obj = obj2;
				}
				else
				{
					try
					{
						if (obj2 == this._tempValues)
						{
							obj = string.Format(culture, base.EffectiveStringFormat, this._tempValues);
						}
						else
						{
							obj = string.Format(culture, base.EffectiveStringFormat, obj2);
						}
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.FormattedValue(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
					}
					catch (FormatException)
					{
						obj = DependencyProperty.UnsetValue;
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.FormattingFailed(new object[]
							{
								TraceData.Identify(this),
								base.EffectiveStringFormat
							}), this);
						}
					}
				}
				Array.Clear(this._tempValues, 0, this._tempValues.Length);
				if (obj != Binding.DoNothing)
				{
					if (base.EffectiveTargetNullValue != DependencyProperty.UnsetValue && BindingExpressionBase.IsNullValue(obj))
					{
						obj = base.EffectiveTargetNullValue;
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.NullConverter(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
					}
					if (obj != DependencyProperty.UnsetValue && !base.TargetProperty.IsValidValue(obj))
					{
						if (TraceData.IsEnabled)
						{
							TraceData.TraceAndNotify(base.TraceLevel, TraceData.BadValueAtTransfer, this, new object[]
							{
								obj,
								this
							}, null);
						}
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.BadValueAtTransferExtended(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
						obj = DependencyProperty.UnsetValue;
					}
					if (obj == DependencyProperty.UnsetValue)
					{
						obj = base.UseFallbackValue();
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UseFallback(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
					}
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.TransferValue(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}), this);
					}
					bool flag2 = !base.IsInUpdate || !ItemsControl.EqualsEx(obj, base.Value);
					if (flag2)
					{
						base.ChangeValue(obj, true);
						base.Invalidate(false);
						Validation.ClearInvalid(this);
					}
					base.Clean();
					if (flag2)
					{
						this.OnTargetUpdated();
					}
				}
			}
			IL_37A:
			base.IsInTransfer = false;
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x001EE640 File Offset: 0x001ED640
		private void OnTargetUpdated()
		{
			if (base.NotifyOnTargetUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					if (base.IsAttaching && this == targetElement.ReadLocalValue(base.TargetProperty))
					{
						base.Engine.AddTask(this, TaskOps.RaiseTargetUpdatedEvent);
						return;
					}
					BindingExpression.OnTargetUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x001EE690 File Offset: 0x001ED690
		private void OnSourceUpdated()
		{
			if (base.NotifyOnSourceUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					BindingExpression.OnSourceUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x001EE6BC File Offset: 0x001ED6BC
		internal override bool ShouldReactToDirtyOverride()
		{
			using (IEnumerator<BindingExpressionBase> enumerator = this.MutableBindingExpressions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.ShouldReactToDirtyOverride())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x001EE710 File Offset: 0x001ED710
		internal override bool UpdateOverride()
		{
			return !base.NeedsUpdate || !base.IsReflective || base.IsInTransfer || base.StatusInternal == BindingStatusInternal.Unattached || base.UpdateValue();
		}

		// Token: 0x04001D6D RID: 7533
		private Collection<BindingExpressionBase> _list = new Collection<BindingExpressionBase>();

		// Token: 0x04001D6E RID: 7534
		private IMultiValueConverter _converter;

		// Token: 0x04001D6F RID: 7535
		private object[] _tempValues;

		// Token: 0x04001D70 RID: 7536
		private Type[] _tempTypes;
	}
}
