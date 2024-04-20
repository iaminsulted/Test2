using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using MS.Internal;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows
{
	// Token: 0x0200036A RID: 874
	[XmlLangProperty("Language")]
	[StyleTypedProperty(Property = "FocusVisualStyle", StyleTargetType = typeof(Control))]
	[UsableDuringInitialization(true)]
	[RuntimeNameProperty("Name")]
	public class FrameworkElement : UIElement, IFrameworkInputElement, IInputElement, ISupportInitialize, IHaveResources, IQueryAmbient
	{
		// Token: 0x060021BA RID: 8634 RVA: 0x00178BE4 File Offset: 0x00177BE4
		public FrameworkElement()
		{
			PropertyMetadata metadata = FrameworkElement.StyleProperty.GetMetadata(base.DependencyObjectType);
			Style style = (Style)metadata.DefaultValue;
			if (style != null)
			{
				FrameworkElement.OnStyleChanged(this, new DependencyPropertyChangedEventArgs(FrameworkElement.StyleProperty, metadata, null, style));
			}
			if ((FlowDirection)FrameworkElement.FlowDirectionProperty.GetDefaultValue(base.DependencyObjectType) == FlowDirection.RightToLeft)
			{
				this.IsRightToLeft = true;
			}
			Application application = Application.Current;
			if (application != null && application.HasImplicitStylesInResources)
			{
				this.ShouldLookupImplicitStyles = true;
			}
			FrameworkElement.EnsureFrameworkServices();
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x00178C72 File Offset: 0x00177C72
		// (set) Token: 0x060021BC RID: 8636 RVA: 0x00178C7A File Offset: 0x00177C7A
		public Style Style
		{
			get
			{
				return this._styleCache;
			}
			set
			{
				base.SetValue(FrameworkElement.StyleProperty, value);
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x00178C88 File Offset: 0x00177C88
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeStyle()
		{
			return !this.IsStyleSetFromGenerator && base.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00178CAC File Offset: 0x00177CAC
		private static void OnStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			frameworkElement.HasLocalStyle = (e.NewEntry.BaseValueSourceInternal == BaseValueSourceInternal.Local);
			StyleHelper.UpdateStyleCache(frameworkElement, null, (Style)e.OldValue, (Style)e.NewValue, ref frameworkElement._styleCache);
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x00178CFE File Offset: 0x00177CFE
		// (set) Token: 0x060021C0 RID: 8640 RVA: 0x00178D10 File Offset: 0x00177D10
		public bool OverridesDefaultStyle
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.OverridesDefaultStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.OverridesDefaultStyleProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x00178D23 File Offset: 0x00177D23
		// (set) Token: 0x060021C2 RID: 8642 RVA: 0x00178D35 File Offset: 0x00177D35
		public bool UseLayoutRounding
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.UseLayoutRoundingProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.UseLayoutRoundingProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x00178D48 File Offset: 0x00177D48
		private static void OnUseLayoutRoundingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Visual visual = (FrameworkElement)d;
			bool value = (bool)e.NewValue;
			visual.SetFlags(value, VisualFlags.UseLayoutRounding);
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060021C4 RID: 8644 RVA: 0x00178D73 File Offset: 0x00177D73
		// (set) Token: 0x060021C5 RID: 8645 RVA: 0x00178D80 File Offset: 0x00177D80
		protected internal object DefaultStyleKey
		{
			get
			{
				return base.GetValue(FrameworkElement.DefaultStyleKeyProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.DefaultStyleKeyProperty, value);
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00178D8E File Offset: 0x00177D8E
		private static void OnThemeStyleKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)d).UpdateThemeStyleProperty();
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x00178D9B File Offset: 0x00177D9B
		internal Style ThemeStyle
		{
			get
			{
				return this._themeStyleCache;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x00178DA4 File Offset: 0x00177DA4
		internal static void OnThemeStyleChanged(DependencyObject d, object oldValue, object newValue)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			StyleHelper.UpdateThemeStyleCache(frameworkElement, null, (Style)oldValue, (Style)newValue, ref frameworkElement._themeStyleCache);
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x060021CA RID: 8650 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual FrameworkTemplate TemplateInternal
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x00109403 File Offset: 0x00108403
		// (set) Token: 0x060021CC RID: 8652 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual FrameworkTemplate TemplateCache
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x00178DD1 File Offset: 0x00177DD1
		internal virtual void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.HasTemplateChanged = true;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x00178DDA File Offset: 0x00177DDA
		protected internal virtual void OnStyleChanged(Style oldStyle, Style newStyle)
		{
			this.HasStyleChanged = true;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected internal virtual void ParentLayoutInvalidated(UIElement child)
		{
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00178DE4 File Offset: 0x00177DE4
		public bool ApplyTemplate()
		{
			this.OnPreApplyTemplate();
			bool flag = false;
			UncommonField<HybridDictionary[]> templateDataField = StyleHelper.TemplateDataField;
			FrameworkTemplate templateInternal = this.TemplateInternal;
			int num = 2;
			int num2 = 0;
			while (templateInternal != null && num2 < num && !this.HasTemplateGeneratedSubTree)
			{
				flag = templateInternal.ApplyTemplateContent(templateDataField, this);
				if (flag)
				{
					this.HasTemplateGeneratedSubTree = true;
					StyleHelper.InvokeDeferredActions(this, templateInternal);
					this.OnApplyTemplate();
				}
				if (templateInternal == this.TemplateInternal)
				{
					break;
				}
				templateInternal = this.TemplateInternal;
				num2++;
			}
			this.OnPostApplyTemplate();
			return flag;
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnPreApplyTemplate()
		{
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		public virtual void OnApplyTemplate()
		{
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnPostApplyTemplate()
		{
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x00178E5A File Offset: 0x00177E5A
		public void BeginStoryboard(Storyboard storyboard)
		{
			this.BeginStoryboard(storyboard, HandoffBehavior.SnapshotAndReplace, false);
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00178E65 File Offset: 0x00177E65
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior)
		{
			this.BeginStoryboard(storyboard, handoffBehavior, false);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00178E70 File Offset: 0x00177E70
		public void BeginStoryboard(Storyboard storyboard, HandoffBehavior handoffBehavior, bool isControllable)
		{
			if (storyboard == null)
			{
				throw new ArgumentNullException("storyboard");
			}
			storyboard.Begin(this, handoffBehavior, isControllable);
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x00178E8C File Offset: 0x00177E8C
		internal static FrameworkElement FindNamedFrameworkElement(FrameworkElement startElement, string targetName)
		{
			FrameworkElement result;
			if (targetName == null || targetName.Length == 0)
			{
				result = startElement;
			}
			else
			{
				DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode(startElement, targetName);
				if (dependencyObject == null)
				{
					throw new ArgumentException(SR.Get("TargetNameNotFound", new object[]
					{
						targetName
					}));
				}
				FrameworkObject frameworkObject = new FrameworkObject(dependencyObject);
				if (!frameworkObject.IsFE)
				{
					throw new InvalidOperationException(SR.Get("NamedObjectMustBeFrameworkElement", new object[]
					{
						targetName
					}));
				}
				result = frameworkObject.FE;
			}
			return result;
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x060021D8 RID: 8664 RVA: 0x00178F08 File Offset: 0x00177F08
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TriggerCollection Triggers
		{
			get
			{
				TriggerCollection triggerCollection = EventTrigger.TriggerCollectionField.GetValue(this);
				if (triggerCollection == null)
				{
					triggerCollection = new TriggerCollection(this);
					EventTrigger.TriggerCollectionField.SetValue(this, triggerCollection);
				}
				return triggerCollection;
			}
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x00178F38 File Offset: 0x00177F38
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeTriggers()
		{
			TriggerCollection value = EventTrigger.TriggerCollectionField.GetValue(this);
			return value != null && value.Count != 0;
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x00178F5F File Offset: 0x00177F5F
		private void PrivateInitialized()
		{
			EventTrigger.ProcessTriggerCollection(this);
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x00178F67 File Offset: 0x00177F67
		public DependencyObject TemplatedParent
		{
			get
			{
				return this._templatedParent;
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x060021DC RID: 8668 RVA: 0x00178F6F File Offset: 0x00177F6F
		internal bool IsTemplateRoot
		{
			get
			{
				return this.TemplateChildIndex == 1;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x00178F7A File Offset: 0x00177F7A
		// (set) Token: 0x060021DE RID: 8670 RVA: 0x00178F82 File Offset: 0x00177F82
		internal virtual UIElement TemplateChild
		{
			get
			{
				return this._templateChild;
			}
			set
			{
				if (value != this._templateChild)
				{
					base.RemoveVisualChild(this._templateChild);
					this._templateChild = value;
					base.AddVisualChild(value);
				}
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x00178FA7 File Offset: 0x00177FA7
		internal virtual FrameworkElement StateGroupsRoot
		{
			get
			{
				return this._templateChild as FrameworkElement;
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x00178FB4 File Offset: 0x00177FB4
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._templateChild != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x00178FC4 File Offset: 0x00177FC4
		protected override Visual GetVisualChild(int index)
		{
			if (this._templateChild == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._templateChild;
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x00179018 File Offset: 0x00178018
		internal bool HasResources
		{
			get
			{
				ResourceDictionary value = FrameworkElement.ResourcesField.GetValue(this);
				return value != null && (value.Count > 0 || value.MergedDictionaries.Count > 0);
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x00179050 File Offset: 0x00178050
		// (set) Token: 0x060021E4 RID: 8676 RVA: 0x001790A0 File Offset: 0x001780A0
		[Ambient]
		public ResourceDictionary Resources
		{
			get
			{
				ResourceDictionary resourceDictionary = FrameworkElement.ResourcesField.GetValue(this);
				if (resourceDictionary == null)
				{
					resourceDictionary = new ResourceDictionary();
					resourceDictionary.AddOwner(this);
					FrameworkElement.ResourcesField.SetValue(this, resourceDictionary);
					if (TraceResourceDictionary.IsEnabled)
					{
						TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.NewResourceDictionary, this, 0, resourceDictionary);
					}
				}
				return resourceDictionary;
			}
			set
			{
				ResourceDictionary value2 = FrameworkElement.ResourcesField.GetValue(this);
				FrameworkElement.ResourcesField.SetValue(this, value);
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.NewResourceDictionary, this, value2, value);
				}
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
					TreeWalkHelper.InvalidateOnResourcesChange(this, null, new ResourcesChangeInfo(value2, value));
				}
				if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.NewResourceDictionary, this, value2, value);
				}
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x00179126 File Offset: 0x00178126
		// (set) Token: 0x060021E6 RID: 8678 RVA: 0x0017912E File Offset: 0x0017812E
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

		// Token: 0x060021E7 RID: 8679 RVA: 0x00179137 File Offset: 0x00178137
		bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
		{
			return propertyName != "Resources" || this.HasResources;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0017914E File Offset: 0x0017814E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeResources()
		{
			return this.Resources != null && this.Resources.Count != 0;
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00179168 File Offset: 0x00178168
		protected internal DependencyObject GetTemplateChild(string childName)
		{
			FrameworkTemplate templateInternal = this.TemplateInternal;
			if (templateInternal == null)
			{
				return null;
			}
			return StyleHelper.FindNameInTemplateContent(this, childName, templateInternal) as DependencyObject;
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0017918E File Offset: 0x0017818E
		public object FindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(this, null, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				Helper.ResourceFailureThrow(resourceKey);
			}
			return obj;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x001791B8 File Offset: 0x001781B8
		public object TryFindResource(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			object obj = FrameworkElement.FindResourceInternal(this, null, resourceKey);
			if (obj == DependencyProperty.UnsetValue)
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x001791E8 File Offset: 0x001781E8
		internal static object FindImplicitStyleResource(FrameworkElement fe, object resourceKey, out object source)
		{
			if (fe.ShouldLookupImplicitStyles)
			{
				object unlinkedParent = null;
				bool allowDeferredResourceReference = false;
				bool mustReturnDeferredResourceReference = false;
				bool isImplicitStyleLookup = true;
				DependencyObject boundaryElement = null;
				if (!(fe is Control))
				{
					boundaryElement = fe.TemplatedParent;
				}
				return FrameworkElement.FindResourceInternal(fe, null, FrameworkElement.StyleProperty, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, boundaryElement, isImplicitStyleLookup, out source);
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x00179238 File Offset: 0x00178238
		internal static object FindImplicitStyleResource(FrameworkContentElement fce, object resourceKey, out object source)
		{
			if (fce.ShouldLookupImplicitStyles)
			{
				object unlinkedParent = null;
				bool allowDeferredResourceReference = false;
				bool mustReturnDeferredResourceReference = false;
				bool isImplicitStyleLookup = true;
				DependencyObject templatedParent = fce.TemplatedParent;
				return FrameworkElement.FindResourceInternal(null, fce, FrameworkContentElement.StyleProperty, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, templatedParent, isImplicitStyleLookup, out source);
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0017927C File Offset: 0x0017827C
		internal static object FindResourceInternal(FrameworkElement fe, FrameworkContentElement fce, object resourceKey)
		{
			object obj;
			return FrameworkElement.FindResourceInternal(fe, fce, null, resourceKey, null, false, false, null, false, out obj);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0017929C File Offset: 0x0017829C
		internal static object FindResourceFromAppOrSystem(object resourceKey, out object source, bool disableThrowOnResourceNotFound, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			return FrameworkElement.FindResourceInternal(null, null, null, resourceKey, null, allowDeferredResourceReference, mustReturnDeferredResourceReference, null, disableThrowOnResourceNotFound, out source);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x001792BC File Offset: 0x001782BC
		internal static object FindResourceInternal(FrameworkElement fe, FrameworkContentElement fce, DependencyProperty dp, object resourceKey, object unlinkedParent, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, DependencyObject boundaryElement, bool isImplicitStyleLookup, out object source)
		{
			InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
			if (TraceResourceDictionary.IsEnabled)
			{
				FrameworkObject frameworkObject = new FrameworkObject(fe, fce);
				TraceResourceDictionary.Trace(TraceEventType.Start, TraceResourceDictionary.FindResource, frameworkObject.DO, resourceKey);
			}
			try
			{
				if (fe != null || fce != null || unlinkedParent != null)
				{
					object obj = FrameworkElement.FindResourceInTree(fe, fce, dp, resourceKey, unlinkedParent, allowDeferredResourceReference, mustReturnDeferredResourceReference, boundaryElement, out inheritanceBehavior, out source);
					if (obj != DependencyProperty.UnsetValue)
					{
						return obj;
					}
				}
				Application application = Application.Current;
				if (application != null && (inheritanceBehavior == InheritanceBehavior.Default || inheritanceBehavior == InheritanceBehavior.SkipToAppNow || inheritanceBehavior == InheritanceBehavior.SkipToAppNext))
				{
					object obj = application.FindResourceInternal(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != null)
					{
						source = application;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInApplication, resourceKey, obj);
						}
						return obj;
					}
				}
				if (!isImplicitStyleLookup && inheritanceBehavior != InheritanceBehavior.SkipAllNow && inheritanceBehavior != InheritanceBehavior.SkipAllNext)
				{
					object obj = SystemResources.FindResourceInternal(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != null)
					{
						source = SystemResourceHost.Instance;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInTheme, source, resourceKey, obj);
						}
						return obj;
					}
				}
			}
			finally
			{
				if (TraceResourceDictionary.IsEnabled)
				{
					FrameworkObject frameworkObject2 = new FrameworkObject(fe, fce);
					TraceResourceDictionary.Trace(TraceEventType.Stop, TraceResourceDictionary.FindResource, frameworkObject2.DO, resourceKey);
				}
			}
			if (TraceResourceDictionary.IsEnabledOverride && !isImplicitStyleLookup)
			{
				if ((fe != null && fe.IsLoaded) || (fce != null && fce.IsLoaded))
				{
					TraceResourceDictionary.Trace(TraceEventType.Warning, TraceResourceDictionary.ResourceNotFound, resourceKey);
				}
				else if (TraceResourceDictionary.IsEnabled)
				{
					TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.ResourceNotFound, resourceKey);
				}
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0017942C File Offset: 0x0017842C
		internal static object FindResourceInTree(FrameworkElement feStart, FrameworkContentElement fceStart, DependencyProperty dp, object resourceKey, object unlinkedParent, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference, DependencyObject boundaryElement, out InheritanceBehavior inheritanceBehavior, out object source)
		{
			FrameworkObject frameworkObject = new FrameworkObject(feStart, fceStart);
			FrameworkObject frameworkObject2 = frameworkObject;
			int num = 0;
			bool flag = true;
			inheritanceBehavior = InheritanceBehavior.Default;
			while (flag)
			{
				if (num > ContextLayoutManager.s_LayoutRecursionLimit)
				{
					throw new InvalidOperationException(SR.Get("LogicalTreeLoop"));
				}
				num++;
				Style style = null;
				FrameworkTemplate frameworkTemplate = null;
				Style style2 = null;
				if (frameworkObject2.IsFE)
				{
					FrameworkElement fe = frameworkObject2.FE;
					object obj = fe.FindResourceOnSelf(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = fe;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceOnElement, source, resourceKey, obj);
						}
						return obj;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromStyle(dp))
					{
						style = fe.Style;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromTemplate(dp))
					{
						frameworkTemplate = fe.TemplateInternal;
					}
					if (fe != frameworkObject.FE || StyleHelper.ShouldGetValueFromThemeStyle(dp))
					{
						style2 = fe.ThemeStyle;
					}
				}
				else if (frameworkObject2.IsFCE)
				{
					FrameworkContentElement fce = frameworkObject2.FCE;
					object obj = fce.FindResourceOnSelf(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = fce;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceOnElement, source, resourceKey, obj);
						}
						return obj;
					}
					if (fce != frameworkObject.FCE || StyleHelper.ShouldGetValueFromStyle(dp))
					{
						style = fce.Style;
					}
					if (fce != frameworkObject.FCE || StyleHelper.ShouldGetValueFromThemeStyle(dp))
					{
						style2 = fce.ThemeStyle;
					}
				}
				if (style != null)
				{
					object obj = style.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = style;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInStyle, new object[]
							{
								style.Resources,
								resourceKey,
								style,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (frameworkTemplate != null)
				{
					object obj = frameworkTemplate.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = frameworkTemplate;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInTemplate, new object[]
							{
								frameworkTemplate.Resources,
								resourceKey,
								frameworkTemplate,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (style2 != null)
				{
					object obj = style2.FindResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference);
					if (obj != DependencyProperty.UnsetValue)
					{
						source = style2;
						if (TraceResourceDictionary.IsEnabled)
						{
							TraceResourceDictionary.TraceActivityItem(TraceResourceDictionary.FoundResourceInThemeStyle, new object[]
							{
								style2.Resources,
								resourceKey,
								style2,
								frameworkObject2.DO,
								obj
							});
						}
						return obj;
					}
				}
				if (boundaryElement != null && frameworkObject2.DO == boundaryElement)
				{
					break;
				}
				if (frameworkObject2.IsValid && TreeWalkHelper.SkipNext(frameworkObject2.InheritanceBehavior))
				{
					inheritanceBehavior = frameworkObject2.InheritanceBehavior;
					break;
				}
				if (unlinkedParent != null)
				{
					DependencyObject dependencyObject = unlinkedParent as DependencyObject;
					if (dependencyObject != null)
					{
						frameworkObject2.Reset(dependencyObject);
						if (frameworkObject2.IsValid)
						{
							flag = true;
						}
						else
						{
							DependencyObject frameworkParent = FrameworkElement.GetFrameworkParent(unlinkedParent);
							if (frameworkParent != null)
							{
								frameworkObject2.Reset(frameworkParent);
								flag = true;
							}
							else
							{
								flag = false;
							}
						}
					}
					else
					{
						flag = false;
					}
					unlinkedParent = null;
				}
				else
				{
					frameworkObject2 = frameworkObject2.FrameworkParent;
					flag = frameworkObject2.IsValid;
				}
				if (frameworkObject2.IsValid && TreeWalkHelper.SkipNow(frameworkObject2.InheritanceBehavior))
				{
					inheritanceBehavior = frameworkObject2.InheritanceBehavior;
					break;
				}
			}
			source = null;
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00179764 File Offset: 0x00178764
		internal static object FindTemplateResourceInternal(DependencyObject target, object item, Type templateType)
		{
			if (item == null || item is UIElement)
			{
				return null;
			}
			Type type;
			object dataType = ContentPresenter.DataTypeForItem(item, target, out type);
			ArrayList arrayList = new ArrayList();
			int num = -1;
			while (dataType != null)
			{
				object obj = null;
				if (templateType == typeof(ItemContainerTemplate))
				{
					obj = new ItemContainerTemplateKey(dataType);
				}
				else if (templateType == typeof(DataTemplate))
				{
					obj = new DataTemplateKey(dataType);
				}
				if (obj != null)
				{
					arrayList.Add(obj);
				}
				if (num == -1)
				{
					num = arrayList.Count;
				}
				if (type != null)
				{
					type = type.BaseType;
					if (type == typeof(object))
					{
						type = null;
					}
				}
				dataType = type;
			}
			int count = arrayList.Count;
			object result = FrameworkElement.FindTemplateResourceInTree(target, arrayList, num, ref count);
			if (count >= num)
			{
				object obj2 = Helper.FindTemplateResourceFromAppOrSystem(target, arrayList, num, ref count);
				if (obj2 != null)
				{
					result = obj2;
				}
			}
			return result;
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x0017983C File Offset: 0x0017883C
		private static object FindTemplateResourceInTree(DependencyObject target, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			FrameworkObject frameworkParent = new FrameworkObject(target);
			while (frameworkParent.IsValid)
			{
				ResourceDictionary resourceDictionary = FrameworkElement.GetInstanceResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetStyleResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetThemeStyleResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				resourceDictionary = FrameworkElement.GetTemplateResourceDictionary(frameworkParent.FE, frameworkParent.FCE);
				if (resourceDictionary != null)
				{
					object obj = FrameworkElement.FindBestMatchInResourceDictionary(resourceDictionary, keys, exactMatch, ref bestMatch);
					if (obj != null)
					{
						result = obj;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
				if (frameworkParent.IsValid && TreeWalkHelper.SkipNext(frameworkParent.InheritanceBehavior))
				{
					break;
				}
				frameworkParent = frameworkParent.FrameworkParent;
				if (frameworkParent.IsValid && TreeWalkHelper.SkipNext(frameworkParent.InheritanceBehavior))
				{
					break;
				}
			}
			return result;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x00179950 File Offset: 0x00178950
		private static object FindBestMatchInResourceDictionary(ResourceDictionary table, ArrayList keys, int exactMatch, ref int bestMatch)
		{
			object result = null;
			if (table != null)
			{
				for (int i = 0; i < bestMatch; i++)
				{
					object obj = table[keys[i]];
					if (obj != null)
					{
						result = obj;
						bestMatch = i;
						if (bestMatch < exactMatch)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x00179990 File Offset: 0x00178990
		private static ResourceDictionary GetInstanceResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.HasResources)
				{
					result = fe.Resources;
				}
			}
			else if (fce.HasResources)
			{
				result = fce.Resources;
			}
			return result;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x001799C4 File Offset: 0x001789C4
		private static ResourceDictionary GetStyleResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.Style != null && fe.Style._resources != null)
				{
					result = fe.Style._resources;
				}
			}
			else if (fce.Style != null && fce.Style._resources != null)
			{
				result = fce.Style._resources;
			}
			return result;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00179A1C File Offset: 0x00178A1C
		private static ResourceDictionary GetThemeStyleResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null)
			{
				if (fe.ThemeStyle != null && fe.ThemeStyle._resources != null)
				{
					result = fe.ThemeStyle._resources;
				}
			}
			else if (fce.ThemeStyle != null && fce.ThemeStyle._resources != null)
			{
				result = fce.ThemeStyle._resources;
			}
			return result;
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00179A74 File Offset: 0x00178A74
		private static ResourceDictionary GetTemplateResourceDictionary(FrameworkElement fe, FrameworkContentElement fce)
		{
			ResourceDictionary result = null;
			if (fe != null && fe.TemplateInternal != null && fe.TemplateInternal._resources != null)
			{
				result = fe.TemplateInternal._resources;
			}
			return result;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x00179AA8 File Offset: 0x00178AA8
		internal bool HasNonDefaultValue(DependencyProperty dp)
		{
			return !Helper.HasDefaultValue(this, dp);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x00179AB4 File Offset: 0x00178AB4
		internal static INameScope FindScope(DependencyObject d)
		{
			DependencyObject dependencyObject;
			return FrameworkElement.FindScope(d, out dependencyObject);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x00179ACC File Offset: 0x00178ACC
		internal static INameScope FindScope(DependencyObject d, out DependencyObject scopeOwner)
		{
			while (d != null)
			{
				INameScope nameScope = NameScope.NameScopeFromObject(d);
				if (nameScope != null)
				{
					scopeOwner = d;
					return nameScope;
				}
				DependencyObject parent = LogicalTreeHelper.GetParent(d);
				d = ((parent != null) ? parent : Helper.FindMentor(d.InheritanceContext));
			}
			scopeOwner = null;
			return null;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x00179B0B File Offset: 0x00178B0B
		public void SetResourceReference(DependencyProperty dp, object name)
		{
			base.SetValue(dp, new ResourceReferenceExpression(name));
			this.HasResourceReference = true;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00179B21 File Offset: 0x00178B21
		internal sealed override void EvaluateBaseValueCore(DependencyProperty dp, PropertyMetadata metadata, ref EffectiveValueEntry newEntry)
		{
			if (dp == FrameworkElement.StyleProperty)
			{
				this.HasStyleEverBeenFetched = true;
				this.HasImplicitStyleFromResources = false;
				this.IsStyleSetFromGenerator = false;
			}
			this.GetRawValue(dp, metadata, ref newEntry);
			Storyboard.GetComplexPathValue(this, dp, ref newEntry, metadata);
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00179B54 File Offset: 0x00178B54
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
			if (dp != FrameworkElement.StyleProperty)
			{
				if (StyleHelper.GetValueFromStyleOrTemplate(new FrameworkObject(this, null), dp, ref entry))
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
			if (frameworkPropertyMetadata != null && frameworkPropertyMetadata.Inherits)
			{
				object inheritableValue = this.GetInheritableValue(dp, frameworkPropertyMetadata);
				if (inheritableValue != DependencyProperty.UnsetValue)
				{
					entry.BaseValueSourceInternal = BaseValueSourceInternal.Inherited;
					entry.Value = inheritableValue;
					return;
				}
			}
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00179C14 File Offset: 0x00178C14
		private bool GetValueFromTemplatedParent(DependencyProperty dp, ref EffectiveValueEntry entry)
		{
			FrameworkTemplate templateInternal = ((FrameworkElement)this._templatedParent).TemplateInternal;
			return templateInternal != null && StyleHelper.GetValueFromTemplatedParent(this._templatedParent, this.TemplateChildIndex, new FrameworkObject(this, null), dp, ref templateInternal.ChildRecordFromChildIndex, templateInternal.VisualTree, ref entry);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00179C60 File Offset: 0x00178C60
		private object GetInheritableValue(DependencyProperty dp, FrameworkPropertyMetadata fmetadata)
		{
			if (!TreeWalkHelper.SkipNext(this.InheritanceBehavior) || fmetadata.OverridesInheritanceBehavior)
			{
				InheritanceBehavior inheritanceBehavior = InheritanceBehavior.Default;
				FrameworkElement frameworkElement;
				FrameworkContentElement frameworkContentElement;
				bool frameworkParent = FrameworkElement.GetFrameworkParent(this, out frameworkElement, out frameworkContentElement);
				while (frameworkParent)
				{
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
							string text = string.Format(CultureInfo.InvariantCulture, "[{0}]{1}({2})", base.GetType().Name, dp.Name, base.GetHashCode());
							EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientPropParentCheck, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Verbose, new object[]
							{
								base.GetHashCode(),
								text
							});
						}
						DependencyObject dependencyObject = frameworkElement;
						if (dependencyObject == null)
						{
							dependencyObject = frameworkContentElement;
						}
						EntryIndex entryIndex = dependencyObject.LookupEntry(dp.GlobalIndex);
						return dependencyObject.GetValueEntry(entryIndex, dp, fmetadata, (RequestFlags)12).Value;
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
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00179D80 File Offset: 0x00178D80
		internal Expression GetExpressionCore(DependencyProperty dp, PropertyMetadata metadata)
		{
			this.IsRequestingExpression = true;
			EffectiveValueEntry effectiveValueEntry = new EffectiveValueEntry(dp);
			effectiveValueEntry.Value = DependencyProperty.UnsetValue;
			this.EvaluateBaseValueCore(dp, metadata, ref effectiveValueEntry);
			this.IsRequestingExpression = false;
			return effectiveValueEntry.Value as Expression;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00179DC8 File Offset: 0x00178DC8
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
				if (property != FrameworkElement.StyleProperty && property != Control.TemplateProperty && property != FrameworkElement.DefaultStyleKeyProperty)
				{
					if (this.TemplatedParent != null)
					{
						FrameworkTemplate templateInternal = (this.TemplatedParent as FrameworkElement).TemplateInternal;
						if (templateInternal != null)
						{
							StyleHelper.OnTriggerSourcePropertyInvalidated(null, templateInternal, this.TemplatedParent, property, e, false, ref templateInternal.TriggerSourceRecordFromChildIndex, ref templateInternal.PropertyTriggersWithActions, this.TemplateChildIndex);
						}
					}
					if (this.Style != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.Style, null, this, property, e, true, ref this.Style.TriggerSourceRecordFromChildIndex, ref this.Style.PropertyTriggersWithActions, 0);
					}
					if (this.TemplateInternal != null)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(null, this.TemplateInternal, this, property, e, !this.HasTemplateGeneratedSubTree, ref this.TemplateInternal.TriggerSourceRecordFromChildIndex, ref this.TemplateInternal.PropertyTriggersWithActions, 0);
					}
					if (this.ThemeStyle != null && this.Style != this.ThemeStyle)
					{
						StyleHelper.OnTriggerSourcePropertyInvalidated(this.ThemeStyle, null, this, property, e, true, ref this.ThemeStyle.TriggerSourceRecordFromChildIndex, ref this.ThemeStyle.PropertyTriggersWithActions, 0);
					}
				}
			}
			FrameworkPropertyMetadata frameworkPropertyMetadata = e.Metadata as FrameworkPropertyMetadata;
			if (frameworkPropertyMetadata != null)
			{
				if (frameworkPropertyMetadata.Inherits && (this.InheritanceBehavior == InheritanceBehavior.Default || frameworkPropertyMetadata.OverridesInheritanceBehavior) && (!DependencyObject.IsTreeWalkOperation(e.OperationType) || this.PotentiallyHasMentees))
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
						TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, null, info, true);
					}
					if (this.PotentiallyHasMentees)
					{
						TreeWalkHelper.OnInheritedPropertyChanged(this, ref info, this.InheritanceBehavior);
					}
				}
				if ((e.IsAValueChange || e.IsASubPropertyChange) && (!this.AncestorChangeInProgress || !this.InVisibilityCollapsedTree))
				{
					bool affectsParentMeasure = frameworkPropertyMetadata.AffectsParentMeasure;
					bool affectsParentArrange = frameworkPropertyMetadata.AffectsParentArrange;
					bool affectsMeasure = frameworkPropertyMetadata.AffectsMeasure;
					bool affectsArrange = frameworkPropertyMetadata.AffectsArrange;
					if (affectsMeasure || affectsArrange || affectsParentArrange || affectsParentMeasure)
					{
						Visual visual = VisualTreeHelper.GetParent(this) as Visual;
						while (visual != null)
						{
							UIElement uielement = visual as UIElement;
							if (uielement != null)
							{
								if (FrameworkElement.DType.IsInstanceOfType(uielement))
								{
									((FrameworkElement)uielement).ParentLayoutInvalidated(this);
								}
								if (affectsParentMeasure)
								{
									uielement.InvalidateMeasure();
								}
								if (affectsParentArrange)
								{
									uielement.InvalidateArrange();
									break;
								}
								break;
							}
							else
							{
								visual = (VisualTreeHelper.GetParent(visual) as Visual);
							}
						}
					}
					if (frameworkPropertyMetadata.AffectsMeasure && (!this.BypassLayoutPolicies || (property != FrameworkElement.WidthProperty && property != FrameworkElement.HeightProperty)))
					{
						base.InvalidateMeasure();
					}
					if (frameworkPropertyMetadata.AffectsArrange)
					{
						base.InvalidateArrange();
					}
					if (frameworkPropertyMetadata.AffectsRender && (e.IsAValueChange || !frameworkPropertyMetadata.SubPropertiesDoNotAffectRender))
					{
						base.InvalidateVisual();
					}
				}
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x0017A178 File Offset: 0x00179178
		internal static DependencyObject GetFrameworkParent(object current)
		{
			FrameworkObject frameworkParent = new FrameworkObject(current as DependencyObject);
			frameworkParent = frameworkParent.FrameworkParent;
			return frameworkParent.DO;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x0017A1A4 File Offset: 0x001791A4
		internal static bool GetFrameworkParent(FrameworkElement current, out FrameworkElement feParent, out FrameworkContentElement fceParent)
		{
			FrameworkObject frameworkParent = new FrameworkObject(current, null);
			frameworkParent = frameworkParent.FrameworkParent;
			feParent = frameworkParent.FE;
			fceParent = frameworkParent.FCE;
			return frameworkParent.IsValid;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0017A1DC File Offset: 0x001791DC
		internal static bool GetFrameworkParent(FrameworkContentElement current, out FrameworkElement feParent, out FrameworkContentElement fceParent)
		{
			FrameworkObject frameworkParent = new FrameworkObject(null, current);
			frameworkParent = frameworkParent.FrameworkParent;
			feParent = frameworkParent.FE;
			fceParent = frameworkParent.FCE;
			return frameworkParent.IsValid;
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0017A214 File Offset: 0x00179214
		internal static bool GetContainingFrameworkElement(DependencyObject current, out FrameworkElement fe, out FrameworkContentElement fce)
		{
			FrameworkObject containingFrameworkElement = FrameworkObject.GetContainingFrameworkElement(current);
			if (containingFrameworkElement.IsValid)
			{
				fe = containingFrameworkElement.FE;
				fce = containingFrameworkElement.FCE;
				return true;
			}
			fe = null;
			fce = null;
			return false;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0017A24C File Offset: 0x0017924C
		internal static void GetTemplatedParentChildRecord(DependencyObject templatedParent, int childIndex, out ChildRecord childRecord, out bool isChildRecordValid)
		{
			isChildRecordValid = false;
			childRecord = default(ChildRecord);
			if (templatedParent != null)
			{
				FrameworkObject frameworkObject = new FrameworkObject(templatedParent, true);
				FrameworkTemplate templateInternal = frameworkObject.FE.TemplateInternal;
				if (templateInternal != null && 0 <= childIndex && childIndex < templateInternal.ChildRecordFromChildIndex.Count)
				{
					childRecord = templateInternal.ChildRecordFromChildIndex[childIndex];
					isChildRecordValid = true;
				}
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x00109403 File Offset: 0x00108403
		internal virtual string GetPlainText()
		{
			return null;
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0017A2A8 File Offset: 0x001792A8
		static FrameworkElement()
		{
			FrameworkElement.RequestBringIntoViewEvent = EventManager.RegisterRoutedEvent("RequestBringIntoView", RoutingStrategy.Bubble, typeof(RequestBringIntoViewEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.SizeChangedEvent = EventManager.RegisterRoutedEvent("SizeChanged", RoutingStrategy.Direct, typeof(SizeChangedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement._actualWidthMetadata = new ReadOnlyFrameworkPropertyMetadata(0.0, new GetReadOnlyValueCallback(FrameworkElement.GetActualWidth));
			FrameworkElement.ActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("ActualWidth", typeof(double), FrameworkElement._typeofThis, FrameworkElement._actualWidthMetadata);
			FrameworkElement.ActualWidthProperty = FrameworkElement.ActualWidthPropertyKey.DependencyProperty;
			FrameworkElement._actualHeightMetadata = new ReadOnlyFrameworkPropertyMetadata(0.0, new GetReadOnlyValueCallback(FrameworkElement.GetActualHeight));
			FrameworkElement.ActualHeightPropertyKey = DependencyProperty.RegisterReadOnly("ActualHeight", typeof(double), FrameworkElement._typeofThis, FrameworkElement._actualHeightMetadata);
			FrameworkElement.ActualHeightProperty = FrameworkElement.ActualHeightPropertyKey.DependencyProperty;
			FrameworkElement.LayoutTransformProperty = DependencyProperty.Register("LayoutTransform", typeof(Transform), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(Transform.Identity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnLayoutTransformChanged)));
			FrameworkElement.WidthProperty = DependencyProperty.Register("Width", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsWidthHeightValid));
			FrameworkElement.MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMinWidthHeightValid));
			FrameworkElement.MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMaxWidthHeightValid));
			FrameworkElement.HeightProperty = DependencyProperty.Register("Height", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsWidthHeightValid));
			FrameworkElement.MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMinWidthHeightValid));
			FrameworkElement.MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(double.PositiveInfinity, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnTransformDirty)), new ValidateValueCallback(FrameworkElement.IsMaxWidthHeightValid));
			FrameworkElement.FlowDirectionProperty = DependencyProperty.RegisterAttached("FlowDirection", typeof(FlowDirection), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(FlowDirection.LeftToRight, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnFlowDirectionChanged), new CoerceValueCallback(FrameworkElement.CoerceFlowDirectionProperty)), new ValidateValueCallback(FrameworkElement.IsValidFlowDirection));
			FrameworkElement.MarginProperty = DependencyProperty.Register("Margin", typeof(Thickness), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(FrameworkElement.IsMarginValid));
			FrameworkElement.HorizontalAlignmentProperty = DependencyProperty.Register("HorizontalAlignment", typeof(HorizontalAlignment), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(FrameworkElement.ValidateHorizontalAlignmentValue));
			FrameworkElement.VerticalAlignmentProperty = DependencyProperty.Register("VerticalAlignment", typeof(VerticalAlignment), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsArrange), new ValidateValueCallback(FrameworkElement.ValidateVerticalAlignmentValue));
			FrameworkElement._defaultFocusVisualStyle = null;
			FrameworkElement.FocusVisualStyleProperty = DependencyProperty.Register("FocusVisualStyle", typeof(Style), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(FrameworkElement.DefaultFocusVisualStyle));
			FrameworkElement.CursorProperty = DependencyProperty.Register("Cursor", typeof(Cursor), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkElement.OnCursorChanged)));
			FrameworkElement.ForceCursorProperty = DependencyProperty.Register("ForceCursor", typeof(bool), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.None, new PropertyChangedCallback(FrameworkElement.OnForceCursorChanged)));
			FrameworkElement.InitializedKey = new EventPrivateKey();
			FrameworkElement.LoadedPendingPropertyKey = DependencyProperty.RegisterReadOnly("LoadedPending", typeof(object[]), FrameworkElement._typeofThis, new PropertyMetadata(null));
			FrameworkElement.LoadedPendingProperty = FrameworkElement.LoadedPendingPropertyKey.DependencyProperty;
			FrameworkElement.UnloadedPendingPropertyKey = DependencyProperty.RegisterReadOnly("UnloadedPending", typeof(object[]), FrameworkElement._typeofThis, new PropertyMetadata(null));
			FrameworkElement.UnloadedPendingProperty = FrameworkElement.UnloadedPendingPropertyKey.DependencyProperty;
			FrameworkElement.LoadedEvent = EventManager.RegisterRoutedEvent("Loaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.UnloadedEvent = EventManager.RegisterRoutedEvent("Unloaded", RoutingStrategy.Direct, typeof(RoutedEventHandler), FrameworkElement._typeofThis);
			FrameworkElement.ToolTipProperty = ToolTipService.ToolTipProperty.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuProperty = ContextMenuService.ContextMenuProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null));
			FrameworkElement.ToolTipOpeningEvent = ToolTipService.ToolTipOpeningEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ToolTipClosingEvent = ToolTipService.ToolTipClosingEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuOpeningEvent = ContextMenuService.ContextMenuOpeningEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.ContextMenuClosingEvent = ContextMenuService.ContextMenuClosingEvent.AddOwner(FrameworkElement._typeofThis);
			FrameworkElement.UnclippedDesiredSizeField = new UncommonField<SizeBox>();
			FrameworkElement.LayoutTransformDataField = new UncommonField<FrameworkElement.LayoutTransformData>();
			FrameworkElement.ResourcesField = new UncommonField<ResourceDictionary>();
			FrameworkElement.UIElementDType = DependencyObjectType.FromSystemTypeInternal(typeof(UIElement));
			FrameworkElement._controlDType = null;
			FrameworkElement._contentPresenterDType = null;
			FrameworkElement._pageFunctionBaseDType = null;
			FrameworkElement._pageDType = null;
			FrameworkElement.ResourcesChangedKey = new EventPrivateKey();
			FrameworkElement.InheritedPropertyChangedKey = new EventPrivateKey();
			FrameworkElement.DType = DependencyObjectType.FromSystemTypeInternal(typeof(FrameworkElement));
			FrameworkElement.InheritanceContextField = new UncommonField<DependencyObject>();
			FrameworkElement.MentorField = new UncommonField<DependencyObject>();
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Mouse.QueryCursorEvent, new QueryCursorEventHandler(FrameworkElement.OnQueryCursorOverride), true);
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.PreviewGotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnPreviewGotKeyboardFocus));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnGotKeyboardFocus));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, Keyboard.LostKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(FrameworkElement.OnLostKeyboardFocus));
			UIElement.AllowDropProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsPressAndHoldEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsFlicksEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTapFeedbackEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			Stylus.IsTouchFeedbackEnabledProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
			PropertyChangedCallback propertyChangedCallback = new PropertyChangedCallback(FrameworkElement.NumberSubstitutionChanged);
			NumberSubstitution.CultureSourceProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(NumberCultureSource.User, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.CultureOverrideProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			NumberSubstitution.SubstitutionProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(NumberSubstitutionMethod.AsCulture, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits, propertyChangedCallback));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ToolTipOpeningEvent, new ToolTipEventHandler(FrameworkElement.OnToolTipOpeningThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ToolTipClosingEvent, new ToolTipEventHandler(FrameworkElement.OnToolTipClosingThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ContextMenuOpeningEvent, new ContextMenuEventHandler(FrameworkElement.OnContextMenuOpeningThunk));
			EventManager.RegisterClassHandler(FrameworkElement._typeofThis, FrameworkElement.ContextMenuClosingEvent, new ContextMenuEventHandler(FrameworkElement.OnContextMenuClosingThunk));
			TextElement.FontFamilyProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontFamily)));
			TextElement.FontSizeProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontSize)));
			TextElement.FontStyleProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontStyle)));
			TextElement.FontWeightProperty.OverrideMetadata(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, null, new CoerceValueCallback(FrameworkElement.CoerceFontWeight)));
			TextOptions.TextRenderingModeProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(new PropertyChangedCallback(FrameworkElement.TextRenderingMode_Changed)));
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0017AD79 File Offset: 0x00179D79
		private static void TextRenderingMode_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)d).pushTextRenderingMode();
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0017AD88 File Offset: 0x00179D88
		internal virtual void pushTextRenderingMode()
		{
			if (DependencyPropertyHelper.GetValueSource(this, TextOptions.TextRenderingModeProperty).BaseValueSource > BaseValueSource.Inherited)
			{
				base.VisualTextRenderingMode = TextOptions.GetTextRenderingMode(this);
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnAncestorChanged()
		{
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0017ADB8 File Offset: 0x00179DB8
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			DependencyObject parentInternal = VisualTreeHelper.GetParentInternal(this);
			if (parentInternal != null)
			{
				this.ClearInheritanceContext();
			}
			BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, oldParent, parentInternal);
			BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, oldParent, parentInternal);
			if (parentInternal != null && !(parentInternal is FrameworkElement))
			{
				Visual visual = parentInternal as Visual;
				if (visual != null)
				{
					visual.VisualAncestorChanged += this.OnVisualAncestorChanged;
				}
				else if (parentInternal is Visual3D)
				{
					((Visual3D)parentInternal).VisualAncestorChanged += this.OnVisualAncestorChanged;
				}
			}
			else if (oldParent != null && !(oldParent is FrameworkElement))
			{
				Visual visual2 = oldParent as Visual;
				if (visual2 != null)
				{
					visual2.VisualAncestorChanged -= this.OnVisualAncestorChanged;
				}
				else if (oldParent is Visual3D)
				{
					((Visual3D)oldParent).VisualAncestorChanged -= this.OnVisualAncestorChanged;
				}
			}
			if (this.Parent == null)
			{
				DependencyObject parent = (parentInternal != null) ? parentInternal : oldParent;
				TreeWalkHelper.InvalidateOnTreeChange(this, null, parent, parentInternal != null);
			}
			this.TryFireInitialized();
			base.OnVisualParentChanged(oldParent);
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0017AEA0 File Offset: 0x00179EA0
		internal new void OnVisualAncestorChanged(object sender, AncestorChangedEventArgs e)
		{
			FrameworkElement frameworkElement = null;
			FrameworkContentElement frameworkContentElement = null;
			FrameworkElement.GetContainingFrameworkElement(VisualTreeHelper.GetParent(this), out frameworkElement, out frameworkContentElement);
			if (e.OldParent == null)
			{
				if (frameworkElement == null || !VisualTreeHelper.IsAncestorOf(e.Ancestor, frameworkElement))
				{
					BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, null, VisualTreeHelper.GetParent(e.Ancestor));
					BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, null, VisualTreeHelper.GetParent(e.Ancestor));
					return;
				}
			}
			else if (frameworkElement == null)
			{
				FrameworkElement.GetContainingFrameworkElement(e.OldParent, out frameworkElement, out frameworkContentElement);
				if (frameworkElement != null)
				{
					BroadcastEventHelper.AddOrRemoveHasLoadedChangeHandlerFlag(this, frameworkElement, null);
					BroadcastEventHelper.BroadcastLoadedOrUnloadedEvent(this, frameworkElement, null);
				}
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x0017AF25 File Offset: 0x00179F25
		// (set) Token: 0x06002210 RID: 8720 RVA: 0x0017AF34 File Offset: 0x00179F34
		protected internal InheritanceBehavior InheritanceBehavior
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
					TreeWalkHelper.InvalidateOnTreeChange(this, null, this._parent, true);
					return;
				}
			}
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x06002211 RID: 8721 RVA: 0x0017AFA6 File Offset: 0x00179FA6
		// (remove) Token: 0x06002212 RID: 8722 RVA: 0x0017AFB4 File Offset: 0x00179FB4
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

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x06002213 RID: 8723 RVA: 0x0017AFC2 File Offset: 0x00179FC2
		// (remove) Token: 0x06002214 RID: 8724 RVA: 0x0017AFD0 File Offset: 0x00179FD0
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

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x06002215 RID: 8725 RVA: 0x0017AFDE File Offset: 0x00179FDE
		// (remove) Token: 0x06002216 RID: 8726 RVA: 0x0017AFEC File Offset: 0x00179FEC
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

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x0017AFFA File Offset: 0x00179FFA
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x0017B007 File Offset: 0x0017A007
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object DataContext
		{
			get
			{
				return base.GetValue(FrameworkElement.DataContextProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.DataContextProperty, value);
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0017B015 File Offset: 0x0017A015
		private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			((FrameworkElement)d).RaiseDependencyPropertyChanged(FrameworkElement.DataContextChangedKey, e);
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x00177718 File Offset: 0x00176718
		public BindingExpression GetBindingExpression(DependencyProperty dp)
		{
			return BindingOperations.GetBindingExpression(this, dp);
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x00177721 File Offset: 0x00176721
		public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
		{
			return BindingOperations.SetBinding(this, dp, binding);
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x0017B037 File Offset: 0x0017A037
		public BindingExpression SetBinding(DependencyProperty dp, string path)
		{
			return (BindingExpression)this.SetBinding(dp, new Binding(path));
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x0017B04B File Offset: 0x0017A04B
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x0017B05D File Offset: 0x0017A05D
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public BindingGroup BindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(FrameworkElement.BindingGroupProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.BindingGroupProperty, value);
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x0017B06B File Offset: 0x0017A06B
		protected internal override DependencyObject GetUIParentCore()
		{
			return this._parent;
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x0017B074 File Offset: 0x0017A074
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

		// Token: 0x06002221 RID: 8737 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void AdjustBranchSource(RoutedEventArgs args)
		{
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x0017B0B5 File Offset: 0x0017A0B5
		internal override bool BuildRouteCore(EventRoute route, RoutedEventArgs args)
		{
			return this.BuildRouteCoreHelper(route, args, true);
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x0017B0C0 File Offset: 0x0017A0C0
		internal bool BuildRouteCoreHelper(EventRoute route, RoutedEventArgs args, bool shouldAddIntermediateElementsToRoute)
		{
			bool result = false;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			DependencyObject uiparentCore = this.GetUIParentCore();
			DependencyObject dependencyObject = route.PeekBranchNode() as DependencyObject;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				args.Source = route.PeekBranchSource();
				this.AdjustBranchSource(args);
				route.AddSource(args.Source);
				route.PopBranchNode();
				if (shouldAddIntermediateElementsToRoute)
				{
					FrameworkElement.AddIntermediateElementsToRoute(this, route, args, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			if (!this.IgnoreModelParentBuildRoute(args))
			{
				if (parent == null)
				{
					result = (uiparentCore != null);
				}
				else if (uiparentCore != null)
				{
					Visual visual = parent as Visual;
					if (visual != null)
					{
						if (visual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
						{
							result = true;
						}
					}
					else if (((Visual3D)parent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
					{
						result = true;
					}
					route.PushBranchNode(this, args.Source);
					args.Source = parent;
				}
			}
			return result;
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x0017B189 File Offset: 0x0017A189
		internal override void AddToEventRouteCore(EventRoute route, RoutedEventArgs args)
		{
			FrameworkElement.AddStyleHandlersToEventRoute(this, null, route, args);
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x0017B194 File Offset: 0x0017A194
		internal static void AddStyleHandlersToEventRoute(FrameworkElement fe, FrameworkContentElement fce, EventRoute route, RoutedEventArgs args)
		{
			DependencyObject source = (fe != null) ? fe : fce;
			FrameworkTemplate frameworkTemplate = null;
			Style style;
			DependencyObject templatedParent;
			int templateChildIndex;
			if (fe != null)
			{
				style = fe.Style;
				frameworkTemplate = fe.TemplateInternal;
				templatedParent = fe.TemplatedParent;
				templateChildIndex = fe.TemplateChildIndex;
			}
			else
			{
				style = fce.Style;
				templatedParent = fce.TemplatedParent;
				templateChildIndex = fce.TemplateChildIndex;
			}
			if (style != null && style.EventHandlersStore != null)
			{
				RoutedEventHandlerInfo[] handlers = style.EventHandlersStore.GetRoutedEventHandlers(args.RoutedEvent);
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
			if (frameworkTemplate != null && frameworkTemplate.EventHandlersStore != null)
			{
				RoutedEventHandlerInfo[] handlers = frameworkTemplate.EventHandlersStore.GetRoutedEventHandlers(args.RoutedEvent);
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
			if (templatedParent != null)
			{
				FrameworkTemplate templateInternal = (templatedParent as FrameworkElement).TemplateInternal;
				RoutedEventHandlerInfo[] handlers = null;
				if (templateInternal != null && templateInternal.HasEventDependents)
				{
					handlers = StyleHelper.GetChildRoutedEventHandlers(templateChildIndex, args.RoutedEvent, ref templateInternal.EventDependents);
				}
				FrameworkElement.AddStyleHandlersToEventRoute(route, source, handlers);
			}
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x0017B280 File Offset: 0x0017A280
		private static void AddStyleHandlersToEventRoute(EventRoute route, DependencyObject source, RoutedEventHandlerInfo[] handlers)
		{
			if (handlers != null)
			{
				for (int i = 0; i < handlers.Length; i++)
				{
					route.Add(source, handlers[i].Handler, handlers[i].InvokeHandledEventsToo);
				}
			}
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x00105F35 File Offset: 0x00104F35
		internal virtual bool IgnoreModelParentBuildRoute(RoutedEventArgs args)
		{
			return false;
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x0017B2C0 File Offset: 0x0017A2C0
		internal override bool InvalidateAutomationAncestorsCore(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree)
		{
			bool shouldInvalidateIntermediateElements = true;
			return this.InvalidateAutomationAncestorsCoreHelper(branchNodeStack, out continuePastCoreTree, shouldInvalidateIntermediateElements);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x0017B2D8 File Offset: 0x0017A2D8
		internal override void InvalidateForceInheritPropertyOnChildren(DependencyProperty property)
		{
			if (property == UIElement.IsEnabledProperty)
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
			base.InvalidateForceInheritPropertyOnChildren(property);
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x0017B320 File Offset: 0x0017A320
		internal bool InvalidateAutomationAncestorsCoreHelper(Stack<DependencyObject> branchNodeStack, out bool continuePastCoreTree, bool shouldInvalidateIntermediateElements)
		{
			bool result = true;
			continuePastCoreTree = false;
			DependencyObject parent = VisualTreeHelper.GetParent(this);
			DependencyObject uiparentCore = this.GetUIParentCore();
			DependencyObject dependencyObject = (branchNodeStack.Count > 0) ? branchNodeStack.Peek() : null;
			if (dependencyObject != null && this.IsLogicalDescendent(dependencyObject))
			{
				branchNodeStack.Pop();
				if (shouldInvalidateIntermediateElements)
				{
					result = FrameworkElement.InvalidateAutomationIntermediateElements(this, LogicalTreeHelper.GetParent(dependencyObject));
				}
			}
			if (parent == null)
			{
				continuePastCoreTree = (uiparentCore != null);
			}
			else if (uiparentCore != null)
			{
				Visual visual = parent as Visual;
				if (visual != null)
				{
					if (visual.CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
					{
						continuePastCoreTree = true;
					}
				}
				else if (((Visual3D)parent).CheckFlagsAnd(VisualFlags.IsLayoutIslandRoot))
				{
					continuePastCoreTree = true;
				}
				branchNodeStack.Push(this);
			}
			return result;
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x0017B3C0 File Offset: 0x0017A3C0
		internal static bool InvalidateAutomationIntermediateElements(DependencyObject mergePoint, DependencyObject modelTreeNode)
		{
			UIElement uielement = null;
			ContentElement contentElement = null;
			UIElement3D uielement3D = null;
			while (modelTreeNode != null && modelTreeNode != mergePoint)
			{
				if (!UIElementHelper.InvalidateAutomationPeer(modelTreeNode, out uielement, out contentElement, out uielement3D))
				{
					return false;
				}
				modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
			}
			return true;
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x0017B3F5 File Offset: 0x0017A3F5
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x0017B407 File Offset: 0x0017A407
		public XmlLanguage Language
		{
			get
			{
				return (XmlLanguage)base.GetValue(FrameworkElement.LanguageProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.LanguageProperty, value);
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0017B415 File Offset: 0x0017A415
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x0017B427 File Offset: 0x0017A427
		[DesignerSerializationOptions(DesignerSerializationOptions.SerializeAsAttribute)]
		[MergableProperty(false)]
		[Localizability(LocalizationCategory.NeverLocalize)]
		public string Name
		{
			get
			{
				return (string)base.GetValue(FrameworkElement.NameProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.NameProperty, value);
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x0017B435 File Offset: 0x0017A435
		// (set) Token: 0x06002231 RID: 8753 RVA: 0x0017B442 File Offset: 0x0017A442
		[Localizability(LocalizationCategory.NeverLocalize)]
		public object Tag
		{
			get
			{
				return base.GetValue(FrameworkElement.TagProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.TagProperty, value);
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0017B450 File Offset: 0x0017A450
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x0017B462 File Offset: 0x0017A462
		public InputScope InputScope
		{
			get
			{
				return (InputScope)base.GetValue(FrameworkElement.InputScopeProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.InputScopeProperty, value);
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06002234 RID: 8756 RVA: 0x0017B470 File Offset: 0x0017A470
		// (remove) Token: 0x06002235 RID: 8757 RVA: 0x0017B47F File Offset: 0x0017A47F
		public event RequestBringIntoViewEventHandler RequestBringIntoView
		{
			add
			{
				base.AddHandler(FrameworkElement.RequestBringIntoViewEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.RequestBringIntoViewEvent, value);
			}
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x0017B48D File Offset: 0x0017A48D
		public void BringIntoView()
		{
			this.BringIntoView(Rect.Empty);
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x0017B49C File Offset: 0x0017A49C
		public void BringIntoView(Rect targetRectangle)
		{
			base.RaiseEvent(new RequestBringIntoViewEventArgs(this, targetRectangle)
			{
				RoutedEvent = FrameworkElement.RequestBringIntoViewEvent
			});
		}

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06002238 RID: 8760 RVA: 0x0017B4C3 File Offset: 0x0017A4C3
		// (remove) Token: 0x06002239 RID: 8761 RVA: 0x0017B4D2 File Offset: 0x0017A4D2
		public event SizeChangedEventHandler SizeChanged
		{
			add
			{
				base.AddHandler(FrameworkElement.SizeChangedEvent, value, false);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.SizeChangedEvent, value);
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0017B4E0 File Offset: 0x0017A4E0
		private static object GetActualWidth(DependencyObject d, out BaseValueSourceInternal source)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.HasWidthEverChanged)
			{
				source = BaseValueSourceInternal.Local;
				return frameworkElement.RenderSize.Width;
			}
			source = BaseValueSourceInternal.Default;
			return 0.0;
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x0017B528 File Offset: 0x0017A528
		public double ActualWidth
		{
			get
			{
				return base.RenderSize.Width;
			}
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0017B544 File Offset: 0x0017A544
		private static object GetActualHeight(DependencyObject d, out BaseValueSourceInternal source)
		{
			FrameworkElement frameworkElement = (FrameworkElement)d;
			if (frameworkElement.HasHeightEverChanged)
			{
				source = BaseValueSourceInternal.Local;
				return frameworkElement.RenderSize.Height;
			}
			source = BaseValueSourceInternal.Default;
			return 0.0;
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x0017B58C File Offset: 0x0017A58C
		public double ActualHeight
		{
			get
			{
				return base.RenderSize.Height;
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x0017B5A7 File Offset: 0x0017A5A7
		// (set) Token: 0x0600223F RID: 8767 RVA: 0x0017B5B9 File Offset: 0x0017A5B9
		public Transform LayoutTransform
		{
			get
			{
				return (Transform)base.GetValue(FrameworkElement.LayoutTransformProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.LayoutTransformProperty, value);
			}
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0017B5C7 File Offset: 0x0017A5C7
		private static void OnLayoutTransformChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)d).AreTransformsClean = false;
		}

		// Token: 0x06002241 RID: 8769 RVA: 0x0017B5D8 File Offset: 0x0017A5D8
		private static bool IsWidthHeightValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		// Token: 0x06002242 RID: 8770 RVA: 0x0017B610 File Offset: 0x0017A610
		private static bool IsMinWidthHeightValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0 && !double.IsPositiveInfinity(num);
		}

		// Token: 0x06002243 RID: 8771 RVA: 0x0017B644 File Offset: 0x0017A644
		private static bool IsMaxWidthHeightValid(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && num >= 0.0;
		}

		// Token: 0x06002244 RID: 8772 RVA: 0x0017B5C7 File Offset: 0x0017A5C7
		private static void OnTransformDirty(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)d).AreTransformsClean = false;
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x0017B671 File Offset: 0x0017A671
		// (set) Token: 0x06002246 RID: 8774 RVA: 0x0017B683 File Offset: 0x0017A683
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double Width
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.WidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.WidthProperty, value);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x0017B696 File Offset: 0x0017A696
		// (set) Token: 0x06002248 RID: 8776 RVA: 0x0017B6A8 File Offset: 0x0017A6A8
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MinWidth
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MinWidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MinWidthProperty, value);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x0017B6BB File Offset: 0x0017A6BB
		// (set) Token: 0x0600224A RID: 8778 RVA: 0x0017B6CD File Offset: 0x0017A6CD
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MaxWidthProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MaxWidthProperty, value);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x0017B6E0 File Offset: 0x0017A6E0
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x0017B6F2 File Offset: 0x0017A6F2
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(LengthConverter))]
		public double Height
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.HeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.HeightProperty, value);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x0017B705 File Offset: 0x0017A705
		// (set) Token: 0x0600224E RID: 8782 RVA: 0x0017B717 File Offset: 0x0017A717
		[TypeConverter(typeof(LengthConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public double MinHeight
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MinHeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MinHeightProperty, value);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x0017B72A File Offset: 0x0017A72A
		// (set) Token: 0x06002250 RID: 8784 RVA: 0x0017B73C File Offset: 0x0017A73C
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		[TypeConverter(typeof(LengthConverter))]
		public double MaxHeight
		{
			get
			{
				return (double)base.GetValue(FrameworkElement.MaxHeightProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MaxHeightProperty, value);
			}
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x0017B750 File Offset: 0x0017A750
		private static object CoerceFlowDirectionProperty(DependencyObject d, object value)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.InvalidateArrange();
				frameworkElement.InvalidateVisual();
				frameworkElement.AreTransformsClean = false;
			}
			return value;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x0017B77C File Offset: 0x0017A77C
		private static void OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FrameworkElement frameworkElement = d as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.IsRightToLeft = ((FlowDirection)e.NewValue == FlowDirection.RightToLeft);
				frameworkElement.AreTransformsClean = false;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x0017B7AF File Offset: 0x0017A7AF
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x0017B7BC File Offset: 0x0017A7BC
		[Localizability(LocalizationCategory.None)]
		public FlowDirection FlowDirection
		{
			get
			{
				if (!this.IsRightToLeft)
				{
					return FlowDirection.LeftToRight;
				}
				return FlowDirection.RightToLeft;
			}
			set
			{
				base.SetValue(FrameworkElement.FlowDirectionProperty, value);
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x0017B7CF File Offset: 0x0017A7CF
		public static FlowDirection GetFlowDirection(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (FlowDirection)element.GetValue(FrameworkElement.FlowDirectionProperty);
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x0017B7EF File Offset: 0x0017A7EF
		public static void SetFlowDirection(DependencyObject element, FlowDirection value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(FrameworkElement.FlowDirectionProperty, value);
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x0017B810 File Offset: 0x0017A810
		private static bool IsValidFlowDirection(object o)
		{
			FlowDirection flowDirection = (FlowDirection)o;
			return flowDirection == FlowDirection.LeftToRight || flowDirection == FlowDirection.RightToLeft;
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x0017B830 File Offset: 0x0017A830
		private static bool IsMarginValid(object value)
		{
			return ((Thickness)value).IsValid(true, false, true, false);
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x0017B84F File Offset: 0x0017A84F
		// (set) Token: 0x0600225A RID: 8794 RVA: 0x0017B861 File Offset: 0x0017A861
		public Thickness Margin
		{
			get
			{
				return (Thickness)base.GetValue(FrameworkElement.MarginProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.MarginProperty, value);
			}
		}

		// Token: 0x0600225B RID: 8795 RVA: 0x0017B874 File Offset: 0x0017A874
		internal static bool ValidateHorizontalAlignmentValue(object value)
		{
			HorizontalAlignment horizontalAlignment = (HorizontalAlignment)value;
			return horizontalAlignment == HorizontalAlignment.Left || horizontalAlignment == HorizontalAlignment.Center || horizontalAlignment == HorizontalAlignment.Right || horizontalAlignment == HorizontalAlignment.Stretch;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x0017B899 File Offset: 0x0017A899
		// (set) Token: 0x0600225D RID: 8797 RVA: 0x0017B8AB File Offset: 0x0017A8AB
		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(FrameworkElement.HorizontalAlignmentProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.HorizontalAlignmentProperty, value);
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0017B8C0 File Offset: 0x0017A8C0
		internal static bool ValidateVerticalAlignmentValue(object value)
		{
			VerticalAlignment verticalAlignment = (VerticalAlignment)value;
			return verticalAlignment == VerticalAlignment.Top || verticalAlignment == VerticalAlignment.Center || verticalAlignment == VerticalAlignment.Bottom || verticalAlignment == VerticalAlignment.Stretch;
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x0017B8E5 File Offset: 0x0017A8E5
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x0017B8F7 File Offset: 0x0017A8F7
		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(FrameworkElement.VerticalAlignmentProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.VerticalAlignmentProperty, value);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x0017B90A File Offset: 0x0017A90A
		internal static Style DefaultFocusVisualStyle
		{
			get
			{
				if (FrameworkElement._defaultFocusVisualStyle == null)
				{
					Style style = new Style();
					style.Seal();
					FrameworkElement._defaultFocusVisualStyle = style;
				}
				return FrameworkElement._defaultFocusVisualStyle;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x0017B928 File Offset: 0x0017A928
		// (set) Token: 0x06002263 RID: 8803 RVA: 0x0017B93A File Offset: 0x0017A93A
		public Style FocusVisualStyle
		{
			get
			{
				return (Style)base.GetValue(FrameworkElement.FocusVisualStyleProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.FocusVisualStyleProperty, value);
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x0017B948 File Offset: 0x0017A948
		// (set) Token: 0x06002265 RID: 8805 RVA: 0x0017B95A File Offset: 0x0017A95A
		public Cursor Cursor
		{
			get
			{
				return (Cursor)base.GetValue(FrameworkElement.CursorProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.CursorProperty, value);
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x0017B968 File Offset: 0x0017A968
		private static void OnCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (((FrameworkElement)d).IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x0017B97C File Offset: 0x0017A97C
		// (set) Token: 0x06002268 RID: 8808 RVA: 0x0017B98E File Offset: 0x0017A98E
		public bool ForceCursor
		{
			get
			{
				return (bool)base.GetValue(FrameworkElement.ForceCursorProperty);
			}
			set
			{
				base.SetValue(FrameworkElement.ForceCursorProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x0017B968 File Offset: 0x0017A968
		private static void OnForceCursorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (((FrameworkElement)d).IsMouseOver)
			{
				Mouse.UpdateCursor();
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x0017B9A4 File Offset: 0x0017A9A4
		private static void OnQueryCursorOverride(object sender, QueryCursorEventArgs e)
		{
			FrameworkElement frameworkElement = (FrameworkElement)sender;
			Cursor cursor = frameworkElement.Cursor;
			if (cursor != null && (!e.Handled || frameworkElement.ForceCursor))
			{
				e.Cursor = cursor;
				e.Handled = true;
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x0017B9E0 File Offset: 0x0017A9E0
		private Transform GetFlowDirectionTransform()
		{
			if (!this.BypassLayoutPolicies && FrameworkElement.ShouldApplyMirrorTransform(this))
			{
				return new MatrixTransform(-1.0, 0.0, 0.0, 1.0, base.RenderSize.Width, 0.0);
			}
			return null;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x0017BA40 File Offset: 0x0017AA40
		internal static bool ShouldApplyMirrorTransform(FrameworkElement fe)
		{
			FlowDirection flowDirection = fe.FlowDirection;
			FlowDirection parentFD = FlowDirection.LeftToRight;
			DependencyObject parent = VisualTreeHelper.GetParent(fe);
			FrameworkElement frameworkElement;
			FrameworkContentElement frameworkContentElement;
			if (parent != null)
			{
				parentFD = FrameworkElement.GetFlowDirectionFromVisual(parent);
			}
			else if (FrameworkElement.GetFrameworkParent(fe, out frameworkElement, out frameworkContentElement))
			{
				if (frameworkElement != null && frameworkElement is IContentHost)
				{
					parentFD = frameworkElement.FlowDirection;
				}
				else if (frameworkContentElement != null)
				{
					parentFD = (FlowDirection)frameworkContentElement.GetValue(FrameworkElement.FlowDirectionProperty);
				}
			}
			return FrameworkElement.ApplyMirrorTransform(parentFD, flowDirection);
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x0017BAA8 File Offset: 0x0017AAA8
		private static FlowDirection GetFlowDirectionFromVisual(DependencyObject visual)
		{
			FlowDirection result = FlowDirection.LeftToRight;
			for (DependencyObject dependencyObject = visual; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				FrameworkElement frameworkElement = dependencyObject as FrameworkElement;
				if (frameworkElement != null)
				{
					result = frameworkElement.FlowDirection;
					break;
				}
				object obj = dependencyObject.ReadLocalValue(FrameworkElement.FlowDirectionProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					result = (FlowDirection)obj;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x0017BAF6 File Offset: 0x0017AAF6
		internal static bool ApplyMirrorTransform(FlowDirection parentFD, FlowDirection thisFD)
		{
			return (parentFD == FlowDirection.LeftToRight && thisFD == FlowDirection.RightToLeft) || (parentFD == FlowDirection.RightToLeft && thisFD == FlowDirection.LeftToRight);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x0017BB0C File Offset: 0x0017AB0C
		private Size FindMaximalAreaLocalSpaceRect(Transform layoutTransform, Size transformSpaceBounds)
		{
			double num = transformSpaceBounds.Width;
			double num2 = transformSpaceBounds.Height;
			if (DoubleUtil.IsZero(num) || DoubleUtil.IsZero(num2))
			{
				return new Size(0.0, 0.0);
			}
			bool flag = double.IsInfinity(num);
			bool flag2 = double.IsInfinity(num2);
			if (flag && flag2)
			{
				return new Size(double.PositiveInfinity, double.PositiveInfinity);
			}
			if (flag)
			{
				num = num2;
			}
			else if (flag2)
			{
				num2 = num;
			}
			Matrix value = layoutTransform.Value;
			if (!value.HasInverse)
			{
				return new Size(0.0, 0.0);
			}
			double m = value.M11;
			double m2 = value.M12;
			double m3 = value.M21;
			double m4 = value.M22;
			double num5;
			double num6;
			if (DoubleUtil.IsZero(m2) || DoubleUtil.IsZero(m3))
			{
				double num3 = flag2 ? double.PositiveInfinity : Math.Abs(num2 / m4);
				double num4 = flag ? double.PositiveInfinity : Math.Abs(num / m);
				if (DoubleUtil.IsZero(m2))
				{
					if (DoubleUtil.IsZero(m3))
					{
						num5 = num3;
						num6 = num4;
					}
					else
					{
						num5 = Math.Min(0.5 * Math.Abs(num / m3), num3);
						num6 = num4 - m3 * num5 / m;
					}
				}
				else
				{
					num6 = Math.Min(0.5 * Math.Abs(num2 / m2), num4);
					num5 = num3 - m2 * num6 / m4;
				}
			}
			else if (DoubleUtil.IsZero(m) || DoubleUtil.IsZero(m4))
			{
				double num7 = Math.Abs(num2 / m2);
				double num8 = Math.Abs(num / m3);
				if (DoubleUtil.IsZero(m))
				{
					if (DoubleUtil.IsZero(m4))
					{
						num5 = num8;
						num6 = num7;
					}
					else
					{
						num5 = Math.Min(0.5 * Math.Abs(num2 / m4), num8);
						num6 = num7 - m4 * num5 / m2;
					}
				}
				else
				{
					num6 = Math.Min(0.5 * Math.Abs(num / m), num7);
					num5 = num8 - m * num6 / m3;
				}
			}
			else
			{
				double num9 = Math.Abs(num / m);
				double num10 = Math.Abs(num / m3);
				double num11 = Math.Abs(num2 / m2);
				double num12 = Math.Abs(num2 / m4);
				num6 = Math.Min(num11, num9) * 0.5;
				num5 = Math.Min(num10, num12) * 0.5;
				if ((DoubleUtil.GreaterThanOrClose(num9, num11) && DoubleUtil.LessThanOrClose(num10, num12)) || (DoubleUtil.LessThanOrClose(num9, num11) && DoubleUtil.GreaterThanOrClose(num10, num12)))
				{
					Rect rect = Rect.Transform(new Rect(0.0, 0.0, num6, num5), layoutTransform.Value);
					double num13 = Math.Min(num / rect.Width, num2 / rect.Height);
					if (!double.IsNaN(num13) && !double.IsInfinity(num13))
					{
						num6 *= num13;
						num5 *= num13;
					}
				}
			}
			return new Size(num6, num5);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x0017BE40 File Offset: 0x0017AE40
		protected sealed override Size MeasureCore(Size availableSize)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			this.ApplyTemplate();
			if (this.BypassLayoutPolicies)
			{
				return this.MeasureOverride(availableSize);
			}
			Thickness margin = this.Margin;
			double num = margin.Left + margin.Right;
			double num2 = margin.Top + margin.Bottom;
			if (useLayoutRounding && (this is ScrollContentPresenter || !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness))
			{
				num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				num2 = UIElement.RoundLayoutValue(num2, dpi.DpiScaleY);
			}
			Size size = new Size(Math.Max(availableSize.Width - num, 0.0), Math.Max(availableSize.Height - num2, 0.0));
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			FrameworkElement.LayoutTransformData layoutTransformData = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Transform layoutTransform = this.LayoutTransform;
			if (layoutTransform != null && !layoutTransform.IsIdentity)
			{
				if (layoutTransformData == null)
				{
					layoutTransformData = new FrameworkElement.LayoutTransformData();
					FrameworkElement.LayoutTransformDataField.SetValue(this, layoutTransformData);
				}
				layoutTransformData.CreateTransformSnapshot(layoutTransform);
				layoutTransformData.UntransformedDS = default(Size);
				if (useLayoutRounding)
				{
					layoutTransformData.TransformedUnroundedDS = default(Size);
				}
			}
			else if (layoutTransformData != null)
			{
				layoutTransformData = null;
				FrameworkElement.LayoutTransformDataField.ClearValue(this);
			}
			if (layoutTransformData != null)
			{
				size = this.FindMaximalAreaLocalSpaceRect(layoutTransformData.Transform, size);
			}
			size.Width = Math.Max(minMax.minWidth, Math.Min(size.Width, minMax.maxWidth));
			size.Height = Math.Max(minMax.minHeight, Math.Min(size.Height, minMax.maxHeight));
			if (useLayoutRounding)
			{
				size = UIElement.RoundLayoutSize(size, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size size2 = this.MeasureOverride(size);
			size2 = new Size(Math.Max(size2.Width, minMax.minWidth), Math.Max(size2.Height, minMax.minHeight));
			Size size3 = size2;
			if (layoutTransformData != null)
			{
				layoutTransformData.UntransformedDS = size3;
				Rect rect = Rect.Transform(new Rect(0.0, 0.0, size3.Width, size3.Height), layoutTransformData.Transform.Value);
				size3.Width = rect.Width;
				size3.Height = rect.Height;
			}
			bool flag = false;
			if (size2.Width > minMax.maxWidth)
			{
				size2.Width = minMax.maxWidth;
				flag = true;
			}
			if (size2.Height > minMax.maxHeight)
			{
				size2.Height = minMax.maxHeight;
				flag = true;
			}
			if (layoutTransformData != null)
			{
				Rect rect2 = Rect.Transform(new Rect(0.0, 0.0, size2.Width, size2.Height), layoutTransformData.Transform.Value);
				size2.Width = rect2.Width;
				size2.Height = rect2.Height;
			}
			double num3 = size2.Width + num;
			double num4 = size2.Height + num2;
			if (num3 > availableSize.Width)
			{
				num3 = availableSize.Width;
				flag = true;
			}
			if (num4 > availableSize.Height)
			{
				num4 = availableSize.Height;
				flag = true;
			}
			if (layoutTransformData != null)
			{
				layoutTransformData.TransformedUnroundedDS = new Size(Math.Max(0.0, num3), Math.Max(0.0, num4));
			}
			if (useLayoutRounding)
			{
				num3 = UIElement.RoundLayoutValue(num3, dpi.DpiScaleX);
				num4 = UIElement.RoundLayoutValue(num4, dpi.DpiScaleY);
			}
			SizeBox sizeBox = FrameworkElement.UnclippedDesiredSizeField.GetValue(this);
			if (flag || num3 < 0.0 || num4 < 0.0)
			{
				if (sizeBox == null)
				{
					sizeBox = new SizeBox(size3);
					FrameworkElement.UnclippedDesiredSizeField.SetValue(this, sizeBox);
				}
				else
				{
					sizeBox.Width = size3.Width;
					sizeBox.Height = size3.Height;
				}
			}
			else if (sizeBox != null)
			{
				FrameworkElement.UnclippedDesiredSizeField.ClearValue(this);
			}
			return new Size(Math.Max(0.0, num3), Math.Max(0.0, num4));
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0017C2F0 File Offset: 0x0017B2F0
		protected sealed override void ArrangeCore(Rect finalRect)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Size size = Size.Empty;
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			if (this.BypassLayoutPolicies)
			{
				Size renderSize = base.RenderSize;
				Size renderSize2 = this.ArrangeOverride(finalRect.Size);
				base.RenderSize = renderSize2;
				this.SetLayoutOffset(new Vector(finalRect.X, finalRect.Y), renderSize);
				return;
			}
			this.NeedsClipBounds = false;
			Size size2 = finalRect.Size;
			Thickness margin = this.Margin;
			double num = margin.Left + margin.Right;
			double num2 = margin.Top + margin.Bottom;
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				num = UIElement.RoundLayoutValue(num, dpi.DpiScaleX);
				num2 = UIElement.RoundLayoutValue(num2, dpi.DpiScaleY);
			}
			size2.Width = Math.Max(0.0, size2.Width - num);
			size2.Height = Math.Max(0.0, size2.Height - num2);
			if (useLayoutRounding && value != null)
			{
				size = value.TransformedUnroundedDS;
				size.Width = Math.Max(0.0, size.Width - num);
				size.Height = Math.Max(0.0, size.Height - num2);
			}
			SizeBox value2 = FrameworkElement.UnclippedDesiredSizeField.GetValue(this);
			Size untransformedDS;
			if (value2 == null)
			{
				untransformedDS = new Size(Math.Max(0.0, base.DesiredSize.Width - num), Math.Max(0.0, base.DesiredSize.Height - num2));
				if (size != Size.Empty)
				{
					untransformedDS.Width = Math.Max(size.Width, untransformedDS.Width);
					untransformedDS.Height = Math.Max(size.Height, untransformedDS.Height);
				}
			}
			else
			{
				untransformedDS = new Size(value2.Width, value2.Height);
			}
			if (DoubleUtil.LessThan(size2.Width, untransformedDS.Width))
			{
				this.NeedsClipBounds = true;
				size2.Width = untransformedDS.Width;
			}
			if (DoubleUtil.LessThan(size2.Height, untransformedDS.Height))
			{
				this.NeedsClipBounds = true;
				size2.Height = untransformedDS.Height;
			}
			if (this.HorizontalAlignment != HorizontalAlignment.Stretch)
			{
				size2.Width = untransformedDS.Width;
			}
			if (this.VerticalAlignment != VerticalAlignment.Stretch)
			{
				size2.Height = untransformedDS.Height;
			}
			if (value != null)
			{
				Size size3 = this.FindMaximalAreaLocalSpaceRect(value.Transform, size2);
				size2 = size3;
				untransformedDS = value.UntransformedDS;
				if (!DoubleUtil.IsZero(size3.Width) && !DoubleUtil.IsZero(size3.Height) && (LayoutDoubleUtil.LessThan(size3.Width, untransformedDS.Width) || LayoutDoubleUtil.LessThan(size3.Height, untransformedDS.Height)))
				{
					size2 = untransformedDS;
				}
				if (DoubleUtil.LessThan(size2.Width, untransformedDS.Width))
				{
					this.NeedsClipBounds = true;
					size2.Width = untransformedDS.Width;
				}
				if (DoubleUtil.LessThan(size2.Height, untransformedDS.Height))
				{
					this.NeedsClipBounds = true;
					size2.Height = untransformedDS.Height;
				}
			}
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			double num3 = Math.Max(untransformedDS.Width, minMax.maxWidth);
			if (DoubleUtil.LessThan(num3, size2.Width))
			{
				this.NeedsClipBounds = true;
				size2.Width = num3;
			}
			double num4 = Math.Max(untransformedDS.Height, minMax.maxHeight);
			if (DoubleUtil.LessThan(num4, size2.Height))
			{
				this.NeedsClipBounds = true;
				size2.Height = num4;
			}
			if (useLayoutRounding)
			{
				size2 = UIElement.RoundLayoutSize(size2, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size renderSize3 = base.RenderSize;
			Size renderSize4 = this.ArrangeOverride(size2);
			base.RenderSize = renderSize4;
			if (useLayoutRounding)
			{
				base.RenderSize = UIElement.RoundLayoutSize(base.RenderSize, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			Size size4 = new Size(Math.Min(renderSize4.Width, minMax.maxWidth), Math.Min(renderSize4.Height, minMax.maxHeight));
			if (useLayoutRounding)
			{
				size4 = UIElement.RoundLayoutSize(size4, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			this.NeedsClipBounds |= (DoubleUtil.LessThan(size4.Width, renderSize4.Width) || DoubleUtil.LessThan(size4.Height, renderSize4.Height));
			if (value != null)
			{
				Rect rect = Rect.Transform(new Rect(0.0, 0.0, size4.Width, size4.Height), value.Transform.Value);
				size4.Width = rect.Width;
				size4.Height = rect.Height;
				if (useLayoutRounding)
				{
					size4 = UIElement.RoundLayoutSize(size4, dpi.DpiScaleX, dpi.DpiScaleY);
				}
			}
			Size size5 = new Size(Math.Max(0.0, finalRect.Width - num), Math.Max(0.0, finalRect.Height - num2));
			if (useLayoutRounding)
			{
				size5 = UIElement.RoundLayoutSize(size5, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			this.NeedsClipBounds |= (DoubleUtil.LessThan(size5.Width, size4.Width) || DoubleUtil.LessThan(size5.Height, size4.Height));
			Vector offset = this.ComputeAlignmentOffset(size5, size4);
			offset.X += finalRect.X + margin.Left;
			offset.Y += finalRect.Y + margin.Top;
			if (useLayoutRounding)
			{
				offset.X = UIElement.RoundLayoutValue(offset.X, dpi.DpiScaleX);
				offset.Y = UIElement.RoundLayoutValue(offset.Y, dpi.DpiScaleY);
			}
			this.SetLayoutOffset(offset, renderSize3);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x0017C998 File Offset: 0x0017B998
		protected internal override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			SizeChangedEventArgs sizeChangedEventArgs = new SizeChangedEventArgs(this, sizeInfo);
			sizeChangedEventArgs.RoutedEvent = FrameworkElement.SizeChangedEvent;
			if (sizeInfo.WidthChanged)
			{
				this.HasWidthEverChanged = true;
				base.NotifyPropertyChange(new DependencyPropertyChangedEventArgs(FrameworkElement.ActualWidthProperty, FrameworkElement._actualWidthMetadata, sizeInfo.PreviousSize.Width, sizeInfo.NewSize.Width));
			}
			if (sizeInfo.HeightChanged)
			{
				this.HasHeightEverChanged = true;
				base.NotifyPropertyChange(new DependencyPropertyChangedEventArgs(FrameworkElement.ActualHeightProperty, FrameworkElement._actualHeightMetadata, sizeInfo.PreviousSize.Height, sizeInfo.NewSize.Height));
			}
			base.RaiseEvent(sizeChangedEventArgs);
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x0017CA54 File Offset: 0x0017BA54
		private Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
		{
			Vector result = default(Vector);
			HorizontalAlignment horizontalAlignment = this.HorizontalAlignment;
			VerticalAlignment verticalAlignment = this.VerticalAlignment;
			if (horizontalAlignment == HorizontalAlignment.Stretch && inkSize.Width > clientSize.Width)
			{
				horizontalAlignment = HorizontalAlignment.Left;
			}
			if (verticalAlignment == VerticalAlignment.Stretch && inkSize.Height > clientSize.Height)
			{
				verticalAlignment = VerticalAlignment.Top;
			}
			if (horizontalAlignment == HorizontalAlignment.Center || horizontalAlignment == HorizontalAlignment.Stretch)
			{
				result.X = (clientSize.Width - inkSize.Width) * 0.5;
			}
			else if (horizontalAlignment == HorizontalAlignment.Right)
			{
				result.X = clientSize.Width - inkSize.Width;
			}
			else
			{
				result.X = 0.0;
			}
			if (verticalAlignment == VerticalAlignment.Center || verticalAlignment == VerticalAlignment.Stretch)
			{
				result.Y = (clientSize.Height - inkSize.Height) * 0.5;
			}
			else if (verticalAlignment == VerticalAlignment.Bottom)
			{
				result.Y = clientSize.Height - inkSize.Height;
			}
			else
			{
				result.Y = 0.0;
			}
			return result;
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x0017CB50 File Offset: 0x0017BB50
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			bool useLayoutRounding = this.UseLayoutRounding;
			DpiScale dpi = base.GetDpi();
			if (useLayoutRounding && !base.CheckFlagsAnd(VisualFlags.UseLayoutRounding))
			{
				base.SetFlags(true, VisualFlags.UseLayoutRounding);
			}
			if (!this.NeedsClipBounds && !base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
			if (useLayoutRounding && !FrameworkAppContextSwitches.DoNotApplyLayoutRoundingToMarginsAndBorderThickness)
			{
				minMax.maxHeight = UIElement.RoundLayoutValue(minMax.maxHeight, dpi.DpiScaleY);
				minMax.maxWidth = UIElement.RoundLayoutValue(minMax.maxWidth, dpi.DpiScaleX);
				minMax.minHeight = UIElement.RoundLayoutValue(minMax.minHeight, dpi.DpiScaleY);
				minMax.minWidth = UIElement.RoundLayoutValue(minMax.minWidth, dpi.DpiScaleX);
			}
			Size renderSize = base.RenderSize;
			double num = double.IsPositiveInfinity(minMax.maxWidth) ? renderSize.Width : minMax.maxWidth;
			double num2 = double.IsPositiveInfinity(minMax.maxHeight) ? renderSize.Height : minMax.maxHeight;
			bool flag = base.ClipToBounds || DoubleUtil.LessThan(num, renderSize.Width) || DoubleUtil.LessThan(num2, renderSize.Height);
			renderSize.Width = Math.Min(renderSize.Width, minMax.maxWidth);
			renderSize.Height = Math.Min(renderSize.Height, minMax.maxHeight);
			FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
			Rect rect = default(Rect);
			if (value != null)
			{
				rect = Rect.Transform(new Rect(0.0, 0.0, renderSize.Width, renderSize.Height), value.Transform.Value);
				renderSize.Width = rect.Width;
				renderSize.Height = rect.Height;
			}
			Thickness margin = this.Margin;
			double num3 = margin.Left + margin.Right;
			double num4 = margin.Top + margin.Bottom;
			Size clientSize = new Size(Math.Max(0.0, layoutSlotSize.Width - num3), Math.Max(0.0, layoutSlotSize.Height - num4));
			bool flag2 = base.ClipToBounds || DoubleUtil.LessThan(clientSize.Width, renderSize.Width) || DoubleUtil.LessThan(clientSize.Height, renderSize.Height);
			Transform flowDirectionTransform = this.GetFlowDirectionTransform();
			if (flag && !flag2)
			{
				Rect rect2 = new Rect(0.0, 0.0, num, num2);
				if (useLayoutRounding)
				{
					rect2 = UIElement.RoundLayoutRect(rect2, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				RectangleGeometry rectangleGeometry = new RectangleGeometry(rect2);
				if (flowDirectionTransform != null)
				{
					rectangleGeometry.Transform = flowDirectionTransform;
				}
				return rectangleGeometry;
			}
			if (!flag2)
			{
				return null;
			}
			Vector vector = this.ComputeAlignmentOffset(clientSize, renderSize);
			if (value == null)
			{
				Rect rect3 = new Rect(-vector.X + rect.X, -vector.Y + rect.Y, clientSize.Width, clientSize.Height);
				if (useLayoutRounding)
				{
					rect3 = UIElement.RoundLayoutRect(rect3, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				if (flag)
				{
					Rect rect4 = new Rect(0.0, 0.0, num, num2);
					if (useLayoutRounding)
					{
						rect4 = UIElement.RoundLayoutRect(rect4, dpi.DpiScaleX, dpi.DpiScaleY);
					}
					rect3.Intersect(rect4);
				}
				RectangleGeometry rectangleGeometry2 = new RectangleGeometry(rect3);
				if (flowDirectionTransform != null)
				{
					rectangleGeometry2.Transform = flowDirectionTransform;
				}
				return rectangleGeometry2;
			}
			Rect rect5 = new Rect(-vector.X + rect.X, -vector.Y + rect.Y, clientSize.Width, clientSize.Height);
			if (useLayoutRounding)
			{
				rect5 = UIElement.RoundLayoutRect(rect5, dpi.DpiScaleX, dpi.DpiScaleY);
			}
			RectangleGeometry rectangleGeometry3 = new RectangleGeometry(rect5);
			Matrix value2 = value.Transform.Value;
			if (value2.HasInverse)
			{
				value2.Invert();
				rectangleGeometry3.Transform = new MatrixTransform(value2);
			}
			if (flag)
			{
				Rect rect6 = new Rect(0.0, 0.0, num, num2);
				if (useLayoutRounding)
				{
					rect6 = UIElement.RoundLayoutRect(rect6, dpi.DpiScaleX, dpi.DpiScaleY);
				}
				PathGeometry pathGeometry = Geometry.Combine(new RectangleGeometry(rect6), rectangleGeometry3, GeometryCombineMode.Intersect, null);
				if (flowDirectionTransform != null)
				{
					pathGeometry.Transform = flowDirectionTransform;
				}
				return pathGeometry;
			}
			if (flowDirectionTransform != null)
			{
				if (rectangleGeometry3.Transform != null)
				{
					rectangleGeometry3.Transform = new MatrixTransform(rectangleGeometry3.Transform.Value * flowDirectionTransform.Value);
				}
				else
				{
					rectangleGeometry3.Transform = flowDirectionTransform;
				}
			}
			return rectangleGeometry3;
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x0017D004 File Offset: 0x0017C004
		internal Geometry GetLayoutClipInternal()
		{
			if (base.IsMeasureValid && base.IsArrangeValid)
			{
				return this.GetLayoutClip(base.PreviousArrangeRect.Size);
			}
			return null;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0017D037 File Offset: 0x0017C037
		protected virtual Size MeasureOverride(Size availableSize)
		{
			return new Size(0.0, 0.0);
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x001136C4 File Offset: 0x001126C4
		protected virtual Size ArrangeOverride(Size finalSize)
		{
			return finalSize;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x0017D050 File Offset: 0x0017C050
		internal static void InternalSetLayoutTransform(UIElement element, Transform layoutTransform)
		{
			FrameworkElement frameworkElement = element as FrameworkElement;
			element.InternalSetOffsetWorkaround(default(Vector));
			Transform transform = (frameworkElement == null) ? null : frameworkElement.GetFlowDirectionTransform();
			Transform transform2 = element.RenderTransform;
			if (transform2 == Transform.Identity)
			{
				transform2 = null;
			}
			TransformCollection transformCollection = new TransformCollection();
			transformCollection.CanBeInheritanceContext = false;
			if (transform != null)
			{
				transformCollection.Add(transform);
			}
			if (transform2 != null)
			{
				transformCollection.Add(transform2);
			}
			transformCollection.Add(layoutTransform);
			element.InternalSetTransformWorkaround(new TransformGroup
			{
				Children = transformCollection
			});
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0017D0D4 File Offset: 0x0017C0D4
		private void SetLayoutOffset(Vector offset, Size oldRenderSize)
		{
			if (!base.AreTransformsClean || !DoubleUtil.AreClose(base.RenderSize, oldRenderSize))
			{
				Transform flowDirectionTransform = this.GetFlowDirectionTransform();
				Transform transform = base.RenderTransform;
				if (transform == Transform.Identity)
				{
					transform = null;
				}
				FrameworkElement.LayoutTransformData value = FrameworkElement.LayoutTransformDataField.GetValue(this);
				TransformGroup transformGroup = null;
				if (flowDirectionTransform != null || transform != null || value != null)
				{
					transformGroup = new TransformGroup();
					transformGroup.CanBeInheritanceContext = false;
					transformGroup.Children.CanBeInheritanceContext = false;
					if (flowDirectionTransform != null)
					{
						transformGroup.Children.Add(flowDirectionTransform);
					}
					if (value != null)
					{
						transformGroup.Children.Add(value.Transform);
						FrameworkElement.MinMax minMax = new FrameworkElement.MinMax(this);
						Size renderSize = base.RenderSize;
						if (double.IsPositiveInfinity(minMax.maxWidth))
						{
							double width = renderSize.Width;
						}
						if (double.IsPositiveInfinity(minMax.maxHeight))
						{
							double height = renderSize.Height;
						}
						renderSize.Width = Math.Min(renderSize.Width, minMax.maxWidth);
						renderSize.Height = Math.Min(renderSize.Height, minMax.maxHeight);
						Rect rect = Rect.Transform(new Rect(renderSize), value.Transform.Value);
						transformGroup.Children.Add(new TranslateTransform(-rect.X, -rect.Y));
					}
					if (transform != null)
					{
						Point renderTransformOrigin = this.GetRenderTransformOrigin();
						bool flag = renderTransformOrigin.X != 0.0 || renderTransformOrigin.Y != 0.0;
						if (flag)
						{
							TranslateTransform translateTransform = new TranslateTransform(-renderTransformOrigin.X, -renderTransformOrigin.Y);
							translateTransform.Freeze();
							transformGroup.Children.Add(translateTransform);
						}
						transformGroup.Children.Add(transform);
						if (flag)
						{
							TranslateTransform translateTransform2 = new TranslateTransform(renderTransformOrigin.X, renderTransformOrigin.Y);
							translateTransform2.Freeze();
							transformGroup.Children.Add(translateTransform2);
						}
					}
				}
				base.VisualTransform = transformGroup;
				base.AreTransformsClean = true;
			}
			Vector visualOffset = base.VisualOffset;
			if (!DoubleUtil.AreClose(visualOffset.X, offset.X) || !DoubleUtil.AreClose(visualOffset.Y, offset.Y))
			{
				base.VisualOffset = offset;
			}
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0017D304 File Offset: 0x0017C304
		private Point GetRenderTransformOrigin()
		{
			Point renderTransformOrigin = base.RenderTransformOrigin;
			Size renderSize = base.RenderSize;
			return new Point(renderSize.Width * renderTransformOrigin.X, renderSize.Height * renderTransformOrigin.Y);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x0017759C File Offset: 0x0017659C
		public sealed override bool MoveFocus(TraversalRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return KeyboardNavigation.Current.Navigate(this, request);
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x001775B8 File Offset: 0x001765B8
		public sealed override DependencyObject PredictFocus(FocusNavigationDirection direction)
		{
			return KeyboardNavigation.Current.PredictFocusedElement(this, direction);
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0017D344 File Offset: 0x0017C344
		private static void OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (e.OriginalSource == sender)
			{
				IInputElement focusedElement = FocusManager.GetFocusedElement((FrameworkElement)sender, true);
				if (focusedElement != null && focusedElement != sender && Keyboard.IsFocusable(focusedElement as DependencyObject))
				{
					IInputElement focusedElement2 = Keyboard.FocusedElement;
					focusedElement.Focus();
					if (Keyboard.FocusedElement == focusedElement || Keyboard.FocusedElement != focusedElement2)
					{
						e.Handled = true;
						return;
					}
				}
			}
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x0017D3A0 File Offset: 0x0017C3A0
		private static void OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (sender == e.OriginalSource)
			{
				FrameworkElement frameworkElement = (FrameworkElement)sender;
				KeyboardNavigation.UpdateFocusedElement(frameworkElement);
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				KeyboardNavigation.ShowFocusVisual();
				keyboardNavigation.NotifyFocusChanged(frameworkElement, e);
				keyboardNavigation.UpdateActiveElement(frameworkElement);
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x00177613 File Offset: 0x00176613
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

		// Token: 0x06002280 RID: 8832 RVA: 0x0017D3DB File Offset: 0x0017C3DB
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (base.IsKeyboardFocused)
			{
				this.BringIntoView();
			}
			base.OnGotFocus(e);
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x0017D3F2 File Offset: 0x0017C3F2
		public virtual void BeginInit()
		{
			if (this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("NestedBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, true);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x0017D41D File Offset: 0x0017C41D
		public virtual void EndInit()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending))
			{
				throw new InvalidOperationException(SR.Get("EndInitWithoutBeginInitNotSupported"));
			}
			this.WriteInternalFlag(InternalFlags.InitPending, false);
			this.TryFireInitialized();
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002283 RID: 8835 RVA: 0x0017D44E File Offset: 0x0017C44E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsInitialized
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsInitialized);
			}
		}

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x06002284 RID: 8836 RVA: 0x0017D45B File Offset: 0x0017C45B
		// (remove) Token: 0x06002285 RID: 8837 RVA: 0x0017D469 File Offset: 0x0017C469
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

		// Token: 0x06002286 RID: 8838 RVA: 0x0017D477 File Offset: 0x0017C477
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

		// Token: 0x06002287 RID: 8839 RVA: 0x0017D4A1 File Offset: 0x0017C4A1
		private void TryFireInitialized()
		{
			if (!this.ReadInternalFlag(InternalFlags.InitPending) && !this.ReadInternalFlag(InternalFlags.IsInitialized))
			{
				this.WriteInternalFlag(InternalFlags.IsInitialized, true);
				this.PrivateInitialized();
				this.OnInitialized(EventArgs.Empty);
			}
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0017D4DC File Offset: 0x0017C4DC
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

		// Token: 0x06002289 RID: 8841 RVA: 0x0017D50B File Offset: 0x0017C50B
		private static void NumberSubstitutionChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			((FrameworkElement)o).HasNumberSubstitutionChanged = true;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0017D51C File Offset: 0x0017C51C
		private static bool ShouldUseSystemFont(FrameworkElement fe, DependencyProperty dp)
		{
			bool flag;
			return (SystemResources.SystemResourcesAreChanging || (fe.ReadInternalFlag(InternalFlags.CreatingRoot) && SystemResources.SystemResourcesHaveChanged)) && fe._parent == null && VisualTreeHelper.GetParent(fe) == null && fe.GetValueSource(dp, null, out flag) == BaseValueSourceInternal.Default;
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x0017D563 File Offset: 0x0017C563
		private static object CoerceFontFamily(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontFamilyProperty))
			{
				return SystemFonts.MessageFontFamily;
			}
			return value;
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x0017D57E File Offset: 0x0017C57E
		private static object CoerceFontSize(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontSizeProperty))
			{
				return SystemFonts.MessageFontSize;
			}
			return value;
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x0017D59E File Offset: 0x0017C59E
		private static object CoerceFontStyle(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontStyleProperty))
			{
				return SystemFonts.MessageFontStyle;
			}
			return value;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0017D5BE File Offset: 0x0017C5BE
		private static object CoerceFontWeight(DependencyObject o, object value)
		{
			if (FrameworkElement.ShouldUseSystemFont((FrameworkElement)o, TextElement.FontWeightProperty))
			{
				return SystemFonts.MessageFontWeight;
			}
			return value;
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x0017D5E0 File Offset: 0x0017C5E0
		internal sealed override void OnPresentationSourceChanged(bool attached)
		{
			base.OnPresentationSourceChanged(attached);
			if (attached)
			{
				this.FireLoadedOnDescendentsInternal();
				if (SystemResources.SystemResourcesHaveChanged)
				{
					this.WriteInternalFlag(InternalFlags.CreatingRoot, true);
					base.CoerceValue(TextElement.FontFamilyProperty);
					base.CoerceValue(TextElement.FontSizeProperty);
					base.CoerceValue(TextElement.FontStyleProperty);
					base.CoerceValue(TextElement.FontWeightProperty);
					this.WriteInternalFlag(InternalFlags.CreatingRoot, false);
					return;
				}
			}
			else
			{
				this.FireUnloadedOnDescendentsInternal();
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x0017D650 File Offset: 0x0017C650
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

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x06002291 RID: 8849 RVA: 0x0017D689 File Offset: 0x0017C689
		// (remove) Token: 0x06002292 RID: 8850 RVA: 0x0017D698 File Offset: 0x0017C698
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

		// Token: 0x06002293 RID: 8851 RVA: 0x0017D6A6 File Offset: 0x0017C6A6
		internal override void OnAddHandler(RoutedEvent routedEvent, Delegate handler)
		{
			base.OnAddHandler(routedEvent, handler);
			if (routedEvent == FrameworkElement.LoadedEvent || routedEvent == FrameworkElement.UnloadedEvent)
			{
				BroadcastEventHelper.AddHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x06002294 RID: 8852 RVA: 0x0017D6C6 File Offset: 0x0017C6C6
		internal override void OnRemoveHandler(RoutedEvent routedEvent, Delegate handler)
		{
			base.OnRemoveHandler(routedEvent, handler);
			if (routedEvent != FrameworkElement.LoadedEvent && routedEvent != FrameworkElement.UnloadedEvent)
			{
				return;
			}
			if (!this.ThisHasLoadedChangeEventHandler)
			{
				BroadcastEventHelper.RemoveHasLoadedChangeHandlerFlagInAncestry(this);
			}
		}

		// Token: 0x06002295 RID: 8853 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		internal void OnLoaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x06002296 RID: 8854 RVA: 0x0017D6F8 File Offset: 0x0017C6F8
		// (remove) Token: 0x06002297 RID: 8855 RVA: 0x0017D707 File Offset: 0x0017C707
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

		// Token: 0x06002298 RID: 8856 RVA: 0x0017D6EF File Offset: 0x0017C6EF
		internal void OnUnloaded(RoutedEventArgs args)
		{
			base.RaiseEvent(args);
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x0017D718 File Offset: 0x0017C718
		internal override void AddSynchronizedInputPreOpportunityHandlerCore(EventRoute route, RoutedEventArgs args)
		{
			UIElement uielement = this._templatedParent as UIElement;
			if (uielement != null)
			{
				uielement.AddSynchronizedInputPreOpportunityHandler(route, args);
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x0017D4DC File Offset: 0x0017C4DC
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

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x0017D73C File Offset: 0x0017C73C
		internal static PopupControlService PopupControlService
		{
			get
			{
				return FrameworkElement.EnsureFrameworkServices()._popupControlService;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x0600229C RID: 8860 RVA: 0x0017D748 File Offset: 0x0017C748
		internal static KeyboardNavigation KeyboardNavigation
		{
			get
			{
				return FrameworkElement.EnsureFrameworkServices()._keyboardNavigation;
			}
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x0017D754 File Offset: 0x0017C754
		private static FrameworkElement.FrameworkServices EnsureFrameworkServices()
		{
			if (FrameworkElement._frameworkServices == null)
			{
				FrameworkElement._frameworkServices = new FrameworkElement.FrameworkServices();
			}
			return FrameworkElement._frameworkServices;
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x00177B29 File Offset: 0x00176B29
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x00177B31 File Offset: 0x00176B31
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.ToolTip)]
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

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x0017D76C File Offset: 0x0017C76C
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x0017D77E File Offset: 0x0017C77E
		public ContextMenu ContextMenu
		{
			get
			{
				return base.GetValue(FrameworkElement.ContextMenuProperty) as ContextMenu;
			}
			set
			{
				base.SetValue(FrameworkElement.ContextMenuProperty, value);
			}
		}

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x060022A2 RID: 8866 RVA: 0x0017D78C File Offset: 0x0017C78C
		// (remove) Token: 0x060022A3 RID: 8867 RVA: 0x0017D79A File Offset: 0x0017C79A
		public event ToolTipEventHandler ToolTipOpening
		{
			add
			{
				base.AddHandler(FrameworkElement.ToolTipOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ToolTipOpeningEvent, value);
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x0017D7A8 File Offset: 0x0017C7A8
		private static void OnToolTipOpeningThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkElement)sender).OnToolTipOpening(e);
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnToolTipOpening(ToolTipEventArgs e)
		{
		}

		// Token: 0x1400005F RID: 95
		// (add) Token: 0x060022A6 RID: 8870 RVA: 0x0017D7B6 File Offset: 0x0017C7B6
		// (remove) Token: 0x060022A7 RID: 8871 RVA: 0x0017D7C4 File Offset: 0x0017C7C4
		public event ToolTipEventHandler ToolTipClosing
		{
			add
			{
				base.AddHandler(FrameworkElement.ToolTipClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ToolTipClosingEvent, value);
			}
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x0017D7D2 File Offset: 0x0017C7D2
		private static void OnToolTipClosingThunk(object sender, ToolTipEventArgs e)
		{
			((FrameworkElement)sender).OnToolTipClosing(e);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnToolTipClosing(ToolTipEventArgs e)
		{
		}

		// Token: 0x14000060 RID: 96
		// (add) Token: 0x060022AA RID: 8874 RVA: 0x0017D7E0 File Offset: 0x0017C7E0
		// (remove) Token: 0x060022AB RID: 8875 RVA: 0x0017D7EE File Offset: 0x0017C7EE
		public event ContextMenuEventHandler ContextMenuOpening
		{
			add
			{
				base.AddHandler(FrameworkElement.ContextMenuOpeningEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ContextMenuOpeningEvent, value);
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x0017D7FC File Offset: 0x0017C7FC
		private static void OnContextMenuOpeningThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkElement)sender).OnContextMenuOpening(e);
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnContextMenuOpening(ContextMenuEventArgs e)
		{
		}

		// Token: 0x14000061 RID: 97
		// (add) Token: 0x060022AE RID: 8878 RVA: 0x0017D80A File Offset: 0x0017C80A
		// (remove) Token: 0x060022AF RID: 8879 RVA: 0x0017D818 File Offset: 0x0017C818
		public event ContextMenuEventHandler ContextMenuClosing
		{
			add
			{
				base.AddHandler(FrameworkElement.ContextMenuClosingEvent, value);
			}
			remove
			{
				base.RemoveHandler(FrameworkElement.ContextMenuClosingEvent, value);
			}
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x0017D826 File Offset: 0x0017C826
		private static void OnContextMenuClosingThunk(object sender, ContextMenuEventArgs e)
		{
			((FrameworkElement)sender).OnContextMenuClosing(e);
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		protected virtual void OnContextMenuClosing(ContextMenuEventArgs e)
		{
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x0017D834 File Offset: 0x0017C834
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

		// Token: 0x060022B3 RID: 8883 RVA: 0x0017D864 File Offset: 0x0017C864
		internal static void AddIntermediateElementsToRoute(DependencyObject mergePoint, EventRoute route, RoutedEventArgs args, DependencyObject modelTreeNode)
		{
			while (modelTreeNode != null && modelTreeNode != mergePoint)
			{
				UIElement uielement = modelTreeNode as UIElement;
				ContentElement contentElement = modelTreeNode as ContentElement;
				UIElement3D uielement3D = modelTreeNode as UIElement3D;
				if (uielement != null)
				{
					uielement.AddToEventRoute(route, args);
					FrameworkElement frameworkElement = uielement as FrameworkElement;
					if (frameworkElement != null)
					{
						FrameworkElement.AddStyleHandlersToEventRoute(frameworkElement, null, route, args);
					}
				}
				else if (contentElement != null)
				{
					contentElement.AddToEventRoute(route, args);
					FrameworkContentElement frameworkContentElement = contentElement as FrameworkContentElement;
					if (frameworkContentElement != null)
					{
						FrameworkElement.AddStyleHandlersToEventRoute(null, frameworkContentElement, route, args);
					}
				}
				else if (uielement3D != null)
				{
					uielement3D.AddToEventRoute(route, args);
				}
				modelTreeNode = LogicalTreeHelper.GetParent(modelTreeNode);
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x0017784B File Offset: 0x0017684B
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

		// Token: 0x060022B5 RID: 8885 RVA: 0x0017D8E5 File Offset: 0x0017C8E5
		internal void EventHandlersStoreAdd(EventPrivateKey key, Delegate handler)
		{
			base.EnsureEventHandlersStore();
			base.EventHandlersStore.Add(key, handler);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0017D8FC File Offset: 0x0017C8FC
		internal void EventHandlersStoreRemove(EventPrivateKey key, Delegate handler)
		{
			EventHandlersStore eventHandlersStore = base.EventHandlersStore;
			if (eventHandlersStore != null)
			{
				eventHandlersStore.Remove(key, handler);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060022B7 RID: 8887 RVA: 0x0017D91B File Offset: 0x0017C91B
		// (set) Token: 0x060022B8 RID: 8888 RVA: 0x0017D924 File Offset: 0x0017C924
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

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060022B9 RID: 8889 RVA: 0x0017D92E File Offset: 0x0017C92E
		// (set) Token: 0x060022BA RID: 8890 RVA: 0x0017D93B File Offset: 0x0017C93B
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

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060022BB RID: 8891 RVA: 0x0017D949 File Offset: 0x0017C949
		// (set) Token: 0x060022BC RID: 8892 RVA: 0x0017D956 File Offset: 0x0017C956
		internal bool InVisibilityCollapsedTree
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.InVisibilityCollapsedTree);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.InVisibilityCollapsedTree, value);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060022BD RID: 8893 RVA: 0x0017D964 File Offset: 0x0017C964
		// (set) Token: 0x060022BE RID: 8894 RVA: 0x0017D971 File Offset: 0x0017C971
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

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060022BF RID: 8895 RVA: 0x0017D97F File Offset: 0x0017C97F
		// (set) Token: 0x060022C0 RID: 8896 RVA: 0x0017D98C File Offset: 0x0017C98C
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

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x0017D99A File Offset: 0x0017C99A
		// (set) Token: 0x060022C2 RID: 8898 RVA: 0x0017D9A7 File Offset: 0x0017C9A7
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

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060022C3 RID: 8899 RVA: 0x0017D9B5 File Offset: 0x0017C9B5
		// (set) Token: 0x060022C4 RID: 8900 RVA: 0x0017D9C2 File Offset: 0x0017C9C2
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

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x0017D9D0 File Offset: 0x0017C9D0
		// (set) Token: 0x060022C6 RID: 8902 RVA: 0x0017D9DD File Offset: 0x0017C9DD
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

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060022C7 RID: 8903 RVA: 0x0017D9EB File Offset: 0x0017C9EB
		// (set) Token: 0x060022C8 RID: 8904 RVA: 0x0017D9F8 File Offset: 0x0017C9F8
		private bool NeedsClipBounds
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.NeedsClipBounds);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.NeedsClipBounds, value);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060022C9 RID: 8905 RVA: 0x0017DA06 File Offset: 0x0017CA06
		// (set) Token: 0x060022CA RID: 8906 RVA: 0x0017DA13 File Offset: 0x0017CA13
		private bool HasWidthEverChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasWidthEverChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasWidthEverChanged, value);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060022CB RID: 8907 RVA: 0x0017DA21 File Offset: 0x0017CA21
		// (set) Token: 0x060022CC RID: 8908 RVA: 0x0017DA2E File Offset: 0x0017CA2E
		private bool HasHeightEverChanged
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.HasHeightEverChanged);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.HasHeightEverChanged, value);
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060022CD RID: 8909 RVA: 0x0017DA3C File Offset: 0x0017CA3C
		// (set) Token: 0x060022CE RID: 8910 RVA: 0x0017DA49 File Offset: 0x0017CA49
		internal bool IsRightToLeft
		{
			get
			{
				return this.ReadInternalFlag(InternalFlags.IsRightToLeft);
			}
			set
			{
				this.WriteInternalFlag(InternalFlags.IsRightToLeft, value);
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x0017DA58 File Offset: 0x0017CA58
		// (set) Token: 0x060022D0 RID: 8912 RVA: 0x0017DA80 File Offset: 0x0017CA80
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

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x0017DACF File Offset: 0x0017CACF
		// (set) Token: 0x060022D2 RID: 8914 RVA: 0x0017DADC File Offset: 0x0017CADC
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

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x060022D3 RID: 8915 RVA: 0x0017DAEA File Offset: 0x0017CAEA
		// (set) Token: 0x060022D4 RID: 8916 RVA: 0x0017DAF7 File Offset: 0x0017CAF7
		internal bool BypassLayoutPolicies
		{
			get
			{
				return this.ReadInternalFlag2((InternalFlags2)2147483648U);
			}
			set
			{
				this.WriteInternalFlag2((InternalFlags2)2147483648U, value);
			}
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0017DB05 File Offset: 0x0017CB05
		internal bool ReadInternalFlag(InternalFlags reqFlag)
		{
			return (this._flags & reqFlag) > (InternalFlags)0U;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0017DB12 File Offset: 0x0017CB12
		internal bool ReadInternalFlag2(InternalFlags2 reqFlag)
		{
			return (this._flags2 & reqFlag) > (InternalFlags2)0U;
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0017DB1F File Offset: 0x0017CB1F
		internal void WriteInternalFlag(InternalFlags reqFlag, bool set)
		{
			if (set)
			{
				this._flags |= reqFlag;
				return;
			}
			this._flags &= ~reqFlag;
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x0017DB42 File Offset: 0x0017CB42
		internal void WriteInternalFlag2(InternalFlags2 reqFlag, bool set)
		{
			if (set)
			{
				this._flags2 |= reqFlag;
				return;
			}
			this._flags2 &= ~reqFlag;
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x060022D9 RID: 8921 RVA: 0x0017DB65 File Offset: 0x0017CB65
		private static DependencyObjectType ControlDType
		{
			get
			{
				if (FrameworkElement._controlDType == null)
				{
					FrameworkElement._controlDType = DependencyObjectType.FromSystemTypeInternal(typeof(Control));
				}
				return FrameworkElement._controlDType;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060022DA RID: 8922 RVA: 0x0017DB87 File Offset: 0x0017CB87
		private static DependencyObjectType ContentPresenterDType
		{
			get
			{
				if (FrameworkElement._contentPresenterDType == null)
				{
					FrameworkElement._contentPresenterDType = DependencyObjectType.FromSystemTypeInternal(typeof(ContentPresenter));
				}
				return FrameworkElement._contentPresenterDType;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x060022DB RID: 8923 RVA: 0x0017DBA9 File Offset: 0x0017CBA9
		private static DependencyObjectType PageDType
		{
			get
			{
				if (FrameworkElement._pageDType == null)
				{
					FrameworkElement._pageDType = DependencyObjectType.FromSystemTypeInternal(typeof(Page));
				}
				return FrameworkElement._pageDType;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x060022DC RID: 8924 RVA: 0x0017DBCB File Offset: 0x0017CBCB
		private static DependencyObjectType PageFunctionBaseDType
		{
			get
			{
				if (FrameworkElement._pageFunctionBaseDType == null)
				{
					FrameworkElement._pageFunctionBaseDType = DependencyObjectType.FromSystemTypeInternal(typeof(PageFunctionBase));
				}
				return FrameworkElement._pageFunctionBaseDType;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x060022DD RID: 8925 RVA: 0x0017DBED File Offset: 0x0017CBED
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x060022DE RID: 8926 RVA: 0x0017DBF0 File Offset: 0x0017CBF0
		public DependencyObject Parent
		{
			get
			{
				return this.ContextVerifiedGetParent();
			}
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x00177E50 File Offset: 0x00176E50
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

		// Token: 0x060022E0 RID: 8928 RVA: 0x00177E94 File Offset: 0x00176E94
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

		// Token: 0x060022E1 RID: 8929 RVA: 0x0017DBF8 File Offset: 0x0017CBF8
		public object FindName(string name)
		{
			DependencyObject dependencyObject;
			return this.FindName(name, out dependencyObject);
		}

		// Token: 0x060022E2 RID: 8930 RVA: 0x00177EEC File Offset: 0x00176EEC
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			INameScope nameScope = FrameworkElement.FindScope(this, out scopeOwner);
			if (nameScope != null)
			{
				return nameScope.FindName(name);
			}
			return null;
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0017DC0E File Offset: 0x0017CC0E
		public void UpdateDefaultStyle()
		{
			TreeWalkHelper.InvalidateOnResourcesChange(this, null, ResourcesChangeInfo.ThemeChangeInfo);
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060022E4 RID: 8932 RVA: 0x00109403 File Offset: 0x00108403
		protected internal virtual IEnumerator LogicalChildren
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0017DC1C File Offset: 0x0017CC1C
		internal object FindResourceOnSelf(object resourceKey, bool allowDeferredResourceReference, bool mustReturnDeferredResourceReference)
		{
			ResourceDictionary value = FrameworkElement.ResourcesField.GetValue(this);
			if (value != null && value.Contains(resourceKey))
			{
				bool flag;
				return value.FetchResource(resourceKey, allowDeferredResourceReference, mustReturnDeferredResourceReference, out flag);
			}
			return DependencyProperty.UnsetValue;
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x0017B06B File Offset: 0x0017A06B
		internal DependencyObject ContextVerifiedGetParent()
		{
			return this._parent;
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x0017DC54 File Offset: 0x0017CC54
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

		// Token: 0x060022E8 RID: 8936 RVA: 0x0017DCB8 File Offset: 0x0017CCB8
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

		// Token: 0x060022E9 RID: 8937 RVA: 0x0017DD20 File Offset: 0x0017CD20
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
			DependencyObject parent2 = (newParent != null) ? newParent : parent;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, parent2, newParent != null);
			this.TryFireInitialized();
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x0017DDC4 File Offset: 0x0017CDC4
		internal virtual void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = this._parent;
			this._parent = newParent;
			if (this._parent != null && this._parent is ContentElement)
			{
				UIElement.SynchronizeForceInheritProperties(this, null, null, this._parent);
			}
			else if (parent is ContentElement)
			{
				UIElement.SynchronizeForceInheritProperties(this, null, null, parent);
			}
			base.SynchronizeReverseInheritPropertyFlags(parent, false);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x0017DE20 File Offset: 0x0017CE20
		internal void OnAncestorChangedInternal(TreeChangeInfo parentTreeState)
		{
			bool isSelfInheritanceParent = base.IsSelfInheritanceParent;
			if (parentTreeState.Root != this)
			{
				this.HasStyleChanged = false;
				this.HasStyleInvalidated = false;
				this.HasTemplateChanged = false;
			}
			if (parentTreeState.IsAddOperation)
			{
				FrameworkObject frameworkObject = new FrameworkObject(this, null);
				frameworkObject.SetShouldLookupImplicitStyles();
			}
			if (this.HasResourceReference)
			{
				TreeWalkHelper.OnResourcesChanged(this, ResourcesChangeInfo.TreeChangeInfo, false);
			}
			FrugalObjectList<DependencyProperty> item = this.InvalidateTreeDependentProperties(parentTreeState, base.IsSelfInheritanceParent, isSelfInheritanceParent);
			parentTreeState.InheritablePropertiesStack.Push(item);
			this.OnAncestorChanged();
			if (this.PotentiallyHasMentees)
			{
				this.RaiseClrEvent(FrameworkElement.ResourcesChangedKey, EventArgs.Empty);
			}
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x0017DEBC File Offset: 0x0017CEBC
		internal FrugalObjectList<DependencyProperty> InvalidateTreeDependentProperties(TreeChangeInfo parentTreeState, bool isSelfInheritanceParent, bool wasSelfInheritanceParent)
		{
			this.AncestorChangeInProgress = true;
			this.InVisibilityCollapsedTree = false;
			if (parentTreeState.TopmostCollapsedParentNode == null)
			{
				if (base.Visibility == Visibility.Collapsed)
				{
					parentTreeState.TopmostCollapsedParentNode = this;
					this.InVisibilityCollapsedTree = true;
				}
			}
			else
			{
				this.InVisibilityCollapsedTree = true;
			}
			FrugalObjectList<DependencyProperty> result;
			try
			{
				if (this.IsInitialized && !this.HasLocalStyle && this != parentTreeState.Root)
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
				result = TreeWalkHelper.InvalidateTreeDependentProperties(parentTreeState, this, null, style, themeStyle, ref childRecord, isChildRecordValid, hasStyleChanged, isSelfInheritanceParent, wasSelfInheritanceParent);
			}
			finally
			{
				this.AncestorChangeInProgress = false;
				this.InVisibilityCollapsedTree = false;
			}
			return result;
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060022ED RID: 8941 RVA: 0x0017DFF8 File Offset: 0x0017CFF8
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				return (base.EventHandlersStore != null && (base.EventHandlersStore.Contains(FrameworkElement.LoadedEvent) || base.EventHandlersStore.Contains(FrameworkElement.UnloadedEvent))) || (this.Style != null && this.Style.HasLoadedChangeHandler) || (this.ThemeStyle != null && this.ThemeStyle.HasLoadedChangeHandler) || (this.TemplateInternal != null && this.TemplateInternal.HasLoadedChangeHandler) || this.HasFefLoadedChangeHandler;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x060022EE RID: 8942 RVA: 0x0017E084 File Offset: 0x0017D084
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

		// Token: 0x060022EF RID: 8943 RVA: 0x0017E0C4 File Offset: 0x0017D0C4
		internal void UpdateStyleProperty()
		{
			if (!this.HasStyleInvalidated)
			{
				if (!this.IsStyleUpdateInProgress)
				{
					this.IsStyleUpdateInProgress = true;
					try
					{
						base.InvalidateProperty(FrameworkElement.StyleProperty);
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

		// Token: 0x060022F0 RID: 8944 RVA: 0x0017E130 File Offset: 0x0017D130
		internal void UpdateThemeStyleProperty()
		{
			if (!this.IsThemeStyleUpdateInProgress)
			{
				this.IsThemeStyleUpdateInProgress = true;
				try
				{
					StyleHelper.GetThemeStyle(this, null);
					ContextMenu contextMenu = base.GetValueEntry(base.LookupEntry(FrameworkElement.ContextMenuProperty.GlobalIndex), FrameworkElement.ContextMenuProperty, null, RequestFlags.DeferredReferences).Value as ContextMenu;
					if (contextMenu != null)
					{
						TreeWalkHelper.InvalidateOnResourcesChange(contextMenu, null, ResourcesChangeInfo.ThemeChangeInfo);
					}
					DependencyObject dependencyObject = base.GetValueEntry(base.LookupEntry(FrameworkElement.ToolTipProperty.GlobalIndex), FrameworkElement.ToolTipProperty, null, RequestFlags.DeferredReferences).Value as DependencyObject;
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

		// Token: 0x060022F1 RID: 8945 RVA: 0x000F6B2C File Offset: 0x000F5B2C
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x0017E224 File Offset: 0x0017D224
		internal void FireLoadedOnDescendentsInternal()
		{
			if (this.LoadedPending == null)
			{
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					parent = VisualTreeHelper.GetParent(this);
				}
				object[] unloadedPending = this.UnloadedPending;
				if (unloadedPending == null || unloadedPending[2] != parent)
				{
					BroadcastEventHelper.AddLoadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveUnloadedCallback(this, unloadedPending);
			}
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x0017E26C File Offset: 0x0017D26C
		internal void FireUnloadedOnDescendentsInternal()
		{
			if (this.UnloadedPending == null)
			{
				DependencyObject parent = this.Parent;
				if (parent == null)
				{
					parent = VisualTreeHelper.GetParent(this);
				}
				object[] loadedPending = this.LoadedPending;
				if (loadedPending == null)
				{
					BroadcastEventHelper.AddUnloadedCallback(this, parent);
					return;
				}
				BroadcastEventHelper.RemoveLoadedCallback(this, loadedPending);
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x00178530 File Offset: 0x00177530
		internal override bool ShouldProvideInheritanceContext(DependencyObject target, DependencyProperty property)
		{
			FrameworkObject frameworkObject = new FrameworkObject(target);
			return !frameworkObject.IsValid;
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x0017E2AB File Offset: 0x0017D2AB
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return FrameworkElement.InheritanceContextField.GetValue(this);
			}
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0017E2B8 File Offset: 0x0017D2B8
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			base.AddInheritanceContext(context, property);
			this.TryFireInitialized();
			if ((property == VisualBrush.VisualProperty || property == BitmapCacheBrush.TargetProperty) && FrameworkElement.GetFrameworkParent(this) == null && !FrameworkObject.IsEffectiveAncestor(this, context))
			{
				if (!this.HasMultipleInheritanceContexts && this.InheritanceContext == null)
				{
					FrameworkElement.InheritanceContextField.SetValue(this, context);
					base.OnInheritanceContextChanged(EventArgs.Empty);
					return;
				}
				if (this.InheritanceContext != null)
				{
					FrameworkElement.InheritanceContextField.ClearValue(this);
					this.WriteInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts, true);
					base.OnInheritanceContextChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0017E346 File Offset: 0x0017D346
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this.InheritanceContext == context)
			{
				FrameworkElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
			base.RemoveInheritanceContext(context, property);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0017E36F File Offset: 0x0017D36F
		private void ClearInheritanceContext()
		{
			if (this.InheritanceContext != null)
			{
				FrameworkElement.InheritanceContextField.ClearValue(this);
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0017E390 File Offset: 0x0017D390
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			DependencyObject value = FrameworkElement.MentorField.GetValue(this);
			DependencyObject dependencyObject = Helper.FindMentor(this.InheritanceContext);
			if (value != dependencyObject)
			{
				FrameworkElement.MentorField.SetValue(this, dependencyObject);
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

		// Token: 0x060022FA RID: 8954 RVA: 0x0017E3DC File Offset: 0x0017D3DC
		private void ConnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged += this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged += this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, frameworkObject.DO, true);
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

		// Token: 0x060022FB RID: 8955 RVA: 0x0017E448 File Offset: 0x0017D448
		private void DisconnectMentor(DependencyObject mentor)
		{
			FrameworkObject frameworkObject = new FrameworkObject(mentor);
			frameworkObject.InheritedPropertyChanged -= this.OnMentorInheritedPropertyChanged;
			frameworkObject.ResourcesChanged -= this.OnMentorResourcesChanged;
			TreeWalkHelper.InvalidateOnTreeChange(this, null, frameworkObject.DO, false);
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

		// Token: 0x060022FC RID: 8956 RVA: 0x0017E4BC File Offset: 0x0017D4BC
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

		// Token: 0x060022FD RID: 8957 RVA: 0x0017E4F4 File Offset: 0x0017D4F4
		private void OnMentorLoaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Loaded -= this.OnMentorLoaded;
			frameworkObject.Unloaded += this.OnMentorUnloaded;
			BroadcastEventHelper.BroadcastLoadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0017E540 File Offset: 0x0017D540
		private void OnMentorUnloaded(object sender, RoutedEventArgs e)
		{
			FrameworkObject frameworkObject = new FrameworkObject((DependencyObject)sender);
			frameworkObject.Unloaded -= this.OnMentorUnloaded;
			frameworkObject.Loaded += this.OnMentorLoaded;
			BroadcastEventHelper.BroadcastUnloadedSynchronously(this, this.IsLoaded);
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0017E58C File Offset: 0x0017D58C
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

		// Token: 0x06002300 RID: 8960 RVA: 0x0017E5BE File Offset: 0x0017D5BE
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

		// Token: 0x06002301 RID: 8961 RVA: 0x0017E5F0 File Offset: 0x0017D5F0
		private void OnMentorInheritedPropertyChanged(object sender, InheritedPropertyChangedEventArgs e)
		{
			TreeWalkHelper.InvalidateOnInheritablePropertyChange(this, null, e.Info, false);
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x0017E600 File Offset: 0x0017D600
		private void OnMentorResourcesChanged(object sender, EventArgs e)
		{
			TreeWalkHelper.InvalidateOnResourcesChange(this, null, ResourcesChangeInfo.CatastrophicDictionaryChangeInfo);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0017E610 File Offset: 0x0017D610
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

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x0017E64A File Offset: 0x0017D64A
		// (set) Token: 0x06002305 RID: 8965 RVA: 0x0017E654 File Offset: 0x0017D654
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

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002306 RID: 8966 RVA: 0x0017E65F File Offset: 0x0017D65F
		// (set) Token: 0x06002307 RID: 8967 RVA: 0x0017E66C File Offset: 0x0017D66C
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

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x0017E67A File Offset: 0x0017D67A
		// (set) Token: 0x06002309 RID: 8969 RVA: 0x0017E687 File Offset: 0x0017D687
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

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x0600230A RID: 8970 RVA: 0x0017E695 File Offset: 0x0017D695
		// (set) Token: 0x0600230B RID: 8971 RVA: 0x0017E69E File Offset: 0x0017D69E
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

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x0017E6A8 File Offset: 0x0017D6A8
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x0017E6B5 File Offset: 0x0017D6B5
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

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600230E RID: 8974 RVA: 0x0017E6C3 File Offset: 0x0017D6C3
		// (set) Token: 0x0600230F RID: 8975 RVA: 0x0017E6CC File Offset: 0x0017D6CC
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

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06002310 RID: 8976 RVA: 0x0017E6D6 File Offset: 0x0017D6D6
		// (set) Token: 0x06002311 RID: 8977 RVA: 0x0017E6E3 File Offset: 0x0017D6E3
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

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002312 RID: 8978 RVA: 0x0017E6F1 File Offset: 0x0017D6F1
		// (set) Token: 0x06002313 RID: 8979 RVA: 0x0017E6FE File Offset: 0x0017D6FE
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

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002314 RID: 8980 RVA: 0x0017E70C File Offset: 0x0017D70C
		// (set) Token: 0x06002315 RID: 8981 RVA: 0x0017E719 File Offset: 0x0017D719
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

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002316 RID: 8982 RVA: 0x0017E727 File Offset: 0x0017D727
		// (set) Token: 0x06002317 RID: 8983 RVA: 0x0017E734 File Offset: 0x0017D734
		internal bool HasTemplateChanged
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasTemplateChanged);
			}
			set
			{
				this.WriteInternalFlag2(InternalFlags2.HasTemplateChanged, value);
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002318 RID: 8984 RVA: 0x0017E742 File Offset: 0x0017D742
		// (set) Token: 0x06002319 RID: 8985 RVA: 0x0017E74F File Offset: 0x0017D74F
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

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x0600231A RID: 8986 RVA: 0x0017E75D File Offset: 0x0017D75D
		// (set) Token: 0x0600231B RID: 8987 RVA: 0x0017E76A File Offset: 0x0017D76A
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

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x0017E778 File Offset: 0x0017D778
		// (set) Token: 0x0600231D RID: 8989 RVA: 0x0017E785 File Offset: 0x0017D785
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

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600231E RID: 8990 RVA: 0x0017E793 File Offset: 0x0017D793
		// (set) Token: 0x0600231F RID: 8991 RVA: 0x0017E7A0 File Offset: 0x0017D7A0
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

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002320 RID: 8992 RVA: 0x0017E7AE File Offset: 0x0017D7AE
		// (set) Token: 0x06002321 RID: 8993 RVA: 0x0017E7BB File Offset: 0x0017D7BB
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

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x0017E7C9 File Offset: 0x0017D7C9
		// (set) Token: 0x06002323 RID: 8995 RVA: 0x0017E7D1 File Offset: 0x0017D7D1
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

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002324 RID: 8996 RVA: 0x0017E7DA File Offset: 0x0017D7DA
		internal object[] LoadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkElement.LoadedPendingProperty);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002325 RID: 8997 RVA: 0x0017E7EC File Offset: 0x0017D7EC
		internal object[] UnloadedPending
		{
			get
			{
				return (object[])base.GetValue(FrameworkElement.UnloadedPendingProperty);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002326 RID: 8998 RVA: 0x0017E7FE File Offset: 0x0017D7FE
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this.ReadInternalFlag2(InternalFlags2.HasMultipleInheritanceContexts);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002327 RID: 8999 RVA: 0x0017E80B File Offset: 0x0017D80B
		// (set) Token: 0x06002328 RID: 9000 RVA: 0x0017E818 File Offset: 0x0017D818
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

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06002329 RID: 9001 RVA: 0x0017E826 File Offset: 0x0017D826
		// (remove) Token: 0x0600232A RID: 9002 RVA: 0x0017E83B File Offset: 0x0017D83B
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

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600232B RID: 9003 RVA: 0x0017E849 File Offset: 0x0017D849
		// (remove) Token: 0x0600232C RID: 9004 RVA: 0x0017E85E File Offset: 0x0017D85E
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

		// Token: 0x0400102F RID: 4143
		private static readonly Type _typeofThis = typeof(FrameworkElement);

		// Token: 0x04001030 RID: 4144
		[CommonDependencyProperty]
		public static readonly DependencyProperty StyleProperty = DependencyProperty.Register("Style", typeof(Style), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnStyleChanged)));

		// Token: 0x04001031 RID: 4145
		public static readonly DependencyProperty OverridesDefaultStyleProperty = DependencyProperty.Register("OverridesDefaultStyle", typeof(bool), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnThemeStyleKeyChanged)));

		// Token: 0x04001032 RID: 4146
		public static readonly DependencyProperty UseLayoutRoundingProperty = DependencyProperty.Register("UseLayoutRounding", typeof(bool), typeof(FrameworkElement), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnUseLayoutRoundingChanged)));

		// Token: 0x04001033 RID: 4147
		protected internal static readonly DependencyProperty DefaultStyleKeyProperty = DependencyProperty.Register("DefaultStyleKey", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(FrameworkElement.OnThemeStyleKeyChanged)));

		// Token: 0x04001034 RID: 4148
		internal static readonly NumberSubstitution DefaultNumberSubstitution = new NumberSubstitution(NumberCultureSource.User, null, NumberSubstitutionMethod.AsCulture);

		// Token: 0x04001035 RID: 4149
		public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register("DataContext", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(FrameworkElement.OnDataContextChanged)));

		// Token: 0x04001036 RID: 4150
		internal static readonly EventPrivateKey DataContextChangedKey = new EventPrivateKey();

		// Token: 0x04001037 RID: 4151
		public static readonly DependencyProperty BindingGroupProperty = DependencyProperty.Register("BindingGroup", typeof(BindingGroup), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001038 RID: 4152
		public static readonly DependencyProperty LanguageProperty = DependencyProperty.RegisterAttached("Language", typeof(XmlLanguage), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("en-US"), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x04001039 RID: 4153
		[CommonDependencyProperty]
		public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.None, null, null, true), new ValidateValueCallback(NameValidationHelper.NameValidationCallback));

		// Token: 0x0400103A RID: 4154
		public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(object), FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null));

		// Token: 0x0400103B RID: 4155
		public static readonly DependencyProperty InputScopeProperty = InputMethod.InputScopeProperty.AddOwner(FrameworkElement._typeofThis, new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		// Token: 0x0400103E RID: 4158
		private static PropertyMetadata _actualWidthMetadata;

		// Token: 0x0400103F RID: 4159
		private static readonly DependencyPropertyKey ActualWidthPropertyKey;

		// Token: 0x04001040 RID: 4160
		public static readonly DependencyProperty ActualWidthProperty;

		// Token: 0x04001041 RID: 4161
		private static PropertyMetadata _actualHeightMetadata;

		// Token: 0x04001042 RID: 4162
		private static readonly DependencyPropertyKey ActualHeightPropertyKey;

		// Token: 0x04001043 RID: 4163
		public static readonly DependencyProperty ActualHeightProperty;

		// Token: 0x04001044 RID: 4164
		public static readonly DependencyProperty LayoutTransformProperty;

		// Token: 0x04001045 RID: 4165
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty;

		// Token: 0x04001046 RID: 4166
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinWidthProperty;

		// Token: 0x04001047 RID: 4167
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxWidthProperty;

		// Token: 0x04001048 RID: 4168
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeightProperty;

		// Token: 0x04001049 RID: 4169
		[CommonDependencyProperty]
		public static readonly DependencyProperty MinHeightProperty;

		// Token: 0x0400104A RID: 4170
		[CommonDependencyProperty]
		public static readonly DependencyProperty MaxHeightProperty;

		// Token: 0x0400104B RID: 4171
		[CommonDependencyProperty]
		public static readonly DependencyProperty FlowDirectionProperty;

		// Token: 0x0400104C RID: 4172
		[CommonDependencyProperty]
		public static readonly DependencyProperty MarginProperty;

		// Token: 0x0400104D RID: 4173
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalAlignmentProperty;

		// Token: 0x0400104E RID: 4174
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalAlignmentProperty;

		// Token: 0x0400104F RID: 4175
		private static Style _defaultFocusVisualStyle;

		// Token: 0x04001050 RID: 4176
		public static readonly DependencyProperty FocusVisualStyleProperty;

		// Token: 0x04001051 RID: 4177
		public static readonly DependencyProperty CursorProperty;

		// Token: 0x04001052 RID: 4178
		public static readonly DependencyProperty ForceCursorProperty;

		// Token: 0x04001053 RID: 4179
		internal static readonly EventPrivateKey InitializedKey;

		// Token: 0x04001054 RID: 4180
		internal static readonly DependencyPropertyKey LoadedPendingPropertyKey;

		// Token: 0x04001055 RID: 4181
		internal static readonly DependencyProperty LoadedPendingProperty;

		// Token: 0x04001056 RID: 4182
		internal static readonly DependencyPropertyKey UnloadedPendingPropertyKey;

		// Token: 0x04001057 RID: 4183
		internal static readonly DependencyProperty UnloadedPendingProperty;

		// Token: 0x0400105A RID: 4186
		public static readonly DependencyProperty ToolTipProperty;

		// Token: 0x0400105B RID: 4187
		public static readonly DependencyProperty ContextMenuProperty;

		// Token: 0x04001060 RID: 4192
		private Style _themeStyleCache;

		// Token: 0x04001061 RID: 4193
		private static readonly UncommonField<SizeBox> UnclippedDesiredSizeField;

		// Token: 0x04001062 RID: 4194
		private static readonly UncommonField<FrameworkElement.LayoutTransformData> LayoutTransformDataField;

		// Token: 0x04001063 RID: 4195
		private Style _styleCache;

		// Token: 0x04001064 RID: 4196
		internal static readonly UncommonField<ResourceDictionary> ResourcesField;

		// Token: 0x04001065 RID: 4197
		internal DependencyObject _templatedParent;

		// Token: 0x04001066 RID: 4198
		private UIElement _templateChild;

		// Token: 0x04001067 RID: 4199
		private InternalFlags _flags;

		// Token: 0x04001068 RID: 4200
		private InternalFlags2 _flags2 = InternalFlags2.Default;

		// Token: 0x04001069 RID: 4201
		internal static DependencyObjectType UIElementDType;

		// Token: 0x0400106A RID: 4202
		private static DependencyObjectType _controlDType;

		// Token: 0x0400106B RID: 4203
		private static DependencyObjectType _contentPresenterDType;

		// Token: 0x0400106C RID: 4204
		private static DependencyObjectType _pageFunctionBaseDType;

		// Token: 0x0400106D RID: 4205
		private static DependencyObjectType _pageDType;

		// Token: 0x0400106E RID: 4206
		[ThreadStatic]
		private static FrameworkElement.FrameworkServices _frameworkServices;

		// Token: 0x0400106F RID: 4207
		internal static readonly EventPrivateKey ResourcesChangedKey;

		// Token: 0x04001070 RID: 4208
		internal static readonly EventPrivateKey InheritedPropertyChangedKey;

		// Token: 0x04001071 RID: 4209
		internal new static DependencyObjectType DType;

		// Token: 0x04001072 RID: 4210
		private new DependencyObject _parent;

		// Token: 0x04001073 RID: 4211
		private FrugalObjectList<DependencyProperty> _inheritableProperties;

		// Token: 0x04001074 RID: 4212
		private static readonly UncommonField<DependencyObject> InheritanceContextField;

		// Token: 0x04001075 RID: 4213
		private static readonly UncommonField<DependencyObject> MentorField;

		// Token: 0x02000A7B RID: 2683
		private struct MinMax
		{
			// Token: 0x06008654 RID: 34388 RVA: 0x0032A4D4 File Offset: 0x003294D4
			internal MinMax(FrameworkElement e)
			{
				this.maxHeight = e.MaxHeight;
				this.minHeight = e.MinHeight;
				double num = e.Height;
				double num2 = DoubleUtil.IsNaN(num) ? double.PositiveInfinity : num;
				this.maxHeight = Math.Max(Math.Min(num2, this.maxHeight), this.minHeight);
				num2 = (DoubleUtil.IsNaN(num) ? 0.0 : num);
				this.minHeight = Math.Max(Math.Min(this.maxHeight, num2), this.minHeight);
				this.maxWidth = e.MaxWidth;
				this.minWidth = e.MinWidth;
				num = e.Width;
				double num3 = DoubleUtil.IsNaN(num) ? double.PositiveInfinity : num;
				this.maxWidth = Math.Max(Math.Min(num3, this.maxWidth), this.minWidth);
				num3 = (DoubleUtil.IsNaN(num) ? 0.0 : num);
				this.minWidth = Math.Max(Math.Min(this.maxWidth, num3), this.minWidth);
			}

			// Token: 0x0400417D RID: 16765
			internal double minWidth;

			// Token: 0x0400417E RID: 16766
			internal double maxWidth;

			// Token: 0x0400417F RID: 16767
			internal double minHeight;

			// Token: 0x04004180 RID: 16768
			internal double maxHeight;
		}

		// Token: 0x02000A7C RID: 2684
		private class LayoutTransformData
		{
			// Token: 0x06008655 RID: 34389 RVA: 0x0032A5E7 File Offset: 0x003295E7
			internal void CreateTransformSnapshot(Transform sourceTransform)
			{
				this._transform = new MatrixTransform(sourceTransform.Value);
				this._transform.Freeze();
			}

			// Token: 0x17001E16 RID: 7702
			// (get) Token: 0x06008656 RID: 34390 RVA: 0x0032A605 File Offset: 0x00329605
			internal Transform Transform
			{
				get
				{
					return this._transform;
				}
			}

			// Token: 0x04004181 RID: 16769
			internal Size UntransformedDS;

			// Token: 0x04004182 RID: 16770
			internal Size TransformedUnroundedDS;

			// Token: 0x04004183 RID: 16771
			private Transform _transform;
		}

		// Token: 0x02000A7D RID: 2685
		private class FrameworkServices
		{
			// Token: 0x06008658 RID: 34392 RVA: 0x0032A60D File Offset: 0x0032960D
			internal FrameworkServices()
			{
				this._keyboardNavigation = new KeyboardNavigation();
				this._popupControlService = new PopupControlService();
				if (!AccessibilitySwitches.UseLegacyToolTipDisplay)
				{
					this._keyboardNavigation.FocusChanged += this._popupControlService.FocusChangedEventHandler;
				}
			}

			// Token: 0x04004184 RID: 16772
			internal KeyboardNavigation _keyboardNavigation;

			// Token: 0x04004185 RID: 16773
			internal PopupControlService _popupControlService;
		}
	}
}
