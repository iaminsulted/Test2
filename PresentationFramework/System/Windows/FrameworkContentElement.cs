using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x02000366 RID: 870
	[XmlLangProperty("Language")]
	[StyleTypedProperty(Property = "FocusVisualStyle", StyleTargetType = typeof(Control))]
	[UsableDuringInitialization(true)]
	[RuntimeNameProperty("Name")]
	public class FrameworkContentElement : ContentElement, IFrameworkInputElement, IInputElement, ISupportInitialize, IQueryAmbient
	{
		// Token: 0x060020D6 RID: 8406 RVA: 0x00176754 File Offset: 0x00175754
		public FrameworkContentElement()
		{
			PropertyMetadata metadata = FrameworkContentElement.StyleProperty.GetMetadata(base.DependencyObjectType);
			Style style = (Style)metadata.DefaultValue;
			if (style != null)
			{
				FrameworkContentElement.OnStyleChanged(this, new DependencyPropertyChangedEventArgs(FrameworkContentElement.StyleProperty, metadata, null, style));
			}
			Application application = Application.Current;
			if (application != null && application.HasImplicitStylesInResources)
			{
				this.ShouldLookupImplicitStyles = true;
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x001767C0 File Offset: 0x001757C0
		static FrameworkContentElement()
		{
			FrameworkContentElement.LoadedEvent = FrameworkElement.LoadedEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.UnloadedEvent = FrameworkElement.UnloadedEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ToolTipProperty = ToolTipService.ToolTipProperty.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuProperty = ContextMenuService.ContextMenuProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null));
			FrameworkContentElement.ToolTipOpeningEvent = ToolTipService.ToolTipOpeningEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ToolTipClosingEvent = ToolTipService.ToolTipClosingEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuOpeningEvent = ContextMenuService.ContextMenuOpeningEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ContextMenuClosingEvent = ContextMenuService.ContextMenuClosingEvent.AddOwner(typeof(FrameworkContentElement));
			FrameworkContentElement.ResourcesField = FrameworkElement.ResourcesField;
			FrameworkContentElement.DType = DependencyObjectType.FromSystemTypeInternal(typeof(FrameworkContentElement));
			FrameworkContentElement.InheritanceContextField = new UncommonField<DependencyObject>();
			FrameworkContentElement.MentorField = new UncommonField<DependencyObject>();
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FrameworkContentElement.NumberSubstitutionChanged);
			NumberSubstitution.CultureSourceProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(NumberCultureSource.Text, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.CultureOverrideProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.SubstitutionProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(NumberSubstitutionMethod.AsCulture, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Mouse.QueryCursorEvent, new QueryCursorEventHandler(FrameworkContentElement.OnQueryCursor), true);
			ContentElement.AllowDropProperty.OverrideMetadata(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsPressAndHoldEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsFlicksEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTapFeedbackEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTouchFeedbackEnabledProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ToolTipOpeningEvent, new ToolTipEventHandler(FrameworkContentElement.OnToolTipOpeningThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ToolTipClosingEvent, new ToolTipEventHandler(FrameworkContentElement.OnToolTipClosingThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkContentElement.OnContextMenuOpeningThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), FrameworkContentElement.ContextMenuClosingEvent, new ContextMenuEventHandler(FrameworkContentElement.OnContextMenuClosingThunk));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkContentElement.OnGotKeyboardFocus));
			EventManager.RegisterClassHandler(typeof(FrameworkContentElement), Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkContentElement.OnLostKeyboardFocus));
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00176CE1 File Offset: 0x00175CE1
		private static void NumberSubstitutionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkContentElement)o).HasNumberSubstitutionChanged = true;
		}

		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x00176CEF File Offset: 0x00175CEF
		// (set) Token: 0x060020DA RID: 8410 RVA: 0x00176CF7 File Offset: 0x00175CF7
		public Style Style
		{
			get
			{
				return this._styleCache;
			}
			set
			{
				base.SetValue(FrameworkContentElement.StyleProperty, value);
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00176D05 File Offset: 0x00175D05
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeStyle()
		{
			return !this.IsStyleSetFromGenerator && base.ReadLocalValue(FrameworkContentElement.StyleProperty) != DependencyProperty.UnsetValue;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00176D28 File Offset: 0x00175D28
		private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			frameworkContentElement.HasLocalStyle = (e.NewEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local);
			StyleHelper.UpdateStyleCache(null, frameworkContentElement, (Style)e.OldValue, (Style)e.NewValue, ref frameworkContentElement._styleCache);
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00176D7A File Offset: 0x00175D7A
		protected internal virtual void OnStyleChanged(Style oldStyle, Style newStyle)
		{
			this.HasStyleChanged = true;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00176D83 File Offset: 0x00175D83
		// (set) Token: 0x060020DF RID: 8415 RVA: 0x00176D95 File Offset: 0x00175D95
		public bool OverridesDefaultStyle
		{
			get
			{
				return (bool)base.GetValue(FrameworkContentElement.OverridesDefaultStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.OverridesDefaultStyleProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x00176DA8 File Offset: 0x00175DA8
		// (set) Token: 0x060020E1 RID: 8417 RVA: 0x00176DB5 File Offset: 0x00175DB5
		protected internal object DefaultStyleKey
		{
			get
			{
				return base.GetValue(FrameworkContentElement.DefaultStyleKeyProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.DefaultStyleKeyProperty, value);
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00176DC3 File Offset: 0x00175DC3
		private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkContentElement)d).UpdateThemeStyleProperty();
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00176DD0 File Offset: 0x00175DD0
		internal Style ThemeStyle
		{
			get
			{
				return this._themeStyleCache;
			}
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00176DD8 File Offset: 0x00175DD8
		internal static void OnThemeStyleChanged(DependencyObject d, object oldValue, object newValue)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)d;
			StyleHelper.UpdateThemeStyleCache(null, frameworkContentElement, (Style)oldValue, (Style)newValue, ref frameworkContentElement._themeStyleCache);
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x00176E05 File Offset: 0x00175E05
		public DependencyObject TemplatedParent
		{
			get
			{
				return this._templatedParent;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x00176E10 File Offset: 0x00175E10
		internal bool HasResources
		{
			get
			{
				ResourceDictionary value = FrameworkContentElement.ResourcesField.GetValue(this);
				return value != null && (value.Count > 0 || value.MergedDictionaries.Count > 0);
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x00176E48 File Offset: 0x00175E48
		// (set) Token: 0x060020E9 RID: 8425 RVA: 0x00176E80 File Offset: 0x00175E80
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				ResourceDictionary resourceDictionary = FrameworkContentElement.ResourcesField.GetValue(this);
				if (resourceDictionary == null)
				{
					resourceDictionary = new ResourceDictionary();
					resourceDictionary.AddOwner(this);
					FrameworkContentElement.ResourcesField.SetValue(this, resourceDictionary);
				}
				return resourceDictionary;
			}
			set
			{
				ResourceDictionary value2 = FrameworkContentElement.ResourcesField.GetValue(this);
				FrameworkContentElement.ResourcesField.SetValue(this, value);
				if (value2 != null)
				{
					value2.RemoveOwner(this);
				}
				if (value != null && !value.ContainsOwner(this))
				{
					value.AddOwner(this);
				}
				if (value2 != value)
				{
					TreeWalkHelper.InvalidateOnResourcesChange(null, this, new ResourcesChangeInfo(value2, value));
				}
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00176ED4 File Offset: 0x00175ED4
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName != "Resources" || this.HasResources;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00176EEB File Offset: 0x00175EEB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources()
		{
			return this.Resources != null && this.Resources.Count != 0;
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00176F05 File Offset: 0x00175F05
		public object FindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(null, this, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00176F2C File Offset: 0x00175F2C
		public object TryFindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(null, this, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00176F5B File Offset: 0x00175F5B
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			base.SetValue(dp, new ResourceReferenceExpression(name));
			this.HasResourceReference = true;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00176F71 File Offset: 0x00175F71
		public void BeginStoryboard(Storyboard storyboard)
		{
			this.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace, false);
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00176F7C File Offset: 0x00175F7C
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior)
		{
			this.BeginStoryboard(storyboard, handoffBehavior, false);
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00176F87 File Offset: 0x00175F87
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior, bool isControllable)
		{
			if (storyboard == null)
			{
				throw new ArgumentNullException("storyboard");
			}
			storyboard.Begin(this, handoffBehavior, isControllable);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x00176FA0 File Offset: 0x00175FA0
		internal sealed override void EvaluateBaseValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
		{
			if (dp == FrameworkContentElement.StyleProperty)
			{
				this.HasStyleEverBeenFetched = true;
				this.HasImplicitStyleFromResources = false;
				this.IsStyleSetFromGenerator = false;
			}
			this.GetRawValue(dp, metadata, ref newEntry);
			Storyboard.GetComplexPathValue(this, dp, ref newEntry, metadata);
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00176FD4 File Offset: 0x00175FD4
		internal void GetRawValue(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry entry)
		{
			if (entry.BaseValueSourceInternal == BaseValueSourceInternal.Local && entry.GetFlattenedEntry(RequestFlags.FullyResolved).Value != DependencyProperty.UnsetValue)
			{
				return;
			}
			if (this.TemplateChildIndex != -1 && this.GetValueFromTemplatedParent(dp, ref entry))
			{
				return;
			}
			if (dp != FrameworkContentElement.StyleProperty)
			{
				if (StyleHelper.GetValueFromStyleOrTemplate(new FrameworkObject(null, this), dp, ref entry))
				{
					return;
				}
			}
			else
			{
				object obj2;
				object obj = FrameworkElement.FindImplicitStyleResource(this, base.GetType(), out obj2);
				if (obj != DependencyProperty.UnsetValue)
				{
					this.HasImplicitStyleFromResources = true;
					entry.BaseValueSourceInternal = BaseValueSourceInternal.ImplicitReference;
					entry.Value = obj;
					return;
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits && (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) || frameworkPropertyMetadata.OverridesInheritanceBehavior))
			{
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				while (frameworkParent)
				{
					InheritanceBehavior inheritanceBehavior;
					bool flag;
					if (frameworkElement != null)
					{
						flag = TreeWalkHelper.IsInheritanceNode(frameworkElement, dp, out inheritanceBehavior);
					}
					else
					{
						flag = TreeWalkHelper.IsInheritanceNode(frameworkContentElement, dp, out inheritanceBehavior);
					}
					if (TreeWalkHelper.SkipNow(inheritanceBehavior))
					{
						break;
					}
					if (flag)
					{
						if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
						{
							string text = "[" + base.GetType().Name + "]" + dp.Name;
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientPropParentCheck, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
							{
								this.GetHashCode(),
								text
							});
						}
						DependencyObject dependencyObject = frameworkElement;
						if (dependencyObject == null)
						{
							dependencyObject = frameworkContentElement;
						}
						EntryIndex entryIndex = dependencyObject.LookupEntry(dp.GlobalIndex);
						entry = dependencyObject.GetValueEntry(entryIndex, dp, frameworkPropertyMetadata, (RequestFlags)12);
						if (entry.Value != DependencyProperty.UnsetValue)
						{
							entry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
						}
						return;
					}
					if (TreeWalkHelper.SkipNext(inheritanceBehavior))
					{
						break;
					}
					if (frameworkElement != null)
					{
						frameworkParent = FrameworkElement.GetFrameworkParent(frameworkElement, out frameworkElement, out frameworkContentElement);
					}
					else
					{
						frameworkParent = FrameworkElement.GetFrameworkParent(frameworkContentElement, out frameworkElement, out frameworkContentElement);
					}
				}
			}
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00177194 File Offset: 0x00176194
		private bool GetValueFromTemplatedParent(DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			FrameworkTemplate templateInternal = ((FrameworkElement)this._templatedParent).TemplateInternal;
			return templateInternal != null && StyleHelper.GetValueFromTemplatedParent(this._templatedParent, this.TemplateChildIndex, new FrameworkObject(null, this), dp, ref templateInternal.ChildRecordFromChildIndex, templateInternal.VisualTree, ref entry);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x001771E0 File Offset: 0x001761E0
		internal Expression GetExpressionCore(DependencyProperty dp, PropertyMetadata metadata)
		{
			this.IsRequestingExpression = true;
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			this.EvaluateBaseValueCore(dp, metadata, ref effectiveValueEntry);
			this.IsRequestingExpression = false;
			return effectiveValueEntry.Value as Expression;
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00177228 File Offset: 0x00176228
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			DependencyProperty property = e.Property;
			VisualDiagnostics.VerifyVisualTreeChange(this);
			base.OnPropertyChanged(e);
			if (e.IsAValueChange || e.IsASubPropertyChange)
			{
				if (property != null && property.OwnerType == typeof(PresentationSource) && property.Name == "RootSource")
				{
					this.TryFireInitialized();
				}
				if (property == FrameworkElement.NameProperty && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose))
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.PerfElementIDName, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
					{
						PerfService.GetPerfElementID(this),
						base.GetType().Name,
						base.GetValue(property)
					});
				}
				if (property != FrameworkContentElement.StyleProperty && property != FrameworkContentElement.DefaultStyleKeyProperty)
				{
					if (this.TemplatedParent != null)
					{
						FrameworkTemplate templateInternal = (this.TemplatedParent as FrameworkElement).TemplateInternal;
						StyleHelper.OnTriggerSourcePropertyInvalidated(null, templateInternal, this.TemplatedParent, property, e, false, ref templateInternal.TriggerSourceRecordFromChildIndex, ref templateInternal.PropertyTriggersWithActions, this.TemplateChildIndex);
					}
					if (this.Style != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.Style, null, this, property, e, true, ref this.Style.TriggerSourceRecordFromChildIndex, ref this.Style.PropertyTriggersWithActions, 0);
					}
					if (this.ThemeStyle != null && this.Style != this.ThemeStyle)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.ThemeStyle, null, this, property, e, true, ref this.ThemeStyle.TriggerSourceRecordFromChildIndex, ref this.ThemeStyle.PropertyTriggersWithActions, 0);
					}
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits && (this.InheritanceBehavior == InheritanceBehavior.Default || frameworkPropertyMetadata.OverridesInheritanceBehavior) && (!DependencyObject.IsTreeWalkOperation(e.OperationType) || this.PotentiallyHasMentees))
			{
				EffectiveValueEntry newEntry = e.NewEntry;
				EffectiveValueEntry oldEntry = e.OldEntry;
				if (oldEntry.BaseValueSourceInternal > newEntry.BaseValueSourceInternal)
				{
					newEntry = new EffectiveValueEntry(property, BaseValueSourceInternal.Inherited);
				}
				else
				{
					newEntry = newEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
					newEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
				}
				if (oldEntry.BaseValueSourceInternal != BaseValueSourceInternal.Default || oldEntry.HasModifiers)
				{
					oldEntry = oldEntry.GetFlattenedEntry(RequestFlags.FullyResolved);
					oldEntry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
				}
				else
				{
					oldEntry = default(EffectiveValueEntry);
				}
				InheritablePropertyChangeInfo info = new InheritablePropertyChangeInfo(this, property, oldEntry, newEntry);
				if (!DependencyObject.IsTreeWalkOperation(e.OperationType))
				{
					TreeWalkHelper.InvalidateOnInheritablePropertyChange(null, this, info, true);
				}
				if (this.PotentiallyHasMentees)
				{
					TreeWalkHelper.OnInheritedPropertyChanged(this, ref info, this.InheritanceBehavior);
				}
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x0017748A File Offset: 0x0017648A
		// (set) Token: 0x060020F8 RID: 8440 RVA: 0x0017749C File Offset: 0x0017649C
		[MergableProperty(false)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string Name
		{
			get
			{
				return (string)base.GetValue(FrameworkContentElement.NameProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.NameProperty, value);
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x001774AA File Offset: 0x001764AA
		// (set) Token: 0x060020FA RID: 8442 RVA: 0x001774B7 File Offset: 0x001764B7
		public object Tag
		{
			get
			{
				return base.GetValue(FrameworkContentElement.TagProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.TagProperty, value);
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x001774C5 File Offset: 0x001764C5
		// (set) Token: 0x060020FC RID: 8444 RVA: 0x001774D7 File Offset: 0x001764D7
		public XmlLanguage Language
		{
			get
			{
				return (XmlLanguage)base.GetValue(FrameworkContentElement.LanguageProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.LanguageProperty, value);
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x001774E5 File Offset: 0x001764E5
		// (set) Token: 0x060020FE RID: 8446 RVA: 0x001774F7 File Offset: 0x001764F7
		public Style FocusVisualStyle
		{
			get
			{
				return (Style)base.GetValue(FrameworkContentElement.FocusVisualStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.FocusVisualStyleProperty, value);
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060020FF RID: 8447 RVA: 0x00177505 File Offset: 0x00176505
		// (set) Token: 0x06002100 RID: 8448 RVA: 0x00177517 File Offset: 0x00176517
		public Cursor Cursor
		{
			get
			{
				return (Cursor)base.GetValue(FrameworkContentElement.CursorProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.CursorProperty, value);
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00177525 File Offset: 0x00176525
		private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (((FrameworkContentElement)d).IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002102 RID: 8450 RVA: 0x00177539 File Offset: 0x00176539
		// (set) Token: 0x06002103 RID: 8451 RVA: 0x0017754B File Offset: 0x0017654B
		public bool ForceCursor
		{
			get
			{
				return (bool)base.GetValue(FrameworkContentElement.ForceCursorProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.ForceCursorProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x00177525 File Offset: 0x00176525
		private static void OnForceCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (((FrameworkContentElement)d).IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00177560 File Offset: 0x00176560
		private static void OnQueryCursor(object sender, QueryCursorEventArgs e)
		{
			FrameworkContentElement frameworkContentElement = (FrameworkContentElement)sender;
			Cursor cursor = frameworkContentElement.Cursor;
			if (cursor != null && (!e.Handled || frameworkContentElement.ForceCursor))
			{
				e.Cursor = cursor;
				e.Handled = true;
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x0017759C File Offset: 0x0017659C
		public sealed override bool MoveFocus(TraversalRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return KeyboardNavigation.Current.Navigate(this, request);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x001775B8 File Offset: 0x001765B8
		public sealed override DependencyObject PredictFocus(FocusNavigationDirection direction)
		{
			return KeyboardNavigation.Current.PredictFocusedElement(this, direction);
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x001775C6 File Offset: 0x001765C6
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (base.IsKeyboardFocused)
			{
				this.BringIntoView();
			}
			base.OnGotFocus(e);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x001775E0 File Offset: 0x001765E0
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				FrameworkContentElement frameworkContentElement = (FrameworkContentElement)sender;
				KeyboardNavigation.UpdateFocusedElement(frameworkContentElement);
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				KeyboardNavigation.ShowFocusVisual();
				keyboardNavigation.UpdateActiveElement(frameworkContentElement);
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x00177613 File Offset: 0x00176613
		private static void OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				KeyboardNavigation.Current.HideFocusVisual();
				if (e.NewFocus == null)
				{
					KeyboardNavigation.Current.NotifyFocusChanged(sender, e);
				}
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x0017763C File Offset: 0x0017663C
		public void BringIntoView()
		{
			base.RaiseEvent(new RequestBringIntoViewEventArgs(this, Rect.Empty)
			{
				RoutedEvent = FrameworkElement.RequestBringIntoViewEvent
			});
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600210C RID: 8460 RVA: 0x00177667 File Offset: 0x00176667
		// (set) Token: 0x0600210D RID: 8461 RVA: 0x00177679 File Offset: 0x00176679
		public InputScope InputScope
		{
			get
			{
				return (InputScope)base.GetValue(FrameworkContentElement.InputScopeProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.InputScopeProperty, value);
			}
		}

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x0600210E RID: 8462 RVA: 0x00177687 File Offset: 0x00176687
		// (remove) Token: 0x0600210F RID: 8463 RVA: 0x00177695 File Offset: 0x00176695
		public event EventHandler<DataTransferEventArgs> TargetUpdated
		{
			add
			{
				base.AddHandler(Binding.TargetUpdatedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Binding.TargetUpdatedEvent, value);
			}
		}

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002110 RID: 8464 RVA: 0x001776A3 File Offset: 0x001766A3
		// (remove) Token: 0x06002111 RID: 8465 RVA: 0x001776B1 File Offset: 0x001766B1
		public event EventHandler<DataTransferEventArgs> SourceUpdated
		{
			add
			{
				base.AddHandler(Binding.SourceUpdatedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Binding.SourceUpdatedEvent, value);
			}
		}

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06002112 RID: 8466 RVA: 0x001776BF File Offset: 0x001766BF
		// (remove) Token: 0x06002113 RID: 8467 RVA: 0x001776CD File Offset: 0x001766CD
		public event DependencyPropertyChangedEventHandler DataContextChanged
		{
			add
			{
				this.EventHandlersStoreAdd(FrameworkElement.DataContextChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.DataContextChangedKey, value);
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x06002114 RID: 8468 RVA: 0x001776DB File Offset: 0x001766DB
		// (set) Token: 0x06002115 RID: 8469 RVA: 0x001776E8 File Offset: 0x001766E8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object DataContext
		{
			get
			{
				return base.GetValue(FrameworkContentElement.DataContextProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.DataContextProperty, value);
			}
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x001776F6 File Offset: 0x001766F6
		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			((FrameworkContentElement)d).RaiseDependencyPropertyChanged(FrameworkElement.DataContextChangedKey, e);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x00177718 File Offset: 0x00176718
		public BindingExpression GetBindingExpression(DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpression(this, dp);
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x00177721 File Offset: 0x00176721
		public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
		{
			return BindingOperations.SetBinding(this, dp, binding);
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x0017772B File Offset: 0x0017672B
		public BindingExpression SetBinding(DependencyProperty dp, string path)
		{
			return (BindingExpression)this.SetBinding(dp, new Binding(path));
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x0017773F File Offset: 0x0017673F
		// (set) Token: 0x0600211B RID: 8475 RVA: 0x00177751 File Offset: 0x00176751
		[Localizability(LocalizationCategory.NeverLocalize)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BindingGroup BindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(FrameworkContentElement.BindingGroupProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.BindingGroupProperty, value);
			}
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x0017775F File Offset: 0x0017675F
		protected internal override DependencyObject GetUIParentCore()
		{
			return this._parent;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x00177768 File Offset: 0x00176768
		internal override object AdjustEventSource(RoutedEventArgs args)
		{
			object result = null;
			if (this._parent != null || this.HasLogicalChildren)
			{
				DependencyObject dependencyObject = args.Source as DependencyObject;
				if (dependencyObject == null || !this.IsLogicalDescendent(dependencyObject))
				{
					args.Source = this;
					result = this;
				}
			}
			return result;
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void AdjustBranchSource(RoutedEventArgs args)
		{
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
		{
			return false;
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x001777AC File Offset: 0x001767AC
		internal sealed override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			bool result = false;
			DependencyObject parent = ContentOperations.GetParent(this);
			DependencyObject parent2 = this._parent;
			DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				args.Source = route.PeekBranchSource();
				this.AdjustBranchSource(args);
				route.AddSource(args.Source);
				route.PopBranchNode();
				FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(dependencyObject));
			}
			if (!this.IgnoreModelParentBuildRoute(args))
			{
				if (parent == null)
				{
					result = (parent2 != null);
				}
				else if (parent2 != null)
				{
					route.PushBranchNode(this, args.Source);
					args.Source = parent;
				}
			}
			return result;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00177840 File Offset: 0x00176840
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			FrameworkElement.AddStyleHandlersToEventRoute(null, this, route, args);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x0017784B File Offset: 0x0017684B
		private bool IsLogicalDescendent(DependencyObject child)
		{
			while (child != null)
			{
				if (child == this)
				{
					return true;
				}
				child = LogicalTreeHelper.GetParent(child);
			}
			return false;
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00177864 File Offset: 0x00176864
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool result = true;
			continuePastCoreTree = false;
			bool parent = ContentOperations.GetParent(this) != null;
			DependencyObject parent2 = this._parent;
			DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				branchNodeStack.Pop();
				result = FrameworkElement.InvalidateAutomationIntermediateElements(this, LogicalTreeHelper.GetParent(dependencyObject));
			}
			if (!parent)
			{
				continuePastCoreTree = (parent2 != null);
			}
			else if (parent2 != null)
			{
				branchNodeStack.Push(this);
			}
			return result;
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnAncestorChanged()
		{
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x001778CB File Offset: 0x001768CB
		internal override void OnContentParentChanged(DependencyObject oldParent)
		{
			ContentOperations.GetParent(this);
			this.TryFireInitialized();
			base.OnContentParentChanged(oldParent);
		}

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06002126 RID: 8486 RVA: 0x001778E1 File Offset: 0x001768E1
		// (set) Token: 0x06002127 RID: 8487 RVA: 0x001778F0 File Offset: 0x001768F0
		internal InheritanceBehavior InheritanceBehavior
		{
			get
			{
				return (InheritanceBehavior)((this._flags & (InternalFlags)56U) >> 3);
			}
			set
			{
				if (this.IsInitialized)
				{
					throw new InvalidOperationException(SR.Get("Illegal_InheritanceBehaviorSettor"));
				}
				if (value < InheritanceBehavior.Default || value > InheritanceBehavior.SkipAllNext)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(InheritanceBehavior));
				}
				uint num = (uint)((uint)value << 3);
				this._flags = (InternalFlags)((num & 56U) | (uint)(this._flags & (InternalFlags)4294967239U));
				if (this._parent != null)
				{
					TreeWalkHelper.InvalidateOnTreeChange(null, this, this._parent, true);
					return;
				}
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x00177962 File Offset: 0x00176962
		public virtual void BeginInit()
		{
			if (this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, true);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0017798D File Offset: 0x0017698D
		public virtual void EndInit()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, false);
			this.TryFireInitialized();
		}

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x0600212A RID: 8490 RVA: 0x001779BE File Offset: 0x001769BE
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsInitialized
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsInitialized);
			}
		}

		// Token: 0x1400004D RID: 77
		// (add) Token: 0x0600212B RID: 8491 RVA: 0x001779CB File Offset: 0x001769CB
		// (remove) Token: 0x0600212C RID: 8492 RVA: 0x001779D9 File Offset: 0x001769D9
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public event EventHandler Initialized
		{
			add
			{
				this.EventHandlersStoreAdd(FrameworkElement.InitializedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.InitializedKey, value);
			}
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x001779E7 File Offset: 0x001769E7
		protected virtual void OnInitialized(EventArgs e)
		{
			if (!this.HasStyleEverBeenFetched)
			{
				this.UpdateStyleProperty();
			}
			if (!this.HasThemeStyleEverBeenFetched)
			{
				this.UpdateThemeStyleProperty();
			}
			this.RaiseInitialized(FrameworkElement.InitializedKey, e);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x00177A11 File Offset: 0x00176A11
		private void TryFireInitialized()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending) && !this.ReadInternalFlag(InternalFlags.IsInitialized))
			{
				this.WriteInternalFlag(InternalFlags.IsInitialized, true);
				this.OnInitialized(EventArgs.Empty);
			}
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x00177A44 File Offset: 0x00176A44
		private void RaiseInitialized(EventPrivateKey key, EventArgs e)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((EventHandler)@delegate)(this, e);
				}
			}
		}

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x00177A74 File Offset: 0x00176A74
		public bool IsLoaded
		{
			get
			{
				bool loadedPending = this.LoadedPending != null;
				object[] unloadedPending = this.UnloadedPending;
				if (loadedPending || unloadedPending != null)
				{
					return unloadedPending != null;
				}
				if (this.SubtreeHasLoadedChangeHandler)
				{
					return this.IsLoadedCache;
				}
				return BroadcastEventHelper.IsParentLoaded(this);
			}
		}

		// Token: 0x1400004E RID: 78
		// (add) Token: 0x06002131 RID: 8497 RVA: 0x00177AAD File Offset: 0x00176AAD
		// (remove) Token: 0x06002132 RID: 8498 RVA: 0x00177ABC File Offset: 0x00176ABC
		public event RoutedEventHandler Loaded
		{
			add
			{
				base.AddHandler(FrameworkElement.LoadedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.LoadedEvent, value);
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00177ACA File Offset: 0x00176ACA
		internal void OnLoaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06002134 RID: 8500 RVA: 0x00177AD3 File Offset: 0x00176AD3
		// (remove) Token: 0x06002135 RID: 8501 RVA: 0x00177AE2 File Offset: 0x00176AE2
		public event RoutedEventHandler Unloaded
		{
			add
			{
				base.AddHandler(FrameworkElement.UnloadedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.UnloadedEvent, value);
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00177ACA File Offset: 0x00176ACA
		internal void OnUnloaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00177AF0 File Offset: 0x00176AF0
		internal override void OnAddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent == FrameworkContentElement.LoadedEvent || routedEvent == FrameworkContentElement.UnloadedEvent)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00177B08 File Offset: 0x00176B08
		internal override void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			if (routedEvent != FrameworkContentElement.LoadedEvent && routedEvent != FrameworkContentElement.UnloadedEvent)
			{
				return;
			}
			if (!this.ThisHasLoadedChangeEventHandler)
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00177A44 File Offset: 0x00176A44
		internal void RaiseClrEvent(EventPrivateKey key, EventArgs args)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((EventHandler)@delegate)(this, args);
				}
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x00177B29 File Offset: 0x00176B29
		// (set) Token: 0x0600213B RID: 8507 RVA: 0x00177B31 File Offset: 0x00176B31
		[Bindable(true)]
		[Category("Appearance")]
		public object ToolTip
		{
			get
			{
				return ToolTipService.GetToolTip(this);
			}
			set
			{
				ToolTipService.SetToolTip(this, value);
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x0600213C RID: 8508 RVA: 0x00177B3A File Offset: 0x00176B3A
		// (set) Token: 0x0600213D RID: 8509 RVA: 0x00177B4C File Offset: 0x00176B4C
		public ContextMenu ContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(FrameworkContentElement.ContextMenuProperty);
			}
			set
			{
				base.SetValue(FrameworkContentElement.ContextMenuProperty, value);
			}
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x0600213E RID: 8510 RVA: 0x00177B5A File Offset: 0x00176B5A
		// (remove) Token: 0x0600213F RID: 8511 RVA: 0x00177B68 File Offset: 0x00176B68
		public event ToolTipEventHandler ToolTipOpening
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ToolTipOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ToolTipOpeningEvent, value);
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x00177B76 File Offset: 0x00176B76
		private static void OnToolTipOpeningThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkContentElement)sender).OnToolTipOpening(e);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnToolTipOpening(ToolTipEventArgs e)
		{
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x06002142 RID: 8514 RVA: 0x00177B84 File Offset: 0x00176B84
		// (remove) Token: 0x06002143 RID: 8515 RVA: 0x00177B92 File Offset: 0x00176B92
		public event ToolTipEventHandler ToolTipClosing
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ToolTipClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ToolTipClosingEvent, value);
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00177BA0 File Offset: 0x00176BA0
		private static void OnToolTipClosingThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkContentElement)sender).OnToolTipClosing(e);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnToolTipClosing(ToolTipEventArgs e)
		{
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06002146 RID: 8518 RVA: 0x00177BAE File Offset: 0x00176BAE
		// (remove) Token: 0x06002147 RID: 8519 RVA: 0x00177BBC File Offset: 0x00176BBC
		public event ContextMenuEventHandler ContextMenuOpening
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ContextMenuOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ContextMenuOpeningEvent, value);
			}
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00177BCA File Offset: 0x00176BCA
		private static void OnContextMenuOpeningThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkContentElement)sender).OnContextMenuOpening(e);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnContextMenuOpening(ContextMenuEventArgs e)
		{
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x0600214A RID: 8522 RVA: 0x00177BD8 File Offset: 0x00176BD8
		// (remove) Token: 0x0600214B RID: 8523 RVA: 0x00177BE6 File Offset: 0x00176BE6
		public event ContextMenuEventHandler ContextMenuClosing
		{
			add
			{
				base.AddHandler(FrameworkContentElement.ContextMenuClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkContentElement.ContextMenuClosingEvent, value);
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00177BF4 File Offset: 0x00176BF4
		private static void OnContextMenuClosingThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkContentElement)sender).OnContextMenuClosing(e);
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnContextMenuClosing(ContextMenuEventArgs e)
		{
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00177C04 File Offset: 0x00176C04
		internal override void InvalidateForceInheritPropertyOnChildren(DependencyProperty property)
		{
			IEnumerator logicalChildren = this.LogicalChildren;
			if (logicalChildren != null)
			{
				while (logicalChildren.MoveNext())
				{
					object obj = logicalChildren.Current;
					DependencyObject dependencyObject = obj as DependencyObject;
					if (dependencyObject != null)
					{
						dependencyObject.CoerceValue(property);
					}
				}
			}
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00177C3C File Offset: 0x00176C3C
		private void RaiseDependencyPropertyChanged(EventPrivateKey key, DependencyPropertyChangedEventArgs args)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(key);
				if (@delegate != null)
				{
					((DependencyPropertyChangedEventHandler)@delegate)(this, args);
				}
			}
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00177C6B File Offset: 0x00176C6B
		private void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
		{
			base.EnsureEventHandlersStore();
			base.EventHandlersStore.Add(key, handler);
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00177C80 File Offset: 0x00176C80
		private void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				eventHandlersStore.Remove(key, handler);
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x00177C9F File Offset: 0x00176C9F
		// (set) Token: 0x06002153 RID: 8531 RVA: 0x00177CA8 File Offset: 0x00176CA8
		internal bool HasResourceReference
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasResourceReferences);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasResourceReferences, value);
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x00177CB2 File Offset: 0x00176CB2
		// (set) Token: 0x06002155 RID: 8533 RVA: 0x00177CBF File Offset: 0x00176CBF
		internal bool IsLogicalChildrenIterationInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsLogicalChildrenIterationInProgress, value);
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002156 RID: 8534 RVA: 0x00177CCD File Offset: 0x00176CCD
		// (set) Token: 0x06002157 RID: 8535 RVA: 0x00177CDA File Offset: 0x00176CDA
		internal bool SubtreeHasLoadedChangeHandler
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.TreeHasLoadedChangeHandler);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.TreeHasLoadedChangeHandler, value);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06002158 RID: 8536 RVA: 0x00177CE8 File Offset: 0x00176CE8
		// (set) Token: 0x06002159 RID: 8537 RVA: 0x00177CF5 File Offset: 0x00176CF5
		internal bool IsLoadedCache
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsLoadedCache);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsLoadedCache, value);
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600215A RID: 8538 RVA: 0x00177D03 File Offset: 0x00176D03
		// (set) Token: 0x0600215B RID: 8539 RVA: 0x00177D10 File Offset: 0x00176D10
		internal bool IsParentAnFE
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsParentAnFE);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsParentAnFE, value);
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600215C RID: 8540 RVA: 0x00177D1E File Offset: 0x00176D1E
		// (set) Token: 0x0600215D RID: 8541 RVA: 0x00177D2B File Offset: 0x00176D2B
		internal bool IsTemplatedParentAnFE
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsTemplatedParentAnFE);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsTemplatedParentAnFE, value);
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600215E RID: 8542 RVA: 0x00177D39 File Offset: 0x00176D39
		// (set) Token: 0x0600215F RID: 8543 RVA: 0x00177D46 File Offset: 0x00176D46
		internal bool HasLogicalChildren
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasLogicalChildren);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasLogicalChildren, value);
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06002160 RID: 8544 RVA: 0x00177D54 File Offset: 0x00176D54
		// (set) Token: 0x06002161 RID: 8545 RVA: 0x00177D7C File Offset: 0x00176D7C
		internal int TemplateChildIndex
		{
			get
			{
				uint num = (uint)(this._flags2 & InternalFlags2.Default);
				if (num == 65535U)
				{
					return -1;
				}
				return (int)num;
			}
			set
			{
				if (value < -1 || value >= 65535)
				{
					throw new ArgumentOutOfRangeException("value", SR.Get("TemplateChildIndexOutOfRange"));
				}
				uint num = (uint)((value == -1) ? 65535 : value);
				this._flags2 = (InternalFlags2)(num | (uint)(this._flags2 & ~(InternalFlags2.R0 | InternalFlags2.R1 | InternalFlags2.R2 | InternalFlags2.R3 | InternalFlags2.R4 | InternalFlags2.R5 | InternalFlags2.R6 | InternalFlags2.R7 | InternalFlags2.R8 | InternalFlags2.R9 | InternalFlags2.RA | InternalFlags2.RB | InternalFlags2.RC | InternalFlags2.RD | InternalFlags2.RE | InternalFlags2.RF)));
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x00177DCB File Offset: 0x00176DCB
		// (set) Token: 0x06002163 RID: 8547 RVA: 0x00177DD8 File Offset: 0x00176DD8
		internal bool IsRequestingExpression
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsRequestingExpression);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsRequestingExpression, value);
			}
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x00177DE6 File Offset: 0x00176DE6
		internal bool ReadInternalFlag(InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (InternalFlags)0U;
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x00177DF3 File Offset: 0x00176DF3
		internal bool ReadInternalFlag2(InternalFlags2 reqFlag)
		{
			return (this._flags2 & reqFlag) > (InternalFlags2)0U;
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x00177E00 File Offset: 0x00176E00
		internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x00177E23 File Offset: 0x00176E23
		internal void WriteInternalFlag2(InternalFlags2 reqFlag, bool set)
		{
			if (set)
			{
				this._flags2 |= reqFlag;
				return;
			}
			this._flags2 &= ~reqFlag;
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x00177E46 File Offset: 0x00176E46
		public new DependencyObject Parent
		{
			get
			{
				return this.ContextVerifiedGetParent();
			}
		}

		// Token: 0x06002169 RID: 8553 RVA: 0x00177E50 File Offset: 0x00176E50
		public void RegisterName(string name, object scopedElement)
		{
			INameScope nameScope = FrameworkElement.FindScope(this);
			if (nameScope != null)
			{
				nameScope.RegisterName(name, scopedElement);
				return;
			}
			throw new InvalidOperationException(SR.Get("NameScopeNotFound", new object[]
			{
				name,
				"register"
			}));
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x00177E94 File Offset: 0x00176E94
		public void UnregisterName(string name)
		{
			INameScope nameScope = FrameworkElement.FindScope(this);
			if (nameScope != null)
			{
				nameScope.UnregisterName(name);
				return;
			}
			throw new InvalidOperationException(SR.Get("NameScopeNotFound", new object[]
			{
				name,
				"unregister"
			}));
		}

		// Token: 0x0600216B RID: 8555 RVA: 0x00177ED4 File Offset: 0x00176ED4
		public object FindName(string name)
		{
			DependencyObject dependencyObject;
			return this.FindName(name, out dependencyObject);
		}

		// Token: 0x0600216C RID: 8556 RVA: 0x00177EEC File Offset: 0x00176EEC
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			INameScope nameScope = FrameworkElement.FindScope(this, out scopeOwner);
			if (nameScope != null)
			{
				return nameScope.FindName(name);
			}
			return null;
		}

		// Token: 0x0600216D RID: 8557 RVA: 0x00177F0D File Offset: 0x00176F0D
		public void UpdateDefaultStyle()
		{
			TreeWalkHelper.InvalidateOnResourcesChange(null, this, ResourcesChangeInfo.ThemeChangeInfo);
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x00109403 File Offset: 0x00108403
		protected internal virtual IEnumerator LogicalChildren
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x00177F1C File Offset: 0x00176F1C
		internal object FindResourceOnSelf(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary value = FrameworkContentElement.ResourcesField.GetValue(this);
			if (value != null && value.Contains(resourceKey))
			{
				bool flag;
				return value.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x0017775F File Offset: 0x0017675F
		internal DependencyObject ContextVerifiedGetParent()
		{
			return this._parent;
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x00177F54 File Offset: 0x00176F54
		protected internal void AddLogicalChild(object child)
		{
			if (child != null)
			{
				if (this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("CannotModifyLogicalChildrenDuringTreeWalk"));
				}
				this.TryFireInitialized();
				try
				{
					this.HasLogicalChildren = true;
					FrameworkObject frameworkObject = new FrameworkObject(child as DependencyObject);
					frameworkObject.ChangeLogicalParent(this);
				}
				finally
				{
				}
			}
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x00177FB8 File Offset: 0x00176FB8
		protected internal void RemoveLogicalChild(object child)
		{
			if (child != null)
			{
				if (this.IsLogicalChildrenIterationInProgress)
				{
					throw new InvalidOperationException(SR.Get("CannotModifyLogicalChildrenDuringTreeWalk"));
				}
				FrameworkObject frameworkObject = new FrameworkObject(child as DependencyObject);
				if (frameworkObject.Parent == this)
				{
					frameworkObject.ChangeLogicalParent(null);
				}
				IEnumerator logicalChildren = this.LogicalChildren;
				if (logicalChildren == null)
				{
					this.HasLogicalChildren = false;
					return;
				}
				this.HasLogicalChildren = logicalChildren.MoveNext();
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x00178020 File Offset: 0x00177020
		internal void ChangeLogicalParent(DependencyObject newParent)
		{
			base.VerifyAccess();
			if (newParent != null)
			{
				newParent.VerifyAccess();
			}
			if (this._parent != null && newParent != null && this._parent != newParent)
			{
				throw new InvalidOperationException(SR.Get("HasLogicalParent"));
			}
			if (newParent == this)
			{
				throw new InvalidOperationException(SR.Get("CannotBeSelfParent"));
			}
			VisualDiagnostics.VerifyVisualTreeChange(this);
			if (newParent != null)
			{
				this.ClearInheritanceContext();
			}
			this.IsParentAnFE = (newParent is FrameworkElement);
			DependencyObject parent = this._parent;
			this.OnNewParent(newParent);
			BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, parent, newParent);
			BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, parent, newParent);
			DependencyObject parent2 = (newParent != null) ? newParent : parent;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, parent2, newParent != null);
			this.TryFireInitialized();
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x001780CC File Offset: 0x001770CC
		internal virtual void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = this._parent;
			this._parent = newParent;
			if (this._parent != null)
			{
				UIElement.SynchronizeForceInheritProperties(null, this, null, this._parent);
			}
			else
			{
				UIElement.SynchronizeForceInheritProperties(null, this, null, parent);
			}
			base.SynchronizeReverseInheritPropertyFlags(parent, false);
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00178110 File Offset: 0x00177110
		internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
		{
			bool isSelfInheritanceParent = base.IsSelfInheritanceParent;
			if (parentTreeState.Root != this)
			{
				this.HasStyleChanged = false;
				this.HasStyleInvalidated = false;
			}
			if (parentTreeState.IsAddOperation)
			{
				FrameworkObject frameworkObject = new FrameworkObject(null, this);
				frameworkObject.SetShouldLookupImplicitStyles();
			}
			if (this.HasResourceReference)
			{
				TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
			}
			FrugalObjectList<DependencyProperty> item = this.InvalidateTreeDependentProperties(parentTreeState, base.IsSelfInheritanceParent, isSelfInheritanceParent);
			parentTreeState.InheritablePropertiesStack.Push(item);
			PresentationSource.OnAncestorChanged(this);
			this.OnAncestorChanged();
			if (this.PotentiallyHasMentees)
			{
				this.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x001781AC File Offset: 0x001771AC
		internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			this.AncestorChangeInProgress = true;
			FrugalObjectList<DependencyProperty> result;
			try
			{
				if (!this.HasLocalStyle && this != parentTreeState.Root)
				{
					this.UpdateStyleProperty();
				}
				ChildRecord childRecord = default(ChildRecord);
				bool isChildRecordValid = false;
				Style style = this.Style;
				Style themeStyle = this.ThemeStyle;
				DependencyObject templatedParent = this.TemplatedParent;
				int templateChildIndex = this.TemplateChildIndex;
				bool hasStyleChanged = this.HasStyleChanged;
				FrameworkElement.GetTemplatedParentChildRecord(templatedParent, templateChildIndex, out childRecord, out isChildRecordValid);
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				DependencyObject parent = null;
				InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
				if (frameworkParent)
				{
					if (frameworkElement != null)
					{
						parent = frameworkElement;
						inheritanceBehavior = frameworkElement.InheritanceBehavior;
					}
					else
					{
						parent = frameworkContentElement;
						inheritanceBehavior = frameworkContentElement.InheritanceBehavior;
					}
				}
				if (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) && !TreeWalkHelper.SkipNow(inheritanceBehavior))
				{
					base.SynchronizeInheritanceParent(parent);
				}
				else if (!base.IsSelfInheritanceParent)
				{
					base.SetIsSelfInheritanceParent();
				}
				result = TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, null, this, style, themeStyle, ref childRecord, isChildRecordValid, hasStyleChanged, isSelfInheritanceParent, wasSelfInheritanceParent);
			}
			finally
			{
				this.AncestorChangeInProgress = false;
			}
			return result;
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x001782A8 File Offset: 0x001772A8
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				return (base.EventHandlersStore != null && (base.EventHandlersStore.Contains(FrameworkContentElement.LoadedEvent) || base.EventHandlersStore.Contains(FrameworkContentElement.UnloadedEvent))) || (this.Style != null && this.Style.HasLoadedChangeHandler) || (this.ThemeStyle != null && this.ThemeStyle.HasLoadedChangeHandler) || this.HasFefLoadedChangeHandler;
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x0017831C File Offset: 0x0017731C
		internal bool HasFefLoadedChangeHandler
		{
			get
			{
				if (this.TemplatedParent == null)
				{
					return false;
				}
				FrameworkElementFactory feftreeRoot = BroadcastEventHelper.GetFEFTreeRoot(this.TemplatedParent);
				if (feftreeRoot == null)
				{
					return false;
				}
				FrameworkElementFactory frameworkElementFactory = StyleHelper.FindFEF(feftreeRoot, this.TemplateChildIndex);
				return frameworkElementFactory != null && frameworkElementFactory.HasLoadedChangeHandler;
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0017835C File Offset: 0x0017735C
		internal void UpdateStyleProperty()
		{
			if (!this.HasStyleInvalidated)
			{
				if (!this.IsStyleUpdateInProgress)
				{
					this.IsStyleUpdateInProgress = true;
					try
					{
						base.InvalidateProperty(FrameworkContentElement.StyleProperty);
						this.HasStyleInvalidated = true;
						return;
					}
					finally
					{
						this.IsStyleUpdateInProgress = false;
					}
				}
				throw new InvalidOperationException(SR.Get("CyclicStyleReferenceDetected", new object[]
				{
					this
				}));
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x001783C8 File Offset: 0x001773C8
		internal void UpdateThemeStyleProperty()
		{
			if (!this.IsThemeStyleUpdateInProgress)
			{
				this.IsThemeStyleUpdateInProgress = true;
				try
				{
					StyleHelper.GetThemeStyle(null, this);
					ContextMenu contextMenu = base.GetValueEntry(base.LookupEntry(FrameworkContentElement.ContextMenuProperty.GlobalIndex), FrameworkContentElement.ContextMenuProperty, null, RequestFlags.DeferredReferences).Value as ContextMenu;
					if (contextMenu != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(contextMenu, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					DependencyObject dependencyObject = base.GetValueEntry(base.LookupEntry(FrameworkContentElement.ToolTipProperty.GlobalIndex), FrameworkContentElement.ToolTipProperty, null, RequestFlags.DeferredReferences).Value as DependencyObject;
					if (dependencyObject != null)
					{
						FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
						if (frameworkObject.IsValid)
						{
							TreeWalkHelper.InvalidateOnResourcesChange(frameworkObject.FE, frameworkObject.FCE, ResourcesChangeInfo.ThemeChangeInfo);
						}
					}
					this.OnThemeChanged();
					return;
				}
				finally
				{
					this.IsThemeStyleUpdateInProgress = false;
				}
			}
			throw new InvalidOperationException(SR.Get("CyclicThemeStyleReferenceDetected", new object[]
			{
				this
			}));
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x001784BC File Offset: 0x001774BC
		internal void FireLoadedOnDescendentsInternal()
		{
			if (this.LoadedPending == null)
			{
				DependencyObject parent = this.Parent;
				object[] unloadedPending = this.UnloadedPending;
				if (unloadedPending == null || unloadedPending[2] != parent)
				{
					BroadcastEventHelper.AddLoadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveUnloadedCallback(this, unloadedPending);
			}
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x001784F8 File Offset: 0x001774F8
		internal void FireUnloadedOnDescendentsInternal()
		{
			if (this.UnloadedPending == null)
			{
				DependencyObject parent = this.Parent;
				object[] loadedPending = this.LoadedPending;
				if (loadedPending == null)
				{
					BroadcastEventHelper.AddUnloadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveLoadedCallback(this, loadedPending);
			}
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00178530 File Offset: 0x00177530
		internal override bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
		{
			FrameworkObject frameworkObject = new FrameworkObject(target);
			return !frameworkObject.IsValid;
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x0017854F File Offset: 0x0017754F
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return FrameworkContentElement.InheritanceContextField.GetValue(this);
			}
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x0017855C File Offset: 0x0017755C
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			base.AddInheritanceContext(context, property);
			this.TryFireInitialized();
			if ((property == VisualBrush.VisualProperty || property == BitmapCacheBrush.TargetProperty) && FrameworkElement.GetFrameworkParent(this) == null && !FrameworkObject.IsEffectiveAncestor(this, context))
			{
				if (!this.HasMultipleInheritanceContexts && this.InheritanceContext == null)
				{
					FrameworkContentElement.InheritanceContextField.SetValue(this, context);
					base.OnInheritanceContextChanged(EventArgs.Empty);
					return;
				}
				if (this.InheritanceContext != null)
				{
					FrameworkContentElement.InheritanceContextField.ClearValue(this);
					this.WriteInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts, true);
					base.OnInheritanceContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x001785EA File Offset: 0x001775EA
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this.InheritanceContext == context)
			{
				FrameworkContentElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
			base.RemoveInheritanceContext(context, property);
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00178613 File Offset: 0x00177613
		private void ClearInheritanceContext()
		{
			if (this.InheritanceContext != null)
			{
				FrameworkContentElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00178634 File Offset: 0x00177634
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			DependencyObject value = FrameworkContentElement.MentorField.GetValue(this);
			DependencyObject dependencyObject = Helper.FindMentor(this.InheritanceContext);
			if (value != dependencyObject)
			{
				FrameworkContentElement.MentorField.SetValue(this, dependencyObject);
				if (value != null)
				{
					this.DisconnectMentor(value);
				}
				if (dependencyObject != null)
				{
					this.ConnectMentor(dependencyObject);
				}
			}
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00178680 File Offset: 0x00177680
		private void ConnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged += this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged += this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, frameworkObject.DO, true);
			if (this.SubtreeHasLoadedChangeHandler)
			{
				bool isLoaded = frameworkObject.IsLoaded;
				this.ConnectLoadedEvents(ref frameworkObject, isLoaded);
				if (isLoaded)
				{
					this.FireLoadedOnDescendentsInternal();
				}
			}
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x001786EC File Offset: 0x001776EC
		private void DisconnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged -= this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged -= this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(null, this, frameworkObject.DO, false);
			if (this.SubtreeHasLoadedChangeHandler)
			{
				bool isLoaded = frameworkObject.IsLoaded;
				this.DisconnectLoadedEvents(ref frameworkObject, isLoaded);
				if (frameworkObject.IsLoaded)
				{
					this.FireUnloadedOnDescendentsInternal();
				}
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00178760 File Offset: 0x00177760
		internal void ChangeSubtreeHasLoadedChangedHandler(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			bool isLoaded = frameworkObject.IsLoaded;
			if (this.SubtreeHasLoadedChangeHandler)
			{
				this.ConnectLoadedEvents(ref frameworkObject, isLoaded);
				return;
			}
			this.DisconnectLoadedEvents(ref frameworkObject, isLoaded);
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00178798 File Offset: 0x00177798
		private void OnMentorLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Loaded -= this.OnMentorLoaded;
			frameworkObject.Unloaded += this.OnMentorUnloaded;
			BroadcastEventHelper.BroadcastLoadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x001787E4 File Offset: 0x001777E4
		private void OnMentorUnloaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Unloaded -= this.OnMentorUnloaded;
			frameworkObject.Loaded += this.OnMentorLoaded;
			BroadcastEventHelper.BroadcastUnloadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x00178830 File Offset: 0x00177830
		private void ConnectLoadedEvents(ref FrameworkObject foMentor, bool isLoaded)
		{
			if (foMentor.IsValid)
			{
				if (isLoaded)
				{
					foMentor.Unloaded += this.OnMentorUnloaded;
					return;
				}
				foMentor.Loaded += this.OnMentorLoaded;
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00178862 File Offset: 0x00177862
		private void DisconnectLoadedEvents(ref FrameworkObject foMentor, bool isLoaded)
		{
			if (foMentor.IsValid)
			{
				if (isLoaded)
				{
					foMentor.Unloaded -= this.OnMentorUnloaded;
					return;
				}
				foMentor.Loaded -= this.OnMentorLoaded;
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x00178894 File Offset: 0x00177894
		private void OnMentorInheritedPropertyChanged(object sender, InheritedPropertyChangedEventArgs e)
		{
			TreeWalkHelper.InvalidateOnInheritablePropertyChange(null, this, e.Info, false);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x001788A4 File Offset: 0x001778A4
		private void OnMentorResourcesChanged(object sender, EventArgs e)
		{
			TreeWalkHelper.InvalidateOnResourcesChange(null, this, ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x001788B4 File Offset: 0x001778B4
		internal void RaiseInheritedPropertyChangedEvent(ref InheritablePropertyChangeInfo info)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				Delegate @delegate = eventHandlersStore.Get(FrameworkElement.InheritedPropertyChangedKey);
				if (@delegate != null)
				{
					InheritedPropertyChangedEventArgs e = new InheritedPropertyChangedEventArgs(ref info);
					((InheritedPropertyChangedEventHandler)@delegate)(this, e);
				}
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x0600218E RID: 8590 RVA: 0x001788EE File Offset: 0x001778EE
		// (set) Token: 0x0600218F RID: 8591 RVA: 0x001788F8 File Offset: 0x001778F8
		internal bool IsStyleUpdateInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsStyleUpdateInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsStyleUpdateInProgress, value);
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x00178903 File Offset: 0x00177903
		// (set) Token: 0x06002191 RID: 8593 RVA: 0x00178910 File Offset: 0x00177910
		internal bool IsThemeStyleUpdateInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsThemeStyleUpdateInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsThemeStyleUpdateInProgress, value);
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0017891E File Offset: 0x0017791E
		// (set) Token: 0x06002193 RID: 8595 RVA: 0x0017892B File Offset: 0x0017792B
		internal bool StoresParentTemplateValues
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.StoresParentTemplateValues);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.StoresParentTemplateValues, value);
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x00178939 File Offset: 0x00177939
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x00178942 File Offset: 0x00177942
		internal bool HasNumberSubstitutionChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasNumberSubstitutionChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasNumberSubstitutionChanged, value);
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x0017894C File Offset: 0x0017794C
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x00178959 File Offset: 0x00177959
		internal bool HasTemplateGeneratedSubTree
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasTemplateGeneratedSubTree);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasTemplateGeneratedSubTree, value);
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06002198 RID: 8600 RVA: 0x00178967 File Offset: 0x00177967
		// (set) Token: 0x06002199 RID: 8601 RVA: 0x00178970 File Offset: 0x00177970
		internal bool HasImplicitStyleFromResources
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasImplicitStyleFromResources);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasImplicitStyleFromResources, value);
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600219A RID: 8602 RVA: 0x0017897A File Offset: 0x0017797A
		// (set) Token: 0x0600219B RID: 8603 RVA: 0x00178987 File Offset: 0x00177987
		internal bool ShouldLookupImplicitStyles
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.ShouldLookupImplicitStyles);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.ShouldLookupImplicitStyles, value);
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600219C RID: 8604 RVA: 0x00178995 File Offset: 0x00177995
		// (set) Token: 0x0600219D RID: 8605 RVA: 0x001789A2 File Offset: 0x001779A2
		internal bool IsStyleSetFromGenerator
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.IsStyleSetFromGenerator);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.IsStyleSetFromGenerator, value);
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x001789B0 File Offset: 0x001779B0
		// (set) Token: 0x0600219F RID: 8607 RVA: 0x001789BD File Offset: 0x001779BD
		internal bool HasStyleChanged
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasStyleChanged);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasStyleChanged, value);
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060021A0 RID: 8608 RVA: 0x001789CB File Offset: 0x001779CB
		// (set) Token: 0x060021A1 RID: 8609 RVA: 0x001789D8 File Offset: 0x001779D8
		internal bool HasStyleInvalidated
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasStyleInvalidated);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasStyleInvalidated, value);
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x060021A2 RID: 8610 RVA: 0x001789E6 File Offset: 0x001779E6
		// (set) Token: 0x060021A3 RID: 8611 RVA: 0x001789F3 File Offset: 0x001779F3
		internal bool HasStyleEverBeenFetched
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasStyleEverBeenFetched);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasStyleEverBeenFetched, value);
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x060021A4 RID: 8612 RVA: 0x00178A01 File Offset: 0x00177A01
		// (set) Token: 0x060021A5 RID: 8613 RVA: 0x00178A0E File Offset: 0x00177A0E
		internal bool HasLocalStyle
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasLocalStyle);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasLocalStyle, value);
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x060021A6 RID: 8614 RVA: 0x00178A1C File Offset: 0x00177A1C
		// (set) Token: 0x060021A7 RID: 8615 RVA: 0x00178A29 File Offset: 0x00177A29
		internal bool HasThemeStyleEverBeenFetched
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasThemeStyleEverBeenFetched);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasThemeStyleEverBeenFetched, value);
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x00178A37 File Offset: 0x00177A37
		// (set) Token: 0x060021A9 RID: 8617 RVA: 0x00178A44 File Offset: 0x00177A44
		internal bool AncestorChangeInProgress
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.AncestorChangeInProgress);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.AncestorChangeInProgress, value);
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060021AA RID: 8618 RVA: 0x00178A52 File Offset: 0x00177A52
		// (set) Token: 0x060021AB RID: 8619 RVA: 0x00178A5A File Offset: 0x00177A5A
		internal FrugalObjectList<DependencyProperty> InheritableProperties
		{
			get
			{
				return this._inheritableProperties;
			}
			set
			{
				this._inheritableProperties = value;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x00178A63 File Offset: 0x00177A63
		internal object[] LoadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkContentElement.LoadedPendingProperty);
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x00178A75 File Offset: 0x00177A75
		internal object[] UnloadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkContentElement.UnloadedPendingProperty);
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060021AE RID: 8622 RVA: 0x00178A87 File Offset: 0x00177A87
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts);
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x00178A94 File Offset: 0x00177A94
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x00178AA1 File Offset: 0x00177AA1
		internal bool PotentiallyHasMentees
		{
			get
			{
				return this.ReadInternalFlag((InternalFlags)2147483648U);
			}
			set
			{
				this.WriteInternalFlag((InternalFlags)2147483648U, value);
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060021B1 RID: 8625 RVA: 0x00178AAF File Offset: 0x00177AAF
		// (remove) Token: 0x060021B2 RID: 8626 RVA: 0x00178AC4 File Offset: 0x00177AC4
		internal event EventHandler ResourcesChanged
		{
			add
			{
				this.PotentiallyHasMentees = true;
				this.EventHandlersStoreAdd(FrameworkElement.ResourcesChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.ResourcesChangedKey, value);
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060021B3 RID: 8627 RVA: 0x00178AD2 File Offset: 0x00177AD2
		// (remove) Token: 0x060021B4 RID: 8628 RVA: 0x00178AE7 File Offset: 0x00177AE7
		internal event InheritedPropertyChangedEventHandler InheritedPropertyChanged
		{
			add
			{
				this.PotentiallyHasMentees = true;
				this.EventHandlersStoreAdd(FrameworkElement.InheritedPropertyChangedKey, value);
			}
			remove
			{
				this.EventHandlersStoreRemove(FrameworkElement.InheritedPropertyChangedKey, value);
			}
		}

		// Token: 0x04001002 RID: 4098
		internal static readonly NumberSubstitution DefaultNumberSubstitution = new NumberSubstitution(NumberCultureSource.Text, null, NumberSubstitutionMethod.AsCulture);

		// Token: 0x04001003 RID: 4099
		[CommonDependencyProperty]
		public static readonly DependencyProperty StyleProperty = FrameworkElement.StyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnStyleChanged)));

		// Token: 0x04001004 RID: 4100
		public static readonly DependencyProperty OverridesDefaultStyleProperty = FrameworkElement.OverridesDefaultStyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnThemeStyleKeyChanged)));

		// Token: 0x04001005 RID: 4101
		protected internal static readonly DependencyProperty DefaultStyleKeyProperty = FrameworkElement.DefaultStyleKeyProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkContentElement.OnThemeStyleKeyChanged)));

		// Token: 0x04001006 RID: 4102
		public static readonly DependencyProperty NameProperty = FrameworkElement.NameProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(string.Empty));

		// Token: 0x04001007 RID: 4103
		public static readonly DependencyProperty TagProperty = FrameworkElement.TagProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null));

		// Token: 0x04001008 RID: 4104
		public static readonly DependencyProperty LanguageProperty = FrameworkElement.LanguageProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("en-US"), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001009 RID: 4105
		public static readonly DependencyProperty FocusVisualStyleProperty = FrameworkElement.FocusVisualStyleProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(FrameworkElement.DefaultFocusVisualStyle));

		// Token: 0x0400100A RID: 4106
		public static readonly DependencyProperty CursorProperty = FrameworkElement.CursorProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkContentElement.OnCursorChanged)));

		// Token: 0x0400100B RID: 4107
		public static readonly DependencyProperty ForceCursorProperty = FrameworkElement.ForceCursorProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkContentElement.OnForceCursorChanged)));

		// Token: 0x0400100C RID: 4108
		public static readonly DependencyProperty InputScopeProperty = InputMethod.InputScopeProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400100D RID: 4109
		public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkContentElement.OnDataContextChanged)));

		// Token: 0x0400100E RID: 4110
		public static readonly DependencyProperty BindingGroupProperty = FrameworkElement.BindingGroupProperty.AddOwner(typeof(FrameworkContentElement), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400100F RID: 4111
		private static readonly DependencyProperty LoadedPendingProperty = FrameworkElement.LoadedPendingProperty.AddOwner(typeof(FrameworkContentElement));

		// Token: 0x04001010 RID: 4112
		private static readonly DependencyProperty UnloadedPendingProperty = FrameworkElement.UnloadedPendingProperty.AddOwner(typeof(FrameworkContentElement));

		// Token: 0x04001013 RID: 4115
		public static readonly DependencyProperty ToolTipProperty;

		// Token: 0x04001014 RID: 4116
		public static readonly DependencyProperty ContextMenuProperty;

		// Token: 0x04001019 RID: 4121
		private Style _styleCache;

		// Token: 0x0400101A RID: 4122
		private Style _themeStyleCache;

		// Token: 0x0400101B RID: 4123
		internal DependencyObject _templatedParent;

		// Token: 0x0400101C RID: 4124
		private static readonly UncommonField<ResourceDictionary> ResourcesField;

		// Token: 0x0400101D RID: 4125
		private InternalFlags _flags;

		// Token: 0x0400101E RID: 4126
		private InternalFlags2 _flags2 = InternalFlags2.Default;

		// Token: 0x0400101F RID: 4127
		internal new static DependencyObjectType DType;

		// Token: 0x04001020 RID: 4128
		private new DependencyObject _parent;

		// Token: 0x04001021 RID: 4129
		private FrugalObjectList<DependencyProperty> _inheritableProperties;

		// Token: 0x04001022 RID: 4130
		private static readonly UncommonField<DependencyObject> InheritanceContextField;

		// Token: 0x04001023 RID: 4131
		private static readonly UncommonField<DependencyObject> MentorField;
	}
}
