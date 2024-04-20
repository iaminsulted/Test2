using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020003DA RID: 986
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public abstract class TriggerBase : DependencyObject
	{
		// Token: 0x06002967 RID: 10599 RVA: 0x00199514 File Offset: 0x00198514
		internal TriggerBase()
		{
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x001998BF File Offset: 0x001988BF
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection EnterActions
		{
			get
			{
				base.VerifyAccess();
				if (this._enterActions == null)
				{
					this._enterActions = new TriggerActionCollection();
					if (base.IsSealed)
					{
						this._enterActions.Seal(this);
					}
				}
				return this._enterActions;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06002969 RID: 10601 RVA: 0x001998F4 File Offset: 0x001988F4
		internal bool HasEnterActions
		{
			get
			{
				return this._enterActions != null && this._enterActions.Count > 0;
			}
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x0600296A RID: 10602 RVA: 0x0019990E File Offset: 0x0019890E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerActionCollection ExitActions
		{
			get
			{
				base.VerifyAccess();
				if (this._exitActions == null)
				{
					this._exitActions = new TriggerActionCollection();
					if (base.IsSealed)
					{
						this._exitActions.Seal(this);
					}
				}
				return this._exitActions;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x0600296B RID: 10603 RVA: 0x00199943 File Offset: 0x00198943
		internal bool HasExitActions
		{
			get
			{
				return this._exitActions != null && this._exitActions.Count > 0;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600296C RID: 10604 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		internal bool ExecuteEnterActionsOnApply
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600296D RID: 10605 RVA: 0x00105F35 File Offset: 0x00104F35
		internal bool ExecuteExitActionsOnApply
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x0019995D File Offset: 0x0019895D
		internal void ProcessParametersContainer(DependencyProperty dp)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				throw new ArgumentException(SR.Get("StylePropertyInStyleNotAllowed"));
			}
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x00199977 File Offset: 0x00198977
		internal string ProcessParametersVisualTreeChild(DependencyProperty dp, string target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (target.Length == 0)
			{
				throw new ArgumentException(SR.Get("ChildNameMustBeNonEmpty"));
			}
			return string.Intern(target);
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x001999A8 File Offset: 0x001989A8
		internal void AddToPropertyValues(string childName, DependencyProperty dp, object value, PropertyValueType valueType)
		{
			this.PropertyValues.Add(new PropertyValue
			{
				ValueType = valueType,
				Conditions = null,
				ChildName = childName,
				Property = dp,
				ValueInternal = value
			});
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x001999F4 File Offset: 0x001989F4
		internal override void Seal()
		{
			base.VerifyAccess();
			base.Seal();
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = this.PropertyValues[i];
				DependencyProperty property = propertyValue.Property;
				for (int j = 0; j < propertyValue.Conditions.Length; j++)
				{
					DependencyProperty property2 = propertyValue.Conditions[j].Property;
					if (property2 == property && propertyValue.ChildName == "~Self")
					{
						throw new InvalidOperationException(SR.Get("PropertyTriggerCycleDetected", new object[]
						{
							property2.Name
						}));
					}
				}
			}
			if (this._enterActions != null)
			{
				this._enterActions.Seal(this);
			}
			if (this._exitActions != null)
			{
				this._exitActions.Seal(this);
			}
			base.DetachFromDispatcher();
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x00199AC8 File Offset: 0x00198AC8
		internal void ProcessSettersCollection(SetterBaseCollection setters)
		{
			if (setters != null)
			{
				setters.Seal();
				for (int i = 0; i < setters.Count; i++)
				{
					Setter setter = setters[i] as Setter;
					if (setter == null)
					{
						throw new InvalidOperationException(SR.Get("VisualTriggerSettersIncludeUnsupportedSetterType", new object[]
						{
							setters[i].GetType().Name
						}));
					}
					DependencyProperty property = setter.Property;
					object valueInternal = setter.ValueInternal;
					string text = setter.TargetName;
					if (text == null)
					{
						this.ProcessParametersContainer(property);
						text = "~Self";
					}
					else
					{
						text = this.ProcessParametersVisualTreeChild(property, text);
					}
					DynamicResourceExtension dynamicResourceExtension = valueInternal as DynamicResourceExtension;
					if (dynamicResourceExtension == null)
					{
						this.AddToPropertyValues(text, property, valueInternal, PropertyValueType.Trigger);
					}
					else
					{
						this.AddToPropertyValues(text, property, dynamicResourceExtension.ResourceKey, PropertyValueType.PropertyTriggerResource);
					}
				}
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x06002973 RID: 10611 RVA: 0x00199B93 File Offset: 0x00198B93
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x00199B9B File Offset: 0x00198B9B
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x00199BB0 File Offset: 0x00198BB0
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x06002976 RID: 10614 RVA: 0x00199BC5 File Offset: 0x00198BC5
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06002977 RID: 10615 RVA: 0x00199BCD File Offset: 0x00198BCD
		internal long Layer
		{
			get
			{
				return this._globalLayerRank;
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x00199BD8 File Offset: 0x00198BD8
		internal void EstablishLayer()
		{
			if (this._globalLayerRank == 0L)
			{
				object synchronized = TriggerBase.Synchronized;
				lock (synchronized)
				{
					long nextGlobalLayerRank = TriggerBase._nextGlobalLayerRank;
					TriggerBase._nextGlobalLayerRank = nextGlobalLayerRank + 1L;
					this._globalLayerRank = nextGlobalLayerRank;
				}
				if (TriggerBase._nextGlobalLayerRank == 9223372036854775807L)
				{
					throw new InvalidOperationException(SR.Get("PropertyTriggerLayerLimitExceeded"));
				}
			}
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool GetCurrentState(DependencyObject container, UncommonField<HybridDictionary[]> dataField)
		{
			return false;
		}

		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x00199C50 File Offset: 0x00198C50
		// (set) Token: 0x0600297B RID: 10619 RVA: 0x00199C58 File Offset: 0x00198C58
		internal TriggerCondition[] TriggerConditions
		{
			get
			{
				return this._triggerConditions;
			}
			set
			{
				this._triggerConditions = value;
			}
		}

		// Token: 0x040014EC RID: 5356
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x040014ED RID: 5357
		private static object Synchronized = new object();

		// Token: 0x040014EE RID: 5358
		private TriggerCondition[] _triggerConditions;

		// Token: 0x040014EF RID: 5359
		private DependencyObject _inheritanceContext;

		// Token: 0x040014F0 RID: 5360
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x040014F1 RID: 5361
		private TriggerActionCollection _enterActions;

		// Token: 0x040014F2 RID: 5362
		private TriggerActionCollection _exitActions;

		// Token: 0x040014F3 RID: 5363
		private long _globalLayerRank;

		// Token: 0x040014F4 RID: 5364
		private static long _nextGlobalLayerRank = Storyboard.Layers.PropertyTriggerStartLayer;
	}
}
