using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Media3D;
using MS.Internal;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x0200036E RID: 878
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class FrameworkElementFactory
	{
		// Token: 0x0600232F RID: 9007 RVA: 0x0017E87E File Offset: 0x0017D87E
		public FrameworkElementFactory() : this(null, null)
		{
		}

		// Token: 0x06002330 RID: 9008 RVA: 0x0017E888 File Offset: 0x0017D888
		public FrameworkElementFactory(Type type) : this(type, null)
		{
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x0017E892 File Offset: 0x0017D892
		public FrameworkElementFactory(string text) : this(null, null)
		{
			this.Text = text;
		}

		// Token: 0x06002332 RID: 9010 RVA: 0x0017E8A3 File Offset: 0x0017D8A3
		public FrameworkElementFactory(Type type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x0017E8CB File Offset: 0x0017D8CB
		// (set) Token: 0x06002334 RID: 9012 RVA: 0x0017E8D4 File Offset: 0x0017D8D4
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (this._text != null)
				{
					throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
				}
				if (value != null && !typeof(FrameworkElement).IsAssignableFrom(value) && !typeof(FrameworkContentElement).IsAssignableFrom(value) && !typeof(Visual3D).IsAssignableFrom(value))
				{
					throw new ArgumentException(SR.Get("MustBeFrameworkOr3DDerived", new object[]
					{
						value.Name
					}));
				}
				this._type = value;
				WpfKnownType wpfKnownType = null;
				if (this._type != null)
				{
					wpfKnownType = (XamlReader.BamlSharedSchemaContext.GetKnownXamlType(this._type) as WpfKnownType);
				}
				this._knownTypeFactory = ((wpfKnownType != null) ? wpfKnownType.DefaultConstructor : null);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06002335 RID: 9013 RVA: 0x0017E9C2 File Offset: 0x0017D9C2
		// (set) Token: 0x06002336 RID: 9014 RVA: 0x0017E9CC File Offset: 0x0017D9CC
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (this._firstChild != null)
				{
					throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._text = value;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002337 RID: 9015 RVA: 0x0017EA2C File Offset: 0x0017DA2C
		// (set) Token: 0x06002338 RID: 9016 RVA: 0x0017EA34 File Offset: 0x0017DA34
		public string Name
		{
			get
			{
				return this._childName;
			}
			set
			{
				if (this._sealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"FrameworkElementFactory"
					}));
				}
				if (value == string.Empty)
				{
					throw new ArgumentException(SR.Get("NameNotEmptyString"));
				}
				this._childName = value;
			}
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0017EA8C File Offset: 0x0017DA8C
		public void AppendChild(FrameworkElementFactory child)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (child == null)
			{
				throw new ArgumentNullException("child");
			}
			if (child._parent != null)
			{
				throw new ArgumentException(SR.Get("FrameworkElementFactoryAlreadyParented"));
			}
			if (this._text != null)
			{
				throw new InvalidOperationException(SR.Get("FrameworkElementFactoryCannotAddText"));
			}
			if (this._firstChild == null)
			{
				this._firstChild = child;
				this._lastChild = child;
			}
			else
			{
				this._lastChild._nextSibling = child;
				this._lastChild = child;
			}
			child._parent = this;
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0017EB30 File Offset: 0x0017DB30
		public void SetValue(DependencyProperty dp, object value)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			if (!dp.IsValidValue(value) && !(value is MarkupExtension) && !(value is DeferredReference))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					value,
					dp.Name
				}));
			}
			if (StyleHelper.IsStylingLogicalTree(dp, value))
			{
				throw new NotSupportedException(SR.Get("ModifyingLogicalTreeViaStylesNotImplemented", new object[]
				{
					value,
					"FrameworkElementFactory.SetValue"
				}));
			}
			if (dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("ReadOnlyPropertyNotAllowed", new object[]
				{
					dp.Name,
					base.GetType().Name
				}));
			}
			ResourceReferenceExpression resourceReferenceExpression = value as ResourceReferenceExpression;
			DynamicResourceExtension dynamicResourceExtension = value as DynamicResourceExtension;
			object obj = null;
			if (resourceReferenceExpression != null)
			{
				obj = resourceReferenceExpression.ResourceKey;
			}
			else if (dynamicResourceExtension != null)
			{
				obj = dynamicResourceExtension.ResourceKey;
			}
			if (obj != null)
			{
				this.UpdatePropertyValueList(dp, PropertyValueType.Resource, obj);
				return;
			}
			TemplateBindingExtension templateBindingExtension = value as TemplateBindingExtension;
			if (templateBindingExtension == null)
			{
				this.UpdatePropertyValueList(dp, PropertyValueType.Set, value);
				return;
			}
			this.UpdatePropertyValueList(dp, PropertyValueType.TemplateBinding, templateBindingExtension);
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0017EC5D File Offset: 0x0017DC5D
		public void SetBinding(DependencyProperty dp, BindingBase binding)
		{
			this.SetValue(dp, binding);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0017EC67 File Offset: 0x0017DC67
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			this.UpdatePropertyValueList(dp, PropertyValueType.Resource, name);
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x0017ECA6 File Offset: 0x0017DCA6
		public void AddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			this.AddHandler(routedEvent, handler, false);
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x0017ECB4 File Offset: 0x0017DCB4
		public void AddHandler(RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetType() != routedEvent.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			if (this._eventHandlersStore == null)
			{
				this._eventHandlersStore = new EventHandlersStore();
			}
			this._eventHandlersStore.AddRoutedEventHandler(routedEvent, handler, handledEventsToo);
			if (routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent)
			{
				this.HasLoadedChangeHandler = true;
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x0017ED60 File Offset: 0x0017DD60
		public void RemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"FrameworkElementFactory"
				}));
			}
			if (routedEvent == null)
			{
				throw new ArgumentNullException("routedEvent");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetType() != routedEvent.HandlerType)
			{
				throw new ArgumentException(SR.Get("HandlerTypeIllegal"));
			}
			if (this._eventHandlersStore != null)
			{
				this._eventHandlersStore.RemoveRoutedEventHandler(routedEvent, handler);
				if ((routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent) && !this._eventHandlersStore.Contains(FrameworkElement.LoadedEvent) && !this._eventHandlersStore.Contains(FrameworkElement.UnloadedEvent))
				{
					this.HasLoadedChangeHandler = false;
				}
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x0017EE22 File Offset: 0x0017DE22
		// (set) Token: 0x06002341 RID: 9025 RVA: 0x0017EE2A File Offset: 0x0017DE2A
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
			set
			{
				this._eventHandlersStore = value;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x0017EE33 File Offset: 0x0017DE33
		// (set) Token: 0x06002343 RID: 9027 RVA: 0x0017EE3B File Offset: 0x0017DE3B
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

		// Token: 0x06002344 RID: 9028 RVA: 0x0017EE44 File Offset: 0x0017DE44
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
			object synchronized;
			if (num >= 0)
			{
				synchronized = this._synchronized;
				lock (synchronized)
				{
					PropertyValue value2 = this.PropertyValues[num];
					value2.ValueType = valueType;
					value2.ValueInternal = value;
					this.PropertyValues[num] = value2;
					return;
				}
			}
			PropertyValue value3 = default(PropertyValue);
			value3.ValueType = valueType;
			value3.ChildName = null;
			value3.Property = dp;
			value3.ValueInternal = value;
			synchronized = this._synchronized;
			lock (synchronized)
			{
				this.PropertyValues.Add(value3);
			}
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x0017EF40 File Offset: 0x0017DF40
		private DependencyObject CreateDependencyObject()
		{
			if (this._knownTypeFactory != null)
			{
				return this._knownTypeFactory() as DependencyObject;
			}
			return (DependencyObject)Activator.CreateInstance(this._type);
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x0017EF6B File Offset: 0x0017DF6B
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x0017EF73 File Offset: 0x0017DF73
		public FrameworkElementFactory Parent
		{
			get
			{
				return this._parent;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x0017EF7B File Offset: 0x0017DF7B
		public FrameworkElementFactory FirstChild
		{
			get
			{
				return this._firstChild;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x0017EF83 File Offset: 0x0017DF83
		public FrameworkElementFactory NextSibling
		{
			get
			{
				return this._nextSibling;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x0017EF8B File Offset: 0x0017DF8B
		internal FrameworkTemplate FrameworkTemplate
		{
			get
			{
				return this._frameworkTemplate;
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x0017EF94 File Offset: 0x0017DF94
		internal object GetValue(DependencyProperty dp)
		{
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].ValueType == PropertyValueType.Set && this.PropertyValues[i].Property == dp)
				{
					return this.PropertyValues[i].ValueInternal;
				}
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x0017EFF5 File Offset: 0x0017DFF5
		internal void Seal(FrameworkTemplate ownerTemplate)
		{
			if (this._sealed)
			{
				return;
			}
			this._frameworkTemplate = ownerTemplate;
			this.Seal();
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x0017F010 File Offset: 0x0017E010
		private void Seal()
		{
			if (this._type == null && this._text == null)
			{
				throw new InvalidOperationException(SR.Get("NullTypeIllegal"));
			}
			if (this._firstChild != null && !typeof(IAddChild).IsAssignableFrom(this._type))
			{
				throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
				{
					this._type.Name
				}));
			}
			this.ApplyAutoAliasRules();
			if (this._childName != null && this._childName != string.Empty)
			{
				if (!this.IsChildNameValid(this._childName))
				{
					throw new InvalidOperationException(SR.Get("ChildNameNamePatternReserved", new object[]
					{
						this._childName
					}));
				}
				this._childName = string.Intern(this._childName);
			}
			else
			{
				this._childName = this.GenerateChildName();
			}
			object synchronized = this._synchronized;
			lock (synchronized)
			{
				for (int i = 0; i < this.PropertyValues.Count; i++)
				{
					PropertyValue propertyValue = this.PropertyValues[i];
					propertyValue.ChildName = this._childName;
					StyleHelper.SealIfSealable(propertyValue.ValueInternal);
					this.PropertyValues[i] = propertyValue;
				}
			}
			this._sealed = true;
			if (this._childName != null && this._childName != string.Empty && this._frameworkTemplate != null)
			{
				this._childIndex = StyleHelper.CreateChildIndexFromChildName(this._childName, this._frameworkTemplate);
			}
			for (FrameworkElementFactory frameworkElementFactory = this._firstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory._nextSibling)
			{
				if (this._frameworkTemplate != null)
				{
					frameworkElementFactory.Seal(this._frameworkTemplate);
				}
			}
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x0017F1D4 File Offset: 0x0017E1D4
		internal DependencyObject InstantiateTree(UncommonField<HybridDictionary[]> dataField, DependencyObject container, DependencyObject parent, List<DependencyObject> affectedChildren, ref List<DependencyObject> noChildIndexChildren, ref FrugalStructList<ChildPropertyDependent> resourceDependents)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseFefCrInstBegin);
			FrameworkElement frameworkElement = container as FrameworkElement;
			bool flag = frameworkElement != null;
			DependencyObject dependencyObject = null;
			if (this._text != null)
			{
				IAddChild addChild = parent as IAddChild;
				if (addChild == null)
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						parent.GetType().Name
					}));
				}
				addChild.AddText(this._text);
			}
			else
			{
				dependencyObject = this.CreateDependencyObject();
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXamlBaml, EventTrace.Level.Verbose, EventTrace.Event.WClientParseFefCrInstEnd);
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
				Visual3D visual3D = null;
				bool flag2 = false;
				if (!frameworkObject.IsValid)
				{
					visual3D = (dependencyObject as Visual3D);
					if (visual3D != null)
					{
						flag2 = true;
					}
				}
				bool isFE = frameworkObject.IsFE;
				if (!flag2)
				{
					FrameworkElementFactory.NewNodeBeginInit(isFE, frameworkObject.FE, frameworkObject.FCE);
					if (StyleHelper.HasResourceDependentsForChild(this._childIndex, ref resourceDependents))
					{
						frameworkObject.HasResourceReference = true;
					}
					FrameworkElementFactory.UpdateChildChains(this._childName, this._childIndex, isFE, frameworkObject.FE, frameworkObject.FCE, affectedChildren, ref noChildIndexChildren);
					FrameworkElementFactory.NewNodeStyledParentProperty(container, flag, isFE, frameworkObject.FE, frameworkObject.FCE);
					if (this._childIndex != -1)
					{
						StyleHelper.CreateInstanceDataForChild(dataField, container, dependencyObject, this._childIndex, this._frameworkTemplate.HasInstanceValues, ref this._frameworkTemplate.ChildRecordFromChildIndex);
					}
					if (this.HasLoadedChangeHandler)
					{
						BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(dependencyObject);
					}
				}
				else if (this._childName != null)
				{
					affectedChildren.Add(dependencyObject);
				}
				else
				{
					if (noChildIndexChildren == null)
					{
						noChildIndexChildren = new List<DependencyObject>(4);
					}
					noChildIndexChildren.Add(dependencyObject);
				}
				if (container == parent)
				{
					TemplateNameScope value = new TemplateNameScope(container);
					NameScope.SetNameScope(dependencyObject, value);
					if (flag)
					{
						frameworkElement.TemplateChild = frameworkObject.FE;
					}
					else
					{
						FrameworkElementFactory.AddNodeToLogicalTree((FrameworkContentElement)parent, this._type, isFE, frameworkObject.FE, frameworkObject.FCE);
					}
				}
				else
				{
					this.AddNodeToParent(parent, frameworkObject);
				}
				if (!flag2)
				{
					StyleHelper.InvalidatePropertiesOnTemplateNode(container, frameworkObject, this._childIndex, ref this._frameworkTemplate.ChildRecordFromChildIndex, false, this);
				}
				else
				{
					for (int i = 0; i < this.PropertyValues.Count; i++)
					{
						if (this.PropertyValues[i].ValueType != PropertyValueType.Set)
						{
							throw new NotSupportedException(SR.Get("Template3DValueOnly", new object[]
							{
								this.PropertyValues[i].Property
							}));
						}
						object obj = this.PropertyValues[i].ValueInternal;
						Freezable freezable = obj as Freezable;
						if (freezable != null && !freezable.CanFreeze)
						{
							obj = freezable.Clone();
						}
						MarkupExtension markupExtension = obj as MarkupExtension;
						if (markupExtension != null)
						{
							ProvideValueServiceProvider provideValueServiceProvider = new ProvideValueServiceProvider();
							provideValueServiceProvider.SetData(visual3D, this.PropertyValues[i].Property);
							obj = markupExtension.ProvideValue(provideValueServiceProvider);
						}
						visual3D.SetValue(this.PropertyValues[i].Property, obj);
					}
				}
				for (FrameworkElementFactory frameworkElementFactory = this._firstChild; frameworkElementFactory != null; frameworkElementFactory = frameworkElementFactory._nextSibling)
				{
					frameworkElementFactory.InstantiateTree(dataField, container, dependencyObject, affectedChildren, ref noChildIndexChildren, ref resourceDependents);
				}
				if (!flag2)
				{
					FrameworkElementFactory.NewNodeEndInit(isFE, frameworkObject.FE, frameworkObject.FCE);
				}
			}
			return dependencyObject;
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x0017F500 File Offset: 0x0017E500
		private void AddNodeToParent(DependencyObject parent, FrameworkObject childFrameworkObject)
		{
			RowDefinition rowDefinition = null;
			Grid grid;
			ColumnDefinition columnDefinition;
			if (childFrameworkObject.IsFCE && (grid = (parent as Grid)) != null && ((columnDefinition = (childFrameworkObject.FCE as ColumnDefinition)) != null || (rowDefinition = (childFrameworkObject.FCE as RowDefinition)) != null))
			{
				if (columnDefinition != null)
				{
					grid.ColumnDefinitions.Add(columnDefinition);
					return;
				}
				if (rowDefinition != null)
				{
					grid.RowDefinitions.Add(rowDefinition);
					return;
				}
			}
			else
			{
				if (!(parent is IAddChild))
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						parent.GetType().Name
					}));
				}
				((IAddChild)parent).AddChild(childFrameworkObject.DO);
			}
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x0017F5A0 File Offset: 0x0017E5A0
		internal FrameworkObject InstantiateUnoptimizedTree()
		{
			if (!this._sealed)
			{
				throw new InvalidOperationException(SR.Get("FrameworkElementFactoryMustBeSealed"));
			}
			FrameworkObject result = new FrameworkObject(this.CreateDependencyObject());
			result.BeginInit();
			ProvideValueServiceProvider provideValueServiceProvider = null;
			FrameworkTemplate.SetTemplateParentValues(this.Name, result.DO, this._frameworkTemplate, ref provideValueServiceProvider);
			FrameworkElementFactory frameworkElementFactory = this._firstChild;
			IAddChild addChild = null;
			if (frameworkElementFactory != null)
			{
				addChild = (result.DO as IAddChild);
				if (addChild == null)
				{
					throw new InvalidOperationException(SR.Get("TypeMustImplementIAddChild", new object[]
					{
						result.DO.GetType().Name
					}));
				}
			}
			while (frameworkElementFactory != null)
			{
				if (frameworkElementFactory._text != null)
				{
					addChild.AddText(frameworkElementFactory._text);
				}
				else
				{
					FrameworkObject childFrameworkObject = frameworkElementFactory.InstantiateUnoptimizedTree();
					this.AddNodeToParent(result.DO, childFrameworkObject);
				}
				frameworkElementFactory = frameworkElementFactory._nextSibling;
			}
			result.EndInit();
			return result;
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x0017F67C File Offset: 0x0017E67C
		private static void UpdateChildChains(string childID, int childIndex, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE, List<DependencyObject> affectedChildren, ref List<DependencyObject> noChildIndexChildren)
		{
			if (childID != null)
			{
				if (treeNodeIsFE)
				{
					treeNodeFE.TemplateChildIndex = childIndex;
				}
				else
				{
					treeNodeFCE.TemplateChildIndex = childIndex;
				}
				affectedChildren.Add(treeNodeIsFE ? treeNodeFE : treeNodeFCE);
				return;
			}
			if (noChildIndexChildren == null)
			{
				noChildIndexChildren = new List<DependencyObject>(4);
			}
			noChildIndexChildren.Add(treeNodeIsFE ? treeNodeFE : treeNodeFCE);
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x0017F6CE File Offset: 0x0017E6CE
		internal static void NewNodeBeginInit(bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE.BeginInit();
				return;
			}
			treeNodeFCE.BeginInit();
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x0017F6E0 File Offset: 0x0017E6E0
		private static void NewNodeEndInit(bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE.EndInit();
				return;
			}
			treeNodeFCE.EndInit();
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x0017F6F2 File Offset: 0x0017E6F2
		private static void NewNodeStyledParentProperty(DependencyObject container, bool isContainerAnFE, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			if (treeNodeIsFE)
			{
				treeNodeFE._templatedParent = container;
				treeNodeFE.IsTemplatedParentAnFE = isContainerAnFE;
				return;
			}
			treeNodeFCE._templatedParent = container;
			treeNodeFCE.IsTemplatedParentAnFE = isContainerAnFE;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x0017F718 File Offset: 0x0017E718
		internal static void AddNodeToLogicalTree(DependencyObject parent, Type type, bool treeNodeIsFE, FrameworkElement treeNodeFE, FrameworkContentElement treeNodeFCE)
		{
			FrameworkContentElement frameworkContentElement = parent as FrameworkContentElement;
			if (frameworkContentElement != null)
			{
				IEnumerator logicalChildren = frameworkContentElement.LogicalChildren;
				if (logicalChildren != null && logicalChildren.MoveNext())
				{
					throw new InvalidOperationException(SR.Get("AlreadyHasLogicalChildren", new object[]
					{
						parent.GetType().Name
					}));
				}
			}
			IAddChild addChild = parent as IAddChild;
			if (addChild == null)
			{
				throw new InvalidOperationException(SR.Get("CannotHookupFCERoot", new object[]
				{
					type.Name
				}));
			}
			if (treeNodeFE != null)
			{
				addChild.AddChild(treeNodeFE);
				return;
			}
			addChild.AddChild(treeNodeFCE);
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0017F7A1 File Offset: 0x0017E7A1
		internal bool IsChildNameValid(string childName)
		{
			return !childName.StartsWith(FrameworkElementFactory.AutoGenChildNamePrefix, StringComparison.Ordinal);
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0017F7B2 File Offset: 0x0017E7B2
		private string GenerateChildName()
		{
			string result = FrameworkElementFactory.AutoGenChildNamePrefix + FrameworkElementFactory.AutoGenChildNamePostfix.ToString(CultureInfo.InvariantCulture);
			Interlocked.Increment(ref FrameworkElementFactory.AutoGenChildNamePostfix);
			return result;
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0017F7D8 File Offset: 0x0017E7D8
		private void ApplyAutoAliasRules()
		{
			if (typeof(ContentPresenter).IsAssignableFrom(this._type))
			{
				object value = this.GetValue(ContentPresenter.ContentSourceProperty);
				string text = (value == DependencyProperty.UnsetValue) ? "Content" : ((string)value);
				if (!string.IsNullOrEmpty(text) && !this.IsValueDefined(ContentPresenter.ContentProperty))
				{
					Type targetTypeInternal = this._frameworkTemplate.TargetTypeInternal;
					DependencyProperty dependencyProperty = DependencyProperty.FromName(text, targetTypeInternal);
					DependencyProperty dependencyProperty2 = DependencyProperty.FromName(text + "Template", targetTypeInternal);
					DependencyProperty dependencyProperty3 = DependencyProperty.FromName(text + "TemplateSelector", targetTypeInternal);
					DependencyProperty dependencyProperty4 = DependencyProperty.FromName(text + "StringFormat", targetTypeInternal);
					if (dependencyProperty == null && value != DependencyProperty.UnsetValue)
					{
						throw new InvalidOperationException(SR.Get("MissingContentSource", new object[]
						{
							text,
							targetTypeInternal
						}));
					}
					if (dependencyProperty != null)
					{
						this.SetValue(ContentPresenter.ContentProperty, new TemplateBindingExtension(dependencyProperty));
					}
					if (!this.IsValueDefined(ContentPresenter.ContentTemplateProperty) && !this.IsValueDefined(ContentPresenter.ContentTemplateSelectorProperty) && !this.IsValueDefined(ContentPresenter.ContentStringFormatProperty))
					{
						if (dependencyProperty2 != null)
						{
							this.SetValue(ContentPresenter.ContentTemplateProperty, new TemplateBindingExtension(dependencyProperty2));
						}
						if (dependencyProperty3 != null)
						{
							this.SetValue(ContentPresenter.ContentTemplateSelectorProperty, new TemplateBindingExtension(dependencyProperty3));
						}
						if (dependencyProperty4 != null)
						{
							this.SetValue(ContentPresenter.ContentStringFormatProperty, new TemplateBindingExtension(dependencyProperty4));
							return;
						}
					}
				}
			}
			else if (typeof(GridViewRowPresenter).IsAssignableFrom(this._type))
			{
				if (!this.IsValueDefined(GridViewRowPresenter.ContentProperty))
				{
					Type targetTypeInternal2 = this._frameworkTemplate.TargetTypeInternal;
					DependencyProperty dependencyProperty5 = DependencyProperty.FromName("Content", targetTypeInternal2);
					if (dependencyProperty5 != null)
					{
						this.SetValue(GridViewRowPresenter.ContentProperty, new TemplateBindingExtension(dependencyProperty5));
					}
				}
				if (!this.IsValueDefined(GridViewRowPresenterBase.ColumnsProperty))
				{
					this.SetValue(GridViewRowPresenterBase.ColumnsProperty, new TemplateBindingExtension(GridView.ColumnCollectionProperty));
				}
			}
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0017F9B4 File Offset: 0x0017E9B4
		private bool IsValueDefined(DependencyProperty dp)
		{
			for (int i = 0; i < this.PropertyValues.Count; i++)
			{
				if (this.PropertyValues[i].Property == dp && (this.PropertyValues[i].ValueType == PropertyValueType.Set || this.PropertyValues[i].ValueType == PropertyValueType.Resource || this.PropertyValues[i].ValueType == PropertyValueType.TemplateBinding))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040010B0 RID: 4272
		private bool _sealed;

		// Token: 0x040010B1 RID: 4273
		internal FrugalStructList<PropertyValue> PropertyValues;

		// Token: 0x040010B2 RID: 4274
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x040010B3 RID: 4275
		internal bool _hasLoadedChangeHandler;

		// Token: 0x040010B4 RID: 4276
		private Type _type;

		// Token: 0x040010B5 RID: 4277
		private string _text;

		// Token: 0x040010B6 RID: 4278
		private Func<object> _knownTypeFactory;

		// Token: 0x040010B7 RID: 4279
		private string _childName;

		// Token: 0x040010B8 RID: 4280
		internal int _childIndex = -1;

		// Token: 0x040010B9 RID: 4281
		private FrameworkTemplate _frameworkTemplate;

		// Token: 0x040010BA RID: 4282
		private static int AutoGenChildNamePostfix = 1;

		// Token: 0x040010BB RID: 4283
		private static string AutoGenChildNamePrefix = "~ChildID";

		// Token: 0x040010BC RID: 4284
		private FrameworkElementFactory _parent;

		// Token: 0x040010BD RID: 4285
		private FrameworkElementFactory _firstChild;

		// Token: 0x040010BE RID: 4286
		private FrameworkElementFactory _lastChild;

		// Token: 0x040010BF RID: 4287
		private FrameworkElementFactory _nextSibling;

		// Token: 0x040010C0 RID: 4288
		private object _synchronized = new object();
	}
}
