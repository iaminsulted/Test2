using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x02000466 RID: 1126
	public sealed class PriorityBindingExpression : BindingExpressionBase
	{
		// Token: 0x060039E1 RID: 14817 RVA: 0x001EEFDB File Offset: 0x001EDFDB
		private PriorityBindingExpression(PriorityBinding binding, BindingExpressionBase owner) : base(binding, owner)
		{
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x001EEFF8 File Offset: 0x001EDFF8
		public PriorityBinding ParentPriorityBinding
		{
			get
			{
				return (PriorityBinding)base.ParentBindingBase;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x060039E3 RID: 14819 RVA: 0x001EF005 File Offset: 0x001EE005
		public ReadOnlyCollection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return new ReadOnlyCollection<BindingExpressionBase>(this.MutableBindingExpressions);
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x001EF012 File Offset: 0x001EE012
		public BindingExpressionBase ActiveBindingExpression
		{
			get
			{
				if (this._activeIndex >= 0)
				{
					return this.MutableBindingExpressions[this._activeIndex];
				}
				return null;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x060039E5 RID: 14821 RVA: 0x001EF030 File Offset: 0x001EE030
		public override bool HasValidationError
		{
			get
			{
				return this._activeIndex >= 0 && this.MutableBindingExpressions[this._activeIndex].HasValidationError;
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x001EF054 File Offset: 0x001EE054
		public override void UpdateTarget()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.UpdateTarget();
			}
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x001EF074 File Offset: 0x001EE074
		public override void UpdateSource()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.UpdateSource();
			}
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x001EF094 File Offset: 0x001EE094
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			bool flag;
			if (activeBindingExpression != null)
			{
				flag = activeBindingExpression.SetValue(d, dp, value);
				if (flag)
				{
					base.Value = activeBindingExpression.Value;
					base.AdoptProperties(activeBindingExpression);
					base.NotifyCommitManager();
				}
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x001EF0D8 File Offset: 0x001EE0D8
		internal static PriorityBindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, PriorityBinding binding, BindingExpressionBase owner)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			return new PriorityBindingExpression(binding, owner);
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x001EF135 File Offset: 0x001EE135
		internal int AttentiveBindingExpressions
		{
			get
			{
				if (this._activeIndex != -1)
				{
					return this._activeIndex + 1;
				}
				return this.MutableBindingExpressions.Count;
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x001EF154 File Offset: 0x001EE154
		internal override bool AttachOverride(DependencyObject d, DependencyProperty dp)
		{
			if (!base.AttachOverride(d, dp))
			{
				return false;
			}
			if (base.TargetElement == null)
			{
				return false;
			}
			base.SetStatus(BindingStatusInternal.Active);
			int count = this.ParentPriorityBinding.Bindings.Count;
			this._activeIndex = -1;
			for (int i = 0; i < count; i++)
			{
				this.AttachBindingExpression(i, false);
			}
			return true;
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x001EF1AC File Offset: 0x001EE1AC
		internal override void DetachOverride()
		{
			int count = this.MutableBindingExpressions.Count;
			for (int i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase != null)
				{
					bindingExpressionBase.Detach();
				}
			}
			base.ChangeSources(null);
			base.DetachOverride();
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x001EF1F4 File Offset: 0x001EE1F4
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
			if (this._isInInvalidateBinding)
			{
				return;
			}
			this._isInInvalidateBinding = true;
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && 0 <= num && num < this.AttentiveBindingExpressions)
			{
				if (num != this._activeIndex || (bindingExpression.StatusInternal != BindingStatusInternal.Active && !bindingExpression.UsingFallbackValue))
				{
					this.ChooseActiveBindingExpression(targetElement);
				}
				base.UsingFallbackValue = false;
				BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
				object obj = (activeBindingExpression != null) ? activeBindingExpression.GetValue(targetElement, base.TargetProperty) : base.UseFallbackValue();
				base.ChangeValue(obj, true);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.PriorityTransfer(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(obj),
						this._activeIndex,
						TraceData.Identify(activeBindingExpression)
					}), this);
				}
				if (!base.IsAttaching)
				{
					targetElement.InvalidateProperty(base.TargetProperty);
				}
			}
			this._isInInvalidateBinding = false;
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x001EF2F0 File Offset: 0x001EE2F0
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			if (num >= 0)
			{
				WeakDependencySource[] newSources2 = BindingExpressionBase.CombineSources(num, this.MutableBindingExpressions, this.AttentiveBindingExpressions, newSources, null);
				base.ChangeSources(newSources2);
			}
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x001EF32C File Offset: 0x001EE32C
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
			int num = this.MutableBindingExpressions.IndexOf(bindingExpression);
			DependencyObject targetElement = base.TargetElement;
			if (num >= 0 && targetElement != null)
			{
				bindingExpression.Detach();
				bindingExpression = this.AttachBindingExpression(num, true);
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x001EF364 File Offset: 0x001EE364
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

		// Token: 0x060039F1 RID: 14833 RVA: 0x001EF3A0 File Offset: 0x001EE3A0
		internal override bool ShouldReactToDirtyOverride()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression != null && activeBindingExpression.ShouldReactToDirtyOverride();
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x001EF3C0 File Offset: 0x001EE3C0
		internal override object GetRawProposedValue()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.GetRawProposedValue();
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x001EF3E4 File Offset: 0x001EE3E4
		internal override object ConvertProposedValue(object rawValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.ConvertProposedValue(rawValue);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x001EF408 File Offset: 0x001EE408
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.ObtainConvertedProposedValue(bindingGroup);
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x001EF428 File Offset: 0x001EE428
		internal override object UpdateSource(object convertedValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			object result;
			if (activeBindingExpression != null)
			{
				result = activeBindingExpression.UpdateSource(convertedValue);
				if (activeBindingExpression.StatusInternal == BindingStatusInternal.UpdateSourceError)
				{
					base.SetStatus(BindingStatusInternal.UpdateSourceError);
				}
			}
			else
			{
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x001EF460 File Offset: 0x001EE460
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				result = activeBindingExpression.UpdateSource(bindingGroup);
				if (activeBindingExpression.StatusInternal == BindingStatusInternal.UpdateSourceError)
				{
					base.SetStatus(BindingStatusInternal.UpdateSourceError);
				}
			}
			return result;
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x001EF494 File Offset: 0x001EE494
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				activeBindingExpression.StoreValueInBindingGroup(value, bindingGroup);
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x001EF4B4 File Offset: 0x001EE4B4
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.Validate(value, validationStep);
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x001EF4D8 File Offset: 0x001EE4D8
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			return activeBindingExpression == null || activeBindingExpression.CheckValidationRules(bindingGroup, validationStep);
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x001EF4FC File Offset: 0x001EE4FC
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.ValidateAndConvertProposedValue(out values);
			}
			values = null;
			return true;
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x001EF520 File Offset: 0x001EE520
		internal override object GetSourceItem(object newValue)
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				return activeBindingExpression.GetSourceItem(newValue);
			}
			return true;
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x001EF548 File Offset: 0x001EE548
		internal override void UpdateCommitState()
		{
			BindingExpressionBase activeBindingExpression = this.ActiveBindingExpression;
			if (activeBindingExpression != null)
			{
				base.AdoptProperties(activeBindingExpression);
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x060039FD RID: 14845 RVA: 0x001EF566 File Offset: 0x001EE566
		private Collection<BindingExpressionBase> MutableBindingExpressions
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x001EF570 File Offset: 0x001EE570
		private BindingExpressionBase AttachBindingExpression(int i, bool replaceExisting)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return null;
			}
			BindingExpressionBase bindingExpressionBase = this.ParentPriorityBinding.Bindings[i].CreateBindingExpression(targetElement, base.TargetProperty, this);
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

		// Token: 0x060039FF RID: 14847 RVA: 0x001EF5D4 File Offset: 0x001EE5D4
		private void ChooseActiveBindingExpression(DependencyObject target)
		{
			int count = this.MutableBindingExpressions.Count;
			int i;
			for (i = 0; i < count; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
				if (bindingExpressionBase.StatusInternal == BindingStatusInternal.Inactive)
				{
					bindingExpressionBase.Activate();
				}
				if (bindingExpressionBase.StatusInternal == BindingStatusInternal.Active || bindingExpressionBase.UsingFallbackValue)
				{
					break;
				}
			}
			int num = (i < count) ? i : -1;
			if (num != this._activeIndex)
			{
				int activeIndex = this._activeIndex;
				this._activeIndex = num;
				base.AdoptProperties(this.ActiveBindingExpression);
				WeakDependencySource[] newSources = BindingExpressionBase.CombineSources(-1, this.MutableBindingExpressions, this.AttentiveBindingExpressions, null, null);
				base.ChangeSources(newSources);
				if (num != -1)
				{
					for (i = activeIndex; i > num; i--)
					{
						this.MutableBindingExpressions[i].Deactivate();
					}
				}
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		private void ChangeValue()
		{
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x001EF694 File Offset: 0x001EE694
		internal override void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotPropertyChanged(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(d),
					property.Name
				}), this);
			}
			for (int i = 0; i < this.AttentiveBindingExpressions; i++)
			{
				BindingExpressionBase bindingExpressionBase = this.MutableBindingExpressions[i];
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
			}
		}

		// Token: 0x04001D81 RID: 7553
		private const int NoActiveBindingExpressions = -1;

		// Token: 0x04001D82 RID: 7554
		private const int UnknownActiveBindingExpression = -2;

		// Token: 0x04001D83 RID: 7555
		private Collection<BindingExpressionBase> _list = new Collection<BindingExpressionBase>();

		// Token: 0x04001D84 RID: 7556
		private int _activeIndex = -2;

		// Token: 0x04001D85 RID: 7557
		private bool _isInInvalidateBinding;
	}
}
