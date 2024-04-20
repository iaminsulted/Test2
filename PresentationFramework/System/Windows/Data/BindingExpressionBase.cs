using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Documents;

namespace System.Windows.Data
{
	// Token: 0x0200044E RID: 1102
	public abstract class BindingExpressionBase : Expression, IWeakEventListener
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x001E0CD8 File Offset: 0x001DFCD8
		internal BindingExpressionBase(BindingBase binding, BindingExpressionBase parent) : base(ExpressionMode.SupportsUnboundSources)
		{
			if (binding == null)
			{
				throw new ArgumentNullException("binding");
			}
			this._binding = binding;
			this.SetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, parent, null);
			this._flags = (BindingExpressionBase.PrivateFlags)binding.Flags;
			if (parent != null)
			{
				this.ResolveNamesInTemplate = parent.ResolveNamesInTemplate;
				Type type = parent.GetType();
				if (type == typeof(MultiBindingExpression))
				{
					this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInMultiBindingExpression, true);
				}
				else if (type == typeof(PriorityBindingExpression))
				{
					this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression, true);
				}
			}
			PresentationTraceLevel traceLevel = PresentationTraceSources.GetTraceLevel(binding);
			if (traceLevel > PresentationTraceLevel.None)
			{
				PresentationTraceSources.SetTraceLevel(this, traceLevel);
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
			{
				if (parent == null)
				{
					TraceData.TraceAndNotify(TraceEventType.Warning, TraceData.CreatedExpression(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(binding)
					}), this, null);
				}
				else
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.CreatedExpressionInParent(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(binding),
						TraceData.Identify(parent)
					}), this);
				}
			}
			if (this.LookupValidationRule(typeof(ExceptionValidationRule)) != null)
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnExceptions, true);
			}
			if (this.LookupValidationRule(typeof(DataErrorValidationRule)) != null)
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors, true);
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x001E0E2E File Offset: 0x001DFE2E
		public DependencyObject Target
		{
			get
			{
				return this.TargetElement;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06003623 RID: 13859 RVA: 0x001E0E36 File Offset: 0x001DFE36
		public DependencyProperty TargetProperty
		{
			get
			{
				return this._targetProperty;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x001E0E3E File Offset: 0x001DFE3E
		public BindingBase ParentBindingBase
		{
			get
			{
				return this._binding;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06003625 RID: 13861 RVA: 0x001E0E48 File Offset: 0x001DFE48
		public BindingGroup BindingGroup
		{
			get
			{
				WeakReference<BindingGroup> weakReference = (WeakReference<BindingGroup>)this.RootBindingExpression.GetValue(BindingExpressionBase.Feature.BindingGroup, null);
				if (weakReference == null)
				{
					return null;
				}
				BindingGroup result;
				if (!weakReference.TryGetTarget(out result))
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06003626 RID: 13862 RVA: 0x001E0E7A File Offset: 0x001DFE7A
		public BindingStatus Status
		{
			get
			{
				return (BindingStatus)this._status;
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06003627 RID: 13863 RVA: 0x001E0E7A File Offset: 0x001DFE7A
		internal BindingStatusInternal StatusInternal
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x001E0E82 File Offset: 0x001DFE82
		public virtual ValidationError ValidationError
		{
			get
			{
				return this.BaseValidationError;
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06003629 RID: 13865 RVA: 0x001E0E8A File Offset: 0x001DFE8A
		internal ValidationError BaseValidationError
		{
			get
			{
				return (ValidationError)this.GetValue(BindingExpressionBase.Feature.ValidationError, null);
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x0600362A RID: 13866 RVA: 0x001E0E99 File Offset: 0x001DFE99
		internal List<ValidationError> NotifyDataErrors
		{
			get
			{
				return (List<ValidationError>)this.GetValue(BindingExpressionBase.Feature.NotifyDataErrors, null);
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x0600362B RID: 13867 RVA: 0x001E0EA8 File Offset: 0x001DFEA8
		public virtual bool HasError
		{
			get
			{
				return this.HasValidationError;
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x001E0EB0 File Offset: 0x001DFEB0
		public virtual bool HasValidationError
		{
			get
			{
				return this.HasValue(BindingExpressionBase.Feature.ValidationError) || this.HasValue(BindingExpressionBase.Feature.NotifyDataErrors);
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x001E0EC4 File Offset: 0x001DFEC4
		public bool IsDirty
		{
			get
			{
				return this.NeedsUpdate;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x001E0ECC File Offset: 0x001DFECC
		public virtual ReadOnlyCollection<ValidationError> ValidationErrors
		{
			get
			{
				if (this.HasError)
				{
					List<ValidationError> list;
					if (!this.HasValue(BindingExpressionBase.Feature.ValidationError))
					{
						list = this.NotifyDataErrors;
					}
					else
					{
						if (this.NotifyDataErrors == null)
						{
							list = new List<ValidationError>();
						}
						else
						{
							list = new List<ValidationError>(this.NotifyDataErrors);
						}
						list.Insert(0, this.BaseValidationError);
					}
					return new ReadOnlyCollection<ValidationError>(list);
				}
				return null;
			}
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public virtual void UpdateTarget()
		{
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public virtual void UpdateSource()
		{
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x001E0F24 File Offset: 0x001DFF24
		public bool ValidateWithoutUpdate()
		{
			Collection<BindingExpressionBase.ProposedValue> collection;
			return !this.NeedsValidation || this.ValidateAndConvertProposedValue(out collection);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x001E0F43 File Offset: 0x001DFF43
		internal sealed override void OnAttach(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			this.Attach(d, dp);
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x001E0F69 File Offset: 0x001DFF69
		internal sealed override void OnDetach(DependencyObject d, DependencyProperty dp)
		{
			this.Detach();
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x001E0F71 File Offset: 0x001DFF71
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			return this.Value;
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x001E0F79 File Offset: 0x001DFF79
		internal override bool SetValue(DependencyObject d, DependencyProperty dp, object value)
		{
			if (this.IsReflective)
			{
				this.Value = value;
				return true;
			}
			return false;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x001E0F90 File Offset: 0x001DFF90
		internal override void OnPropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (this.IsDetached)
			{
				return;
			}
			this.IsTransferPending = true;
			if (this.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.HandlePropertyInvalidation(d, args);
				return;
			}
			this.Engine.Marshal(new DispatcherOperationCallback(this.HandlePropertyInvalidationOperation), new object[]
			{
				d,
				args
			}, 1);
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x001E0FF4 File Offset: 0x001DFFF4
		internal override DependencySource[] GetSources()
		{
			int num = (this._sources != null) ? this._sources.Length : 0;
			if (num == 0)
			{
				return null;
			}
			DependencySource[] array = new DependencySource[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				DependencyObject dependencyObject = this._sources[i].DependencyObject;
				if (dependencyObject != null)
				{
					array[num2++] = new DependencySource(dependencyObject, this._sources[i].DependencyProperty);
				}
			}
			if (num2 < num)
			{
				Array sourceArray = array;
				array = new DependencySource[num2];
				Array.Copy(sourceArray, 0, array, 0, num2);
			}
			return array;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x001E1073 File Offset: 0x001E0073
		internal override Expression Copy(DependencyObject targetObject, DependencyProperty targetDP)
		{
			return this.ParentBindingBase.CreateBindingExpression(targetObject, targetDP);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x001E1082 File Offset: 0x001E0082
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x001E108D File Offset: 0x001E008D
		internal static BindingExpressionBase CreateUntargetedBindingExpression(DependencyObject d, BindingBase binding)
		{
			return binding.CreateBindingExpression(d, BindingExpressionBase.NoTargetProperty);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x001E109B File Offset: 0x001E009B
		internal void Attach(DependencyObject d)
		{
			this.Attach(d, BindingExpressionBase.NoTargetProperty);
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x0600363C RID: 13884 RVA: 0x001E10AC File Offset: 0x001E00AC
		// (remove) Token: 0x0600363D RID: 13885 RVA: 0x001E10E4 File Offset: 0x001E00E4
		internal event EventHandler<BindingValueChangedEventArgs> ValueChanged;

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x001E1119 File Offset: 0x001E0119
		// (set) Token: 0x0600363F RID: 13887 RVA: 0x001E1126 File Offset: 0x001E0126
		internal bool IsAttaching
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iAttaching);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iAttaching, value);
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x001E1134 File Offset: 0x001E0134
		// (set) Token: 0x06003641 RID: 13889 RVA: 0x001E1141 File Offset: 0x001E0141
		internal bool IsDetaching
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDetaching);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDetaching, value);
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x06003642 RID: 13890 RVA: 0x001E114F File Offset: 0x001E014F
		internal bool IsDetached
		{
			get
			{
				return this._status == BindingStatusInternal.Detached;
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x06003643 RID: 13891 RVA: 0x001E115A File Offset: 0x001E015A
		private bool IsAttached
		{
			get
			{
				return this._status != BindingStatusInternal.Unattached && this._status != BindingStatusInternal.Detached && !this.IsDetaching;
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x001E1178 File Offset: 0x001E0178
		internal bool IsDynamic
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iSourceToTarget) && (!this.IsInMultiBindingExpression || this.ParentMultiBindingExpression.IsDynamic);
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06003645 RID: 13893 RVA: 0x001E119A File Offset: 0x001E019A
		// (set) Token: 0x06003646 RID: 13894 RVA: 0x001E11BC File Offset: 0x001E01BC
		internal bool IsReflective
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTargetToSource) && (!this.IsInMultiBindingExpression || this.ParentMultiBindingExpression.IsReflective);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTargetToSource, value);
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003647 RID: 13895 RVA: 0x001E11C6 File Offset: 0x001E01C6
		// (set) Token: 0x06003648 RID: 13896 RVA: 0x001E11D0 File Offset: 0x001E01D0
		internal bool UseDefaultValueConverter
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDefaultValueConverter);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDefaultValueConverter, value);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x001E11DB File Offset: 0x001E01DB
		internal bool IsOneWayToSource
		{
			get
			{
				return (this._flags & BindingExpressionBase.PrivateFlags.iPropagationMask) == BindingExpressionBase.PrivateFlags.iTargetToSource;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x001E11E8 File Offset: 0x001E01E8
		internal bool IsUpdateOnPropertyChanged
		{
			get
			{
				return (this._flags & BindingExpressionBase.PrivateFlags.iUpdateDefault) == ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition);
			}
		}

		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x001E11F9 File Offset: 0x001E01F9
		internal bool IsUpdateOnLostFocus
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus);
			}
		}

		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x001E1206 File Offset: 0x001E0206
		// (set) Token: 0x0600364D RID: 13901 RVA: 0x001E1213 File Offset: 0x001E0213
		internal bool IsTransferPending
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTransferPending);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTransferPending, value);
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x0600364E RID: 13902 RVA: 0x001E1221 File Offset: 0x001E0221
		// (set) Token: 0x0600364F RID: 13903 RVA: 0x001E122E File Offset: 0x001E022E
		internal bool TransferIsDeferred
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTransferDeferred);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTransferDeferred, value);
			}
		}

		// Token: 0x17000B85 RID: 2949
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x001E123C File Offset: 0x001E023C
		// (set) Token: 0x06003651 RID: 13905 RVA: 0x001E1246 File Offset: 0x001E0246
		internal bool IsInTransfer
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInTransfer);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInTransfer, value);
			}
		}

		// Token: 0x17000B86 RID: 2950
		// (get) Token: 0x06003652 RID: 13906 RVA: 0x001E1251 File Offset: 0x001E0251
		// (set) Token: 0x06003653 RID: 13907 RVA: 0x001E125B File Offset: 0x001E025B
		internal bool IsInUpdate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInUpdate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate, value);
			}
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06003654 RID: 13908 RVA: 0x001E1266 File Offset: 0x001E0266
		// (set) Token: 0x06003655 RID: 13909 RVA: 0x001E1273 File Offset: 0x001E0273
		internal bool UsingFallbackValue
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUsingFallbackValue);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUsingFallbackValue, value);
			}
		}

		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x06003656 RID: 13910 RVA: 0x001E1281 File Offset: 0x001E0281
		// (set) Token: 0x06003657 RID: 13911 RVA: 0x001E128E File Offset: 0x001E028E
		internal bool UsingMentor
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iUsingMentor);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUsingMentor, value);
			}
		}

		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x06003658 RID: 13912 RVA: 0x001E129C File Offset: 0x001E029C
		// (set) Token: 0x06003659 RID: 13913 RVA: 0x001E12A9 File Offset: 0x001E02A9
		internal bool ResolveNamesInTemplate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate, value);
			}
		}

		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x0600365A RID: 13914 RVA: 0x001E12B7 File Offset: 0x001E02B7
		// (set) Token: 0x0600365B RID: 13915 RVA: 0x001E12C4 File Offset: 0x001E02C4
		internal bool NeedsDataTransfer
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedDataTransfer);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedDataTransfer, value);
			}
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x001E12D2 File Offset: 0x001E02D2
		// (set) Token: 0x0600365D RID: 13917 RVA: 0x001E12DF File Offset: 0x001E02DF
		internal bool NeedsUpdate
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedsUpdate);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedsUpdate, value);
				if (value)
				{
					this.NeedsValidation = true;
				}
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x001E12F7 File Offset: 0x001E02F7
		// (set) Token: 0x0600365F RID: 13919 RVA: 0x001E130F File Offset: 0x001E030F
		internal bool NeedsValidation
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNeedsValidation) || this.HasValue(BindingExpressionBase.Feature.ValidationError);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNeedsValidation, value);
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x001E131D File Offset: 0x001E031D
		// (set) Token: 0x06003661 RID: 13921 RVA: 0x001E1326 File Offset: 0x001E0326
		internal bool NotifyOnTargetUpdated
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated, value);
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x001E1330 File Offset: 0x001E0330
		// (set) Token: 0x06003663 RID: 13923 RVA: 0x001E133D File Offset: 0x001E033D
		internal bool NotifyOnSourceUpdated
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated, value);
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x001E134B File Offset: 0x001E034B
		// (set) Token: 0x06003665 RID: 13925 RVA: 0x001E1358 File Offset: 0x001E0358
		internal bool NotifyOnValidationError
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iNotifyOnValidationError);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iNotifyOnValidationError, value);
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x001E1366 File Offset: 0x001E0366
		internal bool IsInPriorityBindingExpression
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression);
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06003667 RID: 13927 RVA: 0x001E1373 File Offset: 0x001E0373
		internal bool IsInMultiBindingExpression
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInMultiBindingExpression);
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x001E1380 File Offset: 0x001E0380
		internal bool IsInBindingExpressionCollection
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression);
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x06003669 RID: 13929 RVA: 0x001E138D File Offset: 0x001E038D
		internal bool ValidatesOnExceptions
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnExceptions);
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x0600366A RID: 13930 RVA: 0x001E139A File Offset: 0x001E039A
		internal bool ValidatesOnDataErrors
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors);
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x0600366B RID: 13931 RVA: 0x001E13A7 File Offset: 0x001E03A7
		// (set) Token: 0x0600366C RID: 13932 RVA: 0x001E13B4 File Offset: 0x001E03B4
		internal bool TargetWantsCrossThreadNotifications
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification, value);
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x001E13C2 File Offset: 0x001E03C2
		// (set) Token: 0x0600366E RID: 13934 RVA: 0x001E13CF File Offset: 0x001E03CF
		internal bool IsDataErrorsChangedPending
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending);
			}
			set
			{
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending, value);
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x001E13DD File Offset: 0x001E03DD
		// (set) Token: 0x06003670 RID: 13936 RVA: 0x001E13EA File Offset: 0x001E03EA
		internal bool IsUpdateDeferredForComposition
		{
			get
			{
				return this.TestFlag(~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending));
			}
			set
			{
				this.ChangeFlag(~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending), value);
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x001E13F8 File Offset: 0x001E03F8
		internal bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return this.TestFlag(BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors);
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06003672 RID: 13938 RVA: 0x001E1405 File Offset: 0x001E0405
		internal MultiBindingExpression ParentMultiBindingExpression
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null) as MultiBindingExpression;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x001E1414 File Offset: 0x001E0414
		internal PriorityBindingExpression ParentPriorityBindingExpression
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null) as PriorityBindingExpression;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06003674 RID: 13940 RVA: 0x001E1423 File Offset: 0x001E0423
		internal BindingExpressionBase ParentBindingExpressionBase
		{
			get
			{
				return (BindingExpressionBase)this.GetValue(BindingExpressionBase.Feature.ParentBindingExpressionBase, null);
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x001E1432 File Offset: 0x001E0432
		internal object FallbackValue
		{
			get
			{
				return BindingExpressionBase.ConvertFallbackValue(this.ParentBindingBase.FallbackValue, this.TargetProperty, this);
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x001E144C File Offset: 0x001E044C
		internal object DefaultValue
		{
			get
			{
				DependencyObject targetElement = this.TargetElement;
				if (targetElement != null)
				{
					return this.TargetProperty.GetDefaultValue(targetElement.DependencyObjectType);
				}
				return DependencyProperty.UnsetValue;
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06003677 RID: 13943 RVA: 0x001E147A File Offset: 0x001E047A
		internal string EffectiveStringFormat
		{
			get
			{
				return (string)this.GetValue(BindingExpressionBase.Feature.EffectiveStringFormat, null);
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x001E1489 File Offset: 0x001E0489
		internal object EffectiveTargetNullValue
		{
			get
			{
				return this.GetValue(BindingExpressionBase.Feature.EffectiveTargetNullValue, DependencyProperty.UnsetValue);
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06003679 RID: 13945 RVA: 0x001E1498 File Offset: 0x001E0498
		internal BindingExpressionBase RootBindingExpression
		{
			get
			{
				BindingExpressionBase bindingExpressionBase = this;
				for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = bindingExpressionBase.ParentBindingExpressionBase)
				{
					bindingExpressionBase = parentBindingExpressionBase;
				}
				return bindingExpressionBase;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x0600367A RID: 13946 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x0600367B RID: 13947 RVA: 0x001E14BD File Offset: 0x001E04BD
		internal bool UsesLanguage
		{
			get
			{
				return this.ParentBindingBase.ConverterCultureInternal == null;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x001E14D0 File Offset: 0x001E04D0
		internal bool IsEligibleForCommit
		{
			get
			{
				if (this.IsDetaching)
				{
					return false;
				}
				switch (this.StatusInternal)
				{
				case BindingStatusInternal.Unattached:
				case BindingStatusInternal.Inactive:
				case BindingStatusInternal.Detached:
				case BindingStatusInternal.PathError:
					return false;
				}
				return true;
			}
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x001E1514 File Offset: 0x001E0514
		internal virtual bool AttachOverride(DependencyObject target, DependencyProperty dp)
		{
			this._targetElement = new WeakReference(target);
			this._targetProperty = dp;
			DataBindEngine currentDataBindEngine = DataBindEngine.CurrentDataBindEngine;
			if (currentDataBindEngine == null || currentDataBindEngine.IsShutDown)
			{
				return false;
			}
			this._engine = currentDataBindEngine;
			this.DetermineEffectiveStringFormat();
			this.DetermineEffectiveTargetNullValue();
			this.DetermineEffectiveUpdateBehavior();
			this.DetermineEffectiveValidatesOnNotifyDataErrors();
			if (dp == TextBox.TextProperty && this.IsReflective && !this.IsInBindingExpressionCollection)
			{
				TextBoxBase textBoxBase = target as TextBoxBase;
				if (textBoxBase != null)
				{
					textBoxBase.PreviewTextInput += this.OnPreviewTextInput;
				}
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.AttachExpression(new object[]
				{
					TraceData.Identify(this),
					target.GetType().FullName,
					dp.Name,
					AvTrace.GetHashCodeHelper(target)
				}), this);
			}
			return true;
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x001E15E4 File Offset: 0x001E05E4
		internal virtual void DetachOverride()
		{
			this.UpdateValidationError(null, false);
			this.UpdateNotifyDataErrorValidationErrors(null);
			if (this.TargetProperty == TextBox.TextProperty && this.IsReflective && !this.IsInBindingExpressionCollection)
			{
				TextBoxBase textBoxBase = this.TargetElement as TextBoxBase;
				if (textBoxBase != null)
				{
					textBoxBase.PreviewTextInput -= this.OnPreviewTextInput;
				}
			}
			this._engine = null;
			this._targetElement = null;
			this._targetProperty = null;
			this.SetStatus(BindingStatusInternal.Detached);
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.DetachExpression(new object[]
				{
					TraceData.Identify(this)
				}), this);
			}
		}

		// Token: 0x0600367F RID: 13951
		internal abstract void InvalidateChild(BindingExpressionBase bindingExpression);

		// Token: 0x06003680 RID: 13952
		internal abstract void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources);

		// Token: 0x06003681 RID: 13953
		internal abstract void ReplaceChild(BindingExpressionBase bindingExpression);

		// Token: 0x06003682 RID: 13954
		internal abstract void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args);

		// Token: 0x06003683 RID: 13955 RVA: 0x001E1680 File Offset: 0x001E0680
		private object HandlePropertyInvalidationOperation(object o)
		{
			object[] array = (object[])o;
			this.HandlePropertyInvalidation((DependencyObject)array[0], (DependencyPropertyChangedEventArgs)array[1]);
			return null;
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x001E16AC File Offset: 0x001E06AC
		internal void OnBindingGroupChanged(bool joining)
		{
			if (joining)
			{
				if (this.IsParentBindingUpdateTriggerDefault)
				{
					if (this.IsUpdateOnLostFocus)
					{
						LostFocusEventManager.RemoveHandler(this.TargetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
					}
					this.SetUpdateSourceTrigger(UpdateSourceTrigger.Explicit);
					return;
				}
			}
			else if (this.IsParentBindingUpdateTriggerDefault)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.RestoreUpdateTriggerOperation), null);
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x001E1710 File Offset: 0x001E0710
		private object RestoreUpdateTriggerOperation(object arg)
		{
			DependencyObject targetElement = this.TargetElement;
			if (!this.IsDetached && targetElement != null)
			{
				FrameworkPropertyMetadata fwMetaData = this.TargetProperty.GetMetadata(targetElement.DependencyObjectType) as FrameworkPropertyMetadata;
				UpdateSourceTrigger defaultUpdateSourceTrigger = this.GetDefaultUpdateSourceTrigger(fwMetaData);
				this.SetUpdateSourceTrigger(defaultUpdateSourceTrigger);
				if (this.IsUpdateOnLostFocus)
				{
					LostFocusEventManager.AddHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
				}
			}
			return null;
		}

		// Token: 0x06003686 RID: 13958
		internal abstract void UpdateBindingGroup(BindingGroup bg);

		// Token: 0x06003687 RID: 13959 RVA: 0x001E1774 File Offset: 0x001E0774
		internal bool UpdateValue()
		{
			ValidationError baseValidationError = this.BaseValidationError;
			if (this.StatusInternal == BindingStatusInternal.UpdateSourceError)
			{
				this.SetStatus(BindingStatusInternal.Active);
			}
			object obj = this.GetRawProposedValue();
			if (!this.Validate(obj, ValidationStep.RawProposedValue))
			{
				return false;
			}
			obj = this.ConvertProposedValue(obj);
			if (!this.Validate(obj, ValidationStep.ConvertedProposedValue))
			{
				return false;
			}
			obj = this.UpdateSource(obj);
			if (!this.Validate(obj, ValidationStep.UpdatedValue))
			{
				return false;
			}
			obj = this.CommitSource(obj);
			if (!this.Validate(obj, ValidationStep.CommittedValue))
			{
				return false;
			}
			if (this.BaseValidationError == baseValidationError)
			{
				this.UpdateValidationError(null, false);
			}
			this.EndSourceUpdate();
			this.NotifyCommitManager();
			return !this.HasValue(BindingExpressionBase.Feature.ValidationError);
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x001E1810 File Offset: 0x001E0810
		internal virtual object GetRawProposedValue()
		{
			object obj = this.Value;
			if (ItemsControl.EqualsEx(obj, this.EffectiveTargetNullValue))
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06003689 RID: 13961
		internal abstract object ConvertProposedValue(object rawValue);

		// Token: 0x0600368A RID: 13962
		internal abstract bool ObtainConvertedProposedValue(BindingGroup bindingGroup);

		// Token: 0x0600368B RID: 13963
		internal abstract object UpdateSource(object convertedValue);

		// Token: 0x0600368C RID: 13964
		internal abstract bool UpdateSource(BindingGroup bindingGroup);

		// Token: 0x0600368D RID: 13965 RVA: 0x001136C4 File Offset: 0x001126C4
		internal virtual object CommitSource(object value)
		{
			return value;
		}

		// Token: 0x0600368E RID: 13966
		internal abstract void StoreValueInBindingGroup(object value, BindingGroup bindingGroup);

		// Token: 0x0600368F RID: 13967 RVA: 0x001E1838 File Offset: 0x001E0838
		internal virtual bool Validate(object value, ValidationStep validationStep)
		{
			if (value == Binding.DoNothing)
			{
				return true;
			}
			if (value == DependencyProperty.UnsetValue)
			{
				this.SetStatus(BindingStatusInternal.UpdateSourceError);
				return false;
			}
			ValidationError validationError = this.GetValidationErrors(validationStep);
			if (validationError != null && validationError.RuleInError == DataErrorValidationRule.Instance)
			{
				validationError = null;
			}
			Collection<ValidationRule> validationRulesInternal = this.ParentBindingBase.ValidationRulesInternal;
			if (validationRulesInternal != null)
			{
				CultureInfo culture = this.GetCulture();
				foreach (ValidationRule validationRule in validationRulesInternal)
				{
					if (validationRule.ValidationStep == validationStep)
					{
						ValidationResult validationResult = validationRule.Validate(value, culture, this);
						if (!validationResult.IsValid)
						{
							if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
							{
								TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ValidationRuleFailed(new object[]
								{
									TraceData.Identify(this),
									TraceData.Identify(validationRule)
								}), this);
							}
							this.UpdateValidationError(new ValidationError(validationRule, this, validationResult.ErrorContent, null), false);
							return false;
						}
					}
				}
			}
			if (validationError != null && validationError == this.GetValidationErrors(validationStep))
			{
				this.UpdateValidationError(null, false);
			}
			return true;
		}

		// Token: 0x06003690 RID: 13968
		internal abstract bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep);

		// Token: 0x06003691 RID: 13969
		internal abstract bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values);

		// Token: 0x06003692 RID: 13970 RVA: 0x001E194C File Offset: 0x001E094C
		internal CultureInfo GetCulture()
		{
			if (this._culture == BindingExpressionBase.DefaultValueObject)
			{
				this._culture = this.ParentBindingBase.ConverterCultureInternal;
				if (this._culture == null)
				{
					DependencyObject targetElement = this.TargetElement;
					if (targetElement != null)
					{
						if (this.IsInTransfer && this.TargetProperty == FrameworkElement.LanguageProperty)
						{
							if (TraceData.IsEnabled)
							{
								TraceData.TraceAndNotify(TraceEventType.Critical, TraceData.RequiresExplicitCulture, this, new object[]
								{
									this.TargetProperty.Name,
									this
								}, null);
							}
							throw new InvalidOperationException(SR.Get("RequiresExplicitCulture", new object[]
							{
								this.TargetProperty.Name
							}));
						}
						this._culture = ((XmlLanguage)targetElement.GetValue(FrameworkElement.LanguageProperty)).GetSpecificCulture();
					}
				}
			}
			return (CultureInfo)this._culture;
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x001E1A1D File Offset: 0x001E0A1D
		internal void InvalidateCulture()
		{
			this._culture = BindingExpressionBase.DefaultValueObject;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x001E1A2A File Offset: 0x001E0A2A
		internal void BeginSourceUpdate()
		{
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate, true);
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x001E1A38 File Offset: 0x001E0A38
		internal void EndSourceUpdate()
		{
			if (this.IsInUpdate && this.IsDynamic && this.StatusInternal == BindingStatusInternal.Active)
			{
				TextBoxBase textBoxBase = this.Target as TextBoxBase;
				UndoManager undoManager = (textBoxBase == null) ? null : textBoxBase.TextContainer.UndoManager;
				if (undoManager != null && undoManager.OpenedUnit != null && undoManager.OpenedUnit.GetType() != typeof(TextParentUndoUnit))
				{
					if (!this.HasValue(BindingExpressionBase.Feature.UpdateTargetOperation))
					{
						DispatcherOperation value = this.Dispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(this.UpdateTargetCallback), null);
						this.SetValue(BindingExpressionBase.Feature.UpdateTargetOperation, value);
					}
				}
				else
				{
					this.UpdateTarget();
				}
			}
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iNeedsUpdate, false);
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x001E1AE9 File Offset: 0x001E0AE9
		private object UpdateTargetCallback(object unused)
		{
			this.ClearValue(BindingExpressionBase.Feature.UpdateTargetOperation);
			this.IsInUpdate = true;
			this.UpdateTarget();
			this.IsInUpdate = false;
			return null;
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x001E1B08 File Offset: 0x001E0B08
		internal bool ShouldUpdateWithCurrentValue(DependencyObject target, out object currentValue)
		{
			if (this.IsReflective)
			{
				FrameworkObject frameworkObject = new FrameworkObject(target);
				if (!frameworkObject.IsInitialized)
				{
					DependencyProperty targetProperty = this.TargetProperty;
					EntryIndex entryIndex = target.LookupEntry(targetProperty.GlobalIndex);
					if (entryIndex.Found)
					{
						EffectiveValueEntry valueEntry = target.GetValueEntry(entryIndex, targetProperty, null, RequestFlags.RawEntry);
						if (valueEntry.IsCoercedWithCurrentValue)
						{
							currentValue = valueEntry.GetFlattenedEntry(RequestFlags.FullyResolved).Value;
							if (valueEntry.IsDeferredReference)
							{
								DeferredReference deferredReference = (DeferredReference)currentValue;
								currentValue = deferredReference.GetValue(valueEntry.BaseValueSourceInternal);
							}
							return true;
						}
					}
				}
			}
			currentValue = null;
			return false;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x001E1B9C File Offset: 0x001E0B9C
		internal void ChangeValue(object newValue, bool notify)
		{
			object oldValue = (this._value != BindingExpressionBase.DefaultValueObject) ? this._value : DependencyProperty.UnsetValue;
			this._value = newValue;
			if (notify && this.ValueChanged != null)
			{
				this.ValueChanged(this, new BindingValueChangedEventArgs(oldValue, newValue));
			}
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x001E1BE9 File Offset: 0x001E0BE9
		internal void Clean()
		{
			if (this.NeedsUpdate)
			{
				this.NeedsUpdate = false;
			}
			if (!this.IsInUpdate)
			{
				this.NeedsValidation = false;
				this.NotifyCommitManager();
			}
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x001E1C0F File Offset: 0x001E0C0F
		internal void Dirty()
		{
			if (this.ShouldReactToDirty())
			{
				this.NeedsUpdate = true;
				if (!this.HasValue(BindingExpressionBase.Feature.Timer))
				{
					this.ProcessDirty();
				}
				else
				{
					DispatcherTimer dispatcherTimer = (DispatcherTimer)this.GetValue(BindingExpressionBase.Feature.Timer, null);
					dispatcherTimer.Stop();
					dispatcherTimer.Start();
				}
				this.NotifyCommitManager();
			}
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x001E1C4F File Offset: 0x001E0C4F
		private bool ShouldReactToDirty()
		{
			return !this.IsInTransfer && this.IsAttached && this.ShouldReactToDirtyOverride();
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool ShouldReactToDirtyOverride()
		{
			return true;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001E1C69 File Offset: 0x001E0C69
		private void ProcessDirty()
		{
			if (this.IsUpdateOnPropertyChanged)
			{
				if (Helper.IsComposing(this.Target, this.TargetProperty))
				{
					this.IsUpdateDeferredForComposition = true;
					return;
				}
				this.Update();
			}
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x001E1C95 File Offset: 0x001E0C95
		private void OnTimerTick(object sender, EventArgs e)
		{
			this.ProcessDirty();
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x001E1C9D File Offset: 0x001E0C9D
		private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if (this.IsUpdateDeferredForComposition && e.TextComposition.Source == this.TargetElement && e.TextComposition.Stage == TextCompositionStage.Done)
			{
				this.IsUpdateDeferredForComposition = false;
				this.Dirty();
			}
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x001E1CD8 File Offset: 0x001E0CD8
		internal void Invalidate(bool isASubPropertyChange)
		{
			if (this.IsAttaching)
			{
				return;
			}
			DependencyObject targetElement = this.TargetElement;
			if (targetElement != null)
			{
				if (this.IsInBindingExpressionCollection)
				{
					this.ParentBindingExpressionBase.InvalidateChild(this);
					return;
				}
				if (this.TargetProperty != BindingExpressionBase.NoTargetProperty)
				{
					if (!isASubPropertyChange)
					{
						targetElement.InvalidateProperty(this.TargetProperty);
						return;
					}
					targetElement.NotifySubPropertyChange(this.TargetProperty);
				}
			}
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x001E1D38 File Offset: 0x001E0D38
		internal object UseFallbackValue()
		{
			object obj = this.FallbackValue;
			if (obj == BindingExpressionBase.DefaultValueObject)
			{
				obj = DependencyProperty.UnsetValue;
			}
			if (obj == DependencyProperty.UnsetValue && this.IsOneWayToSource)
			{
				obj = this.DefaultValue;
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				this.UsingFallbackValue = true;
			}
			else
			{
				if (this.StatusInternal == BindingStatusInternal.Active)
				{
					this.SetStatus(BindingStatusInternal.UpdateTargetError);
				}
				if (!this.IsInBindingExpressionCollection)
				{
					obj = this.DefaultValue;
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Information, TraceData.NoValueToTransfer, this, null);
					}
				}
			}
			return obj;
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x001E1DB6 File Offset: 0x001E0DB6
		internal static bool IsNullValue(object value)
		{
			return value == null || Convert.IsDBNull(value) || SystemDataHelper.IsSqlNull(value);
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x001E1DD4 File Offset: 0x001E0DD4
		internal object NullValueForType(Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (SystemDataHelper.IsSqlNullableType(type))
			{
				return SystemDataHelper.NullValueForSqlNullableType(type);
			}
			if (!type.IsValueType)
			{
				return null;
			}
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return null;
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x001E1E2C File Offset: 0x001E0E2C
		internal ValidationRule LookupValidationRule(Type type)
		{
			ValidationRule validationRule = this.ParentBindingBase.GetValidationRule(type);
			if (validationRule == null && this.HasValue(BindingExpressionBase.Feature.ParentBindingExpressionBase))
			{
				validationRule = this.ParentBindingExpressionBase.LookupValidationRule(type);
			}
			return validationRule;
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x001E1E60 File Offset: 0x001E0E60
		internal void JoinBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			BindingGroup bindingGroup = this.RootBindingExpression.FindBindingGroup(isReflective, contextElement);
			if (bindingGroup != null)
			{
				this.JoinBindingGroup(bindingGroup, false);
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x001E1E88 File Offset: 0x001E0E88
		internal void LeaveBindingGroup()
		{
			BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
			BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
			if (bindingGroup != null)
			{
				bindingGroup.BindingExpressions.Remove(rootBindingExpression);
				rootBindingExpression.ClearValue(BindingExpressionBase.Feature.BindingGroup);
			}
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x001E1EBC File Offset: 0x001E0EBC
		internal void RejoinBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
			BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
			WeakReference<BindingGroup> weakReference = (WeakReference<BindingGroup>)rootBindingExpression.GetValue(BindingExpressionBase.Feature.BindingGroup, null);
			rootBindingExpression.SetValue(BindingExpressionBase.Feature.BindingGroup, null, weakReference);
			BindingGroup bindingGroup2;
			try
			{
				bindingGroup2 = rootBindingExpression.FindBindingGroup(isReflective, contextElement);
			}
			finally
			{
				rootBindingExpression.SetValue(BindingExpressionBase.Feature.BindingGroup, weakReference, null);
			}
			if (bindingGroup != bindingGroup2)
			{
				rootBindingExpression.LeaveBindingGroup();
				if (bindingGroup2 != null)
				{
					this.JoinBindingGroup(bindingGroup2, false);
				}
			}
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x001E1F28 File Offset: 0x001E0F28
		internal BindingGroup FindBindingGroup(bool isReflective, DependencyObject contextElement)
		{
			if ((WeakReference<BindingGroup>)this.GetValue(BindingExpressionBase.Feature.BindingGroup, null) != null)
			{
				return this.BindingGroup;
			}
			string bindingGroupName = this.ParentBindingBase.BindingGroupName;
			if (bindingGroupName == null)
			{
				this.MarkAsNonGrouped();
				return null;
			}
			if (!string.IsNullOrEmpty(bindingGroupName))
			{
				DependencyProperty bindingGroupProperty = FrameworkElement.BindingGroupProperty;
				FrameworkObject frameworkParent = new FrameworkObject(Helper.FindMentor(this.TargetElement));
				while (frameworkParent.DO != null)
				{
					BindingGroup bindingGroup = (BindingGroup)frameworkParent.DO.GetValue(bindingGroupProperty);
					if (bindingGroup == null)
					{
						this.MarkAsNonGrouped();
						return null;
					}
					if (bindingGroup.Name == bindingGroupName)
					{
						if (bindingGroup.SharesProposedValues && TraceData.IsEnabled)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.SharesProposedValuesRequriesImplicitBindingGroup(new object[]
							{
								TraceData.Identify(this),
								bindingGroupName,
								TraceData.Identify(bindingGroup)
							}), this);
						}
						return bindingGroup;
					}
					frameworkParent = frameworkParent.FrameworkParent;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.BindingGroupNameMatchFailed(new object[]
					{
						bindingGroupName
					}), this, null);
				}
				this.MarkAsNonGrouped();
				return null;
			}
			if (!isReflective || contextElement == null)
			{
				return null;
			}
			BindingGroup bindingGroup2 = (BindingGroup)contextElement.GetValue(FrameworkElement.BindingGroupProperty);
			if (bindingGroup2 == null)
			{
				this.MarkAsNonGrouped();
				return null;
			}
			DependencyProperty dataContextProperty = FrameworkElement.DataContextProperty;
			DependencyObject inheritanceContext = bindingGroup2.InheritanceContext;
			if (inheritanceContext == null || !ItemsControl.EqualsEx(contextElement.GetValue(dataContextProperty), inheritanceContext.GetValue(dataContextProperty)))
			{
				this.MarkAsNonGrouped();
				return null;
			}
			return bindingGroup2;
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x001E207C File Offset: 0x001E107C
		internal void JoinBindingGroup(BindingGroup bg, bool explicitJoin)
		{
			BindingExpressionBase bindingExpressionBase = null;
			for (BindingExpressionBase bindingExpressionBase2 = this; bindingExpressionBase2 != null; bindingExpressionBase2 = bindingExpressionBase2.ParentBindingExpressionBase)
			{
				bindingExpressionBase = bindingExpressionBase2;
				bindingExpressionBase2.OnBindingGroupChanged(true);
				bg.AddToValueTable(bindingExpressionBase2);
			}
			if (!bindingExpressionBase.HasValue(BindingExpressionBase.Feature.BindingGroup))
			{
				bindingExpressionBase.SetValue(BindingExpressionBase.Feature.BindingGroup, new WeakReference<BindingGroup>(bg));
				if (!explicitJoin || !bg.BindingExpressions.Contains(bindingExpressionBase))
				{
					bg.BindingExpressions.Add(bindingExpressionBase);
				}
				if (explicitJoin)
				{
					bindingExpressionBase.UpdateBindingGroup(bg);
					if (bg.SharesProposedValues && TraceData.IsEnabled)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.SharesProposedValuesRequriesImplicitBindingGroup(new object[]
						{
							TraceData.Identify(bindingExpressionBase),
							bindingExpressionBase.ParentBindingBase.BindingGroupName,
							TraceData.Identify(bg)
						}), this);
						return;
					}
				}
			}
			else if (bindingExpressionBase.BindingGroup != bg)
			{
				throw new InvalidOperationException(SR.Get("BindingGroup_CannotChangeGroups"));
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x001E2147 File Offset: 0x001E1147
		private void MarkAsNonGrouped()
		{
			if (!(this is BindingExpression))
			{
				this.SetValue(BindingExpressionBase.Feature.BindingGroup, BindingExpressionBase.NullBindingGroupReference);
			}
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x001E2160 File Offset: 0x001E1160
		internal void NotifyCommitManager()
		{
			if (this.IsReflective && !this.IsDetached && !this.Engine.IsShutDown)
			{
				bool flag = this.IsEligibleForCommit && (this.IsDirty || this.HasValidationError);
				BindingExpressionBase rootBindingExpression = this.RootBindingExpression;
				BindingGroup bindingGroup = rootBindingExpression.BindingGroup;
				rootBindingExpression.UpdateCommitState();
				if (bindingGroup == null)
				{
					if (rootBindingExpression != this && !flag)
					{
						flag = (rootBindingExpression.IsEligibleForCommit && (rootBindingExpression.IsDirty || rootBindingExpression.HasValidationError));
					}
					if (flag)
					{
						this.Engine.CommitManager.AddBinding(rootBindingExpression);
						return;
					}
					this.Engine.CommitManager.RemoveBinding(rootBindingExpression);
					return;
				}
				else
				{
					if (!flag)
					{
						flag = (bindingGroup.Owner != null && (bindingGroup.IsDirty || bindingGroup.HasValidationError));
					}
					if (flag)
					{
						this.Engine.CommitManager.AddBindingGroup(bindingGroup);
						return;
					}
					this.Engine.CommitManager.RemoveBindingGroup(bindingGroup);
				}
			}
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void UpdateCommitState()
		{
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x001E2258 File Offset: 0x001E1258
		internal void AdoptProperties(BindingExpressionBase bb)
		{
			BindingExpressionBase.PrivateFlags privateFlags = (bb != null) ? bb._flags : (~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition));
			this._flags = ((this._flags & ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iNeedsValidation)) | (privateFlags & BindingExpressionBase.PrivateFlags.iAdoptionMask));
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnLostFocus(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x060036B0 RID: 14000
		internal abstract object GetSourceItem(object newValue);

		// Token: 0x060036B1 RID: 14001 RVA: 0x001E228C File Offset: 0x001E128C
		private bool TestFlag(BindingExpressionBase.PrivateFlags flag)
		{
			return (this._flags & flag) > ~(BindingExpressionBase.PrivateFlags.iSourceToTarget | BindingExpressionBase.PrivateFlags.iTargetToSource | BindingExpressionBase.PrivateFlags.iPropDefault | BindingExpressionBase.PrivateFlags.iNotifyOnTargetUpdated | BindingExpressionBase.PrivateFlags.iDefaultValueConverter | BindingExpressionBase.PrivateFlags.iInTransfer | BindingExpressionBase.PrivateFlags.iInUpdate | BindingExpressionBase.PrivateFlags.iTransferPending | BindingExpressionBase.PrivateFlags.iNeedDataTransfer | BindingExpressionBase.PrivateFlags.iTransferDeferred | BindingExpressionBase.PrivateFlags.iUpdateOnLostFocus | BindingExpressionBase.PrivateFlags.iUpdateExplicitly | BindingExpressionBase.PrivateFlags.iNeedsUpdate | BindingExpressionBase.PrivateFlags.iPathGeneratedInternally | BindingExpressionBase.PrivateFlags.iUsingMentor | BindingExpressionBase.PrivateFlags.iResolveNamesInTemplate | BindingExpressionBase.PrivateFlags.iDetaching | BindingExpressionBase.PrivateFlags.iNeedsCollectionView | BindingExpressionBase.PrivateFlags.iInPriorityBindingExpression | BindingExpressionBase.PrivateFlags.iInMultiBindingExpression | BindingExpressionBase.PrivateFlags.iUsingFallbackValue | BindingExpressionBase.PrivateFlags.iNotifyOnValidationError | BindingExpressionBase.PrivateFlags.iAttaching | BindingExpressionBase.PrivateFlags.iNotifyOnSourceUpdated | BindingExpressionBase.PrivateFlags.iValidatesOnExceptions | BindingExpressionBase.PrivateFlags.iValidatesOnDataErrors | BindingExpressionBase.PrivateFlags.iIllegalInput | BindingExpressionBase.PrivateFlags.iNeedsValidation | BindingExpressionBase.PrivateFlags.iTargetWantsXTNotification | BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors | BindingExpressionBase.PrivateFlags.iDataErrorsChangedPending | BindingExpressionBase.PrivateFlags.iDeferUpdateForComposition);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x001E2299 File Offset: 0x001E1299
		private void ChangeFlag(BindingExpressionBase.PrivateFlags flag, bool value)
		{
			if (value)
			{
				this._flags |= flag;
				return;
			}
			this._flags &= ~flag;
		}

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x001E22BC File Offset: 0x001E12BC
		internal DependencyObject TargetElement
		{
			get
			{
				if (this._targetElement != null)
				{
					DependencyObject dependencyObject = this._targetElement.Target as DependencyObject;
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
					this._targetElement = null;
					this.Detach();
				}
				return null;
			}
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x001E22F5 File Offset: 0x001E12F5
		internal WeakReference TargetElementReference
		{
			get
			{
				return this._targetElement;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060036B5 RID: 14005 RVA: 0x001E22FD File Offset: 0x001E12FD
		internal DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x001E2305 File Offset: 0x001E1305
		internal Dispatcher Dispatcher
		{
			get
			{
				if (this._engine == null)
				{
					return null;
				}
				return this._engine.Dispatcher;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060036B7 RID: 14007 RVA: 0x001E231C File Offset: 0x001E131C
		// (set) Token: 0x060036B8 RID: 14008 RVA: 0x001E233E File Offset: 0x001E133E
		internal object Value
		{
			get
			{
				if (this._value == BindingExpressionBase.DefaultValueObject)
				{
					this.ChangeValue(this.UseFallbackValue(), false);
				}
				return this._value;
			}
			set
			{
				this.ChangeValue(value, true);
				this.Dirty();
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060036B9 RID: 14009 RVA: 0x001E234E File Offset: 0x001E134E
		internal WeakDependencySource[] WeakSources
		{
			get
			{
				return this._sources;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IsDisconnected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x001E2356 File Offset: 0x001E1356
		internal void Attach(DependencyObject target, DependencyProperty dp)
		{
			if (target != null)
			{
				target.VerifyAccess();
			}
			this.IsAttaching = true;
			this.AttachOverride(target, dp);
			this.IsAttaching = false;
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x001E2378 File Offset: 0x001E1378
		internal void Detach()
		{
			if (this.IsDetached || this.IsDetaching)
			{
				return;
			}
			this.IsDetaching = true;
			this.LeaveBindingGroup();
			this.NotifyCommitManager();
			this.DetachOverride();
			this.IsDetaching = false;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x001E23AC File Offset: 0x001E13AC
		internal virtual void Disconnect()
		{
			object obj = DependencyProperty.UnsetValue;
			DependencyProperty targetProperty = this.TargetProperty;
			if (targetProperty == ContentControl.ContentProperty || targetProperty == ContentPresenter.ContentProperty || targetProperty == HeaderedItemsControl.HeaderProperty || targetProperty == HeaderedContentControl.HeaderProperty)
			{
				obj = BindingExpressionBase.DisconnectedItem;
			}
			if (targetProperty.PropertyType == typeof(IEnumerable))
			{
				obj = null;
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				this.ChangeValue(obj, false);
				this.Invalidate(false);
			}
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x001E241C File Offset: 0x001E141C
		internal void SetStatus(BindingStatusInternal status)
		{
			if (this.IsDetached && status != this._status)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionStatusChanged", new object[]
				{
					this._status,
					status
				}));
			}
			this._status = status;
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x001E2470 File Offset: 0x001E1470
		internal static object ConvertFallbackValue(object value, DependencyProperty dp, object sender)
		{
			Exception exception;
			object obj = BindingExpressionBase.ConvertValue(value, dp, out exception);
			if (obj == BindingExpressionBase.DefaultValueObject && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.FallbackConversionFailed(new object[]
				{
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value),
					dp.Name,
					dp.PropertyType.Name
				}), sender as BindingExpressionBase, exception);
			}
			return obj;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x001E24D8 File Offset: 0x001E14D8
		internal static object ConvertTargetNullValue(object value, DependencyProperty dp, object sender)
		{
			Exception exception;
			object obj = BindingExpressionBase.ConvertValue(value, dp, out exception);
			if (obj == BindingExpressionBase.DefaultValueObject && TraceData.IsEnabled)
			{
				TraceData.TraceAndNotify(TraceEventType.Error, TraceData.TargetNullValueConversionFailed(new object[]
				{
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value),
					dp.Name,
					dp.PropertyType.Name
				}), sender as BindingExpressionBase, exception);
			}
			return obj;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x001E2540 File Offset: 0x001E1540
		private static object ConvertValue(object value, DependencyProperty dp, out Exception e)
		{
			e = null;
			object obj;
			if (value == DependencyProperty.UnsetValue || dp.IsValidValue(value))
			{
				obj = value;
			}
			else
			{
				obj = null;
				bool flag = false;
				TypeConverter converter = DefaultValueConverter.GetConverter(dp.PropertyType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
				{
					try
					{
						obj = converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
						flag = dp.IsValidValue(obj);
					}
					catch (Exception ex)
					{
						e = ex;
					}
					catch
					{
					}
				}
				if (!flag)
				{
					obj = BindingExpressionBase.DefaultValueObject;
				}
			}
			return obj;
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x001E25D0 File Offset: 0x001E15D0
		internal TraceEventType TraceLevel
		{
			get
			{
				if (this.ParentBindingBase.FallbackValue != DependencyProperty.UnsetValue)
				{
					return TraceEventType.Warning;
				}
				if (this.IsInBindingExpressionCollection)
				{
					return TraceEventType.Warning;
				}
				return TraceEventType.Error;
			}
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void Activate()
		{
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void Deactivate()
		{
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x001E25F1 File Offset: 0x001E15F1
		internal bool Update()
		{
			if (this.HasValue(BindingExpressionBase.Feature.Timer))
			{
				((DispatcherTimer)this.GetValue(BindingExpressionBase.Feature.Timer, null)).Stop();
			}
			return this.UpdateOverride();
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal virtual bool UpdateOverride()
		{
			return true;
		}

		// Token: 0x060036C7 RID: 14023 RVA: 0x001E2614 File Offset: 0x001E1614
		internal void UpdateValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			ValidationError baseValidationError = this.BaseValidationError;
			this.SetValue(BindingExpressionBase.Feature.ValidationError, validationError, null);
			if (validationError != null)
			{
				this.AddValidationError(validationError, skipBindingGroup);
			}
			if (baseValidationError != null)
			{
				this.RemoveValidationError(baseValidationError, skipBindingGroup);
			}
		}

		// Token: 0x060036C8 RID: 14024 RVA: 0x001E2648 File Offset: 0x001E1648
		internal void UpdateNotifyDataErrorValidationErrors(List<object> errors)
		{
			List<object> list;
			List<ValidationError> list2;
			BindingExpressionBase.GetValidationDelta(this.NotifyDataErrors, errors, out list, out list2);
			if (list != null && list.Count > 0)
			{
				ValidationRule instance = NotifyDataErrorValidationRule.Instance;
				List<ValidationError> list3 = this.NotifyDataErrors;
				if (list3 == null)
				{
					list3 = new List<ValidationError>();
					this.SetValue(BindingExpressionBase.Feature.NotifyDataErrors, list3);
				}
				foreach (object errorContent in list)
				{
					ValidationError validationError = new ValidationError(instance, this, errorContent, null);
					list3.Add(validationError);
					this.AddValidationError(validationError, false);
				}
			}
			if (list2 != null && list2.Count > 0)
			{
				List<ValidationError> notifyDataErrors = this.NotifyDataErrors;
				foreach (ValidationError validationError2 in list2)
				{
					notifyDataErrors.Remove(validationError2);
					this.RemoveValidationError(validationError2, false);
				}
				if (notifyDataErrors.Count == 0)
				{
					this.ClearValue(BindingExpressionBase.Feature.NotifyDataErrors);
				}
			}
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x001E2758 File Offset: 0x001E1758
		internal static void GetValidationDelta(List<ValidationError> previousErrors, List<object> errors, out List<object> toAdd, out List<ValidationError> toRemove)
		{
			if (previousErrors == null || previousErrors.Count == 0)
			{
				toAdd = errors;
				toRemove = null;
				return;
			}
			if (errors == null || errors.Count == 0)
			{
				toAdd = null;
				toRemove = new List<ValidationError>(previousErrors);
				return;
			}
			toAdd = new List<object>();
			toRemove = new List<ValidationError>(previousErrors);
			for (int i = errors.Count - 1; i >= 0; i--)
			{
				object obj = errors[i];
				int j;
				for (j = toRemove.Count - 1; j >= 0; j--)
				{
					if (ItemsControl.EqualsEx(toRemove[j].ErrorContent, obj))
					{
						toRemove.RemoveAt(j);
						break;
					}
				}
				if (j < 0)
				{
					toAdd.Add(obj);
				}
			}
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x001E27F8 File Offset: 0x001E17F8
		internal void AddValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			Validation.AddValidationError(validationError, this.TargetElement, this.NotifyOnValidationError);
			if (!skipBindingGroup)
			{
				BindingGroup bindingGroup = this.BindingGroup;
				if (bindingGroup != null)
				{
					bindingGroup.AddValidationError(validationError);
				}
			}
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x001E282C File Offset: 0x001E182C
		internal void RemoveValidationError(ValidationError validationError, bool skipBindingGroup = false)
		{
			Validation.RemoveValidationError(validationError, this.TargetElement, this.NotifyOnValidationError);
			if (!skipBindingGroup)
			{
				BindingGroup bindingGroup = this.BindingGroup;
				if (bindingGroup != null)
				{
					bindingGroup.RemoveValidationError(validationError);
				}
			}
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x001E2860 File Offset: 0x001E1860
		internal ValidationError GetValidationErrors(ValidationStep validationStep)
		{
			ValidationError baseValidationError = this.BaseValidationError;
			if (baseValidationError == null || baseValidationError.RuleInError.ValidationStep != validationStep)
			{
				return null;
			}
			return baseValidationError;
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x001E2888 File Offset: 0x001E1888
		internal void ChangeSources(WeakDependencySource[] newSources)
		{
			if (this.IsInBindingExpressionCollection)
			{
				this.ParentBindingExpressionBase.ChangeSourcesForChild(this, newSources);
			}
			else
			{
				this.ChangeSources(this.TargetElement, this.TargetProperty, newSources);
			}
			this._sources = newSources;
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x001E28BC File Offset: 0x001E18BC
		internal static WeakDependencySource[] CombineSources(int index, Collection<BindingExpressionBase> bindingExpressions, int count, WeakDependencySource[] newSources, WeakDependencySource[] commonSources = null)
		{
			if (index == count)
			{
				count++;
			}
			Collection<WeakDependencySource> collection = new Collection<WeakDependencySource>();
			if (commonSources != null)
			{
				for (int i = 0; i < commonSources.Length; i++)
				{
					collection.Add(commonSources[i]);
				}
			}
			for (int j = 0; j < count; j++)
			{
				BindingExpressionBase bindingExpressionBase = bindingExpressions[j];
				WeakDependencySource[] array = (j == index) ? newSources : ((bindingExpressionBase != null) ? bindingExpressionBase.WeakSources : null);
				int num = (array == null) ? 0 : array.Length;
				for (int k = 0; k < num; k++)
				{
					WeakDependencySource weakDependencySource = array[k];
					for (int l = 0; l < collection.Count; l++)
					{
						WeakDependencySource weakDependencySource2 = collection[l];
						if (weakDependencySource.DependencyObject == weakDependencySource2.DependencyObject && weakDependencySource.DependencyProperty == weakDependencySource2.DependencyProperty)
						{
							weakDependencySource = null;
							break;
						}
					}
					if (weakDependencySource != null)
					{
						collection.Add(weakDependencySource);
					}
				}
			}
			WeakDependencySource[] array2;
			if (collection.Count > 0)
			{
				array2 = new WeakDependencySource[collection.Count];
				collection.CopyTo(array2, 0);
				collection.Clear();
			}
			else
			{
				array2 = null;
			}
			return array2;
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x001E29C4 File Offset: 0x001E19C4
		internal void ResolvePropertyDefaultSettings(BindingMode mode, UpdateSourceTrigger updateTrigger, FrameworkPropertyMetadata fwMetaData)
		{
			if (mode == BindingMode.Default)
			{
				BindingExpressionBase.BindingFlags bindingFlags = BindingExpressionBase.BindingFlags.OneWay;
				if (fwMetaData != null && fwMetaData.BindsTwoWayByDefault)
				{
					bindingFlags = BindingExpressionBase.BindingFlags.TwoWay;
				}
				this.ChangeFlag(BindingExpressionBase.PrivateFlags.iPropagationMask, false);
				this.ChangeFlag((BindingExpressionBase.PrivateFlags)bindingFlags, true);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ResolveDefaultMode(new object[]
					{
						TraceData.Identify(this),
						(bindingFlags == BindingExpressionBase.BindingFlags.OneWay) ? BindingMode.OneWay : BindingMode.TwoWay
					}), this);
				}
			}
			if (updateTrigger == UpdateSourceTrigger.Default)
			{
				UpdateSourceTrigger defaultUpdateSourceTrigger = this.GetDefaultUpdateSourceTrigger(fwMetaData);
				this.SetUpdateSourceTrigger(defaultUpdateSourceTrigger);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ResolveDefaultUpdate(new object[]
					{
						TraceData.Identify(this),
						defaultUpdateSourceTrigger
					}), this);
				}
			}
			Invariant.Assert((this._flags & BindingExpressionBase.PrivateFlags.iUpdateDefault) != BindingExpressionBase.PrivateFlags.iUpdateDefault, "BindingExpression should not have Default update trigger");
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x001E2A87 File Offset: 0x001E1A87
		internal UpdateSourceTrigger GetDefaultUpdateSourceTrigger(FrameworkPropertyMetadata fwMetaData)
		{
			if (this.IsInMultiBindingExpression)
			{
				return UpdateSourceTrigger.Explicit;
			}
			if (fwMetaData == null)
			{
				return UpdateSourceTrigger.PropertyChanged;
			}
			return fwMetaData.DefaultUpdateSourceTrigger;
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x001E2A9E File Offset: 0x001E1A9E
		internal void SetUpdateSourceTrigger(UpdateSourceTrigger ust)
		{
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iUpdateDefault, false);
			this.ChangeFlag((BindingExpressionBase.PrivateFlags)BindingBase.FlagsFrom(ust), true);
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x001E2ABC File Offset: 0x001E1ABC
		internal Type GetEffectiveTargetType()
		{
			Type result = this.TargetProperty.PropertyType;
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					result = typeof(object);
					break;
				}
			}
			return result;
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x001E2B00 File Offset: 0x001E1B00
		internal void DetermineEffectiveStringFormat()
		{
			Type left = this.TargetProperty.PropertyType;
			if (left != typeof(string))
			{
				return;
			}
			string stringFormat = this.ParentBindingBase.StringFormat;
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					left = typeof(object);
					break;
				}
				if (stringFormat == null && parentBindingExpressionBase is PriorityBindingExpression)
				{
					stringFormat = parentBindingExpressionBase.ParentBindingBase.StringFormat;
				}
			}
			if (left == typeof(string) && !string.IsNullOrEmpty(stringFormat))
			{
				this.SetValue(BindingExpressionBase.Feature.EffectiveStringFormat, Helper.GetEffectiveStringFormat(stringFormat), null);
			}
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x001E2BA0 File Offset: 0x001E1BA0
		internal void DetermineEffectiveTargetNullValue()
		{
			Type propertyType = this.TargetProperty.PropertyType;
			object obj = this.ParentBindingBase.TargetNullValue;
			BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase;
			while (parentBindingExpressionBase != null && !(parentBindingExpressionBase is MultiBindingExpression))
			{
				if (obj == DependencyProperty.UnsetValue && parentBindingExpressionBase is PriorityBindingExpression)
				{
					obj = parentBindingExpressionBase.ParentBindingBase.TargetNullValue;
				}
				parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase;
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				obj = BindingExpressionBase.ConvertTargetNullValue(obj, this.TargetProperty, this);
				if (obj == BindingExpressionBase.DefaultValueObject)
				{
					obj = DependencyProperty.UnsetValue;
				}
			}
			this.SetValue(BindingExpressionBase.Feature.EffectiveTargetNullValue, obj, DependencyProperty.UnsetValue);
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x001E2C30 File Offset: 0x001E1C30
		private void DetermineEffectiveUpdateBehavior()
		{
			if (!this.IsReflective)
			{
				return;
			}
			for (BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase; parentBindingExpressionBase != null; parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase)
			{
				if (parentBindingExpressionBase is MultiBindingExpression)
				{
					return;
				}
			}
			int delay = this.ParentBindingBase.Delay;
			if (delay > 0 && this.IsUpdateOnPropertyChanged)
			{
				DispatcherTimer dispatcherTimer = new DispatcherTimer();
				this.SetValue(BindingExpressionBase.Feature.Timer, dispatcherTimer);
				dispatcherTimer.Interval = TimeSpan.FromMilliseconds((double)delay);
				dispatcherTimer.Tick += this.OnTimerTick;
			}
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x001E2CA8 File Offset: 0x001E1CA8
		internal void DetermineEffectiveValidatesOnNotifyDataErrors()
		{
			bool flag = this.ParentBindingBase.ValidatesOnNotifyDataErrorsInternal;
			BindingExpressionBase parentBindingExpressionBase = this.ParentBindingExpressionBase;
			while (flag && parentBindingExpressionBase != null)
			{
				flag = parentBindingExpressionBase.ValidatesOnNotifyDataErrors;
				parentBindingExpressionBase = parentBindingExpressionBase.ParentBindingExpressionBase;
			}
			this.ChangeFlag(BindingExpressionBase.PrivateFlags.iValidatesOnNotifyDataErrors, flag);
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x001E2CEA File Offset: 0x001E1CEA
		internal static object CreateReference(object item)
		{
			if (item != null && !(item is BindingListCollectionView) && item != BindingExpression.NullDataItem && item != BindingExpressionBase.DisconnectedItem)
			{
				item = new WeakReference(item);
			}
			return item;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x000F93D3 File Offset: 0x000F83D3
		internal static object CreateReference(WeakReference item)
		{
			return item;
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x001E2D10 File Offset: 0x001E1D10
		internal static object ReplaceReference(object oldReference, object item)
		{
			if (item != null && !(item is BindingListCollectionView) && item != BindingExpression.NullDataItem && item != BindingExpressionBase.DisconnectedItem)
			{
				WeakReference weakReference = oldReference as WeakReference;
				if (weakReference != null)
				{
					weakReference.Target = item;
					item = weakReference;
				}
				else
				{
					item = new WeakReference(item);
				}
			}
			return item;
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x001E2D58 File Offset: 0x001E1D58
		internal static object GetReference(object reference)
		{
			if (reference == null)
			{
				return null;
			}
			WeakReference weakReference = reference as WeakReference;
			if (weakReference != null)
			{
				return weakReference.Target;
			}
			return reference;
		}

		// Token: 0x060036DB RID: 14043 RVA: 0x001E2D7C File Offset: 0x001E1D7C
		internal static void InitializeTracing(BindingExpressionBase expr, DependencyObject d, DependencyProperty dp)
		{
			BindingBase parentBindingBase = expr.ParentBindingBase;
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x001E2D88 File Offset: 0x001E1D88
		private void ChangeSources(DependencyObject target, DependencyProperty dp, WeakDependencySource[] newSources)
		{
			DependencySource[] array;
			if (newSources != null)
			{
				array = new DependencySource[newSources.Length];
				int num = 0;
				for (int i = 0; i < newSources.Length; i++)
				{
					DependencyObject dependencyObject = newSources[i].DependencyObject;
					if (dependencyObject != null)
					{
						array[num++] = new DependencySource(dependencyObject, newSources[i].DependencyProperty);
					}
				}
				if (num < newSources.Length)
				{
					DependencySource[] array2;
					if (num > 0)
					{
						array2 = new DependencySource[num];
						Array.Copy(array, 0, array2, 0, num);
					}
					else
					{
						array2 = null;
					}
					array = array2;
				}
			}
			else
			{
				array = null;
			}
			base.ChangeSources(target, dp, array);
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x001E2E04 File Offset: 0x001E1E04
		internal bool HasValue(BindingExpressionBase.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x001E2E12 File Offset: 0x001E1E12
		internal object GetValue(BindingExpressionBase.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x001E2E21 File Offset: 0x001E1E21
		internal void SetValue(BindingExpressionBase.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x001E2E30 File Offset: 0x001E1E30
		internal void SetValue(BindingExpressionBase.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x001E2E55 File Offset: 0x001E1E55
		internal void ClearValue(BindingExpressionBase.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x04001CB9 RID: 7353
		internal static readonly DependencyProperty NoTargetProperty = DependencyProperty.RegisterAttached("NoTarget", typeof(object), typeof(BindingExpressionBase), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None));

		// Token: 0x04001CBA RID: 7354
		private BindingBase _binding;

		// Token: 0x04001CBB RID: 7355
		private WeakReference _targetElement;

		// Token: 0x04001CBC RID: 7356
		private DependencyProperty _targetProperty;

		// Token: 0x04001CBD RID: 7357
		private DataBindEngine _engine;

		// Token: 0x04001CBE RID: 7358
		private BindingExpressionBase.PrivateFlags _flags;

		// Token: 0x04001CBF RID: 7359
		private object _value = BindingExpressionBase.DefaultValueObject;

		// Token: 0x04001CC0 RID: 7360
		private BindingStatusInternal _status;

		// Token: 0x04001CC1 RID: 7361
		private WeakDependencySource[] _sources;

		// Token: 0x04001CC2 RID: 7362
		private object _culture = BindingExpressionBase.DefaultValueObject;

		// Token: 0x04001CC3 RID: 7363
		internal static readonly object DefaultValueObject = new NamedObject("DefaultValue");

		// Token: 0x04001CC4 RID: 7364
		internal static readonly object DisconnectedItem = new NamedObject("DisconnectedItem");

		// Token: 0x04001CC5 RID: 7365
		private static readonly WeakReference<BindingGroup> NullBindingGroupReference = new WeakReference<BindingGroup>(null);

		// Token: 0x04001CC6 RID: 7366
		private UncommonValueTable _values;

		// Token: 0x02000ACD RID: 2765
		[Flags]
		internal enum BindingFlags : uint
		{
			// Token: 0x04004699 RID: 18073
			OneWay = 1U,
			// Token: 0x0400469A RID: 18074
			TwoWay = 3U,
			// Token: 0x0400469B RID: 18075
			OneWayToSource = 2U,
			// Token: 0x0400469C RID: 18076
			OneTime = 0U,
			// Token: 0x0400469D RID: 18077
			PropDefault = 4U,
			// Token: 0x0400469E RID: 18078
			NotifyOnTargetUpdated = 8U,
			// Token: 0x0400469F RID: 18079
			NotifyOnSourceUpdated = 8388608U,
			// Token: 0x040046A0 RID: 18080
			NotifyOnValidationError = 2097152U,
			// Token: 0x040046A1 RID: 18081
			UpdateOnPropertyChanged = 0U,
			// Token: 0x040046A2 RID: 18082
			UpdateOnLostFocus = 1024U,
			// Token: 0x040046A3 RID: 18083
			UpdateExplicitly = 2048U,
			// Token: 0x040046A4 RID: 18084
			UpdateDefault = 3072U,
			// Token: 0x040046A5 RID: 18085
			PathGeneratedInternally = 8192U,
			// Token: 0x040046A6 RID: 18086
			ValidatesOnExceptions = 16777216U,
			// Token: 0x040046A7 RID: 18087
			ValidatesOnDataErrors = 33554432U,
			// Token: 0x040046A8 RID: 18088
			ValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x040046A9 RID: 18089
			Default = 3076U,
			// Token: 0x040046AA RID: 18090
			IllegalInput = 67108864U,
			// Token: 0x040046AB RID: 18091
			PropagationMask = 7U,
			// Token: 0x040046AC RID: 18092
			UpdateMask = 3072U
		}

		// Token: 0x02000ACE RID: 2766
		[Flags]
		private enum PrivateFlags : uint
		{
			// Token: 0x040046AE RID: 18094
			iSourceToTarget = 1U,
			// Token: 0x040046AF RID: 18095
			iTargetToSource = 2U,
			// Token: 0x040046B0 RID: 18096
			iPropDefault = 4U,
			// Token: 0x040046B1 RID: 18097
			iNotifyOnTargetUpdated = 8U,
			// Token: 0x040046B2 RID: 18098
			iDefaultValueConverter = 16U,
			// Token: 0x040046B3 RID: 18099
			iInTransfer = 32U,
			// Token: 0x040046B4 RID: 18100
			iInUpdate = 64U,
			// Token: 0x040046B5 RID: 18101
			iTransferPending = 128U,
			// Token: 0x040046B6 RID: 18102
			iNeedDataTransfer = 256U,
			// Token: 0x040046B7 RID: 18103
			iTransferDeferred = 512U,
			// Token: 0x040046B8 RID: 18104
			iUpdateOnLostFocus = 1024U,
			// Token: 0x040046B9 RID: 18105
			iUpdateExplicitly = 2048U,
			// Token: 0x040046BA RID: 18106
			iUpdateDefault = 3072U,
			// Token: 0x040046BB RID: 18107
			iNeedsUpdate = 4096U,
			// Token: 0x040046BC RID: 18108
			iPathGeneratedInternally = 8192U,
			// Token: 0x040046BD RID: 18109
			iUsingMentor = 16384U,
			// Token: 0x040046BE RID: 18110
			iResolveNamesInTemplate = 32768U,
			// Token: 0x040046BF RID: 18111
			iDetaching = 65536U,
			// Token: 0x040046C0 RID: 18112
			iNeedsCollectionView = 131072U,
			// Token: 0x040046C1 RID: 18113
			iInPriorityBindingExpression = 262144U,
			// Token: 0x040046C2 RID: 18114
			iInMultiBindingExpression = 524288U,
			// Token: 0x040046C3 RID: 18115
			iUsingFallbackValue = 1048576U,
			// Token: 0x040046C4 RID: 18116
			iNotifyOnValidationError = 2097152U,
			// Token: 0x040046C5 RID: 18117
			iAttaching = 4194304U,
			// Token: 0x040046C6 RID: 18118
			iNotifyOnSourceUpdated = 8388608U,
			// Token: 0x040046C7 RID: 18119
			iValidatesOnExceptions = 16777216U,
			// Token: 0x040046C8 RID: 18120
			iValidatesOnDataErrors = 33554432U,
			// Token: 0x040046C9 RID: 18121
			iIllegalInput = 67108864U,
			// Token: 0x040046CA RID: 18122
			iNeedsValidation = 134217728U,
			// Token: 0x040046CB RID: 18123
			iTargetWantsXTNotification = 268435456U,
			// Token: 0x040046CC RID: 18124
			iValidatesOnNotifyDataErrors = 536870912U,
			// Token: 0x040046CD RID: 18125
			iDataErrorsChangedPending = 1073741824U,
			// Token: 0x040046CE RID: 18126
			iDeferUpdateForComposition = 2147483648U,
			// Token: 0x040046CF RID: 18127
			iPropagationMask = 7U,
			// Token: 0x040046D0 RID: 18128
			iUpdateMask = 3072U,
			// Token: 0x040046D1 RID: 18129
			iAdoptionMask = 134221827U
		}

		// Token: 0x02000ACF RID: 2767
		internal class ProposedValue
		{
			// Token: 0x06008AF4 RID: 35572 RVA: 0x00338CDF File Offset: 0x00337CDF
			internal ProposedValue(BindingExpression bindingExpression, object rawValue, object convertedValue)
			{
				this._bindingExpression = bindingExpression;
				this._rawValue = rawValue;
				this._convertedValue = convertedValue;
			}

			// Token: 0x17001E67 RID: 7783
			// (get) Token: 0x06008AF5 RID: 35573 RVA: 0x00338CFC File Offset: 0x00337CFC
			internal BindingExpression BindingExpression
			{
				get
				{
					return this._bindingExpression;
				}
			}

			// Token: 0x17001E68 RID: 7784
			// (get) Token: 0x06008AF6 RID: 35574 RVA: 0x00338D04 File Offset: 0x00337D04
			internal object RawValue
			{
				get
				{
					return this._rawValue;
				}
			}

			// Token: 0x17001E69 RID: 7785
			// (get) Token: 0x06008AF7 RID: 35575 RVA: 0x00338D0C File Offset: 0x00337D0C
			internal object ConvertedValue
			{
				get
				{
					return this._convertedValue;
				}
			}

			// Token: 0x040046D2 RID: 18130
			private BindingExpression _bindingExpression;

			// Token: 0x040046D3 RID: 18131
			private object _rawValue;

			// Token: 0x040046D4 RID: 18132
			private object _convertedValue;
		}

		// Token: 0x02000AD0 RID: 2768
		internal enum Feature
		{
			// Token: 0x040046D6 RID: 18134
			ParentBindingExpressionBase,
			// Token: 0x040046D7 RID: 18135
			ValidationError,
			// Token: 0x040046D8 RID: 18136
			NotifyDataErrors,
			// Token: 0x040046D9 RID: 18137
			EffectiveStringFormat,
			// Token: 0x040046DA RID: 18138
			EffectiveTargetNullValue,
			// Token: 0x040046DB RID: 18139
			BindingGroup,
			// Token: 0x040046DC RID: 18140
			Timer,
			// Token: 0x040046DD RID: 18141
			UpdateTargetOperation,
			// Token: 0x040046DE RID: 18142
			Converter,
			// Token: 0x040046DF RID: 18143
			SourceType,
			// Token: 0x040046E0 RID: 18144
			DataProvider,
			// Token: 0x040046E1 RID: 18145
			CollectionViewSource,
			// Token: 0x040046E2 RID: 18146
			DynamicConverter,
			// Token: 0x040046E3 RID: 18147
			DataErrorValue,
			// Token: 0x040046E4 RID: 18148
			LastFeatureId
		}
	}
}
