using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Baml2006;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xaml;
using MS.Internal;
using MS.Internal.Xaml.Context;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x02000372 RID: 882
	[ContentProperty("VisualTree")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public abstract class FrameworkTemplate : DispatcherObject, INameScope, ISealable, IHaveResources, IQueryAmbient
	{
		// Token: 0x0600238C RID: 9100 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x0600238D RID: 9101 RVA: 0x00180191 File Offset: 0x0017F191
		public bool IsSealed
		{
			get
			{
				base.VerifyAccess();
				return this._sealed;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x0600238E RID: 9102 RVA: 0x0018019F File Offset: 0x0017F19F
		// (set) Token: 0x0600238F RID: 9103 RVA: 0x001801AD File Offset: 0x0017F1AD
		public FrameworkElementFactory VisualTree
		{
			get
			{
				base.VerifyAccess();
				return this._templateRoot;
			}
			set
			{
				base.VerifyAccess();
				this.CheckSealed();
				this.ValidateVisualTree(value);
				this._templateRoot = value;
			}
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x001801C9 File Offset: 0x0017F1C9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeVisualTree()
		{
			base.VerifyAccess();
			return this.HasContent || this.VisualTree != null;
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x001801E4 File Offset: 0x0017F1E4
		// (set) Token: 0x06002392 RID: 9106 RVA: 0x001801EC File Offset: 0x0017F1EC
		[Ambient]
		[DefaultValue(null)]
		public TemplateContent Template
		{
			get
			{
				return this._templateHolder;
			}
			set
			{
				this.CheckSealed();
				if (!this._hasXamlNodeContent)
				{
					value.OwnerTemplate = this;
					value.ParseXaml();
					this._templateHolder = value;
					this._hasXamlNodeContent = true;
					return;
				}
				throw new System.Windows.Markup.XamlParseException(SR.Get("TemplateContentSetTwice"));
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x00180228 File Offset: 0x0017F228
		// (set) Token: 0x06002394 RID: 9108 RVA: 0x00180274 File Offset: 0x0017F274
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
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
				}
				if (this.IsSealed)
				{
					this._resources.IsReadOnly = true;
				}
				return this._resources;
			}
			set
			{
				base.VerifyAccess();
				if (this.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"Template"
					}));
				}
				this._resources = value;
				if (this._resources != null)
				{
					this._resources.CanBeAccessedAcrossThreads = true;
				}
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x001802C8 File Offset: 0x0017F2C8
		// (set) Token: 0x06002396 RID: 9110 RVA: 0x001802D0 File Offset: 0x0017F2D0
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

		// Token: 0x06002397 RID: 9111 RVA: 0x001802DC File Offset: 0x0017F2DC
		internal object FindResource(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			if (this._resources != null && this._resources.Contains(resourceKey))
			{
				bool flag;
				return this._resources.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x00180315 File Offset: 0x0017F315
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return (!(propertyName == "Resources") || this._resources != null) && (!(propertyName == "Template") || this._hasXamlNodeContent);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x00180346 File Offset: 0x0017F346
		public object FindName(string name, FrameworkElement templatedParent)
		{
			base.VerifyAccess();
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (this != templatedParent.TemplateInternal)
			{
				throw new InvalidOperationException(SR.Get("TemplateFindNameInInvalidElement"));
			}
			return StyleHelper.FindNameInTemplateContent(templatedParent, name, this);
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x0018037D File Offset: 0x0017F37D
		public void RegisterName(string name, object scopedElement)
		{
			base.VerifyAccess();
			this._nameScope.RegisterName(name, scopedElement);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x00180392 File Offset: 0x0017F392
		public void UnregisterName(string name)
		{
			base.VerifyAccess();
			this._nameScope.UnregisterName(name);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x001803A6 File Offset: 0x0017F3A6
		object INameScope.FindName(string name)
		{
			base.VerifyAccess();
			return this._nameScope.FindName(name);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x001803BC File Offset: 0x0017F3BC
		private void ValidateVisualTree(FrameworkElementFactory templateRoot)
		{
			if (templateRoot != null && typeof(FrameworkContentElement).IsAssignableFrom(templateRoot.Type))
			{
				throw new ArgumentException(SR.Get("VisualTreeRootIsFrameworkElement", new object[]
				{
					typeof(FrameworkElement).Name,
					templateRoot.Type.Name
				}));
			}
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void ProcessTemplateBeforeSeal()
		{
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x0018041C File Offset: 0x0017F41C
		public void Seal()
		{
			base.VerifyAccess();
			StyleHelper.SealTemplate(this, ref this._sealed, this._templateRoot, this.TriggersInternal, this._resources, this.ChildIndexFromChildName, ref this.ChildRecordFromChildIndex, ref this.TriggerSourceRecordFromChildIndex, ref this.ContainerDependents, ref this.ResourceDependents, ref this.EventDependents, ref this._triggerActions, ref this._dataTriggerRecordFromBinding, ref this._hasInstanceValues, ref this._eventHandlersStore);
			if (this._templateHolder != null)
			{
				this._templateHolder.ResetTemplateLoadData();
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x0018049C File Offset: 0x0017F49C
		internal void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"Template"
				}));
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x001804C4 File Offset: 0x0017F4C4
		internal void SetResourceReferenceState()
		{
			StyleHelper.SortResourceDependents(ref this.ResourceDependents);
			for (int i = 0; i < this.ResourceDependents.Count; i++)
			{
				if (this.ResourceDependents[i].ChildIndex == 0)
				{
					this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasContainerResourceReferences, true);
				}
				else
				{
					this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasChildResourceReferences, true);
				}
			}
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x0018051C File Offset: 0x0017F51C
		internal bool ApplyTemplateContent(UncommonField<HybridDictionary[]> templateDataField, FrameworkElement container)
		{
			if (TraceDependencyProperty.IsEnabled)
			{
				TraceDependencyProperty.Trace(TraceEventType.Start, TraceDependencyProperty.ApplyTemplateContent, container, this);
			}
			this.ValidateTemplatedParent(container);
			bool result = StyleHelper.ApplyTemplateContent(templateDataField, container, this._templateRoot, this._lastChildIndex, this.ChildIndexFromChildName, this);
			if (TraceDependencyProperty.IsEnabled)
			{
				TraceDependencyProperty.Trace(TraceEventType.Stop, TraceDependencyProperty.ApplyTemplateContent, container, this);
			}
			return result;
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x0018057C File Offset: 0x0017F57C
		public DependencyObject LoadContent()
		{
			base.VerifyAccess();
			if (this.VisualTree != null)
			{
				return this.VisualTree.InstantiateUnoptimizedTree().DO;
			}
			return this.LoadContent(null, null);
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x001805B4 File Offset: 0x0017F5B4
		internal DependencyObject LoadContent(DependencyObject container, List<DependencyObject> affectedChildren)
		{
			if (!this.HasContent)
			{
				return null;
			}
			object themeDictionaryLock = SystemResources.ThemeDictionaryLock;
			DependencyObject result;
			lock (themeDictionaryLock)
			{
				result = this.LoadOptimizedTemplateContent(container, null, this._styleConnector, affectedChildren, null);
			}
			return result;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x0018060C File Offset: 0x0017F60C
		internal static bool IsNameScope(XamlType type)
		{
			return typeof(ResourceDictionary).IsAssignableFrom(type.UnderlyingType) || type.IsNameScope;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060023A6 RID: 9126 RVA: 0x0018062D File Offset: 0x0017F62D
		public bool HasContent
		{
			get
			{
				base.VerifyAccess();
				return this._hasXamlNodeContent;
			}
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool BuildVisualTree(FrameworkElement container)
		{
			return false;
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x0018063B File Offset: 0x0017F63B
		// (set) Token: 0x060023A9 RID: 9129 RVA: 0x00180644 File Offset: 0x0017F644
		internal bool CanBuildVisualTree
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.CanBuildVisualTree);
			}
			set
			{
				this.WriteInternalFlag(FrameworkTemplate.InternalFlags.CanBuildVisualTree, value);
			}
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x00180650 File Offset: 0x0017F650
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources(XamlDesignerSerializationManager manager)
		{
			base.VerifyAccess();
			bool result = true;
			if (manager != null)
			{
				result = (manager.XmlWriter == null);
			}
			return result;
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x00180673 File Offset: 0x0017F673
		private bool ReadInternalFlag(FrameworkTemplate.InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (FrameworkTemplate.InternalFlags)0U;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x00180680 File Offset: 0x0017F680
		private void WriteInternalFlag(FrameworkTemplate.InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x001806A4 File Offset: 0x0017F6A4
		private bool ReceivePropertySet(object targetObject, XamlMember member, object value, DependencyObject templatedParent)
		{
			DependencyObject dependencyObject = targetObject as DependencyObject;
			WpfXamlMember wpfXamlMember = member as WpfXamlMember;
			if (!(wpfXamlMember != null))
			{
				return false;
			}
			DependencyProperty dependencyProperty = wpfXamlMember.DependencyProperty;
			if (dependencyProperty == null || dependencyObject == null)
			{
				return false;
			}
			FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
			if (frameworkObject.TemplatedParent == null || templatedParent == null)
			{
				return false;
			}
			if (dependencyProperty == BaseUriHelper.BaseUriProperty)
			{
				if (!frameworkObject.IsInitialized)
				{
					return true;
				}
			}
			else if (dependencyProperty == UIElement.UidProperty)
			{
				return true;
			}
			HybridDictionary hybridDictionary;
			if (!frameworkObject.StoresParentTemplateValues)
			{
				hybridDictionary = new HybridDictionary();
				StyleHelper.ParentTemplateValuesField.SetValue(dependencyObject, hybridDictionary);
				frameworkObject.StoresParentTemplateValues = true;
			}
			else
			{
				hybridDictionary = StyleHelper.ParentTemplateValuesField.GetValue(dependencyObject);
			}
			int templateChildIndex = frameworkObject.TemplateChildIndex;
			Expression expression;
			if ((expression = (value as Expression)) != null)
			{
				BindingExpressionBase bindingExpressionBase;
				TemplateBindingExpression templateBindingExpression;
				if ((bindingExpressionBase = (expression as BindingExpressionBase)) != null)
				{
					HybridDictionary instanceValues = StyleHelper.EnsureInstanceData(StyleHelper.TemplateDataField, templatedParent, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValue(dependencyObject, templateChildIndex, instanceValues, dependencyProperty, -1, true);
					value = bindingExpressionBase.ParentBindingBase;
				}
				else if ((templateBindingExpression = (expression as TemplateBindingExpression)) != null)
				{
					TemplateBindingExtension templateBindingExtension = templateBindingExpression.TemplateBindingExtension;
					HybridDictionary instanceValues2 = StyleHelper.EnsureInstanceData(StyleHelper.TemplateDataField, templatedParent, InstanceStyleData.InstanceValues);
					StyleHelper.ProcessInstanceValue(dependencyObject, templateChildIndex, instanceValues2, dependencyProperty, -1, true);
					value = new Binding
					{
						Mode = BindingMode.OneWay,
						RelativeSource = RelativeSource.TemplatedParent,
						Path = new PropertyPath(templateBindingExtension.Property),
						Converter = templateBindingExtension.Converter,
						ConverterParameter = templateBindingExtension.ConverterParameter
					};
				}
			}
			bool flag = value is MarkupExtension;
			if (!dependencyProperty.IsValidValue(value) && !flag && !(value is DeferredReference))
			{
				throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
				{
					value,
					dependencyProperty.Name
				}));
			}
			hybridDictionary[dependencyProperty] = value;
			dependencyObject.ProvideSelfAsInheritanceContext(value, dependencyProperty);
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dependencyProperty);
			effectiveValueEntry.BaseValueSourceInternal = BaseValueSourceInternal.ParentTemplate;
			effectiveValueEntry.Value = value;
			if (flag)
			{
				StyleHelper.GetInstanceValue(StyleHelper.TemplateDataField, templatedParent, frameworkObject.FE, frameworkObject.FCE, templateChildIndex, dependencyProperty, -1, ref effectiveValueEntry);
			}
			dependencyObject.UpdateEffectiveValue(dependencyObject.LookupEntry(dependencyProperty.GlobalIndex), dependencyProperty, dependencyProperty.GetMetadata(dependencyObject.DependencyObjectType), default(EffectiveValueEntry), ref effectiveValueEntry, false, false, OperationType.Unknown);
			return true;
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x001808C8 File Offset: 0x0017F8C8
		private DependencyObject LoadOptimizedTemplateContent(DependencyObject container, IComponentConnector componentConnector, IStyleConnector styleConnector, List<DependencyObject> affectedChildren, UncommonField<Hashtable> templatedNonFeChildrenField)
		{
			if (this.Names == null)
			{
				this.Names = new XamlContextStack<FrameworkTemplate.Frame>(() => new FrameworkTemplate.Frame());
			}
			DependencyObject rootObject = null;
			if (TraceMarkup.IsEnabled)
			{
				TraceMarkup.Trace(TraceEventType.Start, TraceMarkup.Load);
			}
			FrameworkElement feContainer = container as FrameworkElement;
			FrameworkElement feContainer2 = feContainer;
			TemplateNameScope nameScope = new TemplateNameScope(container, affectedChildren, this);
			XamlObjectWriterSettings xamlObjectWriterSettings = System.Windows.Markup.XamlReader.CreateObjectWriterSettings(this._templateHolder.ObjectWriterParentSettings);
			xamlObjectWriterSettings.ExternalNameScope = nameScope;
			xamlObjectWriterSettings.RegisterNamesOnExternalNamescope = true;
			IEnumerator<string> nameEnumerator = this.ChildNames.GetEnumerator();
			xamlObjectWriterSettings.AfterBeginInitHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				this.HandleAfterBeginInit(args.Instance, ref rootObject, container, feContainer, nameScope, nameEnumerator);
				if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
				{
					XamlSourceInfoHelper.SetXamlSourceInfo(args.Instance, args, null);
				}
			};
			xamlObjectWriterSettings.BeforePropertiesHandler = delegate(object sender, XamlObjectEventArgs args)
			{
				this.HandleBeforeProperties(args.Instance, ref rootObject, container, feContainer, nameScope);
			};
			xamlObjectWriterSettings.XamlSetValueHandler = delegate(object sender, XamlSetValueEventArgs setArgs)
			{
				setArgs.Handled = this.ReceivePropertySet(sender, setArgs.Member, setArgs.Value, container);
			};
			XamlObjectWriter xamlObjectWriter = this._templateHolder.ObjectWriterFactory.GetXamlObjectWriter(xamlObjectWriterSettings);
			try
			{
				this.LoadTemplateXaml(xamlObjectWriter);
			}
			finally
			{
				if (TraceMarkup.IsEnabled)
				{
					TraceMarkup.Trace(TraceEventType.Stop, TraceMarkup.Load, rootObject);
				}
			}
			return rootObject;
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x00180A24 File Offset: 0x0017FA24
		private void LoadTemplateXaml(XamlObjectWriter objectWriter)
		{
			System.Xaml.XamlReader templateReader = this._templateHolder.PlayXaml();
			this.LoadTemplateXaml(templateReader, objectWriter);
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x00180A48 File Offset: 0x0017FA48
		private void LoadTemplateXaml(System.Xaml.XamlReader templateReader, XamlObjectWriter currentWriter)
		{
			try
			{
				int num = 0;
				IXamlLineInfoConsumer xamlLineInfoConsumer = null;
				IXamlLineInfo xamlLineInfo = null;
				if (XamlSourceInfoHelper.IsXamlSourceInfoEnabled)
				{
					xamlLineInfo = (templateReader as IXamlLineInfo);
					if (xamlLineInfo != null)
					{
						xamlLineInfoConsumer = currentWriter;
					}
				}
				while (templateReader.Read())
				{
					if (xamlLineInfoConsumer != null)
					{
						xamlLineInfoConsumer.SetLineInfo(xamlLineInfo.LineNumber, xamlLineInfo.LinePosition);
					}
					currentWriter.WriteNode(templateReader);
					switch (templateReader.NodeType)
					{
					case System.Xaml.XamlNodeType.StartObject:
					{
						bool flag = this.Names.Depth > 0 && (FrameworkTemplate.IsNameScope(this.Names.CurrentFrame.Type) || this.Names.CurrentFrame.InsideNameScope);
						this.Names.PushScope();
						this.Names.CurrentFrame.Type = templateReader.Type;
						if (flag)
						{
							this.Names.CurrentFrame.InsideNameScope = true;
						}
						break;
					}
					case System.Xaml.XamlNodeType.GetObject:
					{
						bool flag2 = FrameworkTemplate.IsNameScope(this.Names.CurrentFrame.Type) || this.Names.CurrentFrame.InsideNameScope;
						this.Names.PushScope();
						this.Names.CurrentFrame.Type = this.Names.PreviousFrame.Property.Type;
						if (flag2)
						{
							this.Names.CurrentFrame.InsideNameScope = true;
						}
						break;
					}
					case System.Xaml.XamlNodeType.EndObject:
						this.Names.PopScope();
						break;
					case System.Xaml.XamlNodeType.StartMember:
						this.Names.CurrentFrame.Property = templateReader.Member;
						if (templateReader.Member.DeferringLoader != null)
						{
							num++;
						}
						break;
					case System.Xaml.XamlNodeType.EndMember:
						if (this.Names.CurrentFrame.Property.DeferringLoader != null)
						{
							num--;
						}
						this.Names.CurrentFrame.Property = null;
						break;
					case System.Xaml.XamlNodeType.Value:
						if (num == 0 && this.Names.CurrentFrame.Property == XamlLanguage.ConnectionId && this._styleConnector != null)
						{
							this._styleConnector.Connect((int)templateReader.Value, this.Names.CurrentFrame.Instance);
						}
						break;
					}
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalException(ex) || ex is System.Windows.Markup.XamlParseException)
				{
					throw;
				}
				System.Windows.Markup.XamlReader.RewrapException(ex, null);
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x00180CC0 File Offset: 0x0017FCC0
		internal static bool IsNameProperty(XamlMember member, XamlType owner)
		{
			return member == owner.GetAliasedProperty(XamlLanguage.Name) || XamlLanguage.Name == member;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x00180CE8 File Offset: 0x0017FCE8
		private void HandleAfterBeginInit(object createdObject, ref DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, TemplateNameScope nameScope, IEnumerator<string> nameEnumerator)
		{
			if (!this.Names.CurrentFrame.InsideNameScope && (createdObject is FrameworkElement || createdObject is FrameworkContentElement))
			{
				nameEnumerator.MoveNext();
				nameScope.RegisterNameInternal(nameEnumerator.Current, createdObject);
			}
			this.Names.CurrentFrame.Instance = createdObject;
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x00180D3F File Offset: 0x0017FD3F
		private void HandleBeforeProperties(object createdObject, ref DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, INameScope nameScope)
		{
			if (createdObject is FrameworkElement || createdObject is FrameworkContentElement)
			{
				if (rootObject == null)
				{
					rootObject = FrameworkTemplate.WireRootObjectToParent(createdObject, rootObject, container, feContainer, nameScope);
				}
				this.InvalidatePropertiesOnTemplate(container, createdObject);
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x00180D6C File Offset: 0x0017FD6C
		private static DependencyObject WireRootObjectToParent(object createdObject, DependencyObject rootObject, DependencyObject container, FrameworkElement feContainer, INameScope nameScope)
		{
			rootObject = (createdObject as DependencyObject);
			if (rootObject != null)
			{
				if (feContainer != null)
				{
					UIElement uielement = rootObject as UIElement;
					if (uielement == null)
					{
						throw new InvalidOperationException(SR.Get("TemplateMustBeFE", new object[]
						{
							rootObject.GetType().FullName
						}));
					}
					feContainer.TemplateChild = uielement;
				}
				else if (container != null)
				{
					FrameworkElement frameworkElement;
					FrameworkContentElement treeNodeFCE;
					Helper.DowncastToFEorFCE(rootObject, out frameworkElement, out treeNodeFCE, true);
					FrameworkElementFactory.AddNodeToLogicalTree((FrameworkContentElement)container, rootObject.GetType(), frameworkElement != null, frameworkElement, treeNodeFCE);
				}
				if (NameScope.GetNameScope(rootObject) == null)
				{
					NameScope.SetNameScope(rootObject, nameScope);
				}
			}
			return rootObject;
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x00180DF4 File Offset: 0x0017FDF4
		private void InvalidatePropertiesOnTemplate(DependencyObject container, object currentObject)
		{
			if (container != null)
			{
				DependencyObject dependencyObject = currentObject as DependencyObject;
				if (dependencyObject != null)
				{
					FrameworkObject child = new FrameworkObject(dependencyObject);
					if (child.IsValid)
					{
						int templateChildIndex = child.TemplateChildIndex;
						if (StyleHelper.HasResourceDependentsForChild(templateChildIndex, ref this.ResourceDependents))
						{
							child.HasResourceReference = true;
						}
						StyleHelper.InvalidatePropertiesOnTemplateNode(container, child, templateChildIndex, ref this.ChildRecordFromChildIndex, false, this.VisualTree);
					}
				}
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00180E54 File Offset: 0x0017FE54
		internal static void SetTemplateParentValues(string name, object element, FrameworkTemplate frameworkTemplate, ref ProvideValueServiceProvider provideValueServiceProvider)
		{
			if (!frameworkTemplate.IsSealed)
			{
				frameworkTemplate.Seal();
			}
			HybridDictionary childIndexFromChildName = frameworkTemplate.ChildIndexFromChildName;
			FrugalStructList<ChildRecord> childRecordFromChildIndex = frameworkTemplate.ChildRecordFromChildIndex;
			int num = StyleHelper.QueryChildIndexFromChildName(name, childIndexFromChildName);
			if (num < childRecordFromChildIndex.Count)
			{
				ChildRecord childRecord = childRecordFromChildIndex[num];
				for (int i = 0; i < childRecord.ValueLookupListFromProperty.Count; i++)
				{
					for (int j = 0; j < childRecord.ValueLookupListFromProperty.Entries[i].Value.Count; j++)
					{
						ChildValueLookup childValueLookup = childRecord.ValueLookupListFromProperty.Entries[i].Value.List[j];
						if (childValueLookup.LookupType == ValueLookupType.Simple || childValueLookup.LookupType == ValueLookupType.Resource || childValueLookup.LookupType == ValueLookupType.TemplateBinding)
						{
							object obj = childValueLookup.Value;
							if (childValueLookup.LookupType == ValueLookupType.TemplateBinding)
							{
								obj = new TemplateBindingExpression(obj as TemplateBindingExtension);
							}
							else if (childValueLookup.LookupType == ValueLookupType.Resource)
							{
								obj = new ResourceReferenceExpression(obj);
							}
							MarkupExtension markupExtension = obj as MarkupExtension;
							if (markupExtension != null)
							{
								if (provideValueServiceProvider == null)
								{
									provideValueServiceProvider = new ProvideValueServiceProvider();
								}
								provideValueServiceProvider.SetData(element, childValueLookup.Property);
								obj = markupExtension.ProvideValue(provideValueServiceProvider);
								provideValueServiceProvider.ClearData();
							}
							(element as DependencyObject).SetValue(childValueLookup.Property, obj);
						}
					}
				}
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual Type TargetTypeInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060023B8 RID: 9144
		internal abstract void SetTargetTypeInternal(Type targetType);

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x060023B9 RID: 9145 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual object DataTypeInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060023BA RID: 9146 RVA: 0x000FD7B7 File Offset: 0x000FC7B7
		bool ISealable.CanSeal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x060023BB RID: 9147 RVA: 0x00180FB4 File Offset: 0x0017FFB4
		bool ISealable.IsSealed
		{
			get
			{
				return this.IsSealed;
			}
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x00180FBC File Offset: 0x0017FFBC
		void ISealable.Seal()
		{
			this.Seal();
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual TriggerCollection TriggersInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x00180FC4 File Offset: 0x0017FFC4
		internal bool HasResourceReferences
		{
			get
			{
				return this.ResourceDependents.Count > 0;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x00180FD4 File Offset: 0x0017FFD4
		internal bool HasContainerResourceReferences
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasContainerResourceReferences);
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x00180FDE File Offset: 0x0017FFDE
		internal bool HasChildResourceReferences
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasChildResourceReferences);
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x00180FE8 File Offset: 0x0017FFE8
		internal bool HasEventDependents
		{
			get
			{
				return this.EventDependents.Count > 0;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x00180FF8 File Offset: 0x0017FFF8
		internal bool HasInstanceValues
		{
			get
			{
				return this._hasInstanceValues;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060023C3 RID: 9155 RVA: 0x00181000 File Offset: 0x00180000
		// (set) Token: 0x060023C4 RID: 9156 RVA: 0x00181009 File Offset: 0x00180009
		internal bool HasLoadedChangeHandler
		{
			get
			{
				return this.ReadInternalFlag(FrameworkTemplate.InternalFlags.HasLoadedChangeHandler);
			}
			set
			{
				this.WriteInternalFlag(FrameworkTemplate.InternalFlags.HasLoadedChangeHandler, value);
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x00181013 File Offset: 0x00180013
		internal void CopyParserContext(ParserContext parserContext)
		{
			this._parserContext = parserContext.ScopedCopy(false);
			this._parserContext.SkipJournaledProperties = false;
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060023C6 RID: 9158 RVA: 0x0018102E File Offset: 0x0018002E
		internal ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060023C7 RID: 9159 RVA: 0x00181036 File Offset: 0x00180036
		internal EventHandlersStore EventHandlersStore
		{
			get
			{
				return this._eventHandlersStore;
			}
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060023C8 RID: 9160 RVA: 0x0018103E File Offset: 0x0018003E
		// (set) Token: 0x060023C9 RID: 9161 RVA: 0x00181046 File Offset: 0x00180046
		internal IStyleConnector StyleConnector
		{
			get
			{
				return this._styleConnector;
			}
			set
			{
				this._styleConnector = value;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060023CA RID: 9162 RVA: 0x0018104F File Offset: 0x0018004F
		// (set) Token: 0x060023CB RID: 9163 RVA: 0x00181057 File Offset: 0x00180057
		internal IComponentConnector ComponentConnector
		{
			get
			{
				return this._componentConnector;
			}
			set
			{
				this._componentConnector = value;
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060023CC RID: 9164 RVA: 0x00181060 File Offset: 0x00180060
		// (set) Token: 0x060023CD RID: 9165 RVA: 0x00181068 File Offset: 0x00180068
		internal object[] StaticResourceValues
		{
			get
			{
				return this._staticResourceValues;
			}
			set
			{
				this._staticResourceValues = value;
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x060023CE RID: 9166 RVA: 0x00181071 File Offset: 0x00180071
		internal bool HasXamlNodeContent
		{
			get
			{
				return this._hasXamlNodeContent;
			}
		}

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x00181079 File Offset: 0x00180079
		internal HybridDictionary ChildIndexFromChildName
		{
			get
			{
				return this._childIndexFromChildName;
			}
		}

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x060023D0 RID: 9168 RVA: 0x00181081 File Offset: 0x00180081
		internal Dictionary<int, Type> ChildTypeFromChildIndex
		{
			get
			{
				return this._childTypeFromChildIndex;
			}
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x00181089 File Offset: 0x00180089
		// (set) Token: 0x060023D2 RID: 9170 RVA: 0x00181091 File Offset: 0x00180091
		internal int LastChildIndex
		{
			get
			{
				return this._lastChildIndex;
			}
			set
			{
				this._lastChildIndex = value;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x0018109A File Offset: 0x0018009A
		internal List<string> ChildNames
		{
			get
			{
				return this._childNames;
			}
		}

		// Token: 0x040010CF RID: 4303
		private NameScope _nameScope = new NameScope();

		// Token: 0x040010D0 RID: 4304
		private XamlContextStack<FrameworkTemplate.Frame> Names;

		// Token: 0x040010D1 RID: 4305
		private FrameworkTemplate.InternalFlags _flags;

		// Token: 0x040010D2 RID: 4306
		private bool _sealed;

		// Token: 0x040010D3 RID: 4307
		internal bool _hasInstanceValues;

		// Token: 0x040010D4 RID: 4308
		private ParserContext _parserContext;

		// Token: 0x040010D5 RID: 4309
		private IStyleConnector _styleConnector;

		// Token: 0x040010D6 RID: 4310
		private IComponentConnector _componentConnector;

		// Token: 0x040010D7 RID: 4311
		private FrameworkElementFactory _templateRoot;

		// Token: 0x040010D8 RID: 4312
		private TemplateContent _templateHolder;

		// Token: 0x040010D9 RID: 4313
		private bool _hasXamlNodeContent;

		// Token: 0x040010DA RID: 4314
		private HybridDictionary _childIndexFromChildName = new HybridDictionary();

		// Token: 0x040010DB RID: 4315
		private Dictionary<int, Type> _childTypeFromChildIndex = new Dictionary<int, Type>();

		// Token: 0x040010DC RID: 4316
		private int _lastChildIndex = 1;

		// Token: 0x040010DD RID: 4317
		private List<string> _childNames = new List<string>();

		// Token: 0x040010DE RID: 4318
		internal ResourceDictionary _resources;

		// Token: 0x040010DF RID: 4319
		internal HybridDictionary _triggerActions;

		// Token: 0x040010E0 RID: 4320
		internal FrugalStructList<ChildRecord> ChildRecordFromChildIndex;

		// Token: 0x040010E1 RID: 4321
		internal FrugalStructList<ItemStructMap<TriggerSourceRecord>> TriggerSourceRecordFromChildIndex;

		// Token: 0x040010E2 RID: 4322
		internal FrugalMap PropertyTriggersWithActions;

		// Token: 0x040010E3 RID: 4323
		internal FrugalStructList<ContainerDependent> ContainerDependents;

		// Token: 0x040010E4 RID: 4324
		internal FrugalStructList<ChildPropertyDependent> ResourceDependents;

		// Token: 0x040010E5 RID: 4325
		internal HybridDictionary _dataTriggerRecordFromBinding;

		// Token: 0x040010E6 RID: 4326
		internal HybridDictionary DataTriggersWithActions;

		// Token: 0x040010E7 RID: 4327
		internal ConditionalWeakTable<DependencyObject, List<DeferredAction>> DeferredActions;

		// Token: 0x040010E8 RID: 4328
		internal HybridDictionary _TemplateChildLoadedDictionary = new HybridDictionary();

		// Token: 0x040010E9 RID: 4329
		internal ItemStructList<ChildEventDependent> EventDependents = new ItemStructList<ChildEventDependent>(1);

		// Token: 0x040010EA RID: 4330
		private EventHandlersStore _eventHandlersStore;

		// Token: 0x040010EB RID: 4331
		private object[] _staticResourceValues;

		// Token: 0x02000A7E RID: 2686
		private class Frame : XamlFrame
		{
			// Token: 0x17001E17 RID: 7703
			// (get) Token: 0x0600865A RID: 34394 RVA: 0x0032A648 File Offset: 0x00329648
			// (set) Token: 0x0600865B RID: 34395 RVA: 0x0032A650 File Offset: 0x00329650
			public XamlType Type { get; set; }

			// Token: 0x17001E18 RID: 7704
			// (get) Token: 0x0600865C RID: 34396 RVA: 0x0032A659 File Offset: 0x00329659
			// (set) Token: 0x0600865D RID: 34397 RVA: 0x0032A661 File Offset: 0x00329661
			public XamlMember Property { get; set; }

			// Token: 0x17001E19 RID: 7705
			// (get) Token: 0x0600865E RID: 34398 RVA: 0x0032A66A File Offset: 0x0032966A
			// (set) Token: 0x0600865F RID: 34399 RVA: 0x0032A672 File Offset: 0x00329672
			public bool InsideNameScope { get; set; }

			// Token: 0x17001E1A RID: 7706
			// (get) Token: 0x06008660 RID: 34400 RVA: 0x0032A67B File Offset: 0x0032967B
			// (set) Token: 0x06008661 RID: 34401 RVA: 0x0032A683 File Offset: 0x00329683
			public object Instance { get; set; }

			// Token: 0x06008662 RID: 34402 RVA: 0x0032A68C File Offset: 0x0032968C
			public override void Reset()
			{
				this.Type = null;
				this.Property = null;
				this.Instance = null;
				if (this.InsideNameScope)
				{
					this.InsideNameScope = false;
				}
			}
		}

		// Token: 0x02000A7F RID: 2687
		internal class TemplateChildLoadedFlags
		{
			// Token: 0x0400418A RID: 16778
			public bool HasLoadedChangedHandler;

			// Token: 0x0400418B RID: 16779
			public bool HasUnloadedChangedHandler;
		}

		// Token: 0x02000A80 RID: 2688
		[Flags]
		private enum InternalFlags : uint
		{
			// Token: 0x0400418D RID: 16781
			CanBuildVisualTree = 4U,
			// Token: 0x0400418E RID: 16782
			HasLoadedChangeHandler = 8U,
			// Token: 0x0400418F RID: 16783
			HasContainerResourceReferences = 16U,
			// Token: 0x04004190 RID: 16784
			HasChildResourceReferences = 32U
		}
	}
}
