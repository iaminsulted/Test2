using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x020003A0 RID: 928
	[Localizability(LocalizationCategory.Ignore)]
	[DictionaryKeyProperty("TargetType")]
	[ContentProperty("Setters")]
	public class Style : DispatcherObject, INameScope, IAddChild, ISealable, IHaveResources, IQueryAmbient
	{
		// Token: 0x06002579 RID: 9593 RVA: 0x00186949 File Offset: 0x00185949
		static Style()
		{
			StyleHelper.RegisterAlternateExpressionStorage();
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x0018696F File Offset: 0x0018596F
		public Style()
		{
			this.GetUniqueGlobalIndex();
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x0018699F File Offset: 0x0018599F
		public Style(Type targetType)
		{
			this.TargetType = targetType;
			this.GetUniqueGlobalIndex();
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x001869D6 File Offset: 0x001859D6
		public Style(Type targetType, Style basedOn)
		{
			this.TargetType = targetType;
			this.BasedOn = basedOn;
			this.GetUniqueGlobalIndex();
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00186A14 File Offset: 0x00185A14
		public void RegisterName(string name, object scopedElement)
		{
			base.VerifyAccess();
			this._nameScope.RegisterName(name, scopedElement);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00186A29 File Offset: 0x00185A29
		public void UnregisterName(string name)
		{
			base.VerifyAccess();
			this._nameScope.UnregisterName(name);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00186A3D File Offset: 0x00185A3D
		object INameScope.FindName(string name)
		{
			base.VerifyAccess();
			return this._nameScope.FindName(name);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00186A54 File Offset: 0x00185A54
		private void GetUniqueGlobalIndex()
		{
			object synchronized = Style.Synchronized;
			lock (synchronized)
			{
				Style.StyleInstanceCount++;
				this.GlobalIndex = Style.StyleInstanceCount;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x00186AA4 File Offset: 0x00185AA4
		public bool IsSealed
		{
			get
			{
				base.VerifyAccess();
				return this._sealed;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002582 RID: 9602 RVA: 0x00186AB2 File Offset: 0x00185AB2
		// (set) Token: 0x06002583 RID: 9603 RVA: 0x00186AC0 File Offset: 0x00185AC0
		[Ambient]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public Type TargetType
		{
			get
			{
				base.VerifyAccess();
				return this._targetType;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!typeof(FrameworkElement).IsAssignableFrom(value) && !typeof(FrameworkContentElement).IsAssignableFrom(value) && !(Style.DefaultTargetType == value))
				{
					throw new ArgumentException(SR.Get("MustBeFrameworkDerived", new object[]
					{
						value.Name
					}));
				}
				this._targetType = value;
				this.SetModified(1);
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x00186B6B File Offset: 0x00185B6B
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x00186B7C File Offset: 0x00185B7C
		[DefaultValue(null)]
		[Ambient]
		public Style BasedOn
		{
			get
			{
				base.VerifyAccess();
				return this._basedOn;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				if (value == this)
				{
					throw new ArgumentException(SR.Get("StyleCannotBeBasedOnSelf"));
				}
				this._basedOn = value;
				this.SetModified(2);
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x00186BD7 File Offset: 0x00185BD7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerCollection Triggers
		{
			get
			{
				base.VerifyAccess();
				if (this._visualTriggers == null)
				{
					this._visualTriggers = new TriggerCollection();
					if (this._sealed)
					{
						this._visualTriggers.Seal();
					}
				}
				return this._visualTriggers;
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x00186C0B File Offset: 0x00185C0B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SetterBaseCollection Setters
		{
			get
			{
				base.VerifyAccess();
				if (this._setters == null)
				{
					this._setters = new SetterBaseCollection();
					if (this._sealed)
					{
						this._setters.Seal();
					}
				}
				return this._setters;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x00186C40 File Offset: 0x00185C40
		// (set) Token: 0x06002589 RID: 9609 RVA: 0x00186C8C File Offset: 0x00185C8C
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				base.VerifyAccess();
				if (this._resources == null)
				{
					this._resources = new ResourceDictionary();
					this._resources.CanBeAccessedAcrossThreads = true;
					if (this._sealed)
					{
						this._resources.IsReadOnly = true;
					}
				}
				return this._resources;
			}
			set
			{
				base.VerifyAccess();
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Style"
					}));
				}
				this._resources = value;
				if (this._resources != null)
				{
					this._resources.CanBeAccessedAcrossThreads = true;
				}
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x00186CE0 File Offset: 0x00185CE0
		// (set) Token: 0x0600258B RID: 9611 RVA: 0x00186CE8 File Offset: 0x00185CE8
		ResourceDictionary IHaveResources.Resources
		{
			get
			{
				return this.Resources;
			}
			set
			{
				this.Resources = value;
			}
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x00186CF4 File Offset: 0x00185CF4
		internal object FindResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				bool flag;
				return this._resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			if (this._basedOn != null)
			{
				return this._basedOn.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00186D44 File Offset: 0x00185D44
		internal ResourceDictionary FindResourceDictionary(object resourceKey)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				return this._resources;
			}
			if (this._basedOn != null)
			{
				return this._basedOn.FindResourceDictionary(resourceKey);
			}
			return null;
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x00186D79 File Offset: 0x00185D79
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			if (!(propertyName == "Resources"))
			{
				if (propertyName == "BasedOn")
				{
					if (this._basedOn == null)
					{
						return false;
					}
				}
			}
			else if (this._resources == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00186DAC File Offset: 0x00185DAC
		void IAddChild.AddChild(object value)
		{
			base.VerifyAccess();
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			SetterBase setterBase = value as SetterBase;
			if (setterBase == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					value.GetType(),
					typeof(SetterBase)
				}), "value");
			}
			this.Setters.Add(setterBase);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x00174CBA File Offset: 0x00173CBA
		void IAddChild.AddText(string text)
		{
			base.VerifyAccess();
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x00186E14 File Offset: 0x00185E14
		private void UpdatePropertyValueList(DependencyProperty dp, PropertyValueType valueType, object value)
		{
			int num = -1;
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].Property == dp)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				PropertyValue value2 = this.PropertyValues[num];
				value2.ValueType = valueType;
				value2.ValueInternal = value;
				this.PropertyValues[num] = value2;
				return;
			}
			this.PropertyValues.Add(new PropertyValue
			{
				ValueType = valueType,
				ChildName = "~Self",
				Property = dp,
				ValueInternal = value
			});
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x00186EBC File Offset: 0x00185EBC
		internal void CheckTargetType(object element)
		{
			if (Style.DefaultTargetType == this.TargetType)
			{
				return;
			}
			Type type = element.GetType();
			if (!this.TargetType.IsAssignableFrom(type))
			{
				throw new InvalidOperationException(SR.Get("StyleTargetTypeMismatchWithElement", new object[]
				{
					this.TargetType.Name,
					type.Name
				}));
			}
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x00186F20 File Offset: 0x00185F20
		public void Seal()
		{
			base.VerifyAccess();
			if (this._sealed)
			{
				return;
			}
			if (this._targetType == null)
			{
				throw new InvalidOperationException(SR.Get("NullPropertyIllegal", new object[]
				{
					"TargetType"
				}));
			}
			if (this._basedOn != null && Style.DefaultTargetType != this._basedOn.TargetType && !this._basedOn.TargetType.IsAssignableFrom(this._targetType))
			{
				throw new InvalidOperationException(SR.Get("MustBaseOnStyleOfABaseType", new object[]
				{
					this._targetType.Name
				}));
			}
			if (this._setters != null)
			{
				this._setters.Seal();
			}
			if (this._visualTriggers != null)
			{
				this._visualTriggers.Seal();
			}
			this.CheckForCircularBasedOnReferences();
			if (this._basedOn != null)
			{
				this._basedOn.Seal();
			}
			if (this._resources != null)
			{
				this._resources.IsReadOnly = true;
			}
			this.ProcessSetters(this);
			StyleHelper.AddEventDependent(0, this.EventHandlersStore, ref this.EventDependents);
			this.ProcessSelfStyles(this);
			this.ProcessVisualTriggers(this);
			StyleHelper.SortResourceDependents(ref this.ResourceDependents);
			this._sealed = true;
			base.DetachFromDispatcher();
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x00187058 File Offset: 0x00186058
		private void CheckForCircularBasedOnReferences()
		{
			Stack stack = new Stack(10);
			for (Style style = this; style != null; style = style.BasedOn)
			{
				if (stack.Contains(style))
				{
					throw new InvalidOperationException(SR.Get("StyleBasedOnHasLoop"));
				}
				stack.Push(style);
			}
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x0018709C File Offset: 0x0018609C
		private void ProcessSetters(Style style)
		{
			if (style == null)
			{
				return;
			}
			style.Setters.Seal();
			if (this.PropertyValues.Count == 0)
			{
				this.PropertyValues = new FrugalStructList<PropertyValue>(style.Setters.Count);
			}
			for (int i = 0; i < style.Setters.Count; i++)
			{
				SetterBase setterBase = style.Setters[i];
				Setter setter = setterBase as Setter;
				if (setter != null)
				{
					if (setter.TargetName != null)
					{
						throw new InvalidOperationException(SR.Get("SetterOnStyleNotAllowedToHaveTarget", new object[]
						{
							setter.TargetName
						}));
					}
					if (style == this)
					{
						DynamicResourceExtension dynamicResourceExtension = setter.ValueInternal as DynamicResourceExtension;
						if (dynamicResourceExtension == null)
						{
							this.UpdatePropertyValueList(setter.Property, PropertyValueType.Set, setter.ValueInternal);
						}
						else
						{
							this.UpdatePropertyValueList(setter.Property, PropertyValueType.Resource, dynamicResourceExtension.ResourceKey);
						}
					}
				}
				else
				{
					EventSetter eventSetter = (EventSetter)setterBase;
					if (this._eventHandlersStore == null)
					{
						this._eventHandlersStore = new EventHandlersStore();
					}
					this._eventHandlersStore.AddRoutedEventHandler(eventSetter.Event, eventSetter.Handler, eventSetter.HandledEventsToo);
					this.SetModified(16);
					if (eventSetter.Event == FrameworkElement.LoadedEvent || eventSetter.Event == FrameworkElement.UnloadedEvent)
					{
						this._hasLoadedChangeHandler = true;
					}
				}
			}
			this.ProcessSetters(style._basedOn);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x001871E8 File Offset: 0x001861E8
		private void ProcessSelfStyles(Style style)
		{
			if (style == null)
			{
				return;
			}
			this.ProcessSelfStyles(style._basedOn);
			for (int i = 0; i < style.PropertyValues.Count; i++)
			{
				PropertyValue propertyValue = style.PropertyValues[i];
				StyleHelper.UpdateTables(ref propertyValue, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ResourceDependents, ref this._dataTriggerRecordFromBinding, null, ref this._hasInstanceValues);
				StyleHelper.AddContainerDependent(propertyValue.Property, false, ref this.ContainerDependents);
			}
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00187260 File Offset: 0x00186260
		private void ProcessVisualTriggers(Style style)
		{
			if (style == null)
			{
				return;
			}
			this.ProcessVisualTriggers(style._basedOn);
			if (style._visualTriggers != null)
			{
				int count = style._visualTriggers.Count;
				for (int i = 0; i < count; i++)
				{
					TriggerBase triggerBase = style._visualTriggers[i];
					for (int j = 0; j < triggerBase.PropertyValues.Count; j++)
					{
						PropertyValue propertyValue = triggerBase.PropertyValues[j];
						if (propertyValue.ChildName != "~Self")
						{
							throw new InvalidOperationException(SR.Get("StyleTriggersCannotTargetTheTemplate"));
						}
						TriggerCondition[] conditions = propertyValue.Conditions;
						for (int k = 0; k < conditions.Length; k++)
						{
							if (conditions[k].SourceName != "~Self")
							{
								throw new InvalidOperationException(SR.Get("TriggerOnStyleNotAllowedToHaveSource", new object[]
								{
									conditions[k].SourceName
								}));
							}
						}
						StyleHelper.AddContainerDependent(propertyValue.Property, true, ref this.ContainerDependents);
						StyleHelper.UpdateTables(ref propertyValue, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ResourceDependents, ref this._dataTriggerRecordFromBinding, null, ref this._hasInstanceValues);
					}
					if (triggerBase.HasEnterActions || triggerBase.HasExitActions)
					{
						if (triggerBase is Trigger)
						{
							StyleHelper.AddPropertyTriggerWithAction(triggerBase, ((Trigger)triggerBase).Property, ref this.PropertyTriggersWithActions);
						}
						else if (triggerBase is MultiTrigger)
						{
							MultiTrigger multiTrigger = (MultiTrigger)triggerBase;
							for (int l = 0; l < multiTrigger.Conditions.Count; l++)
							{
								Condition condition = multiTrigger.Conditions[l];
								StyleHelper.AddPropertyTriggerWithAction(triggerBase, condition.Property, ref this.PropertyTriggersWithActions);
							}
						}
						else if (triggerBase is DataTrigger)
						{
							StyleHelper.AddDataTriggerWithAction(triggerBase, ((DataTrigger)triggerBase).Binding, ref this.DataTriggersWithActions);
						}
						else
						{
							if (!(triggerBase is MultiDataTrigger))
							{
								throw new InvalidOperationException(SR.Get("UnsupportedTriggerInStyle", new object[]
								{
									triggerBase.GetType().Name
								}));
							}
							MultiDataTrigger multiDataTrigger = (MultiDataTrigger)triggerBase;
							for (int m = 0; m < multiDataTrigger.Conditions.Count; m++)
							{
								Condition condition2 = multiDataTrigger.Conditions[m];
								StyleHelper.AddDataTriggerWithAction(triggerBase, condition2.Binding, ref this.DataTriggersWithActions);
							}
						}
					}
					EventTrigger eventTrigger = triggerBase as EventTrigger;
					if (eventTrigger != null)
					{
						if (eventTrigger.SourceName != null && eventTrigger.SourceName.Length > 0)
						{
							throw new InvalidOperationException(SR.Get("EventTriggerOnStyleNotAllowedToHaveTarget", new object[]
							{
								eventTrigger.SourceName
							}));
						}
						StyleHelper.ProcessEventTrigger(eventTrigger, null, ref this._triggerActions, ref this.EventDependents, null, null, ref this._eventHandlersStore, ref this._hasLoadedChangeHandler);
					}
				}
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x0018751A File Offset: 0x0018651A
		public override int GetHashCode()
		{
			base.VerifyAccess();
			return this.GlobalIndex;
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ISealable.CanSeal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600259A RID: 9626 RVA: 0x00187528 File Offset: 0x00186528
		bool ISealable.IsSealed
		{
			get
			{
				return this.IsSealed;
			}
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00187530 File Offset: 0x00186530
		void ISealable.Seal()
		{
			this.Seal();
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x00187538 File Offset: 0x00186538
		internal bool HasResourceReferences
		{
			get
			{
				return this.ResourceDependents.Count > 0;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x00187548 File Offset: 0x00186548
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x00187550 File Offset: 0x00186550
		internal bool HasEventDependents
		{
			get
			{
				return this.EventDependents.Count > 0;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x00187560 File Offset: 0x00186560
		internal bool HasEventSetters
		{
			get
			{
				return this.IsModified(16);
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x0018756A File Offset: 0x0018656A
		internal bool HasInstanceValues
		{
			get
			{
				return this._hasInstanceValues;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x00187572 File Offset: 0x00186572
		// (set) Token: 0x060025A2 RID: 9634 RVA: 0x0018757A File Offset: 0x0018657A
		internal bool HasLoadedChangeHandler
		{
			get
			{
				return this._hasLoadedChangeHandler;
			}
			set
			{
				this._hasLoadedChangeHandler = value;
			}
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x001641A9 File Offset: 0x001631A9
		private static bool IsEqual(object a, object b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x00187583 File Offset: 0x00186583
		internal bool IsBasedOnModified
		{
			get
			{
				return this.IsModified(2);
			}
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x0018758C File Offset: 0x0018658C
		private void SetModified(int id)
		{
			this._modified |= id;
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x0018759C File Offset: 0x0018659C
		internal bool IsModified(int id)
		{
			return (id & this._modified) != 0;
		}

		// Token: 0x04001191 RID: 4497
		private NameScope _nameScope = new NameScope();

		// Token: 0x04001192 RID: 4498
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x04001193 RID: 4499
		private bool _sealed;

		// Token: 0x04001194 RID: 4500
		private bool _hasInstanceValues;

		// Token: 0x04001195 RID: 4501
		internal static readonly Type DefaultTargetType = typeof(IFrameworkInputElement);

		// Token: 0x04001196 RID: 4502
		private bool _hasLoadedChangeHandler;

		// Token: 0x04001197 RID: 4503
		private Type _targetType = Style.DefaultTargetType;

		// Token: 0x04001198 RID: 4504
		private Style _basedOn;

		// Token: 0x04001199 RID: 4505
		private TriggerCollection _visualTriggers;

		// Token: 0x0400119A RID: 4506
		private SetterBaseCollection _setters;

		// Token: 0x0400119B RID: 4507
		internal ResourceDictionary _resources;

		// Token: 0x0400119C RID: 4508
		internal int GlobalIndex;

		// Token: 0x0400119D RID: 4509
		internal FrugalStructList<ChildRecord> ChildRecordFromChildIndex;

		// Token: 0x0400119E RID: 4510
		internal FrugalStructList<ItemStructMap<TriggerSourceRecord>> TriggerSourceRecordFromChildIndex;

		// Token: 0x0400119F RID: 4511
		internal FrugalMap PropertyTriggersWithActions;

		// Token: 0x040011A0 RID: 4512
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x040011A1 RID: 4513
		internal FrugalStructList<ContainerDependent> ContainerDependents;

		// Token: 0x040011A2 RID: 4514
		internal FrugalStructList<ChildPropertyDependent> ResourceDependents;

		// Token: 0x040011A3 RID: 4515
		internal ItemStructList<ChildEventDependent> EventDependents = new ItemStructList<ChildEventDependent>(1);

		// Token: 0x040011A4 RID: 4516
		internal HybridDictionary _triggerActions;

		// Token: 0x040011A5 RID: 4517
		internal HybridDictionary _dataTriggerRecordFromBinding;

		// Token: 0x040011A6 RID: 4518
		internal HybridDictionary DataTriggersWithActions;

		// Token: 0x040011A7 RID: 4519
		private static int StyleInstanceCount = 0;

		// Token: 0x040011A8 RID: 4520
		internal static object Synchronized = new object();

		// Token: 0x040011A9 RID: 4521
		private const int TargetTypeID = 1;

		// Token: 0x040011AA RID: 4522
		internal const int BasedOnID = 2;

		// Token: 0x040011AB RID: 4523
		private const int HasEventSetter = 16;

		// Token: 0x040011AC RID: 4524
		private int _modified;
	}
}
