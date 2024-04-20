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
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	// Token: 0x0200044D RID: 1101
	public sealed class BindingExpression : BindingExpressionBase, IDataBindEngineClient, IWeakEventListener
	{
		// Token: 0x060035BD RID: 13757 RVA: 0x001DE578 File Offset: 0x001DD578
		private BindingExpression(Binding binding, BindingExpressionBase owner) : base(binding, owner)
		{
			base.UseDefaultValueConverter = (this.ParentBinding.Converter == null);
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
			{
				PropertyPath path = binding.Path;
				string o = (path != null) ? path.Path : string.Empty;
				if (string.IsNullOrEmpty(binding.XPath))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.BindingPath(new object[]
					{
						TraceData.Identify(o)
					}), this);
					return;
				}
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.BindingXPathAndPath(new object[]
				{
					TraceData.Identify(binding.XPath),
					TraceData.Identify(o)
				}), this);
			}
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x001DE614 File Offset: 0x001DD614
		void IDataBindEngineClient.TransferValue()
		{
			this.TransferValue();
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x001DE61C File Offset: 0x001DD61C
		void IDataBindEngineClient.UpdateValue()
		{
			base.UpdateValue();
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x001DE625 File Offset: 0x001DD625
		bool IDataBindEngineClient.AttachToContext(bool lastChance)
		{
			this.AttachToContext(lastChance ? BindingExpression.AttachAttempt.Last : BindingExpression.AttachAttempt.Again);
			return base.StatusInternal > BindingStatusInternal.Unattached;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x001DE640 File Offset: 0x001DD640
		void IDataBindEngineClient.VerifySourceReference(bool lastChance)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			ObjectRef sourceReference = this.ParentBinding.SourceReference;
			DependencyObject d = (!base.UsingMentor) ? targetElement : Helper.FindMentor(targetElement);
			ObjectRefArgs args = new ObjectRefArgs
			{
				ResolveNamesInTemplate = base.ResolveNamesInTemplate
			};
			if (sourceReference.GetDataObject(d, args) != this.DataItem)
			{
				this.AttachToContext(lastChance ? BindingExpression.AttachAttempt.Last : BindingExpression.AttachAttempt.Again);
			}
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x001DE6A3 File Offset: 0x001DD6A3
		void IDataBindEngineClient.OnTargetUpdated()
		{
			this.OnTargetUpdated();
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x001DE6AB File Offset: 0x001DD6AB
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

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x060035C4 RID: 13764 RVA: 0x001DE6C7 File Offset: 0x001DD6C7
		public Binding ParentBinding
		{
			get
			{
				return (Binding)base.ParentBindingBase;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x001DE6D4 File Offset: 0x001DD6D4
		public object DataItem
		{
			get
			{
				return BindingExpressionBase.GetReference(this._dataItem);
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060035C6 RID: 13766 RVA: 0x001DE6E1 File Offset: 0x001DD6E1
		public object ResolvedSource
		{
			get
			{
				return this.SourceItem;
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x001DE6E9 File Offset: 0x001DD6E9
		public string ResolvedSourcePropertyName
		{
			get
			{
				return this.SourcePropertyName;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060035C8 RID: 13768 RVA: 0x001DE6F4 File Offset: 0x001DD6F4
		internal object DataSource
		{
			get
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement == null)
				{
					return null;
				}
				if (this._ctxElement != null)
				{
					return this.GetDataSourceForDataContext(this.ContextElement);
				}
				return this.ParentBinding.SourceReference.GetObject(targetElement, new ObjectRefArgs());
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x001DE738 File Offset: 0x001DD738
		public override void UpdateSource()
		{
			if (base.IsDetached)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			base.NeedsUpdate = true;
			base.Update();
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x001DE760 File Offset: 0x001DD760
		public override void UpdateTarget()
		{
			if (base.IsDetached)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			if (this.Worker != null)
			{
				this.Worker.RefreshValue();
			}
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001DE790 File Offset: 0x001DD790
		internal override void OnPropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			DependencyProperty property = args.Property;
			if (property == null)
			{
				throw new InvalidOperationException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"Property",
					"args"
				}));
			}
			bool flag = !this.IgnoreSourcePropertyChange;
			if (property == FrameworkElement.DataContextProperty && d == this.ContextElement)
			{
				flag = true;
			}
			else if (property == CollectionViewSource.ViewProperty && d == this.CollectionViewSource)
			{
				flag = true;
			}
			else if (property == FrameworkElement.LanguageProperty && base.UsesLanguage && d == base.TargetElement)
			{
				flag = true;
			}
			else if (flag)
			{
				flag = (this.Worker != null && this.Worker.UsesDependencyProperty(d, property));
			}
			if (!flag)
			{
				return;
			}
			base.OnPropertyInvalidation(d, args);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001DE855 File Offset: 0x001DD855
		internal override void UpdateBindingGroup(BindingGroup bg)
		{
			bg.UpdateTable(this);
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x060035D0 RID: 13776 RVA: 0x001DE85E File Offset: 0x001DD85E
		internal DependencyObject ContextElement
		{
			get
			{
				if (this._ctxElement != null)
				{
					return this._ctxElement.Target as DependencyObject;
				}
				return null;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x001DE87C File Offset: 0x001DD87C
		// (set) Token: 0x060035D2 RID: 13778 RVA: 0x001DE8A8 File Offset: 0x001DD8A8
		internal CollectionViewSource CollectionViewSource
		{
			get
			{
				WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.CollectionViewSource, null);
				if (weakReference != null)
				{
					return (CollectionViewSource)weakReference.Target;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					base.ClearValue(BindingExpressionBase.Feature.CollectionViewSource);
					return;
				}
				base.SetValue(BindingExpressionBase.Feature.CollectionViewSource, new WeakReference(value));
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x001DE8C4 File Offset: 0x001DD8C4
		internal bool IgnoreSourcePropertyChange
		{
			get
			{
				return base.IsTransferPending || base.IsInUpdate;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x001DE8D9 File Offset: 0x001DD8D9
		internal PropertyPath Path
		{
			get
			{
				return this.ParentBinding.Path;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x001DE8E6 File Offset: 0x001DD8E6
		// (set) Token: 0x060035D6 RID: 13782 RVA: 0x001DE8F5 File Offset: 0x001DD8F5
		internal IValueConverter Converter
		{
			get
			{
				return (IValueConverter)base.GetValue(BindingExpressionBase.Feature.Converter, null);
			}
			set
			{
				base.SetValue(BindingExpressionBase.Feature.Converter, value, null);
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x060035D7 RID: 13783 RVA: 0x001DE900 File Offset: 0x001DD900
		internal Type ConverterSourceType
		{
			get
			{
				return this._sourceType;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x001DE908 File Offset: 0x001DD908
		internal object SourceItem
		{
			get
			{
				if (this.Worker == null)
				{
					return null;
				}
				return this.Worker.SourceItem;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x060035D9 RID: 13785 RVA: 0x001DE91F File Offset: 0x001DD91F
		internal string SourcePropertyName
		{
			get
			{
				if (this.Worker == null)
				{
					return null;
				}
				return this.Worker.SourcePropertyName;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x001DE936 File Offset: 0x001DD936
		internal object SourceValue
		{
			get
			{
				if (this.Worker == null)
				{
					return DependencyProperty.UnsetValue;
				}
				return this.Worker.RawValue();
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x001DE951 File Offset: 0x001DD951
		internal override bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return this.ParentBinding.UpdateSourceTrigger == UpdateSourceTrigger.Default;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x001DE961 File Offset: 0x001DD961
		internal override bool IsDisconnected
		{
			get
			{
				return BindingExpressionBase.GetReference(this._dataItem) == BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x001DE978 File Offset: 0x001DD978
		internal static BindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, Binding binding, BindingExpressionBase parent)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			BindingExpression bindingExpression = new BindingExpression(binding, parent);
			bindingExpression.ResolvePropertyDefaultSettings(binding.Mode, binding.UpdateSourceTrigger, frameworkPropertyMetadata);
			if (bindingExpression.IsReflective && binding.XPath == null && (binding.Path == null || string.IsNullOrEmpty(binding.Path.Path)))
			{
				throw new InvalidOperationException(SR.Get("TwoWayBindingNeedsPath"));
			}
			return bindingExpression;
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001DEA24 File Offset: 0x001DDA24
		internal void SetupDefaultValueConverter(Type type)
		{
			if (!base.UseDefaultValueConverter)
			{
				return;
			}
			if (base.IsInMultiBindingExpression)
			{
				this.Converter = null;
				this._sourceType = type;
				return;
			}
			if (type != null && type != this._sourceType)
			{
				this._sourceType = type;
				IValueConverter valueConverter = base.Engine.GetDefaultValueConverter(type, base.TargetProperty.PropertyType, base.IsReflective);
				if (valueConverter == null && TraceData.IsEnabled)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Error, TraceData.CannotCreateDefaultValueConverter(new object[]
					{
						type,
						base.TargetProperty.PropertyType,
						base.IsReflective ? "two-way" : "one-way"
					}), this);
				}
				if (valueConverter == DefaultValueConverter.ValueConverterNotNeeded)
				{
					valueConverter = null;
				}
				this.Converter = valueConverter;
			}
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001DEAE8 File Offset: 0x001DDAE8
		internal static bool HasLocalDataContext(DependencyObject d)
		{
			bool flag;
			BaseValueSourceInternal valueSource = d.GetValueSource(FrameworkElement.DataContextProperty, null, out flag);
			return valueSource != BaseValueSourceInternal.Inherited && (valueSource != BaseValueSourceInternal.Default || flag);
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x060035E0 RID: 13792 RVA: 0x001DEB13 File Offset: 0x001DDB13
		private bool CanActivate
		{
			get
			{
				return base.StatusInternal > BindingStatusInternal.Unattached;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x060035E1 RID: 13793 RVA: 0x001DEB1E File Offset: 0x001DDB1E
		private BindingWorker Worker
		{
			get
			{
				return this._worker;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x060035E2 RID: 13794 RVA: 0x001DEB28 File Offset: 0x001DDB28
		private DynamicValueConverter DynamicConverter
		{
			get
			{
				if (!base.HasValue(BindingExpressionBase.Feature.DynamicConverter))
				{
					Invariant.Assert(this.Worker != null);
					base.SetValue(BindingExpressionBase.Feature.DynamicConverter, new DynamicValueConverter(base.IsReflective, this.Worker.SourcePropertyType, this.Worker.TargetPropertyType), null);
				}
				return (DynamicValueConverter)base.GetValue(BindingExpressionBase.Feature.DynamicConverter, null);
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x060035E3 RID: 13795 RVA: 0x001DEB85 File Offset: 0x001DDB85
		// (set) Token: 0x060035E4 RID: 13796 RVA: 0x001DEB95 File Offset: 0x001DDB95
		private DataSourceProvider DataProvider
		{
			get
			{
				return (DataSourceProvider)base.GetValue(BindingExpressionBase.Feature.DataProvider, null);
			}
			set
			{
				base.SetValue(BindingExpressionBase.Feature.DataProvider, value, null);
			}
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001DEBA4 File Offset: 0x001DDBA4
		internal override bool AttachOverride(DependencyObject target, DependencyProperty dp)
		{
			if (!base.AttachOverride(target, dp))
			{
				return false;
			}
			if (this.ParentBinding.SourceReference == null || this.ParentBinding.SourceReference.UsesMentor)
			{
				DependencyObject dependencyObject = Helper.FindMentor(target);
				if (dependencyObject != target)
				{
					InheritanceContextChangedEventManager.AddHandler(target, new EventHandler<EventArgs>(this.OnInheritanceContextChanged));
					base.UsingMentor = true;
					if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UseMentor(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(dependencyObject)
						}), this);
					}
				}
			}
			if (base.IsUpdateOnLostFocus)
			{
				Invariant.Assert(!base.IsInMultiBindingExpression, "Source BindingExpressions of a MultiBindingExpression should never be UpdateOnLostFocus.");
				LostFocusEventManager.AddHandler(target, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			this.AttachToContext(BindingExpression.AttachAttempt.First);
			if (base.StatusInternal == BindingStatusInternal.Unattached)
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
			GC.KeepAlive(target);
			return true;
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001DECA4 File Offset: 0x001DDCA4
		internal override void DetachOverride()
		{
			this.Deactivate();
			this.DetachFromContext();
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.RemoveHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			if (base.ValidatesOnNotifyDataErrors)
			{
				WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.DataErrorValue, null);
				INotifyDataErrorInfo notifyDataErrorInfo = (weakReference == null) ? null : (weakReference.Target as INotifyDataErrorInfo);
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
					base.SetValue(BindingExpressionBase.Feature.DataErrorValue, null, null);
				}
			}
			base.ChangeValue(DependencyProperty.UnsetValue, false);
			base.DetachOverride();
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001DED3C File Offset: 0x001DDD3C
		private void AttachToContext(BindingExpression.AttachAttempt attempt)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			bool isTracing = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			if (attempt == BindingExpression.AttachAttempt.First)
			{
				ObjectRef sourceReference = this.ParentBinding.SourceReference;
				if (sourceReference != null && sourceReference.TreeContextIsRequired(targetElement))
				{
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.SourceRequiresTreeContext(new object[]
						{
							TraceData.Identify(this),
							sourceReference.Identify()
						}), this);
					}
					return;
				}
			}
			bool flag2 = attempt == BindingExpression.AttachAttempt.Last;
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.AttachToContext(new object[]
				{
					TraceData.Identify(this),
					flag2 ? " (last chance)" : string.Empty
				}), this);
			}
			if (!flag2 && this.ParentBinding.TreeContextIsRequired && (targetElement.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty) == null || targetElement.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty) == null))
			{
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.PathRequiresTreeContext(new object[]
					{
						TraceData.Identify(this),
						this.ParentBinding.Path.Path
					}), this);
				}
				return;
			}
			DependencyObject dependencyObject = (!base.UsingMentor) ? targetElement : Helper.FindMentor(targetElement);
			if (dependencyObject == null)
			{
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.NoMentorExtended(new object[]
					{
						TraceData.Identify(this)
					}), this);
				}
				if (flag2)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.NoMentor, this, null);
					}
				}
				return;
			}
			DependencyObject dependencyObject2 = null;
			bool flag3 = true;
			if (this.ParentBinding.SourceReference == null)
			{
				dependencyObject2 = dependencyObject;
				CollectionViewSource collectionViewSource;
				if (base.TargetProperty == FrameworkElement.DataContextProperty || (base.TargetProperty == ContentPresenter.ContentProperty && targetElement is ContentPresenter) || (base.UsingMentor && (collectionViewSource = (targetElement as CollectionViewSource)) != null && collectionViewSource.PropertyForInheritanceContext == FrameworkElement.DataContextProperty))
				{
					dependencyObject2 = FrameworkElement.GetFrameworkParent(dependencyObject2);
					flag3 = (dependencyObject2 != null);
				}
			}
			else
			{
				RelativeObjectRef relativeObjectRef = this.ParentBinding.SourceReference as RelativeObjectRef;
				if (relativeObjectRef != null && relativeObjectRef.ReturnsDataContext)
				{
					object @object = relativeObjectRef.GetObject(dependencyObject, new ObjectRefArgs
					{
						IsTracing = isTracing
					});
					dependencyObject2 = (@object as DependencyObject);
					flag3 = (@object != DependencyProperty.UnsetValue);
				}
			}
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ContextElement(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(dependencyObject2),
					flag3 ? "OK" : "error"
				}), this);
			}
			if (!flag3)
			{
				if (flag2)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.NoDataContext, this, null);
					}
				}
				return;
			}
			object obj;
			ObjectRef sourceReference2;
			if (dependencyObject2 != null)
			{
				obj = dependencyObject2.GetValue(FrameworkElement.DataContextProperty);
				if (obj == null && !flag2 && !BindingExpression.HasLocalDataContext(dependencyObject2))
				{
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.NullDataContext(new object[]
						{
							TraceData.Identify(this)
						}), this);
					}
					return;
				}
			}
			else if ((sourceReference2 = this.ParentBinding.SourceReference) != null)
			{
				ObjectRefArgs objectRefArgs = new ObjectRefArgs
				{
					IsTracing = isTracing,
					ResolveNamesInTemplate = base.ResolveNamesInTemplate
				};
				obj = sourceReference2.GetDataObject(dependencyObject, objectRefArgs);
				if (obj == DependencyProperty.UnsetValue)
				{
					if (flag2)
					{
						base.SetStatus(BindingStatusInternal.PathError);
						if (TraceData.IsEnabled)
						{
							TraceData.TraceAndNotify(base.TraceLevel, TraceData.NoSource(new object[]
							{
								sourceReference2
							}), this, null);
						}
					}
					return;
				}
				if (!flag2 && objectRefArgs.NameResolvedInOuterScope)
				{
					base.Engine.AddTask(this, TaskOps.VerifySourceReference);
				}
			}
			else
			{
				obj = null;
			}
			if (dependencyObject2 != null)
			{
				this._ctxElement = new WeakReference(dependencyObject2);
			}
			this.ChangeWorkerSources(null, 0);
			if (!base.UseDefaultValueConverter)
			{
				this.Converter = this.ParentBinding.Converter;
				if (this.Converter == null)
				{
					throw new InvalidOperationException(SR.Get("MissingValueConverter"));
				}
			}
			base.JoinBindingGroup(base.IsReflective, dependencyObject2);
			base.SetStatus(BindingStatusInternal.Inactive);
			if (base.IsInPriorityBindingExpression)
			{
				base.ParentPriorityBindingExpression.InvalidateChild(this);
			}
			else
			{
				this.Activate(obj);
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x001DF0FC File Offset: 0x001DE0FC
		private void DetachFromContext()
		{
			if (base.HasValue(BindingExpressionBase.Feature.DataProvider))
			{
				DataChangedEventManager.RemoveHandler(this.DataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
			}
			if (!base.UseDefaultValueConverter)
			{
				this.Converter = null;
			}
			if (!base.IsInBindingExpressionCollection)
			{
				base.ChangeSources(null);
			}
			if (base.UsingMentor)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					InheritanceContextChangedEventManager.RemoveHandler(targetElement, new EventHandler<EventArgs>(this.OnInheritanceContextChanged));
				}
			}
			this._ctxElement = null;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001DF174 File Offset: 0x001DE174
		internal override void Activate()
		{
			if (!this.CanActivate)
			{
				return;
			}
			if (this._ctxElement == null)
			{
				if (base.StatusInternal == BindingStatusInternal.Inactive)
				{
					DependencyObject dependencyObject = base.TargetElement;
					if (dependencyObject != null)
					{
						if (base.UsingMentor)
						{
							dependencyObject = Helper.FindMentor(dependencyObject);
							if (dependencyObject == null)
							{
								base.SetStatus(BindingStatusInternal.PathError);
								if (TraceData.IsEnabled)
								{
									TraceData.TraceAndNotify(TraceEventType.Error, TraceData.NoMentor, this, null);
								}
								return;
							}
						}
						this.Activate(this.ParentBinding.SourceReference.GetDataObject(dependencyObject, new ObjectRefArgs
						{
							ResolveNamesInTemplate = base.ResolveNamesInTemplate
						}));
						return;
					}
				}
			}
			else
			{
				DependencyObject contextElement = this.ContextElement;
				if (contextElement == null)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.TraceAndNotify(TraceEventType.Error, TraceData.NoDataContext, this, null);
					}
					return;
				}
				object value = contextElement.GetValue(FrameworkElement.DataContextProperty);
				if (base.StatusInternal == BindingStatusInternal.Inactive || !ItemsControl.EqualsEx(value, this.DataItem))
				{
					this.Activate(value);
				}
			}
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001DF254 File Offset: 0x001DE254
		internal void Activate(object item)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (item == BindingExpressionBase.DisconnectedItem)
			{
				this.Disconnect();
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			this.Deactivate();
			if (!this.ParentBinding.BindsDirectlyToSource)
			{
				CollectionViewSource collectionViewSource = item as CollectionViewSource;
				this.CollectionViewSource = collectionViewSource;
				if (collectionViewSource != null)
				{
					item = collectionViewSource.CollectionView;
					this.ChangeWorkerSources(null, 0);
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UseCVS(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(collectionViewSource)
						}), this);
					}
				}
				else
				{
					item = this.DereferenceDataProvider(item);
				}
			}
			this._dataItem = BindingExpressionBase.CreateReference(item);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ActivateItem(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(item)
				}), this);
			}
			if (this.Worker == null)
			{
				this.CreateWorker();
			}
			base.SetStatus(BindingStatusInternal.Active);
			this.Worker.AttachDataItem();
			bool flag2 = base.IsOneWayToSource;
			object newValue;
			if (base.ShouldUpdateWithCurrentValue(targetElement, out newValue))
			{
				flag2 = true;
				base.ChangeValue(newValue, false);
				base.NeedsUpdate = true;
			}
			if (!flag2)
			{
				ValidationError validationError;
				object initialValue = this.GetInitialValue(targetElement, out validationError);
				bool flag3 = initialValue == BindingExpression.NullDataItem;
				if (!flag3)
				{
					this.TransferValue(initialValue, false);
				}
				if (validationError != null)
				{
					base.UpdateValidationError(validationError, flag3);
				}
			}
			else if (!base.IsInMultiBindingExpression)
			{
				base.UpdateValue();
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001DF3B0 File Offset: 0x001DE3B0
		private object GetInitialValue(DependencyObject target, out ValidationError error)
		{
			BindingGroup bindingGroup = base.RootBindingExpression.FindBindingGroup(true, this.ContextElement);
			BindingGroup.ProposedValueEntry proposedValueEntry;
			object obj;
			if (bindingGroup == null || (proposedValueEntry = bindingGroup.GetProposedValueEntry(this.SourceItem, this.SourcePropertyName)) == null)
			{
				error = null;
				obj = DependencyProperty.UnsetValue;
			}
			else
			{
				error = proposedValueEntry.ValidationError;
				if (base.IsReflective && base.TargetProperty.IsValidValue(proposedValueEntry.RawValue))
				{
					target.SetValue(base.TargetProperty, proposedValueEntry.RawValue);
					obj = BindingExpression.NullDataItem;
					bindingGroup.RemoveProposedValueEntry(proposedValueEntry);
				}
				else if (proposedValueEntry.ConvertedValue == DependencyProperty.UnsetValue)
				{
					obj = base.UseFallbackValue();
				}
				else
				{
					obj = proposedValueEntry.ConvertedValue;
				}
				if (obj != BindingExpression.NullDataItem)
				{
					bindingGroup.AddBindingForProposedValue(this, this.SourceItem, this.SourcePropertyName);
				}
			}
			return obj;
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001DF474 File Offset: 0x001DE474
		internal override void Deactivate()
		{
			if (base.StatusInternal == BindingStatusInternal.Inactive)
			{
				return;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.Deactivate(new object[]
				{
					TraceData.Identify(this)
				}), this);
			}
			this.CancelPendingTasks();
			if (this.Worker != null)
			{
				this.Worker.DetachDataItem();
			}
			base.ChangeValue(BindingExpressionBase.DefaultValueObject, false);
			this._dataItem = null;
			base.SetStatus(BindingStatusInternal.Inactive);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x001DF4E2 File Offset: 0x001DE4E2
		internal override void Disconnect()
		{
			this._dataItem = BindingExpressionBase.CreateReference(BindingExpressionBase.DisconnectedItem);
			if (this.Worker == null)
			{
				return;
			}
			this.Worker.AttachDataItem();
			base.Disconnect();
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001DF510 File Offset: 0x001DE510
		private object DereferenceDataProvider(object item)
		{
			DataSourceProvider dataSourceProvider = item as DataSourceProvider;
			DataSourceProvider dataSourceProvider2 = this.DataProvider;
			if (dataSourceProvider != dataSourceProvider2)
			{
				if (dataSourceProvider2 != null)
				{
					DataChangedEventManager.RemoveHandler(dataSourceProvider2, new EventHandler<EventArgs>(this.OnDataChanged));
				}
				this.DataProvider = dataSourceProvider;
				dataSourceProvider2 = dataSourceProvider;
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UseDataProvider(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(dataSourceProvider)
					}), this);
				}
				if (dataSourceProvider != null)
				{
					DataChangedEventManager.AddHandler(dataSourceProvider, new EventHandler<EventArgs>(this.OnDataChanged));
					dataSourceProvider.InitialLoad();
				}
			}
			if (dataSourceProvider2 == null)
			{
				return item;
			}
			return dataSourceProvider2.Data;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001DE6E1 File Offset: 0x001DD6E1
		internal override object GetSourceItem(object newValue)
		{
			return this.SourceItem;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001DF5A0 File Offset: 0x001DE5A0
		private void CreateWorker()
		{
			Invariant.Assert(this.Worker == null, "duplicate call to CreateWorker");
			this._worker = new ClrBindingWorker(this, base.Engine);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001DF5C8 File Offset: 0x001DE5C8
		internal void ChangeWorkerSources(WeakDependencySource[] newWorkerSources, int n)
		{
			int destinationIndex = 0;
			int num = n;
			DependencyObject contextElement = this.ContextElement;
			CollectionViewSource collectionViewSource = this.CollectionViewSource;
			bool usesLanguage = base.UsesLanguage;
			if (contextElement != null)
			{
				num++;
			}
			if (collectionViewSource != null)
			{
				num++;
			}
			if (usesLanguage)
			{
				num++;
			}
			WeakDependencySource[] array = (num > 0) ? new WeakDependencySource[num] : null;
			if (contextElement != null)
			{
				array[destinationIndex++] = new WeakDependencySource(this._ctxElement, FrameworkElement.DataContextProperty);
			}
			if (collectionViewSource != null)
			{
				WeakReference weakReference = base.GetValue(BindingExpressionBase.Feature.CollectionViewSource, null) as WeakReference;
				array[destinationIndex++] = ((weakReference != null) ? new WeakDependencySource(weakReference, CollectionViewSource.ViewProperty) : new WeakDependencySource(collectionViewSource, CollectionViewSource.ViewProperty));
			}
			if (usesLanguage)
			{
				array[destinationIndex++] = new WeakDependencySource(base.TargetElementReference, FrameworkElement.LanguageProperty);
			}
			if (n > 0)
			{
				Array.Copy(newWorkerSources, 0, array, destinationIndex, n);
			}
			base.ChangeSources(array);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001DF695 File Offset: 0x001DE695
		private void TransferValue()
		{
			this.TransferValue(DependencyProperty.UnsetValue, false);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001DF6A4 File Offset: 0x001DE6A4
		internal void TransferValue(object newValue, bool isASubPropertyChange)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (this.Worker == null)
			{
				return;
			}
			Type effectiveTargetType = base.GetEffectiveTargetType();
			IValueConverter valueConverter = null;
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			base.IsTransferPending = false;
			base.IsInTransfer = true;
			base.UsingFallbackValue = false;
			object obj = (newValue == DependencyProperty.UnsetValue) ? this.Worker.RawValue() : newValue;
			this.UpdateNotifyDataErrors(obj);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GetRawValue(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(obj)
				}), this);
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				bool flag2 = false;
				if (!base.UseDefaultValueConverter)
				{
					obj = this.Converter.Convert(obj, effectiveTargetType, this.ParentBinding.ConverterParameter, base.GetCulture());
					if (base.IsDetached)
					{
						return;
					}
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UserConverter(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}), this);
					}
					if (obj != null && obj != Binding.DoNothing && obj != DependencyProperty.UnsetValue && !effectiveTargetType.IsAssignableFrom(obj.GetType()))
					{
						valueConverter = this.DynamicConverter;
					}
				}
				else
				{
					valueConverter = this.Converter;
				}
				if (obj != Binding.DoNothing && obj != DependencyProperty.UnsetValue)
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
					else if (obj == DBNull.Value && (this.Converter == null || base.UseDefaultValueConverter))
					{
						if (effectiveTargetType.IsGenericType && effectiveTargetType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							obj = null;
						}
						else
						{
							obj = DependencyProperty.UnsetValue;
							flag2 = true;
						}
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ConvertDBNull(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
					}
					else if (valueConverter != null || base.EffectiveStringFormat != null)
					{
						obj = this.ConvertHelper(valueConverter, obj, effectiveTargetType, base.TargetElement, base.GetCulture());
						if (flag)
						{
							TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.DefaultConverter(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}), this);
						}
					}
				}
				if (!flag2 && obj == DependencyProperty.UnsetValue)
				{
					base.SetStatus(BindingStatusInternal.UpdateTargetError);
				}
			}
			if (obj != Binding.DoNothing)
			{
				if (!base.IsInMultiBindingExpression && obj != BindingExpression.IgnoreDefaultValue && obj != DependencyProperty.UnsetValue && !base.TargetProperty.IsValidValue(obj))
				{
					if (TraceData.IsEnabled && !base.IsInBindingExpressionCollection)
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
					if (base.StatusInternal == BindingStatusInternal.Active)
					{
						base.SetStatus(BindingStatusInternal.UpdateTargetError);
					}
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
				if (obj == BindingExpression.IgnoreDefaultValue)
				{
					obj = Expression.NoValue;
				}
				if (flag)
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.TransferValue(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(obj)
					}), this);
				}
				bool flag3 = !base.IsInUpdate || !ItemsControl.EqualsEx(obj, base.Value);
				if (flag3)
				{
					base.ChangeValue(obj, true);
					base.Invalidate(isASubPropertyChange);
					this.ValidateOnTargetUpdated();
				}
				base.Clean();
				if (flag3)
				{
					this.OnTargetUpdated();
				}
			}
			base.IsInTransfer = false;
			GC.KeepAlive(targetElement);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x001DFA74 File Offset: 0x001DEA74
		private void ValidateOnTargetUpdated()
		{
			ValidationError validationError = null;
			Collection<ValidationRule> validationRulesInternal = this.ParentBinding.ValidationRulesInternal;
			CultureInfo cultureInfo = null;
			bool flag = this.ParentBinding.ValidatesOnDataErrors;
			if (validationRulesInternal != null)
			{
				object obj = DependencyProperty.UnsetValue;
				object obj2 = DependencyProperty.UnsetValue;
				foreach (ValidationRule validationRule in validationRulesInternal)
				{
					if (validationRule.ValidatesOnTargetUpdated)
					{
						if (validationRule is DataErrorValidationRule)
						{
							flag = false;
						}
						ValidationStep validationStep = validationRule.ValidationStep;
						object value;
						if (validationStep != ValidationStep.RawProposedValue)
						{
							if (validationStep - ValidationStep.ConvertedProposedValue > 2)
							{
								throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
								{
									validationRule.ValidationStep,
									validationRule
								}));
							}
							if (obj2 == DependencyProperty.UnsetValue)
							{
								obj2 = this.Worker.RawValue();
							}
							value = obj2;
						}
						else
						{
							if (obj == DependencyProperty.UnsetValue)
							{
								obj = this.GetRawProposedValue();
							}
							value = obj;
						}
						if (cultureInfo == null)
						{
							cultureInfo = base.GetCulture();
						}
						validationError = this.RunValidationRule(validationRule, value, cultureInfo);
						if (validationError != null)
						{
							break;
						}
					}
				}
			}
			if (flag && validationError == null)
			{
				if (cultureInfo == null)
				{
					cultureInfo = base.GetCulture();
				}
				validationError = this.RunValidationRule(DataErrorValidationRule.Instance, this, cultureInfo);
			}
			base.UpdateValidationError(validationError, false);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x001DFBBC File Offset: 0x001DEBBC
		private ValidationError RunValidationRule(ValidationRule validationRule, object value, CultureInfo culture)
		{
			ValidationResult validationResult = validationRule.Validate(value, culture, this);
			ValidationError result;
			if (validationResult.IsValid)
			{
				result = null;
			}
			else
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
				{
					TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.ValidationRuleFailed(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(validationRule)
					}), this);
				}
				result = new ValidationError(validationRule, this, validationResult.ErrorContent, null);
			}
			return result;
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x001DFC1C File Offset: 0x001DEC1C
		private object ConvertHelper(IValueConverter converter, object value, Type targetType, object parameter, CultureInfo culture)
		{
			string effectiveStringFormat = base.EffectiveStringFormat;
			Invariant.Assert(converter != null || effectiveStringFormat != null);
			object result = null;
			try
			{
				if (effectiveStringFormat != null)
				{
					result = string.Format(culture, effectiveStringFormat, value);
				}
				else
				{
					result = converter.Convert(value, targetType, parameter, culture);
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					string text = string.IsNullOrEmpty(effectiveStringFormat) ? converter.GetType().Name : "StringFormat";
					TraceData.TraceAndNotify(base.TraceLevel, TraceData.BadConverterForTransfer(new object[]
					{
						text,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this, ex);
				}
				result = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(base.TraceLevel, TraceData.BadConverterForTransfer(new object[]
					{
						converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this, null);
				}
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x001DFD2C File Offset: 0x001DED2C
		private object ConvertBackHelper(IValueConverter converter, object value, Type sourceType, object parameter, CultureInfo culture)
		{
			Invariant.Assert(converter != null);
			object result = null;
			try
			{
				result = converter.ConvertBack(value, sourceType, parameter, culture);
			}
			catch (Exception ex)
			{
				ex = CriticalExceptions.Unwrap(ex);
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.BadConverterForUpdate(new object[]
					{
						AvTrace.ToStringHelper(base.Value),
						AvTrace.TypeName(value)
					}), this, ex);
				}
				this.ProcessException(ex, base.ValidatesOnExceptions);
				result = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.BadConverterForUpdate(new object[]
					{
						AvTrace.ToStringHelper(base.Value),
						AvTrace.TypeName(value)
					}), this, null);
				}
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x001DFE08 File Offset: 0x001DEE08
		internal void ScheduleTransfer(bool isASubPropertyChange)
		{
			if (isASubPropertyChange && this.Converter != null)
			{
				isASubPropertyChange = false;
			}
			this.TransferValue(DependencyProperty.UnsetValue, isASubPropertyChange);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x001DFE24 File Offset: 0x001DEE24
		private void OnTargetUpdated()
		{
			if (base.NotifyOnTargetUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null && ((!base.IsInMultiBindingExpression && (!base.IsInPriorityBindingExpression || this == base.ParentPriorityBindingExpression.ActiveBindingExpression)) || (base.IsAttaching && (base.StatusInternal == BindingStatusInternal.Active || base.UsingFallbackValue))))
				{
					if (base.IsAttaching && base.RootBindingExpression == targetElement.ReadLocalValue(base.TargetProperty))
					{
						base.Engine.AddTask(this, TaskOps.RaiseTargetUpdatedEvent);
						return;
					}
					BindingExpression.OnTargetUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x001DFEB0 File Offset: 0x001DEEB0
		private void OnSourceUpdated()
		{
			if (base.NotifyOnSourceUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null && !base.IsInMultiBindingExpression && (!base.IsInPriorityBindingExpression || this == base.ParentPriorityBindingExpression.ActiveBindingExpression))
				{
					BindingExpression.OnSourceUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x001DFEF9 File Offset: 0x001DEEF9
		internal override bool ShouldReactToDirtyOverride()
		{
			return this.DataItem != BindingExpressionBase.DisconnectedItem;
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x001DFF0B File Offset: 0x001DEF0B
		internal override bool UpdateOverride()
		{
			return !base.NeedsUpdate || !base.IsReflective || base.IsInTransfer || this.Worker == null || !this.Worker.CanUpdate || base.UpdateValue();
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x001DFF44 File Offset: 0x001DEF44
		internal override object ConvertProposedValue(object value)
		{
			object obj = value;
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UpdateRawValue(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(value)
				}), this);
			}
			Type sourcePropertyType = this.Worker.SourcePropertyType;
			IValueConverter valueConverter = null;
			CultureInfo culture = base.GetCulture();
			if (this.Converter != null)
			{
				if (!base.UseDefaultValueConverter)
				{
					value = this.Converter.ConvertBack(value, sourcePropertyType, this.ParentBinding.ConverterParameter, culture);
					if (base.IsDetached)
					{
						return Binding.DoNothing;
					}
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.UserConvertBack(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(value)
						}), this);
					}
					if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue && !this.IsValidValueForUpdate(value, sourcePropertyType))
					{
						valueConverter = this.DynamicConverter;
					}
				}
				else
				{
					valueConverter = this.Converter;
				}
			}
			if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue)
			{
				if (BindingExpressionBase.IsNullValue(value))
				{
					if (value == null || !this.IsValidValueForUpdate(value, sourcePropertyType))
					{
						if (this.Worker.IsDBNullValidForUpdate)
						{
							value = DBNull.Value;
						}
						else
						{
							value = base.NullValueForType(sourcePropertyType);
						}
					}
				}
				else if (valueConverter != null)
				{
					value = this.ConvertBackHelper(valueConverter, value, sourcePropertyType, base.TargetElement, culture);
					if (flag)
					{
						TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.DefaultConvertBack(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(value)
						}), this);
					}
				}
			}
			if (flag)
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.Update(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(value)
				}), this);
			}
			if (value == DependencyProperty.UnsetValue && this.ValidationError == null)
			{
				ValidationError validationError = new ValidationError(ConversionValidationRule.Instance, this, SR.Get("Validation_ConversionFailed", new object[]
				{
					obj
				}), null);
				base.UpdateValidationError(validationError, false);
			}
			return value;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x001E010C File Offset: 0x001DF10C
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			bool result = true;
			object obj;
			if (base.NeedsUpdate)
			{
				obj = bindingGroup.GetValue(this);
				if (obj != DependencyProperty.UnsetValue)
				{
					obj = this.ConvertProposedValue(obj);
					if (obj == DependencyProperty.UnsetValue)
					{
						result = false;
					}
				}
			}
			else
			{
				obj = BindingGroup.DeferredSourceValue;
			}
			bindingGroup.SetValue(this, obj);
			return result;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x001E0158 File Offset: 0x001DF158
		internal override object UpdateSource(object value)
		{
			if (value == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
			}
			if (value == Binding.DoNothing || value == DependencyProperty.UnsetValue || this.ShouldIgnoreUpdate())
			{
				return value;
			}
			try
			{
				base.BeginSourceUpdate();
				this.Worker.UpdateValue(value);
			}
			catch (Exception ex)
			{
				ex = CriticalExceptions.Unwrap(ex);
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.WorkerUpdateFailed, this, ex);
				}
				this.ProcessException(ex, base.ValidatesOnExceptions || base.BindingGroup != null);
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				value = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.TraceAndNotify(TraceEventType.Error, TraceData.WorkerUpdateFailed, this, null);
				}
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				value = DependencyProperty.UnsetValue;
			}
			finally
			{
				base.EndSourceUpdate();
			}
			this.OnSourceUpdated();
			return value;
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x001E0250 File Offset: 0x001DF250
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object obj = bindingGroup.GetValue(this);
				obj = this.UpdateSource(obj);
				bindingGroup.SetValue(this, obj);
				if (obj == DependencyProperty.UnsetValue)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x001E028A File Offset: 0x001DF28A
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			bindingGroup.SetValue(this, value);
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x001E0294 File Offset: 0x001DF294
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			bool flag = base.Validate(value, validationStep);
			if (validationStep == ValidationStep.UpdatedValue)
			{
				if (flag && this.ParentBinding.ValidatesOnDataErrors)
				{
					ValidationError validationError = base.GetValidationErrors(validationStep);
					if (validationError != null && validationError.RuleInError != DataErrorValidationRule.Instance)
					{
						validationError = null;
					}
					ValidationError validationError2 = this.RunValidationRule(DataErrorValidationRule.Instance, this, base.GetCulture());
					if (validationError2 != null)
					{
						base.UpdateValidationError(validationError2, false);
						flag = false;
					}
					else if (validationError != null)
					{
						base.UpdateValidationError(null, false);
					}
				}
			}
			else if (validationStep == ValidationStep.CommittedValue && flag)
			{
				base.NeedsValidation = false;
			}
			return flag;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x001E0318 File Offset: 0x001DF318
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			if (!base.NeedsValidation)
			{
				return true;
			}
			object value;
			switch (validationStep)
			{
			case ValidationStep.RawProposedValue:
				value = this.GetRawProposedValue();
				break;
			case ValidationStep.ConvertedProposedValue:
				value = bindingGroup.GetValue(this);
				break;
			case ValidationStep.UpdatedValue:
			case ValidationStep.CommittedValue:
				value = this;
				break;
			default:
				throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
				{
					validationStep,
					bindingGroup
				}));
			}
			return this.Validate(value, validationStep);
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x001E038C File Offset: 0x001DF38C
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			values = null;
			object rawProposedValue = this.GetRawProposedValue();
			bool flag = this.Validate(rawProposedValue, ValidationStep.RawProposedValue);
			object obj = flag ? this.ConvertProposedValue(rawProposedValue) : DependencyProperty.UnsetValue;
			if (obj == Binding.DoNothing)
			{
				obj = DependencyProperty.UnsetValue;
			}
			else
			{
				flag = (obj != DependencyProperty.UnsetValue && this.Validate(obj, ValidationStep.ConvertedProposedValue));
			}
			values = new Collection<BindingExpressionBase.ProposedValue>();
			values.Add(new BindingExpressionBase.ProposedValue(this, rawProposedValue, obj));
			return flag;
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x001E03FA File Offset: 0x001DF3FA
		private bool IsValidValueForUpdate(object value, Type sourceType)
		{
			return value == null || sourceType.IsAssignableFrom(value.GetType()) || (Convert.IsDBNull(value) && this.Worker.IsDBNullValidForUpdate);
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x001E0428 File Offset: 0x001DF428
		private void ProcessException(Exception ex, bool validate)
		{
			object obj = null;
			ValidationError validationError = null;
			if (this.ExceptionFilterExists())
			{
				obj = this.CallDoFilterException(ex);
				if (obj == null)
				{
					return;
				}
				validationError = (obj as ValidationError);
			}
			if (validationError == null && validate)
			{
				ValidationRule instance = ExceptionValidationRule.Instance;
				if (obj == null)
				{
					validationError = new ValidationError(instance, this, ex.Message, ex);
				}
				else
				{
					validationError = new ValidationError(instance, this, obj, ex);
				}
			}
			if (validationError != null)
			{
				base.UpdateValidationError(validationError, false);
			}
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x001E048C File Offset: 0x001DF48C
		private bool ShouldIgnoreUpdate()
		{
			if (base.TargetProperty.OwnerType != typeof(Selector) && base.TargetProperty != ComboBox.TextProperty)
			{
				return false;
			}
			DependencyObject contextElement = this.ContextElement;
			object obj;
			if (contextElement == null)
			{
				DependencyObject dependencyObject = base.TargetElement;
				if (dependencyObject != null && base.UsingMentor)
				{
					dependencyObject = Helper.FindMentor(dependencyObject);
				}
				if (dependencyObject == null)
				{
					return true;
				}
				obj = this.ParentBinding.SourceReference.GetDataObject(dependencyObject, new ObjectRefArgs
				{
					ResolveNamesInTemplate = base.ResolveNamesInTemplate
				});
			}
			else
			{
				obj = contextElement.GetValue(FrameworkElement.DataContextProperty);
			}
			if (!this.ParentBinding.BindsDirectlyToSource)
			{
				CollectionViewSource collectionViewSource;
				DataSourceProvider dataSourceProvider;
				if ((collectionViewSource = (obj as CollectionViewSource)) != null)
				{
					obj = collectionViewSource.CollectionView;
				}
				else if ((dataSourceProvider = (obj as DataSourceProvider)) != null)
				{
					obj = dataSourceProvider.Data;
				}
			}
			return !ItemsControl.EqualsEx(this.DataItem, obj) || !this.Worker.IsPathCurrent();
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x001E0570 File Offset: 0x001DF570
		internal void UpdateNotifyDataErrors(INotifyDataErrorInfo indei, string propertyName, object value)
		{
			if (!base.ValidatesOnNotifyDataErrors || base.IsDetached)
			{
				return;
			}
			WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.DataErrorValue, null);
			INotifyDataErrorInfo notifyDataErrorInfo = (weakReference == null) ? null : (weakReference.Target as INotifyDataErrorInfo);
			if (value != DependencyProperty.UnsetValue && value != notifyDataErrorInfo && base.IsDynamic)
			{
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
				INotifyDataErrorInfo notifyDataErrorInfo2 = value as INotifyDataErrorInfo;
				object value2 = BindingExpressionBase.ReplaceReference(weakReference, notifyDataErrorInfo2);
				base.SetValue(BindingExpressionBase.Feature.DataErrorValue, value2, null);
				notifyDataErrorInfo = notifyDataErrorInfo2;
				if (notifyDataErrorInfo2 != null)
				{
					ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo2, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
			}
			base.IsDataErrorsChangedPending = false;
			try
			{
				List<object> dataErrors = BindingExpression.GetDataErrors(indei, propertyName);
				List<object> dataErrors2 = BindingExpression.GetDataErrors(notifyDataErrorInfo, string.Empty);
				List<object> errors = this.MergeErrors(dataErrors, dataErrors2);
				base.UpdateNotifyDataErrorValidationErrors(errors);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x001E0658 File Offset: 0x001DF658
		private void UpdateNotifyDataErrors(object value)
		{
			if (!base.ValidatesOnNotifyDataErrors)
			{
				return;
			}
			this.UpdateNotifyDataErrors(this.SourceItem as INotifyDataErrorInfo, this.SourcePropertyName, value);
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x001E067C File Offset: 0x001DF67C
		internal static List<object> GetDataErrors(INotifyDataErrorInfo indei, string propertyName)
		{
			List<object> list = null;
			if (indei != null && indei.HasErrors)
			{
				for (int i = 3; i >= 0; i--)
				{
					try
					{
						list = new List<object>();
						IEnumerable errors = indei.GetErrors(propertyName);
						if (errors != null)
						{
							foreach (object item in errors)
							{
								list.Add(item);
							}
						}
						break;
					}
					catch (InvalidOperationException)
					{
						if (i == 0)
						{
							throw;
						}
					}
				}
			}
			if (list != null && list.Count == 0)
			{
				list = null;
			}
			return list;
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x001E0720 File Offset: 0x001DF720
		private List<object> MergeErrors(List<object> list1, List<object> list2)
		{
			if (list1 == null)
			{
				return list2;
			}
			if (list2 == null)
			{
				return list1;
			}
			foreach (object item in list2)
			{
				list1.Add(item);
			}
			return list1;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x001E077C File Offset: 0x001DF77C
		private void OnDataContextChanged(DependencyObject contextElement)
		{
			if (!base.IsInUpdate && this.CanActivate)
			{
				if (base.IsReflective && base.RootBindingExpression.ParentBindingBase.BindingGroupName == string.Empty)
				{
					base.RejoinBindingGroup(base.IsReflective, contextElement);
				}
				object value = contextElement.GetValue(FrameworkElement.DataContextProperty);
				if (!ItemsControl.EqualsEx(this.DataItem, value))
				{
					this.Activate(value);
				}
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x001E07EC File Offset: 0x001DF7EC
		internal void OnCurrentChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"CurrentChanged",
					TraceData.Identify(sender)
				}), this);
			}
			this.Worker.OnCurrentChanged(sender as ICollectionView, e);
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x001E0840 File Offset: 0x001DF840
		internal void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"CurrentChanging",
					TraceData.Identify(sender)
				}), this);
			}
			base.Update();
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x001E087E File Offset: 0x001DF87E
		private void OnDataChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"DataChanged",
					TraceData.Identify(sender)
				}), this);
			}
			this.Activate(sender);
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x001E08BC File Offset: 0x001DF8BC
		private void OnInheritanceContextChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"InheritanceContextChanged",
					TraceData.Identify(sender)
				}), this);
			}
			if (base.StatusInternal == BindingStatusInternal.Unattached)
			{
				base.Engine.CancelTask(this, TaskOps.AttachToContext);
				this.AttachToContext(BindingExpression.AttachAttempt.Again);
				if (base.StatusInternal == BindingStatusInternal.Unattached)
				{
					base.Engine.AddTask(this, TaskOps.AttachToContext);
					return;
				}
			}
			else
			{
				this.AttachToContext(BindingExpression.AttachAttempt.Last);
			}
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x001E0937 File Offset: 0x001DF937
		internal override void OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.TraceAndNotifyWithNoParameters(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"LostFocus",
					TraceData.Identify(sender)
				}), this);
			}
			base.Update();
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x001E0978 File Offset: 0x001DF978
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.UpdateNotifyDataErrors(DependencyProperty.UnsetValue);
				return;
			}
			if (!base.IsDataErrorsChangedPending)
			{
				base.IsDataErrorsChangedPending = true;
				base.Engine.Marshal(delegate(object arg)
				{
					this.UpdateNotifyDataErrors(DependencyProperty.UnsetValue);
					return null;
				}, null, 1);
			}
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x00105F35 File Offset: 0x00104F35
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x001E09CC File Offset: 0x001DF9CC
		private object CallDoFilterException(Exception ex)
		{
			if (this.ParentBinding.UpdateSourceExceptionFilter != null)
			{
				return this.ParentBinding.DoFilterException(this, ex);
			}
			if (base.IsInMultiBindingExpression)
			{
				return base.ParentMultiBindingExpression.ParentMultiBinding.DoFilterException(this, ex);
			}
			return null;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x001E0A05 File Offset: 0x001DFA05
		private bool ExceptionFilterExists()
		{
			return this.ParentBinding.UpdateSourceExceptionFilter != null || (base.IsInMultiBindingExpression && base.ParentMultiBindingExpression.ParentMultiBinding.UpdateSourceExceptionFilter != null);
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x001E0A33 File Offset: 0x001DFA33
		internal IDisposable ChangingValue()
		{
			return new BindingExpression.ChangingValueHelper(this);
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x001E0A3B File Offset: 0x001DFA3B
		internal void CancelPendingTasks()
		{
			base.Engine.CancelTasks(this);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x001E0A4C File Offset: 0x001DFA4C
		private void Replace()
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null)
			{
				if (base.IsInBindingExpressionCollection)
				{
					base.ParentBindingExpressionBase.ReplaceChild(this);
					return;
				}
				BindingOperations.SetBinding(targetElement, base.TargetProperty, this.ParentBinding);
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x001E0A8C File Offset: 0x001DFA8C
		internal static void OnTargetUpdated(DependencyObject d, DependencyProperty dp)
		{
			DataTransferEventArgs dataTransferEventArgs = new DataTransferEventArgs(d, dp);
			dataTransferEventArgs.RoutedEvent = Binding.TargetUpdatedEvent;
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!frameworkObject.IsValid && d != null)
			{
				frameworkObject.Reset(Helper.FindMentor(d));
			}
			frameworkObject.RaiseEvent(dataTransferEventArgs);
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x001E0AD8 File Offset: 0x001DFAD8
		internal static void OnSourceUpdated(DependencyObject d, DependencyProperty dp)
		{
			DataTransferEventArgs dataTransferEventArgs = new DataTransferEventArgs(d, dp);
			dataTransferEventArgs.RoutedEvent = Binding.SourceUpdatedEvent;
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!frameworkObject.IsValid && d != null)
			{
				frameworkObject.Reset(Helper.FindMentor(d));
			}
			frameworkObject.RaiseEvent(dataTransferEventArgs);
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x001E0B24 File Offset: 0x001DFB24
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
			if (property == FrameworkElement.DataContextProperty)
			{
				DependencyObject contextElement = this.ContextElement;
				if (d == contextElement)
				{
					base.IsTransferPending = false;
					this.OnDataContextChanged(contextElement);
				}
			}
			if (property == CollectionViewSource.ViewProperty)
			{
				CollectionViewSource collectionViewSource = this.CollectionViewSource;
				if (d == collectionViewSource)
				{
					this.Activate(collectionViewSource);
				}
			}
			if (property == FrameworkElement.LanguageProperty && base.UsesLanguage && d == base.TargetElement)
			{
				base.InvalidateCulture();
				this.TransferValue();
			}
			if (this.Worker != null)
			{
				this.Worker.OnSourceInvalidation(d, property, args.IsASubPropertyChange);
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x001E0BEB File Offset: 0x001DFBEB
		private void SetDataItem(object newItem)
		{
			this._dataItem = BindingExpressionBase.CreateReference(newItem);
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x001E0BFC File Offset: 0x001DFBFC
		private object GetDataSourceForDataContext(DependencyObject d)
		{
			BindingExpression bindingExpression = null;
			for (DependencyObject dependencyObject = d; dependencyObject != null; dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject))
			{
				if (BindingExpression.HasLocalDataContext(dependencyObject))
				{
					bindingExpression = BindingOperations.GetBindingExpression(dependencyObject, FrameworkElement.DataContextProperty);
					break;
				}
			}
			if (bindingExpression != null)
			{
				return bindingExpression.DataSource;
			}
			return null;
		}

		// Token: 0x04001CB1 RID: 7345
		private WeakReference _ctxElement;

		// Token: 0x04001CB2 RID: 7346
		private object _dataItem;

		// Token: 0x04001CB3 RID: 7347
		private BindingWorker _worker;

		// Token: 0x04001CB4 RID: 7348
		private Type _sourceType;

		// Token: 0x04001CB5 RID: 7349
		internal static readonly object NullDataItem = new NamedObject("NullDataItem");

		// Token: 0x04001CB6 RID: 7350
		internal static readonly object IgnoreDefaultValue = new NamedObject("IgnoreDefaultValue");

		// Token: 0x04001CB7 RID: 7351
		internal static readonly object StaticSource = new NamedObject("StaticSource");

		// Token: 0x02000ACA RID: 2762
		internal enum SourceType
		{
			// Token: 0x04004690 RID: 18064
			Unknown,
			// Token: 0x04004691 RID: 18065
			CLR,
			// Token: 0x04004692 RID: 18066
			XML
		}

		// Token: 0x02000ACB RID: 2763
		private enum AttachAttempt
		{
			// Token: 0x04004694 RID: 18068
			First,
			// Token: 0x04004695 RID: 18069
			Again,
			// Token: 0x04004696 RID: 18070
			Last
		}

		// Token: 0x02000ACC RID: 2764
		private class ChangingValueHelper : IDisposable
		{
			// Token: 0x06008AF2 RID: 35570 RVA: 0x00338CB7 File Offset: 0x00337CB7
			internal ChangingValueHelper(BindingExpression b)
			{
				this._bindingExpression = b;
				b.CancelPendingTasks();
			}

			// Token: 0x06008AF3 RID: 35571 RVA: 0x00338CCC File Offset: 0x00337CCC
			public void Dispose()
			{
				this._bindingExpression.TransferValue();
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004697 RID: 18071
			private BindingExpression _bindingExpression;
		}
	}
}
