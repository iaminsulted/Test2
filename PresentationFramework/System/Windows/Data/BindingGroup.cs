using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x0200044F RID: 1103
	public class BindingGroup : DependencyObject
	{
		// Token: 0x060036E2 RID: 14050 RVA: 0x001E2E64 File Offset: 0x001E1E64
		public BindingGroup()
		{
			this._validationRules = new ValidationRuleCollection();
			this.Initialize();
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x001E2EC4 File Offset: 0x001E1EC4
		internal BindingGroup(BindingGroup master)
		{
			this._validationRules = master._validationRules;
			this._name = master._name;
			this._notifyOnValidationError = master._notifyOnValidationError;
			this._sharesProposedValues = master._sharesProposedValues;
			this._validatesOnNotifyDataError = master._validatesOnNotifyDataError;
			this.Initialize();
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x001E2F54 File Offset: 0x001E1F54
		private void Initialize()
		{
			this._engine = DataBindEngine.CurrentDataBindEngine;
			this._bindingExpressions = new BindingGroup.BindingExpressionCollection();
			((INotifyCollectionChanged)this._bindingExpressions).CollectionChanged += this.OnBindingsChanged;
			this._itemsRW = new Collection<WeakReference>();
			this._items = new WeakReadOnlyCollection<object>(this._itemsRW);
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060036E5 RID: 14053 RVA: 0x001E2FAA File Offset: 0x001E1FAA
		public DependencyObject Owner
		{
			get
			{
				return this.InheritanceContext;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060036E6 RID: 14054 RVA: 0x001E2FB2 File Offset: 0x001E1FB2
		public Collection<ValidationRule> ValidationRules
		{
			get
			{
				return this._validationRules;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060036E7 RID: 14055 RVA: 0x001E2FBA File Offset: 0x001E1FBA
		public Collection<BindingExpressionBase> BindingExpressions
		{
			get
			{
				return this._bindingExpressions;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060036E8 RID: 14056 RVA: 0x001E2FC2 File Offset: 0x001E1FC2
		// (set) Token: 0x060036E9 RID: 14057 RVA: 0x001E2FCA File Offset: 0x001E1FCA
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060036EA RID: 14058 RVA: 0x001E2FD3 File Offset: 0x001E1FD3
		// (set) Token: 0x060036EB RID: 14059 RVA: 0x001E2FDB File Offset: 0x001E1FDB
		public bool NotifyOnValidationError
		{
			get
			{
				return this._notifyOnValidationError;
			}
			set
			{
				this._notifyOnValidationError = value;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060036EC RID: 14060 RVA: 0x001E2FE4 File Offset: 0x001E1FE4
		// (set) Token: 0x060036ED RID: 14061 RVA: 0x001E2FEC File Offset: 0x001E1FEC
		public bool ValidatesOnNotifyDataError
		{
			get
			{
				return this._validatesOnNotifyDataError;
			}
			set
			{
				this._validatesOnNotifyDataError = value;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060036EE RID: 14062 RVA: 0x001E2FF5 File Offset: 0x001E1FF5
		// (set) Token: 0x060036EF RID: 14063 RVA: 0x001E2FFD File Offset: 0x001E1FFD
		public bool SharesProposedValues
		{
			get
			{
				return this._sharesProposedValues;
			}
			set
			{
				if (this._sharesProposedValues != value)
				{
					this._proposedValueTable.Clear();
					this._sharesProposedValues = value;
				}
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x060036F0 RID: 14064 RVA: 0x001E301C File Offset: 0x001E201C
		public bool CanRestoreValues
		{
			get
			{
				IList items = this.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					if (!(items[i] is IEditableObject))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x001E3054 File Offset: 0x001E2054
		public IList Items
		{
			get
			{
				this.EnsureItems();
				return this._items;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x001E3064 File Offset: 0x001E2064
		public bool IsDirty
		{
			get
			{
				if (this._proposedValueTable.Count > 0)
				{
					return true;
				}
				using (IEnumerator<BindingExpressionBase> enumerator = this._bindingExpressions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsDirty)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060036F3 RID: 14067 RVA: 0x001E30C8 File Offset: 0x001E20C8
		public bool HasValidationError
		{
			get
			{
				ValidationErrorCollection validationErrorCollection;
				bool flag;
				return this.GetValidationErrors(out validationErrorCollection, out flag);
			}
		}

		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x001E30E0 File Offset: 0x001E20E0
		public ReadOnlyCollection<ValidationError> ValidationErrors
		{
			get
			{
				ValidationErrorCollection validationErrorCollection;
				bool flag;
				if (!this.GetValidationErrors(out validationErrorCollection, out flag))
				{
					return null;
				}
				if (flag)
				{
					return new ReadOnlyCollection<ValidationError>(validationErrorCollection);
				}
				List<ValidationError> list = new List<ValidationError>();
				foreach (ValidationError validationError in validationErrorCollection)
				{
					if (this.Belongs(validationError))
					{
						list.Add(validationError);
					}
				}
				return new ReadOnlyCollection<ValidationError>(list);
			}
		}

		// Token: 0x060036F5 RID: 14069 RVA: 0x001E3158 File Offset: 0x001E2158
		private bool GetValidationErrors(out ValidationErrorCollection superset, out bool isPure)
		{
			superset = null;
			isPure = true;
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return false;
			}
			superset = Validation.GetErrorsInternal(dependencyObject);
			if (superset == null || superset.Count == 0)
			{
				return false;
			}
			for (int i = superset.Count - 1; i >= 0; i--)
			{
				ValidationError error = superset[i];
				if (!this.Belongs(error))
				{
					isPure = false;
					break;
				}
			}
			return true;
		}

		// Token: 0x060036F6 RID: 14070 RVA: 0x001E31BC File Offset: 0x001E21BC
		private bool Belongs(ValidationError error)
		{
			BindingExpressionBase bindingExpressionBase;
			return error.BindingInError == this || this._proposedValueTable.HasValidationError(error) || ((bindingExpressionBase = (error.BindingInError as BindingExpressionBase)) != null && bindingExpressionBase.BindingGroup == this);
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060036F7 RID: 14071 RVA: 0x001E31FC File Offset: 0x001E21FC
		private DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x001E3204 File Offset: 0x001E2204
		public void BeginEdit()
		{
			if (!this.IsEditing)
			{
				IList items = this.Items;
				for (int i = items.Count - 1; i >= 0; i--)
				{
					IEditableObject editableObject = items[i] as IEditableObject;
					if (editableObject != null)
					{
						editableObject.BeginEdit();
					}
				}
				this.IsEditing = true;
			}
		}

		// Token: 0x060036F9 RID: 14073 RVA: 0x001E3250 File Offset: 0x001E2250
		public bool CommitEdit()
		{
			bool flag = this.UpdateAndValidate(ValidationStep.CommittedValue);
			this.IsEditing = (this.IsEditing && !flag);
			return flag;
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x001E327C File Offset: 0x001E227C
		public void CancelEdit()
		{
			this.ClearValidationErrors();
			IList items = this.Items;
			for (int i = items.Count - 1; i >= 0; i--)
			{
				IEditableObject editableObject = items[i] as IEditableObject;
				if (editableObject != null)
				{
					editableObject.CancelEdit();
				}
			}
			for (int j = this._bindingExpressions.Count - 1; j >= 0; j--)
			{
				this._bindingExpressions[j].UpdateTarget();
			}
			this._proposedValueTable.UpdateDependents();
			this._proposedValueTable.Clear();
			this.IsEditing = false;
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x001E3305 File Offset: 0x001E2305
		public bool ValidateWithoutUpdate()
		{
			return this.UpdateAndValidate(ValidationStep.ConvertedProposedValue);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x001E330E File Offset: 0x001E230E
		public bool UpdateSources()
		{
			return this.UpdateAndValidate(ValidationStep.UpdatedValue);
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x001E3318 File Offset: 0x001E2318
		public object GetValue(object item, string propertyName)
		{
			object obj;
			if (this.TryGetValueImpl(item, propertyName, out obj))
			{
				return obj;
			}
			if (obj == Binding.DoNothing)
			{
				throw new ValueUnavailableException(SR.Get("BindingGroup_NoEntry", new object[]
				{
					item,
					propertyName
				}));
			}
			throw new ValueUnavailableException(SR.Get("BindingGroup_ValueUnavailable", new object[]
			{
				item,
				propertyName
			}));
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x001E3376 File Offset: 0x001E2376
		public bool TryGetValue(object item, string propertyName, out object value)
		{
			bool result = this.TryGetValueImpl(item, propertyName, out value);
			if (value == Binding.DoNothing)
			{
				value = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x001E3394 File Offset: 0x001E2394
		private bool TryGetValueImpl(object item, string propertyName, out object value)
		{
			BindingGroup.GetValueTableEntry getValueTableEntry = this._getValueTable[item, propertyName];
			ValidationStep validationStep;
			if (getValueTableEntry == null)
			{
				BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[item, propertyName];
				if (proposedValueEntry != null)
				{
					validationStep = this._validationStep;
					if (validationStep == ValidationStep.RawProposedValue)
					{
						value = proposedValueEntry.RawValue;
						return true;
					}
					if (validationStep - ValidationStep.ConvertedProposedValue <= 2)
					{
						value = proposedValueEntry.ConvertedValue;
						return value != DependencyProperty.UnsetValue;
					}
				}
				value = Binding.DoNothing;
				return false;
			}
			validationStep = this._validationStep;
			if (validationStep <= ValidationStep.CommittedValue)
			{
				value = getValueTableEntry.Value;
			}
			else
			{
				value = getValueTableEntry.BindingExpressionBase.RootBindingExpression.GetRawProposedValue();
			}
			if (value == Binding.DoNothing)
			{
				BindingExpression bindingExpression = (BindingExpression)getValueTableEntry.BindingExpressionBase;
				value = bindingExpression.SourceValue;
			}
			return value != DependencyProperty.UnsetValue;
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x001E3450 File Offset: 0x001E2450
		internal override DependencyObject InheritanceContext
		{
			get
			{
				DependencyObject dependencyObject;
				if (!this._inheritanceContext.TryGetTarget(out dependencyObject))
				{
					this.CheckDetach(dependencyObject);
				}
				return dependencyObject;
			}
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x001E3474 File Offset: 0x001E2474
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (property != null && property.PropertyType != typeof(BindingGroup) && TraceData.IsEnabled)
			{
				string text = (property != null) ? property.Name : "(null)";
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.BindingGroupWrongProperty(new object[]
				{
					text,
					context.GetType().FullName
				}), null);
			}
			DependencyObject dependencyObject;
			this._inheritanceContext.TryGetTarget(out dependencyObject);
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref dependencyObject);
			this.CheckDetach(dependencyObject);
			this._inheritanceContext = ((dependencyObject == null) ? BindingGroup.NullInheritanceContext : new WeakReference<DependencyObject>(dependencyObject));
			if (property == FrameworkElement.BindingGroupProperty && !this._hasMultipleInheritanceContexts && (this.ValidatesOnDataTransfer || this.ValidatesOnNotifyDataError))
			{
				UIElement uielement = Helper.FindMentor(this) as UIElement;
				if (uielement != null)
				{
					uielement.LayoutUpdated += this.OnLayoutUpdated;
				}
			}
			if (this._hasMultipleInheritanceContexts && property != ItemsControl.ItemBindingGroupProperty && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.BindingGroupMultipleInheritance, null);
			}
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x001E3578 File Offset: 0x001E2578
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			DependencyObject dependencyObject;
			this._inheritanceContext.TryGetTarget(out dependencyObject);
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref dependencyObject);
			this.CheckDetach(dependencyObject);
			this._inheritanceContext = ((dependencyObject == null) ? BindingGroup.NullInheritanceContext : new WeakReference<DependencyObject>(dependencyObject));
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x001E35BF File Offset: 0x001E25BF
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x001E35C7 File Offset: 0x001E25C7
		private void CheckDetach(DependencyObject newOwner)
		{
			if (newOwner != null || this._inheritanceContext == BindingGroup.NullInheritanceContext)
			{
				return;
			}
			this.Engine.CommitManager.RemoveBindingGroup(this);
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06003705 RID: 14085 RVA: 0x001E35EB File Offset: 0x001E25EB
		// (set) Token: 0x06003706 RID: 14086 RVA: 0x001E35F3 File Offset: 0x001E25F3
		private bool IsEditing { get; set; }

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x001E35FC File Offset: 0x001E25FC
		// (set) Token: 0x06003708 RID: 14088 RVA: 0x001E3604 File Offset: 0x001E2604
		private bool IsItemsValid
		{
			get
			{
				return this._isItemsValid;
			}
			set
			{
				this._isItemsValid = value;
				if (!value && (this.IsEditing || this.ValidatesOnNotifyDataError))
				{
					this.EnsureItems();
				}
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x001E3626 File Offset: 0x001E2626
		internal void UpdateTable(BindingExpression bindingExpression)
		{
			if (this._getValueTable.Update(bindingExpression))
			{
				this._proposedValueTable.Remove(bindingExpression);
			}
			this.IsItemsValid = false;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x001E3649 File Offset: 0x001E2649
		internal void AddToValueTable(BindingExpressionBase bindingExpressionBase)
		{
			this._getValueTable.EnsureEntry(bindingExpressionBase);
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x001E3657 File Offset: 0x001E2657
		internal object GetValue(BindingExpressionBase bindingExpressionBase)
		{
			return this._getValueTable.GetValue(bindingExpressionBase);
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x001E3665 File Offset: 0x001E2665
		internal void SetValue(BindingExpressionBase bindingExpressionBase, object value)
		{
			this._getValueTable.SetValue(bindingExpressionBase, value);
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x001E3674 File Offset: 0x001E2674
		internal void UseSourceValue(BindingExpressionBase bindingExpressionBase)
		{
			this._getValueTable.UseSourceValue(bindingExpressionBase);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x001E3682 File Offset: 0x001E2682
		internal BindingGroup.ProposedValueEntry GetProposedValueEntry(object item, string propertyName)
		{
			return this._proposedValueTable[item, propertyName];
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x001E3691 File Offset: 0x001E2691
		internal void RemoveProposedValueEntry(BindingGroup.ProposedValueEntry entry)
		{
			this._proposedValueTable.Remove(entry);
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x001E36A0 File Offset: 0x001E26A0
		internal void AddBindingForProposedValue(BindingExpressionBase dependent, object item, string propertyName)
		{
			BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[item, propertyName];
			if (proposedValueEntry != null)
			{
				proposedValueEntry.AddDependent(dependent);
			}
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x001E36C8 File Offset: 0x001E26C8
		internal void AddValidationError(ValidationError validationError)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			Validation.AddValidationError(validationError, dependencyObject, this.NotifyOnValidationError);
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x001E36F0 File Offset: 0x001E26F0
		internal void RemoveValidationError(ValidationError validationError)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			Validation.RemoveValidationError(validationError, dependencyObject, this.NotifyOnValidationError);
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x001E3715 File Offset: 0x001E2715
		private void ClearValidationErrors(ValidationStep validationStep)
		{
			this.ClearValidationErrorsImpl(validationStep, false);
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x001E371F File Offset: 0x001E271F
		private void ClearValidationErrors()
		{
			this.ClearValidationErrorsImpl(ValidationStep.RawProposedValue, true);
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x001E372C File Offset: 0x001E272C
		private void ClearValidationErrorsImpl(ValidationStep validationStep, bool allSteps)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null)
			{
				return;
			}
			ValidationErrorCollection errorsInternal = Validation.GetErrorsInternal(dependencyObject);
			if (errorsInternal == null)
			{
				return;
			}
			for (int i = errorsInternal.Count - 1; i >= 0; i--)
			{
				ValidationError validationError = errorsInternal[i];
				if ((allSteps || validationError.RuleInError.ValidationStep == validationStep) && (validationError.BindingInError == this || this._proposedValueTable.HasValidationError(validationError)))
				{
					this.RemoveValidationError(validationError);
				}
			}
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x001E379C File Offset: 0x001E279C
		private void EnsureItems()
		{
			if (this.IsItemsValid)
			{
				return;
			}
			IList<WeakReference> list = new Collection<WeakReference>();
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject != null)
			{
				object value = dependencyObject.GetValue(FrameworkElement.DataContextProperty);
				if (value != null && value != CollectionView.NewItemPlaceholder && value != BindingExpressionBase.DisconnectedItem)
				{
					WeakReference weakReference = (this._itemsRW.Count > 0) ? this._itemsRW[0] : null;
					if (weakReference == null || !ItemsControl.EqualsEx(value, weakReference.Target))
					{
						weakReference = new WeakReference(value);
					}
					list.Add(weakReference);
				}
			}
			this._getValueTable.AddUniqueItems(list);
			this._proposedValueTable.AddUniqueItems(list);
			for (int i = this._itemsRW.Count - 1; i >= 0; i--)
			{
				int num = BindingGroup.FindIndexOf(this._itemsRW[i], list);
				if (num >= 0)
				{
					list.RemoveAt(num);
				}
				else
				{
					if (this.ValidatesOnNotifyDataError)
					{
						INotifyDataErrorInfo notifyDataErrorInfo = this._itemsRW[i].Target as INotifyDataErrorInfo;
						if (notifyDataErrorInfo != null)
						{
							ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						}
					}
					this._itemsRW.RemoveAt(i);
				}
			}
			for (int j = list.Count - 1; j >= 0; j--)
			{
				this._itemsRW.Add(list[j]);
				if (this.IsEditing)
				{
					IEditableObject editableObject = list[j].Target as IEditableObject;
					if (editableObject != null)
					{
						editableObject.BeginEdit();
					}
				}
				if (this.ValidatesOnNotifyDataError)
				{
					INotifyDataErrorInfo notifyDataErrorInfo = list[j].Target as INotifyDataErrorInfo;
					if (notifyDataErrorInfo != null)
					{
						ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
						this.UpdateNotifyDataErrors(notifyDataErrorInfo, list[j]);
					}
				}
			}
			this.IsItemsValid = true;
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x001E3958 File Offset: 0x001E2958
		private bool ValidatesOnDataTransfer
		{
			get
			{
				if (this.ValidationRules != null)
				{
					for (int i = this.ValidationRules.Count - 1; i >= 0; i--)
					{
						if (this.ValidationRules[i].ValidatesOnTargetUpdated)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x001E399C File Offset: 0x001E299C
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			UIElement uielement = dependencyObject as UIElement;
			if (uielement != null)
			{
				uielement.LayoutUpdated -= this.OnLayoutUpdated;
			}
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			Helper.DowncastToFEorFCE(dependencyObject, out frameworkElement, out frameworkContentElement, false);
			if (frameworkElement != null)
			{
				frameworkElement.DataContextChanged += this.OnDataContextChanged;
			}
			else if (frameworkContentElement != null)
			{
				frameworkContentElement.DataContextChanged += this.OnDataContextChanged;
			}
			this.ValidateOnDataTransfer();
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x001E3A07 File Offset: 0x001E2A07
		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			this.IsItemsValid = false;
			this.ValidateOnDataTransfer();
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x001E3A28 File Offset: 0x001E2A28
		private void ValidateOnDataTransfer()
		{
			if (!this.ValidatesOnDataTransfer)
			{
				return;
			}
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject == null || this.ValidationRules.Count == 0)
			{
				return;
			}
			Collection<ValidationError> collection;
			if (!Validation.GetHasError(dependencyObject))
			{
				collection = null;
			}
			else
			{
				collection = new Collection<ValidationError>();
				ReadOnlyCollection<ValidationError> errors = Validation.GetErrors(dependencyObject);
				int i = 0;
				int count = errors.Count;
				while (i < count)
				{
					ValidationError validationError = errors[i];
					if (validationError.RuleInError.ValidatesOnTargetUpdated && validationError.BindingInError == this)
					{
						collection.Add(validationError);
					}
					i++;
				}
			}
			CultureInfo culture = this.GetCulture();
			int j = 0;
			int count2 = this.ValidationRules.Count;
			while (j < count2)
			{
				ValidationRule validationRule = this.ValidationRules[j];
				if (validationRule.ValidatesOnTargetUpdated)
				{
					try
					{
						ValidationResult validationResult = validationRule.Validate(DependencyProperty.UnsetValue, culture, this);
						if (!validationResult.IsValid)
						{
							this.AddValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null));
						}
					}
					catch (ValueUnavailableException ex)
					{
						this.AddValidationError(new ValidationError(validationRule, this, ex.Message, ex));
					}
				}
				j++;
			}
			if (collection != null)
			{
				int k = 0;
				int count3 = collection.Count;
				while (k < count3)
				{
					this.RemoveValidationError(collection[k]);
					k++;
				}
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x001E3B78 File Offset: 0x001E2B78
		private bool UpdateAndValidate(ValidationStep validationStep)
		{
			DependencyObject dependencyObject = Helper.FindMentor(this);
			if (dependencyObject != null && dependencyObject.GetValue(FrameworkElement.DataContextProperty) == CollectionView.NewItemPlaceholder)
			{
				return true;
			}
			this.PrepareProposedValuesForUpdate(dependencyObject, validationStep >= ValidationStep.UpdatedValue);
			bool flag = true;
			this._validationStep = ValidationStep.RawProposedValue;
			while (this._validationStep <= validationStep && flag)
			{
				switch (this._validationStep)
				{
				case ValidationStep.RawProposedValue:
					this._getValueTable.ResetValues();
					break;
				case ValidationStep.ConvertedProposedValue:
					flag = this.ObtainConvertedProposedValues();
					break;
				case ValidationStep.UpdatedValue:
					flag = this.UpdateValues();
					break;
				case ValidationStep.CommittedValue:
					flag = this.CommitValues();
					break;
				}
				if (!this.CheckValidationRules())
				{
					flag = false;
				}
				this._validationStep++;
			}
			this.ResetProposedValuesAfterUpdate(dependencyObject, flag && validationStep == ValidationStep.CommittedValue);
			this._validationStep = (ValidationStep)(-1);
			this._getValueTable.ResetValues();
			this.NotifyCommitManager();
			return flag;
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x001E3C54 File Offset: 0x001E2C54
		private void UpdateNotifyDataErrors(INotifyDataErrorInfo indei, WeakReference itemWR)
		{
			if (itemWR == null)
			{
				int num = BindingGroup.FindIndexOf(indei, this._itemsRW);
				if (num < 0)
				{
					return;
				}
				itemWR = this._itemsRW[num];
			}
			List<object> dataErrors;
			try
			{
				dataErrors = BindingExpression.GetDataErrors(indei, string.Empty);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				return;
			}
			this.UpdateNotifyDataErrorValidationErrors(itemWR, dataErrors);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x001E3CB8 File Offset: 0x001E2CB8
		private void UpdateNotifyDataErrorValidationErrors(WeakReference itemWR, List<object> errors)
		{
			List<ValidationError> list;
			if (!this._notifyDataErrors.TryGetValue(itemWR, out list))
			{
				list = null;
			}
			List<object> list2;
			List<ValidationError> list3;
			BindingExpressionBase.GetValidationDelta(list, errors, out list2, out list3);
			if (list2 != null && list2.Count > 0)
			{
				ValidationRule instance = NotifyDataErrorValidationRule.Instance;
				if (list == null)
				{
					list = new List<ValidationError>();
				}
				foreach (object errorContent in list2)
				{
					ValidationError validationError = new ValidationError(instance, this, errorContent, null);
					list.Add(validationError);
					this.AddValidationError(validationError);
				}
			}
			if (list3 != null && list3.Count > 0)
			{
				foreach (ValidationError validationError2 in list3)
				{
					list.Remove(validationError2);
					this.RemoveValidationError(validationError2);
				}
				if (list.Count == 0)
				{
					list = null;
				}
			}
			if (list == null)
			{
				this._notifyDataErrors.Remove(itemWR);
				return;
			}
			this._notifyDataErrors[itemWR] = list;
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x001E3DD4 File Offset: 0x001E2DD4
		private bool ObtainConvertedProposedValues()
		{
			bool flag = true;
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				flag = (this._bindingExpressions[i].ObtainConvertedProposedValue(this) && flag);
			}
			return flag;
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x001E3E14 File Offset: 0x001E2E14
		private bool UpdateValues()
		{
			bool flag = true;
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				flag = (this._bindingExpressions[i].UpdateSource(this) && flag);
			}
			if (this._proposedValueBindingExpressions != null)
			{
				for (int j = this._proposedValueBindingExpressions.Length - 1; j >= 0; j--)
				{
					BindingExpression bindingExpression = this._proposedValueBindingExpressions[j];
					BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[bindingExpression];
					flag = (bindingExpression.UpdateSource(proposedValueEntry.ConvertedValue) != DependencyProperty.UnsetValue && flag);
				}
			}
			return flag;
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x001E3EA0 File Offset: 0x001E2EA0
		private bool CheckValidationRules()
		{
			bool result = true;
			this.ClearValidationErrors(this._validationStep);
			for (int i = this._bindingExpressions.Count - 1; i >= 0; i--)
			{
				if (!this._bindingExpressions[i].CheckValidationRules(this, this._validationStep))
				{
					result = false;
				}
			}
			if (this._validationStep >= ValidationStep.UpdatedValue && this._proposedValueBindingExpressions != null)
			{
				for (int j = this._proposedValueBindingExpressions.Length - 1; j >= 0; j--)
				{
					if (!this._proposedValueBindingExpressions[j].CheckValidationRules(this, this._validationStep))
					{
						result = false;
					}
				}
			}
			CultureInfo culture = this.GetCulture();
			int k = 0;
			int count = this._validationRules.Count;
			while (k < count)
			{
				ValidationRule validationRule = this._validationRules[k];
				if (validationRule.ValidationStep == this._validationStep)
				{
					try
					{
						ValidationResult validationResult = validationRule.Validate(DependencyProperty.UnsetValue, culture, this);
						if (!validationResult.IsValid)
						{
							this.AddValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null));
							result = false;
						}
					}
					catch (ValueUnavailableException ex)
					{
						this.AddValidationError(new ValidationError(validationRule, this, ex.Message, ex));
						result = false;
					}
				}
				k++;
			}
			return result;
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x001E3FD4 File Offset: 0x001E2FD4
		private bool CommitValues()
		{
			bool result = true;
			IList items = this.Items;
			for (int i = items.Count - 1; i >= 0; i--)
			{
				IEditableObject editableObject = items[i] as IEditableObject;
				if (editableObject != null)
				{
					try
					{
						editableObject.EndEdit();
					}
					catch (Exception ex)
					{
						if (CriticalExceptions.IsCriticalApplicationException(ex))
						{
							throw;
						}
						ValidationError validationError = new ValidationError(ExceptionValidationRule.Instance, this, ex.Message, ex);
						this.AddValidationError(validationError);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x001E4058 File Offset: 0x001E3058
		private static int FindIndexOf(WeakReference wr, IList<WeakReference> list)
		{
			object target = wr.Target;
			if (target == null)
			{
				return -1;
			}
			return BindingGroup.FindIndexOf(target, list);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x001E4078 File Offset: 0x001E3078
		private static int FindIndexOf(object item, IList<WeakReference> list)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				if (ItemsControl.EqualsEx(item, list[i].Target))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x001E40B0 File Offset: 0x001E30B0
		private CultureInfo GetCulture()
		{
			if (this._culture == null)
			{
				DependencyObject dependencyObject = Helper.FindMentor(this);
				if (dependencyObject != null)
				{
					this._culture = ((XmlLanguage)dependencyObject.GetValue(FrameworkElement.LanguageProperty)).GetSpecificCulture();
				}
			}
			return this._culture;
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x001E40F0 File Offset: 0x001E30F0
		private void OnBindingsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				BindingExpressionBase bindingExpressionBase = e.NewItems[0] as BindingExpressionBase;
				bindingExpressionBase.JoinBindingGroup(this, true);
				break;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				BindingExpressionBase bindingExpressionBase = e.OldItems[0] as BindingExpressionBase;
				this.RemoveBindingExpression(bindingExpressionBase);
				break;
			}
			case NotifyCollectionChangedAction.Replace:
			{
				BindingExpressionBase bindingExpressionBase = e.OldItems[0] as BindingExpressionBase;
				this.RemoveBindingExpression(bindingExpressionBase);
				bindingExpressionBase = (e.NewItems[0] as BindingExpressionBase);
				bindingExpressionBase.JoinBindingGroup(this, true);
				break;
			}
			case NotifyCollectionChangedAction.Reset:
				this.RemoveAllBindingExpressions();
				break;
			}
			this.IsItemsValid = false;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x001E419C File Offset: 0x001E319C
		private void RemoveBindingExpression(BindingExpressionBase exprBase)
		{
			BindingExpressionBase rootBindingExpression = exprBase.RootBindingExpression;
			if (this.SharesProposedValues && rootBindingExpression.NeedsValidation)
			{
				Collection<BindingExpressionBase.ProposedValue> proposedValues;
				rootBindingExpression.ValidateAndConvertProposedValue(out proposedValues);
				this.PreserveProposedValues(proposedValues);
			}
			foreach (BindingExpressionBase bindingExpressionBase in this._getValueTable.RemoveRootBinding(rootBindingExpression))
			{
				bindingExpressionBase.OnBindingGroupChanged(false);
				this._bindingExpressions.Remove(bindingExpressionBase);
			}
			rootBindingExpression.LeaveBindingGroup();
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x001E4230 File Offset: 0x001E3230
		private void RemoveAllBindingExpressions()
		{
			BindingGroup.GetValueTableEntry firstEntry;
			while ((firstEntry = this._getValueTable.GetFirstEntry()) != null)
			{
				this.RemoveBindingExpression(firstEntry.BindingExpressionBase);
			}
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x001E425C File Offset: 0x001E325C
		private void PreserveProposedValues(Collection<BindingExpressionBase.ProposedValue> proposedValues)
		{
			if (proposedValues == null)
			{
				return;
			}
			int i = 0;
			int count = proposedValues.Count;
			while (i < count)
			{
				this._proposedValueTable.Add(proposedValues[i]);
				i++;
			}
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x001E4294 File Offset: 0x001E3294
		private void PrepareProposedValuesForUpdate(DependencyObject mentor, bool isUpdating)
		{
			int count = this._proposedValueTable.Count;
			if (count == 0)
			{
				return;
			}
			if (isUpdating)
			{
				this._proposedValueBindingExpressions = new BindingExpression[count];
				for (int i = 0; i < count; i++)
				{
					BindingGroup.ProposedValueEntry proposedValueEntry = this._proposedValueTable[i];
					Binding binding = proposedValueEntry.Binding;
					Binding binding2 = new Binding();
					binding2.Source = proposedValueEntry.Item;
					binding2.Mode = BindingMode.TwoWay;
					binding2.Path = new PropertyPath(proposedValueEntry.PropertyName, new object[]
					{
						binding.Path.PathParameters
					});
					binding2.ValidatesOnDataErrors = binding.ValidatesOnDataErrors;
					binding2.ValidatesOnNotifyDataErrors = binding.ValidatesOnNotifyDataErrors;
					binding2.ValidatesOnExceptions = binding.ValidatesOnExceptions;
					Collection<ValidationRule> validationRulesInternal = binding.ValidationRulesInternal;
					if (validationRulesInternal != null)
					{
						int j = 0;
						int count2 = validationRulesInternal.Count;
						while (j < count2)
						{
							binding2.ValidationRules.Add(validationRulesInternal[j]);
							j++;
						}
					}
					BindingExpression bindingExpression = (BindingExpression)BindingExpressionBase.CreateUntargetedBindingExpression(mentor, binding2);
					bindingExpression.Attach(mentor);
					bindingExpression.NeedsUpdate = true;
					this._proposedValueBindingExpressions[i] = bindingExpression;
				}
			}
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x001E43B8 File Offset: 0x001E33B8
		private void ResetProposedValuesAfterUpdate(DependencyObject mentor, bool isFullUpdate)
		{
			if (this._proposedValueBindingExpressions != null)
			{
				int i = 0;
				int num = this._proposedValueBindingExpressions.Length;
				while (i < num)
				{
					BindingExpression bindingExpression = this._proposedValueBindingExpressions[i];
					ValidationError validationError = bindingExpression.ValidationError;
					bindingExpression.Detach();
					if (validationError != null)
					{
						ValidationError validationError2 = new ValidationError(validationError.RuleInError, this, validationError.ErrorContent, validationError.Exception);
						this.AddValidationError(validationError2);
					}
					i++;
				}
				this._proposedValueBindingExpressions = null;
			}
			if (isFullUpdate)
			{
				this._proposedValueTable.UpdateDependents();
				this._proposedValueTable.Clear();
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x001E443C File Offset: 0x001E343C
		private void NotifyCommitManager()
		{
			if (this.Engine.IsShutDown)
			{
				return;
			}
			if (this.Owner != null && (this.IsDirty || this.HasValidationError))
			{
				this.Engine.CommitManager.AddBindingGroup(this);
				return;
			}
			this.Engine.CommitManager.RemoveBindingGroup(this);
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001E4498 File Offset: 0x001E3498
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.UpdateNotifyDataErrors((INotifyDataErrorInfo)sender, null);
				return;
			}
			this.Engine.Marshal(delegate(object arg)
			{
				this.UpdateNotifyDataErrors((INotifyDataErrorInfo)arg, null);
				return null;
			}, sender, 1);
		}

		// Token: 0x04001CC8 RID: 7368
		private ValidationRuleCollection _validationRules;

		// Token: 0x04001CC9 RID: 7369
		private string _name;

		// Token: 0x04001CCA RID: 7370
		private bool _notifyOnValidationError;

		// Token: 0x04001CCB RID: 7371
		private bool _sharesProposedValues;

		// Token: 0x04001CCC RID: 7372
		private bool _validatesOnNotifyDataError = true;

		// Token: 0x04001CCD RID: 7373
		private DataBindEngine _engine;

		// Token: 0x04001CCE RID: 7374
		private BindingGroup.BindingExpressionCollection _bindingExpressions;

		// Token: 0x04001CCF RID: 7375
		private bool _isItemsValid;

		// Token: 0x04001CD0 RID: 7376
		private ValidationStep _validationStep = (ValidationStep)(-1);

		// Token: 0x04001CD1 RID: 7377
		private BindingGroup.GetValueTable _getValueTable = new BindingGroup.GetValueTable();

		// Token: 0x04001CD2 RID: 7378
		private BindingGroup.ProposedValueTable _proposedValueTable = new BindingGroup.ProposedValueTable();

		// Token: 0x04001CD3 RID: 7379
		private BindingExpression[] _proposedValueBindingExpressions;

		// Token: 0x04001CD4 RID: 7380
		private Collection<WeakReference> _itemsRW;

		// Token: 0x04001CD5 RID: 7381
		private WeakReadOnlyCollection<object> _items;

		// Token: 0x04001CD6 RID: 7382
		private CultureInfo _culture;

		// Token: 0x04001CD7 RID: 7383
		private Dictionary<WeakReference, List<ValidationError>> _notifyDataErrors = new Dictionary<WeakReference, List<ValidationError>>();

		// Token: 0x04001CD8 RID: 7384
		internal static readonly object DeferredTargetValue = new NamedObject("DeferredTargetValue");

		// Token: 0x04001CD9 RID: 7385
		internal static readonly object DeferredSourceValue = new NamedObject("DeferredSourceValue");

		// Token: 0x04001CDA RID: 7386
		private static WeakReference<DependencyObject> NullInheritanceContext = new WeakReference<DependencyObject>(null);

		// Token: 0x04001CDB RID: 7387
		private WeakReference<DependencyObject> _inheritanceContext = BindingGroup.NullInheritanceContext;

		// Token: 0x04001CDC RID: 7388
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x02000AD1 RID: 2769
		private class GetValueTable
		{
			// Token: 0x17001E6A RID: 7786
			public BindingGroup.GetValueTableEntry this[object item, string propertyName]
			{
				get
				{
					for (int i = this._table.Count - 1; i >= 0; i--)
					{
						BindingGroup.GetValueTableEntry getValueTableEntry = this._table[i];
						if (propertyName == getValueTableEntry.PropertyName && ItemsControl.EqualsEx(item, getValueTableEntry.Item))
						{
							return getValueTableEntry;
						}
					}
					return null;
				}
			}

			// Token: 0x17001E6B RID: 7787
			public BindingGroup.GetValueTableEntry this[BindingExpressionBase bindingExpressionBase]
			{
				get
				{
					for (int i = this._table.Count - 1; i >= 0; i--)
					{
						BindingGroup.GetValueTableEntry getValueTableEntry = this._table[i];
						if (bindingExpressionBase == getValueTableEntry.BindingExpressionBase)
						{
							return getValueTableEntry;
						}
					}
					return null;
				}
			}

			// Token: 0x06008AFA RID: 35578 RVA: 0x00338DA6 File Offset: 0x00337DA6
			public void EnsureEntry(BindingExpressionBase bindingExpressionBase)
			{
				if (this[bindingExpressionBase] == null)
				{
					this._table.Add(new BindingGroup.GetValueTableEntry(bindingExpressionBase));
				}
			}

			// Token: 0x06008AFB RID: 35579 RVA: 0x00338DC4 File Offset: 0x00337DC4
			public bool Update(BindingExpression bindingExpression)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpression];
				bool flag = getValueTableEntry == null;
				if (flag)
				{
					this._table.Add(new BindingGroup.GetValueTableEntry(bindingExpression));
					return flag;
				}
				getValueTableEntry.Update(bindingExpression);
				return flag;
			}

			// Token: 0x06008AFC RID: 35580 RVA: 0x00338DFC File Offset: 0x00337DFC
			public List<BindingExpressionBase> RemoveRootBinding(BindingExpressionBase rootBindingExpression)
			{
				List<BindingExpressionBase> list = new List<BindingExpressionBase>();
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					BindingExpressionBase bindingExpressionBase = this._table[i].BindingExpressionBase;
					if (bindingExpressionBase.RootBindingExpression == rootBindingExpression)
					{
						list.Add(bindingExpressionBase);
						this._table.RemoveAt(i);
					}
				}
				return list;
			}

			// Token: 0x06008AFD RID: 35581 RVA: 0x00338E58 File Offset: 0x00337E58
			public void AddUniqueItems(IList<WeakReference> list)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (this._table[i].BindingExpressionBase.StatusInternal != BindingStatusInternal.PathError)
					{
						WeakReference itemReference = this._table[i].ItemReference;
						if (itemReference != null && BindingGroup.FindIndexOf(itemReference, list) < 0)
						{
							list.Add(itemReference);
						}
					}
				}
			}

			// Token: 0x06008AFE RID: 35582 RVA: 0x00338EBC File Offset: 0x00337EBC
			public object GetValue(BindingExpressionBase bindingExpressionBase)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpressionBase];
				if (getValueTableEntry == null)
				{
					return DependencyProperty.UnsetValue;
				}
				return getValueTableEntry.Value;
			}

			// Token: 0x06008AFF RID: 35583 RVA: 0x00338EE0 File Offset: 0x00337EE0
			public void SetValue(BindingExpressionBase bindingExpressionBase, object value)
			{
				BindingGroup.GetValueTableEntry getValueTableEntry = this[bindingExpressionBase];
				if (getValueTableEntry != null)
				{
					getValueTableEntry.Value = value;
				}
			}

			// Token: 0x06008B00 RID: 35584 RVA: 0x00338F00 File Offset: 0x00337F00
			public void ResetValues()
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					this._table[i].Value = BindingGroup.DeferredTargetValue;
				}
			}

			// Token: 0x06008B01 RID: 35585 RVA: 0x00338F3C File Offset: 0x00337F3C
			public void UseSourceValue(BindingExpressionBase rootBindingExpression)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (this._table[i].BindingExpressionBase.RootBindingExpression == rootBindingExpression)
					{
						this._table[i].Value = BindingGroup.DeferredSourceValue;
					}
				}
			}

			// Token: 0x06008B02 RID: 35586 RVA: 0x00338F90 File Offset: 0x00337F90
			public BindingGroup.GetValueTableEntry GetFirstEntry()
			{
				if (this._table.Count <= 0)
				{
					return null;
				}
				return this._table[0];
			}

			// Token: 0x040046E5 RID: 18149
			private Collection<BindingGroup.GetValueTableEntry> _table = new Collection<BindingGroup.GetValueTableEntry>();
		}

		// Token: 0x02000AD2 RID: 2770
		private class GetValueTableEntry
		{
			// Token: 0x06008B04 RID: 35588 RVA: 0x00338FC1 File Offset: 0x00337FC1
			public GetValueTableEntry(BindingExpressionBase bindingExpressionBase)
			{
				this._bindingExpressionBase = bindingExpressionBase;
			}

			// Token: 0x06008B05 RID: 35589 RVA: 0x00338FDC File Offset: 0x00337FDC
			public void Update(BindingExpression bindingExpression)
			{
				object sourceItem = bindingExpression.SourceItem;
				if (sourceItem == null)
				{
					this._itemWR = null;
				}
				else if (this._itemWR == null)
				{
					this._itemWR = new WeakReference(sourceItem);
				}
				else
				{
					this._itemWR.Target = bindingExpression.SourceItem;
				}
				this._propertyName = bindingExpression.SourcePropertyName;
			}

			// Token: 0x17001E6C RID: 7788
			// (get) Token: 0x06008B06 RID: 35590 RVA: 0x0033902F File Offset: 0x0033802F
			public object Item
			{
				get
				{
					return this._itemWR.Target;
				}
			}

			// Token: 0x17001E6D RID: 7789
			// (get) Token: 0x06008B07 RID: 35591 RVA: 0x0033903C File Offset: 0x0033803C
			public WeakReference ItemReference
			{
				get
				{
					return this._itemWR;
				}
			}

			// Token: 0x17001E6E RID: 7790
			// (get) Token: 0x06008B08 RID: 35592 RVA: 0x00339044 File Offset: 0x00338044
			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			// Token: 0x17001E6F RID: 7791
			// (get) Token: 0x06008B09 RID: 35593 RVA: 0x0033904C File Offset: 0x0033804C
			public BindingExpressionBase BindingExpressionBase
			{
				get
				{
					return this._bindingExpressionBase;
				}
			}

			// Token: 0x17001E70 RID: 7792
			// (get) Token: 0x06008B0A RID: 35594 RVA: 0x00339054 File Offset: 0x00338054
			// (set) Token: 0x06008B0B RID: 35595 RVA: 0x003390BB File Offset: 0x003380BB
			public object Value
			{
				get
				{
					if (this._value == BindingGroup.DeferredTargetValue)
					{
						this._value = this._bindingExpressionBase.RootBindingExpression.GetRawProposedValue();
					}
					else if (this._value == BindingGroup.DeferredSourceValue)
					{
						BindingExpression bindingExpression = this._bindingExpressionBase as BindingExpression;
						this._value = ((bindingExpression != null) ? bindingExpression.SourceValue : DependencyProperty.UnsetValue);
					}
					return this._value;
				}
				set
				{
					this._value = value;
				}
			}

			// Token: 0x040046E6 RID: 18150
			private BindingExpressionBase _bindingExpressionBase;

			// Token: 0x040046E7 RID: 18151
			private WeakReference _itemWR;

			// Token: 0x040046E8 RID: 18152
			private string _propertyName;

			// Token: 0x040046E9 RID: 18153
			private object _value = BindingGroup.DeferredTargetValue;
		}

		// Token: 0x02000AD3 RID: 2771
		private class ProposedValueTable
		{
			// Token: 0x06008B0C RID: 35596 RVA: 0x003390C4 File Offset: 0x003380C4
			public void Add(BindingExpressionBase.ProposedValue proposedValue)
			{
				BindingExpression bindingExpression = proposedValue.BindingExpression;
				object sourceItem = bindingExpression.SourceItem;
				string sourcePropertyName = bindingExpression.SourcePropertyName;
				object rawValue = proposedValue.RawValue;
				object convertedValue = proposedValue.ConvertedValue;
				this.Remove(sourceItem, sourcePropertyName);
				this._table.Add(new BindingGroup.ProposedValueEntry(sourceItem, sourcePropertyName, rawValue, convertedValue, bindingExpression));
			}

			// Token: 0x06008B0D RID: 35597 RVA: 0x00339114 File Offset: 0x00338114
			public void Remove(object item, string propertyName)
			{
				int num = this.IndexOf(item, propertyName);
				if (num >= 0)
				{
					this._table.RemoveAt(num);
				}
			}

			// Token: 0x06008B0E RID: 35598 RVA: 0x0033913A File Offset: 0x0033813A
			public void Remove(BindingExpression bindExpr)
			{
				if (this._table.Count > 0)
				{
					this.Remove(bindExpr.SourceItem, bindExpr.SourcePropertyName);
				}
			}

			// Token: 0x06008B0F RID: 35599 RVA: 0x0033915C File Offset: 0x0033815C
			public void Remove(BindingGroup.ProposedValueEntry entry)
			{
				this._table.Remove(entry);
			}

			// Token: 0x06008B10 RID: 35600 RVA: 0x0033916B File Offset: 0x0033816B
			public void Clear()
			{
				this._table.Clear();
			}

			// Token: 0x17001E71 RID: 7793
			// (get) Token: 0x06008B11 RID: 35601 RVA: 0x00339178 File Offset: 0x00338178
			public int Count
			{
				get
				{
					return this._table.Count;
				}
			}

			// Token: 0x17001E72 RID: 7794
			public BindingGroup.ProposedValueEntry this[object item, string propertyName]
			{
				get
				{
					int num = this.IndexOf(item, propertyName);
					if (num >= 0)
					{
						return this._table[num];
					}
					return null;
				}
			}

			// Token: 0x17001E73 RID: 7795
			public BindingGroup.ProposedValueEntry this[int index]
			{
				get
				{
					return this._table[index];
				}
			}

			// Token: 0x17001E74 RID: 7796
			public BindingGroup.ProposedValueEntry this[BindingExpression bindExpr]
			{
				get
				{
					return this[bindExpr.SourceItem, bindExpr.SourcePropertyName];
				}
			}

			// Token: 0x06008B15 RID: 35605 RVA: 0x003391D4 File Offset: 0x003381D4
			public void AddUniqueItems(IList<WeakReference> list)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					WeakReference itemReference = this._table[i].ItemReference;
					if (itemReference != null && BindingGroup.FindIndexOf(itemReference, list) < 0)
					{
						list.Add(itemReference);
					}
				}
			}

			// Token: 0x06008B16 RID: 35606 RVA: 0x00339220 File Offset: 0x00338220
			public void UpdateDependents()
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					Collection<BindingExpressionBase> dependents = this._table[i].Dependents;
					if (dependents != null)
					{
						for (int j = dependents.Count - 1; j >= 0; j--)
						{
							if (!dependents[j].IsDetached)
							{
								dependents[j].UpdateTarget();
							}
						}
					}
				}
			}

			// Token: 0x06008B17 RID: 35607 RVA: 0x00339288 File Offset: 0x00338288
			public bool HasValidationError(ValidationError validationError)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					if (validationError == this._table[i].ValidationError)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06008B18 RID: 35608 RVA: 0x003392C4 File Offset: 0x003382C4
			private int IndexOf(object item, string propertyName)
			{
				for (int i = this._table.Count - 1; i >= 0; i--)
				{
					BindingGroup.ProposedValueEntry proposedValueEntry = this._table[i];
					if (propertyName == proposedValueEntry.PropertyName && ItemsControl.EqualsEx(item, proposedValueEntry.Item))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x040046EA RID: 18154
			private Collection<BindingGroup.ProposedValueEntry> _table = new Collection<BindingGroup.ProposedValueEntry>();
		}

		// Token: 0x02000AD4 RID: 2772
		internal class ProposedValueEntry
		{
			// Token: 0x06008B1A RID: 35610 RVA: 0x00339328 File Offset: 0x00338328
			public ProposedValueEntry(object item, string propertyName, object rawValue, object convertedValue, BindingExpression bindExpr)
			{
				this._itemReference = new WeakReference(item);
				this._propertyName = propertyName;
				this._rawValue = rawValue;
				this._convertedValue = convertedValue;
				this._error = bindExpr.ValidationError;
				this._binding = bindExpr.ParentBinding;
			}

			// Token: 0x17001E75 RID: 7797
			// (get) Token: 0x06008B1B RID: 35611 RVA: 0x00339377 File Offset: 0x00338377
			public object Item
			{
				get
				{
					return this._itemReference.Target;
				}
			}

			// Token: 0x17001E76 RID: 7798
			// (get) Token: 0x06008B1C RID: 35612 RVA: 0x00339384 File Offset: 0x00338384
			public string PropertyName
			{
				get
				{
					return this._propertyName;
				}
			}

			// Token: 0x17001E77 RID: 7799
			// (get) Token: 0x06008B1D RID: 35613 RVA: 0x0033938C File Offset: 0x0033838C
			public object RawValue
			{
				get
				{
					return this._rawValue;
				}
			}

			// Token: 0x17001E78 RID: 7800
			// (get) Token: 0x06008B1E RID: 35614 RVA: 0x00339394 File Offset: 0x00338394
			public object ConvertedValue
			{
				get
				{
					return this._convertedValue;
				}
			}

			// Token: 0x17001E79 RID: 7801
			// (get) Token: 0x06008B1F RID: 35615 RVA: 0x0033939C File Offset: 0x0033839C
			public ValidationError ValidationError
			{
				get
				{
					return this._error;
				}
			}

			// Token: 0x17001E7A RID: 7802
			// (get) Token: 0x06008B20 RID: 35616 RVA: 0x003393A4 File Offset: 0x003383A4
			public Binding Binding
			{
				get
				{
					return this._binding;
				}
			}

			// Token: 0x17001E7B RID: 7803
			// (get) Token: 0x06008B21 RID: 35617 RVA: 0x003393AC File Offset: 0x003383AC
			public WeakReference ItemReference
			{
				get
				{
					return this._itemReference;
				}
			}

			// Token: 0x17001E7C RID: 7804
			// (get) Token: 0x06008B22 RID: 35618 RVA: 0x003393B4 File Offset: 0x003383B4
			public Collection<BindingExpressionBase> Dependents
			{
				get
				{
					return this._dependents;
				}
			}

			// Token: 0x06008B23 RID: 35619 RVA: 0x003393BC File Offset: 0x003383BC
			public void AddDependent(BindingExpressionBase dependent)
			{
				if (this._dependents == null)
				{
					this._dependents = new Collection<BindingExpressionBase>();
				}
				this._dependents.Add(dependent);
			}

			// Token: 0x040046EB RID: 18155
			private WeakReference _itemReference;

			// Token: 0x040046EC RID: 18156
			private string _propertyName;

			// Token: 0x040046ED RID: 18157
			private object _rawValue;

			// Token: 0x040046EE RID: 18158
			private object _convertedValue;

			// Token: 0x040046EF RID: 18159
			private ValidationError _error;

			// Token: 0x040046F0 RID: 18160
			private Binding _binding;

			// Token: 0x040046F1 RID: 18161
			private Collection<BindingExpressionBase> _dependents;
		}

		// Token: 0x02000AD5 RID: 2773
		private class BindingExpressionCollection : ObservableCollection<BindingExpressionBase>
		{
			// Token: 0x06008B24 RID: 35620 RVA: 0x003393DD File Offset: 0x003383DD
			protected override void InsertItem(int index, BindingExpressionBase item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				base.InsertItem(index, item);
			}

			// Token: 0x06008B25 RID: 35621 RVA: 0x003393F5 File Offset: 0x003383F5
			protected override void SetItem(int index, BindingExpressionBase item)
			{
				if (item == null)
				{
					throw new ArgumentNullException("item");
				}
				base.SetItem(index, item);
			}
		}
	}
}
